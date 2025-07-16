using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class PatrolEnemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float patrolRange = 3f;
    public float patrolSpeed = 2f;

    [Header("Chase & Attack Settings")]
    [Tooltip("Player sẽ bị phát hiện khi cách enemy không quá X mét")]
    public float chaseRange = 5f;
    [Tooltip("Enemy sẽ tấn công khi cách player không quá X mét")]
    public float attackRange = 1f;
    [Tooltip("Tốc độ khi đuổi player")]
    public float chaseSpeed = 3.5f;
    [Tooltip("Thời gian giữa hai lần tấn công (giây)")]
    public float attackCooldown = 1f;
    public int damage = 1;

    [Header("Attack Hitbox")]
    [Tooltip("Kéo GameObject chứa EnemyDamageDealer vào đây")]
    public EnemyDamageDealer attackHitbox;

    // References
    Transform player;

    // Component cache
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    // Patrol calculation
    Vector2 startPos;
    float leftX, rightX;
    bool movingRight = true;
    bool isAttacking = false;

    // State
    float lastAttackTime = -Mathf.Infinity;

    void Awake()
    {
        // Cache components
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Thiết lập collider & rigidbody
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Frictionless material để tránh “dính” tường/platform
        var mat = new PhysicsMaterial2D { friction = 0f, bounciness = 0f };
        GetComponent<Collider2D>().sharedMaterial = mat;

        // Tính vùng patrol
        startPos = transform.position;
        leftX = startPos.x - patrolRange;
        rightX = startPos.x + patrolRange;

        // Tìm player theo tag
        var go = GameObject.FindWithTag("Player");
        if (go != null) player = go.transform;
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            float dist = Vector2.Distance(transform.position, player.position);

            if (dist <= attackRange)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                TryAttack();
                return;
            }
            else if (dist <= chaseRange)
            {
                ChasePlayer();
                return;
            }
        }

        Patrol();
    }

    void Patrol()
    {
        isAttacking = false;
        float dir = movingRight ? 1f : -1f;
        rb.velocity = new Vector2(dir * patrolSpeed, rb.velocity.y);
        sr.flipX = dir < 0f;
        anim.SetBool("isMoving", true);

        float px = rb.position.x;
        if (movingRight && px >= rightX) movingRight = false;
        else if (!movingRight && px <= leftX) movingRight = true;
    }

    void ChasePlayer()
    {
        float dir = player.position.x > transform.position.x ? 1f : -1f;
        rb.velocity = new Vector2(dir * chaseSpeed, rb.velocity.y);
        sr.flipX = dir < 0f;
        anim.SetBool("isMoving", true);
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;
        Attack();
    }

    void Attack()
    {
        // Trigger animation
        isAttacking = true;
        anim.SetTrigger("Attack");

        // Bật hitbox nếu có
        if (attackHitbox != null)
        {
            attackHitbox.damage = damage;
            attackHitbox.DoAttack();
        }
        else
        {
            // Fallback: direct damage to player
            var hp = player.GetComponent<PlayerHealth>();
            if (hp != null)
                hp.TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 basePos = Application.isPlaying ? (Vector3)startPos : transform.position;

        // Patrol line
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            new Vector3(basePos.x - patrolRange, basePos.y, 0),
            new Vector3(basePos.x + patrolRange, basePos.y, 0)
        );
        Gizmos.DrawSphere(new Vector3(basePos.x - patrolRange, basePos.y, 0), 0.1f);
        Gizmos.DrawSphere(new Vector3(basePos.x + patrolRange, basePos.y, 0), 0.1f);

        // Chase range
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        // Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}