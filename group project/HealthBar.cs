using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace group_project
{
    class HealthBar
    {
        public Rectangle rectangle;
        public Rectangle rectangle2;
        public Rectangle rectangle3;
        public Rectangle rectangle4;
        public Texture2D texture;
        public Vector2 position;
        public Vector2 size;


       
        

        public void setPosition(Vector2 newPosition)
        {
            position = newPosition;

           
        }


        public void Update(GameTime gameTime, Player player1, Player player2, Player player3, Player player4)
        {
            rectangle = new Rectangle(35, 40, player1.health, 25);
            rectangle2 = new Rectangle(310, 40, player2.health, 25);

            rectangle3 = new Rectangle(610, 40, player3.health, 25);
            rectangle4 = new Rectangle(910, 40, player4.health, 25);








        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);

            spriteBatch.Draw(texture, rectangle2, Color.White);

            spriteBatch.Draw(texture, rectangle3, Color.White);

            spriteBatch.Draw(texture, rectangle4, Color.White);
        }

      




    }
}
