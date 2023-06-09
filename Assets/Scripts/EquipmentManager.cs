using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton;
    public static EquipmentManager instance;

    void Awake()
    {
        instance = this;
    }
    #endregion
    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanaged;
    public Transform itemsParent;
    public GameObject handGun, AR, securityDoorKey, key, marketKey, money, dog, banannas;
    public Item item;

    Slot[] slots;
    Equipment[] currentEquipment; // array of all items currently equipped
    Inventory inventory;
    EquippedInventory equippedInventory;
    public GameObject activeGun, startDoor, marketDoor, securityDoor, kingsley, gary, bill;
    public bool dogUsed, banannaUsed, moneyUsed, keyUsed;
    void Start()
    {
        inventory = Inventory.instance;
        equippedInventory = EquippedInventory.instance;
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots]; // initialise equipment array to length of equipment slots
        slots = itemsParent.GetComponentsInChildren<Slot>();
    }

    public void WeaponEquip(Equipment newItem)
    {
        Equipment weaponEquip = newItem;
        if (newItem.name == "Handgun")
        {
            activeGun.GetComponent<CameraMovement>().handGunActive = true;
            activeGun.GetComponent<CameraMovement>().ARActive = false;
            handGun.SetActive(true);
            AR.SetActive(false);
        }
        if (newItem.name == "AR")
        {
            activeGun.GetComponent<CameraMovement>().handGunActive = false;
            activeGun.GetComponent<CameraMovement>().ARActive = true;
            handGun.SetActive(false);
            AR.SetActive(true);
        }
        if (newItem.name == "Key")
        {
            securityDoor.GetComponent<DoorAccess>().hasSecurityKey = false;
            startDoor.GetComponent<DoorAccess>().playerHasKey = true;
            marketDoor.GetComponent<DoorAccess>().playerHasMarketKey = false;
            gary.GetComponent<Mannequins>().playerHasBannanas = false;
            kingsley.GetComponent<Mannequins>().playerHasDog = false;
            bill.GetComponent<Mannequins>().playerHasMoney = false;
        }
        if (newItem.name == "MarketKey")
        {
            securityDoor.GetComponent<DoorAccess>().hasSecurityKey = false;
            startDoor.GetComponent<DoorAccess>().playerHasKey = false;
            marketDoor.GetComponent<DoorAccess>().playerHasMarketKey = true;
            gary.GetComponent<Mannequins>().playerHasBannanas = false;
            kingsley.GetComponent<Mannequins>().playerHasDog = false;
            bill.GetComponent<Mannequins>().playerHasMoney = false;
        }
        if (newItem.name == "SecurityDoorKey")
        {
            securityDoor.GetComponent<DoorAccess>().hasSecurityKey = true;
            startDoor.GetComponent<DoorAccess>().playerHasKey = false;
            marketDoor.GetComponent<DoorAccess>().playerHasMarketKey = false;
            gary.GetComponent<Mannequins>().playerHasBannanas = false;
            kingsley.GetComponent<Mannequins>().playerHasDog = false;
            bill.GetComponent<Mannequins>().playerHasMoney = false;
        }
        if (newItem.name == "Money")
        {
            securityDoor.GetComponent<DoorAccess>().hasSecurityKey = false;
            startDoor.GetComponent<DoorAccess>().playerHasKey = false;
            marketDoor.GetComponent<DoorAccess>().playerHasMarketKey = false;
            gary.GetComponent<Mannequins>().playerHasBannanas = false;
            kingsley.GetComponent<Mannequins>().playerHasDog = false;
            bill.GetComponent<Mannequins>().playerHasMoney = true;
        }
        if (newItem.name == "Dog")
        {
            securityDoor.GetComponent<DoorAccess>().hasSecurityKey = false;
            startDoor.GetComponent<DoorAccess>().playerHasKey = false;
            marketDoor.GetComponent<DoorAccess>().playerHasMarketKey = false;
            gary.GetComponent<Mannequins>().playerHasBannanas = false ;
            kingsley.GetComponent<Mannequins>().playerHasDog = true;
            bill.GetComponent<Mannequins>().playerHasMoney = false;
        }
        if (newItem.name == "Banannas")
        {
            securityDoor.GetComponent<DoorAccess>().hasSecurityKey = false;
            startDoor.GetComponent<DoorAccess>().playerHasKey = false;
            marketDoor.GetComponent<DoorAccess>().playerHasMarketKey = false;
            gary.GetComponent<Mannequins>().playerHasBannanas = true;
            kingsley.GetComponent<Mannequins>().playerHasDog = false;
            bill.GetComponent<Mannequins>().playerHasMoney = false;
        }
    }

    public void Equip(Equipment newItem) // gets the index of the enum
    {
        int slotIndex = (int)newItem.equipSlot; // get slot index
        Equipment currentItem = currentEquipment[slotIndex];
        Equipment oldItem = null; // store the previous item which was equipped to slot

        if (currentEquipment[slotIndex] != null) // check if there is an item in the slot
        {
            oldItem = currentEquipment[slotIndex];
            inventory.AddItem(oldItem);
            equippedInventory.Remove(currentItem);
        }

        if (onEquipmentChanaged != null)
        {
            onEquipmentChanaged.Invoke(newItem, oldItem);
        }
        currentEquipment[slotIndex] = newItem; // set slot to item picked up
    }

    public void UnequipItem(int slotIndex)
    {
        Equipment oldItem = currentEquipment[slotIndex];
        if (currentEquipment[slotIndex] != null)
        {
            //inventory.AddItem(oldItem);
            equippedInventory.Remove(oldItem);
            currentEquipment[slotIndex] = null;
            if (oldItem.name == "Handgun")
            {
                inventory.AddItem(oldItem);
                handGun.SetActive(false);
                activeGun.GetComponent<CameraMovement>().handGunActive = false;
            }
            if (oldItem.name == "AR")
            {
                inventory.AddItem(oldItem);
                AR.SetActive(false);
                activeGun.GetComponent<CameraMovement>().ARActive = false;
            }
            if (oldItem.name == "Key")
            {
                if (keyUsed)
                {

                }
                else if (!keyUsed)
                {
                    inventory.AddItem(oldItem);
                    startDoor.GetComponent<DoorAccess>().playerHasKey = false;
                }
            }
            if (oldItem.name == "MarketKey")
            {
                if (keyUsed)
                {

                }
                else if (!keyUsed)
                {
                    inventory.AddItem(oldItem);
                    marketDoor.GetComponent<DoorAccess>().playerHasMarketKey = false;
                }
            }
            if (oldItem.name == "SecurityDoorKey")
            {
                if (keyUsed)
                {

                }
                else if (!keyUsed)
                {
                    inventory.AddItem(oldItem);
                    marketDoor.GetComponent<DoorAccess>().hasSecurityKey = false;
                }
            }
            if (oldItem.name == "Money")
            {
                if (moneyUsed)
                {

                }
                else if (!moneyUsed)
                {
                    inventory.AddItem(oldItem);
                    bill.GetComponent<Mannequins>().playerHasMoney = false;
                }
            }
            if (oldItem.name == "Dog")
            {
                if (dogUsed)
                {

                }
                else if (!dogUsed)
                {
                    inventory.AddItem(oldItem);
                    kingsley.GetComponent<Mannequins>().playerHasDog = false;
                }
            }
            if (oldItem.name == "Banannas")
            {
                if (banannaUsed)
                {

                }
                else if (!banannaUsed)
                {
                    inventory.AddItem(oldItem);
                    gary.GetComponent<Mannequins>().playerHasBannanas = false;
                }
            }
            onEquipmentChanaged.Invoke(null, oldItem);
        }
    }

    public void Unequip(Equipment item)
    {
        int slotIndex = (int)item.equipSlot; // get slot index
        Equipment currentItem = currentEquipment[slotIndex];
        UnequipItem(slotIndex);
    }
}
