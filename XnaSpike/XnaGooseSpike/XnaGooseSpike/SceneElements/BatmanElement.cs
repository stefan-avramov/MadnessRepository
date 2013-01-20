using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XnaGooseGame
{
	class BatmanElement : SceneElement, IInteractiveObject
	{
		Texture2D texture;
		int offset = 0;
		int frame = 0;
		int maxOffset = 700;
		bool dir = false;

		private const int FRAMES_COUNT = 12;
		private const int SPRITE_WIDTH = 100;
		private const int SPRITE_HEIGHT = 131;

		public BatmanElement(float x, float y)
			: this(new Vector2(x, y))
		{
		}

		public BatmanElement(Vector2 location)
		{
			this.Location = location;
		}

		public override void Update(Microsoft.Xna.Framework.GameTime time)
		{
			base.Update(time);

			frame = ((int)time.TotalGameTime.TotalMilliseconds / 50) % FRAMES_COUNT;
			offset += 3*(dir?1:-1);
			if (dir && offset > maxOffset)
			{
				offset -= maxOffset - offset;
				dir = !dir;
			}
			if (!dir && offset < 0)
			{
				offset = -offset;
				dir = !dir;
			}
		}

		public override void DrawFrame(Microsoft.Xna.Framework.Graphics.SpriteBatch batch, Microsoft.Xna.Framework.Vector2 screenPos)
		{
			screenPos.X += offset;
			Rectangle sourcerect = new Rectangle(frame*SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT);
			batch.Draw(this.texture, screenPos, sourcerect, Color.White);
		}

		public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager manager)
		{
			texture = manager.Load<Texture2D>("batman");
		}

		public void Interact(PlayerElement player)
		{
			player.Die();
		}

		public bool CanInteract(PlayerElement player)
		{
			Rectangle playerBounds = new Rectangle((int)player.Location.X, (int)player.Location.Y, PlayerElement.PLAYER_WIDTH, PlayerElement.PLAYER_HEIGHT);
			Rectangle batmanBounds = new Rectangle((int)this.Location.X + offset, (int)this.Location.Y, SPRITE_WIDTH, SPRITE_HEIGHT);
			return batmanBounds.Intersects(playerBounds);
		}
	}
}
