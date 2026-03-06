using UnityEngine;

public class KillCounter : MonoBehaviour
{
    public static KillCounter Instance { get; private set; }

    [Header("Prototype")]
    public int killsRequired = 4;

    public int Kills {  get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddKill(int amount = 1)
    {
        Kills += amount;
        Debug.Log($"Kills: {Kills}/{killsRequired}");
    }

    public bool HasEnoughKills() => Kills >= killsRequired;

    public void ResetKills()
    {
        Kills = 0;
    }
}