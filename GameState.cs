using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    public class GameState : AIE.State
    {
        bool isLoaded = false;
        SpriteFont font = null;

        public GameState() : base()
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
                spriteBatch.DrawString(font, "You are playing the game.", new Vector2(200, 200), Color.White);
            spriteBatch.End();
        }
        public override void Update(ContentManager Content, GameTime gameTime)
        {
            if(isLoaded == false)
            {
                font = Content.Load<SpriteFont>("Arial");
                isLoaded = true;
            }

            if(Keyboard.GetState().IsKeyDown(Keys.Enter) == true)
            {
                AIE.StateManager.ChangeState("GAMEOVER");
            }
        }
    }
}
