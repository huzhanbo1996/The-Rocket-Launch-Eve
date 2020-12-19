using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizPlat : MonoBehaviour, IQuizSerializable
{
    public GameObject mPart1;
    public GameObject mPart2;
    public GameObject mRelatedSceneObj;
    public int mPhase = 0;
    private ItemsBox mItemsBox;
    private SpriteRenderer mSpR;
    private string RESOURCES_PATH = "QuizPlat";
    private List<Sprite> mSps = new List<Sprite>();

    public void Deserialize(QuizData data)
    {
        mPhase = data.mIntData[0];
        int childCnt = data.mIntData[1];
        for (int idx = 0; idx < childCnt;idx++)
        {
            var fakeItem = new GameObject();
            fakeItem.transform.parent = this.gameObject.transform;
        }
    }

    public QuizData Serialize()
    {
        var ret = new QuizData();
        ret.mIntData.Add(mPhase);
        ret.mIntData.Add(this.transform.childCount);
        return ret;
    }

    // Start is called before the first frame update
    void Start()
    {
        // mPhase = 0;
        mPart1.SetActive(false);
        mPart2.SetActive(false);
        mItemsBox = FindObjectOfType<ItemsBox>();
        mSpR = this.GetComponent<SpriteRenderer>();
        for (int i = 0; i < 4; i++)
        {
            var sp = Instantiate(Resources.Load<Sprite>(RESOURCES_PATH + "/" + i.ToString())) as Sprite;
            mSps.Add(sp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mPhase == -1) return;
        mSpR.sprite = mSps[mPhase];

        if (mPhase == 3)
        {
            mItemsBox.MoveItemIn(mPart1);
            mItemsBox.MoveItemIn(mPart2);
            mPhase = -1;
            return;
        }

        if (mPhase == 2 && Common.Utils.ClickedOn(this.gameObject))
        {
            mPhase = 3;
            //mRelatedSceneObj.GetComponent<SceneObj>().QuizResolved();
        }
        if (mPhase < 2)
        {
            mPhase = this.transform.childCount;
        }
    }
}
