using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Dash")]
    public float dashDistance = 5f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    float lastDashTime = -Mathf.Infinity;
    bool isDashing;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    [Header("Combat")]
    [Tooltip("Kéo GameObject Hitbox vào đây (chứa EnemyDamageDealer)")]
    public EnemyDamageDealer attackHitbox;
    public KeyCode attackKey = KeyCode.Mouse0;
    public float attackCooldown = 0.5f;
    float lastAttackTime = -Mathf.Infinity;

    // Components
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    // State
    float moveInput;
    bool jumpRequested;
    bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        rb.gravityScale = 3f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // 1) Input di chuyển
        moveInput = Input.GetAxisRaw("Horizontal");

        // 2) Kiểm tra chạm đất
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
        anim.SetBool("isGrounded", isGrounded);

        // 3) Nhảy
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequested = true;
            anim.SetTrigger("Jump");
        }

        // 4) Dash
        if (Input.GetKeyDown(KeyCode.X) && Time.time >= lastDashTime + dashCooldown)
        {
            lastDashTime = Time.time;
            StartCoroutine(DoDash());
        }

        // 5) Attack
        if (Input.GetKeyDown(attackKey) && Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("Attack pressed");
            lastAttackTime = Time.time;
            anim.SetTrigger("Attack");

            if (attackHitbox != null)
            {
                Debug.Log("Calling DoAttack()");
                attackHitbox.DoAttack();
            }
            else
            {
                Debug.LogWarning("attackHitbox is null! Kéo Hitbox vào slot này.");
            }
        }

        // Update chạy animation
        anim.SetFloat("speed", Mathf.Abs(moveInput));
    }

    void FixedUpdate()
    {
        // A) Nhảy
        if (jumpRequested)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpRequested = false;
        }

        // B) Di chuyển
        if (!isDashing)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }

        // C) Flip sprite
        if (moveInput != 0f)
            sr.flipX = moveInput < 0f;
    }

    IEnumerator DoDash()
    {
        isDashing = true;
        anim.SetTrigger("Dash");

        Vector2 dir = sr.flipX ? Vector2.left : Vector2.right;
        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            rb.velocity = dir * (dashDistance / dashDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
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