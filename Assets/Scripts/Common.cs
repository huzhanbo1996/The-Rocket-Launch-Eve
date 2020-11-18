using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class Utils
    {
        static private LayerMask activeLayers = LayerMask.GetMask("Default");  
        static public bool ClickedAnywhereOut(GameObject obj)
        {
            Debug.Assert(obj.GetComponent<Collider2D>() != null);
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (col.Length == 0)
                {
                    return true;
                }
                else
                {
                    int i = 0;
                    for (i = 0; i < col.Length; i++)
                    {
                        if (col[i].gameObject == obj) break;
                    }
                    if (i == col.Length) return true;
                }

                //Collider2D col = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                //if (col == null) return true;
                //if (col.gameObject != obj) return true;
            }
            return false;
        }

        static public bool ClickedOn(GameObject obj)
        {
            Debug.Assert(obj.GetComponent<Collider2D>() != null);
            if (Input.GetMouseButtonDown(0))
            {
                //Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), activeLayers);
                //foreach(var c in col)
                //{
                //    if (c.gameObject == obj) return true;
                //}
                Collider2D col = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), activeLayers);
                if (col == null) return false;
                if (col.gameObject == obj)
                {
                    return true;
                }
            }
            return false;
        }

        static public void SetActiveLayer(string layerName)
        {
            activeLayers = LayerMask.GetMask(layerName);
        }
    }
}
