using GameTanks;
using SDL2;
using System.Collections.Generic;
using System.Drawing;
namespace Shard
{
    enum GameState
    {
        MainMenu,
        Play,
        End
    }
    class GameTanks : Game, InputListener
    {
        List<Dirt> dirt;
        Floor fl = null;
        Tank1 playerTank1 = null;
        Tank2 playerTank2 = null;
        GameState state = GameState.MainMenu;

        GameObject p1 = null;
        GameObject p2 = null;
        GameObject title = null;

        bool bWinner1 = false;

        private const float delay = 1;
        private float delayCount = 0;

        public override void initialize()
        {
            // TODO:
            Bootstrap.getInput().addListener(this);

            if (state == GameState.MainMenu)
            {
                
                title = new GameObject();
                title.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("TanksTitle.PNG");
                title.Transform.X = 120;
                title.Transform.Y = 200;
                
            }

        }

        public override void update()
        {
            if (state == GameState.MainMenu)
            {
                //Bootstrap.getDisplay().showText("Press SPACE to start", 300, 300, 50, Color.White);
                Bootstrap.getDisplay().addToDraw(title);
                Bootstrap.getDisplay().showTextAlt("Press SPACE to start", "Corbel", 290, 500, 50, 255, 0, 255);

            }
            else if (state == GameState.Play)
            {
                //Bootstrap.getSound().playSound("bgm.wav");  // File too big
                if (p1 == null)
                {
                    p1 = new GameObject();
                    p1.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("NewPlayer1.png");
                    p1.Transform.X = 80;
                    p1.Transform.Y = 300;
                    p1.Transform.Scalex = 2;
                    p1.Transform.Scaley = 2;
                }
                
                if (p2 == null)
                {
                    p2 = new GameObject();
                    p2.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("NewPlayer2.png");
                    p2.Transform.X = 880;
                    p2.Transform.Y = 300;
                    p2.Transform.Scalex = 2;
                    p2.Transform.Scaley = 2;
                }

                Bootstrap.getDisplay().addToDraw(p1);
                Bootstrap.getDisplay().addToDraw(p2);

                if (dirt == null)
                {
                    // Setup walls
                    dirt = new List<Dirt>();
                    setupFloor();
                    setupDirt();
                    // walls.Add(bg);
                }

                // Setup Tanks
                if (playerTank1 == null)
                {
                    playerTank1 = new Tank1();
                    playerTank1.IsDead = false;
                }
                
                if (playerTank2 == null)
                {
                    playerTank2 = new Tank2();
                    playerTank2.IsDead = false;
                }


                if (playerTank1.IsDead || playerTank2.IsDead)
                {
                    if (!playerTank1.IsDead)
                    {
                        bWinner1 = true;
                    }

                    float deltaTime = (float)Bootstrap.getDeltaTime();

                    // Update fire rate counter
                    delayCount += deltaTime;

                    if (delayCount > delay)
                    {
                        state = GameState.End;
                        playerTank1.IsGameEnd = true;
                        playerTank2.IsGameEnd = true;


                        if (!playerTank1.IsDead)
                        {
                            bWinner1 = true;
                        }
                    }
                }
            }
            else if (state == GameState.End)
            {
                if (playerTank1 != null)
                {
                    playerTank1.ToBeDestroyed = true;
                    playerTank1 = null;
                }

                if (playerTank2 != null)
                {
                    playerTank2.ToBeDestroyed = true;
                    playerTank2 = null;
                }

                if (dirt != null)
                {
                    for (int i = 0; i < dirt.Count; i++)
                    {
                        dirt[i].ToBeDestroyed = true;
                        dirt[i] = null;
                    }
                }
                dirt = null;
                if (fl != null)
                {
                    fl.ToBeDestroyed = true;
                    fl = null;
                }


                if (bWinner1)
                {
                    //Debug.Log("P1 wins");
                    Bootstrap.getDisplay().showText("RED Wins!", 370, 320, 70, Color.Red);
                    Bootstrap.getDisplay().showText("press SPACE to restart", 420, 520, 20, Color.White);
                }
                else
                {
                    //Debug.Log("P2 wins");
                    Bootstrap.getDisplay().showText("BLUE Wins!", 350, 320, 70, Color.Blue);
                    Bootstrap.getDisplay().showText("press SPACE to restart", 420, 520, 20, Color.White);
                }
            }
        }

        public void handleInput(InputEvent inp, string eventType)
        {
            // TODO:

            if (state == GameState.MainMenu)
            {
                if (eventType == "KeyDown")
                {
                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE)
                    {
                        //fireBullet();
                        p1 = null;
                        p2 = null;
                        state = GameState.Play;
                    }
                }
            }
            else if (state == GameState.Play)
            {
                // No entry
            }
            else if (state == GameState.End)
            {
                if (eventType == "KeyDown")
                {
                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE)
                    {
                        //fireBullet();
                        p1 = null;
                        p2 = null;
                        state = GameState.MainMenu;
                    }
                }
            }
        }

        public override int getTargetFrameRate()
        {
            return 144;
        }

        ///////////////////////////////////////////////////////////////////////
        // User functions
        private void setupFloor() 
        {
            fl = new Floor();
            fl.Transform.X = 200;
            fl.Transform.Y = 50;
        }
        private void setupDirt()
        {
            // Walls, simple
            // Display is now 1000x700 (Subject to change)
            for (int i = 0; i < 15; i++)   //One dimension - Columns
            {
                for (int j = 0; j < 15; j++)   // Two dimension - Rows
                {
                    if (i == 0 || i == 14 || j == 0 || j == 14)    // Boundaries of arena
                    {
                        Dirt br = new Dirt();
                        br.Transform.X = 200 + (i * 40);
                        br.Transform.Y = 50 + (j * 40);
                        dirt.Add(br);
                    }
                    //obstacles inside the arena
                    //currently for the old 15x15 version
                    else if ((i == 1 && j == 10) || (i == 3 && j == 1) || (i == 3 && j == 2) || (i == 4 && j == 6) || (i == 4 && j == 7) || (i == 4 && j == 8) || (i == 4 && j == 9) || (i == 4 && j == 10) || (i == 4 && j == 11) || (i == 7 && j == 8) || (i == 7 && j == 9) || (i == 7 && j == 10) || (i == 8 && j == 10) || (i == 9 && j == 10) || (i == 10 && j == 10) || (i == 11 && j == 10) || (i == 12 && j == 3) || (i == 13 && j == 3))
                    {
                        Dirt br = new Dirt();
                        br.Transform.X = 200 + (i * 40);
                        br.Transform.Y = 50 + (j * 40);
                        dirt.Add(br);
                    }

                }
            }
        }

    }
}