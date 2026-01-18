using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damageToPlayer = 20;     // wartoœæ zabierana z ¿ycia gracza
    public float attackCooldown = 2f;   // przerwa miêdzy atakami przeciwnika

    private float lastAttackTime;

    private void OnTriggerStay(Collider other)
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        if (other.CompareTag("Shield"))
        {
            LuxBarrier barrier = other.GetComponent<LuxBarrier>();
            if (barrier != null)
            {
                barrier.TakeHit();
                lastAttackTime = Time.time;
                return;
            }
        }

        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damageToPlayer);
                lastAttackTime += Time.time;
            }
        }
    }
}