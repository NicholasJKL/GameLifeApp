namespace GameLifeApp.Entities
{
	public class Screen
	{
		Tile[,] _screen;
		int _width, _height;

		public Screen(int width, int height)
		{
			_screen = new Tile[height + 2, width + 2];

			_width = width;
			_height = height;

			for (int x = 1; x < height + 1; x++)
			{
				for (int y = 1; y < width + 1; y++)
				{
					var cell = new Cell('`');

					_screen[x, y] = cell;
				}
			}

			SetBorders(width, height);
		}

		private void SetBorders(int width, int height)
		{
			for (int x = 1; x < height + 1; x++)
			{
				var leftBorder = new Border('1', _screen[x, width]);
				var rightBorder = new Border('1', _screen[x, 1]);

				_screen[x, 0] = leftBorder;
				_screen[x, width + 1] = rightBorder;
			}

			for (int y = 1; y < width + 1; y++)
			{
				var upperBorder = new Border('1', _screen[height, y]);
				var underBorder = new Border('1', _screen[1, y]);

				_screen[0, y] = upperBorder;
				_screen[height + 1, y] = underBorder;
			}

			var leftUp = new Border('1', _screen[height, width]);
			var rightUp = new Border('1', _screen[height, 1]);
			var leftDown = new Border('1', _screen[1, width]);
			var rightDown = new Border('1', _screen[1, 1]);

			_screen[0, 0] = leftUp;
			_screen[0, width + 1] = rightUp;
			_screen[height + 1, 0] = leftDown;
			_screen[height + 1, width + 1] = rightDown;
		}

		public void SetAlive(int x, int y)
		{
			_screen[y + 1, x + 1].IsAlive = true;
		}

		public void UnsetAlive(int x, int y)
		{
			_screen[y + 1, x + 1].IsAlive = false;
		}

		public Tile GetTile(int x, int y)
		{
			return _screen[y + 1, x + 1];
		}

		public void Clear()
		{
			for (int x = 1; x < _height + 1; x++)
			{
				for (int y = 1; y < _width + 1; y++)
				{
					_screen[x, y].IsAlive = false;
				}
			}
		}

		public void Show()
		{
			var columns = _screen.GetLength(0);
			var rows = _screen.GetLength(1);

			for (int x = 1; x < _height; x++) //TEST
			{
				for (int y = 1; y < _width; y++) //TEST
				{
					if (_screen[x, y].IsAlive)
						Console.Write('0');
					else
						Console.Write(_screen[x, y].CharDisplay);

					Console.Write(' ');
				}
				Console.Write('\n');
			}
		}

		private int CountPop(int x, int y)
		{
			int count = 0;

			if (_screen[x - 1, y - 1].IsAlive)
				count++;
			if (_screen[x - 1, y].IsAlive)
				count++;
			if (_screen[x - 1, y + 1].IsAlive)
				count++;
			if (_screen[x, y - 1].IsAlive)
				count++;
			if (_screen[x, y + 1].IsAlive)
				count++;
			if (_screen[x + 1, y - 1].IsAlive)
				count++;
			if (_screen[x + 1, y].IsAlive)
				count++;
			if (_screen[x + 1, y + 1].IsAlive)
				count++;

			return count;
		}

		public void Update()
		{
			var columns = _screen.GetLength(0);
			var rows = _screen.GetLength(1);

			var toAlive = new List<Tile>();
			var toDead = new List<Tile>();

			for (int x = 1; x < columns - 1; x++)
			{
				for (int y = 1; y < rows - 1; y++)
				{
					int count = CountPop(x, y);

					if (count == 3 && !_screen[x, y].IsAlive)
						toAlive.Add(_screen[x, y]);
					else if (count < 2 || count > 3)
						toDead.Add(_screen[x, y]);
				}
			}
			toAlive.ForEach(cell => cell.IsAlive = true);
			toDead.ForEach(cell => cell.IsAlive = false);
		}
	}
}

