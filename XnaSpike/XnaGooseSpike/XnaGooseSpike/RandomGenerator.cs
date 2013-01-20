using System;

namespace XnaGooseGame
{
	static class RandomGenerator
	{
		static Random rand = new Random((int)DateTime.Now.TimeOfDay.TotalMilliseconds);
		
		public static int Next(int left, int right)
		{
			return rand.Next(left, right);
		}

		public static double NextDouble()
		{
			return rand.NextDouble();
		}

		public static PlayerAction NextMove()
		{
			int next = RandomGenerator.Next(0, 1000);
			PlayerAction action = 
				next < 250 ?
				PlayerAction.MoveBackward :
				next < 500 ?
				PlayerAction.Jump :
				next < 750 ?
				PlayerAction.MoveForward :

				PlayerAction.Stay;
		
			return action;
		}
	}
}
