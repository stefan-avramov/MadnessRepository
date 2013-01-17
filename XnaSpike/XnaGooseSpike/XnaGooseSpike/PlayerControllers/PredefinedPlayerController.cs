using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace XnaGooseGame
{
	class PredefinedPlayerController
	{
		public PlayerElement Player { get; private set; }

		public List<PlayerAction> Actions { get; private set; }

		public double ActionDuration { get; set; }

		private int lastActionIndex = -1;
		private TimeSpan start;

		public PredefinedPlayerController(PlayerElement player)
		{
			this.Player = player;
			this.Actions = new List<PlayerAction>();
			this.ActionDuration = 300;

			for (int i = 0; i < 300; i++)
			{
				int next = RandomGenerator.Next(0, 1000);
				PlayerAction action = next < 50 ? PlayerAction.MoveBackward : next < 700 ? PlayerAction.Jump : PlayerAction.MoveForward;
				this.Actions.Add(action);
			}
		}

		public void Start(GameTime gameTime)
		{
			start = gameTime.TotalGameTime;
			lastActionIndex = -1;
		}

		public void Update(GameTime gameTime)
		{
			int index = (int)((gameTime.TotalGameTime - start).TotalMilliseconds / this.ActionDuration);
			if (index < 0 || index >= Actions.Count)
			{
				Player.Die();
				return;
			}

			if (lastActionIndex < index)
			{
				lastActionIndex = index;
				switch (Actions[index])
				{
					case PlayerAction.MoveForward:
						Player.MoveForward();
						break;
					case PlayerAction.MoveBackward:
						Player.MoveBackward();
						break;
					case PlayerAction.Jump:
						Player.Jump();
						break;
				}
			}
		}
	}
}