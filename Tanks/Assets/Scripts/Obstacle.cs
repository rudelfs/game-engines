using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public bool isDestructible = false; // Can this obstacle be destroyed?
    public int maxHits = 1; // Number of hits it can take before being destroyed
    public bool ricochetBullets = true; // Do bullets ricochet off this obstacle?

    private int currentHits = 0; // Tracks the number of hits taken
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null && isDestructible)
        {
            Debug.LogWarning("SpriteRenderer not found! Color change will not work.");
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDestructible)
        {
            currentHits++;

            Debug.Log($"Obstacle took {damage} damage!");

            // Update color to indicate damage
            if (spriteRenderer != null)
            {
                float darkness = 1f - (currentHits / (float)maxHits);
                spriteRenderer.color = new Color(darkness, darkness, darkness);
            }

            if (currentHits >= maxHits)
            {
                Destroy(gameObject); // Destroy the obstacle
                Debug.Log("Obstacle destroyed!");
            }
        }
        else
        {
            Debug.Log($"Obstacle is indestructible and took {damage} damage.");
        }
    }
}
