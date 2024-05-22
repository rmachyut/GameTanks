using Shard;

namespace GameTanks
{
    ///////////////////////////////////////////////////////////////////////
    // BULLET 1
    class Bullet1 : GameObject, CollisionHandler
    {
        // Owner reference
        private Tank1 owner;

        // Bullet speed
        private const float bulletSpeed = 600.0f;

        public Tank1 Owner { get => owner; }
        public void setupBullet(Tank1 tank, float posX, float posY)
        {
            // Setup owner
            owner = tank;

            // Setup transform
            Transform.X = posX;
            Transform.Y = posY;
            
            // Setup physics
            setPhysicsEnabled();
            PhyBody.Mass = 100.0f;
            PhyBody.MaxForce = 100.0f;
            PhyBody.Drag = 10;
            PhyBody.PassThrough = true;

            // Setup colliders
            PhyBody.addRectCollider();

            Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("ball.png");
            
            // Setup tag
            addTag("Bullet1");
        }

        public void onCollisionEnter(PhysicsBody x)
        {
            if (x.Parent.checkTag("Tank2") == true)
            {
                Debug.Log("Hit Tank2");

                ToBeDestroyed = true;

                //}
            }

            if (x.Parent.checkTag("Dirt") == true)
            {
                Debug.Log("Hit Dirt");
                ToBeDestroyed = true;
            }
        }

        public void onCollisionExit(PhysicsBody x)
        {
        }

        public void onCollisionStay(PhysicsBody x)
        {
        }

        public override void initialize()
        {
            Transient = true;
        }

        public override void update()
        {
            Bootstrap.getDisplay().addToDraw(this);
        }

        public override void physicsUpdate()
        {
            PhyBody.addForce(this.Transform.Forward, bulletSpeed);
        }
    }
    


    ///////////////////////////////////////////////////////////////////////
    // BULLET 2
    class Bullet2 : GameObject, CollisionHandler
    {
        // Owner reference
        private Tank2 owner;

        // Bullet speed
        private const float bulletSpeed = 600.0f;

        public Tank2 Owner { get => owner; }

        public void setupBullet(Tank2 tank, float posX, float posY)
        {
            // Setup owner
            owner = tank;

            // Setup transform
            Transform.X = posX;
            Transform.Y = posY;
            Transform.Wid = 10;
            Transform.Ht = 10;

            // Setup physics
            setPhysicsEnabled();
            PhyBody.Mass = 100.0f;
            PhyBody.MaxForce = 100.0f;
            PhyBody.Drag = 10;
            PhyBody.PassThrough = true;

            // Setup colliders
            PhyBody.addRectCollider();

            Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("ball.png");

            // Setup tag
            addTag("Bullet2");
        }

        public void onCollisionEnter(PhysicsBody x)
        {
            if (x.Parent.checkTag("Tank1") == true)
            {
                Debug.Log("Hit Tank1");
                ToBeDestroyed = true;
            }
            
            if (x.Parent.checkTag("Dirt") == true)
            {
                Debug.Log("Hit Dirt");
                ToBeDestroyed = true;
            }
        }

        public void onCollisionExit(PhysicsBody x)
        {
        }

        public void onCollisionStay(PhysicsBody x)
        {
        }

        public override void initialize()
        {
            Transient = true;
        }

        public override void update()
        {
            Bootstrap.getDisplay().addToDraw(this);
        }

        public override void physicsUpdate()
        {
            PhyBody.addForce(this.Transform.Forward, bulletSpeed);
        }
    }
}
