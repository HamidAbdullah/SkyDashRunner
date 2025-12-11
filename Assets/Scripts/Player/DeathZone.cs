using UnityEngine;
using SkyDashRunner.Player;
using SkyDashRunner.Managers;

namespace SkyDashRunner.Player
{
    public class DeathZone : MonoBehaviour
    {
        [SerializeField] private float deathY = -10f;
        
        private void Update()
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null && player.Position.y < deathY)
            {
                // Player fell off
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.GameOver();
                }
                
                if (player != null)
                {
                    player.Die();
                }
            }
        }
    }
}

