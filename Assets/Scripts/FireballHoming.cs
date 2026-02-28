using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class FireballHoming : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 30f;
    public float turnRate = 360f;
    public float lifeTime = 4f;

    [Header("Targeting")]
    public float acquireRadius = 25f;
    public LayerMask enemyLayer;
    public Transform target;

    [Header("Damage")]
    public int damage = 25;

    [Header("Ignore Player")]
    public Transform ownerRoot;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);

        if (ownerRoot == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) ownerRoot = player.transform;
        }

        if (ownerRoot != null)
        {
            var myCol = GetComponent<Collider>();
            foreach (var c in ownerRoot.GetComponentsInChildren<Collider>())
                Physics.IgnoreCollision(myCol, c);
        }

        if (target == null)
            target = AcquireTarget();

        rb.linearVelocity = transform.forward * speed;
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
            return;
        }

        Vector3 toTarget = (target.position - transform.position);
        if (toTarget.sqrMagnitude < 0.01f) return;

        Vector3 desiredDir = toTarget.normalized;

        Vector3 currentDir = rb.linearVelocity.sqrMagnitude > 0.01f ? rb.linearVelocity.normalized : transform.forward;
        float maxRadians = turnRate * Mathf.Deg2Rad * Time.fixedDeltaTime;

        Vector3 newDir = Vector3.RotateTowards(currentDir, desiredDir, maxRadians, 0f);

        rb.linearVelocity = newDir * speed;
        transform.forward = newDir;
    }

    private Transform AcquireTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, acquireRadius, enemyLayer);

        float best = float.PositiveInfinity;
        Transform bestT = null;

        foreach (var h in hits)
        {
            var hp = h.GetComponentInParent<EnemyHealth>();
            if (hp == null) continue;

            float d = (hp.transform.position - transform.position).sqrMagnitude;
            if (d < best)
            {
                best = d;
                bestT = hp.transform;
            }
        }

        return bestT;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[FIREBALL] Trigger enter: {other.name} | layer={LayerMask.LayerToName(other.gameObject.layer)} | tag={other.tag}");

        var hp = other.GetComponentInParent<EnemyHealth>();
        Debug.Log(hp != null ? $"[FIREBALL] EnemyHealth FOUND on: {hp.name}" : "[FIREBALL] EnemyHealth NOT found in parents");

        if (hp != null)
        {
            hp.TakeDamage(damage);
            Debug.Log("[FIREBALL] Damage applied");
            Destroy(gameObject);
        }
    }
}