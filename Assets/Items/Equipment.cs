using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;
    public bool isActive = false;
    public int damageModifier;
    public Item item;
    public override void Use()
    {
        base.Use();
        item = this;
        EquippedInventory.instance.AddItem(item);
        // equip item
        item.isActive = true;
        EquipmentManager.instance.Equip(this);
        EquipmentManager.instance.WeaponEquip(this);
        // remove from inventory
        RemoveFromInventory();
    }

    public override void EquippedUse()
    {
        base.EquippedUse();
        item = this;
        item.isActive = false;
        EquipmentManager.instance.Unequip(this);
        RemoveFromInventory();
    }
}
public enum EquipmentSlot { Weapon, SpecialItem }
