using UnityEngine;
using System.Collections.Generic;

public class GroundSpawner : MonoBehaviour
{
    [Header("Ground Settings")]
    public GameObject groundPrefab;
    public int initialGroundCount = 5;
    public float groundLength = 50f;
    
    private List<GameObject> activeGroundPieces;
    private Transform playerTransform;
    private float spawnZ = 0f;
    
    void Start()
    {
        activeGroundPieces = new List<GameObject>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
            return;
        }
        
        for (int i = 0; i < initialGroundCount; i++)
        {
            SpawnGroundPiece();
        }
    }
    
    void Update()
    {
        if (playerTransform == null) 
            return;
            
        if (playerTransform.position.z > (spawnZ - initialGroundCount * groundLength))
        {
            SpawnGroundPiece();
           // DeleteOldGround();
        }
    }
    
    void SpawnGroundPiece()
    {
        if (groundPrefab == null)
        {
            Debug.LogError("Ground prefab is not assigned!");
            return;
        }
        
        GameObject ground = Instantiate(groundPrefab, new Vector3(0, 0, spawnZ), Quaternion.identity);
        ground.layer = LayerMask.NameToLayer("Ground");
        
        Rigidbody groundRb = ground.GetComponent<Rigidbody>();
        if (groundRb != null)
        {
            Destroy(groundRb);
        }
        
        activeGroundPieces.Add(ground);
        spawnZ += groundLength;
    }
    
    void DeleteOldGround()
    {
        if (activeGroundPieces.Count > initialGroundCount)
        {
            Destroy(activeGroundPieces[0]);
            activeGroundPieces.RemoveAt(0);
        }
    }
}
