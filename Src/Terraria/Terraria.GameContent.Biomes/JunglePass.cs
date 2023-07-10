using System;
using Microsoft.Xna.Framework;
using GameManager.IO;
using GameManager.WorldBuilding;

namespace GameManager.GameContent.Biomes
{
	public class JunglePass : GenPass
	{
		private float _worldScale;

		public int JungleOriginX
		{
			get;
			set;
		}

		public int DungeonSide
		{
			get;
			set;
		}

		public double WorldSurface
		{
			get;
			set;
		}

		public int LeftBeachEnd
		{
			get;
			set;
		}

		public int RightBeachStart
		{
			get;
			set;
		}

		public int JungleX
		{
			get;
			private set;
		}

		public JunglePass()
			: base("Jungle", 10154.6523f)
		{
		}

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = Lang.gen[11].Value;
			_worldScale = (float)(Main.maxTilesX / 4200) * 1.5f;
			float worldScale = _worldScale;
			Point point = CreateStartPoint();
			int x = point.X;
			int y = point.Y;
			Point zero = Point.Zero;
			ApplyRandomMovement(x, y, 100, 100);
			zero.X += x;
			zero.Y += y;
			PlaceFirstPassMud(x, y, 3);
			PlaceGemsAt(x, y, 63, 2);
			progress.Set(0.15f);
			ApplyRandomMovement(x, y, 250, 150);
			zero.X += x;
			zero.Y += y;
			PlaceFirstPassMud(x, y, 0);
			PlaceGemsAt(x, y, 65, 2);
			progress.Set(0.3f);
			int oldX = x;
			int oldY = y;
			ApplyRandomMovement(x, y, 400, 150);
			zero.X += x;
			zero.Y += y;
			PlaceFirstPassMud(x, y, -3);
			PlaceGemsAt(x, y, 67, 2);
			progress.Set(0.45f);
			x = zero.X / 3;
			y = zero.Y / 3;
			int num = GenBase._random.Next((int)(400f * worldScale), (int)(600f * worldScale));
			int num2 = (int)(25f * worldScale);
			x = Utils.Clamp(x, LeftBeachEnd + num / 2 + num2, RightBeachStart - num / 2 - num2);
			WorldGen.mudWall = true;
			WorldGen.TileRunner(x, y, num, 10000, 59, addTile: false, 0f, -20f, noYChange: true);
			GenerateTunnelToSurface(x, y);
			WorldGen.mudWall = false;
			progress.Set(0.6f);
			GenerateHolesInMudWalls();
			GenerateFinishingTouches(progress, oldX, oldY);
		}

		private void PlaceGemsAt(int x, int y, ushort baseGem, int gemVariants)
		{
			for (int i = 0; (float)i < 6f * _worldScale; i++)
			{
				WorldGen.TileRunner(x + GenBase._random.Next(-(int)(125f * _worldScale), (int)(125f * _worldScale)), y + GenBase._random.Next(-(int)(125f * _worldScale), (int)(125f * _worldScale)), GenBase._random.Next(3, 7), GenBase._random.Next(3, 8), GenBase._random.Next(baseGem, baseGem + gemVariants));
			}
		}

		private void PlaceFirstPassMud(int x, int y, int xSpeedScale)
		{
			WorldGen.mudWall = true;
			WorldGen.TileRunner(x, y, GenBase._random.Next((int)(250f * _worldScale), (int)(500f * _worldScale)), GenBase._random.Next(50, 150), 59, addTile: false, DungeonSide * xSpeedScale);
			WorldGen.mudWall = false;
		}

		private Point CreateStartPoint()
		{
			return new Point(JungleOriginX, (int)((double)Main.maxTilesY + Main.rockLayer) / 2);
		}

		private void ApplyRandomMovement(int x, int y, int xRange, int yRange)
		{
			x += GenBase._random.Next((int)((float)(-xRange) * _worldScale), 1 + (int)((float)xRange * _worldScale));
			y += GenBase._random.Next((int)((float)(-yRange) * _worldScale), 1 + (int)((float)yRange * _worldScale));
			y = Utils.Clamp(y, (int)Main.rockLayer, Main.maxTilesY);
		}

