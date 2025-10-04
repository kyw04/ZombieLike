using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Item;
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

public class Inventory : MonoBehaviour
{
    private PlayerInput input;
    private InputAction inventoryAction;
    private InputAction closeAction;
    private Image[] slotImage;
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

    private void Start()
    {
        slotImage = inventoryUI.GetComponentsInChildren<Image>(true); // [0] is the inventory background image.
    }

    public void Push(ItemBase item, int count = 1)
    {
        if (checker.TryGetValue(item, out var slotList))
        {
            foreach (var slot in slotList)
            {
                var target = items[slot];
                if (target.count < item.maxCount)
                {
                    target.count += count;
                    if (item.maxCount < target.count)
                    {
                        count = target.count % item.maxCount;
                        target.count = item.maxCount;
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
                Debug.Log("new");
                checker.Add(item, new List<int> {items.Count});
            }

            ItemInSlot newItem = new ItemInSlot(item, count, items.Count);
            items.Add(newItem);
            slotImage[items.Count].sprite = item.sprite;
        }
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