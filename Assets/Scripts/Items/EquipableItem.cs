using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EquipableItem : Item, IPointerClickHandler {

    public InventoryManager.EquipSlot equipSlot;

    protected override void setItemType()
    {
        _itemType = ItemType.Equipable;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && _itemType == ItemType.Equipable)
        {
            _inventoryManager.getSwapEquipmentEvent().Invoke(this);
        }
    }
}
