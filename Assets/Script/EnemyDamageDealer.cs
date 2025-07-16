using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class EnemyDamageDealer : MonoBehaviour
{
    [Header("Attack Hitbox")]
    public int damage = 1;
    public float activeDuration = 0.2f;
    Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
        col.enabled = false;
    }

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
        if (other.CompareTag("Enemy"))
        {
            var eh = other.GetComponent<EnemyHealth>();
            if (eh != null)
            {
                Debug.Log("Hit Enemy! Calling TakeDamage");
                eh.TakeDamage(damage);
            }
        }
    }
}