using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Dialogue/Actions/OpenShop")]
public class OpenShopAction : DialogueAction
{

    public string ShopName;
    public override void Execute(DialogueManager manager)
    {
        //Action specific code here
        Debug.Log("Open shop action!");
    }

}
