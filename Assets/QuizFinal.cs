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
    private QuizReception quizReception;
    private List<GameObject> itBk;
    // Start is called before the first frame update
    void Start()
    {
        isResolved = false;
        quizReception = this.GetComponent<QuizReception>();
        itBk = new List<GameObject>(quizReception.GetItems().ToArray());
        this.GetComponent<SpriteRenderer>().sprite = mSpArea[mPhase];
    }

    // Update is called once per frame
    void Update()
    {
        if (mPhase == mSpArea.Count)
        {
            isResolved = true;
            return;
        }
        //this.GetComponent<SpriteRenderer>().sprite = mSpArea[mPhase];
        var its = quizReception.GetItems();
        if (its.Count != itBk.Count)
        {
            GameObject newObj = null;
            foreach (var obj in its)
            {
                if (!itBk.Contains(obj))
                {
                    newObj = obj;
                    break;
                }
            }
            if (newObj.name.IndexOf(mAnsNameOrder[mPhase]) > 0)
            {
                mObjAdd.GetComponent<SpriteRenderer>().sprite = mSpAdd[mPhase];
                StartCoroutine(DelaySeconds(mTimeDelay));
                mPhase++;

            }
            else
            {
                foreach (var obj in its)
                {
                    FindObjectOfType<ItemsBox>().MoveItemIn(obj);
                }
                its.Clear();
                //StartCoroutine(DelaySeconds(mTimeDelay));
                mPhase = 0;
                mObjAdd.GetComponent<SpriteRenderer>().sprite = null;
                this.GetComponent<SpriteRenderer>().sprite = mSpArea[mPhase];
                this.transform.parent.gameObject.SetActive(false);
            }
            //switch (mPhase)
            //{
            //    case 0:   
            //        break;
            //    case 1:
            //        break;
            //    case 2:
            //        break;
            //}
        }
        itBk = new List<GameObject>(quizReception.GetItems().ToArray());
    }
    private IEnumerator DelaySeconds(float delay)
    {
        Common.Utils.SetActive(false);
        yield return new WaitForSeconds(delay);
        Common.Utils.SetActive(true);
        mObjAdd.GetComponent<SpriteRenderer>().sprite = null;
        this.GetComponent<SpriteRenderer>().sprite = mSpArea[mPhase];
    }
}
