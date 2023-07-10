using System;
using Microsoft.Xna.Framework;
using GameManager.Utilities;
using GameManager.WorldBuilding;

namespace GameManager.GameContent.Biomes
{
	public class HiveBiome : MicroBiome
	{
		public override bool Place(Point origin, StructureMap structures)
		{
			if (!structures.CanPlace(new Rectangle(origin.X - 50, origin.Y - 50, 100, 100)))
			{
				return false;
			}
			if (TooCloseToImportantLocations(origin))
			{
				return false;
			}
			Ref<int> @ref = new Ref<int>(0);
			Ref<int> ref2 = new Ref<int>(0);
			Ref<int> ref3 = new Ref<int>(0);
			WorldUtils.Gen(origin, new Shapes.Circle(15), Actions.Chain(new Modifiers.IsSolid(), new Actions.Scanner(@ref), new Modifiers.OnlyTiles(60, 59), new Actions.Scanner(ref2), new Modifiers.OnlyTiles(60), new Actions.Scanner(ref3)));
			if ((float)ref2.Value / (float)@ref.Value < 0.75f || ref3.Value < 2)
			{
				return false;
			}
			int num = 0;
			int[] array = new int[1000];
			int[] array2 = new int[1000];
			Vector2 vector = origin.ToVector2();
			int num2 = WorldGen.genRand.Next(2, 5);
			if (WorldGen.drunkWorldGen)
			{
				num2 += WorldGen.genRand.Next(7, 10);
			}
			for (int i = 0; i < num2; i++)
			{
				Vector2 vector2 = vector;
				int num3 = WorldGen.genRand.Next(2, 5);
				for (int j = 0; j < num3; j++)
				{
					vector2 = CreateHiveTunnel((int)vector.X, (int)vector.Y, WorldGen.genRand);
				}
				vector = vector2;
				array[num] = (int)vector.X;
				array2[num] = (int)vector.Y;
				num++;
			}
			FrameOutAllHiveContents(origin, 50);
			for (int k = 0; k < num; k++)
			{
				int num4 = array[k];
				int y = array2[k];
				int num5 = 1;
				if (WorldGen.genRand.Next(2) == 0)
				{
					num5 = -1;
				}
				bool flag = false;
				while (WorldGen.InWorld(num4, y, 10) && BadSpotForHoneyFall(num4, y))
				{
					num4 += num5;
					if (Math.Abs(num4 - array[k]) > 50)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					num4 += num5;
					if (!SpotActuallyNotInHive(num4, y))
					{
						CreateBlockedHoneyCube(num4, y);
						CreateDentForHoneyFall(num4, y, num5);
					}
				}
			}
			CreateStandForLarva(vector);
			if (WorldGen.drunkWorldGen)
			{
				for (int l = 0; l < 1000; l++)
				{
					Vector2 vector3 = vector;
					vector3.X += WorldGen.genRand.Next(-50, 51);
					vector3.Y += WorldGen.genRand.Next(-50, 51);
					if (WorldGen.InWorld((int)vector3.X, (int)vector3.Y) && Vector2.Distance(vector, vector3) > 10f && !Main.tile[(int)vector3.X, (int)vector3.Y].active() && Main.tile[(int)vector3.X, (int)vector3.Y].wall == 86)
					{
						CreateStandForLarva(vector3);
						break;
					}
				}
			}
			structures.AddProtectedStructure(new Rectangle(origin.X - 50, origin.Y - 50, 100, 100), 5);
			return true;
		}

