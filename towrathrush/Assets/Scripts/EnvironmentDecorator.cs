using UnityEngine;

public class EnvironmentDecorator : MonoBehaviour
{
    [Header("Level Dimensions")]
    public float levelLength = 200f;
    public float playableWidth = 10f;
    
    [Header("Distant Dunes")]
    public int dunesPerSide = 8;
    public float duneDistanceFromCenter = 30f;
    public Vector2 duneWidthRange = new Vector2(15f, 30f);
    public Vector2 duneHeightRange = new Vector2(5f, 15f);
    public Color duneColor = new Color(0.85f, 0.75f, 0.55f);
    
    [Header("Background Props")]
    public int rockCount = 20;
    public Vector2 rockSizeRange = new Vector2(1f, 4f);
    public float rockSpawnDistance = 20f;
    
    [Header("Atmospheric")]
    public bool addFog = true;
    public Color fogColor = new Color(0.9f, 0.85f, 0.7f);
    public float fogDensity = 0.01f;
    
    [ContextMenu("Generate Environment")]
    public void GenerateEnvironment()
    {
        ClearOldEnvironment();
        CreateDistantDunes();
        CreateBackgroundRocks();
        SetupAtmosphere();
        
        Debug.Log("Environment decoration complete!");
    }
    
    void ClearOldEnvironment()
    {
        Transform envParent = transform.Find("Environment");
        if (envParent != null)
        {
            DestroyImmediate(envParent.gameObject);
        }
    }
    
    void CreateDistantDunes()
    {
        GameObject dunesParent = new GameObject("Environment");
        dunesParent.transform.parent = transform;
        
        GameObject dunesLeft = new GameObject("Dunes_Left");
        dunesLeft.transform.parent = dunesParent.transform;
        
        GameObject dunesRight = new GameObject("Dunes_Right");
        dunesRight.transform.parent = dunesParent.transform;
        
        for (int i = 0; i < dunesPerSide; i++)
        {
            float zPos = Random.Range(-levelLength * 0.5f, levelLength * 1.5f);
            float width = Random.Range(duneWidthRange.x, duneWidthRange.y);
            float height = Random.Range(duneHeightRange.x, duneHeightRange.y);
            
            CreateDune(dunesLeft.transform, new Vector3(-duneDistanceFromCenter, height * 0.5f, zPos), width, height);
            CreateDune(dunesRight.transform, new Vector3(duneDistanceFromCenter, height * 0.5f, zPos), width, height);
        }
    }
    
    void CreateDune(Transform parent, Vector3 position, float width, float height)
    {
        GameObject dune = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        dune.name = "Dune";
        dune.transform.parent = parent;
        dune.transform.position = position;
        dune.transform.localScale = new Vector3(width, height, width * 1.5f);
        
        Renderer renderer = dune.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.color = duneColor;
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
    
    void CreateBackgroundRocks()
    {
        Transform envParent = transform.Find("Environment");
        if (envParent == null)
            return;
            
        GameObject rocksParent = new GameObject("Rocks");
        rocksParent.transform.parent = envParent;
        
        for (int i = 0; i < rockCount; i++)
        {
            float side = Random.value > 0.5f ? 1f : -1f;
            float xPos = side * Random.Range(rockSpawnDistance, duneDistanceFromCenter - 5f);
            float zPos = Random.Range(-levelLength * 0.3f, levelLength * 1.2f);
            float size = Random.Range(rockSizeRange.x, rockSizeRange.y);
            
            CreateRock(rocksParent.transform, new Vector3(xPos, size * 0.5f, zPos), size);
        }
    }
    
    void CreateRock(Transform parent, Vector3 position, float size)
    {
        GameObject rock = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rock.name = "Rock";
        rock.transform.parent = parent;
        rock.transform.position = position;
        rock.transform.localScale = new Vector3(size, size * 1.2f, size * 0.8f);
        rock.transform.rotation = Quaternion.Euler(
            Random.Range(-15f, 15f),
            Random.Range(0f, 360f),
            Random.Range(-15f, 15f)
        );
        
        Renderer renderer = rock.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.color = new Color(0.4f, 0.35f, 0.3f);
            renderer.material = mat;
        }
        
        Collider collider = rock.GetComponent<Collider>();
        if (collider != null)
        {
            DestroyImmediate(collider);
        }
    }
    
    void SetupAtmosphere()
    {
        if (addFog)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogMode = FogMode.Exponential;
            RenderSettings.fogDensity = fogDensity;
        }
        
        Light sun = FindAnyObjectByType<Light>();
        if (sun != null && sun.type == LightType.Directional)
        {
            sun.color = new Color(1f, 0.95f, 0.85f);
            sun.intensity = 1.2f;
        }
    }
}
