using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShowName : MonoBehaviour
{
    public GameObject nameSp;
    // Start is called before the first frame update
    void Start()
    {
        nameSp.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        nameSp.SetActive(true);
    }
    private void OnMouseExit()
    {
        nameSp.SetActive(false);
    }
}
