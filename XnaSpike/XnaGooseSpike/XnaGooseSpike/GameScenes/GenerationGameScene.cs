using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace XnaGooseGame
{
	class GenerationGameScene : GameScene
	{
		List<PlayerController> players;
		double speedMultiplier = 1.0;
		int playersCount;
		bool started = false;
		List<CoinElement> coins;

		public GenerationGameScene(int playersCount)
		{
			this.playersCount = playersCount;
			this.InitializePlayers();
			this.InitializeCoins();
		}

		protected virtual void InitializePlayers()
		{
			this.players = new List<PlayerController>();
			for (int i = 0; i < playersCount; i++)
			{
				PlayerController info = new PlayerController(new PlayerElement());
				this.players.Add(info);
				this.Elements.Add(info.Player);
			}
		}

		private void InitializeCoins()
		{
			this.coins = new List<CoinElement>()
			{
				new CoinElement(780, 360),
				new CoinElement(520, 550),
			};
			this.Elements.AddRange(this.coins);
		}

		public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
		{
			base.LoadContent(Content);
			foreach (SceneElement element in this.Elements)
			{
				element.LoadContent(Content);
			}
		}

		public override void Update(GameTime gameTime)
		{
			gameTime = new GameTime(
				TimeSpan.FromMilliseconds(gameTime.TotalGameTime.TotalMilliseconds * speedMultiplier),
				TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.TotalMilliseconds * speedMultiplier),
				gameTime.IsRunningSlowly);

			if (Keyboard.GetState().IsKeyDown(Keys.Enter))
			{
				Start(gameTime);
			}

			if (this.started)
			{
				foreach (PlayerController info in this.players)
				{
					info.Update(gameTime);
				}
			}

			base.Update(gameTime);
		}

		private void Start(GameTime gameTime)
		{
			if (started) return;
			started = true;
			foreach (PlayerController info in this.players)
			{
				info.Start(gameTime);
			}
		}
	}
}