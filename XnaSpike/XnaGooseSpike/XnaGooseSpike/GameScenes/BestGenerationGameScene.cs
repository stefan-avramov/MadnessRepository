using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace XnaGooseGame
{
	class BestGenerationGameScene : GameScene
	{
		IList<PlayerController> aliveControllers;
		IList<PlayerController> deadControllers;
		readonly int playersCount;
		bool started = false;

		public BestGenerationGameScene(int playersCount)
		{
			this.playersCount = playersCount;
			this.InitializePlayers();
		}

		protected virtual void InitializePlayers()
		{
			this.aliveControllers = new List<PlayerController>();
			this.deadControllers = new List<PlayerController>();
			for (int i = 0; i < playersCount; i++)
			{
				PlayerController playerController = new PlayerController(new PlayerElement());
				this.aliveControllers.Add(playerController);
				this.Elements.Add(playerController.Player);
			}
		}

		public override void Update(GameTime gameTime)
		{
			if (this.started)
			{
				if (aliveControllers.Count < playersCount - PopulatonLogger.Wins && aliveControllers.Count > 0)
				{
					PlayerController randomAliveController = aliveControllers[RandomGenerator.Next(0, aliveControllers.Count)];
					PlayerController newController = randomAliveController.Clone(gameTime);
					this.Elements.Add(newController.Player);
					aliveControllers.Add(newController); 
				}

				for (int index = 0; index < this.aliveControllers.Count; index++)
				{
					PlayerController controller = this.aliveControllers[index];
					controller.Update(gameTime);
					if (controller.Player.IsDead || controller.Player.HasWon)
					{
						this.deadControllers.Add(this.aliveControllers[index]);
						this.aliveControllers.RemoveAt(index);
						index--;
					}
				}
			}

			base.Update(gameTime);
		}

		protected override void Start(GameTime gameTime)
		{
			if (!started)
			{
				started = true;
				foreach (PlayerController info in this.aliveControllers)
				{
					info.Start(gameTime);
				}
			}
		}

		protected override IEnumerable<PlayerElement> GetPlayers()
		{
			return this.aliveControllers.Select(x => x.Player);
		}
	}
}
