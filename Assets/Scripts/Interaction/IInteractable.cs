using UnityEngine;

namespace Interaction
{
    public interface IInteractable
    {
        float GetHoldSeconds() { return 0f; }
        string GetInteractText();
        Vector2 GetTextPosition();
        void Interact();
    }
}
