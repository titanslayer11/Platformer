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
    class Enemy
    {
        Sprite sprite = new Sprite();
        // keep a reference to the Game object to check for collisions on the map
        Game1 game = null;
        Vector2 velocity = Vector2.Zero;
        float pause = 0;
        bool moveRight = true;

        static float enemyAcceleration = Game1.acceleration / 5.0f;
        static Vector2 enemyMaxVelocity = Game1.maxVelocity / 5.0f;

        public Vector2 Position
        {
            get
            {
                return
                sprite.position;
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
                return
                sprite.Bounds;
            }
        }
        public Enemy(Game1 game)
        {
            this.game = game;
            velocity = Vector2.Zero;
        }
        public void Load(ContentManager content)
        {
            AnimatedTexture animation = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
            animation.Load(content,"monster", 1, 1);

            sprite.Add(animation, 0, 1);
        }
        public void Update(float deltaTime)
        {
            sprite.Update(deltaTime);

            if(pause > 0)
            {
                pause -= deltaTime;
            }
            else
            {
                float ddx = 0; // acceleration
                int tx = game.PixelToTile(Position.X);
                int ty = game.PixelToTile(Position.Y);
                bool nx = (Position.X) %
                Game1.tile != 0;
                // zombie overlaps right?
                bool ny = (Position.Y) %
                Game1.tile != 0;
                // zombie overlaps below?
                bool cell = game.CellAtTileCoord(tx, ty) != 0;
                bool cellright = game.CellAtTileCoord(tx + 1, ty) != 0;
                bool celldown = game.CellAtTileCoord(tx, ty + 1) != 0;
                bool celldiag = game.CellAtTileCoord(tx + 1, ty + 1) != 0;
                if(moveRight)
                {
                    if
                    (celldiag && !cellright)
                    {
                        ddx = ddx + enemyAcceleration;
                        // zombie wants to go right
                    }
                    else
                    {
                        this.velocity.X = 0;
                        this.moveRight = false;
                        this.pause = 0.5f;
                    }
                }
                if(!this.moveRight)
                {
                    if
                    (celldown && !cell)
                    {
                        ddx = ddx - enemyAcceleration;
                        // enemy wants to go left
                    }
                    else
                    {
                        this.velocity.X = 0;
                        this.moveRight = true;
                        this.pause = 0.5f;
                    }
                }
                Position = new Vector2 ((float)Math.Floor(Position.X + (deltaTime * velocity.X)), Position.Y);
                velocity.X = MathHelper.Clamp(velocity.X + (deltaTime * ddx), -enemyMaxVelocity.X, enemyMaxVelocity.X);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, Position);
        }
    }
}
