using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizVendingMachine : MonoBehaviour, IQuizSerializable
{
    public GameObject mPieceBonus;
    private int cntReceived = 0;
    private ItemsBox itemsBox;
    private QuizReception quizReception;
    private Ending mEnding;

    public void Deserialize(QuizData data)
    {
        int idx = 0;
        cntReceived = data.mIntData[idx++];

        var quizReception = this.transform.Find("Area").GetComponent<QuizReception>();
        Debug.Assert(quizReception != null);
        for (idx = 0; idx < data.mStringData.Count; idx++)
        {
            var fakeItem = new GameObject(data.mStringData[idx]);
            quizReception.GetRefuse().Add(fakeItem);
        }
    }

    public QuizData Serialize()
    {
        var ret = new QuizData();
        ret.mIntData.Add(cntReceived);

        var quizReception = this.transform.Find("Area").GetComponent<QuizReception>();
        Debug.Assert(quizReception != null);
        foreach(var refuse in quizReception.GetRefuse())
        {
            ret.mStringData.Add(refuse.name);
            // ret.mIntData.Add(FindObjectOfType<Persist>().OBJVSUid[refuse]);
        }
        return ret;
    }

    // Start is called before the first frame update
    void Start()
    {
        mEnding = FindObjectOfType<Ending>();
        Debug.Assert(mEnding != null);
        // cntReceived = 0;
        itemsBox = FindObjectOfType<ItemsBox>();
        quizReception = this.transform.Find("Area").GetComponent<QuizReception>();
    }

    // Update is called once per frame
    void Update()
    {
        var inventory = quizReception.GetItems();
        // first a coin
        if (cntReceived == 0 && inventory.Count > 0)
        {
            cntReceived++;
            itemsBox.MoveItemIn(mPieceBonus);
            quizReception.GetRefuse().Add(inventory[0]);
            quizReception.RemoveItem(inventory[0]);
        }
        else if (inventory.Count > 0)  // this logic should be abondonned due to GetRefuse() above
        {
            itemsBox.MoveItemIn(inventory[0]);
            quizReception.RemoveItem(inventory[0]);
        }
    }
}

