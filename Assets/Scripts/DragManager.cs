using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IDraggable
{
    int SourceIndex { get; }
    Image GetIcon();
    void OnDragStarted();
    void OnDragEnded(bool dropped);
}

public interface IDropTarget
{
    int TargetIndex { get; }
    bool CanReceive(int sourceIndex);
    void ReceiveDrop(int sourceIndex);
}

[RequireComponent(typeof(GraphicRaycaster))]
public class DragManager : MonoBehaviour
{
    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;
    private PointerEventData eventData;

    private void Awake()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
        eventData = new PointerEventData(eventSystem);
    }

    
}
