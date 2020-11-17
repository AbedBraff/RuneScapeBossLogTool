﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlotGlob
{
    public int itemID;
    public uint quantity;

    public ItemSlotGlob(in ItemSlot itemSlot)
    {
        this.itemID = itemSlot.item.itemID;
        this.quantity = itemSlot.quantity;
    }
}