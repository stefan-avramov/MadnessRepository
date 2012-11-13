using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using World;
using Common;
using System.Windows.Threading;

namespace GooseUnrunnableRuns
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private DispatcherTimer timer;

		public MainWindow()
		{
			InitializeComponent();
			Init();
			BindToViewModel();
		}

		private void Init()
		{
			mainWindow.Height = GameEnvironment.WindowHeight;
			mainWindow.Width = GameEnvironment.WindowWidth;
			
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromMilliseconds(GameEnvironment.GameTickInterval);
			timer.Tick += GameIntervalTickHandler;
			timer.Start();
		}

		private void GameIntervalTickHandler(object sender, EventArgs e)
		{
			if (mainWindow.IsActive)
			{
				WorldContainer.GameTick();
			}
		}

		private void BindToViewModel()
		{
			BindGroundObjects();
			BindGoose();
		}

		private void BindGoose()
		{
			GooseObject goose = WorldContainer.Goose;

			var image = new Image();
			image.Height = GameEnvironment.GooseHeight;
			image.Width = GameEnvironment.GooseWidth * 2;
			image.Margin = new Thickness(-18, 0, 0, 0);

			Binding imageBinding = new Binding();
			imageBinding.Source = goose;
			imageBinding.Mode = BindingMode.TwoWay;
			imageBinding.Path = new PropertyPath("ImageSource");

			BindingOperations.SetBinding(image, Image.SourceProperty, imageBinding);

			Canvas rect = new Canvas();
			rect.Height = GameEnvironment.GooseHeight;
			rect.Width = GameEnvironment.GooseWidth;
			rect.ClipToBounds = false;
			rect.Children.Add(image);

			Binding binding = new Binding();
			binding.Source = goose;
			binding.Mode = BindingMode.TwoWay;
			binding.Path = new PropertyPath("Point");
			binding.Converter = new PointToMarginConverter();

			BindingOperations.SetBinding(rect, Rectangle.MarginProperty, binding);

			mainCanvas.Children.Add(rect);
		}

		private void BindGroundObjects()
		{
			foreach (var groundObject in WorldContainer.GroundObjects)
			{
				Rectangle rect = new Rectangle();
				rect.Fill = new SolidColorBrush(Color.FromRgb(80, 80, 80));
				rect.Width = groundObject.Width;
				rect.Height = groundObject.Height;

				Binding binding = new Binding();
				binding.Source = groundObject;
				binding.Mode = BindingMode.TwoWay;
				binding.Path = new PropertyPath("OffsetX");
				binding.Converter = new OffsetToMarginConverter();
				binding.ConverterParameter = groundObject.OffsetY;

				BindingOperations.SetBinding(rect, Rectangle.MarginProperty, binding);
				mainCanvas.Children.Add(rect);
			}
		}

		private void MainWindowKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				mainWindow.Close();
			}
		}
	}
}
