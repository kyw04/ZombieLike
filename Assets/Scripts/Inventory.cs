using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Item;

public class Inventory : MonoBehaviour
{
    [Serializable]
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
    private PlayerInput input;
    private InputAction inventoryAction;
    private InputAction closeAction;
    private Dictionary<ItemBase, List<int>> invenCheck; // <ItemBase, Slots>
    public List<ItemInSlot>  items;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject player;
    
    private void Awake()
    {
        invenCheck = new Dictionary<ItemBase, List<int>>();
        items = new List<ItemInSlot>();
        input = player.GetComponent<PlayerInput>();
        inventoryAction = input.actions["Inventory"];
        closeAction = input.actions["Close"];
    }

    public void Push(ItemBase item, int count = 1)
    {
        if (invenCheck.TryGetValue(item, out var slotList))
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
            if (invenCheck.ContainsKey(item))
            {
                invenCheck[item].Add(items.Count);
            }
            else
            {
                Debug.Log("new");
                invenCheck.Add(item, new List<int> {items.Count});
            }
            
            ItemInSlot newItem = new ItemInSlot(item, count, items.Count);
            items.Add(newItem);
        }
    }
    
    private void OnEnable()
    {
        inventoryAction.performed += OnInventory;
        closeAction.performed += OnClose;
        
        inventoryAction.Enable();
        closeAction.Enable();
    }

    private void OnDisable()
    {
        inventoryAction.performed -= OnInventory;
        closeAction.performed -= OnClose;
    
        inventoryAction.Disable();
        closeAction.Disable();
    }

    private void OnInventory(InputAction.CallbackContext context)
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    private void OnClose(InputAction.CallbackContext context)
    {
        inventoryUI.SetActive(false);
    }
}