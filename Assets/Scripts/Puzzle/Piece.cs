using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Puzzle
{
    [RequireComponent(typeof(GraphicRaycaster))]
    public class Piece : MonoBehaviour
    {
        private GraphicRaycaster raycaster;
        private EventSystem eventSystem;
        private PointerEventData eventData;

        private Transform target;
        private Vector2 velocity = Vector2.zero;

        [SerializeField] private Camera UICamera;
        [SerializeField] private float smoothTime;

        private void Awake()
        {
            raycaster = GetComponent<GraphicRaycaster>();
            eventSystem = GetComponent<EventSystem>();
            eventData = new PointerEventData(eventSystem);
        }

        private void FixedUpdate()
        {
            if (Mouse.current.leftButton.isPressed)
            {
                OnClick();
            }
            else
            {
                velocity = Vector2.zero;
                target = null;
            }
            
            if (target)
                Move();
        }

        public void OnClick()
        {
            if (target)
                return;
            
            eventData.position = Mouse.current.position.ReadValue();
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(eventData, results);

            if (results.Count > 0)
            {
                target = results[0].gameObject.transform;
            }
        }

        private void Move()
        {
            target.position = Vector2.SmoothDamp(target.position, Mouse.current.position.ReadValue(), ref velocity, smoothTime);
        }
    }
}