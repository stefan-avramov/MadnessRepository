using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGooseGame
{
	class CoinElement : SceneElement, IInteractiveObject
	{
		public const int COIN_HEIGHT = 32;
		public const int COIN_WIDTH = 24;
		private int value = 1;
		private Texture2D texture;
		private int frame = 0;

		public int Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

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

		public override void Update(GameTime time)
		{
			base.Update(time);
			frame = time.TotalGameTime.TotalMilliseconds % 500 < 250 ? 0 : 1;
		}

		public override void DrawFrame(Microsoft.Xna.Framework.Graphics.SpriteBatch batch, Microsoft.Xna.Framework.Vector2 screenPos)
		{
			Rectangle sourcerect = new Rectangle(frame* COIN_WIDTH, 0, COIN_WIDTH, COIN_HEIGHT);
			batch.Draw(this.texture, screenPos, sourcerect, Color.White, 0f, new Vector2(), 1.0f, SpriteEffects.None, 0.5f);
		}

		public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager manager)
		{
			this.texture = manager.Load<Texture2D>("scene/coin");
		}

		public void Interact(PlayerElement player)
		{
			player.CollectCoin(this);
		}


		public bool CanInteract(PlayerElement player)
		{
			Rectangle playerBounds = new Rectangle((int)player.Location.X, (int)player.Location.Y, PlayerElement.PLAYER_WIDTH, PlayerElement.PLAYER_HEIGHT);
			Rectangle bounds = new Rectangle((int)this.Location.X, (int)this.Location.Y, COIN_WIDTH , COIN_HEIGHT);
			return bounds.Intersects(playerBounds);
		}
	}
}
