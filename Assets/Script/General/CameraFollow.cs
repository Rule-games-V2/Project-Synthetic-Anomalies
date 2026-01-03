using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;         
    [Range(0, 1)]
    public float smoothSpeed = 0.125f; 
    public Vector3 offset;           

    private float fixedY;     

    void Start()
    {
        fixedY = transform.position.y;
        if (offset.z == 0) offset.z = -10f;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x + offset.x, fixedY + offset.y, offset.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        }
    }
}