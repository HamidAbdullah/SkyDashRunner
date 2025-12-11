using UnityEngine;
using SkyDashRunner.Player;

namespace SkyDashRunner.Managers
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0, 5, -10);
        [SerializeField] private float smoothSpeed = 0.125f;
        [SerializeField] private float lookAheadDistance = 5f;
        
        [Header("Camera Shake")]
        [SerializeField] private float shakeDuration = 0.3f;
        [SerializeField] private float shakeMagnitude = 0.1f;
        
        private Vector3 velocity = Vector3.zero;
        private float shakeTimer = 0f;
        private Vector3 originalPosition;
        private PlayerController player;
        
        private void Start()
        {
            if (target == null)
            {
                player = FindObjectOfType<PlayerController>();
                if (player != null)
                {
                    target = player.transform;
                }
            }
            
            originalPosition = transform.localPosition;
        }
        
        private void LateUpdate()
        {
            if (target == null) return;
            
            // Calculate desired position
            Vector3 desiredPosition = target.position + offset;
            desiredPosition.z += lookAheadDistance;
            
            // Smooth follow
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
            transform.position = smoothedPosition;
            
            // Look at target
            transform.LookAt(target.position + Vector3.up * 2f);
            
            // Handle camera shake
            if (shakeTimer > 0f)
            {
                shakeTimer -= Time.deltaTime;
                Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
                transform.localPosition = originalPosition + shakeOffset;
                
                if (shakeTimer <= 0f)
                {
                    transform.localPosition = originalPosition;
                }
            }
        }
        
        public void ShakeCamera(float duration = 0.3f, float magnitude = 0.1f)
        {
            shakeTimer = duration;
            shakeMagnitude = magnitude;
        }
        
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }
}

