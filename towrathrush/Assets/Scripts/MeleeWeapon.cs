using UnityEngine;
using System.Collections;

public class MeleeWeapon : MonoBehaviour
{
    [Header("Melee Settings")]
    public int damage = 2;
    public float attackRange = 3f;
    public float attackAngle = 60f;
    public float attackCooldown = 0.5f;
    public LayerMask enemyLayer;
    
    [Header("Visual")]
    public GameObject swordVisual;
    public float swingDuration = 0.3f;
    public float swingAngle = 90f;
    
    private bool canAttack = true;
    private bool isSwinging = false;
    private Quaternion originalRotation;
    
    void Start()
    {
        if (swordVisual != null)
        {
            originalRotation = swordVisual.transform.localRotation;
        }
    }
    
    public void Attack()
    {
        if (!canAttack || isSwinging)
            return;
        
        StartCoroutine(PerformAttack());
    }
    
    IEnumerator PerformAttack()
    {
        canAttack = false;
        isSwinging = true;
        
        HitEnemies();
        
        if (swordVisual != null)
        {
            yield return StartCoroutine(SwingSword());
        }
        else
        {
            yield return new WaitForSeconds(swingDuration);
        }
        
        isSwinging = false;
        yield return new WaitForSeconds(attackCooldown - swingDuration);
        canAttack = true;
    }
    
    void HitEnemies()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        
        foreach (Collider enemyCollider in hitEnemies)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                Vector3 directionToEnemy = (enemyCollider.transform.position - transform.position).normalized;
                float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);
                
                if (angleToEnemy <= attackAngle / 2f)
                {
                    Enemy enemy = enemyCollider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage);
                        Debug.Log($"Melee hit enemy for {damage} damage!");
                    }
                }
            }
        }
    }
    
    IEnumerator SwingSword()
    {
        float elapsed = 0f;
        Quaternion startRotation = swordVisual.transform.localRotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, -swingAngle);
        
        while (elapsed < swingDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / swingDuration;
            
            swordVisual.transform.localRotation = Quaternion.Lerp(startRotation, endRotation, t);
            
            yield return null;
        }
        
        swordVisual.transform.localRotation = originalRotation;
    }
    
    public bool CanAttack()
    {
        return canAttack && !isSwinging;
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        Vector3 rightBoundary = Quaternion.Euler(0, attackAngle / 2f, 0) * transform.forward * attackRange;
        Vector3 leftBoundary = Quaternion.Euler(0, -attackAngle / 2f, 0) * transform.forward * attackRange;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
    }
}
