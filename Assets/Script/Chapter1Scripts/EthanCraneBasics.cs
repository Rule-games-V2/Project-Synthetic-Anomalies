using System.Collections;
using UnityEngine;

public class EthanCraneBasics : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool canMove = true;
    private Rigidbody2D rb;
    private float moveInputX;
    private float moveInputY;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(WaitFlip());
    }

    void Update()
    {
        moveInputX = Input.GetAxisRaw("Horizontal");
        moveInputY = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (!canMove) return;
        rb.linearVelocity = new Vector2(moveInputX * moveSpeed, moveInputY * moveSpeed);
        if (moveInputX > 0 && !facingRight) Flip();
        else if (moveInputX < 0 && facingRight) Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    IEnumerator WaitFlip()
    {
        yield return new WaitForSeconds(12f);
        facingRight = true;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}