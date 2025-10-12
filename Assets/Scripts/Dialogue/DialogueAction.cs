using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class DialogueAction : ScriptableObject
{
    public abstract void Execute(DialogueManager manager, DialogueContext context);
}
