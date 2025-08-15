using System.Collections.Generic;
using Unity.Mathematics;
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
        private Vector2 velocityPos = Vector2.zero;
        private Vector3 velocityRot = Vector3.zero;
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
                velocityPos = Vector2.zero;
                velocityRot = Vector3.zero;
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
            Vector3 targetRotation = Vector3.zero;
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
                    targetRotation = piecePosition.pos.rotation.eulerAngles;
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

            float currentSmoothTime = smoothTime;
            if (maxDistance < board.radius)
                currentSmoothTime = 0.05f;
            
            target.position = Vector2.SmoothDamp(target.position, targetPosition, ref velocityPos, currentSmoothTime);
            target.rotation = Quaternion.Euler(Vector3.SmoothDamp(target.rotation.eulerAngles, targetRotation, ref velocityRot, currentSmoothTime));
        }
    }
}