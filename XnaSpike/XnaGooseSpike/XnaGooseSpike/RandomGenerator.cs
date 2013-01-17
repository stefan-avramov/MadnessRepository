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
	}
}
