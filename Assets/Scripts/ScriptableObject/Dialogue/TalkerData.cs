using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "Talker Data", menuName = "Scriptable Object/Talker Data")]
    public class TalkerData : ScriptableObject
    {
        public string talkerName;
        public Texture2D image;
        public bool isHide;
    }

}
