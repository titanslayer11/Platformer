﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class sprite
    {
        
        public Vector2 offset = Vector2.Zero;

        Texture2D texture;

        public sprite()
        {
        }

        public void Load(ContentManager content, string asset)
        {
            texture = content.Load<Texture2D>(asset);
        }

        public void Update(float deltaTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects = SpriteEffects.None)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0, offset, 1, effects, 0);
        }
    }
}
