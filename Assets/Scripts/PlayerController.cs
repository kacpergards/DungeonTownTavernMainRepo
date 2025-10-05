using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    
    private Vector2 moveInput;
    private CapsuleCollider playerCollider;



    public float speed;
    private LayerMask interactableLayerMask;
    private LayerMask groundMask;
    public SpriteRenderer spriteRenderer;
    public Animation anim;
    public Animator animator;
    public Terrain terrain;

    

    public float checkDistance = 1f;
    private Vector3 previousPosition;
    public Vector3 direction;
    public Vector3 directionBobber;
    public Vector3 directionRaycast;
    public float castDist = 20;
    Rigidbody body;
    bool onGround = false;
    public GameObject BobberGO;


    private PlayerInputActions playerInputActions;
    private InputActionMap previousMap; // Holds previous action map to handle e.g. switching back to dialogue map after closing menu
    public DialogueManager dialogueManager;
    public MenuManager menuManager;

    void Awake()
    {
        //This is a really nice pattern and much better than what was here before.
        //However! It is arguable that this additionally should be abstracted to a top-level InputManager
        //Player, MenuManager, and DialogueManager would then subscribe to events from the InputManager
        //As it is, PlayerController handles physics, animations, player state - as well as Player World Input, and Switching of the action set to Menu, Dialogue, World
        //MenuManager and DialogueManager do handle their own inputs via their SubscribeToEvents functions.
        //This is fine for now, but keep this in mind.

        // Actions must be registered in awake
        playerInputActions = new PlayerInputActions();
        menuManager.SubscribeToEvents(playerInputActions);
        dialogueManager.SubscribeToEvents(playerInputActions);

        // Set worldactions as default action set
        playerInputActions.PlayerWorldActions.Enable();

        // Player movement - opting to subscribe to events directly for clarity
        playerInputActions.PlayerWorldActions.move.performed += OnMoveInput;
        playerInputActions.PlayerWorldActions.move.canceled += OnMoveCanceled;
        playerInputActions.PlayerWorldActions.jump.performed += OnJump;

        //Player interact
        playerInputActions.PlayerWorldActions.interact.performed += OnInteract;

        //Player fish
        playerInputActions.PlayerWorldActions.throwBobber.performed += OnThrowBobber;

        // Subscribe to dialogue open/close events to handle input switching
        dialogueManager.OnShowDialogue += DialogueOpened;
        dialogueManager.OnHideDialogue += DialogueClosed;

        // Subscribe to menu open/close events to handle input switching
        menuManager.OnShowMenu += MenuOpened;
        menuManager.OnHideMenu += MenuClosed;
    }

    void Start()
    {
        interactableLayerMask = LayerMask.GetMask("Interactable");
        groundMask = LayerMask.GetMask("Terrain");

        playerCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();

        BobberGO = Resources.Load<GameObject>("Prefabs/Bobber");
    }

    private void OnDisable() => playerInputActions.Disable(); // Disable all inputmaps if we disable the player (e.g. cutscene maybe)

    private void OnMoveInput(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }
    private void OnMoveCanceled(InputAction.CallbackContext ctx){
        moveInput = Vector2.zero;
    }
    private void OnInteract(InputAction.CallbackContext ctx)
    {
        TryInteract();
    }
    private void OnThrowBobber(InputAction.CallbackContext ctx)
    {
        if (moveInput == Vector2.zero)
        { //Only allow fishing if player is stopped
            Fish();
        }
    }
    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (!onGround) return; //Don't allow jumping if player is not on the ground
        onGround = false;
        body.AddForce(Vector3.up * 200f, ForceMode.Acceleration);
    }

    private void DialogueOpened()
    {
        SwitchActionMap(playerInputActions.PlayerDialogueActions);
    }
    private void DialogueClosed()
    {
        if (!RestorePreviousActionMap())
        {
            // I should have used a stack for this
            // Menu can be opened over dialogue
            // Therefore an edge case where we go World -> Dialogue -> Menu (Previous set: dialogue, current: menu), Close Menu (restore previous set), close dialogue (previous set now null so error)
            // cba fixing so if in doubt, switch to world actions. Fix if we create more states so it doesn't get unmanageable
            SwitchActionMap(playerInputActions.PlayerWorldActions);
        }
    }
    private void MenuOpened()
    {
        SwitchActionMap(playerInputActions.PlayerMenuActions);
    }
    private void MenuClosed()
    {
        RestorePreviousActionMap(); //Could be world or dialogue
    }
    private InputActionMap GetCurrentMap()
    {
        if (playerInputActions.PlayerWorldActions.enabled) return playerInputActions.PlayerWorldActions;
        if (playerInputActions.PlayerDialogueActions.enabled) return playerInputActions.PlayerDialogueActions;
        if (playerInputActions.PlayerMenuActions.enabled) return playerInputActions.PlayerMenuActions;
        return null;
    }

    private void SwitchActionMap(InputActionMap newMap)
    {
        //Debug.Log("Switching to actionMap "+newMap.name);
        previousMap = GetCurrentMap();
        GetCurrentMap()?.Disable();
        newMap.Enable();
    }
    private bool RestorePreviousActionMap()
    {
        if (previousMap != null)
        {
            //Debug.Log("Switching to actionMap "+previousMap.name);
            GetCurrentMap()?.Disable();
            previousMap.Enable();
            previousMap = null;
            return true;
        }
        else
        {
            return false;
        }
    }


    void Update()
    {
        HandleMovementAndCalculateDirection();

        UpdatePlayerOnGroundStatus();   //Calling this every frame presents a minor performance hit, but allows us to detect non-player-initiated mid-air status
                                        // e.g. if they fall off a cliff
                                        // TODO Move this to fixedUpdate? Less of a hit
    }

    private void HandleMovementAndCalculateDirection()
    {
        if (moveInput != Vector2.zero)
        {
            previousPosition = transform.position;
            animator.SetFloat("MoveX", moveInput.x);
            animator.SetFloat("MoveY", moveInput.y);
            applyMovement(moveInput);
        }
        else
        { //i.e. Player is not moving
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", 0);
        }
        direction = (transform.position - previousPosition).normalized;
    }
    void applyMovement(Vector2 movement)
    {
        gameObject.transform.position = new Vector3(transform.position.x + (movement.x  * speed * Time.deltaTime), transform.position.y, transform.position.z + (movement.y *  speed * Time.deltaTime));
    }

    private void UpdatePlayerOnGroundStatus()
    {
        Vector3 bottom = transform.position + playerCollider.center - Vector3.up * (playerCollider.height / 2f);
        Vector3 rayOrigin = bottom + Vector3.up * 0.01f;

        // Gameobject can only be assigned to one layer
        // Therefore, e.g. a barrel cannot be both interactable and act as a jump ground
        // For future, suggest we use child gameobjects. Parent gameobject is assigned to the 'primary' layer for the object - usually interactable if it is
        // Then child gameobject is assigned to the 'secondary' layer - e.g. terrain
        // Cheaper than comparing tags
        onGround = Physics.Raycast(
        rayOrigin,
        Vector3.down,
        0.1f,
        groundMask
    );
    //Debug.DrawRay(rayOrigin, Vector3.down * (0.1f), hit ? Color.green : Color.red);
    }
    void TryInteract()
    { //Combining checking and interact logic for now. If in future we want to e.g. display the interact options before interacting we should split out
        //Debug.Log("Player tried to interact");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, checkDistance, interactableLayerMask))
        { //i.e. If we hit something that's on the interactable layer
            Interactable thing = hit.collider.GetComponent<Interactable>(); //This will be the first thing the ray hits.
            thing?.Interact(this);
        }

        //directionRaycast = new Vector3(direction.x*1000, direction.y*0, direction.z * 1000);
        //Debug.DrawRay(transform.position, directionRaycast * checkDistance, Color.red);
    }

    public void Fish()
    {
        Vector3 spawnPosition = transform.position;
        GameObject spawnedBobber = Instantiate(BobberGO, spawnPosition,Quaternion.identity);
        Bobber Bobberclass = spawnedBobber.GetComponent<Bobber>();
        Bobberclass.Player = gameObject;
        Rigidbody Bobber_rb = spawnedBobber.GetComponent<Rigidbody>();
        directionBobber = new Vector3(direction.x, direction.y + 2, direction.z);
        Bobber_rb.isKinematic = true;
        Bobber_rb.isKinematic = false;
        Bobber_rb.AddForce(directionBobber * castDist, ForceMode.Impulse);
    }
}