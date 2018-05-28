﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Platformer
{
    class Goal
    {
        Sprite sprite = new Sprite();
        // keep a reference to the Game object to check for collisions on the map
        Game1 game = null;

        

        
      
        public Vector2 Position
        {
            get
            {
                return sprite.position;
            }
            set
            {
                sprite.position = value;
            }
        }
        public Rectangle Bounds
        {
            get
            {
                return sprite.Bounds;
            }
        }
        public Goal(Game1 game)
        {
            this.game = game;
        }
        public void Load(ContentManager content)
        {
            AnimatedTexture animation = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
            animation.Load(content, "gem_blue", 1, 1);

            sprite.Add(animation, 0, 1);
        }
        public void Update(float deltaTime)
        {
            sprite.Update(deltaTime);

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, Position);
        }
    }
}

