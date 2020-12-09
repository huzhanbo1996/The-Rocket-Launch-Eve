using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizComputer : MonoBehaviour, IQuizSerializable
{
    public GameObject ItemIdCard;
    public int mPhase = 0;
    public List<Sprite> mSps = new List<Sprite>();
    private ItemsBox itemsBox;


    public QuizData Serialize()
    {
        var ret = new QuizData();
        ret.mIntData.Add(mPhase);
        
        var inventory = this.GetComponent<QuizReception>().GetItems();
        Debug.Assert(inventory != null);
        foreach(var obj in inventory)
        {
            ret.mStringData.Add(obj.name);
        }
        return ret;
    }
    public void Deserialize(QuizData data)
    {
        mPhase = data.mIntData[0];

        var inventory = this.GetComponent<QuizReception>();
        Debug.Assert(inventory != null);
        foreach(var objName in data.mStringData)
        {
            var fakeItem = new GameObject(objName);
            inventory.AddItem(fakeItem);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
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
