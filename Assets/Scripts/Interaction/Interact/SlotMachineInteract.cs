using Interaction;
using UnityEngine;
using Random = UnityEngine.Random;

public class SlotMachineInteract : MonoBehaviour, IInteractable
{
    public SpriteRenderer[] textures;
    private Texture2D[] slots = new Texture2D[3];
    private int[] correctCount;
    public bool isPlayed;
    public int placeCount;
    
    public Texture2D[] reels;
    public float speed;
    
    public string GetInteractText()
    {
        return "[E]";
    }

    public Vector2 GetTextPosition()
    {
        return transform.position + Vector3.up;
    }

    private void Update()
    {
        if (isPlayed)
        {
            for (int i = placeCount; i < textures.Length; i++)
            {
                float centerIndex = textures[i].material.GetFloat("_CenterIndex");
                centerIndex = (centerIndex + Time.deltaTime * speed) % reels.Length;
                textures[i].material.SetFloat("_CenterIndex", centerIndex);
            }
        }
    }

    public void Interact()
    {
        if (placeCount > reels.Length)
            End();

        if (isPlayed && placeCount < textures.Length)
        {
            correctCount = new int[slots.Length];
            int max = 0, maxIndex = 0;
            int index = Random.Range(0, reels.Length);
        
            slots[placeCount] = reels[index];
            correctCount[index]++;
            textures[placeCount].material.SetFloat("_CenterIndex", index);
        
            if (max < correctCount[index])
            {
                max = correctCount[index];
                maxIndex = index;
            }
            Debug.Log($"maxIndex: {maxIndex}, max: {max}");
            placeCount++;
        }
        isPlayed = true;

    }
    
    private void End()
    {
        isPlayed = false;
        placeCount = 0;
    }
}
