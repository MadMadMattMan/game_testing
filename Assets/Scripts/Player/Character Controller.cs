using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour {
    // The actions defined in the InputSystemAsset relevent to this controller
    InputAction moveAction, sprintAction, jumpAction, interactAction;

    [Header("Movement Settings")]
    float xVelocity;
    [Tooltip("Speed for base walk")] public float walkSpeed = 0.8f;
    [Tooltip("Speed for shift run")] public float runSpeed = 1.6f;
    float maxSpeed = 0f; // The current max walk speed
    [Tooltip("Rate of speed up for walking")] public float acceleration = 0.25f;
    [Tooltip("Rate of stopping for walking")] public float friction = 2f;
    float epsilon = 0.2f; // tolerance value
    int xFlip = 1; // look direction multiplier
    float scale; // character scale

    [Header("Jumping Settings")]
    [Tooltip("How high the player jumps (depends on gravity)")] public float jumpheight = 2f;
    [Tooltip("How fast the player falls")] public float gravity = 20f;
    bool isGrounded = false; // is currently on ground
    bool pastGrounded = false; // was last frame on ground
    [Tooltip("How close the player needs to be from the ground to count as grounded")]
    public float groundedDistance = 0.075f;

    [Header("Inventory Settings")]
    public InventoryManager inventoryManager;

    [Header("Other")]
    // looping
    GameObject leftSide, rightSide;
    float mapSize; // calculated at runtime

    // private references
    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoxCollider2D col;
    [SerializeField] Animator amr;
    [SerializeField] Transform tf;
    [SerializeField] CinemachineCamera cine;
    [SerializeField] List<GameObject> triggerOverlaps = new List<GameObject>();

    // Awake is called when script is initialized
    void Awake() {
        rb = GetComponent<Rigidbody2D>(); 
        amr = GetComponent<Animator>();
        tf = GetComponent<Transform>();
        col = GetComponent<BoxCollider2D>();

        // Check for missing - if so force error
        if (rb == null || amr == null || tf == null || col == null)
            Debug.LogWarning("Failed to setup all components in " + gameObject.name);
    } 

    // Start called before first frame of game, used to initialize values
    void Start() {
        inventoryManager.InitializeInventory(this);
        scale = tf.localScale.x;

        // Get inputs
        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        jumpAction = InputSystem.actions.FindAction("Jump");
        interactAction = InputSystem.actions.FindAction("Interact");

        scale = tf.localScale.x;
        SetupPlayer();

        // Saves player data
        DontDestroyOnLoad(gameObject);
    }

    public void SetupPlayer() {
        // Get rid of extra players
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
            if (p != gameObject)
                Destroy(p);

        // Search the scene for objects
        leftSide = GameObject.FindWithTag("left tp");
        rightSide = GameObject.FindWithTag("right tp");
        cine = GameObject.FindWithTag("Cinemachine").GetComponent<CinemachineCamera>();
        cine.Target.TrackingTarget = transform;
        
        // Check for missing - if so force error
        if (leftSide == null || rightSide == null || cine == null)
            Debug.LogWarning("Failed to find core GameObjects in scene");

        // Calculate constants
        mapSize = rightSide.transform.position.x - leftSide.transform.position.x;
        transform.localScale = new Vector3(scale, scale, scale);
    }

    // FixedUpdate called at a fixed framerate (typically 60fps) for smoothed physics
    void FixedUpdate() {
        // Collect inputs
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        bool jumpInput = jumpAction.IsPressed();
        bool sprintInput = sprintAction.IsPressed();
        bool interactInput = interactAction.IsPressed();

        // Execute inputs
        SprintCharacter(sprintInput);
        MoveCharacter(moveVector);
        JumpCharacter(jumpInput);
        Interact(interactInput);

        // Apply movement changes
        transform.Translate(new Vector2(xVelocity, 0) * Time.deltaTime);
        transform.localScale = new Vector3(xFlip*scale, scale); // flip the char to move dir

        // Run final checks
        Animations();
        Teleport();
    }

    void SprintCharacter(bool input) {
        if (input)
            maxSpeed = runSpeed;
        else
            maxSpeed = walkSpeed;
    }
    void MoveCharacter(Vector2 input) {
        Vector2 delta = input.normalized * acceleration;
        if (Mathf.Abs(delta.x) <= epsilon)
            xVelocity = Mathf.MoveTowards(xVelocity, 0, friction);

        xVelocity = Mathf.Clamp(xVelocity += delta.x, -maxSpeed, maxSpeed);

        if (xVelocity < 0) xFlip = -1;
        else if (xVelocity > 0) xFlip = 1;
    }
    void JumpCharacter(bool input) {
        // async event handler for coyote time
        Vector3 startpos = new Vector2(tf.position.x, tf.position.y - (col.size.y * tf.localScale.y / 2) -0.01f);
        RaycastHit2D rch = Physics2D.Raycast(startpos, Vector2.down, groundedDistance);
        Debug.DrawRay(startpos, Vector2.down * groundedDistance, rch ? Color.green : Color.red, 0.05f);
        if (rch && rch.collider.tag.Equals("ground") && rch.collider != col)
            isGrounded = true;
        else 
            isGrounded = false;

        // set gravity
        Physics2D.gravity = new Vector2(0, -gravity);

        // handle input
        if (input && isGrounded) {
            rb.AddForce(new Vector2(0, jumpheight), ForceMode2D.Impulse);
            amr.SetTrigger("Jumping");
        }
    }
    void Interact(bool input) {
        if (input && triggerOverlaps.Count > 0) {
            for (int i = 0; i < triggerOverlaps.Count; i++) {
                GameObject t = triggerOverlaps[i];
                Interactable interactable;
                if (t.TryGetComponent<Interactable>(out interactable))
                    interactable.Interact(gameObject);
            }
        }
    }
    void Animations() {
        amr.SetFloat("MoveSpeed", Mathf.Abs(xVelocity));
        if (isGrounded && !pastGrounded)
            amr.SetTrigger("Landing");
        
        pastGrounded = isGrounded; // Set up for next update
    }
    void Teleport() {
        if (tf.position.x < leftSide.transform.position.x && xVelocity < 0) {
            tf.Translate(Vector3.right * mapSize);
            TranslateCinemachinePos(Vector3.right * mapSize);
        }
        else if (tf.position.x > rightSide.transform.position.x && xVelocity > 0) {
            tf.Translate(Vector3.left * mapSize);
            TranslateCinemachinePos(Vector3.left * mapSize);
        }
    }

    public void BeeTaxiTakeoff() {

    }

    // Event methods
    private void OnTriggerEnter2D(Collider2D trigger) {
        triggerOverlaps.Add(trigger.gameObject);
    }
    private void OnTriggerExit2D(Collider2D trigger) {
        triggerOverlaps.Remove(trigger.gameObject);
    }

    // Method helps translate the cinemachine camera (can't edit cinemachine transform like normal objs)
    void TranslateCinemachinePos(Vector3 translation) {
        Vector3 pos = cine.gameObject.transform.position + translation;
        Quaternion quart = cine.gameObject.transform.rotation;
        cine.ForceCameraPosition(pos, quart);
    }
}
