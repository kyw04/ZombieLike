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
        correctCount = new int[textures.Length];

        int width = textures[0].width;
        int height = textures[0].height;

        Texture2DArray texArray = new Texture2DArray(width, height, textures.Length, TextureFormat.RGBA32, false);

        for (int i = 0; i < textures.Length; i++)
        {
            if (textures[i].width != width || textures[i].height != height)
            {
                Debug.LogError("모든 텍스처의 크기가 같아야 합니다.");
                return;
            }

            var w = textures[i].width;
            var h = textures[i].height;
            RenderTexture rt = new RenderTexture(w, h, 0);
            Graphics.Blit(textures[i], rt);

            Texture2D result = new Texture2D(w, h, TextureFormat.RGBA32, false);
            RenderTexture.active = rt;
            result.ReadPixels(new Rect(0, 0, w, h), 0, 0);
            result.Apply();
            RenderTexture.active = null;
            
            Graphics.CopyTexture(result, 0, 0, texArray, i, 0);
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
        if (placeCount >= reels.Length)
            End();

        if (isPlayed && placeCount < reels.Length)
        {
            int max = 0, maxIndex = 0;
            int index = Random.Range(0, textures.Length);
            
            Debug.Log(index);
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
        correctCount = new int[textures.Length];
        isPlayed = false;
        placeCount = 0;
    }
}
