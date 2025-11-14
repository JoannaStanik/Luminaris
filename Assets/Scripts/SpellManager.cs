using UnityEngine;

public class SpellManager : MonoBehaviour
{
    [Header("Spawn")]
    public Transform wandTip;

    [Header("Prefaby FX")]
    public GameObject ignisPrefab;     // kula ognia
    public GameObject fulmenPrefab;    // ³añcuch b³yskawic / uderzenie
    public GameObject terraPrefab;     // kolce ziemi
    public GameObject aerisPrefab;     // podmuch
    public GameObject luxShieldPrefab; // bariera
    public GameObject curatioPrefab;   // leczenie
    public GameObject tempusPrefab;    // spowolnienie czasu
    public GameObject clarusPrefab;    // ujawnienie obiektów

    [Header("Mana (opcjonalnie)")]
    public float mana = 100f;
    public float ignisCost = 10f, fulmenCost = 12f, terraCost = 8f, aerisCost = 6f,
                 luxCost = 10f, curatioCost = 10f, tempusCost = 15f, clarusCost = 5f;

    bool HasMana(float cost) => mana >= cost;
    void Spend(float cost) => mana -= cost;

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

    // --- Zaklêcia ---
    public void CastIgnis() { if (HasMana(ignisCost)) { Spawn(ignisPrefab); Spend(ignisCost); } }
    public void CastFulmen() { if (HasMana(fulmenCost)) { Spawn(fulmenPrefab); Spend(fulmenCost); } }
    public void CastTerra() { if (HasMana(terraCost)) { Spawn(terraPrefab); Spend(terraCost); } }
    public void CastAeris() { if (HasMana(aerisCost)) { Spawn(aerisPrefab); Spend(aerisCost); } }
    public void CastLux() { if (HasMana(luxCost)) { Spawn(luxShieldPrefab); Spend(luxCost); } }
    public void CastCuratio() { if (HasMana(curatioCost)) { Spawn(curatioPrefab); Spend(curatioCost); } }
    public void CastTempus() { if (HasMana(tempusCost)) { Spawn(tempusPrefab); Spend(tempusCost); } }
    public void CastClarus() { if (HasMana(clarusCost)) { Spawn(clarusPrefab); Spend(clarusCost); } }
}
