using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleCameraController : MonoBehaviour
{
    public float moveSpeed = 3.0f;           // Movement speed
    public float rotationSpeed = 100.0f;      // Rotation speed for A/D keys in degrees per second
    public float mouseSensitivity = 1.0f;    // Mouse look sensitivity

    public InputAction moveAction;
    public InputAction lookAction;

    private float rotationX = 0.0f;          // Rotation around the X axis (up and down look)

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
    }

    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        
        // Movement
        float moveForward = moveInput.y * moveSpeed * Time.deltaTime;
        // Translate only on the X-Z plane (global Y remains unchanged)
        Vector3 moveDirection = new Vector3(transform.forward.x, 0.0f, transform.forward.z).normalized;
        transform.Translate(moveDirection * moveForward, Space.World);

        // Rotation
        float turn = moveInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, turn, 0, Space.World); // Rotate around the global Y axis

        // Mouse Look
        float mouseY = lookAction.ReadValue<float>() * mouseSensitivity;

        rotationX -= mouseY;  // Subtracting to invert the up and down look
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);  // Clamp the up and down look to avoid flipping

        transform.localEulerAngles = new Vector3(rotationX, transform.localEulerAngles.y, 0);
    }
    
    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
    }
}
