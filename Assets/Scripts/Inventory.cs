using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Item;
using Unity.VisualScripting;

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
    private HashSet<ItemInSlot> itemHash;
    public List<ItemInSlot>  items;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject player;
    
    private void Awake()
    {
        itemHash = new HashSet<ItemInSlot>();
        items = new List<ItemInSlot>();
        input = player.GetComponent<PlayerInput>();
        inventoryAction = input.actions["Inventory"];
        closeAction = input.actions["Close"];
    }

    public void Push(ItemBase item, int count = 1)
    {
        var newItem = new ItemInSlot(item, count, items.Count);
        if (itemHash.Add(newItem))
        {
            Debug.Log(itemHash.Comparer);
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