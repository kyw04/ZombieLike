using UnityEngine;

[ExecuteInEditMode]
public class TestShader : MonoBehaviour
{
    public Material targetMaterial;
    public Texture2D[] sourceTextures;
    [Range(0, 25)] public int layerIndex; // 인스펙터에서 선택

    void Update()
    {
        if (targetMaterial == null || sourceTextures == null || sourceTextures.Length == 0)
            return;

        int width = sourceTextures[0].width;
        int height = sourceTextures[0].height;

        Texture2DArray texArray = new Texture2DArray(width, height, sourceTextures.Length, sourceTextures[0].format, false);

        for (int i = 0; i < sourceTextures.Length; i++)
        {
            if (sourceTextures[i].width != width || sourceTextures[i].height != height)
            {
                Debug.LogError("모든 텍스처의 크기가 같아야 합니다.");
                return;
            }
            Graphics.CopyTexture(sourceTextures[i], 0, 0, texArray, i, 0);
        }

        targetMaterial.SetTexture("_TexArray", texArray);
        targetMaterial.SetInt("_LayerIndex", Mathf.Clamp(layerIndex, 0, sourceTextures.Length - 1));
    }
}
