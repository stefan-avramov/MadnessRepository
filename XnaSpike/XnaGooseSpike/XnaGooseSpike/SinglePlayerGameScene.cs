using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace XnaGooseGame
{
    class SinglePlayerGameScene : GameScene
    {
        PlayerElement player;

        public SinglePlayerGameScene()
        {
            player = new PlayerElement();
            player.Location = new Vector2(3900, 10);
            this.Offset = new Vector2(-3800, 0);
            this.Elements.Add(player);
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            base.LoadContent(Content);
            this.player.LoadContent(Content);
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                this.Offset = new Vector2(this.Offset.X + 20, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                this.Offset = new Vector2(this.Offset.X - 20, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                player.MoveBackward();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                player.MoveForward();
            }
            else
            {
                player.Stop();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                player.Jump();
            }

            base.Update(gameTime);
        }
    }
}