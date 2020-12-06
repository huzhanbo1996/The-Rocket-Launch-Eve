using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

public class QuizFinal : MonoBehaviour
{
    public float mTimeDelay;
    public int mPhase = 0;
    public List<Sprite> mSpArea;
    public List<Sprite> mSpAdd;
    public GameObject mObjAdd;
    public List<string> mAnsNameOrder;
    public bool isResolved;
    public GameObject End;
    private Dictionary<string, Sprite> mNameVSPic = new Dictionary<string, Sprite>();
    private QuizReception quizReception;
    private List<GameObject> itBk;
    private List<GameObject> mNowOrd = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        End.SetActive(false);
        isResolved = false;
        quizReception = this.GetComponent<QuizReception>();
        itBk = new List<GameObject>(quizReception.GetItems().ToArray());
        this.GetComponent<SpriteRenderer>().sprite = mSpArea[mPhase];
        for (int i = 0; i < mAnsNameOrder.Count; i++)
        {
            mNameVSPic.Add(mAnsNameOrder[i], mSpAdd[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isResolved)
        {
            End.SetActive(true);
            Common.Utils.SetActive(false);
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            return;
        }
        var its = quizReception.GetItems();
        if (mPhase == mSpArea.Count)
        {
            int i;
            for(i = 0; i < its.Count; i++)
            {
                if(mNowOrd[i].name.IndexOf(mAnsNameOrder[i]) < 0)
                {
                    break;
                }
            }
            if (i != its.Count)
            {
                foreach (var obj in its)
                {
                    FindObjectOfType<ItemsBox>().MoveItemIn(obj);
                }
                its.Clear();
                //StartCoroutine(DelaySeconds(mTimeDelay));
                mPhase = 0;
                mNowOrd.Clear();
                itBk = new List<GameObject>(quizReception.GetItems().ToArray());
                mObjAdd.GetComponent<SpriteRenderer>().sprite = null;
                this.GetComponent<SpriteRenderer>().sprite = mSpArea[mPhase];
                Common.Utils.SetActiveLayer("Default");
                Common.Utils.SetActive(true);
                this.transform.parent.gameObject.SetActive(false);
                return;
            }
            else
            {
                isResolved = true;
                return;
            }
            
        }
        //this.GetComponent<SpriteRenderer>().sprite = mSpArea[mPhase];
        
        if (its.Count != itBk.Count)
        {
            GameObject newObj = null;
            string name = "";
            foreach (var obj in its)
            {
                if (!itBk.Contains(obj))
                {
                    newObj = obj;
                    break;
                }
            }
            foreach(var s in mAnsNameOrder)
            {
                if (newObj.name.IndexOf(s) > 0)
                {
                    name = s;
                }
            }
            mNowOrd.Add(newObj);
            mObjAdd.GetComponent<SpriteRenderer>().sprite = mNameVSPic[name];
            StartCoroutine(DelaySeconds(mTimeDelay));
            mPhase++;
        }
        itBk = new List<GameObject>(quizReception.GetItems().ToArray());
    }
    private IEnumerator DelaySeconds(float delay)
    {
        Common.Utils.SetActive(false);
        yield return new WaitForSeconds(delay);
        Common.Utils.SetActive(true);
        mObjAdd.GetComponent<SpriteRenderer>().sprite = null;
        if (mPhase < mSpArea.Count) this.GetComponent<SpriteRenderer>().sprite = mSpArea[mPhase];
    }
}
