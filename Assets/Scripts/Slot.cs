using System;
using Item;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private Image image;
    public int index;
    public ItemBase item;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void SetItem(ItemBase other)
    {
        image.sprite = other.sprite;
        item = other;
    }

    public void ChangeSlot(Slot other)
    {
        Slot temp = other;

        other.SetItem(item);
        other.index = index;
        SetItem(temp.item);
        index = temp.index;
    }
}
