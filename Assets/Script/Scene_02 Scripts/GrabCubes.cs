using UnityEngine;
using System.Collections.Generic;

public class FinalChapterManager : MonoBehaviour
{
    [Header("Ayarlar")]
    public float grabRange = 2f;
    public float speedReduction = 0.85f;
    public LayerMask blockLayer;
    public LayerMask slotLayer;
    public Transform resetPoint;

    [Header("Durum")]
    public GameObject grabbedBlock;
    public bool isGrabbing = false;
    public static int placedBlocksCount = 0;

    private float originalSpeed;
    private EthanCraneBasics movementScript;
    private Dictionary<GameObject, Vector3> blockHomePositions = new Dictionary<GameObject, Vector3>();

    void Start()
    {
        movementScript = GetComponent<EthanCraneBasics>();
        if (movementScript != null) originalSpeed = movementScript.moveSpeed;

        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject block in blocks)
        {
            blockHomePositions[block] = block.transform.position;
        }
        placedBlocksCount = 0;
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
            Vector3 holdPosition = transform.position + (transform.right * 1.2f);
            grabbedBlock.transform.position = holdPosition;
        }
    }

    void TryGrabBlock()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, grabRange, blockLayer);
        if (hit != null && hit.CompareTag("Block"))
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb == null || rb.bodyType == RigidbodyType2D.Static) return;

            grabbedBlock = hit.gameObject;
            isGrabbing = true;
            if (movementScript != null) movementScript.moveSpeed = originalSpeed * speedReduction;

            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void ReleaseBlock()
    {
        if (grabbedBlock != null)
        {
            Rigidbody2D rb = grabbedBlock.GetComponent<Rigidbody2D>();
            Collider2D slotHit = Physics2D.OverlapCircle(grabbedBlock.transform.position, 1.2f, slotLayer);

            if (slotHit != null)
            {
                Slot slotScript = slotHit.GetComponent<Slot>();
                if (slotScript != null && !slotScript.isOccupied)
                {
                    grabbedBlock.transform.position = slotHit.transform.position;
                    rb.bodyType = RigidbodyType2D.Static;
                    slotScript.isOccupied = true;
                    placedBlocksCount++;
                }
                else if (rb != null) rb.bodyType = RigidbodyType2D.Dynamic;
            }
            else if (rb != null) rb.bodyType = RigidbodyType2D.Dynamic;

            grabbedBlock = null;
        }
        isGrabbing = false;
        if (movementScript != null) movementScript.moveSpeed = originalSpeed;
    }

    public void HandleLaserHit()
    {
        if (grabbedBlock != null)
        {
            GameObject blockToReset = grabbedBlock;
            ReleaseBlock();
            if (blockHomePositions.ContainsKey(blockToReset))
                blockToReset.transform.position = blockHomePositions[blockToReset];
        }
        if (resetPoint != null) transform.position = resetPoint.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser")) HandleLaserHit();
    }
}