﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGooseSpike
{
    class GameScene
    {
        List<SceneElement> elements = new List<SceneElement>();
        GraphicsDevice device;
        Texture2D backgroundTexture;

        public GameScene(GraphicsDevice device, Texture2D backgroundTexture)
        {
            this.device = device;
            this.backgroundTexture = backgroundTexture;
        }

        public List<SceneElement> Elements
        {
            get
            {
                return elements;
            }
        }

        public Vector2 Offset { get; set; }

        public void Update(GameTime gameTime) 
        {
            foreach (SceneElement element in this.Elements)
            {
                element.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            this.OwnDraw(batch);
            this.DrawElements(batch);
        }

        private void DrawElements(SpriteBatch batch)
        {
            foreach (SceneElement element in this.Elements)
            {
                element.DrawFrame(batch, this.Offset + element.Location);
            }
        }

        private void OwnDraw(SpriteBatch batch)
        {
            batch.Draw(backgroundTexture, Offset, null,
                    Color.White, 0, new Vector2(0,0), 1, SpriteEffects.None, 0f);
        }
    }
}
