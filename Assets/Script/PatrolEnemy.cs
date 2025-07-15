using UnityEngine;
using System.Collections;  // mặc dù không dùng IEnumerator ở đây, vẫn ổn nếu cần mở rộng

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Collider2D))]
public class PatrolEnemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float patrolRange = 3f;
    public float patrolSpeed = 2f;

    [Header("Chase & Attack")]
    public float chaseRange = 5f;
    public float attackRange = 1f;
    public float chaseSpeed = 3.5f;
    public float attackCooldown = 1f;
    public int damage = 1;

    [Tooltip("Transform của Player (set bằng Tag hoặc drag thẳng)")]
    public Transform player;

    // Components
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    // Patrol state
    Vector2 startPos;
    float leftX, rightX;
    bool movingRight = true;

    // Attack timing
    float lastAttackTime = -Mathf.Infinity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Patrol bounds
        startPos = transform.position;
        leftX = startPos.x - patrolRange;
        rightX = startPos.x + patrolRange;

        // Rigidbody setup
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Frictionless
        var mat = new PhysicsMaterial2D { friction = 0f, bounciness = 0f };
        GetComponent<Collider2D>().sharedMaterial = mat;

        // Auto-find player by tag nếu chưa drag
        if (player == null)
        {
            var p = GameObject.FindWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            float dist = Vector2.Distance(transform.position, player.position);

            if (dist <= attackRange)
            {
                // Attack
                rb.velocity = new Vector2(0, rb.velocity.y);
                TryAttack();
                return;
            }
            else if (dist <= chaseRange)
            {
                // Chase
                Chase();
                return;
            }
        }

        // Patrol nếu không chase/attack
        Patrol();
    }

    void Patrol()
    {
        float dir = movingRight ? 1f : -1f;
        rb.velocity = new Vector2(dir * patrolSpeed, rb.velocity.y);
        sr.flipX = dir < 0f;
        anim.SetBool("isMoving", Mathf.Abs(dir) > 0f);

        float px = rb.position.x;
        if (movingRight && px >= rightX) movingRight = false;
        else if (!movingRight && px <= leftX) movingRight = true;
    }

    void Chase()
    {
        float dir = player.position.x > transform.position.x ? 1f : -1f;
        rb.velocity = new Vector2(dir * chaseSpeed, rb.velocity.y);
        sr.flipX = dir < 0f;
        anim.SetBool("isMoving", true);
    }

    void TryAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            anim.SetTrigger("Attack");
            // Gây damage cho Player nếu có component PlayerHealth
            var ph = player.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 bp = Application.isPlaying ? (Vector3)startPos : transform.position;
        Gizmos.DrawLine(bp + Vector3.left * patrolRange, bp + Vector3.right * patrolRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}