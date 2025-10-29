using System.Collections.Generic;
using UnityEngine;
using Item;

[System.Serializable]
public class InventorySlot
{
    public UISlot ui;
    public ItemBase item;
    public int count;
    
    public bool IsEmpty => item == null || count <= 0;

    public InventorySlot(UISlot ui, ItemBase item, int count)
    {
        this.ui = ui;
        Set(item, count);
    }

    public InventorySlot(InventorySlot other)
    {
        ui = other.ui;
        Set(other.item, other.count);
    }

    public void Set(ItemBase target, int amount)
    {
        item = target;
        count = amount;
        ui.Set(this);
    }

    public void Set(InventorySlot other)
    {
        Set(other.item, other.count);
    }
    
    public void Add(int amount)
    {
        int total = count + amount;
        Set(item, total <= item.maxCount ? total : item.maxCount);
    }

    public void Remove(int amount)
    {
        int total = count - amount;
        Set(item, total < 0 ? 0 : total);
    }
}


public class Inventory : MonoBehaviour
{
    private int capacity;
    public List<InventorySlot> slots;

    private void Start()
    {
        var ui = GetComponentsInChildren<UISlot>(true);
        var slotController = GetComponentsInChildren<SlotController>(true);
        capacity = ui.Length;
        slots = new List<InventorySlot>();
        for (int i = 0; i < capacity; i++)
        {
            slots.Add(new InventorySlot(ui[i], null, 0));
            slotController[i].slotIndex = i;
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
                slot.Set(item, count);
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

    public bool CanMoveTo(int sourceIndex, int targetIndex)
    {
        return slots[targetIndex] != null && !slots[targetIndex].IsEmpty
                && slots[sourceIndex] != null;
    }

    public void Transfer(int sourceIndex, int targetIndex)
    {
        InventorySlot temp = new InventorySlot(slots[sourceIndex]);
        slots[sourceIndex].Set(slots[targetIndex]);
        slots[targetIndex].Set(temp);
    }
}