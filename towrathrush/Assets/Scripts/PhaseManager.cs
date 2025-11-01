using UnityEngine;

public enum GamePhase
{
    Forward,
    Transition,
    Reverse
}

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager Instance { get; private set; }
    
    [Header("Phase Settings")]
    public GamePhase currentPhase = GamePhase.Forward;
    public float transitionDuration = 2f;
    
    [Header("Level Positions")]
    public Vector3 startPosition = new Vector3(0, 2, -136);
    public Vector3 endPosition = new Vector3(0, 2, 200);
    
    [Header("References")]
    public Transform player;
    public Transform mainCamera;
    
    [Header("Enemy Spawning - Phase 2")]
    public GameObject enemyPrefab;
    public float enemySpawnInterval = 5f;
    public float enemySpawnDistance = 30f;
    
    private float transitionTimer = 0f;
    private float nextEnemySpawnTime = 0f;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
        
        if (mainCamera == null)
        {
            mainCamera = Camera.main.transform;
        }
        
        StartForwardPhase();
    }
    
    void Update()
    {
        if (currentPhase == GamePhase.Transition)
        {
            UpdateTransition();
        }
        else if (currentPhase == GamePhase.Reverse)
        {
            UpdateReversePhase();
        }
    }
    
    public void StartForwardPhase()
    {
        currentPhase = GamePhase.Forward;
        
        if (player != null)
        {
            player.position = startPosition;
            player.rotation = Quaternion.Euler(0, 0, 0);
        }
        
        if (mainCamera != null)
        {
            mainCamera.localRotation = Quaternion.identity;
        }
        
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdatePhase("PHASE 1: FORWARD - Remember the obstacles!", Color.cyan);
        }
        
        Debug.Log("Phase 1: FORWARD - Remember the obstacles!");
    }
    
    public bool IsReversePhase()
    {
        return currentPhase == GamePhase.Reverse;
    }
    
    public void TriggerEndReached()
    {
        if (currentPhase != GamePhase.Forward)
            return;
        
        Debug.Log("End reached! Starting transition to REVERSE phase...");
        StartTransition();
    }
    
    void StartTransition()
    {
        currentPhase = GamePhase.Transition;
        transitionTimer = 0f;
        
        if (player != null)
        {
            player.position = endPosition;
            
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
        
        Debug.Log("Transitioning to reverse phase...");
    }
    
    void UpdateTransition()
    {
        transitionTimer += Time.deltaTime;
        
        if (transitionTimer >= transitionDuration)
        {
            StartReversePhase();
        }
    }
    
    void StartReversePhase()
    {
        currentPhase = GamePhase.Reverse;
        nextEnemySpawnTime = Time.time + enemySpawnInterval;
        
        if (player != null)
        {
            player.rotation = Quaternion.Euler(0, 180, 0);
        }
        
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdatePhase("PHASE 2: REVERSE - Dodge from memory!", Color.red);
        }
        
        Debug.Log("Phase 2: REVERSE - Dodge from memory while fighting enemies!");
    }
    
    void UpdateReversePhase()
    {
        if (Time.time >= nextEnemySpawnTime)
        {
            SpawnEnemy();
            nextEnemySpawnTime = Time.time + enemySpawnInterval;
        }
        
        if (player != null && player.position.z <= startPosition.z + 10f)
        {
            LevelComplete();
        }
    }
    
    void SpawnEnemy()
    {
        if (enemyPrefab == null || player == null)
            return;
        
        int randomLane = Random.Range(0, 3);
        float xPosition = (randomLane - 1) * 3f;
        float zPosition = player.position.z - enemySpawnDistance;
        
        Vector3 spawnPosition = new Vector3(xPosition, 1f, zPosition);
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.tag = "Enemy";
        
        Debug.Log($"Enemy spawned in reverse phase at {spawnPosition}");
    }
    
    void LevelComplete()
    {
        Debug.Log("LEVEL COMPLETE! You made it back to the start!");
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LevelComplete();
        }
    }
    
    public bool IsForwardPhase()
    {
        return currentPhase == GamePhase.Forward;
    }
}
