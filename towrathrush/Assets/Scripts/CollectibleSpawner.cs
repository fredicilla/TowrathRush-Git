using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Collectible Settings")]
    public GameObject collectiblePrefab;
    public float spawnDistance = 50f;
    public float minSpawnInterval = 15f;
    public float maxSpawnInterval = 30f;
    public float laneDistance = 3f;
    public float collectibleHeight = 1.5f;
    
    [Header("Spawn Patterns")]
    public bool spawnInLines = true;
    public int lineLength = 5;
    public float lineSpacing = 3f;
    
    private Transform playerTransform;
    private float nextSpawnZ;
    private const int LANE_COUNT = 3;
    private const float COLLECTIBLE_DESTROY_DELAY = 15f;
    
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
            SpawnCollectibles();
        }
    }
    
    void SpawnCollectibles()
    {
        if (collectiblePrefab == null)
        {
            Debug.LogError("Collectible prefab is not assigned!");
            return;
        }
        
        if (spawnInLines && Random.value > 0.3f)
        {
            SpawnLine();
        }
        else
        {
            SpawnSingle();
        }
        
        nextSpawnZ += Random.Range(minSpawnInterval, maxSpawnInterval);
    }
    
    void SpawnSingle()
    {
        int randomLane = Random.Range(0, LANE_COUNT);
        float xPosition = (randomLane - 1) * laneDistance;
        
        Vector3 spawnPosition = new Vector3(xPosition, collectibleHeight, nextSpawnZ);
        GameObject collectible = Instantiate(collectiblePrefab, spawnPosition, Quaternion.identity);
        
        Destroy(collectible, COLLECTIBLE_DESTROY_DELAY);
    }
    
    void SpawnLine()
    {
        int randomLane = Random.Range(0, LANE_COUNT);
        float xPosition = (randomLane - 1) * laneDistance;
        
        for (int i = 0; i < lineLength; i++)
        {
            Vector3 spawnPosition = new Vector3(xPosition, collectibleHeight, nextSpawnZ + (i * lineSpacing));
            GameObject collectible = Instantiate(collectiblePrefab, spawnPosition, Quaternion.identity);
            
            Destroy(collectible, COLLECTIBLE_DESTROY_DELAY);
        }
    }
}
