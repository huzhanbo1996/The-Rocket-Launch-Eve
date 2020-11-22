using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizPlat : MonoBehaviour
{
    public GameObject mPart1;
    public GameObject mPart2;
    public GameObject mRelatedSceneObj;

    private ItemsBox mItemsBox;
    private SpriteRenderer mSpR;
    private string RESOURCES_PATH = "QuizPlat";
    private List<Sprite> mSps = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        mItemsBox = FindObjectOfType<ItemsBox>();
        mSpR = this.GetComponent<SpriteRenderer>();
        for (int i = 0; i < 3; i++)
        {
            var sp = Instantiate(Resources.Load<Sprite>(RESOURCES_PATH + "/" + i.ToString())) as Sprite;
            mSps.Add(sp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var cnt = this.transform.childCount;
        mSpR.sprite = mSps[cnt];
        if(cnt == 2)
        {
            mItemsBox.MoveItemIn(mPart1);
            mItemsBox.MoveItemIn(mPart2);
            cnt++;
            mRelatedSceneObj.GetComponent<SceneObj>().QuizResolved();
        }
    }
}
