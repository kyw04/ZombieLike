using System;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

namespace Interaction
{
    public class InteractionChecker : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI interactText;
        [SerializeField] private Entity target;
        [SerializeField] private float reach = 0.5f;

        private IInteractable currentInteract;
        private PlayerInput input;
        private InputAction interactAction;

        private float currentHoldTime;
        private bool isPlay; // interactable 인터페이스가 가지고 있는게 좋을듯?
        
        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            interactAction = input.actions["Interact"];
        }

        public void FixedUpdate()
        {
            Check(target.forward);
        }

        private void StartIntercation()
        {
            float holdTime = currentInteract.GetHoldSeconds();
            if (holdTime < currentHoldTime && !isPlay)
            {
                isPlay = true;
                currentInteract.Interact();
            }
            else
                currentHoldTime += Time.deltaTime;
        }
        
        public IInteractable Check(Vector2 look)
        {
            RaycastHit2D hit = Physics2D.Raycast(target.transform.position, look, reach, LayerMask.GetMask("Interactable"));
            if (hit)
            {
                currentInteract = hit.collider.GetComponent<IInteractable>();
                ShowInteractText();

                if (interactAction.IsPressed())
                {
                    StartIntercation();

                }
                else
                {
                    isPlay = false;
                }
                
                return currentInteract;
            }

            isPlay = false;
            currentHoldTime = 0f;
            interactText.gameObject.SetActive(false);
            return null;
        }

        private void ShowInteractText()
        {
            if (currentInteract == null)
                return;
            
            interactText.transform.position = currentInteract.GetTextPosition();
            interactText.text = currentInteract.GetInteractText();
            interactText.gameObject.SetActive(true);
        }
    }
}
