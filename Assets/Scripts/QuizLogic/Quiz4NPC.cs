using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Quiz4NPC : MonoBehaviour, ICapturable
{
    public List<GameObject> mNPCinOrder;
    public string mAnsOrder;
    public float mShowMsgTime = 2.0f;
    public GameObject mRelatedSceneObj;
    public GameObject mItemBonus;
    public Sprite mPictureHold;

    private string mNowOrder;
    private QuizReception mQuizReception;
    private List<NPC> mNPCs = new List<NPC>();
    private LayerMask tmpLayer;
    private class NPC
    {
        public GameObject obj;
        public bool isActive;
        public char num;
        public bool talked;
        public GameObject msg;
        public GameObject musicSign;

        public NPC(GameObject obj, bool isActive, char num)
        {
            this.obj = obj;
            this.isActive = isActive;
            this.num = num;
            this.talked = false;
            msg = this.obj.transform.Find("msg").gameObject;
            musicSign = this.obj.transform.Find("music sign").gameObject;
            msg.SetActive(false);
            musicSign.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tmpLayer = -1;
        mQuizReception = this.transform.Find("Area").GetComponent<QuizReception>();
        for (int i = 0; i < mNPCinOrder.Count; i++)
        {
            var obj = mNPCinOrder[i];
            mNPCs.Add(new NPC(obj, false, char.Parse((i + 1).ToString())));
        }
    }

    // Update is called once per frame
    void Update()
    {

        // don't render then, just for knowing them received
        var invenotory = mQuizReception.GetItems();
        foreach (var obj in invenotory)
        {
            obj.SetActive(false);
            foreach (var tgt in mNPCs)
            {
                if (Common.Utils.TrimClone(obj.name) == tgt.obj.name)
                {
                    foreach (var _tgt in mNPCs)
                    {
                        _tgt.isActive = true;
                    }
                    //tgt.isActive = true;
                }
            }
        }

        foreach (var tgt in mNPCs)
        {
            if (Common.Utils.ClickedOn(tgt.obj))
            {
                if (!tgt.isActive)  // initial state
                {
                    if(!tgt.talked)
                    {
                        ShowMsg(tgt.msg, mShowMsgTime);
                        tgt.talked = true;
                    }
                }
                else // state 2
                {
                    //ShowMsg(tgt.msg, mShowMsgTime);
                    if(tgt.musicSign.activeSelf)
                    {
                        foreach(var clr in mNPCs)
                        {
                            clr.musicSign.SetActive(false);
                        }
                        mNowOrder = "";
                    }
                    else
                    {
                        tgt.musicSign.SetActive(true);
                        mNowOrder += tgt.num;
                    }
                    
                }
            }
        }

        if (mNowOrder == mAnsOrder && tmpLayer == -1)
        {
            tmpLayer = Common.Utils.GetActiveLayer();
            Common.Utils.SetActive(false);
            Ending();
        }

    }

    private void Ending()
    {
        for (int i = 0; i < mAnsOrder.Length; i++)
        {
            char c = mAnsOrder[i];
            foreach (var tgt in mNPCs)
            {
                if (tgt.num == c)
                {
                    StartCoroutine(DelayShowMsg(tgt.msg, i * mShowMsgTime, (i + 1) * mShowMsgTime));
                }

            }
        }
        StartCoroutine(DelayDestory(mAnsOrder.Length * mShowMsgTime));
    }


    public Sprite GetPicture()
    {
        if (mNPCs[0].isActive)
            return mPictureHold;
        else
            return null;
    }

    public GameObject GetScene()
    {
        Debug.Assert(this.transform.parent.gameObject.name.Contains("Scene"));
        return this.transform.parent.gameObject;
    }

    private void OnEnable()
    {
        foreach (var tgt in mNPCs)
        {
            tgt.talked = false;
        }
    }
    private void ShowMsg(GameObject msg, float time)
    {
        msg.SetActive(true);
        StartCoroutine(DelayToDiasable(msg, time));
    }
    public static IEnumerator DelayToDiasable(GameObject obj, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        obj.SetActive(false);
    }

    private IEnumerator DelayDestory(float delay1)
    {
        yield return new WaitForSeconds(delay1);
        Common.Utils.SetActive(true);
        FindObjectOfType<ItemsBox>().MoveItemIn(mItemBonus);
        mRelatedSceneObj.GetComponent<SceneObj>().QuizResolved();
    }
    private static IEnumerator DelayShowMsg(GameObject msg, float delay1, float delay2)
    {
        yield return new WaitForSeconds(delay1);
        msg.SetActive(true);
        yield return new WaitForSeconds(delay2);
        msg.SetActive(false);
    }
}
