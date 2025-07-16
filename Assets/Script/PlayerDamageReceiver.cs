using UnityEngine;

public class PlayerDamageReceiver : MonoBehaviour
{
    PlayerHealth hp;

    void Awake() => hp = GetComponent<PlayerHealth>();

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("EnemyAttack"))  // tag do Enemy đặt
        {
            hp.TakeDamage(1);
        }
    }
}