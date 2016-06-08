using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_project
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        Player player1;
        Player player2;
        Player player3;
        Player player4;
        List<missile> player1Missiles;
        List<missile> player2Missiles;
        List<missile> player3Missiles;
        List<missile> player4Missiles;

        Texture2D missilesTexture;

        SoundEffect soundjump;

        List<missile> Mymissiles;

        Texture2D backgroundTexture;


        HealthBar health1;
        HealthBar health2;
        HealthBar health3;
        HealthBar health4;

        SpriteBatch spriteBatch;

        Map map;
        Texture2D fireTexture;
        Texture2D burstTexture;        

        Emitter emitterRight;

        Emitter emitterLeft;

        Emitter smokeRight;

        Emitter smokeLeft;

        List<CollisionTiles> destroyedTiles;

        //SoundPlayer soundExplosion;
        //SoundPlayer soundEndOfGame;
        //SoundPlayer soundPowerUp;
        //SoundPlayer soundExtraLife;
        List<Powerup> powerUpList;

        SpriteFont scoreText;
        private int score;
        float gameTimer;
        Timer powerUpTimer;

        Powerup powerUp;

        public enum GameStates
        {
            MAINMENU,
            PLAYING,
            CONTROLS,
            GAMEOVER,
            EXIT,
        }
        GameStates currentGameStates = GameStates.MAINMENU;

        Buttons playbtn;
        Buttons cntrlbtn;
        Buttons exitbtn;
        Buttons rttrnbttn;

        const int numEmitterSettings = 3;
        int emitterSettingsID = 0;
        string[] emitterSettingsMessatges;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1440;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 800;   // set this value to the desired height of your window
            graphics.ApplyChanges();


            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            map = new Map();
            gameTimer = 120f;     // 2 minutes of game time
                                  //player1.m_flipHorosontal = false;

            player1Missiles = new List<missile>();
            player2Missiles = new List<missile>();
            player3Missiles = new List<missile>();
            player4Missiles = new List<missile>();

            health1 = new HealthBar();
            health2 = new HealthBar();
            health3 = new HealthBar();
            health4 = new HealthBar();

            player1 = new Player(Content.Load<Texture2D>("player"), new Vector2(200, 400), 1, 200, player1Missiles);
            player2 = new Player(Content.Load<Texture2D>("player"), new Vector2(600, 400), 2, 200, player2Missiles);
            player3 = new Player(Content.Load<Texture2D>("player"), new Vector2(600, 400), 2, 200, player3Missiles);
            player4 = new Player(Content.Load<Texture2D>("player"), new Vector2(600, 400), 2, 200, player4Missiles);

            

            powerUpTimer = new Timer(5);
            powerUp = new Powerup();
            powerUp.existing = false;
            //powerUp.CreatePowerup(player1, powerUpTimer);

            destroyedTiles = new List<CollisionTiles>();

            Mymissiles = new List<missile>();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            fireTexture = Content.Load<Texture2D>("fire");
            burstTexture = Content.Load<Texture2D>("spark");
            scoreText = Content.Load<SpriteFont>("scoreFont");
            powerUp.texture = Content.Load<Texture2D>("frame1");

            missilesTexture = Content.Load<Texture2D>("bullet");

            backgroundTexture = Content.Load<Texture2D>("backgrounds2");

            health1.texture = Content.Load<Texture2D>("health");
            health2.texture = Content.Load<Texture2D>("health");
            health3.texture = Content.Load<Texture2D>("health");
            health4.texture = Content.Load<Texture2D>("health");

            player1.textureLeft = Content.Load<Texture2D>("playerLeft");
            player2.textureLeft = Content.Load<Texture2D>("playerLeft");
            player3.textureLeft = Content.Load<Texture2D>("playerLeft");
            player4.textureLeft = Content.Load<Texture2D>("playerLeft");

            soundjump = Content.Load<SoundEffect>("jump");
            SoundEffect.MasterVolume = 0.1f;

            playbtn = new Buttons(Content.Load<Texture2D>("playbutton"), graphics.GraphicsDevice);
            playbtn.setPosition(new Vector2(600, 500));
            cntrlbtn = new Buttons(Content.Load<Texture2D>("controlbutton"), graphics.GraphicsDevice);
            cntrlbtn.setPosition(new Vector2(600, 550));
            exitbtn = new Buttons(Content.Load<Texture2D>("exitbutton"), graphics.GraphicsDevice);
            exitbtn.setPosition(new Vector2(600, 600));
            rttrnbttn = new Buttons(Content.Load<Texture2D>("returnbutton"), graphics.GraphicsDevice);
            rttrnbttn.setPosition(new Vector2(600, 700));

            //Mymissiles.missleTexture = Content.Load<Texture2D>("bullet");
            
            emitterRight = Emitter.CreateFireEmitter(fireTexture, new Vector2(0, 0));
            emitterRight.wind = 1000f;
            emitterLeft = Emitter.CreateFireEmitter(fireTexture, new Vector2(0, 0));
            emitterLeft.wind = -1000f;
            smokeLeft = Emitter.CreateBurstEmitter(burstTexture, new Vector2(0, 0));
            smokeLeft.wind = -1000f;
            smokeRight = Emitter.CreateBurstEmitter(burstTexture, new Vector2(0, 0));
            smokeRight.wind = 1000f;


            Tiles.Content = Content;

            map.Generate(new int[,]{              
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                {0,2,1,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,1,1,1,1,1,1,1,1,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                {1,1,1,1,1,1,1,1,3,0,0,0,0,0,0,0,0,0,0,0,0,0,2,1,1,1,1,1,1,1,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                {0,0,0,0,0,0,0,0,0,0,2,1,1,1,1,1,1,1,3,0,0,0,0,0,0,0,0,0,0,0,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                {0,0,0,2,1,1,1,1,3,0,0,0,0,2,1,1,1,1,3,0,0,0,0,0,2,1,1,1,1,1,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,},
            }, 50);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        /// 

        
    
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
          //  if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();


            MouseState mouse = Mouse.GetState();


            switch (currentGameStates)
            {
                case GameStates.MAINMENU:
                    playbtn.Update(mouse, gameTime);
                    cntrlbtn.Update(mouse, gameTime);
                    exitbtn.Update(mouse, gameTime);

                    if (cntrlbtn.isClicked) currentGameStates = GameStates.CONTROLS;
                    if (playbtn.isClicked) currentGameStates = GameStates.PLAYING;
                    if (exitbtn.isClicked)
                    {
                        Exit();
                    }
                    IsMouseVisible = true;
                    break;

                case GameStates.PLAYING:

                    UpdateHealth(gameTime);
                    UpdateScore(gameTime);
                    UpdateSound(gameTime);
                    MissileCollisions(player1Missiles, player1, player2, player3, player4);
                    MissileCollisions(player2Missiles, player2, player1, player3, player4);
                    MissileCollisions(player3Missiles, player3, player1, player2, player4);
                    MissileCollisions(player4Missiles, player4, player1, player2, player4);

                    CheckCollisons(gameTime);
                    UpdatePlayers(gameTime);
                    emitterRight.Update(gameTime);
                    emitterLeft.Update(gameTime);
                    smokeRight.Update(gameTime);
                    smokeLeft.Update(gameTime);
                    UpdateEmitter(gameTime);
                    powerUp.Update(player1, gameTime, powerUpTimer);
                    powerUp.Update(player2,gameTime, powerUpTimer);
                    powerUp.Update(player3, gameTime, powerUpTimer);
                    powerUp.Update(player4, gameTime, powerUpTimer);
                    powerUpTimer.update(gameTime, 0);

                    health1.Update(gameTime, player1, player2, player3, player4);
                    //health2.Update(gameTime, player2);
                    //health3.Update(gameTime, player3);
                    //health4.Update(gameTime, player4);

                    float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    gameTimer -= delta;
                    if (gameTimer <= 0)
                    {
                        currentGameStates = GameStates.GAMEOVER;
                    }
                    else
                    {


                        //missile.updateMissiles();
                    }


                    break;

                case GameStates.CONTROLS:
                    rttrnbttn.Update(mouse, gameTime);
                    IsMouseVisible = true;
                    if (rttrnbttn.isClicked) currentGameStates = GameStates.MAINMENU;
                    break;


                case GameStates.EXIT:

                    break;

                case GameStates.GAMEOVER:
                    IsMouseVisible = true;
                    if (exitbtn.isClicked)
                    {
                        Exit();
                    }
                    exitbtn.Update(mouse, gameTime);
                    break;
            }



         

           
            // TODO: Add your update logic here

            base.Update(gameTime);

        }

        private void DrawGameTime()
        {
            if (gameTimer <= 10)
            {
                spriteBatch.DrawString(scoreText, "Time Left  " + ((int)gameTimer + 1), new Vector2((graphics.PreferredBackBufferWidth / 2) - 100, (graphics.PreferredBackBufferHeight / 2) + 60), Color.Red);
            }
        }

        protected void UpdateHealth(GameTime gameTime)
        {
           if (player1.position.Y >= 800)
            {
                player1.health = 100;
                player1.score -= 20;
                player1.Respawn();
            }
            if (player2.position.Y >= 800)
            {
                player2.health = 100;
                player2.score -= 20;
                player2.Respawn();
            }
            if (player3.position.Y >= 800)
            {
                player3.health = 100;
                player3.score -= 20;
                player3.Respawn();
            }
            if (player4.position.Y >= 800)
            {
                player4.health = 100;
                player4.score -= 20;
                player4.Respawn();
            }
        }


        protected void UpdateScore(GameTime gameTime)
        {
            
        }
                
        

        protected void CheckCollisons(GameTime gameTime)
        {
            foreach (CollisionTiles tile in map.CollisionTiles)
            {
                player1.CheckCollision(tile.Rectangle, map.Width, map.Height);
                player2.CheckCollision(tile.Rectangle, map.Width, map.Height);
                player3.CheckCollision(tile.Rectangle, map.Width, map.Height);
                player4.CheckCollision(tile.Rectangle, map.Width, map.Height);
            }
            
        }

        protected void UpdateSound(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W) && player1.hasJumped == false)
            { 
                soundjump.Play();
                player1.hasJumped = true;
            }
        }
   
        

        protected void UpdatePlayers(GameTime gameTime)
        {
            player1.Update(gameTime, player1, player1Missiles, player2);
            player1.Input(gameTime,Keys.A, Keys.D, Keys.W, Keys.E, player1, player1Missiles);
            player2.Update(gameTime, player2, player2Missiles, player2);
            player2.Input(gameTime, Keys.Left, Keys.Right, Keys.Up, Keys.RightControl, player2, player2Missiles);
            player3.Update(gameTime, player3, player3Missiles, player2);
            player3.Input(gameTime,Keys.H, Keys.K, Keys.U, Keys.I, player3, player3Missiles);
            player4.Update(gameTime, player4, player4Missiles, player4);
            player4.Input(gameTime,Keys.F, Keys.H, Keys.T, Keys.Y, player4, player4Missiles);
        }

        protected void UpdateEmitter(GameTime gameTime)
        {
          
            emitterRight.position = new Vector2(-30, 790);
            emitterLeft.position = new Vector2(1550, 790);
            smokeLeft.position = new Vector2(1400, 790);
            smokeRight.position = new Vector2(20, 790);
            
        }




        //private void drawScore()
        //{
        //    spriteBatch.DrawString(scoreText, "SCORE: ", new Vector2(400, 400), Color.White);
        //    spriteBatch.DrawString(scoreText, score.ToString(), new Vector2(80, 5), Color.White);

        //    spriteBatch.DrawString(scoreText, "SCORE: ", new Vector2(graphics.PreferredBackBufferWidth - 95, 5), Color.White);
        //    spriteBatch.DrawString(scoreText, player1.score.ToString(), new Vector2(graphics.PreferredBackBufferWidth - 25, 5), Color.White);

        //}

            private void DrawBackground()
        {
            spriteBatch.Draw(backgroundTexture, new Vector2(0, 0), Color.White);
        }


        private void DrawScore()
        {
            spriteBatch.DrawString(scoreText, "PLAYER1 SCORE :", new Vector2(30, 5), Color.White);
            spriteBatch.DrawString(scoreText, player1.score.ToString(), new Vector2(200, 5), Color.White);

            spriteBatch.DrawString(scoreText, "PLAYER2 SCORE :", new Vector2(300, 5), Color.White);
            spriteBatch.DrawString(scoreText, player2.score.ToString(), new Vector2(475,5), Color.White);

            spriteBatch.DrawString(scoreText, "PLAYER3 SCORE :", new Vector2(600, 5), Color.White);
            spriteBatch.DrawString(scoreText, player3.score.ToString(), new Vector2(775, 5), Color.White);

            spriteBatch.DrawString(scoreText, "PLAYER4 SCORE :", new Vector2(900, 5), Color.White);
            spriteBatch.DrawString(scoreText, player3.score.ToString(), new Vector2(1075, 5), Color.White);
        }


        public void MissileCollisions(List<missile> currectMissiles, Player safePlayer, Player enemy1, Player enemy2, Player enemy3)
        {
            List<missile> missileDeathRow = new List<missile>();

            foreach (missile missile in currectMissiles)
            {
                missile.Position += missile.Velocity;

                missile.rectangle.X += (int)missile.Velocity.X;
                missile.rectangle.Y += (int)missile.Velocity.Y;



                foreach (CollisionTiles tile in map.CollisionTiles)
                {
                    if (missile.rectangle.Intersects(tile.rectangle))
                    {
                        missileDeathRow.Add(missile);
                        tile.tileLife++;
                    }
                    if (tile.tileLife > 5)
                    {
                        destroyedTiles.Add(tile);
                    }
                }

                if (missile.rectangle.Intersects(enemy1.rectangle))
                {
                    enemy1.health -= 5;
                    safePlayer.score += 5;
                    missileDeathRow.Add(missile);
                }

                if (missile.rectangle.Intersects(enemy2.rectangle))
                {
                    enemy2.health -= 5;
                    safePlayer.score += 5;
                    missileDeathRow.Add(missile);
                }

                if (missile.rectangle.Intersects(enemy3.rectangle))
                {
                    enemy3.health -= 5;
                    safePlayer.score += 5;
                    missileDeathRow.Add(missile);
                }

            }

            foreach (missile missile in missileDeathRow)
            {
                currectMissiles.Remove(missile);
            }

            foreach (CollisionTiles tile in destroyedTiles)
            {
                map.CollisionTiles.Remove(tile);
            }
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            spriteBatch.Begin();

            switch (currentGameStates)
            {
                case GameStates.MAINMENU:
                    spriteBatch.Draw(Content.Load<Texture2D>("menuback"), new Rectangle(0, 0, 1440, 800), Color.White);
                    playbtn.Draw(spriteBatch);
                    cntrlbtn.Draw(spriteBatch);
                    exitbtn.Draw(spriteBatch);
                    break;

                case GameStates.PLAYING:
                    DrawBackground();
                    DrawScore();
                    player1.Draw(spriteBatch, missilesTexture, player1Missiles);
                    player2.Draw(spriteBatch, missilesTexture, player2Missiles);
                    player3.Draw(spriteBatch, missilesTexture, player3Missiles);
                    player4.Draw(spriteBatch, missilesTexture, player4Missiles);

                    smokeRight.Draw(spriteBatch);
                    smokeLeft.Draw(spriteBatch); 
                    emitterRight.Draw(spriteBatch);
                    emitterLeft.Draw(spriteBatch);
                    
                  
                    DrawGameTime();
                    map.Draw(spriteBatch);  
                    SpriteEffects effects = SpriteEffects.None;
                    if (player1.m_flipHorosontal) effects = SpriteEffects.FlipHorizontally;
                    if (player2.m_flipHorosontal) effects = SpriteEffects.FlipHorizontally;
                    if (player3.m_flipHorosontal) effects = SpriteEffects.FlipHorizontally;
                    if (player4.m_flipHorosontal) effects = SpriteEffects.FlipHorizontally;
                    powerUp.Draw(spriteBatch);
                    health1.Draw(spriteBatch);
                    health2.Draw(spriteBatch);
                    health3.Draw(spriteBatch);
                    health4.Draw(spriteBatch);
                    break;
                    
                case GameStates.CONTROLS:
                    spriteBatch.Draw(Content.Load<Texture2D>("controlmenu"), new Rectangle(0, 0, 1440, 800), Color.White);
                    rttrnbttn.Draw(spriteBatch);
                  


                    break;


                case GameStates.EXIT:

                    break;

                case GameStates.GAMEOVER:
                    spriteBatch.Draw(Content.Load<Texture2D>("gameoverback"), new Rectangle(0, 0, 1440, 800), Color.White);
                    exitbtn.Draw(spriteBatch);
                    DrawEndGameScore();
            
                    break;
            }








            //DrawBackground();
            //DrawScore();
            
            //DrawGameTime();
            //player1.Draw(spriteBatch);
            //player2.Draw(spriteBatch);
            //player3.Draw(spriteBatch);
            //player4.Draw(spriteBatch);
            //map.Draw(spriteBatch);


            //SpriteEffects effects = SpriteEffects.None;
            //if (player1.m_flipHorosontal) effects = SpriteEffects.FlipHorizontally;
            //if (player2.m_flipHorosontal) effects = SpriteEffects.FlipHorizontally;
            //if (player3.m_flipHorosontal) effects = SpriteEffects.FlipHorizontally;
            //if (player4.m_flipHorosontal) effects = SpriteEffects.FlipHorizontally;


            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

            //emitterRight.Draw(spriteBatch);

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }


        private void DrawEndGameScore()
        {
            spriteBatch.DrawString(scoreText, "PLAYER1 SCORE :", new Vector2(600, 300), Color.White);
            spriteBatch.DrawString(scoreText, player1.score.ToString(), new Vector2(800, 300), Color.White);

            spriteBatch.DrawString(scoreText, "PLAYER2 SCORE :", new Vector2(600, 350), Color.White);
            spriteBatch.DrawString(scoreText, player2.score.ToString(), new Vector2(800, 350), Color.White);

            spriteBatch.DrawString(scoreText, "PLAYER3 SCORE :", new Vector2(600, 400), Color.White);
            spriteBatch.DrawString(scoreText, player3.score.ToString(), new Vector2(800, 400), Color.White);

            spriteBatch.DrawString(scoreText, "PLAYER4 SCORE :", new Vector2(600, 450), Color.White);
            spriteBatch.DrawString(scoreText, player3.score.ToString(), new Vector2(800, 450), Color.White);
        }


    }
}
