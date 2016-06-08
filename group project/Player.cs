using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace group_project
{
    public class Player
    {
        GraphicsDeviceManager graphics;

        public static List<missile> myMissiles;

        public int health = 0;
 

        public int score = 0;
        public int playerNum;
        public Texture2D texture;
        public Vector2 position = new Vector2(400, 400);
        public Vector2 velocity;
        public Vector2 size;
        public float acceleration;
        public Rectangle rectangle;
        public int DeathTimer = 0;
        public int InvulnerabilityTimer = 0;
        public bool Visible;
        public bool Vulnerable;
        public bool Dead;
        public bool Invulnerable = true;
        public float m_respawnTime;
        public float m_invulnerabillityTimer;
        public float m_invulnerabillity;
        public float m_respawnTimer;
        public float m_invulnerabillityTime;
        public float respawnTime;
        public bool hasJumped = false;
        public bool m_flipHorosontal = false;
        public bool m_grounded;
        public float m_groundHeight;
        public float gravity;
        public bool grounded;
        public SpriteEffects flip;
        public float movementSpeed;
        int timesJumped;
        public int maxJumps;
        public Timer JumpTimer;
        public bool left;
        public TimeSpan lastShot = new TimeSpan(0, 0, 0, 0, 0);
        public TimeSpan shotCoolDown = new TimeSpan(0, 0, 0, 0, 100);
        public Texture2D textureLeft;

        Player player1;
        Player player2;


        public void createMissile(GameTime gameTime, Player objPlayer, List<missile> myMissiles)
        {
            TimeSpan timeScinceLastShot = gameTime.TotalGameTime - objPlayer.lastShot;

            if (timeScinceLastShot > objPlayer.shotCoolDown)
            {
                missile missile = new missile();

                missile.playerNum = objPlayer.playerNum;
                
                if (left == true)
                {
                    missile.Position = new Vector2(objPlayer.position.X, objPlayer.position.Y + objPlayer.texture.Height / 2 - 7);
                }
                else
                {
                    missile.Position = new Vector2(objPlayer.position.X + objPlayer.texture.Width, objPlayer.position.Y + objPlayer.texture.Height / 2 - 7);
                }
                missile.rectangle = new Rectangle((int)objPlayer.position.X + objPlayer.texture.Width / 2, (int)objPlayer.position.Y + objPlayer.texture.Height / 2 -10, 16, 16);
                missile.Rotation = 1.6f;

                Matrix missleRotationMatrix = Matrix.CreateRotationZ(missile.Rotation);
                if (left == true)
                    missile.Velocity = new Vector2(-10, 0);
                else
                    missile.Velocity = new Vector2(10, 0);

                missile.MaxLimit = new Vector2(2000, 2000);
                missile.MinLimit = new Vector2(-50, -50);
                missile.Size = new Vector2(6, 10);

                myMissiles.Add(missile);
                objPlayer.lastShot = gameTime.TotalGameTime;

                //soundShoot.Play();

            }
        }



        public Player(Texture2D newTexture, Vector2 newPosition,int iPlayer, int newHealth, List<missile> myMissiles)
        {
            health = newHealth;
    
            playerNum = iPlayer;
            texture = newTexture;
            position = newPosition;
            movementSpeed = 3f;
            maxJumps = 1;
            myMissiles = new List<missile>();
            JumpTimer = new Timer(0);
        }

        public void Respawn()
        {
            if (playerNum == 1)
                position = new Vector2(200, 400);

            else if (playerNum == 2)
                position = new Vector2(600, 400);
            else if (playerNum == 3)
                position = new Vector2(400, 400);
            else if (playerNum == 4)
                position = new Vector2(800, 200);

            velocity = new Vector2(0, 0);
            acceleration = 0f;
            Dead = false;
            Visible = true;
            health = 200;
        }

        public void Die()
        {
            if (Vulnerable)
            {
                Dead = true;
                Visible = false;
                Vulnerable = false;
            }
        }

        public void initialisePlayer(Vector2 position, int iPlayer)
        {
            
            playerNum = iPlayer;
            Visible = true;
            Vulnerable = false;
            Dead = false;
            m_respawnTimer = 0f;
            m_respawnTime = 1.0f;
            m_invulnerabillityTimer = 4.0f;
            m_invulnerabillityTime = 5.0f;

        }

        private void PlayerDie(GameTime gameTime)
        {
            DeathTimer = 0;
            InvulnerabilityTimer = 0;
            m_invulnerabillityTimer = 0;
            m_respawnTimer = 0;
            Dead = true;
            Invulnerable = true;
            Visible = false;
            velocity = new Vector2(0, 0);
        }


        public Vector2 Position 
        {
            get { return position; }
        }

     

        public void Update(GameTime gameTime, Player objPlayer, List<missile> myMissiles, Player Enemy)
        {
            position += velocity;
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            JumpTimer.update(gameTime, 0);

            if (velocity.Y < 1)
                velocity.Y += 0.4f;

            

            float Delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;


            if (Dead && m_invulnerabillityTimer == 0f)
            {
                m_invulnerabillityTimer = m_invulnerabillityTime;
                m_respawnTimer = m_respawnTime;
            }

            if (m_respawnTimer > 0)
            {
                m_respawnTimer -= Delta;
            }
            else if (m_respawnTimer < 0)
            {
                m_respawnTimer = 0;
                Respawn();
            }

            if (m_invulnerabillityTimer > 0)
            {
                m_invulnerabillityTimer -= Delta;
            }
            else if (m_invulnerabillityTimer < 0)
            {
                m_invulnerabillityTimer = 0;
                Vulnerable = true;
            }
            if (m_invulnerabillityTimer > 0 && m_respawnTimer == 0)
            {
                Visible = !Visible;
            }
            else if (m_invulnerabillityTimer == 0 && m_respawnTimer == 0)
            {
                Visible = true;
            }
            
            if (health <= 0)
            {
                Respawn();
            }

        }


        public void Input(GameTime gameTime, Keys moveLeft, Keys moveRight, Keys jump, Keys Shoot, Player playernumber, List<missile> myMissiles)
        {
            if (Dead == false)
            {

                if (Keyboard.GetState().IsKeyDown(moveRight))
                {
                    velocity.X = movementSpeed;
                    flip = SpriteEffects.None;
                    left = false;
                }
                else if (Keyboard.GetState().IsKeyDown(moveLeft))
                {
                    velocity.X = -movementSpeed;
                    flip = SpriteEffects.FlipHorizontally;
                    left = true;
                }
                else velocity.X = 0f;



                if (Keyboard.GetState().IsKeyDown(Shoot))
                {
                    createMissile(gameTime, playernumber, myMissiles);
                }


                if ((playerNum == 1 && Keyboard.GetState().IsKeyDown(jump) && hasJumped == false)|| (playerNum == 2 && Keyboard.GetState().IsKeyDown(jump) && hasJumped == false))
                {

                    position.Y -= 9f;
                    velocity.Y = -12f;
                    hasJumped = true;
                    timesJumped++;
       
              
                    if (timesJumped < maxJumps)
                    {
                        JumpTimer.timer = 0.5f;
                    }

                }


                if (JumpTimer.finished == true && timesJumped <= maxJumps)
                {
                    hasJumped = false;
                }

                if ((playerNum == 1 && Keyboard.GetState().IsKeyDown(Keys.Space) && hasJumped == false) || (playerNum == 2 && Keyboard.GetState().IsKeyDown(Keys.NumPad0) && hasJumped == false))
                {
                    TimeSpan timeScinceLastShot = gameTime.TotalGameTime - lastShot;

                    missile missile = new missile();
                    if (Keyboard.GetState().IsKeyDown(Shoot))
                    {
                        if (timeScinceLastShot > shotCoolDown)
                        {
                            missile.Position = position;

                            missile.Velocity = new Vector2(2, 0);

                            missile.Size = new Vector2(16, 16);

                            missile.Velocity = missile.Velocity + velocity;

                            missile.MaxLimit = new Vector2(graphics.PreferredBackBufferWidth + 500,
                                graphics.PreferredBackBufferHeight + 500);
                            missile.MinLimit = new Vector2(-500, -500);

                            myMissiles.Add(missile);

                            lastShot = gameTime.TotalGameTime;
                        }
                    }
                }

                float i = 1;
                velocity.Y += 0.05f * i;
            }
        }

        public void CheckCollision(Rectangle newRectangle, int xOffset, int yOffset)
        {
            if (rectangle.TouchTopOf(newRectangle))
            {
                rectangle.Y = newRectangle.Y - rectangle.Height;
                velocity.Y = 0f;
                hasJumped = false;
                timesJumped = 0;
            }

            if (rectangle.TouchLeftOf(newRectangle))
            {
                position.X = newRectangle.X - rectangle.Width - 2;
            }
            if (rectangle.TouchRightOf(newRectangle))
            {
                position.X = newRectangle.X + newRectangle.Width + 2;
            }
            //if (rectangle.TouchBottomOf(newRectangle))
            //    velocity.Y = 1f;





        }

        public void Draw(SpriteBatch spritebatch, Texture2D missileTexture, List<missile> myMissiles)
        {
            if (left)
            {
                spritebatch.Draw(textureLeft, rectangle, Color.White);
            }
            else
            {
                spritebatch.Draw(texture, rectangle, Color.White);
            }

            foreach (missile missile in myMissiles)
            {
                spritebatch.Draw(missileTexture,
                    missile.Position
               , null
               , Color.White
               , missile.Rotation
               , new Vector2(missileTexture.Width / 2, missileTexture.Height / 2)
               , new Vector2(missile.Size.X / missileTexture.Width, missile.Size.Y / missileTexture.Height)
               , SpriteEffects.None
               , 0);
            }
        }
    }
}
        