using SkyDashRunner.Player;

namespace SkyDashRunner.PowerUps
{
    public class SlowMotionPowerUp : PowerUp
    {
        [SerializeField] private float slowMotionFactor = 0.5f;
        [SerializeField] private float slowMotionDuration = 5f;
        
        protected override void ApplyPowerUp()
        {
            if (player != null)
            {
                player.SetSlowMotion(slowMotionFactor, slowMotionDuration);
            }
        }
    }
}

