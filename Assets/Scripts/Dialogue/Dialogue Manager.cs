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
        private DialogueData dialogueData;
        private int currentTextIndex;

        public void Play(DialogueData data)
        {
            dialogueData = data;
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
                textCount = dialogueData.talk[0].text.Count;
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
            
            int index = dialogueData.talk[0].enumValue[currentTextIndex];
            nameBox.text = "???";
            if (!dialogueData.talk[0].talker[index].isHide)
            {
                nameBox.text = dialogueData.talk[0].enumName[index];
            }
            
            TextEvent.instante.Play(textBox, dialogueData.talk[0].text[currentTextIndex], 0.1f);
        }
    }
}

