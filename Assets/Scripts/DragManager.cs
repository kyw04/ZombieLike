using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

interface IDragable
{
    GameObject target { get; }
    
}

[RequireComponent(typeof(GraphicRaycaster))]
public class DragManager : MonoBehaviour
{
    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;
    private PointerEventData eventData;

    private IDragable selected;
    
    private void Awake()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
        eventData = new PointerEventData(eventSystem);
    }

    private void Update()
    {
        if (Mouse.current.leftButton.isPressed && selected != null)
        {
            
        }
    }
}
