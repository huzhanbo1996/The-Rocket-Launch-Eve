using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePick : MonoBehaviour, IQuizSerializable
{
    public GameObject mItemBounus;
    public Sprite mSpEnd;
    public Sprite mSpEnd2;
    private int mPhase = 0;
    private LayerMask mRestoreMask;
    private SpriteRenderer mSpR;
    private ItemsBox itemsBox;
    private GameObject mArea;
    private bool isGaved = false;
    // Start is called before the first frame update
    void Start()
    {
        // mPhase = 0;
        // isGaved = false;
        itemsBox = FindObjectOfType<ItemsBox>();
        // mArea = this.transform.Find("Area").gameObject;
        // mSpR = mArea.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mSpEnd2 != null)
        {
            if (mPhase == 0 && Common.Utils.ClickedOn(mArea))
            {
                mSpR.sprite = mSpEnd;
                mPhase++;
            }
            else if (mPhase == 1 && Common.Utils.ClickedOn(mArea))
            {
                mPhase++;
                mSpR.sprite = mSpEnd2;
                itemsBox.MoveItemIn(mItemBounus);
                mRestoreMask = Common.Utils.GetActiveLayer();
                //Common.Utils.SetActiveLayer("Nothing");
                Invoke("SeldDeactive", 1);
            }
        }
        else
        {
            if (!isGaved && Common.Utils.ClickedOn(mArea))
            {
                isGaved = true;
                mSpR.sprite = mSpEnd;
                itemsBox.MoveItemIn(mItemBounus);
                mRestoreMask = Common.Utils.GetActiveLayer();
                //Common.Utils.SetActiveLayer("Nothing");
                Invoke("SeldDeactive", 1);
            }
        } 
    }
    private void SeldDeactive()
    {
        //Common.Utils.SetActiveLayer(mRestoreMask);
        this.gameObject.SetActive(false);
    }

    public QuizData Serialize()
    {
        mArea = this.transform.Find("Area").gameObject;
        mSpR = mArea.GetComponent<SpriteRenderer>();
        var ret = new QuizData();
        ret.mIntData.Add(mPhase);
        ret.mBoolData.Add(isGaved);
        ret.mBoolData.Add(mSpR.sprite == mSpEnd);
        ret.mBoolData.Add(mSpR.sprite == mSpEnd2);
        return ret;
    }

    public void Deserialize(QuizData data)
    {
        mArea = this.transform.Find("Area").gameObject;
        mSpR = mArea.GetComponent<SpriteRenderer>();
        mPhase = data.mIntData[0];
        isGaved = data.mBoolData[0];
        if (data.mBoolData[1]) mSpR.sprite = mSpEnd;
        if (data.mBoolData[2]) mSpR.sprite = mSpEnd2;

    }
}
