using System;
using GameManager.IO;
using GameManager.WorldBuilding;

namespace GameManager.GameContent.Biomes
{
	public class TerrainPass : GenPass
	{
		private enum TerrainFeatureType
		{
			Plateau,
			Hill,
			Dale,
			Mountain,
			Valley
		}

		private class SurfaceHistory
		{
			private readonly double[] _heights;

			private int _index;

			public double this[int index]
			{
				get
				{
					return _heights[(index + _index) % _heights.Length];
				}
				set
				{
					_heights[(index + _index) % _heights.Length] = value;
				}
			}

			public int Length => _heights.Length;

			public SurfaceHistory(int size)
			{
				_heights = new double[size];
			}

			public void Record(double height)
			{
				_heights[_index] = height;
				_index = (_index + 1) % _heights.Length;
			}
		}

		public double WorldSurface
		{
			get;
			private set;
		}

		public double WorldSurfaceHigh
		{
			get;
			private set;
		}

		public double WorldSurfaceLow
		{
			get;
			private set;
		}

		public double RockLayer
		{
			get;
			private set;
		}

		public double RockLayerHigh
		{
			get;
			private set;
		}

		public double RockLayerLow
		{
			get;
			private set;
		}

		public int WaterLine
		{
			get;
			private set;
		}

		public int LavaLine
		{
			get;
			private set;
		}

		public int LeftBeachSize
		{
			get;
			set;
		}

		public int RightBeachSize
		{
			get;
			set;
		}

