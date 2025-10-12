using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{

    public GameObject shopPrefab;

    public GameObject inventoryUI;
    public GameObject activeShop;
    public event Action OnShowMenu;
    public event Action OnHideMenu;

    private bool inInventory = false;
    private bool inShop = false;


    public void SubscribeToEvents(PlayerInputActions inputActions)
    { //Required to avoid 'race' condition
        inputActions.PlayerWorldActions.menuOpen.performed += OnMenuOpenInput; //Further reason to create InputManager...
        inputActions.PlayerDialogueActions.menuOpen.performed += OnMenuOpenInput; //HMMMMM 🤔🤔🤔🤔
        inputActions.PlayerMenuActions.close.performed += OnCloseInput;
        inputActions.PlayerMenuActions.close.performed += HideShop;
    }

    void Start()
    {
        inventoryUI.SetActive(false);
    }


    void OnMenuOpenInput(InputAction.CallbackContext ctx)
    { // This is purposeful. Lets keep this pattern. Input events trigger an On<eventName>Input() which then calls whatever
        ShowMenu();
    }
    void OnCloseInput(InputAction.CallbackContext ctx)
    { 
        HideMenu();
    }

    void ShowMenu()
    {
        inventoryUI.transform.Find("Inventory").GetComponent<InventoryUI>().Initialize();
        inventoryUI.SetActive(true);
        inInventory = true;
        OnShowMenu?.Invoke(); //Player controller subscribes to this, to switch action map.
    }
    void HideMenu()
    {
        if (inInventory)
        {            
            inventoryUI.SetActive(false);
            inInventory = false;
            OnHideMenu?.Invoke(); //same as above
        }
    }

    public void ShowShop(DialogueContext context)
    {
        inShop = true;
        OnShowMenu?.Invoke();
        //Instantiate Shop prefab
        activeShop = Instantiate(shopPrefab, transform);
        InventoryUI inv = activeShop.transform.Find("Inventory").GetComponent<InventoryUI>();
        inv.inventory = context.NPCInventory;
        inv.Initialize();
        
    }
    
    void HideShop(InputAction.CallbackContext ctx)
    {
        if (inShop)
        {
            inShop = false;
            OnHideMenu?.Invoke();
            //Destroy shop prefab
            Destroy(activeShop);
        }   
    }
}
