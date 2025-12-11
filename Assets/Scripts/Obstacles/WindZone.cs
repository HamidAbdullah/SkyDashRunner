using UnityEngine;
using SkyDashRunner.Player;

namespace SkyDashRunner.Obstacles
{
    public class WindZone : Obstacle
    {
        [Header("Wind Settings")]
        [SerializeField] private Vector3 windDirection = Vector3.right;
        [SerializeField] private float windForce = 5f;
        [SerializeField] private ParticleSystem windEffect;
        
        protected override void Start()
        {
            base.Start();
            isDeadly = false;
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ApplyWindForce();
            }
        }
        
        private void ApplyWindForce()
        {
            if (player != null)
            {
                // Apply wind force to player
                CharacterController controller = player.GetComponent<CharacterController>();
                if (controller != null)
                {
                    Vector3 windMovement = windDirection * windForce * Time.deltaTime;
                    controller.Move(windMovement);
                }
            }
        }
    }
}

