using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Wintellect.PowerCollections;
using System.Threading.Tasks;

namespace XnaGooseGame
{
	class AStarGameScene : SinglePlayerGameScene
	{
		private const int FramesPerSecond = 60;
		private const double MoveDuration = 1000.0 / FramesPerSecond;

		private readonly Queue<PlayerAction> pendingActions = new Queue<PlayerAction>();

		private List<SceneElement> timeDependentElements;
		private OrderedBag<PlayerNode> orderedSet;
		private GameTime sceneTime = new GameTime(new TimeSpan(), new TimeSpan((long)MoveDuration * TimeSpan.TicksPerMillisecond));

		protected override void InitializeElements()
		{
			base.InitializeElements();
			this.player.Location = new Vector2(20, 300);
			this.Offset = new Vector2(0, 0);
			
			timeDependentElements = new List<SceneElement>(this.Elements.Where(element => element is ITimeDependentInteractionElement));
		}

		public override void Update(GameTime gameTime)
		{
			if (player.IsDead)
			{
				return;
			}

			if (!player.HasWon && pendingActions.Count == 0)
			{
				ComputeNextActions();
			}

			PlayerAction currentAction = player.HasWon ? PlayerAction.None : pendingActions.Dequeue();
			switch (currentAction)
			{
				case PlayerAction.None:
					player.Stop();
					break;
				case PlayerAction.MoveRight:
					player.MoveForward();
					break;
				case PlayerAction.MoveLeft:
					player.MoveBackward();
					break;
				case PlayerAction.Jump:
					player.Jump();
					break;
			}

			sceneTime = IncrementTime(sceneTime);
			Parallel.ForEach(this.Elements, element => element.Update(sceneTime));
			HandleInteraction(player);
			this.Offset = new Vector2(-this.player.Location.X + Game1.VIEWPORT_WIDTH / 2 - PlayerElement.PLAYER_WIDTH / 2, 0);
		}


		PlayerAction[] possibleActions = new PlayerAction[] { PlayerAction.None, PlayerAction.MoveLeft, PlayerAction.MoveRight, PlayerAction.Jump };
		private void ComputeNextActions()
		{
			float goalDestinationX = player.Location.X + 700;
			HashSet<int> visited = new HashSet<int>();

			orderedSet = new OrderedBag<PlayerNode>((a, b) => a.Destination((int)goalDestinationX).CompareTo(b.Destination((int)goalDestinationX)));

			PlayerNode initialNode = new PlayerNode(null, sceneTime, 0, PlayerAction.None, player.Clone(), 0);
			orderedSet.Add(initialNode);
			visited.Add((int)initialNode.PlayerElement.Location.X << 10 + (int)initialNode.PlayerElement.Location.Y);

			PlayerNode bestNode = initialNode;
			bool foundBest = false;

			while (orderedSet.Count > 0 && !foundBest)
			{
				PlayerNode current = orderedSet.RemoveFirst();

				if (current.PlayerElement.IsOnGround() && bestNode.Destination((int)goalDestinationX) > current.Destination((int)goalDestinationX))
				{
					bestNode = current;
				}

				foreach (PlayerAction action in possibleActions)
				{
					if (action == PlayerAction.Jump && !current.PlayerElement.IsOnGround())
					{
						continue;
					}

					PlayerElement newPlayer = current.PlayerElement.Clone();
					switch (action)
					{
						case PlayerAction.None:
							newPlayer.Stop();
							break;
						case PlayerAction.MoveRight:
							newPlayer.MoveForward();
							break;
						case PlayerAction.MoveLeft:
							newPlayer.MoveBackward();
							break;
						case PlayerAction.Jump:
							newPlayer.Jump();
							break;
					}

					GameTime newTime = IncrementTime(current.Time);
					newPlayer.Update(newTime);
					Parallel.ForEach(timeDependentElements, element => element.Update(newTime));
					HandleInteraction(newPlayer);
					if (newPlayer.IsDead)
					{
						continue;
					}

					int hash = ((int)(newPlayer.Location.X * 10) << 7) + (int)newPlayer.Location.Y;
					if (!visited.Add(hash))
					{
						continue;
					}
					
					PlayerNode newNode = new PlayerNode(current, newTime, current.Moves + 1, action, newPlayer, current.Jumps + (action == PlayerAction.Jump ? 1 : 0));
					if (newPlayer.Location.X > goalDestinationX && newPlayer.IsOnGround() || newPlayer.HasWon)
					{
						bestNode = newNode;
						foundBest = true;
						break;
					}
					orderedSet.Add(newNode);
				}
			}

			orderedSet.Clear();
			GetActionsFromNode(bestNode);
		}

		private void GetActionsFromNode(PlayerNode node)
		{
			if (node.Parent == null)
			{
				return;
			}
			else
			{
				GetActionsFromNode(node.Parent);
			}

			pendingActions.Enqueue(node.Action);
		}

		private GameTime IncrementTime(GameTime currentTime)
		{
			return new GameTime(currentTime.TotalGameTime + currentTime.ElapsedGameTime, currentTime.ElapsedGameTime);
		}

		private class PlayerNode
		{
			public PlayerNode Parent;
			public GameTime Time;
			public PlayerAction Action;
			public int Moves;
			public PlayerElement PlayerElement;
			public int Jumps;

			public PlayerNode(PlayerNode parent, GameTime time, int moves, PlayerAction action, PlayerElement playerElement, int jumps)
			{
				this.Parent = parent;
				this.Time = time;
				this.Moves = moves;
				this.Action = action;
				this.PlayerElement = playerElement;
				this.Jumps = jumps;
			}
			
			public int Destination(int goal)
			{
				int x = (int)PlayerElement.Location.X;
				return goal - x;
			}
		}

		private enum PlayerAction : byte
		{
			None,
			MoveRight,
			MoveLeft,
			Jump
		}
	}
}