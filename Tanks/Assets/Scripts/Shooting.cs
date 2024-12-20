using UnityEngine;

public class TankShooting : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab for the bullet
    public Transform firePoint; // Firing position
    public Collider2D tankCollider; // Reference to the tank's main collider

    private TankStats stats; // Reference to TankStats
    private float lastFireTime = 0f; // Tracks the last time the tank fired

    void Start()
    {
        // Find TankStats on the parent or the same GameObject
        stats = GetComponentInParent<TankStats>();

        // Log an error if TankStats is not found
        if (stats == null)
        {
            Debug.LogError("TankStats component is missing on this GameObject or its parent!");
        }
    }

    void Update()
    {
        if (stats != null && Input.GetMouseButtonDown(0) && Time.time >= lastFireTime + stats.fireRate)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        lastFireTime = Time.time;

        // Create a projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Ensure the tank's collider is ignored by the bullet
        Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
        if (tankCollider != null && projectileCollider != null)
        {
            Physics2D.IgnoreCollision(projectileCollider, tankCollider);
        }

        // Add velocity to the projectile
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.linearVelocity = firePoint.right * stats.movementSpeed; // Use stats.movementSpeed for bullet speed

        // Assign bullet damage from TankStats
        Bullet bullet = projectile.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.damage = stats.bulletDamage; // Pass the damage stat to the bullet
        }
    }
}
