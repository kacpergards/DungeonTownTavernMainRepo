using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    [SerializeField] Text dialogueText;
    [SerializeField] int lettersPerSecond;
    [SerializeField] int currentLine;

    Dialogue dialogue;
    private Coroutine typeCoroutine;

    public event Action OnShowDialogue;
    public event Action OnHideDialogue;

    public static DialogueManager Instance { get; private set; }

    public void SubscribeToEvents(PlayerInputActions inputActions)
    { //Required to avoid 'race' condition
        inputActions.PlayerDialogueActions.proceed.performed += OnProceedInput;
    }

    private void Awake()
    {
        Instance = this;
        currentLine = 0;
    }

    private void OnProceedInput(InputAction.CallbackContext ctx)
    {
        HandleUpdate();
    }

    private void StopTypeCoroutineIfExists() { if (typeCoroutine != null) { StopCoroutine(typeCoroutine); } typeCoroutine = null; }


    // Invoked by dialogue initiator - NPC - Interact() function
    // Therefore, as we switch action sets immediately, this cannot be called while a dialogue is in progress
    public IEnumerator ShowDialogue(Dialogue dialogue)
    {
        yield return new WaitForEndOfFrame();

        OnShowDialogue?.Invoke();

        this.dialogue = dialogue; // Move to OnShowDialogue subscription?
        dialogueBox.SetActive(true); //^

        StopTypeCoroutineIfExists();
        typeCoroutine = StartCoroutine(TypeDialogue(dialogue.Lines[0]));
    }



    public void HandleUpdate()
    {
        currentLine++;
        if (currentLine < dialogue.Lines.Count)
        {
            StopTypeCoroutineIfExists();
            typeCoroutine = StartCoroutine(TypeDialogue(dialogue.Lines[currentLine]));
        }
        else
        {
            dialogueBox.SetActive(false);
            currentLine = 0;
            StopTypeCoroutineIfExists();
            dialogueText.text = "";
            OnHideDialogue?.Invoke();
        }
    }

    public IEnumerator TypeDialogue(string line)
    {
        dialogueText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        typeCoroutine = null;
    }
}
