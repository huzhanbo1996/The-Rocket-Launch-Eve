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
    // Start is called before the first frame update
    void Start()
    {
        mSpR = this.transform.Find("Area").GetComponent<SpriteRenderer>();
        RESOURCES_PATH += "/wire_";
        foreach(var button in mBtns)
        {
            Debug.Log(RESOURCES_PATH + button.name);
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
            if(Common.Utils.ClickedOn(buttom))
            {
                mCurrSp = mBtnVSSp[buttom];
                mSpR.sprite = mCurrSp;
            }
        }
    }

    public Sprite GetPicture()
    {
        return mCurrSp;
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

