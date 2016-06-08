using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_project
{
    class Powerup
    {
        public Rectangle rectangle;
        public Texture2D texture;
        public Timer powerTimer;
        public int type;
        public bool existing;
        public bool visible;

        public enum powerUp
        {
            SPEED,
            FIRERATE,
            DOUBLEJUMP
        }

        public powerUp thisPowerup;

        public void CreatePowerup(Player player, Timer timer)
        {
            if (timer.finished == true && existing == false)
            {
                Random randNum;
                randNum = new Random();
                type = randNum.Next(1, 4);
                rectangle = new Rectangle(randNum.Next(0, 800), randNum.Next(0, 400), 32, 32);
                powerTimer = new Timer(0);

                switch (type)
                {
                    case 1:
                        thisPowerup = powerUp.DOUBLEJUMP;
                        break;

                    case 2:
                        thisPowerup = powerUp.FIRERATE;
                        break;

                    case 3:
                        thisPowerup = powerUp.SPEED;
                        break;
                }
                existing = true;
                visible = true;
                timer.timer = 20;
                timer.finished = false;
            }
            if (visible == true && timer.finished == true)
            {
                visible = false;
                existing = false;
                timer.timer = 20;
                timer.finished = false;
            }
        }

        public void Update(Player player, GameTime gameTime, Timer timer)
        {
            CreatePowerup(player, timer);

            if (existing == true)
            {
                powerTimer.update(gameTime, 0f);

                if (rectangle.Intersects(player.rectangle) && visible == true)
                {
                    switch (thisPowerup)
                    {
                        case powerUp.SPEED:
                            player.movementSpeed *= 2;
                            powerTimer.timer = 10f;
                            break;

                        case powerUp.FIRERATE:
                            //player.position = new Vector2(10, 10);
                            
                            powerTimer.timer = 10f;
                            break;

                        case powerUp.DOUBLEJUMP:
                            player.maxJumps = 2;
                            powerTimer.timer = 10f;
                            break;
                    }
                    visible = false;
                }
                if (powerTimer.finished == true)
                {
                    player.movementSpeed = 3f;
                    player.maxJumps = 1;
                    
                    existing = false;
                }
            }

            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (visible == true)
            {
                spriteBatch.Draw(texture, rectangle, Color.White);
            }
        }
    }
}
