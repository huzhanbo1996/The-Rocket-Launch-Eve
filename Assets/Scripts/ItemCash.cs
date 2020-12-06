using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCash : MonoBehaviour
{
    public Sprite picIdle;
    public Sprite picPicked;
    public GameObject objToGive;
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
            if (itemsBox != null) itemsBox.objPickedUp.Add(this.gameObject);
        }

        if (objToGive != null && Common.Utils.ClickedOnChildenOf(objToGive) && objToGive.activeSelf && mSRender.sprite == picPicked)
        {
            var newItem = Instantiate(objCarried);
            objToGive.transform.Find("Area").GetComponent<QuizReception>().AddItem(newItem);
            if (!mKeephAfterUse) itemsBox.RemoveItem(this.gameObject);
        }

        if (Common.Utils.ClickedAnywhereOut(this.gameObject) &&
            (objToGive == null || !Common.Utils.ClickedOnChildenOf(objToGive))
            )
        {
            mSRender.sprite = picIdle;
            outLineMaterial.SetFloat("_Thickness", 0);
            if (itemsBox != null) itemsBox.objPickedUp.Remove(this.gameObject);
        }
    }

}
