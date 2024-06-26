using SDL2;
using Shard;
using System.Collections.Generic;
using System.Numerics;

//Health Regen

namespace GameTanks
{
    ///////////////////////////////////////////////////////////////////////
    // TANK 1
    class Tank1 : GameObject, InputListener, CollisionHandler
    {
        // Speed Control
        enum t1speed
        {
            Normal,
            Slow
        }
        t1speed speed = t1speed.Normal;

        // Movement input
        private bool inputUp = false;
        private bool inputDown = false;
        private bool inputLeft = false;
        private bool inputRight = false;

        // Movement data
        private const float forwardSpeed = 150f;
        private const float backwardSpeed = 120f;
        private const float turnRate = 20f;
        private const float slowSpeed = 50f;
        private const float slowTurn = 15f;

        // Shooting data
        private const float fireRate = 1.5f;
        private float fireCounter = 0.0f;
        private const float shootOffset = 30.0f;

        //Spawned Bullets
        List<Bullet1> bullet1Array;

        // Mine data
        //private int mineCounter = 0;
        private const float mineFireRate = 10.0f;
        private float mineFireCounter = 10.0f;
        private const float mineOffset = 40.0f;
        private const float mineAdjust = 10.0f;
        
        // Spawned mine
        List<Mine1> mine1Array;

        // Health data
        private const int maxLives = 3;
        private int currentLives = maxLives;
        private bool bDead = false;
        private bool bGameEnd = false;

        // Health regeneration data
        private const float regenerationRate = 10.0f; // Adjust as needed
        private float regenerationCounter = 0.0f;

        
        // Accessors
        public bool IsDead { get => bDead; set => bDead = value; }
        public bool IsGameEnd { set => bGameEnd = value; }
        public int CurrentLives { get => currentLives; }

        public override void initialize()
        {
            // TODO:

            // Setup position
            Transform.X = 260.0f;
            Transform.Y = 100.0f;
            Transform.rotate(90);


            // Setup sprite
            Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("RedTank.png");
            Transform.Scalex = 1.2f;
            Transform.Scaley = 1.2f;

            // Setup input listener
            Bootstrap.getInput().addListener(this);

            // Default inputs
            inputUp = false;
            inputDown = false;
            inputLeft = false;
            inputRight = false;

            // Default data
            fireCounter = 0.0f;
            mineFireCounter = 10.0f;

            // Setup physics
            setPhysicsEnabled();
            PhyBody.Mass = 150.0f;
            PhyBody.MaxForce = 10;
            PhyBody.AngularDrag = 0.1f;
            PhyBody.Drag = 10f;
            PhyBody.StopOnCollision = false;
            PhyBody.ReflectOnCollision = false;
            PhyBody.ImpartForce = false;
            PhyBody.Kinematic = false;



            // Setup colliders
            PhyBody.addRectCollider();

            // Setup tag
            addTag("Tank1");

            mine1Array = new List<Mine1>();
            bullet1Array = new List<Bullet1>();
        }

        public override void update()
        {
            // TODO:

            // If dead, don't update
            if (bDead)
            {
                return;
            }

            // Store deltaTime
            float deltaTime = (float)Bootstrap.getDeltaTime();

            // Update fire rate counter
            fireCounter += deltaTime;
            mineFireCounter += deltaTime;

            // Render
            Bootstrap.getDisplay().addToDraw(this);

            //Show health
            showHealth();

            // Call health regeneration method
            regenerateHealth();
        }

        public override void physicsUpdate()
        {
            // TODO:

            // If dead or end of game, don't update
            if (bDead || bGameEnd)
            {
                return;
            }

            // Tank movement
            if (speed == t1speed.Normal)
            {
                if (inputLeft)
                {
                    PhyBody.addTorque(-turnRate);
                }

                if (inputRight)
                {
                    PhyBody.addTorque(turnRate);
                }

                if (inputUp)
                {
                    PhyBody.addForce(this.Transform.Forward, forwardSpeed);
                }

                if (inputDown)
                {
                    PhyBody.addForce(this.Transform.Forward, -backwardSpeed);
                }
            }
            else if (speed == t1speed.Slow)
            {
                if (inputLeft)
                {
                    PhyBody.addTorque(-slowTurn);
                }

                if (inputRight)
                {
                    PhyBody.addTorque(slowTurn);
                }

                if (inputUp)
                {
                    PhyBody.addForce(this.Transform.Forward, slowSpeed);
                }

                if (inputDown)
                {
                    PhyBody.addForce(this.Transform.Forward, -slowSpeed);
                }
            }

        }

