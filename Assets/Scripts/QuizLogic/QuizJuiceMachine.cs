using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizJuiceMachine : MonoBehaviour
{
    public GameObject mLiguidRed;
    public GameObject mLiguidGreen;
    private QuizReception mQuizReception;
    private ItemsBox mItemsBox;
    // Start is called before the first frame update
    void Start()
    {
        mItemsBox = FindObjectOfType<ItemsBox>();
        mQuizReception = this.GetComponent<QuizReception>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach ( var it in mQuizReception.GetItems())
        {
            if(Common.Utils.TrimClone(it.name) == mLiguidGreen.name)
            {
                mItemsBox.MoveItemIn(mLiguidGreen);
                mQuizReception.RemoveItem(it);
            }
            if (Common.Utils.TrimClone(it.name) == mLiguidRed.name)
            {
                mItemsBox.MoveItemIn(mLiguidRed);
                mQuizReception.RemoveItem(it);
            }
        }
    }
}
