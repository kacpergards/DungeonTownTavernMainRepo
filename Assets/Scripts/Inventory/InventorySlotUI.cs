using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI References")]
    public Image icon;
    public TMP_Text quantityText;
    public GameObject contextMenuAsset;
    public GameObject contextMenuItemAsset;

    private InventorySlot slot;
    private Inventory inventory;
    private GameObject player;
    public Canvas menuCanvas;
    public BaseItem item;
    public InventoryUI parent;

    public void Setup(Inventory inventory, InventorySlot slotToAdd, GameObject player, Canvas menu, InventoryUI InvUI)
    {
        this.inventory = inventory;
        this.slot = slotToAdd;
        this.player = player;
        this.menuCanvas = menu;
        this.parent = InvUI;

        // Set icon
        if (slot.item != null)
        {
            item = slot.item;
            icon.sprite = slot.item.icon;
            icon.enabled = true;
        }
        else
        {
            icon.enabled = false;
        }

        // Set quantity text (hide if only 1)
        quantityText.text = slot.quantity > 0 ? slot.quantity.ToString() : "";
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Left-click
            //inventory.UseItem(slotIndex, player);

            if (parent.isShop)
            {
                Debug.Log("clicked shop slot");
                parent.DisplayItemInBuySellPanel(slot);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (parent != null)
            {
                if (!parent.isShop)
                {                
                    HandleContextMenu();
                }
            }
        }
    }

    private void HandleContextMenu()
    {
        if (parent.currentContextMenu != null)
        {
            Debug.Log("Killing old context menu");
            Destroy(parent.currentContextMenu.gameObject);
            parent.currentContextMenu = null;
        }

        GameObject contextMenu = Instantiate(contextMenuAsset, menuCanvas.gameObject.transform.parent);
        contextMenu.GetComponent<InventoryItemContextMenu>().buildContextMenu(item.actions, this);

        parent.currentContextMenu = contextMenu.GetComponent<Canvas>();
    }
}

