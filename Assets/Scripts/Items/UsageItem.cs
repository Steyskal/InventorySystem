using UnityEngine;
using System.Collections;

public class UsageItem : Item {

    protected override void setItemType()
    {
        _itemType = ItemType.Usable;
    }

    public void OnItemPickup()
    {
        Debug.Log(itemName + " picked up!");
        Destroy(gameObject);
    }
}
