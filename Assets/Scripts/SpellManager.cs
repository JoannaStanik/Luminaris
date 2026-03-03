using UnityEngine;

public enum SpellType { Ignis, Kuratio, Lux, Abrario } // spells

public class SpellManager : MonoBehaviour
{
    [Header("Spawn")]
    public Transform wandTip;
    public Camera mainCam;

    [Header("Prefaby FX")]
    public GameObject ignisPrefab;     // kula ognia
    public GameObject kuratioPrefab;     // leczenie
    public GameObject luxShieldPrefab; // bariera
    public GameObject abrarioPrefab;   // aktywacja portalu

    [Header("Curatio VFX (circle)")]
    public float kuratioGroundOffsetY = 0.02f;
    public LayerMask groundMask = ~0;
    public bool kuratioFollowPlayer = false;
    public bool kuratioAlignToGround = true;
    public float kuratioRaycastHeight = 2f;
    public float kuratioRaycastDistance = 50f;
    public float kuratioVfxLifetime = 2.5f;

    [Header("Mana")]
    public PlayerMana playerMana;
    public float ignisCost = 25f, kuratioCost = 50f, luxCost = 40f, abrarioCost = 60f;

    [Header("Portal")]
    public PortalAbrario portalInScene;

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
        if (mainCam == null) mainCam = Camera.main;
        if (playerMana == null) playerMana = GetComponent<PlayerMana>();
        if (player == null) player = GetComponent<PlayerMovement>();
        if (playerHealth == null) playerHealth = GetComponent<PlayerHealth>();
        if (playerShield == null) playerShield = GetComponent<ShieldController>();
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
    // Kuratio spawn na ziemi pod graczem
    // -----------------------------
    private Transform GetCaster()
    {
        if (player != null) return player.transform;
        if (playerHealth != null) return playerHealth.transform;
        return transform;
    }
    private void SpawnKuratioOnGround()
    {
        if (kuratioPrefab == null)
        {
            Debug.LogWarning("Kuratio prefab nie jest przypiêty.");
            return;
        }

        Transform caster = GetCaster();

        Vector3 start = caster.position + Vector3.up * kuratioRaycastHeight;

        if (Physics.Raycast(start, Vector3.down, out RaycastHit hit, kuratioRaycastDistance, groundMask))
        {
            Vector3 spawnPos = hit.point + Vector3.up * kuratioGroundOffsetY;

            Quaternion rot = Quaternion.identity;
            if (kuratioAlignToGround)
                rot = Quaternion.FromToRotation(Vector3.up, hit.normal);

            GameObject vfx = Instantiate(kuratioPrefab, spawnPos, rot);
            Destroy(vfx, kuratioVfxLifetime);

            if (kuratioFollowPlayer && vfx != null)
                vfx.transform.SetParent(caster, true);
        }
        else
        {
            Vector3 pos = caster.position;
            pos.y += kuratioGroundOffsetY;
            Instantiate(kuratioPrefab, pos, Quaternion.identity);
        }
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

        Vector3 pos = wandTip != null ? wandTip.position : transform.position + transform.forward * 0.5f;

        Quaternion rot;
        Camera cam = Camera.main;
        if (cam != null)
            rot = Quaternion.LookRotation(cam.transform.forward, Vector3.up);
        else
            rot = wandTip != null ? wandTip.rotation : transform.rotation;

        GameObject go = Instantiate(ignisPrefab, pos, rot);

        if (go != null)
        {
            var homing = go.GetComponent<FireballHoming>();
            if (homing != null)
                homing.ownerRoot = transform.root;
        }

        SetOnCooldown(SpellType.Ignis);
    }

    public void CastKuratio()
    {
        if (playerHealth != null && playerHealth.IsDead) return;
        if (!IsReady(SpellType.Kuratio)) return;

        if (playerMana == null) return;
        if (playerHealth != null && playerHealth.CurrentHealth >= playerHealth.maxHealth) return;
        if (!playerMana.TrySpend(kuratioCost)) return;

        SpawnKuratioOnGround();

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

        if (portalInScene != null)
        {
            Debug.Log("Casting Abrario -> activating portal: " +  portalInScene.name);
            portalInScene.ActivatePortal();
        }
        else
            Debug.LogWarning("SpellManager: portalInScene nie jest podpiêty!");

        SetOnCooldown(SpellType.Abrario);
    }

}
