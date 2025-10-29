using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Item;

public class UISlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Transform dragField;

    public Image GetImage() => icon;

    public void Set(InventorySlot slot)
    {
        if (slot == null || slot.IsEmpty)
        {
            ClearVisual();
            return;
        }

        icon.color = Color.white;
        icon.sprite = slot.item.icon;
        countText.text = slot.count > 1 ? slot.count.ToString() : "";
    }

    private void ClearVisual()
    {
        icon.color = Color.clear;
        countText.text = "";
    }

    public void SetDraggedVisual(bool isDragged)
    {
        if (isDragged)
        {
            icon.transform.SetParent(dragField);
            icon.rectTransform.localScale *= 1.25f;
        }
        else
        {
            icon.transform.SetParent(transform);
            icon.rectTransform.localScale = transform.localScale;
            icon.rectTransform.localPosition = Vector2.zero;
        }
    }
}
