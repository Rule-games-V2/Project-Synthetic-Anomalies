using UnityEngine;

public class LaneLaser : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float moveDistance = 4f;
    public float speed = 2f;
    public float offset = 0f; // Her lazerin farklý zamanlamayla baþlamasý için

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
        // (Time.time + offset) kullanarak her lazerin döngüsünü kaydýrdýk
        float newY = Mathf.PingPong((Time.time + offset) * speed, moveDistance);
        transform.position = startPos + new Vector3(0, newY, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Lazer Ethan'a veya bloða deðerse hata tetiklenir
        if (other.CompareTag("Player") || other.CompareTag("Block"))
        {
            HandleFailure();
        }
    }

    void HandleFailure()
    {
        string quote = victorQuotes[Random.Range(0, victorQuotes.Length)];
        Debug.Log("<color=red>Victor:</color> " + quote);

        // Reset iþlemi burada tetiklenecek
    }
}