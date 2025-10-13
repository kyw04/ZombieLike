using UnityEngine;
using UnityEngine.UI;
using Item;

public class UISlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMPro.TextMeshProUGUI countText;

    public void SetSlot(ItemBase item, int count)
    {
        if (item == null)
        {
            icon.enabled = false;
            countText.text = "";
        }
        else
        {
            icon.enabled = true;
            icon.sprite = item.icon;
            countText.text = count > 1 ? count.ToString() : "";
        }
    }
}
