using UnityEngine;

[CreateAssetMenu(fileName = "Normal Accessory", menuName = "Items/Gear/Normal Accessory")]
public class NormalAccessory : BaseItem
{
    public enum ACCESSORY_TYPE
    {
        NECKLACE,
        RING,
        EARRING,
        BUTTPLUG
    }
    public int swag = 10;
    public ACCESSORY_TYPE type = ACCESSORY_TYPE.NECKLACE;



}
