using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [Serializable]
    public class Talk
    {
        public List<GameObject> talker;
        public List<string> text;
        public List<int> enumValue;
    }
    
    [CreateAssetMenu(fileName = "Dialogue Data", menuName = "Scriptable Object/Dialogue Data")]
    public class DialogueData : ScriptableObject
    {
        public List<Talk> talk;
    }
}