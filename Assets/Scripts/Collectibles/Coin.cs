namespace SkyDashRunner.Collectibles
{
    public class Coin : Collectible
    {
        protected override void Start()
        {
            base.Start();
            scoreValue = 5;
            coinValue = 1;
        }
    }
}