		private static void FrameOutAllHiveContents(Point origin, int squareHalfWidth)
		{
			int num = Math.Max(10, origin.X - squareHalfWidth);
			int num2 = Math.Min(Main.maxTilesX - 10, origin.X + squareHalfWidth);
			int num3 = Math.Max(10, origin.Y - squareHalfWidth);
			int num4 = Math.Min(Main.maxTilesY - 10, origin.Y + squareHalfWidth);
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile.active() && tile.type == 225)
					{
						WorldGen.SquareTileFrame(i, j);
					}
					if (tile.wall == 86)
					{
						WorldGen.SquareWallFrame(i, j);
					}
				}
			}
		}

		private static Vector2 CreateHiveTunnel(int i, int j, UnifiedRandom random)
		{
			double num = random.Next(12, 21);
			float num2 = random.Next(10, 21);
			if (WorldGen.drunkWorldGen)
			{
				num = random.Next(8, 26);
				num2 = random.Next(10, 41);
				float num3 = Main.maxTilesX / 4200;
				num3 = (num3 + 1f) / 2f;
				num *= (double)num3;
				num2 *= num3;
			}
			double num4 = num;
			Vector2 result = default(Vector2);
			result.X = i;
			result.Y = j;
			Vector2 vector = default(Vector2);
			vector.X = (float)random.Next(-10, 11) * 0.2f;
			vector.Y = (float)random.Next(-10, 11) * 0.2f;
			while (num > 0.0 && num2 > 0f)
			{
				if (result.Y > (float)(Main.maxTilesY - 250))
				{
					num2 = 0f;
				}
				num = num4 * (double)(1f + (float)random.Next(-20, 20) * 0.01f);
				num2 -= 1f;
				int num5 = (int)((double)result.X - num);
				int num6 = (int)((double)result.X + num);
				int num7 = (int)((double)result.Y - num);
				int num8 = (int)((double)result.Y + num);
				if (num5 < 1)
				{
					num5 = 1;
				}
				if (num6 > Main.maxTilesX - 1)
				{
					num6 = Main.maxTilesX - 1;
				}
				if (num7 < 1)
				{
					num7 = 1;
				}
				if (num8 > Main.maxTilesY - 1)
				{
					num8 = Main.maxTilesY - 1;
				}
				for (int k = num5; k < num6; k++)
				{
					for (int l = num7; l < num8; l++)
					{
						if (!WorldGen.InWorld(k, l, 50))
						{
							num2 = 0f;
						}
						else
						{
							if (Main.tile[k - 10, l].wall == 87)
							{
								num2 = 0f;
							}
							if (Main.tile[k + 10, l].wall == 87)
							{
								num2 = 0f;
							}
							if (Main.tile[k, l - 10].wall == 87)
							{
								num2 = 0f;
							}
							if (Main.tile[k, l + 10].wall == 87)
							{
								num2 = 0f;
							}
						}
						if ((double)l < Main.worldSurface && Main.tile[k, l - 5].wall == 0)
						{
							num2 = 0f;
						}
						float num9 = Math.Abs((float)k - result.X);
						float num10 = Math.Abs((float)l - result.Y);
						double num11 = Math.Sqrt(num9 * num9 + num10 * num10);
						if (num11 < num4 * 0.4 * (1.0 + (double)random.Next(-10, 11) * 0.005))
						{
							if (random.Next(3) == 0)
							{
								Main.tile[k, l].liquid = byte.MaxValue;
							}
							if (WorldGen.drunkWorldGen)
							{
								Main.tile[k, l].liquid = byte.MaxValue;
							}
							Main.tile[k, l].honey(honey: true);
							Main.tile[k, l].wall = 86;
							Main.tile[k, l].active(active: false);
							Main.tile[k, l].halfBrick(halfBrick: false);
							Main.tile[k, l].slope(0);
						}
						else if (num11 < num4 * 0.75 * (1.0 + (double)random.Next(-10, 11) * 0.005))
						{
							Main.tile[k, l].liquid = 0;
							if (Main.tile[k, l].wall != 86)
							{
								Main.tile[k, l].active(active: true);
								Main.tile[k, l].halfBrick(halfBrick: false);
								Main.tile[k, l].slope(0);
								Main.tile[k, l].type = 225;
							}
						}
						if (num11 < num4 * 0.6 * (1.0 + (double)random.Next(-10, 11) * 0.005))
						{
							Main.tile[k, l].wall = 86;
							if (WorldGen.drunkWorldGen && random.Next(2) == 0)
							{
								Main.tile[k, l].liquid = byte.MaxValue;
								Main.tile[k, l].honey(honey: true);
							}
						}
					}
				}
				result += vector;
				num2 -= 1f;
				vector.Y += (float)random.Next(-10, 11) * 0.05f;
				vector.X += (float)random.Next(-10, 11) * 0.05f;
			}
			return result;
		}

		private static bool TooCloseToImportantLocations(Point origin)
		{
			int x = origin.X;
			int y = origin.Y;
			int num = 150;
			for (int i = x - num; i < x + num; i += 10)
			{
				if (i <= 0 || i > Main.maxTilesX - 1)
				{
					continue;
				}
				for (int j = y - num; j < y + num; j += 10)
				{
					if (j > 0 && j <= Main.maxTilesY - 1)
					{
						if (Main.tile[i, j].active() && Main.tile[i, j].type == 226)
						{
							return true;
						}
						if (Main.tile[i, j].wall == 83 || Main.tile[i, j].wall == 3 || Main.tile[i, j].wall == 87)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		private static void CreateDentForHoneyFall(int x, int y, int dir)
		{
			dir *= -1;
			y++;
			int num = 0;
			while ((num < 4 || WorldGen.SolidTile(x, y)) && x > 10 && x < Main.maxTilesX - 10)
			{
				num++;
				x += dir;
				if (WorldGen.SolidTile(x, y))
				{
					WorldGen.PoundTile(x, y);
					if (!Main.tile[x, y + 1].active())
					{
						Main.tile[x, y + 1].active(active: true);
						Main.tile[x, y + 1].type = 225;
					}
				}
			}
		}

		private static void CreateBlockedHoneyCube(int x, int y)
		{
			for (int i = x - 1; i <= x + 2; i++)
			{
				for (int j = y - 1; j <= y + 2; j++)
				{
					if (i >= x && i <= x + 1 && j >= y && j <= y + 1)
					{
						Main.tile[i, j].active(active: false);
						Main.tile[i, j].liquid = byte.MaxValue;
						Main.tile[i, j].honey(honey: true);
					}
					else
					{
						Main.tile[i, j].active(active: true);
						Main.tile[i, j].type = 225;
					}
				}
			}
		}

		private static bool SpotActuallyNotInHive(int x, int y)
		{
			for (int i = x - 1; i <= x + 2; i++)
			{
				for (int j = y - 1; j <= y + 2; j++)
				{
					if (i < 10 || i > Main.maxTilesX - 10)
					{
						return true;
					}
					if (Main.tile[i, j].active() && Main.tile[i, j].type != 225)
					{
						return true;
					}
				}
			}
			return false;
		}

		private static bool BadSpotForHoneyFall(int x, int y)
		{
			if (Main.tile[x, y].active() && Main.tile[x, y + 1].active() && Main.tile[x + 1, y].active())
			{
				return !Main.tile[x + 1, y + 1].active();
			}
			return true;
		}

		public static void CreateStandForLarva(Vector2 position)
		{
			WorldGen.larvaX[WorldGen.numLarva] = Utils.Clamp((int)position.X, 5, Main.maxTilesX - 5);
			WorldGen.larvaY[WorldGen.numLarva] = Utils.Clamp((int)position.Y, 5, Main.maxTilesY - 5);
			WorldGen.numLarva++;
			if (WorldGen.numLarva >= WorldGen.larvaX.Length)
			{
				WorldGen.numLarva = WorldGen.larvaX.Length - 1;
			}
			int num = (int)position.X;
			int num2 = (int)position.Y;
			for (int i = num - 1; i <= num + 1 && i > 0 && i < Main.maxTilesX; i++)
			{
				for (int j = num2 - 2; j <= num2 + 1 && j > 0 && j < Main.maxTilesY; j++)
				{
					if (j != num2 + 1)
					{
						Main.tile[i, j].active(active: false);
						continue;
					}
					Main.tile[i, j].active(active: true);
					Main.tile[i, j].type = 225;
					Main.tile[i, j].slope(0);
					Main.tile[i, j].halfBrick(halfBrick: false);
				}
			}
		}
	}
}
