using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizBook : MonoBehaviour
{
    public GameObject mBtnRight;
    public GameObject mBtnLeft;

    private int mCurrPage;
    private SpriteRenderer mSR;
    private string RESOURCES_PATH = "QuizLibrary";
    private List<Sprite> mSps = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        RESOURCES_PATH += "/" + this.gameObject.name;
        for (int i = 0; i <= 10; i++)
        {
            string spriteName = i.ToString().ToUpper();
            Debug.Log(RESOURCES_PATH + "/" + spriteName);
            var rs = Resources.Load<Sprite>(RESOURCES_PATH + "/" + spriteName);
            if(rs != null)
            {
                var sp = Instantiate(rs) as Sprite;
                mSps.Add(sp);
            }
        }
        Debug.Assert(mSps.Count > 0);
        mSR = this.transform.Find("Area").GetComponent<SpriteRenderer>();
        mCurrPage = 0;
        Debug.Assert(mBtnLeft != null);
        Debug.Assert(mBtnRight != null);
    }

    // Update is called once per frame
    void Update()
    {
        mSR.sprite = mSps[mCurrPage];
        mBtnLeft.SetActive(true);
        mBtnRight.SetActive(true);
        if(mCurrPage == 0)
        {
            mBtnLeft.SetActive(false);
        }
        if(mCurrPage == mSps.Count - 1)
        {
            mBtnRight.SetActive(false);
        }
        if(Common.Utils.ClickedOn(mBtnLeft))
        {
            mCurrPage--;
        }
        if(Common.Utils.ClickedOn(mBtnRight))
        {
            mCurrPage++;
        }
    }
}
