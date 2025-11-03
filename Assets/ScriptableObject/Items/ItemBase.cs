using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item")]

    public class ItemBase : ScriptableObject
    {
        public readonly string id;
        public Sprite icon;
        public string itemName;
        [TextArea] public string explanation;
        public int count;
        public int maxCount;
        public bool isStackable;
    }
}
