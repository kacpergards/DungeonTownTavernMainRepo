using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Generic Item")]
public class BaseItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public string description;
    public int maxStackSize = 1;

    public ItemAction[] actions;
}
