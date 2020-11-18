using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizReception : MonoBehaviour
{
    private List<GameObject> mInventory = new List<GameObject>();
    private Rect mArea;
    // Start is called before the first frame update
    void Start()
    {
        mArea = this.transform.Find("Area").GetComponent<SpriteRenderer>().sprite.rect;
        var mSpriteTextureRect = this.transform.Find("Area").GetComponent<SpriteRenderer>().sprite.textureRect;
        var mPixelsPerUnit = this.transform.Find("Area").GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        var mSpriteCenter = this.transform.Find("Area").position;
        mArea.xMin = mSpriteCenter.x - mSpriteTextureRect.width / mPixelsPerUnit / 2.0f;
        mArea.xMax = mSpriteCenter.x + mSpriteTextureRect.width / mPixelsPerUnit / 2.0f;
        mArea.yMin = mSpriteCenter.y - mSpriteTextureRect.height / mPixelsPerUnit / 2.0f;
        mArea.yMax = mSpriteCenter.y + mSpriteTextureRect.height / mPixelsPerUnit / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(GameObject item)
    {
        Debug.Assert(item != null);
        item.transform.parent = this.transform;
        item.transform.position = new Vector3(Random.Range(mArea.xMin, mArea.xMax),
                                              Random.Range(mArea.yMin, mArea.yMax),
                                              0);
        item.SetActive(true);
        mInventory.Add(item);
    }

    public List<GameObject> GetItems()
    {
        return mInventory;
    }
}
