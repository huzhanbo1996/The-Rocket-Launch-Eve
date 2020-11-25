using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class SceneObjNoExitg : MonoBehaviour
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
    public GameObject relatedQuizArea;

    private bool mNeedSetLayerMask;
    private ItemsBox itemsBox;
    private SpriteRenderer mSRender;
    public bool showQuiz;
    // Start is called before the first frame update
    void Start()
    {
        mNeedSetLayerMask = false;
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
        if (relatedQuiz != null)
        {
            relatedQuizArea = relatedQuiz.transform.Find("Area").gameObject;
            Debug.Assert(relatedQuizArea != null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        mSRender.enabled = visiable;
        if (itemsBox.objPickedUp.Count > 0 && this.gameObject.name == "SceneObjPuzzle")
        {

            //Debug.Log(receiveItems.ToString() + (!showQuiz).ToString() + (itemsBox.objPickedUp.Count > 0).ToString() 
            //    + itemsBox.objPickedUp[0].GetComponent<Item>().objToGive.name);


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
        else if (hasQuiz && Common.Utils.ClickedOn(this.gameObject)) // focus on quiz
        {
            //itemsBox.gameObject.SetActive(false);
            // don't change quizCamera's layerMask
            Common.Utils.SetActiveLayer("Quiz");
            relatedQuiz.SetActive(true);
            showQuiz = true;
        }

    }

    private void LateUpdate()
    {
        if (mNeedSetLayerMask)
        {
            Common.Utils.SetActiveLayer("Default");
            mNeedSetLayerMask = false;
        }

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
