using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerSwitch : MonoBehaviour
{
    public int mStage;
    public bool isResolve;

    public GameObject mQuizComputer;
    public GameObject mQuizRotate;
    public GameObject mQuizPoints;
    public QuizElec mQuizElec;
    public List<Sprite> mSpScene;


    private bool mIsSetup;
    private SceneObj mSceneObj;
    private ItemsBox mItemsBox;
    private SpriteRenderer mSpR;
    private List<Sprite> mSps = new List<Sprite>();
    private string RESOURCES_PATH = "QuizComputer";
    // Start is called before the first frame update
    void Start()
    {
        mSpR = this.GetComponent<SpriteRenderer>();
        mSceneObj = this.GetComponent<SceneObj>();
        mStage = 0;
        isResolve = false;
        mSceneObj.relatedQuiz = mQuizComputer;
    }

    // Update is called once per frame
    void Update()
    {
        if ( (mQuizElec == null || mQuizElec.mPhase != 2) && 
             !mIsSetup)
        {
            this.gameObject.GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            mIsSetup = true;
            this.gameObject.GetComponent<Collider2D>().enabled = true;
        }
        mItemsBox = FindObjectOfType<ItemsBox>();
        for (int i = 0; i < 3; i++)
        {
            var sp = Instantiate(Resources.Load<Sprite>(RESOURCES_PATH + "/" + i.ToString())) as Sprite;
            mSps.Add(sp);
        }
        switch (mStage)
        {
            case 0:
                mSpR.sprite = mSpScene[0];
                mSceneObj.relatedQuiz = mQuizComputer;
                mSceneObj.relatedQuizArea = mQuizComputer.transform.Find("Area").gameObject;
                break;
            case 1:
                mSpR.sprite = mSpScene[1];
                this.transform.localScale = new Vector3(1, 1, 1);
                this.transform.position = new Vector3(0, 0, 0);
                this.GetComponent<BoxCollider2D>().offset = new Vector2(-2.96f, -2.65f);
                this.GetComponent<BoxCollider2D>().size = new Vector2(1.42f, 0.92f);
                mSceneObj.relatedQuiz = mQuizRotate;
                mSceneObj.relatedQuizArea = mQuizRotate.transform.Find("Area").gameObject;
                break;
            case 2:
                mSpR.sprite = mSpScene[2];
                this.transform.localScale = new Vector3(1, 1, 1);
                this.transform.position = new Vector3(0, 0, 0);
                this.GetComponent<BoxCollider2D>().offset = new Vector2(-2.96f, -2.65f);
                this.GetComponent<BoxCollider2D>().size = new Vector2(1.42f, 0.92f);
                mSceneObj.relatedQuiz = mQuizPoints;
                mSceneObj.relatedQuizArea = mQuizPoints.transform.Find("Area").gameObject;
                break;
            default:
                Debug.LogError("UNKOWN STAGE");
                break;
        }
    }

    private SpriteRenderer SRofQuizObj(GameObject obj)
    {
        return obj.transform.Find("Area").GetComponent<SpriteRenderer>();
    }

    public void QuizResolved()
    {
        //mSceneObj.relatedQuiz.gameObject.SetActive(false);
        //Common.Utils.SetActiveLayer("Default");
        isResolve = true;
        //SRofQuizObj(mSceneObj.relatedQuiz).sprite = mSps[mStage];
        FindObjectOfType<LevelCtrl>().LevelUp();
    }

    public void TouchLevel()
    {
        mStage++;
        isResolve = false;
    }
}
