// GameObjectGUIDInspector.cs
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
[CustomEditor(typeof(SpritesPath))]
public class SpritesPathInspector : Editor
{
    void OnEnable()
    {
        var spritesPath = (SpritesPath)target;
        if (spritesPath.SpriteVSPath.Count != 0 ) return;
        spritesPath.SpriteVSPath.Clear();
        // spritesPath.PathVSSprite.Clear();

        var allSp = Resources.FindObjectsOfTypeAll<Sprite>();
        foreach(var sp in allSp)
        {
            spritesPath.SpriteVSPath.Add(sp, GetResourcePath(sp));
            // spritesPath.PathVSSprite.Add(GetResourcePath(sp), sp);
        }

        var allSceneObj = Resources.FindObjectsOfTypeAll<SceneObj>();
        foreach(var obj in allSceneObj)
        {
            if (obj.picIdle == null || spritesPath.SpriteVSPath.ContainsKey(obj.picIdle)) continue;
            spritesPath.SpriteVSPath.Add(obj.picIdle, GetResourcePath(obj.picIdle));
            // spritesPath.PathVSSprite.Add(GetResourcePath(sp), sp);
        }

        var quizWire = Resources.FindObjectsOfTypeAll<QuizWire>()[0];
        foreach(var sp in quizWire.mSpVSPic.Values)
        {
            if (sp == null || spritesPath.SpriteVSPath.ContainsKey(sp)) continue;
            spritesPath.SpriteVSPath.Add(sp, GetResourcePath(sp));
        }

        var quiz4NPC = Resources.FindObjectsOfTypeAll<Quiz4NPC>()[0];
        if (quiz4NPC.mPictureHold != null && !spritesPath.SpriteVSPath.ContainsKey(quiz4NPC.mPictureHold))
        {
            spritesPath.SpriteVSPath.Add(quiz4NPC.mPictureHold, GetResourcePath(quiz4NPC.mPictureHold));
        }

        var quizMirror = Resources.FindObjectsOfTypeAll<QuizMirror>()[0];
        if (quizMirror.mPic != null && !spritesPath.SpriteVSPath.ContainsKey(quizMirror.mPic))
        {
            spritesPath.SpriteVSPath.Add(quizMirror.mPic, GetResourcePath(quizMirror.mPic));
        }

        foreach(var p in spritesPath.SpriteVSPath)
        {
            Debug.Log(p.Key.ToString() + ":" + p.Value);
        }
    }
    private string GetResourcePath(Sprite obj)
    {
        var ret = AssetDatabase.GetAssetPath(obj);
        if ( ret.IndexOf("Assets/Resources/") < 0) 
        {
            Debug.LogWarning("NOT IN RESOURCES : " + ret);
            return ret;
        }
        // Debug.Assert(ret.IndexOf("Assets/Resources/") >= 0);
        ret = new string(ret.Skip("Assets/Resources/".Length).ToArray());
        ret = ret.Split('.')[0];
        return ret;
    }
}
#endif