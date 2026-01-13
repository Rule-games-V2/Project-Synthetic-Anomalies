using UnityEngine;
using System.Collections;

public class FinalChapterLaser : MonoBehaviour
{
    public enum LaserType { Vertical, Horizontal, Blink }
    [Header("Bu Lazerin Tipi")]
    public LaserType laserType;

    [Header("Hareket Ayarlarý")]
    public float speed = 3f;
    public float distance = 5f;
    public float offset = 0f;

    [Header("Referanslar")]
    public Transform player; // Ethan
    public Transform resetPoint; // Baþlangýç Noktasý

    private Vector3 startPos;
    private Collider2D laserCollider;
    private SpriteRenderer laserRenderer;

    // Victor'un 6 repliði
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
        laserCollider = GetComponent<Collider2D>();
        laserRenderer = GetComponent<SpriteRenderer>();

        if (laserType == LaserType.Blink)
        {
            StartCoroutine(BlinkRoutine());
        }
    }

    void Update()
    {
        // Zamanlama hesaplamasý
        float move = Mathf.PingPong((Time.time + offset) * speed, distance);

        if (laserType == LaserType.Vertical)
        {
            // TAVANA ÇIKAN LAZER: Yukarý gidince altýndan koþarak geçersin.
            transform.position = startPos + new Vector3(0, move, 0);
        }
        else if (laserType == LaserType.Horizontal)
        {
            // SAÐA KAYAN LAZER: Saða gidince solunda oluþan boþluktan geçersin.
            transform.position = startPos + new Vector3(move, 0, 0);
        }
    }

    IEnumerator BlinkRoutine()
    {
        while (true)
        {
            laserRenderer.enabled = true;
            laserCollider.enabled = true;
            yield return new WaitForSeconds(2f);


            laserRenderer.enabled = false;
            laserCollider.enabled = false;
            yield return new WaitForSeconds(1.5f);

            laserRenderer.enabled = true;
            laserCollider.enabled = true;
            yield return new WaitForSeconds(1f);

            laserRenderer.enabled = false;
            laserCollider.enabled = false;
            yield return new WaitForSeconds(0.5f);

            laserRenderer.enabled = true;
            laserCollider.enabled = true;
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ethan veya blok deðerse her þey baþa döner.
        if (other.CompareTag("Player") || other.CompareTag("Block"))
        {
            // Victor konuþur.
            string quote = victorQuotes[Random.Range(0, victorQuotes.Length)];
            Debug.Log("<color=red>Victor:</color> " + quote);

            // Ethan'ý baþlangýca salla.
            if (player != null && resetPoint != null)
            {
                player.position = resetPoint.position;
            }
        }
    }
}