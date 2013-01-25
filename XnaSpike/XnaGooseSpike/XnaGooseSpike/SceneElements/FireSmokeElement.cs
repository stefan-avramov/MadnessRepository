using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XnaGooseGame
{
	class FireSmokeElement : SceneElement, IInteractiveObject, ITimeDependentInteractionElement
	{
		Texture2D texture;
		int frame;

		private const int WIDTH = 75;
		private const int HEIGHT = 204;
		private int visibleInterval = 1500;
		private bool visible = true;

		public FireSmokeElement(float x, float y)
			: this(new Vector2(x, y))
		{
		}

		public FireSmokeElement(Vector2 location)
		{
			this.Location = location;
		}

		public override void Update(Microsoft.Xna.Framework.GameTime time)
		{
			base.Update(time);
			frame = (int)((time.TotalGameTime.TotalMilliseconds / 40) % 11);
			visible = (time.TotalGameTime.TotalMilliseconds % (2 * visibleInterval) < visibleInterval);
		}

		public override void DrawFrame(Microsoft.Xna.Framework.Graphics.SpriteBatch batch, Microsoft.Xna.Framework.Vector2 screenPos)
		{
			if (this.visible)
			{
				Rectangle sourcerect = new Rectangle((int)(frame * 75.5), 0, WIDTH, HEIGHT);
				batch.Draw(this.texture, screenPos, sourcerect, Color.White, 0f, new Vector2(), 1.0f, SpriteEffects.None, 0.5f);
			}
		}

		public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager manager)
		{
			texture = manager.Load<Texture2D>("scene/firesmoke");
		}

		public void Interact(PlayerElement player)
		{
			player.Die();
		}


		public bool CanInteract(PlayerElement player)
		{
			if (!this.visible)
			{
				return false;
			}

			Rectangle playerBounds = new Rectangle((int)player.Location.X, (int)player.Location.Y, PlayerElement.PLAYER_WIDTH, PlayerElement.PLAYER_HEIGHT);
			Rectangle bounds = new Rectangle((int)this.Location.X + WIDTH / 4, (int)this.Location.Y + WIDTH / 4, WIDTH / 2, HEIGHT - WIDTH / 2);
			return bounds.Intersects(playerBounds);
		}
	}
}
