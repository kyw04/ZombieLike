using UnityEngine;
using Interaction;
using Dialogue;

public class DialogueInteract : MonoBehaviour, IInteractable
{
    private DialogueManager manager;
    public DialogueData data;

    private void Start()
    {
        manager = GameObject.Find("Canvas").transform.Find("Dialogue").GetComponentInChildren<DialogueManager>();
    }

    public string GetInteractText()
    {
        return "[E]";
    }

    public Vector2 GetTextPosition()
    {
        return transform.position + Vector3.up;
    }

    public void Interact()
    {
        manager.Play(data);
    }
}
