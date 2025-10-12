using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventorySlot> items = new List<InventorySlot>();
    [SerializeField] private int gold;
    [SerializeField] public float buySellModifier = 1;


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
        // See if this inventory already contains the same item as being added
        InventorySlot slot = items.Find(s => s.item == newItem);
        if (slot != null && slot.quantity + count <= newItem.maxStackSize) //Bug adds a new inventory slot if buying more than maxStack TODO
        { //If we already have the item, and not at the maximum amount trying to be added
            slot.quantity += count;
        }
        else
        {
            items.Add(new InventorySlot { item = newItem, quantity = count });
        }

        onInventoryChangedCallback?.Invoke();
    }

    public void RemoveItemSingle(BaseItem itemToRemove)
    {
        //Find the slot...
        InventorySlot slot = items.Find(s => s.item == itemToRemove);
        if (slot == null)
        {   // If we can't find the slot..
            Debug.LogError("Attempted to remove an item " + itemToRemove.itemName + " but inventory does not contain it!");
            return;
        }
        //Otherwise..
        slot.quantity -= 1;
        if (slot.quantity <= 0)
        { //If we removed the last one, remove the item slot from the inventory
            items.Remove(slot);
        }

        onInventoryChangedCallback?.Invoke();
    }
    public void BuyItem(BaseItem item, int buyPrice, int amount, Inventory sellerInventory) //Seller inventory would be the shop (if player is buying), or player (if selling)
    {
        if (gold < buyPrice)
        {
            Debug.Log("Not enough gold to buy item!");
            return;
        }
        gold -= buyPrice * amount;
        sellerInventory.gold += buyPrice * amount;

        AddItem(item, amount);

        for (int i = 0; i < amount; i++)
        {
            sellerInventory.RemoveItemSingle(item);
        }
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
