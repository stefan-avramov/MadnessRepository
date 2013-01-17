using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;

namespace XnaGooseGame
{
    class GameScene
    {
        List<SceneElement> elements = new List<SceneElement>();

        public GameScene()
        {

        }

        public List<SceneElement> Elements
        {
            get
            {
                return elements;
            }
        }

        public Vector2 Offset { get; set; }

        public virtual void Update(GameTime gameTime) 
        {
            //foreach (SceneElement element in this.Elements)
            //{
            //    element.Update(gameTime);
            //}
            if (!this.musicPlaying && GameLevelManager.CurrentLevel.MusicTheme != null)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = 0.1f;
                MediaPlayer.Play(GameLevelManager.CurrentLevel.MusicTheme);
                this.musicPlaying = true;
            }

            Parallel.ForEach(this.Elements, x => x.Update(gameTime));
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
            //batch.Draw(backgroundTexture, Offset, null,
            //        Color.White, 0, new Vector2(0,0), 1, SpriteEffects.None, 0f);
            GameLevelManager.CurrentLevel.Draw(batch, Offset);
        }

        public virtual void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            
        }

        public bool musicPlaying = false;
    }
}
