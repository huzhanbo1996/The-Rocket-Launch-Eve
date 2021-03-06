﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizPuzzle : MonoBehaviour
{
    public float mShortDistance;
    public GameObject mSceneObj;
    public bool isSolved;
    public GameObject mBonus;
    private QuizReception mQuizReception;
    private Dictionary<string, Vector3> mAnsPositions = new Dictionary<string, Vector3>();
    private int mAnsCnt;
    private int mStillNeedCnt;
    private Ending mEnding;
    // Start is called before the first frame update
    void Start()
    {
        isSolved = false;
        mEnding = FindObjectOfType<Ending>();
        Debug.Assert(mEnding != null);
        mQuizReception = this.transform.Find("Area").GetComponent<QuizReception>();
        Debug.Assert(mQuizReception != null);
        var childPieces = this.transform.Find("Pieces");
        mStillNeedCnt = mAnsCnt = childPieces.childCount;
        for (int i = 0; i < mAnsCnt; i++)
        {
            var piece = childPieces.GetChild(i).gameObject;
            mAnsPositions.Add(piece.name, piece.transform.position);
            piece.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isSolved) return;
        var invenotory = mQuizReception.GetItems();
        Debug.Assert(invenotory.Count <= mAnsCnt);
        List<GameObject> pieceToRemove = new List<GameObject>();
        foreach(var piece in invenotory)
        {
            if(Vector3.Distance(piece.transform.position, mAnsPositions[piece.name.Replace("(Clone)","").Trim()]) < mShortDistance)
            {
                //piece.GetComponent<QuizPuzzlePiece>().freeze = true;
                //piece.transform.position = mAnsPositions[piece.name.Replace("(Clone)", "").Trim()];
                //mStillNeedCnt -= 1;
                //pieceToRemove.Add(piece);
            }
        }
        //foreach(var obj in pieceToRemove)
        //{
        //    invenotory.Remove(obj);
        //}
        var cnt = 0;
        foreach (var piece in invenotory)
        {
            if (Vector3.Distance(piece.transform.position, mAnsPositions[piece.name.Replace("(Clone)", "").Trim()]) < mShortDistance)
            {
                cnt++;
            }
        }
        if (cnt == mAnsCnt)
        {
            foreach (var piece in invenotory)
            {
                piece.GetComponent<QuizPuzzlePiece>().freeze = true;
                piece.transform.position = mAnsPositions[piece.name.Replace("(Clone)", "").Trim()];
            }
            mEnding.FinishOneLine();
            isSolved = true;
            if (mBonus != null)
            {
                FindObjectOfType<ItemsBox>().MoveItemIn(mBonus);
                mBonus = null;
            }
            //this.mSceneObj.GetComponent<SceneObj>().QuizResolved();
        }
    }
}
