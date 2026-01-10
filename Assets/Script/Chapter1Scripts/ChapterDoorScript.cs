using UnityEngine;
using System.Collections;

public class ChapterDoorScript : MonoBehaviour
{
    public Collider2D doorCollider;     // Kapýnýn Collider'ý (Baþta kapalý olmalý)
    public Transform playerTransform;
    public Transform corridorTeleportLoc;

    private bool canInteract = false;

    void Start()
    {
        // Baþlangýçta kapý kapalý
        if (doorCollider != null) doorCollider.enabled = false;
    }

    // WentScript'ten bu fonksiyon çaðrýlacak
    public void StartDoorProcess(IEnumerator sequence)
    {
        StartCoroutine(RunSequenceThenOpen(sequence));
    }

    private IEnumerator RunSequenceThenOpen(IEnumerator sequence)
    {
        yield return StartCoroutine(sequence);
        doorCollider.enabled = true;
    }

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            Teleport();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            Debug.Log("Çýk [E]");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
        }
    }

    private void Teleport()
    {
        playerTransform.position = corridorTeleportLoc.position;
        canInteract = false;
    }
}