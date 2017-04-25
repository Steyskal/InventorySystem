using UnityEngine;
using System.Collections;

public class NormalItem : Item {

    protected override void setItemType()
    {
        _itemType = ItemType.Normal;
    }
}
