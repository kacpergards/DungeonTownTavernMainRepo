using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public GameObject slotPrefab;
    public Transform slotParent; // Panel where slots get added
    public GameObject player;    // Reference to player using items
    public Canvas menu;

    public Canvas currentContextMenu;

    private void OnEnable()
    {
        inventory.onInventoryChangedCallback += RefreshUI;
        RefreshUI();

        //var grid = GetComponent<GridLayoutGroup>();
        //grid.startAxis = GridLayoutGroup.Axis.Horizontal;
        //grid.childAlignment = TextAnchor.UpperLeft;
        //grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        //grid.constraintCount = 5;
    }

    private void OnDisable()
    {
        inventory.onInventoryChangedCallback -= RefreshUI;
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
        for (int i = 0; i < inventory.items.Count; i++)
        {
            var slotGO = Instantiate(slotPrefab, slotParent);
            var slotUI = slotGO.GetComponent<InventorySlotUI>();
            slotUI.Setup(inventory, i, player, menu, this);
        }
    }
}
