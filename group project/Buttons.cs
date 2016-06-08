using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace group_project
{
    class Buttons
    {
        public Texture2D texture;
        public Vector2 position;
        public Rectangle rectangle;

        public Vector2 centre;
        public Matrix transform;
        private Matrix Transform
        {
            get { return transform; }
        }


        public Viewport viewport;
        public Buttons(Viewport newViewport)
        {
            viewport = newViewport;

        }



        Color colour = new Color(255, 255, 255, 255);

        public Vector2 size;

        public void Load(Texture2D newTexture, Vector2 newPosition)
        {
            texture = newTexture;
            position = newPosition;
        }

        public Buttons(Texture2D newTexture, GraphicsDevice graphics)
        {
            texture = newTexture;


            size = new Vector2(graphics.Viewport.Width / 8, graphics.Viewport.Height / 30);

        }

        public Buttons()
        {
        }

        bool down;
        public bool isClicked;


        public void Update(MouseState mouse, GameTime gameTime)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y,
                (int)size.X, (int)size.Y);

            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(rectangle))
            {
                if (colour.A == 255) down = false;
                if (colour.A == 0) down = true;
                if (down) colour.A += 3; else colour.A -= 3;
                if (mouse.LeftButton == ButtonState.Pressed) isClicked = true;
            }
            else if (colour.A < 255)
            {
                colour.A += 3;
                isClicked = false;
            }


            transform = Matrix.CreateTranslation(new Vector3(-centre.X + (viewport.Width / 2)
            , -centre.Y + (viewport.Height / 2), 0));

        }





        public void setPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, colour);
        }
    }
}
