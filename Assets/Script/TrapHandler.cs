using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TrapHandler : MonoBehaviour
{
    [Header("Trap Settings")]
    [Tooltip("Thẻ this object: 'Trap' hay 'Jumper'")]
    public string trapTag;           // gõ đúng 'Trap' hoặc 'Jumper' trong Inspector
    [Tooltip("Máu mất khi chạm Trap")]
    public int damage = 1;
    [Tooltip("Lực hất tung khi chạm Jumper")]
    public float bounceForce = 15f;

    void Awake()
    {
        // Đảm bảo collider là trigger
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        PlayerHealth ph = other.GetComponent<PlayerHealth>();

        if (trapTag == "Trap")
        {
            // Mất máu
            if (ph != null)
                ph.TakeDamage(damage);
        }
        else if (trapTag == "Jumper")
        {
            // Hất tung
            if (rb != null)
            {
                // reset vận tốc đứng y
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
            // Trigger animation jump nếu có
            Animator anim = other.GetComponent<Animator>();
            if (anim != null)
                anim.SetTrigger("Jump");
        }
    }
}