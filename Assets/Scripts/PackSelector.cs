using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackSelector : MonoBehaviour
{
    public Sprite view;
    private PackCtrl packCtrl;
    // Start is called before the first frame update
    void Start()
    {
        packCtrl = FindObjectOfType<PackCtrl>();
        Debug.Assert(packCtrl != null);
        Debug.Assert(view != null);
    }

    // Update is called once per frame
    void Update()
    {
        if (Common.Utils.ClickedOn(this.gameObject))
        {
            packCtrl.SetView(view);
        }
    }

}
