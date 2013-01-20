using System;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XnaGooseGame
{
	class PopulatonLogger
	{
		private static object syncObj = new object();
		private static int deathCount = 0;
		private static int lifeCount = 0;

		private static int? generationNumber;

		private static int winCount = 0;

		public static int Lives
		{
			get
			{
				return lifeCount;
			}
		}

		public static int Wins
		{
			get
			{
				return winCount;
			}
		}

		public static void LogPlayerDeath()
		{
			lock (syncObj)
			{
				deathCount++;
				lifeCount--;
			}
		}

		public static void LogPlayerWin()
		{
			lock (syncObj)
			{
				winCount++;
				lifeCount--;
			}
		}

		public static void LogPlayerBirth()
		{
			lock (syncObj)
			{
				lifeCount++;
			}
		}

		public static void SetGeneration(int generation)
		{
			generationNumber = generation;
		}

		SpriteFont segoeFont;

		public PopulatonLogger(ContentManager content)
		{
			segoeFont = content.Load<SpriteFont>("segoe");
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			string printString = "deaths: " + deathCount.ToString() + "\nlives: " + lifeCount + "\nwins: " + winCount;
			if (generationNumber.HasValue)
			{
				printString += "\ngeneration:" + generationNumber;
			}

			spriteBatch.DrawString(segoeFont, printString, new Vector2(10, 30), Color.White); 
		}
	}
}
