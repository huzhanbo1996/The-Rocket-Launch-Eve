using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class SceneObj : MonoBehaviour
{
    public bool pickable;
    public bool draguable;
    public bool visiable;
    public bool hasQuiz;
    public bool receiveItems;
    public GameObject relatedItem;
    public Sprite picIdle;
    public Sprite picPicked;
    public GameObject relatedQuiz;
    public GameObject objToGive;
    public GameObject objCarried;

    private ItemsBox itemsBox;
    private SpriteRenderer mSRender;
    private GameObject relatedQuizArea;
    public bool showQuiz;
    // Start is called before the first frame update
    void Start()
    {
        Common.Utils.SetActiveLayer("Default");
        mSRender = this.GetComponent<SpriteRenderer>();
        mSRender.enabled = visiable;
        itemsBox = FindObjectOfType<ItemsBox>();
        Debug.Assert(itemsBox != null);
        // TODO a more complete assertion
        //Debug.Assert(pickable && objToGive != null);
        Debug.Assert(!receiveItems || relatedQuiz != null);
        Debug.Assert(pickable || draguable || visiable);
        Debug.Assert(pickable == false || relatedItem != null);
        if(relatedQuiz != null)
        {
            relatedQuizArea = relatedQuiz.transform.Find("Area").gameObject;
            Debug.Assert(relatedQuizArea != null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        mSRender.enabled = visiable;
        if (itemsBox.objPickedUp.Count > 0 && this.gameObject.name=="SceneObjPuzzle")
        {
            
            Debug.Log(receiveItems.ToString() + (!showQuiz).ToString() + (itemsBox.objPickedUp.Count > 0).ToString() 
                + itemsBox.objPickedUp[0].GetComponent<Item>().objToGive.name);
            
            
        }
        if (receiveItems && !showQuiz 
            && itemsBox.objPickedUp.Count > 0 
            && itemsBox.objPickedUp[0].GetComponent<Item>().objToGive == this.gameObject
            && Common.Utils.ClickedOn(this.gameObject))  // collect item
        {
            var item = itemsBox.objPickedUp[0].GetComponent<Item>();
            var cpy = Instantiate(item.objCarried);
            relatedQuiz.GetComponent<QuizReception>().AddItem(cpy);
            itemsBox.RemoveItem(item.gameObject);
        }
        else if(hasQuiz && !showQuiz && Common.Utils.ClickedOn(this.gameObject))  // focus on quiz
        {
            //itemsBox.gameObject.SetActive(false);
            relatedQuiz.SetActive(true);
            Common.Utils.SetActiveLayer("Quiz");
            showQuiz = true;
        }
        else if (hasQuiz && showQuiz
            && Common.Utils.ClickedAnywhereOut(itemsBox.gameObject)
            && Common.Utils.ClickedAnywhereOut(relatedQuizArea))   // exit quiz
        {
            //itemsBox.gameObject.SetActive(true);
            relatedQuiz.SetActive(false);
            Common.Utils.SetActiveLayer("Default");
            showQuiz = false;
        }

        if (pickable && Common.Utils.ClickedOn(this.gameObject))
        {
            bool ret = itemsBox.AddItem(relatedItem, picIdle, picPicked, objToGive, objCarried);
            Debug.Assert(ret); // get false only if itemsbox is full, meaning should redesign the itemsbox
            Destroy(this.gameObject);
        }

    }

    private void LateUpdate()
    {
        
    }
    public void QuizResolved()
    {
        //itemsBox.gameObject.SetActive(true);
        relatedQuiz.SetActive(false);
        Common.Utils.SetActiveLayer("Default");
        showQuiz = false;
        Destroy(this.gameObject);
    }

    private void OnMouseDrag()
    {
        
    }
}
