using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Damage")]
    public int damage = 25;

    [Header("Hit detection")]
    public Transform hitPoint;
    public float hitRadius = 1.2f;
    public LayerMask enemyLayer;

    [Header("Anti multi-hit")]
    public float hitCoolDown = 0.2f;
    private float lastHitTime = -999f;

    [Header("DEBUG")]
    public KeyCode debugAttackKey = KeyCode.T;
    public bool enableDebugAttack = true;

    public void Update()
    {
        if (!enableDebugAttack) return;

        if (Input.GetKeyDown(debugAttackKey))
        {
            Debug.Log("T pressed");
            DealDamage();
        }
    }
    public void DealDamage()
    {
        if (Time.time < lastHitTime + hitCoolDown) return;
        lastHitTime = Time.time;

        if (hitPoint == null )
        {
            Debug.LogWarning("Brak hitPoint w PlayerAttack");
            return;
        }

        Collider[] hits = Physics.OverlapSphere(hitPoint.position, hitRadius, enemyLayer);

        foreach (var h in hits)
        {
            EnemyHealth hp = h.GetComponentInParent<EnemyHealth>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (hitPoint == null) return;
        Gizmos.DrawWireSphere(hitPoint.position, hitRadius);
    }
}