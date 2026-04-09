using System.Collections;
using Unity.Android.Gradle.Manifest;
using Unity.Cinemachine;
using Unity.ProjectAuditor.Editor.Core;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour {
    // The actions defined in the InputSystemAsset relevent to this controller
    InputAction moveAction, sprintAction, jumpAction;

    [Header("Character Settings")]
    // horizontal
    public Vector2 velocity;
    public float walkSpeed = 1f;
    public float sprintSpeed = 2f;
    float maxSpeed = 0f;
    public float acceleration = 2f;
    public float friction = 2f;
    float epsilon = 0.2f;
    int xFlip = 1;
    float scale = 0.1f;

    // vertical
    public float jumpheight = 1f;
    public float gravity = 9.81f;
    public bool isGrounded = false;
    bool pastGrounded = false;
    public float coyoteTime = 0.15f;

    // looping
    GameObject leftSide, rightSide;
    float mapSize;

    // private references
    Rigidbody2D rb;
    Animator amr;
    Transform tf;
    CinemachineCamera cine;

    // Awake is called when script is initialized
    void Awake() {
        rb = GetComponent<Rigidbody2D>(); 
        amr = GetComponent<Animator>();
        tf = GetComponent<Transform>();
    } 

    // Start called before first frame of game, used to initialize values
    void Start() {
        // Get inputs
        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        jumpAction = InputSystem.actions.FindAction("Jump");

        // Search the scene for objects
        leftSide = GameObject.FindGameObjectWithTag("left tp");
        rightSide = GameObject.FindGameObjectWithTag("right tp");
        cine = GameObject.FindGameObjectWithTag("Cinemachine").GetComponent<CinemachineCamera>();

        // Check for missing - if so force error
        if (leftSide == null || rightSide == null || cine == null)
            Debug.LogWarning("No valid left|right teleporter");

        // Calculate constants
        mapSize = rightSide.transform.position.x - leftSide.transform.position.x;
    }

    // FixedUpdate called at a fixed framerate (typically 60fps) for smoothed physics
    void FixedUpdate() {
        // Collect inputs
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        bool jumpInput = jumpAction.IsPressed() && isGrounded;
        bool sprintInput = sprintAction.IsPressed();

        // Execute inputs
        SprintCharacter(sprintInput);
        MoveCharacter(moveVector);
        JumpCharacter(jumpInput);

        // Apply movement changes
        transform.Translate(velocity * Time.deltaTime);
        transform.localScale = new Vector3(xFlip*scale, scale); // flip the char to move dir

        // Run final checks
        Animations();
        Teleport();
    }

    void SprintCharacter(bool input) {
        if (input)
            maxSpeed = sprintSpeed;
        else
            maxSpeed = walkSpeed;
    }
    void MoveCharacter(Vector2 input) {
        Vector2 delta = input.normalized * acceleration;
        if (Mathf.Abs(delta.x) <= epsilon)
            velocity.x = Mathf.MoveTowards(velocity.x, 0, friction);

        velocity.x = Mathf.Clamp(velocity.x += delta.x, -maxSpeed, maxSpeed);

        if (velocity.x < 0) xFlip = -1;
        else if (velocity.x > 0) xFlip = 1;
    }
    void JumpCharacter(bool input) {
        // async event handler for coyote time


        // set gravity
        Physics2D.gravity = new Vector2(0, -gravity);

        // handle input
        if (input) {
            rb.AddForce(new Vector2(0, jumpheight), ForceMode2D.Impulse);
            amr.SetTrigger("Jumping");
        }
    }
    void Animations() {
        amr.SetFloat("MoveSpeed", Mathf.Abs(velocity.x));
        if (isGrounded && !pastGrounded) {
            amr.SetTrigger("Landing");
        }

        pastGrounded = isGrounded; // Set up for next update
    }
    void Teleport() {
        if (tf.position.x < leftSide.transform.position.x && velocity.x < 0) {
            tf.Translate(Vector3.right * mapSize);
            TranslateCinemachinePos(Vector3.right * mapSize);
        }
        else if (tf.position.x > rightSide.transform.position.x && velocity.x > 0) {
            tf.Translate(Vector3.left * mapSize);
            TranslateCinemachinePos(Vector3.left * mapSize);
        }
    }

    // Event Methods
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "ground")
            isGrounded = true;
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "ground") 
            isGrounded = false;
    }

    // Method helps translate the cinemachine camera (can't edit transform like normal objs)
    void TranslateCinemachinePos(Vector3 translation) {
        Vector3 pos = cine.gameObject.transform.position + translation;
        Quaternion quart = cine.gameObject.transform.rotation;
        cine.ForceCameraPosition(pos, quart);
    }
}
