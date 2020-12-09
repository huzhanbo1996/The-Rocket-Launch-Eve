using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCtrl : MonoBehaviour, IQuizSerializable
{
    public List<GameObject> mChangeSceneBtn = new List<GameObject>();
    public List<GameObject> mGateSceneObj = new List<GameObject>();
    public int mCurrGate;
    public ComputerSwitch mCS;

    private bool mJustLevelUp = false;
    // Start is called before the first frame update
    void Start()
    {
        // mJustLevelUp = false;
        foreach (var g in mChangeSceneBtn)
        {
            g.SetActive(false);
        }
        Debug.Assert(mCS != null);
    }

    // Update is called once per frame
    void Update()
    {
        if (mJustLevelUp &&
            mCurrGate + 1 < mGateSceneObj.Count &&
            Common.Utils.ClickedOn(mGateSceneObj[mCurrGate + 1])) // inc level when last gate touched
        {
            mJustLevelUp = false;
            mCurrGate++;
            mCS.TouchLevel();
        }
    }

    public void LevelUp()
    {
        mJustLevelUp = true;
        mGateSceneObj[mCurrGate].SetActive(false);
        mChangeSceneBtn[mCurrGate].SetActive(true);  // active one when puzzle resolve
    }

    public QuizData Serialize()
    {
        var ret = new QuizData();
        ret.mBoolData.Add(mJustLevelUp);
        ret.mIntData.Add(mCurrGate);
        return ret;
    }

    public void Deserialize(QuizData data)
    {
        mJustLevelUp = data.mBoolData[0];
        mCurrGate = data.mIntData[0];
        for (int idx = 0; idx < mCurrGate; idx++)
        {
            mGateSceneObj[idx].SetActive(false);
            mChangeSceneBtn[idx].SetActive(true);
        }
    }
}
