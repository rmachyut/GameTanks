// In the GameTanks namespace

using Shard;

namespace GameTanks
{
    class Health1 : GameObject
    {
        public Health Health { get; private set; }

        public Health1(float initialPosX, float initialPosY)
        {
            Health = new Health();
            Health.SetupHealth(this, initialPosX, initialPosY);
        }
    }

    class Health2 : GameObject
    {
        public Health Health { get; private set; }

        public Health2(float initialPosX, float initialPosY)
        {
            Health = new Health();
            Health.SetupHealth(this, initialPosX, initialPosY);
        }
    }
}
