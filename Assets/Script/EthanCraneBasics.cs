using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public bool useGravity = true;

    [Header("Ground Check")]
    public Transform feet;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private float moveInputX;
    private float moveInputY;
    private bool isGrounded;

    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = useGravity ? 1 : 0;
    }

    void Update()
    {
        moveInputX = Input.GetAxisRaw("Horizontal");

        if (useGravity)
        {
            isGrounded = Physics2D.OverlapCircle(
                feet.position,
                groundCheckRadius,
                groundLayer
            );

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
        else
        {
            moveInputY = Input.GetAxisRaw("Vertical");
        }
    }

    void FixedUpdate()
    {
        if (useGravity)
        {
            rb.linearVelocity = new Vector2(moveInputX * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(moveInputX * moveSpeed, moveInputY * moveSpeed);
        }

        // YÖN FLIP
        if (moveInputX > 0 && !facingRight)
            Flip();
        else if (moveInputX < 0 && facingRight)
            Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
