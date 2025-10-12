using System;
using TMPro;
using UnityEngine;

public class InventoryItemContextMenuItem : MonoBehaviour
{
    public InventoryItemContextMenu parent;
    public ItemAction action;
    public TMP_Text text;
    public BaseItem item;

    public void Setup(ItemAction action, BaseItem item, InventoryItemContextMenu parent)
    {
        this.action = action;
        this.text.text = action.actionName;
        this.item = item;
        this.parent = parent;
    }

    public void OnClickAction()
    {
        Debug.Log($"Used action {text.text} for item {item.itemName}");
        action.Execute(item);
        parent.DestroySelf();
    }
    
}
