using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Collider2D))]
public class PatrolEnemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float patrolRange = 3f;   // nửa chiều dài đường đi lại
    public float speed = 2f;   // tốc độ ngang
    // (bỏ arrivalThreshold để toggle chính xác tại biên)

    Rigidbody2D rb;
    SpriteRenderer sr;
    Vector2 startPos;
    float leftX, rightX;
    bool movingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        //--- tính biên trái/phải dựa vào vị trí khởi đầu ---
        startPos = transform.position;
        leftX = startPos.x - patrolRange;
        rightX = startPos.x + patrolRange;

        //--- thiết lập Rigidbody2D Dynamic nhưng không lật ---
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        //--- PhysicsMaterial2D friction=0 để không bị kẹt ---
        var mat = new PhysicsMaterial2D
        {
            friction = 0f,
            bounciness = 0f
        };
        GetComponent<Collider2D>().sharedMaterial = mat;
    }

    void FixedUpdate()
    {
        // 1) luôn gán vận tốc ngang  
        float dir = movingRight ? 1f : -1f;
        rb.velocity = new Vector2(dir * speed, rb.velocity.y);

        // 2) lật sprite (flipX) cho đúng chiều
        sr.flipX = dir < 0f;

        // 3) toggle ngay khi tới biên (không dùng threshold lớn nữa)
        float px = rb.position.x;
        if (movingRight && px >= rightX)
        {
            movingRight = false;
        }
        else if (!movingRight && px <= leftX)
        {
            movingRight = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        // vẽ đường patrol trong Editor
        Vector3 basePos = Application.isPlaying ? (Vector3)startPos : transform.position;
        Vector3 a = new Vector3(basePos.x - patrolRange, basePos.y, 0f);
        Vector3 b = new Vector3(basePos.x + patrolRange, basePos.y, 0f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(a, b);
        Gizmos.DrawSphere(a, 0.1f);
        Gizmos.DrawSphere(b, 0.1f);
    }
}