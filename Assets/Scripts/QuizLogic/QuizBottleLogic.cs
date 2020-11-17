using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class QuizBottleLogic : MonoBehaviour
{
    public List<GameObject> mButtoms = new List<GameObject>();
    public List<GameObject> mAnsOrder = new List<GameObject>();
    public GameObject mButtomFlush;
    public GameObject mButtunLine;
    public GameObject mBandPrefab;
    public GameObject mSceneObj;
    public bool mResolved;

    private string mResourcePath = "QuizBottle";
    private Vector3 mButtun;
    private Vector3 mCurrPosition;
    private List<GameObject> mBottleBandsKeeper = new List<GameObject>();
    private Dictionary<GameObject, Sprite> mButtomsVSSprite = new Dictionary<GameObject, Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        mResolved = false;
        mCurrPosition = mButtun = mButtunLine.transform.localPosition;
        foreach(var buttom in mButtoms)
        {
            string spriteName = buttom.name.Replace("bottun", "band");
            Debug.Log(mResourcePath + "/" + spriteName);
            var sp = Instantiate(Resources.Load<Sprite>(mResourcePath + "/" + spriteName)) as Sprite;
            mButtomsVSSprite.Add(buttom, sp);
        }
        //foreach(var pairs in mButtomsVSSprite)
        //{
        //    Debug.Log(pairs.Key.name + " : " + pairs.Value.name);
        //    Debug.Log(pairs.Key.GetComponent<Collider2D>());
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Common.Utils.ClickedOn(mButtomFlush))
        {
            foreach (var band in mBottleBandsKeeper)
            {
                Destroy(band.gameObject);
            }
            mCurrPosition = mButtun;
            mBottleBandsKeeper.Clear();
        }
        foreach(var buttom in mButtomsVSSprite)
        {
            if (Common.Utils.ClickedOn(buttom.Key))
            {
                var newBand = Instantiate(mBandPrefab);
                newBand.GetComponent<SpriteRenderer>().sprite = buttom.Value;
                newBand.transform.localPosition = mCurrPosition;
                newBand.transform.parent = this.transform;
                newBand.transform.name = buttom.Key.name;
                mCurrPosition += (Vector3)(Vector2.up * buttom.Value.textureRect.height 
                                                        / buttom.Value.pixelsPerUnit);
                mBottleBandsKeeper.Add(newBand);
            }
        }
        if(mBottleBandsKeeper.Count > mAnsOrder.Count)
        {
            foreach(var band in mBottleBandsKeeper)
            {
                Destroy(band.gameObject);
            }
            mCurrPosition = mButtun;
            mBottleBandsKeeper.Clear();
        }
        else if (mBottleBandsKeeper.Count == mAnsOrder.Count)
        {
            int i = 0;
            for (i = 0; i < mBottleBandsKeeper.Count; i++)
            {
                Debug.Log(mBottleBandsKeeper[i].name + ":" + mAnsOrder[i].name);
                if (mBottleBandsKeeper[i].name != mAnsOrder[i].name) break;
            }
            if(i == mBottleBandsKeeper.Count)
            {
                mResolved = true;
                mSceneObj.GetComponent<SceneObj>().QuizResolved();
            }
        }
    }
}
