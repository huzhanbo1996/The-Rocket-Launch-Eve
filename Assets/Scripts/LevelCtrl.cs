using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCtrl : MonoBehaviour
{
    public List<GameObject> mGates = new List<GameObject>();
    public int mCurrGate;
    public ComputerSwitch mCS;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var g in mGates)
        {
            g.SetActive(false);
        }
        Debug.Assert(mCS != null);
    }

    // Update is called once per frame
    void Update()
    {
        if(Common.Utils.ClickedOn(mGates[mCurrGate])) // inc level when last gate touched
        {
            mCS.TouchLevel();
            mCurrGate++;
        }
    }

    public void LevelUp()
    {
        mGates[mCurrGate].SetActive(true);  // active one when puzzle resolve
    }
}
