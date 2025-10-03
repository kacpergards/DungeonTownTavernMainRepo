using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI References")]
    public Image icon;              // Assigned in prefab
    public TMP_Text quantityText;   // Assigned in prefab
    public GameObject contextMenuAsset;
    public GameObject contextMenuItemAsset;

    private int slotIndex;
    private Inventory inventory;
    private GameObject player;
    public Canvas menuCanvas;
    public BaseItem item;
    public InventoryUI parent;

    /// <summary>
    /// Sets up this slot with the correct item data.
    /// </summary>
    public void Setup(Inventory inventory, int index, GameObject player, Canvas menu, InventoryUI InvUI)
    {
        this.inventory = inventory;
        this.slotIndex = index;
        this.player = player;
        this.menuCanvas = menu;
        this.parent = InvUI;

        var slot = inventory.items[index];

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
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (parent.currentContextMenu != null)
            {
                Debug.Log("Killing old context menu");
                Destroy(parent.currentContextMenu.gameObject);
                parent.currentContextMenu = null;
            }
            // Right-click
            Debug.Log($"Right-clicked slot {slotIndex}");

            GameObject contextMenu = Instantiate(contextMenuAsset, menuCanvas.gameObject.transform.parent);
            contextMenu.GetComponent<InventoryItemContextMenu>().buildContextMenu(item.actions, this);
            
            parent.currentContextMenu = contextMenu.GetComponent<Canvas>();
        }
    }
}

