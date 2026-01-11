using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Vector2 moveDirection = Vector2.right;
    public float startSpeed = 5.0f;
    public float maxSpeed = 15.0f;
    public float acceleration = 0.5f;

    public bool isMoving = false;
    private float currentSpeed;

    void Start() { currentSpeed = startSpeed; }

    void Update()
    {
        if (isMoving)
        {
            if (currentSpeed < maxSpeed)
                currentSpeed += acceleration * Time.deltaTime;

            transform.position += (Vector3)(moveDirection * currentSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager gm = Object.FindFirstObjectByType<GameManager>();
            if (gm != null)
            {
                // KRÝTÝK EKSÝK BURASIYDI: Þalteri indiriyoruz!
                gm.endNightmareSeq = true;

                // Mevcut coroutine'i durdurup temizce baþtan baþlatýyoruz
                gm.StopAllCoroutines();
                gm.StartCoroutine(gm.InstantBedTeleport());
            }
            Destroy(gameObject);
        }
    }
}