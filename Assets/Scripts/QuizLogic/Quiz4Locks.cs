using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Quiz4Locks : MonoBehaviour, IQuizSerializable
{
    public string mInitalCode1;
    public string mInitalCode2;
    public string mAnsCode1;
    public string mAnsCode2;
    public GameObject mBonus1;
    public GameObject mBonus2;
    public GameObject mSceneObj;
    public GameObject piece1;
    public GameObject piece2;

    private List<Sprite> mNumSp = new List<Sprite>();
    private string RESOURCES_PATH = "Quiz4Locks";
    private List<NumCode> mLock1 = new List<NumCode>();
    private List<NumCode> mLock2 = new List<NumCode>();
    private class NumCode
    {
        public GameObject obj;
        public int num;
        public NumCode(GameObject obj, int num)
        {
            this.obj = obj;
            this.num = num;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            string spriteName = i.ToString().ToUpper();
            Debug.Log(RESOURCES_PATH + "/" + spriteName);
            var sp = Instantiate(Resources.Load<Sprite>(RESOURCES_PATH + "/" + spriteName)) as Sprite;
            mNumSp.Add(sp);
        }
        var lock1 = this.transform.Find("Lock1");
        for (int i = 0; i < lock1.transform.childCount; i++)
        {
            mLock1.Add(new NumCode(lock1.transform.GetChild(i).gameObject, i));
        }
        var lock2 = this.transform.Find("Lock2");
        for (int i = 0; i < lock2.transform.childCount; i++)
        {
            mLock2.Add(new NumCode(lock2.transform.GetChild(i).gameObject, i));
        }

        for (int i = 0; i < mInitalCode1.Length; i++)
        {
            int num = int.Parse(mInitalCode1[i].ToString());
            mLock1[i].obj.GetComponent<SpriteRenderer>().sprite = mNumSp[num];
            mLock1[i].num = num;
        }

        for (int i = 0; i < mInitalCode2.Length; i++)
        {
            int num = int.Parse(mInitalCode2[i].ToString());
            mLock2[i].obj.GetComponent<SpriteRenderer>().sprite = mNumSp[num];
            mLock2[i].num = num;
        }
        SetActiveLock2(false);
        mBonus1.SetActive(true);
        mBonus2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var code in mLock1)
        {
            if(Common.Utils.ClickedOn(code.obj))
            {
                int num = (code.num + 1) % 10;
                code.num = num;
                code.obj.GetComponent<SpriteRenderer>().sprite = mNumSp[num];
            }
        }
        if(!mBonus1.activeSelf) // lock lock#2 until lock#1 resolved
        {
            SetActiveLock2(true);
            foreach (var code in mLock2)
            {
                if (Common.Utils.ClickedOn(code.obj))
                {
                    int num = (code.num + 1) % 10;
                    code.num = num;
                    code.obj.GetComponent<SpriteRenderer>().sprite = mNumSp[num];
                }
            }
        }
        
        if(CatAns(mLock1) == mAnsCode1)
        {
            mPhase = 1;
        }

        if(CatAns(mLock2) == mAnsCode2)
        {
            mPhase = 2;
        }

        if(mPhase == 1)
        {
            // make sure it'll be only moved one time
            if (mBonus1.activeSelf == true) FindObjectOfType<ItemsBox>().MoveItemIn(piece1);
            mBonus1.SetActive(false);
            mBonus2.SetActive(true);
        }

        if(mPhase == 2)
        {
            //mBonus2.SetActive(true);
            FindObjectOfType<ItemsBox>().MoveItemIn(piece2);
            mSceneObj.GetComponent<SceneObj>().QuizResolved();
        }

    }

    private void SetActiveLock2(bool b)
    {
        for (int i = 0; i < mInitalCode2.Length; i++)
        {
            mLock2[i].obj.SetActive(b);
        }

        for (int i = 0; i < mInitalCode2.Length; i++)
        {
            mLock1[i].obj.SetActive(!b);
        }
    }


    private string CatAns(List<NumCode> s)
    {
        string ret = "";
        foreach(var code in s)
        {
            ret += code.num.ToString();
        }
        return ret;
    }

    private int mPhase = 0;
    public QuizData Serialize()
    {
        var ret = new QuizData();
        ret.mIntData.Add(mPhase);
        return ret;
    }

    public void Deserialize(QuizData data)
    {
        mPhase = data.mIntData[0];
    }
}
