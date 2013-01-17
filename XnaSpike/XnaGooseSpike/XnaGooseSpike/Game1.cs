using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XnaGooseGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public const int VIEWPORT_WIDTH = 890;
        public const int VIEWPORT_HEIGHT = 672;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameScene scene;
		FpsLogger fpsLogger;
		DeathLogger deathLogger;

        GameMode mode;
        int level;
        int gooseCount;

        public Game1(GameMode mode, int level, int gooseCount)
        {
            this.mode = mode;
            this.level = level;
            this.gooseCount = gooseCount;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = VIEWPORT_HEIGHT;
            graphics.PreferredBackBufferWidth = VIEWPORT_WIDTH; 
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
            
            GameLevelManager.LoadLevel(this.level, Content);

            switch (this.mode)
            {
                case GameMode.Single:
                    this.scene = new SinglePlayerGameScene();
                    break;
                case GameMode.GeneticAlgorithm1:
                    this.scene = new GenerationGameScene(this.gooseCount);
                    break;
                case GameMode.Credits:
                default:
                   // this.scene = new CreditsGameScene(GraphicsDevice, null);
                    break;
            }
            
            //this.scene = new SinglePlayerGameScene(GraphicsDevice, background);
            this.scene.LoadContent(Content);
			this.fpsLogger = new FpsLogger(this.Content);
			this.deathLogger = new DeathLogger(this.Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
				Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				this.Exit();
			}

            this.scene.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin();
            this.scene.Draw(spriteBatch);
            this.fpsLogger.Draw(gameTime, spriteBatch);
			this.deathLogger.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
