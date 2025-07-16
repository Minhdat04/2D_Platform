using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Health")]
    public int maxHealth = 3;

    [Header("Animation Timings")]
    [Tooltip("How long to stay in the Hit state before you can be hit again")]
    public float hitRecoveryTime = 0.4f;
    [Tooltip("Length of the Die animation (seconds)")]
    public float deathAnimationTime = 1f;

    int currentHealth;
    bool isHit = false;
    bool isDead = false;
    Animator anim;
    AudioManager audioManager;

    void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void TakeDamage(int amt)
    {
        // block if already in hit?stun or dead
        if (isHit || isDead)
            return;

        currentHealth -= amt;
        Debug.Log($"{name} takes {amt} damage. HP = {currentHealth}");

        if (currentHealth > 0)
        {
            StartCoroutine(Hit());
        }
        else
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Hit()
    {
        isHit = true;
        anim.SetTrigger("Hit");
        audioManager.PlaySFX(audioManager.skeletonHit);

        // wait out the hit?stun window
        yield return new WaitForSeconds(hitRecoveryTime);
        isHit = false;
    }

    IEnumerator Die()
    {
        isDead = true;
        anim.SetTrigger("Die");

        // Optional: disable colliders or movement scripts here
        // e.g. GetComponent<Collider2D>().enabled = false;

        // let the Die animation play
        yield return new WaitForSeconds(deathAnimationTime);

        Destroy(gameObject);
    }
}