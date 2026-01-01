using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public EthanCraneBasics playerMovement;
    public Rigidbody2D playerRb;
    public Transform masa;
    public float mesafe = 1f;

    void Start()
    {
        Debug.Log("[SÝSTEM]: Tiz Ses Seviyesi %100. Ethan Uyanýyor.");
        playerRb.simulated = false;
        playerMovement.canMove = false;
        StartCoroutine(ControlTutorial());
    }

    IEnumerator ControlTutorial()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("(W) Ýleri");
        yield return new WaitForSeconds(3f);
        Debug.Log("(A) Sol");
        yield return new WaitForSeconds(3f);
        Debug.Log("(S) Geri");
        yield return new WaitForSeconds(3f);
        Debug.Log("(D) Sað");

        playerRb.simulated = true;
        playerMovement.canMove = true;
        Debug.Log("Þimdi A/D ile masaya git!");

        yield return new WaitUntil(() =>
            Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)
        );

        Debug.Log("Masaya doðru ilerle...");

        yield return new WaitUntil(() =>
            Vector2.Distance(playerRb.position, masa.position) < mesafe
        );

        Debug.Log("Masaya ulaþtýn! Tutorial bitti.");
    }
}
