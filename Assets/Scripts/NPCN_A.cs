using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCN_A : MonoBehaviour
{
    private Ending mEnding;
    private QuizReception mQuizReception;
    // Start is called before the first frame update
    void Start()
    {
        //mEnding = FindObjectOfType<Ending>();
        mQuizReception = GetComponent<QuizReception>();
        Debug.Assert(mQuizReception != null);
    }

    // Update is called once per frame
    void Update()
    {
        if(mQuizReception.GetItems().Count > 0)
        {
            //mEnding.FinishOneLine();
            mQuizReception.GetItems().Clear();
        }
    }
}
