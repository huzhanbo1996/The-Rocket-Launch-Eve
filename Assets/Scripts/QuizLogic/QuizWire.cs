using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizWire : MonoBehaviour, ICapturable
{
    public List<GameObject> mBtns = new List<GameObject>();

    private SpriteRenderer mSpR;
    private Dictionary<GameObject, Sprite> mBtnVSSp = new Dictionary<GameObject, Sprite>();
    private string RESOURCES_PATH = "QuizWire";
    private Sprite mCurrSp;
    private QuizReception mQuizReception;
    // Start is called before the first frame update
    void Start()
    {
        mQuizReception = this.transform.Find("Area").GetComponent<QuizReception>();
        mSpR = this.transform.Find("Area").GetComponent<SpriteRenderer>();
        mCurrSp = mSpR.sprite;
        RESOURCES_PATH += "/wire_";
        foreach(var button in mBtns)
        {
            Debug.Log(RESOURCES_PATH + button.name.ToLower());
            var rs = Resources.Load<Sprite>(RESOURCES_PATH + button.name);
            var sp = Instantiate(rs) as Sprite;
            mBtnVSSp.Add(button, sp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var buttom in mBtns)
        {
            foreach (var it in mQuizReception.GetItems())
            {
                if (it.name.IndexOf(buttom.name) >= 0)
                {
                    mCurrSp = mBtnVSSp[buttom];
                    mSpR.sprite = mCurrSp;
                }
            }
        }
    }
    private void OnEnable()
    {
        
    }

    public Sprite GetPicture()
    {
        if (this.gameObject.gameObject.activeSelf)
        {
            return mCurrSp;
        }
        else
        {
            return null;
        }
    }
    public GameObject GetScene()
    {
        Debug.Assert(this.transform.parent.gameObject.name.Contains("Scene"));
        return this.transform.parent.gameObject;
    }
}

public interface ICapturable
{
    Sprite GetPicture();
    GameObject GetScene();
}