        public void handleInput(InputEvent inp, string eventType)
        {
            // TODO:

            // If dead, don't check for input??
            if (bDead || bGameEnd)
            {
                return;
            }
            // Player 1 controls
            if (eventType == "KeyDown")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_W)
                {
                    inputUp = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_S)
                {
                    inputDown = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                {
                    inputRight = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                {
                    inputLeft = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE)
                {
                    fireBullet();
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_E)
                {
                    dropMine();
                }
            }
            else if (eventType == "KeyUp")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_W)
                {
                    inputUp = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_S)
                {
                    inputDown = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                {
                    inputRight = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                {
                    inputLeft = false;
                }
            }
        }

        public void onCollisionEnter(PhysicsBody x)
        {
            // Reduce health on hit
            if (x.Parent.checkTag("Bullet2") == true || x.Parent.checkTag("Mine2") == true)
            {
                currentLives--;
                if (currentLives <= 0)
                {
                    bDead = true;
                    this.ToBeDestroyed = true;
                }
            }

            // Collision with Tank 2
            if (x.Parent.checkTag("Tank2") == true)
            {
                x.stopForces();  //Not doing much
            }
        }

        public void onCollisionExit(PhysicsBody x)
        {
            speed = t1speed.Normal;
        }

        public void onCollisionStay(PhysicsBody x)
        {
            if (x.Parent.checkTag("Mine1") == true || x.Parent.checkTag("Mine2") == true)
            {
                speed = t1speed.Normal;
            }
            else
            {
                speed = t1speed.Slow;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        // User functions
        public void fireBullet()
        {
            // Shooting delay
            if (fireCounter < fireRate)
            {
                return;
            }

            // Actually fire
            Bullet1 bullet = new Bullet1();

            Vector2 cen = Transform.Centre;
            Vector2 spawnLocation = shootOffset * Transform.Forward;
            spawnLocation += cen;

            bullet.setupBullet(this, spawnLocation.X, spawnLocation.Y);
            bullet.Transform.rotate(Transform.Rotz);

            // Reset fire rate counter
            fireCounter = 0.0f;

            bullet1Array.Add(bullet);
            // Play sound
            Bootstrap.getSound().playSound("fire.wav");
        }

        // Health Bar
        public void showHealth()
        {
            Health h1 = new Health();
            h1.SetupHealth(this, 50, 350);

            if (currentLives == 3)
            {
                h1.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("Red3Health.png");
                Bootstrap.getDisplay().addToDraw(h1);
            }
            else if (currentLives == 2)
            {
                h1.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("Red2Health.png");
                Bootstrap.getDisplay().addToDraw(h1);
            }
            else if (currentLives == 1)
            {
                h1.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("Red1Health.png");
                Bootstrap.getDisplay().addToDraw(h1);
            }
            else if (currentLives == 0)
            {
                h1.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("0Health.png");
                Bootstrap.getDisplay().addToDraw(h1);
            }
        }
        public void dropMine()
        {
            //if (mineCounter > 1)
            //{
            //    return;
            //}

            // Mine delay (try to get a mine counter instead)
            if (mineFireCounter < mineFireRate)
            {
                return;
            }

            // Actually fire
            Mine1 mine = new Mine1();

            //Transform.recalculateCentre();
            Vector2 cen = Transform.Centre;
            Vector2 spawnLocation = (mineOffset * -Transform.Forward); // + (mineAdjust * Transform.Right);
            spawnLocation += cen;

            mine.setupMine(this, spawnLocation.X, spawnLocation.Y);

            //mineCounter = 1;
            mineFireCounter = 0.0f;

            mine1Array.Add(mine);

            // Play sound
            //Bootstrap.getSound().playSound("fire.wav");
        }

        public void clearBullets()
        {
            for (int i = 0; i < bullet1Array.Count; i++)
            {
                if (bullet1Array[i] != null)
                {
                    bullet1Array[i].ToBeDestroyed = true;
                    bullet1Array[i] = null;
                }
            }

            bullet1Array.Clear();
            bullet1Array = null;
        }

        public void clearMines()
        {
            for (int i = 0; i < mine1Array.Count; i++)
            {
                if (mine1Array[i] != null)
                {
                    mine1Array[i].ToBeDestroyed = true;
                    mine1Array[i] = null;
                }
            }

            mine1Array.Clear();
            mine1Array = null;
        }

        // Health regeneration method
        private void regenerateHealth()
        {
            // Increment regeneration counter
            regenerationCounter += (float)Bootstrap.getDeltaTime();

            // Check if it's time to regenerate health
            if (regenerationCounter >= regenerationRate)
            {
                // Regenerate health here
                if (currentLives < maxLives)
                {
                    currentLives++; // Increase health by 1 (adjust as needed)
                }

                // Reset regeneration counter
                regenerationCounter = 0.0f;
            }
        }
    }



    ///////////////////////////////////////////////////////////////////////
    // TANK 2
    class Tank2 : GameObject, InputListener, CollisionHandler
    {
        // Speed control
        enum t2speed
        {
            Normal,
            Slow
        }
        t2speed speed = t2speed.Normal;

        // Movement input
        private bool inputUp = false;
        private bool inputDown = false;
        private bool inputLeft = false;
        private bool inputRight = false;

        // Movement data
        private const float forwardSpeed = 150f;
        private const float backwardSpeed = 120f;
        private const float turnRate = 20f;
        private const float slowSpeed = 50f;
        private const float slowTurn = 15f;

        // Shooting data
        private const float fireRate = 1.5f;
        private float fireCounter = 0.0f;
        private const float shootOffset = 30.0f;

        //Spawned bullets
        List<Bullet2> bullet2Array;

        // Mine data
        //private int mineCounter = 0;
        private const float mineFireRate = 10.0f;
        private float mineFireCounter = 10.0f;
        private const float mineOffset = 40.0f;
        private const float mineAdjust = 10.0f;

        // Spawned mine
        List<Mine2> mine2Array;

        // Health data
        private const int maxLives = 3;
        private int currentLives = maxLives;
        private bool bDead = false;
        private bool bGameEnd = false;

        // Health regeneration data
        private const float regenerationRate = 10.0f; // Adjust as needed
        private float regenerationCounter = 0.0f;

        // Accessors
        public bool IsDead { get => bDead; set => bDead = value; }
        public bool IsGameEnd { set => bGameEnd = value; }
        public int CurrentLives { get => currentLives; }

        public override void initialize()
        {
            // TODO:

            // Setup position
            Transform.X = 700.0f;
            Transform.Y = 550.0f;
            Transform.rotate(-90);

            ///////////////// Position to fight quickly

            /*Transform.X = 250.0f;
            Transform.Y = 200.0f;
            Transform.rotate(-90);
            */

            // Setup sprite
            Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("BlueTank.png");
            Transform.Scalex = 1.2f;
            Transform.Scaley = 1.2f;

            // Setup input listener
            Bootstrap.getInput().addListener(this);

            // Default inputs
            inputUp = false;
            inputDown = false;
            inputLeft = false;
            inputRight = false;

            // Default data
            fireCounter = 0.0f;
            mineFireCounter = 10.0f;

            setPhysicsEnabled();
            PhyBody.Mass = 150.0f;
            PhyBody.MaxForce = 10;
            PhyBody.AngularDrag = 0.1f;
            PhyBody.Drag = 10f;
            PhyBody.StopOnCollision = false;
            PhyBody.ReflectOnCollision = false;
            PhyBody.ImpartForce = false;
            PhyBody.Kinematic = false;

            // Setup colliders
            PhyBody.addRectCollider();

            // Setup tag
            addTag("Tank2");

            mine2Array = new List<Mine2>();
            bullet2Array = new List<Bullet2>();
        }

        public override void update()
        {

            // If dead, don't update
            if (bDead)
            {
                return;
            }

            // Store deltaTime
            float deltaTime = (float)Bootstrap.getDeltaTime();

            // Update fire rate counter
            fireCounter += deltaTime;
            mineFireCounter += deltaTime;

            // Render
            Bootstrap.getDisplay().addToDraw(this);

            //Show health
            showHealth();

            // Call health regeneration method
            regenerateHealth();
        }

        public override void physicsUpdate()
        {

            // If dead or end of game, don't update
            if (bDead || bGameEnd)
            {
                return;
            }

            // Tank movement
            if (speed == t2speed.Normal)
            {
                if (inputLeft)
                {
                    PhyBody.addTorque(-turnRate);
                }

                if (inputRight)
                {
                    PhyBody.addTorque(turnRate);
                }

                if (inputUp)
                {
                    PhyBody.addForce(this.Transform.Forward, forwardSpeed);
                }

                if (inputDown)
                {
                    PhyBody.addForce(this.Transform.Forward, -backwardSpeed);
                }
            }
            else if (speed == t2speed.Slow)
            {
                if (inputLeft)
                {
                    PhyBody.addTorque(-slowTurn);
                }

                if (inputRight)
                {
                    PhyBody.addTorque(slowTurn);
                }

                if (inputUp)
                {
                    PhyBody.addForce(this.Transform.Forward, slowSpeed);
                }

                if (inputDown)
                {
                    PhyBody.addForce(this.Transform.Forward, -slowSpeed);
                }
            }
        }

        public void handleInput(InputEvent inp, string eventType)
        {

            // If dead, don't check for input??
            if (bDead)
            {
                return;
            }

            // Player 2 controls
            if (eventType == "KeyDown")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_UP)
                {
                    inputUp = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_DOWN)
                {
                    inputDown = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_RIGHT)
                {
                    inputRight = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_LEFT)
                {
                    inputLeft = true;
                }
                
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_RETURN)
                {
                    fireBullet();
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SLASH)
                {
                    dropMine();
                }
            }

