using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Moves forward/backward and rotates with WASD/Arrow keys.
/// </summary>
public class PlayerController : MonoBehaviour
{


    // Class-Level Variables--------------------

    [Tooltip("Forward/back speed (units/sec).")]
    public float speed = 1.5f;

    [Tooltip("Turn speed (degrees/sec).")]
    public float rotationSpeed = 90.0f;

    public float jumpForce = 5f;

    private Vector2 moveInput; // Default value = (0, 0)

    private bool jumpRequested; // Default value = false

    private Rigidbody rb; // Default value = null




    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Assigns a reference to the Rigidbody component
        if (rb == null) Debug.LogWarning("PlayerController needs a Rigidbody.");
    }




    private void Update()
    {
        // INPUT ----------------------------

        moveInput = Vector2.zero;

        // Forward/backward
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            moveInput.y = 1f;

        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            moveInput.y = -1f;

        // Left/right
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            moveInput.x = -1f;

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            moveInput.x = 1f;

        // Jump
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            jumpRequested = true;
    }




    private void FixedUpdate()
    {
        // PHYSICS --------------------------

        // Forward/backward movement
        Vector3 movement = transform.forward * moveInput.y * speed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + movement);

        // Y-axis rotation
        float turnDirection = moveInput.x;

        if (moveInput.y < 0)
            turnDirection = -turnDirection;

        float turn = turnDirection * rotationSpeed * Time.fixedDeltaTime;

        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        rb.MoveRotation(rb.rotation * turnRotation);

        // Jump
        if (jumpRequested)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

            jumpRequested = false;
        }
    }
}
