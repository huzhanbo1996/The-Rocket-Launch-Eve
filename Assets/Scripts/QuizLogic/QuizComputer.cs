using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizComputer : MonoBehaviour
{
    public GameObject ItemIdCard;
    private ItemsBox itemsBox;
    // Start is called before the first frame update
    void Start()
    {
        itemsBox = FindObjectOfType<ItemsBox>();
    }

    // Update is called once per frame
    void Update()
    {
        var inventory = this.GetComponent<QuizReception>().GetItems();
        if (inventory.Count > 0)
        {
            if(itemsBox.ContainsCloneOf("ItemIdCard"))
            {
                FindObjectOfType<ComputerSwitch>().QuizResolved();
                inventory.Clear();
            }
            else
            {
                itemsBox.MoveItemIn(inventory[0]);
                inventory.Clear();
            }
        }
    }
}
