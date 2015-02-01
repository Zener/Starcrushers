///////////////////////////////////////////////////////////////////////////////////////////////
// Starcrushers
// Programmed by Carlos Peris
// Barcelona, March-June 2007
///////////////////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using System.IO;
//using ZFramework;




namespace ShipsWar.GameStates
{
   

    class War : GameState
    {
        #region Constants

        



        #endregion

        // -------------
        #region Members

        public Random m_Rand = new Random();
        int m_GameOver = 0;
        Vector3 cameraPos;
        Vector3 vrpPos;
        static TimeSpan shieldDestroyedAnimationTimer;
        static Planet shieldDestroyedPlanet;
        public static bool gameHalted = false;
        private String backImageStr;
        private TimeSpan timeToEndGame;
        SpriteFont spriteFont;

        #endregion

        

       


        // ---------------------------------------------------------------------
        // Game management
        // ---------------------------------------------------------------------
        #region Game Management


        bool gameStarted = false;
        



        public void GameLogic(GameTime gameTime)
        {
            int currentHumanController = 0;
            int planetFocusedID;

            tutorialUpdate(gameTime);

            for (int i = 0; i < Player.players.Length; i++)
            {

                if (Player.players[i].playerType == Player.HUMAN_PLAYER)
                {
                    planetFocusedID = Player.players[i].planetFocusedID;
                    
                    if (!disableHumanControl)
                    {
                        if (Input.WasPressed(currentHumanController, Input.LEFT))
                        {
                            if (universe.planets[planetFocusedID].planetLeft != -1)
                            {
                                planetFocusedID = universe.planets[planetFocusedID].planetLeft;
                            }
                        }
                        if (Input.WasPressed(currentHumanController, Input.RIGHT))
                        {
                            if (universe.planets[planetFocusedID].planetRight != -1)
                            {
                                planetFocusedID = universe.planets[planetFocusedID].planetRight;
                            }
                        }
                        if (Input.WasPressed(currentHumanController, Input.UP))
                        {
                            if (universe.planets[planetFocusedID].planetUp != -1)
                            {
                                planetFocusedID = universe.planets[planetFocusedID].planetUp;
                            }
                        }
                        if (Input.WasPressed(currentHumanController, Input.DOWN))
                        {
                            if (universe.planets[planetFocusedID].planetDown != -1)
                            {
                                planetFocusedID = universe.planets[planetFocusedID].planetDown;
                            }
                        }
                    }
                    if (!disableHumanHitA)
                    {
                        if ((Input.IsPressed(currentHumanController, Input.HIT)) && (DateTime.Now - Player.players[i].lastHitTime > new TimeSpan(1000000)))
                        {
                            Player.players[i].lastHitTime = DateTime.Now;

                            Player.players[i].ActionSendShip(universe.planets[planetFocusedID]);

                        }
                    }
                    if (!disableHumanHitB)
                    {
                        if ((Input.IsPressed(currentHumanController, Input.HIT2)) && (DateTime.Now - Player.players[i].lastHitTime > new TimeSpan(1000000)))
                        {
                            Player.players[i].lastHitTime = DateTime.Now;
                            Player.players[i].ActionSendShips(universe.planets[planetFocusedID]);
                        }
                    }
                    #if !XBOX
                    if (currentHumanController == 0)
                    {
                        
                        
                        {
                            if (Input.mouseLeftPressed && (DateTime.Now - Player.players[i].lastHitTime > new TimeSpan(1000000)))
                            {
                                if (!disableHumanControl)
                                {
                                    planetFocusedID = GetClosestPlanet();
                                }
                                Player.players[i].lastHitTime = DateTime.Now;
                                //planetFocusedID = GetClosestPlanet();
                                if (!disableHumanHitA)
                                Player.players[i].ActionSendShip(universe.planets[planetFocusedID]);
                            }
                        }
                        
                        {
                            if (Input.mouseRightPressed && (DateTime.Now - Player.players[i].lastHitTime > new TimeSpan(1000000)))
                            {
                                if (!disableHumanControl)
                                {
                                    planetFocusedID = GetClosestPlanet();
                                }
                                Player.players[i].lastHitTime = DateTime.Now;
                                //planetFocusedID = GetClosestPlanet();
                                if (!disableHumanHitB)
                                Player.players[i].ActionSendShips(universe.planets[planetFocusedID]);
                            }
                        }
                    }
                    #endif
                    Player.players[i].planetFocusedID = planetFocusedID;
                    currentHumanController++;
                }
                else
                {
                    UpdateCompPlayer(i, gameTime);
                }
            }

            universe.Update(gameTime);

            for (int i = 0; i < universe.particles.Count; i++)
            {
                universe.particles[i].Update(gameTime);
            }

            Statistics.Update(gameTime, false);
        }


        /// <summary>
        /// Run a logic tick
        /// </summary>
        public void Run()
        {
        }

        #endregion

        
        TimeSpan startGameTimer;

        

        public override bool IsFullScreen() { return true; }

        public War(Game1 g, int _numPlayers, List<int> _playerData, int universeSize): base(g)
        {
            int _projRatioOption = 0;
            float projRatio = (float)m_Game.Window.ClientBounds.Width / (float)m_Game.Window.ClientBounds.Height;
            switch(GameVars.aspectRatio)
            {
                case 1: projRatio = 16.0f/10.0f;
                    break;
                case 2: projRatio = 16.0f / 9.0f;
                    break;
            }
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, projRatio,
            0.01f, 1000.0f);
            world = Matrix.Identity;
            ResetCameraStartGame();
            

            for (int i = 0; i < 6; i++)
            {
                planetTexture[i] = m_Game.content.Load<Texture2D>(m_Game.DataPath + @"Images\\Planets\\" + i);
            }

            planetTexture[6] = m_Game.content.Load<Texture2D>(m_Game.DataPath + @"Images\\Planets\\p2");
            
            LoadEffect();
            //LoadModels();
            
            
            CreatePlanetVertexBuffer(new Vector3(0, 0, 0), 1.0f, 32, offsetAngle);

            
            SoundManager.SoundStop();

            int numPlanets = 20;
            switch (universeSize)
            {
                case 0:
                    numPlanets = 9;
                    break;
                case 1:
                    numPlanets = 12;
                    break;                
                case 2: numPlanets = 20;
                    break;
                case 3: numPlanets = 40;
                    break;
                case 4: numPlanets = 54;
                    break;
            }
            InitGame(_numPlayers, _playerData, numPlanets);

            if (GameVars.tutorialMode)
            {
                GameVars.tutorialStep = 0;
                disableHumanControl = true;
                disableHumanHitA = true;
                disableHumanHitB = true;
            }
            backImageStr = "Space/background" + m_Rand.Next(3);

