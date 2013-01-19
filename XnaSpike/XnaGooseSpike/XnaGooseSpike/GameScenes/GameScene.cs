using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;

namespace XnaGooseGame
{
	public class GameScene
	{
		List<SceneElement> elements = new List<SceneElement>();
		public Vector2 Offset { get; set; }
		private bool musicPlaying = false;

		public List<SceneElement> Elements
		{
			get
			{
				return elements;
			}
		}

		public GameScene()
		{
			this.InitializeElements();
		}

		protected virtual void InitializeElements()
		{
			foreach (IInteractiveObject interactive in GameLevelManager.CurrentLevel.InteractionObjects)
			{
				if (interactive is SceneElement)
				{
					this.Elements.Add((SceneElement)interactive);
				}
			}
		}

		public virtual void Update(GameTime gameTime) 
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Left))
			{
				this.Offset = new Vector2(this.Offset.X + 20, 0);
			}

			if (Keyboard.GetState().IsKeyDown(Keys.Right))
			{
				this.Offset = new Vector2(this.Offset.X - 20, 0);
			}

			if (!this.musicPlaying && GameLevelManager.CurrentLevel.MusicTheme != null)
			{
				MediaPlayer.IsRepeating = true;
				MediaPlayer.Volume = 0.1f;
				MediaPlayer.Play(GameLevelManager.CurrentLevel.MusicTheme);
				this.musicPlaying = true;
			}

			Parallel.ForEach(this.Elements, x => x.Update(gameTime));
			Parallel.ForEach(this.GetPlayers(), x => HandleInteraction(x));
		}

		private void HandleInteraction(PlayerElement player)
		{
			foreach (IInteractiveObject obj in GameLevelManager.CurrentLevel.InteractionObjects)
			{
				if (obj.CanInteract(player))
				{
					obj.Interact(player);
				}
			}
		}

		public void Draw(SpriteBatch batch)
		{
			this.OwnDraw(batch);
			this.DrawElements(batch);
		}

		protected virtual void DrawElements(SpriteBatch batch)
		{
			foreach (SceneElement element in this.Elements)
			{
				element.DrawFrame(batch, this.Offset + element.Location);
			}
		}

		protected virtual void OwnDraw(SpriteBatch batch)
		{
			GameLevelManager.CurrentLevel.Draw(batch, Offset);
		}

		public virtual void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
		{
			foreach (SceneElement element in this.Elements)
			{
				element.LoadContent(Content);
			}
		}

		protected virtual IEnumerable<PlayerElement> GetPlayers()
		{
			return Enumerable.Empty<PlayerElement>();
		}
	}
}
