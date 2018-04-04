using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class Player
    {
        sprite sprite = new sprite();
        
        public Player()
        {
            
        }

        public void Load(ContentManager content)
        {
            sprite.Load(content, "hero");
        }

        public void Update(float deltaTime)
        {
            KeyboardState state = Keyboard.GetState();
            int speed = 50;

            if(state.IsKeyDown(Keys.Up) == true)
            {
                sprite.position.Y -= speed * deltaTime;
            }
            if(state.IsKeyDown(Keys.Down) == true)
            {
                sprite.position.Y += speed * deltaTime;
            }
            if(state.IsKeyDown(Keys.Left) == true)
            {
                sprite.position.X -= speed * deltaTime;
            }
            if(state.IsKeyDown(Keys.Right) == true)
            {
                sprite.position.X += speed * deltaTime;
            }

            sprite.Update(deltaTime);
                  
                 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}
