using UnityEngine;

public class TentWinTrigger : MonoBehaviour
{
    [Header("Visual")]
    public Color tentColor = Color.yellow;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PhaseManager.Instance != null && PhaseManager.Instance.IsReversePhase())
            {
                Debug.Log("Player reached the tent during reverse phase - LEVEL COMPLETE!");
                
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.LevelComplete();
                }
            }
            else
            {
                Debug.Log("Player at tent but not in reverse phase yet...");
            }
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = tentColor;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
