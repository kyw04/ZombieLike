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
        private PiecePosition closest;

        [SerializeField] private Board board;
        [SerializeField] private Transform poolPosition;
        [SerializeField] private Transform selectedPosition;

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
            else if (target)
            {
                target.SetParent(poolPosition);
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
                target.SetParent(selectedPosition);
            }
        }

        private void Move()
        {
            Vector2 targetPosition = Mouse.current.position.ReadValue();
            float maxDistance = board.radius;
            
            foreach (PiecePosition piecePosition in board.piecePosition)
            {
                float currentDistance = Vector2.Distance(piecePosition.pos.position, Mouse.current.position.ReadValue());
                if ((!piecePosition.isPlaced || piecePosition.placedObj == target.gameObject) && currentDistance <= maxDistance)
                {
                    if (closest is { isPlaced: true } && closest != piecePosition && Vector2.Distance(target.position, closest.pos.position) <= 5f)
                    {
                        closest.placedObj = null;
                        closest.isPlaced = false;
                    }
                    closest = piecePosition;
                    maxDistance = currentDistance;
                    targetPosition = piecePosition.pos.position;
                }
            }
            
            if (closest != null)
            {
                if (!closest.isPlaced && Vector2.Distance(target.position, closest.pos.position) <= 5f)
                {
                    closest.placedObj = target.gameObject;
                    closest.isPlaced = true;
                }
                
                if (target.gameObject == closest.placedObj && Vector2.Distance(target.position, Mouse.current.position.ReadValue()) >= board.radius)
                {
                    closest.placedObj = null;
                    closest.isPlaced = false;
                }
            }
            
            target.position = Vector2.SmoothDamp(target.position, targetPosition, ref velocity, smoothTime);
        }
    }
}