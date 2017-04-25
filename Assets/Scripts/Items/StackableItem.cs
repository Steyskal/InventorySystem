using UnityEngine;
using System.Collections;

public class StackableItem : Item {

    public bool hasLimit;
    //TODO: Inspector custom editor
    public int stackLimit;

    private int _stacks = 1;

    protected override void setItemType()
    {
        _itemType = ItemType.Stackable;
    }

    public bool AddStack()
    {
        if (!hasLimit || !isFull())
        {
            _stacks++;
            return true;
        }

        return false;
    }

    public bool isFull()
    {
        if (_stacks == stackLimit)
            return true;

        return false;
    }

    public string getStackStatus()
    {
        if (hasLimit){
            string status = _stacks == stackLimit ? "<color=orange>" : "<color=yellow>";
            return status + _stacks + "/" + stackLimit + "</color>";
        }
        else{
            return _stacks.ToString();
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
