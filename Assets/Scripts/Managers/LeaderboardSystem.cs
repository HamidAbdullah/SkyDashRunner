using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace SkyDashRunner.Managers
{
    [System.Serializable]
    public class LeaderboardEntry
    {
        public string playerName;
        public int score;
        public float distance;
        public System.DateTime date;
    }
    
    public class LeaderboardSystem : MonoBehaviour
    {
        public static LeaderboardSystem Instance { get; private set; }
        
        [Header("Leaderboard Settings")]
        [SerializeField] private int maxEntries = 100;
        [SerializeField] private bool useOnlineLeaderboard = false;
        [SerializeField] private string leaderboardURL = ""; // Your backend URL
        
        private List<LeaderboardEntry> localLeaderboard = new List<LeaderboardEntry>();
        
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
            
            LoadLocalLeaderboard();
        }
        
        public void SubmitScore(string playerName, int score, float distance)
        {
            LeaderboardEntry entry = new LeaderboardEntry
            {
                playerName = playerName,
                score = score,
                distance = distance,
                date = System.DateTime.Now
            };
            
            localLeaderboard.Add(entry);
            localLeaderboard = localLeaderboard.OrderByDescending(e => e.score).Take(maxEntries).ToList();
            
            SaveLocalLeaderboard();
            
            if (useOnlineLeaderboard)
            {
                SubmitToOnlineLeaderboard(entry);
            }
        }
        
        public List<LeaderboardEntry> GetTopScores(int count = 10)
        {
            return localLeaderboard.Take(count).ToList();
        }
        
        public int GetPlayerRank(int score)
        {
            return localLeaderboard.FindIndex(e => e.score <= score) + 1;
        }
        
        private void SubmitToOnlineLeaderboard(LeaderboardEntry entry)
        {
            // Implement online leaderboard submission
            // This would typically use UnityWebRequest to send data to your backend
            StartCoroutine(SubmitScoreCoroutine(entry));
        }
        
        private System.Collections.IEnumerator SubmitScoreCoroutine(LeaderboardEntry entry)
        {
            // Example using UnityWebRequest
            /*
            using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Post(
                leaderboardURL, 
                JsonUtility.ToJson(entry)))
            {
                yield return www.SendWebRequest();
                
                if (www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Leaderboard submission failed: {www.error}");
                }
            }
            */
            yield return null;
        }
        
        private void SaveLocalLeaderboard()
        {
            string json = JsonUtility.ToJson(new LeaderboardData { entries = localLeaderboard });
            PlayerPrefs.SetString("Leaderboard", json);
            PlayerPrefs.Save();
        }
        
        private void LoadLocalLeaderboard()
        {
            string json = PlayerPrefs.GetString("Leaderboard", "");
            if (!string.IsNullOrEmpty(json))
            {
                LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(json);
                if (data != null && data.entries != null)
                {
                    localLeaderboard = data.entries;
                }
            }
        }
        
        [System.Serializable]
        private class LeaderboardData
        {
            public List<LeaderboardEntry> entries;
        }
    }
}

