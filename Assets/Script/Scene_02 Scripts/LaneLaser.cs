using UnityEngine;

public class SmartLaneLaser : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float moveSpeed = 2f;
    public float moveDistance = 5f;
    public float offset = 0f;
    private Vector3 startPos;
    public Transform player;
    public Transform SafeLimitLocation;

    private string[] victorQuotes = {
        "Hassasiyetin zayýf Ethan.",
        "Zaman kaybediyoruz, odaklan!",
        "Hayal kýrýklýðý... Tekrar baþla.",
        "Vücudun zihninden daha yavaþ hareket ediyor.",
        "Hadi Ethan...",
        "Tekrar dene."
    };

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Görseldeki gibi saða-sola sakin ve gergin gidiþ
        float newX = Mathf.PingPong((Time.time + offset) * moveSpeed, moveDistance);
        transform.position = startPos + new Vector3(newX, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Katman matrisini ayarladýðýn için bu lazer 
        // sadece kendi kulvarýndaki objeyi algýlayacak.
        if (other.CompareTag("Player") || other.CompareTag("Block"))
        {
            string quote = victorQuotes[Random.Range(0, victorQuotes.Length)];
            Debug.Log("<color=red>Victor:</color> " + quote);

            SafeLimitLocation.position = player.position;
        }
    }
}