using System.Collections;
using UnityEngine;

public class EthanCraneBasics : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public bool canMove = true;
    public bool canSprint = false; // Senin istediðin kilit burada
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

        // SHIFT KONTROLÜ: Eðer kilit açýksa ve Shift'e basýlýyorsa hýzý 10 yap, yoksa 5 kalsýn.
        if (canSprint && Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 5f;
        }
        else
        {
            moveSpeed = 3.5f;
        }
    }

    void FixedUpdate()
    {
        if (!canMove) return;
        // Senin orijinal hareket kodun, hiçbir þeyi deðiþtirmedim
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