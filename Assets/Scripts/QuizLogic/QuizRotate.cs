using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class QuizRotate : MonoBehaviour
{
    // refer to https://docs.qq.com/doc/DRnNjR3N6eEJRY2dH
    public GameObject mButtomLeft;
    public GameObject mButtomMiddle;
    public GameObject mButtomRight;
    public string mInitalOrder;
    public string mAnsOrder;
    public GameObject mSpRender;
    public List<GameObject> mTargets = new List<GameObject>();
    public GameObject mSceneObj;
    
    private List<KeyValuePair<GameObject,int>> mRotateGroupLeft = new List<KeyValuePair<GameObject, int>>();
    private List<KeyValuePair<GameObject,int>> mRotateGroupRight = new List<KeyValuePair<GameObject, int>>();
    private List<Vector3> mPosistions = new List<Vector3>();
    private bool mResolved;
    // Start is called before the first frame update
    void Start()
    {
        mResolved = false;
        foreach (var obj in mTargets)
        {
            mPosistions.Add(obj.transform.position);
        }
        for (int i = 0; i < mInitalOrder.Length; i++) 
        {
            var idxTarget = int.Parse(mInitalOrder[i].ToString()) - 1;
            if (i < mTargets.Count / 2)
            {
                mRotateGroupLeft.Add(new KeyValuePair<GameObject, int>(mTargets[idxTarget], idxTarget));
            }
            else
            {
                mRotateGroupRight.Add(new KeyValuePair<GameObject, int>(mTargets[idxTarget], idxTarget));
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mResolved) return;
        if (Common.Utils.ClickedOn(mButtomLeft))
        {
            var backup = new List<KeyValuePair<GameObject, int>>(mRotateGroupLeft);
            mRotateGroupLeft.Clear();
            mRotateGroupLeft.Add(backup[3]);
            mRotateGroupLeft.AddRange(backup.GetRange(0, 3));
        }
        else if (Common.Utils.ClickedOn(mButtomMiddle))
        {
            var backup = mRotateGroupLeft[2];
            mRotateGroupLeft[2] = mRotateGroupRight[0];
            mRotateGroupRight[0] = backup;
        }
        else if (Common.Utils.ClickedOn(mButtomRight))
        {
            var backup = new List<KeyValuePair<GameObject, int>>(mRotateGroupRight);
            mRotateGroupRight.Clear();
            mRotateGroupRight.Add(backup[3]);
            mRotateGroupRight.AddRange(backup.GetRange(0, 3));
        }

        bool isCorret = true;
        for (int i = 0; i < mPosistions.Count; i++)
        {
            var idxAns = int.Parse(mAnsOrder[i].ToString()) - 1;
            if (i < mRotateGroupLeft.Count)
            {
                mRotateGroupLeft[i].Key.transform.position = mPosistions[i];
                if (mRotateGroupLeft[i].Value != idxAns) isCorret = false;
            }
            else
            {
                mRotateGroupRight[i - mRotateGroupLeft.Count].Key.transform.position = mPosistions[i];
                if (mRotateGroupRight[i - mRotateGroupLeft.Count].Value != idxAns) isCorret = false;
            }
        }

        if(isCorret)
        {
            FindObjectOfType<ComputerSwitch>().QuizResolved();
            mResolved = true;
            foreach(var p in mRotateGroupRight)
            {
                Destroy(p.Key);
            }
            foreach(var p in mRotateGroupLeft)
            {
                Destroy(p.Key);
            }
            Destroy(mButtomLeft);
            Destroy(mButtomMiddle);
            Destroy(mButtomRight);
        }
    }
}
