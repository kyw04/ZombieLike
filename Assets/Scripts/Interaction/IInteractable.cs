using UnityEngine;

namespace Interaction
{
    public interface IInteractable
    {
        float GetCoolDown() { return 0f; }
        float GetHoldSeconds() { return 0f; }
        string GetInteractText();
        Vector2 GetTextPosition();
        void Interact();
    }
}
