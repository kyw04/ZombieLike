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

        public void Play()
        {
            currentTextIndex = 0;
        }

        private void Close()
        {
            
        }
        
        public void Next()
        {
            nameBox.text = dialogues[0].talk[0].enumValue[currentTextIndex].ToString();
            textBox.text = dialogues[0].talk[0].text[currentTextIndex++];
        }
        
    }
}

