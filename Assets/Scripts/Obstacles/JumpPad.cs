using UnityEngine;
using SkyDashRunner.Player;

namespace SkyDashRunner.Obstacles
{
    public class JumpPad : Obstacle
    {
        [Header("Jump Pad Settings")]
        [SerializeField] private float jumpBoost = 15f;
        [SerializeField] private ParticleSystem boostEffect;
        
        protected override void Start()
        {
            base.Start();
            isDeadly = false;
        }
        
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                BoostPlayer();
            }
        }
        
        private void BoostPlayer()
        {
            if (player != null)
            {
                // Apply upward force
                CharacterController controller = player.GetComponent<CharacterController>();
                if (controller != null)
                {
                    // This would need to be handled in PlayerController
                    // For now, we'll trigger a jump
                    player.Jump();
                }
                
                if (boostEffect != null)
                {
                    boostEffect.Play();
                }
            }
        }
    }
}

