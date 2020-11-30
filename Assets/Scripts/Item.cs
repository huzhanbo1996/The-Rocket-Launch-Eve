using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite picIdle;
    public Sprite picPicked;
    public GameObject objToGive;
    public GameObject objToGive2;
    public GameObject objCarried;
    public bool mKeephAfterUse;

    public Material outLineMaterial;
    private QuizReception mQuizReception;
    private SpriteRenderer mSRender;
    // Start is called before the first frame update
    void Start()
    {
        mSRender = this.GetComponent<SpriteRenderer>();
        outLineMaterial = mSRender.material;
        mSRender.sprite = picIdle;
        picPicked = Instantiate(picIdle);
        outLineMaterial.SetFloat("_Thickness", 0);
    }

    // Update is called once per frame
    void Update()
    {
        var itemsBox = FindObjectOfType<ItemsBox>();
        //Debug.Assert(itemsBox != null);
        if (Common.Utils.ClickedOn(this.gameObject))
        {
            mSRender.sprite = picPicked;
            outLineMaterial.SetFloat("_Thickness", 10);
            if (itemsBox != null && !itemsBox.objPickedUp.Contains(this.gameObject)) itemsBox.objPickedUp.Add(this.gameObject);
        }

        if( objToGive!= null && Common.Utils.ClickedOn(objToGive) && mSRender.sprite == picPicked && 
            !objToGive.GetComponent<QuizReception>().IsRefuse(this.gameObject.name))
        {
            var newItem = Instantiate(objCarried);
            objToGive.GetComponent<QuizReception>().AddItem(newItem);
            if (!mKeephAfterUse) itemsBox.RemoveItem(this.gameObject);
        }

        if (objToGive2 != null && Common.Utils.ClickedOn(objToGive2) && mSRender.sprite == picPicked &&
            !objToGive2.GetComponent<QuizReception>().IsRefuse(this.gameObject.name))
        {
            var newItem = Instantiate(objCarried);
            objToGive2.GetComponent<QuizReception>().AddItem(newItem);
            if (!mKeephAfterUse) itemsBox.RemoveItem(this.gameObject);
        }


        //if (Common.Utils.ClickedAnywhereOut(this.gameObject) && 
        //    (objToGive==null || !Common.Utils.ClickedOn(objToGive)) &&
        //    (objToGive2==null || !Common.Utils.ClickedOn(objToGive2))
        //    )
        if (Common.Utils.ClickedAnywhereOut(this.gameObject) && 
            Common.Utils.ClickedOnChildenOf(FindObjectOfType<ItemsBox>().gameObject)
            )
        {
            mSRender.sprite = picIdle;
            outLineMaterial.SetFloat("_Thickness", 0);
            if (itemsBox != null && itemsBox.objPickedUp.Contains(this.gameObject)) itemsBox.objPickedUp.Remove(this.gameObject);
        }
    }

}
