using UnityEngine;

public class TankStats : MonoBehaviour
{
    [Header("Tank Stats")]
    public float movementSpeed = 5f; // Speed of the tank
    public float turnSpeed = 100f; // Rotation speed of the tank
    public float fireRate = 0.5f; // Time (in seconds) between each shot
    public float health = 100f; // Tank's health
    public float bulletDamage = 20f; // Damage dealt by bullets

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Tank took damage: " + damage + ", Remaining Health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        Debug.Log("Tank healed: " + amount + ", Current Health: " + health);
    }

    private void Die()
    {
        Debug.Log("Tank destroyed!");
        Destroy(gameObject); // Destroy the tank GameObject
    }
}
