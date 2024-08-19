using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public Rigidbody2D rb; // Reference to the Rigidbody2D component
    public Vector2 moveDirection = Vector2.right; // Direction of movement
    // public AnimationCurve speedCurve; // Speed curve to control movement speed
    public float cycleDuration = 2f; // Time to complete a full back-and-forth cycle
    public float rotationSpeed = 0f;
    private float elapsedTime = 0f; // Time elapsed in the current cycle

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    void FixedUpdate()
    {
        if (rb == null)
            return;

        // Update elapsed time
        elapsedTime += Time.fixedDeltaTime;

        // Calculate normalized time (0 to 1 over cycle duration)
        float normalizedTime = (elapsedTime % cycleDuration) / cycleDuration;

        // Calculate movement for this frame
        Vector2 movement = moveDirection * Time.fixedDeltaTime;
        // Reverse the direction at the midpoint of the cycle
        if (normalizedTime >= 0.5f)
        {
            movement = -movement;
        }

        // Move the Rigidbody2D
        rb.MovePosition(rb.position + movement);

        rb.MoveRotation(rb.rotation + rotationSpeed);
    }
}
