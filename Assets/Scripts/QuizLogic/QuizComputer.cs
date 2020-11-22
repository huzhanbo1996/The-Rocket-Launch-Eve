using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizComputer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.GetComponent<QuizReception>().GetItems().Count> 0)
        {
            FindObjectOfType<ComputerSwitch>().QuizResolved();
            this.GetComponent<QuizReception>().GetItems().Clear();
        }
    }
}
