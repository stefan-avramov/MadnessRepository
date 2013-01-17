using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System.Runtime.InteropServices;

namespace XnaGooseGame
{
    class MapSegment
    {
        public Texture2D DisplayTexture { get; set; }
        public Texture2D MapTexture { get; set; }
    }

    public class GameLevel
    {
        List<MapSegment> segments;
        public Song MusicTheme { get; private set; }

        public GameLevel(IEnumerable<Texture2D> levelTextures, IEnumerable<Texture2D> mapTextures, Song song = null)
        {
            this.MusicTheme = song;
            segments = new List<MapSegment>();
            
            foreach (Texture2D texture in levelTextures)
            {
                segments.Add(new MapSegment() { DisplayTexture = texture });
            }

            int i = 0;
            foreach (Texture2D texture in mapTextures)
            {
                if (segments[i].DisplayTexture.Width != texture.Width || segments[i].DisplayTexture.Height != texture.Height)
                {
                    throw new ArgumentException("levelTextures and mapTextures must have the same size");
                }

                segments[i++].MapTexture = texture;
            }

            if (i != segments.Count)
            {
                throw new ArgumentException("levelTextures and mapTextures must have the same number of elements");
            }
        }

        public void Draw(SpriteBatch batch, Microsoft.Xna.Framework.Vector2 offset)
        {
            Rectangle viewPort = new Rectangle(0, 0, Game1.VIEWPORT_WIDTH, Game1.VIEWPORT_HEIGHT);
            Point currentPoint = new Point((int)offset.X, (int)offset.Y);

            foreach (MapSegment seg in this.segments)
            {
                Rectangle textureRect = new Rectangle(currentPoint.X, currentPoint.Y, seg.DisplayTexture.Width, seg.DisplayTexture.Height);
                if (viewPort.Intersects(textureRect))
                {
                    batch.Draw(seg.DisplayTexture, new Vector2(currentPoint.X, currentPoint.Y), null,
                    Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0f);
                }

                currentPoint.X += seg.DisplayTexture.Width;
            }
        } 

		private static object syncObj = new object();
        public void GetData(Microsoft.Xna.Framework.Rectangle playerBounds, Color[] result, Color[] buffer, int startIndex, int count)
        {
            Point currentPoint = new Point(0,0);
            MapSegment segment1 = null;
            MapSegment segment2 = null;
			Rectangle segment1Rect = Rectangle.Empty;
            Rectangle segment2Rect = Rectangle.Empty;

			foreach (MapSegment seg in this.segments)
			{
				Rectangle textureRect = new Rectangle(currentPoint.X, currentPoint.Y, seg.MapTexture.Width, seg.MapTexture.Height);
				if (playerBounds.Intersects(textureRect))
				{
					if (segment1 == null)
					{
						segment1 = seg;
						segment1Rect = textureRect;
					}
					else if (segment2 == null)
					{
						segment2 = seg;
						segment2Rect = textureRect;
					}
					else
					{
						//throw new ArgumentException("Goose object intersects more than two level segments. Please increase the size of the segments to avoid this.");
					}
				}

				currentPoint.X += seg.DisplayTexture.Width;
			}

			for (int i = 0; i < result.Length; i++)
			{
				result[i] = ColorConsts.SolidWallColor;
			}
            
            if (segment1 != null)
            {
                Texture2D texture = segment1.MapTexture;
                Rectangle bounds = Intersect(playerBounds, segment1Rect);

                bounds.Offset(-segment1Rect.X, -segment1Rect.Y);
				lock (syncObj)
				{
					if (segment2 == null)
					{
						texture.GetData<Color>(0, bounds, result, 0, bounds.Width * bounds.Height);
						return;
					}
					else
					{
						texture.GetData<Color>(0, bounds, buffer, 0, bounds.Width * bounds.Height);
					}
				}
                bounds.Offset(segment1Rect.Location);
                MapData(playerBounds, bounds, result, buffer);
            }
            if (segment2 != null)
            {
                Texture2D texture = segment2.MapTexture;
                Rectangle bounds = Intersect(playerBounds, segment2Rect);

                bounds.Offset(-segment2Rect.X, -segment2Rect.Y);
				lock (syncObj)
				{
					texture.GetData<Color>(0, bounds, buffer, 0, bounds.Width * bounds.Height);
				}
                bounds.Offset(segment2Rect.Location);
                MapData(playerBounds, bounds, result, buffer);
            }
        }

        private static Rectangle Intersect(Rectangle rect1, Rectangle rect2)
        {
            return new Rectangle(Math.Max(rect1.X, rect2.X), Math.Max(rect1.Y, rect2.Y),
                Math.Min(rect1.Right, rect2.Right) - Math.Max(rect1.X, rect2.X),
                Math.Min(rect1.Bottom, rect2.Bottom) - Math.Max(rect1.Y, rect2.Y));
        }

        private static void MapData(Rectangle playerBounds, Rectangle bounds, Color[] result, Color[] buffer)
        {
            for (int i = 0; i < bounds.Height; i++)
            {
				//Array.Copy
				//Array.Copy(buffer, i * playerBounds.Width + (bounds.Left - playerBounds.Left), result, i * bounds.Width, bounds.Width);
				for (int j = 0; j < bounds.Width; j++)
				{
					result[i * playerBounds.Width + j + (bounds.Left - playerBounds.Left)] = buffer[i * bounds.Width + j];
				}
            }
        }
    }

    public static class GameLevelManager
    {
        public static GameLevel CurrentLevel { get; private set; }
        
        public static void LoadLevel(int levelNumber, ContentManager manager)
        {
            if (levelNumber == 1)
            {
                LoadLevel1(manager);
            }
        }

        private static void LoadLevel1(ContentManager manager)
        {
            Texture2D levelTexture = manager.Load<Texture2D>("level1");
            Texture2D map0Texture = manager.Load<Texture2D>("map0");
            Texture2D mapTexture = manager.Load<Texture2D>("map");
            CurrentLevel = new GameLevel(
                new Texture2D[] { levelTexture, levelTexture, levelTexture }, 
                new Texture2D[] { map0Texture, map0Texture, mapTexture },
                manager.Load<Song>("music"));
        }
    }
}
