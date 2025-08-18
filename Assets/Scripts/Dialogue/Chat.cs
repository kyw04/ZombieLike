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
    public class Chat : ScriptableObject
    {
        public List<Talk> talk;
    }
}