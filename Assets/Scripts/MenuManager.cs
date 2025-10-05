using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public GameObject inventoryUI;
    public event Action OnShowMenu;
    public event Action OnHideMenu;


    public void SubscribeToEvents(PlayerInputActions inputActions)
    { //Required to avoid 'race' condition
        inputActions.PlayerWorldActions.menuOpen.performed += OnMenuOpenInput; //Further reason to create InputManager...
        inputActions.PlayerDialogueActions.menuOpen.performed += OnMenuOpenInput; //HMMMMM 🤔🤔🤔🤔
        inputActions.PlayerMenuActions.close.performed += OnCloseInput;
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
        inventoryUI.SetActive(true);
        OnShowMenu?.Invoke(); //Player controller subscribes to this, to switch action map.
    }
    void HideMenu()
    {
        inventoryUI.SetActive(false);
        OnHideMenu?.Invoke(); //same as above
    }
}
