using UnityEngine;
using System;
using System.Collections.Generic;

namespace SkyDashRunner.Managers
{
    [Serializable]
    public class Mission
    {
        public string id;
        public string description;
        public MissionType type;
        public int targetValue;
        public int currentValue;
        public int reward;
        public bool isCompleted;
        
        public enum MissionType
        {
            CollectCoins,
            JumpCount,
            Distance,
            Score,
            PowerUpsCollected
        }
    }
    
    public class MissionSystem : MonoBehaviour
    {
        public static MissionSystem Instance { get; private set; }
        
        [Header("Mission Settings")]
        [SerializeField] private int maxDailyMissions = 3;
        
        private List<Mission> dailyMissions = new List<Mission>();
        private DateTime lastResetDate;
        
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
            
            LoadMissions();
            CheckDailyReset();
        }
        
        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged += OnScoreChanged;
                GameManager.Instance.OnCoinsChanged += OnCoinsChanged;
                GameManager.Instance.OnDistanceChanged += OnDistanceChanged;
            }
        }
        
        private void CheckDailyReset()
        {
            string lastResetString = PlayerPrefs.GetString("LastMissionReset", DateTime.Now.ToString());
            DateTime lastReset = DateTime.Parse(lastResetString);
            
            if (DateTime.Now.Date > lastReset.Date)
            {
                GenerateDailyMissions();
                lastResetDate = DateTime.Now;
                PlayerPrefs.SetString("LastMissionReset", lastResetDate.ToString());
            }
        }
        
        private void GenerateDailyMissions()
        {
            dailyMissions.Clear();
            
            for (int i = 0; i < maxDailyMissions; i++)
            {
                Mission mission = CreateRandomMission();
                dailyMissions.Add(mission);
            }
            
            SaveMissions();
        }
        
        private Mission CreateRandomMission()
        {
            Mission mission = new Mission();
            mission.id = Guid.NewGuid().ToString();
            mission.isCompleted = false;
            mission.currentValue = 0;
            
            Array values = Enum.GetValues(typeof(Mission.MissionType));
            mission.type = (Mission.MissionType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
            
            switch (mission.type)
            {
                case Mission.MissionType.CollectCoins:
                    mission.description = "Collect {0} coins";
                    mission.targetValue = UnityEngine.Random.Range(20, 100);
                    mission.reward = mission.targetValue / 2;
                    break;
                    
                case Mission.MissionType.JumpCount:
                    mission.description = "Jump {0} times";
                    mission.targetValue = UnityEngine.Random.Range(10, 50);
                    mission.reward = mission.targetValue * 2;
                    break;
                    
                case Mission.MissionType.Distance:
                    mission.description = "Run {0} meters";
                    mission.targetValue = UnityEngine.Random.Range(500, 2000);
                    mission.reward = mission.targetValue / 10;
                    break;
                    
                case Mission.MissionType.Score:
                    mission.description = "Score {0} points";
                    mission.targetValue = UnityEngine.Random.Range(1000, 5000);
                    mission.reward = mission.targetValue / 20;
                    break;
                    
                case Mission.MissionType.PowerUpsCollected:
                    mission.description = "Collect {0} power-ups";
                    mission.targetValue = UnityEngine.Random.Range(5, 20);
                    mission.reward = mission.targetValue * 10;
                    break;
            }
            
            return mission;
        }
        
        private void OnScoreChanged(int score)
        {
            UpdateMission(Mission.MissionType.Score, score);
        }
        
        private void OnCoinsChanged(int coins)
        {
            UpdateMission(Mission.MissionType.CollectCoins, coins);
        }
        
        private void OnDistanceChanged(float distance)
        {
            UpdateMission(Mission.MissionType.Distance, (int)distance);
        }
        
        public void OnJump()
        {
            UpdateMission(Mission.MissionType.JumpCount, 1);
        }
        
        public void OnPowerUpCollected()
        {
            UpdateMission(Mission.MissionType.PowerUpsCollected, 1);
        }
        
        private void UpdateMission(Mission.MissionType type, int value)
        {
            foreach (Mission mission in dailyMissions)
            {
                if (mission.type == type && !mission.isCompleted)
                {
                    if (type == Mission.MissionType.JumpCount || type == Mission.MissionType.PowerUpsCollected)
                    {
                        mission.currentValue += value;
                    }
                    else
                    {
                        mission.currentValue = value;
                    }
                    
                    if (mission.currentValue >= mission.targetValue)
                    {
                        CompleteMission(mission);
                    }
                    
                    SaveMissions();
                }
            }
        }
        
        private void CompleteMission(Mission mission)
        {
            mission.isCompleted = true;
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddCoins(mission.reward);
            }
            
            // Show completion notification
            Debug.Log($"Mission Completed: {mission.description} - Reward: {mission.reward} coins");
        }
        
        public List<Mission> GetDailyMissions()
        {
            return dailyMissions;
        }
        
        private void SaveMissions()
        {
            // Save missions to PlayerPrefs (simplified)
            for (int i = 0; i < dailyMissions.Count; i++)
            {
                Mission mission = dailyMissions[i];
                PlayerPrefs.SetString($"Mission_{i}_ID", mission.id);
                PlayerPrefs.SetInt($"Mission_{i}_Type", (int)mission.type);
                PlayerPrefs.SetInt($"Mission_{i}_Target", mission.targetValue);
                PlayerPrefs.SetInt($"Mission_{i}_Current", mission.currentValue);
                PlayerPrefs.SetInt($"Mission_{i}_Reward", mission.reward);
                PlayerPrefs.SetInt($"Mission_{i}_Completed", mission.isCompleted ? 1 : 0);
            }
            PlayerPrefs.SetInt("MissionCount", dailyMissions.Count);
            PlayerPrefs.Save();
        }
        
        private void LoadMissions()
        {
            int missionCount = PlayerPrefs.GetInt("MissionCount", 0);
            
            if (missionCount == 0)
            {
                GenerateDailyMissions();
                return;
            }
            
            dailyMissions.Clear();
            
            for (int i = 0; i < missionCount; i++)
            {
                Mission mission = new Mission();
                mission.id = PlayerPrefs.GetString($"Mission_{i}_ID", Guid.NewGuid().ToString());
                mission.type = (Mission.MissionType)PlayerPrefs.GetInt($"Mission_{i}_Type", 0);
                mission.targetValue = PlayerPrefs.GetInt($"Mission_{i}_Target", 0);
                mission.currentValue = PlayerPrefs.GetInt($"Mission_{i}_Current", 0);
                mission.reward = PlayerPrefs.GetInt($"Mission_{i}_Reward", 0);
                mission.isCompleted = PlayerPrefs.GetInt($"Mission_{i}_Completed", 0) == 1;
                
                // Set description based on type
                switch (mission.type)
                {
                    case Mission.MissionType.CollectCoins:
                        mission.description = $"Collect {mission.targetValue} coins";
                        break;
                    case Mission.MissionType.JumpCount:
                        mission.description = $"Jump {mission.targetValue} times";
                        break;
                    case Mission.MissionType.Distance:
                        mission.description = $"Run {mission.targetValue} meters";
                        break;
                    case Mission.MissionType.Score:
                        mission.description = $"Score {mission.targetValue} points";
                        break;
                    case Mission.MissionType.PowerUpsCollected:
                        mission.description = $"Collect {mission.targetValue} power-ups";
                        break;
                }
                
                dailyMissions.Add(mission);
            }
        }
        
        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged -= OnScoreChanged;
                GameManager.Instance.OnCoinsChanged -= OnCoinsChanged;
                GameManager.Instance.OnDistanceChanged -= OnDistanceChanged;
            }
        }
    }
}