            startGameTimer = new TimeSpan(0, 0, 0, 0, 3000);
            gameHalted = true;
        }


        ~War()
        {
            Dispose();            
        }

        public void Dispose()
        {

        }


        // ---------------------------------------------------------------------
        // Camera
        // ---------------------------------------------------------------------
        #region Camera
        public void ResetCamera()
        {
            cameraPos = new Vector3(50.0f, 50.0f, GameVars.cameraDefaultZ + (GameVars.cameraScalableZ / Game1.gameScale));
            vrpPos = new Vector3(50.0f, 50.0f, 0.0f);
            view = Matrix.CreateLookAt(cameraPos, vrpPos, new Vector3(0.0f, 1.0f, 0.0f));

        }

        public void ResetCameraStartGame()
        {
            cameraPos = new Vector3(50.0f, 50.0f, GameVars.cameraStartZ + (GameVars.cameraScalableZ / Game1.gameScale));
            vrpPos = new Vector3(50.0f, 50.0f, 0.0f);
            view = Matrix.CreateLookAt(cameraPos, vrpPos, new Vector3(0.0f, 1.0f, 0.0f));

        }


        public void FocusCameraOnPlanet(Planet _planet, GameTime _gameTime)
        {

            Vector3 tvrpPos; 
            Vector3 tcameraPos;

            if (_planet != null)
            {
                tvrpPos = _planet.getPosition();
                tcameraPos = new Vector3(_planet.getPosition().X, _planet.getPosition().Y, GameVars.cameraPlanetZ + (GameVars.cameraScalableZ / Game1.gameScale));
            }
            else
            {
                tcameraPos = new Vector3(50.0f, 50.0f, GameVars.cameraDefaultZ + (GameVars.cameraScalableZ / Game1.gameScale));
                tvrpPos = new Vector3(50.0f, 50.0f, 0.0f);

            }           
            
            Vector3 dvrpPos = tvrpPos - vrpPos;
            Vector3 dcameraPos = tcameraPos - cameraPos;

            if (dvrpPos.Length() > 0.5f)
            {
                dvrpPos.Normalize();
                vrpPos += dvrpPos * (float)_gameTime.ElapsedRealTime.TotalMilliseconds * 0.01f;
            
            }
            if (dcameraPos.Length() > 0.5f)
            {
                dcameraPos.Normalize();
                cameraPos += dcameraPos * (float)_gameTime.ElapsedRealTime.TotalMilliseconds * 0.02f;
            
            }
             
            view = Matrix.CreateLookAt(cameraPos, vrpPos, new Vector3(0.0f, 1.0f, 0.0f));
            float a = Vector3.Distance(cameraPos, tcameraPos);
            float b = Vector3.Distance(vrpPos, tvrpPos);
            if (Vector3.Distance(cameraPos, tcameraPos) < 1.5f && Vector3.Distance(vrpPos, tvrpPos) < 1.5f)
            {
                ResetCamera();
            }
        }
        #endregion

       

        /// <summary>
        /// Allows the game state to run logic.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <returns>The next gamestate to transition to, this to stay, null to pop back to previous state. </returns>
        public override GameState Update(GameTime gameTime)
        {
            if (PauseMenu.endGame)
            {
                m_Finished = true;
                
                PauseMenu.endGame = false;
                return new GameStates.MainMenu(m_Game);
            }
            if (Input.WasPressed(-1, Input.BACK))
            {
                //Dispose();
                //return null;
                //return new GameStates.MainMenu(m_Game);
                
                m_Finished = false;                
                return new GameStates.PauseMenu(m_Game, (m_GameOver == 1) ? @"images\HumanWins" : @"images\ComputerWins");
            }
            if (m_GameOver != 0)
            {
                Statistics.Update(gameTime, true);
                //Dispose();
                m_Finished = true;
                return new GameStates.Results(m_Game);
            }
            
            // Detect game won condition.
            /*
            if (GameWon(1))
                m_GameOver = 1;
            else if (GameWon(-1))
                m_GameOver = -1;*/

            if (gameStarted)
            {
                if (launchPopUp)
                {
                    m_Finished = false;
                    launchPopUp = false;
                    return new GameStates.PopUp(m_Game, popUpText);
                }

                if (activatePopUp)
                {
                    launchPopUp = true;
                    activatePopUp = false;
                }
            }
            m_GameOver = Player.PlayerWon();
            if (m_GameOver != 0 && !gameHalted)
            {
                timeToEndGame -= gameTime.ElapsedGameTime;
                if (timeToEndGame.Seconds < 0)
                {
                    //Dispose();
                    m_Finished = false;
                    bool win = (Player.players[m_GameOver - 1].playerType == Player.HUMAN_PLAYER);

                    return new GameStates.Status(m_Game, m_GameOver, win);
                }
                else
                {
                    m_GameOver = 0;
                }
            }
            else
            {
                timeToEndGame = new TimeSpan(0, 0, 0, 10, 0);
                
                m_GameOver = 0;

                
            }
            //Run();
            SoundManager.Update(gameTime);
            /*
            if (Input.IsPressed(1, Input.UP))
            {
                cameraPos.Z -= 0.02f * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (Input.IsPressed(1, Input.DOWN))
            {
                cameraPos.Z += 0.02f * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (Input.IsPressed(1, Input.LEFT))
            {
                vrpPos.X -= 0.02f * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (Input.IsPressed(1, Input.RIGHT))
            {
                vrpPos.X += 0.02f * gameTime.ElapsedGameTime.Milliseconds;
            }
            //view = Matrix.CreateLookAt(cameraPos, vrpPos, new Vector3(0.0f, 1.0f, 0.0f));
            */
            if (gameStarted)
            {
                if (!gameHalted)
                {
                    FocusCameraOnPlanet(null, gameTime);
                        
                    GameLogic(gameTime);
                }
                else
                {
                    if (shieldDestroyedAnimationTimer.TotalMilliseconds >= 0)
                    {
                        shieldDestroyedAnimationTimer -= gameTime.ElapsedRealTime;
                        FocusCameraOnPlanet(shieldDestroyedPlanet, gameTime);
                        if (shieldDestroyedAnimationTimer.TotalMilliseconds <= 0)
                        {
                            gameHalted = false;
                            SoundManager.SoundPlay(SoundManager.MUSIC_STARBASEBLAST);
                            if (Player.players[shieldDestroyedPlanet.homePlanetSide - 1].playerType == Player.HUMAN_PLAYER)
                            {
                                Input.Vibrate(Player.GetControllerBySide(shieldDestroyedPlanet.homePlanetSide), 1.0f, 1.0f, 800);
                            }
                            shieldDestroyedPlanet.homePlanetShields--;
                            //ResetCamera();
                        }
                    }
                    
                }
            }
            else
            {
                if (gameHalted)
                {
                    if (Input.IsPressed(-1, Input.HIT) || Input.IsPressed(-1, Input.START) || Input.mouseLeftPressed)
                    {
                        gameHalted = false;
                        SoundManager.SoundPlay(SoundManager.MUSIC_INGAME);
            
                    }
                }
                else
                {
                    FocusCameraOnPlanet(null, gameTime);
                    
                    startGameTimer -= gameTime.ElapsedGameTime;
                    if (startGameTimer.Ticks < 0)
                    {
                        gameStarted = true;
                    }
                }
            }

            offsetAngle += (float)(0.0005f * gameTime.ElapsedGameTime.TotalMilliseconds);
            return this;
        }


       


        




        

