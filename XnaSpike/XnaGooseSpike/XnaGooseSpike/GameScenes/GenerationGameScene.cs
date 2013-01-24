using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;
using System.Text;
using System.IO;

namespace XnaGooseGame
{
	class GenerationGameScene : GameScene
	{
		const double CROSS_PERCENT = 0.7;
		const double MUTATION_PERCENT = 0.1;
		const double RESET_PERCENT = 0.05;
		const int MAX_ACTIONS = 400;
		const string STATE_FILE = "generation_state.txt";

		private List<PlayerController> players;
		private double speedMultiplier = 1.0;
		private int playersCount;
		private bool started = false;
		private List<CoinElement> coins;
		private int currentGeneration = 1;

		public GenerationGameScene(int playersCount)
		{
			this.playersCount = playersCount;
			this.InitializePlayers();
		}

		private void InitializePlayers()
		{
			StreamReader sr = null;
			if (System.IO.File.Exists(STATE_FILE))
			{
				sr = File.OpenText(STATE_FILE);
				string gen = sr.ReadLine();
				int.TryParse(gen, out currentGeneration);
			}

			this.players = new List<PlayerController>();
			for (int i = 0; i < playersCount; i++)
			{
				PlayerController info = new PlayerController(new PlayerElement());
				info.UsePredefinedActions = true;
				if (sr != null && !sr.EndOfStream)
				{
					string line = sr.ReadLine();
					info.PredefinedActions.AddRange(ReadActions(line));
				}
				else
				{
					info.PredefinedActions.AddRange(PickReset());
				}

				this.players.Add(info);
				this.Elements.Add(info.Player);
			}

			if (sr != null)
			{
				sr.Close();
				sr.Dispose();
			}

			PopulatonLogger.SetGeneration(currentGeneration);
		}

		private IEnumerable<PlayerAction> ReadActions(string line)
		{
			List<PlayerAction> list = new List<PlayerAction>();
			string[] actions = line.Split(',');
			foreach (string action in actions)
			{
				list.Add((PlayerAction)Enum.Parse(typeof(PlayerAction), action));
			}

			return list;
		}

		private void NextGeneration()
		{			
			foreach (PlayerController controller in this.players)
			{
				this.Elements.Remove(controller.Player);
			}

			RandomShuffle(this.players);

			List<double> scores = new List<double>(new double[this.players.Count]);
			List<double> probabilities = new List<double>(scores);
			List<PlayerController> newList = new List<PlayerController>();

			double totalScore = 0;
			for (int i = 0; i < scores.Count; i++)
			{
				if (this.players[i].StatusHistory.Count == 0)
				{
					scores[i] = probabilities[i] = 0;
					continue;
				}

				PlayerStatus status = this.players[i].StatusHistory[this.players[i].StatusHistory.Count - 1];
				scores[i] = GetScore(status);
				totalScore += scores[i];
			}

			for (int i = 0; i < scores.Count; i++)
			{
				probabilities[i] = scores[i] / totalScore;
			}

			for (int i = 0; i < playersCount; i++)
			{
				PlayerController info = new PlayerController(players[i].Player.Clone());
				info.Player.Reset();
				info.UsePredefinedActions = true;

				if (i <= playersCount * MUTATION_PERCENT)
				{
					info.PredefinedActions.AddRange(PickMutated(this.players));
				}
				else if (i <= playersCount * (MUTATION_PERCENT + CROSS_PERCENT))
				{
					info.PredefinedActions.AddRange(PickCrossed(this.players, probabilities));
				}
				else if (i <= playersCount * (MUTATION_PERCENT + CROSS_PERCENT + RESET_PERCENT))
				{
					info.PredefinedActions.AddRange(PickReset());
				}
				else
				{
					info.PredefinedActions.AddRange(PickUnchaged(this.players, probabilities));
				}

				newList.Add(info);
				this.Elements.Add(info.Player);
			}

			this.players = newList;
			currentGeneration++;
			PopulatonLogger.SetGeneration(currentGeneration);
			PopulatonLogger.ResetCoins();
			SaveGeneration(this.players);
		}

		private void SaveGeneration(List<PlayerController> list)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(currentGeneration.ToString() + "\n");
			foreach (PlayerController controller in list)
			{
				foreach (PlayerAction action in controller.PredefinedActions)
				{
					sb.Append(action.ToString());
					sb.Append(",");
				}
				sb.Remove(sb.Length - 1, 1);
				sb.Append("\n");
			}

			System.IO.File.WriteAllText(STATE_FILE, sb.ToString());
		}

		private static double GetScore(PlayerStatus status)
		{
			return (status.Location.X * status.Location.X / 100.0d + status.CollectedValue * 3000.0d) / (double)status.Step;
		}

		private static IEnumerable<PlayerAction> PickUnchaged(List<PlayerController> controllers, List<double> probabilities)
		{
			double rand = RandomGenerator.NextDouble();
			double sum = probabilities[0];
			int index = 0;

			for (int i = 1; i < probabilities.Count; i++)
			{
				if (rand < sum)
				{
					break;
				}

				index = i;
				sum += probabilities[i];
			}

			PlayerController controller = controllers[index];
			List<PlayerAction> actions = new List<PlayerAction>();
			actions.AddRange(controller.ActionsHistory);
			if (controller.Player.IsDead)
			{
				actions.RemoveRange(actions.Count - Math.Min(5, actions.Count), Math.Min(5, actions.Count));
			}

			for (int j = actions.Count; j < MAX_ACTIONS; j++)
			{
				actions.Add(RandomGenerator.NextMove());
			}

			return actions;
		}

