using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    int mStillNeed;
    // Start is called before the first frame update
    void Start()
    {
        mStillNeed = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if(mStillNeed == 0)
        {
            Application.Quit();
        }
    }

    public void FinishOneLine()
    {
        mStillNeed--;
    }
}
