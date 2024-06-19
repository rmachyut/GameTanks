// In the GameTanks namespace

using Shard;

namespace GameTanks
{
    class Health1 : GameObject
    {
        public Health Player1 { get; private set; }

        public Health1(float initialPosX, float initialPosY)
        {
            Player1 = new Health();
            Player1.SetupHealth(this, initialPosX, initialPosY);
        }
    }

    class Health2 : GameObject
    {
        public Health Player2 { get; private set; }

        public Health2(float initialPosX, float initialPosY)
        {
            Player2 = new Health();
            Player2.SetupHealth(this, initialPosX, initialPosY);
        }
    }
}
