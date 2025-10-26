using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game State")]
    public bool isGameActive = true;
    
    [Header("Game Settings")]
    public float scoreMultiplier = 10f;
    public int coinScoreValue = 10;
    
    private float score = 0f;
    private int coins = 0;
    
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
    
    void Update()
    {
        if (isGameActive)
        {
            score += Time.deltaTime * scoreMultiplier;
            
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateScore(GetScore());
            }
        }
    }
    
    public void GameOver()
    {
        if (!isGameActive)
            return;
            
        isGameActive = false;
        
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOver(GetScore());
        }
        
        Debug.Log($"Game Over! Score: {GetScore()}");
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public int GetScore()
    {
        return Mathf.FloorToInt(score);
    }
    
    public void ResetScore()
    {
        score = 0f;
        coins = 0;
    }
    
    public void CollectCoin(int amount = 1)
    {
        coins += amount;
        score += coinScoreValue * amount;
        
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateCoins(coins);
            UIManager.Instance.UpdateScore(GetScore());
        }
        
        Debug.Log($"Coin collected! Total coins: {coins}");
    }
    
    public int GetCoins()
    {
        return coins;
    }
    
    public void AddScore(int amount)
    {
        score += amount;
        
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScore(GetScore());
        }
    }
}
