using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public GameObject obstaclePrefab;
    public float spawnDistance = 50f;
    public float minSpawnInterval = 2f;
    public float maxSpawnInterval = 5f;
    public float laneDistance = 3f;
    
    private Transform playerTransform;
    private float nextSpawnZ;
    private const int LANE_COUNT = 3;
    private const float OBSTACLE_DESTROY_DELAY = 10f;
    
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            nextSpawnZ = playerTransform.position.z + spawnDistance;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }
    }
    
    void Update()
    {
        if (playerTransform == null) 
            return;
            
        if (GameManager.Instance != null && !GameManager.Instance.isGameActive)
            return;
        
        if (playerTransform.position.z > nextSpawnZ - spawnDistance)
        {
            SpawnObstacle();
        }
    }
    
    void SpawnObstacle()
    {
        if (obstaclePrefab == null)
        {
            Debug.LogError("Obstacle prefab is not assigned!");
            return;
        }
        
        int randomLane = Random.Range(0, LANE_COUNT);
        float xPosition = (randomLane - 1) * laneDistance;
        
        Vector3 spawnPosition = new Vector3(xPosition, 1, nextSpawnZ);
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        obstacle.tag = "Obstacle";
        
        Destroy(obstacle, OBSTACLE_DESTROY_DELAY);
        
        nextSpawnZ += Random.Range(minSpawnInterval, maxSpawnInterval) * 10f;
    }
}
