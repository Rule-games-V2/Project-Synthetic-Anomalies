using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    [Range(0, 1)]
    public float smoothSpeed = 1f;
    public Vector3 offset;

    void Start()
    {
        // Baþlangýçta manuel offset girilmemiþse otomatik ayarlanmasý
        if (offset == Vector3.zero) offset = new Vector3(0, 0, -10f);
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // fixedY deðiþkenini sildik, doðrudan target.position.y kullanýlmalý
            Vector3 targetPosition = target.position + offset;

            // Yumuþak takip
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        }
    }

    // Iþýnlanma durumunda kamerayý anýnda hedefe kilitlemek için bu fonksiyonu çaðýr
    public void SnapToTarget()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}