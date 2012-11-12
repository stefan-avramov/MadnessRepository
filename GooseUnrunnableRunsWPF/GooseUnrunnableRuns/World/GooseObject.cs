using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Common;
using System.Windows.Input;

namespace World
{
	public class GooseObject : ViewModel
	{
		Point point;
		int height;
		int width;

		private bool isJumping = false;
		private int accelaration = 0;
		private bool isDead = false;

		private GroundObject groundObjectUnder = null;

		public GooseObject()
		{
			Respawn();
		}

		private void Respawn()
		{
			this.Point = 
				new Point(GameEnvironment.WindowWidth / 2,
							GameEnvironment.WindowHeight - WorldContainer.GroundObjects[0].Height - GameEnvironment.GooseHeight);
			this.Height = GameEnvironment.GooseHeight; 
			this.Width = GameEnvironment.GooseWidth;

			this.isDead = false;

			Move(WorldContainer.GroundObjects[0].OffsetX, 0);
			groundObjectUnder = GetGroundObjectUnder();
		}

		private void Jump()
		{
			if (!isJumping)
			{
				isJumping = true;
				accelaration = GameEnvironment.GooseJumpSpeed;
			}
		}

		private void Fall()
		{
			if (!isJumping)
			{
				isJumping = true;
				accelaration = -2;
			}
		}

		public void Tick()
		{
			if (!isDead)
			{
				if (Keyboard.IsKeyDown(Key.Left))
				{
					MoveLeft();
				}
				else if (Keyboard.IsKeyDown(Key.Right))
				{
					MoveRight();
				}

				if (Keyboard.IsKeyDown(Key.Space))
				{
					Jump();
				}
				if (groundObjectUnder == null)
				{
					Fall();
				}

				if (isJumping)
				{
					ProceedJumping();
				}
			}
			else
			{
				if (Keyboard.IsKeyDown(Key.R))
				{
					Respawn();
				}
			}
		}

		private void ProceedJumping()
		{
			Move(0, -accelaration);
			accelaration -= 2;

			if (Point.Y > GameEnvironment.WindowHeight)
			{
				isDead = true;
				isJumping = false;
			}
			else
			{
				if (groundObjectUnder != null)
				{
					if (Point.Y + Height > GameEnvironment.WindowHeight - groundObjectUnder.Height)
					{
						Point = new Point(Point.X, groundObjectUnder.OffsetY - Height);
						isJumping = false;
					}
				}
			}
		}

		private GroundObject GetGroundObjectUnder()
		{
			foreach (var groundObject in WorldContainer.GroundObjects)
			{
				if (Point.X + Width > groundObject.OffsetX && Point.X < groundObject.OffsetX + groundObject.Width)
				{
					return groundObject;
				}
			}
			return null;
		}

		private void MoveLeft()
		{
			if (GameEnvironment.GooseSpeed <= Point.X)
			{
				Move(-GameEnvironment.GooseSpeed, 0);
			}
			else
			{
				Move(-(int)Point.X, 0);
			}
		}

		private void MoveRight()
		{
			Move(GameEnvironment.GooseSpeed, 0);
		}

		private void Move(int offsetX, int offsetY)
		{
			WorldContainer.Move(offsetX);
			point.Offset(0, offsetY);
			groundObjectUnder = GetGroundObjectUnder();
			OnPropertyChanged("Point");
		}

		public int Height
		{
			get 
			{
				return height;
			}
			set 
			{
				if (height != value)
				{
					height = value;
					OnPropertyChanged("Height");
				}
			}
		}

		public int Width
		{
			get 
			{
				return width;
			}
			set
			{
				if (width != value)
				{
					width = value;
					OnPropertyChanged("Width");
				}
			}
		}

		public Point Point
		{
			get
			{
				return point;
			}
			set
			{
				if (point != value)
				{
					point = value;
					OnPropertyChanged("Point");
				}
			}
		}
	}
}
