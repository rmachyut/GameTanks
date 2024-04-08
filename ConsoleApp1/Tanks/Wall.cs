using Shard;

namespace GameTanks
{
    class Dirt : GameObject, CollisionHandler
    {
        public void onCollisionEnter(PhysicsBody x)
        {
            if ((x.Parent.checkTag("Tank1") == true) || (x.Parent.checkTag("Tank2") == true))
            {
                x.stopForces();
            }
        }

        public void onCollisionExit(PhysicsBody x)
        {
        }

        public void onCollisionStay(PhysicsBody x)
        {
            if ((x.Parent.checkTag("Tank1") == true) || (x.Parent.checkTag("Tank2") == true))
            {
                x.stopForces();
            }
        }

        public override void initialize()
        {
            // Setup sprite
            Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("dirt.png");
            this.Transform.Scalex = 2.0f;
            this.Transform.Scaley = 2.0f;

            // Setup physics
            setPhysicsEnabled();
            PhyBody.Mass = 450.0f;
            PhyBody.Kinematic = true;

            // Setup collider
            PhyBody.addRectCollider();

            // Set tag
            addTag("Dirt");
        }

        public override void update()
        {
            // Render
            Bootstrap.getDisplay().addToDraw(this);
        }
    }

    class Floor : GameObject, CollisionHandler
    {
        public void onCollisionEnter(PhysicsBody x)
        {
        }

        public void onCollisionExit(PhysicsBody x)
        {
        }

        public void onCollisionStay(PhysicsBody x)
        {
        }

        public override void initialize()
        {
            // Setup sprite
            Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("floor.png");
            this.Transform.Scalex = 30.0f;
            this.Transform.Scaley = 30.0f;

            // Setup physics
            setPhysicsEnabled();
            PhyBody.Mass = 450.0f;
            PhyBody.Kinematic = true;

            // Setup collider

            // Set tag
            addTag("Floor");
        }

        public override void update()
        {
            // Render
            Bootstrap.getDisplay().addToDraw(this);
        }
    }
}
