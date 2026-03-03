using UnityEngine;

public class LuxBarrier : MonoBehaviour
{
    [Header("HP bariery")]
    public int maxHits = 2;

    private int hitsLeft;
    private ShieldController owner;

    private void OnEnable()
    {
        hitsLeft = maxHits;
    }

    public void SetOwner(ShieldController shieldOwner)
    {
        owner = shieldOwner;
    }

    public void ResetHits(int newMaxHits)
    {
        maxHits = newMaxHits;
        hitsLeft = maxHits;
    }

    public void TakeHit()
    {
        if (maxHits <= 0) return;

        hitsLeft--;
        if (hitsLeft <= 0)
        {
            if (owner != null)
                owner.OnBarrierBroken();
            else
                Destroy(gameObject);
        }
    }
}