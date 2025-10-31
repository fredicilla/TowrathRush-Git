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
    
    [Header("Speed Progression")]
    public float speedIncreaseInterval = 10f;
    public float speedIncreaseAmount = 0.5f;
    public float maxSpeedMultiplier = 3f;
    
    private float score = 0f;
    private int coins = 0;
    private float gameTime = 0f;
    private float currentSpeedMultiplier = 1f;
    
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
            gameTime += Time.deltaTime;
            score += Time.deltaTime * scoreMultiplier;
            
            UpdateSpeedMultiplier();
            
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateScore(GetScore());
            }
        }
    }
    
    void UpdateSpeedMultiplier()
    {
        float targetMultiplier = 1f + (gameTime / speedIncreaseInterval) * speedIncreaseAmount;
        currentSpeedMultiplier = Mathf.Min(targetMultiplier, maxSpeedMultiplier);
    }
    
    public float GetSpeedMultiplier()
    {
        return currentSpeedMultiplier;
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
    
    public void LevelComplete()
    {
        isGameActive = false;
        Debug.Log($"Level Complete! Final Score: {GetScore()}");
        
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOver(GetScore());
        }
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
        gameTime = 0f;
        currentSpeedMultiplier = 1f;
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
