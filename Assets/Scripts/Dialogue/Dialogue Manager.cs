using System;
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
        private int currentTalkIndex;
        public bool isPlayed { get; private set; }

        public void Play(DialogueData data)
        {
            isPlayed = true;
            dialogueData = data;
            currentTextIndex = -1;
            currentTalkIndex = 0;
            dialogue.SetActive(true);
            Next();
        }

        private void Close()
        {
            isPlayed = false;
            dialogue.SetActive(false);
        }
        
        public void Next()
        {
            if (!TextEvent.instante.isPlayed)
                currentTextIndex++;

            int talkCount = dialogueData.talk.Count;
            if (talkCount <= currentTalkIndex)
            {
                Debug.Log("끝났습니다.");
                Close();
                return;
            }

            Talk currentTalk;
            int textCount;
            try
            {
                currentTalk = dialogueData.talk[currentTalkIndex];
                textCount = currentTalk.text.Count;
            }
            catch (Exception e)
            {
                Debug.Log("버그 났어요");
                Debug.Log(e.ToString());
                nameBox.text = "3초 뒤에 종료.";
                textBox.text = e.ToString();
                Invoke(nameof(Close), 3f);
                return;
            }

            if (textCount <= currentTextIndex)
            {
                currentTalkIndex++;
                currentTextIndex = -1;
                return;
            }

            int index = currentTalk.enumValue[currentTextIndex];
            nameBox.text = "???";
            if (!currentTalk.talker[index].isHide)
            {
                nameBox.text = currentTalk.enumName[index];
            }
            
            TextEvent.instante.Play(textBox, currentTalk.text[currentTextIndex], 0.1f);
        }
    }
}

