using UnityEngine;
using TMPro;

public class KillCounterUI : MonoBehaviour
{
    public TextMeshProUGUI killsText;

    private void Update()
    {
        if (KillCounter.Instance == null)
        {
            killsText.text = "Przeciwnicy: 0/0";
            return;
        }

        killsText.text = $"Przeciwnicy: {KillCounter.Instance.Kills}/{KillCounter.Instance.killsRequired}";
    }
}