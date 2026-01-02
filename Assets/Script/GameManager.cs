using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public EthanCraneBasics playerMovement;
    public Rigidbody2D playerRb;
    public Transform masa;
    public float mesafe = 1f;

    void Start()
    {
        Debug.Log("[S�STEM]: Tiz Ses Seviyesi %100. Ethan Uyan�yor.");
        playerRb.simulated = false;
        playerMovement.canMove = false;
        StartCoroutine(ControlTutorial());
    }

    IEnumerator ControlTutorial()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("(W) �leri");
        yield return new WaitForSeconds(3f);
        Debug.Log("(A) Sol");
        yield return new WaitForSeconds(3f);
        Debug.Log("(S) Geri");
        yield return new WaitForSeconds(3f);
        Debug.Log("(D) Sa�");

        playerRb.simulated = true;
        playerMovement.canMove = true;
        Debug.Log("�imdi A/D ile masaya git!");

        yield return new WaitUntil(() =>
            Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)
        );

        Debug.Log("Masaya do�ru ilerle...");

        yield return new WaitUntil(() =>
            Vector2.Distance(playerRb.position, masa.position) < mesafe
        );

        Debug.Log("");
    }
}
