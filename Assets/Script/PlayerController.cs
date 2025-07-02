using UnityEngine;

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

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    // Các biến nhận input / trạng thái
    float moveInput;
    bool jumpRequested;
    bool isGrounded;

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
        // 1) Đọc input trái/phải
        moveInput = Input.GetAxisRaw("Horizontal");

        // 2) Kiểm tra chạm đất
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
        anim.SetBool("isGrounded", isGrounded);

        // 3) Đọc input nhảy (chỉ đánh dấu yêu cầu nhảy)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequested = true;
            anim.SetTrigger("Jump");
        }
    }

    void FixedUpdate()
    {
        // 4) Áp dụng vận tốc ngang
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // 5) Thực hiện nhảy (Impulse)
        if (jumpRequested)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpRequested = false;
        }

        // 6) Flip sprite & animation chạy
        if (moveInput != 0)
            sr.flipX = moveInput < 0;
        anim.SetFloat("speed", Mathf.Abs(moveInput));
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}