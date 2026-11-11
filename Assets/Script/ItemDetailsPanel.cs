using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailsPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemQtyText;

    public void Show(ItemDetails item, int quantity){
        if(item == null){
            Clear();
            return;
        }

        itemImage.sprite = item.ItemIcon;
        itemImage.enabled = true;
        itemNameText.text = item.ItemName;
        itemQtyText.text = "Quantity: " + quantity.ToString();
        itemDescriptionText.text = item.Description;
    }

    public void Clear(){
        itemImage.sprite = null;
        itemImage.enabled = false;
        itemNameText.text = "";
        itemQtyText.text = "";
        itemDescriptionText.text = "";
    }
}