		public TerrainPass()
			: base("Terrain", 449.3722f)
		{
		}

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			int num = configuration.Get<int>("FlatBeachPadding");
			progress.Message = Lang.gen[0].Value;
			TerrainFeatureType terrainFeatureType = TerrainFeatureType.Plateau;
			int num2 = 0;
			double num3 = (double)Main.maxTilesY * 0.3;
			num3 *= (double)GenBase._random.Next(90, 110) * 0.005;
			double num4 = num3 + (double)Main.maxTilesY * 0.2;
			num4 *= (double)GenBase._random.Next(90, 110) * 0.01;
			double num5 = num3;
			double num6 = num3;
			double num7 = num4;
			double num8 = num4;
			double num9 = (double)Main.maxTilesY * 0.23;
			SurfaceHistory surfaceHistory = new SurfaceHistory(500);
			num2 = LeftBeachSize + num;
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				progress.Set((float)i / (float)Main.maxTilesX);
				num5 = Math.Min(num3, num5);
				num6 = Math.Max(num3, num6);
				num7 = Math.Min(num4, num7);
				num8 = Math.Max(num4, num8);
				if (num2 <= 0)
				{
					terrainFeatureType = (TerrainFeatureType)GenBase._random.Next(0, 5);
					num2 = GenBase._random.Next(5, 40);
					if (terrainFeatureType == TerrainFeatureType.Plateau)
					{
						num2 *= (int)((double)GenBase._random.Next(5, 30) * 0.2);
					}
				}
				num2--;
				if ((double)i > (double)Main.maxTilesX * 0.45 && (double)i < (double)Main.maxTilesX * 0.55 && (terrainFeatureType == TerrainFeatureType.Mountain || terrainFeatureType == TerrainFeatureType.Valley))
				{
					terrainFeatureType = (TerrainFeatureType)GenBase._random.Next(3);
				}
				if ((double)i > (double)Main.maxTilesX * 0.48 && (double)i < (double)Main.maxTilesX * 0.52)
				{
					terrainFeatureType = TerrainFeatureType.Plateau;
				}
				num3 += GenerateWorldSurfaceOffset(terrainFeatureType);
				float num10 = 0.17f;
				float num11 = 0.26f;
				if (WorldGen.drunkWorldGen)
				{
					num10 = 0.15f;
					num11 = 0.28f;
				}
				if (i < LeftBeachSize + num || i > Main.maxTilesX - RightBeachSize - num)
				{
					num3 = Utils.Clamp(num3, (double)Main.maxTilesY * 0.17, num9);
				}
				else if (num3 < (double)((float)Main.maxTilesY * num10))
				{
					num3 = (float)Main.maxTilesY * num10;
					num2 = 0;
				}
				else if (num3 > (double)((float)Main.maxTilesY * num11))
				{
					num3 = (float)Main.maxTilesY * num11;
					num2 = 0;
				}
				while (GenBase._random.Next(0, 3) == 0)
				{
					num4 += (double)GenBase._random.Next(-2, 3);
				}
				if (num4 < num3 + (double)Main.maxTilesY * 0.06)
				{
					num4 += 1.0;
				}
				if (num4 > num3 + (double)Main.maxTilesY * 0.35)
				{
					num4 -= 1.0;
				}
				surfaceHistory.Record(num3);
				FillColumn(i, num3, num4);
				if (i == Main.maxTilesX - RightBeachSize - num)
				{
					if (num3 > num9)
					{
						RetargetSurfaceHistory(surfaceHistory, i, num9);
					}
					terrainFeatureType = TerrainFeatureType.Plateau;
					num2 = Main.maxTilesX - i;
				}
			}
			Main.worldSurface = (int)(num6 + 25.0);
			Main.rockLayer = num8;
			double num12 = (int)((Main.rockLayer - Main.worldSurface) / 6.0) * 6;
			Main.rockLayer = (int)(Main.worldSurface + num12);
			int num13 = (int)(Main.rockLayer + (double)Main.maxTilesY) / 2;
			num13 += GenBase._random.Next(-100, 20);
			int lavaLine = num13 + GenBase._random.Next(50, 80);
			int num14 = 20;
			if (num7 < num6 + (double)num14)
			{
				double num15 = (num7 + num6) / 2.0;
				double num16 = Math.Abs(num7 - num6);
				if (num16 < (double)num14)
				{
					num16 = num14;
				}
				num7 = num15 + num16 / 2.0;
				num6 = num15 - num16 / 2.0;
			}
			RockLayer = num4;
			RockLayerHigh = num8;
			RockLayerLow = num7;
			WorldSurface = num3;
			WorldSurfaceHigh = num6;
			WorldSurfaceLow = num5;
			WaterLine = num13;
			LavaLine = lavaLine;
		}

		private static void FillColumn(int x, double worldSurface, double rockLayer)
		{
			for (int i = 0; (double)i < worldSurface; i++)
			{
				Main.tile[x, i].active(active: false);
				Main.tile[x, i].frameX = -1;
				Main.tile[x, i].frameY = -1;
			}
			for (int j = (int)worldSurface; j < Main.maxTilesY; j++)
			{
				if ((double)j < rockLayer)
				{
					Main.tile[x, j].active(active: true);
					Main.tile[x, j].type = 0;
					Main.tile[x, j].frameX = -1;
					Main.tile[x, j].frameY = -1;
				}
				else
				{
					Main.tile[x, j].active(active: true);
					Main.tile[x, j].type = 1;
					Main.tile[x, j].frameX = -1;
					Main.tile[x, j].frameY = -1;
				}
			}
		}

		private static void RetargetColumn(int x, double worldSurface)
		{
			for (int i = 0; (double)i < worldSurface; i++)
			{
				Main.tile[x, i].active(active: false);
				Main.tile[x, i].frameX = -1;
				Main.tile[x, i].frameY = -1;
			}
			for (int j = (int)worldSurface; j < Main.maxTilesY; j++)
			{
				if (Main.tile[x, j].type != 1 || !Main.tile[x, j].active())
				{
					Main.tile[x, j].active(active: true);
					Main.tile[x, j].type = 0;
					Main.tile[x, j].frameX = -1;
					Main.tile[x, j].frameY = -1;
				}
			}
		}

		private static double GenerateWorldSurfaceOffset(TerrainFeatureType featureType)
		{
			double num = 0.0;
			if ((WorldGen.drunkWorldGen || WorldGen.getGoodWorldGen) && WorldGen.genRand.Next(2) == 0)
			{
				switch (featureType)
				{
				case TerrainFeatureType.Plateau:
					while (GenBase._random.Next(0, 6) == 0)
					{
						num += (double)GenBase._random.Next(-1, 2);
					}
					break;
				case TerrainFeatureType.Hill:
					while (GenBase._random.Next(0, 3) == 0)
					{
						num -= 1.0;
					}
					while (GenBase._random.Next(0, 10) == 0)
					{
						num += 1.0;
					}
					break;
				case TerrainFeatureType.Dale:
					while (GenBase._random.Next(0, 3) == 0)
					{
						num += 1.0;
					}
					while (GenBase._random.Next(0, 10) == 0)
					{
						num -= 1.0;
					}
					break;
				case TerrainFeatureType.Mountain:
					while (GenBase._random.Next(0, 3) != 0)
					{
						num -= 1.0;
					}
					while (GenBase._random.Next(0, 6) == 0)
					{
						num += 1.0;
					}
					break;
				case TerrainFeatureType.Valley:
					while (GenBase._random.Next(0, 3) != 0)
					{
						num += 1.0;
					}
					while (GenBase._random.Next(0, 5) == 0)
					{
						num -= 1.0;
					}
					break;
				}
			}
			else
			{
				switch (featureType)
				{
				case TerrainFeatureType.Plateau:
					while (GenBase._random.Next(0, 7) == 0)
					{
						num += (double)GenBase._random.Next(-1, 2);
					}
					break;
				case TerrainFeatureType.Hill:
					while (GenBase._random.Next(0, 4) == 0)
					{
						num -= 1.0;
					}
					while (GenBase._random.Next(0, 10) == 0)
					{
						num += 1.0;
					}
					break;
				case TerrainFeatureType.Dale:
					while (GenBase._random.Next(0, 4) == 0)
					{
						num += 1.0;
					}
					while (GenBase._random.Next(0, 10) == 0)
					{
						num -= 1.0;
					}
					break;
				case TerrainFeatureType.Mountain:
					while (GenBase._random.Next(0, 2) == 0)
					{
						num -= 1.0;
					}
					while (GenBase._random.Next(0, 6) == 0)
					{
						num += 1.0;
					}
					break;
				case TerrainFeatureType.Valley:
					while (GenBase._random.Next(0, 2) == 0)
					{
						num += 1.0;
					}
					while (GenBase._random.Next(0, 5) == 0)
					{
						num -= 1.0;
					}
					break;
				}
			}
			return num;
		}

		private static void RetargetSurfaceHistory(SurfaceHistory history, int targetX, double targetHeight)
		{
			for (int i = 0; i < history.Length / 2; i++)
			{
				if (history[history.Length - 1] <= targetHeight)
				{
					break;
				}
				for (int j = 0; j < history.Length - i * 2; j++)
				{
					double num = history[history.Length - j - 1];
					num -= 1.0;
					history[history.Length - j - 1] = num;
					if (num <= targetHeight)
					{
						break;
					}
				}
			}
			for (int k = 0; k < history.Length; k++)
			{
				double worldSurface = history[history.Length - k - 1];
				RetargetColumn(targetX - k, worldSurface);
			}
		}
	}
}
