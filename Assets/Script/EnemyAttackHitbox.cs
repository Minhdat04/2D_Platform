using UnityEngine;

/// <summary>
/// Gán tag "EnemyAttack" cho GameObject này khi khởi tạo,
/// để PlayerDamageReceiver có thể nhận diện và xử lý sát thương.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class EnemyAttackHitbox : MonoBehaviour
{
    void Awake()
    {
        // Tự gán tag "EnemyAttack"
        gameObject.tag = "EnemyAttack";
    }

    // Nếu muốn thêm hiệu ứng va chạm (như tiếng đánh, vết đạn, v.v.) 
    // có thể xử lý ở đây. Hiện để trống cho collider trigger.
    void OnTriggerEnter2D(Collider2D other)
    {
        // Ví dụ debug:
        // Debug.Log($"Hitbox của địch chạm vào {other.name}");
    }
}