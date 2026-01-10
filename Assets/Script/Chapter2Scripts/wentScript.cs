using UnityEngine;
using System.Collections;

public class wentScript : MonoBehaviour
{
    [Header("Ayarlar")]
    public Transform wentTargetTransform;
    public Transform bedTransform;
    public BoxCollider2D bedCollider;

    public Collider2D playerCollider;
    private bool isPlayerInside = false;
    private GameObject playerObj;
    private bool isSequencing = false;

    void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E) && !isSequencing)
        {
            StartCoroutine(SequenceExit());
        }
    }

    public IEnumerator SequenceExit()
    {
        isSequencing = true;
        playerObj.transform.position = wentTargetTransform.position;
        var movement = playerObj.GetComponent<EthanCraneBasics>();
        var rb = playerObj.GetComponent<Rigidbody2D>();

        if (playerCollider != null) playerCollider.enabled = false; // Canavar artýk deðemez
        if (movement != null) movement.canMove = false;
        if (rb != null) rb.linearVelocity = Vector2.zero;


        yield return new WaitForSeconds(6f);

        playerObj.transform.position = bedTransform.position;
        if (bedCollider != null) bedCollider.enabled = true;
        if (movement != null) movement.canMove = true;

        yield return new WaitForSeconds(7f);
        Debug.Log("Victor: Uyandýðýný görüyorum Ethan. Kapý açýldý.");

        isSequencing = false;
        playerCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            playerObj = other.gameObject;
            Debug.Log("Havalandýrmaya Saklan [E]");
            isPlayerInside = true;
            playerObj = other.gameObject;
            playerCollider = other.GetComponent<Collider2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }
}