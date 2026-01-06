using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseTest : MonoBehaviour
{
    public Transform player;
    public float repathInterval = 0.25f;

    NavMeshAgent agent;
    float t;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!agent.isOnNavMesh || player == null) return;

        t -= Time.deltaTime;
        if (t <= 0f)
        {
            bool ok = agent.SetDestination(player.position);
            // Debug: zobacz w konsoli czy path siê ustawia
            // Debug.Log($"SetDestination {ok}, hasPath={agent.hasPath}, status={agent.pathStatus}");
            t = repathInterval;
        }
    }
}
