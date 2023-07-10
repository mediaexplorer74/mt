using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameManager.ID;
using GameManager.WorldBuilding;

namespace GameManager
{
	public class SceneMetrics
	{
		public static int CorruptionTileThreshold = 300;

		public static int CorruptionTileMax = 1000;

		public static int CrimsonTileThreshold = 300;

		public static int CrimsonTileMax = 1000;

		public static int HallowTileThreshold = 125;

		public static int HallowTileMax = 600;

		public static int JungleTileThreshold = 140;

		public static int JungleTileMax = 700;

		public static int SnowTileThreshold = 1500;

		public static int SnowTileMax = 6000;

		public static int DesertTileThreshold = 1500;

		public static int MushroomTileThreshold = 100;

		public static int MushroomTileMax = 200;

		public static int MeteorTileThreshold = 75;

		public static int GraveyardTileMax = 32;

		public static int GraveyardTileMin = 12;

		public static int GraveyardTileThreshold = 24;

		public bool[] NPCBannerBuff = new bool[289];

		public bool hasBanner;

		private readonly int[] _tileCounts = new int[623];

		private readonly World _world;

		private readonly List<Point> _oreFinderTileLocations = new List<Point>(512);

		public int bestOre;

		public Point? ClosestOrePosition
		{
			get;
			private set;
		}

		public int EvilTileCount
		{
			get;
			set;
		}

		public int HolyTileCount
		{
			get;
			set;
		}

		public int HoneyBlockCount
		{
			get;
			set;
		}

		public int ActiveMusicBox
		{
			get;
			set;
		}

		public int SandTileCount
		{
			get;
			private set;
		}

		public int MushroomTileCount
		{
			get;
			private set;
		}

		public int SnowTileCount
		{
			get;
			private set;
		}

		public int WaterCandleCount
		{
			get;
			private set;
		}

		public int PeaceCandleCount
		{
			get;
			private set;
		}

		public int PartyMonolithCount
		{
			get;
			private set;
		}

		public int MeteorTileCount
		{
			get;
			private set;
		}

		public int BloodTileCount
		{
			get;
			private set;
		}

		public int JungleTileCount
		{
			get;
			private set;
		}

		public int DungeonTileCount
		{
			get;
			private set;
		}

		public bool HasSunflower
		{
			get;
			private set;
		}

		public bool HasGardenGnome
		{
			get;
			private set;
		}

		public bool HasClock
		{
			get;
			private set;
		}

		public bool HasCampfire
		{
			get;
			private set;
		}

		public bool HasStarInBottle
		{
			get;
			private set;
		}

		public bool HasHeartLantern
		{
			get;
			private set;
		}

		public int ActiveFountainColor
		{
			get;
			private set;
		}

		public int ActiveMonolithType
		{
			get;
			private set;
		}

		public bool BloodMoonMonolith
		{
			get;
			private set;
		}

		public bool MoonLordMonolith
		{
			get;
			private set;
		}

		public bool HasCatBast
		{
			get;
			private set;
		}

		public int GraveyardTileCount
		{
			get;
			private set;
		}

		public bool EnoughTilesForJungle => JungleTileCount >= JungleTileThreshold;

		public bool EnoughTilesForHallow => HolyTileCount >= HallowTileThreshold;

		public bool EnoughTilesForSnow => SnowTileCount >= SnowTileThreshold;

		public bool EnoughTilesForGlowingMushroom => MushroomTileCount >= MushroomTileThreshold;

		public bool EnoughTilesForDesert => SandTileCount >= DesertTileThreshold;

		public bool EnoughTilesForCorruption => EvilTileCount >= CorruptionTileThreshold;

		public bool EnoughTilesForCrimson => BloodTileCount >= CrimsonTileThreshold;

		public bool EnoughTilesForMeteor => MeteorTileCount >= MeteorTileThreshold;

		public bool EnoughTilesForGraveyard => GraveyardTileCount >= GraveyardTileThreshold;

		public SceneMetrics(World world)
		{
			_world = world;
			Reset();
		}

