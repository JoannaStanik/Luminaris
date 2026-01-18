using UnityEngine;

public class SpellManager : MonoBehaviour
{
    [Header("Spawn")]
    public Transform wandTip;

    [Header("Prefaby FX")]
    public GameObject ignisPrefab;     // kula ognia
    public GameObject aerisPrefab;     // podmuch
    public GameObject luxShieldPrefab; // bariera
    public GameObject abrarioPrefab;   // aktywacja portalu

    [Header("Mana")]
    public PlayerMana playerMana;
    public float ignisCost = 25f, aerisCost = 15f, luxCost = 40f, abrarioCost = 60f;

    public PlayerMovement player;
    public ShieldController playerShield;
    public PlayerHealth playerHealth;

    void Awake()
    {
        if (playerMana == null) playerMana = GetComponent<PlayerMana>();
        if (player == null) player = GetComponent<PlayerMovement>();
        if (playerHealth == null) playerHealth = GetComponent<PlayerHealth>();
    }

    // Ustawienia punktu, z którego wychodz¹ zaklêcia
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

    public void CastIgnis()
    {
        if (playerHealth != null && playerHealth.IsDead) return;
        if (playerMana == null) return;
        if (!playerMana.TrySpend(ignisCost)) return;

        if (player != null) player.PlayerAttackAnimation();
        Spawn(ignisPrefab);
    }

    public void CastAeris()
    {
        if (playerHealth != null && playerHealth.IsDead) return;

        if (playerMana == null) return;
        if (!playerMana.TrySpend(aerisCost)) return;

        Spawn(aerisPrefab);
    }

    public void CastLux()
    {
        if (playerHealth != null && playerHealth.IsDead) return;

        if (playerMana == null) return;
        if (!playerMana.TrySpend(luxCost)) return;

        if (player != null) player.PlayerDefendAnimation();

        if (playerShield != null)
            playerShield.Activate();
        else
            Spawn(luxShieldPrefab);
    }

    public void CastAbrario()
    {
        if (playerHealth != null && playerHealth.IsDead) return;

        if (playerMana == null) return;
        if (!playerMana.TrySpend(abrarioCost)) return;

        Spawn(abrarioPrefab);
    }
}
