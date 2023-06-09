using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EquippedSlot : MonoBehaviour
{
    public Image icon; // UI icon image
    public Button removeButton;
    private Transform player;
    private GameObject gameManager;
    public Text itemName;

    SpawnItem itemToSpawn;
    Item item; // UI item
    GameObject itemToDrop;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Canvas");
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void AddItem(Item newItem)
    {
        item = newItem; // add item to ui

        itemName.text = item.name;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        if (item.name == "Gun")
        {
            itemToDrop = gameManager.GetComponent<GameManager>().handGun;
        }
        if (item.name == "Knife")
        {
            itemToDrop = gameManager.GetComponent<GameManager>().AR;
        }

        Inventory.instance.Remove(item);
        Vector3 playerPos = new Vector3(player.position.x, player.position.y, player.position.z + 2);

        Instantiate(itemToDrop, playerPos, Quaternion.identity);
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }
    }

    public void SpawnDroppedItem()
    {

    }
    //private Inventory inventory;
    //public int i;
    //private void Start()
    //{
    //    inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    //}

    //private void Update()
    //{
    //    if(transform.childCount <= 0)
    //    {
    //        inventory.isFull[i] = false;
    //    }
    //}
    public void DropItem()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<SpawnItem>().SpawnDroppedItem();
            GameObject.Destroy(child.gameObject);
        }
    }
}