        public void InitGame(int _numPlayers, List<int> _playerData, int _numPlanets)
        {
            timeToEndGame = new TimeSpan(0,0,10);

            universe = new Universe(_numPlanets);
            Player.Init(universe, _numPlayers);

            for (int i = 0; i < _numPlayers; i++)
            {
                Player.players[i] = new Player(i + 1, _playerData[i] > 0 ? 1 : 0, _playerData[i]-1);
                universe.AddHomePlanet(Player.players[i]);
                universe.AddShipsToPlanet(Player.players[i].homePlanet, 50);
            }
            
            GameHUD.Start(Player.players, universe);

            
            #if MOUSE_ACTIVE
            //Make the mouse pointer visible in the game window
            m_Game.IsMouseVisible = false;

            //Initialize the previous mouse state. This stores the current state of the mouse
            Input.mPreviousMouseState = Mouse.GetState();
            #endif

            Statistics.Reset(_numPlayers, universe);
        }


        // ---------------------------------------------------------------------
        // Computer AI
        // ---------------------------------------------------------------------
        #region Computer AI






        public void UpdateCompPlayer(int _np, GameTime _gametime)
        {
            switch (Player.players[_np].playerCompAI)
            {
                case 0: UpdateCompPlayerLowIA(_np, _gametime);
                    break;
                case 1: UpdateCompPlayerMediumIA(_np, _gametime);
                    break;
                case 2: UpdateCompPlayerHighIA(_np, _gametime);
                    break;
            }
        }


        public void UpdateCompPlayerLowIA(int _np, GameTime _gametime)
        {
            int compSide = _np + 1;
            int planetFocusedID = Player.players[_np].planetFocusedID;

            if (DateTime.Now - Player.players[_np].lastHitTime > new TimeSpan(100000000))
            {
                Player.players[_np].lastHitTime = DateTime.Now;

                //HACK
                int pardi = _np;
                while (pardi == _np)
                {
                    pardi = m_Rand.Next(Player.players.Length);
                }

                /*
                if (Player.players[pardi].homePlanet.ships.Count <= Player.players[_np].homePlanet.ships.Count / 2)
                {
                    planetFocusedID = Player.players[pardi].homePlanetID;
                }
                else
                {*/
                    planetFocusedID = m_Rand.Next(universe.planets.Length);
                //}


                int shipsToSend = 1;
                shipsToSend += m_Rand.Next(Player.players[_np].homePlanet.ships.Count/2);
                /*
                if (universe.planets[planetFocusedID].side > 0 && universe.planets[planetFocusedID].side != compSide)
                {
                    shipsToSend += universe.planets[planetFocusedID].ships.Count;
                    
                }
                */
            
                if (Player.players[_np].homePlanet.side == Player.players[_np].side /*&& Player.players[_np].homePlanet.ships.Count > shipsToSend*/)
                {
                    for (int i = 0; i < shipsToSend; i++)
                    {
                        Player.players[_np].ActionSendShip(universe.planets[planetFocusedID]);
                        //universe.SendShip(Player.players[_np].homePlanet, universe.planets[planetFocusedID]);
                    }
                }


                // My base is captured?
                if (Player.players[_np].homePlanet.side != compSide && Player.players[_np].homePlanet.homePlanetShields <= 0)
                {
                    Player.players[_np].ActionSendShips(Player.players[_np].homePlanet);
                }

                Player.players[_np].planetFocusedID = planetFocusedID;
            }
        }



        public void UpdateCompPlayerMediumIA(int _np, GameTime _gametime)
        {
            int compSide = _np+1;
            Player cpuPlayer = Player.players[_np];

            int planetFocusedID = cpuPlayer.planetFocusedID;

            if (DateTime.Now - cpuPlayer.lastHitTime > new TimeSpan(10000000))
            {
                cpuPlayer.lastHitTime = DateTime.Now;
                
                //HACK
                int pardi = _np;
                while (pardi == _np)
                {
                    pardi = m_Rand.Next(Player.players.Length);
                }

                if (m_Rand.Next(256) > 196 && Player.players[pardi].homePlanet.side == Player.players[pardi].homePlanet.homePlanetSide && Player.players[pardi].homePlanet.ships.Count <= Player.players[_np].homePlanet.ships.Count / 2)
                {
                    planetFocusedID = Player.players[pardi].homePlanetID;
                }
                else
                {
                    planetFocusedID = m_Rand.Next(universe.planets.Length);
                }


                int shipsToSend = 1;

                if (universe.planets[planetFocusedID].side > 0)
                {
                    shipsToSend += 4;
                }

                if (universe.planets[planetFocusedID].side > 0 && universe.planets[planetFocusedID].side != compSide)
                {
                    shipsToSend += universe.planets[planetFocusedID].ships.Count+3;
                    /*if (universe.planets[planetFocusedID].homePlanet)
                    {
                        //TODO
                    }*/
                }

                if (cpuPlayer.homePlanet.side == cpuPlayer.side && cpuPlayer.homePlanet.ships.Count > shipsToSend)
                {
                    for (int i = 0; i < shipsToSend; i++)
                    {
                        cpuPlayer.ActionSendShip(universe.planets[planetFocusedID]);
                        //universe.SendShip(Player.players[_np].homePlanet, universe.planets[planetFocusedID]);
                    }
                }
                else
                {
                    int number = cpuPlayer.GetOwnedPlanetsCount();
                    if (number >= 3)
                    {
                        for (int i = 0; i < (shipsToSend*2 / number) + 2; i++)
                        {
                            cpuPlayer.ActionSendShips(universe.planets[planetFocusedID]);
                        }
                    }
                }

                // My base is captured?
                if (cpuPlayer.homePlanet.side != compSide && cpuPlayer.homePlanet.homePlanetShields <= 0)
                {
                    cpuPlayer.ActionSendShips(cpuPlayer.homePlanet);
                }

                cpuPlayer.planetFocusedID = planetFocusedID;
            }
        }

