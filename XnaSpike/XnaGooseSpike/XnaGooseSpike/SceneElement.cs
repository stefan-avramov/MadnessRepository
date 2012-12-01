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
        public Vector2 Location { get; set; }

        public virtual void Update(GameTime time)
        {

        }

        public abstract void DrawFrame(SpriteBatch batch, Vector2 screenPos);
    }
}
