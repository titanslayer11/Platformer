using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ParticleEffects;

namespace Platformer
{
    class Particles_splat
    {

        
        Game1 game = null;
        Vector2 position = new Vector2(0, 0);

        Emitter burstEmitter = null;
        Texture2D burstTexture = null;

        float lifeTime = 1;

        public float LifeTime
        {
            get
            {
                return lifeTime;
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        
        public Particles_splat(Game1 game)
        {
            this.game = game;
        }
        public void Load(ContentManager content)
        {
            burstTexture = content.Load<Texture2D>("spark");
            burstEmitter = new Emitter(burstTexture, position);

        }
        public void Update(float deltaTime)
        {
            burstEmitter.position = position;
            //effects changes

            burstEmitter.emissionRate = 15;
            burstEmitter.transparency = 0.75f;

            burstEmitter.Update(deltaTime);

            lifeTime -= deltaTime;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }

}

