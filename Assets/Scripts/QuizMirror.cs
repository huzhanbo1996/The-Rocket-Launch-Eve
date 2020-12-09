using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizMirror : MonoBehaviour, ICapturable, IQuizSerializable
{
    public GameObject item;
    public Sprite mPic;
    private bool isGaved = false;
    // Start is called before the first frame update
    void Start()
    {
        // mPic = GetComponent<SpriteRenderer>().sprite;
        // isGaved = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sprite GetPicture()
    {
        if (this.gameObject.transform.parent.gameObject.activeSelf)
        {
            if(!isGaved)
            {
                isGaved = true;
                FindObjectOfType<ItemsBox>().MoveItemIn(item);
            }
            var ans = mPic;
            mPic = null;
            return ans;
        }
        else
        {
            return null;
        }
    }
    public GameObject GetScene()
    {
        return this.transform.parent.parent.gameObject;
    }

    public QuizData Serialize()
    {
        var ret = new QuizData();
        ret.mBoolData.Add(isGaved);
        ret.mBoolData.Add(mPic == null);
        return ret;
    }

    public void Deserialize(QuizData data)
    {
        isGaved = data.mBoolData[0];
        mPic = data.mBoolData[1] ? null : mPic;
    }
}
