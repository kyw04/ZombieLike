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
            icon.sprite = default;
            countText.text = "";
        }
        else
        {
            icon.sprite = item.icon;
            Debug.Log("set icon");
            countText.text = count > 1 ? count.ToString() : "";
        }
    }
}
