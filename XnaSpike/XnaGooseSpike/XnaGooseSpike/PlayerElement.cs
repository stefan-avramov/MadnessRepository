using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XnaGooseSpike
{
    class PlayerElement : SceneElement
    {
        bool isForward = true;
        bool isMoving = false;

        public Texture2D ForwardTexture { get; set; }
        public Texture2D BackwardTexture { get; set; }
        public Texture2D MapTexture { get; set; }
        public Rectangle BoundingBox { get; set; }
        public int FramesCount { get; set; }
        public int FramesPerSec { get; set; }

        int frame = 0;
        double totalTime = 0;
        private bool isJumping;
        private float jumpAcceleration = 1f;
         
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
                Color[] myColors = new Color[this.BoundingBox.Width * this.BoundingBox.Height];
                MapTexture.GetData<Color>(0, new Rectangle((int)value.X + this.BoundingBox.X, (int)value.Y + this.BoundingBox.Y, this.BoundingBox.Width, this.BoundingBox.Height), myColors, 0, this.BoundingBox.Width * this.BoundingBox.Height);
                return !myColors.Contains(Color.Black);
            }
            catch
            {
                
            }

            return false;
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

                if (totalTime > 1.0f / FramesPerSec)
                {
                    totalTime = 0;
                    frame++;
                    frame %= FramesCount;
                }
            }

            this.Location = new Vector2(this.Location.X, this.Location.Y + (float)time.ElapsedGameTime.TotalSeconds * 600f * jumpAcceleration);

            if (this.isJumping)
            {
                this.jumpAcceleration = Math.Min(1.0f, this.jumpAcceleration + 0.02f);
                if (this.IsOnGround())
                {
                    this.isJumping = false;
                }
            }
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
            this.jumpAcceleration = -1f;
        }

        private bool IsOnGround()
        {
            return this.jumpAcceleration > 0 && !this.IsValidLocation(new Vector2(this.Location.X, this.Location.Y + 10f));
        }

        public override void DrawFrame(Microsoft.Xna.Framework.Graphics.SpriteBatch batch, Microsoft.Xna.Framework.Vector2 screenPos)
        {
            int fameWidth = 155;
            Texture2D myTexture = isForward ? this.ForwardTexture : this.BackwardTexture;

            Rectangle sourcerect = new Rectangle(fameWidth * (this.isMoving ? frame : (this.isForward ? 0 : 3)), 0, fameWidth, myTexture.Height);

            batch.Draw(myTexture, screenPos, sourcerect, Color.White,
               0f, new Vector2(), 1f, SpriteEffects.None, 0.5f);
        }
    }
}
