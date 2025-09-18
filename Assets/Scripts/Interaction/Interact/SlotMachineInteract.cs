using Interaction;
using UnityEngine;
using Random = UnityEngine.Random;

public class SlotMachineInteract : MonoBehaviour, IInteractable
{
    public SpriteRenderer[] reels;
    private Texture2D[] slots = new Texture2D[3];
    private int[] correctCount;
    private int[] placeIndex;
    private float[] currentSpeed;
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
        placeIndex = new int[reels.Length];
        currentSpeed = new float[reels.Length];

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
            currentSpeed[i] = speed;
            reels[i].material.SetTexture("_TexArray", texArray);
            reels[i].material.SetInt("_TexCount", textures.Length);
        }
    }

    private void Update()
    {
        if (isPlayed)
        {
            for (int i = 0; i < reels.Length; i++)
            {
                float centerIndex = reels[i].material.GetFloat("_CenterIndex");
                centerIndex = (centerIndex + Time.deltaTime * currentSpeed[i]) % textures.Length;
                reels[i].material.SetFloat("_CenterIndex", centerIndex);

                if (i < placeCount)
                {
                    if (currentSpeed[i] < 3.0f)
                    {
                        float min = Mathf.Abs(placeIndex[i] - (int)centerIndex);
                        min = Mathf.Min(min, Mathf.Abs(placeIndex[i] - ((int)centerIndex + textures.Length)));
                        currentSpeed[i] = speed * min / textures.Length;

                        if (min == 0)
                        {
                            reels[i].material.SetFloat("_CenterIndex", placeIndex[i]);
                        }
                    }
                    else if (0 < currentSpeed[i])
                    {
                        currentSpeed[i] -= Time.deltaTime * speed * 0.5f;
                    }
                    else if (0 > currentSpeed[i])
                    {
                        currentSpeed[i] = 0;
                    }
                }
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
            
            placeIndex[placeCount] = index;
            slots[placeCount] = textures[index];
            correctCount[index]++;
        
            if (max < correctCount[index])
            {
                max = correctCount[index];
                maxIndex = index;
            }
            // Debug.Log($"maxIndex: {maxIndex}, max: {max}");
            placeCount++;
        }
        isPlayed = true;

    }
    
    private void End()
    {
        for (int i = 0; i < reels.Length; i++)
            currentSpeed[i] = speed;
        
        correctCount = new int[textures.Length];
        isPlayed = false;
        placeCount = 0;
    }
}
