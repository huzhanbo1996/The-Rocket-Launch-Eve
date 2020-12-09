using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class ItemsBox : MonoBehaviour
{
    public float sizeOfItemX;
    public float sizeOfItemY;
    public List<GameObject> objPickedUp = new List<GameObject>();

    public float mHideMove;
    public GameObject mBtnLeft;
    public GameObject mBtnRight;
    public GameObject mBtnUp;
    public GameObject mBtnDown;
    public GameObject mQuizCamera;
    public List<GameObject> Axis = new List<GameObject>();
    public float mHideUpDown;

    private int mCurrVisible;
    private int mStorageX;
    private int mStorageY;
    private Rect mSpriteTextureRect;
    private float mPixelsPerUnit;
    private List<GameObject[]> mInventoryList = new List<GameObject[]>();
    private SoundEffect mSE;
    private bool mDuringArragement;
    // Start is called before the first frame update
    void Start()
    {
        mDuringArragement = false;
        mSE = FindObjectOfType<SoundEffect>();
        Axis[2].transform.position = Axis[1].transform.position = Axis[0].transform.position;
        Axis[1].transform.position += (Vector3)(Vector2.up * mHideUpDown);
        Axis[2].transform.position += (Vector3)(Vector2.up * mHideUpDown * 2);
        mCurrVisible = 0;
        mBtnUp.SetActive(false);
        mBtnDown.SetActive(false);
        mBtnLeft.SetActive(false);
        mSpriteTextureRect = this.GetComponent<SpriteRenderer>().sprite.textureRect;
        mPixelsPerUnit = this.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        mStorageX = (int)(mSpriteTextureRect.width / sizeOfItemX);
        mStorageY = (int)(mSpriteTextureRect.height / sizeOfItemY);
        Debug.Log("mStorageX : " + mStorageX);
        Debug.Log("mStorageY : " + mStorageY);
        for (int cnt = 0; cnt < 3; cnt++)
        {
            var mInventory = new GameObject[mStorageX * mStorageY];
            for (int i = 0; i < mInventory.Length; i++)
            {
                mInventory[i] = null;
            }
            mInventoryList.Add(mInventory);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void LateUpdate()
    {
        if (!mQuizCamera.activeSelf)
        {
            if (Common.Utils.ClickedOn(mBtnRight))
            {
                this.transform.position += (Vector3)(Vector2.right * mHideMove);
                mBtnRight.SetActive(false);
                mBtnLeft.SetActive(true);
            }
            if (Common.Utils.ClickedOn(mBtnLeft))
            {
                this.transform.position -= (Vector3)(Vector2.right * mHideMove);
                mBtnLeft.SetActive(false);
                mBtnRight.SetActive(true);
            }

            if (Common.Utils.ClickedOn(mBtnUp))
            {
                mCurrVisible--;
                for (int cnt = 0; cnt < 3; cnt++)
                {
                    Axis[cnt].transform.position += (Vector3)(Vector2.up * mHideUpDown);
                }
            }

            if (Common.Utils.ClickedOn(mBtnDown))
            {
                mCurrVisible++;
                for (int cnt = 0; cnt < 3; cnt++)
                {
                    Axis[cnt].transform.position += (Vector3)(Vector2.down * mHideUpDown);
                }
            }
        }

        mBtnUp.SetActive(true);
        mBtnDown.SetActive(true);
        //Debug.Log(mCurrVisible);
        //Debug.Log("after" + FindIsEmpty(true).ToString());
        //Debug.Log("before" + FindIsEmpty(false).ToString());


        if (FindIsEmpty(true))
        {
            mBtnDown.SetActive(false);
        }
        if (FindIsEmpty(false))
        {
            mBtnUp.SetActive(false);
        }
    }

    private bool FindIsEmpty(bool towardAfter)
    {
        if (towardAfter)
        {
            for (int idx = mCurrVisible + 1; idx < mInventoryList.Count; idx++)
            {
                foreach(var it in mInventoryList[idx])
                {
                    if (it != null) return false;
                }
            }
        }
        else
        {
            for (int idx = mCurrVisible - 1; idx >= 0; idx--)
            {
                foreach (var it in mInventoryList[idx])
                {
                    if (it != null) return false;
                }
            }
        }
        return true;   
    }

    public bool MoveItemIn(GameObject obj, bool duringPersist = false)
    {
        Debug.Assert(obj != null);
        for (int idx = 0; idx < mInventoryList.Count; idx++)
        {
            var mInventory = mInventoryList[idx];
            for (int i = 0; i < mInventory.Length; i++)
            {
                if (mInventory[i] == obj)
                {
                    mInventory[i] = null;
                }
            }
        }

        for (int idx = 0; idx < mInventoryList.Count; idx++)
        {
            var mInventory = mInventoryList[idx];
            for (int i = 0; i < mInventory.Length; i++)
            {
                if (mInventory[i] == null)
                {
                    var col = i / mStorageY;
                    var raw = i % mStorageY;
                    obj.transform.parent = Axis[idx].transform;
                    obj.transform.localPosition = new Vector3(
                        col * sizeOfItemX / mPixelsPerUnit + sizeOfItemX / 2.0f / mPixelsPerUnit,
                        -raw * sizeOfItemY / mPixelsPerUnit - sizeOfItemY / 2.0f / mPixelsPerUnit,
                        -2);
                        //obj.transform.localPosition.z);
                    mInventory[i] = obj;
                    obj.SetActive(true);
                    if (!mDuringArragement && !duringPersist) 
                    {
                        FindObjectOfType<Persist>().AddItem(obj.GetComponent<Item>());
                    }
                    mSE.Play(SoundEffect.SOUND_TYPE.PICK);
                    return true;
                }
            }
        }
        
        Debug.LogError("itemsBox is full, consider refactorization!!!");
        return false;
    }

    public bool AddItem(GameObject it, Sprite picIdle, Sprite picPicked, GameObject objToGive, GameObject objCarried)
    {
        Debug.Log("AddItem");
        Debug.Assert(it != null);
        for (int idx = 0; idx < mInventoryList.Count; idx++)
        {
            var mInventory = mInventoryList[idx];
            for (int i = 0; i < mInventory.Length; i++)
            {
                if (mInventory[i] == null)
                {
                    var obj = Instantiate(it);
                    obj.GetComponent<Item>().objToGive = objToGive;
                    obj.GetComponent<Item>().objCarried = objCarried;
                    obj.GetComponent<Item>().picIdle = picIdle;
                    obj.GetComponent<Item>().picPicked = picPicked;
                    obj.transform.parent = Axis[idx].transform;
                    var col = i / mStorageY;
                    var raw = i % mStorageY;
                    obj.transform.localPosition = new Vector3(
                        col * sizeOfItemX / mPixelsPerUnit + sizeOfItemX / 2.0f / mPixelsPerUnit,
                        -raw * sizeOfItemY / mPixelsPerUnit - sizeOfItemY / 2.0f / mPixelsPerUnit,
                        obj.transform.position.z);
                    obj.SetActive(true);
                    mInventory[i] = obj;
                    //Rearangement();
                    FindObjectOfType<Persist>().AddItem(obj.GetComponent<Item>());
                    mSE.Play(SoundEffect.SOUND_TYPE.PICK);
                    return true;
                }
            }
        }
        Debug.LogError("itemsBox is full, consider refactorization!!!");
        return false;
    }

    public bool RemoveItem(GameObject it)
    {
        Debug.Assert(it != null);
        if(objPickedUp.Contains(it))
        {
            objPickedUp.Remove(it);
        }
        for (int idx = 0; idx < mInventoryList.Count; idx++)
        {
            var mInventory = mInventoryList[idx];
            for (int i = 0; i < mInventory.Length; i++)
            {
                if (mInventory[i] == it)
                {
                    mInventory[i] = null;
                    FindObjectOfType<Persist>().RemoveItem(it.GetComponent<Item>().picIdle);
                    Destroy(it);
                    Rearangement();
                    return true;
                }
            }
        }
        Debug.LogError("No items found in imtesBox : " + it.name);
        return false;
    }

    private void Rearangement()
    {
        mSE.SetActive(false);
        mDuringArragement = true;
        for (int idx = 0; idx < mInventoryList.Count; idx++)
        {
            var mInventory = mInventoryList[idx];
            for (int i = 0; i < mInventory.Length; i++)
            {
                var obj = mInventory[i];
                mInventory[i] = null;
                if (obj != null)
                {
                    MoveItemIn(obj);
                }
            }
        }
        mDuringArragement = false;
        mSE.SetActive(true);
    }

    public bool ContainsCloneOf(GameObject obj)
    {
        for (int idx = 0; idx < mInventoryList.Count; idx++)
        {
            var mInventory = mInventoryList[idx];
            foreach (var it in mInventory)
            {
                if (it != null && Common.Utils.TrimClone(it.name) == obj.name)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool ContainsCloneOf(string name)
    {
        for (int idx = 0; idx < mInventoryList.Count; idx++)
        {
            var mInventory = mInventoryList[idx];
            foreach (var it in mInventory)
            {
                if (it != null && Common.Utils.TrimClone(it.name) == name)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
