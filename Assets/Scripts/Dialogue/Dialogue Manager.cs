using System;
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

            int textCount;
            try
            {
                textCount = dialogues[0].talk[0].text.Count;
            }
            catch (Exception e)
            {
                Debug.Log("버그 났어요");
                nameBox.text = "3초 뒤에 종료.";
                textBox.text = e.ToString();
                Invoke(nameof(Close), 3f);
                return;
            }

            if (textCount <= currentTextIndex)
            {
                Debug.Log("끝났습니다.");
                Close();
                return;
            }
            
            int index = dialogues[0].talk[0].enumValue[currentTextIndex];
            nameBox.text = "???";
            if (!dialogues[0].talk[0].talker[index].isHide)
            {
                nameBox.text = dialogues[0].talk[0].enumName[index];
            }
            
            TextEvent.instante.Play(textBox, dialogues[0].talk[0].text[currentTextIndex], 0.1f);
        }
    }
}

