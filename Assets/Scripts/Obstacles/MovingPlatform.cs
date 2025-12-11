using UnityEngine;

namespace SkyDashRunner.Obstacles
{
    public class MovingPlatform : Obstacle
    {
        [Header("Movement Settings")]
        [SerializeField] private Vector3 movementDirection = Vector3.right;
        [SerializeField] private float movementSpeed = 3f;
        [SerializeField] private float movementDistance = 5f;
        
        private Vector3 startPosition;
        private float movementProgress = 0f;
        
        protected override void Start()
        {
            base.Start();
            startPosition = transform.position;
            isDeadly = false; // Moving platforms are not deadly
        }
        
        private void Update()
        {
            movementProgress += movementSpeed * Time.deltaTime;
            float sinValue = Mathf.Sin(movementProgress);
            transform.position = startPosition + movementDirection * sinValue * movementDistance;
        }
        
        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // Move player with platform
                collision.transform.position += movementDirection * movementSpeed * Time.deltaTime;
            }
        }
    }
}

