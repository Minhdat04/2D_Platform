using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private Image healthBarFillImage;
    [SerializeField] private Image healthBarTrailingFillImage;
    [SerializeField] private float trailDelay = 0.4f;
    public int maxHealth = 5;
    int currentHealth;
    bool isDead = false;
    Animator anim;
    AudioManager audioManager;
    void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        healthBarFillImage.fillAmount = 1f;
        healthBarTrailingFillImage.fillAmount = 1f;
    }

    public void TakeDamage(int amt)
    {
        if (isDead) return;

        currentHealth -= amt;
        // Cast to float so we get a decimal ratio
        float ratio = (float)currentHealth / maxHealth;

        // Animate the main bar immediately
        Sequence sequence = DOTween.Sequence();
        sequence.Append(
            healthBarFillImage
                .DOFillAmount(ratio, 0.1f)
                .SetEase(Ease.InOutSine)
        );

        // Wait a bit, then animate the trailing bar
        sequence.AppendInterval(trailDelay);
        sequence.Append(
            healthBarTrailingFillImage
                .DOFillAmount(ratio, 0.15f)
                .SetEase(Ease.InOutSine)
        );

        sequence.Play();
        Debug.Log($"Player takes {amt} damage. HP = {currentHealth}");
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player died!");
        anim.SetTrigger("Die");
        audioManager.PlaySFX(audioManager.playerDeath);

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