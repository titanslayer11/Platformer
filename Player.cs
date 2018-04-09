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
        public Vector2 position = Vector2.Zero;

        sprite sprite = new sprite();
        SpriteEffects effects = SpriteEffects.None;

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
                position.Y -= speed * deltaTime;
                effects = SpriteEffects.None;
            }
            if(state.IsKeyDown(Keys.Down) == true)
            {
                position.Y += speed * deltaTime;
                effects = SpriteEffects.FlipVertically;
            }
            if(state.IsKeyDown(Keys.Left) == true)
            {
                position.X -= speed * deltaTime;
                effects |= SpriteEffects.FlipHorizontally;
            }
            if(state.IsKeyDown(Keys.Right) == true)
            {
                position.X += speed * deltaTime;
                effects = SpriteEffects.None;
            }

            sprite.Update(deltaTime);
                
                
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position, effects);
        }
    }
}
