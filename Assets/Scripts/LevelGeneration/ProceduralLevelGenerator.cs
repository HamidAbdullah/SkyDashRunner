using UnityEngine;
using System.Collections.Generic;
using SkyDashRunner.Managers;

namespace SkyDashRunner.LevelGeneration
{
    public class ProceduralLevelGenerator : MonoBehaviour
    {
        [Header("Island Settings")]
        [SerializeField] private GameObject[] islandPrefabs;
        [SerializeField] private int islandsAhead = 5;
        [SerializeField] private float islandSpacing = 50f;
        [SerializeField] private float islandWidth = 10f;
        
        [Header("Spawn Settings")]
        [SerializeField] private Transform player;
        [SerializeField] private float spawnDistance = 200f;
        [SerializeField] private float despawnDistance = -50f;
        
        [Header("Difficulty")]
        [SerializeField] private float difficultyIncreaseRate = 0.1f;
        [SerializeField] private float maxDifficulty = 10f;
        
        private List<GameObject> activeIslands = new List<GameObject>();
        private float lastIslandZ = 0f;
        private float currentDifficulty = 1f;
        private int islandsSpawned = 0;
        
        private void Start()
        {
            // Spawn initial islands
            for (int i = 0; i < islandsAhead; i++)
            {
                SpawnIsland();
            }
        }
        
        private void Update()
        {
            if (player == null) return;
            
            // Update difficulty
            if (GameManager.Instance != null)
            {
                currentDifficulty = Mathf.Min(
                    1f + (GameManager.Instance.Distance / 100f) * difficultyIncreaseRate,
                    maxDifficulty
                );
            }
            
            // Spawn new islands ahead
            if (player.position.z + spawnDistance > lastIslandZ)
            {
                SpawnIsland();
            }
            
            // Despawn islands behind
            for (int i = activeIslands.Count - 1; i >= 0; i--)
            {
                if (activeIslands[i] != null)
                {
                    if (activeIslands[i].transform.position.z < player.position.z + despawnDistance)
                    {
                        Destroy(activeIslands[i]);
                        activeIslands.RemoveAt(i);
                    }
                }
                else
                {
                    activeIslands.RemoveAt(i);
                }
            }
        }
        
        private void SpawnIsland()
        {
            if (islandPrefabs == null || islandPrefabs.Length == 0) return;
            
            // Select random island prefab
            GameObject islandPrefab = islandPrefabs[Random.Range(0, islandPrefabs.Length)];
            
            // Calculate position
            Vector3 position = new Vector3(0, 0, lastIslandZ);
            
            // Spawn island
            GameObject island = Instantiate(islandPrefab, position, Quaternion.identity, transform);
            
            // Configure island
            IslandController islandController = island.GetComponent<IslandController>();
            if (islandController != null)
            {
                islandController.Initialize(currentDifficulty, islandsSpawned);
            }
            
            activeIslands.Add(island);
            lastIslandZ += islandSpacing;
            islandsSpawned++;
        }
        
        public void ResetGenerator()
        {
            // Destroy all active islands
            foreach (GameObject island in activeIslands)
            {
                if (island != null) Destroy(island);
            }
            activeIslands.Clear();
            
            lastIslandZ = 0f;
            currentDifficulty = 1f;
            islandsSpawned = 0;
            
            // Respawn initial islands
            for (int i = 0; i < islandsAhead; i++)
            {
                SpawnIsland();
            }
        }
    }
}

