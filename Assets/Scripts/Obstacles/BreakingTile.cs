using UnityEngine;
using System.Collections;

namespace SkyDashRunner.Obstacles
{
    public class BreakingTile : Obstacle
    {
        [Header("Breaking Settings")]
        [SerializeField] private float breakDelay = 0.5f;
        [SerializeField] private float respawnTime = 3f;
        [SerializeField] private ParticleSystem breakEffect;
        
        private bool isBroken = false;
        private Vector3 originalPosition;
        private Renderer tileRenderer;
        private Collider tileCollider;
        
        protected override void Start()
        {
            base.Start();
            originalPosition = transform.position;
            tileRenderer = GetComponent<Renderer>();
            tileCollider = GetComponent<Collider>();
            isDeadly = false; // Breaking tiles are not deadly, just obstacles
        }
        
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isBroken)
            {
                StartCoroutine(BreakTile());
            }
        }
        
        private IEnumerator BreakTile()
        {
            isBroken = true;
            
            // Wait for break delay
            yield return new WaitForSeconds(breakDelay);
            
            // Play break effect
            if (breakEffect != null)
            {
                breakEffect.Play();
            }
            
            // Disable tile
            if (tileRenderer != null) tileRenderer.enabled = false;
            if (tileCollider != null) tileCollider.enabled = false;
            
            // Respawn after delay
            yield return new WaitForSeconds(respawnTime);
            
            // Reset tile
            transform.position = originalPosition;
            if (tileRenderer != null) tileRenderer.enabled = true;
            if (tileCollider != null) tileCollider.enabled = true;
            isBroken = false;
        }
    }
}

