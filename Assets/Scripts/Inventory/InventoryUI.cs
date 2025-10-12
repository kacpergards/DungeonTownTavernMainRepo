using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public bool isShop;

    public Inventory inventory;
    public GameObject slotPrefab;
    public Transform slotParent; // Panel where slots get added
    public GameObject player;    // Reference to player using items
    public Canvas menu;

    public Canvas currentContextMenu;
    public BuySellPanelHandler buySellPanel;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        inventory.onInventoryChangedCallback -= RefreshUI;
    }
    public void Initialize()
    {
        inventory.onInventoryChangedCallback += RefreshUI;
        RefreshUI();
    }

    public void RefreshUI()
    {
        // Clear old slots
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }
        if (currentContextMenu != null)
        {
            Destroy(currentContextMenu.gameObject);
            currentContextMenu = null;

        }

        // Add slots for each item
        foreach(InventorySlot slot in inventory.items)
        {
            var slotGO = Instantiate(slotPrefab, slotParent);
            var slotUI = slotGO.GetComponent<InventorySlotUI>();
            slotUI.Setup(inventory, slot, player, menu, this);
        }
    }

    public void DisplayItemInBuySellPanel(InventorySlot invSlot)
    {
        if (buySellPanel is null)
        {
            Debug.LogError("Tried to access buySellPanel but it's null! Called from outside a shop?");
            return;
        }
        buySellPanel.SetItem(inventory, invSlot);

    }
}
