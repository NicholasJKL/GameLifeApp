﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using GameLifeApp.Entities;
using System.Windows.Threading;
using System.Diagnostics.Metrics;


namespace GameLifeApp
{
	public partial class MainWindow : Window
	{
		WriteableBitmap _writeableBitmap;
		Screen _screen;
		DispatcherTimer _timer;

		bool _timerPause = true;
		int _width = 200;
		int _height = 100;
		int _speed = 250;


		public MainWindow()
		{
			InitializeComponent();

			_writeableBitmap = new WriteableBitmap(_width, _height, 96, 96, PixelFormats.Bgra32, null);
			_screen = new Screen(_width, _height);
			_timer = new DispatcherTimer();

			_timer.Interval = TimeSpan.FromMilliseconds(_speed);
			_timer.Tick += Update;

			MyImage.Source = _writeableBitmap;

			byte[] pixels = CreatePixelData();

			UpdateBitmap(pixels);
		}

		void Update(object? sender = null, EventArgs? e = null)
		{
			_screen.Update();

			byte[] pixels = CreatePixelData();

			UpdateBitmap(pixels);
		}

		byte[] CreatePixelData() // BGRA
		{
			byte[] pixels = new byte[_width * _height * 4];

			for (int y = 0; y < _height; y++)
			{
				for (int x = 0; x < _width; x++)
				{
					int index = (y * _width + x) * 4;

					byte b = 0;
					byte g = 0;
					byte r = 0;

					if (_screen.GetStatus(x, y))
					{
						g = 255;
					}

					pixels[index] = b;
					pixels[index + 1] = g;
					pixels[index + 2] = r;
					pixels[index + 3] = 255;
				}
			}
			return pixels;
		}

		void UpdateBitmap(byte[] pixels)
		{
			Int32Rect rect = new Int32Rect(0, 0, _width, _height);
			int stride = _width * 4;

			_writeableBitmap.WritePixels(rect, pixels, stride, 0);
		}

		void TimerManagerButton_Click(object sender, RoutedEventArgs e)
		{
			if (_timerPause)
			{
				_timer.Start();
				TimerManagerButton.Content = "Пауза";
			}
			else
			{
				_timer.Stop();
				TimerManagerButton.Content = "Запустить";
			}
			_timerPause = !_timerPause;
		}

		void RandomConfigurationButton_Click(object sender, RoutedEventArgs e) 
		{
			Random rnd = new Random();

			int count = rnd.Next(3, _width * _height + 1);

			for (int i = 0; i < count; i++)
			{
				int x = rnd.Next(0, _width);
				int y = rnd.Next(0, _height);

				_screen.SetAlive(x, y);
			}

			Update();
		}

		void SpeedTryParse(object sender, EventArgs e) 
		{

			if (int.TryParse(SpeedBox.Text, out int value) && value >= 100) 
			{
				_speed = value;

				if (_timer is not null)
				{
					_timer.Interval = TimeSpan.FromMilliseconds(_speed);
				}
			}
			else 
			{
				SpeedBox.Text = Convert.ToString(_speed);
			}
		}

		void IncrementSpeedButton_Click(object sender, RoutedEventArgs e) 
		{
			SpeedBox.Text = Convert.ToString(_speed + 10);
			SpeedTryParse(SpeedBox, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.None));
		}

		void DecrementSpeedButton_Click(object sender, RoutedEventArgs e) 
		{
			SpeedBox.Text = Convert.ToString(_speed - 10);
			SpeedTryParse(SpeedBox, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.None));
		}

	}
}