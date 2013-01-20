using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace XnaGooseGame
{
	class DymamicPlayerController : PlayerController
	{
		public List<PlayerAction> ActionsHistory { get; private set; }

		private int lastActionIndex = -1;
		private TimeSpan start;

		public DymamicPlayerController(PlayerElement player)
		{
			this.Player = player;
			this.ActionsHistory = new List<PlayerAction>();
			this.ActionDuration = 300;
		}

		public DymamicPlayerController Clone(GameTime gameTime)
		{
			var player = this.Player.Clone(gameTime);
			DymamicPlayerController controller = new DymamicPlayerController(player);
			controller.ActionsHistory = this.ActionsHistory;
			return controller;
		}

		public void Start(GameTime gameTime)
		{
			start = gameTime.TotalGameTime;
			lastActionIndex = -1;
		}

		public void Update(GameTime gameTime)
		{
			int index = (int)((gameTime.TotalGameTime - start).TotalMilliseconds / this.ActionDuration);
			if (index < 0)
			{
				Player.Die();
				return;
			}

			if (lastActionIndex < index)
			{
				PlayerAction action = GetRandomAction();

				switch (action)
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
					case PlayerAction.Stay:
						Player.Stop();
						break;
				}

				ActionsHistory.Add(action);
				lastActionIndex = index;
			}
		}
	}
}
