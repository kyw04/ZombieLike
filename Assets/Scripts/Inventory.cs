using UnityEngine;
using UnityEngine.InputSystem;
using Item;

public class Inventory : MonoBehaviour
{
    public class ItemInSlot
    {
        public ItemBase item;
        public int count;
        public int slot;
    }
    private PlayerInput input;
    private InputAction inventoryAction;
    private InputAction closeAction;
    private GameObject inventoryUI;
    [SerializeField] private GameObject player;
        
    private void Awake()
    {
        inventoryUI = transform.GetChild(0).gameObject;
        input = player.GetComponent<PlayerInput>();
        inventoryAction = input.actions["Inventory"];
        closeAction = input.actions["Close"];
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