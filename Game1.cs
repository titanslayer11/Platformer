using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;

namespace Platformer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static int tile = 70;
        // abitary choice for in (1 tile = 1 meter)
        public static float meter = tile;
        // very exaggerated gravity (6x)
        public static float gravity = meter * 9.8f * 6.0f;
        // max vertical speed (10 tile/sec horizontal, 15 tile/sec vertical)
        public static Vector2 maxVelocity = new Vector2(meter * 10, meter * 15);
        // horizontal acceleration - take 1/2 second to reach max velocity
        public static float acceleration = maxVelocity.X * 2;
        // horizontal friction - take 1/6 second to stop from max velocity
        public static float friction = maxVelocity.X * 6;
        // (a large) instanteneous jump impluse
        public static float jumpImpulse = meter * 750;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Color _backgroundColour = Color.CornflowerBlue;

        private List<Component> _gameComponents;

        Player player = null;

        Song gameMusic;

        SpriteFont arialFont;
        int score = 0;
        int lives = 3;

        Texture2D heartImage = null;
        Texture2D SplashScreenSprite;

        Camera2D camera = null;
        TiledMap map = null;
        TiledMapRenderer mapRenderer = null;
        TiledMapTileLayer collisionLayer;

        List<Enemy> enemies = new List<Enemy>();
        List<Goal> goals = new List<Goal>();
        List<Particles_splat> splats = new List<Particles_splat>();

       
        enum GameState
        {
            Splash_State,
            Menu_State,
            Game_State, 
            GameLose_State,
            GameWin_State
        }

        GameState GetGameState = GameState.Splash_State;

        public int ScreenWidth
        {
            get
            {
                return graphics.GraphicsDevice.Viewport.Width;
            }
        }
        public int ScreenHeight
        {
            get
            {
                return graphics.GraphicsDevice.Viewport.Height;
            }
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player(this);
            player.Position = new Vector2(180, 6850); 
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //AIE.StateManager.CreateState("Splash", new SplashState());
           // AIE.StateManager.CreateState("Game", new GameState());
            //AIE.StateManager.CreateState("GameOver", new GameOverState());

            player.Load(Content);

            arialFont = Content.Load<SpriteFont>("Arial");
            heartImage = Content.Load<Texture2D>("heart (life)");
            SplashScreenSprite = Content.Load<Texture2D>("blizzardskull");

            gameMusic = Content.Load<Song>("Music/SuperHero_original_no_Intro");
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.Play(gameMusic);

            var playGameButton = new Button(Content.Load<Texture2D>("play button"), Content.Load<SpriteFont>("Arial"))
            {
                Position = new Vector2(ScreenWidth / 2 - 100, ScreenHeight / 2 - 100),
                Text = "",
            };

            playGameButton.Click += PlayGameButton_Click;

            var quitButton = new Button(Content.Load<Texture2D>("Quit button 2"), Content.Load<SpriteFont>("Arial"))
            {
                Position = new Vector2(ScreenWidth / 2 - 100, ScreenHeight / 2),
                Text = "",
            };

            quitButton.Click += QuitButton_Click;

            _gameComponents = new List<Component>()
            {
                playGameButton,
                quitButton
            };


            BoxingViewportAdapter viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice,
                ScreenWidth, ScreenHeight);

            camera = new Camera2D(viewportAdapter);
            camera.Position = new Vector2(0, ScreenHeight);

            map = Content.Load<TiledMap>("map 2"); 
            mapRenderer = new TiledMapRenderer(GraphicsDevice);

            foreach(TiledMapTileLayer layer in map.TileLayers)
            {
                if(layer.Name == "Tile Layer 1")
                {
                    collisionLayer = layer;
                }
            }

            foreach(TiledMapObjectLayer layer in map.ObjectLayers)
            {
                if (layer.Name == "Enemies")
                {
                    foreach(TiledMapObject obj in layer.Objects)
                    {
                        Enemy enemy = new Enemy(this);
                        enemy.Load(Content);
                        enemy.Position = new Vector2(obj.Position.X, obj.Position.Y);
                        enemies.Add(enemy);
                    }
                }
                if(layer.Name == "Loot") 
                {
                    
                    foreach(TiledMapObject obj in layer.Objects)
                    {
                        AnimatedTexture anim = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
                        anim.Load(Content, "gem_blue", 1, 1);

                        Goal goal = new Goal(this);
                        goal.Load(Content);
                        goal.Position = new Vector2(obj.Position.X, obj.Position.Y);
                        goals.Add(goal);
                    }
                }
            }
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            Exit();
        }
        private void PlayGameButton_Click(object sender, EventArgs e)
        {
            ChangeState(GameState.Game_State);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        bool RunOnce = false;
        float Timer = 3f;
                        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            


            //AIE.StateManager.Update(Content, gameTime);

            switch (GetGameState)
            {
                case GameState.Splash_State:

                    if(RunOnce != true)
                    {
                        Timer = 3f;
                        IsMouseVisible = false;
                        _backgroundColour = Color.White;
                        RunOnce = true;
                    }

                    Timer -= deltaTime;
                    if(Timer <= 0)
                    {
                        ChangeState(GameState.Menu_State);
                    }

                    break;
                case GameState.Menu_State:

                    if(RunOnce != true)
                    {
                        Timer = 3f;
                        IsMouseVisible = true;
                        _backgroundColour = Color.CornflowerBlue;
                        RunOnce = true;
                    }

                    foreach (var component in _gameComponents)
                    {
                        component.Update(gameTime);
                    }

                    break;
                case GameState.Game_State:

                    if(RunOnce != true)
                    {
                        MediaPlayer.Play(gameMusic);
                        IsMouseVisible = false;
                        _backgroundColour = Color.CornflowerBlue;
                        RunOnce = true;
                    }

                    player.Update(deltaTime);
                    ResetHit(deltaTime);
                    CheckLives();
                    CheckScore();
                    HasFellOutOfWorld();


                    foreach (Enemy e in enemies)
                    {
                        e.Update(deltaTime);
                    }
                    foreach (Particles_splat ps in splats)
                    {
                        ps.Update(deltaTime);
                    }

                    camera.Position = player.Position - new Vector2(ScreenWidth / 2, ScreenHeight / 2);
                    camera.Zoom = 0.5f;         // zooms in or out of the object being observed.

                    Console.WriteLine(player.Position);

                    CheckCollisions();

                    break;
                case GameState.GameLose_State:

                    if(RunOnce != true)
                    {
                        IsMouseVisible = true;
                        _backgroundColour = Color.Red;
                        RunOnce = true;
                    }

                    break;
                case GameState.GameWin_State:

                    if(RunOnce != true)
                    {
                        IsMouseVisible = true;
                        _backgroundColour = Color.Green;
                        RunOnce = true;
                    }

                    break;
                default:
                    break;
            }

            
            foreach(Particles_splat ps in splats)
            {
                if(ps.LifeTime <= 0)
                {
                    splats.Remove(ps);
                    break;
                }
            }

            base.Update(gameTime);
        }

        private void HasFellOutOfWorld()
        {
            if(player.Position.Y >= 7000)
            {
                lives = 0;
            }
        }
                
        void ChangeState(GameState ChangeToState)
        {
            GetGameState = ChangeToState;
            RunOnce = false;
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_backgroundColour);
                                                
            switch (GetGameState)
            {
                case GameState.Splash_State:

                    spriteBatch.Begin();

                    spriteBatch.Draw(SplashScreenSprite, new Vector2(ScreenWidth / 2 - SplashScreenSprite.Width / 2, ScreenHeight / 2 - SplashScreenSprite.Height / 2),
                        null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    spriteBatch.End();

                    break;
                case GameState.Menu_State:

                    spriteBatch.Begin();
                    
                    foreach(var component in _gameComponents)
                    {
                        component.Draw(gameTime, spriteBatch);
                    }

                    spriteBatch.End();

                    break;
                case GameState.Game_State:

                    Matrix viewMatrix = camera.GetViewMatrix();
                    Matrix projectionMatrix = Matrix.CreateOrthographicOffCenter(0,
                        GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0f, 0f, -1f);

                    spriteBatch.Begin(transformMatrix: viewMatrix);
                    mapRenderer.Draw(map, ref viewMatrix, ref projectionMatrix);

                    player.Draw(spriteBatch);

                    foreach (Enemy e in enemies)
                    {
                        e.Draw(spriteBatch);
                    }

                    foreach (Goal l in goals)
                    {
                        l.Draw(spriteBatch);
                    }

                    foreach (Particles_splat ps in splats)
                    {
                        ps.Draw(spriteBatch);
                    }

                    spriteBatch.End();

                    spriteBatch.Begin();

                    spriteBatch.DrawString(arialFont, "Score : " + score.ToString(), new Vector2(20, 20), Color.Red);

                    for (int i = 0; i < lives; i++)
                    {
                        spriteBatch.Draw(heartImage, new Vector2(ScreenWidth - 80 - i * 34, 44), Color.White);
                    }

                    spriteBatch.End();

                    break;
                case GameState.GameLose_State:

                    spriteBatch.Begin();

                    spriteBatch.DrawString(arialFont, "YOU DIED!? \n I THOUGHT THIS GAME WAS EASY!", new Vector2(ScreenWidth / 2 - arialFont.MeasureString("YOU DIED!?/ I THOUGHT THIS GAME WAS EASY!").X / 2, ScreenHeight / 2), Color.Black);

                    spriteBatch.End();

                    break;
                case GameState.GameWin_State:

                    spriteBatch.Begin();

                    spriteBatch.DrawString(arialFont, "YAY YOU WIN!\nDON'T BE BOTHERED CHEERING,\nTHIS GAME WAS MADE TO BE REALLY EASY", new Vector2(ScreenWidth / 2 - arialFont.MeasureString("YAY YOU WIN!/ DON'T BE BOTHERED CHEERING,/ THIS GAME WAS MADE TO BE REALLY EASY").X / 2, ScreenHeight / 2), Color.Black);

                    spriteBatch.End();

                    break;
                default:
                    break;
            }
            
            base.Draw(gameTime);
        }

        public int PixelToTile(float pixelCoord)
        {
            return (int)Math.Floor(pixelCoord / tile);
        }

        public int TileToPixel(int tileCoord)
        {
            return tile * tileCoord;
        }

        public int CellAtPixelCoord(Vector2 pixelCoords)
        {
            if (pixelCoords.X < 0 || pixelCoords.X > map.WidthInPixels || pixelCoords.Y < 0)
            {
                return 1;
            }
            if(pixelCoords.Y > map.HeightInPixels)
            {
                return 0;
            }
            return CellAtTileCoord(PixelToTile(pixelCoords.X), PixelToTile(pixelCoords.Y));
        }

        public int CellAtTileCoord(int tx, int ty)
        {
            if(tx < 0 || tx >= map.Width || ty < 0)
            {
                return 1;
            }
            if(ty >= map.Height)
            {
                return 0;
            }

            TiledMapTile? tile;
            collisionLayer.TryGetTile(tx, ty, out tile);
            return tile.Value.GlobalIdentifier; 

        }

        private void CheckLives()
        {
            if(lives <= 0)
            {
                ChangeState(GameState.GameLose_State);
            }
        }

        private void CheckScore()
        {
            if(score >= 16)
            {
                ChangeState(GameState.GameWin_State);
            }
        }

        private bool hit;
        private float resetTime = 1;
        private void ResetHit(float dt)
        {
            if (hit == false)
            {
                resetTime -= dt;
                if (resetTime <= 0)
                {
                    hit = false;
                    resetTime = 1;
                }
            }
        }
        


        private void CheckCollisions()
        {
            foreach (Enemy e in enemies)
            {
                if (IsColliding(player.Bounds, e.Bounds) == true)
                {
                    if (player.IsJumping && player.Velocity.Y > 0)
                    {
                        player.JumpOnCollision();
                        enemies.Remove(e);

                        Particles_splat splat = new Particles_splat(this);
                        splat.Load(Content);
                        splat.Position = new Vector2(e.Position.X, e.Position.Y);
                        splats.Add(splat);
                        break;
                    }
                    else if (hit == false)
                    {
                        lives--;
                        hit = true;
                    }
                }

            }

            foreach (Goal l in goals)
            {
                if (IsColliding(player.Bounds, l.Bounds) == true)
                {
                    goals.Remove(l);

                    score++;

                    Particles_splat splat = new Particles_splat(this);
                    splat.Load(Content);
                    splat.Position = new Vector2(l.Position.X, l.Position.Y);
                    splats.Add(splat);

                    break;
                }
            }
        }
        private bool IsColliding(Rectangle rect1, Rectangle rect2)
        {
            if (rect1.X + rect1.Width < rect2.X ||
                rect1.X > rect2.X + rect2.Width ||
                rect1.Y + rect1.Height < rect2.Y ||
                rect1.Y > rect2.Y + rect2.Height)
            {
                // these two rectangles are not colliding
                return false;
            }
            else
            {
                return true;
            }
                
            // else, the two AABB rectangles overlap, therefore collision
            
        }

    }
    
}
