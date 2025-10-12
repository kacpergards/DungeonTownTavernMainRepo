using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuySellPanelHandler : MonoBehaviour
{
    [SerializeField] private GameObject BuyButton;
    [SerializeField] private Image ItemImage;
    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private TextMeshProUGUI ItemDesc;
    [SerializeField] private TextMeshProUGUI ItemPrice;
    [SerializeField] private TextMeshProUGUI QuantityAmount;
    [SerializeField] private GameObject quantityGO;

    private Inventory shopInventory;
    private InventorySlot CurrentItemSlot;

    public void SetItem(Inventory ShopInventory, InventorySlot item)
    {
        CurrentItemSlot = item;
        shopInventory = ShopInventory;

        BuyButton.SetActive(true);
        ItemName.gameObject.SetActive(true);
        ItemImage.gameObject.SetActive(true);
        ItemDesc.gameObject.SetActive(true);
        ItemPrice.gameObject.SetActive(true);
        quantityGO.SetActive(true);


        ItemImage.sprite = item.item.icon;
        ItemName.text = item.item.itemName;
        ItemDesc.text = item.item.description;
        ItemPrice.text = (item.item.baseValue * ShopInventory.buySellModifier).ToString();
        QuantityAmount.text = item.quantity.ToString();
    }
    
    public void UnSetItem()
    {
        BuyButton.SetActive(false);
        ItemName.gameObject.SetActive(false);
        ItemImage.gameObject.SetActive(false);
        ItemDesc.gameObject.SetActive(false);
        ItemPrice.gameObject.SetActive(false);
        quantityGO.SetActive(false);
    }

    public void SetQuantity(int newQuantity)
    {
        QuantityAmount.text = newQuantity.ToString();
    }
    
    public void BuyItem() //Triggered by the 'Buy' button in the buysellPanel
    { //TODO in shopManager refactor. BuyItem should be on shopManager, which shouldn't be doing a global Find to get the player inventory.
        GameObject.Find("Player").GetComponent<Inventory>().BuyItem(CurrentItemSlot.item, CurrentItemSlot.item.baseValue, 1, shopInventory);
        if (CurrentItemSlot.quantity <= 0)
        {
            UnSetItem();
        }
        else
        {
            SetQuantity(CurrentItemSlot.quantity);
        } 
        
    }

}
