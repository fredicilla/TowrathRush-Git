using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBuilder : MonoBehaviour
{
    [ContextMenu("Build Complete UI")]
    public void BuildUI()
    {
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        CanvasScaler scaler = canvasObj.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;
        
        GameObject gameplayPanel = CreateGameplayPanel(canvasObj.transform);
        GameObject gameOverPanel = CreateGameOverPanel(canvasObj.transform);
        
        GameObject uiManagerObj = new GameObject("UIManager");
        UIManager uiManager = uiManagerObj.AddComponent<UIManager>();
        
        uiManager.gameplayPanel = gameplayPanel;
        uiManager.gameOverPanel = gameOverPanel;
        
        uiManager.scoreText = gameplayPanel.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        uiManager.highScoreText = gameplayPanel.transform.Find("HighScoreText").GetComponent<TextMeshProUGUI>();
        uiManager.coinsText = gameplayPanel.transform.Find("CoinsText").GetComponent<TextMeshProUGUI>();
        uiManager.healthText = gameplayPanel.transform.Find("HealthText").GetComponent<TextMeshProUGUI>();
        uiManager.weaponText = gameplayPanel.transform.Find("WeaponText").GetComponent<TextMeshProUGUI>();
        
        uiManager.finalScoreText = gameOverPanel.transform.Find("Panel/FinalScoreText").GetComponent<TextMeshProUGUI>();
        uiManager.finalCoinsText = gameOverPanel.transform.Find("Panel/FinalCoinsText").GetComponent<TextMeshProUGUI>();
        uiManager.newHighScoreText = gameOverPanel.transform.Find("Panel/NewHighScoreText").GetComponent<TextMeshProUGUI>();
        uiManager.restartButton = gameOverPanel.transform.Find("Panel/RestartButton").GetComponent<Button>();
        
        gameOverPanel.SetActive(false);
        
        Debug.Log("UI Built Successfully! Canvas, UIManager, Gameplay Panel, and Game Over Panel created.");
    }
    
    GameObject CreateGameplayPanel(Transform parent)
    {
        GameObject panel = new GameObject("GameplayPanel");
        panel.transform.SetParent(parent, false);
        RectTransform rt = panel.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        
        CreateTextElement(panel.transform, "ScoreText", new Vector2(50, -50), TextAnchor.UpperLeft, "Score: 0", 36, Color.white);
        CreateTextElement(panel.transform, "HighScoreText", new Vector2(50, -100), TextAnchor.UpperLeft, "Best: 0", 28, Color.yellow);
        CreateTextElement(panel.transform, "CoinsText", new Vector2(50, -150), TextAnchor.UpperLeft, "Coins: 0", 32, Color.yellow);
        CreateTextElement(panel.transform, "HealthText", new Vector2(50, -200), TextAnchor.UpperLeft, "Health: 3/3", 32, Color.green);
        CreateTextElement(panel.transform, "WeaponText", new Vector2(50, -250), TextAnchor.UpperLeft, "Weapon: Gun", 28, Color.cyan);
        
        return panel;
    }
    
    GameObject CreateGameOverPanel(Transform parent)
    {
        GameObject panel = new GameObject("GameOverPanel");
        panel.transform.SetParent(parent, false);
        RectTransform rt = panel.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        
        Image bgImage = panel.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.8f);
        
        GameObject centerPanel = new GameObject("Panel");
        centerPanel.transform.SetParent(panel.transform, false);
        RectTransform centerRt = centerPanel.AddComponent<RectTransform>();
        centerRt.anchorMin = new Vector2(0.5f, 0.5f);
        centerRt.anchorMax = new Vector2(0.5f, 0.5f);
        centerRt.sizeDelta = new Vector2(600, 500);
        centerRt.anchoredPosition = Vector2.zero;
        
        Image panelBg = centerPanel.AddComponent<Image>();
        panelBg.color = new Color(0.2f, 0.2f, 0.2f, 0.95f);
        
        CreateCenteredText(centerPanel.transform, "TitleText", new Vector2(0, 150), "GAME OVER", 72, Color.red);
        CreateCenteredText(centerPanel.transform, "FinalScoreText", new Vector2(0, 50), "Score: 0", 48, Color.white);
        CreateCenteredText(centerPanel.transform, "FinalCoinsText", new Vector2(0, 0), "Coins: 0", 40, Color.yellow);
        CreateCenteredText(centerPanel.transform, "NewHighScoreText", new Vector2(0, -60), "NEW HIGH SCORE!", 36, Color.yellow);
        
        CreateButton(centerPanel.transform, "RestartButton", new Vector2(0, -140), new Vector2(250, 60), "RESTART");
        
        return panel;
    }
    
    void CreateTextElement(Transform parent, string name, Vector2 position, TextAnchor alignment, string text, int fontSize, Color color)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent, false);
        
        RectTransform rt = textObj.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = position;
        rt.sizeDelta = new Vector2(800, 60);
        
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Left;
        tmp.fontStyle = FontStyles.Bold;
    }
    
    void CreateCenteredText(Transform parent, string name, Vector2 position, string text, int fontSize, Color color)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent, false);
        
        RectTransform rt = textObj.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = position;
        rt.sizeDelta = new Vector2(550, 80);
        
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;
    }
    
    void CreateButton(Transform parent, string name, Vector2 position, Vector2 size, string buttonText)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent, false);
        
        RectTransform rt = buttonObj.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = position;
        rt.sizeDelta = size;
        
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.3f, 0.6f, 0.3f, 1f);
        
        Button button = buttonObj.AddComponent<Button>();
        
        ColorBlock colors = button.colors;
        colors.normalColor = new Color(0.3f, 0.6f, 0.3f, 1f);
        colors.highlightedColor = new Color(0.4f, 0.8f, 0.4f, 1f);
        colors.pressedColor = new Color(0.2f, 0.4f, 0.2f, 1f);
        button.colors = colors;
        
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        RectTransform textRt = textObj.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = Vector2.zero;
        textRt.offsetMax = Vector2.zero;
        
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = buttonText;
        tmp.fontSize = 32;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;
    }
}
