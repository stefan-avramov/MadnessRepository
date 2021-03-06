using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGooseGame
{
	class FpsLogger
	{
		private int previousSecondFramesCount = -1;
		private int currentSecondFramesCount = 0;
		private int currentElapsedMilliseconds = 0;

		readonly SpriteFont segoeFont;

		public FpsLogger(ContentManager content)
		{
			segoeFont = content.Load<SpriteFont>("segoe");
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (currentElapsedMilliseconds >= 1000)
			{
				previousSecondFramesCount = currentSecondFramesCount;
				currentSecondFramesCount = 0;
				currentElapsedMilliseconds -= 1000;
			}

			spriteBatch.DrawString(segoeFont, "fps: " + previousSecondFramesCount.ToString(), new Vector2(10, 5), Color.White);

			currentSecondFramesCount++;
			currentElapsedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
		}
	}
}
