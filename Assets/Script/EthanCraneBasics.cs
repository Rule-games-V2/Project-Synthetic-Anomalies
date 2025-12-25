using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private float moveInputX;
    private float moveInputY;

    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInputX = Input.GetAxisRaw("Horizontal");
        moveInputY = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
       
        rb.linearVelocity = new Vector2(moveInputX * moveSpeed, moveInputY * moveSpeed);

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
