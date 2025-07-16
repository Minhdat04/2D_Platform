using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("Máu tối đa của player")]
    public int maxHealth = 5;

    [Tooltip("Máu hiện tại của player")]
    public int currentHealth { get; private set; }

    // Sự kiện báo thay đổi máu (dùng để UI lắng nghe)
    public event Action<int, int> OnHealthChanged; // (current, max)

    void Awake()
    {
        currentHealth = maxHealth;
        // Phát ngay sự kiện để UI khởi tạo đúng
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    /// <summary>
    /// Giam máu player
    /// </summary>
    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    /// <summary>
    /// Hồi máu player
    /// </summary>
    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }


    void Die()
    {
        // TODO: thêm hiệu ứng/chuyển scene khi chết
        Debug.Log("Player đã chết!");
    }
}
