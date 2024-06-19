using Shard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTanks
{
    ///////////////////////////////////////////////////////////////////////
    // MINE 1
    class Mine1 : GameObject, CollisionHandler
    {
        // Owner reference
        private Tank1 owner;

        public Tank1 Owner { get => owner; }
        public void setupMine(Tank1 tank, float posX, float posY)
        {
            // Setup owner
            owner = tank;

            // Setup transform
            Transform.X = posX;
            Transform.Y = posY;

            //// Setup physics
            setPhysicsEnabled();
            PhyBody.Mass = 100.0f;
            //PhyBody.MaxForce = 100.0f;
            PhyBody.Drag = 100;
            //PhyBody.PassThrough = true;

            // Setup colliders
            PhyBody.addRectCollider();

            Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("RedMine.png");

            // Setup tag
            addTag("Mine1");
        }

        public void onCollisionEnter(PhysicsBody x)
        {
            if (x.Parent.checkTag("Tank2") == true)
            {
                Debug.Log("Mine:Hit Tank2");

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
        }
    }

    ///////////////////////////////////////////////////////////////////////
    // MINE 2
    class Mine2 : GameObject, CollisionHandler
    {
        // Owner reference
        private Tank2 owner;

        public Tank2 Owner { get => owner; }
        public void setupMine(Tank2 tank, float posX, float posY)
        {
            // Setup owner
            owner = tank;

            // Setup transform
            Transform.X = posX;
            Transform.Y = posY;

            //// Setup physics
            setPhysicsEnabled();
            PhyBody.Mass = 100.0f;
            //PhyBody.MaxForce = 100.0f;
            PhyBody.Drag = 100;
            //PhyBody.PassThrough = true;

            // Setup colliders
            PhyBody.addRectCollider();

            Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("BlueMine.png");

            // Setup tag
            addTag("Mine2");
        }

        public void onCollisionEnter(PhysicsBody x)
        {
            if (x.Parent.checkTag("Tank1") == true)
            {
                Debug.Log("Mine:Hit Tank1");

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
        }
    }

}