        public void UpdateCompPlayerHighIA(int _np, GameTime _gametime)
        {
            int cpuSide = _np+1;
            Player cpuPlayer = Player.players[_np];

            int planetFocusedID = cpuPlayer.planetFocusedID;

            if (DateTime.Now - cpuPlayer.lastHitTime > new TimeSpan(10000000))
            {
                cpuPlayer.lastHitTime = DateTime.Now;
                //Search for a free planet
                int i = universe.GetRandomFreePlanet();
                
                if (i != -1)
                {
                    //
                    planetFocusedID = i;
                    int nShips = m_Rand.Next(5) + 1;
                    for (int j = 0; j < nShips; j++)
                    {
                        cpuPlayer.ActionSendShip(universe.planets[planetFocusedID]);
                    }
                    return;
                }

                // Ok, no free planets found
                // Search an enemy
                int tries = 10;
                int pardi = _np;
                while (tries > 0 && (pardi == _np || universe.GetPlanetCount(pardi + 1) == 0))
                {
                    pardi = m_Rand.Next(Player.players.Length);
                    tries--;
                }
                if (tries <= 0) return;
                // We have an enemy, get his weakest planet
                int pardiSide = pardi + 1;
                int planetToAttack = universe.GetWeakestPlanet(pardiSide);
                planetFocusedID = planetToAttack;

                if (planetToAttack != -1)
                {
                    // Here we go (if we can)
                    int enemyShips = universe.planets[planetToAttack].ships.Count;
                    int ourShips = cpuPlayer.homePlanet.ships.Count;

                    if (cpuPlayer.homePlanet.side == cpuSide && ourShips > enemyShips + 5)
                    {
                        //Attack with the motherShip
                        int nShips = enemyShips + m_Rand.Next(5) + 1;
                        for (int j = 0; j < nShips; j++)
                        {
                            cpuPlayer.ActionSendShip(universe.planets[planetFocusedID]);
                        }
                        return;
                    }
                    else
                    {
                        int ourPlanets = universe.GetPlanetCount(cpuSide);
                        ourShips = universe.GetShipCount(cpuSide);

                        if (ourPlanets > 0 && ourShips > ourPlanets && ourShips > enemyShips)
                        {
                            //Attack with all the planets
                            int nShips = ((enemyShips * 2) / ourPlanets) + 1;
                            for (int j = 0; j < nShips; j++)
                            {
                                cpuPlayer.ActionSendShips(universe.planets[planetFocusedID]);
                            }
                        }
                        else
                        {
                            // Nothing to do, just reinforce
                            int planetToReinforce = universe.GetWeakestPlanet(cpuSide);
                            if (ourShips > ourPlanets*3 && (universe.planets[planetToReinforce].ships.Count < (ourShips / (ourPlanets+1))) && planetToReinforce != -1)
                            {
                                planetFocusedID = planetToReinforce;
                                if (universe.planets[planetToReinforce] == cpuPlayer.homePlanet)
                                {
                                    cpuPlayer.ActionSendShips(universe.planets[planetFocusedID]);
                                }
                                else
                                {
                                    int nShips = m_Rand.Next(5) + 1;
                                    for (int j = 0; j < nShips; j++)
                                    {
                                        cpuPlayer.ActionSendShips(universe.planets[planetFocusedID]);
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }


        #endregion


        // ---------------------------------------------------------------------
        // Graphics
        // ---------------------------------------------------------------------
        #region Render

        


        public void DrawActionKeys()
        {
            Vector2 actionPos;
            Color colorin, colorout;
            GraphicsDevice device = m_Game.graphics.GraphicsDevice;
            int screenWidth = device.PresentationParameters.BackBufferWidth;
            int screenHeight = device.PresentationParameters.BackBufferHeight;

            int marginSize = screenHeight / 32;
            int iconSize = screenHeight / 24;

            colorin = new Color(new Vector4(1, 1, 1, 1));
            colorout = new Color(new Vector4(0, 0, 0, 1));

            // Action Keys
            actionPos = new Vector2((screenWidth / 2), screenHeight - (marginSize * 2) - iconSize);

            if (GameHUD.spriteFont != null)
            {
                Util.DrawSprite(device, "Menus/a-button", actionPos, new Vector2(iconSize, iconSize), SpriteBlendMode.AlphaBlend, Color.White, Util.SPRITE_ALIGN_TOP_LEFT);
                actionPos.X += iconSize + marginSize;
                Util.DrawText(GameHUD.spriteFont, "Start", actionPos, colorin, colorout, new Vector2(), Game1.menuScale, SpriteEffects.None);
            }
        }
        /// <summary>
        /// This is called when the game state should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {




            GraphicsDevice device = m_Game.graphics.GraphicsDevice;

            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            // reset the font when the GraphicsDevice changes

            DrawBackground(device);




            //evolved.Render(Matrix.CreateScale(0.0010f), view, projection);
            device.RenderState.DepthBufferEnable = true;
            device.RenderState.DepthBufferWriteEnable = true;

            //defaultEffect = new BasicEffect(device, null);//new BasicEffect(device, null);
            effect.Parameters["xView"].SetValue(view);

            //graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            //Matrix worldMatrix = Matrix.Identity;
            //defaultEffect.CurrentTechnique = defaultEffect.Techniques["Colored"];
            //defaultEffect.Parameters["xWorld"].SetValue(worldMatrix);
            //effect.Parameters["xView"].SetValue(viewMatrix);
            //defaultEffect.Parameters["xProjection"].SetValue(projection);
            //defaultEffect.Parameters["xTexture"].SetValue(scenerytexture);

            //effect.CurrentTechnique = effect.Techniques["Textured"];

            DrawPlanets(device);

            DrawShips(device);

            if (!gameHalted)
            {
                for (int i = 0; i < universe.particles.Count; i++)
                {
                    universe.particles[i].Draw(view, projection);
                }
            }

            GameHUD.Draw(view, projection);

            if (!gameStarted)
            {
                if (gameHalted)
                {
                    int screenWidth = device.PresentationParameters.BackBufferWidth;
                    int screenHeight = device.PresentationParameters.BackBufferHeight;
                    Util.DrawSprite(device, "Menus/labelboxwhite", new Vector2(screenWidth, screenHeight) * 0.025f, new Vector2(screenWidth, screenHeight) * 0.95f, SpriteBlendMode.AlphaBlend, new Color(32, 32, 90, 220), Util.SPRITE_ALIGN_TOP_LEFT);
#if XBOX
                    Util.DrawSprite(device, "Menus/controller", new Vector2(screenWidth, screenHeight)*0.5f, new Vector2(screenWidth, screenHeight) * 0.8f, SpriteBlendMode.AlphaBlend, new Color(255, 255, 255), Util.SPRITE_ALIGN_CENTERED);
#else

                    int numControllablePlayers = Player.GetHumanPlayerCount();
                    if (numControllablePlayers == 0)
                    {
                        numControllablePlayers = 1;
                    }
                    if (numControllablePlayers > 2)
                    {
                        numControllablePlayers = 2;
                    }
                    float padScale = 1.0f / (numControllablePlayers + 1);
                    if (padScale > 0.3f) padScale = 0.3f;
                    float lineStep = 1.0f / (numControllablePlayers + 1);

                    Util.DrawSprite(device, "Menus/controller", new Vector2(screenWidth * 0.25f, screenHeight * lineStep), new Vector2(screenWidth, screenHeight) * padScale, SpriteBlendMode.AlphaBlend, new Color(255, 255, 255), Util.SPRITE_ALIGN_CENTERED);
                    Util.DrawSprite(device, "Menus/mouse", new Vector2(screenWidth * 0.5f, screenHeight * lineStep), new Vector2(screenWidth, screenHeight) * padScale, SpriteBlendMode.AlphaBlend, new Color(255, 255, 255), Util.SPRITE_ALIGN_CENTERED);
                    Util.DrawSprite(device, "Menus/keyboardA", new Vector2(screenWidth * 0.75f, screenHeight * lineStep), new Vector2(screenWidth, screenHeight) * padScale, SpriteBlendMode.AlphaBlend, new Color(255, 255, 255), Util.SPRITE_ALIGN_CENTERED);

                    if (numControllablePlayers > 1)
                    {
                        Util.DrawSprite(device, "Menus/controller", new Vector2(screenWidth * 0.25f, screenHeight * lineStep * 2), new Vector2(screenWidth, screenHeight) * padScale, SpriteBlendMode.AlphaBlend, new Color(255, 255, 255), Util.SPRITE_ALIGN_CENTERED);
                        Util.DrawSprite(device, "Menus/keyboardB", new Vector2(screenWidth * 0.5f, screenHeight * lineStep * 2), new Vector2(screenWidth, screenHeight) * padScale, SpriteBlendMode.AlphaBlend, new Color(255, 255, 255), Util.SPRITE_ALIGN_CENTERED);
                    }
                    if (numControllablePlayers > 2)
                    {
                        Util.DrawSprite(device, "Menus/controller", new Vector2(screenWidth * 0.25f, screenHeight * lineStep * 3), new Vector2(screenWidth, screenHeight) * padScale, SpriteBlendMode.AlphaBlend, new Color(255, 255, 255), Util.SPRITE_ALIGN_CENTERED);
                    }
                    if (numControllablePlayers > 3)
                    {
                        Util.DrawSprite(device, "Menus/controller", new Vector2(screenWidth * 0.25f, screenHeight * lineStep * 4), new Vector2(screenWidth, screenHeight) * padScale, SpriteBlendMode.AlphaBlend, new Color(255, 255, 255), Util.SPRITE_ALIGN_CENTERED);
                    }
#endif
                    DrawActionKeys();
                }
                else
                {
                    GameHUD.DrawRemainingTimeToStart("Get ready", new Color(255, 255, 255), new Color(128, 255, 255), 2.0f);

                    //GameHUD.Draw(view, projection);

                    //Util.DrawText(spriteFont)
                }
            }

        }



        private void DrawBackground(GraphicsDevice device)
        {

            int width = Util.GetImageWidth(backImageStr);
            int height = Util.GetImageHeight(backImageStr);
            int screenWidth = device.PresentationParameters.BackBufferWidth;
            int screenHeight = device.PresentationParameters.BackBufferHeight;

            float backScale = 120.0f / (Game1.gameScale * cameraPos.Z);
            float ratioW = (float)screenWidth / (float)width;
            float ratioH = (float)screenHeight / (float)height;
            float ratio;
            if (ratioW < ratioH)
            {
                ratio = ratioH;
            }
            else
            {
                ratio = ratioW;
            }

            backScale = backScale * ratio;
            Util.DrawSprite(device, backImageStr, new Vector2(screenWidth / 2, screenHeight / 2), new Vector2(width, height) * backScale, SpriteBlendMode.None, Color.White, Util.SPRITE_ALIGN_CENTERED);

        }


        private void DrawPlanets(GraphicsDevice device)
        {

            device.RenderState.DepthBufferEnable = false;
            device.RenderState.DepthBufferWriteEnable = true;

            effect.Parameters["xColor"].SetValue(Player.players[0].GetSideColor());

            for (int i = 0; i < universe.planets.Length; i++)
            {

                /*if (i == Player.players[0].planetFocusedID)
                {
                    effect.CurrentTechnique = effect.Techniques["Colored"];
                    //float[] kk = { 0.0f, 1.0f, 0.0f, 0.5f };
                    effect.Parameters["xColor"].SetValue(Player.players[0].GetSideColor());
                }
                else if (i == Player.players[1].planetFocusedID && Player.players[1].playerType == Player.HUMAN_PLAYER)
                {
                    effect.CurrentTechnique = effect.Techniques["Colored"];
                    //float[] kk = { 0.0f, 1.0f, 0.0f, 0.5f };
                    effect.Parameters["xColor"].SetValue(Player.players[1].GetSideColor());
                }
                else*/
                {
                    effect.CurrentTechnique = effect.Techniques["Textured"];
                    if (universe.planets[i].side != 0)
                    {
                        effect.CurrentTechnique = effect.Techniques["GlowTextured"];
                    }
                }

                //float[] kk = { 0.3f, 0.3f, 0.3f, 1.0f };
                //effect.Parameters["xGlowColor"].SetValue(kk);
                //effect.Parameters["xColor"].SetValue(kk);

                if (universe.planets[i].side != 0)
                {
                    effect.Parameters["xGlowColor"].SetValue(Player.players[universe.planets[i].side - 1].GetSideColor());

                }
                /*else
                {
                    float[] kk = { 0.3f, 0.3f, 0.3f, 1.0f };
                    effect.Parameters["xGlowColor"].SetValue(kk);
                }*/
                //device.RenderState.AlphaTestEnable = false;
                //device.RenderState.AlphaBlendEnable = false;

                Vector3 v = universe.planets[i].getPosition();
                Matrix m = Matrix.CreateScale(universe.planets[i].getRadius());
                m.Translation = v;// Matrix.CreateTranslation(v);
                Matrix rY = Matrix.CreateRotationY(offsetAngle);
                Matrix rX = Matrix.CreateRotationX((float)Math.PI);
                m = rX * rY * m * world;
                effect.Parameters["xWorld"].SetValue(m);
                effect.Parameters["xTexture"].SetValue(planetTexture[universe.planets[i].textureID]);

                Matrix mt = Matrix.CreateScale(0.020f);
                mt.Translation = m.Translation;
                mt = rX * rY * mt;
                //mt.Translation += new Vector3(0,0,-100.0f);
                //evolved.Render(mt, view, projection);
                //if (1 == 1) continue;


                if (universe.planets[i].homePlanet)
                {
                    DrawMotherShip(device, universe.planets[i]);
                }
                else
                {
                    effect.Begin();
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Begin();

                        device.VertexDeclaration = new VertexDeclaration(device, VertexPositionNormalTexture.VertexElements);
                        device.DrawUserPrimitives(PrimitiveType.TriangleStrip, verticesarray, 0, verticesarray.Length - 2);

                        pass.End();
                    }
                    effect.End();
                }



                //draw debug lines
#if DEBUG_PLANETS
                effect.Parameters["xWorld"].SetValue(world);
                effect.CurrentTechnique = effect.Techniques["Colored"];
                device.RenderState.AlphaTestEnable = true;
                device.RenderState.AlphaBlendEnable = true;
                device.RenderState.AlphaSourceBlend = Blend.SourceAlpha;
                device.RenderState.AlphaDestinationBlend = Blend.SourceAlpha;
                device.RenderState.BlendFactor = new Color(128,128,128,128);
                for (int j = 0; j < 2; j++)
                {
                    int s = 0;

                    float[] eColor1 = { 0.0f, 1.0f, 1.0f, 0.1f };
                    float[] eColor2 = { 1.0f, 0.0f, 0.0f, 0.9f };
                    
                    switch (j)
                    {
                        case 0: s = universe.planets[i].planetDown;
                            effect.Parameters["xColor"].SetValue(eColor1);
                
                            break;
                        case 1: s = universe.planets[i].planetRight;
                            effect.Parameters["xColor"].SetValue(eColor2);
                
                            break;
                    }


                    device.RenderState.DepthBufferWriteEnable = true;
                    
                    effect.Begin();
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Begin();

                        if (s != -1)
                        {
                            Planet p = universe.planets[s];

                            ArrayList verticeslistArrow = new ArrayList();
                            verticeslistArrow.Clear();

                            VertexPositionColor[] verticesarrayArrow;
                            Vector3 pos = universe.planets[i].getPosition();

                            verticeslistArrow.Add(new VertexPositionColor(new Vector3(pos.X, pos.Y, pos.Z), new Color(255, 255, 255,128)));
                            verticeslistArrow.Add(new VertexPositionColor(new Vector3(p.getPosition().X, p.getPosition().Y, p.getPosition().Z), new Color(255, 255, 255,128)));

                            verticesarrayArrow = (VertexPositionColor[])verticeslistArrow.ToArray(typeof(VertexPositionColor));


                            device.VertexDeclaration = new VertexDeclaration(device, VertexPositionColor.VertexElements);
                            device.DrawUserPrimitives(PrimitiveType.LineList, verticesarrayArrow, 0, 1);
                        }


                        pass.End();
                    }
                    effect.End();

                }
#endif

            }
        }



        private void DrawShips(GraphicsDevice device)
        {
            device.RenderState.CullMode = CullMode.CullCounterClockwiseFace;            
            device.VertexDeclaration = new VertexDeclaration(device, VertexPositionNormalTexture.VertexElements);

            Matrix m = Matrix.CreateScale(0.4f);
            Matrix rY = Matrix.CreateRotationY((float)Math.PI / 2);


            for (int i = 0; i < universe.ships.Count; i++)
            {
                float angleShip = universe.ships[i].yaw - (float)Math.PI / 2;
                if (universe.ships[i].State == Ship.STATE_LANDED) continue;

                effect.CurrentTechnique = effect.Techniques["Colored"];
                effect.Parameters["xColor"].SetValue(Player.players[universe.ships[i].side - 1].GetSideColor());

                Vector3 v = universe.ships[i].getPosition();
                
                Matrix rX = Matrix.CreateRotationZ(angleShip);
                Matrix m2 = m * rX * world;

                m2.Translation = v;

                effect.Parameters["xWorld"].SetValue(m2);
                //effect.Parameters["xTexture"].SetValue(planetTexture[universe.planets[i].textureID]);

                effect.Begin();

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Begin();

                    universe.ships[i].DrawShip();

                    pass.End();
                }
                effect.End();

            }
        }


        Effect effect;

        private void LoadEffect()
        {
#if XBOX
            effect = m_Game.content.Load<Effect>(m_Game.DataPath + @"effects");
#else
            CompiledEffect compiledEffect = Effect.CompileEffectFromFile(m_Game.DataPath + "effects.fx", null, null, CompilerOptions.None, TargetPlatform.Windows);
            effect = new Effect(m_Game.graphics.GraphicsDevice, compiledEffect.GetEffectCode(), CompilerOptions.None, null);
#endif
            effect.Parameters["xView"].SetValue(view);
            effect.Parameters["xProjection"].SetValue(projection);
            effect.Parameters["xWorld"].SetValue(Matrix.Identity);
            effect.Parameters["xEnableLighting"].SetValue(true);
            Vector3 light = new Vector3(0.5f, -1, -0.5f);
            light.Normalize();
            effect.Parameters["xLightDirection"].SetValue(light);
            effect.Parameters["xAmbient"].SetValue(0.4f);
            //effect.Parameters["BlurScale"].SetValue(0.5f);
            effect.CurrentTechnique = effect.Techniques["Textured"];
        }


        ArrayList verticeslist = new ArrayList();
        VertexPositionNormalTexture[] verticesarray;
        Texture2D[] planetTexture = new Texture2D[10];
        private float offsetAngle = 0.0f;
        Universe universe;
        private Matrix projection;
        private Matrix view;
        private Matrix world;



        protected void CreatePlanetVertexBuffer(Vector3 c, double r, int n, float offsetAngle)
        {
            int i, j;
            double theta1, theta2, theta3;
            Vector3 e, p;

            verticeslist.Clear();

            e = new Vector3();
            p = new Vector3();

            if (r < 0) r = -r;
            if (n < 0) n = -n;


            for (j = 0; j < n / 2; j++)
            {
                theta1 = (j * Math.PI * 2) / n - (Math.PI / 2);
                theta2 = ((j + 1) * Math.PI * 2) / n - (Math.PI / 2);

                for (i = 0; i <= n; i++)
                {
                    theta3 = (i * Math.PI * 2) / n;

                    theta3 += offsetAngle;

                    e.X = (float)(Math.Cos(theta2) * Math.Cos(theta3));
                    e.Y = (float)(Math.Sin(theta2));
                    e.Z = (float)(Math.Cos(theta2) * Math.Sin(theta3));
                    p.X = (float)(c.X + r * e.X);
                    p.Y = (float)(c.Y + r * e.Y);
                    p.Z = (float)(c.Z + r * e.Z);

                    verticeslist.Add(new VertexPositionNormalTexture(new Vector3(p.X, p.Y, p.Z), new Vector3(e.X, e.Y, e.Z), new Vector2((float)(i / (double)n), (float)(2 * (j + 1) / (double)n))));
                    
                    e.X = (float)(Math.Cos(theta1) * Math.Cos(theta3));
                    e.Y = (float)(Math.Sin(theta1));
                    e.Z = (float)(Math.Cos(theta1) * Math.Sin(theta3));
                    p.X = (float)(c.X + r * e.X);
                    p.Y = (float)(c.Y + r * e.Y);
                    p.Z = (float)(c.Z + r * e.Z);

                    verticeslist.Add(new VertexPositionNormalTexture(new Vector3(p.X, p.Y, p.Z), new Vector3(e.X, e.Y, e.Z), new Vector2((float)(i / (double)n), (float)(2 * (j) / (double)n))));
                }




            }

            verticesarray = (VertexPositionNormalTexture[])verticeslist.ToArray(typeof(VertexPositionNormalTexture));
        }


        


        private void DrawMotherShip(GraphicsDevice device, Planet p)
        {
            Matrix m;
            Matrix rX, rY;// = Matrix.CreateRotationY(offsetAngle);
            Vector3 v = p.getPosition();
            effect.CurrentTechnique = effect.Techniques["TexturedColor"];
            effect.Parameters["xTexture"].SetValue(planetTexture[6]);
            effect.Parameters["xColor"].SetValue(Player.players[p.side - 1].GetSideColor());

            m = Matrix.CreateScale(p.getRadius());
            m.Translation = v;// Matrix.CreateTranslation(v);
            rY = Matrix.CreateRotationZ(offsetAngle * 12.0f);
            //rX = Matrix.CreateRotationX((float)Math.PI);
            m = rY * m * world;
            effect.Parameters["xWorld"].SetValue(m);
            

            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();

                device.VertexDeclaration = new VertexDeclaration(device, VertexPositionNormalTexture.VertexElements);
                device.DrawUserPrimitives(PrimitiveType.TriangleStrip, verticesarray, 0, verticesarray.Length - 2);

                pass.End();
            }
            effect.End();

            

            for (int n = 0; n < p.homePlanetShields; n++)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (i == 4)
                        m = Matrix.CreateScale(1.0f);
                    else
                        m = Matrix.CreateScale(0.4f);
                    rY = Matrix.CreateRotationZ((offsetAngle * 12.0f) + (float)(n * (Math.PI * 2) / 3));
                    m.Translation = new Vector3(((i-2) * 0.2f) + p.getRadius(), ((i-2) * 0.2f) + p.getRadius(), 0.0f);
                    m = m * rY;
                    m.Translation += v;
                    //m *= Matrix.CreateScale(0.33f);
                    effect.Parameters["xWorld"].SetValue(m);
                    //effect.CurrentTechnique = effect.Techniques["Textured"];

                    effect.Begin();
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Begin();

                        device.VertexDeclaration = new VertexDeclaration(device, VertexPositionNormalTexture.VertexElements);
                        device.DrawUserPrimitives(PrimitiveType.TriangleStrip, verticesarray, 0, verticesarray.Length - 2);

                        pass.End();
                    }
                    effect.End();
                }
            }
        }


        #endregion

