using System;

namespace XnaGooseGame
{
#if WINDOWS || XBOX
    static class Program
    {
        const int DEFAULT_LEVEL = 1;
        const int DEFAULT_COUNT = 20;
		const GameMode DEFAULT_MODE = GameMode.Credits;
		
        /// <summary>
        /// The main entry point for the application.
        /// Use the folloing command line options:
        /// -l X (loads the level with number X)
        /// -m [single|algo1|algo2|credits] (specifies the game mode)
        /// -n X (specifies the number of gooses loaded
        /// </summary>
        static void Main(string[] args)
        {
            GameMode mode;
            int level, count;
            ParseArguments(args, out mode, out level, out count);
         
            using (Game1 game = new Game1(mode, level, count))
            {
                game.Run();
            }
        }

        private static void ParseArguments(string[] args, out GameMode mode, out int level, out int count)
        {
            mode = DEFAULT_MODE;
            level = DEFAULT_LEVEL;
            count = DEFAULT_COUNT;

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
                                case "algo2":
                                    mode = GameMode.GeneticAlgorithm2;
                                    break;
								case "algoBest":
                                    mode = GameMode.BestGenerationAlgorithm;
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
                }
            }
        }
    }
#endif
}

