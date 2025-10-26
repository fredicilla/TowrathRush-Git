using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Collectible Settings")]
    public int coinValue = 1;
    public float rotationSpeed = 100f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.3f;
    
    private Vector3 startPosition;
    private float bobTimer;
    
    void Start()
    {
        startPosition = transform.position;
        bobTimer = Random.Range(0f, Mathf.PI * 2f);
    }
    
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        
        bobTimer += Time.deltaTime * bobSpeed;
        Vector3 newPosition = startPosition;
        newPosition.y += Mathf.Sin(bobTimer) * bobHeight;
        transform.position = newPosition;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CollectCoin(coinValue);
            }
            
            Destroy(gameObject);
        }
    }
}
