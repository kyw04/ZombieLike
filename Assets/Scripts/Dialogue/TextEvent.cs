using System.Collections;
using UnityEngine;
using TMPro;

public class TextEvent : MonoBehaviour
{
    public static TextEvent instante;
    public bool isPlayed;
    private IEnumerator coroutine;
    
    private void Awake()
    {
        if (instante == null)
            instante = this;
        else
            Debug.LogWarning("too many TextEvent");
    }

    public void Play(TextMeshProUGUI textBox, string text, float speed)
    {
        if (isPlayed)
        {
            Stop(textBox, text);
            return;
        }

        isPlayed = true;
        coroutine = ShowText(textBox, text, speed);
        StartCoroutine(coroutine);
    }
    
    private IEnumerator ShowText(TextMeshProUGUI textBox, string text, float speed)
    {
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == ' ')
                continue;
            
            textBox.text = text.Substring(0, i);
            yield return new WaitForSeconds(speed);
        }

        Stop(textBox, text);
    }

    public void Stop(TextMeshProUGUI textBox, string text)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = null;
        
        isPlayed = false;
        textBox.text = text;
    }

    
}
