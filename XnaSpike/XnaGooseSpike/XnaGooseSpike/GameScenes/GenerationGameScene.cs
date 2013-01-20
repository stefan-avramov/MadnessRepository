using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using System.Linq;

namespace XnaGooseGame
{
	class GenerationGameScene : GameScene
	{
		List<DymamicPlayerController> players;
		double speedMultiplier = 1.0;
		int playersCount;
		bool started = false;
		List<CoinElement> coins;

		public GenerationGameScene(int playersCount)
		{
			this.playersCount = playersCount;
			this.InitializePlayers(); 
		}
		 
		protected virtual void InitializePlayers()
		{
			this.players = new List<DymamicPlayerController>();
			for (int i = 0; i < playersCount; i++)
			{
				DymamicPlayerController info = new DymamicPlayerController(new PlayerElement());
				this.players.Add(info);
				this.Elements.Add(info.Player);
			}
		}

		public override void Update(GameTime gameTime)
		{
			gameTime = new GameTime(
				TimeSpan.FromMilliseconds(gameTime.TotalGameTime.TotalMilliseconds * speedMultiplier),
				TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.TotalMilliseconds * speedMultiplier),
				gameTime.IsRunningSlowly);

			if (this.started)
			{
				foreach (DymamicPlayerController info in this.players)
				{
					info.Update(gameTime);
				}
			}

			base.Update(gameTime);

			this.IntersectSceneObjects();
		}

		private void IntersectSceneObjects()
		{
			
		}

		protected override void Start(GameTime gameTime)
		{
			if (started) return;
			started = true;
			foreach (DymamicPlayerController info in this.players)
			{
				info.Start(gameTime);
			}
		}

		protected override IEnumerable<PlayerElement> GetPlayers()
		{
			return this.players.Select(x => x.Player);
		}
	}
}