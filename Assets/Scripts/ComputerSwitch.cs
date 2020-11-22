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
    private SceneObj mSceneObj;
    private ItemsBox mItemsBox;
    private List<Sprite> mSps = new List<Sprite>();
    private string RESOURCES_PATH = "QuizComputer";
    // Start is called before the first frame update
    void Start()
    {
        mSceneObj = this.GetComponent<SceneObj>();
        mStage = 0;
        isResolve = false;
        mSceneObj.relatedQuiz = mQuizComputer;
    }

    // Update is called once per frame
    void Update()
    {
        mItemsBox = FindObjectOfType<ItemsBox>();
        for (int i = 0; i < 3; i++)
        {
            var sp = Instantiate(Resources.Load<Sprite>(RESOURCES_PATH + "/" + i.ToString())) as Sprite;
            mSps.Add(sp);
        }
        switch (mStage)
        {
            case 0:
                mSceneObj.relatedQuiz = mQuizComputer;
                mSceneObj.relatedQuizArea = mQuizComputer.transform.Find("Area").gameObject;
                break;
            case 1:
                mSceneObj.relatedQuiz = mQuizRotate;
                mSceneObj.relatedQuizArea = mQuizRotate.transform.Find("Area").gameObject;
                break;
            case 2:
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
        SRofQuizObj(mSceneObj.relatedQuiz).sprite = mSps[mStage];
        FindObjectOfType<LevelCtrl>().LevelUp();
    }

    public void TouchLevel()
    {
        mStage++;
        isResolve = false;
    }
}
