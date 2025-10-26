using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int maxHealth = 2;
    public int damageToPlayer = 1;
    public float moveSpeed = 5f;
    public int scoreValue = 50;
    
    [Header("Combat")]
    public float attackRange = 2f;
    
    private int currentHealth;
    private Transform playerTransform;
    private bool hasAttacked = false;
    private Rigidbody rb;
    
    void Start()
    {
        currentHealth = maxHealth;
        
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }
    
    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.isGameActive)
            return;
        
        MoveTowardPlayer();
        CheckAttack();
    }
    
    void MoveTowardPlayer()
    {
        if (playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction.y = 0;
            
            transform.position += direction * moveSpeed * Time.deltaTime;
            
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
    
    void CheckAttack()
    {
        if (playerTransform == null || hasAttacked)
            return;
        
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        
        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
    }
    
    void AttackPlayer()
    {
        hasAttacked = true;
        
        HealthSystem playerHealth = playerTransform.GetComponent<HealthSystem>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageToPlayer);
            Debug.Log($"Enemy attacked player for {damageToPlayer} damage!");
        }
        
        Destroy(gameObject, 0.5f);
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Enemy took {damage} damage! Health: {currentHealth}/{maxHealth}");
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }
        
        Debug.Log($"Enemy destroyed! +{scoreValue} score");
        Destroy(gameObject);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasAttacked)
        {
            AttackPlayer();
        }
    }
}
