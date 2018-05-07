using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    public class GameOverState : AIE.State
    {
        bool isLoaded = false;
        SpriteFont font = null;

        KeyboardState oldState;

        public GameOverState() : base()
        {

        }

        public override void CleanUp()
        {
            font = null;
            isLoaded = false;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
                spriteBatch.DrawString(font, "Game Over!", new Vector2(200, 200), Color.White);
            spriteBatch.End();
        }
        public override void Update(ContentManager Content, GameTime gameTime)
        {
            if(isLoaded == false)
            {
                font = Content.Load<SpriteFont>("Arial");
                oldState = Keyboard.GetState();
                isLoaded = true;
            }

            KeyboardState newState = Keyboard.GetState();

            if(newState.IsKeyDown(Keys.Enter) == true)
            {
                if(oldState.IsKeyDown(Keys.Enter) == false)
                {
                    AIE.StateManager.ChangeState("Splash");
                }
            }

            oldState = newState;
        }
    }
}
