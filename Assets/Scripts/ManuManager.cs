using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManuManager : MonoBehaviour
{
    public GameObject inventoryUI;
    bool inventoryOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        inventoryUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !inventoryOpen)
        {
            inventoryUI.SetActive(true);
            inventoryOpen = true;
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && inventoryOpen)
        {
            inventoryUI.SetActive(false);
            inventoryOpen = false;
        }

    }
}
