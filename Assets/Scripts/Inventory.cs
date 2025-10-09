using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Item;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInSlot
{
    public ItemBase item;
    public int count;
    public int slot;

    public ItemInSlot(ItemBase item, int count, int slot)
    {
        this.item = item;
        this.count = count;
        this.slot = slot;
    }
}

public class Inventory : ImageSelector<Slot>
{
    private PlayerInput input;
    private InputAction inventoryAction;
    private InputAction closeAction;
    
    private Slot[] slotSlots;
    private Dictionary<ItemBase, List<int>> checker; // <ItemBase, Slots>
    private List<ItemInSlot> items;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject player;
    
    private void Awake()
    {
        checker = new Dictionary<ItemBase, List<int>>();
        items = new List<ItemInSlot>();
        input = player.GetComponent<PlayerInput>();
        inventoryAction = input.actions["Inventory"];
        closeAction = input.actions["Close"];
    }

    private new void Start()
    {
        base.Start();
        slotSlots = inventoryUI.GetComponentsInChildren<Slot>(true);
        for (int i = 0; i < slotSlots.Length; i++)
        {
            slotSlots[i].Init(i);
        }
    }
    
    private void Update()
    {
        Select();
        if (onTarget && GetTarget().item != null)
            Move();
    }

    public void Push(ItemBase item, int count = 1)
    {
        if (checker.TryGetValue(item, out var slotList))
        {
            foreach (var slot in slotList)
            {
                ItemInSlot itemInSlot = items[slot];
                if (itemInSlot.count < item.maxCount)
                {
                    itemInSlot.count += count;
                    if (item.maxCount < itemInSlot.count)
                    {
                        count = itemInSlot.count % item.maxCount;
                        itemInSlot.count = item.maxCount;
                    }
                    else
                    {
                        count = 0;
                    }
                }
            }
        }
        
        if (0 < count)
        {
            if (checker.ContainsKey(item))
            {
                checker[item].Add(items.Count);
            }
            else
            {
                checker.Add(item, new List<int> {items.Count});
            }

            slotSlots[items.Count].SetItem(item);
            ItemInSlot newItem = new ItemInSlot(item, count, items.Count);
            items.Add(newItem);
        }
    }

    private void Pop()
    {
        
    }
    
    private void OnEnable()
    {
        inventoryAction.performed += InventoryOnOff;
        closeAction.performed += OnClose;
        
        inventoryAction.Enable();
        closeAction.Enable();
    }

    private void OnDisable()
    {
        inventoryAction.performed -= InventoryOnOff;
        closeAction.performed -= OnClose;
    
        inventoryAction.Disable();
        closeAction.Disable();
    }

    private void InventoryOnOff(InputAction.CallbackContext context)
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    private void OnClose(InputAction.CallbackContext context)
    {
        inventoryUI.SetActive(false);
    }
}