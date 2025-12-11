using UnityEngine;

namespace SkyDashRunner.Obstacles
{
    public class RotatingBlade : Obstacle
    {
        [Header("Rotation Settings")]
        [SerializeField] private Vector3 rotationAxis = Vector3.up;
        [SerializeField] private float rotationSpeed = 360f;
        
        protected override void Start()
        {
            base.Start();
            isDeadly = true;
        }
        
        private void Update()
        {
            transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
        }
    }
}

