using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [Header("Level Settings")]
    public float levelLength = 200f;
    public float groundWidth = 10f;
    
    [Header("Prefabs")]
    public GameObject obstaclePrefab;
    public GameObject groundPrefab;
    
    [ContextMenu("Create Ground")]
    public void CreateGround()
    {
        if (groundPrefab == null)
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "LevelGround";
            ground.transform.position = new Vector3(0, 0, levelLength / 2f);
            ground.transform.localScale = new Vector3(groundWidth / 10f, 1, levelLength / 10f);
            
            Renderer renderer = ground.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.gray;
            }
            
            ground.layer = LayerMask.NameToLayer("Ground");
        }
        else
        {
            GameObject ground = Instantiate(groundPrefab, new Vector3(0, 0, levelLength / 2f), Quaternion.identity);
            ground.name = "LevelGround";
        }
        
        Debug.Log($"Ground created! Length: {levelLength} units");
    }
    
    [ContextMenu("Create Goal")]
    public void CreateGoal()
    {
        GameObject goal = GameObject.CreatePrimitive(PrimitiveType.Cube);
        goal.name = "Goal";
        goal.transform.position = new Vector3(0, 2, levelLength);
        goal.transform.localScale = new Vector3(10, 5, 2);
        
        Renderer renderer = goal.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.color = Color.green;
            renderer.material = mat;
        }
        
        Collider collider = goal.GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
        
        goal.AddComponent<GoalTrigger>();
        
        Debug.Log($"Goal created at Z = {levelLength}");
    }
    
    [ContextMenu("Spawn Obstacle at Player Position")]
    public void SpawnObstacleAtPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }
        
        if (obstaclePrefab == null)
        {
            GameObject obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obstacle.name = "Obstacle";
            obstacle.transform.position = player.transform.position + Vector3.forward * 10f;
            obstacle.transform.localScale = new Vector3(2, 2, 2);
            obstacle.tag = "Obstacle";
            
            Renderer renderer = obstacle.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                mat.color = Color.red;
                renderer.material = mat;
            }
        }
        else
        {
            Vector3 spawnPos = player.transform.position + Vector3.forward * 10f;
            GameObject obstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
            obstacle.tag = "Obstacle";
        }
        
        Debug.Log("Obstacle spawned ahead of player!");
    }
}
