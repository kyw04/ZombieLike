using System;
using Interaction;
using UnityEngine;
using UnityEngine.Serialization;

namespace Item
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemBase : MonoBehaviour, IInteractable
    {
        private SpriteRenderer render;

        public readonly string id;
        public Sprite sprite;
        [FormerlySerializedAs("name")] public string itemName;
        [TextArea] public string explanation;
        public int count;
        public int maxCount;

        private void Awake()
        {
            render = GetComponent<SpriteRenderer>();
            GetComponent<SpriteRenderer>().sprite = sprite;
        }

        public string GetInteractText()
        {
            return "[E]";
        }

        public Vector2 GetTextPosition()
        {
            return transform.position + Vector3.up;
        }

        // public override int GetHashCode()
        // {
        //     return id.GetHashCode() ^ name.GetHashCode();
        // }

        public void Interact(Entity.EntityBase user)
        {
            user.GetComponent<Inventory>().Push(this, count);
        }
    }
}
