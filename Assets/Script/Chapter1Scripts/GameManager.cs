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
    public bool endNightmareSeq = false;

    [Header("Kondisyon Koridoru Ayarları")]
    public Transform miraTransform;
    public GameObject enjektor;
    public float corridorWalkSpeed = 2f;
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
                doorCollider.enabled = true;
                Debug.Log("<Victor>: Dürüstlük Bağımızı güçlendirir Ethan.");
                yield return new WaitForSeconds(1.3f);
                Debug.Log("<Victor>: Kapı açıldı, Mira Koridorda Bekliyor ");
                yield return new WaitForSeconds(1f);
                Debug.Log("Sadakat +8");
                SadakatPuani += 8;
                chosen = true;
                playerMovement.canMove = true;
                StartCoroutine(StartConditioningCorridor());
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
                yield return new WaitForSeconds(1f);
                Debug.Log("Sadakat -8");
                SadakatPuani -= 8;
                yield return new WaitForSeconds(1f);
                Debug.Log(" <== Yataga Yat");
                chosen = true;
                playerMovement.canMove = true;
                StartCoroutine(WaitForBedInteraction());
            }
            yield return null;
        }
    }

    IEnumerator WaitForBedInteraction()
    {
        yield return new WaitUntil(() => Vector2.Distance(playerTransform.position, bed.transform.position) < mesafe);
        StartCoroutine(InstantBedTeleport());
    }

    public IEnumerator InstantBedTeleport()
    {
        yield return new WaitUntil(() => endNightmareSeq == true);

        playerTransform.position = bed.transform.position;

        playerRb.simulated = true;
        playerRb.linearVelocity = Vector2.zero;
        playerMovement.canMove = true;
        doorCollider.enabled = true;
        bed.enabled = false;

        yield return new WaitForSeconds(5f);

        Debug.Log("Victor: Uyandığını görüyorum Ethan.");
        yield return new WaitForSeconds(3f);
        Debug.Log("Kapı açıldı.");
        StartCoroutine(StartConditioningCorridor());
    }

    IEnumerator StartConditioningCorridor()
    {
        playerMovement.moveSpeed = corridorWalkSpeed;
        yield return new WaitUntil(() => Vector2.Distance(playerTransform.position, miraTransform.position) < interactionDistance);

        playerMovement.canMove = false;
        playerRb.linearVelocity = Vector2.zero;

        Debug.Log("<Mira>: 'Kalbin çok hızlı atıyor Ethan.");
        yield return new WaitForSeconds(2f);
        Debug.Log(" Baban bunu almanı istiyor seni rahatlatacak.'");
        if (enjektor != null) enjektor.SetActive(true);

        yield return new WaitForSeconds(2.5f);
        Debug.Log("<Victor - Hoparlör>: 'Mira, vakit kaybediyoruz. Nabzını stabilize et ve bir sonraki odaya yönlendir.'");
        yield return new WaitForSeconds(2f);

        Debug.Log("KRİTİK SEÇİM: [Q] İtaat | [F] Red");

        bool choiceMade = false;
        while (!choiceMade)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SadakatPuani += 8;
                choiceMade = true;
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                SadakatPuani -= 8;
                if (enjektor != null) enjektor.SetActive(false);
                yield return new WaitForSeconds(2f);
                Debug.Log("<Victor>: 'Mira, onu test odasına götür!'");
                yield return new WaitForSeconds(2f);
                Debug.Log("<Mira>: 'Ah Ethan...'");
                choiceMade = true;
            }
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(FinalTransition());
    }

    IEnumerator FinalTransition()
    {
        playerMovement.canMove = false;
        playerRb.simulated = false;
        corridorWalkSpeed = 4f;

        while (Mathf.Abs(playerTransform.position.x - finalDoorPoint.position.x) > 0.05f)
        {
            float step = corridorWalkSpeed * Time.deltaTime;
            float newPlayerX = Mathf.MoveTowards(playerTransform.position.x, finalDoorPoint.position.x, step);
            playerTransform.position = new Vector3(newPlayerX, playerTransform.position.y, playerTransform.position.z);
            float newMiraX = Mathf.MoveTowards(miraTransform.position.x, finalDoorPoint.position.x + 0.6f, step);
            miraTransform.position = new Vector3(newMiraX, miraTransform.position.y, miraTransform.position.z);
            yield return null;
        }

        while (Vector2.Distance(playerTransform.position, finalDoorPoint.position) > 0.05f)
        {
            corridorWalkSpeed = 6f;
            playerTransform.position = Vector2.MoveTowards(playerTransform.position, finalDoorPoint.position, corridorWalkSpeed * Time.deltaTime);
            miraTransform.position = Vector2.MoveTowards(miraTransform.position, finalDoorPoint.position + new Vector3(0.4f, 0, 0), corridorWalkSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene(nextSceneName);
    }
}