using UnityEngine;

[CreateAssetMenu(fileName = "HealAction", menuName = "Items/Actions/ExamineItem")]
public class ExamineAction : ItemAction
{

    public override void Execute(GameObject user, BaseItem item)
    {
        Debug.Log($"It's a {item.itemName}!");
    }
    public override void Execute(BaseItem item)
    {
        Debug.Log("Executed");
    }

    public void someActualLogic(GameObject EnemyOrAnotherItemEtc) {
        
    }
}
