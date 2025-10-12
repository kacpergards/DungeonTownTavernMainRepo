using System.Linq;
using UnityEngine;

public class InventoryItemContextMenu : MonoBehaviour
{
    public InventorySlotUI parent;
    public GameObject contextMenuItemContainer;

    public InventoryItemContextMenuItem[] menuItems;
    void Awake()
    {
        Canvas popupCanvas = GetComponent<Canvas>();
        popupCanvas.overrideSorting = true; // allows custom sorting order
        popupCanvas.sortingOrder = 100;

    }

    public void buildContextMenu(ItemAction[] itemActions, InventorySlotUI parent)
    {
        this.parent = parent;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            GetComponent<RectTransform>(),
            Input.mousePosition,
            null,
            out localPoint
        );
        RectTransform rt = contextMenuItemContainer.GetComponent<RectTransform>();

        rt.anchoredPosition = localPoint;

        foreach (ItemAction action in itemActions)
        {
            var menuItem = Instantiate(parent.contextMenuItemAsset, contextMenuItemContainer.transform);
            InventoryItemContextMenuItem item = menuItem.GetComponent<InventoryItemContextMenuItem>();
            item.Setup(action, parent.item, this);
            menuItems.Append(item);
        }

    }
    public void DestroySelf()
    {
        parent.parent.currentContextMenu = null;
        Destroy(gameObject);
    }
}