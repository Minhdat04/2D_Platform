using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class EnemyDamageDealer : MonoBehaviour
{
    [Tooltip("Số sát thương gây ra")]
    public int damage = 1;

    [Tooltip("Thời gian collider hoạt động (giây)")]
    public float activeDuration = 0.2f;

    Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
        col.enabled = false;
    }

    /// <summary> Gọi khi PlayerController muốn bật hitbox tấn công </summary>
    public void DoAttack()
    {
        StopAllCoroutines();
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        Debug.Log("Hitbox ON");
        col.enabled = true;
        yield return new WaitForSeconds(activeDuration);
        col.enabled = false;
        Debug.Log("Hitbox OFF");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var eh = other.GetComponent<EnemyHealth>();
        if (eh != null)
        {
            Debug.Log("Hit Enemy! Calling TakeDamage");
            eh.TakeDamage(damage);
        }
    }
}