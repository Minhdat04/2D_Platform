using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Health")]
    public int maxHealth = 3;
    int currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amt)
    {
        currentHealth -= amt;
        Debug.Log($"{name} takes {amt} damage. HP = {currentHealth}");
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        Debug.Log($"{name} died!");
        Destroy(gameObject);
    }
}