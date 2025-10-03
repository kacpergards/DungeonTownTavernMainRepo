using UnityEngine;

public abstract class ItemAction : ScriptableObject
{
    public string actionName; // e.g. "Use", "Equip"

    /// <summary>
    /// Called when this action is triggered.
    /// </summary>
    public abstract void Execute(GameObject user, BaseItem item);
    public abstract void Execute(BaseItem item);
}
