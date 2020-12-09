using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class QuizLock : MonoBehaviour, IQuizSerializable
{
    public string mAllCodes;
    public string mInitialCode;
    public string mAnsCode;
    public List<Code> mCodes = new List<Code>();
    public GameObject SceneObj;
    public GameObject mQuizBonus;
    public Sprite mSpScneObj2;
    public bool isResolved = false;

    public class Code
    {
        public GameObject obj;
        public string name;

        public Code(GameObject gameObject, string v)
        {
            this.obj = gameObject;
            this.name = v;
        }
    }
    private ItemsBox mItemsBox;
    private string mCodeSeq;
    private string mNowCode;
    private Dictionary<string, Sprite> mCodeVSSprite = new Dictionary<string, Sprite>();
    private string RESOURCES_PATH = "QuizLock";
    // Start is called before the first frame update
    void Start()
    {
        // isResolved = false;
        mItemsBox = FindObjectOfType<ItemsBox>();
        var childCodes = this.transform.Find("Codes");
        for (int i = 0; i < childCodes.childCount; i++)
        {
            mCodes.Add(new Code(childCodes.GetChild(i).gameObject, mInitialCode[i].ToString().ToUpper()));
        }
        for (int i = 0; i< mAllCodes.Length;i++)
        {
            string spriteName = mAllCodes[i].ToString().ToUpper();
            Debug.Log(RESOURCES_PATH + "/" + spriteName);
            var sp = Instantiate(Resources.Load<Sprite>(RESOURCES_PATH + "/" + spriteName)) as Sprite;
            mCodeVSSprite.Add(spriteName, sp);
        }
        mCodeSeq = mAllCodes;
        mNowCode = mInitialCode;
        RenderCode();
        Debug.Assert(mCodeVSSprite.Count == mAllCodes.Length);
        Debug.Assert(mCodes.Count == mInitialCode.Length);
        //foreach(var obj in mCodes)
        //{
        //    Debug.Log(obj.obj.name);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (isResolved) return;
        for (int i = 0; i < mCodes.Count; i++)
        {
            var code = mCodes[i];
            if(Common.Utils.ClickedOn(code.obj))
            {
                var newName = FindNextInSeq(mCodeSeq, code.name[0]);
                mNowCode = mNowCode.Substring(0, i) + newName + mNowCode.Substring(i + 1);
                code.name = newName;
            }
        }
        RenderCode();
        if(mNowCode == mAnsCode)
        {
            //mQuizBonus.transform.parent = this.transform.parent;
            //mQuizBonus.transform.localPosition = new Vector3(
            //    SceneObj.transform.localPosition.x,
            //    SceneObj.transform.localPosition.y,
            //    mQuizBonus.transform.position.z
            //    );
            FindObjectOfType<SoundEffect>().Play(SoundEffect.SOUND_TYPE.QUIZ);
            mQuizBonus.SetActive(true);
            isResolved = true;
            SceneObj.GetComponent<SpriteRenderer>().sprite = mSpScneObj2;
            SceneObj.GetComponent<SpriteRenderer>().GetComponent<Collider2D>().enabled = false;
            //SceneObj.GetComponent<SceneObj>().QuizResolved();
        }
    }

    private string FindNextInSeq(string seq, char now)
    {
        var idx = seq.IndexOf(now);
        idx++;
        idx %= seq.Length;
        return seq[idx].ToString().ToUpper();
    }
    private void RenderCode()
    {
        for (int i = 0; i < mNowCode.Length; i++)
        {
            string code = mNowCode[i].ToString().ToUpper();
            mCodes[i].obj.GetComponent<SpriteRenderer>().sprite = mCodeVSSprite[code];
        }
    }

    public QuizData Serialize()
    {
        var ret = new QuizData();
        ret.mBoolData.Add(isResolved);
        return ret;
    }

    public void Deserialize(QuizData data)
    {
        isResolved = data.mBoolData[0];
        if (isResolved)
        {
            mQuizBonus.SetActive(true);
            SceneObj.GetComponent<SpriteRenderer>().sprite = mSpScneObj2;
            SceneObj.GetComponent<SpriteRenderer>().GetComponent<Collider2D>().enabled = false;
        }
    }
}
