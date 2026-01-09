using System.Collections;
using UnityEngine;

public class ChapterDoorScript : MonoBehaviour
{
    public Transform nightmareTeleportLoc; // Kabus baþlangýcý
    public Transform corridorTeleportLoc;  // Normal koridor baþlangýcý
    public Transform playerTransform;

    // Seçim verisini buradan kontrol edeceðiz
    public bool isHonest = false;
    public bool isLying = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Sadece bir seçim yapýldýysa etkileþime izin ver
            if (isHonest || isLying)
            {
                Debug.Log("Devam Et [E]");
                StartCoroutine(WaitPressE());
            }
        }
    }

    public IEnumerator WaitPressE()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

        if (isLying)
        {
            playerTransform.position = nightmareTeleportLoc.position;
        }
        else if (isHonest)
        {
            playerTransform.position = corridorTeleportLoc.position;
        }
    }
}