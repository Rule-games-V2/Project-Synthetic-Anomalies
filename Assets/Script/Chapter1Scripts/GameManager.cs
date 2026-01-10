using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public wentScript wentScript;
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

    [Header("Kondisyon Koridoru Ayarları")]
    public Transform miraTransform;
    public GameObject enjektor;
    public float corridorWalkSpeed = 1.0f;
    public float interactionDistance = 1.5f;
    public Transform finalDoorPoint;
    public string nextSceneName;

    void Start()
    {
        Debug.Log("[SİSTEM]: Tiz Ses Seviyesi %100. Ethan Uyanıyor.");
        playerRb.simulated = false;
        bed.enabled = false;
        StartCoroutine(ControlTutorial());
    }

    IEnumerator ControlTutorial()
    {
        yield return new WaitForSeconds(4f);
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
        yield return new WaitForSeconds(0.1f);
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
                Debug.Log("<Victor>: Kapı açıldı, Mira Koridorda Bekliyor ");
                SadakatPuani += 8;
                yield return new WaitForSeconds(1.3f);
                Debug.Log("Sadakat '+8'");
                yield return new WaitForSeconds(1.3f);
                doorCollider.enabled = true;
                chosen = true;
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                bed.enabled = true;
                doorCollider.enabled = false;
                Debug.Log("Nabzın yalan söylüyor Ethan.");
                yield return new WaitForSeconds(1.3f);
                Debug.Log("Sağlık kontrolün için sonra konuşacağız");
                yield return new WaitForSeconds(1.3f);
                Debug.Log("Tekrar geleceğim, Şimdi biraz uyu.");
                yield return new WaitForSeconds(1.3f);
                SadakatPuani -= 8;
                Debug.Log("Sadakat '-8'");
                chosen = true;
            }
            yield return null;
        }
        playerRb.linearVelocity = Vector2.zero;
        playerMovement.canMove = true;
    }

    public IEnumerator InstantBedTeleport()
    {
        playerMovement.canMove = false;
        playerRb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(0.1f);

        playerTransform.position = bed.transform.position;
        playerMovement.canMove = true;

        yield return new WaitForSeconds(7f);

        yield return StartCoroutine(wentScript.SequenceExit());

        doorCollider.enabled = true;

        StartCoroutine(StartConditioningCorridor());
    }

    IEnumerator StartConditioningCorridor()
    {
        // Koridorda yavaş yürüme hızı
        playerMovement.moveSpeed = corridorWalkSpeed;
        playerMovement.canMove = true;

        // Mira'nın yanına gidene kadar bekle
        yield return new WaitUntil(() => Vector2.Distance(playerTransform.position, miraTransform.position) < interactionDistance);

        playerMovement.canMove = false;
        playerRb.linearVelocity = Vector2.zero;

        // Diyalog ve Etkileşim
        Debug.Log("<Mira>: (Diz çöker) 'Kalbin çok hızlı atıyor Ethan. Baban Bunu Almanı istiyor seni rahatlatacak.'");
        if (enjektor != null) enjektor.SetActive(true);

        yield return new WaitForSeconds(2f);
        Debug.Log("<Victor - Hoparlör>: 'Mira, vakit kaybediyoruz. Nabzını stabilize et ve bir sonraki odaya yönlendir.'");

        Debug.Log("KRİTİK SEÇİM: [Q] İtaat (Enjektörü kabul et) | [F] Red (Enjektörü it)");

        bool choiceMade = false;
        while (!choiceMade)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SadakatPuani += 8;
                Debug.Log("[ETKİ]: Ekran anlık yumuşar, Ethan'ın titremesi durur.");

                yield return new WaitForSeconds(1f);
                Debug.Log("Sadakat +8");
                choiceMade = true;
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                SadakatPuani -= 8;
                enjektor.SetActive(false);
                Debug.Log("[ETKİ]: Mira şaşırır, iğneyi saklar. Victor: 'Mira, onu test odasına götür!'");

                yield return new WaitForSeconds(1f);
                Debug.Log("Sadakat -8");
                choiceMade = true;
            }
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine(FinalTransition());
    }

    IEnumerator FinalTransition()
    {
        Debug.Log("Mira, Ethan'ı koridorun sonundaki kapıdan içeri sokar.");
        if (finalDoorPoint != null) playerTransform.position = finalDoorPoint.position;

        yield return new WaitForSeconds(1f);
        Debug.Log("EKRAN KARARDI.");

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(nextSceneName);
    }
}