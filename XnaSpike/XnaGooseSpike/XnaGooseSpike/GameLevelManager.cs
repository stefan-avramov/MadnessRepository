using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace XnaGooseGame
{
	static class GameLevelManager
	{
		public static GameLevel CurrentLevel { get; private set; }


		public static void LoadLevel(int levelNumber, bool hasBatman, ContentManager manager)
		{
			switch (levelNumber)
			{
				case 1:
					LoadLevel1(manager, hasBatman);
					break;
				case 2:
					LoadLevel2(manager);
					break;
			}
		}

		private static void LoadLevel1(ContentManager manager, bool hasBatman)
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

		private static void LoadLevel2(ContentManager manager)
		{
			Texture2D map0Texture = manager.Load<Texture2D>("Level2/map0");
			Texture2D map1Texture = manager.Load<Texture2D>("Level2/map1");
			Texture2D level0Texture = manager.Load<Texture2D>("Level2/level0");
			Texture2D level1Texture = manager.Load<Texture2D>("Level2/level1");
			CurrentLevel = new GameLevel(
				new Texture2D[] { level0Texture, level1Texture},
				new Texture2D[] { map0Texture, map1Texture },
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