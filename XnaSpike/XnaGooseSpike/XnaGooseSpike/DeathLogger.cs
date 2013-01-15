using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XnaGooseGame
{
	class DeathLogger
	{
		private static object syncObj = new object();
		public static int deathCount = 0;

		public static void IncreaseDeathCount()
		{
			lock (syncObj)
			{
				deathCount++;
			}
		}

		SpriteFont segoeFont;

		public DeathLogger(ContentManager content)
		{
			segoeFont = content.Load<SpriteFont>("segoe");
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(segoeFont, "deaths: " + deathCount.ToString(), new Vector2(10, 20), Color.White);

		}
	}
}
