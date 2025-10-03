using UnityEngine;

[CreateAssetMenu(fileName = "Normal Armour", menuName = "Items/Gear/Normal Armour")]
public class NormalArmour : BaseItem
{
    public enum SLOT
    {
        HEAD,
        CHEST,
        LEGS,
        GLOVES,
        BACK,
        FEET
    }
    public int defence = 10;
    public SLOT armor_slot = SLOT.CHEST;


}
