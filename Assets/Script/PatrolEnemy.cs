using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Collider2D))]
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

    // References
    public Transform player;

    // Component cache
    Rigidbody2D rb;
    SpriteRenderer sr;

    // Patrol calculation
    Vector2 startPos;
    float leftX, rightX;
    bool movingRight = true;

    // State
    float lastAttackTime = -Mathf.Infinity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        // Patrol boundaries
        startPos = transform.position;
        leftX = startPos.x - patrolRange;
        rightX = startPos.x + patrolRange;

        // Rigidbody setup
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Frictionless material
        var mat = new PhysicsMaterial2D { friction = 0f, bounciness = 0f };
        GetComponent<Collider2D>().sharedMaterial = mat;

        // Tự tìm Player nếu chưa gán
        if (player == null && GameObject.FindWithTag("Player") != null)
            player = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            float distToPlayer = Vector2.Distance(transform.position, player.position);

            if (distToPlayer <= attackRange)
            {
                // Trong tầm attack → dừng lại và đánh
                rb.velocity = new Vector2(0, rb.velocity.y);
                TryAttack();
                return;
            }
            else if (distToPlayer <= chaseRange)
            {
                // Trong tầm chase → đuổi
                ChasePlayer();
                return;
            }
        }

        // Nếu không chase hoặc attack, chuyển về patrol
        Patrol();
    }

    void Patrol()
    {
        float dir = movingRight ? 1f : -1f;
        rb.velocity = new Vector2(dir * patrolSpeed, rb.velocity.y);
        sr.flipX = dir < 0f;

        float px = rb.position.x;
        if (movingRight && px >= rightX) movingRight = false;
        else if (!movingRight && px <= leftX) movingRight = true;
    }

    void ChasePlayer()
    {
        // Hướng ngang tới player
        float dir = player.position.x > transform.position.x ? 1f : -1f;
        rb.velocity = new Vector2(dir * chaseSpeed, rb.velocity.y);
        sr.flipX = dir < 0f;
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
        // TODO: trigger animation tại đây
        // anim.SetTrigger("Attack");

        // TODO: trừ HP cho Player nếu có script Health
        var hp = player.GetComponent<PlayerHealth>();
        if (hp != null)
            hp.TakeDamage(damage);
    }

    void OnDrawGizmosSelected()
    {
        // Vẽ patrol line
        Vector3 basePos = Application.isPlaying ? (Vector3)startPos : transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            new Vector3(basePos.x - patrolRange, basePos.y, 0),
            new Vector3(basePos.x + patrolRange, basePos.y, 0)
        );
        Gizmos.DrawSphere(new Vector3(basePos.x - patrolRange, basePos.y, 0), 0.1f);
        Gizmos.DrawSphere(new Vector3(basePos.x + patrolRange, basePos.y, 0), 0.1f);

        // Vẽ chase range
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        // Vẽ attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
