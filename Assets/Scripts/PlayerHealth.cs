using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;

    public ShieldController shield;

    [Header("Animator")]
    public Animator animator;
    public float getHitCooldown = 0.25f;
    private float lastHitAnimTime = -999f;

    public event Action<int, int> OnHealthChanged;

    public int CurrentHealth => currentHealth;
    public bool IsDead { get; private set; }
    public UIManager uiManager;
    public float gameOverDelay = 1.2f;

    void Start()
    {
        currentHealth = maxHealth;
        IsDead = false;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        Debug.Log("PlayerHealth animator: " + (animator ? animator.name : "NULL"));

        NotifyHealthChanged();

        if (uiManager == null) uiManager = GetComponentInChildren<UIManager>(true);
    }

    public void TakeDamage(int dmg)
    {
        if (IsDead) return;
        if (shield != null && shield.IsActive) return;

        currentHealth -= dmg;
        if (currentHealth < 0) currentHealth = 0;

        NotifyHealthChanged();

        if (animator != null && Time.time >= lastHitAnimTime + getHitCooldown)
        {
            animator.ResetTrigger("GetHit");
            animator.SetTrigger("GetHit");
            lastHitAnimTime = Time.time;
        }

        if (currentHealth <= 0) Die();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        NotifyHealthChanged();
    }

    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Die()
    {
        if (IsDead) return;
        IsDead = true;

        if (animator != null)
        {
            animator.SetFloat("Speed", 0f);
            animator.SetBool("IsRunning", false);

            animator.ResetTrigger("Attack");
            animator.ResetTrigger("Defend");
            animator.ResetTrigger("GetHit");

            animator.ResetTrigger("Die");
            animator.SetTrigger("Die");

            animator.Play("Die", 0, 0f);
        }

        var movement = GetComponent<PlayerMovement>();
        if (movement != null) movement.enabled = false;

        var attack = GetComponent<PlayerAttack>();
        if (attack != null) attack.enabled = false;

        Debug.Log("Player died");

        StartCoroutine(ShowGameOverRealTime());
    }

    public void ResetPlayer()
    {
        IsDead = false;
        currentHealth = maxHealth;

        NotifyHealthChanged();

        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }

        var movement = GetComponent <PlayerMovement>();
        if (movement != null) movement.enabled = true;

        var attack = GetComponent<PlayerAttack>();
        if (attack != null) attack.enabled = true;
    }

    private IEnumerator ShowGameOverRealTime()
    {
        yield return new WaitForSecondsRealtime(gameOverDelay);

        if (uiManager == null) uiManager = FindObjectOfType<UIManager>(true);

        if (uiManager != null)
            uiManager.ShowGameOver();
        else
            Debug.LogError("UIManager nie znaleziony - GameOver nie mo¿e siê pokazaæ.");
    }
}
