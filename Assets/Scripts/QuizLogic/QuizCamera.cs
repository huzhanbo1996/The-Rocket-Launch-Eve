using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public Sprite mSpBackGournd;
    public int mScreenWidth;
    public int mScreenHeight;
    public bool isGrab = true;
    public ItemCamera mItemCamera;
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
        mScreenWidth = mSpR.sprite.texture.width;
        mScreenHeight = mSpR.sprite.texture.height;
        //mSpR.sprite = mSpBackGournd;

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
            isGrab = true;
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
                    Sprite newSp = null; 

                    if (!mInventory.Contains(sp) && sp != null)
                    {
                        var newTx = Instantiate(sp.texture);
                        TextureScale.Bilinear(newTx, mScreenWidth, mScreenHeight);
                        newSp = Sprite.Create(newTx, new Rect(0, 0, mScreenWidth, mScreenHeight), new Vector2(0, 0));
                        mInventory.Add(newSp);
                    }
                    mCurrCaptureIndex = mInventory.IndexOf(newSp);
                    mCurrReviewIndex = mCurrCaptureIndex;
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
            mSpR.sprite = mSpBackGournd;
            //if (mCurrCaptureIndex != -1)
            //{
            //    mSpR.sprite = mInventory[mCurrCaptureIndex];
            //}
            //else
            //{
            //    mSpR.sprite = mSpBackGournd;
            //}
        }
        else
        {
            mSpR.sprite = mInventory[mCurrReviewIndex];
        }
    }

    private void OnEnable()
    {
        //Start();
        //isGrab = false;
        //mSpR.sprite = mSpBackGournd;
        mLayerBefore = Common.Utils.GetActiveLayer();
        Common.Utils.SetActiveLayer("Camera");
        mCurrCaptureIndex = -1;
        mCurrReviewIndex = -1;
    }
}
