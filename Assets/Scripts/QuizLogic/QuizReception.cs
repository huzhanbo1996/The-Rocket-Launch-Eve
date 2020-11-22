using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizReception : MonoBehaviour
{
    public bool mIsFake;
    private List<GameObject> mInventory = new List<GameObject>();
    private Rect mArea;
    // Start is called before the first frame update
    void Start()
    {
        if (!mIsFake)
        {
            Transform area;
            if(this.gameObject.name == "Area")
            {
                area = this.transform;
            }
            else
            {
                area = this.transform.Find("Area");
            }
            mArea = area.GetComponent<SpriteRenderer>().sprite.rect;
            var mSpriteTextureRect = area.GetComponent<SpriteRenderer>().sprite.textureRect;
            var mPixelsPerUnit = area.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
            var mSpriteCenter = area.position;
            mArea.xMin = mSpriteCenter.x - mSpriteTextureRect.width / mPixelsPerUnit / 2.0f;
            mArea.xMax = mSpriteCenter.x + mSpriteTextureRect.width / mPixelsPerUnit / 2.0f;
            mArea.yMin = mSpriteCenter.y - mSpriteTextureRect.height / mPixelsPerUnit / 2.0f;
            mArea.yMax = mSpriteCenter.y + mSpriteTextureRect.height / mPixelsPerUnit / 2.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(GameObject item)
    {
        Debug.Assert(item != null);
        item.transform.parent = this.transform;
        if (!mIsFake)
        {
            item.transform.position = new Vector3(Random.Range(mArea.xMin, mArea.xMax),
                                                  Random.Range(mArea.yMin, mArea.yMax),
                                                  item.transform.position.z);
            item.SetActive(true);
        }
        else
        {
            item.SetActive(false);
        }
        mInventory.Add(item);
    }

    public List<GameObject> GetItems()
    {
        return mInventory;
    }

    public void RemoveItem(GameObject obj)
    {
        mInventory.Remove(obj);
    }
}
