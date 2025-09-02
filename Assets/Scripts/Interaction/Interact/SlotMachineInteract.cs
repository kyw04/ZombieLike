using Interaction;
using Unity.VisualScripting;
using UnityEngine;

public class SlotMachineInteract : MonoBehaviour, IInteractable
{
    private Texture2D[] slots = new Texture2D[3];
    private int[] correctCount;
    private bool isPlayed; 
    public Texture2D[] reels;
    
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
        if (isPlayed)
            return;
        
        isPlayed = true;
        correctCount = new int[slots.Length];
        int max = 0, maxIndex = 0;
        
        for (int i = 0; i < slots.Length; i++)
        {
            int index = Random.Range(0, reels.Length);
            slots[i] = reels[index];
            correctCount[index]++;

            Debug.Log(index);
            
            if (max < correctCount[index])
            {
                max = correctCount[index];
                maxIndex = index;
            }
        }

        Debug.Log($"maxIndex: {maxIndex}, max: {max}");
        Invoke(nameof(End), 1f);
    }

    private void End()
    {
        isPlayed = false;
    }
}
