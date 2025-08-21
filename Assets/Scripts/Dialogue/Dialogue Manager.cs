using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public GameObject dialogue;
        public TextMeshProUGUI nameBox;
        public TextMeshProUGUI textBox;
        public List<DialogueData> dialogues;
        private int currentTextIndex;

        public void Play()
        {
            currentTextIndex = -1;
            dialogue.SetActive(true);
            Next();
        }

        private void Close()
        {
            dialogue.SetActive(false);
        }
        
        public void Next()
        {
            if (!TextEvent.instante.isPlayed)
                currentTextIndex++;

            if (dialogues[0].talk[0].text.Count <= currentTextIndex)
            {
                Debug.Log("끝났습니다.");
                Close();
                return;
            }
            
            int index = dialogues[0].talk[0].enumValue[currentTextIndex];
            nameBox.text = dialogues[0].talk[0].enumName[index];
            TextEvent.instante.Play(textBox, dialogues[0].talk[0].text[currentTextIndex], 0.1f);
        }
        
    }
}