		public void ScanAndExportToMain(SceneMetricsScanSettings settings)
		{
			Reset();
			int num = 0;
			int num2 = 0;
			if (settings.ScanOreFinderData)
			{
				_oreFinderTileLocations.Clear();
			}
			if (settings.BiomeScanCenterPositionInWorld.HasValue)
			{
				Point point = settings.BiomeScanCenterPositionInWorld.Value.ToTileCoordinates();
				Rectangle rectangle = WorldUtils.ClampToWorld(tileRectangle: new Rectangle(point.X - Main.buffScanAreaWidth / 2, point.Y - Main.buffScanAreaHeight / 2, Main.buffScanAreaWidth, Main.buffScanAreaHeight), world: _world);
				for (int i = rectangle.Left; i < rectangle.Right; i++)
				{
					for (int j = rectangle.Top; j < rectangle.Bottom; j++)
					{
						if (!rectangle.Contains(i, j))
						{
							continue;
						}
						Tile tile = _world.Tiles[i, j];
						if (tile == null || !tile.active())
						{
							continue;
						}
						rectangle.Contains(i, j);
						if (!TileID.Sets.isDesertBiomeSand[tile.type] || !WorldGen.oceanDepths(i, j))
						{
							_tileCounts[tile.type]++;
						}
						if (tile.type == 215 && tile.frameY < 36)
						{
							HasCampfire = true;
						}
						if (tile.type == 49 && tile.frameX < 18)
						{
							num++;
						}
						if (tile.type == 372 && tile.frameX < 18)
						{
							num2++;
						}
						if (tile.type == 405 && tile.frameX < 54)
						{
							HasCampfire = true;
						}
						if (tile.type == 506 && tile.frameX < 72)
						{
							HasCatBast = true;
						}
						if (tile.type == 42 && tile.frameY >= 324 && tile.frameY <= 358)
						{
							HasHeartLantern = true;
						}
						if (tile.type == 42 && tile.frameY >= 252 && tile.frameY <= 286)
						{
							HasStarInBottle = true;
						}
						if (tile.type == 91 && (tile.frameX >= 396 || tile.frameY >= 54))
						{
							int num3 = tile.frameX / 18 - 21;
							for (int num4 = tile.frameY; num4 >= 54; num4 -= 54)
							{
								num3 += 90;
								num3 += 21;
							}
							int num5 = Item.BannerToItem(num3);
							if (ItemID.Sets.BannerStrength[num5].Enabled)
							{
								NPCBannerBuff[num3] = true;
								hasBanner = true;
							}
						}
						if (settings.ScanOreFinderData && Main.tileOreFinderPriority[tile.type] != 0)
						{
							_oreFinderTileLocations.Add(new Point(i, j));
						}
					}
				}
			}
			if (settings.VisualScanArea.HasValue)
			{
				Rectangle rectangle2 = WorldUtils.ClampToWorld(_world, settings.VisualScanArea.Value);
				for (int k = rectangle2.Left; k < rectangle2.Right; k++)
				{
					for (int l = rectangle2.Top; l < rectangle2.Bottom; l++)
					{
						Tile tile2 = _world.Tiles[k, l];
						if (tile2 == null || !tile2.active())
						{
							continue;
						}
						if (tile2.type == 104)
						{
							HasClock = true;
						}
						switch (tile2.type)
						{
						case 139:
							if (tile2.frameX >= 36)
							{
								ActiveMusicBox = tile2.frameY / 36;
							}
							break;
						case 207:
							if (tile2.frameY >= 72)
							{
								switch (tile2.frameX / 36)
								{
								case 0:
									ActiveFountainColor = 0;
									break;
								case 1:
									ActiveFountainColor = 12;
									break;
								case 2:
									ActiveFountainColor = 3;
									break;
								case 3:
									ActiveFountainColor = 5;
									break;
								case 4:
									ActiveFountainColor = 2;
									break;
								case 5:
									ActiveFountainColor = 10;
									break;
								case 6:
									ActiveFountainColor = 4;
									break;
								case 7:
									ActiveFountainColor = 9;
									break;
								case 8:
									ActiveFountainColor = 8;
									break;
								case 9:
									ActiveFountainColor = 6;
									break;
								default:
									ActiveFountainColor = -1;
									break;
								}
							}
							break;
						case 410:
							if (tile2.frameY >= 56)
							{
								int num7 = (ActiveMonolithType = tile2.frameX / 36);
							}
							break;
						case 509:
							if (tile2.frameY >= 56)
							{
								ActiveMonolithType = 4;
							}
							break;
						case 480:
							if (tile2.frameY >= 54)
							{
								BloodMoonMonolith = true;
							}
							break;
						}
					}
				}
			}
			WaterCandleCount = num;
			PeaceCandleCount = num2;
			ExportTileCountsToMain();
			if (settings.ScanOreFinderData)
			{
				UpdateOreFinderData();
			}
		}

