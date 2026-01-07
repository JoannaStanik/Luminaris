using UnityEngine;

public class LuxBarrier : MonoBehaviour
{
    [Header("Ustawienia bariery")]
    public float lifetime = 3f;
    public float forwardOffset = 1.5f;
    public float heightOffset = 1.0f;

    [Header("HP bariery")]
    public int maxHits = 2;
    private int hitsLeft;

    void Start()
    {
        hitsLeft = maxHits;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Transform p = player.transform;

            Vector3 pos = p.position + p.forward * forwardOffset;
            pos.y += heightOffset;

            transform.position = pos;

            transform.rotation = Quaternion.LookRotation(p.forward, Vector3.up);
        }

        Destroy(gameObject, lifetime);
    }

    public void TakeHit()
    {
        if (maxHits <= 0) return;

        hitsLeft--;
        if (hitsLeft <= 0)
        {
            Destroy(gameObject);
        }
    }
}