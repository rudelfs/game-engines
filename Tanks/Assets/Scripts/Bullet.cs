using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Speed of the bullet
    public float lifetime = 5f; // Time before the bullet despawns
    public float damage = 20f; // Default bullet damage
    private Rigidbody2D rb;
    private bool canHitTank = false; // Tracks whether the bullet can hit the tank
    private int ricochetCount = 0; // Tracks the number of ricochets

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed; // Initialize bullet velocity

        // Enable hitting the tank after 0.1 seconds
        Invoke(nameof(EnableTankCollision), 0.1f);

        Destroy(gameObject, lifetime); // Automatically destroy after lifetime seconds
    }

    private void EnableTankCollision()
    {
        canHitTank = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                if (obstacle.isDestructible)
                {
                    // Apply damage to destructible obstacles
                    obstacle.TakeDamage(damage);
                    Destroy(gameObject); // Destroy the bullet after damaging the obstacle
                }
                else if (obstacle.ricochetBullets)
                {
                    // Ricochet if the obstacle allows it
                    Ricochet(collision.contacts[0].normal);
                }
                else
                {
                    Destroy(gameObject); // Destroy the bullet if it cannot ricochet
                }
            }
        }
        else if (collision.gameObject.CompareTag("Player") && canHitTank)
        {
            // Damage the tank if the bullet can now hit it
            TankStats tank = collision.gameObject.GetComponent<TankStats>();
            if (tank != null)
            {
                tank.TakeDamage(damage);
                Destroy(gameObject); // Destroy the bullet after damaging the tank
            }
        }
        else if (!collision.gameObject.CompareTag("Player")) // Ignore initial collisions with the player
        {
            Destroy(gameObject); // Destroy bullet on hitting non-obstacle objects
        }
    }

    private void Ricochet(Vector2 normal)
    {
        // Check ricochet count
        ricochetCount++;
        if (ricochetCount > 1)
        {
            Destroy(gameObject); // Destroy the bullet after the third collision
            return;
        }

        // Calculate a 90-degree ricochet direction
        Vector2 incident = rb.linearVelocity.normalized; // Current direction of the bullet
        Vector2 perpendicularDirection = Vector2.Perpendicular(incident); // 90 degrees to current line

        // Decide the ricochet direction based on the angle to the normal
        Vector2 ricochetDirection;
        if (Vector2.Dot(perpendicularDirection, normal) > 0)
        {
            ricochetDirection = perpendicularDirection;
        }
        else
        {
            ricochetDirection = -perpendicularDirection;
        }

        // Update the bullet velocity with the ricochet direction
        rb.linearVelocity = ricochetDirection * speed;
    }
}
