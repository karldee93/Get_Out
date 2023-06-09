using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public bool isActive = false;
    public int itemIdentifier;

    public virtual void Use() // allows for this funtion to be changed depedning on the item
    {
        // use the item
        // something might happen

        Debug.Log("Item used " + name);
        if (this.name == "HandgunAmmo")
        {
            GameObject player;
            player = GameObject.FindGameObjectWithTag("MainCamera");
            player.GetComponent<CameraMovement>().isHgAmmo = true;
            player.GetComponent<CameraMovement>().AddAmmo();
            if (player.GetComponent<CameraMovement>().ammoAdded == true)
            {
                RemoveFromInventory();
                player.GetComponent<CameraMovement>().ammoAdded = false;
            }

        }
        if (this.name == "ARAmmo")
        {
            GameObject player;
            player = GameObject.FindGameObjectWithTag("MainCamera");
            player.GetComponent<CameraMovement>().isArAmmo = true;
            player.GetComponent<CameraMovement>().AddAmmo();
            if (player.GetComponent<CameraMovement>().ammoAdded == true)
            {
                RemoveFromInventory();
                player.GetComponent<CameraMovement>().ammoAdded = false;
            }
        }
        if (this.name == "MedKit")
        {
            GameObject player;
            player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerController>().AddHealth();
            if (player.GetComponent<PlayerController>().healthAdded == true)
            {
                RemoveFromInventory();
                player.GetComponent<PlayerController>().healthAdded = false;
            }
        }
    }
    public virtual void EquippedUse()
    {

    }
    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }
}
