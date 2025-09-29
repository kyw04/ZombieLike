using UnityEngine;
using UnityEngine.Serialization;


namespace Item
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/Item")]
    public class ItemBase : ScriptableObject
    {
        public Sprite image;
        [FormerlySerializedAs("name")] public string itemName;
        [TextArea] public string explanation;
    }
}
