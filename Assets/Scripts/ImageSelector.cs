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
    private Image image;
    private Transform targetTransform;
    private Transform targetParent;

    protected List<T> behindTargets { get; private set; }
    protected T target { get; private set;  }
    protected bool onTarget { get; private set; }
    
    public GameObject targetCanvas;
    public Transform selected;
    
    
    protected void Start()
    {
        onTarget = false;
        behindTargets = new List<T>();
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
            behindTargets.Clear();
            
            foreach (var result in results)
            {
                if (result.gameObject.transform.GetComponent<T>() != null ||
                    result.gameObject.transform.GetComponentInParent<T>() != null)
                {
                    GameObject temp = result.gameObject;
                    T targetCompo = temp.GetComponent<T>() ?? temp.GetComponentInParent<T>();
                    behindTargets.Add(targetCompo);
                    if (!onTarget)
                    {
                        target = targetCompo;
                        onTarget = true;
                        image = temp.GetComponent<Image>() ?? temp.GetComponentInChildren<Image>();
                        targetTransform = image.transform;
                        targetParent = targetTransform.parent;
                        targetTransform.SetParent(selected);
                    }
                }
            }
        }
        else if (onTarget)
        {
            behindTargets.Clear();
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
}
