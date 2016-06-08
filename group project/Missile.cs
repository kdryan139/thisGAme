using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;


namespace group_project
{
    public class missile
    {
        GraphicsDeviceManager graphics;

        public int playerNum;
        public Vector2 Position;
        public Vector2 Velocity;
        public float Rotation;
        public Rectangle rectangle;
        public Vector2 Size;
        public Vector2 MaxLimit;
        public Vector2 MinLimit;
        //public bool powerup;

      
        SoundPlayer soundShoot;
        public Texture2D missileTexture;

        public static List<missile> myMissiles;

        TimeSpan lastShotPlayer2 = new TimeSpan(0, 0, 0, 0, 0);
        TimeSpan shotCoolDownPlayer2 = new TimeSpan(0, 0, 0, 0, 100);

        private static ContentManager content;
        public static ContentManager Content
        {
            protected get { return content; }
            set { content = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (missile Missile in myMissiles)
            {
                Color misslecolor;

                if (Missile.playerNum == 1)
                    misslecolor = Color.HotPink;
                else
                    misslecolor = Color.Orange;

                spriteBatch.Draw(missileTexture
               , Position
               , null
               , misslecolor
               , Rotation
               , new Vector2(missileTexture.Width / 2, missileTexture.Height / 2)
               , new Vector2(Size.X / missileTexture.Width, Size.Y / missileTexture.Height)
               , SpriteEffects.None
               , 0);
            }
        }

        

        public void drawMissiles(SpriteBatch spriteBatch)
        {
            foreach (missile missile in myMissiles)
            {
                Color misslecolor;

                misslecolor = Color.Orange;

                spriteBatch.Draw(missileTexture
               , missile.Position
               , null
               , misslecolor
               , missile.Rotation
               , new Vector2(missileTexture.Width / 2, missileTexture.Height / 2)
               , new Vector2(missile.Size.X / missileTexture.Width, missile.Size.Y / missileTexture.Height)
               , SpriteEffects.None
               , 0);
            }

           
        }

      

        public void updateMissiles()
        {
            foreach (missile missile in myMissiles)
            {
                missile.Position += missile.Velocity;
            }
        }

        public void LoadContent()
        {
            soundShoot = new SoundPlayer();
            soundShoot.SoundLocation = ("sounds\\shoot.wav");
            soundShoot.Load();

            missileTexture = Content.Load<Texture2D>("bullet");
        }
    }
}
