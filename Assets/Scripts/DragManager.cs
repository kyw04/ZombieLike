using UnityEngine;
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