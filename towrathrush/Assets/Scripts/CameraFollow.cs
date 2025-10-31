using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float smoothSpeed = 0.125f;
    
    private const float LOOK_AT_HEIGHT = 2f;
    
    void LateUpdate()
    {
        if (target == null) 
            return;
        
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        
        if (PhaseManager.Instance != null && PhaseManager.Instance.currentPhase == GamePhase.Reverse)
        {
            Vector3 reverseLookTarget = target.position - Vector3.forward * 5f + Vector3.up * LOOK_AT_HEIGHT;
            transform.LookAt(reverseLookTarget);
        }
        else if (PhaseManager.Instance != null && PhaseManager.Instance.currentPhase == GamePhase.Transition)
        {
        }
        else
        {
            transform.LookAt(target.position + Vector3.up * LOOK_AT_HEIGHT);
        }
    }
}
