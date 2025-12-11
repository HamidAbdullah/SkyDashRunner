using SkyDashRunner.Player;

namespace SkyDashRunner.PowerUps
{
    public class ShieldPowerUp : PowerUp
    {
        [SerializeField] private float shieldDuration = 10f;
        
        protected override void ApplyPowerUp()
        {
            if (player != null)
            {
                player.ActivateShield(shieldDuration);
            }
        }
    }
}

