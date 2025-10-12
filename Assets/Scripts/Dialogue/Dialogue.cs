using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/Dialogue")]
public class Dialogue : ScriptableObject {
    public List<DialogueLine> lines = new List<DialogueLine>();

    public List<DialogueLine> Lines
    {
        get { return lines; }
    }

}
