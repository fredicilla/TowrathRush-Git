using UnityEngine;

public class EnvironmentObstacle : MonoBehaviour
{
    [Header("Obstacle Configuration")]
    [Tooltip("Cactus = 1 damage, Others = Instant Death")]
    public bool isInstantDeath = true;
    public int damageAmount = 1;
    
    [Header("Lane Visual Guide")]
    [Tooltip("For reference only - helps you position obstacles correctly")]
    public string laneInfo = "Left=-3, Middle=0, Right=3";
    
    private bool hasHitPlayer = false;
    
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{gameObject.name} collision detected with {collision.gameObject.name}");
        
        if (hasHitPlayer)
            return;
            
        if (collision.gameObject.CompareTag("Player"))
        {
            hasHitPlayer = true;
            HandlePlayerCollision(collision.gameObject);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (hasHitPlayer)
            return;
            
        if (other.CompareTag("Player"))
        {
            hasHitPlayer = true;
            HandlePlayerCollision(other.gameObject);
        }
    }
    
    void HandlePlayerCollision(GameObject player)
    {
        HealthSystem healthSystem = player.GetComponent<HealthSystem>();
        
        if (healthSystem != null)
        {
            if (isInstantDeath)
            {
                healthSystem.TakeDamage(healthSystem.maxHealth);
                Debug.Log($"Player hit {gameObject.name} - Instant Death!");
            }
            else
            {
                healthSystem.TakeDamage(damageAmount);
                Debug.Log($"Player hit {gameObject.name} - Damage: {damageAmount}");
            }
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = isInstantDeath ? Color.red : Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 5f);
    }
}
