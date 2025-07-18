using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health UI")]
    [SerializeField] private Image healthBarFillImage;
    [SerializeField] private Image healthBarTrailingFillImage;
    [SerializeField] private float trailDelay = 0.4f;

    [Header("Health Settings")]
    [Tooltip("Máu tối đa của nhân vật")]
    public int maxHealth = 5;

    private int currentHealth;
    private bool isDead = false;

    private Animator anim;
    private AudioManager audioManager;

    // ** Thêm reference đến DeathMenuController **
    private DeathMenuController deathMenu;

    void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        healthBarFillImage.fillAmount = 1f;
        healthBarTrailingFillImage.fillAmount = 1f;

        // ** Lấy reference đến DeathMenuController **
        deathMenu = FindObjectOfType<DeathMenuController>();
    }

    public void TakeDamage(int amt)
    {
        if (isDead) return;

        currentHealth = Mathf.Max(currentHealth - amt, 0);
        float ratio = (float)currentHealth / maxHealth;

        // Tween thanh chính giảm ngay, trailing giảm sau
        Sequence seq = DOTween.Sequence();
        seq.Append(
            healthBarFillImage
                .DOFillAmount(ratio, 0.1f)
                .SetEase(Ease.InOutSine)
        );
        seq.AppendInterval(trailDelay);
        seq.Append(
            healthBarTrailingFillImage
                .DOFillAmount(ratio, 0.15f)
                .SetEase(Ease.InOutSine)
        );
        seq.Play();

        Debug.Log($"Player takes {amt} damage. HP = {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amt)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amt, maxHealth);
        float ratio = (float)currentHealth / maxHealth;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(trailDelay);
        seq.Insert(0,
            healthBarTrailingFillImage
                .DOFillAmount(ratio, 0.1f)
                .SetEase(Ease.InOutSine)
        );
        seq.Append(
            healthBarFillImage
                .DOFillAmount(ratio, 0.15f)
                .SetEase(Ease.InOutSine)
        );
        seq.Play();

        Debug.Log($"Player healed {amt}. HP = {currentHealth}/{maxHealth}");
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player died!");
        anim.SetTrigger("Die");
        audioManager.PlaySFX(audioManager.playerDeath);

        // Vô hiệu hoá điều khiển nhân vật
        GetComponent<PlayerController>().enabled = false;

        // Chạy coroutine để chờ animation xong
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        // Chờ animation Die (giả sử 1s)
        yield return new WaitForSeconds(1f);

        // ** Hiện death menu thay vì Destroy -->
        if (deathMenu != null)
            deathMenu.ShowDeathMenu();
        else
            Debug.LogWarning("DeathMenuController not found in scene!");
    }
}