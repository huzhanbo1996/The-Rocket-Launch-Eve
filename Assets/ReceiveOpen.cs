using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveOpen : MonoBehaviour
{
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var its = this.GetComponent<QuizReception>().GetItems();
        if (its.Count > 0)
        {
            obj.SetActive(true);
            FindObjectOfType<ItemsBox>().MoveItemIn(its[0]);
            its.Clear();
            this.gameObject.SetActive(false);
        }
    }
}
