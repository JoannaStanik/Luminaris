using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public GameObject shieldPrefab;
    public Vector3 worldOffset = Vector3.zero;

    public float lifetime = 3f;
    public int maxHits = 2;

    private GameObject currentShieldGO;
    private LuxBarrier currentBarrier;
    private float endTime;

    public bool IsActive => currentShieldGO != null;

    public void Activate()
    {
        if (shieldPrefab == null)
        {
            Debug.LogWarning("ShieldController: Brak shieldPrefab!");
            return;
        }

        if (currentShieldGO != null)
        {
            endTime = Time.time + lifetime;
            if (currentBarrier != null) currentBarrier.ResetHits(maxHits);
            return;
        }

        currentShieldGO = Instantiate(shieldPrefab, transform);
        currentBarrier = currentShieldGO.GetComponent<LuxBarrier>();
        if (currentBarrier != null)
        {
            currentBarrier.SetOwner(this);
            currentBarrier.ResetHits(maxHits);
        }

        RepositionShield();

        endTime = Time.time + lifetime;
    }

    private void Update()
    {
        if (currentShieldGO == null) return;

        RepositionShield();

        if (Time.time >= endTime)
            Deactivate();
    }

    private void RepositionShield()
    {
        var cc = GetComponent<CharacterController>();
        if (cc != null)
        {
            Vector3 center = transform.TransformPoint(cc.center);
            currentShieldGO.transform.position = center + worldOffset;
            return;
        }

        var col = GetComponent<CapsuleCollider>();
        if (col != null)
        {
            Vector3 center = transform.TransformPoint(col.center);
            currentShieldGO.transform.position = center + worldOffset;
            return;
        }

        currentShieldGO.transform.position = transform.position + Vector3.up * 1.0f + worldOffset;
    }

    public void Deactivate()
    {
        if (currentShieldGO != null)
            Destroy(currentShieldGO);

        currentShieldGO = null;
        currentBarrier = null;
    }

    public void OnBarrierBroken()
    {
        Deactivate();
    }
}