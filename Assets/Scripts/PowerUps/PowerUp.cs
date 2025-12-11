using UnityEngine;
using SkyDashRunner.Player;
using SkyDashRunner.Managers;

namespace SkyDashRunner.PowerUps
{
    public abstract class PowerUp : MonoBehaviour
    {
        [Header("Power-Up Settings")]
        [SerializeField] protected float duration = 5f;
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
            
            // Play effects
            if (collectEffect != null)
            {
                collectEffect.Play();
            }
            
            if (collectSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(collectSound);
            }
            
            // Apply power-up effect
            ApplyPowerUp();
            
            // Hide and destroy
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            
            Destroy(gameObject, 2f);
        }
        
        protected abstract void ApplyPowerUp();
    }
}

