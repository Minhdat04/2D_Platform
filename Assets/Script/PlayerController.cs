using UnityEngine;
using System.Collections;  // để dùng IEnumerator

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    [Header("Combat")]
    [Tooltip("Empty GameObject con chứa EnemyDamageDealer")]
    public EnemyDamageDealer attackHitbox;
    public KeyCode attackKey = KeyCode.J;
    public float attackCooldown = 0.5f;

    // Components
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    // State
    float moveInput;
    bool jumpRequested;
    bool isGrounded;
    float lastAttackTime = -Mathf.Infinity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Physics setup
        rb.gravityScale = 3f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // --- Movement Input ---
        moveInput = Input.GetAxisRaw("Horizontal");

        // --- Ground Check ---
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
        anim.SetBool("isGrounded", isGrounded);

        // --- Jump ---
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequested = true;
            anim.SetTrigger("Jump");
        }

        // --- Attack ---
        if (Input.GetKeyDown(attackKey) && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            anim.SetTrigger("Attack");
            if (attackHitbox != null)
                attackHitbox.DoAttack();
        }
    }

    void FixedUpdate()
    {
        // Horizontal move
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Jump force
        if (jumpRequested)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpRequested = false;
        }

        // Flip & run
        if (moveInput != 0f)
            sr.flipX = moveInput < 0f;
        anim.SetFloat("speed", Mathf.Abs(moveInput));
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}