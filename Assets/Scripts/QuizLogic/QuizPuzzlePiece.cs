using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class QuizPuzzlePiece : MonoBehaviour
{
    public Transform area;
    public bool freeze;
    public bool isMouseDown = false;

    //private Vector3 lastMousePosition = Vector3.zero;
    private Rect mArea;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {

        mArea = area.GetComponent<SpriteRenderer>().sprite.rect;
        var mSpriteTextureRect = area.GetComponent<SpriteRenderer>().sprite.textureRect;
        var mPixelsPerUnit = area.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        var mSpriteCenter = area.position;
        mArea.xMin = mSpriteCenter.x - mSpriteTextureRect.width / mPixelsPerUnit / 2.0f;
        mArea.xMax = mSpriteCenter.x + mSpriteTextureRect.width / mPixelsPerUnit / 2.0f;
        mArea.yMin = mSpriteCenter.y - mSpriteTextureRect.height / mPixelsPerUnit / 2.0f;
        mArea.yMax = mSpriteCenter.y + mSpriteTextureRect.height / mPixelsPerUnit / 2.0f;

        freeze = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(freeze)
        {
            return;
        }
        if (Common.Utils.ClickedOn(this.gameObject))
        {
            isMouseDown = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }

        if (isMouseDown)
        {
            
            offset = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            offset.z = transform.position.z;
            offset.x = Mathf.Clamp(offset.x, mArea.xMin, mArea.xMax);
            offset.y = Mathf.Clamp(offset.y, mArea.yMin, mArea.yMax);
            // Debug.Log("offset:" + offset);
            transform.position = offset;
            //lastMousePosition = transform.position;
             
        }
    }
}
