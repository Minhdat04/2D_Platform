using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Health")]
    public int maxHealth = 3;
    public int currentHealth { get; private set; }

    public event Action<int, int> OnHealthChanged;
    public event Action OnDied;

    void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        OnDied?.Invoke();
        Destroy(gameObject);
    }
}