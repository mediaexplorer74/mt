using System;
using Microsoft.Xna.Framework;

namespace GameManager.GameContent.Drawing
{
	public class WindGrid
	{
		private struct WindCoord
		{
			public int Time;

			public int Direction;
		}

		private WindCoord[,] _grid = new WindCoord[1, 1];

		private int _width = 1;

		private int _height = 1;

		private int _gameTime;

		public void SetSize(int targetWidth, int targetHeight)
		{
			_width = Math.Max(_width, targetWidth);
			_height = Math.Max(_height, targetHeight);
			ResizeGrid();
		}

		public void Update()
		{
			_gameTime++;
			if (Main.SettingsEnabled_TilesSwayInWind)
			{
				ScanPlayers();
			}
		}

		public void GetWindTime(int tileX, int tileY, int timeThreshold, out int windTimeLeft, out int direction)
		{
			WindCoord windCoord = _grid[tileX % _width, tileY % _height];
			direction = windCoord.Direction;
			if (windCoord.Time + timeThreshold < _gameTime)
			{
				windTimeLeft = 0;
			}
			else
			{
				windTimeLeft = _gameTime - windCoord.Time;
			}
		}

		private void ResizeGrid()
		{
			if (_width > _grid.GetLength(0) || _height > _grid.GetLength(1))
			{
				_grid = new WindCoord[_width, _height];
			}
		}

		private void SetWindTime(int tileX, int tileY, int direction)
		{
			_grid[tileX % _width, tileY % _height].Time = _gameTime;
			_grid[tileX % _width, tileY % _height].Direction = direction;
		}

		private void ScanPlayers()
		{
			if (Main.netMode == 0)
			{
				ScanPlayer(Main.myPlayer);
			}
			else if (Main.netMode == 1)
			{
				for (int i = 0; i < 255; i++)
				{
					ScanPlayer(i);
				}
			}
		}

		private void ScanPlayer(int i)
		{
			Player player = Main.player[i];
			if (!player.active || player.dead || player.velocity.X == 0f || !Utils.CenteredRectangle(Main.Camera.Center, Main.Camera.UnscaledSize).Intersects(player.Hitbox) || player.velocity.HasNaNs())
			{
				return;
			}
			int direction = Math.Sign(player.velocity.X);
			foreach (Point item in Collision.GetTilesIn(player.TopLeft, player.BottomRight))
			{
				SetWindTime(item.X, item.Y, direction);
			}
		}
	}
}
