using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea] public string textLine;
    public List<DialogueOption> dialogueOptions = new List<DialogueOption>();

}