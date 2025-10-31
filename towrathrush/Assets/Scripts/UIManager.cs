using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [Header("UI Panels")]
    public GameObject gameplayPanel;
    public GameObject gameOverPanel;
    
    [Header("Gameplay UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI phaseText;
    
    [Header("Game Over UI")]
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalCoinsText;
    public TextMeshProUGUI newHighScoreText;
    public Button restartButton;
    
    private int highScore = 0;
    private const string HIGH_SCORE_KEY = "HighScore";
    
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
        
        LoadHighScore();
    }
    
    void Start()
    {
        ShowGameplayUI();
        
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartButtonClicked);
        }
        
        UpdateHighScoreDisplay();
    }
    
    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }
    
    public void UpdateCoins(int coins)
    {
        if (coinsText != null)
        {
            coinsText.text = $"Coins: {coins}";
        }
    }
    
    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}/{maxHealth}";
        }
    }
    
    public void UpdateWeapon(string weaponName)
    {
        if (weaponText != null)
        {
            weaponText.text = $"Weapon: {weaponName}";
        }
    }
    
    public void UpdatePhase(string phaseName, Color phaseColor)
    {
        if (phaseText != null)
        {
            phaseText.text = phaseName;
            phaseText.color = phaseColor;
        }
    }
    
    public void ShowGameOver(int finalScore)
    {
        if (gameplayPanel != null)
            gameplayPanel.SetActive(false);
            
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        
        if (finalScoreText != null)
            finalScoreText.text = $"Score: {finalScore}";
        
        if (finalCoinsText != null && GameManager.Instance != null)
            finalCoinsText.text = $"Coins: {GameManager.Instance.GetCoins()}";
        
        bool isNewHighScore = finalScore > highScore;
        if (isNewHighScore)
        {
            highScore = finalScore;
            SaveHighScore();
        }
        
        if (newHighScoreText != null)
        {
            newHighScoreText.gameObject.SetActive(isNewHighScore);
            if (isNewHighScore)
            {
                newHighScoreText.text = "NEW HIGH SCORE!";
            }
        }
    }
    
    public void ShowGameplayUI()
    {
        if (gameplayPanel != null)
            gameplayPanel.SetActive(true);
            
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        UpdateScore(0);
        UpdateCoins(0);
        UpdateHealth(3, 3);
    }
    
    void UpdateHighScoreDisplay()
    {
        if (highScoreText != null)
        {
            highScoreText.text = $"Best: {highScore}";
        }
    }
    
    void OnRestartButtonClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
    }
    
    void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }
    
    void SaveHighScore()
    {
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
        PlayerPrefs.Save();
    }
}
