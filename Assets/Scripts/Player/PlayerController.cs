using UnityEngine;
using UnityEngine.InputSystem;

namespace SkyDashRunner.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float forwardSpeed = 10f;
        [SerializeField] private float sideSpeed = 8f;
        [SerializeField] private float jumpForce = 8f;
        [SerializeField] private float gravity = -20f;
        [SerializeField] private float slideDuration = 1f;
        [SerializeField] private float dashForce = 15f;
        [SerializeField] private float dashDuration = 0.3f;
        
        [Header("Ground Check")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundDistance = 0.4f;
        [SerializeField] private LayerMask groundMask;
        
        [Header("References")]
        [SerializeField] private Transform playerModel;
        [SerializeField] private ParticleSystem dashParticles;
        [SerializeField] private ParticleSystem jumpParticles;
        
        private CharacterController controller;
        private Animator animator;
        private Vector3 velocity;
        private float currentForwardSpeed;
        private float targetLane = 0f; // -1 = left, 0 = center, 1 = right
        private float laneDistance = 3f;
        private bool isGrounded;
        private bool isSliding = false;
        private bool isDashing = false;
        private bool canDoubleJump = false;
        private bool hasDoubleJump = false;
        private float slideTimer = 0f;
        private float dashTimer = 0f;
        
        // Power-up states
        private bool isHyperDashActive = false;
        private bool isShieldActive = false;
        private bool isMagnetActive = false;
        private float slowMotionFactor = 1f;
        
        // Input
        private Vector2 moveInput;
        private bool jumpPressed;
        private bool slidePressed;
        private bool dashPressed;
        
        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            currentForwardSpeed = forwardSpeed;
            
            if (groundCheck == null)
            {
                GameObject groundCheckObj = new GameObject("GroundCheck");
                groundCheckObj.transform.SetParent(transform);
                groundCheckObj.transform.localPosition = new Vector3(0, -0.9f, 0);
                groundCheck = groundCheckObj.transform;
            }
        }
        
        private void Update()
        {
            HandleGroundCheck();
            HandleMovement();
            HandleGravity();
            HandleAnimations();
        }
        
        private void HandleGroundCheck()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
                canDoubleJump = false;
            }
        }
        
        private void HandleMovement()
        {
            // Forward movement (automatic)
            float speedMultiplier = isHyperDashActive ? 2f : 1f;
            speedMultiplier *= slowMotionFactor;
            float finalForwardSpeed = currentForwardSpeed * speedMultiplier;
            
            // Side movement (lane switching)
            float targetX = targetLane * laneDistance;
            float currentX = transform.position.x;
            float newX = Mathf.Lerp(currentX, targetX, sideSpeed * Time.deltaTime);
            
            // Apply movement
            Vector3 move = new Vector3(newX - currentX, 0, finalForwardSpeed * Time.deltaTime);
            
            // Handle sliding
            if (isSliding)
            {
                slideTimer -= Time.deltaTime;
                if (slideTimer <= 0f)
                {
                    isSliding = false;
                    controller.height = 2f;
                    controller.center = new Vector3(0, 0, 0);
                }
            }
            
            // Handle dashing
            if (isDashing)
            {
                dashTimer -= Time.deltaTime;
                move.z += dashForce * Time.deltaTime;
                if (dashTimer <= 0f)
                {
                    isDashing = false;
                }
            }
            
            controller.Move(move);
        }
        
        private void HandleGravity()
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        
        private void HandleAnimations()
        {
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetBool("IsSliding", isSliding);
            animator.SetBool("IsDashing", isDashing);
            animator.SetFloat("Speed", currentForwardSpeed);
        }
        
        // Input Methods (called by Input System)
        public void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>();
            
            // Lane switching
            if (moveInput.x > 0.5f && targetLane < 1f)
            {
                targetLane += 1f;
            }
            else if (moveInput.x < -0.5f && targetLane > -1f)
            {
                targetLane -= 1f;
            }
        }
        
        public void OnJump(InputValue value)
        {
            if (value.isPressed)
            {
                Jump();
            }
        }
        
        public void OnSlide(InputValue value)
        {
            if (value.isPressed && !isSliding)
            {
                Slide();
            }
        }
        
        public void OnDash(InputValue value)
        {
            if (value.isPressed && !isDashing)
            {
                Dash();
            }
        }
        
        // Touch/Swipe Controls
        public void MoveLeft()
        {
            if (targetLane > -1f)
            {
                targetLane -= 1f;
            }
        }
        
        public void MoveRight()
        {
            if (targetLane < 1f)
            {
                targetLane += 1f;
            }
        }
        
        public void Jump()
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                canDoubleJump = true;
                if (jumpParticles != null) jumpParticles.Play();
            }
            else if (hasDoubleJump && canDoubleJump)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                canDoubleJump = false;
                hasDoubleJump = false;
                if (jumpParticles != null) jumpParticles.Play();
            }
        }
        
        public void Slide()
        {
            if (isGrounded && !isSliding)
            {
                isSliding = true;
                slideTimer = slideDuration;
                controller.height = 1f;
                controller.center = new Vector3(0, -0.5f, 0);
            }
        }
        
        public void Dash()
        {
            if (!isDashing)
            {
                isDashing = true;
                dashTimer = dashDuration;
                if (dashParticles != null) dashParticles.Play();
            }
        }
        
        // Power-up Methods
        public void ActivateHyperDash(float duration)
        {
            isHyperDashActive = true;
            Invoke(nameof(DeactivateHyperDash), duration);
        }
        
        private void DeactivateHyperDash()
        {
            isHyperDashActive = false;
        }
        
        public void ActivateShield(float duration)
        {
            isShieldActive = true;
            Invoke(nameof(DeactivateShield), duration);
        }
        
        private void DeactivateShield()
        {
            isShieldActive = false;
        }
        
        public void ActivateMagnet(float duration)
        {
            isMagnetActive = true;
            Invoke(nameof(DeactivateMagnet), duration);
        }
        
        private void DeactivateMagnet()
        {
            isMagnetActive = false;
        }
        
        public void ActivateDoubleJump()
        {
            hasDoubleJump = true;
        }
        
        public void SetSlowMotion(float factor, float duration)
        {
            slowMotionFactor = factor;
            Invoke(nameof(ResetSlowMotion), duration);
        }
        
        private void ResetSlowMotion()
        {
            slowMotionFactor = 1f;
        }
        
        // Getters
        public bool IsShieldActive => isShieldActive;
        public bool IsMagnetActive => isMagnetActive;
        public float CurrentSpeed => currentForwardSpeed;
        public Vector3 Position => transform.position;
        
        // Death handling
        public void Die()
        {
            animator.SetTrigger("Die");
            currentForwardSpeed = 0f;
            enabled = false;
        }
        
        private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
            }
        }
    }
}

