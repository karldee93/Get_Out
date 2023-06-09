using UnityEngine;
using UnityEngine.UI;
public class Slot : MonoBehaviour
{
    public Image icon; // UI icon image
    public Button removeButton;
    public int slotIdentifier;

    private Transform player;
    private GameObject gameManager;

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
        if (item.name == "Handgun")
        {
            itemToDrop = gameManager.GetComponent<GameManager>().handGun;
        }
        if (item.name == "AR")
        {
            itemToDrop = gameManager.GetComponent<GameManager>().AR;
        }
        if(item.name == "Key")
        {
            itemToDrop = gameManager.GetComponent<GameManager>().key;
        }
        if (item.name == "MarketKey")
        {
            itemToDrop = gameManager.GetComponent<GameManager>().marketKey;
        }
        if (item.name == "Dog")
        {
            itemToDrop = gameManager.GetComponent<GameManager>().dog;
        }
        if (item.name == "Banannas")
        {
            itemToDrop = gameManager.GetComponent<GameManager>().banannas;
        }
        if (item.name == "Money")
        {
            itemToDrop = gameManager.GetComponent<GameManager>().money;
        }
        if (item.name == "ARAmmo")
        {
            itemToDrop = gameManager.GetComponent<GameManager>().aRAmmo;
        }
        if (item.name == "HandgunAmmo")
        {
            itemToDrop = gameManager.GetComponent<GameManager>().hGAmmo;
        }
        if (item.name == "MedKit")
        {
            itemToDrop = gameManager.GetComponent<GameManager>().medKit;
        }

        Inventory.instance.Remove(item);
        Vector3 playerPos = new Vector3(player.position.x - 2, 0, player.position.z);

        Instantiate(itemToDrop, playerPos, Quaternion.identity);
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }
    }

    public void UseEquipped()
    {
        if (item != null)
        {
            item.EquippedUse();
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
