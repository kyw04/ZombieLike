using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ImageSelector<T> : MonoBehaviour
{
    private GraphicRaycaster graphicRaycaster;
    private EventSystem eventSystem;
    private PointerEventData eventData;
    
    private T target;
    private Image image;
    private Transform targetTransform;
    private Transform targetParent;
    protected bool onTarget { get; private set; }
    
    public GameObject targetCanvas;
    public Transform selected;
    
    
    protected void Start()
    {
        onTarget = false;
        graphicRaycaster = targetCanvas.GetComponent<GraphicRaycaster>();
        eventSystem = targetCanvas.GetComponent<EventSystem>();
        eventData = new PointerEventData(eventSystem);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    protected void Select()
    {
        if (targetCanvas.activeSelf && Mouse.current.leftButton.isPressed)
        {
            eventData.position = Mouse.current.position.ReadValue();
            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, results);

            if (!onTarget && 0 < results.Count &&
                (results[0].gameObject.transform.GetComponent<T>() != null ||
                 results[0].gameObject.transform.GetComponentInParent<T>() != null))
            {
                onTarget = true;
                GameObject temp = results[0].gameObject;
                target = temp.GetComponent<T>() ?? temp.GetComponentInParent<T>();
                image = temp.GetComponent<Image>() ?? temp.GetComponentInChildren<Image>();
                targetTransform = image.transform;
                targetParent = targetTransform.parent;
                targetTransform.SetParent(selected);
            }
        }
        else if (onTarget)
        {
            targetTransform.SetParent(targetParent);
            targetTransform.position = targetParent.position;
            onTarget = false;
        }
    }

    protected void Move()
    {
        if (onTarget)
        {
            targetTransform.position = Mouse.current.position.ReadValue();
        }
    }

    public T GetTarget()
    {
        if (onTarget)
            return target;

        return default;
    }
}
