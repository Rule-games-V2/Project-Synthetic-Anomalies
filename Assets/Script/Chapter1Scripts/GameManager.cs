using System.Collections;
using UnityEngine;

public class GameManagerC1 : MonoBehaviour
{
    public Collider2D doorCollider;
    public int SadakatPuani = 0;
    public EthanCraneBasics playerMovement;
    public Rigidbody2D playerRb;
    public Transform masa;
    public GameObject player;
    public Transform playerTransform;
    public float mesafe = 1f;
    public GameObject tableObj;
    bool hasTriggered = false;
    public BoxCollider2D bed;

    void Start()
    {
        Debug.Log("[SİSTEM]: Tiz Ses Seviyesi %100. Ethan Uyanıyor.");
        playerRb.simulated = false;
        bed.enabled = false;
        StartCoroutine(ControlTutorial());
    }

    IEnumerator ControlTutorial()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("(W) İleri");
        yield return new WaitForSeconds(1f);
        Debug.Log("(A) Sol");
        yield return new WaitForSeconds(1f);
        Debug.Log("(S) Geri");
        yield return new WaitForSeconds(1f);
        Debug.Log("(D) Sağ");

        yield return new WaitForSeconds(1f);
        playerRb.simulated = true;

        yield return new WaitForSeconds(1f);
        Debug.Log("Masaya doğru ilerle...");

        yield return new WaitUntil(() =>
            Vector2.Distance(playerRb.position, masa.position) < mesafe
        );

        if (!hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(WaitAndPrint());
        }
    }

    public IEnumerator WaitAndPrint()
    {
        yield return new WaitForSeconds(0.5f);
        playerMovement.canMove = false;
        playerRb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(1f);
        Debug.Log("<Victor>: 'Günaydın Ethan. bugün seninle gurur duymak istiyorum.'");
        yield return new WaitForSeconds(2.5f);
        Debug.Log("<Victor>: (Nabzı Ölçer.)");
        yield return new WaitForSeconds(2f);
        Debug.Log("<Victor>: 'Kalbin Hızlı Atıyor iyimisin ethan yoksa benden bişey mi saklıyorsun'");
        yield return new WaitForSeconds(1.4f);
        Debug.Log("Seçim: [Q] 'Baba Korkuyorum...'");
        Debug.Log("Seçim: [F] 'İyiyim'");

        bool chosen = false;
        while (!chosen)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                bed.enabled = true;
                Debug.Log("<Victor>: Dürüstlük Bağımızı güçlendirir Ethan.");
                yield return new WaitForSeconds(1.3f);
                SadakatPuani += 8;
                Debug.Log("Sadakat '+8'");
                yield return new WaitForSeconds(1.3f);
                Debug.Log(" <= Yatağa Yat");
                chosen = true;
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                bed.enabled = true;
                doorCollider.enabled = !doorCollider.enabled;
                Debug.Log("Nabzın yalan söylüyor Ethan, beni üzüyorsun.");
                yield return new WaitForSeconds(1.3f);
                Debug.Log("Git biraz uyu");
                yield return new WaitForSeconds(1.3f);
                SadakatPuani -= 8;
                Debug.Log("Sadakat '-8'");
                yield return new WaitForSeconds(1.3f);
                Debug.Log(" <= Yatağa Yat");
                chosen = true;
            }
            yield return null;
        }
        playerRb.linearVelocity = Vector2.zero;
        playerMovement.canMove = true;
    }
}