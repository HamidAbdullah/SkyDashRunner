using UnityEngine;
using SkyDashRunner.Obstacles;
using SkyDashRunner.Collectibles;
using SkyDashRunner.PowerUps;

namespace SkyDashRunner.LevelGeneration
{
    public class IslandController : MonoBehaviour
    {
        [Header("Spawn Points")]
        [SerializeField] private Transform[] obstacleSpawnPoints;
        [SerializeField] private Transform[] collectibleSpawnPoints;
        [SerializeField] private Transform[] powerUpSpawnPoints;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject[] obstaclePrefabs;
        [SerializeField] private GameObject[] collectiblePrefabs;
        [SerializeField] private GameObject[] powerUpPrefabs;
        
        [Header("Spawn Settings")]
        [SerializeField] private float obstacleSpawnChance = 0.6f;
        [SerializeField] private float collectibleSpawnChance = 0.8f;
        [SerializeField] private float powerUpSpawnChance = 0.2f;
        
        private float difficulty = 1f;
        
        public void Initialize(float difficultyLevel, int islandIndex)
        {
            difficulty = difficultyLevel;
            
            // Spawn obstacles
            SpawnObstacles();
            
            // Spawn collectibles
            SpawnCollectibles();
            
            // Spawn power-ups
            SpawnPowerUps();
        }
        
        private void SpawnObstacles()
        {
            if (obstacleSpawnPoints == null || obstacleSpawnPoints.Length == 0) return;
            if (obstaclePrefabs == null || obstaclePrefabs.Length == 0) return;
            
            float adjustedSpawnChance = Mathf.Min(obstacleSpawnChance * difficulty, 0.95f);
            
            foreach (Transform spawnPoint in obstacleSpawnPoints)
            {
                if (Random.value < adjustedSpawnChance)
                {
                    GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                    GameObject obstacle = Instantiate(obstaclePrefab, spawnPoint.position, spawnPoint.rotation, transform);
                    
                    // Configure obstacle based on difficulty
                    Obstacle obstacleScript = obstacle.GetComponent<Obstacle>();
                    if (obstacleScript != null)
                    {
                        obstacleScript.SetDifficulty(difficulty);
                    }
                }
            }
        }
        
        private void SpawnCollectibles()
        {
            if (collectibleSpawnPoints == null || collectibleSpawnPoints.Length == 0) return;
            if (collectiblePrefabs == null || collectiblePrefabs.Length == 0) return;
            
            foreach (Transform spawnPoint in collectibleSpawnPoints)
            {
                if (Random.value < collectibleSpawnChance)
                {
                    GameObject collectiblePrefab = collectiblePrefabs[Random.Range(0, collectiblePrefabs.Length)];
                    Instantiate(collectiblePrefab, spawnPoint.position, spawnPoint.rotation, transform);
                }
            }
        }
        
        private void SpawnPowerUps()
        {
            if (powerUpSpawnPoints == null || powerUpSpawnPoints.Length == 0) return;
            if (powerUpPrefabs == null || powerUpPrefabs.Length == 0) return;
            
            foreach (Transform spawnPoint in powerUpSpawnPoints)
            {
                if (Random.value < powerUpSpawnChance)
                {
                    GameObject powerUpPrefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
                    Instantiate(powerUpPrefab, spawnPoint.position, spawnPoint.rotation, transform);
                }
            }
        }
    }
}

