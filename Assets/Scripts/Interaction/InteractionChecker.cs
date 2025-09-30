using Entity;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

namespace Interaction
{
    public class InteractionChecker : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI interactText;
        [SerializeField] private EntityBase user;
        [SerializeField] private float reach = 0.5f;

        private IInteractable currentInteract;
        private PlayerInput input;
        private InputAction interactAction;

        private float currentHoldTime;
        private bool isStarted;
        
        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            interactAction = input.actions["Interact"];
        }

        public void FixedUpdate()
        {
            Check(user.forward);
        }

        private void StartIntercation(EntityBase user)
        {
            float holdTime = currentInteract.GetHoldSeconds();
            if (!isStarted && holdTime < currentHoldTime)
            {
                isStarted = true;
                currentInteract.Interact(user);
            }
            else
                currentHoldTime += Time.deltaTime;
        }
        
        public IInteractable Check(Vector2 look)
        {
            RaycastHit2D hit = Physics2D.Raycast(user.transform.position, look, reach, LayerMask.GetMask("Interactable"));
            if (hit)
            {
                currentInteract = hit.collider.GetComponent<IInteractable>();
                ShowInteractText();

                if (interactAction.IsPressed())
                {
                    StartIntercation(user);
                }
                else
                {
                    isStarted = false;
                    currentHoldTime = 0f;
                }
                
                return currentInteract;
            }

            isStarted = false;
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
