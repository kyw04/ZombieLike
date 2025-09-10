using System;
using Interaction;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SlotMachineInteract : MonoBehaviour, IInteractable
{
    public SpriteRenderer[] reels;
    private Texture2D[] slots = new Texture2D[3];
    private int[] correctCount;
    public bool isPlayed;
    public int placeCount;
    
    public Texture2D[] textures;
    public float speed;
    
    public string GetInteractText()
    {
        return "[E]";
    }

    public Vector2 GetTextPosition()
    {
        return transform.position + Vector3.up;
    }

    private void Awake()
    {
        int width = textures[0].width;
        int height = textures[0].height;

        Texture2DArray texArray = new Texture2DArray(width, height, textures.Length, textures[0].format, false);

        for (int i = 0; i < textures.Length; i++)
        {
            if (textures[i].width != width || textures[i].height != height)
            {
                Debug.LogError("모든 텍스처의 크기가 같아야 합니다.");
                return;
            }
            Graphics.CopyTexture(textures[i], 0, 0, texArray, i, 0);
        }
        
        for (int i = placeCount; i < reels.Length; i++)
        {
            reels[i].material.SetTexture("_TexArray", texArray);
            reels[i].material.SetInt("_TexCount", textures.Length);
        }
    }

    private void Update()
    {
        if (isPlayed)
        {
            for (int i = placeCount; i < reels.Length; i++)
            {
                float centerIndex = reels[i].material.GetFloat("_CenterIndex");
                centerIndex = (centerIndex + Time.deltaTime * speed) % textures.Length;
                reels[i].material.SetFloat("_CenterIndex", centerIndex);
            }
        }
    }

    public void Interact()
    {
        if (placeCount > textures.Length)
            End();

        if (isPlayed && placeCount < reels.Length)
        {
            correctCount = new int[slots.Length];
            int max = 0, maxIndex = 0;
            int index = Random.Range(0, textures.Length);
        
            slots[placeCount] = textures[index];
            correctCount[index]++;
            reels[placeCount].material.SetFloat("_CenterIndex", index);
        
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
