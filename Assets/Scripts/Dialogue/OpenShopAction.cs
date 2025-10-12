using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Dialogue/Actions/OpenShop")]
public class OpenShopAction : DialogueAction
{

    public string ShopName;
    public override void Execute(DialogueManager manager, DialogueContext context)
    {
        //Action specific code here
        Debug.Log("Open shop action!");
        //call menumanager to instantiate the shop
        manager.menuManager.ShowShop(context); //TODO in the menuManager, DialogueManager, GameController refactor
    }

}
