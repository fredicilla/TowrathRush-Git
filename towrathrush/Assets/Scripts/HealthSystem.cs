using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    public float invincibilityDuration = 1f;
    
    [Header("Events")]
    public UnityEvent<int> onHealthChanged;
    public UnityEvent onDeath;
    public UnityEvent onDamaged;
    
    private int currentHealth;
    private bool isInvincible = false;
    private float invincibilityTimer = 0f;
    
    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }
    
    void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (isInvincible || currentHealth <= 0)
            return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        
        onDamaged?.Invoke();
        UpdateUI();
        
        if (currentHealth > 0)
        {
            isInvincible = true;
            invincibilityTimer = invincibilityDuration;
            Debug.Log($"Player damaged! Health: {currentHealth}/{maxHealth}");
        }
        else
        {
            Die();
        }
    }
    
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        UpdateUI();
        Debug.Log($"Player healed! Health: {currentHealth}/{maxHealth}");
    }
    
    void Die()
    {
        Debug.Log("Player died!");
        onDeath?.Invoke();
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
    }
    
    void UpdateUI()
    {
        onHealthChanged?.Invoke(currentHealth);
        
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealth(currentHealth, maxHealth);
        }
    }
    
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    
    public bool IsInvincible()
    {
        return isInvincible;
    }
}
