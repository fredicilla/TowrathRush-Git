using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    [Header("Sand Floor Tiles")]
    public GameObject[] sandFloorTiles;
    
    [Header("Decorations - Close to Player")]
    public GameObject[] smallRocks;
    public GameObject[] cacti;
    public GameObject[] dryTrees;
    
    [Header("Decorations - Far Background")]
    public GameObject[] bigBoulders;
    public GameObject[] nShapedRocks;
    
    [Header("Spawn Settings")]
    public float levelLength = 200f;
    public float tileSize = 10f;
    
    [Header("Close Decoration Settings")]
    public int closeDecorationsCount = 30;
    public float closeMinDistance = 8f;
    public float closeMaxDistance = 15f;
    
    [Header("Far Decoration Settings")]
    public int farDecorationsCount = 20;
    public float farMinDistance = 25f;
    public float farMaxDistance = 50f;
    
    [Header("Sand Dune Settings")]
    public int dunesPerSide = 6;
    public float duneDistance = 40f;
    public Vector2 duneHeightRange = new Vector2(3f, 8f);
    
    private Transform environmentParent;
    
    [ContextMenu("Generate Full Environment")]
    public void GenerateEnvironment()
    {
        ClearEnvironment();
        CreateEnvironmentParent();
        SpawnSandFloor();
        SpawnCloseDecorations();
        SpawnFarDecorations();
        SpawnDistantDunes();
        
        Debug.Log("Environment generation complete!");
    }
    
    [ContextMenu("Clear Environment")]
    public void ClearEnvironment()
    {
        Transform existing = transform.Find("GeneratedEnvironment");
        if (existing != null)
        {
            DestroyImmediate(existing.gameObject);
        }
    }
    
    void CreateEnvironmentParent()
    {
        GameObject parent = new GameObject("GeneratedEnvironment");
        parent.transform.parent = transform;
        parent.transform.localPosition = Vector3.zero;
        environmentParent = parent.transform;
    }
    
    void SpawnSandFloor()
    {
        if (sandFloorTiles == null || sandFloorTiles.Length == 0)
        {
            Debug.LogWarning("No sand floor tiles assigned!");
            return;
        }
        
        GameObject floorParent = new GameObject("SandFloor");
        floorParent.transform.parent = environmentParent;
        
        int tilesNeeded = Mathf.CeilToInt(levelLength / tileSize);
        float startZ = -tileSize;
        
        for (int i = 0; i < tilesNeeded; i++)
        {
            GameObject randomTile = sandFloorTiles[Random.Range(0, sandFloorTiles.Length)];
            Vector3 position = new Vector3(0, 0, startZ + (i * tileSize));
            
            GameObject tile = Instantiate(randomTile, position, Quaternion.Euler(0, Random.Range(0, 4) * 90f, 0));
            tile.transform.parent = floorParent.transform;
            tile.name = $"SandTile_{i}";
        }
        
        Debug.Log($"Spawned {tilesNeeded} sand floor tiles");
    }
    
    void SpawnCloseDecorations()
    {
        GameObject decorParent = new GameObject("CloseDecorations");
        decorParent.transform.parent = environmentParent;
        
        for (int i = 0; i < closeDecorationsCount; i++)
        {
            float side = Random.value > 0.5f ? 1f : -1f;
            float xPos = side * Random.Range(closeMinDistance, closeMaxDistance);
            float zPos = Random.Range(-levelLength * 0.2f, levelLength * 1.2f);
            
            GameObject prefab = GetRandomDecoration(smallRocks, cacti, dryTrees);
            if (prefab != null)
            {
                Vector3 position = new Vector3(xPos, 0, zPos);
                float randomRotation = Random.Range(0f, 360f);
                GameObject decoration = Instantiate(prefab, position, Quaternion.Euler(0, randomRotation, 0));
                decoration.transform.parent = decorParent.transform;
                
                float randomScale = Random.Range(0.8f, 1.3f);
                decoration.transform.localScale = Vector3.one * randomScale;
            }
        }
    }
    
    void SpawnFarDecorations()
    {
        GameObject decorParent = new GameObject("FarDecorations");
        decorParent.transform.parent = environmentParent;
        
        for (int i = 0; i < farDecorationsCount; i++)
        {
            float side = Random.value > 0.5f ? 1f : -1f;
            float xPos = side * Random.Range(farMinDistance, farMaxDistance);
            float zPos = Random.Range(-levelLength * 0.3f, levelLength * 1.3f);
            
            GameObject prefab = GetRandomDecoration(bigBoulders, nShapedRocks);
            if (prefab != null)
            {
                Vector3 position = new Vector3(xPos, 0, zPos);
                float randomRotation = Random.Range(0f, 360f);
                GameObject decoration = Instantiate(prefab, position, Quaternion.Euler(0, randomRotation, 0));
                decoration.transform.parent = decorParent.transform;
                
                float randomScale = Random.Range(1f, 2f);
                decoration.transform.localScale = Vector3.one * randomScale;
            }
        }
    }
    
    void SpawnDistantDunes()
    {
        GameObject dunesParent = new GameObject("DistantDunes");
        dunesParent.transform.parent = environmentParent;
        
        for (int i = 0; i < dunesPerSide; i++)
        {
            float zPos = Random.Range(-levelLength * 0.5f, levelLength * 1.5f);
            float height = Random.Range(duneHeightRange.x, duneHeightRange.y);
            
            CreateDune(dunesParent.transform, new Vector3(-duneDistance, height * 0.5f, zPos), height);
            CreateDune(dunesParent.transform, new Vector3(duneDistance, height * 0.5f, zPos), height);
        }
    }
    
    void CreateDune(Transform parent, Vector3 position, float height)
    {
        GameObject dune = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        dune.name = "Dune";
        dune.transform.parent = parent;
        dune.transform.position = position;
        
        float width = height * Random.Range(3f, 5f);
        dune.transform.localScale = new Vector3(width, height, width * 1.5f);
        
        Renderer renderer = dune.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.color = new Color(0.82f, 0.72f, 0.52f);
            mat.SetFloat("_Smoothness", 0.1f);
            renderer.material = mat;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
        
        Collider collider = dune.GetComponent<Collider>();
        if (collider != null)
        {
            DestroyImmediate(collider);
        }
    }
    
    GameObject GetRandomDecoration(params GameObject[][] prefabArrays)
    {
        int totalCount = 0;
        foreach (var array in prefabArrays)
        {
            if (array != null && array.Length > 0)
                totalCount += array.Length;
        }
        
        if (totalCount == 0)
            return null;
        
        int randomIndex = Random.Range(0, totalCount);
        int currentCount = 0;
        
        foreach (var array in prefabArrays)
        {
            if (array != null && array.Length > 0)
            {
                if (randomIndex < currentCount + array.Length)
                {
                    return array[randomIndex - currentCount];
                }
                currentCount += array.Length;
            }
        }
        
        return null;
    }
}
