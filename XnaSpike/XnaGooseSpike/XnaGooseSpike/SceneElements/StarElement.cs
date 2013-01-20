using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGooseGame
{
	class StarElement : SceneElement
	{
		Texture2D texture;
		private const int FRAME_COUNT = 11;
		private const int SPRITE_WIDTH = 28;
		private const int SPRITE_HEIGHT = 28;
 		private const int COLUMNS = 4;
		private const int ROWS = 3;
		private const double VISIBLE_DURATION = 2000;
		private int frame = 0;
		private bool isVisible = false;

		public StarElement Clone()
		{
			var starElement = new StarElement();
			starElement.texture = texture;
			return starElement;
		}

		public void Update(double lastShowMilliseconds, double elapsedMilliseconds)
		{
			this.isVisible = (elapsedMilliseconds - lastShowMilliseconds) <= VISIBLE_DURATION;
			if (isVisible)
			{
				frame = (((int)elapsedMilliseconds) / 20) % FRAME_COUNT;
			}
		}

		public override void DrawFrame(Microsoft.Xna.Framework.Graphics.SpriteBatch batch, Vector2 location)
		{
			if (this.isVisible)
			{
				location = new Vector2(location.X - 20, location.Y - 20); 
				int row = frame / COLUMNS;
				int col = frame % COLUMNS;
				batch.Draw(this.texture, location, new Rectangle(col * SPRITE_HEIGHT, row * SPRITE_WIDTH, SPRITE_WIDTH, SPRITE_HEIGHT), Color.White);
			}
		}

		public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager manager)
		{
			texture = manager.Load<Texture2D>("spinning_star");
		}
	}
}
