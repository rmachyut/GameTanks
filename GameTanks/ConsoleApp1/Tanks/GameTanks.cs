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
        bool bWinner2 = false;

        private const float delay = 1;
        private float delayCount = 0;
        private const float gameDuration = 5 * 60;
        private float elapsedTime = 0;

        public override void initialize()
        {
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
                Bootstrap.getDisplay().addToDraw(title);
                Bootstrap.getDisplay().showTextAlt("Press SPACE to start", "Corbel", 290, 500, 50, 255, 0, 255);
            }
            else if (state == GameState.Play)
            {
                elapsedTime += (float)Bootstrap.getDeltaTime();
                float remainingTime = gameDuration - elapsedTime;
                int minutes = (int)(remainingTime / 60);
                int seconds = (int)(remainingTime % 60);
                string timeText = $"{minutes:00}:{seconds:00}";

                // Display the remaining time
                Bootstrap.getDisplay().showTextAlt(timeText, "Corbel", 450, 8, 30, 255, 255, 255);

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
                    dirt = new List<Dirt>();
                    setupFloor();
                    setupDirt();
                }

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

                if (playerTank1.IsDead || playerTank2.IsDead || elapsedTime >= gameDuration)
                {
                    if (!playerTank1.IsDead && playerTank2.IsDead)
                    {
                        bWinner1 = true;
                    }
                    else if (playerTank1.IsDead && !playerTank2.IsDead)
                    {
                        bWinner2 = true;
                    }

                    
                    playerTank1.IsGameEnd = true;
                    playerTank2.IsGameEnd = true;
                    state = GameState.End;
                }
            }
            else if (state == GameState.End)
            {
                if (playerTank1 != null)
                {
                    playerTank1.clearMines();
                    playerTank1.clearBullets();
                    playerTank1.ToBeDestroyed = true;
                    playerTank1 = null;
                }

                if (playerTank2 != null)
                {
                    playerTank2.clearMines();
                    playerTank2.clearBullets();
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

                Bootstrap.getDisplay().clearDisplay();

                if (bWinner1)
                {
                    Bootstrap.getDisplay().showText("RED Wins!", 370, 320, 70, Color.Red);
                    Bootstrap.getDisplay().showText("press SPACE to restart", 420, 520, 20, Color.White);
                }
                else if (bWinner2)
                {
                    Bootstrap.getDisplay().showText("BLUE Wins!", 350, 320, 70, Color.Blue);
                    Bootstrap.getDisplay().showText("press SPACE to restart", 420, 520, 20, Color.White);
                }
                else
                {
                    Bootstrap.getDisplay().showText("DRAW", 420, 320, 70, Color.Purple);
                    Bootstrap.getDisplay().showText("press SPACE to restart", 420, 520, 20, Color.White);
                }
            }
        }


        public void handleInput(InputEvent inp, string eventType)
        {
            if (state == GameState.MainMenu)
            {
                if (eventType == "KeyDown")
                {
                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE)
                    {
                        p1 = null;
                        p2 = null;
                        elapsedTime = 0; // Reset the timer when starting the game
                        state = GameState.Play;
                    }
                }
            }
            else if (state == GameState.End)
            {
                if (eventType == "KeyDown")
                {
                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE)
                    {
                        p1 = null;
                        p2 = null;
                        elapsedTime = 0; // Reset the timer when restarting the game
                        state = GameState.MainMenu;
                    }
                }
            }
        }

        public override int getTargetFrameRate()
        {
            return 144;
        }

        private void setupFloor()
        {
            fl = new Floor();
            fl.Transform.X = 200;
            fl.Transform.Y = 50;
        }

        private void setupDirt()
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (i == 0 || i == 14 || j == 0 || j == 14)
                    {
                        Dirt br = new Dirt();
                        br.Transform.X = 200 + (i * 40);
                        br.Transform.Y = 50 + (j * 40);
                        dirt.Add(br);
                    }
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
