﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;

namespace XnaGooseGame
{
	class GameScene
	{
		public Vector2 Offset 
		{ 
			get { return offset; } 
			set 
			{
				value.X = Math.Max(-GameLevelManager.CurrentLevel.TotalWidth + Game1.VIEWPORT_WIDTH, Math.Min(value.X, 0)); 
				this.offset = value; 
			} 
		}

		private Vector2 offset;
		private bool musicPlaying = false;
		private Point mouseAnchor;
		private List<SceneElement> elements = new List<SceneElement>();

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

			if (Game1.Instance.IsActive)
			{
				this.HandleInput(gameTime);
			}

			if (Game1.MUSIC_ENABLED && !this.musicPlaying && GameLevelManager.CurrentLevel.MusicTheme != null)
			{
				MediaPlayer.IsRepeating = true;
				MediaPlayer.Volume = 0.1f;
				MediaPlayer.Play(GameLevelManager.CurrentLevel.MusicTheme);
				this.musicPlaying = true;
			}

			Parallel.ForEach(this.Elements, x => x.Update(gameTime));
			Parallel.ForEach(this.GetPlayers(), x => HandleInteraction(x));
		}

		protected virtual void HandleInput(GameTime gameTime)
		{ 
			if (Keyboard.GetState().IsKeyDown(Keys.Left))
			{
				this.Offset = new Vector2(this.Offset.X + 20, 0);
			}

			if (Keyboard.GetState().IsKeyDown(Keys.Right))
			{
				this.Offset = new Vector2(this.Offset.X - 20, 0);
			}

			if (Mouse.GetState().LeftButton == ButtonState.Pressed)
			{
				Vector2 offset = new Vector2(Mouse.GetState().X - mouseAnchor.X, Mouse.GetState().Y - mouseAnchor.Y);
				this.Offset = new Vector2(this.Offset.X + (-offset.X * (float)gameTime.ElapsedGameTime.TotalSeconds * 10), 0);

			}
			else
			{
				mouseAnchor = new Point(Mouse.GetState().X, Mouse.GetState().Y);
			}
		} 

		protected void HandleInteraction(PlayerElement player)
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

		public virtual void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
		{
			foreach (SceneElement element in this.Elements)
			{
				element.LoadContent(content);
			}
		}

		protected virtual IEnumerable<PlayerElement> GetPlayers()
		{
			return Enumerable.Empty<PlayerElement>();
		}
	}
}
