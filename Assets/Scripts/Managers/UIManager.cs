using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SkyDashRunner.Managers;

namespace SkyDashRunner.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        [Header("Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private GameObject missionsPanel;
        
        [Header("Main Menu")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button shopButton;
        [SerializeField] private Button missionsButton;
        [SerializeField] private TextMeshProUGUI bestScoreText;
        
        [Header("Game UI")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI coinsText;
        [SerializeField] private TextMeshProUGUI distanceText;
        [SerializeField] private Button pauseButton;
        
        [Header("Game Over")]
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private TextMeshProUGUI bestScoreDisplayText;
        [SerializeField] private TextMeshProUGUI coinsEarnedText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button menuButton;
        
        [Header("Score Popup")]
        [SerializeField] private GameObject scorePopupPrefab;
        [SerializeField] private Transform scorePopupParent;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged += UpdateScore;
                GameManager.Instance.OnCoinsChanged += UpdateCoins;
                GameManager.Instance.OnDistanceChanged += UpdateDistance;
                GameManager.Instance.OnGameOver += ShowGameOverScreen;
                GameManager.Instance.OnGameStart += ShowGameUI;
            }
            
            // Button listeners
            if (playButton != null) playButton.onClick.AddListener(StartGame);
            if (shopButton != null) shopButton.onClick.AddListener(ShowShop);
            if (missionsButton != null) missionsButton.onClick.AddListener(ShowMissions);
            if (pauseButton != null) pauseButton.onClick.AddListener(PauseGame);
            if (restartButton != null) restartButton.onClick.AddListener(RestartGame);
            if (menuButton != null) menuButton.onClick.AddListener(GoToMainMenu);
        }
        
        private void Start()
        {
            ShowMainMenu();
        }
        
        public void ShowMainMenu()
        {
            HideAllPanels();
            if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
            if (bestScoreText != null)
            {
                bestScoreText.text = $"Best Score: {GameManager.Instance?.HighestScore ?? 0}";
            }
        }
        
        public void ShowGameUI()
        {
            HideAllPanels();
            if (gamePanel != null) gamePanel.SetActive(true);
        }
        
        public void ShowPauseMenu()
        {
            if (pausePanel != null) pausePanel.SetActive(true);
        }
        
        public void ShowGameOverScreen(int score, int bestScore, int coins)
        {
            HideAllPanels();
            if (gameOverPanel != null) gameOverPanel.SetActive(true);
            
            if (finalScoreText != null) finalScoreText.text = $"Score: {score}";
            if (bestScoreDisplayText != null) bestScoreDisplayText.text = $"Best: {bestScore}";
            if (coinsEarnedText != null) coinsEarnedText.text = $"Coins: +{coins}";
        }
        
        public void ShowShop()
        {
            HideAllPanels();
            if (shopPanel != null) shopPanel.SetActive(true);
        }
        
        public void ShowMissions()
        {
            HideAllPanels();
            if (missionsPanel != null) missionsPanel.SetActive(true);
        }
        
        private void HideAllPanels()
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (gamePanel != null) gamePanel.SetActive(false);
            if (pausePanel != null) pausePanel.SetActive(false);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            if (shopPanel != null) shopPanel.SetActive(false);
            if (missionsPanel != null) missionsPanel.SetActive(false);
        }
        
        private void UpdateScore(int score)
        {
            if (scoreText != null)
            {
                scoreText.text = $"Score: {score}";
            }
        }
        
        private void UpdateCoins(int coins)
        {
            if (coinsText != null)
            {
                coinsText.text = $"Coins: {coins}";
            }
        }
        
        private void UpdateDistance(float distance)
        {
            if (distanceText != null)
            {
                distanceText.text = $"Distance: {distance:F0}m";
            }
        }
        
        public void ShowScorePopup(int points, Vector3 worldPosition)
        {
            if (scorePopupPrefab != null && scorePopupParent != null)
            {
                GameObject popup = Instantiate(scorePopupPrefab, scorePopupParent);
                TextMeshProUGUI popupText = popup.GetComponent<TextMeshProUGUI>();
                if (popupText != null)
                {
                    popupText.text = $"+{points}";
                }
                
                // Animate popup
                LeanTween.moveY(popup.GetComponent<RectTransform>(), 100f, 1f)
                    .setEase(LeanTweenType.easeOutQuad);
                LeanTween.alphaText(popup.GetComponent<RectTransform>(), 0f, 1f)
                    .setEase(LeanTweenType.easeOutQuad)
                    .setOnComplete(() => Destroy(popup));
            }
        }
        
        private void StartGame()
        {
            GameManager.Instance?.StartGame();
        }
        
        private void PauseGame()
        {
            GameManager.Instance?.PauseGame();
            ShowPauseMenu();
        }
        
        private void RestartGame()
        {
            GameManager.Instance?.RestartGame();
        }
        
        private void GoToMainMenu()
        {
            GameManager.Instance?.RestartGame();
        }
        
        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged -= UpdateScore;
                GameManager.Instance.OnCoinsChanged -= UpdateCoins;
                GameManager.Instance.OnDistanceChanged -= UpdateDistance;
                GameManager.Instance.OnGameOver -= ShowGameOverScreen;
                GameManager.Instance.OnGameStart -= ShowGameUI;
            }
        }
    }
}

