using UnityEngine;

[CreateAssetMenu(fileName = "HealAction", menuName = "Items/Actions/HealUser")]
public class HealAction : ItemAction
{
    public int healAmount;

    public override void Execute(GameObject user, BaseItem item)
    {
        Debug.Log("user and item");
    }
    public override void Execute(BaseItem item)
    {
        Debug.Log($"{item.itemName} used. Healed {healAmount} HP.");
    }
}
