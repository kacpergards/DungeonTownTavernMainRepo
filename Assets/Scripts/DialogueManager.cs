using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    [SerializeField] Text dialogueText;
    [SerializeField] int lettersPerSecond;
    [SerializeField] int currentLine;
    [SerializeField] GameObject dialogueOptionAsset;

    Dialogue dialogue;
    private bool inOptions = false;
    private List<GameObject> availableOptions;
    private List<DialogueAction> availableOptionsActions;
    private int selectedOptionIndex;
    private Coroutine typeCoroutine;

    public event Action OnShowDialogue;
    public event Action OnHideDialogue;

    public static DialogueManager Instance { get; private set; }

    public void SubscribeToEvents(PlayerInputActions inputActions)
    { //Required to avoid 'race' condition
        inputActions.PlayerDialogueActions.proceed.performed += OnProceedInput;
        inputActions.PlayerDialogueActions.dialogueOptionUp.performed += OnDialogueOptionUpInput;
        inputActions.PlayerDialogueActions.dialogueOptionDown.performed += OnDialogueOptionDownInput;
    }

    private void Awake()
    {
        Instance = this;
        currentLine = 0;
    }

    private void OnProceedInput(InputAction.CallbackContext ctx)
    {
        if (!inOptions)
        {
            HandleUpdate();
        }
        else
        {
            HandleOptionSelectProceed();
        }
    }

    private void OnDialogueOptionUpInput(InputAction.CallbackContext ctx)
    {
        if (!inOptions) return;
        if (selectedOptionIndex == 0)
        {//If the current selected option is the last one
            selectedOptionIndex = availableOptions.Count - 1;
        }
        else
        {
            selectedOptionIndex--;
        }
        SelectOption();
    }
    private void OnDialogueOptionDownInput(InputAction.CallbackContext ctx)
    {
        if (!inOptions) return;
        if (selectedOptionIndex == availableOptions.Count - 1)
        {//If the current selected option is the last one, loop back to 0
            selectedOptionIndex = 0;
        }
        else
        {
            selectedOptionIndex++;
        }
        SelectOption();
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

        ResetDialogueOptions();
        currentLine++;
        if (currentLine < dialogue.Lines.Count)
        {
            StopTypeCoroutineIfExists();
            typeCoroutine = StartCoroutine(TypeDialogue(dialogue.Lines[currentLine]));
        }
        else
        {
            CloseDialogue();
        }
    }
    public void ResetDialogueOptions()
    {
        //reset options
        availableOptions = null;
        availableOptionsActions = null;
        selectedOptionIndex = 0;
        inOptions = false;

        //Destroy all option gameobjects
        Transform dialogueOptionsParentTransform = dialogueBox.transform.Find("DialogueOptions").transform;
        foreach (Transform child in dialogueOptionsParentTransform)
        {
            Destroy(child.gameObject);
        }
    }
    public void CloseDialogue()
    {
        dialogueBox.SetActive(false);
        currentLine = 0;
        StopTypeCoroutineIfExists();
        dialogueText.text = "";
        OnHideDialogue?.Invoke();
    }
    public void HandleOptionSelectProceed()
    {
        //Call execute on currently selected option
        availableOptionsActions[selectedOptionIndex].Execute(this);
    }
    public void SelectOption()
    {
        GameObject option = availableOptions[selectedOptionIndex]; //wowee two globals very good code very good
        //hack - loop through options and 'reset' color
        foreach (GameObject opt in availableOptions)
        {
            opt.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        //Highlight the selected gameobject
        option.GetComponent<TextMeshProUGUI>().color = Color.red;
    }

    public IEnumerator TypeDialogue(DialogueLine line)
    {
        dialogueText.text = "";
        foreach (var letter in line.textLine.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        ShowOptionsForDialogueLine(line);
        typeCoroutine = null;
    }

    public void ShowOptionsForDialogueLine(DialogueLine line)
    {

        //Get the dialogueOptions container on the dialogue box
        Transform dialogueOptionsParentTransform = dialogueBox.transform.Find("DialogueOptions").transform;
        //If no options, don't show them
        if (line.dialogueOptions.Count == 0)
        {
            return;
        }
        
        //Create a list of the currently available options
        availableOptions = new List<GameObject>();
        availableOptionsActions = new List<DialogueAction>();
        //Display the options, adding them to the available list
        foreach (DialogueOption option in line.dialogueOptions)
        {
            //Create the option
            GameObject instantiatedDialogueOptionGO = Instantiate(dialogueOptionAsset, dialogueOptionsParentTransform);
            //Set the optionText
            instantiatedDialogueOptionGO.GetComponent<TextMeshProUGUI>().text = option.optionText;

            availableOptions.Add(instantiatedDialogueOptionGO);
            availableOptionsActions.Add(option.action);
        }

        inOptions = true;
        //Automatically Select first option
        selectedOptionIndex = 0;
        SelectOption();
    }
}
