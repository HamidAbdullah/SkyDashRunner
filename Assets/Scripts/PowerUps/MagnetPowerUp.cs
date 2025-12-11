using SkyDashRunner.Player;

namespace SkyDashRunner.PowerUps
{
    public class MagnetPowerUp : PowerUp
    {
        [SerializeField] private float magnetDuration = 10f;
        
        protected override void ApplyPowerUp()
        {
            if (player != null)
            {
                player.ActivateMagnet(magnetDuration);
            }
        }
    }
}

