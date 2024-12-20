using UnityEngine;

public class EnemyTankBehavior : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float moveSpeed = 1.5f; // Speed of the tank during patrol
    public float patrolDuration = 3f; // Time spent moving during patrol
    public float stopDuration = 2f; // Time spent stationary during patrol

    [Header("Detection Settings")]
    public float detectionRange = 15f; // Range to detect the player
    public LayerMask detectionLayer; // Layer to detect the player tank

    [Header("Shooting Settings")]
    public GameObject projectilePrefab; // Bullet prefab
    public Transform firePoint; // Fire point of the tank
    public float fireRate = 2f; // Time in seconds between shots
    public float projectileSpeed = 5f; // Speed of the projectile

    private Transform playerTank; // Reference to the player tank
    private Rigidbody2D rb;
    private float patrolTimer; // Tracks the patrol movement time
    private float stopTimer; // Tracks the stop time
    private bool isPatrolling = true; // Whether the tank is patrolling
    private bool playerDetected = false; // Whether the player is detected
    private float lastFireTime = 0f; // Tracks the last time the enemy fired
    private Vector2 patrolDirection; // Current patrol direction

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Initialize patrol behavior
        patrolTimer = patrolDuration;
        patrolDirection = GetRandomDirection();

        // Find the player tank
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTank = player.transform;
        }
        else
        {
            Debug.LogError("Player tank not found! Ensure the player tank is tagged as 'Player'.");
        }
    }

    void Update()
    {
        if (playerDetected)
        {
            // If player is detected, focus on shooting and tracking the player
            TrackAndShootPlayer();
        }
        else
        {
            // Patrol if the player is not detected
            Patrol();
            CheckForPlayer();
        }
    }

    private void Patrol()
    {
        if (isPatrolling)
        {
            // Move in the patrol direction
            rb.linearVelocity = patrolDirection * moveSpeed;
            patrolTimer -= Time.deltaTime;

            if (patrolTimer <= 0f)
            {
                // Stop patrolling and begin stationary behavior
                isPatrolling = false;
                rb.linearVelocity = Vector2.zero;
                stopTimer = stopDuration;
            }
        }
        else
        {
            // Stay stationary for a set duration
            stopTimer -= Time.deltaTime;

            if (stopTimer <= 0f)
            {
                // Resume patrolling with a new direction
                isPatrolling = true;
                patrolTimer = patrolDuration;
                patrolDirection = GetRandomDirection();
            }
        }
    }

    private Vector2 GetRandomDirection()
    {
        // Generate a random direction
        float randomAngle = Random.Range(0f, 360f);
        return new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;
    }

    private void CheckForPlayer()
    {
        if (playerTank == null) return;

        // Check if the player is within detection range
        float distanceToPlayer = Vector2.Distance(transform.position, playerTank.position);
        if (distanceToPlayer > detectionRange) return;

        // Check for line of sight to the player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (playerTank.position - transform.position).normalized, detectionRange, detectionLayer);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            playerDetected = true;
            Debug.Log("Player detected!");
        }
    }

    private void TrackAndShootPlayer()
    {
        if (playerTank == null) return;

        // Stop moving and rotate to face the player
        rb.linearVelocity = Vector2.zero;
        Vector2 directionToPlayer = (playerTank.position - transform.position).normalized;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        rb.rotation = angle;

        // Shoot at the player
        ShootAtPlayer();
    }

    private void ShootAtPlayer()
    {
        if (Time.time < lastFireTime + fireRate) return; // Fire rate cooldown

        lastFireTime = Time.time;

        // Spawn a projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        if (projectile == null)
        {
            Debug.LogError("Projectile prefab not assigned or invalid.");
            return;
        }

        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        if (projectileRb != null)
        {
            projectileRb.linearVelocity = firePoint.right * projectileSpeed; // Use firePoint's direction for velocity
        }
        else
        {
            Debug.LogError("Projectile prefab does not have a Rigidbody2D component.");
        }
    }
}
