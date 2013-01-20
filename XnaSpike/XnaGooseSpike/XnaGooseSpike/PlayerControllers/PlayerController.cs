using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace XnaGooseGame
{
	class PlayerStatus
	{
		public Vector2 Location { get; set; }
		public bool IsAlive { get; set; }
		public bool HasWon { get; set; }
		public int CollectedValue { get; set; }
		public int Step { get; set; }

	}

	class PlayerController
	{
		public PlayerElement Player { get; private set; }

		public List<PlayerAction> ActionsHistory { get; private set; }
		public List<PlayerStatus> StatusHistory { get; private set; }
		public List<PlayerAction> PredefinedActions { get; private set; }

		public double ActionDuration { get; set; }
		public bool UsePredefinedActions { get; set; }

		private int lastActionIndex = -1;
		private TimeSpan start;

		public PlayerController(PlayerElement player)
		{
			this.Player = player;
			this.ActionsHistory = new List<PlayerAction>();
			this.PredefinedActions = new List<PlayerAction>();
			this.StatusHistory = new List<PlayerStatus>();
			this.ActionDuration = 300;
		}

		public PlayerController Clone()
		{
			var player = this.Player.Clone();
			PlayerController controller = new PlayerController(player);
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
			if (Player.IsDead || Player.HasWon)
			{
				return;
			}

			int index = (int)((gameTime.TotalGameTime - start).TotalMilliseconds / this.ActionDuration);
			if (index < 0)
			{
				Player.Die();
				return;
			}
			
			if (lastActionIndex < index)
			{
				PlayerAction action = GetNextAction(index);

				switch (action)
				{
					case PlayerAction.Die:
						Player.Die();
						break;
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
				StatusHistory.Add(new PlayerStatus() { Location = Player.Location, IsAlive = !Player.IsDead, CollectedValue = Player.CollectedValue, Step = index });
				lastActionIndex = index;
			}
		}

		private PlayerAction GetNextAction(int index)
		{
			if (!this.UsePredefinedActions)
			{
				int next = RandomGenerator.Next(0, 1000);
				return next < 50 ? PlayerAction.MoveBackward : next < 500 ? PlayerAction.Jump : next < 800 ? PlayerAction.MoveForward : PlayerAction.Stay;
			}
			else
			{
				if (index < 0 || index >= this.PredefinedActions.Count)
				{
					return PlayerAction.Die;
				}

				return this.PredefinedActions[index];
			}
		}
	}
}
