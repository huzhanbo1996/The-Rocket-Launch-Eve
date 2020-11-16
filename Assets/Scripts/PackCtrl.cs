using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackCtrl : MonoBehaviour
{
    public float gapRate;

    private Vector3 mSelectorPosition;
    private float mSelectorHeight;
    private Image mViewImage;
    // Start is called before the first frame update
    void Start()
    {
        mViewImage = transform.Find("View").GetComponent<Image>();
        var dummy = transform.Find("SelectorDummy");
        Debug.Assert(dummy != null);
        mSelectorHeight = dummy.GetComponent<RectTransform>().sizeDelta[1];
        mSelectorPosition = dummy.transform.position;
        //Destroy(dummy.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddHistory(GameObject selector)
    {
        var newSelector = Instantiate(selector);
        newSelector.transform.position = mSelectorPosition;
        mSelectorPosition += (Vector3)(Vector2.down * gapRate * mSelectorHeight);

    }

    public void SetView(Sprite view)
    {
        mViewImage.sprite = view;
    }
}
