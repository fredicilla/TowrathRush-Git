using UnityEngine;

public class FixZaydPosition : MonoBehaviour
{
    [ContextMenu("Fix Zayd Position")]
    public void FixPosition()
    {
        Transform player = GameObject.Find("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }
        
        Transform zayd = player.Find("Zayd");
        if (zayd == null)
        {
            Debug.LogError("Zayd not found under Player!");
            return;
        }
        
        zayd.localPosition = new Vector3(0, -0.5f, 0);
        zayd.localRotation = Quaternion.identity;
        zayd.localScale = Vector3.one;
        
        Debug.Log("Zayd position fixed! Position: (0, -0.5, 0)");
    }
}
