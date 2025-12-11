namespace SkyDashRunner.Collectibles
{
    public class Orb : Collectible
    {
        protected override void Start()
        {
            base.Start();
            scoreValue = 20;
            coinValue = 2;
        }
    }
}

