using SkyDashRunner.Player;

namespace SkyDashRunner.PowerUps
{
    public class DoubleJumpPowerUp : PowerUp
    {
        protected override void ApplyPowerUp()
        {
            if (player != null)
            {
                player.ActivateDoubleJump();
            }
        }
    }
}

