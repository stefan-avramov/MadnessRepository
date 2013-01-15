using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGooseGame
{
	class CoinElement : SceneElement
	{
		private const int COIN_HEIGHT = 32;
		private const int COIN_WIDTH = 24;

		private Texture2D texture;

		public int Height
		{
			get { return COIN_HEIGHT; }
		}

		public int Width
		{
			get { return COIN_WIDTH; }
		}

		public CoinElement(float x, float y) 
			: this(new Vector2(x, y))
		{
		}

		public CoinElement(Vector2 location)
		{
			this.Location = location;
		}

		public override void DrawFrame(Microsoft.Xna.Framework.Graphics.SpriteBatch batch, Microsoft.Xna.Framework.Vector2 screenPos)
		{
			Rectangle sourcerect = new Rectangle(0, 0, COIN_WIDTH, COIN_HEIGHT);
			batch.Draw(this.texture, screenPos, sourcerect, Color.White, 0f, new Vector2(), 1.0f, SpriteEffects.None, 0.5f);
		}

		public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager manager)
		{
			this.texture = manager.Load<Texture2D>("coin");
		}
	}
}
