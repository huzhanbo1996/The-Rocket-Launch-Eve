using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class ItemsBox : MonoBehaviour
{
    public float sizeOfItemX;
    public float sizeOfItemY;

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

    public bool AddItem(GameObject it)
    {
        Debug.Log("AddItem");
        Debug.Assert(it != null);
        for (int i = 0; i < mInventory.Length; i++)
        {
            if (mInventory[i] == null)
            {
                var obj = Instantiate(it);
                obj.transform.parent = this.transform;
                var col = i / mStorageY;
                var raw = i % mStorageY;
                obj.transform.localPosition = new Vector3(
                    col * sizeOfItemX / mPixelsPerUnit,
                    - raw * sizeOfItemY / mPixelsPerUnit,
                    obj.transform.position.z);
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
}
