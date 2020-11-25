using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizMirror : MonoBehaviour, ICapturable
{
    public GameObject item;

    private bool isGaved;
    // Start is called before the first frame update
    void Start()
    {
        isGaved = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sprite GetPicture()
    {
        if (this.gameObject.transform.parent.gameObject.activeSelf)
        {
            if(!isGaved)
            {
                isGaved = true;
                FindObjectOfType<ItemsBox>().MoveItemIn(item);
            }
            return GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            return null;
        }
    }
    public GameObject GetScene()
    {
        return this.transform.parent.parent.gameObject;
    }
}
