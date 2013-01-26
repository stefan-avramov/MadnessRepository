using System;
using Microsoft.Xna.Framework;
using System.IO;
using System.Linq;

namespace XnaGooseGame
{
#if WINDOWS || XBOX
    static class Program
    {
        const int DEFAULT_LEVEL = 1;
        const int DEFAULT_COUNT = 150; 
		const GameMode DEFAULT_MODE = GameMode.AStar; 

        /// <summary>
        /// The main entry point for the application.
        /// Use the folloing command line options:
        /// -l X (loads the level with number X)
        /// -m [single|algo1|algoBest|credits] (specifies the game mode)
        /// -n X (specifies the number of gooses loaded
		/// -b [true|false] (specifies whether to use Batman)
        /// </summary>
        static void Main(string[] args)
        {
            GameMode mode;
            int level, count;
			bool hasBatman;
            ParseArguments(args, out mode, out level, out count, out hasBatman);
         
            using (Game1 game = new Game1(mode, level, count, hasBatman))
            {
                game.Run();
            }
        }

        private static void ParseArguments(string[] args, out GameMode mode, out int level, out int count, out bool hasBatman)
        {
            mode = DEFAULT_MODE;
            level = DEFAULT_LEVEL;
            count = DEFAULT_COUNT;
			hasBatman = false;

            for (int i = 0; i < args.Length - 1; i++)
            {
                switch (args[i])
                {
                    case "-m":
                        {
                            switch (args[i + 1])
                            {
                                case "single":
                                    mode = GameMode.Single;
                                    break;
                                case "algo1":
                                    mode = GameMode.GeneticAlgorithm1;
                                    break;
								case "algoBest":
                                    mode = GameMode.BestGenerationAlgorithm;
									break;
								case "algoAstar":
									mode = GameMode.AStar;
									break;
                                case "credits":
                                    mode = GameMode.Credits;
                                    break;
                            }
                            break;
                        }
                    case "-l":
                        int.TryParse(args[i + 1],out level);
                        break;
                    case "-n":
                        int.TryParse(args[i + 1],out count);
                        break; 
					case "-b":
						bool.TryParse(args[i + 1], out hasBatman);
						break;
                }
            }
        }
    }
#endif
}

