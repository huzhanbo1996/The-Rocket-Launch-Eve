using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite picIdle;
    public Sprite picPicked;
    public GameObject objToGive;
    public GameObject objCarried;

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
        if(Common.Utils.ClickedAnywhereOut(this.gameObject) && !Common.Utils.ClickedOn(objToGive))
        {
            mSRender.sprite = picIdle;
            if (itemsBox != null) itemsBox.objPickedUp.Remove(this.gameObject);
        }
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //    if (col.Length == 0)
        //    {
        //        mSRender.sprite = picIdle;
        //        if (itemsBox != null) itemsBox.objPickedUp.Remove(this.gameObject);
        //    }
        //    else
        //    {
        //        int i = 0;
        //        for(i=0; i<col.Length; i++)
        //        {
        //            if (col[i].gameObject == this.gameObject) break;
        //        }
        //        if (i == col.Length)
        //        {
        //            if (itemsBox != null) itemsBox.objPickedUp.Remove(this.gameObject);
        //            mSRender.sprite = picIdle;
        //        }
        //    }
        //}
    }

}
