﻿using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class InventoryGlob : ICollection<ItemSlotGlob>
{
    public List<ItemSlotGlob> itemSlots { get; set; }

    public InventoryGlob(in Inventory inventory)
    {
        itemSlots = new List<ItemSlotGlob>();

        foreach (ItemSlot itemSlot in inventory.GetData())
        {
            ItemSlotGlob glob = new ItemSlotGlob(in itemSlot);
            itemSlots.Add(glob);
        }
    }

    public int Count => ((ICollection<ItemSlotGlob>)itemSlots).Count;

    public bool IsReadOnly => ((ICollection<ItemSlotGlob>)itemSlots).IsReadOnly;

    public void Add(ItemSlotGlob item)
    {
        ((ICollection<ItemSlotGlob>)itemSlots).Add(item);
    }

    public void Clear()
    {
        ((ICollection<ItemSlotGlob>)itemSlots).Clear();
    }

    public bool Contains(ItemSlotGlob item)
    {
        return ((ICollection<ItemSlotGlob>)itemSlots).Contains(item);
    }

    public void CopyTo(ItemSlotGlob[] array, int arrayIndex)
    {
        ((ICollection<ItemSlotGlob>)itemSlots).CopyTo(array, arrayIndex);
    }

    public IEnumerator<ItemSlotGlob> GetEnumerator()
    {
        return ((ICollection<ItemSlotGlob>)itemSlots).GetEnumerator();
    }

    public bool Remove(ItemSlotGlob item)
    {
        return ((ICollection<ItemSlotGlob>)itemSlots).Remove(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((ICollection<ItemSlotGlob>)itemSlots).GetEnumerator();
    }
}
