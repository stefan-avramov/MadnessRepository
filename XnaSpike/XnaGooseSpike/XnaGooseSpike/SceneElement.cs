using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGooseSpike
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
    }
}
