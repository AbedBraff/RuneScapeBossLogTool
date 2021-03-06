﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Collection for drop objects
public class ItemSlotList : IEnumerable
{
    public ItemSlotList()
    {
        data = new List<ItemSlot>();
    }

    private List<ItemSlot> data;

    // Wrapper for List.Add
    public void Add(in ItemSlot itemSlot)
    {
        data.Add(itemSlot);
        EventManager.Instance.DropListModified(itemSlot.item.itemID);
    }

    //  Modify an existing drop in the collection
    public void AddToDrop(string dropName, in uint addedQuantity)
    {
        ItemSlot drop = Find(dropName);

        //  Make sure adding wouldn't wrap the Drop's quantity (ushort)
        if (drop.quantity.WillWrap(in addedQuantity) && !RareItemDB.IsRare(CacheManager.currentBoss.bossName, drop.item.itemID))
        {
            InputWarningWindow.Instance.OpenWindow($"Cannot add {addedQuantity} to {drop.item.itemName}!\nMaximum item quantity for non-rares is {uint.MaxValue}.");
            return;
        }
        else if(drop.quantity + addedQuantity > ushort.MaxValue && RareItemDB.IsRare(CacheManager.currentBoss.bossName, drop.item.itemID))
        {
            InputWarningWindow.Instance.OpenWindow($"Cannot add {addedQuantity} to {drop.item.itemName}!\nMaximum item quantity for rares is {ushort.MaxValue}.");
            return;
        }

        //  Make sure adding wouldn't wrap the BossLog's lootValue (ulong)
        //  Seriously though it's 18 quintillion and the current most expensive drops hover around 500m - that's 36 billion of that item to reach this code
        if(TotalValue().WillWrap(addedQuantity * drop.item.price))
        {
            InputWarningWindow.Instance.OpenWindow($"Cannot add {addedQuantity} to {drop.item.itemName}!\nMaximum loot value is {ulong.MaxValue}.");
            return;
        }

        drop.quantity += addedQuantity;
        EventManager.Instance.DropListModified(drop.item.itemID);
    }

    //  Wrapper for List.Remove
    public void Remove(in ItemSlot itemSlot)
    {
        data.Remove(itemSlot);
        EventManager.Instance.DropListModified(itemSlot.item.itemID);
    }

    //  Wrapper for List.Clear
    public void Clear()
    {
        data.Clear();
        EventManager.Instance.DropListModified(-1);
    }

    public void Print()
    {
        Debug.Log($"Printing Drop List");

        for (int i = 0; i < data.Count; ++i)
        {
            Debug.Log(data[i].Print());
        }
    }

    //  Wrapper for List.Exists w/ string
    public bool Exists(string dropName)
    {
        return data.Exists(drop => drop.item.itemName.CompareTo(dropName) == 0);
    }

    //  Wrapper for List.Find w/ string
    public ItemSlot Find(string dropName)
    {
        return data.Find(drop => drop.item.itemName.CompareTo(dropName) == 0);
    }

    //  Wrapper for List.Find w/ int
    public ItemSlot Find(int itemID)
    {
        return data.Find(drop => drop.item.itemID == itemID);
    }

    //  Returns the drop at the specified index
    public ItemSlot AtIndex(in int index)
    {
        if(index >= 0 && index < data.Count)
        {
            return data[index];
        }
        else
        {
            return null;
            throw new System.ArgumentOutOfRangeException();
        }
    }

    //  Return the index of the passed ItemSlot
    //  Return -1 if not found
    public int IndexOf(in ItemSlot passedItemSlot)
    {
        for(int i = 0; i < data.Count; ++i)
        {
            if (data[i] == passedItemSlot)
                return i;
        }

        return -1;
    }

    //  Wrapper for List.Count
    public int Count
    {
        get { return data.Count; }
    }

    //  Return the total value of drops in the collection
    public ulong TotalValue()
    {
        ulong totalValue = 0;

        for(int i = 0; i < data.Count; ++i)
        {
            totalValue += data[i].GetValue();
        }

        return totalValue;
    }

    public IEnumerator GetEnumerator()
    {
        return ((IEnumerable)data).GetEnumerator();
    }
}
