using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    // Item Data
    public string itemName;
    public int itemQuantity;
    public Sprite itemIcon;
    public bool isFull;

    // Item Slot
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;
    [SerializeField] private int MaskStackState = 90;
    // Slot Management
    [SerializeField] public GameObject selectedPanel;
    public bool isItemSelected;
    public InventoryManager inventoryManager;
    public ItemDetails ItemDetails => itemDetails;
    private ItemDetails itemDetails;


    private void Awake(){
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    private void Start()
{
    if (!isFull)
    {
        quantityText.enabled = false;
        itemImage.enabled = false;
    }
}

    public void AddItem(ItemDetails item)
    {
        if (item == null) return;

        itemDetails = item;
        itemName = item.ItemName;
        itemQuantity = Mathf.Min(item.Quantity, MaskStackState);
        itemIcon = item.ItemIcon;
        isFull = true;

        quantityText.text = itemQuantity.ToString();
        quantityText.enabled = true;

        itemImage.sprite = itemIcon;
        itemImage.enabled = itemIcon != null;
    }

    public bool CanStackWith(string otherItemName)
    {
        return isFull
        && itemName == otherItemName
        && itemQuantity < MaskStackState;
}

    public void AddItemQuantity(int quantity)
    {
        itemQuantity = Mathf.Min(
        itemQuantity + quantity,
        MaskStackState
    );

    isFull = itemQuantity > 0;

    quantityText.text = itemQuantity.ToString();
    quantityText.enabled = true;

    itemImage.sprite = itemIcon;
    itemImage.enabled = itemIcon != null;

    }

    
public void OnPointerClick(PointerEventData eventData)
{
    if (eventData.button == PointerEventData.InputButton.Left)
    {
        LeftClick();
    }

    if (eventData.button == PointerEventData.InputButton.Right)
    {
        RightClick();
    }
}
     public bool IsEmpty => itemQuantity <= 0;
    private void LeftClick()
    {
        Debug.Log("Left Clicked on ");
        inventoryManager.DeselectAllSlots();
        selectedPanel.SetActive(true);
        isItemSelected = true;
        // inventoryManager.DesselectAllSlot();
        // selectedPanel.SetActive(true);
        // isItemSelected = true;

        if (IsEmpty || inventoryManager == null)
        {
            return;
        }

        inventoryManager.SelectSlot(this);
    }

private void RightClick()
{
    Debug.Log("Right Clicked on " + itemName);
}

}
