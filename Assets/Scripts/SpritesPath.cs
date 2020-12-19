using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class SpritesPath : MonoBehaviour
{
    // public Dictionary<string, Sprite> PathVSSprite = new Dictionary<string, Sprite>();
    [SerializeField] public List<Sprite> AllSp = new List<Sprite>();
    [SerializeField] public List<string> AllSpName = new List<string>();
    public Dictionary<Sprite, string> SpriteVSPath = new Dictionary<Sprite, string>();

    private void Start()
    {
        SpriteVSPath.Clear();
        for (int idx = 0; idx < AllSp.Count;idx++)
        {
            if (SpriteVSPath.ContainsKey(AllSp[idx]))
            {
                if(SpriteVSPath[AllSp[idx]] != AllSpName[idx])
                {
                    Debug.LogError("Sprite resource path inconsistent : " + AllSp[idx].ToString()
                        + " : " + SpriteVSPath[AllSp[idx]] + " : " + AllSpName[idx]);
                }
                continue;
            }
            SpriteVSPath.Add(AllSp[idx], AllSpName[idx]);
        }
    }
}