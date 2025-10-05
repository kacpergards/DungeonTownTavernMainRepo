using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private PlayerInputActions playerInputActions;
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
    public DialogueManager dialogueManager;

    void Awake()
    {
        // Actions must be registered in awake as it runs before gameobject enabled
        playerInputActions = new PlayerInputActions();

        // Player movement - opting to subscribe to events directly for clarity
        playerInputActions.PlayerWorldActions.move.performed += OnMoveInput;
        playerInputActions.PlayerWorldActions.move.canceled += OnMoveCanceled;
        playerInputActions.PlayerWorldActions.jump.performed += OnJump;

        //Player Interact - no need to handle interact cancel right now
        playerInputActions.PlayerWorldActions.interact.performed += OnInteract;

        //Player fish - as with interact^^
        playerInputActions.PlayerWorldActions.throwBobber.performed += OnThrowBobber;
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

    private void OnEnable() => playerInputActions.Enable();
    private void OnDisable() => playerInputActions.Disable();

    private void OnMoveInput(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }
    private void OnMoveCanceled(InputAction.CallbackContext ctx){
        moveInput = Vector2.zero;
    }
    private void OnInteract(InputAction.CallbackContext ctx)
    {
        Interact();
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

    void Update()
    {
        HandleMovementAndCalculateDirection();

        UpdatePlayerOnGroundStatus();   //Calling this every frame presents a minor performance hit, but allows us to detect non-player-initiated mid-air status
                                        // e.g. if they fall off a cliff
    }

    void HandleMovementAndCalculateDirection()
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

    void UpdatePlayerOnGroundStatus()
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
    void Interact()
    { //Combining checking and interact logic for now. If in future we want to e.g. display the interact options before interacting we should split out
   
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, checkDistance, interactableLayerMask))
        {
            if (hit.collider.gameObject.CompareTag("NPC"))
            { //TODO refactor to use generic Interactable, move logic to the interactable itself
                NPCController npc = hit.collider.GetComponent<NPCController>();
                npc.OnDetected();
                StartCoroutine(dialogueManager.ShowDialogue(npc.dialogue));
            }
        }
 
        directionRaycast = new Vector3(direction.x*1000, direction.y*0, direction.z * 1000);
        Debug.DrawRay(transform.position, directionRaycast * checkDistance, Color.red);
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
