using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Tooltip("Số máu hồi cho player khi nhặt")]
    public int healAmount = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Chỉ hồi máu khi va chạm với Player (phải tag Player)
        if (!collision.CompareTag("Player"))
            return;

        PlayerHealth ph = collision.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.Heal(healAmount);
        }

        // (Tùy chọn) phát hiệu ứng hay âm thanh ở đây
        // e.g. AudioManager.PlayPickupSFX();

        Destroy(gameObject);
    }
}