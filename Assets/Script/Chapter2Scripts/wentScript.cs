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

    void Start()
    {
        // Hata almamak için baþlangýçta oyuncuyu garantiye alýyoruz
        if (playerObj == null)
        {
            playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null && playerCollider == null)
            {
                playerCollider = playerObj.GetComponent<Collider2D>();
            }
        }
    }

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

        // Eðer hala null ise (örneðin canavar yakaladýðýnda) tekrar kontrol et
        if (playerObj == null) playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerCollider == null && playerObj != null) playerCollider = playerObj.GetComponent<Collider2D>();

        // Havalandýrma hedefine ýþýnla
        playerObj.transform.position = wentTargetTransform.position;

        var movement = playerObj.GetComponent<EthanCraneBasics>();
        var rb = playerObj.GetComponent<Rigidbody2D>();

        if (playerCollider != null) playerCollider.enabled = false;
        if (movement != null) movement.canMove = false;
        if (rb != null) rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(6f);

        // Yataða geri ýþýnla
        playerObj.transform.position = bedTransform.position;
        if (bedCollider != null) bedCollider.enabled = true;
        if (movement != null) movement.canMove = true;

        yield return new WaitForSeconds(7f);
        Debug.Log("Victor: Uyandýðýný görüyorum Ethan. Kapý açýldý.");

        isSequencing = false;
        if (playerCollider != null) playerCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            playerObj = other.gameObject;
            playerCollider = other.GetComponent<Collider2D>();
            Debug.Log("Havalandýrmaya Saklan [E]");
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