		private void GenerateTunnelToSurface(int i, int j)
		{
			double num = GenBase._random.Next(5, 11);
			Vector2 vector = default(Vector2);
			vector.X = i;
			vector.Y = j;
			Vector2 vector2 = default(Vector2);
			vector2.X = (float)GenBase._random.Next(-10, 11) * 0.1f;
			vector2.Y = (float)GenBase._random.Next(10, 20) * 0.1f;
			int num2 = 0;
			bool flag = true;
			while (flag)
			{
				if ((double)vector.Y < Main.worldSurface)
				{
					if (WorldGen.drunkWorldGen)
					{
						flag = false;
					}
					int value = (int)vector.X;
					int value2 = (int)vector.Y;
					value = Utils.Clamp(value, 10, Main.maxTilesX - 10);
					value2 = Utils.Clamp(value2, 10, Main.maxTilesY - 10);
					if (value2 < 5)
					{
						value2 = 5;
					}
					if (Main.tile[value, value2].wall == 0 && !Main.tile[value, value2].active() && Main.tile[value, value2 - 3].wall == 0 && !Main.tile[value, value2 - 3].active() && Main.tile[value, value2 - 1].wall == 0 && !Main.tile[value, value2 - 1].active() && Main.tile[value, value2 - 4].wall == 0 && !Main.tile[value, value2 - 4].active() && Main.tile[value, value2 - 2].wall == 0 && !Main.tile[value, value2 - 2].active() && Main.tile[value, value2 - 5].wall == 0 && !Main.tile[value, value2 - 5].active())
					{
						flag = false;
					}
				}
				JungleX = (int)vector.X;
				num += (double)((float)GenBase._random.Next(-20, 21) * 0.1f);
				if (num < 5.0)
				{
					num = 5.0;
				}
				if (num > 10.0)
				{
					num = 10.0;
				}
				int value3 = (int)((double)vector.X - num * 0.5);
				int value4 = (int)((double)vector.X + num * 0.5);
				int value5 = (int)((double)vector.Y - num * 0.5);
				int value6 = (int)((double)vector.Y + num * 0.5);
				int num3 = Utils.Clamp(value3, 10, Main.maxTilesX - 10);
				value4 = Utils.Clamp(value4, 10, Main.maxTilesX - 10);
				value5 = Utils.Clamp(value5, 10, Main.maxTilesY - 10);
				value6 = Utils.Clamp(value6, 10, Main.maxTilesY - 10);
				for (int k = num3; k < value4; k++)
				{
					for (int l = value5; l < value6; l++)
					{
						if ((double)(Math.Abs((float)k - vector.X) + Math.Abs((float)l - vector.Y)) < num * 0.5 * (1.0 + (double)GenBase._random.Next(-10, 11) * 0.015))
						{
							WorldGen.KillTile(k, l);
						}
					}
				}
				num2++;
				if (num2 > 10 && GenBase._random.Next(50) < num2)
				{
					num2 = 0;
					int num4 = -2;
					if (GenBase._random.Next(2) == 0)
					{
						num4 = 2;
					}
					WorldGen.TileRunner((int)vector.X, (int)vector.Y, GenBase._random.Next(3, 20), GenBase._random.Next(10, 100), -1, addTile: false, num4);
				}
				vector += vector2;
				vector2.Y += (float)GenBase._random.Next(-10, 11) * 0.01f;
				if (vector2.Y > 0f)
				{
					vector2.Y = 0f;
				}
				if (vector2.Y < -2f)
				{
					vector2.Y = -2f;
				}
				vector2.X += (float)GenBase._random.Next(-10, 11) * 0.1f;
				if (vector.X < (float)(i - 200))
				{
					vector2.X += (float)GenBase._random.Next(5, 21) * 0.1f;
				}
				if (vector.X > (float)(i + 200))
				{
					vector2.X -= (float)GenBase._random.Next(5, 21) * 0.1f;
				}
				if ((double)vector2.X > 1.5)
				{
					vector2.X = 1.5f;
				}
				if ((double)vector2.X < -1.5)
				{
					vector2.X = -1.5f;
				}
			}
		}

