using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XnaGooseGame
{
	class BatmanElement : SceneElement, IInteractiveObject, ITimeDependentInteractionElement
	{
		Texture2D texture;
		int offset = 0;
		int frame = 0;
		int maxOffset;
		bool dir = false;

		private const int FRAMES_COUNT = 12;
		private const int SPRITE_WIDTH = 100;
		private const int SPRITE_HEIGHT = 131;

		public BatmanElement(float x, float y, int maxOffset = 700)
		{
			this.Location = new Vector2(x, y);
			this.maxOffset = maxOffset;
		}

		public override void Update(Microsoft.Xna.Framework.GameTime time)
		{
			base.Update(time);
			
			frame = ((int)time.TotalGameTime.TotalMilliseconds / 50) % FRAMES_COUNT;
			int path = (int)((time.TotalGameTime.TotalMilliseconds / 4) % (2*maxOffset));
			offset = path < maxOffset ? path : 2 * maxOffset - path;
		}

		public override void DrawFrame(Microsoft.Xna.Framework.Graphics.SpriteBatch batch, Microsoft.Xna.Framework.Vector2 screenPos)
		{
			screenPos.X += offset;
			Rectangle sourcerect = new Rectangle(frame*SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT);
			batch.Draw(this.texture, screenPos, sourcerect, Color.White);
		}

		public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager manager)
		{
			texture = manager.Load<Texture2D>("scene/batman");
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
