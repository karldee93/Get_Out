using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Equipped_Invent : MonoBehaviour
{
    public Transform itemsParent;

    EquippedInventory equippedInventory;

    Slot[] slots;

    Item item;
    private void Start()
    {
        equippedInventory = EquippedInventory.instance; // calls on singleton
        equippedInventory.onItemChangedCallback += UpdateUI; // event trigger whenever a new item has been added or removed

        slots = itemsParent.GetComponentsInChildren<Slot>(); // fill array with each icon slot
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < equippedInventory.items.Count) // more items to add
            {
                Debug.Log("add item");
                slots[i].AddItem(equippedInventory.items[i]); // pass in item at the index location on the invent items array

            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
