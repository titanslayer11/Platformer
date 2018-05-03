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
    class Sprite
    {
        public Vector2 position = Vector2.Zero;
        List<AnimatedTexture> animations = new List<AnimatedTexture>();
        List<Vector2> animationOffsets = new List<Vector2>();

        int currentAnimation = 0;

        SpriteEffects effects = SpriteEffects.None;

        public Sprite()
        {
        }
        public void Add(AnimatedTexture animation, int xOffset=0, int yOffset=0)
        {
            animations.Add(animation);
            animationOffsets.Add(new Vector2(xOffset, yOffset));
        }

        public void Load(ContentManager content, string asset)
        {
            //texture = content.Load<Texture2D>(asset);
        }

        public void Update(float deltaTime)
        {
            animations[currentAnimation].UpdateFrame(deltaTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, position, null, Color.White, 0, offset, 1, effects, 0);
            animations[currentAnimation].DrawFrame(spriteBatch,
                        position + animationOffsets[currentAnimation], effects);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            //spriteBatch.Draw(texture, position, null, Color.White, 0, offset, 1, effects, 0);
            animations[currentAnimation].DrawFrame(spriteBatch, 
                        position + animationOffsets[currentAnimation], effects);
        }

        public void SetFlipped(bool state)
        {
            if (state == true)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            else
            {
                effects = SpriteEffects.None;
            }   
        }

        public void Pause()
        {
            animations[currentAnimation].Pause();
        }

        public void Play()
        {
            animations[currentAnimation].Play();
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(new Point((int) position.X, (int)position.Y), animations[currentAnimation].FrameSize);
            }
        }

    }
}
