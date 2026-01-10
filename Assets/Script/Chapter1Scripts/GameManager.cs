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
                Debug.Log(" <= Yatağa Yat");
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
        playerMovement.canMove = true;
    }

    public IEnumerator InstantBedTeleport()
    {
        playerMovement.canMove = false;
        playerRb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(0.5f);

        playerTransform.position = bed.transform.position;
        playerMovement.canMove = true;

        yield return new WaitForSeconds(7f);

        // EKSİK OLAN DİYALOG BURAYA EKLENDİ
        Debug.Log("Victor: Uyandığını görüyorum Ethan. Kapı açıldı.");

        doorCollider.enabled = true;
        StartCoroutine(StartConditioningCorridor());
    }

    IEnumerator StartConditioningCorridor()
    {
        playerMovement.moveSpeed = corridorWalkSpeed;

        yield return new WaitUntil(() => Vector2.Distance(playerTransform.position, miraTransform.position) < interactionDistance);

        playerMovement.canMove = false;
        playerRb.linearVelocity = Vector2.zero;

        Debug.Log("<Mira>: (Diz çöker) 'Kalbin çok hızlı atıyor Ethan. Baban bunu almanı istiyor seni rahatlatacak.'");
        if (enjektor != null) enjektor.SetActive(true);

        yield return new WaitForSeconds(2.5f);
        Debug.Log("<Victor - Hoparlör>: 'Mira, vakit kaybediyoruz. Nabzını stabilize et ve bir sonraki odaya yönlendir.'");

        Debug.Log("KRİTİK SEÇİM: [Q] İtaat (Enjektörü kabul et) | [F] Red (Enjektörü it)");

        bool choiceMade = false;
        while (!choiceMade)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SadakatPuani += 8;
                Debug.Log("Ethan enjektörü kabul etti. Titreme durdu. Sadakat +8");
                choiceMade = true;
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                SadakatPuani -= 8;
                if (enjektor != null) enjektor.SetActive(false);
                Debug.Log("Ethan iğneyi itti ve Mira'nın elini tuttu. Mira şaşırdı.");
                yield return new WaitForSeconds(1f);
                Debug.Log("<Victor>: 'Mira, onu test odasına götür!'");
                choiceMade = true;
            }
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine(FinalTransition());
    }

    IEnumerator FinalTransition()
    {
        playerMovement.canMove = false;
        if (finalDoorPoint != null) playerTransform.position = finalDoorPoint.position;
        Debug.Log("Mira, Ethan'ı kapıdan içeri sokar. Kapı kapanır. EKRAN KARARDI.");

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(nextSceneName);
    }
}