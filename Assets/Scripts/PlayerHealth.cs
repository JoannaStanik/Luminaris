using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;

    public ShieldController shield;

    void Start() => currentHealth = maxHealth;

    public void TakeDamage(int dmg)
    {
        if (shield != null && shield.IsActive) return;

        currentHealth -= dmg;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        Debug.Log("Player died");
    }
}