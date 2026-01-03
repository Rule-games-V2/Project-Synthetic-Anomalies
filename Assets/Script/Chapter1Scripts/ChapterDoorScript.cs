using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ChapterDoorScript : MonoBehaviour
{
    public GameObject TeleportLoc;
    public Transform TeleportLocTransform;
    public GameObject player;
    public Transform playerTransform;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Devam Et [E]");
            StartCoroutine(WaitPressE());

        }
    }

    public IEnumerator WaitPressE()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        yield return new WaitForSeconds(1.5f);
        playerTransform.position = TeleportLocTransform.position;
    }
}