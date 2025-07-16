using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth { get; private set; }
    public event Action<int, int> OnHealthChanged;
    public event Action OnDied;

    void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int amt)
    {
        Debug.Log($"{name} takes {amt} damage");
        currentHealth = Mathf.Clamp(currentHealth - amt, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        Debug.Log($"{name} died");
        OnDied?.Invoke();
        Destroy(gameObject);
    }
}