﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace XnaGooseGame
{
	class PlayerController
	{
		public PlayerElement Player { get; private set; }

		public List<PlayerAction> ActionsHistory { get; private set; }

		public double ActionDuration { get; set; }

		private int lastActionIndex = -1;
		private TimeSpan start;

		public PlayerController(PlayerElement player)
		{
			this.Player = player;
			this.ActionsHistory = new List<PlayerAction>();
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
			int index = (int)((gameTime.TotalGameTime - start).TotalMilliseconds / this.ActionDuration);
			if (index < 0)
			{
				Player.Die();
				return;
			}

			if (lastActionIndex < index)
			{
				PlayerAction action = GetNextAction();

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
				}

				ActionsHistory.Add(action);
				lastActionIndex = index;
			}
		}

		private PlayerAction GetNextAction()
		{
			int next = RandomGenerator.Next(0, 1000);
			return next < 50 ? PlayerAction.MoveBackward : next < 700 ? PlayerAction.Jump : PlayerAction.MoveForward;
		}
	}
}