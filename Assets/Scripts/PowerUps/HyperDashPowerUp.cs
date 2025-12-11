using SkyDashRunner.Player;

namespace SkyDashRunner.PowerUps
{
    public class HyperDashPowerUp : PowerUp
    {
        [SerializeField] private float hyperDashDuration = 8f;
        
        protected override void ApplyPowerUp()
        {
            if (player != null)
            {
                player.ActivateHyperDash(hyperDashDuration);
            }
        }
    }
}

