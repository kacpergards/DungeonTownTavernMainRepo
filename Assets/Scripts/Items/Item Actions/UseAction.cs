using UnityEngine;

[CreateAssetMenu(fileName = "UseAction", menuName = "Items/Actions/Use")]
public class UseAction : ItemAction
{

    public override void Execute(GameObject user, BaseItem item)
    {
        Debug.Log("user and item required");
    }
    public override void Execute(BaseItem item)
    {
        Debug.Log($"{item.itemName} used.");
    }
}
