using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace XnaGooseGame
{
	static class GameLevelManager
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
			Texture2D levelTexture = manager.Load<Texture2D>("Level1/level1");
			Texture2D map0Texture = manager.Load<Texture2D>("Level1/map0");
			Texture2D mapTexture = manager.Load<Texture2D>("Level1/map");
			CurrentLevel = new GameLevel(
				new Texture2D[] { levelTexture, levelTexture },
				new Texture2D[] { map0Texture, mapTexture },
				manager.Load<Song>("music"));


			CurrentLevel.InteractionObjects.Add(new AxeElement(1600, 300));
			CurrentLevel.InteractionObjects.Add(new AxeElement(7800, 593));
			CurrentLevel.InteractionObjects.Add(new FireSmokeElement(4000, 400));
			CurrentLevel.InteractionObjects.Add(new FireSmokeElement(5711, 400));
			CurrentLevel.InteractionObjects.Add(new BatmanElement(530, 469, 700));
			CurrentLevel.LoadCoints();
		}

		//TODO: add more levels

		public static void LoadCredits(ContentManager manager)
		{
			Texture2D levelTexture = manager.Load<Texture2D>("credits");
			
			CurrentLevel = new GameLevel(
				new Texture2D[] { levelTexture },
				new Texture2D[] { levelTexture },
				manager.Load<Song>("credits_theme"));
		}
	}
}