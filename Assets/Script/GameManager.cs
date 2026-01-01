using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{    
    void Start()
    {
        Debug.Log("[SÝSTEM]: Tiz Ses Seviyesi %100. Ethan Uyanýyor.");
    }

    void Update()
    {
        
    }

    IEnumerator ControlTutorial()
    {
        yield return new WaitForSeconds(6f);
        Debug.Log("(W) Ýleri");
        yield return new WaitForSeconds(4f);
        Debug.Log("(A) Sol");
        yield return new WaitForSeconds(4f);
        Debug.Log("(S) Geri");
        yield return new WaitForSeconds(4f);
        Debug.Log("(D) Sað");
    }
}
