using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(UISlot))]
public class SlotController : MonoBehaviour, IDraggable, IDropTarget,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int slotIndex;
    private UISlot ui;
    private Inventory inventory;

    private GraphicRaycaster raycaster;

    private void Awake()
    {
        ui = GetComponent<UISlot>();
        inventory = GetComponentInParent<Inventory>(true); 
    
        raycaster = inventory.GetComponentInChildren<GraphicRaycaster>();
    }

    public int SourceIndex => slotIndex;
    public Image GetIcon() => ui.GetImage();
    public void OnDragStarted() => ui.SetDraggedVisual(true);
    public void OnDragEnded(bool dropped) => ui.SetDraggedVisual(false);
    
    public int TargetIndex => slotIndex;
    public bool CanReceive(int sourceIndex) => inventory.CanMoveTo(sourceIndex, TargetIndex);
    public void ReceiveDrop(int sourceIndex) => inventory.Transfer(sourceIndex, TargetIndex);

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnDragStarted();
    }

    public void OnDrag(PointerEventData eventData)
    {
        ui.GetImage().rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnDragEnded(false);

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);
        
        foreach (var result in results)
        {
            int index = result.gameObject.GetComponentInParent<SlotController>().slotIndex;
            if (CanReceive(index) && index != slotIndex)
            {
                ReceiveDrop(index);
                break;
            }
        }
    }
}
