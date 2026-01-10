using UnityEngine;

public class MonsterDestroyTrigger : MonoBehaviour
{
    public GameObject monster;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == monster)
        {
            Destroy(other.gameObject);
        }
    }
}