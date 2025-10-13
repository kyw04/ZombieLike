using System.Collections.Generic;
using UnityEngine;
using Item;

[System.Serializable]
public class InventorySlot
{
    public ItemBase item;
    public int count;

    public InventorySlot(ItemBase item, int count)
    {
        this.item = item;
        this.count = count;
    }

    public void Add(int amount)
    {
        count += amount;
        if (count > item.maxCount)
            count = item.maxCount;
    }

    public void Remove(int amount)
    {
        count -= amount;
        if (count < 0) count = 0;
    }

    public bool IsEmpty => item == null || count <= 0;
}


public class Inventory : MonoBehaviour
{
    [SerializeField] private int capacity = 20;
    public List<InventorySlot> slots = new List<InventorySlot>();

    private void Awake()
    {
        for (int i = 0; i < capacity; i++)
        {
            slots.Add(new InventorySlot(null, 0));
        }
    }

    public bool AddItem(ItemBase item, int count = 1)
    {
        if (item.isStackable)
        {
            foreach (var slot in slots)
            {
                if (slot.item == item && slot.count < item.maxCount)
                {
                    slot.Add(count);
                    return true;
                }
            }
        }

        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                slot.item = item;
                slot.count = count;
                return true;
            }
        }

        Debug.Log("인벤토리가 가득 찼습니다!");
        return false;
    }

    public bool RemoveItem(ItemBase item, int count = 1)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item)
            {
                slot.Remove(count);
                if (slot.IsEmpty)
                {
                    slot.item = null;
                }
                return true;
            }
        }
        return false;
    }
}