		private static IEnumerable<PlayerAction> PickReset()
		{
			List<PlayerAction> actions = new List<PlayerAction>();
			for (int j = 0; j < MAX_ACTIONS; j++)
			{
				actions.Add(RandomGenerator.NextMove());
			}

			return actions;
		}

		private static IEnumerable<PlayerAction> PickCrossed(List<PlayerController> controllers, List<double> probabilities)
		{
			double rand1 = RandomGenerator.NextDouble();
			double rand2 = RandomGenerator.NextDouble();

			double sum = probabilities[0];
			int index1 = 0;
			int index2 = 0;

			for (int i = 1; i < probabilities.Count; i++)
			{
				if (rand1 > sum)
				{
					index1 = i;
				}
				if (rand2 > sum)
				{
					index2 = i;
				}

				sum += probabilities[i];
			}

			return Cross(controllers[index1], controllers[index2]);
		}

		private static IEnumerable<PlayerAction> Cross(PlayerController c1, PlayerController c2)
		{
			if (c1.Player.IsDead)
			{
				c1.StatusHistory.RemoveRange(c1.StatusHistory.Count - Math.Min(5, c1.StatusHistory.Count), Math.Min(5, c1.StatusHistory.Count));
			}
			if (c2.Player.IsDead)
			{
				c2.StatusHistory.RemoveRange(c2.StatusHistory.Count - Math.Min(5, c2.StatusHistory.Count), Math.Min(5, c2.StatusHistory.Count));
			}

			List<PlayerAction> actions = new List<PlayerAction>();

			for (int i = 0; i < Math.Max(c1.StatusHistory.Count, c2.StatusHistory.Count); )
			{
				int changeLen = RandomGenerator.Next(30, 50);
				if (i + changeLen < c1.StatusHistory.Count && i + changeLen < c2.StatusHistory.Count)
				{
					if (2*GetScore(c1.StatusHistory[i + changeLen]) - 0.5*GetScore(c1.StatusHistory[i]) < 
						2*GetScore(c2.StatusHistory[i + changeLen]) - 0.5*GetScore(c2.StatusHistory[i]))
					{
						for (int j = i; j < i + changeLen; j++)
						{
							actions.Add(c2.ActionsHistory[j]);
						}
					}
					else
					{
						for (int j = i; j < i + changeLen; j++)
						{
							actions.Add(c1.ActionsHistory[j]);
						}
					}
				}
				else if (i + changeLen < c1.StatusHistory.Count)
				{
					for (int j = i; j < i + changeLen; j++)
					{
						actions.Add(c1.ActionsHistory[j]);
					}
				}
				else if (i + changeLen < c2.StatusHistory.Count)
				{
					for (int j = i; j < i + changeLen; j++)
					{
						actions.Add(c2.ActionsHistory[j]);
					}
				}
				else
				{
					break;
				}

				i += changeLen;
			}

			for (int j = actions.Count; j < MAX_ACTIONS; j++)
			{
				actions.Add(RandomGenerator.NextMove());
			}

			return actions;
		}

		private static IEnumerable<PlayerAction> PickMutated(List<PlayerController> controllers)
		{
			int index = RandomGenerator.Next(0, controllers.Count);
			List<PlayerAction> actions = new List<PlayerAction>();
			actions.AddRange(controllers[index].ActionsHistory);
			if (controllers[index].Player.IsDead)
			{
				actions.RemoveRange(actions.Count - Math.Min(5, actions.Count), Math.Min(5, actions.Count));
			}

			int changeLen = RandomGenerator.Next(3, 5);
			int start = RandomGenerator.Next(0, actions.Count);
			for (int i = start; i < Math.Min(start + changeLen, actions.Count); i++)
			{
				actions[i] = RandomGenerator.NextMove();
			}

			for (int j = actions.Count; j < MAX_ACTIONS; j++)
			{
				actions.Add(RandomGenerator.NextMove());
			}

			return actions;
		}

		public override void Update(GameTime gameTime)
		{
			gameTime = new GameTime(
				TimeSpan.FromMilliseconds(gameTime.TotalGameTime.TotalMilliseconds * speedMultiplier),
				TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.TotalMilliseconds * speedMultiplier),
				gameTime.IsRunningSlowly);

			if (this.started)
			{

				foreach (PlayerController info in this.players)
				{
					info.Update(gameTime);
				}
			}

			base.Update(gameTime);

			if (this.IsGenerationFinished())
			{
				this.NextGeneration();
				this.started = false;
				this.Start(gameTime);
			}
		}

		private bool IsGenerationFinished()
		{
			foreach (PlayerController player in this.players)
			{
				if (!player.Player.IsDead && !player.Player.HasWon)
				{
					return false;
				}
			}

			return true;
		}

		protected override void Start(GameTime gameTime)
		{
			if (started) return;
			started = true;
			foreach (PlayerController info in this.players)
			{
				info.Start(gameTime);
			}
		}

		protected override IEnumerable<PlayerElement> GetPlayers()
		{
			return this.players.Select(x => x.Player);
		}

		private static void RandomShuffle(IList list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				int index = RandomGenerator.Next(0, list.Count);
				object obj = list[i];
				list[i] = list[index];
				list[index] = obj;
			}
		}
	}
}