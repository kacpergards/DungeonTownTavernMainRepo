using UnityEngine;
//bool interactable;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] public Dialogue dialogue;

    public void Interact(PlayerController player)
    {
        //Warn: This means there can only ever be one dialogueManager instance - or this will break.
        //Not necessarily a bad thing, but keep in mind when designing more complex dialogues in the future.
        StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue));
    }
}
