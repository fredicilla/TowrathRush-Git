using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int maxHealth = 3;
    public int damageToPlayer = 1;
    public float moveSpeed = 6f;
    public int scoreValue = 100;
    public float flyHeight = 3f;
    
    [Header("Combat")]
    public float attackRange = 3f;
    public float attackCooldown = 2f;
    
    [Header("Movement")]
    public float bobSpeed = 2f;
    public float bobAmount = 0.5f;
    
    private int currentHealth;
    private Transform playerTransform;
    private float nextAttackTime = 0f;
    private Rigidbody rb;
    private float bobTimer = 0f;
    private float baseYPosition;
    
    void Start()
    {
        currentHealth = maxHealth;
        
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        
        baseYPosition = transform.position.y;
    }
    
    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.isGameActive)
            return;
        
        MoveTowardPlayer();
        ApplyBobbing();
        CheckAttack();
    }
    
    void MoveTowardPlayer()
    {
        if (playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction.y = 0;
            
            Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;
            newPosition.y = baseYPosition + Mathf.Sin(bobTimer * bobSpeed) * bobAmount;
            
            transform.position = newPosition;
            
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
    
    void ApplyBobbing()
    {
        bobTimer += Time.deltaTime;
    }
    
    void CheckAttack()
    {
        if (playerTransform == null || Time.time < nextAttackTime)
            return;
        
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        
        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
    }
    
    void AttackPlayer()
    {
        nextAttackTime = Time.time + attackCooldown;
        
        HealthSystem playerHealth = playerTransform.GetComponent<HealthSystem>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageToPlayer);
            Debug.Log($"Flying Enemy attacked player for {damageToPlayer} damage!");
        }
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Flying Enemy took {damage} damage! Health: {currentHealth}/{maxHealth}");
        
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
        
        Debug.Log($"Flying Enemy destroyed! +{scoreValue} score");
        Destroy(gameObject);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Time.time >= nextAttackTime)
        {
            AttackPlayer();
        }
    }
}
