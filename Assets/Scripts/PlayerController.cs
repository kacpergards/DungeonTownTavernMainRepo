using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public LayerMask terrainLayer;
    public SpriteRenderer spriteRenderer;
    public Animation anim;
    public Animator animator;
    public Terrain terrain;
    float terrainOffset = 0.515f;
    //public float moveX;
    //public float moveY;
    

    public float checkDistance = 2f;
    private Vector3 previousPosition;
    public Vector3 direction;
    public Vector3 directionBobber;
    public Vector3 directionRaycast;
    public float castDist = 20;
    Rigidbody body;
    bool onGround = false;
    public GameObject BobberGO;
    public DialogueManager dialogueManager;

    // Update is called once per frame
    void Start()
    {
        if (terrain == null)
        {
            //Debug.LogWarning("Terrain is not assigned in the Inspector.");
        }
        else
        {
            //Debug.Log("Terrain successfully assigned: " + terrain.name);
        }
        BobberGO = Resources.Load<GameObject>("Prefabs/Bobber");
    }
private void Awake()
    {
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        Vector2 movementDiff = new Vector2(0, 0);
        Vector3 newPosition = transform.position;

        //check if anything is infront of character
        CheckForObjectInFront();

        //fixanimations
        if (Input.GetKey(KeyCode.W))
        {
            movementDiff.y += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementDiff.x -=1;
            animator.SetFloat("MoveX", -1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementDiff.y -=1;
            animator.SetFloat("MoveY", -1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementDiff.x +=1;
            animator.SetFloat("MoveX", 1);
        }

        if (movementDiff != Vector2.zero)
        {
            previousPosition = transform.position;
            applyMovement(movementDiff);
        }


        direction = (transform.position - previousPosition).normalized;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (Input.GetKeyUp(KeyCode.Space) && onGround)
        {
            body.AddForce(Vector3.up * 400f, ForceMode.Acceleration);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        //Debug.LogWarning("Terrain is not assigned!");

        if (Input.GetKeyDown(KeyCode.F) && (movementDiff == Vector2.zero))
        {
            Fish();
        }
    }

    void applyMovement(Vector2 movement)
    {
        gameObject.transform.position = new Vector3(transform.position.x + (movement.x  * speed * Time.deltaTime), transform.position.y, transform.position.z + (movement.y *  speed * Time.deltaTime));
        animator.SetFloat("MoveY", 1);
    }

    void applyAnimation(Vector2 movement)
    {

    }
    void CheckForObjectInFront()
    {
        // Get the player's forward direction
        //Vector3 direction = facing;
        LayerMask layerMask = LayerMask.GetMask("Interactable");

        // Perform a raycast in the forward direction
   
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, checkDistance, layerMask))
        {
            Debug.Log("Hit collider "+hit.collider.gameObject.name);
            if (hit.collider.gameObject.CompareTag("NPC"))
            {
                NPCController npc = hit.collider.GetComponent<NPCController>();
                npc.OnDetected(); // Call the method on the NPC
                if (Input.GetKeyUp(KeyCode.C))
                {
                    Debug.Log("pressing C and its not working");
                    StartCoroutine(dialogueManager.ShowDialogue(npc.dialogue));

                }
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

        Debug.Log("cast");
    }

    void Interact()
    {
        CheckForObjectInFront();
    }
}
