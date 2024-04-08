using Shard;

namespace GameTanks
{
    class Health1 : GameObject
    {
        private Tank1 owner;
        public Tank1 Owner { get => owner; }

        public void setupHealth(Tank1 tank, float posX, float posY)
        {
            owner = tank;
            Transform.X = posX;
            Transform.Y = posY;
            Transform.Scalex = 2;
            Transform.Scaley = 2;
        }
    }

    class Health2 : GameObject
    {
        private Tank2 owner;
        public Tank2 Owner { get => owner; }

        public void setupHealth(Tank2 tank, float posX, float posY)
        {
            owner = tank;
            Transform.X = posX;
            Transform.Y = posY;
            Transform.Scalex = 2;
            Transform.Scaley = 2;
        }
    }
}
