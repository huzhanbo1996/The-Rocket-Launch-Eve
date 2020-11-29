using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizIngredient : MonoBehaviour
{
    public List<GameObject> mObjs;
    public List<GameObject> mShows;
    public bool enable;
    public GameObject mCamera;

    private ItemsBox mItemsBox;
    private Dictionary<GameObject, GameObject> mObjVSShow = new Dictionary<GameObject, GameObject>();
    private GameObject mCurrObj;
    private GameObject mCurrObj2;
    // Start is called before the first frame update
    void Start()
    {
        enable = false;
        mItemsBox = FindObjectOfType<ItemsBox>();
        Debug.Assert(mItemsBox != null);
        mCurrObj = null;
        mCurrObj2 = null;
        for (int i = 0; i < mObjs.Count; i++)
        {
            Debug.Assert(mObjs[i] != null && mShows[i] != null);
            Debug.Assert(mShows[i].transform.Find("Area").gameObject != null);
            mObjVSShow.Add(mObjs[i], mShows[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!enable) return;
        if (mCurrObj == null)
        {
            foreach(var obj in mObjs)
            {
                if(Common.Utils.ClickedOn(obj))
                {
                    mObjVSShow[obj].SetActive(true);
                    mCurrObj = obj;
                    return;
                }
            }
            if( Common.Utils.ClickedAnywhereOut(this.gameObject) &&
                !Common.Utils.ClickedOnChildenOf(mItemsBox.gameObject) &&
                !mCamera.activeSelf
               )
            {
                Common.Utils.SetActiveLayer("Default");
                this.transform.parent.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (var obj in mObjs)
            {
                if (Common.Utils.ClickedOn(obj) && obj!=mCurrObj)
                {
                    mObjVSShow[obj].SetActive(true);
                    mCurrObj2 = obj;
                    return;
                }
            }
            if (Common.Utils.ClickedAnywhereOut(mObjVSShow[mCurrObj].transform.Find("Area").gameObject) &&
                !Common.Utils.ClickedOnChildenOf(mItemsBox.gameObject)
                )
            {
                mObjVSShow[mCurrObj].SetActive(false);
                if (mCurrObj2 != null) mObjVSShow[mCurrObj2].SetActive(false);
                mCurrObj = null;
                mCurrObj2 = null;
            }
        }
    }
    private void OnEnable()
    {
        Invoke("DelayEnalbe", 0.5f);
    }
    private void DelayEnalbe()
    {
        enable = true;
    }
}
