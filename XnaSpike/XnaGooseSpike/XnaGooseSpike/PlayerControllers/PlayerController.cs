using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaGooseGame
{
	abstract class PlayerController
	{
		public PlayerElement Player { get; protected set; }

		public double ActionDuration { get; set; }

		protected PlayerAction GetRandomAction()
		{
			int next = RandomGenerator.Next(0, 1000);
			return next < 50 ? PlayerAction.MoveBackward : next < 500 ? PlayerAction.Jump : next < 800 ? PlayerAction.MoveForward : PlayerAction.Stay;
		}
	}
}
