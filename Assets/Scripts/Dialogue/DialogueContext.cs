using UnityEngine;

public class DialogueContext
{
    public GameObject NPC;
    public GameObject Player;
    public Dialogue Dialogue;
    public Inventory NPCInventory => NPC?.GetComponent<Inventory>();
    public Inventory PlayerInventory => Player?.GetComponent<Inventory>();

    public DialogueContext(GameObject NPC, GameObject Player, Dialogue dialogue)
    {
        this.NPC = NPC;
        this.Player = Player;
        this.Dialogue = dialogue;
    }
}
