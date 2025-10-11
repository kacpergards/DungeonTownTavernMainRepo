using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Actions/CloseDialogue")]
public class CloseDialogueAction : DialogueAction
{
    public override void Execute(DialogueManager manager)
    {
        Debug.Log("Close dialogue!");
        manager.HandleUpdate();
    }
}
