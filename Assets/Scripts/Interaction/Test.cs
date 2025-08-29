using Dialogue;
using Interaction;
using UnityEngine;

public class Test : MonoBehaviour, IInteractable
{
    public DialogueManager manager;
    public DialogueData data;
    
    public string GetInteractText()
    {
        return "[E]";
    }

    public Vector2 GetTextPosition()
    {
        return transform.position;
    }

    public void Interact()
    {
        manager.Play(data);
    }
}
