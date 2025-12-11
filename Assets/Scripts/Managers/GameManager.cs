using UnityEngine;
using UnityEngine.SceneManagement;
using SkyDashRunner.UI;
using SkyDashRunner.Collectibles;

namespace SkyDashRunner.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [Header("Game Settings")]
        [SerializeField] private float gameSpeedIncreaseRate = 0.1f;
        [SerializeField] private float maxGameSpeed = 20f;
        
        [Header("References")]
        [SerializeField] private Player.PlayerController player;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private AudioManager audioManager;
        
        private float currentGameSpeed = 10f;
        private int score = 0;
        private int coins = 0;
        private float distance = 0f;
        private bool isGameRunning = false;
        private bool isGamePaused = false;
        private int highestScore = 0;
        
        // Events
        public System.Action<int> OnScoreChanged;
        public System.Action<int> OnCoinsChanged;
        public System.Action<float> OnDistanceChanged;
        public System.Action OnGameOver;
        public System.Action OnGameStart;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            LoadGameData();
        }
        
        private void Start()
        {
            if (uiManager != null)
            {
                uiManager.ShowMainMenu();
            }
        }
        
        private void Update()
        {
            if (isGameRunning && !isGamePaused)
            {
                UpdateGameSpeed();
                UpdateDistance();
            }
        }
        
        private void UpdateGameSpeed()
        {
            currentGameSpeed = Mathf.Min(currentGameSpeed + gameSpeedIncreaseRate * Time.deltaTime, maxGameSpeed);
        }
        
        private void UpdateDistance()
        {
            distance += currentGameSpeed * Time.deltaTime;
            OnDistanceChanged?.Invoke(distance);
        }
        
        public void StartGame()
        {
            isGameRunning = true;
            isGamePaused = false;
            score = 0;
            coins = 0;
            distance = 0f;
            currentGameSpeed = 10f;
            
            if (player != null)
            {
                player.enabled = true;
            }
            
            OnGameStart?.Invoke();
            
            if (audioManager != null)
            {
                audioManager.PlayGameMusic();
            }
        }
        
        public void PauseGame()
        {
            isGamePaused = true;
            Time.timeScale = 0f;
        }
        
        public void ResumeGame()
        {
            isGamePaused = false;
            Time.timeScale = 1f;
        }
        
        public void GameOver()
        {
            if (!isGameRunning) return;
            
            isGameRunning = false;
            
            if (score > highestScore)
            {
                highestScore = score;
                SaveGameData();
            }
            
            OnGameOver?.Invoke();
            
            if (audioManager != null)
            {
                audioManager.PlayGameOverSound();
            }
            
            if (uiManager != null)
            {
                uiManager.ShowGameOverScreen(score, highestScore, coins);
            }
        }
        
        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        public void AddScore(int points)
        {
            score += points;
            OnScoreChanged?.Invoke(score);
        }
        
        public void AddCoins(int amount)
        {
            coins += amount;
            OnCoinsChanged?.Invoke(coins);
            SaveGameData();
        }
        
        public void SpendCoins(int amount)
        {
            coins = Mathf.Max(0, coins - amount);
            OnCoinsChanged?.Invoke(coins);
            SaveGameData();
        }
        
        // Getters
        public float CurrentGameSpeed => currentGameSpeed;
        public int Score => score;
        public int Coins => coins;
        public float Distance => distance;
        public bool IsGameRunning => isGameRunning;
        public bool IsGamePaused => isGamePaused;
        public int HighestScore => highestScore;
        
        // Save/Load
        private void SaveGameData()
        {
            PlayerPrefs.SetInt("HighestScore", highestScore);
            PlayerPrefs.SetInt("TotalCoins", coins);
            PlayerPrefs.Save();
        }
        
        private void LoadGameData()
        {
            highestScore = PlayerPrefs.GetInt("HighestScore", 0);
            coins = PlayerPrefs.GetInt("TotalCoins", 0);
        }
    }
}