		private void GenerateHolesInMudWalls()
		{
			for (int i = 0; i < Main.maxTilesX / 4; i++)
			{
				int num = GenBase._random.Next(20, Main.maxTilesX - 20);
				int num2 = GenBase._random.Next((int)WorldSurface + 10, Main.UnderworldLayer);
				while (Main.tile[num, num2].wall != 64 && Main.tile[num, num2].wall != 15)
				{
					num = GenBase._random.Next(20, Main.maxTilesX - 20);
					num2 = GenBase._random.Next((int)WorldSurface + 10, Main.UnderworldLayer);
				}
				WorldGen.MudWallRunner(num, num2);
			}
		}

		private void GenerateFinishingTouches(GenerationProgress progress, int oldX, int oldY)
		{
			int num = oldX;
			int num2 = oldY;
			float worldScale = _worldScale;
			for (int i = 0; (float)i <= 20f * worldScale; i++)
			{
				progress.Set((60f + (float)i / worldScale) * 0.01f);
				num += GenBase._random.Next((int)(-5f * worldScale), (int)(6f * worldScale));
				num2 += GenBase._random.Next((int)(-5f * worldScale), (int)(6f * worldScale));
				WorldGen.TileRunner(num, num2, GenBase._random.Next(40, 100), GenBase._random.Next(300, 500), 59);
			}
			for (int j = 0; (float)j <= 10f * worldScale; j++)
			{
				progress.Set((80f + (float)j / worldScale * 2f) * 0.01f);
				num = oldX + GenBase._random.Next((int)(-600f * worldScale), (int)(600f * worldScale));
				num2 = oldY + GenBase._random.Next((int)(-200f * worldScale), (int)(200f * worldScale));
				while (num < 1 || num >= Main.maxTilesX - 1 || num2 < 1 || num2 >= Main.maxTilesY - 1 || Main.tile[num, num2].type != 59)
				{
					num = oldX + GenBase._random.Next((int)(-600f * worldScale), (int)(600f * worldScale));
					num2 = oldY + GenBase._random.Next((int)(-200f * worldScale), (int)(200f * worldScale));
				}
				for (int k = 0; (float)k < 8f * worldScale; k++)
				{
					num += GenBase._random.Next(-30, 31);
					num2 += GenBase._random.Next(-30, 31);
					int type = -1;
					if (GenBase._random.Next(7) == 0)
					{
						type = -2;
					}
					WorldGen.TileRunner(num, num2, GenBase._random.Next(10, 20), GenBase._random.Next(30, 70), type);
				}
			}
			for (int l = 0; (float)l <= 300f * worldScale; l++)
			{
				num = oldX + GenBase._random.Next((int)(-600f * worldScale), (int)(600f * worldScale));
				num2 = oldY + GenBase._random.Next((int)(-200f * worldScale), (int)(200f * worldScale));
				while (num < 1 || num >= Main.maxTilesX - 1 || num2 < 1 || num2 >= Main.maxTilesY - 1 || Main.tile[num, num2].type != 59)
				{
					num = oldX + GenBase._random.Next((int)(-600f * worldScale), (int)(600f * worldScale));
					num2 = oldY + GenBase._random.Next((int)(-200f * worldScale), (int)(200f * worldScale));
				}
				WorldGen.TileRunner(num, num2, GenBase._random.Next(4, 10), GenBase._random.Next(5, 30), 1);
				if (GenBase._random.Next(4) == 0)
				{
					int type2 = GenBase._random.Next(63, 69);
					WorldGen.TileRunner(num + GenBase._random.Next(-1, 2), num2 + GenBase._random.Next(-1, 2), GenBase._random.Next(3, 7), GenBase._random.Next(4, 8), type2);
				}
			}
		}
	}
}
