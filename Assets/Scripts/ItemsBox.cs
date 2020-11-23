using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class ItemsBox : MonoBehaviour
{
    public float sizeOfItemX;
    public float sizeOfItemY;
    public List<GameObject> objPickedUp = new List<GameObject>();

    private int mStorageX;
    private int mStorageY;
    private Rect mSpriteTextureRect;
    private float mPixelsPerUnit;
    private GameObject[] mInventory;
    // Start is called before the first frame update
    void Start()
    {
        mSpriteTextureRect = this.GetComponent<SpriteRenderer>().sprite.textureRect;
        mPixelsPerUnit = this.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        mStorageX = (int)(mSpriteTextureRect.width / sizeOfItemX);
        mStorageY = (int)(mSpriteTextureRect.height / sizeOfItemY);
        Debug.Log("mStorageX : " + mStorageX);
        Debug.Log("mStorageY : " + mStorageY);
        mInventory = new GameObject[mStorageX * mStorageY];
        for(int i=0; i < mInventory.Length; i++)
        {
            mInventory[i] = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool MoveItemIn(GameObject obj)
    {
        Debug.Assert(obj != null);
        for (int i = 0; i < mInventory.Length; i++)
        {
            if (mInventory[i] == null)
            {
                var col = i / mStorageY;
                var raw = i % mStorageY;
                obj.transform.parent = this.transform;
                obj.transform.localPosition = new Vector3(
                    col * sizeOfItemX / mPixelsPerUnit + sizeOfItemX / 2.0f / mPixelsPerUnit,
                    -raw * sizeOfItemY / mPixelsPerUnit - sizeOfItemY / 2.0f / mPixelsPerUnit,
                    obj.transform.position.z);
                mInventory[i] = obj;
                obj.SetActive(true);
                return true;
            }
        }
        Debug.LogError("itemsBox is full, consider refactorization!!!");
        return false;
    }

    public bool AddItem(GameObject it, Sprite picIdle, Sprite picPicked, GameObject objToGive, GameObject objCarried)
    {
        Debug.Log("AddItem");
        Debug.Assert(it != null);
        for (int i = 0; i < mInventory.Length; i++)
        {
            if (mInventory[i] == null)
            {
                var obj = Instantiate(it);
                obj.GetComponent<Item>().objToGive = objToGive;
                obj.GetComponent<Item>().objCarried = objCarried;
                obj.GetComponent<Item>().picIdle = picIdle;
                obj.GetComponent<Item>().picPicked = picPicked;
                obj.transform.parent = this.transform;
                var col = i / mStorageY;
                var raw = i % mStorageY;
                obj.transform.localPosition = new Vector3(
                    col * sizeOfItemX / mPixelsPerUnit + sizeOfItemX / 2.0f / mPixelsPerUnit,
                    - raw * sizeOfItemY / mPixelsPerUnit - sizeOfItemY / 2.0f / mPixelsPerUnit,
                    obj.transform.position.z);
                obj.SetActive(true);
                mInventory[i] = obj;
                return true;
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
        for (int i = 0; i < mInventory.Length; i++)
        {
            if (mInventory[i] == it)
            {
                mInventory[i] = null;
                Destroy(it);
                return true;
            }
        }
        Debug.LogError("No items found in imtesBox : " + it.name);
        return false;
    }

    public bool ContainsCloneOf(GameObject obj)
    {
        foreach(var it in mInventory)
        {
            if (it != null && Common.Utils.TrimClone(it.name) == obj.name)
            {
                return true;
            }
        }
        return false;
    }

    public bool ContainsCloneOf(string name)
    {
        foreach (var it in mInventory)
        {
            if (it != null && Common.Utils.TrimClone(it.name) == name)
            {
                return true;
            }
        }
        return false;
    }
}
