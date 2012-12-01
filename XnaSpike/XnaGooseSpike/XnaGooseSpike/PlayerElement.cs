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
        public int FramesCount { get; set; }
        public int FramesPerSec { get; set; }
        int frame = 0;
        double totalTime = 0;

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            
            if (this.isMoving)
            {
                if (this.isForward)
                {
                    this.Location = new Vector2(this.Location.X + (float)time.ElapsedGameTime.TotalSeconds * 150f, this.Location.Y);
                }
                else
                {
                    this.Location = new Vector2(this.Location.X - (float)time.ElapsedGameTime.TotalSeconds * 150f, this.Location.Y);
                }

                totalTime += time.ElapsedGameTime.TotalSeconds;


                if (totalTime > 1.0f / FramesPerSec)
                {
                    totalTime = 0;
                    frame++;
                    frame %= FramesCount;
                }
            } 
        }

        public void MoveForward()
        {
            if (this.isMoving) return;

            this.isMoving = true;
            this.isForward = true;
            this.totalTime = 0;
        }

        public void MoveBackward()
        {
            if (this.isMoving) return;

            this.isMoving = true;
            this.isForward = false;
            this.totalTime = 0;
        }

        public void Stop()
        {
            this.isMoving = false;
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
