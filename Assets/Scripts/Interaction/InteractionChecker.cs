using UnityEngine;
using TMPro;

namespace Interaction
{
    public class InteractionChecker : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI interactText;
        [SerializeField] private Entity target;
        [SerializeField] private float reach = 0.5f;

        private IInteractable currentInteract;

        public void FixedUpdate()
        {
            Check(target.forward);
        }
        
        public IInteractable Check(Vector2 look)
        {

            RaycastHit2D hit = Physics2D.Raycast(target.transform.position, look, reach, LayerMask.GetMask("Interactable"));
            if (hit)
            {
                currentInteract = hit.collider.GetComponent<IInteractable>();
                ShowInteractText();
                return currentInteract;
            }
            
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
