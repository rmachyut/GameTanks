using SDL2;
using Shard;
using System.Drawing;

namespace GameTest
{
    class Spaceship : GameObject, InputListener, CollisionHandler
    {
        bool up, down, turnLeft, turnRight;


        public override void initialize()
        {

            this.Transform.X = 500.0f;
            this.Transform.Y = 500.0f;
            this.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("spaceship.png");


            Bootstrap.getInput().addListener(this);

            up = false;
            down = false;

            setPhysicsEnabled();

            PhyBody.Mass = 1;
            PhyBody.MaxForce = 10;
            PhyBody.AngularDrag = 0.01f;
            PhyBody.Drag = 0f;
            PhyBody.StopOnCollision = false;
            PhyBody.ReflectOnCollision = false;
            PhyBody.ImpartForce = false;
            PhyBody.Kinematic = false;

            //MyBody.PassThrough = true;
            //MyBody.addCircleCollider(0, 0, 5);
            //MyBody.addCircleCollider(0, 34, 5);
            //MyBody.addCircleCollider(60, 18, 5);
            //MyBody.addCircleCollider();

            PhyBody.addRectCollider();

            addTag("Spaceship");
        }

        public void fireBullet()
        {
            Bullet b = new Bullet();

            b.setupBullet(this, this.Transform.Centre.X, this.Transform.Centre.Y);

            b.Transform.rotate(this.Transform.Rotz);

            Bootstrap.getSound().playSound ("fire.wav");
        }

        public void handleInput(InputEvent inp, string eventType)
        {
            if (eventType == "KeyDown")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_W)
                {
                    up = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_S)
                {
                    down = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                {
                    turnRight = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                {
                    turnLeft = true;
                }

            }
            else if (eventType == "KeyUp")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_W)
                {
                    up = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_S)
                {
                    down = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                {
                    turnRight = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                {
                    turnLeft = false;
                }
            }

            if (eventType == "KeyUp")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE)
                {
                    fireBullet();
                }
            }
        }

        public override void physicsUpdate()
        {
            if (turnLeft)
            {
                PhyBody.addTorque(-0.3f);
            }

            if (turnRight)
            {
                PhyBody.addTorque(0.3f);
            }

            if (up)
            {
                PhyBody.addForce(this.Transform.Forward, 0.5f);
            }

            if (down)
            {
                PhyBody.addForce(this.Transform.Forward, -0.2f);
            }
        }

        public override void update()
        {
            Bootstrap.getDisplay().addToDraw(this);
        }

        public void onCollisionEnter(PhysicsBody x)
        {
            if (x.Parent.checkTag("Bullet") == false)
            {
                PhyBody.DebugColor = Color.Red;
            }
        }

        public void onCollisionExit(PhysicsBody x)
        {
            PhyBody.DebugColor = Color.Green;
        }

        public void onCollisionStay(PhysicsBody x)
        {
            PhyBody.DebugColor = Color.Blue;
        }

        public override string ToString()
        {
            return "Spaceship: [" + Transform.X + ", " + Transform.Y + ", " + Transform.Wid + ", " + Transform.Ht + "]";
        }
    }
}
