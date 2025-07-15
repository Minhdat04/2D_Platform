using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Collider2D))]
public class EnemyDamageDealer : MonoBehaviour
{
    public int damage = 1;
    public float activeDuration = 0.1f;

    Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
        col.enabled = false;  // chỉ bật khi tấn công
    }

    public void DoAttack()
    {
        StopAllCoroutines();
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        col.enabled = true;
        yield return new WaitForSeconds(activeDuration);
        col.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var eh = other.GetComponent<EnemyHealth>();
        if (eh != null)
            eh.TakeDamage(damage);
    }
}