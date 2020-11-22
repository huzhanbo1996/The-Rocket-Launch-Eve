using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite picIdle;
    public Sprite picPicked;
    public GameObject objToGive;
    public GameObject objCarried;

    private QuizReception mQuizReception;
    private SpriteRenderer mSRender;
    // Start is called before the first frame update
    void Start()
    {
        mSRender = this.GetComponent<SpriteRenderer>();
        mSRender.sprite = picIdle;
    }

    // Update is called once per frame
    void Update()
    {
        var itemsBox = this.transform.parent.GetComponent<ItemsBox>();
        //Debug.Assert(itemsBox != null);
        if (Common.Utils.ClickedOn(this.gameObject))
        {
            mSRender.sprite = picPicked;
            if (itemsBox != null) itemsBox.objPickedUp.Add(this.gameObject);
        }

        if(objToGive!= null && Common.Utils.ClickedOn(objToGive) && mSRender.sprite == picPicked)
        {
            var newItem = Instantiate(objCarried);
            objToGive.GetComponent<QuizReception>().AddItem(newItem);
            itemsBox.RemoveItem(this.gameObject);
        }

        if(Common.Utils.ClickedAnywhereOut(this.gameObject) && 
            (objToGive==null || !Common.Utils.ClickedOn(objToGive))
            )
        {
            mSRender.sprite = picIdle;
            if (itemsBox != null) itemsBox.objPickedUp.Remove(this.gameObject);
        }
    }

}
