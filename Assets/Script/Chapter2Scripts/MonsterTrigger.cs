using UnityEngine;

public class MonsterTrigger : MonoBehaviour
{
    public MonsterController monster;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (monster != null)
            {
                monster.isMoving = true;
            }
        }
    }
}