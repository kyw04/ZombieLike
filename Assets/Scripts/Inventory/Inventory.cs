using System.Collections.Generic;
using UnityEngine;
using Item;

[System.Serializable]
public class InventorySlot
{
    public UISlot ui;
    public ItemBase item;
    public int count;

    public InventorySlot(UISlot ui, ItemBase item, int count)
    {
        this.ui = ui;
        this.item = item;
        this.count = count;
    }

    public void Set(ItemBase target, int amount = 0)
    {
        item = target;
        count = amount;
        ui.SetSlot(item, count);
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

    public bool IsEmpty => item == null || count <= 0;
}


public class Inventory : MonoBehaviour
{
    [SerializeField] private int capacity;
    public List<InventorySlot> slots;

    private void Awake()
    {
        var ui = GetComponentsInChildren<UISlot>(true);
        capacity = ui.Length;
        slots = new List<InventorySlot>();
        for (int i = 0; i < capacity; i++)
        {
            ui[i].SetSlot(null, 0);
            slots.Add(new InventorySlot(ui[i], null, 0));
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
}