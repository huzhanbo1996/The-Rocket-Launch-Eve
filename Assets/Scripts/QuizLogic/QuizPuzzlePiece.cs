using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class QuizPuzzlePiece : MonoBehaviour
{
    public bool freeze;
    public bool isMouseDown = false;
    //private Vector3 lastMousePosition = Vector3.zero;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
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
            offset.z = 0;
            // Debug.Log("offset:" + offset);
            transform.position = offset;
            //lastMousePosition = transform.position;
             
        }
    }
}
