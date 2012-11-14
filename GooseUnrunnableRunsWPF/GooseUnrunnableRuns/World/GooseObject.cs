using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Common;
using System.Windows.Input;
using System.Windows.Media;

namespace World
{
	public class GooseObject : ViewModel
	{
		Point point;
		readonly int height;
		readonly int width;

		private bool isJumping = false;
		private int accelaration = 0;
		private bool isDead = false;
		
		private ImageSource imageSource;
		private int imageIndex = -1;
		private bool isMoving = false;
		private bool isFacingLeft = false;

		private GroundObject groundObjectUnder = null;

		public GooseObject()
		{
			this.height = GameEnvironment.GooseHeight;
			this.width = GameEnvironment.GooseWidth;
			Respawn();
		}

		private void Respawn()
		{
			this.Point = 
				new Point(GameEnvironment.WindowWidth / 2,
							GameEnvironment.WindowHeight - WorldContainer.GroundObjects[0].Height - GameEnvironment.GooseHeight);

			this.isDead = false;
			this.isFacingLeft = false;

			Move(WorldContainer.GroundObjects[0].OffsetX, 0);
			groundObjectUnder = GetGroundObjectUnder();

			ImageIndex = 0;
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
				else if (!isJumping)
				{
					ImageIndex = 0;
				}

				if (isJumping)
				{
					ProceedJumping();
				}
				else if (Keyboard.IsKeyDown(Key.Space) || Keyboard.IsKeyDown(Key.Up))
				{
					Jump();
				}
				else if (groundObjectUnder == null)
				{
					Fall();
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
			MoveHorizontally(-GameEnvironment.GooseSpeed);
		}

		private void MoveRight()
		{
			MoveHorizontally(GameEnvironment.GooseSpeed);
		}

		private void MoveHorizontally(int offset)
		{
			Move(offset, 0);

			isFacingLeft = offset < 0;

			if (!isMoving)
			{
				imageIndex = 2;
			}

			isMoving = true;
			ImageIndex = (imageIndex + 1)%6;
		}

		private void RefreshRunImage()
		{
			if (isFacingLeft)
			{
				ImageSource = ImagesContainer.GooseRunLeftImages[imageIndex / 3];
			}
			else
			{
				ImageSource = ImagesContainer.GooseRunRightImages[imageIndex / 3];
			}
		}

		private void Move(int offsetX, int offsetY)
		{
			WorldContainer.Move(offsetX);
			point.Offset(0, offsetY);
			groundObjectUnder = GetGroundObjectUnder();
			OnPropertyChanged("Point");
		}

		private int ImageIndex
		{
			get
			{
				return imageIndex;
			}
			set
			{
				if (imageIndex != value)
				{
					imageIndex = value;
					RefreshRunImage();
				}
			}
		}

		public int Height
		{
			get 
			{
				return height;
			}
		}

		public int Width
		{
			get 
			{
				return width;
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

		public ImageSource ImageSource
		{
			get
			{
				return imageSource;
			}
			set
			{
				if (imageSource != value)
				{
					imageSource = value;
					OnPropertyChanged("ImageSource");
				}
			}
		}
	}
}
