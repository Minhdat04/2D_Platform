using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health")]
    public int maxHealth = 5;
    int currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amt)
    {
        currentHealth -= amt;
        Debug.Log($"Player takes {amt} damage. HP = {currentHealth}");
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        Debug.Log("Player died!");
        // TODO: play death animation, reload scene, etc.
        Destroy(gameObject);
    }
}