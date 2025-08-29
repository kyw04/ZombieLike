using Interaction;
using UnityEngine;

public class Test : MonoBehaviour, IInteractable
{
    public string GetInteractText()
    {
        return "12312";
    }

    public Vector2 GetTextPosition()
    {
        return transform.position;
    }

    public void Interact()
    {
        Debug.Log("aSD");
    }
}
