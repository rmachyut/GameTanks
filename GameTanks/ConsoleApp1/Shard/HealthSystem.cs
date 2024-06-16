// In the Engine namespace (e.g., Shard)

namespace Shard
{
    class Health : GameObject
    {
        private GameObject owner;
        public GameObject Owner { get => owner; }

        public void SetupHealth(GameObject ownerObject, float posX, float posY)
        {
            owner = ownerObject;
            Transform.X = posX;
            Transform.Y = posY;
            Transform.Scalex = 2;
            Transform.Scaley = 2;
        }
    }
}
