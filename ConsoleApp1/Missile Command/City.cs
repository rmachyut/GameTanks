using Shard;

namespace MissileCommand
{
    class City : GameObject, CollisionHandler
    {

        public override void initialize()
        {
            this.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("city.png");

            setPhysicsEnabled();

            PhyBody.addCircleCollider(64, 64, 64);

            addTag("City");

            PhyBody.Kinematic = true;

        }


        public override void update()
        {

            Bootstrap.getDisplay().addToDraw(this);

        }

        public void onCollisionEnter(PhysicsBody x)
        {

        }

        public void onCollisionExit(PhysicsBody x)
        {
        }

        public void onCollisionStay(PhysicsBody x)
        {
        }

        public override string ToString()
        {
            return "City: [" + Transform.X + ", " + Transform.Y + "]";
        }


    }
}
