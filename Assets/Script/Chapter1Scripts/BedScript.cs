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
    public float moveWarning;

    private bool playerInside = false;
    private bool isSleeping = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            isSleeping = !isSleeping;

            if (isSleeping)
            {
                playerTransform.position = bedTransform.position;
                playerMovement.canMove = false;
                StartCoroutine(SequanceStarting());
            }
            else
            {
                playerMovement.canMove = true;
                StopAllCoroutines();
            }
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
        yield return new WaitForSeconds(3f);

        if (isSleeping)
        {
            playerTransform.position = TeleportLocTransform.position;
            StartCoroutine(SequanceNightmare());
        }
    }

    public IEnumerator SequanceNightmare()
    {
        playerMovement.canMove = false;

        yield return new WaitForSeconds(4f);

        float sayac = 0;
        while (sayac < 2f)
        {
            playerTransform.Translate(Vector2.left * 3f * Time.deltaTime);
            sayac += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        Vector3 tempScale = playerTransform.localScale;
        tempScale.x = 1f;
        playerTransform.localScale = tempScale;

        yield return new WaitForSeconds(1.5f);

        tempScale.x = -1f;
        playerTransform.localScale = tempScale;
        yield return new WaitForSeconds(0.5f);

        sayac = 0;
        while (sayac < 4f)
        {
            playerTransform.Translate(Vector2.left * 4.5f * Time.deltaTime);
            sayac += Time.deltaTime;
            yield return null;
        }

        playerMovement.canSprint = true;

        playerMovement.canMove = true;
        isSleeping = false;
        moveWarning = playerMovement.moveSpeed = 0f;
        if (moveWarning <= 1)
        {
            Debug.Log("Canavardan Kaç [Shift]");
        }
    }
}