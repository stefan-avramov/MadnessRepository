using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace XnaGooseGame
{
    abstract class SceneElement
    {
        private Vector2 location;

        public virtual Vector2 Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;  
            }
        }

        public virtual void Update(GameTime time)
        {

        }

        public abstract void DrawFrame(SpriteBatch batch, Vector2 screenPos);

		public abstract void LoadContent(ContentManager manager);
    }
}
