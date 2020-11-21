using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draguable : MonoBehaviour
{
    public bool freeze;
    public bool isMouseDown = false;
    public Vector2Int limit;

    public bool allowVerticle = true;
    public bool allowHorizental = true;

    private Vector3 mMouseStartPosition;
    private Vector3 mObjStartPosition;
    private Vector3 offset;
    private int INF = 10000;

    public void Initialize(Sprite sp)
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = sp;
        float width = sp.textureRect.width / sp.pixelsPerUnit;
        float height = sp.textureRect.height / sp.pixelsPerUnit;
        this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(width, height);
        this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(
                                                                            width / 2.0f,
                                                                            height / 2.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        limit.x = -INF;
        limit.y = INF;
        freeze = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (freeze)
        {
            return;
        }
        if (Common.Utils.ClickedOn(this.gameObject))
        {
            mMouseStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mObjStartPosition = this.transform.position;
            isMouseDown = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }

        if (isMouseDown)
        {

            offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - mMouseStartPosition;
            offset.x = Mathf.Clamp(offset.x, limit.x, limit.y);
            offset.y = Mathf.Clamp(offset.y, limit.x, limit.y);
            offset.x = allowVerticle ? offset.x : 0;
            offset.y = allowHorizental ? offset.y : 0;
            offset.z = 0;
            transform.position = mObjStartPosition + offset;

        }
    }
}
