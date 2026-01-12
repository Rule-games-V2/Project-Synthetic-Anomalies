using System.Collections;
using UnityEngine;

public class BedScript : MonoBehaviour
{
    [Header("Transforms")]
    public Transform bedTransform;
    public Transform playerTransform;
    public Transform TeleportLocTransform;

    [Header("Scripts")]
    public EthanCraneBasics playerMovement;
    private Rigidbody2D rb;

    private bool playerInside = false;
    private bool isSleeping = false;

    void Start()
    {
        rb = playerTransform.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E) && !isSleeping)
        {
            isSleeping = true;
            if (rb != null) rb.linearVelocity = Vector2.zero;
            playerTransform.position = bedTransform.position;
            playerMovement.canMove = false;
            StartCoroutine(SequanceStarting());
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInside = true;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInside = false;
    }

    public IEnumerator SequanceStarting()
    {
        yield return new WaitForSeconds(5f);

        if (isSleeping)
        {
            playerTransform.position = TeleportLocTransform.position;
            StartCoroutine(SequanceNightmare());
        }
    }

    public IEnumerator SequanceNightmare()
    {
        if (rb != null) rb.simulated = true;

        playerMovement.canMove = false;

        float sayac = 0;
        while (sayac < 3f)
        {
            playerTransform.Translate(Vector2.left * 3f * Time.deltaTime);
            sayac += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        Vector3 tempScale = playerTransform.localScale;
        tempScale.x = 1f;
        playerTransform.localScale = tempScale;

        yield return new WaitForSeconds(0.8f);

        tempScale.x = -1f;
        playerTransform.localScale = tempScale;
        yield return new WaitForSeconds(0.5f);

        sayac = 0;
        while (sayac < 4f)
        {
            playerTransform.Translate(Vector2.left * 5f * Time.deltaTime);
            sayac += Time.deltaTime;
            yield return null;
        }

        playerMovement.canSprint = true;
        playerMovement.canMove = true;
        isSleeping = false;

        Debug.Log("Hýzlan [Shift]");
    }
}