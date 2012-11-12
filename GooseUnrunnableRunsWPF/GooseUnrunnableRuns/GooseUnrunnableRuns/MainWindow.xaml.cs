﻿using System;
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
        DispatcherTimer timer;

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
            timer.Interval = TimeSpan.FromMilliseconds(GameEnvironment.GameInterval);
            timer.Tick += GameIntervalTickHandler;
            timer.Start();
        }

        private void GameIntervalTickHandler(object sender, EventArgs e)
        {
            WorldContainer.GameTick();
        }

        private void BindToViewModel()
        {
            BindGroundObjects();
            BindGoose();
        }

        private void BindGoose()
        {
            GooseObject goose = WorldContainer.Goose;

            Rectangle rect = new Rectangle();
            rect.Fill = new SolidColorBrush(Color.FromRgb(58, 67, 100));
            rect.Width = goose.Width;
            rect.Height = goose.Height;

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
