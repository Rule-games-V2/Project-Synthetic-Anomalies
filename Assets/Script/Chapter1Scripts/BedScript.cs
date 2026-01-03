using System.Collections;
using UnityEngine;

public class BedScript : MonoBehaviour
{
    public Transform bedTransform;
    public GameObject bed;
    public GameObject player;
    public Transform playerTransform;
    public EthanCraneBasics playerMovement;
    public Transform TeleportLocTransform;
    bool playerInside = false;
    bool isSleeping = false;

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
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    public IEnumerator SequanceStarting()
    {
        yield return new WaitForSeconds(4f);
        if (isSleeping)
        {
            playerTransform.position = TeleportLocTransform.position;
            isSleeping = false;
            playerMovement.canMove = true;
        }
    }
}