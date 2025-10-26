using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public float spawnDistance = 60f;
    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 6f;
    public float laneDistance = 3f;
    public float enemyHeight = 1f;
    
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
            SpawnEnemy();
        }
    }
    
    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned!");
            return;
        }
        
        int randomLane = Random.Range(0, LANE_COUNT);
        float xPosition = (randomLane - 1) * laneDistance;
        
        Vector3 spawnPosition = new Vector3(xPosition, enemyHeight, nextSpawnZ);
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.tag = "Enemy";
        
        nextSpawnZ += Random.Range(minSpawnInterval, maxSpawnInterval) * 10f;
    }
}
