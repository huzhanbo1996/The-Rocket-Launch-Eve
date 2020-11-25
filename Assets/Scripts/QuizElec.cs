using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizElec : MonoBehaviour
{
 
    public GameObject relatedQuiz;
    public GameObject relatedQuizArea;
    public List<Sprite> mSpScene;
    public List<Sprite> mSpQuizs;

    public int mPhase;
    private int mIdxSpScene;
    private int mIdxSpQuiz;
    private bool mNeedSetLayerMask;
    private ItemsBox itemsBox;
    private SpriteRenderer mSRender;
    public bool showQuiz;
    // Start is called before the first frame update
    void Start()
    {
        mPhase = 0;
        mIdxSpScene = mIdxSpQuiz = 1;
        mNeedSetLayerMask = false;
        Common.Utils.SetActiveLayer("Default");
        mSRender = this.GetComponent<SpriteRenderer>();
        itemsBox = FindObjectOfType<ItemsBox>();
        Debug.Assert(itemsBox != null);
        if (relatedQuiz != null)
        {
            relatedQuizArea = relatedQuiz.transform.Find("Area").gameObject;
            Debug.Assert(relatedQuizArea != null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!showQuiz &&
            Common.Utils.ClickedOn(this.gameObject)) // focus on quiz
        {
            // don't change quizCamera's layerMask
            Common.Utils.SetActiveLayer("Quiz");
            relatedQuiz.SetActive(true);
            showQuiz = true;
        }
        else if (showQuiz &&
                 (!Common.Utils.ClickedOnChildenOf(itemsBox.gameObject)) &&
                 Common.Utils.ClickedAnywhereOut(itemsBox.gameObject) &&
                 Common.Utils.ClickedAnywhereOut(relatedQuizArea) 
                 )
        {
            relatedQuiz.SetActive(false);
            mNeedSetLayerMask = true;
            showQuiz = false;
        }

        var mInventory = relatedQuiz.transform.Find("Area").GetComponent<QuizReception>().GetItems();
        var mQuizRender =  relatedQuiz.transform.Find("Area").GetComponent<SpriteRenderer>();
        if (mPhase == 1 && Common.Utils.ClickedOn(relatedQuiz.transform.Find("Area").gameObject))
        {
            mPhase++;
            QuizResolved();
            mQuizRender.sprite = mSpQuizs[mIdxSpQuiz++];
        }

        if (mPhase == 0 && mInventory.Count > 0)
        {
            mPhase++;
            QuizResolved();
            mInventory.Clear();
            mQuizRender.sprite = mSpQuizs[mIdxSpQuiz++];
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
        this.GetComponent<SpriteRenderer>().sprite = mSpScene[mIdxSpScene++];
    }
}
