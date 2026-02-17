using UnityEngine;

public enum SpellType { Ignis, Kuratio, Lux, Abrario } // spells

public class SpellManager : MonoBehaviour
{
    [Header("Spawn")]
    public Transform wandTip;

    [Header("Prefaby FX")]
    public GameObject ignisPrefab;     // kula ognia
    public GameObject kuratioPrefab;     // leczenie
    public GameObject luxShieldPrefab; // bariera
    public GameObject abrarioPrefab;   // aktywacja portalu

    [Header("Mana")]
    public PlayerMana playerMana;
    public float ignisCost = 25f, kuratioCost = 50f, luxCost = 40f, abrarioCost = 60f;

    [Header("Cooldowns (sekundy)")]
    public float ignisCooldown = 2.0f;
    public float kuratioCooldown = 3.0f;
    public float luxCooldown = 8.0f;
    public float abrarioCooldown = 1.0f;

    [Header("Refs")]
    public PlayerMovement player;
    public ShieldController playerShield;
    public PlayerHealth playerHealth;

    // ready times
    private float ignisReadyTime;
    private float kuratioReadyTime;
    private float luxReadyTime;
    private float abrarioReadyTime;

    void Awake()
    {
        if (playerMana == null) playerMana = GetComponent<PlayerMana>();
        if (player == null) player = GetComponent<PlayerMovement>();
        if (playerHealth == null) playerHealth = GetComponent<PlayerHealth>();
    }

    // -----------------------------
    // COOLDOWN API dla SpellCooldownUI
    // -----------------------------
    public float GetCooldownDuration(SpellType t)
    {
        return t switch
        {
            SpellType.Ignis => ignisCooldown,
            SpellType.Kuratio => kuratioCooldown,
            SpellType.Lux => luxCooldown,
            SpellType.Abrario => abrarioCooldown,
            _ => 0f
        };
    }

    public float GetCooldownRemaining(SpellType t)
    {
        float readyTime = t switch
        {
            SpellType.Ignis => ignisReadyTime,
            SpellType.Kuratio => kuratioReadyTime,
            SpellType.Lux => luxReadyTime,
            SpellType.Abrario => abrarioReadyTime,
            _ => 0f
        };

        return Mathf.Max(0f, readyTime - Time.time);
    }

    public bool IsReady(SpellType t) => GetCooldownRemaining(t) <= 0.0001f;

    private void SetOnCooldown(SpellType t)
    {
        float ready = Time.time + GetCooldownDuration(t);
        switch (t)
        {
            case SpellType.Ignis: ignisReadyTime = ready; break;
            case SpellType.Kuratio: kuratioReadyTime = ready; break;
            case SpellType.Lux: luxReadyTime = ready; break;
            case SpellType.Abrario: abrarioReadyTime = ready; break;
        }
    }

    // -----------------------------
    // Spawn
    // -----------------------------
    void Spawn(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("Brak przypisanego prefabu FX.");
            return;
        }

        var pos = wandTip != null ? wandTip.position : transform.position + transform.forward * 0.5f;
        var rot = wandTip != null ? wandTip.rotation : Quaternion.identity;
        Instantiate(prefab, pos, rot);
    }

    // -----------------------------
    // Casty (z cooldownem + man¹)
    // -----------------------------
    public void CastIgnis()
    {
        if (playerHealth != null && playerHealth.IsDead) return;
        if (!IsReady(SpellType.Ignis)) return;

        if (playerMana == null) return;
        if (!playerMana.TrySpend(ignisCost)) return;

        if (player != null) player.PlayerAttackAnimation();
        Spawn(ignisPrefab);

        SetOnCooldown(SpellType.Ignis);
    }

    public void CastKuratio()
    {
        if (playerHealth != null && playerHealth.IsDead) return;
        if (!IsReady(SpellType.Kuratio)) return;

        if (playerMana == null) return;
        if (playerHealth.CurrentHealth >= playerHealth.maxHealth) return;
        if (!playerMana.TrySpend(kuratioCost)) return;

        Spawn(kuratioPrefab);

        if (playerHealth != null)
            playerHealth.Heal(30);

        SetOnCooldown(SpellType.Kuratio);
    }

    public void CastLux()
    {
        if (playerHealth != null && playerHealth.IsDead) return;
        if (!IsReady(SpellType.Lux)) return;

        if (playerMana == null) return;
        if (!playerMana.TrySpend(luxCost)) return;

        if (player != null) player.PlayerDefendAnimation();

        if (playerShield != null)
            playerShield.Activate();
        else
            Spawn(luxShieldPrefab);

        SetOnCooldown(SpellType.Lux);
    }

    public void CastAbrario()
    {
        if (playerHealth != null && playerHealth.IsDead) return;
        if (!IsReady(SpellType.Abrario)) return;

        if (playerMana == null) return;
        if (!playerMana.TrySpend(abrarioCost)) return;

        Spawn(abrarioPrefab);

        SetOnCooldown(SpellType.Abrario);
    }
}
