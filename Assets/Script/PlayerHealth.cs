using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health")]
    public int maxHealth = 5;
    int currentHealth;
    bool isDead = false;
    Animator anim;

    void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int amt)
    {
        if (isDead) return;

        currentHealth -= amt;
        Debug.Log($"Player takes {amt} damage. HP = {currentHealth}");
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player died!");
        anim.SetTrigger("Die");

        // Optional: disable movement and control scripts
        GetComponent<PlayerController>().enabled = false;

        // Start cleanup after animation
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(1f); // Adjust based on animation length
        Destroy(gameObject); // Or trigger respawn/reload here
    }

}