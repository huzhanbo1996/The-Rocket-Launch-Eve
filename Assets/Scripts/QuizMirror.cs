using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizMirror : MonoBehaviour, ICapturable
{
    public GameObject item;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sprite GetPicture()
    {
        if(this.gameObject.transform.parent.gameObject.activeSelf)
        {
            FindObjectOfType<ItemsBox>().MoveItemIn(item);
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
