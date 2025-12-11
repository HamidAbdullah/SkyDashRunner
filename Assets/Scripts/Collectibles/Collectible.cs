using UnityEngine;
using SkyDashRunner.Player;
using SkyDashRunner.Managers;

namespace SkyDashRunner.Collectibles
{
    public class Collectible : MonoBehaviour
    {
        [Header("Collectible Settings")]
        [SerializeField] protected int scoreValue = 10;
        [SerializeField] protected int coinValue = 1;
        [SerializeField] protected float rotationSpeed = 90f;
        [SerializeField] protected float floatSpeed = 2f;
        [SerializeField] protected float floatAmount = 0.5f;
        
        [Header("Effects")]
        [SerializeField] protected ParticleSystem collectEffect;
        [SerializeField] protected AudioClip collectSound;
        
        protected PlayerController player;
        protected bool isCollected = false;
        private Vector3 startPosition;
        private float floatOffset = 0f;
        
        protected virtual void Start()
        {
            startPosition = transform.position;
            player = FindObjectOfType<PlayerController>();
        }
        
        protected virtual void Update()
        {
            if (isCollected) return;
            
            // Rotate
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            
            // Float animation
            floatOffset += floatSpeed * Time.deltaTime;
            transform.position = startPosition + Vector3.up * Mathf.Sin(floatOffset) * floatAmount;
            
            // Magnet effect
            if (player != null && player.IsMagnetActive)
            {
                float distance = Vector3.Distance(transform.position, player.Position);
                if (distance < 10f)
                {
                    Vector3 direction = (player.Position - transform.position).normalized;
                    transform.position += direction * 15f * Time.deltaTime;
                }
            }
        }
        
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (isCollected) return;
            
            if (other.CompareTag("Player"))
            {
                Collect();
            }
        }
        
        protected virtual void Collect()
        {
            isCollected = true;
            
            // Add score and coins
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(scoreValue);
                GameManager.Instance.AddCoins(coinValue);
            }
            
            // Play effects
            if (collectEffect != null)
            {
                collectEffect.Play();
            }
            
            if (collectSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(collectSound);
            }
            
            // Show score popup
            UIManager uiManager = FindObjectOfType<UIManager>();
            if (uiManager != null)
            {
                uiManager.ShowScorePopup(scoreValue, transform.position);
            }
            
            // Hide and destroy
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            
            Destroy(gameObject, 2f);
        }
    }
}

