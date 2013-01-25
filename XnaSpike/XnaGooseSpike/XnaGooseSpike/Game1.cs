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
using System.ComponentModel;

namespace XnaGooseGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class Game1 : Microsoft.Xna.Framework.Game
    {
        public const int VIEWPORT_WIDTH = 890;
        public const int VIEWPORT_HEIGHT = 672;
		public const bool MUSIC_ENABLED = false;
		public const bool ALLOW_BATMAN = false;

		public static Game1 Instance { get; private set; }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameScene scene;
		FpsLogger fpsLogger;
		PopulatonLogger deathLogger;

		BackgroundWorker updateWorker;

        GameMode mode;
        int level;
        int gooseCount;

        public Game1(GameMode mode, int level, int gooseCount)
        {
            this.mode = mode;
            this.level = level;
            this.gooseCount = gooseCount;
			this.IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = VIEWPORT_HEIGHT;
            graphics.PreferredBackBufferWidth = VIEWPORT_WIDTH;
			
			updateWorker = new BackgroundWorker();
			updateWorker.WorkerSupportsCancellation = true;
			updateWorker.DoWork += new DoWorkEventHandler(updateWorker_DoWork);
			updateWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(updateWorker_RunWorkerCompleted);

			Game1.Instance = this;
        }

		public GameMode Mode
		{
			get
			{
				return this.mode;
			}
		}

		public int Level
		{
			get
			{
				return this.level;
			}
		}

		public int GooseCount
		{
			get
			{
				return this.gooseCount;
			}
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

			if (this.mode == GameMode.Credits)
			{
				GameLevelManager.LoadCredits(Content);
			}
			else
			{
				GameLevelManager.LoadLevel(this.level, ALLOW_BATMAN, Content);
			}

            switch (this.mode)
            {
                case GameMode.Single:
                    this.scene = new SinglePlayerGameScene();
                    break;
                case GameMode.GeneticAlgorithm1:
                    this.scene = new GenerationGameScene(this.gooseCount);
                    break;
				case GameMode.BestGenerationAlgorithm:
					this.scene = new BestGenerationGameScene(this.gooseCount);
					break;
				case GameMode.AStar:
					this.scene = new AStarGameScene();
					break;
                case GameMode.Credits:
                default:
                    this.scene = new CreditsGameScene();
                    break;
            }
            
            //this.scene = new SinglePlayerGameScene(GraphicsDevice, background);
            this.scene.LoadContent(Content);
			this.fpsLogger = new FpsLogger(this.Content);
			this.deathLogger = new PopulatonLogger(this.Content); 
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

		GameTime offsetTime = new GameTime(TimeSpan.Zero, TimeSpan.Zero);

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

			if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.R))
			{
				if (!this.updateWorker.IsBusy)
				{
					this.updateWorker.RunWorkerAsync(offsetTime);
				} 
			}
			if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.T))
			{
				if (this.updateWorker.IsBusy)
				{
					this.updateWorker.CancelAsync();
				}
			}

			if (!this.updateWorker.IsBusy)
			{
				offsetTime = new GameTime(offsetTime.TotalGameTime.Add(gameTime.ElapsedGameTime), gameTime.ElapsedGameTime);
				this.scene.Update(offsetTime);
			}

            base.Update(gameTime);
        }


		void updateWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			
		}

		void updateWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			GameTime time = (GameTime)e.Argument;
			DateTime start = DateTime.Now;
			int cnt =0;
			while (true)
			{
				this.scene.Update(time);
				offsetTime = time;
				cnt ++;
				time = new GameTime(time.TotalGameTime.Add(TimeSpan.FromMilliseconds(1000d / 60)), TimeSpan.FromMilliseconds(1000d / 60));
				if (this.updateWorker.CancellationPending)
				{
					System.IO.File.WriteAllText("fff.txt", cnt + " " + ((DateTime.Now - start).TotalMilliseconds / 1000d) + " fps:" + (cnt / ((DateTime.Now - start).TotalMilliseconds / 1000d)));
					break;
				}
			}
		}


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin();
			if (!updateWorker.IsBusy)
			{
				this.scene.Draw(spriteBatch);
			}
            this.fpsLogger.Draw(gameTime, spriteBatch);
			this.deathLogger.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
