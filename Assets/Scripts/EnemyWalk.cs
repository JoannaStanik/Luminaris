using UnityEngine;
using UnityEngine.AI;

public class EnemyWalk : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;
    float t;

    void Awake() => agent = GetComponent<NavMeshAgent>();

    void Start()
    {
        EnsureOnNav();
        agent.autoRepath = true;
        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.speed = Mathf.Max(agent.speed, 3.5f);
        agent.acceleration = Mathf.Max(agent.acceleration, 8f);
        agent.stoppingDistance = Mathf.Max(agent.stoppingDistance, 0.5f);
        agent.baseOffset = 0f;
    }

    void Update()
    {
        t -= Time.deltaTime;
        if (t <= 0f && target != null)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            t = 0.25f;
        }

        if (Time.frameCount % 60 == 0)
        {
        }
    }

    void EnsureOnNav()
    {
        if (agent.isOnNavMesh) return;

        if (NavMesh.SamplePosition(transform.position, out var hit, 5f, NavMesh.AllAreas))
        {
            Debug.LogWarning("[AgentProbe] Warp na najbli¿szy NavMesh");
            agent.Warp(hit.position);
        }
        else
        {
            Debug.LogError("[AgentProbe] Brak NavMesh w promieniu 5m! Zaznacz pod³ogê 'Navigation Static' i Rebake.");
        }
    }
}