#if !XBOX
        public int GetClosestPlanet()
        {
            //Planet closestPlanet = universe.planets[0];
            Vector2 pos = Util.TransformTo2D(universe.planets[0].getPosition(), view, projection);
            Vector2 mousePos = new Vector2(Input.mouseX, Input.mouseY);
            float distanceToClosest = Vector2.Distance(pos, mousePos);
            int closestIndex = 0;
                
            for(int i = 1; i < universe.planets.Length; i++)
            {
                pos = Util.TransformTo2D(universe.planets[i].getPosition(), view, projection);
                float distance = Vector2.Distance(pos, mousePos);
                if (distance < distanceToClosest)
                {
                    distanceToClosest = distance;
                    closestIndex = i;
                }
            }
            return closestIndex;
        }
        #endif

        public static void ShieldDestroyed(Planet _planet)
        {
            if (gameHalted) return;
            gameHalted = true;
            shieldDestroyedAnimationTimer = new TimeSpan(0,0,0,0,1500);;
            shieldDestroyedPlanet = _planet;
        }


       

        bool activatePopUp = false;
        String popUpText;
        bool launchPopUp = false;


        public void LaunchPopUp(String text)
        {
            if (!GameVars.tutorialMode) return;
            activatePopUp = true;
            popUpText = text;

            //switch()


        }

        // ---------------------------------------------------------------------
        // Tutorial
        // ---------------------------------------------------------------------
        #region tutorial

        TimeSpan tutorialTimer;
        bool disableHumanControl = false;
        bool disableHumanHitA = false;
        bool disableHumanHitB = false;
        bool tutorialTimerActive = false;

        public void tutorialUpdate(GameTime _gameTime)
        {
            if (!GameVars.tutorialMode) return;

            switch(GameVars.tutorialStep)
            {
                case 0:
                    {
                        bool disableHumanControl = true;
                        bool disableHumanHitA = true;
                        bool disableHumanHitB = true;
                        Particle p = new Particle(universe);
                        Vector3 pos = Player.players[0].homePlanet.getPosition();
                        p.SetParticle(Particle.PARTICLE_TYPE_HOOP, 5000, pos,
                            new Vector3(), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), "hoop", SpriteBlendMode.Additive);
                        universe.AddParticle(p);

                        LaunchPopUp(GameVars.TUTORIAL_TEXT_1);
                        GameVars.tutorialStep++;
                        tutorialTimerActive = false;
                        break;
                    }
                case 1:
                    {
                        if (!tutorialTimerActive)
                        {
                            tutorialTimer = new TimeSpan(0, 0, 0, 5);
                            tutorialTimerActive = true;
                            break;
                        }
                        tutorialTimer -= _gameTime.ElapsedGameTime;
                        if (tutorialTimer.Seconds < 0)
                        {
                            LaunchPopUp(GameVars.TUTORIAL_TEXT_2);
                            GameVars.tutorialStep++;
                            tutorialTimerActive = false;
                        }
                        break;
                    }
                case 2:
                    {
                        if (!tutorialTimerActive)
                        {
                            tutorialTimer = new TimeSpan(0, 0, 0, 3);
                            tutorialTimerActive = true;
                            break;
                        }
                        tutorialTimer -= _gameTime.ElapsedGameTime;
                        if (tutorialTimer.Seconds < 0)
                        {

                            Particle p = new Particle(universe);
                            Vector3 pos = universe.planets[5].getPosition();
                            p.SetParticle(Particle.PARTICLE_TYPE_HOOP, 5000, pos,
                                new Vector3(), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), "hoop", SpriteBlendMode.Additive);
                            universe.AddParticle(p);


                            LaunchPopUp(GameVars.TUTORIAL_TEXT_3);
                            GameVars.tutorialStep++;
                            tutorialTimerActive = false;

                        }
                        break;
                    }
                case 3:
                    {
                        disableHumanControl = false;
                        
                        if (Player.players[0].planetFocusedID == 5)
                        {
                            LaunchPopUp(GameVars.TUTORIAL_TEXT_4);
                            GameVars.tutorialStep++;
                            tutorialTimerActive = false;
                        }
                        break;
                    }
                case 4:
                    {
                        disableHumanControl = true;
                        disableHumanHitA = false;
                        Player.players[0].planetFocusedID = 5;
                        if (universe.planets[5].ships.Count > 10)
                        {
                            LaunchPopUp(GameVars.TUTORIAL_TEXT_5);
                            GameVars.tutorialStep++;
                            tutorialTimerActive = false;
                        }
                        break;
                    }
                case 5:
                    {
                        disableHumanControl = true;
                        disableHumanHitA = true;
                        if (!tutorialTimerActive)
                        {
                            tutorialTimer = new TimeSpan(0, 0, 0, 3);
                            tutorialTimerActive = true;
                            break;
                        }
                        tutorialTimer -= _gameTime.ElapsedGameTime;
                        if (tutorialTimer.Seconds < 0)
                        {

                            Particle p = new Particle(universe);
                            Vector3 pos = universe.planets[3].getPosition();
                            p.SetParticle(Particle.PARTICLE_TYPE_HOOP, 5000, pos,
                                new Vector3(), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), "hoop", SpriteBlendMode.Additive);
                            universe.AddParticle(p);


                            LaunchPopUp(GameVars.TUTORIAL_TEXT_6);
                            GameVars.tutorialStep++;
                            tutorialTimerActive = false;

                        }
                        break;
                    }
                case 6:
                    disableHumanControl = false;

                    if (Player.players[0].planetFocusedID == 3)
                    {
                        LaunchPopUp(GameVars.TUTORIAL_TEXT_7);
                        GameVars.tutorialStep++;
                        tutorialTimerActive = false;
                    }                    
                    break;
                case 7:
                    {
                        disableHumanControl = true;
                        disableHumanHitA = false;
                        Player.players[0].planetFocusedID = 3;
                        if (universe.planets[3].side == 1)
                        {
                            LaunchPopUp(GameVars.TUTORIAL_TEXT_8);
                            GameVars.tutorialStep++;
                            tutorialTimerActive = false;
                        }
                        break;
                    }
                case 8:
                    {
                        disableHumanControl = true;
                        disableHumanHitA = true;
                        if (!tutorialTimerActive)
                        {
                            tutorialTimer = new TimeSpan(0, 0, 0, 3);
                            tutorialTimerActive = true;
                            break;
                        }
                        tutorialTimer -= _gameTime.ElapsedGameTime;
                        if (tutorialTimer.Seconds < 0)
                        {
                            Particle p = new Particle(universe);
                            Vector3 pos = universe.planets[0].getPosition();
                            p.SetParticle(Particle.PARTICLE_TYPE_HOOP, 5000, pos,
                                new Vector3(), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), "hoop", SpriteBlendMode.Additive);
                            universe.AddParticle(p);


                            LaunchPopUp(GameVars.TUTORIAL_TEXT_9);
                            GameVars.tutorialStep++;
                            tutorialTimerActive = false;
                        }
                        break;
                    }
                case 9:
                    {
                        disableHumanControl = false;

                        if (Player.players[0].planetFocusedID == 0)
                        {
                            LaunchPopUp(GameVars.TUTORIAL_TEXT_10);
                            GameVars.tutorialStep++;
                            tutorialTimerActive = false;
                        }
                        break;
                    }
                case 10:
                    {
                        disableHumanControl = true;
                        disableHumanHitA = true;
                        disableHumanHitB = false;
                        Player.players[0].planetFocusedID = 0;
                        if (universe.planets[0].homePlanetShields < 3)
                        {
                            LaunchPopUp(GameVars.TUTORIAL_TEXT_11);
                            GameVars.tutorialStep++;
                            tutorialTimerActive = false;
                        }
                        break;
                    }
                case 11:
                    {
                        disableHumanControl = true;
                        disableHumanHitA = true;
                        disableHumanHitB = true;
                        if (!tutorialTimerActive)
                        {
                            tutorialTimer = new TimeSpan(0, 0, 0, 3);
                            tutorialTimerActive = true;
                            break;
                        }
                        tutorialTimer -= _gameTime.ElapsedGameTime;
                        if (tutorialTimer.Seconds < 0)
                        {
                          
                            LaunchPopUp(GameVars.TUTORIAL_TEXT_12);
                            GameVars.tutorialStep++;
                            tutorialTimerActive = false;
                        }
                        break;
                    }
                case 12:
                    {
                        disableHumanControl = true;
                        disableHumanHitA = true;
                        disableHumanHitB = true;
                        if (!tutorialTimerActive)
                        {
                            tutorialTimer = new TimeSpan(0, 0, 0, 3);
                            tutorialTimerActive = true;
                            break;
                        }
                        tutorialTimer -= _gameTime.ElapsedGameTime;
                        if (tutorialTimer.Seconds < 0)
                        {

                            LaunchPopUp(GameVars.TUTORIAL_TEXT_13);
                            GameVars.tutorialStep++;
                            tutorialTimerActive = false;
                        }
                        break;
                    }
                case 13:
                    {
                        disableHumanControl = false;
                        disableHumanHitA = false;
                        disableHumanHitB = false;
                        if (universe.planets[0].homePlanetShields <= 0 && universe.planets[0].side == 1)
                        {
                            LaunchPopUp(GameVars.TUTORIAL_TEXT_14);
                            GameVars.tutorialStep++;
                            tutorialTimerActive = false;
                        }
                        break;
                    }
            }
        }
        #endregion
    }
}
