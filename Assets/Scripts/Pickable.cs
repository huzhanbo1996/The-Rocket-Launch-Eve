using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    public GameObject mItemBounus;
    public GameObject mItemBounus2;
    private ItemsBox itemsBox;
    private bool isGaved;
    // Start is called before the first frame update
    void Start()
    {
        isGaved = false;
        itemsBox = FindObjectOfType<ItemsBox>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGaved && Common.Utils.ClickedOn(this.gameObject))
        {
            isGaved = true;
            if (mItemBounus != null) itemsBox.MoveItemIn(mItemBounus);
            if (mItemBounus2 != null) itemsBox.MoveItemIn(mItemBounus2);
        }
    }
}