		private void ExportTileCountsToMain()
		{
			if (_tileCounts[27] > 0)
			{
				HasSunflower = true;
			}
			if (_tileCounts[567] > 0)
			{
				HasGardenGnome = true;
			}
			HoneyBlockCount = _tileCounts[229];
			HolyTileCount = _tileCounts[109] + _tileCounts[492] + _tileCounts[110] + _tileCounts[113] + _tileCounts[117] + _tileCounts[116] + _tileCounts[164] + _tileCounts[403] + _tileCounts[402];
			EvilTileCount = _tileCounts[23] + _tileCounts[24] + _tileCounts[25] + _tileCounts[32] + _tileCounts[112] + _tileCounts[163] + _tileCounts[400] + _tileCounts[398] + -10 * _tileCounts[27];
			BloodTileCount = _tileCounts[199] + _tileCounts[203] + _tileCounts[200] + _tileCounts[401] + _tileCounts[399] + _tileCounts[234] + _tileCounts[352] - 10 * _tileCounts[27];
			SnowTileCount = _tileCounts[147] + _tileCounts[148] + _tileCounts[161] + _tileCounts[162] + _tileCounts[164] + _tileCounts[163] + _tileCounts[200];
			JungleTileCount = _tileCounts[60] + _tileCounts[61] + _tileCounts[62] + _tileCounts[74] + _tileCounts[226] + _tileCounts[225];
			MushroomTileCount = _tileCounts[70] + _tileCounts[71] + _tileCounts[72] + _tileCounts[528];
			MeteorTileCount = _tileCounts[37];
			DungeonTileCount = _tileCounts[41] + _tileCounts[43] + _tileCounts[44] + _tileCounts[481] + _tileCounts[482] + _tileCounts[483];
			SandTileCount = _tileCounts[53] + _tileCounts[112] + _tileCounts[116] + _tileCounts[234] + _tileCounts[397] + _tileCounts[398] + _tileCounts[402] + _tileCounts[399] + _tileCounts[396] + _tileCounts[400] + _tileCounts[403] + _tileCounts[401];
			PartyMonolithCount = _tileCounts[455];
			GraveyardTileCount = _tileCounts[85];
			GraveyardTileCount -= _tileCounts[27] / 2;
			if (_tileCounts[27] > 0)
			{
				HasSunflower = true;
			}
			if (GraveyardTileCount > GraveyardTileMin)
			{
				HasSunflower = false;
			}
			if (GraveyardTileCount < 0)
			{
				GraveyardTileCount = 0;
			}
			if (HolyTileCount < 0)
			{
				HolyTileCount = 0;
			}
			if (EvilTileCount < 0)
			{
				EvilTileCount = 0;
			}
			if (BloodTileCount < 0)
			{
				BloodTileCount = 0;
			}
			int holyTileCount = HolyTileCount;
			HolyTileCount -= EvilTileCount;
			HolyTileCount -= BloodTileCount;
			EvilTileCount -= holyTileCount;
			BloodTileCount -= holyTileCount;
			if (HolyTileCount < 0)
			{
				HolyTileCount = 0;
			}
			if (EvilTileCount < 0)
			{
				EvilTileCount = 0;
			}
			if (BloodTileCount < 0)
			{
				BloodTileCount = 0;
			}
		}

		public int GetTileCount(ushort tileId)
		{
			return _tileCounts[tileId];
		}

		public void Reset()
		{
			Array.Clear(_tileCounts, 0, _tileCounts.Length);
			SandTileCount = 0;
			EvilTileCount = 0;
			BloodTileCount = 0;
			GraveyardTileCount = 0;
			MushroomTileCount = 0;
			SnowTileCount = 0;
			HolyTileCount = 0;
			MeteorTileCount = 0;
			JungleTileCount = 0;
			DungeonTileCount = 0;
			HasCampfire = false;
			HasSunflower = false;
			HasGardenGnome = false;
			HasStarInBottle = false;
			HasHeartLantern = false;
			HasClock = false;
			HasCatBast = false;
			ActiveMusicBox = -1;
			WaterCandleCount = 0;
			ActiveFountainColor = -1;
			ActiveMonolithType = -1;
			bestOre = -1;
			BloodMoonMonolith = false;
			MoonLordMonolith = false;
			Array.Clear(NPCBannerBuff, 0, NPCBannerBuff.Length);
			hasBanner = false;
		}

		private void UpdateOreFinderData()
		{
			int num = -1;
			foreach (Point oreFinderTileLocation in _oreFinderTileLocations)
			{
				Tile tile = _world.Tiles[oreFinderTileLocation.X, oreFinderTileLocation.Y];
				if (IsValidForOreFinder(tile) && (num < 0 || Main.tileOreFinderPriority[tile.type] > Main.tileOreFinderPriority[num]))
				{
					num = tile.type;
					ClosestOrePosition = oreFinderTileLocation;
				}
			}
			bestOre = num;
		}

		public static bool IsValidForOreFinder(Tile t)
		{
			if (t.type == 227 && (t.frameX < 272 || t.frameX > 374))
			{
				return false;
			}
			return Main.tileOreFinderPriority[t.type] > 0;
		}
	}
}
