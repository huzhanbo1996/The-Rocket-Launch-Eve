using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizCamera : MonoBehaviour
{
    public GameObject mBtnExit;
    public GameObject mBtnCapture;
    public GameObject mBtnReviewLeft;
    public GameObject mBtnReviewRight;

    public int mCurrReviewIndex;
    public int mCurrCaptureIndex;
    private SpriteRenderer mSpR;
    public List<Sprite> mInventory = new List<Sprite>();
    private List<ICapturable> mCapturable = new List<ICapturable>();
    private SceneCtrl mSceneCtrl;
    private ItemsBox mItemsBox;
    private LayerMask mLayerBefore;
    //  size = scale * pixels / pixPerUnit
    //  scale = size / pixels * pixPerUnit
    private Vector2 mSpriteSize;  
    // Start is called before the first frame update
    void Start()
    {
        mCurrCaptureIndex = -1;
        mCurrReviewIndex = -1;
        mSpR = this.transform.Find("Screen").GetComponent<SpriteRenderer>();
        //mSpriteSize = new Vector2(
        //    mSpR.transform.localScale.x * mSpR.sprite.textureRect.width / mSpR.sprite.pixelsPerUnit,
        //    mSpR.transform.localScale.y * mSpR.sprite.textureRect.height / mSpR.sprite.pixelsPerUnit
        //    );
        GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var rootGameObject in rootGameObjects)
        {
            ICapturable[] childrenInterfaces = rootGameObject.GetComponentsInChildren<ICapturable>(true);
            foreach (var childInterface in childrenInterfaces)
            {
                mCapturable.Add(childInterface);
            }
        }
        mItemsBox = FindObjectOfType<ItemsBox>();
        mSceneCtrl = FindObjectOfType<SceneCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Common.Utils.ClickedOn(mBtnExit))
        {
            Common.Utils.SetActiveLayer(mLayerBefore);
            this.gameObject.SetActive(false);
        }
        if(Common.Utils.ClickedOn(mBtnCapture))
        {
            var sceneObj = mSceneCtrl.GetCurrScene();
            foreach (var capturable in mCapturable)
            {
                if (sceneObj == capturable.GetScene())
                {
                    Debug.Log(sceneObj.name);
                    var sp = capturable.GetPicture();

                    if (!mInventory.Contains(sp) && sp != null)
                    {
                        var newSp = Instantiate(sp);
                        mInventory.Add(sp);
                    }
                    mCurrCaptureIndex = mInventory.IndexOf(sp);
                }
            }
        }

        mBtnReviewLeft.SetActive(true);
        mBtnReviewRight.SetActive(true);
        if (mCurrReviewIndex == -1) mBtnReviewLeft.SetActive(false);
        if (mCurrReviewIndex == mInventory.Count - 1) mBtnReviewRight.SetActive(false);

        if(Common.Utils.ClickedOn(mBtnReviewRight)) mCurrReviewIndex++;
        if(Common.Utils.ClickedOn(mBtnReviewLeft)) mCurrReviewIndex--;

        if( mCurrReviewIndex == -1)
        {
            if (mCurrCaptureIndex != -1)
            {
                mSpR.sprite = mInventory[mCurrCaptureIndex];
            }
            else
            {
                mSpR.sprite = null;
            }
        }
        else
        {
            mSpR.sprite = mInventory[mCurrReviewIndex];
        }
    }

    private void OnEnable()
    {
        mLayerBefore = Common.Utils.GetActiveLayer();
        Common.Utils.SetActiveLayer("Camera");
        mCurrCaptureIndex = -1;
        mCurrReviewIndex = -1;
    }
}
