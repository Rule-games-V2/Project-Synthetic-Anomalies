using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class FinalChapterMaster : MonoBehaviour
{
    [Header("Ayarlar")]
    public float grabRange = 2f;
    public float speedReduction = 0.85f;
    public LayerMask blockLayer;
    public LayerMask slotLayer;
    public Transform resetPoint;
    public string nextSceneName;
    public EthanCraneBasics player;

    [Header("Kapý")]
    public BoxCollider2D doorCollider;
    private bool isNearDoor = false;
    private bool isMessageShown = false;

    [Header("Seçim")]
    public bool isChoicePending = false;
    private bool choiceMessageSent = false;
    public bool hasChosenPersist = false;
    private bool hasChoiceTriggered = false; // TEK SEFERLÝK TETÝKLEYÝCÝ

    [Header("Durum")]
    public GameObject grabbedBlock;
    public bool isGrabbing = false;
    public static int placedBlocksCount = 0;

    private float originalSpeed;
    private EthanCraneBasics movementScript;
    private Dictionary<GameObject, Vector3> blockHomePositions = new Dictionary<GameObject, Vector3>();

    [Header("Puan")]
    public GameManager SpManager;

    void Start()
    {
        movementScript = GetComponent<EthanCraneBasics>();
        if (movementScript != null) originalSpeed = movementScript.moveSpeed;
        if (doorCollider != null) doorCollider.enabled = false;

        placedBlocksCount = 0;
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject block in blocks) blockHomePositions[block] = block.transform.position;

        Debug.Log("Victor: Ethan, hücreleri yuvalarýna yerleþtir ve testi tamamla. Gözüm üzerinde.");
    }

    void Update()
    {
        if (doorCollider != null && doorCollider.enabled && isNearDoor)
        {
            if (!isMessageShown)
            {
                Debug.Log("Çýk [E]");
                isMessageShown = true;
            }
            if (Input.GetKeyDown(KeyCode.E)) StartCoroutine(LoadNextChapterDelay());
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isGrabbing) TryPlaceBlockInSlot();
            else if (!isChoicePending) TryGrabBlock();
        }

        if (isChoicePending)
        {
            if (!choiceMessageSent)
            {
                StartCoroutine(VictorChoiceSequence());
                choiceMessageSent = true;
            }

            if (Input.GetKeyDown(KeyCode.Q)) { isChoicePending = false; hasChosenPersist = true; }
            if (Input.GetKeyDown(KeyCode.F)) { isChoicePending = false; FinalFailure(); }
        }

        if (isGrabbing && grabbedBlock != null)
        {
            float dir = transform.localScale.x > 0 ? 0.6f : -0.6f;
            grabbedBlock.transform.position = transform.position + new Vector3(dir, 0, 0);
        }
    }

    IEnumerator VictorChoiceSequence()
    {
        player.canMove = false;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        Debug.Log("Victor: Ethan, bu son hücrenin enerjisi dengesiz. Eðer þimdi býrakmazsan ufak bir þok alabilirsin. Ama testi bitirirsen kusursuz bir koordinasyon sergilemiþ olacaksýn. Devam edecekmisin?");
        yield return new WaitForSeconds(3f);
        Debug.Log("[F] Býrak ya da Devam Et");
        yield return new WaitForSeconds(2f);
        player.canMove = true;
    }

    void TryGrabBlock()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, grabRange, blockLayer);
        if (hit != null && hit.CompareTag("Block"))
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb == null || rb.bodyType == RigidbodyType2D.Static) return;

            grabbedBlock = hit.gameObject;
            isGrabbing = true;
            if (movementScript != null) movementScript.moveSpeed = originalSpeed * speedReduction;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;

            if (placedBlocksCount == 2 && !hasChoiceTriggered)
            {
                isChoicePending = true;
                choiceMessageSent = false;
                hasChoiceTriggered = true;
                FinalChapterLaser[] lasers = Object.FindObjectsByType<FinalChapterLaser>(FindObjectsSortMode.None);
                foreach (var l in lasers) l.speed = Mathf.Max(0.2f, l.speed - 0.5f);
            }
        }
    }

    void TryPlaceBlockInSlot()
    {
        Collider2D slotHit = Physics2D.OverlapCircle(grabbedBlock.transform.position, 1.2f, slotLayer);
        if (slotHit != null)
        {
            Slot slotScript = slotHit.GetComponent<Slot>();
            if (slotScript != null && !slotScript.isOccupied)
            {
                if (hasChosenPersist && placedBlocksCount == 2) FinalSuccess(slotHit.gameObject);
                else
                {
                    LockBlockToSlot(slotHit.gameObject);
                    if (placedBlocksCount == 3) StartCoroutine(FinishLevelRoutine());
                }
                return;
            }
        }
        ReleaseBlockToGround();
    }

    void FinalSuccess(GameObject targetSlot)
    {
        hasChosenPersist = false;
        if (SpManager != null) { SpManager.SadakatPuani += 8; Debug.Log("Sadakat +8" + SpManager.SadakatPuani); }
        LockBlockToSlot(targetSlot);
        Debug.Log("Victor: Sýnýrlarýný aþtýn Ethan, aferin.");
        StartCoroutine(FinishLevelRoutine());
    }

    void FinalFailure()
    {
        if (SpManager != null) { SpManager.SadakatPuani -= 8; Debug.Log("Sadakat -8" + SpManager.SadakatPuani); }
        Debug.Log("Victor: Süreci tamamlayamadýn Ethan. Koordinasyonun korkuyla kýsýtlanýyor.");
        ReleaseBlockToGround();
        StartCoroutine(FinishLevelRoutine());
    }

    void ReleaseBlockToGround()
    {
        if (grabbedBlock != null)
        {
            Rigidbody2D rb = grabbedBlock.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
            ResetGrabStatus();
        }
    }

    IEnumerator FinishLevelRoutine()
    {
        FinalChapterLaser[] lasers = Object.FindObjectsByType<FinalChapterLaser>(FindObjectsSortMode.None);
        foreach (var l in lasers) l.enabled = false;
        yield return new WaitForSeconds(2f);
        foreach (var l in lasers) l.gameObject.SetActive(false);
        if (doorCollider != null) doorCollider.enabled = true;
        Debug.Log("Victor: Test bitti. Çýkabilirsin.");
    }

    IEnumerator LoadNextChapterDelay()
    {
        yield return new WaitForSeconds(4f);
        if (!string.IsNullOrEmpty(nextSceneName)) SceneManager.LoadScene(nextSceneName);
    }

    void LockBlockToSlot(GameObject slotObj)
    {
        grabbedBlock.transform.position = slotObj.transform.position;
        grabbedBlock.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        slotObj.GetComponent<Slot>().isOccupied = true;
        placedBlocksCount++;
        ResetGrabStatus();
    }

    void ResetGrabStatus()
    {
        grabbedBlock = null; isGrabbing = false;
        if (movementScript != null) movementScript.moveSpeed = originalSpeed;
    }

    public void HandleLaserHit()
    {
        if (hasChosenPersist) return;
        isChoicePending = false;

        if (grabbedBlock != null)
        {
            grabbedBlock.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            if (blockHomePositions.ContainsKey(grabbedBlock)) grabbedBlock.transform.position = blockHomePositions[grabbedBlock];
            ResetGrabStatus();
        }
        if (resetPoint != null) transform.position = resetPoint.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser")) HandleLaserHit();
        if (other.CompareTag("Exit")) isNearDoor = true;
        if (Input.GetKey(KeyCode.E))
        {
            player.canMove = false;

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Exit"))
        {
            isNearDoor = false;
            isMessageShown = false;
        }
    }
}