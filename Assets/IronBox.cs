using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronBox : MonoBehaviour
{
    public bool isReiceved;
    // Start is called before the first frame update
    void Start()
    {
        isReiceved = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReiceved) return;
        var its = this.GetComponent<QuizReception>().GetItems();
        if(its.Count > 0)
        {
            FindObjectOfType<ItemsBox>().MoveItemIn(its[0]);
            its.Clear();
            this.GetComponent<SceneObj>().hasQuiz = true;
            isReiceved = true;
        }
        else
        {
            this.GetComponent<SceneObj>().hasQuiz = false;
        }
    }
}
