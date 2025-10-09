using UnityEngine;

namespace Entity
{
    public class EntityBase : MonoBehaviour
    {
        [HideInInspector] public Vector2 forward;
        public Inventory inventory;
    }
}