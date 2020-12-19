using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizJuiceMachine : MonoBehaviour, IQuizSerializable
{
    public GameObject mLiguidRed;
    public GameObject mLiguidGreen;
    public Sprite mSpRed;
    public Sprite mSpGreen;
    public Sprite mSpVide;
    private enum STATE { HAS, CAN_GIVE, GAVE};
    private STATE mStGreen = STATE.HAS;
    private STATE mStRed = STATE.HAS;
    private QuizReception mQuizReception;
    private ItemsBox mItemsBox;
    // Start is called before the first frame update
    void Start()
    {
        // mSpVide = Instantiate(this.GetComponent<SpriteRenderer>().sprite);
        // mStGreen = STATE.HAS;
        // mStRed = STATE.HAS;
        mItemsBox = FindObjectOfType<ItemsBox>();
        mQuizReception = this.GetComponent<QuizReception>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mStRed==STATE.CAN_GIVE || mStGreen == STATE.CAN_GIVE)
        {
            if (mStGreen == STATE.CAN_GIVE && Common.Utils.ClickedOn(this.gameObject))
            {
                mStGreen = STATE.GAVE;
                this.GetComponent<SpriteRenderer>().sprite = mSpVide;
                mItemsBox.MoveItemIn(mLiguidGreen);
            }
            if (mStRed == STATE.CAN_GIVE && Common.Utils.ClickedOn(this.gameObject))
            {
                mStRed = STATE.GAVE;
                this.GetComponent<SpriteRenderer>().sprite = mSpVide;
                mItemsBox.MoveItemIn(mLiguidRed);
            }
            return;
        }
        foreach ( var it in mQuizReception.GetItems())
        {
            if(mStGreen == STATE.HAS && Common.Utils.TrimClone(it.name) == mLiguidGreen.name)
            {
                mStGreen = STATE.CAN_GIVE;
                this.GetComponent<SpriteRenderer>().sprite = mSpGreen;
                mQuizReception.RemoveItem(it);
                return;
            }
            if (mStRed == STATE.HAS && Common.Utils.TrimClone(it.name) == mLiguidRed.name)
            {
                mStRed = STATE.CAN_GIVE;
                this.GetComponent<SpriteRenderer>().sprite = mSpRed;
                mQuizReception.RemoveItem(it);
                return;
            }
        }
    }

    public QuizData Serialize()
    {
        var ret = new QuizData();
        ret.mIntData.Add((int)mStRed);
        ret.mIntData.Add((int)mStGreen);

        var inventory = this.GetComponent<QuizReception>();
        Debug.Assert(inventory != null);
        foreach(var obj in inventory.GetItems())
        {
            ret.mStringData.Add(obj.name);
        }
        return ret;
    }

    public void Deserialize(QuizData data)
    {
        int idx = 0;
        mStRed = (STATE)data.mIntData[idx++];
        mStGreen = (STATE)data.mIntData[idx++];
        if (mStRed == STATE.CAN_GIVE) this.GetComponent<SpriteRenderer>().sprite = mSpRed;
        if (mStGreen == STATE.CAN_GIVE) this.GetComponent<SpriteRenderer>().sprite = mSpGreen;
        var inventory = this.GetComponent<QuizReception>();
        Debug.Assert(inventory != null);
        foreach(var objName in data.mStringData)
        {
            var fakeItem = new GameObject(objName);
            inventory.AddItem(fakeItem);
        }
    }
}
