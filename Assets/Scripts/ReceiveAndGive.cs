using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveAndGive : MonoBehaviour
{
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
        if (quizReception.GetItems().Count > 0)
        {
            //cntReceived++;
            itemsBox.MoveItemIn(quizReception.GetItems()[0]);
            quizReception.RemoveItem(quizReception.GetItems()[0]);
        }
    }
}
