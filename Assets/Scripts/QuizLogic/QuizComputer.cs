using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizComputer : MonoBehaviour
{
    public GameObject ItemIdCard;
    public int mPhase;
    public List<Sprite> mSps = new List<Sprite>();
    private ItemsBox itemsBox;
    // Start is called before the first frame update
    void Start()
    {
        mPhase = 0;
        itemsBox = FindObjectOfType<ItemsBox>();
    }

    // Update is called once per frame
    void Update()
    {

        var inventory = this.GetComponent<QuizReception>().GetItems();
        if (mPhase == 1 && inventory.Count > 0)
        {
            if(itemsBox.ContainsCloneOf("ItemIdCard"))
            {
                mPhase++;
                FindObjectOfType<ComputerSwitch>().QuizResolved();
                inventory.Clear();
            }
            else
            {
                itemsBox.MoveItemIn(inventory[0]);
                inventory.Clear();
            }
        }

        if (mPhase == 0 && itemsBox.ContainsCloneOf("ItemIdCard"))
        {
            mPhase++;
        }
        this.GetComponent<SpriteRenderer>().sprite = mSps[mPhase];
    }
}