            else if (eventType == "KeyUp")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_UP)
                {
                    inputUp = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_DOWN)
                {
                    inputDown = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_RIGHT)
                {
                    inputRight = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_LEFT)
                {
                    inputLeft = false;
                }
            }
        }

        public void onCollisionEnter(PhysicsBody x)
        {
            if (x.Parent.checkTag("Bullet1") == true || x.Parent.checkTag("Mine1") == true)
            {

                currentLives--;
                if (currentLives <= 0)
                {
                    bDead = true;
                    this.ToBeDestroyed = true;
                }

            }

            // Collision with Tank 1
            if (x.Parent.checkTag("Tank1") == true)
            {
                x.stopForces();
            }
        }

        public void onCollisionExit(PhysicsBody x)
        {
            speed = t2speed.Normal;
        }

        public void onCollisionStay(PhysicsBody x)
        {
            if (x.Parent.checkTag("Mine1") == true || x.Parent.checkTag("Mine2") == true)
            {
                speed = t2speed.Normal;
            }
            else
            {
                speed = t2speed.Slow;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        // User functions
        public void fireBullet()
        {
            if (fireCounter < fireRate)
            {
                return;
            }

            // Actually fire
            Bullet2 bullet = new Bullet2();

            Vector2 cen = Transform.Centre;
            Vector2 spawnLocation = shootOffset * Transform.Forward;
            spawnLocation += cen;

            bullet.setupBullet(this, spawnLocation.X, spawnLocation.Y);
            bullet.Transform.rotate(Transform.Rotz);

            // Reset fire rate counter
            fireCounter = 0.0f;

            bullet2Array.Add(bullet);

            // Play sound
            Bootstrap.getSound().playSound("fire.wav");
        }

        public void showHealth()
        {
            Health h2 = new Health();
            h2.SetupHealth(this, 850, 350);

            if (currentLives == 3)
            {
                h2.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("Blue3Health.png");
                Bootstrap.getDisplay().addToDraw(h2);
            }
            else if (currentLives == 2)
            {
                h2.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("Blue2Health.png");
                Bootstrap.getDisplay().addToDraw(h2);
            }
            else if (currentLives == 1)
            {
                h2.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("Blue1Health.png");
                Bootstrap.getDisplay().addToDraw(h2);
            }
            else if (currentLives == 0)
            {
                h2.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("0Health.png");
                Bootstrap.getDisplay().addToDraw(h2);
            }
        }

        public void dropMine()
        {
            if (mineFireCounter < mineFireRate)
            {
                return;
            }

            // Actually fire
            Mine2 mine = new Mine2();

            Vector2 cen = Transform.Centre;
            Vector2 spawnLocation = (mineOffset * -Transform.Forward); // + (mineAdjust * -Transform.Right);
            spawnLocation += cen;

            mine.setupMine(this, spawnLocation.X, spawnLocation.Y);

            // Reset fire rate counter
            //fireCounter = 0.0f;
            mineFireCounter = 0.0f;

            mine2Array.Add(mine);

            // Play sound
            //Bootstrap.getSound().playSound("fire.wav");
        }

        public void clearBullets()
        {
            for (int i = 0; i < bullet2Array.Count; i++)
            {
                if (bullet2Array[i] != null)
                {
                    bullet2Array[i].ToBeDestroyed = true;
                    bullet2Array[i] = null;
                }
            }

            bullet2Array.Clear();
            bullet2Array = null;
        }
        public void clearMines()
        {
            for (int i = 0; i < mine2Array.Count; i++)
            {
                if (mine2Array[i] != null)
                {
                    mine2Array[i].ToBeDestroyed = true;
                    mine2Array[i] = null;
                }
            }

            mine2Array.Clear();
            mine2Array = null;
        }


    // Health regeneration method
    private void regenerateHealth()
        {
            // Increment regeneration counter
            regenerationCounter += (float)Bootstrap.getDeltaTime();

            // Check if it's time to regenerate health
            if (regenerationCounter >= regenerationRate)
            {
                // Regenerate health here
                if (currentLives < maxLives)
                {
                    currentLives++; // Increase health by 1 
                }

                // Reset regeneration counter
                regenerationCounter = 0.0f;
            }
        }
    }
}