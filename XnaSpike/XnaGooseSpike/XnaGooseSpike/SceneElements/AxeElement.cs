using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XnaGooseGame
{
	class AxeElement : SceneElement, IInteractiveObject
	{
		Texture2D texture;
		double angle;


		public AxeElement(float x, float y)
			: this(new Vector2(x, y))
		{
		}

		public AxeElement(Vector2 location)
		{
			this.Location = location;
		}

		public override void Update(Microsoft.Xna.Framework.GameTime time)
		{
			base.Update(time);
			angle = (time.TotalGameTime.TotalMilliseconds / 120) % 6.28;
		}

		public override void DrawFrame(Microsoft.Xna.Framework.Graphics.SpriteBatch batch, Microsoft.Xna.Framework.Vector2 screenPos)
		{
			Rectangle sourcerect = new Rectangle(0, 0, texture.Width, texture.Height);
			batch.Draw(this.texture, screenPos, sourcerect, Color.White, (float)angle, new Vector2(texture.Width / 2, texture.Height / 2), 1.0f, SpriteEffects.None, 0.5f);
		}

		public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager manager)
		{
			texture = manager.Load<Texture2D>("axe");
		}

		public void Interact(PlayerElement player)
		{
			player.Die();
		}


		public bool CanInteract(PlayerElement player)
		{
			Rectangle playerBounds = new Rectangle((int)player.Location.X, (int)player.Location.Y, PlayerElement.PLAYER_WIDTH, PlayerElement.PLAYER_HEIGHT);
			double radius = Math.Max(texture.Width / 2.0d, texture.Height / 2.0d);
			Point center = new Point((int)this.Location.X + this.texture.Width/2, (int)this.Location.Y + this.texture.Height / 2);
			Point playerCenter = new Point(playerBounds.X + playerBounds.Width / 2, playerBounds.Y + playerBounds.Height / 2);
			List<Point> points = new List<Point>()
			{
				//TODO: if we fix the bounding box of the goose, uncomment these
				//new Point(playerBounds.Left, playerBounds.Top),
				//new Point(playerBounds.Right, playerBounds.Top),
				//new Point(playerBounds.Right, playerBounds.Bottom),
				//new Point(playerBounds.Left, playerBounds.Bottom),
				
			    new Point(playerBounds.Left + playerBounds.Width / 2, playerBounds.Top),
			    new Point(playerBounds.Right, playerBounds.Top  + playerBounds.Height /2),
			    new Point(playerBounds.Right, playerBounds.Bottom  + playerBounds.Height /2),
			    new Point(playerBounds.Left + playerBounds.Width / 2, playerBounds.Bottom),

				playerCenter
			};
			
			foreach(Point p in points)
			{
				if(Dist(p, center) <= radius * radius)
				{
					return true;
				}
			}

			return false;
		}

		private static double Dist(Point p1, Point p2)
		{
			return (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
		}
	}
}
