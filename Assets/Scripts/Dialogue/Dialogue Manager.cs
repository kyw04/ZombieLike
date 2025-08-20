using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public TextMeshProUGUI nameBox;
        public TextMeshProUGUI textBox;
        public List<DialogueData> dialogues;
        private int currentTextIndex;

        public void Start()
        {
            currentTextIndex = -1;
        }

        private void Close()
        {
            
        }
        
        public void Next()
        {
            if (!TextEvent.instante.isPlayed)
                currentTextIndex++;

            if (dialogues[0].talk[0].text.Count <= currentTextIndex)
            {
                Debug.Log("끝났습니다.");
                return;
            }
            
            int index = dialogues[0].talk[0].enumValue[currentTextIndex];
            nameBox.text = dialogues[0].talk[0].enumName[index];
            TextEvent.instante.Play(textBox, dialogues[0].talk[0].text[currentTextIndex], 0.1f);
        }
        
    }
}

