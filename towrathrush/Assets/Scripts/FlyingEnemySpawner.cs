using UnityEngine;

public class FlyingEnemySpawner : MonoBehaviour
{
    [Header("Flying Enemy Settings")]
    public GameObject flyingEnemyPrefab;
    public float spawnDistance = 70f;
    public float minSpawnInterval = 4f;
    public float maxSpawnInterval = 8f;
    public float laneDistance = 3f;
    public float flyHeight = 4f;
    
    private Transform playerTransform;
    private float nextSpawnZ;
    private const int LANE_COUNT = 3;
    
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
            SpawnFlyingEnemy();
        }
    }
    
    void SpawnFlyingEnemy()
    {
        if (flyingEnemyPrefab == null)
        {
            Debug.LogError("Flying Enemy prefab is not assigned!");
            return;
        }
        
        int randomLane = Random.Range(0, LANE_COUNT);
        float xPosition = (randomLane - 1) * laneDistance;
        
        Vector3 spawnPosition = new Vector3(xPosition, flyHeight, nextSpawnZ);
        GameObject flyingEnemy = Instantiate(flyingEnemyPrefab, spawnPosition, Quaternion.identity);
        flyingEnemy.tag = "Enemy";
        
        float spawnIntervalMultiplier = 1f;
        if (GameManager.Instance != null)
        {
            spawnIntervalMultiplier = 1f / GameManager.Instance.GetSpeedMultiplier();
        }
        
        float adjustedInterval = Random.Range(minSpawnInterval, maxSpawnInterval) * spawnIntervalMultiplier;
        nextSpawnZ += adjustedInterval * 10f;
    }
}
