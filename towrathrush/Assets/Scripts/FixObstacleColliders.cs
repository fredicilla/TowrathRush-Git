using UnityEngine;

public class FixObstacleColliders : MonoBehaviour
{
    [ContextMenu("Fix All Obstacle Colliders in Scene")]
    void FixAllObstacleColliders()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        
        int fixedCount = 0;
        
        foreach (GameObject obstacle in obstacles)
        {
            BoxCollider boxCollider = obstacle.GetComponent<BoxCollider>();
            MeshFilter meshFilter = obstacle.GetComponent<MeshFilter>();
            
            if (boxCollider != null && meshFilter != null)
            {
                Bounds bounds = meshFilter.sharedMesh.bounds;
                
                boxCollider.center = bounds.center;
                boxCollider.size = bounds.size;
                
                Debug.Log($"Fixed collider for: {obstacle.name} - Size: {bounds.size}");
                fixedCount++;
            }
        }
        
        Debug.Log($"Fixed {fixedCount} obstacle colliders!");
    }
    
    [ContextMenu("Fix This Obstacle Collider")]
    void FixThisObstacleCollider()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        
        if (boxCollider != null && meshFilter != null)
        {
            Bounds bounds = meshFilter.sharedMesh.bounds;
            
            boxCollider.center = bounds.center;
            boxCollider.size = bounds.size;
            
            Debug.Log($"Fixed collider for: {gameObject.name} - Size: {bounds.size}");
        }
        else
        {
            Debug.LogError("Missing BoxCollider or MeshFilter!");
        }
    }
}
