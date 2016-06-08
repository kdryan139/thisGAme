using Microsoft.Xna.Framework;

namespace group_project
{
    public class Timer 
    {
        public float timer;
        public bool finished;

        public Timer (float time)
        {
            timer = time;
        }

        public void update(GameTime gameTime, float reset)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer < 0 || timer - delta == 0)
            {
                timer = reset;
                finished = true;
            }
            else if (timer > 0)
            {
                timer -= delta;
                finished = false;
            }
        }
    }
}
