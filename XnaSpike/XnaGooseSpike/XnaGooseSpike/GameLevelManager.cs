using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace XnaGooseGame
{
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


			CurrentLevel.InteractionObjects.Add(new AxeElement(4200, 250));
			CurrentLevel.InteractionObjects.Add(new FireSmokeElement(4200, 380));
		}

		//TODO: add more levels
	}
}