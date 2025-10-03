using UnityEngine;
//bool interactable;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] public Dialogue dialogue;
    
    public void OnDetected()
    {
        Debug.Log("NPC detected");
    }
    
    public void Interact()
    {
        
        //StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue));
    }
}
