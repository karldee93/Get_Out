using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    public Transform itemsParent;

    Inventory inventory;

    Slot[] slots;
    private void Start()
    {
        inventory = Inventory.instance; // calls on singleton
        inventory.onItemChangedCallback += UpdateUI; // event trigger whenever a new item has been added or removed

        slots = itemsParent.GetComponentsInChildren<Slot>(); // fill array with each icon slot
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count) // more items to add
            {
                slots[i].AddItem(inventory.items[i]); // pass in item at the index location on the invent items array
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
