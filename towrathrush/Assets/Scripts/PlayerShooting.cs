using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 0.3f;
    
    [Header("Auto Aim")]
    public bool useAutoAim = true;
    public float autoAimRange = 50f;
    public LayerMask enemyLayer;
    
    private float nextFireTime = 0f;
    private bool isShooting = false;
    private InputAction attackAction;
    
    void Start()
    {
        var playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        if (playerInput != null)
        {
            attackAction = playerInput.actions["Attack"];
        }
    }
    
    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.isGameActive)
            return;
        
        if (attackAction != null)
        {
            isShooting = attackAction.IsPressed();
        }
        
        if (isShooting && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }
    
    void OnAttack(InputValue value)
    {
        isShooting = value.isPressed;
        Debug.Log($"OnAttack called - Shooting: {isShooting}");
    }
    
    void Shoot()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile prefab is not assigned!");
            return;
        }
        
        Transform spawnPoint = firePoint != null ? firePoint : transform;
        
        Vector3 direction = GetShootDirection();
        Quaternion rotation = Quaternion.LookRotation(direction);
        
        Instantiate(projectilePrefab, spawnPoint.position, rotation);
        
        Debug.Log("Player fired!");
    }
    
    Vector3 GetShootDirection()
    {
        if (useAutoAim)
        {
            Transform nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                return (nearestEnemy.position - transform.position).normalized;
            }
        }
        
        return transform.forward;
    }
    
    Transform FindNearestEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, autoAimRange, enemyLayer);
        
        Transform nearest = null;
        float closestDistance = float.MaxValue;
        
        foreach (Collider enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Vector3 directionToEnemy = enemy.transform.position - transform.position;
                
                if (directionToEnemy.z > 0)
                {
                    float distance = directionToEnemy.magnitude;
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        nearest = enemy.transform;
                    }
                }
            }
        }
        
        return nearest;
    }
}
