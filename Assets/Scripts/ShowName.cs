using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShowName : MonoBehaviour
{
    public GameObject nameSp;
    public bool enable = true;
    // Start is called before the first frame update
    void Start()
    {
        //enable = true;
        nameSp.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        if (enable) nameSp.SetActive(true);
    }
    private void OnMouseExit()
    {
        if (enable) nameSp.SetActive(false);
    }

    public void SetActive(bool enable)
    {
        nameSp.SetActive(enable);
        this.enable = enable;
    }
}
