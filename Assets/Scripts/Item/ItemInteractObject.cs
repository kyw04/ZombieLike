using System;
using Interaction;
using JetBrains.Annotations;
using UnityEngine;

namespace Item
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemDisplay : MonoBehaviour, IInteractable
    {
        private SpriteRenderer render;
        [NotNull] public ItemBase itemBase;

        private void Awake()
        {
            render = GetComponent<SpriteRenderer>();
            GetComponent<SpriteRenderer>().sprite = itemBase.icon;
        }

        public string GetInteractText()
        {
            return "[E]";
        }

        public Vector2 GetTextPosition()
        {
            return transform.position + Vector3.up;
        }
        
        public void Interact(Entity.EntityBase user)
        {
            user.inventory.AddItem(itemBase, itemBase.count);
            Destroy(this.gameObject); // change destroy system
        }
    }
}
