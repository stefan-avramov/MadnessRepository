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

		public override void LoadContent(ContentManager content)
		{
			base.LoadContent(content);
			foreach (SceneElement element in this.Elements)
			{
				element.LoadContent(content);
			}
		}

		public override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Enter))
			{
				Start(gameTime);
			}

			if (this.started)
			{
				if (aliveControllers.Count < playersCount)
				{
					PlayerController randomAliveController = aliveControllers[RandomGenerator.Next(0, aliveControllers.Count)];
					PlayerController newController = randomAliveController.Clone();
					this.Elements.Add(newController.Player);
					aliveControllers.Add(newController); 
				}

				for (int index = 0; index < this.aliveControllers.Count; index++)
				{
					this.aliveControllers[index].Update(gameTime);
					if (this.aliveControllers[index].Player.IsDead)
					{
						this.deadControllers.Add(this.aliveControllers[index]);
						this.aliveControllers.RemoveAt(index);
						index--;
					}
				}
			}

			base.Update(gameTime);
		}

		private void Start(GameTime gameTime)
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
	}
}
