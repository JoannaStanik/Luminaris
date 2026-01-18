using UnityEngine;
using UnityEngine.AI;

public class EnemyWalk : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public string playerTag = "Player";

    [Header("Aggro")]
    public float detectionRadius = 250f;
    public float forgetRadius = 500f;
    public float startChaseDelayMax = 0.35f;

    [Header("Debug")]
    public bool debug = true;

    private NavMeshAgent agent;
    private float t;
    private bool isChasing;
    private float chaseDelay;

    void Awake() => agent = GetComponent<NavMeshAgent>();

    void Start()
    {
        if (target == null)
        {
            var p = GameObject.FindGameObjectWithTag(playerTag);
            if (p != null) target = p.transform;
        }

        if (forgetRadius < detectionRadius)
            forgetRadius = detectionRadius + 4f;

        EnsureOnNav();

        agent.autoRepath = true;
        agent.updatePosition = true;
        agent.updateRotation = true;

        if (debug)
            Debug.Log($"[EnemyWalk] START on {name}. Target={(target ? target.name : "NULL")}, isOnNavMesh={agent.isOnNavMesh}");
    }

    void Update()
    {
        if (target == null) return;

        // Dystans PO ZIEMI (ignorujemy Y)
        Vector3 a = transform.position; a.y = 0f;
        Vector3 b = target.position; b.y = 0f;
        float dist = Vector3.Distance(a, b);

        // Wejœcie w aggro / wyjœcie z aggro
        if (!isChasing && dist <= detectionRadius)
        {
            isChasing = true;
            chaseDelay = Random.Range(0f, startChaseDelayMax);
        }
        else if (isChasing && dist >= forgetRadius)
        {
            isChasing = false;
            agent.isStopped = true;
            return;
        }

        if (!isChasing)
        {
            agent.isStopped = true;
            return;
        }

        if (chaseDelay > 0f)
        {
            chaseDelay -= Time.deltaTime;
            agent.isStopped = true;
            return;
        }

        // Goni: ustaw destination co 0.25s
        t -= Time.deltaTime;
        if (t <= 0f)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            t = 0.25f;
        }
    }

    void EnsureOnNav()
    {
        if (agent.isOnNavMesh) return;

        if (NavMesh.SamplePosition(transform.position, out var hit, 5f, NavMesh.AllAreas))
        {
            Debug.LogWarning("[EnemyWalk] Warp na najbli¿szy NavMesh");
            agent.Warp(hit.position);
        }
        else
        {
            Debug.LogError("[EnemyWalk] Brak NavMesh w promieniu 5m! Zaznacz pod³ogê 'Navigation Static' i Rebake.");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, forgetRadius);

        if (target != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
