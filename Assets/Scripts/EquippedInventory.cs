using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedInventory : MonoBehaviour
{
    public static EquippedInventory instance;
    #region Singleton
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    //public bool[] isFull;
    //public GameObject[] slots;
    public int space = 20;

    public List<Item> items = new List<Item>();

    public bool AddItem(Item item)
    {
        if (items.Count >= space)
        {
            return false;
        }
        items.Add(item);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke(); // update inventory UI
        }
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke(); // update inventory UI
        }
    }
}
