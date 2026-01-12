using UnityEngine;

public class SimpleLaneLaser : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float moveSpeed = 2f;     // Saða sola gidiþ hýzý
    public float moveDistance = 3f;  // Gidiþ mesafesi
    public float offset = 0f;        // Lazerlerin senkronunu bozmak için

    private Vector3 startPos;

    [Header("Victor Replikleri")]
    private string[] victorQuotes = {
        "Hassasiyetin zayýf Ethan. Tekrar dene.",
        "Zaman kaybediyoruz, odaklan!",
        "Hayal kýrýklýðý... Tekrar baþla.",
        "Vücudun zihninden daha yavaþ hareket ediyor.",
        "Veri akýþýný bozuyorsun, stabil kal!",
        "Sistem hata kabul etmez."
    };

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Sadece X ekseninde sakin bir ping-pong hareketi
        float newX = Mathf.PingPong((Time.time + offset) * moveSpeed, moveDistance);
        transform.position = startPos + new Vector3(newX, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Katmanlar (Layers) doðru ayarlandýysa, sadece kendi þeridindeki Ethan'a çarpar
        if (other.CompareTag("Player") || other.CompareTag("Block"))
        {
            HandleFailure();
        }
    }

    void HandleFailure()
    {
        string quote = victorQuotes[Random.Range(0, victorQuotes.Length)];
        Debug.Log("<color=red>Victor:</color> " + quote);

        // Buraya Ethan'ý baþlangýca ýþýnlama kodunu ekleyeceðiz.
    }
}