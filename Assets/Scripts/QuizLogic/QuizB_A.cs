using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizB_A : MonoBehaviour
{
    public GameObject mPieceBonus;
    private int cntReceived;
    private ItemsBox itemsBox;
    private QuizReception quizReception;
    private Ending mEnding;
    // Start is called before the first frame update
    void Start()
    {
        mEnding = FindObjectOfType<Ending>();
        Debug.Assert(mEnding != null);
        cntReceived = 0;
        itemsBox = FindObjectOfType<ItemsBox>();
        quizReception = this.transform.Find("Area").GetComponent<QuizReception>();
    }

    // Update is called once per frame
    void Update()
    {
        var inventory = quizReception.GetItems();
        // first a coin
        if (cntReceived==0 && inventory.Count > 0) 
        {
            cntReceived++;
            itemsBox.MoveItemIn(mPieceBonus);
            quizReception.RemoveItem(inventory[0]);
        }
        if (cntReceived == 1 && inventory.Count > 0 )
        {   
            // second a booze
            if(Common.Utils.TrimClone(inventory[0].gameObject.name).Contains("ItemBooze"))
            {
                cntReceived++;
                mEnding.FinishOneLine();
            }
            else
            {
                itemsBox.MoveItemIn(inventory[0]);
                quizReception.RemoveItem(inventory[0]);
            }
        }
    }
}
