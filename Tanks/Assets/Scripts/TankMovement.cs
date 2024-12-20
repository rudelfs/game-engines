using UnityEngine;

public class TankMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private TankStats stats; // Reference to TankStats

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<TankStats>(); // Access TankStats
    }

    void Update()
    {
        // Get movement and rotation inputs
        float moveInput = Input.GetAxis("Vertical"); // W/S or Up/Down
        float turnInput = Input.GetAxis("Horizontal"); // A/D or Left/Right

        // Handle rotation
        if (Mathf.Abs(turnInput) > 0.01f)
        {
            float turn = turnInput * stats.turnSpeed * Time.deltaTime; // Use turnSpeed from TankStats
            rb.rotation -= turn;
        }

        // Handle movement
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            Vector2 forward = transform.up * moveInput * stats.movementSpeed; // Use movementSpeed from TankStats
            rb.linearVelocity = forward; // Set the velocity
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // Stop the tank if no movement input is detected
        }
    }
}
