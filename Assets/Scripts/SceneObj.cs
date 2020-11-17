using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class SceneObj : MonoBehaviour
{
    public bool pickable;
    public bool draguable;
    public bool visiable;
    public bool hasQuiz;
    public GameObject relatedItem;
    public GameObject relatedQuiz;

    private ItemsBox itemsBox;
    private SpriteRenderer mSRender;
    private GameObject relatedQuizArea;
    private bool showQuiz;
    // Start is called before the first frame update
    void Start()
    {
        Common.Utils.SetActiveLayer("Default");
        mSRender = this.GetComponent<SpriteRenderer>();
        mSRender.enabled = visiable;
        itemsBox = FindObjectOfType<ItemsBox>();
        Debug.Assert(itemsBox != null);
        // TODO a more complete assertion
        Debug.Assert(pickable || draguable || visiable);
        Debug.Assert(pickable == false || relatedItem != null);
        if(relatedQuiz != null)
        {
            relatedQuizArea = relatedQuiz.transform.Find("Area").gameObject;
            Debug.Assert(relatedQuizArea != null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        mSRender.enabled = visiable;
        
        if(hasQuiz && !showQuiz && Common.Utils.ClickedOn(this.gameObject))
        {
            itemsBox.gameObject.SetActive(false);
            relatedQuiz.SetActive(true);
            Common.Utils.SetActiveLayer("Quiz");
            showQuiz = true;
        }
        else if (hasQuiz && showQuiz && Common.Utils.ClickedAnywhereOut(relatedQuizArea))
        {
            itemsBox.gameObject.SetActive(true);
            relatedQuiz.SetActive(false);
            Common.Utils.SetActiveLayer("Default");
            showQuiz = false;
        }

        if (pickable && Common.Utils.ClickedOn(this.gameObject))
        {
            bool ret = itemsBox.AddItem(relatedItem);
            Debug.Assert(ret); // get false only if itemsbox is full, meaning should redesign the itemsbox
            Destroy(this.gameObject);
        }

    }

    private void LateUpdate()
    {
        
    }
    public void QuizResolved()
    {
        itemsBox.gameObject.SetActive(true);
        relatedQuiz.SetActive(false);
        Common.Utils.SetActiveLayer("Default");
        showQuiz = false;
        Destroy(this.gameObject);
    }

    private void OnMouseDrag()
    {
        
    }
}
