using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class UIToggle : MonoBehaviour
{
    [SerializeField] private GameObject targetUI;
    [SerializeField] private string inputActionName;
    [SerializeField, Tooltip("미완")] private bool pauseGameWhenOpen = true;
    [SerializeField] private float toggleCooldown = 0.1f;

    private PlayerInput input;
    private InputAction action;
    private bool canToggle = true;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        action = input.actions[inputActionName];
    }

    private void OnEnable()
    {
        action.performed += OnPerformed;
        action.Enable();
    }

    private void OnDisable()
    {
        action.performed -= OnPerformed;
        action.Disable();
    }
    
    private void OnPerformed(InputAction.CallbackContext ctx)
    {
        if (!canToggle)
            return;
        
        StartCoroutine(Toggle());
    }
    
    private IEnumerator Toggle()
    {
        canToggle = false;
        SetUIActive(!targetUI.activeSelf);

        yield return new WaitForSeconds(toggleCooldown);
        canToggle = true;
    }

    private void SetUIActive(bool active)
    {
        if (targetUI == null)
            return;

        targetUI.SetActive(active);

        if (pauseGameWhenOpen)
        {
            // 게임 멈추는 시스템 구현
        }

        if (active)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
