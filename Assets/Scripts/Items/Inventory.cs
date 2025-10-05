using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventorySlot> items = new List<InventorySlot>();
    public BaseItem potionTemp;

    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;

    void Awake()
    {
        /*BaseItem[] items = Resources.LoadAll<BaseItem>("Item Assets");
        foreach (BaseItem item in items)
        {
            AddItem(item, 1);
            Debug.Log("Added item " + item.itemName);
        }*/
    }

    public void AddItem(BaseItem newItem, int count = 1)
    {
        // Try stacking
        var slot = items.Find(s => s.item == newItem);
        if (slot != null && slot.quantity + count <= newItem.maxStackSize)
        {
            slot.quantity += count;
        }
        else
        {
            items.Add(new InventorySlot { item = newItem, quantity = count });
        }

        onInventoryChangedCallback?.Invoke();
    }

    public void UseItem(int index, GameObject user)
    {
        if (index < 0 || index >= items.Count) return;

       // items[index].item.Use(user);
        items[index].quantity--;

        if (items[index].quantity <= 0)
            items.RemoveAt(index);

        onInventoryChangedCallback?.Invoke();
    }
}
