using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Inventory<Item> : List<Item>{

    private int _size;

    public void setSize(int size)
    {
        _size = size;
    }

    public bool isFull()
    {
        if (Count == _size)
        {
            foreach (Item i in ToArray())
            {
                if (i == null)
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

    public new void Add(Item item)
    {
        foreach (Item i in ToArray())
        {
            if (i == null)
            {
                base[IndexOf(i)] = item;
                return;
            }
        }

        base.Add(item);
    }

    public new bool Remove(Item item)
    {
        try
        {
            base[IndexOf(item)] = default(Item);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
