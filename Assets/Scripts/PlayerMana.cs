using System;
using System.Collections.Specialized;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [Header("Mana")]
    public float maxMana = 100f;
    public float currentMana = 100f;

    [Header("Regen")]
    public bool regenEnabled = true;
    public float regenPerSecond = 5f;

    public event Action<float, float> OnManaChanged;

    private void Start()
    {
        currentMana = maxMana;
        Notify();
    }

    private void Update()
    {
        if (!regenEnabled) return;

        if (currentMana < maxMana)
        {
            currentMana = Mathf.Min(maxMana, currentMana + regenPerSecond * Time.deltaTime);
            Notify();
        }
    }

    public bool HasMana(float cost) => currentMana >= cost;

    public bool TrySpend(float cost)
    {
        if (!HasMana(cost)) return false;
        currentMana -= cost;
        Notify();
        return true;
            
    }
    
    public void SetMaxMana(float newMax, bool refill = true)
    {
        maxMana = Mathf.Max(0f, newMax);
        if (refill) currentMana = maxMana;
        currentMana = Mathf.Clamp(currentMana, 0f, maxMana);
        Notify();
    }

    void Notify() => OnManaChanged?.Invoke(currentMana, maxMana);
}