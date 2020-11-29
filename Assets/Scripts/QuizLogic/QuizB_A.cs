using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizB_A : MonoBehaviour
{
    public GameObject mPieceBonus;
    public GameObject mAgreementBonus;
    private List<ShowName> mShowName;
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
        mShowName = new List<ShowName>(this.transform.Find("Area").GetComponents<ShowName>());
        mShowName[0].SetActive(true);
        mShowName[1].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        var inventory = quizReception.GetItems();
        if (cntReceived == 0 && inventory.Count > 0)
        {
            if(inventory[0].name.IndexOf("Coin") > 0)
            {
                cntReceived++;
                if (mPieceBonus != null) itemsBox.MoveItemIn(mPieceBonus);
                inventory.Clear();
            }
            else
            {
                itemsBox.MoveItemIn(inventory[0]);
                inventory.Clear();
            }
        }

        if (cntReceived == 1 && inventory.Count > 0)
        {
            if(inventory[0].name.IndexOf("Booze") > 0)
            {
                mShowName[0].SetActive(false);
                mShowName[1].SetActive(true);
                cntReceived++;
                itemsBox.MoveItemIn(mAgreementBonus);
                inventory.Clear();
            }
            else
            {
                itemsBox.MoveItemIn(inventory[0]);
                inventory.Clear();
            }
            
        }
        if (cntReceived == 2 && inventory.Count > 0)
        {
            itemsBox.MoveItemIn(inventory[0]);
            inventory.Clear();
        }
        //// first a coin
        //if (cntReceived==0 && inventory.Count > 0) 
        //{
        //    cntReceived++;
        //    itemsBox.MoveItemIn(mPieceBonus);
        //    quizReception.RemoveItem(inventory[0]);
        //}
        //if (cntReceived == 1 && inventory.Count > 0 )
        //{   
        //    // second a booze
        //    if(Common.Utils.TrimClone(inventory[0].gameObject.name).IndexOf("Booze") >= 0)
        //    {
        //        itemsBox.MoveItemIn(mAgreementBonus);
        //        cntReceived++;
        //        mEnding.FinishOneLine();
        //    }
        //    else
        //    {
        //        itemsBox.MoveItemIn(inventory[0]);
        //        quizReception.RemoveItem(inventory[0]);
        //    }
        //}
    }
}
