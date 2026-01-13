using UnityEngine;

public class EthanGrabbing : MonoBehaviour
{
    [Header("Ayarlar")]
    public float grabRange = 2f;
    public float speedReduction = 0.85f; // %15 yavaşlama
    public LayerMask blockLayer;

    public GameObject grabbedBlock;
    public bool isGrabbing = false;
    private float originalSpeed;
    private EthanCraneBasics movementScript;

    void Start()
    {
        movementScript = GetComponent<EthanCraneBasics>();
        if (movementScript != null) originalSpeed = movementScript.moveSpeed;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isGrabbing) TryGrabBlock();
            else ReleaseBlock();
        }

        if (isGrabbing && grabbedBlock != null)
        {
            // Bloğu Ethan'ın önünde tut
            Vector3 holdPosition = transform.position + (transform.right * 1f);
            grabbedBlock.transform.position = holdPosition;
        }
    }

    void TryGrabBlock()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, grabRange, blockLayer);
        if (hit != null && hit.CompareTag("Block"))
        {
            grabbedBlock = hit.gameObject;
            isGrabbing = true;
            if (movementScript != null) movementScript.moveSpeed = originalSpeed * speedReduction;

            Rigidbody2D rb = grabbedBlock.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    public void ReleaseBlock()
    {
        if (grabbedBlock != null)
        {
            Rigidbody2D rb = grabbedBlock.GetComponent<Rigidbody2D>();
            if (rb != null) rb.bodyType = RigidbodyType2D.Dynamic;
            grabbedBlock = null;
        }
        isGrabbing = false;
        if (movementScript != null) movementScript.moveSpeed = originalSpeed;
    }
}