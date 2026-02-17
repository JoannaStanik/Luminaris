using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellCooldownUI : MonoBehaviour
{
    [Header("Refs")]
    public SpellManager spellManager;

    [Header("UI")]
    public Image cooldownOverlay;
    public TMP_Text cooldownText;

    [Header("Which spell?")]
    public SpellType spellType;

    void Update()
    {
        if (spellManager == null) return;

        float remaining = spellManager.GetCooldownRemaining(spellType);
        float duration = spellManager.GetCooldownDuration(spellType);

        if (duration <= 0f)
        {
            cooldownOverlay.fillAmount = 0f;
            if (cooldownText) cooldownText.text = "";
            return;
        }

        float t = Mathf.Clamp01(remaining / duration);
        cooldownOverlay.fillAmount = t;

        if (cooldownText)
            cooldownText.text = remaining > 0.01f ? Mathf.CeilToInt(remaining).ToString() : "";
    }
}
