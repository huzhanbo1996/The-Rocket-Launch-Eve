using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizWire : MonoBehaviour, ICapturable, IQuizSerializable
{
    public List<GameObject> mBtns = new List<GameObject>();

    private SpriteRenderer mSpR;
    private Dictionary<GameObject, Sprite> mBtnVSSp = new Dictionary<GameObject, Sprite>();
    public Dictionary<Sprite, Sprite> mSpVSPic = new Dictionary<Sprite, Sprite>();
    private string RESOURCES_PATH = "QuizWire";
    private Sprite mCurrSp;
    private QuizReception mQuizReception;
    // Start is called before the first frame update
    void Start()
    {
        mQuizReception = this.transform.Find("Area").GetComponent<QuizReception>();
        mSpR = this.transform.Find("Area").GetComponent<SpriteRenderer>();
        RESOURCES_PATH += "/wire_";
        foreach(var button in mBtns)
        {
            Debug.Log(RESOURCES_PATH + button.name.ToLower());
            var rs = Resources.Load<Sprite>(RESOURCES_PATH + button.name);
            var sp = Instantiate(rs) as Sprite;
            var rs2 = Resources.Load<Sprite>(RESOURCES_PATH + button.name + "_wire");
            var sp2 = Instantiate(rs2) as Sprite;
            Debug.Log(RESOURCES_PATH + button.name);
            mBtnVSSp.Add(button, sp);
            mSpVSPic.Add(sp, sp2);
        }
        mCurrSp = mBtnVSSp[mBtns[4]]; // last for orig
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var buttom in mBtns)
        {
            foreach (var it in mQuizReception.GetItems())
            {
                if (it.name.ToLower().IndexOf(buttom.name) >= 0)
                {
                    mCurrSp = mBtnVSSp[buttom];
                    mSpR.sprite = mCurrSp;
                }
            }
        }
        // TODO
        // resolve this memory leak
        mQuizReception.GetItems().Clear(); 
    }
    private void OnEnable()
    {
        if (mBtnVSSp.Count > 0)
        {
            mCurrSp = mBtnVSSp[mBtns[4]]; // last for orig
            mSpR.sprite = mCurrSp;
        }
    }

    public Sprite GetPicture()
    {
        Sprite ret = null;
        if (this.gameObject.gameObject.activeSelf)
        {
            ret =  mSpVSPic[mCurrSp];
            mSpVSPic[mCurrSp] = null;
        }
        return ret;
    }
    public GameObject GetScene()
    {
        Debug.Assert(this.transform.parent.gameObject.name.Contains("Scene"));
        return this.transform.parent.gameObject;
    }

    public QuizData Serialize()
    {
        var ret = new QuizData();
        foreach(var key in mSpVSPic.Keys)
        {
            ret.mBoolData.Add(mSpVSPic[key] == null);
        }
        return ret;
    }

    public void Deserialize(QuizData data)
    {
        int idx = 0;
        foreach(var key in mSpVSPic.Keys)
        {
            if (data.mBoolData[idx++])
            {
                mSpVSPic[key] = null;
            }
        }
    }
}

public interface ICapturable
{
    Sprite GetPicture();
    GameObject GetScene();
}

