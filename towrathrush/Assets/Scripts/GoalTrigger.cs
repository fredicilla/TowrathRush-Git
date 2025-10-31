using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    [Header("Visual")]
    public Color goalColor = Color.green;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the goal!");
            
            if (PhaseManager.Instance != null)
            {
                PhaseManager.Instance.TriggerEndReached();
            }
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = goalColor;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
