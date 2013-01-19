using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace XnaGooseGame
{
	class SinglePlayerGameScene : GameScene
	{
		PlayerElement player;

		protected override void InitializeElements()
		{
			base.InitializeElements();

			player = new PlayerElement();
			player.Location = new Vector2(3900, 10);
			this.Offset = new Vector2(-3800, 0);
			this.Elements.Add(player);
		}

		public override void Update(GameTime gameTime)
		{ 
			base.Update(gameTime);
			this.Offset = new Vector2(-this.player.Location.X + Game1.VIEWPORT_WIDTH / 2 - PlayerElement.PLAYER_WIDTH / 2, 0);
		}

		protected override void HandleInput(GameTime gameTime) 
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
			{
				player.MoveBackward();
			}
			else if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
			{
				player.MoveForward();
			}
			else
			{
				player.Stop();
			}

			if (Keyboard.GetState().IsKeyDown(Keys.Space))
			{
				player.Jump();
			}
		}

		protected override System.Collections.Generic.IEnumerable<PlayerElement> GetPlayers()
		{
			yield return this.player;
			yield break;
		}
	}
}