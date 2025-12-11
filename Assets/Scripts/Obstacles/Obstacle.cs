using UnityEngine;
using SkyDashRunner.Player;
using SkyDashRunner.Managers;

namespace SkyDashRunner.Obstacles
{
    public class Obstacle : MonoBehaviour
    {
        [Header("Obstacle Settings")]
        [SerializeField] protected bool isDeadly = true;
        [SerializeField] protected float damage = 1f;
        [SerializeField] protected bool canBeDestroyed = false;
        
        [Header("Effects")]
        [SerializeField] protected ParticleSystem hitEffect;
        [SerializeField] protected AudioClip hitSound;
        
        protected float difficulty = 1f;
        protected PlayerController player;
        
        protected virtual void Start()
        {
            player = FindObjectOfType<PlayerController>();
        }
        
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                HandlePlayerCollision();
            }
        }
        
        protected virtual void HandlePlayerCollision()
        {
            if (player == null) return;
            
            // Check if player has shield
            if (player.IsShieldActive && isDeadly)
            {
                // Shield protects player
                PlayHitEffect();
                DestroyObstacle();
                return;
            }
            
            if (isDeadly)
            {
                // Player dies
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.GameOver();
                }
                
                if (player != null)
                {
                    player.Die();
                }
            }
            else
            {
                // Non-deadly obstacle (slows player, etc.)
                PlayHitEffect();
            }
        }
        
        protected virtual void PlayHitEffect()
        {
            if (hitEffect != null)
            {
                hitEffect.Play();
            }
            
            if (hitSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(hitSound);
            }
        }
        
        protected virtual void DestroyObstacle()
        {
            if (canBeDestroyed)
            {
                Destroy(gameObject, 0.5f);
            }
        }
        
        public virtual void SetDifficulty(float difficultyLevel)
        {
            difficulty = difficultyLevel;
        }
    }
}

