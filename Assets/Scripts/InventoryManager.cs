using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class ToggleEquipItemEvent : UnityEvent<EquipableItem>
{
}

[System.Serializable]
public class InventoryManager
{
    public PlayerController player;
    public Tooltip tooltip;

    public enum EquipSlot
    {
        Head, Torso, Leggs, Weapon
    }

    [Serializable]
    public struct EquipmentSlot
    {
        public EquipSlot type;
        public GameObject slot;
    }

    public EquipmentSlot[] equipmentSlots;
    public Dictionary<EquipSlot, EquipableItem> equipInventory;
    public List<GameObject> inventorySlots;
    public Inventory<Item> inventory = new Inventory<Item>();

    private ToggleEquipItemEvent swapEquipmentEvent = new ToggleEquipItemEvent();

    public InventoryManager()
    {
        equipInventory = new Dictionary<EquipSlot, EquipableItem>();

        foreach (EquipSlot slot in Enum.GetValues(typeof(EquipSlot)))
        {
            equipInventory.Add(slot, null);
        }

        swapEquipmentEvent.AddListener(EquipToggle);
    }

    public ToggleEquipItemEvent getSwapEquipmentEvent()
    {
        return swapEquipmentEvent;
    }

    public void UpdateInventorySize()
    {
        inventory.setSize(inventorySlots.Count);
    }

    public void AddItem(Item pickedUpItem)
    {
        if (pickedUpItem == null)
        {
            Debug.Log("Picked up item does not have an item script on it!");
            return;
        }

        switch (pickedUpItem.getItemType())
        {
            case Item.ItemType.Usable:
                ((UsageItem)pickedUpItem).OnItemPickup();
                break;

            case Item.ItemType.Equipable:
                EquipItem((EquipableItem)pickedUpItem, false);
                break;

            case Item.ItemType.Stackable:
                AddStackItem((StackableItem)pickedUpItem);
                break;

            case Item.ItemType.Normal:
                AddToInventory(pickedUpItem);
                break;
            }
    }

    private GameObject getEquipmentSlot(EquipSlot slotType)
    {
        foreach (EquipmentSlot es in equipmentSlots)
        {
            if (es.type == slotType)
            {
                return es.slot;
            }
        }

        return null;
    }

    private void EquipItem(EquipableItem pickedUpItem, bool swapItems)
    {
        EquipableItem equippedItem;
        equipInventory.TryGetValue(pickedUpItem.equipSlot, out equippedItem);

        if (equippedItem == null && !swapItems)
        {
            equipInventory[pickedUpItem.equipSlot] = pickedUpItem;
            pickedUpItem.transform.SetParent(getEquipmentSlot(pickedUpItem.equipSlot).transform);
            pickedUpItem.setInventoryManager(this);
            pickedUpItem.setItemState(Item.ItemState.Equiped);
        }
        else if (swapItems)
        {
            Item tempItem = equipInventory[pickedUpItem.equipSlot];
            inventory[inventory.IndexOf(pickedUpItem)] = tempItem;

            if (tempItem != null)
            {
                tempItem.transform.SetParent(inventorySlots[inventory.IndexOf(tempItem)].transform);
                tempItem.setItemState(Item.ItemState.InInventory);
            }

            equipInventory[pickedUpItem.equipSlot] = null;

            EquipItem(pickedUpItem, false);
        }
        else
        {
            AddToInventory(pickedUpItem);
        }
    }

    public void DropItem(Item itemToDrop)
    {
        if(!inventory.Remove(itemToDrop))
        {
            equipInventory[((EquipableItem)itemToDrop).equipSlot] = null;
        }

        itemToDrop.setItemState(Item.ItemState.InGame);
    }

    private void EquipToggle(EquipableItem equipableItem)
    {
        switch (equipableItem.getItemState())
        {
            case Item.ItemState.Equiped:
                if (!AddToInventory(equipableItem))
                {
                    DropItem(equipableItem);
                }

                equipInventory[equipableItem.equipSlot] = null;
                break;

            case Item.ItemState.InInventory:
                EquipItem(equipableItem, true);
                break;
        }
    }

    private void AddStackItem(StackableItem pickedUpItem)
    {
        foreach (Item item in inventory)
        {
            if (item != null)
            {
                if (item.getItemType() == Item.ItemType.Stackable)
                {
                    if (item.itemName.Equals(pickedUpItem.itemName))
                    {
                        StackableItem tempStackableItem = (StackableItem)item;

                        if (tempStackableItem.AddStack())
                        {
                            pickedUpItem.Destroy();
                            inventorySlots[inventory.IndexOf(tempStackableItem)].GetComponentInChildren<Text>().text = tempStackableItem.getStackStatus();

                            return;
                        }
                    }
                }
            }
        }

        if (AddToInventory(pickedUpItem))
        {
            inventorySlots[inventory.IndexOf(pickedUpItem)].transform.GetChild(1).GetComponent<Image>().enabled = true;
            inventorySlots[inventory.IndexOf(pickedUpItem)].GetComponentInChildren<Text>().text = pickedUpItem.getStackStatus();
        }
    }

    private bool AddToInventory(Item pickedUpItem)
    {
        if (inventory.isFull())
        {
            string message = "Inventory is full!";
            Debug.Log(message);
            player.DisplayMessage(message);

            return false;
        }

        inventory.Add(pickedUpItem);
        pickedUpItem.setInventoryManager(this);
        pickedUpItem.transform.SetParent(inventorySlots[inventory.IndexOf(pickedUpItem)].transform);

        pickedUpItem.setItemState(Item.ItemState.InInventory);

        return true;
    }
}
