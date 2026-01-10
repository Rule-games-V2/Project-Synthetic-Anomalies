using UnityEngine;

public class MonsterDestroyTrigger : MonoBehaviour
{
    public GameObject Monster;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (Monster.CompareTag("other"))
        {
            Destroy(Monster);
        }
    }
}
