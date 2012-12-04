using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XnaGooseSpike
{
    class PlayerElement : SceneElement
    {
		private int framesCount;
		private int framesPerSec;

		private const int PLAYER_HEIGHT = 42;
		private const int PLAYER_WIDTH = 57;

        bool isForward = true;
        bool isMoving = false;

        public Texture2D Texture { get; set; }
        public Texture2D MapTexture { get; set; }

        int frame = 0;
        double totalTime = 0;
        private bool isJumping;
		private const float jumpPower = 1.8f;
		private float jumpSpeed = jumpPower;
		private float jumpAccelaration = 0.07f;

		public void LoadContent(ContentManager content)
		{
			this.framesCount = 2;
			this.framesPerSec = 12;

			this.Texture = content.Load<Texture2D>("goose");
			this.MapTexture = content.Load<Texture2D>("map");
		}

		public override Vector2 Location
		{
            get
            {
                return base.Location;
            }
            set
            {
                if (IsValidLocation(value))
                {
                    base.Location = value; 
                }
            }
        }

		private bool IsValidLocation(Vector2 value)
		{
			if (value.X < 0 || value.Y < 0) return false;
			try
			{
				Color[] myColors = GetMapIntersectionRectangle(value);
				return !myColors.Contains(Color.Black);
			}
			catch
			{
			}

			return false;
		}

		private Color[] GetMapIntersectionRectangle(Vector2 location)
		{
			var result = new Color[PLAYER_WIDTH * PLAYER_HEIGHT];
			MapTexture.GetData<Color>(0, new Rectangle((int)location.X, (int)location.Y, PLAYER_WIDTH, PLAYER_HEIGHT),
				result, 0, PLAYER_WIDTH * PLAYER_HEIGHT);
			return result;
		}

		public override void Update(Microsoft.Xna.Framework.GameTime time)
		{
			base.Update(time);

			if (this.isMoving)
			{
				if (this.isForward)
				{
					this.Location = new Vector2(this.Location.X + (float)time.ElapsedGameTime.TotalSeconds * 250f, this.Location.Y);
				}
				else
				{
					this.Location = new Vector2(this.Location.X - (float)time.ElapsedGameTime.TotalSeconds * 250f, this.Location.Y);
				}

				totalTime += time.ElapsedGameTime.TotalSeconds;

				if (totalTime > 1.0f / this.framesPerSec)
				{
					totalTime = 0;
					frame++;
					frame %= this.framesCount;
				}
			}
			
			// epeck block
			{
				Vector2 newLocation = new Vector2(this.Location.X, this.Location.Y + (float)time.ElapsedGameTime.TotalSeconds * 600f * jumpSpeed);
				int intersectLevel = GroundLevelIntersection(newLocation);
				if (intersectLevel > 0)
				{
					newLocation = new Vector2(newLocation.X, newLocation.Y - intersectLevel);
					this.isJumping = false;
				}
				this.Location = newLocation;
			}

			if (this.isJumping)
			{
				this.jumpSpeed += jumpAccelaration;
				int intersectLevel = GroundLevelIntersection(Location);
				if (intersectLevel > 0)
				{
					this.isJumping = false;
					this.Location = new Vector2(Location.X - intersectLevel, Location.Y);
				}
			}
		}

		private int GroundLevelIntersection(Vector2 location)
		{
			try
			{
				Color[,] rect = ConvertArrayToRectangle(GetMapIntersectionRectangle(location));
				for (int i = PLAYER_HEIGHT - 1; i >= 0; i--)
				{
					bool hasBlack = false;
					for (int j = 0; j < PLAYER_WIDTH; j++)
					{
						if (rect[i, j] == Color.Black)
						{
							hasBlack = true;
						}
					}
					if (!hasBlack)
					{
						return PLAYER_HEIGHT - 1 - i;
					}
				}
				return PLAYER_HEIGHT;
			}
			catch { }

			return 0;
		}

		Color[,] ConvertArrayToRectangle(Color[] color)
		{
			var answer = new Color[PLAYER_HEIGHT, PLAYER_WIDTH];
			for (int i = 0; i < color.Length; i++)
			{
				answer[i / PLAYER_WIDTH, i % PLAYER_WIDTH] = color[i];
			}
			return answer;
		}

		public void MoveForward()
        {
            this.isMoving = true;
            this.isForward = true;
            if (this.isMoving) return;
            this.totalTime = 0;
        }

        public void MoveBackward()
        {
            this.isMoving = true;
            this.isForward = false;
            if (this.isMoving) return;
            this.totalTime = 0;
        }

        public void Stop()
        {
            this.isMoving = false;
        }

        public void Jump()
        {
            if (!this.IsOnGround() || this.isJumping)
            {
                return;
            }

            this.isJumping = true;
            this.jumpSpeed = -jumpPower;
        }

        private bool IsOnGround()
        {
            return !this.IsValidLocation(new Vector2(this.Location.X, this.Location.Y + 1));
        }

		public override void DrawFrame(Microsoft.Xna.Framework.Graphics.SpriteBatch batch, Microsoft.Xna.Framework.Vector2 screenPos)
		{
			Rectangle sourcerect = new Rectangle(frame * PLAYER_WIDTH, 0, PLAYER_WIDTH, PLAYER_HEIGHT);
			batch.Draw(this.Texture, screenPos, sourcerect, Color.White, 0f, new Vector2(), 1f, 
				isForward ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0.5f);
		}
    }
}
