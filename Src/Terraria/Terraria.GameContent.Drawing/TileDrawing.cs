using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.DataStructures;
using GameManager.GameContent.Events;
using GameManager.GameContent.Tile_Entities;
using GameManager.Graphics.Capture;
using GameManager.ID;
using GameManager.ObjectData;
using GameManager.UI;
using GameManager.Utilities;

namespace GameManager.GameContent.Drawing
{
	public class TileDrawing
	{
		private enum TileCounterType
		{
			Tree,
			DisplayDoll,
			HatRack,
			WindyGrass,
			MultiTileGrass,
			MultiTileVine,
			Vine,
			BiomeGrass,
			VoidLens,
			ReverseVine,
			TeleportationPylon,
			MasterTrophy,
			Count
		}

		private struct TileFlameData
		{
			public Texture2D flameTexture;

			public ulong flameSeed;

			public int flameCount;

			public Color flameColor;

			public int flameRangeXMin;

			public int flameRangeXMax;

			public int flameRangeYMin;

			public int flameRangeYMax;

			public float flameRangeMultX;

			public float flameRangeMultY;
		}

		private const int MAX_SPECIALS = 9000;

		private const int MAX_SPECIALS_LEGACY = 1000;

		private const float FORCE_FOR_MIN_WIND = 0.08f;

		private const float FORCE_FOR_MAX_WIND = 1.2f;

		private int _leafFrequency = 100000;

		private int[] _specialsCount = new int[12];

		private Point[][] _specialPositions = new Point[12][];

		private Dictionary<Point, int> _displayDollTileEntityPositions = new Dictionary<Point, int>();

		private Dictionary<Point, int> _hatRackTileEntityPositions = new Dictionary<Point, int>();

		private Dictionary<Point, int> _trainingDummyTileEntityPositions = new Dictionary<Point, int>();

		private Dictionary<Point, int> _itemFrameTileEntityPositions = new Dictionary<Point, int>();

		private Dictionary<Point, int> _foodPlatterTileEntityPositions = new Dictionary<Point, int>();

		private Dictionary<Point, int> _weaponRackTileEntityPositions = new Dictionary<Point, int>();

		private Dictionary<Point, int> _chestPositions = new Dictionary<Point, int>();

		private int _specialTilesCount;

		private int[] _specialTileX = new int[1000];

		private int[] _specialTileY = new int[1000];

		private UnifiedRandom _rand;

		private double _treeWindCounter;

		private double _grassWindCounter;

		private double _sunflowerWindCounter;

		private double _vineWindCounter;

		private WindGrid _windGrid = new WindGrid();

		private bool _shouldShowInvisibleBlocks;

		private List<Point> _vineRootsPositions = new List<Point>();

		private List<Point> _reverseVineRootsPositions = new List<Point>();

		private TilePaintSystemV2 _paintSystem;

		private Color _martianGlow = new Color(0, 0, 0, 0);

		private Color _meteorGlow = new Color(100, 100, 100, 0);

		private Color _lavaMossGlow = new Color(150, 100, 50, 0);

		private Color _kryptonMossGlow = new Color(0, 200, 0, 0);

		private Color _xenonMossGlow = new Color(0, 180, 250, 0);

		private Color _argonMossGlow = new Color(225, 0, 125, 0);

		private bool _isActiveAndNotPaused;

		private Player _localPlayer = new Player();

		private Color _highQualityLightingRequirement;

		private Color _mediumQualityLightingRequirement;

		private static readonly Vector2 _zero;

		private ThreadLocal<TileDrawInfo> _currentTileDrawInfo = new ThreadLocal<TileDrawInfo>(() => new TileDrawInfo());

		private TileDrawInfo _currentTileDrawInfoNonThreaded = new TileDrawInfo();

		private Vector3[] _glowPaintColorSlices = new Vector3[9]
		{
			Vector3.One,
			Vector3.One,
			Vector3.One,
			Vector3.One,
			Vector3.One,
			Vector3.One,
			Vector3.One,
			Vector3.One,
			Vector3.One
		};

		private List<DrawData> _voidLensData = new List<DrawData>();

		private bool[] _tileSolid => Main.tileSolid;

		private bool[] _tileSolidTop => Main.tileSolidTop;

		private Dust[] _dust => Main.dust;

		private Gore[] _gore => Main.gore;

		private void AddSpecialPoint(int x, int y, TileCounterType type)
		{
			_specialPositions[(int)type][_specialsCount[(int)type]++] = new Point(x, y);
		}

		public TileDrawing(TilePaintSystemV2 paintSystem)
		{
			_paintSystem = paintSystem;
			_rand = new UnifiedRandom();
			for (int i = 0; i < _specialPositions.Length; i++)
			{
				_specialPositions[i] = new Point[9000];
			}
		}

		public void PreparePaintForTilesOnScreen()
		{
			if (Main.GameUpdateCount % 6u == 0)
			{
				Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
				Vector2 value = new Vector2(Main.offScreenRange, Main.offScreenRange);
				if (Main.drawToScreen)
				{
					value = Vector2.Zero;
				}
				GetScreenDrawArea(unscaledPosition, value + (Main.Camera.UnscaledPosition - Main.Camera.ScaledPosition), out var firstTileX, out var lastTileX, out var firstTileY, out var lastTileY);
				PrepareForAreaDrawing(firstTileX, lastTileX, firstTileY, lastTileY, prepareLazily: true);
			}
		}

		public void PrepareForAreaDrawing(int firstTileX, int lastTileX, int firstTileY, int lastTileY, bool prepareLazily)
		{
			TilePaintSystemV2.TileVariationkey lookupKey = default(TilePaintSystemV2.TileVariationkey);
			TilePaintSystemV2.WallVariationKey lookupKey2 = default(TilePaintSystemV2.WallVariationKey);
			for (int i = firstTileY; i < lastTileY + 4; i++)
			{
				for (int j = firstTileX - 2; j < lastTileX + 2; j++)
				{
					Tile tile = Main.tile[j, i];
					if (tile == null)
					{
						continue;
					}
					if (tile.active())
					{
						Main.instance.LoadTiles(tile.type);
						lookupKey.TileType = tile.type;
						lookupKey.PaintColor = tile.color();
						int tileStyle = 0;
						switch (tile.type)
						{
						case 5:
							tileStyle = GetTreeBiome(j, i, tile.frameX, tile.frameY);
							break;
						case 323:
							tileStyle = GetPalmTreeBiome(j, i);
							break;
						}
						lookupKey.TileStyle = tileStyle;
						if (lookupKey.PaintColor != 0)
						{
							_paintSystem.RequestTile(lookupKey);
						}
					}
					if (tile.wall != 0)
					{
						Main.instance.LoadWall(tile.wall);
						lookupKey2.WallType = tile.wall;
						lookupKey2.PaintColor = tile.wallColor();
						if (lookupKey2.PaintColor != 0)
						{
							_paintSystem.RequestWall(lookupKey2);
						}
					}
					if (!prepareLazily)
					{
						MakeExtraPreparations(tile, j, i);
					}
				}
			}
		}

		private void MakeExtraPreparations(Tile tile, int x, int y)
		{
			TilePaintSystemV2.TreeFoliageVariantKey treeFoliageVariantKey;
			switch (tile.type)
			{
			case 5:
			{
				int treeFrame = 0;
				int floorY = 0;
				int topTextureFrameWidth = 0;
				int topTextureFrameHeight = 0;
				int treeStyle = 0;
				int xoffset = (tile.frameX == 44).ToInt() - (tile.frameX == 66).ToInt();
				if (WorldGen.GetCommonTreeFoliageData(x, y, xoffset, treeFrame, treeStyle, out floorY, out topTextureFrameWidth, out topTextureFrameHeight))
				{
					treeFoliageVariantKey = default(TilePaintSystemV2.TreeFoliageVariantKey);
					treeFoliageVariantKey.TextureIndex = treeStyle;
					treeFoliageVariantKey.PaintColor = tile.color();
					TilePaintSystemV2.TreeFoliageVariantKey lookupKey2 = treeFoliageVariantKey;
					_paintSystem.RequestTreeTop(lookupKey2);
					_paintSystem.RequestTreeBranch(lookupKey2);
				}
				break;
			}
			case 583:
			case 584:
			case 585:
			case 586:
			case 587:
			case 588:
			case 589:
			{
				int treeFrame2 = 0;
				int floorY2 = 0;
				int topTextureFrameWidth2 = 0;
				int topTextureFrameHeight2 = 0;
				int treeStyle2 = 0;
				int xoffset2 = (tile.frameX == 44).ToInt() - (tile.frameX == 66).ToInt();
				if (WorldGen.GetGemTreeFoliageData(x, y, xoffset2, treeFrame2, treeStyle2, out floorY2, out topTextureFrameWidth2, out topTextureFrameHeight2))
				{
					treeFoliageVariantKey = default(TilePaintSystemV2.TreeFoliageVariantKey);
					treeFoliageVariantKey.TextureIndex = treeStyle2;
					treeFoliageVariantKey.PaintColor = tile.color();
					TilePaintSystemV2.TreeFoliageVariantKey lookupKey3 = treeFoliageVariantKey;
					_paintSystem.RequestTreeTop(lookupKey3);
					_paintSystem.RequestTreeBranch(lookupKey3);
				}
				break;
			}
			case 596:
			case 616:
			{
				int treeFrame3 = 0;
				int floorY3 = 0;
				int topTextureFrameWidth3 = 0;
				int topTextureFrameHeight3 = 0;
				int treeStyle3 = 0;
				int xoffset3 = (tile.frameX == 44).ToInt() - (tile.frameX == 66).ToInt();
				if (WorldGen.GetVanityTreeFoliageData(x, y, xoffset3, treeFrame3, treeStyle3, out floorY3, out topTextureFrameWidth3, out topTextureFrameHeight3))
				{
					treeFoliageVariantKey = default(TilePaintSystemV2.TreeFoliageVariantKey);
					treeFoliageVariantKey.TextureIndex = treeStyle3;
					treeFoliageVariantKey.PaintColor = tile.color();
					TilePaintSystemV2.TreeFoliageVariantKey lookupKey4 = treeFoliageVariantKey;
					_paintSystem.RequestTreeTop(lookupKey4);
					_paintSystem.RequestTreeBranch(lookupKey4);
				}
				break;
			}
			case 323:
			{
				int textureIndex = 15;
				if (x >= WorldGen.beachDistance && x <= Main.maxTilesX - WorldGen.beachDistance)
				{
					textureIndex = 21;
				}
				treeFoliageVariantKey = default(TilePaintSystemV2.TreeFoliageVariantKey);
				treeFoliageVariantKey.TextureIndex = textureIndex;
				treeFoliageVariantKey.PaintColor = tile.color();
				TilePaintSystemV2.TreeFoliageVariantKey lookupKey = treeFoliageVariantKey;
				_paintSystem.RequestTreeTop(lookupKey);
				_paintSystem.RequestTreeBranch(lookupKey);
				break;
			}
			}
		}

		public void Update()
		{
			double num = Math.Abs(Main.WindForVisuals);
			num = Utils.GetLerpValue(0.08f, 1.2f, (float)num, clamped: true);
			_treeWindCounter += 0.0041666666666666666 + 0.0041666666666666666 * num * 2.0;
			_grassWindCounter += 0.0055555555555555558 + 0.0055555555555555558 * num * 4.0;
			_sunflowerWindCounter += 0.0023809523809523812 + 0.0023809523809523812 * num * 5.0;
			_vineWindCounter += 0.0083333333333333332 + 0.0083333333333333332 * num * 0.40000000596046448;
			UpdateLeafFrequency();
			EnsureWindGridSize();
			_windGrid.Update();
			_shouldShowInvisibleBlocks = Main.LocalPlayer.CanSeeInvisibleBlocks;
		}

		public void PreDrawTiles(bool solidLayer, bool forRenderTargets, bool intoRenderTargets)
		{
			bool flag = intoRenderTargets || Lighting.UpdateEveryFrame;
			if (!solidLayer && flag)
			{
				_specialsCount[5] = 0;
				_specialsCount[4] = 0;
				_specialsCount[8] = 0;
				_specialsCount[6] = 0;
				_specialsCount[3] = 0;
				_specialsCount[0] = 0;
				_specialsCount[9] = 0;
				_specialsCount[10] = 0;
				_specialsCount[11] = 0;
			}
		}

		public void PostDrawTiles(bool solidLayer, bool forRenderTargets, bool intoRenderTargets)
		{
			if (!solidLayer && !intoRenderTargets)
			{
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
				DrawMultiTileVines();
				DrawMultiTileGrass();
				DrawVoidLenses();
				DrawTeleportationPylons();
				DrawMasterTrophies();
				DrawGrass();
				DrawTrees();
				DrawVines();
				DrawReverseVines();
				Main.spriteBatch.End();
			}
			if (solidLayer && !intoRenderTargets)
			{
				DrawEntities_HatRacks();
				DrawEntities_DisplayDolls();
			}
		}

		public void Draw(bool solidLayer, bool forRenderTargets, bool intoRenderTargets, int waterStyleOverride = -1)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			_isActiveAndNotPaused = !Main.gamePaused && Main.instance.IsActive;
			_localPlayer = Main.LocalPlayer;
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 vector = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				vector = Vector2.Zero;
			}
			if (!solidLayer)
			{
				Main.critterCage = false;
			}
			EnsureWindGridSize();
			ClearLegacyCachedDraws();
			bool flag = intoRenderTargets || Main.LightingEveryFrame;
			if (flag)
			{
				ClearCachedTileDraws(solidLayer);
			}
			float num = 255f * (1f - Main.gfxQuality) + 30f * Main.gfxQuality;
			_highQualityLightingRequirement.R = (byte)num;
			_highQualityLightingRequirement.G = (byte)((double)num * 1.1);
			_highQualityLightingRequirement.B = (byte)((double)num * 1.2);
			float num2 = 50f * (1f - Main.gfxQuality) + 2f * Main.gfxQuality;
			_mediumQualityLightingRequirement.R = (byte)num2;
			_mediumQualityLightingRequirement.G = (byte)((double)num2 * 1.1);
			_mediumQualityLightingRequirement.B = (byte)((double)num2 * 1.2);
			GetScreenDrawArea(unscaledPosition, vector + (Main.Camera.UnscaledPosition - Main.Camera.ScaledPosition), out var firstTileX, out var lastTileX, out var firstTileY, out var lastTileY);
			byte b = (byte)(100f + 150f * Main.martianLight);
			_martianGlow = new Color((int)b, (int)b, (int)b, 0);
			TileDrawInfo value = _currentTileDrawInfo.Value;
			for (int i = firstTileY; i < lastTileY + 4; i++)
			{
				for (int j = firstTileX - 2; j < lastTileX + 2; j++)
				{
					Tile tile = Main.tile[j, i];
					if (tile == null)
					{
						tile = new Tile();
						Main.tile[j, i] = tile;
						Main.mapTime += 60;
					}
					else
					{
						if (!tile.active() || IsTileDrawLayerSolid(tile.type) != solidLayer)
						{
							continue;
						}
						ushort type = tile.type;
						short frameX = tile.frameX;
						short frameY = tile.frameY;
						if (!TextureAssets.Tile[type].IsLoaded)
						{
							Main.instance.LoadTiles(type);
						}
						switch (type)
						{
						case 541:
							if (!_shouldShowInvisibleBlocks)
							{
								continue;
							}
							break;
						case 52:
						case 62:
						case 115:
						case 205:
						case 382:
						case 528:
							if (flag)
							{
								CrawlToTopOfVineAndAddSpecialPoint(i, j);
							}
							continue;
						case 549:
							if (flag)
							{
								CrawlToBottomOfReverseVineAndAddSpecialPoint(i, j);
							}
							continue;
						case 34:
							if (frameX % 54 == 0 && frameY % 54 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileVine);
							}
							continue;
						case 454:
							if (frameX % 72 == 0 && frameY % 54 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileVine);
							}
							continue;
						case 42:
						case 270:
						case 271:
						case 572:
						case 581:
							if (frameX % 18 == 0 && frameY % 36 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileVine);
							}
							continue;
						case 91:
							if (frameX % 18 == 0 && frameY % 54 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileVine);
							}
							continue;
						case 95:
						case 126:
						case 444:
							if (frameX % 36 == 0 && frameY % 36 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileVine);
							}
							continue;
						case 465:
						case 591:
						case 592:
							if (frameX % 36 == 0 && frameY % 54 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileVine);
							}
							continue;
						case 27:
							if (frameX % 36 == 0 && frameY == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
							}
							continue;
						case 236:
						case 238:
							if (frameX % 36 == 0 && frameY == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
							}
							continue;
						case 233:
							if (frameY == 0 && frameX % 54 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
							}
							if (frameY == 34 && frameX % 36 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
							}
							continue;
						case 530:
							if (frameX < 270)
							{
								if (frameX % 54 == 0 && frameY == 0 && flag)
								{
									AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
								}
								continue;
							}
							break;
						case 485:
						case 489:
						case 490:
							if (frameY == 0 && frameX % 36 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
							}
							continue;
						case 521:
						case 522:
						case 523:
						case 524:
						case 525:
						case 526:
						case 527:
							if (frameY == 0 && frameX % 36 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
							}
							continue;
						case 493:
							if (frameY == 0 && frameX % 18 == 0 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
							}
							continue;
						case 519:
							if (frameX / 18 <= 4 && flag)
							{
								AddSpecialPoint(j, i, TileCounterType.MultiTileGrass);
							}
							continue;
						case 373:
						case 374:
						case 375:
						case 461:
							EmitLiquidDrops(i, j, tile, type);
							continue;
						case 491:
							if (flag && frameX == 18 && frameY == 18)
							{
								AddSpecialPoint(j, i, TileCounterType.VoidLens);
							}
							break;
						case 597:
							if (flag && frameX % 54 == 0 && frameY == 0)
							{
								AddSpecialPoint(j, i, TileCounterType.TeleportationPylon);
							}
							break;
						case 617:
							if (flag && frameX % 54 == 0 && frameY % 72 == 0)
							{
								AddSpecialPoint(j, i, TileCounterType.MasterTrophy);
							}
							break;
						default:
							if (ShouldSwayInWind(j, i, tile))
							{
								if (flag)
								{
									AddSpecialPoint(j, i, TileCounterType.WindyGrass);
								}
								continue;
							}
							break;
						}
						DrawSingleTile(value, solidLayer, waterStyleOverride, unscaledPosition, vector, j, i);
					}
				}
			}
			if (solidLayer)
			{
				Main.instance.DrawTileCracks(1, Main.player[Main.myPlayer].hitReplace);
				Main.instance.DrawTileCracks(1, Main.player[Main.myPlayer].hitTile);
			}
			DrawSpecialTilesLegacy(unscaledPosition, vector);
			if (TileObject.objectPreview.Active && _localPlayer.cursorItemIconEnabled && Main.placementPreview && !CaptureManager.Instance.Active)
			{
				Main.instance.LoadTiles(TileObject.objectPreview.Type);
				TileObject.DrawPreview(Main.spriteBatch, TileObject.objectPreview, unscaledPosition - vector);
			}
			if (solidLayer)
			{
				TimeLogger.DrawTime(0, stopwatch.Elapsed.TotalMilliseconds);
			}
			else
			{
				TimeLogger.DrawTime(1, stopwatch.Elapsed.TotalMilliseconds);
			}
		}

		private void CrawlToTopOfVineAndAddSpecialPoint(int j, int i)
		{
			int y = j;
			for (int num = j - 1; num > 0; num--)
			{
				Tile tile = Main.tile[i, num];
				if (WorldGen.SolidTile(i, num) || !tile.active())
				{
					y = num + 1;
					break;
				}
			}
			Point item = new Point(i, y);
			if (!_vineRootsPositions.Contains(item))
			{
				_vineRootsPositions.Add(item);
				AddSpecialPoint(i, y, TileCounterType.Vine);
			}
		}

		private void CrawlToBottomOfReverseVineAndAddSpecialPoint(int j, int i)
		{
			int y = j;
			for (int k = j; k < Main.maxTilesY; k++)
			{
				Tile tile = Main.tile[i, k];
				if (WorldGen.SolidTile(i, k) || !tile.active())
				{
					y = k - 1;
					break;
				}
			}
			Point item = new Point(i, y);
			if (!_reverseVineRootsPositions.Contains(item))
			{
				_reverseVineRootsPositions.Add(item);
				AddSpecialPoint(i, y, TileCounterType.ReverseVine);
			}
		}

		private void DrawSingleTile(TileDrawInfo drawData, bool solidLayer, int waterStyleOverride, Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY)
		{
			drawData.tileCache = Main.tile[tileX, tileY];
			drawData.typeCache = drawData.tileCache.type;
			drawData.tileFrameX = drawData.tileCache.frameX;
			drawData.tileFrameY = drawData.tileCache.frameY;
			drawData.tileLight = Lighting.GetColor(tileX, tileY);
			if (drawData.tileCache.liquid > 0 && drawData.tileCache.type == 518)
			{
				return;
			}
			GetTileDrawData(tileX, tileY, drawData.tileCache, drawData.typeCache, drawData.tileFrameX, drawData.tileFrameY, out drawData.tileWidth, out drawData.tileHeight, out drawData.tileTop, out drawData.halfBrickHeight, out drawData.addFrX, out drawData.addFrY, out drawData.tileSpriteEffect, out drawData.glowTexture, out drawData.glowSourceRect, out drawData.glowColor);
			drawData.drawTexture = GetTileDrawTexture(drawData.tileCache, tileX, tileY);
			Texture2D highlightTexture = null;
			Rectangle empty = Rectangle.Empty;
			Color highlightColor = Color.Transparent;
			if (TileID.Sets.HasOutlines[drawData.typeCache])
			{
				GetTileOutlineInfo(tileX, tileY, drawData.typeCache, drawData.tileLight, highlightTexture, highlightColor);
			}
			if (_localPlayer.dangerSense && IsTileDangerous(_localPlayer, drawData.tileCache, drawData.typeCache))
			{
				if (drawData.tileLight.R < byte.MaxValue)
				{
					drawData.tileLight.R = byte.MaxValue;
				}
				if (drawData.tileLight.G < 50)
				{
					drawData.tileLight.G = 50;
				}
				if (drawData.tileLight.B < 50)
				{
					drawData.tileLight.B = 50;
				}
				if (_isActiveAndNotPaused && _rand.Next(30) == 0)
				{
					int num = Dust.NewDust(new Vector2(tileX * 16, tileY * 16), 16, 16, 60, 0f, 0f, 100, default(Color), 0.3f);
					_dust[num].fadeIn = 1f;
					_dust[num].velocity *= 0.1f;
					_dust[num].noLight = true;
					_dust[num].noGravity = true;
				}
			}
			if (_localPlayer.findTreasure && Main.IsTileSpelunkable(drawData.typeCache, drawData.tileFrameX, drawData.tileFrameY))
			{
				if (drawData.tileLight.R < 200)
				{
					drawData.tileLight.R = 200;
				}
				if (drawData.tileLight.G < 170)
				{
					drawData.tileLight.G = 170;
				}
				if (_isActiveAndNotPaused && _rand.Next(60) == 0)
				{
					int num2 = Dust.NewDust(new Vector2(tileX * 16, tileY * 16), 16, 16, 204, 0f, 0f, 150, default(Color), 0.3f);
					_dust[num2].fadeIn = 1f;
					_dust[num2].velocity *= 0.1f;
					_dust[num2].noLight = true;
				}
			}
			if (_isActiveAndNotPaused)
			{
				if (!Lighting.UpdateEveryFrame || new FastRandom(Main.TileFrameSeed).WithModifier(tileX, tileY).Next(4) == 0)
				{
					DrawTiles_EmitParticles(tileY, tileX, drawData.tileCache, drawData.typeCache, drawData.tileFrameX, drawData.tileFrameY, drawData.tileLight);
				}
				drawData.tileLight = DrawTiles_GetLightOverride(tileY, tileX, drawData.tileCache, drawData.typeCache, drawData.tileFrameX, drawData.tileFrameY, drawData.tileLight);
			}
			CacheSpecialDraws(tileX, tileY, drawData);
			if (drawData.typeCache == 72 && drawData.tileFrameX >= 36)
			{
				int num3 = 0;
				if (drawData.tileFrameY == 18)
				{
					num3 = 1;
				}
				else if (drawData.tileFrameY == 36)
				{
					num3 = 2;
				}
				Main.spriteBatch.Draw(TextureAssets.ShroomCap.Value, new Vector2(tileX * 16 - (int)screenPosition.X - 22, tileY * 16 - (int)screenPosition.Y - 26) + screenOffset, new Rectangle(num3 * 62, 0, 60, 42), Lighting.GetColor(tileX, tileY), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			Rectangle normalTileRect = new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight - drawData.halfBrickHeight);
			Vector2 vector = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop + drawData.halfBrickHeight) + screenOffset;
			if (drawData.tileLight.R < 1 && drawData.tileLight.G < 1 && drawData.tileLight.B < 1)
			{
				return;
			}
			DrawTile_LiquidBehindTile(solidLayer, waterStyleOverride, screenPosition, screenOffset, tileX, tileY, drawData);
			drawData.colorTint = Color.White;
			drawData.finalColor = GetFinalLight(drawData.tileCache, drawData.typeCache, drawData.tileLight, drawData.colorTint);
			switch (drawData.typeCache)
			{
			case 136:
				switch (drawData.tileFrameX / 18)
				{
				case 1:
					vector.X += -2f;
					break;
				case 2:
					vector.X += 2f;
					break;
				}
				break;
			case 442:
			{
				int num6 = drawData.tileFrameX / 22;
				if (num6 == 3)
				{
					vector.X += 2f;
				}
				break;
			}
			case 51:
				drawData.finalColor = drawData.tileLight * 0.5f;
				break;
			case 160:
			{
				Color color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 255);
				if (drawData.tileCache.inActive())
				{
					color = drawData.tileCache.actColor(color);
				}
				drawData.finalColor = color;
				break;
			}
			case 129:
			{
				drawData.finalColor = new Color(255, 255, 255, 100);
				int num5 = 2;
				if (drawData.tileFrameX >= 324)
				{
					drawData.finalColor = Color.Transparent;
				}
				if (drawData.tileFrameY < 36)
				{
					vector.Y += num5 * (drawData.tileFrameY == 0).ToDirectionInt();
				}
				else
				{
					vector.X += num5 * (drawData.tileFrameY == 36).ToDirectionInt();
				}
				break;
			}
			case 272:
			{
				int num4 = Main.tileFrame[drawData.typeCache];
				num4 += tileX % 2;
				num4 += tileY % 2;
				num4 += tileX % 3;
				num4 += tileY % 3;
				num4 %= 2;
				num4 *= 90;
				drawData.addFrY += num4;
				normalTileRect.Y += num4;
				break;
			}
			case 80:
			{
				GetCactusType(tileX, tileY, drawData.tileFrameX, drawData.tileFrameY, out var evil, out var good, out var crimson);
				if (evil)
				{
					normalTileRect.Y += 54;
				}
				if (good)
				{
					normalTileRect.Y += 108;
				}
				if (crimson)
				{
					normalTileRect.Y += 162;
				}
				break;
			}
			case 83:
				drawData.drawTexture = GetTileDrawTexture(drawData.tileCache, tileX, tileY);
				break;
			case 323:
				if (drawData.tileCache.frameX <= 132 && drawData.tileCache.frameX >= 88)
				{
					return;
				}
				vector.X += drawData.tileCache.frameY;
				break;
			}
			if (drawData.typeCache == 314)
			{
				DrawTile_MinecartTrack(screenPosition, screenOffset, tileX, tileY, drawData);
			}
			else if (drawData.typeCache == 171)
			{
				DrawXmasTree(screenPosition, screenOffset, tileX, tileY, drawData);
			}
			else
			{
				DrawBasicTile(screenPosition, screenOffset, tileX, tileY, drawData, normalTileRect, vector);
			}
			if (Main.tileGlowMask[drawData.tileCache.type] != -1)
			{
				short num7 = Main.tileGlowMask[drawData.tileCache.type];
				if (TextureAssets.GlowMask.IndexInRange(num7))
				{
					drawData.drawTexture = TextureAssets.GlowMask[num7].Value;
				}
				double num8 = Main.timeForVisualEffects * 0.08;
				Color color2 = Color.White;
				bool flag = false;
				switch (drawData.tileCache.type)
				{
				case 350:
					color2 = new Color(new Vector4((float)((0.0 - Math.Cos(((int)(num8 / 6.283) % 3 == 1) ? num8 : 0.0)) * 0.2 + 0.2)));
					break;
				case 381:
				case 517:
					color2 = _lavaMossGlow;
					break;
				case 534:
				case 535:
					color2 = _kryptonMossGlow;
					break;
				case 536:
				case 537:
					color2 = _xenonMossGlow;
					break;
				case 539:
				case 540:
					color2 = _argonMossGlow;
					break;
				case 370:
				case 390:
					color2 = _meteorGlow;
					break;
				case 391:
					color2 = new Color(250, 250, 250, 200);
					break;
				case 209:
					color2 = PortalHelper.GetPortalColor(Main.myPlayer, (drawData.tileCache.frameX >= 288) ? 1 : 0);
					break;
				case 429:
				case 445:
					drawData.drawTexture = GetTileDrawTexture(drawData.tileCache, tileX, tileY);
					drawData.addFrY = 18;
					break;
				case 129:
				{
					if (drawData.tileFrameX < 324)
					{
						flag = true;
						break;
					}
					drawData.drawTexture = GetTileDrawTexture(drawData.tileCache, tileX, tileY);
					color2 = Main.hslToRgb(0.7f + (float)Math.Sin((float)Math.PI * 2f * Main.GlobalTimeWrappedHourly * 0.16f + (float)tileX * 0.3f + (float)tileY * 0.7f) * 0.16f, 1f, 0.5f);
					color2.A /= 2;
					color2 *= 0.3f;
					int num9 = 72;
					for (float num10 = 0f; num10 < (float)Math.PI * 2f; num10 += (float)Math.PI / 2f)
					{
						Main.spriteBatch.Draw(drawData.drawTexture, vector + num10.ToRotationVector2() * 2f, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY + num9, drawData.tileWidth, drawData.tileHeight), color2, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					color2 = new Color(255, 255, 255, 100);
					break;
				}
				}
				if (!flag)
				{
					if (drawData.tileCache.slope() == 0 && !drawData.tileCache.halfBrick())
					{
						Main.spriteBatch.Draw(drawData.drawTexture, vector, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), color2, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					else if (drawData.tileCache.halfBrick())
					{
						Main.spriteBatch.Draw(drawData.drawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + 10) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY + 10, drawData.tileWidth, 6), color2, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					else
					{
						byte b = drawData.tileCache.slope();
						for (int i = 0; i < 8; i++)
						{
							int num11 = i << 1;
							Rectangle value = new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY + i * 2, num11, 2);
							int num12 = 0;
							switch (b)
							{
							case 2:
								value.X = 16 - num11;
								num12 = 16 - num11;
								break;
							case 3:
								value.Width = 16 - num11;
								break;
							case 4:
								value.Width = 14 - num11;
								value.X = num11 + 2;
								num12 = num11 + 2;
								break;
							}
							Main.spriteBatch.Draw(drawData.drawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + (float)num12, tileY * 16 - (int)screenPosition.Y + i * 2) + screenOffset, value, color2, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
						}
					}
				}
			}
			if (drawData.glowTexture != null)
			{
				Vector2 position = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset;
				if (TileID.Sets.Platforms[drawData.typeCache])
				{
					position = vector;
				}
				Main.spriteBatch.Draw(drawData.glowTexture, position, drawData.glowSourceRect, drawData.glowColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			if (highlightTexture != null)
			{
				empty = new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight);
				int num13 = 0;
				int num14 = 0;
				Main.spriteBatch.Draw(highlightTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + (float)num13, tileY * 16 - (int)screenPosition.Y + drawData.tileTop + num14) + screenOffset, empty, highlightColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
		}

		private Texture2D GetTileDrawTexture(Tile tile, int tileX, int tileY)
		{
			Texture2D result = TextureAssets.Tile[tile.type].Value;
			int tileStyle = 0;
			int num = tile.type;
			switch (tile.type)
			{
			case 5:
				tileStyle = GetTreeBiome(tileX, tileY, tile.frameX, tile.frameY);
				break;
			case 323:
				tileStyle = GetPalmTreeBiome(tileX, tileY);
				break;
			case 83:
				if (IsAlchemyPlantHarvestable(tile.frameX / 18))
				{
					num = 84;
				}
				Main.instance.LoadTiles(num);
				break;
			}
			Texture2D texture2D = _paintSystem.TryGetTileAndRequestIfNotReady(num, tileStyle, tile.color());
			if (texture2D != null)
			{
				result = texture2D;
			}
			return result;
		}

		private Texture2D GetTileDrawTexture(Tile tile, int tileX, int tileY, int paintOverride)
		{
			Texture2D result = TextureAssets.Tile[tile.type].Value;
			int tileStyle = 0;
			int num = tile.type;
			switch (tile.type)
			{
			case 5:
				tileStyle = GetTreeBiome(tileX, tileY, tile.frameX, tile.frameY);
				break;
			case 323:
				tileStyle = GetPalmTreeBiome(tileX, tileY);
				break;
			case 83:
				if (IsAlchemyPlantHarvestable(tile.frameX / 18))
				{
					num = 84;
				}
				Main.instance.LoadTiles(num);
				break;
			}
			Texture2D texture2D = _paintSystem.TryGetTileAndRequestIfNotReady(num, tileStyle, paintOverride);
			if (texture2D != null)
			{
				result = texture2D;
			}
			return result;
		}

		private void DrawBasicTile(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, TileDrawInfo drawData, Rectangle normalTileRect, Vector2 normalTilePosition)
		{
			if (drawData.tileCache.slope() > 0)
			{
				if (TileID.Sets.Platforms[drawData.tileCache.type])
				{
					Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, normalTileRect, drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					if (drawData.tileCache.slope() == 1 && Main.tile[tileX + 1, tileY + 1].active() && Main.tileSolid[Main.tile[tileX + 1, tileY + 1].type] && Main.tile[tileX + 1, tileY + 1].slope() != 2 && !Main.tile[tileX + 1, tileY + 1].halfBrick() && (!Main.tile[tileX, tileY + 1].active() || (Main.tile[tileX, tileY + 1].blockType() != 0 && Main.tile[tileX, tileY + 1].blockType() != 5) || (!TileID.Sets.BlocksStairs[Main.tile[tileX, tileY + 1].type] && !TileID.Sets.BlocksStairsAbove[Main.tile[tileX, tileY + 1].type])))
					{
						Rectangle value = new Rectangle(198, drawData.tileFrameY, 16, 16);
						if (TileID.Sets.Platforms[Main.tile[tileX + 1, tileY + 1].type] && Main.tile[tileX + 1, tileY + 1].slope() == 0)
						{
							value.X = 324;
						}
						Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 16f), value, drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					else if (drawData.tileCache.slope() == 2 && Main.tile[tileX - 1, tileY + 1].active() && Main.tileSolid[Main.tile[tileX - 1, tileY + 1].type] && Main.tile[tileX - 1, tileY + 1].slope() != 1 && !Main.tile[tileX - 1, tileY + 1].halfBrick() && (!Main.tile[tileX, tileY + 1].active() || (Main.tile[tileX, tileY + 1].blockType() != 0 && Main.tile[tileX, tileY + 1].blockType() != 4) || (!TileID.Sets.BlocksStairs[Main.tile[tileX, tileY + 1].type] && !TileID.Sets.BlocksStairsAbove[Main.tile[tileX, tileY + 1].type])))
					{
						Rectangle value2 = new Rectangle(162, drawData.tileFrameY, 16, 16);
						if (TileID.Sets.Platforms[Main.tile[tileX - 1, tileY + 1].type] && Main.tile[tileX - 1, tileY + 1].slope() == 0)
						{
							value2.X = 306;
						}
						Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 16f), value2, drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					return;
				}
				if (TileID.Sets.HasSlopeFrames[drawData.tileCache.type])
				{
					Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, 16, 16), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					return;
				}
				int num = drawData.tileCache.slope();
				int num2 = 2;
				for (int i = 0; i < 8; i++)
				{
					int num3 = i * -2;
					int num4 = 16 - i * 2;
					int num5 = 16 - num4;
					int num6;
					switch (num)
					{
					case 1:
						num3 = 0;
						num6 = i * 2;
						num4 = 14 - i * 2;
						num5 = 0;
						break;
					case 2:
						num3 = 0;
						num6 = 16 - i * 2 - 2;
						num4 = 14 - i * 2;
						num5 = 0;
						break;
					case 3:
						num6 = i * 2;
						break;
					default:
						num6 = 16 - i * 2 - 2;
						break;
					}
					Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(num6, i * num2 + num3), new Rectangle(drawData.tileFrameX + drawData.addFrX + num6, drawData.tileFrameY + drawData.addFrY + num5, num2, num4), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				}
				int num7 = ((num <= 2) ? 14 : 0);
				Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, num7), new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY + num7, 16, 2), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				return;
			}
			if (!TileID.Sets.Platforms[drawData.typeCache] && !TileID.Sets.IgnoresNearbyHalfbricksWhenDrawn[drawData.typeCache] && _tileSolid[drawData.typeCache] && !TileID.Sets.NotReallySolid[drawData.typeCache] && !drawData.tileCache.halfBrick() && (Main.tile[tileX - 1, tileY].halfBrick() || Main.tile[tileX + 1, tileY].halfBrick()))
			{
				if (Main.tile[tileX - 1, tileY].halfBrick() && Main.tile[tileX + 1, tileY].halfBrick())
				{
					Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 8f), new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.addFrY + drawData.tileFrameY + 8, drawData.tileWidth, 8), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					Rectangle value3 = new Rectangle(126 + drawData.addFrX, drawData.addFrY, 16, 8);
					if (Main.tile[tileX, tileY - 1].active() && !Main.tile[tileX, tileY - 1].bottomSlope() && Main.tile[tileX, tileY - 1].type == drawData.typeCache)
					{
						value3 = new Rectangle(90 + drawData.addFrX, drawData.addFrY, 16, 8);
					}
					Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, value3, drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				}
				else if (Main.tile[tileX - 1, tileY].halfBrick())
				{
					int num8 = 4;
					if (TileID.Sets.AllBlocksWithSmoothBordersToResolveHalfBlockIssue[drawData.typeCache])
					{
						num8 = 2;
					}
					Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 8f), new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.addFrY + drawData.tileFrameY + 8, drawData.tileWidth, 8), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(num8, 0f), new Rectangle(drawData.tileFrameX + num8 + drawData.addFrX, drawData.addFrY + drawData.tileFrameY, drawData.tileWidth - num8, drawData.tileHeight), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle(144 + drawData.addFrX, drawData.addFrY, num8, 8), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					if (num8 == 2)
					{
						Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle(148 + drawData.addFrX, drawData.addFrY, 2, 2), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
				}
				else if (Main.tile[tileX + 1, tileY].halfBrick())
				{
					int num9 = 4;
					if (TileID.Sets.AllBlocksWithSmoothBordersToResolveHalfBlockIssue[drawData.typeCache])
					{
						num9 = 2;
					}
					Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 8f), new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.addFrY + drawData.tileFrameY + 8, drawData.tileWidth, 8), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.addFrY + drawData.tileFrameY, drawData.tileWidth - num9, drawData.tileHeight), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(16 - num9, 0f), new Rectangle(144 + (16 - num9), 0, num9, 8), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					if (num9 == 2)
					{
						Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(14f, 0f), new Rectangle(156, 0, 2, 2), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
				}
				return;
			}
			if (Lighting.NotRetro && _tileSolid[drawData.typeCache] && !drawData.tileCache.halfBrick() && !drawData.tileCache.inActive() && drawData.typeCache != 137 && drawData.typeCache != 235 && drawData.typeCache != 388 && drawData.typeCache != 476 && drawData.typeCache != 160 && drawData.typeCache != 138)
			{
				DrawSingleTile_SlicedBlock(normalTilePosition, tileX, tileY, drawData);
				return;
			}
			if (drawData.halfBrickHeight == 8 && (!Main.tile[tileX, tileY + 1].active() || !_tileSolid[Main.tile[tileX, tileY + 1].type] || Main.tile[tileX, tileY + 1].halfBrick()))
			{
				if (TileID.Sets.Platforms[drawData.typeCache])
				{
					Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, normalTileRect, drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				}
				else
				{
					Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, normalTileRect.Modified(0, 0, 0, -4), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 4f), new Rectangle(144 + drawData.addFrX, 66 + drawData.addFrY, drawData.tileWidth, 4), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
				}
			}
			else
			{
				Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, normalTileRect, drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			DrawSingleTile_Flames(screenPosition, screenOffset, tileX, tileY, drawData);
		}

		private int GetPalmTreeBiome(int tileX, int tileY)
		{
			int i;
			for (i = tileY; Main.tile[tileX, i].active() && Main.tile[tileX, i].type == 323; i++)
			{
			}
			return GetPalmTreeVariant(tileX, i);
		}

		private static int GetTreeBiome(int tileX, int tileY, int tileFrameX, int tileFrameY)
		{
			int num = tileX;
			int i = tileY;
			int type = Main.tile[num, i].type;
			if (tileFrameX == 66 && tileFrameY <= 45)
			{
				num++;
			}
			if (tileFrameX == 88 && tileFrameY >= 66 && tileFrameY <= 110)
			{
				num--;
			}
			if (tileFrameY >= 198)
			{
				switch (tileFrameX)
				{
				case 66:
					num--;
					break;
				case 44:
					num++;
					break;
				}
			}
			else if (tileFrameY >= 132)
			{
				switch (tileFrameX)
				{
				case 22:
					num--;
					break;
				case 44:
					num++;
					break;
				}
			}
			for (; Main.tile[num, i].active() && Main.tile[num, i].type == type; i++)
			{
			}
			return GetTreeVariant(num, i);
		}

		public static int GetTreeVariant(int x, int y)
		{
			if (Main.tile[x, y] == null || !Main.tile[x, y].active())
			{
				return -1;
			}
			switch (Main.tile[x, y].type)
			{
			case 23:
				return 0;
			case 60:
				if (!((double)y > Main.worldSurface))
				{
					return 1;
				}
				return 5;
			case 70:
				return 6;
			case 109:
			case 492:
				return 2;
			case 147:
				return 3;
			case 199:
				return 4;
			default:
				return -1;
			}
		}

		private TileFlameData GetTileFlameData(int tileX, int tileY, int type, int tileFrameY)
		{
			TileFlameData tileFlameData;
			switch (type)
			{
			case 270:
				tileFlameData = default(TileFlameData);
				tileFlameData.flameTexture = TextureAssets.FireflyJar.Value;
				tileFlameData.flameColor = new Color(200, 200, 200, 0);
				tileFlameData.flameCount = 1;
				return tileFlameData;
			case 271:
				tileFlameData = default(TileFlameData);
				tileFlameData.flameTexture = TextureAssets.LightningbugJar.Value;
				tileFlameData.flameColor = new Color(200, 200, 200, 0);
				tileFlameData.flameCount = 1;
				return tileFlameData;
			case 581:
				tileFlameData = default(TileFlameData);
				tileFlameData.flameTexture = TextureAssets.GlowMask[291].Value;
				tileFlameData.flameColor = new Color(200, 100, 100, 0);
				tileFlameData.flameCount = 1;
				return tileFlameData;
			default:
			{
				if (!Main.tileFlame[type])
				{
					tileFlameData = default(TileFlameData);
					return tileFlameData;
				}
				ulong flameSeed = Main.TileFrameSeed ^ (ulong)(((long)tileX << 32) | (uint)tileY);
				int num = 0;
				switch (type)
				{
				case 4:
					num = 0;
					break;
				case 33:
				case 174:
					num = 1;
					break;
				case 100:
				case 173:
					num = 2;
					break;
				case 34:
					num = 3;
					break;
				case 93:
					num = 4;
					break;
				case 49:
					num = 5;
					break;
				case 372:
					num = 16;
					break;
				case 98:
					num = 6;
					break;
				case 35:
					num = 7;
					break;
				case 42:
					num = 13;
					break;
				}
				tileFlameData = default(TileFlameData);
				tileFlameData.flameTexture = TextureAssets.Flames[num].Value;
				tileFlameData.flameSeed = flameSeed;
				TileFlameData result = tileFlameData;
				switch (num)
				{
				case 7:
					result.flameCount = 4;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 10;
					result.flameRangeMultX = 0f;
					result.flameRangeMultY = 0f;
					break;
				case 1:
					switch (Main.tile[tileX, tileY].frameY / 22)
					{
					case 5:
					case 6:
					case 7:
					case 10:
						result.flameCount = 7;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.075f;
						result.flameRangeMultY = 0.075f;
						break;
					case 8:
						result.flameCount = 7;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.3f;
						result.flameRangeMultY = 0.3f;
						break;
					case 12:
						result.flameCount = 7;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0.1f;
						result.flameRangeMultY = 0.15f;
						break;
					case 14:
						result.flameCount = 8;
						result.flameColor = new Color(75, 75, 75, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.1f;
						result.flameRangeMultY = 0.1f;
						break;
					case 16:
						result.flameCount = 4;
						result.flameColor = new Color(75, 75, 75, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.15f;
						result.flameRangeMultY = 0.15f;
						break;
					case 27:
					case 28:
						result.flameCount = 1;
						result.flameColor = new Color(75, 75, 75, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						break;
					default:
						result.flameCount = 7;
						result.flameColor = new Color(100, 100, 100, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0.15f;
						result.flameRangeMultY = 0.35f;
						break;
					}
					break;
				case 2:
					switch (Main.tile[tileX, tileY].frameY / 36)
					{
					case 3:
						result.flameCount = 3;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.05f;
						result.flameRangeMultY = 0.15f;
						break;
					case 6:
						result.flameCount = 5;
						result.flameColor = new Color(75, 75, 75, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.15f;
						result.flameRangeMultY = 0.15f;
						break;
					case 9:
						result.flameCount = 7;
						result.flameColor = new Color(100, 100, 100, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.3f;
						result.flameRangeMultY = 0.3f;
						break;
					case 11:
						result.flameCount = 7;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0.1f;
						result.flameRangeMultY = 0.15f;
						break;
					case 13:
						result.flameCount = 8;
						result.flameColor = new Color(75, 75, 75, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.1f;
						result.flameRangeMultY = 0.1f;
						break;
					case 28:
					case 29:
						result.flameCount = 1;
						result.flameColor = new Color(75, 75, 75, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						break;
					default:
						result.flameCount = 7;
						result.flameColor = new Color(100, 100, 100, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0.15f;
						result.flameRangeMultY = 0.35f;
						break;
					}
					break;
				case 3:
					switch (Main.tile[tileX, tileY].frameY / 54)
					{
					case 8:
						result.flameCount = 7;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.075f;
						result.flameRangeMultY = 0.075f;
						break;
					case 9:
						result.flameCount = 3;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.05f;
						result.flameRangeMultY = 0.15f;
						break;
					case 11:
						result.flameCount = 7;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.3f;
						result.flameRangeMultY = 0.3f;
						break;
					case 15:
						result.flameCount = 7;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0.1f;
						result.flameRangeMultY = 0.15f;
						break;
					case 17:
					case 20:
						result.flameCount = 7;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.075f;
						result.flameRangeMultY = 0.075f;
						break;
					case 18:
						result.flameCount = 8;
						result.flameColor = new Color(75, 75, 75, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.1f;
						result.flameRangeMultY = 0.1f;
						break;
					case 34:
					case 35:
						result.flameCount = 1;
						result.flameColor = new Color(75, 75, 75, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						break;
					default:
						result.flameCount = 7;
						result.flameColor = new Color(100, 100, 100, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0.15f;
						result.flameRangeMultY = 0.35f;
						break;
					}
					break;
				case 4:
					switch (Main.tile[tileX, tileY].frameY / 54)
					{
					case 1:
						result.flameCount = 3;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.15f;
						result.flameRangeMultY = 0.15f;
						break;
					case 2:
					case 4:
						result.flameCount = 7;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.075f;
						result.flameRangeMultY = 0.075f;
						break;
					case 3:
						result.flameCount = 7;
						result.flameColor = new Color(100, 100, 100, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -20;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0.2f;
						result.flameRangeMultY = 0.35f;
						break;
					case 5:
						result.flameCount = 7;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.3f;
						result.flameRangeMultY = 0.3f;
						break;
					case 9:
						result.flameCount = 7;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0.1f;
						result.flameRangeMultY = 0.15f;
						break;
					case 13:
						result.flameCount = 8;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.1f;
						result.flameRangeMultY = 0.1f;
						break;
					case 12:
						result.flameCount = 1;
						result.flameColor = new Color(100, 100, 100, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.01f;
						result.flameRangeMultY = 0.01f;
						break;
					case 28:
					case 29:
						result.flameCount = 1;
						result.flameColor = new Color(75, 75, 75, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						break;
					default:
						result.flameCount = 7;
						result.flameColor = new Color(100, 100, 100, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0.15f;
						result.flameRangeMultY = 0.35f;
						break;
					}
					break;
				case 13:
					switch (tileFrameY / 36)
					{
					case 1:
					case 3:
					case 6:
					case 8:
					case 19:
					case 27:
					case 29:
					case 30:
					case 31:
					case 32:
					case 36:
					case 39:
						result.flameCount = 7;
						result.flameColor = new Color(100, 100, 100, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0.15f;
						result.flameRangeMultY = 0.35f;
						break;
					case 2:
					case 16:
					case 25:
						result.flameCount = 7;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0.15f;
						result.flameRangeMultY = 0.1f;
						break;
					case 11:
						result.flameCount = 7;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.075f;
						result.flameRangeMultY = 0.075f;
						break;
					case 34:
					case 35:
						result.flameCount = 1;
						result.flameColor = new Color(75, 75, 75, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						break;
					case 44:
						result.flameCount = 7;
						result.flameColor = new Color(100, 100, 100, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0.15f;
						result.flameRangeMultY = 0.35f;
						break;
					default:
						result.flameCount = 0;
						break;
					}
					break;
				default:
					result.flameCount = 7;
					result.flameColor = new Color(100, 100, 100, 0);
					if (tileFrameY / 22 == 14)
					{
						result.flameColor = new Color((float)Main.DiscoR / 255f, (float)Main.DiscoG / 255f, (float)Main.DiscoB / 255f, 0f);
					}
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.35f;
					break;
				}
				return result;
			}
			}
		}

		private void DrawSingleTile_Flames(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, TileDrawInfo drawData)
		{
			if (drawData.typeCache == 548 && drawData.tileFrameX / 54 > 6)
			{
				Main.spriteBatch.Draw(TextureAssets.GlowMask[297].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), Color.White, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			if (drawData.typeCache == 613)
			{
				Main.spriteBatch.Draw(TextureAssets.GlowMask[298].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), Color.White, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			if (drawData.typeCache == 614)
			{
				Main.spriteBatch.Draw(TextureAssets.GlowMask[299].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), Color.White, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			if (drawData.typeCache == 593)
			{
				Main.spriteBatch.Draw(TextureAssets.GlowMask[295].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), Color.White, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			if (drawData.typeCache == 594)
			{
				Main.spriteBatch.Draw(TextureAssets.GlowMask[296].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), Color.White, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			if (drawData.typeCache == 215 && drawData.tileFrameY < 36)
			{
				int num = 15;
				Color color = new Color(255, 255, 255, 0);
				if (drawData.tileFrameX / 54 == 5)
				{
					color = new Color((float)Main.DiscoR / 255f, (float)Main.DiscoG / 255f, (float)Main.DiscoB / 255f, 0f);
				}
				Main.spriteBatch.Draw(TextureAssets.Flames[num].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), color, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			if (drawData.typeCache == 85)
			{
				float graveyardVisualIntensity = Main.GraveyardVisualIntensity;
				if (graveyardVisualIntensity > 0f)
				{
					ulong num2 = Main.TileFrameSeed ^ (ulong)(((long)tileX << 32) | (uint)tileY);
					TileFlameData tileFlameData = GetTileFlameData(tileX, tileY, drawData.typeCache, drawData.tileFrameY);
					if (num2 == 0L)
					{
						num2 = tileFlameData.flameSeed;
					}
					tileFlameData.flameSeed = num2;
					Vector2 vector = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset;
					Rectangle value = new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight);
					for (int i = 0; i < tileFlameData.flameCount; i++)
					{
						Color color2 = tileFlameData.flameColor * graveyardVisualIntensity;
						float x = (float)Utils.RandomInt(tileFlameData.flameSeed, tileFlameData.flameRangeXMin, tileFlameData.flameRangeXMax) * tileFlameData.flameRangeMultX;
						float y = (float)Utils.RandomInt(tileFlameData.flameSeed, tileFlameData.flameRangeYMin, tileFlameData.flameRangeYMax) * tileFlameData.flameRangeMultY;
						for (float num3 = 0f; num3 < 1f; num3 += 0.25f)
						{
							Main.spriteBatch.Draw(tileFlameData.flameTexture, vector + new Vector2(x, y) + Vector2.UnitX.RotatedBy(num3 * ((float)Math.PI * 2f)) * 2f, value, color2, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						Main.spriteBatch.Draw(tileFlameData.flameTexture, vector, value, Color.White * graveyardVisualIntensity, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
				}
			}
			if (drawData.typeCache == 286)
			{
				Main.spriteBatch.Draw(TextureAssets.GlowSnail.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(75, 100, 255, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			if (drawData.typeCache == 582)
			{
				Main.spriteBatch.Draw(TextureAssets.GlowMask[293].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(200, 100, 100, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			if (drawData.typeCache == 391)
			{
				Main.spriteBatch.Draw(TextureAssets.GlowMask[131].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(250, 250, 250, 200), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			if (drawData.typeCache == 619)
			{
				Main.spriteBatch.Draw(TextureAssets.GlowMask[300].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(75, 100, 255, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			if (drawData.typeCache == 270)
			{
				Main.spriteBatch.Draw(TextureAssets.FireflyJar.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			if (drawData.typeCache == 271)
			{
				Main.spriteBatch.Draw(TextureAssets.LightningbugJar.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			if (drawData.typeCache == 581)
			{
				Main.spriteBatch.Draw(TextureAssets.GlowMask[291].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			if (drawData.typeCache == 316 || drawData.typeCache == 317 || drawData.typeCache == 318)
			{
				int num4 = tileX - drawData.tileFrameX / 18;
				int num5 = tileY - drawData.tileFrameY / 18;
				int num6 = num4 / 2 * (num5 / 3);
				num6 %= Main.cageFrames;
				Main.spriteBatch.Draw(TextureAssets.JellyfishBowl[drawData.typeCache - 316].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + Main.jellyfishCageFrame[drawData.typeCache - 316, num6] * 36, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			if (drawData.typeCache == 149 && drawData.tileFrameX < 54)
			{
				Main.spriteBatch.Draw(TextureAssets.XmasLight.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			if (drawData.typeCache == 300 || drawData.typeCache == 302 || drawData.typeCache == 303 || drawData.typeCache == 306)
			{
				int num7 = 9;
				if (drawData.typeCache == 302)
				{
					num7 = 10;
				}
				if (drawData.typeCache == 303)
				{
					num7 = 11;
				}
				if (drawData.typeCache == 306)
				{
					num7 = 12;
				}
				Main.spriteBatch.Draw(TextureAssets.Flames[num7].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			else if (Main.tileFlame[drawData.typeCache])
			{
				ulong seed = Main.TileFrameSeed ^ (ulong)(((long)tileX << 32) | (uint)tileY);
				int typeCache = drawData.typeCache;
				int num8 = 0;
				switch (typeCache)
				{
				case 4:
					num8 = 0;
					break;
				case 33:
				case 174:
					num8 = 1;
					break;
				case 100:
				case 173:
					num8 = 2;
					break;
				case 34:
					num8 = 3;
					break;
				case 93:
					num8 = 4;
					break;
				case 49:
					num8 = 5;
					break;
				case 372:
					num8 = 16;
					break;
				case 98:
					num8 = 6;
					break;
				case 35:
					num8 = 7;
					break;
				case 42:
					num8 = 13;
					break;
				}
				switch (num8)
				{
				case 7:
				{
					for (int k = 0; k < 4; k++)
					{
						float num11 = (float)Utils.RandomInt(seed, -10, 11) * 0.15f;
						float num12 = (float)Utils.RandomInt(seed, -10, 10) * 0.15f;
						num11 = 0f;
						num12 = 0f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num11, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num12) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				case 1:
					switch (Main.tile[tileX, tileY].frameY / 22)
					{
					case 5:
					case 6:
					case 7:
					case 10:
					{
						for (int num91 = 0; num91 < 7; num91++)
						{
							float num92 = (float)Utils.RandomInt(seed, -10, 11) * 0.075f;
							float num93 = (float)Utils.RandomInt(seed, -10, 11) * 0.075f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num92, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num93) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 8:
					{
						for (int num97 = 0; num97 < 7; num97++)
						{
							float num98 = (float)Utils.RandomInt(seed, -10, 11) * 0.3f;
							float num99 = (float)Utils.RandomInt(seed, -10, 11) * 0.3f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num98, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num99) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 12:
					{
						for (int num85 = 0; num85 < 7; num85++)
						{
							float num86 = (float)Utils.RandomInt(seed, -10, 11) * 0.1f;
							float num87 = (float)Utils.RandomInt(seed, -10, 1) * 0.15f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num86, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num87) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 14:
					{
						for (int num94 = 0; num94 < 8; num94++)
						{
							float num95 = (float)Utils.RandomInt(seed, -10, 11) * 0.1f;
							float num96 = (float)Utils.RandomInt(seed, -10, 11) * 0.1f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num95, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num96) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 16:
					{
						for (int num88 = 0; num88 < 4; num88++)
						{
							float num89 = (float)Utils.RandomInt(seed, -10, 11) * 0.15f;
							float num90 = (float)Utils.RandomInt(seed, -10, 11) * 0.15f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num89, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num90) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 27:
					case 28:
						Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						break;
					default:
					{
						for (int num82 = 0; num82 < 7; num82++)
						{
							float num83 = (float)Utils.RandomInt(seed, -10, 11) * 0.15f;
							float num84 = (float)Utils.RandomInt(seed, -10, 1) * 0.35f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num83, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num84) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					}
					break;
				case 2:
					switch (Main.tile[tileX, tileY].frameY / 36)
					{
					case 3:
					{
						for (int num73 = 0; num73 < 3; num73++)
						{
							float num74 = (float)Utils.RandomInt(seed, -10, 11) * 0.05f;
							float num75 = (float)Utils.RandomInt(seed, -10, 11) * 0.15f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num74, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num75) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 6:
					{
						for (int num79 = 0; num79 < 5; num79++)
						{
							float num80 = (float)Utils.RandomInt(seed, -10, 11) * 0.15f;
							float num81 = (float)Utils.RandomInt(seed, -10, 11) * 0.15f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num80, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num81) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 9:
					{
						for (int num67 = 0; num67 < 7; num67++)
						{
							float num68 = (float)Utils.RandomInt(seed, -10, 11) * 0.3f;
							float num69 = (float)Utils.RandomInt(seed, -10, 11) * 0.3f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num68, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num69) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 11:
					{
						for (int num76 = 0; num76 < 7; num76++)
						{
							float num77 = (float)Utils.RandomInt(seed, -10, 11) * 0.1f;
							float num78 = (float)Utils.RandomInt(seed, -10, 1) * 0.15f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num77, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num78) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 13:
					{
						for (int num70 = 0; num70 < 8; num70++)
						{
							float num71 = (float)Utils.RandomInt(seed, -10, 11) * 0.1f;
							float num72 = (float)Utils.RandomInt(seed, -10, 11) * 0.1f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num71, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num72) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 28:
					case 29:
						Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						break;
					default:
					{
						for (int num64 = 0; num64 < 7; num64++)
						{
							float num65 = (float)Utils.RandomInt(seed, -10, 11) * 0.15f;
							float num66 = (float)Utils.RandomInt(seed, -10, 1) * 0.35f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num65, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num66) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					}
					break;
				case 3:
					switch (Main.tile[tileX, tileY].frameY / 54)
					{
					case 8:
					{
						for (int num58 = 0; num58 < 7; num58++)
						{
							float num59 = (float)Utils.RandomInt(seed, -10, 11) * 0.075f;
							float num60 = (float)Utils.RandomInt(seed, -10, 11) * 0.075f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num59, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num60) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 9:
					{
						for (int num46 = 0; num46 < 3; num46++)
						{
							float num47 = (float)Utils.RandomInt(seed, -10, 11) * 0.05f;
							float num48 = (float)Utils.RandomInt(seed, -10, 11) * 0.15f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num47, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num48) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 11:
					{
						for (int num52 = 0; num52 < 7; num52++)
						{
							float num53 = (float)Utils.RandomInt(seed, -10, 11) * 0.3f;
							float num54 = (float)Utils.RandomInt(seed, -10, 11) * 0.3f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num53, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num54) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 15:
					{
						for (int num61 = 0; num61 < 7; num61++)
						{
							float num62 = (float)Utils.RandomInt(seed, -10, 11) * 0.1f;
							float num63 = (float)Utils.RandomInt(seed, -10, 1) * 0.15f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num62, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num63) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 17:
					case 20:
					{
						for (int num55 = 0; num55 < 7; num55++)
						{
							float num56 = (float)Utils.RandomInt(seed, -10, 11) * 0.075f;
							float num57 = (float)Utils.RandomInt(seed, -10, 11) * 0.075f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num56, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num57) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 18:
					{
						for (int num49 = 0; num49 < 8; num49++)
						{
							float num50 = (float)Utils.RandomInt(seed, -10, 11) * 0.1f;
							float num51 = (float)Utils.RandomInt(seed, -10, 11) * 0.1f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num50, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num51) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 34:
					case 35:
						Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						break;
					default:
					{
						for (int num43 = 0; num43 < 7; num43++)
						{
							float num44 = (float)Utils.RandomInt(seed, -10, 11) * 0.15f;
							float num45 = (float)Utils.RandomInt(seed, -10, 1) * 0.35f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num44, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num45) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					}
					break;
				case 4:
					switch (Main.tile[tileX, tileY].frameY / 54)
					{
					case 1:
					{
						for (int num37 = 0; num37 < 3; num37++)
						{
							float num38 = (float)Utils.RandomInt(seed, -10, 11) * 0.15f;
							float num39 = (float)Utils.RandomInt(seed, -10, 11) * 0.15f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num38, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num39) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 2:
					case 4:
					{
						for (int num23 = 0; num23 < 7; num23++)
						{
							float num24 = (float)Utils.RandomInt(seed, -10, 11) * 0.075f;
							float num25 = (float)Utils.RandomInt(seed, -10, 11) * 0.075f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num24, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num25) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 3:
					{
						for (int num31 = 0; num31 < 7; num31++)
						{
							float num32 = (float)Utils.RandomInt(seed, -10, 11) * 0.2f;
							float num33 = (float)Utils.RandomInt(seed, -20, 1) * 0.35f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num32, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num33) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 5:
					{
						for (int num40 = 0; num40 < 7; num40++)
						{
							float num41 = (float)Utils.RandomInt(seed, -10, 11) * 0.3f;
							float num42 = (float)Utils.RandomInt(seed, -10, 11) * 0.3f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num41, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num42) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 9:
					{
						for (int num34 = 0; num34 < 7; num34++)
						{
							float num35 = (float)Utils.RandomInt(seed, -10, 11) * 0.1f;
							float num36 = (float)Utils.RandomInt(seed, -10, 1) * 0.15f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num35, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num36) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 13:
					{
						for (int num28 = 0; num28 < 8; num28++)
						{
							float num29 = (float)Utils.RandomInt(seed, -10, 11) * 0.1f;
							float num30 = (float)Utils.RandomInt(seed, -10, 11) * 0.1f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num29, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num30) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 12:
					{
						float num26 = (float)Utils.RandomInt(seed, -10, 11) * 0.01f;
						float num27 = (float)Utils.RandomInt(seed, -10, 11) * 0.01f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num26, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num27) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(Utils.RandomInt(seed, 90, 111), Utils.RandomInt(seed, 90, 111), Utils.RandomInt(seed, 90, 111), 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						break;
					}
					case 28:
					case 29:
						Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						break;
					default:
					{
						for (int num20 = 0; num20 < 7; num20++)
						{
							float num21 = (float)Utils.RandomInt(seed, -10, 11) * 0.15f;
							float num22 = (float)Utils.RandomInt(seed, -10, 1) * 0.35f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num21, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num22) + screenOffset, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					}
					break;
				case 13:
				{
					int num13 = drawData.tileFrameY / 36;
					switch (num13)
					{
					case 1:
					case 3:
					case 6:
					case 8:
					case 19:
					case 27:
					case 29:
					case 30:
					case 31:
					case 32:
					case 36:
					case 39:
					{
						for (int n = 0; n < 7; n++)
						{
							float num18 = (float)Utils.RandomInt(seed, -10, 11) * 0.15f;
							float num19 = (float)Utils.RandomInt(seed, -10, 1) * 0.35f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num18, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num19) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(100, 100, 100, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					case 2:
					case 16:
					case 25:
					{
						for (int m = 0; m < 7; m++)
						{
							float num16 = (float)Utils.RandomInt(seed, -10, 11) * 0.15f;
							float num17 = (float)Utils.RandomInt(seed, -10, 1) * 0.1f;
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num16, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num17) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(50, 50, 50, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
						}
						break;
					}
					default:
						switch (num13)
						{
						case 29:
						{
							for (int l = 0; l < 7; l++)
							{
								float num14 = (float)Utils.RandomInt(seed, -10, 11) * 0.15f;
								float num15 = (float)Utils.RandomInt(seed, -10, 1) * 0.15f;
								Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num14, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num15) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(25, 25, 25, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
							}
							break;
						}
						case 34:
						case 35:
							Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(75, 75, 75, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
							break;
						}
						break;
					}
					break;
				}
				default:
				{
					for (int j = 0; j < 7; j++)
					{
						Color color3 = new Color(100, 100, 100, 0);
						if (drawData.tileFrameY / 22 == 14)
						{
							color3 = new Color((float)Main.DiscoR / 255f, (float)Main.DiscoG / 255f, (float)Main.DiscoB / 255f, 0f);
						}
						float num9 = (float)Utils.RandomInt(seed, -10, 11) * 0.15f;
						float num10 = (float)Utils.RandomInt(seed, -10, 1) * 0.35f;
						Main.spriteBatch.Draw(TextureAssets.Flames[num8].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num9, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num10) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), color3, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
					}
					break;
				}
				}
			}
			if (drawData.typeCache == 144)
			{
				Main.spriteBatch.Draw(TextureAssets.Timer.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color(200, 200, 200, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
			if (drawData.typeCache == 237)
			{
				Main.spriteBatch.Draw(TextureAssets.SunAltar.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(drawData.tileFrameX, drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight), new Color((int)Main.mouseTextColor / 2, (int)Main.mouseTextColor / 2, (int)Main.mouseTextColor / 2, 0), 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
		}

		private int GetPalmTreeVariant(int x, int y)
		{
			int num = -1;
			if (Main.tile[x, y].active() && Main.tile[x, y].type == 53)
			{
				num = 0;
			}
			if (Main.tile[x, y].active() && Main.tile[x, y].type == 234)
			{
				num = 1;
			}
			if (Main.tile[x, y].active() && Main.tile[x, y].type == 116)
			{
				num = 2;
			}
			if (Main.tile[x, y].active() && Main.tile[x, y].type == 112)
			{
				num = 3;
			}
			if (WorldGen.IsPalmOasisTree(x))
			{
				num += 4;
			}
			return num;
		}

		private void DrawSingleTile_SlicedBlock(Vector2 normalTilePosition, int tileX, int tileY, TileDrawInfo drawData)
		{
			Color color = default(Color);
			Vector2 origin = default(Vector2);
			Rectangle value = default(Rectangle);
			Vector3 tileLight = default(Vector3);
			Vector2 position = default(Vector2);
			if (drawData.tileLight.R > _highQualityLightingRequirement.R || drawData.tileLight.G > _highQualityLightingRequirement.G || drawData.tileLight.B > _highQualityLightingRequirement.B)
			{
				Vector3[] slices = drawData.colorSlices;
				Lighting.GetColor9Slice(tileX, tileY, slices);
				Vector3 vector = drawData.tileLight.ToVector3();
				Vector3 tint = drawData.colorTint.ToVector3();
				if (drawData.tileCache.color() == 31)
				{
					slices = _glowPaintColorSlices;
				}
				for (int i = 0; i < 9; i++)
				{
					value.X = 0;
					value.Y = 0;
					value.Width = 4;
					value.Height = 4;
					switch (i)
					{
					case 1:
						value.Width = 8;
						value.X = 4;
						break;
					case 2:
						value.X = 12;
						break;
					case 3:
						value.Height = 8;
						value.Y = 4;
						break;
					case 4:
						value.Width = 8;
						value.Height = 8;
						value.X = 4;
						value.Y = 4;
						break;
					case 5:
						value.X = 12;
						value.Y = 4;
						value.Height = 8;
						break;
					case 6:
						value.Y = 12;
						break;
					case 7:
						value.Width = 8;
						value.Height = 4;
						value.X = 4;
						value.Y = 12;
						break;
					case 8:
						value.X = 12;
						value.Y = 12;
						break;
					}
					tileLight.X = (slices[i].X + vector.X) * 0.5f;
					tileLight.Y = (slices[i].Y + vector.Y) * 0.5f;
					tileLight.Z = (slices[i].Z + vector.Z) * 0.5f;
					GetFinalLight(drawData.tileCache, drawData.typeCache, tileLight, tint);
					position.X = normalTilePosition.X + (float)value.X;
					position.Y = normalTilePosition.Y + (float)value.Y;
					value.X += drawData.tileFrameX + drawData.addFrX;
					value.Y += drawData.tileFrameY + drawData.addFrY;
					int num = (int)(tileLight.X * 255f);
					int num2 = (int)(tileLight.Y * 255f);
					int num3 = (int)(tileLight.Z * 255f);
					if (num > 255)
					{
						num = 255;
					}
					if (num2 > 255)
					{
						num2 = 255;
					}
					if (num3 > 255)
					{
						num3 = 255;
					}
					num3 <<= 16;
					num2 <<= 8;
					color.PackedValue = (uint)(num | num2 | num3) | 0xFF000000u;
					Main.spriteBatch.Draw(drawData.drawTexture, position, value, color, 0f, origin, 1f, drawData.tileSpriteEffect, 0f);
				}
			}
			else if (drawData.tileLight.R > _mediumQualityLightingRequirement.R || drawData.tileLight.G > _mediumQualityLightingRequirement.G || drawData.tileLight.B > _mediumQualityLightingRequirement.B)
			{
				Vector3[] slices2 = drawData.colorSlices;
				Lighting.GetColor4Slice(tileX, tileY, slices2);
				Vector3 vector2 = drawData.tileLight.ToVector3();
				Vector3 tint2 = drawData.colorTint.ToVector3();
				value.Width = 8;
				value.Height = 8;
				for (int j = 0; j < 4; j++)
				{
					value.X = 0;
					value.Y = 0;
					switch (j)
					{
					case 1:
						value.X = 8;
						break;
					case 2:
						value.Y = 8;
						break;
					case 3:
						value.X = 8;
						value.Y = 8;
						break;
					}
					tileLight.X = (slices2[j].X + vector2.X) * 0.5f;
					tileLight.Y = (slices2[j].Y + vector2.Y) * 0.5f;
					tileLight.Z = (slices2[j].Z + vector2.Z) * 0.5f;
					GetFinalLight(drawData.tileCache, drawData.typeCache, tileLight, tint2);
					position.X = normalTilePosition.X + (float)value.X;
					position.Y = normalTilePosition.Y + (float)value.Y;
					value.X += drawData.tileFrameX + drawData.addFrX;
					value.Y += drawData.tileFrameY + drawData.addFrY;
					int num4 = (int)(tileLight.X * 255f);
					int num5 = (int)(tileLight.Y * 255f);
					int num6 = (int)(tileLight.Z * 255f);
					if (num4 > 255)
					{
						num4 = 255;
					}
					if (num5 > 255)
					{
						num5 = 255;
					}
					if (num6 > 255)
					{
						num6 = 255;
					}
					num6 <<= 16;
					num5 <<= 8;
					color.PackedValue = (uint)(num4 | num5 | num6) | 0xFF000000u;
					Main.spriteBatch.Draw(drawData.drawTexture, position, value, color, 0f, origin, 1f, drawData.tileSpriteEffect, 0f);
				}
			}
			else
			{
				Main.spriteBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight), drawData.finalColor, 0f, _zero, 1f, drawData.tileSpriteEffect, 0f);
			}
		}

		private void GetCactusType(int tileX, int tileY, int frameX, int frameY, out bool evil, out bool good, out bool crimson)
		{
			evil = false;
			good = false;
			crimson = false;
			int num = tileX;
			if (frameX == 36)
			{
				num--;
			}
			if (frameX == 54)
			{
				num++;
			}
			if (frameX == 108)
			{
				num = ((frameY != 18) ? (num + 1) : (num - 1));
			}
			int num2 = tileY;
			bool flag = false;
			if (Main.tile[num, num2].type == 80 && Main.tile[num, num2].active())
			{
				flag = true;
			}
			while (!Main.tile[num, num2].active() || !_tileSolid[Main.tile[num, num2].type] || !flag)
			{
				if (Main.tile[num, num2].type == 80 && Main.tile[num, num2].active())
				{
					flag = true;
				}
				num2++;
				if (num2 > tileY + 20)
				{
					break;
				}
			}
			if (Main.tile[num, num2].type == 112)
			{
				evil = true;
			}
			if (Main.tile[num, num2].type == 116)
			{
				good = true;
			}
			if (Main.tile[num, num2].type == 234)
			{
				crimson = true;
			}
		}

		private void DrawXmasTree(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, TileDrawInfo drawData)
		{
			if (tileY - drawData.tileFrameY > 0 && drawData.tileFrameY == 7 && Main.tile[tileX, tileY - drawData.tileFrameY] != null)
			{
				drawData.tileTop -= 16 * drawData.tileFrameY;
				drawData.tileFrameX = Main.tile[tileX, tileY - drawData.tileFrameY].frameX;
				drawData.tileFrameY = Main.tile[tileX, tileY - drawData.tileFrameY].frameY;
			}
			if (drawData.tileFrameX < 10)
			{
				return;
			}
			int num = 0;
			if ((drawData.tileFrameY & 1) == 1)
			{
				num++;
			}
			if ((drawData.tileFrameY & 2) == 2)
			{
				num += 2;
			}
			if ((drawData.tileFrameY & 4) == 4)
			{
				num += 4;
			}
			int num2 = 0;
			if ((drawData.tileFrameY & 8) == 8)
			{
				num2++;
			}
			if ((drawData.tileFrameY & 0x10) == 16)
			{
				num2 += 2;
			}
			if ((drawData.tileFrameY & 0x20) == 32)
			{
				num2 += 4;
			}
			int num3 = 0;
			if ((drawData.tileFrameY & 0x40) == 64)
			{
				num3++;
			}
			if ((drawData.tileFrameY & 0x80) == 128)
			{
				num3 += 2;
			}
			if ((drawData.tileFrameY & 0x100) == 256)
			{
				num3 += 4;
			}
			if ((drawData.tileFrameY & 0x200) == 512)
			{
				num3 += 8;
			}
			int num4 = 0;
			if ((drawData.tileFrameY & 0x400) == 1024)
			{
				num4++;
			}
			if ((drawData.tileFrameY & 0x800) == 2048)
			{
				num4 += 2;
			}
			if ((drawData.tileFrameY & 0x1000) == 4096)
			{
				num4 += 4;
			}
			if ((drawData.tileFrameY & 0x2000) == 8192)
			{
				num4 += 8;
			}
			Color color = Lighting.GetColor(tileX + 1, tileY - 3);
			Main.spriteBatch.Draw(TextureAssets.XmasTree[0].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(0, 0, 64, 128), color, 0f, _zero, 1f, SpriteEffects.None, 0f);
			if (num > 0)
			{
				num--;
				Color color2 = color;
				if (num != 3)
				{
					color2 = new Color(255, 255, 255, 255);
				}
				Main.spriteBatch.Draw(TextureAssets.XmasTree[3].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(66 * num, 0, 64, 128), color2, 0f, _zero, 1f, SpriteEffects.None, 0f);
			}
			if (num2 > 0)
			{
				num2--;
				Main.spriteBatch.Draw(TextureAssets.XmasTree[1].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(66 * num2, 0, 64, 128), color, 0f, _zero, 1f, SpriteEffects.None, 0f);
			}
			if (num3 > 0)
			{
				num3--;
				Main.spriteBatch.Draw(TextureAssets.XmasTree[2].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(66 * num3, 0, 64, 128), color, 0f, _zero, 1f, SpriteEffects.None, 0f);
			}
			if (num4 > 0)
			{
				num4--;
				Main.spriteBatch.Draw(TextureAssets.XmasTree[4].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + screenOffset, new Rectangle(66 * num4, 130 * Main.tileFrame[171], 64, 128), new Color(255, 255, 255, 255), 0f, _zero, 1f, SpriteEffects.None, 0f);
			}
		}

		private void DrawTile_MinecartTrack(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, TileDrawInfo drawData)
		{
			drawData.tileLight = GetFinalLight(drawData.tileCache, drawData.typeCache, drawData.tileLight, drawData.colorTint);
			Minecart.TrackColors(tileX, tileY, drawData.tileCache, out var frontColor, out var backColor);
			drawData.drawTexture = GetTileDrawTexture(drawData.tileCache, tileX, tileY, frontColor);
			Texture2D tileDrawTexture = GetTileDrawTexture(drawData.tileCache, tileX, tileY, backColor);
			drawData.tileCache.frameNumber();
			if (drawData.tileFrameY != -1)
			{
				Main.spriteBatch.Draw(tileDrawTexture, new Vector2(tileX * 16 - (int)screenPosition.X, tileY * 16 - (int)screenPosition.Y) + screenOffset, Minecart.GetSourceRect(drawData.tileFrameY, Main.tileFrame[314]), drawData.tileLight, 0f, default(Vector2), 1f, drawData.tileSpriteEffect, 0f);
			}
			Main.spriteBatch.Draw(drawData.drawTexture, new Vector2(tileX * 16 - (int)screenPosition.X, tileY * 16 - (int)screenPosition.Y) + screenOffset, Minecart.GetSourceRect(drawData.tileFrameX, Main.tileFrame[314]), drawData.tileLight, 0f, default(Vector2), 1f, drawData.tileSpriteEffect, 0f);
			if (Minecart.DrawLeftDecoration(drawData.tileFrameY))
			{
				Main.spriteBatch.Draw(tileDrawTexture, new Vector2(tileX * 16 - (int)screenPosition.X, (tileY + 1) * 16 - (int)screenPosition.Y) + screenOffset, Minecart.GetSourceRect(36), drawData.tileLight, 0f, default(Vector2), 1f, drawData.tileSpriteEffect, 0f);
			}
			if (Minecart.DrawLeftDecoration(drawData.tileFrameX))
			{
				Main.spriteBatch.Draw(drawData.drawTexture, new Vector2(tileX * 16 - (int)screenPosition.X, (tileY + 1) * 16 - (int)screenPosition.Y) + screenOffset, Minecart.GetSourceRect(36), drawData.tileLight, 0f, default(Vector2), 1f, drawData.tileSpriteEffect, 0f);
			}
			if (Minecart.DrawRightDecoration(drawData.tileFrameY))
			{
				Main.spriteBatch.Draw(tileDrawTexture, new Vector2(tileX * 16 - (int)screenPosition.X, (tileY + 1) * 16 - (int)screenPosition.Y) + screenOffset, Minecart.GetSourceRect(37, Main.tileFrame[314]), drawData.tileLight, 0f, default(Vector2), 1f, drawData.tileSpriteEffect, 0f);
			}
			if (Minecart.DrawRightDecoration(drawData.tileFrameX))
			{
				Main.spriteBatch.Draw(drawData.drawTexture, new Vector2(tileX * 16 - (int)screenPosition.X, (tileY + 1) * 16 - (int)screenPosition.Y) + screenOffset, Minecart.GetSourceRect(37), drawData.tileLight, 0f, default(Vector2), 1f, drawData.tileSpriteEffect, 0f);
			}
			if (Minecart.DrawBumper(drawData.tileFrameX))
			{
				Main.spriteBatch.Draw(drawData.drawTexture, new Vector2(tileX * 16 - (int)screenPosition.X, (tileY - 1) * 16 - (int)screenPosition.Y) + screenOffset, Minecart.GetSourceRect(39), drawData.tileLight, 0f, default(Vector2), 1f, drawData.tileSpriteEffect, 0f);
			}
			else if (Minecart.DrawBouncyBumper(drawData.tileFrameX))
			{
				Main.spriteBatch.Draw(drawData.drawTexture, new Vector2(tileX * 16 - (int)screenPosition.X, (tileY - 1) * 16 - (int)screenPosition.Y) + screenOffset, Minecart.GetSourceRect(38), drawData.tileLight, 0f, default(Vector2), 1f, drawData.tileSpriteEffect, 0f);
			}
		}

		private void DrawTile_LiquidBehindTile(bool solidLayer, int waterStyleOverride, Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, TileDrawInfo drawData)
		{
			Tile tile = Main.tile[tileX + 1, tileY];
			Tile tile2 = Main.tile[tileX - 1, tileY];
			Tile tile3 = Main.tile[tileX, tileY - 1];
			Tile tile4 = Main.tile[tileX, tileY + 1];
			if (tile == null)
			{
				tile = new Tile();
				Main.tile[tileX + 1, tileY] = tile;
			}
			if (tile2 == null)
			{
				tile2 = new Tile();
				Main.tile[tileX - 1, tileY] = tile2;
			}
			if (tile3 == null)
			{
				tile3 = new Tile();
				Main.tile[tileX, tileY - 1] = tile3;
			}
			if (tile4 == null)
			{
				tile4 = new Tile();
				Main.tile[tileX, tileY + 1] = tile4;
			}
			if (!solidLayer || drawData.tileCache.inActive() || _tileSolidTop[drawData.typeCache] || (drawData.tileCache.halfBrick() && (tile2.liquid > 160 || tile.liquid > 160) && Main.instance.waterfallManager.CheckForWaterfall(tileX, tileY)) || (TileID.Sets.BlocksWaterDrawingBehindSelf[drawData.tileCache.type] && drawData.tileCache.slope() == 0))
			{
				return;
			}
			int num = 0;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			int num2 = 0;
			bool flag6 = false;
			int num3 = drawData.tileCache.slope();
			int num4 = drawData.tileCache.blockType();
			if (drawData.tileCache.type == 546 && drawData.tileCache.liquid > 0)
			{
				flag5 = true;
				flag4 = true;
				flag = true;
				flag2 = true;
				switch (drawData.tileCache.liquidType())
				{
				case 0:
					flag6 = true;
					break;
				case 1:
					num2 = 1;
					break;
				case 2:
					num2 = 11;
					break;
				}
				num = drawData.tileCache.liquid;
			}
			else
			{
				if (drawData.tileCache.liquid > 0 && num4 != 0 && (num4 != 1 || drawData.tileCache.liquid > 160))
				{
					flag5 = true;
					switch (drawData.tileCache.liquidType())
					{
					case 0:
						flag6 = true;
						break;
					case 1:
						num2 = 1;
						break;
					case 2:
						num2 = 11;
						break;
					}
					if (drawData.tileCache.liquid > num)
					{
						num = drawData.tileCache.liquid;
					}
				}
				if (tile2.liquid > 0 && num3 != 1 && num3 != 3)
				{
					flag = true;
					switch (tile2.liquidType())
					{
					case 0:
						flag6 = true;
						break;
					case 1:
						num2 = 1;
						break;
					case 2:
						num2 = 11;
						break;
					}
					if (tile2.liquid > num)
					{
						num = tile2.liquid;
					}
				}
				if (tile.liquid > 0 && num3 != 2 && num3 != 4)
				{
					flag2 = true;
					switch (tile.liquidType())
					{
					case 0:
						flag6 = true;
						break;
					case 1:
						num2 = 1;
						break;
					case 2:
						num2 = 11;
						break;
					}
					if (tile.liquid > num)
					{
						num = tile.liquid;
					}
				}
				if (tile3.liquid > 0 && num3 != 3 && num3 != 4)
				{
					flag3 = true;
					switch (tile3.liquidType())
					{
					case 0:
						flag6 = true;
						break;
					case 1:
						num2 = 1;
						break;
					case 2:
						num2 = 11;
						break;
					}
				}
				if (tile4.liquid > 0 && num3 != 1 && num3 != 2)
				{
					if (tile4.liquid > 240)
					{
						flag4 = true;
					}
					switch (tile4.liquidType())
					{
					case 0:
						flag6 = true;
						break;
					case 1:
						num2 = 1;
						break;
					case 2:
						num2 = 11;
						break;
					}
				}
			}
			if (!flag3 && !flag4 && !flag && !flag2 && !flag5)
			{
				return;
			}
			if (waterStyleOverride != -1)
			{
				Main.waterStyle = waterStyleOverride;
			}
			if (num2 == 0)
			{
				num2 = Main.waterStyle;
			}
			Color color = Lighting.GetColor(tileX, tileY);
			Vector2 value = new Vector2(tileX * 16, tileY * 16);
			Rectangle liquidSize = new Rectangle(0, 4, 16, 16);
			if (flag4 && (flag || flag2))
			{
				flag = true;
				flag2 = true;
			}
			if ((!flag3 || !(flag || flag2)) && !(flag4 && flag3))
			{
				if (flag3)
				{
					liquidSize = new Rectangle(0, 4, 16, 4);
					if (drawData.tileCache.halfBrick() || drawData.tileCache.slope() != 0)
					{
						liquidSize = new Rectangle(0, 4, 16, 12);
					}
				}
				else if (flag4 && !flag && !flag2)
				{
					value = new Vector2(tileX * 16, tileY * 16 + 12);
					liquidSize = new Rectangle(0, 4, 16, 4);
				}
				else
				{
					float num5 = 256 - num;
					num5 /= 32f;
					int y = 4;
					if (tile3.liquid == 0 && (num4 != 0 || !WorldGen.SolidTile(tileX, tileY - 1)))
					{
						y = 0;
					}
					if (drawData.tileCache.slope() != 0)
					{
						value = new Vector2(tileX * 16, tileY * 16 + (int)num5 * 2);
						liquidSize = new Rectangle(0, (int)num5 * 2, 16, 16 - (int)num5 * 2);
					}
					else if ((flag && flag2) || drawData.tileCache.halfBrick())
					{
						value = new Vector2(tileX * 16, tileY * 16 + (int)num5 * 2);
						liquidSize = new Rectangle(0, y, 16, 16 - (int)num5 * 2);
					}
					else if (!flag)
					{
						value = new Vector2(tileX * 16 + 12, tileY * 16 + (int)num5 * 2);
						liquidSize = new Rectangle(0, y, 4, 16 - (int)num5 * 2);
					}
					else
					{
						value = new Vector2(tileX * 16, tileY * 16 + (int)num5 * 2);
						liquidSize = new Rectangle(0, y, 4, 16 - (int)num5 * 2);
					}
				}
			}
			float num6 = 0.5f;
			switch (num2)
			{
			case 1:
				num6 = 1f;
				break;
			case 11:
				num6 = Math.Max(num6 * 1.7f, 1f);
				break;
			}
			if ((double)tileY <= Main.worldSurface || num6 > 1f)
			{
				num6 = 1f;
				if (drawData.tileCache.wall == 21)
				{
					num6 = 0.9f;
				}
				else if (drawData.tileCache.wall > 0)
				{
					num6 = 0.6f;
				}
			}
			if (drawData.tileCache.halfBrick() && tile3.liquid > 0 && drawData.tileCache.wall > 0)
			{
				num6 = 0f;
			}
			if (drawData.tileCache.bottomSlope() && ((tile2.liquid == 0 && !WorldGen.SolidTile(tileX - 1, tileY)) || (tile.liquid == 0 && !WorldGen.SolidTile(tileX + 1, tileY))))
			{
				num6 = 0f;
			}
			color *= num6;
			bool flag7 = false;
			if (flag6)
			{
				for (int i = 0; i < 13; i++)
				{
					if (Main.IsLiquidStyleWater(i) && Main.liquidAlpha[i] > 0f && i != num2)
					{
						DrawPartialLiquid(drawData.tileCache, value - screenPosition + screenOffset, liquidSize, i, color);
						flag7 = true;
						break;
					}
				}
			}
			DrawPartialLiquid(drawData.tileCache, value - screenPosition + screenOffset, liquidSize, num2, color * (flag7 ? Main.liquidAlpha[num2] : 1f));
		}

		private void CacheSpecialDraws(int tileX, int tileY, TileDrawInfo drawData)
		{
			if (TileID.Sets.BasicChest[drawData.typeCache])
			{
				Point key = new Point(tileX, tileY);
				if (drawData.tileFrameX % 36 != 0)
				{
					key.X--;
				}
				if (drawData.tileFrameY % 36 != 0)
				{
					key.Y--;
				}
				if (!_chestPositions.ContainsKey(key))
				{
					_chestPositions[key] = Chest.FindChest(key.X, key.Y);
				}
				int num = drawData.tileFrameX / 18;
				int num2 = drawData.tileFrameY / 18;
				int num3 = drawData.tileFrameX / 36;
				int num4 = num * 18;
				drawData.addFrX = num4 - drawData.tileFrameX;
				int num5 = num2 * 18;
				if (_chestPositions[key] != -1)
				{
					int frame = Main.chest[_chestPositions[key]].frame;
					if (frame == 1)
					{
						num5 += 38;
					}
					if (frame == 2)
					{
						num5 += 76;
					}
				}
				drawData.addFrY = num5 - drawData.tileFrameY;
				if (num2 != 0)
				{
					drawData.tileHeight = 18;
				}
				if (drawData.typeCache == 21 && (num3 == 48 || num3 == 49))
				{
					drawData.glowSourceRect = new Rectangle(16 * (num % 2), drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight);
				}
			}
			if (drawData.typeCache == 378)
			{
				Point key2 = new Point(tileX, tileY);
				if (drawData.tileFrameX % 36 != 0)
				{
					key2.X--;
				}
				if (drawData.tileFrameY % 54 != 0)
				{
					key2.Y -= drawData.tileFrameY / 18;
				}
				if (!_trainingDummyTileEntityPositions.ContainsKey(key2))
				{
					_trainingDummyTileEntityPositions[key2] = TETrainingDummy.Find(key2.X, key2.Y);
				}
				if (_trainingDummyTileEntityPositions[key2] != -1)
				{
					int npc = ((TETrainingDummy)TileEntity.ByID[_trainingDummyTileEntityPositions[key2]]).npc;
					if (npc != -1)
					{
						int num6 = Main.npc[npc].frame.Y / 55;
						num6 *= 54;
						num6 += drawData.tileFrameY;
						drawData.addFrY = num6 - drawData.tileFrameY;
					}
				}
			}
			if (drawData.typeCache == 395)
			{
				Point point = new Point(tileX, tileY);
				if (drawData.tileFrameX % 36 != 0)
				{
					point.X--;
				}
				if (drawData.tileFrameY % 36 != 0)
				{
					point.Y--;
				}
				if (!_itemFrameTileEntityPositions.ContainsKey(point))
				{
					_itemFrameTileEntityPositions[point] = TEItemFrame.Find(point.X, point.Y);
					if (_itemFrameTileEntityPositions[point] != -1)
					{
						AddSpecialLegacyPoint(point);
					}
				}
			}
			if (drawData.typeCache == 520)
			{
				Point point2 = new Point(tileX, tileY);
				if (!_foodPlatterTileEntityPositions.ContainsKey(point2))
				{
					_foodPlatterTileEntityPositions[point2] = TEFoodPlatter.Find(point2.X, point2.Y);
					if (_foodPlatterTileEntityPositions[point2] != -1)
					{
						AddSpecialLegacyPoint(point2);
					}
				}
			}
			if (drawData.typeCache == 471)
			{
				Point point3 = new Point(tileX, tileY);
				point3.X -= drawData.tileFrameX % 54 / 18;
				point3.Y -= drawData.tileFrameY % 54 / 18;
				if (!_weaponRackTileEntityPositions.ContainsKey(point3))
				{
					_weaponRackTileEntityPositions[point3] = TEWeaponsRack.Find(point3.X, point3.Y);
					if (_weaponRackTileEntityPositions[point3] != -1)
					{
						AddSpecialLegacyPoint(point3);
					}
				}
			}
			if (drawData.typeCache == 470)
			{
				Point point4 = new Point(tileX, tileY);
				point4.X -= drawData.tileFrameX % 36 / 18;
				point4.Y -= drawData.tileFrameY % 54 / 18;
				if (!_displayDollTileEntityPositions.ContainsKey(point4))
				{
					_displayDollTileEntityPositions[point4] = TEDisplayDoll.Find(point4.X, point4.Y);
					if (_displayDollTileEntityPositions[point4] != -1)
					{
						AddSpecialLegacyPoint(point4);
					}
				}
			}
			if (drawData.typeCache == 475)
			{
				Point point5 = new Point(tileX, tileY);
				point5.X -= drawData.tileFrameX % 54 / 18;
				point5.Y -= drawData.tileFrameY % 72 / 18;
				if (!_hatRackTileEntityPositions.ContainsKey(point5))
				{
					_hatRackTileEntityPositions[point5] = TEHatRack.Find(point5.X, point5.Y);
					if (_hatRackTileEntityPositions[point5] != -1)
					{
						AddSpecialLegacyPoint(point5);
					}
				}
			}
			if (drawData.typeCache == 323 && drawData.tileFrameX <= 132 && drawData.tileFrameX >= 88)
			{
				AddSpecialPoint(tileX, tileY, TileCounterType.Tree);
			}
			if (drawData.typeCache == 412 && drawData.tileFrameX == 0 && drawData.tileFrameY == 0)
			{
				AddSpecialLegacyPoint(tileX, tileY);
			}
			if (drawData.typeCache == 620 && drawData.tileFrameX == 0 && drawData.tileFrameY == 0)
			{
				AddSpecialLegacyPoint(tileX, tileY);
			}
			if (drawData.typeCache == 237 && drawData.tileFrameX == 18 && drawData.tileFrameY == 0)
			{
				AddSpecialLegacyPoint(tileX, tileY);
			}
			switch (drawData.typeCache)
			{
			case 5:
			case 583:
			case 584:
			case 585:
			case 586:
			case 587:
			case 588:
			case 589:
			case 596:
			case 616:
				if (drawData.tileFrameY >= 198 && drawData.tileFrameX >= 22)
				{
					AddSpecialPoint(tileX, tileY, TileCounterType.Tree);
				}
				break;
			}
		}

		private static Color GetFinalLight(Tile tileCache, ushort typeCache, Color tileLight, Color tint)
		{
			int num = (int)((float)(tileLight.R * tint.R) / 255f);
			int num2 = (int)((float)(tileLight.G * tint.G) / 255f);
			int num3 = (int)((float)(tileLight.B * tint.B) / 255f);
			if (num > 255)
			{
				num = 255;
			}
			if (num2 > 255)
			{
				num2 = 255;
			}
			if (num3 > 255)
			{
				num3 = 255;
			}
			num3 <<= 16;
			num2 <<= 8;
			tileLight.PackedValue = (uint)(num | num2 | num3) | 0xFF000000u;
			if (tileCache.color() == 31)
			{
				tileLight = Color.White;
			}
			if (tileCache.inActive())
			{
				tileLight = tileCache.actColor(tileLight);
			}
			else if (ShouldTileShine(typeCache, tileCache.frameX))
			{
				tileLight = Main.shine(tileLight, typeCache);
			}
			return tileLight;
		}

		private static void GetFinalLight(Tile tileCache, ushort typeCache, Vector3 tileLight, Vector3 tint)
		{
			tileLight *= tint;
			if (tileCache.inActive())
			{
				tileCache.actColor(tileLight);
			}
			else if (ShouldTileShine(typeCache, tileCache.frameX))
			{
				Main.shine(tileLight, typeCache);
			}
		}

		private static bool ShouldTileShine(ushort type, short frameX)
		{
			if (!Main.tileShine2[type])
			{
				return false;
			}
			switch (type)
			{
			case 467:
			case 468:
				if (frameX >= 144)
				{
					return frameX < 178;
				}
				return false;
			case 21:
			case 441:
				if (frameX >= 36)
				{
					return frameX < 178;
				}
				return false;
			default:
				return true;
			}
		}

		private static bool IsTileDangerous(Player localPlayer, Tile tileCache, ushort typeCache)
		{
			bool flag = false || typeCache == 135 || typeCache == 137 || typeCache == 138 || typeCache == 484 || typeCache == 141 || typeCache == 210 || typeCache == 442 || typeCache == 443 || typeCache == 444 || typeCache == 411 || typeCache == 485 || typeCache == 85;
			if (tileCache.slope() == 0 && !tileCache.inActive())
			{
				flag = flag || typeCache == 32 || typeCache == 69 || typeCache == 48 || typeCache == 232 || typeCache == 352 || typeCache == 483 || typeCache == 482 || typeCache == 481 || typeCache == 51 || typeCache == 229;
				if (!localPlayer.fireWalk)
				{
					flag = flag || typeCache == 37 || typeCache == 58 || typeCache == 76;
				}
				if (!localPlayer.iceSkate)
				{
					flag = flag || typeCache == 162;
				}
			}
			return flag;
		}

		private bool IsTileDrawLayerSolid(ushort typeCache)
		{
			if (TileID.Sets.DrawTileInSolidLayer[typeCache].HasValue)
			{
				return TileID.Sets.DrawTileInSolidLayer[typeCache].Value;
			}
			return _tileSolid[typeCache];
		}

		private void GetTileOutlineInfo(int x, int y, ushort typeCache, Color tileLight, Texture2D highlightTexture, Color highlightColor)
		{
			if (Main.InSmartCursorHighlightArea(x, y, out var actuallySelected))
			{
				int num = (tileLight.R + tileLight.G + tileLight.B) / 3;
				if (num > 10)
				{
					highlightTexture = TextureAssets.HighlightMask[typeCache].Value;
					highlightColor = Colors.GetSelectionGlowColor(actuallySelected, num);
				}
			}
		}

		private void DrawPartialLiquid(Tile tileCache, Vector2 position, Rectangle liquidSize, int liquidType, Color aColor)
		{
			int num = tileCache.slope();
			if (!TileID.Sets.BlocksWaterDrawingBehindSelf[tileCache.type] || num == 0)
			{
				Main.spriteBatch.Draw(TextureAssets.Liquid[liquidType].Value, position, liquidSize, aColor, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
				return;
			}
			liquidSize.X += 18 * (num - 1);
			if (tileCache.slope() == 1)
			{
				Main.spriteBatch.Draw(TextureAssets.LiquidSlope[liquidType].Value, position, liquidSize, aColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else if (tileCache.slope() == 2)
			{
				Main.spriteBatch.Draw(TextureAssets.LiquidSlope[liquidType].Value, position, liquidSize, aColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else if (tileCache.slope() == 3)
			{
				Main.spriteBatch.Draw(TextureAssets.LiquidSlope[liquidType].Value, position, liquidSize, aColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else if (tileCache.slope() == 4)
			{
				Main.spriteBatch.Draw(TextureAssets.LiquidSlope[liquidType].Value, position, liquidSize, aColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}

		private bool InAPlaceWithWind(int x, int y, int width, int height)
		{
			return WorldGen.InAPlaceWithWind(x, y, width, height);
		}

		private void GetTileDrawData(int x, int y, Tile tileCache, ushort typeCache, short tileFrameX, short tileFrameY, out int tileWidth, out int tileHeight, out int tileTop, out int halfBrickHeight, out int addFrX, out int addFrY, out SpriteEffects tileSpriteEffect, out Texture2D glowTexture, out Rectangle glowSourceRect, out Color glowColor)
		{
			tileTop = 0;
			tileWidth = 16;
			tileHeight = 16;
			halfBrickHeight = 0;
			addFrY = Main.tileFrame[typeCache] * 38;
			addFrX = 0;
			tileSpriteEffect = SpriteEffects.None;
			glowTexture = null;
			glowSourceRect = Rectangle.Empty;
			glowColor = Color.Transparent;
			switch (typeCache)
			{
			case 571:
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
				}
				tileTop = 2;
				break;
			case 136:
				if (tileFrameX == 0)
				{
					tileTop = 2;
				}
				break;
			case 561:
				tileTop -= 2;
				tileHeight = 20;
				addFrY = tileFrameY / 18 * 4;
				break;
			case 518:
			{
				int num40 = (int)tileCache.liquid / 16;
				num40 -= 3;
				if (WorldGen.SolidTile(x, y - 1) && num40 > 8)
				{
					num40 = 8;
				}
				if (tileCache.liquid == 0)
				{
					Tile tileSafely = Framing.GetTileSafely(x, y + 1);
					if (tileSafely.nactive())
					{
						switch (tileSafely.blockType())
						{
						case 1:
							num40 = -16 + Math.Max(8, (int)tileSafely.liquid / 16);
							break;
						case 2:
						case 3:
							num40 -= 4;
							break;
						}
					}
				}
				tileTop -= num40;
				break;
			}
			case 330:
			case 331:
			case 332:
			case 333:
				tileTop += 2;
				break;
			case 129:
				addFrY = 0;
				if (tileFrameX >= 324)
				{
					int num47 = (tileFrameX - 324) / 18;
					int num48 = (num47 + Main.tileFrame[typeCache]) % 6 - num47;
					addFrX = num48 * 18;
				}
				break;
			case 5:
			{
				tileWidth = 20;
				tileHeight = 20;
				int treeBiome = GetTreeBiome(x, y, tileFrameX, tileFrameY);
				tileFrameX += (short)(176 * (treeBiome + 1));
				break;
			}
			case 583:
			case 584:
			case 585:
			case 586:
			case 587:
			case 588:
			case 589:
			case 596:
			case 616:
				tileWidth = 20;
				tileHeight = 20;
				break;
			case 476:
				tileWidth = 20;
				tileHeight = 18;
				break;
			case 323:
			{
				tileWidth = 20;
				tileHeight = 20;
				int palmTreeBiome = GetPalmTreeBiome(x, y);
				tileFrameY = (short)(22 * palmTreeBiome);
				break;
			}
			case 4:
				tileWidth = 20;
				tileHeight = 20;
				if (WorldGen.SolidTile(x, y - 1))
				{
					tileTop = 4;
				}
				break;
			case 78:
			case 85:
			case 100:
			case 133:
			case 134:
			case 173:
			case 210:
			case 233:
			case 254:
			case 283:
			case 378:
			case 457:
			case 466:
			case 520:
				tileTop = 2;
				break;
			case 530:
			{
				int num15 = y - tileFrameY % 36 / 18 + 2;
				int num16 = x - tileFrameX % 54 / 18;
				WorldGen.GetBiomeInfluence(num16, num16 + 3, num15, num15, out var corruptCount, out var crimsonCount, out var hallowedCount);
				int num17 = corruptCount;
				if (num17 < crimsonCount)
				{
					num17 = crimsonCount;
				}
				if (num17 < hallowedCount)
				{
					num17 = hallowedCount;
				}
				int num18 = 0;
				num18 = ((corruptCount != 0 || crimsonCount != 0 || hallowedCount != 0) ? ((hallowedCount == num17) ? 1 : ((crimsonCount != num17) ? 3 : 2)) : 0);
				addFrY += 36 * num18;
				tileTop = 2;
				break;
			}
			case 485:
			{
				tileTop = 2;
				int num3 = Main.tileFrameCounter[typeCache];
				num3 /= 5;
				int num4 = y - tileFrameY / 18;
				int num5 = x - tileFrameX / 18;
				num3 += num4 + num5;
				num3 %= 4;
				addFrY = num3 * 36;
				break;
			}
			case 489:
			{
				tileTop = 2;
				int num34 = y - tileFrameY / 18;
				int num35 = x - tileFrameX / 18;
				if (InAPlaceWithWind(num35, num34, 2, 3))
				{
					int num36 = Main.tileFrameCounter[typeCache];
					num36 /= 5;
					num36 += num34 + num35;
					num36 %= 16;
					addFrY = num36 * 54;
				}
				break;
			}
			case 490:
			{
				tileTop = 2;
				int y2 = y - tileFrameY / 18;
				int x2 = x - tileFrameX / 18;
				bool num25 = InAPlaceWithWind(x2, y2, 2, 2);
				int num26 = (num25 ? Main.tileFrame[typeCache] : 0);
				int num27 = 0;
				if (num25)
				{
					if (Math.Abs(Main.WindForVisuals) > 0.5f)
					{
						switch (Main.weatherVaneBobframe)
						{
						case 0:
							num27 = 0;
							break;
						case 1:
							num27 = 1;
							break;
						case 2:
							num27 = 2;
							break;
						case 3:
							num27 = 1;
							break;
						case 4:
							num27 = 0;
							break;
						case 5:
							num27 = -1;
							break;
						case 6:
							num27 = -2;
							break;
						case 7:
							num27 = -1;
							break;
						}
					}
					else
					{
						switch (Main.weatherVaneBobframe)
						{
						case 0:
							num27 = 0;
							break;
						case 1:
							num27 = 1;
							break;
						case 2:
							num27 = 0;
							break;
						case 3:
							num27 = -1;
							break;
						case 4:
							num27 = 0;
							break;
						case 5:
							num27 = 1;
							break;
						case 6:
							num27 = 0;
							break;
						case 7:
							num27 = -1;
							break;
						}
					}
				}
				num26 += num27;
				if (num26 < 0)
				{
					num26 += 12;
				}
				num26 %= 12;
				addFrY = num26 * 36;
				break;
			}
			case 33:
			case 49:
			case 174:
			case 372:
				tileHeight = 20;
				tileTop = -4;
				break;
			case 529:
			{
				int num29 = y + 1;
				WorldGen.GetBiomeInfluence(x, x, num29, num29, out var corruptCount2, out var crimsonCount2, out var hallowedCount2);
				int num30 = corruptCount2;
				if (num30 < crimsonCount2)
				{
					num30 = crimsonCount2;
				}
				if (num30 < hallowedCount2)
				{
					num30 = hallowedCount2;
				}
				int num31 = 0;
				num31 = ((corruptCount2 == 0 && crimsonCount2 == 0 && hallowedCount2 == 0) ? ((x < WorldGen.beachDistance || x > Main.maxTilesX - WorldGen.beachDistance) ? 1 : 0) : ((hallowedCount2 == num30) ? 2 : ((crimsonCount2 != num30) ? 4 : 3)));
				addFrY += 34 * num31 - tileFrameY;
				tileHeight = 32;
				tileTop = -14;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
				}
				break;
			}
			case 3:
			case 24:
			case 61:
			case 71:
			case 110:
			case 201:
				tileHeight = 20;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
				}
				break;
			case 20:
			case 590:
			case 595:
				tileHeight = 18;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
				}
				break;
			case 615:
				tileHeight = 18;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
				}
				break;
			case 16:
			case 17:
			case 18:
			case 26:
			case 32:
			case 69:
			case 72:
			case 77:
			case 79:
			case 124:
			case 137:
			case 138:
			case 352:
			case 462:
			case 487:
			case 488:
			case 574:
			case 575:
			case 576:
			case 577:
			case 578:
				tileHeight = 18;
				break;
			case 14:
			case 21:
			case 411:
			case 467:
			case 469:
				if (tileFrameY == 18)
				{
					tileHeight = 18;
				}
				break;
			case 15:
			case 497:
				if (tileFrameY % 40 == 18)
				{
					tileHeight = 18;
				}
				break;
			case 172:
			case 376:
				if (tileFrameY % 38 == 18)
				{
					tileHeight = 18;
				}
				break;
			case 27:
				if (tileFrameY % 74 == 54)
				{
					tileHeight = 18;
				}
				break;
			case 132:
			case 135:
				tileTop = 2;
				tileHeight = 18;
				break;
			case 82:
			case 83:
			case 84:
				tileHeight = 20;
				tileTop = -2;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
				}
				break;
			case 324:
				tileWidth = 20;
				tileHeight = 20;
				tileTop = -2;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
				}
				break;
			case 494:
				tileTop = 2;
				break;
			case 52:
			case 62:
			case 115:
			case 205:
			case 382:
			case 528:
				tileTop = -2;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
				}
				break;
			case 80:
			case 142:
			case 143:
				tileTop = 2;
				break;
			case 139:
			{
				tileTop = 2;
				int num28 = tileFrameY / 2016;
				addFrY -= 2016 * num28;
				addFrX += 72 * num28;
				break;
			}
			case 73:
			case 74:
			case 113:
				tileTop = -12;
				tileHeight = 32;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
				}
				break;
			case 388:
			case 389:
			{
				TileObjectData.GetTileData(typeCache, tileFrameX / 18);
				int num24 = 94;
				tileTop = -2;
				if (tileFrameY == num24 - 20 || tileFrameY == num24 * 2 - 20 || tileFrameY == 0 || tileFrameY == num24)
				{
					tileHeight = 18;
				}
				if (tileFrameY != 0 && tileFrameY != num24)
				{
					tileTop = 0;
				}
				break;
			}
			case 227:
				tileWidth = 32;
				tileHeight = 38;
				if (tileFrameX == 238)
				{
					tileTop -= 6;
				}
				else
				{
					tileTop -= 20;
				}
				if (tileFrameX == 204)
				{
					GetCactusType(x, y, tileFrameX, tileFrameY, out var evil, out var good, out var crimson);
					if (good)
					{
						tileFrameX += 238;
					}
					if (evil)
					{
						tileFrameX += 204;
					}
					if (crimson)
					{
						tileFrameX += 272;
					}
				}
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
				}
				break;
			case 579:
			{
				tileWidth = 20;
				tileHeight = 20;
				tileTop -= 2;
				bool flag = (float)(x * 16 + 8) > Main.LocalPlayer.Center.X;
				if (tileFrameX > 0)
				{
					if (flag)
					{
						addFrY = 22;
					}
					else
					{
						addFrY = 0;
					}
				}
				else if (flag)
				{
					addFrY = 0;
				}
				else
				{
					addFrY = 22;
				}
				break;
			}
			case 567:
				tileWidth = 26;
				tileHeight = 18;
				tileTop = 2;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
				}
				break;
			case 185:
			case 186:
			case 187:
				tileTop = 2;
				switch (typeCache)
				{
				case 185:
					if (tileFrameY == 18 && tileFrameX >= 576 && tileFrameX <= 882)
					{
						Main.tileShine2[185] = true;
					}
					else
					{
						Main.tileShine2[185] = false;
					}
					if (tileFrameY == 18)
					{
						int num38 = tileFrameX / 1908;
						addFrX -= 1908 * num38;
						addFrY += 18 * num38;
					}
					break;
				case 186:
					if (tileFrameX >= 864 && tileFrameX <= 1170)
					{
						Main.tileShine2[186] = true;
					}
					else
					{
						Main.tileShine2[186] = false;
					}
					break;
				case 187:
				{
					int num37 = tileFrameX / 1890;
					addFrX -= 1890 * num37;
					addFrY += 36 * num37;
					break;
				}
				}
				break;
			case 178:
				if (tileFrameY <= 36)
				{
					tileTop = 2;
				}
				break;
			case 184:
				tileWidth = 20;
				if (tileFrameY <= 36)
				{
					tileTop = 2;
				}
				else if (tileFrameY <= 108)
				{
					tileTop = -2;
				}
				break;
			case 519:
				tileTop = 2;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
				}
				break;
			case 493:
				if (tileFrameY == 0)
				{
					int num20 = Main.tileFrameCounter[typeCache];
					float num21 = Math.Abs(Main.WindForVisuals);
					int num22 = y - tileFrameY / 18;
					int num23 = x - tileFrameX / 18;
					if (!InAPlaceWithWind(num23, num22, 1, 1))
					{
						num21 = 0f;
					}
					if (!(num21 < 0.1f))
					{
						if (num21 < 0.5f)
						{
							num20 /= 20;
							num20 += num22 + num23;
							num20 %= 6;
							num20 = ((!(Main.WindForVisuals < 0f)) ? (num20 + 1) : (6 - num20));
							addFrY = num20 * 36;
						}
						else
						{
							num20 /= 10;
							num20 += num22 + num23;
							num20 %= 6;
							num20 = ((!(Main.WindForVisuals < 0f)) ? (num20 + 7) : (12 - num20));
							addFrY = num20 * 36;
						}
					}
				}
				tileTop = 2;
				break;
			case 28:
			case 105:
			case 470:
			case 475:
			case 506:
			case 547:
			case 548:
			case 552:
			case 560:
			case 597:
			case 613:
			case 621:
			case 622:
				tileTop = 2;
				break;
			case 617:
				tileTop = 2;
				tileFrameY %= 144;
				tileFrameX %= 54;
				break;
			case 614:
				addFrX = Main.tileFrame[typeCache] * 54;
				addFrY = 0;
				tileTop = 2;
				break;
			case 81:
				tileTop -= 8;
				tileHeight = 26;
				tileWidth = 24;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
				}
				break;
			case 272:
				addFrY = 0;
				break;
			case 106:
				addFrY = Main.tileFrame[typeCache] * 54;
				break;
			case 300:
			case 301:
			case 302:
			case 303:
			case 304:
			case 305:
			case 306:
			case 307:
			case 308:
			case 354:
			case 355:
			case 499:
				addFrY = Main.tileFrame[typeCache] * 54;
				tileTop = 2;
				break;
			case 377:
				addFrY = Main.tileFrame[typeCache] * 38;
				tileTop = 2;
				break;
			case 463:
			case 464:
				addFrY = Main.tileFrame[typeCache] * 72;
				tileTop = 2;
				break;
			case 491:
				tileTop = 2;
				addFrX = 54;
				break;
			case 379:
				addFrY = Main.tileFrame[typeCache] * 90;
				break;
			case 349:
			{
				tileTop = 2;
				int num9 = tileFrameX % 36;
				int num10 = tileFrameY % 54;
				if (Animation.GetTemporaryFrame(x - num9 / 18, y - num10 / 18, out var frameData2))
				{
					tileFrameX = (short)(36 * frameData2 + num9);
				}
				break;
			}
			case 441:
			case 468:
			{
				if (tileFrameY == 18)
				{
					tileHeight = 18;
				}
				int num6 = tileFrameX % 36;
				int num7 = tileFrameY % 38;
				if (Animation.GetTemporaryFrame(x - num6 / 18, y - num7 / 18, out var frameData))
				{
					tileFrameY = (short)(38 * frameData + num7);
				}
				break;
			}
			case 390:
				addFrY = Main.tileFrame[typeCache] * 36;
				break;
			case 412:
				addFrY = 0;
				tileTop = 2;
				break;
			case 406:
			{
				tileHeight = 16;
				if (tileFrameY % 54 >= 36)
				{
					tileHeight = 18;
				}
				int num50 = Main.tileFrame[typeCache];
				if (tileFrameY >= 108)
				{
					num50 = 6 - tileFrameY / 54;
				}
				else if (tileFrameY >= 54)
				{
					num50 = Main.tileFrame[typeCache] - 1;
				}
				addFrY = num50 * 56;
				addFrY += tileFrameY / 54 * 2;
				break;
			}
			case 452:
			{
				int num49 = Main.tileFrame[typeCache];
				if (tileFrameX >= 54)
				{
					num49 = 0;
				}
				addFrY = num49 * 54;
				break;
			}
			case 455:
			{
				addFrY = 0;
				tileTop = 2;
				int num46 = 1 + Main.tileFrame[typeCache];
				if (!BirthdayParty.PartyIsUp)
				{
					num46 = 0;
				}
				addFrY = num46 * 54;
				break;
			}
			case 454:
				addFrY = Main.tileFrame[typeCache] * 54;
				break;
			case 453:
			{
				int num44 = Main.tileFrameCounter[typeCache];
				num44 /= 20;
				int num45 = y - tileFrameY / 18;
				num44 += num45 + x;
				num44 %= 3;
				addFrY = num44 * 54;
				break;
			}
			case 456:
			{
				int num41 = Main.tileFrameCounter[typeCache];
				num41 /= 20;
				int num42 = y - tileFrameY / 18;
				int num43 = x - tileFrameX / 18;
				num41 += num42 + num43;
				num41 %= 4;
				addFrY = num41 * 54;
				break;
			}
			case 405:
			{
				tileHeight = 16;
				if (tileFrameY > 0)
				{
					tileHeight = 18;
				}
				int num39 = Main.tileFrame[typeCache];
				if (tileFrameX >= 54)
				{
					num39 = 0;
				}
				addFrY = num39 * 38;
				break;
			}
			case 12:
			case 31:
			case 96:
				addFrY = Main.tileFrame[typeCache] * 36;
				break;
			case 238:
				tileTop = 2;
				addFrY = Main.tileFrame[typeCache] * 36;
				break;
			case 593:
			{
				if (tileFrameX >= 18)
				{
					addFrX = -18;
				}
				tileTop = 2;
				if (Animation.GetTemporaryFrame(x, y, out var frameData4))
				{
					addFrY = (short)(18 * frameData4);
				}
				else if (tileFrameX < 18)
				{
					addFrY = Main.tileFrame[typeCache] * 18;
				}
				else
				{
					addFrY = 0;
				}
				break;
			}
			case 594:
			{
				if (tileFrameX >= 36)
				{
					addFrX = -36;
				}
				tileTop = 2;
				int num32 = tileFrameX % 36;
				int num33 = tileFrameY % 36;
				if (Animation.GetTemporaryFrame(x - num32 / 18, y - num33 / 18, out var frameData3))
				{
					addFrY = (short)(36 * frameData3);
				}
				else if (tileFrameX < 36)
				{
					addFrY = Main.tileFrame[typeCache] * 36;
				}
				else
				{
					addFrY = 0;
				}
				break;
			}
			case 215:
				if (tileFrameY < 36)
				{
					addFrY = Main.tileFrame[typeCache] * 36;
				}
				else
				{
					addFrY = 252;
				}
				tileTop = 2;
				break;
			case 592:
				addFrY = Main.tileFrame[typeCache] * 54;
				break;
			case 228:
			case 231:
			case 243:
			case 247:
				tileTop = 2;
				addFrY = Main.tileFrame[typeCache] * 54;
				break;
			case 244:
				tileTop = 2;
				if (tileFrameX < 54)
				{
					addFrY = Main.tileFrame[typeCache] * 36;
				}
				else
				{
					addFrY = 0;
				}
				break;
			case 565:
				tileTop = 2;
				if (tileFrameX < 36)
				{
					addFrY = Main.tileFrame[typeCache] * 36;
				}
				else
				{
					addFrY = 0;
				}
				break;
			case 235:
				addFrY = Main.tileFrame[typeCache] * 18;
				break;
			case 217:
			case 218:
			case 564:
				addFrY = Main.tileFrame[typeCache] * 36;
				tileTop = 2;
				break;
			case 219:
			case 220:
				addFrY = Main.tileFrame[typeCache] * 54;
				tileTop = 2;
				break;
			case 270:
			case 271:
			case 581:
			{
				int num19 = Main.tileFrame[typeCache] + x % 6;
				if (x % 2 == 0)
				{
					num19 += 3;
				}
				if (x % 3 == 0)
				{
					num19 += 3;
				}
				if (x % 4 == 0)
				{
					num19 += 3;
				}
				while (num19 > 5)
				{
					num19 -= 6;
				}
				addFrX = num19 * 18;
				addFrY = 0;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
				}
				break;
			}
			case 572:
			{
				int num14;
				for (num14 = Main.tileFrame[typeCache] + x % 6; num14 > 3; num14 -= 3)
				{
				}
				addFrX = num14 * 18;
				addFrY = 0;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
				}
				break;
			}
			case 428:
				tileTop += 4;
				if (PressurePlateHelper.PressurePlatesPressed.ContainsKey(new Point(x, y)))
				{
					addFrX += 18;
				}
				break;
			case 442:
				tileWidth = 20;
				tileHeight = 20;
				switch (tileFrameX / 22)
				{
				case 1:
					tileTop = -4;
					break;
				case 2:
					tileTop = -2;
					tileWidth = 24;
					break;
				case 3:
					tileTop = -2;
					break;
				}
				break;
			case 426:
			case 430:
			case 431:
			case 432:
			case 433:
			case 434:
				addFrY = 90;
				break;
			case 275:
			case 276:
			case 277:
			case 278:
			case 279:
			case 280:
			case 281:
			case 296:
			case 297:
			case 309:
			case 358:
			case 359:
			case 413:
			case 414:
			case 542:
			case 550:
			case 551:
			case 553:
			case 554:
			case 558:
			case 559:
			case 599:
			case 600:
			case 601:
			case 602:
			case 603:
			case 604:
			case 605:
			case 606:
			case 607:
			case 608:
			case 609:
			case 610:
			case 611:
			case 612:
			{
				tileTop = 2;
				Main.critterCage = true;
				int bigAnimalCageFrame = GetBigAnimalCageFrame(x, y, tileFrameX, tileFrameY);
				switch (typeCache)
				{
				case 275:
				case 359:
				case 599:
				case 600:
				case 601:
				case 602:
				case 603:
				case 604:
				case 605:
					addFrY = Main.bunnyCageFrame[bigAnimalCageFrame] * 54;
					break;
				case 550:
				case 551:
					addFrY = Main.turtleCageFrame[bigAnimalCageFrame] * 54;
					break;
				case 542:
					addFrY = Main.owlCageFrame[bigAnimalCageFrame] * 54;
					break;
				case 276:
				case 413:
				case 414:
				case 606:
				case 607:
				case 608:
				case 609:
				case 610:
				case 611:
				case 612:
					addFrY = Main.squirrelCageFrame[bigAnimalCageFrame] * 54;
					break;
				case 277:
					addFrY = Main.mallardCageFrame[bigAnimalCageFrame] * 54;
					break;
				case 278:
					addFrY = Main.duckCageFrame[bigAnimalCageFrame] * 54;
					break;
				case 553:
					addFrY = Main.grebeCageFrame[bigAnimalCageFrame] * 54;
					break;
				case 554:
					addFrY = Main.seagullCageFrame[bigAnimalCageFrame] * 54;
					break;
				case 279:
				case 358:
					addFrY = Main.birdCageFrame[bigAnimalCageFrame] * 54;
					break;
				case 280:
					addFrY = Main.blueBirdCageFrame[bigAnimalCageFrame] * 54;
					break;
				case 281:
					addFrY = Main.redBirdCageFrame[bigAnimalCageFrame] * 54;
					break;
				case 296:
				case 297:
					addFrY = Main.scorpionCageFrame[0, bigAnimalCageFrame] * 54;
					break;
				case 309:
					addFrY = Main.penguinCageFrame[bigAnimalCageFrame] * 54;
					break;
				case 558:
				case 559:
					addFrY = Main.seahorseCageFrame[bigAnimalCageFrame] * 54;
					break;
				}
				break;
			}
			case 285:
			case 286:
			case 298:
			case 299:
			case 310:
			case 339:
			case 361:
			case 362:
			case 363:
			case 364:
			case 391:
			case 392:
			case 393:
			case 394:
			case 532:
			case 533:
			case 538:
			case 544:
			case 555:
			case 556:
			case 582:
			case 619:
			{
				tileTop = 2;
				Main.critterCage = true;
				int smallAnimalCageFrame2 = GetSmallAnimalCageFrame(x, y, tileFrameX, tileFrameY);
				switch (typeCache)
				{
				case 285:
					addFrY = Main.snailCageFrame[smallAnimalCageFrame2] * 36;
					break;
				case 286:
				case 582:
					addFrY = Main.snail2CageFrame[smallAnimalCageFrame2] * 36;
					break;
				case 298:
				case 361:
					addFrY = Main.frogCageFrame[smallAnimalCageFrame2] * 36;
					break;
				case 339:
				case 362:
					addFrY = Main.grasshopperCageFrame[smallAnimalCageFrame2] * 36;
					break;
				case 299:
				case 363:
					addFrY = Main.mouseCageFrame[smallAnimalCageFrame2] * 36;
					break;
				case 310:
				case 364:
				case 391:
				case 619:
					addFrY = Main.wormCageFrame[smallAnimalCageFrame2] * 36;
					break;
				case 392:
				case 393:
				case 394:
					addFrY = Main.slugCageFrame[typeCache - 392, smallAnimalCageFrame2] * 36;
					break;
				case 532:
					addFrY = Main.maggotCageFrame[smallAnimalCageFrame2] * 36;
					break;
				case 533:
					addFrY = Main.ratCageFrame[smallAnimalCageFrame2] * 36;
					break;
				case 538:
				case 544:
					addFrY = Main.ladybugCageFrame[smallAnimalCageFrame2] * 36;
					break;
				case 555:
				case 556:
					addFrY = Main.waterStriderCageFrame[smallAnimalCageFrame2] * 36;
					break;
				}
				break;
			}
			case 282:
			case 505:
			case 543:
			{
				tileTop = 2;
				Main.critterCage = true;
				int waterAnimalCageFrame5 = GetWaterAnimalCageFrame(x, y, tileFrameX, tileFrameY);
				addFrY = Main.fishBowlFrame[waterAnimalCageFrame5] * 36;
				break;
			}
			case 598:
			{
				tileTop = 2;
				Main.critterCage = true;
				int waterAnimalCageFrame4 = GetWaterAnimalCageFrame(x, y, tileFrameX, tileFrameY);
				addFrY = Main.lavaFishBowlFrame[waterAnimalCageFrame4] * 36;
				break;
			}
			case 568:
			case 569:
			case 570:
			{
				tileTop = 2;
				Main.critterCage = true;
				int waterAnimalCageFrame3 = GetWaterAnimalCageFrame(x, y, tileFrameX, tileFrameY);
				addFrY = Main.fairyJarFrame[waterAnimalCageFrame3] * 36;
				break;
			}
			case 288:
			case 289:
			case 290:
			case 291:
			case 292:
			case 293:
			case 294:
			case 295:
			case 360:
			case 580:
			case 620:
			{
				tileTop = 2;
				Main.critterCage = true;
				int waterAnimalCageFrame2 = GetWaterAnimalCageFrame(x, y, tileFrameX, tileFrameY);
				int num13 = typeCache - 288;
				if (typeCache == 360 || typeCache == 580 || typeCache == 620)
				{
					num13 = 8;
				}
				addFrY = Main.butterflyCageFrame[num13, waterAnimalCageFrame2] * 36;
				break;
			}
			case 521:
			case 522:
			case 523:
			case 524:
			case 525:
			case 526:
			case 527:
			{
				tileTop = 2;
				Main.critterCage = true;
				int waterAnimalCageFrame = GetWaterAnimalCageFrame(x, y, tileFrameX, tileFrameY);
				int num12 = typeCache - 521;
				addFrY = Main.dragonflyJarFrame[num12, waterAnimalCageFrame] * 36;
				break;
			}
			case 316:
			case 317:
			case 318:
			{
				tileTop = 2;
				Main.critterCage = true;
				int smallAnimalCageFrame = GetSmallAnimalCageFrame(x, y, tileFrameX, tileFrameY);
				int num11 = typeCache - 316;
				addFrY = Main.jellyfishCageFrame[num11, smallAnimalCageFrame] * 36;
				break;
			}
			case 207:
				tileTop = 2;
				if (tileFrameY >= 72)
				{
					addFrY = Main.tileFrame[typeCache];
					int num8 = x;
					if (tileFrameX % 36 != 0)
					{
						num8--;
					}
					addFrY += num8 % 6;
					if (addFrY >= 6)
					{
						addFrY -= 6;
					}
					addFrY *= 72;
				}
				else
				{
					addFrY = 0;
				}
				break;
			case 410:
				if (tileFrameY == 36)
				{
					tileHeight = 18;
				}
				if (tileFrameY >= 56)
				{
					addFrY = Main.tileFrame[typeCache];
					addFrY *= 56;
				}
				else
				{
					addFrY = 0;
				}
				break;
			case 480:
			case 509:
				if (tileFrameY >= 54)
				{
					addFrY = Main.tileFrame[typeCache];
					addFrY *= 54;
				}
				else
				{
					addFrY = 0;
				}
				break;
			case 326:
			case 327:
			case 328:
			case 329:
			case 345:
			case 351:
			case 421:
			case 422:
			case 458:
			case 459:
				addFrY = Main.tileFrame[typeCache] * 90;
				break;
			case 541:
				addFrY = ((!_shouldShowInvisibleBlocks) ? 90 : 0);
				break;
			case 507:
			case 508:
			{
				int num = 20;
				int num2 = (Main.tileFrameCounter[typeCache] + x * 11 + y * 27) % (num * 8);
				addFrY = 90 * (num2 / num);
				break;
			}
			case 336:
			case 340:
			case 341:
			case 342:
			case 343:
			case 344:
				addFrY = Main.tileFrame[typeCache] * 90;
				tileTop = 2;
				break;
			case 89:
				tileTop = 2;
				break;
			case 102:
				tileTop = 2;
				break;
			}
			if (tileCache.halfBrick())
			{
				halfBrickHeight = 8;
			}
			switch (typeCache)
			{
			case 568:
				glowTexture = TextureAssets.GlowMask[268].Value;
				glowSourceRect = new Rectangle(tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
				glowColor = Color.White;
				break;
			case 569:
				glowTexture = TextureAssets.GlowMask[269].Value;
				glowSourceRect = new Rectangle(tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
				glowColor = Color.White;
				break;
			case 570:
				glowTexture = TextureAssets.GlowMask[270].Value;
				glowSourceRect = new Rectangle(tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
				glowColor = Color.White;
				break;
			case 580:
				glowTexture = TextureAssets.GlowMask[289].Value;
				glowSourceRect = new Rectangle(tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
				glowColor = new Color(225, 110, 110, 0);
				break;
			case 564:
				if (tileCache.frameX < 36)
				{
					glowTexture = TextureAssets.GlowMask[267].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
					glowColor = new Color(200, 200, 200, 0) * ((float)(int)Main.mouseTextColor / 255f);
				}
				addFrY = 0;
				break;
			case 184:
				if (tileCache.frameX == 110)
				{
					glowTexture = TextureAssets.GlowMask[127].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY, tileWidth, tileHeight);
					glowColor = _lavaMossGlow;
				}
				if (tileCache.frameX == 132)
				{
					glowTexture = TextureAssets.GlowMask[127].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY, tileWidth, tileHeight);
					glowColor = _kryptonMossGlow;
				}
				if (tileCache.frameX == 154)
				{
					glowTexture = TextureAssets.GlowMask[127].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY, tileWidth, tileHeight);
					glowColor = _xenonMossGlow;
				}
				if (tileCache.frameX == 176)
				{
					glowTexture = TextureAssets.GlowMask[127].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY, tileWidth, tileHeight);
					glowColor = _argonMossGlow;
				}
				break;
			case 463:
				glowTexture = TextureAssets.GlowMask[243].Value;
				glowSourceRect = new Rectangle(tileFrameX, tileFrameY + addFrY, tileWidth, tileHeight);
				glowColor = new Color(127, 127, 127, 0);
				break;
			case 19:
			{
				int num71 = tileFrameY / 18;
				if (num71 == 26)
				{
					glowTexture = TextureAssets.GlowMask[65].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY % 18, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				if (num71 == 27)
				{
					glowTexture = TextureAssets.GlowMask[112].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY % 18, tileWidth, tileHeight);
					glowColor = _meteorGlow;
				}
				break;
			}
			case 90:
			{
				int num64 = tileFrameY / 36;
				if (num64 == 27)
				{
					glowTexture = TextureAssets.GlowMask[52].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY % 36, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				if (num64 == 28)
				{
					glowTexture = TextureAssets.GlowMask[113].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY % 36, tileWidth, tileHeight);
					glowColor = _meteorGlow;
				}
				break;
			}
			case 79:
			{
				int num59 = tileFrameY / 36;
				if (num59 == 27)
				{
					glowTexture = TextureAssets.GlowMask[53].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY % 36, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				if (num59 == 28)
				{
					glowTexture = TextureAssets.GlowMask[114].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY % 36, tileWidth, tileHeight);
					glowColor = _meteorGlow;
				}
				break;
			}
			case 89:
			{
				int num67 = tileFrameX / 54;
				int num68 = tileFrameX / 1998;
				addFrX -= 1998 * num68;
				addFrY += 36 * num68;
				if (num67 == 29)
				{
					glowTexture = TextureAssets.GlowMask[66].Value;
					glowSourceRect = new Rectangle(tileFrameX % 54, tileFrameY, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				if (num67 == 30)
				{
					glowTexture = TextureAssets.GlowMask[123].Value;
					glowSourceRect = new Rectangle(tileFrameX % 54, tileFrameY, tileWidth, tileHeight);
					glowColor = _meteorGlow;
				}
				break;
			}
			case 100:
				if (tileFrameX / 36 == 0 && tileFrameY / 36 == 27)
				{
					glowTexture = TextureAssets.GlowMask[68].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY % 36, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				break;
			case 33:
				if (tileFrameX / 18 == 0 && tileFrameY / 22 == 26)
				{
					glowTexture = TextureAssets.GlowMask[61].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY % 22, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				break;
			case 15:
			{
				int num52 = tileFrameY / 40;
				if (num52 == 32)
				{
					glowTexture = TextureAssets.GlowMask[54].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY % 40, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				if (num52 == 33)
				{
					glowTexture = TextureAssets.GlowMask[116].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY % 40, tileWidth, tileHeight);
					glowColor = _meteorGlow;
				}
				break;
			}
			case 34:
				if (tileFrameX / 54 == 0 && tileFrameY / 54 == 33)
				{
					glowTexture = TextureAssets.GlowMask[55].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY % 54, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				break;
			case 21:
			case 467:
			{
				int num66 = tileFrameX / 36;
				if (num66 == 48)
				{
					glowTexture = TextureAssets.GlowMask[56].Value;
					glowSourceRect = new Rectangle(tileFrameX % 36, tileFrameY, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				if (num66 == 49)
				{
					glowTexture = TextureAssets.GlowMask[117].Value;
					glowSourceRect = new Rectangle(tileFrameX % 36, tileFrameY, tileWidth, tileHeight);
					glowColor = _meteorGlow;
				}
				break;
			}
			case 441:
			case 468:
			{
				int num61 = tileFrameX / 36;
				if (num61 == 48)
				{
					glowTexture = TextureAssets.GlowMask[56].Value;
					glowSourceRect = new Rectangle(tileFrameX % 36, tileFrameY, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				if (num61 == 49)
				{
					glowTexture = TextureAssets.GlowMask[117].Value;
					glowSourceRect = new Rectangle(tileFrameX % 36, tileFrameY, tileWidth, tileHeight);
					glowColor = _meteorGlow;
				}
				break;
			}
			case 10:
				if (tileFrameY / 54 == 32)
				{
					glowTexture = TextureAssets.GlowMask[57].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY % 54, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				break;
			case 11:
			{
				int num56 = tileFrameY / 54;
				if (num56 == 32)
				{
					glowTexture = TextureAssets.GlowMask[58].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY % 54, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				if (num56 == 33)
				{
					glowTexture = TextureAssets.GlowMask[119].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY % 54, tileWidth, tileHeight);
					glowColor = _meteorGlow;
				}
				break;
			}
			case 88:
			{
				int num53 = tileFrameX / 54;
				int num54 = tileFrameX / 1998;
				addFrX -= 1998 * num54;
				addFrY += 36 * num54;
				if (num53 == 24)
				{
					glowTexture = TextureAssets.GlowMask[59].Value;
					glowSourceRect = new Rectangle(tileFrameX % 54, tileFrameY, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				if (num53 == 25)
				{
					glowTexture = TextureAssets.GlowMask[120].Value;
					glowSourceRect = new Rectangle(tileFrameX % 54, tileFrameY, tileWidth, tileHeight);
					glowColor = _meteorGlow;
				}
				break;
			}
			case 42:
				if (tileFrameY / 36 == 33)
				{
					glowTexture = TextureAssets.GlowMask[63].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY % 36, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				break;
			case 87:
			{
				int num69 = tileFrameX / 54;
				int num70 = tileFrameX / 1998;
				addFrX -= 1998 * num70;
				addFrY += 36 * num70;
				if (num69 == 26)
				{
					glowTexture = TextureAssets.GlowMask[64].Value;
					glowSourceRect = new Rectangle(tileFrameX % 54, tileFrameY, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				if (num69 == 27)
				{
					glowTexture = TextureAssets.GlowMask[121].Value;
					glowSourceRect = new Rectangle(tileFrameX % 54, tileFrameY, tileWidth, tileHeight);
					glowColor = _meteorGlow;
				}
				break;
			}
			case 14:
			{
				int num65 = tileFrameX / 54;
				if (num65 == 31)
				{
					glowTexture = TextureAssets.GlowMask[67].Value;
					glowSourceRect = new Rectangle(tileFrameX % 54, tileFrameY, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				if (num65 == 32)
				{
					glowTexture = TextureAssets.GlowMask[124].Value;
					glowSourceRect = new Rectangle(tileFrameX % 54, tileFrameY, tileWidth, tileHeight);
					glowColor = _meteorGlow;
				}
				break;
			}
			case 93:
			{
				int num62 = tileFrameY / 54;
				int num63 = tileFrameY / 1998;
				addFrY -= 1998 * num63;
				addFrX += 36 * num63;
				tileTop += 2;
				if (num62 == 27)
				{
					glowTexture = TextureAssets.GlowMask[62].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY % 54, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				break;
			}
			case 18:
			{
				int num60 = tileFrameX / 36;
				if (num60 == 27)
				{
					glowTexture = TextureAssets.GlowMask[69].Value;
					glowSourceRect = new Rectangle(tileFrameX % 36, tileFrameY, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				if (num60 == 28)
				{
					glowTexture = TextureAssets.GlowMask[125].Value;
					glowSourceRect = new Rectangle(tileFrameX % 36, tileFrameY, tileWidth, tileHeight);
					glowColor = _meteorGlow;
				}
				break;
			}
			case 101:
			{
				int num57 = tileFrameX / 54;
				int num58 = tileFrameX / 1998;
				addFrX -= 1998 * num58;
				addFrY += 72 * num58;
				if (num57 == 28)
				{
					glowTexture = TextureAssets.GlowMask[60].Value;
					glowSourceRect = new Rectangle(tileFrameX % 54, tileFrameY, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				if (num57 == 29)
				{
					glowTexture = TextureAssets.GlowMask[115].Value;
					glowSourceRect = new Rectangle(tileFrameX % 54, tileFrameY, tileWidth, tileHeight);
					glowColor = _meteorGlow;
				}
				break;
			}
			case 104:
			{
				int num55 = tileFrameX / 36;
				tileTop = 2;
				if (num55 == 24)
				{
					glowTexture = TextureAssets.GlowMask[51].Value;
					glowSourceRect = new Rectangle(tileFrameX % 36, tileFrameY, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				if (num55 == 25)
				{
					glowTexture = TextureAssets.GlowMask[118].Value;
					glowSourceRect = new Rectangle(tileFrameX % 36, tileFrameY, tileWidth, tileHeight);
					glowColor = _meteorGlow;
				}
				break;
			}
			case 172:
			{
				int num51 = tileFrameY / 38;
				if (num51 == 28)
				{
					glowTexture = TextureAssets.GlowMask[88].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY % 38, tileWidth, tileHeight);
					glowColor = _martianGlow;
				}
				if (num51 == 29)
				{
					glowTexture = TextureAssets.GlowMask[122].Value;
					glowSourceRect = new Rectangle(tileFrameX, tileFrameY % 38, tileWidth, tileHeight);
					glowColor = _meteorGlow;
				}
				break;
			}
			}
		}

		private bool IsWindBlocked(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			if (tile == null)
			{
				return true;
			}
			if (tile.wall > 0 && !WallID.Sets.AllowsWind[tile.wall])
			{
				return true;
			}
			if ((double)y > Main.worldSurface)
			{
				return true;
			}
			return false;
		}

		private int GetWaterAnimalCageFrame(int x, int y, int tileFrameX, int tileFrameY)
		{
			int num = x - tileFrameX / 18;
			int num2 = y - tileFrameY / 18;
			return num / 2 * (num2 / 3) % Main.cageFrames;
		}

		private int GetSmallAnimalCageFrame(int x, int y, int tileFrameX, int tileFrameY)
		{
			int num = x - tileFrameX / 18;
			int num2 = y - tileFrameY / 18;
			return num / 3 * (num2 / 3) % Main.cageFrames;
		}

		private int GetBigAnimalCageFrame(int x, int y, int tileFrameX, int tileFrameY)
		{
			int num = x - tileFrameX / 18;
			int num2 = y - tileFrameY / 18;
			return num / 6 * (num2 / 4) % Main.cageFrames;
		}

		private void GetScreenDrawArea(Vector2 screenPosition, Vector2 offSet, out int firstTileX, out int lastTileX, out int firstTileY, out int lastTileY)
		{
			firstTileX = (int)((screenPosition.X - offSet.X) / 16f - 1f);
			lastTileX = (int)((screenPosition.X + (float)Main.screenWidth + offSet.X) / 16f) + 2;
			firstTileY = (int)((screenPosition.Y - offSet.Y) / 16f - 1f);
			lastTileY = (int)((screenPosition.Y + (float)Main.screenHeight + offSet.Y) / 16f) + 5;
			if (firstTileX < 4)
			{
				firstTileX = 4;
			}
			if (lastTileX > Main.maxTilesX - 4)
			{
				lastTileX = Main.maxTilesX - 4;
			}
			if (firstTileY < 4)
			{
				firstTileY = 4;
			}
			if (lastTileY > Main.maxTilesY - 4)
			{
				lastTileY = Main.maxTilesY - 4;
			}
			if (Main.sectionManager.FrameSectionsLeft > 0)
			{
				TimeLogger.DetailedDrawReset();
				WorldGen.SectionTileFrameWithCheck(firstTileX, firstTileY, lastTileX, lastTileY);
				TimeLogger.DetailedDrawTime(5);
			}
		}

		public void ClearCachedTileDraws(bool solidLayer)
		{
			if (solidLayer)
			{
				_displayDollTileEntityPositions.Clear();
				_hatRackTileEntityPositions.Clear();
				_vineRootsPositions.Clear();
				_reverseVineRootsPositions.Clear();
			}
		}

		private void AddSpecialLegacyPoint(Point p)
		{
			AddSpecialLegacyPoint(p.X, p.Y);
		}

		private void AddSpecialLegacyPoint(int x, int y)
		{
			_specialTileX[_specialTilesCount] = x;
			_specialTileY[_specialTilesCount] = y;
			_specialTilesCount++;
		}

		private void ClearLegacyCachedDraws()
		{
			_chestPositions.Clear();
			_trainingDummyTileEntityPositions.Clear();
			_foodPlatterTileEntityPositions.Clear();
			_itemFrameTileEntityPositions.Clear();
			_weaponRackTileEntityPositions.Clear();
			_specialTilesCount = 0;
		}

		private Color DrawTiles_GetLightOverride(int j, int i, Tile tileCache, ushort typeCache, short tileFrameX, short tileFrameY, Color tileLight)
		{
			if (tileCache.color() == 31)
			{
				return Color.White;
			}
			switch (typeCache)
			{
			case 541:
				return Color.White;
			case 83:
			{
				int num = tileFrameX / 18;
				if (!IsAlchemyPlantHarvestable(num))
				{
					break;
				}
				if (num == 5)
				{
					tileLight.A = (byte)((int)Main.mouseTextColor / 2);
					tileLight.G = Main.mouseTextColor;
					tileLight.B = Main.mouseTextColor;
				}
				if (num == 6)
				{
					byte b6 = (byte)((Main.mouseTextColor + tileLight.G * 2) / 3);
					byte b7 = (byte)((Main.mouseTextColor + tileLight.B * 2) / 3);
					if (b6 > tileLight.G)
					{
						tileLight.G = b6;
					}
					if (b7 > tileLight.B)
					{
						tileLight.B = b7;
					}
				}
				break;
			}
			case 61:
				if (tileFrameX == 144)
				{
					byte b2 = (tileLight.B = (byte)(245f - (float)(int)Main.mouseTextColor * 1.5f));
					byte b4 = (tileLight.G = b2);
					byte a = (tileLight.R = b4);
					tileLight.A = a;
				}
				break;
			}
			return tileLight;
		}

		private void DrawTiles_EmitParticles(int j, int i, Tile tileCache, ushort typeCache, short tileFrameX, short tileFrameY, Color tileLight)
		{
			switch (typeCache)
			{
			case 238:
				if (_rand.Next(10) == 0)
				{
					int num = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 168);
					_dust[num].noGravity = true;
					_dust[num].alpha = 200;
				}
				break;
			case 463:
			{
				if (tileFrameY == 54 && tileFrameX == 0)
				{
					for (int l = 0; l < 4; l++)
					{
						if (_rand.Next(2) != 0)
						{
							Dust dust2 = Dust.NewDustDirect(new Vector2(i * 16 + 4, j * 16), 36, 8, 16);
							dust2.noGravity = true;
							dust2.alpha = 140;
							dust2.fadeIn = 1.2f;
							dust2.velocity = Vector2.Zero;
						}
					}
				}
				if (tileFrameY != 18 || (tileFrameX != 0 && tileFrameX != 36))
				{
					break;
				}
				for (int m = 0; m < 1; m++)
				{
					if (_rand.Next(13) == 0)
					{
						Dust dust3 = Dust.NewDustDirect(new Vector2(i * 16, j * 16), 8, 8, 274);
						dust3.position = new Vector2(i * 16 + 8, j * 16 + 8);
						dust3.position.X += ((tileFrameX == 36) ? 4 : (-4));
						dust3.noGravity = true;
						dust3.alpha = 128;
						dust3.fadeIn = 1.2f;
						dust3.noLight = true;
						dust3.velocity = new Vector2(0f, _rand.NextFloatDirection() * 1.2f);
					}
				}
				break;
			}
			case 497:
			{
				if (tileCache.frameY / 40 != 31 || tileCache.frameY % 40 != 0)
				{
					break;
				}
				for (int k = 0; k < 1; k++)
				{
					if (_rand.Next(10) == 0)
					{
						Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16 + 8), 16, 12, 43);
						dust.noGravity = true;
						dust.alpha = 254;
						dust.color = Color.White;
						dust.scale = 0.7f;
						dust.velocity = Vector2.Zero;
						dust.noLight = true;
					}
				}
				break;
			}
			}
			if (typeCache == 139 && tileCache.frameX == 36 && tileCache.frameY % 36 == 0 && (int)Main.timeForVisualEffects % 7 == 0 && _rand.Next(3) == 0)
			{
				int num2 = _rand.Next(570, 573);
				Vector2 position = new Vector2(i * 16 + 8, j * 16 - 8);
				Vector2 velocity = new Vector2(Main.WindForVisuals * 2f, -0.5f);
				velocity.X *= 1f + (float)_rand.Next(-50, 51) * 0.01f;
				velocity.Y *= 1f + (float)_rand.Next(-50, 51) * 0.01f;
				if (num2 == 572)
				{
					position.X -= 8f;
				}
				if (num2 == 571)
				{
					position.X -= 4f;
				}
				Gore.NewGore(position, velocity, num2, 0.8f);
			}
			if (typeCache == 244 && tileFrameX == 18 && tileFrameY == 18 && _rand.Next(2) == 0)
			{
				if (_rand.Next(500) == 0)
				{
					Gore.NewGore(new Vector2(i * 16 + 8, j * 16 + 8), default(Vector2), 415, (float)_rand.Next(51, 101) * 0.01f);
				}
				else if (_rand.Next(250) == 0)
				{
					Gore.NewGore(new Vector2(i * 16 + 8, j * 16 + 8), default(Vector2), 414, (float)_rand.Next(51, 101) * 0.01f);
				}
				else if (_rand.Next(80) == 0)
				{
					Gore.NewGore(new Vector2(i * 16 + 8, j * 16 + 8), default(Vector2), 413, (float)_rand.Next(51, 101) * 0.01f);
				}
				else if (_rand.Next(10) == 0)
				{
					Gore.NewGore(new Vector2(i * 16 + 8, j * 16 + 8), default(Vector2), 412, (float)_rand.Next(51, 101) * 0.01f);
				}
				else if (_rand.Next(3) == 0)
				{
					Gore.NewGore(new Vector2(i * 16 + 8, j * 16 + 8), default(Vector2), 411, (float)_rand.Next(51, 101) * 0.01f);
				}
			}
			if (typeCache == 565 && tileFrameX == 0 && tileFrameY == 18 && _rand.Next(3) == 0 && ((Main.drawToScreen && _rand.Next(4) == 0) || !Main.drawToScreen))
			{
				Vector2 value = new Point(i, j).ToWorldCoordinates();
				int type = 1202;
				float scale = 8f + Main.rand.NextFloat() * 1.6f;
				Vector2 position2 = value + new Vector2(0f, -18f);
				Vector2 velocity2 = Main.rand.NextVector2Circular(0.7f, 0.25f) * 0.4f + Main.rand.NextVector2CircularEdge(1f, 0.4f) * 0.1f;
				velocity2 *= 4f;
				Gore.NewGorePerfect(position2, velocity2, type, scale);
			}
			if (typeCache == 165 && tileFrameX >= 162 && tileFrameX <= 214 && tileFrameY == 72 && _rand.Next(60) == 0)
			{
				int num3 = Dust.NewDust(new Vector2(i * 16 + 2, j * 16 + 6), 8, 4, 153);
				_dust[num3].scale -= (float)_rand.Next(3) * 0.1f;
				_dust[num3].velocity.Y = 0f;
				_dust[num3].velocity.X *= 0.05f;
				_dust[num3].alpha = 100;
			}
			if (typeCache == 42 && tileFrameX == 0)
			{
				int num4 = tileFrameY / 36;
				int num5 = tileFrameY / 18 % 2;
				if (num4 == 7 && num5 == 1)
				{
					if (_rand.Next(50) == 0)
					{
						int num6 = Dust.NewDust(new Vector2(i * 16 + 4, j * 16 + 4), 8, 8, 58, 0f, 0f, 150);
						_dust[num6].velocity *= 0.5f;
					}
					if (_rand.Next(100) == 0)
					{
						int num7 = Gore.NewGore(new Vector2(i * 16 - 2, j * 16 - 4), default(Vector2), _rand.Next(16, 18));
						_gore[num7].scale *= 0.7f;
						_gore[num7].velocity *= 0.25f;
					}
				}
				else if (num4 == 29 && num5 == 1 && _rand.Next(40) == 0)
				{
					int num8 = Dust.NewDust(new Vector2(i * 16 + 4, j * 16), 8, 8, 59, 0f, 0f, 100);
					if (_rand.Next(3) != 0)
					{
						_dust[num8].noGravity = true;
					}
					_dust[num8].velocity *= 0.3f;
					_dust[num8].velocity.Y -= 1.5f;
				}
			}
			if (typeCache == 215 && tileFrameY < 36 && _rand.Next(3) == 0 && ((Main.drawToScreen && _rand.Next(4) == 0) || !Main.drawToScreen) && tileFrameY == 0)
			{
				int num9 = Dust.NewDust(new Vector2(i * 16 + 2, j * 16 - 4), 4, 8, 31, 0f, 0f, 100);
				if (tileFrameX == 0)
				{
					_dust[num9].position.X += _rand.Next(8);
				}
				if (tileFrameX == 36)
				{
					_dust[num9].position.X -= _rand.Next(8);
				}
				_dust[num9].alpha += _rand.Next(100);
				_dust[num9].velocity *= 0.2f;
				_dust[num9].velocity.Y -= 0.5f + (float)_rand.Next(10) * 0.1f;
				_dust[num9].fadeIn = 0.5f + (float)_rand.Next(10) * 0.1f;
			}
			if (typeCache == 592 && tileFrameY == 18 && _rand.Next(3) == 0 && ((Main.drawToScreen && _rand.Next(6) == 0) || !Main.drawToScreen))
			{
				int num10 = Dust.NewDust(new Vector2(i * 16 + 2, j * 16 + 4), 4, 8, 31, 0f, 0f, 100);
				if (tileFrameX == 0)
				{
					_dust[num10].position.X += _rand.Next(8);
				}
				if (tileFrameX == 36)
				{
					_dust[num10].position.X -= _rand.Next(8);
				}
				_dust[num10].alpha += _rand.Next(100);
				_dust[num10].velocity *= 0.2f;
				_dust[num10].velocity.Y -= 0.5f + (float)_rand.Next(10) * 0.1f;
				_dust[num10].fadeIn = 0.5f + (float)_rand.Next(10) * 0.1f;
			}
			if (typeCache == 4 && _rand.Next(40) == 0 && tileFrameX < 66)
			{
				int num11 = tileFrameY / 22;
				num11 = num11 switch
				{
					0 => 6, 
					8 => 75, 
					9 => 135, 
					10 => 158, 
					11 => 169, 
					12 => 156, 
					13 => 234, 
					14 => 66, 
					15 => 242, 
					16 => 293, 
					17 => 294, 
					_ => 58 + num11, 
				};
				int num12 = 0;
				num12 = tileFrameX switch
				{
					22 => Dust.NewDust(new Vector2(i * 16 + 6, j * 16), 4, 4, num11, 0f, 0f, 100), 
					44 => Dust.NewDust(new Vector2(i * 16 + 2, j * 16), 4, 4, num11, 0f, 0f, 100), 
					_ => Dust.NewDust(new Vector2(i * 16 + 4, j * 16), 4, 4, num11, 0f, 0f, 100), 
				};
				if (_rand.Next(3) != 0)
				{
					_dust[num12].noGravity = true;
				}
				_dust[num12].velocity *= 0.3f;
				_dust[num12].velocity.Y -= 1.5f;
				if (num11 == 66)
				{
					_dust[num12].color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
					_dust[num12].noGravity = true;
				}
			}
			if (typeCache == 93 && _rand.Next(40) == 0 && tileFrameX == 0)
			{
				int num13 = tileFrameY / 54;
				if (tileFrameY / 18 % 3 == 0)
				{
					int num14;
					switch (num13)
					{
					case 0:
					case 6:
					case 7:
					case 8:
					case 10:
					case 14:
					case 15:
					case 16:
						num14 = 6;
						break;
					case 20:
						num14 = 59;
						break;
					default:
						num14 = -1;
						break;
					}
					if (num14 != -1)
					{
						int num15 = Dust.NewDust(new Vector2(i * 16 + 4, j * 16 + 2), 4, 4, num14, 0f, 0f, 100);
						if (_rand.Next(3) != 0)
						{
							_dust[num15].noGravity = true;
						}
						_dust[num15].velocity *= 0.3f;
						_dust[num15].velocity.Y -= 1.5f;
					}
				}
			}
			if (typeCache == 100 && _rand.Next(40) == 0 && tileFrameX < 36)
			{
				int num16 = tileFrameY / 36;
				if (tileFrameY / 18 % 2 == 0)
				{
					int num17;
					switch (num16)
					{
					case 0:
					case 5:
					case 7:
					case 8:
					case 10:
					case 12:
					case 14:
					case 15:
					case 16:
						num17 = 6;
						break;
					case 20:
						num17 = 59;
						break;
					default:
						num17 = -1;
						break;
					}
					if (num17 != -1)
					{
						int num18 = 0;
						Vector2 position3 = ((tileFrameX == 0) ? ((_rand.Next(3) == 0) ? new Vector2(i * 16 + 4, j * 16 + 2) : new Vector2(i * 16 + 14, j * 16 + 2)) : ((_rand.Next(3) == 0) ? new Vector2(i * 16 + 6, j * 16 + 2) : new Vector2(i * 16, j * 16 + 2)));
						num18 = Dust.NewDust(position3, 4, 4, num17, 0f, 0f, 100);
						if (_rand.Next(3) != 0)
						{
							_dust[num18].noGravity = true;
						}
						_dust[num18].velocity *= 0.3f;
						_dust[num18].velocity.Y -= 1.5f;
					}
				}
			}
			if (typeCache == 98 && _rand.Next(40) == 0 && tileFrameY == 0 && tileFrameX == 0)
			{
				int num19 = Dust.NewDust(new Vector2(i * 16 + 12, j * 16 + 2), 4, 4, 6, 0f, 0f, 100);
				if (_rand.Next(3) != 0)
				{
					_dust[num19].noGravity = true;
				}
				_dust[num19].velocity *= 0.3f;
				_dust[num19].velocity.Y -= 1.5f;
			}
			if (typeCache == 49 && tileFrameX == 0 && _rand.Next(2) == 0)
			{
				int num20 = Dust.NewDust(new Vector2(i * 16 + 4, j * 16 - 4), 4, 4, 172, 0f, 0f, 100);
				if (_rand.Next(3) == 0)
				{
					_dust[num20].scale = 0.5f;
				}
				else
				{
					_dust[num20].scale = 0.9f;
					_dust[num20].noGravity = true;
				}
				_dust[num20].velocity *= 0.3f;
				_dust[num20].velocity.Y -= 1.5f;
			}
			if (typeCache == 372 && tileFrameX == 0 && _rand.Next(2) == 0)
			{
				int num21 = Dust.NewDust(new Vector2(i * 16 + 4, j * 16 - 4), 4, 4, 242, 0f, 0f, 100);
				if (_rand.Next(3) == 0)
				{
					_dust[num21].scale = 0.5f;
				}
				else
				{
					_dust[num21].scale = 0.9f;
					_dust[num21].noGravity = true;
				}
				_dust[num21].velocity *= 0.3f;
				_dust[num21].velocity.Y -= 1.5f;
			}
			if (typeCache == 34 && _rand.Next(40) == 0 && tileFrameX < 54)
			{
				int num22 = tileFrameY / 54;
				int num23 = tileFrameX / 18 % 3;
				if (tileFrameY / 18 % 3 == 1 && num23 != 1)
				{
					int num24;
					switch (num22)
					{
					case 0:
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
					case 12:
					case 13:
					case 16:
					case 19:
					case 21:
						num24 = 6;
						break;
					case 25:
						num24 = 59;
						break;
					default:
						num24 = -1;
						break;
					}
					if (num24 != -1)
					{
						int num25 = Dust.NewDust(new Vector2(i * 16, j * 16 + 2), 14, 6, num24, 0f, 0f, 100);
						if (_rand.Next(3) != 0)
						{
							_dust[num25].noGravity = true;
						}
						_dust[num25].velocity *= 0.3f;
						_dust[num25].velocity.Y -= 1.5f;
					}
				}
			}
			int leafFrequency = _leafFrequency;
			leafFrequency /= 4;
			if (typeCache == 192 && _rand.Next(leafFrequency) == 0)
			{
				EmitLivingTreeLeaf(i, j, 910);
			}
			if (typeCache == 384 && _rand.Next(leafFrequency) == 0)
			{
				EmitLivingTreeLeaf(i, j, 914);
			}
			if (typeCache == 83)
			{
				int style = tileFrameX / 18;
				if (IsAlchemyPlantHarvestable(style))
				{
					EmitAlchemyHerbParticles(j, i, style);
				}
			}
			if (typeCache == 22 && _rand.Next(400) == 0)
			{
				Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 14);
			}
			else if ((typeCache == 23 || typeCache == 24 || typeCache == 32) && _rand.Next(500) == 0)
			{
				Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 14);
			}
			else if (typeCache == 25 && _rand.Next(700) == 0)
			{
				Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 14);
			}
			else if (typeCache == 112 && _rand.Next(700) == 0)
			{
				Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 14);
			}
			else if (typeCache == 31 && _rand.Next(20) == 0)
			{
				if (tileFrameX >= 36)
				{
					int num26 = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 5, 0f, 0f, 100);
					_dust[num26].velocity.Y = 0f;
					_dust[num26].velocity.X *= 0.3f;
				}
				else
				{
					Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 14, 0f, 0f, 100);
				}
			}
			else if (typeCache == 26 && _rand.Next(20) == 0)
			{
				if (tileFrameX >= 54)
				{
					int num27 = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 5, 0f, 0f, 100);
					_dust[num27].scale = 1.5f;
					_dust[num27].noGravity = true;
					_dust[num27].velocity *= 0.75f;
				}
				else
				{
					Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 14, 0f, 0f, 100);
				}
			}
			else if ((typeCache == 71 || typeCache == 72) && _rand.Next(500) == 0)
			{
				Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 41, 0f, 0f, 250, default(Color), 0.8f);
			}
			else if ((typeCache == 17 || typeCache == 77 || typeCache == 133) && _rand.Next(40) == 0)
			{
				if (tileFrameX == 18 && tileFrameY == 18)
				{
					int num28 = Dust.NewDust(new Vector2(i * 16 - 4, j * 16 - 6), 8, 6, 6, 0f, 0f, 100);
					if (_rand.Next(3) != 0)
					{
						_dust[num28].noGravity = true;
					}
				}
			}
			else if (typeCache == 405 && _rand.Next(20) == 0)
			{
				if (tileFrameX == 18 && tileFrameY == 18)
				{
					int num29 = Dust.NewDust(new Vector2(i * 16 - 4, j * 16 - 6), 24, 10, 6, 0f, 0f, 100);
					if (_rand.Next(5) != 0)
					{
						_dust[num29].noGravity = true;
					}
				}
			}
			else if (typeCache == 452 && tileFrameY == 0 && tileFrameX == 0 && _rand.Next(3) == 0)
			{
				Vector2 position4 = new Vector2(i * 16 + 16, j * 16 + 8);
				Vector2 velocity3 = new Vector2(0f, 0f);
				if (Main.WindForVisuals < 0f)
				{
					velocity3.X = 0f - Main.WindForVisuals;
				}
				int num30 = Main.tileFrame[typeCache];
				int type2 = 907 + num30 / 5;
				if (_rand.Next(2) == 0)
				{
					Gore.NewGore(position4, velocity3, type2, _rand.NextFloat() * 0.4f + 0.4f);
				}
			}
			else if (typeCache == 406 && tileFrameY == 54 && tileFrameX == 0 && _rand.Next(3) == 0)
			{
				Vector2 position5 = new Vector2(i * 16 + 16, j * 16 + 8);
				Vector2 velocity4 = new Vector2(0f, 0f);
				if (Main.WindForVisuals < 0f)
				{
					velocity4.X = 0f - Main.WindForVisuals;
				}
				int type3 = _rand.Next(825, 828);
				if (_rand.Next(4) == 0)
				{
					Gore.NewGore(position5, velocity4, type3, _rand.NextFloat() * 0.2f + 0.2f);
				}
				else if (_rand.Next(2) == 0)
				{
					Gore.NewGore(position5, velocity4, type3, _rand.NextFloat() * 0.3f + 0.3f);
				}
				else
				{
					Gore.NewGore(position5, velocity4, type3, _rand.NextFloat() * 0.4f + 0.4f);
				}
			}
			else if (typeCache == 37 && _rand.Next(250) == 0)
			{
				int num31 = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 6, 0f, 0f, 0, default(Color), _rand.Next(3));
				if (_dust[num31].scale > 1f)
				{
					_dust[num31].noGravity = true;
				}
			}
			else if ((typeCache == 58 || typeCache == 76) && _rand.Next(250) == 0)
			{
				int num32 = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 6, 0f, 0f, 0, default(Color), _rand.Next(3));
				if (_dust[num32].scale > 1f)
				{
					_dust[num32].noGravity = true;
				}
				_dust[num32].noLight = true;
			}
			else if (typeCache == 61)
			{
				if (tileFrameX == 144 && _rand.Next(60) == 0)
				{
					int num33 = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 44, 0f, 0f, 250, default(Color), 0.4f);
					_dust[num33].fadeIn = 0.7f;
				}
			}
			else
			{
				if (Main.tileShine[typeCache] <= 0 || (tileLight.R <= 20 && tileLight.B <= 20 && tileLight.G <= 20))
				{
					return;
				}
				int num34 = tileLight.R;
				if (tileLight.G > num34)
				{
					num34 = tileLight.G;
				}
				if (tileLight.B > num34)
				{
					num34 = tileLight.B;
				}
				num34 /= 30;
				if (_rand.Next(Main.tileShine[typeCache]) >= num34 || ((typeCache == 21 || typeCache == 441) && (tileFrameX < 36 || tileFrameX >= 180) && (tileFrameX < 396 || tileFrameX > 409)) || ((typeCache == 467 || typeCache == 468) && (tileFrameX < 144 || tileFrameX >= 180)))
				{
					return;
				}
				Color newColor = Color.White;
				switch (typeCache)
				{
				case 178:
				{
					switch (tileFrameX / 18)
					{
					case 0:
						newColor = new Color(255, 0, 255, 255);
						break;
					case 1:
						newColor = new Color(255, 255, 0, 255);
						break;
					case 2:
						newColor = new Color(0, 0, 255, 255);
						break;
					case 3:
						newColor = new Color(0, 255, 0, 255);
						break;
					case 4:
						newColor = new Color(255, 0, 0, 255);
						break;
					case 5:
						newColor = new Color(255, 255, 255, 255);
						break;
					case 6:
						newColor = new Color(255, 255, 0, 255);
						break;
					}
					int num35 = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 43, 0f, 0f, 254, newColor, 0.5f);
					_dust[num35].velocity *= 0f;
					return;
				}
				case 63:
					newColor = new Color(0, 0, 255, 255);
					break;
				}
				if (typeCache == 64)
				{
					newColor = new Color(255, 0, 0, 255);
				}
				if (typeCache == 65)
				{
					newColor = new Color(0, 255, 0, 255);
				}
				if (typeCache == 66)
				{
					newColor = new Color(255, 255, 0, 255);
				}
				if (typeCache == 67)
				{
					newColor = new Color(255, 0, 255, 255);
				}
				if (typeCache == 68)
				{
					newColor = new Color(255, 255, 255, 255);
				}
				if (typeCache == 12)
				{
					newColor = new Color(255, 0, 0, 255);
				}
				if (typeCache == 204)
				{
					newColor = new Color(255, 0, 0, 255);
				}
				if (typeCache == 211)
				{
					newColor = new Color(50, 255, 100, 255);
				}
				int num36 = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 43, 0f, 0f, 254, newColor, 0.5f);
				_dust[num36].velocity *= 0f;
			}
		}

		private void EmitLivingTreeLeaf(int i, int j, int leafGoreType)
		{
			EmitLivingTreeLeaf_Below(i, j, leafGoreType);
			if (_rand.Next(2) == 0)
			{
				EmitLivingTreeLeaf_Sideways(i, j, leafGoreType);
			}
		}

		private void EmitLivingTreeLeaf_Below(int x, int y, int leafGoreType)
		{
			Tile tile = Main.tile[x, y + 1];
			if (!WorldGen.SolidTile(tile) && tile.liquid <= 0)
			{
				float windForVisuals = Main.WindForVisuals;
				if ((!(windForVisuals < -0.2f) || (!WorldGen.SolidTile(Main.tile[x - 1, y + 1]) && !WorldGen.SolidTile(Main.tile[x - 2, y + 1]))) && (!(windForVisuals > 0.2f) || (!WorldGen.SolidTile(Main.tile[x + 1, y + 1]) && !WorldGen.SolidTile(Main.tile[x + 2, y + 1]))))
				{
					Gore.NewGorePerfect(new Vector2(x * 16, y * 16 + 16), Vector2.Zero, leafGoreType).Frame.CurrentColumn = Main.tile[x, y].color();
				}
			}
		}

		private void EmitLivingTreeLeaf_Sideways(int x, int y, int leafGoreType)
		{
			int num = 0;
			if (Main.WindForVisuals > 0.2f)
			{
				num = 1;
			}
			else if (Main.WindForVisuals < -0.2f)
			{
				num = -1;
			}
			Tile tile = Main.tile[x + num, y];
			if (!WorldGen.SolidTile(tile) && tile.liquid <= 0)
			{
				int num2 = 0;
				if (num == -1)
				{
					num2 = -10;
				}
				Gore.NewGorePerfect(new Vector2(x * 16 + 8 + 4 * num + num2, y * 16 + 8), Vector2.Zero, leafGoreType).Frame.CurrentColumn = Main.tile[x, y].color();
			}
		}

		private void EmitLiquidDrops(int j, int i, Tile tileCache, ushort typeCache)
		{
			int num = 60;
			switch (typeCache)
			{
			case 374:
				num = 120;
				break;
			case 375:
				num = 180;
				break;
			case 461:
				num = 180;
				break;
			}
			if (_rand.Next(num * 2) != 0 || tileCache.liquid != 0)
			{
				return;
			}
			Rectangle rectangle = new Rectangle(i * 16, j * 16, 16, 16);
			rectangle.X -= 34;
			rectangle.Width += 68;
			rectangle.Y -= 100;
			rectangle.Height = 400;
			bool flag = true;
			for (int k = 0; k < 600; k++)
			{
				if (_gore[k].active && ((_gore[k].type >= 706 && _gore[k].type <= 717) || _gore[k].type == 943 || _gore[k].type == 1147 || (_gore[k].type >= 1160 && _gore[k].type <= 1162)))
				{
					Rectangle value = new Rectangle((int)_gore[k].position.X, (int)_gore[k].position.Y, 16, 16);
					if (rectangle.Intersects(value))
					{
						flag = false;
					}
				}
			}
			if (!flag)
			{
				return;
			}
			Vector2 position = new Vector2(i * 16, j * 16);
			int type = 706;
			if (Main.waterStyle == 12)
			{
				type = 1147;
			}
			else if (Main.waterStyle > 1)
			{
				type = 706 + Main.waterStyle - 1;
			}
			if (typeCache == 374)
			{
				type = 716;
			}
			if (typeCache == 375)
			{
				type = 717;
			}
			if (typeCache == 461)
			{
				type = 943;
				if (Main.player[Main.myPlayer].ZoneCorrupt)
				{
					type = 1160;
				}
				if (Main.player[Main.myPlayer].ZoneCrimson)
				{
					type = 1161;
				}
				if (Main.player[Main.myPlayer].ZoneHallow)
				{
					type = 1162;
				}
			}
			int num2 = Gore.NewGore(position, default(Vector2), type);
			_gore[num2].velocity *= 0f;
		}

		private float GetWindCycle(int x, int y, double windCounter)
		{
			if (!Main.SettingsEnabled_TilesSwayInWind)
			{
				return 0f;
			}
			float num = (float)x * 0.5f + (float)(y / 100) * 0.5f;
			float num2 = (float)Math.Cos(windCounter * 6.2831854820251465 + (double)num) * 0.5f;
			if ((double)y < Main.worldSurface)
			{
				num2 += Main.WindForVisuals;
				float lerpValue = Utils.GetLerpValue(0.08f, 0.18f, Math.Abs(Main.WindForVisuals), clamped: true);
				return num2 * lerpValue;
			}
			return 0f;
		}

		private bool ShouldSwayInWind(int x, int y, Tile tileCache)
		{
			if (!Main.SettingsEnabled_TilesSwayInWind)
			{
				return false;
			}
			if (!TileID.Sets.SwaysInWindBasic[tileCache.type])
			{
				return false;
			}
			if (tileCache.type == 227 && (tileCache.frameX == 204 || tileCache.frameX == 238 || tileCache.frameX == 408 || tileCache.frameX == 442 || tileCache.frameX == 476))
			{
				return false;
			}
			return true;
		}

		private void UpdateLeafFrequency()
		{
			float num = Math.Abs(Main.WindForVisuals);
			if (num <= 0.1f)
			{
				_leafFrequency = 2000;
			}
			else if (num <= 0.2f)
			{
				_leafFrequency = 1000;
			}
			else if (num <= 0.3f)
			{
				_leafFrequency = 450;
			}
			else if (num <= 0.4f)
			{
				_leafFrequency = 300;
			}
			else if (num <= 0.5f)
			{
				_leafFrequency = 200;
			}
			else if (num <= 0.6f)
			{
				_leafFrequency = 130;
			}
			else if (num <= 0.7f)
			{
				_leafFrequency = 75;
			}
			else if (num <= 0.8f)
			{
				_leafFrequency = 50;
			}
			else if (num <= 0.9f)
			{
				_leafFrequency = 40;
			}
			else if (num <= 1f)
			{
				_leafFrequency = 30;
			}
			else if (num <= 1.1f)
			{
				_leafFrequency = 20;
			}
			else
			{
				_leafFrequency = 10;
			}
			_leafFrequency *= 7;
		}

		private void EnsureWindGridSize()
		{
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 offSet = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				offSet = Vector2.Zero;
			}
			GetScreenDrawArea(unscaledPosition, offSet, out var firstTileX, out var lastTileX, out var firstTileY, out var lastTileY);
			_windGrid.SetSize(lastTileX - firstTileX, lastTileY - firstTileY);
		}

		private void EmitTreeLeaves(int tilePosX, int tilePosY, int grassPosX, int grassPosY)
		{
			if (!_isActiveAndNotPaused)
			{
				return;
			}
			int treeHeight = grassPosY - tilePosY;
			Tile tile = Main.tile[tilePosX, tilePosY];
			if (tile.liquid > 0)
			{
				return;
			}
			WorldGen.GetTreeLeaf(tilePosX, tile, Main.tile[grassPosX, grassPosY], treeHeight, out var _, out var passStyle);
			int num;
			switch (passStyle)
			{
			case -1:
			case 912:
			case 913:
				return;
			default:
				num = ((passStyle >= 1113 && passStyle <= 1121) ? 1 : 0);
				break;
			case 917:
			case 918:
			case 919:
			case 920:
			case 921:
			case 922:
			case 923:
			case 924:
			case 925:
				num = 1;
				break;
			}
			bool flag = (byte)num != 0;
			int num2 = _leafFrequency;
			bool flag2 = tilePosX - grassPosX != 0;
			if (flag)
			{
				num2 /= 2;
			}
			if ((double)tilePosY > Main.worldSurface)
			{
				num2 = 10000;
			}
			if (flag2)
			{
				num2 *= 3;
			}
			if (_rand.Next(num2) != 0)
			{
				return;
			}
			int num3 = 2;
			Vector2 vector = new Vector2(tilePosX * 16 + 8, tilePosY * 16 + 8);
			if (flag2)
			{
				int num4 = tilePosX - grassPosX;
				vector.X += num4 * 12;
				int num5 = 0;
				if (tile.frameY == 220)
				{
					num5 = 1;
				}
				else if (tile.frameY == 242)
				{
					num5 = 2;
				}
				if (tile.frameX == 66)
				{
					switch (num5)
					{
					case 0:
						vector += new Vector2(0f, -6f);
						break;
					case 1:
						vector += new Vector2(0f, -6f);
						break;
					case 2:
						vector += new Vector2(0f, 8f);
						break;
					}
				}
				else
				{
					switch (num5)
					{
					case 0:
						vector += new Vector2(0f, 4f);
						break;
					case 1:
						vector += new Vector2(2f, -6f);
						break;
					case 2:
						vector += new Vector2(6f, -6f);
						break;
					}
				}
			}
			else
			{
				vector += new Vector2(-16f, -16f);
				if (flag)
				{
					vector.Y -= Main.rand.Next(0, 28) * 4;
				}
			}
			if (!WorldGen.SolidTile(vector.ToTileCoordinates()))
			{
				Gore.NewGoreDirect(vector, Utils.RandomVector2(Main.rand, -num3, num3), passStyle, 0.7f + Main.rand.NextFloat() * 0.6f).Frame.CurrentColumn = Main.tile[tilePosX, tilePosY].color();
			}
		}

		private void DrawSpecialTilesLegacy(Vector2 screenPosition, Vector2 offSet)
		{
			for (int i = 0; i < _specialTilesCount; i++)
			{
				int num = _specialTileX[i];
				int num2 = _specialTileY[i];
				Tile tile = Main.tile[num, num2];
				ushort type = tile.type;
				short frameX = tile.frameX;
				short frameY = tile.frameY;
				if (type == 237)
				{
					Main.spriteBatch.Draw(TextureAssets.SunOrb.Value, new Vector2((float)(num * 16 - (int)screenPosition.X) + 8f, num2 * 16 - (int)screenPosition.Y - 36) + offSet, new Rectangle(0, 0, TextureAssets.SunOrb.Width(), TextureAssets.SunOrb.Height()), new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, 0), Main.sunCircle, new Vector2(TextureAssets.SunOrb.Width() / 2, TextureAssets.SunOrb.Height() / 2), 1f, SpriteEffects.None, 0f);
				}
				if (type == 334 && frameX >= 5000)
				{
					_ = frameY / 18;
					int num3 = frameX;
					int num4 = 0;
					int num5 = num3 % 5000;
					num5 -= 100;
					while (num3 >= 5000)
					{
						num4++;
						num3 -= 5000;
					}
					int frameX2 = Main.tile[num + 1, num2].frameX;
					frameX2 = ((frameX2 < 25000) ? (frameX2 - 10000) : (frameX2 - 25000));
					Item item = new Item();
					item.netDefaults(num5);
					item.Prefix(frameX2);
					Main.instance.LoadItem(item.type);
					Texture2D value = TextureAssets.Item[item.type].Value;
					Rectangle value2 = ((Main.itemAnimations[item.type] == null) ? value.Frame() : Main.itemAnimations[item.type].GetFrame(value));
					int width = value2.Width;
					int height = value2.Height;
					float num6 = 1f;
					if (width > 40 || height > 40)
					{
						num6 = ((width <= height) ? (40f / (float)height) : (40f / (float)width));
					}
					num6 *= item.scale;
					SpriteEffects effects = SpriteEffects.None;
					if (num4 >= 3)
					{
						effects = SpriteEffects.FlipHorizontally;
					}
					Color color = Lighting.GetColor(num, num2);
					Main.spriteBatch.Draw(value, new Vector2(num * 16 - (int)screenPosition.X + 24, num2 * 16 - (int)screenPosition.Y + 8) + offSet, value2, Lighting.GetColor(num, num2), 0f, new Vector2(width / 2, height / 2), num6, effects, 0f);
					if (item.color != default(Color))
					{
						Main.spriteBatch.Draw(value, new Vector2(num * 16 - (int)screenPosition.X + 24, num2 * 16 - (int)screenPosition.Y + 8) + offSet, value2, item.GetColor(color), 0f, new Vector2(width / 2, height / 2), num6, effects, 0f);
					}
				}
				if (type == 395)
				{
					Item item2 = ((TEItemFrame)TileEntity.ByPosition[new Point16(num, num2)]).item;
					Vector2 screenPositionForItemCenter = new Vector2(num * 16 - (int)screenPosition.X + 16, num2 * 16 - (int)screenPosition.Y + 16) + offSet;
					Color color2 = Lighting.GetColor(num, num2);
					Main.DrawItemIcon(Main.spriteBatch, item2, screenPositionForItemCenter, color2, 20f);
				}
				if (type == 520)
				{
					Item item3 = ((TEFoodPlatter)TileEntity.ByPosition[new Point16(num, num2)]).item;
					if (!item3.IsAir)
					{
						Main.instance.LoadItem(item3.type);
						Texture2D value3 = TextureAssets.Item[item3.type].Value;
						Rectangle value4 = ((!ItemID.Sets.IsFood[item3.type]) ? value3.Frame() : value3.Frame(1, 3, 0, 2));
						int width2 = value4.Width;
						int height2 = value4.Height;
						float num7 = 1f;
						SpriteEffects effects2 = ((tile.frameX == 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
						Color color3 = Lighting.GetColor(num, num2);
						Color currentColor = color3;
						float scale = 1f;
						ItemSlot.GetItemLight(currentColor, scale, item3);
						num7 *= scale;
						Vector2 position = new Vector2(num * 16 - (int)screenPosition.X + 8, num2 * 16 - (int)screenPosition.Y + 16) + offSet;
						position.Y += 2f;
						Vector2 origin = new Vector2(width2 / 2, height2);
						Main.spriteBatch.Draw(value3, position, value4, currentColor, 0f, origin, num7, effects2, 0f);
						if (item3.color != default(Color))
						{
							Main.spriteBatch.Draw(value3, position, value4, item3.GetColor(color3), 0f, origin, num7, effects2, 0f);
						}
					}
				}
				if (type == 471)
				{
					Item item4 = (TileEntity.ByPosition[new Point16(num, num2)] as TEWeaponsRack).item;
					Main.instance.LoadItem(item4.type);
					Texture2D value5 = TextureAssets.Item[item4.type].Value;
					Rectangle value6 = ((Main.itemAnimations[item4.type] == null) ? value5.Frame() : Main.itemAnimations[item4.type].GetFrame(value5));
					int width3 = value6.Width;
					int height3 = value6.Height;
					float num8 = 1f;
					float num9 = 40f;
					if ((float)width3 > num9 || (float)height3 > num9)
					{
						num8 = ((width3 <= height3) ? (num9 / (float)height3) : (num9 / (float)width3));
					}
					num8 *= item4.scale;
					SpriteEffects effects3 = SpriteEffects.FlipHorizontally;
					if (tile.frameX < 54)
					{
						effects3 = SpriteEffects.None;
					}
					Color color4 = Lighting.GetColor(num, num2);
					Color currentColor2 = color4;
					float scale2 = 1f;
					ItemSlot.GetItemLight(currentColor2, scale2, item4);
					num8 *= scale2;
					Main.spriteBatch.Draw(value5, new Vector2(num * 16 - (int)screenPosition.X + 24, num2 * 16 - (int)screenPosition.Y + 24) + offSet, value6, currentColor2, 0f, new Vector2(width3 / 2, height3 / 2), num8, effects3, 0f);
					if (item4.color != default(Color))
					{
						Main.spriteBatch.Draw(value5, new Vector2(num * 16 - (int)screenPosition.X + 24, num2 * 16 - (int)screenPosition.Y + 24) + offSet, value6, item4.GetColor(color4), 0f, new Vector2(width3 / 2, height3 / 2), num8, effects3, 0f);
					}
				}
				if (type == 412)
				{
					Texture2D value7 = TextureAssets.GlowMask[202].Value;
					int num10 = Main.tileFrame[type] / 60;
					int frameY2 = (num10 + 1) % 4;
					float num11 = (float)(Main.tileFrame[type] % 60) / 60f;
					Color value8 = new Color(255, 255, 255, 255);
					Main.spriteBatch.Draw(value7, new Vector2(num * 16 - (int)screenPosition.X, num2 * 16 - (int)screenPosition.Y + 10) + offSet, value7.Frame(1, 4, 0, num10), value8 * (1f - num11), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					Main.spriteBatch.Draw(value7, new Vector2(num * 16 - (int)screenPosition.X, num2 * 16 - (int)screenPosition.Y + 10) + offSet, value7.Frame(1, 4, 0, frameY2), value8 * num11, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				}
				if (type == 620)
				{
					Texture2D value9 = TextureAssets.Extra[202].Value;
					_ = (float)(Main.tileFrame[type] % 60) / 60f;
					int num12 = 2;
					Main.critterCage = true;
					int waterAnimalCageFrame = GetWaterAnimalCageFrame(num, num2, frameX, frameY);
					int num13 = 8;
					int num14 = Main.butterflyCageFrame[num13, waterAnimalCageFrame];
					int num15 = 6;
					float num16 = 1f;
					Rectangle value10 = new Rectangle(0, 34 * num14, 32, 32);
					Vector2 vector = new Vector2(num * 16 - (int)screenPosition.X, num2 * 16 - (int)screenPosition.Y + num12) + offSet;
					Main.spriteBatch.Draw(value9, vector, value10, new Color(255, 255, 255, 255), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					for (int j = 0; j < num15; j++)
					{
						Color color5 = new Color(127, 127, 127, 0).MultiplyRGBA(Main.hslToRgb((Main.GlobalTimeWrappedHourly + (float)j / (float)num15) % 1f, 1f, 0.5f));
						color5 *= 1f - num16 * 0.5f;
						color5.A = 0;
						int num17 = 2;
						Vector2 position2 = vector + ((float)j / (float)num15 * ((float)Math.PI * 2f)).ToRotationVector2() * ((float)num17 * num16 + 2f);
						Main.spriteBatch.Draw(value9, position2, value10, color5, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					Main.spriteBatch.Draw(value9, vector, value10, new Color(255, 255, 255, 0) * 0.1f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				}
			}
		}

		private void DrawEntities_DisplayDolls()
		{
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
			foreach (KeyValuePair<Point, int> displayDollTileEntityPosition in _displayDollTileEntityPositions)
			{
				if (displayDollTileEntityPosition.Value != -1 && TileEntity.ByPosition.TryGetValue(new Point16(displayDollTileEntityPosition.Key.X, displayDollTileEntityPosition.Key.Y), out var value))
				{
					(value as TEDisplayDoll).Draw(displayDollTileEntityPosition.Key.X, displayDollTileEntityPosition.Key.Y);
				}
			}
			Main.spriteBatch.End();
		}

		private void DrawEntities_HatRacks()
		{
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
			foreach (KeyValuePair<Point, int> hatRackTileEntityPosition in _hatRackTileEntityPositions)
			{
				if (hatRackTileEntityPosition.Value != -1 && TileEntity.ByPosition.TryGetValue(new Point16(hatRackTileEntityPosition.Key.X, hatRackTileEntityPosition.Key.Y), out var value))
				{
					(value as TEHatRack).Draw(hatRackTileEntityPosition.Key.X, hatRackTileEntityPosition.Key.Y);
				}
			}
			Main.spriteBatch.End();
		}

		private void DrawTrees()
		{
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 zero = Vector2.Zero;
			int num = 0;
			int num2 = _specialsCount[num];
			float num3 = 0.08f;
			float num4 = 0.06f;
			for (int i = 0; i < num2; i++)
			{
				Point point = _specialPositions[num][i];
				int x = point.X;
				int y = point.Y;
				Tile tile = Main.tile[x, y];
				if (tile == null || !tile.active())
				{
					continue;
				}
				ushort type = tile.type;
				short frameX = tile.frameX;
				short frameY = tile.frameY;
				bool flag = tile.wall > 0;
				WorldGen.GetTreeFoliageDataMethod getTreeFoliageDataMethod = null;
				try
				{
					bool flag2 = false;
					switch (type)
					{
					case 5:
						flag2 = true;
						getTreeFoliageDataMethod = WorldGen.GetCommonTreeFoliageData;
						break;
					case 583:
					case 584:
					case 585:
					case 586:
					case 587:
					case 588:
					case 589:
						flag2 = true;
						getTreeFoliageDataMethod = WorldGen.GetGemTreeFoliageData;
						break;
					case 596:
					case 616:
						flag2 = true;
						getTreeFoliageDataMethod = WorldGen.GetVanityTreeFoliageData;
						break;
					}
					if (flag2 && frameY >= 198 && frameX >= 22)
					{
						int treeFrame = WorldGen.GetTreeFrame(tile);
						switch (frameX)
						{
						case 22:
						{
							int treeStyle2 = 0;
							int topTextureFrameWidth2 = 80;
							int topTextureFrameHeight2 = 80;
							int num9 = 0;
							int grassPosX = x + num9;
							int floorY2 = y;
							if (!getTreeFoliageDataMethod(x, y, num9, treeFrame, treeStyle2, out floorY2, out topTextureFrameWidth2, out topTextureFrameHeight2))
							{
								continue;
							}
							EmitTreeLeaves(x, y, grassPosX, floorY2);
							if (treeStyle2 == 14)
							{
								float num10 = (float)_rand.Next(28, 42) * 0.005f;
								num10 += (float)(270 - Main.mouseTextColor) / 1000f;
								Lighting.AddLight(x, y, 0.1f, 0.2f + num10 / 2f, 0.7f + num10);
							}
							byte tileColor2 = tile.color();
							Texture2D treeTopTexture = GetTreeTopTexture(treeStyle2, 0, tileColor2);
							Vector2 position2 = (position2 = new Vector2(x * 16 - (int)unscaledPosition.X + 8, y * 16 - (int)unscaledPosition.Y + 16) + zero);
							float num11 = 0f;
							if (!flag)
							{
								num11 = GetWindCycle(x, y, _treeWindCounter);
							}
							position2.X += num11 * 2f;
							position2.Y += Math.Abs(num11) * 2f;
							Color color2 = Lighting.GetColor(x, y);
							if (tile.color() == 31)
							{
								color2 = Color.White;
							}
							Main.spriteBatch.Draw(treeTopTexture, position2, new Rectangle(treeFrame * (topTextureFrameWidth2 + 2), 0, topTextureFrameWidth2, topTextureFrameHeight2), color2, num11 * num3, new Vector2(topTextureFrameWidth2 / 2, topTextureFrameHeight2), 1f, SpriteEffects.None, 0f);
							break;
						}
						case 44:
						{
							int treeStyle3 = 0;
							int num12 = x;
							int floorY3 = y;
							int num13 = 1;
							if (!getTreeFoliageDataMethod(x, y, num13, treeFrame, treeStyle3, out floorY3, out var _, out var _))
							{
								continue;
							}
							EmitTreeLeaves(x, y, num12 + num13, floorY3);
							if (treeStyle3 == 14)
							{
								float num14 = (float)_rand.Next(28, 42) * 0.005f;
								num14 += (float)(270 - Main.mouseTextColor) / 1000f;
								Lighting.AddLight(x, y, 0.1f, 0.2f + num14 / 2f, 0.7f + num14);
							}
							byte tileColor3 = tile.color();
							Texture2D treeBranchTexture2 = GetTreeBranchTexture(treeStyle3, 0, tileColor3);
							Vector2 position3 = new Vector2(x * 16, y * 16) - unscaledPosition + zero + new Vector2(16f, 12f);
							float num15 = 0f;
							if (!flag)
							{
								num15 = GetWindCycle(x, y, _treeWindCounter);
							}
							if (num15 > 0f)
							{
								position3.X += num15;
							}
							position3.X += Math.Abs(num15) * 2f;
							Color color3 = Lighting.GetColor(x, y);
							if (tile.color() == 31)
							{
								color3 = Color.White;
							}
							Main.spriteBatch.Draw(treeBranchTexture2, position3, new Rectangle(0, treeFrame * 42, 40, 40), color3, num15 * num4, new Vector2(40f, 24f), 1f, SpriteEffects.None, 0f);
							break;
						}
						case 66:
						{
							int treeStyle = 0;
							int num5 = x;
							int floorY = y;
							int num6 = -1;
							if (!getTreeFoliageDataMethod(x, y, num6, treeFrame, treeStyle, out floorY, out var _, out var _))
							{
								continue;
							}
							EmitTreeLeaves(x, y, num5 + num6, floorY);
							if (treeStyle == 14)
							{
								float num7 = (float)_rand.Next(28, 42) * 0.005f;
								num7 += (float)(270 - Main.mouseTextColor) / 1000f;
								Lighting.AddLight(x, y, 0.1f, 0.2f + num7 / 2f, 0.7f + num7);
							}
							byte tileColor = tile.color();
							Texture2D treeBranchTexture = GetTreeBranchTexture(treeStyle, 0, tileColor);
							Vector2 position = new Vector2(x * 16, y * 16) - unscaledPosition + zero + new Vector2(0f, 18f);
							float num8 = 0f;
							if (!flag)
							{
								num8 = GetWindCycle(x, y, _treeWindCounter);
							}
							if (num8 < 0f)
							{
								position.X += num8;
							}
							position.X -= Math.Abs(num8) * 2f;
							Color color = Lighting.GetColor(x, y);
							if (tile.color() == 31)
							{
								color = Color.White;
							}
							Main.spriteBatch.Draw(treeBranchTexture, position, new Rectangle(42, treeFrame * 42, 40, 40), color, num8 * num4, new Vector2(0f, 30f), 1f, SpriteEffects.None, 0f);
							break;
						}
						}
					}
					if (type == 323 && frameX >= 88 && frameX <= 132)
					{
						int num16 = 0;
						switch (frameX)
						{
						case 110:
							num16 = 1;
							break;
						case 132:
							num16 = 2;
							break;
						}
						int treeTextureIndex = 15;
						int num17 = 80;
						int num18 = 80;
						int num19 = 32;
						int num20 = 0;
						int palmTreeBiome = GetPalmTreeBiome(x, y);
						int y2 = palmTreeBiome * 82;
						if (palmTreeBiome >= 4 && palmTreeBiome <= 7)
						{
							treeTextureIndex = 21;
							num17 = 114;
							num18 = 98;
							y2 = (palmTreeBiome - 4) * 98;
							num19 = 48;
							num20 = 2;
						}
						int frameY2 = Main.tile[x, y].frameY;
						byte tileColor4 = tile.color();
						Texture2D treeTopTexture2 = GetTreeTopTexture(treeTextureIndex, palmTreeBiome, tileColor4);
						Vector2 position4 = new Vector2(x * 16 - (int)unscaledPosition.X - num19 + frameY2 + num17 / 2, y * 16 - (int)unscaledPosition.Y + 16 + num20) + zero;
						float num21 = 0f;
						if (!flag)
						{
							num21 = GetWindCycle(x, y, _treeWindCounter);
						}
						position4.X += num21 * 2f;
						position4.Y += Math.Abs(num21) * 2f;
						Color color4 = Lighting.GetColor(x, y);
						if (tile.color() == 31)
						{
							color4 = Color.White;
						}
						Main.spriteBatch.Draw(treeTopTexture2, position4, new Rectangle(num16 * (num17 + 2), y2, num17, num18), color4, num21 * num3, new Vector2(num17 / 2, num18), 1f, SpriteEffects.None, 0f);
					}
				}
				catch
				{
				}
			}
		}

		private Texture2D GetTreeTopTexture(int treeTextureIndex, int treeTextureStyle, byte tileColor)
		{
			Texture2D texture2D = _paintSystem.TryGetTreeTopAndRequestIfNotReady(treeTextureIndex, treeTextureStyle, tileColor);
			if (texture2D == null)
			{
				texture2D = TextureAssets.TreeTop[treeTextureIndex].Value;
			}
			return texture2D;
		}

		private Texture2D GetTreeBranchTexture(int treeTextureIndex, int treeTextureStyle, byte tileColor)
		{
			Texture2D texture2D = _paintSystem.TryGetTreeBranchAndRequestIfNotReady(treeTextureIndex, treeTextureStyle, tileColor);
			if (texture2D == null)
			{
				texture2D = TextureAssets.TreeBranch[treeTextureIndex].Value;
			}
			return texture2D;
		}

		private void DrawGrass()
		{
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 zero = Vector2.Zero;
			int num = 3;
			int num2 = _specialsCount[num];
			for (int i = 0; i < num2; i++)
			{
				Point point = _specialPositions[num][i];
				int x = point.X;
				int y = point.Y;
				Tile tile = Main.tile[x, y];
				if (tile == null || !tile.active())
				{
					continue;
				}
				ushort type = tile.type;
				short tileFrameX = tile.frameX;
				short tileFrameY = tile.frameY;
				GetTileDrawData(x, y, tile, type, tileFrameX, tileFrameY, out var tileWidth, out var tileHeight, out var tileTop, out var halfBrickHeight, out var addFrX, out var addFrY, out var tileSpriteEffect, out var _, out var _, out var _);
				bool flag = _rand.Next(4) == 0;
				Color tileLight = Lighting.GetColor(x, y);
				DrawAnimatedTile_AdjustForVisionChangers(x, y, tile, type, tileFrameX, tileFrameY, tileLight, flag);
				tileLight = DrawTiles_GetLightOverride(y, x, tile, type, tileFrameX, tileFrameY, tileLight);
				if (_isActiveAndNotPaused && flag)
				{
					DrawTiles_EmitParticles(y, x, tile, type, tileFrameX, tileFrameY, tileLight);
				}
				if (type == 83 && IsAlchemyPlantHarvestable(tileFrameX / 18))
				{
					type = 84;
					Main.instance.LoadTiles(type);
				}
				if (tile.type == 227 && tileFrameX == 202)
				{
					GetCactusType(x, y, tileFrameX, tileFrameY, out var evil, out var good, out var crimson);
					if (good)
					{
						tileFrameX = (short)(tileFrameX + 170);
					}
					if (evil)
					{
						tileFrameX = (short)(tileFrameX + 204);
					}
					if (crimson)
					{
						tileFrameX = (short)(tileFrameX + 238);
					}
				}
				Vector2 position = new Vector2(x * 16 - (int)unscaledPosition.X + 8, y * 16 - (int)unscaledPosition.Y + 16) + zero;
				_ = _grassWindCounter;
				float num3 = GetWindCycle(x, y, _grassWindCounter);
				if (!WallID.Sets.AllowsWind[tile.wall])
				{
					num3 = 0f;
				}
				if (!InAPlaceWithWind(x, y, 1, 1))
				{
					num3 = 0f;
				}
				num3 += GetWindGridPush(x, y, 20, 0.35f);
				position.X += num3 * 1f;
				position.Y += Math.Abs(num3) * 1f;
				Texture2D tileDrawTexture = GetTileDrawTexture(tile, x, y);
				if (tileDrawTexture != null)
				{
					Main.spriteBatch.Draw(tileDrawTexture, position, new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), tileLight, num3 * 0.1f, new Vector2(tileWidth / 2, 16 - halfBrickHeight - tileTop), 1f, tileSpriteEffect, 0f);
				}
			}
		}

		private void DrawAnimatedTile_AdjustForVisionChangers(int i, int j, Tile tileCache, ushort typeCache, short tileFrameX, short tileFrameY, Color tileLight, bool canDoDust)
		{
			if (_localPlayer.dangerSense && IsTileDangerous(_localPlayer, tileCache, typeCache))
			{
				if (tileLight.R < byte.MaxValue)
				{
					tileLight.R = byte.MaxValue;
				}
				if (tileLight.G < 50)
				{
					tileLight.G = 50;
				}
				if (tileLight.B < 50)
				{
					tileLight.B = 50;
				}
				if (_isActiveAndNotPaused && canDoDust && _rand.Next(30) == 0)
				{
					int num = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 60, 0f, 0f, 100, default(Color), 0.3f);
					_dust[num].fadeIn = 1f;
					_dust[num].velocity *= 0.1f;
					_dust[num].noLight = true;
					_dust[num].noGravity = true;
				}
			}
			if (_localPlayer.findTreasure && Main.IsTileSpelunkable(typeCache, tileFrameX, tileFrameY))
			{
				if (tileLight.R < 200)
				{
					tileLight.R = 200;
				}
				if (tileLight.G < 170)
				{
					tileLight.G = 170;
				}
				if (_isActiveAndNotPaused && _rand.Next(60) == 0 && canDoDust)
				{
					int num2 = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 204, 0f, 0f, 150, default(Color), 0.3f);
					_dust[num2].fadeIn = 1f;
					_dust[num2].velocity *= 0.1f;
					_dust[num2].noLight = true;
				}
			}
		}

		private float GetWindGridPush(int i, int j, int pushAnimationTimeTotal, float pushForcePerFrame)
		{
			_windGrid.GetWindTime(i, j, pushAnimationTimeTotal, out var windTimeLeft, out var direction);
			if (windTimeLeft >= pushAnimationTimeTotal / 2)
			{
				return (float)(pushAnimationTimeTotal - windTimeLeft) * pushForcePerFrame * (float)direction;
			}
			return (float)windTimeLeft * pushForcePerFrame * (float)direction;
		}

		private float GetWindGridPushComplex(int i, int j, int pushAnimationTimeTotal, float totalPushForce, int loops, bool flipDirectionPerLoop)
		{
			_windGrid.GetWindTime(i, j, pushAnimationTimeTotal, out var windTimeLeft, out var direction);
			float num = (float)windTimeLeft / (float)pushAnimationTimeTotal;
			int num2 = (int)(num * (float)loops);
			float num3 = num * (float)loops % 1f;
			_ = 1f / (float)loops;
			if (flipDirectionPerLoop && num2 % 2 == 1)
			{
				direction *= -1;
			}
			if (num * (float)loops % 1f > 0.5f)
			{
				return (1f - num3) * totalPushForce * (float)direction * (float)(loops - num2);
			}
			return num3 * totalPushForce * (float)direction * (float)(loops - num2);
		}

		private void DrawMasterTrophies()
		{
			int num = 11;
			int num2 = _specialsCount[num];
			for (int i = 0; i < num2; i++)
			{
				Point p = _specialPositions[num][i];
				Tile tile = Main.tile[p.X, p.Y];
				if (tile != null && tile.active())
				{
					Texture2D value = TextureAssets.Extra[198].Value;
					int frameY = tile.frameX / 54;
					bool num3 = tile.frameY / 72 != 0;
					int horizontalFrames = 1;
					int verticalFrames = 27;
					Rectangle rectangle = value.Frame(horizontalFrames, verticalFrames, 0, frameY);
					Vector2 origin = rectangle.Size() / 2f;
					Vector2 value2 = p.ToWorldCoordinates(24f, 64f);
					float num4 = (float)Math.Sin(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f) / 5f);
					Vector2 value3 = value2 + new Vector2(0f, -40f) + new Vector2(0f, num4 * 4f);
					Color color = Lighting.GetColor(p.X, p.Y);
					SpriteEffects effects = (num3 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
					Main.spriteBatch.Draw(value, value3 - Main.screenPosition, rectangle, color, 0f, origin, 1f, effects, 0f);
					float scale = (float)Math.Sin(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f) / 2f) * 0.3f + 0.7f;
					Color value4 = color;
					value4.A = 0;
					value4 = value4 * 0.1f * scale;
					for (float num5 = 0f; num5 < 1f; num5 += 355f / (678f * (float)Math.PI))
					{
						Main.spriteBatch.Draw(value, value3 - Main.screenPosition + ((float)Math.PI * 2f * num5).ToRotationVector2() * (6f + num4 * 2f), rectangle, value4, 0f, origin, 1f, effects, 0f);
					}
				}
			}
		}

		private void DrawTeleportationPylons()
		{
			int num = 10;
			int num2 = _specialsCount[num];
			for (int i = 0; i < num2; i++)
			{
				Point p = _specialPositions[num][i];
				Tile tile = Main.tile[p.X, p.Y];
				if (tile == null || !tile.active())
				{
					continue;
				}
				Texture2D value = TextureAssets.Extra[181].Value;
				int num3 = tile.frameX / 54;
				int num4 = 3;
				int horizontalFrames = num4 + 9;
				int verticalFrames = 8;
				int frameY = (Main.tileFrameCounter[597] + p.X + p.Y) % 64 / 8;
				Rectangle rectangle = value.Frame(horizontalFrames, verticalFrames, num4 + num3, frameY);
				Rectangle value2 = value.Frame(horizontalFrames, verticalFrames, 2, frameY);
				value.Frame(horizontalFrames, verticalFrames, 0, frameY);
				Vector2 origin = rectangle.Size() / 2f;
				Vector2 value3 = p.ToWorldCoordinates(24f, 64f);
				float num5 = (float)Math.Sin(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f) / 5f);
				Vector2 vector = value3 + new Vector2(0f, -40f) + new Vector2(0f, num5 * 4f);
				bool flag = _rand.Next(4) == 0;
				if (_isActiveAndNotPaused && flag && _rand.Next(10) == 0)
				{
					Rectangle dustBox = Utils.CenteredRectangle(vector, rectangle.Size());
					TeleportPylonsSystem.SpawnInWorldDust(num3, dustBox);
				}
				Color color = Lighting.GetColor(p.X, p.Y);
				color = Color.Lerp(color, Color.White, 0.8f);
				Main.spriteBatch.Draw(value, vector - Main.screenPosition, rectangle, color * 0.7f, 0f, origin, 1f, SpriteEffects.None, 0f);
				float scale = (float)Math.Sin(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f) / 1f) * 0.2f + 0.8f;
				Color color2 = new Color(255, 255, 255, 0) * 0.1f * scale;
				for (float num6 = 0f; num6 < 1f; num6 += 355f / (678f * (float)Math.PI))
				{
					Main.spriteBatch.Draw(value, vector - Main.screenPosition + ((float)Math.PI * 2f * num6).ToRotationVector2() * (6f + num5 * 2f), rectangle, color2, 0f, origin, 1f, SpriteEffects.None, 0f);
				}
				int num7 = 0;
				if (Main.InSmartCursorHighlightArea(p.X, p.Y, out var actuallySelected))
				{
					num7 = 1;
					if (actuallySelected)
					{
						num7 = 2;
					}
				}
				if (num7 != 0)
				{
					int num8 = (color.R + color.G + color.B) / 3;
					if (num8 > 10)
					{
						Color selectionGlowColor = Colors.GetSelectionGlowColor(num7 == 2, num8);
						Main.spriteBatch.Draw(value, vector - Main.screenPosition, value2, selectionGlowColor, 0f, origin, 1f, SpriteEffects.None, 0f);
					}
				}
			}
		}

		private void DrawVoidLenses()
		{
			int num = 8;
			int num2 = _specialsCount[num];
			_voidLensData.Clear();
			for (int i = 0; i < num2; i++)
			{
				Point p = _specialPositions[num][i];
				VoidLensHelper voidLensHelper = new VoidLensHelper(p.ToWorldCoordinates(), 1f);
				if (!Main.gamePaused)
				{
					voidLensHelper.Update();
				}
				int selectionMode = 0;
				if (Main.InSmartCursorHighlightArea(p.X, p.Y, out var actuallySelected))
				{
					selectionMode = 1;
					if (actuallySelected)
					{
						selectionMode = 2;
					}
				}
				voidLensHelper.DrawToDrawData(_voidLensData, selectionMode);
			}
			foreach (DrawData voidLensDatum in _voidLensData)
			{
				voidLensDatum.Draw(Main.spriteBatch);
			}
		}

		private void DrawMultiTileGrass()
		{
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 zero = Vector2.Zero;
			int num = 4;
			int num2 = _specialsCount[num];
			for (int i = 0; i < num2; i++)
			{
				Point point = _specialPositions[num][i];
				int x = point.X;
				int num3 = point.Y;
				int sizeX = 1;
				int num4 = 1;
				Tile tile = Main.tile[x, num3];
				if (tile != null && tile.active())
				{
					switch (Main.tile[x, num3].type)
					{
					case 27:
						sizeX = 2;
						num4 = 5;
						break;
					case 236:
					case 238:
						sizeX = (num4 = 2);
						break;
					case 233:
						sizeX = ((Main.tile[x, num3].frameY != 0) ? 2 : 3);
						num4 = 2;
						break;
					case 530:
						sizeX = 3;
						num4 = 2;
						break;
					case 485:
					case 490:
					case 521:
					case 522:
					case 523:
					case 524:
					case 525:
					case 526:
					case 527:
						sizeX = 2;
						num4 = 2;
						break;
					case 489:
						sizeX = 2;
						num4 = 3;
						break;
					case 493:
						sizeX = 1;
						num4 = 2;
						break;
					case 519:
						sizeX = 1;
						num4 = ClimbCatTail(x, num3);
						num3 -= num4 - 1;
						break;
					}
					DrawMultiTileGrassInWind(unscaledPosition, zero, x, num3, sizeX, num4);
				}
			}
		}

		private int ClimbCatTail(int originx, int originy)
		{
			int num = 0;
			int num2 = originy;
			while (num2 > 10)
			{
				Tile tile = Main.tile[originx, num2];
				if (!tile.active() || tile.type != 519)
				{
					break;
				}
				if (tile.frameX >= 180)
				{
					num++;
					break;
				}
				num2--;
				num++;
			}
			return num;
		}

		private void DrawMultiTileVines()
		{
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 zero = Vector2.Zero;
			int num = 5;
			int num2 = _specialsCount[num];
			for (int i = 0; i < num2; i++)
			{
				Point point = _specialPositions[num][i];
				int x = point.X;
				int y = point.Y;
				int sizeX = 1;
				int sizeY = 1;
				Tile tile = Main.tile[x, y];
				if (tile != null && tile.active())
				{
					switch (Main.tile[x, y].type)
					{
					case 34:
						sizeX = 3;
						sizeY = 3;
						break;
					case 454:
						sizeX = 4;
						sizeY = 3;
						break;
					case 42:
					case 270:
					case 271:
					case 572:
					case 581:
						sizeX = 1;
						sizeY = 2;
						break;
					case 91:
						sizeX = 1;
						sizeY = 3;
						break;
					case 95:
					case 126:
					case 444:
						sizeX = 2;
						sizeY = 2;
						break;
					case 465:
					case 591:
					case 592:
						sizeX = 2;
						sizeY = 3;
						break;
					}
					DrawMultiTileVinesInWind(unscaledPosition, zero, x, y, sizeX, sizeY);
				}
			}
		}

		private void DrawVines()
		{
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 zero = Vector2.Zero;
			int num = 6;
			int num2 = _specialsCount[num];
			for (int i = 0; i < num2; i++)
			{
				Point point = _specialPositions[num][i];
				int x = point.X;
				int y = point.Y;
				DrawVineStrip(unscaledPosition, zero, x, y);
			}
		}

		private void DrawReverseVines()
		{
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 zero = Vector2.Zero;
			int num = 9;
			int num2 = _specialsCount[num];
			for (int i = 0; i < num2; i++)
			{
				Point point = _specialPositions[num][i];
				int x = point.X;
				int y = point.Y;
				DrawRisingVineStrip(unscaledPosition, zero, x, y);
			}
		}

		private void DrawMultiTileGrassInWind(Vector2 screenPosition, Vector2 offSet, int topLeftX, int topLeftY, int sizeX, int sizeY)
		{
			float windCycle = GetWindCycle(topLeftX, topLeftY, _sunflowerWindCounter);
			new Vector2((float)(sizeX * 16) * 0.5f, sizeY * 16);
			Vector2 value = new Vector2((float)(topLeftX * 16 - (int)screenPosition.X) + (float)sizeX * 16f * 0.5f, topLeftY * 16 - (int)screenPosition.Y + 16 * sizeY) + offSet;
			float num = 0.07f;
			int type = Main.tile[topLeftX, topLeftY].type;
			Texture2D texture2D = null;
			Color color = Color.Transparent;
			bool flag = InAPlaceWithWind(topLeftX, topLeftY, sizeX, sizeY);
			switch (type)
			{
			case 27:
				texture2D = TextureAssets.Flames[14].Value;
				color = Color.White;
				break;
			case 519:
				flag = InAPlaceWithWind(topLeftX, topLeftY, sizeX, 1);
				break;
			default:
				num = 0.15f;
				break;
			case 521:
			case 522:
			case 523:
			case 524:
			case 525:
			case 526:
			case 527:
				num = 0f;
				flag = false;
				break;
			}
			for (int i = topLeftX; i < topLeftX + sizeX; i++)
			{
				for (int j = topLeftY; j < topLeftY + sizeY; j++)
				{
					Tile tile = Main.tile[i, j];
					ushort type2 = tile.type;
					if (type2 != type)
					{
						continue;
					}
					Math.Abs(((float)(i - topLeftX) + 0.5f) / (float)sizeX - 0.5f);
					short tileFrameX = tile.frameX;
					short tileFrameY = tile.frameY;
					float num2 = 1f - (float)(j - topLeftY + 1) / (float)sizeY;
					if (num2 == 0f)
					{
						num2 = 0.1f;
					}
					if (!flag)
					{
						num2 = 0f;
					}
					GetTileDrawData(i, j, tile, type2, tileFrameX, tileFrameY, out var tileWidth, out var tileHeight, out var tileTop, out var halfBrickHeight, out var addFrX, out var addFrY, out var tileSpriteEffect, out var _, out var _, out var _);
					bool flag2 = _rand.Next(4) == 0;
					Color tileLight = Lighting.GetColor(i, j);
					DrawAnimatedTile_AdjustForVisionChangers(i, j, tile, type2, tileFrameX, tileFrameY, tileLight, flag2);
					tileLight = DrawTiles_GetLightOverride(j, i, tile, type2, tileFrameX, tileFrameY, tileLight);
					if (_isActiveAndNotPaused && flag2)
					{
						DrawTiles_EmitParticles(j, i, tile, type2, tileFrameX, tileFrameY, tileLight);
					}
					Vector2 value2 = new Vector2(i * 16 - (int)screenPosition.X, j * 16 - (int)screenPosition.Y + tileTop) + offSet;
					if (tile.type == 493 && tile.frameY == 0)
					{
						if (Main.WindForVisuals >= 0f)
						{
							tileSpriteEffect ^= SpriteEffects.FlipHorizontally;
						}
						if (!tileSpriteEffect.HasFlag(SpriteEffects.FlipHorizontally))
						{
							value2.X -= 6f;
						}
						else
						{
							value2.X += 6f;
						}
					}
					Vector2 vector = new Vector2(windCycle * 1f, Math.Abs(windCycle) * 2f * num2);
					Vector2 origin = value - value2;
					Texture2D tileDrawTexture = GetTileDrawTexture(tile, i, j);
					if (tileDrawTexture != null)
					{
						Main.spriteBatch.Draw(tileDrawTexture, value + new Vector2(0f, vector.Y), new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), tileLight, windCycle * num * num2, origin, 1f, tileSpriteEffect, 0f);
						if (texture2D != null)
						{
							Main.spriteBatch.Draw(texture2D, value + new Vector2(0f, vector.Y), new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), color, windCycle * num * num2, origin, 1f, tileSpriteEffect, 0f);
						}
					}
				}
			}
		}

		private void DrawVineStrip(Vector2 screenPosition, Vector2 offSet, int x, int startY)
		{
			int num = 0;
			int num2 = 0;
			Vector2 value = new Vector2(x * 16 + 8, startY * 16 - 2);
			float amount = Math.Abs(Main.WindForVisuals) / 1.2f;
			amount = MathHelper.Lerp(0.2f, 1f, amount);
			float num3 = -0.08f * amount;
			float windCycle = GetWindCycle(x, startY, _vineWindCounter);
			float num4 = 0f;
			float num5 = 0f;
			for (int i = startY; i < Main.maxTilesY - 10; i++)
			{
				Tile tile = Main.tile[x, i];
				if (tile != null)
				{
					ushort type = tile.type;
					if (!tile.active() || !TileID.Sets.VineThreads[type])
					{
						break;
					}
					num++;
					if (num2 >= 5)
					{
						num3 += 0.0075f * amount;
					}
					if (num2 >= 2)
					{
						num3 += 0.0025f;
					}
					if (WallID.Sets.AllowsWind[tile.wall] && (double)i < Main.worldSurface)
					{
						num2++;
					}
					float windGridPush = GetWindGridPush(x, i, 20, 0.01f);
					num4 = ((windGridPush != 0f || num5 == 0f) ? (num4 - windGridPush) : (num4 * -0.78f));
					num5 = windGridPush;
					short tileFrameX = tile.frameX;
					short tileFrameY = tile.frameY;
					Color color = Lighting.GetColor(x, i);
					GetTileDrawData(x, i, tile, type, tileFrameX, tileFrameY, out var tileWidth, out var tileHeight, out var tileTop, out var halfBrickHeight, out var addFrX, out var addFrY, out var tileSpriteEffect, out var _, out var _, out var _);
					Vector2 position = new Vector2(-(int)screenPosition.X, -(int)screenPosition.Y) + offSet + value;
					if (tile.color() == 31)
					{
						color = Color.White;
					}
					float num6 = (float)num2 * num3 * windCycle + num4;
					Texture2D tileDrawTexture = GetTileDrawTexture(tile, x, i);
					if (tileDrawTexture == null)
					{
						break;
					}
					Main.spriteBatch.Draw(tileDrawTexture, position, new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), color, num6, new Vector2(tileWidth / 2, halfBrickHeight - tileTop), 1f, tileSpriteEffect, 0f);
					value += (num6 + (float)Math.PI / 2f).ToRotationVector2() * 16f;
				}
			}
		}

		private void DrawRisingVineStrip(Vector2 screenPosition, Vector2 offSet, int x, int startY)
		{
			int num = 0;
			int num2 = 0;
			Vector2 value = new Vector2(x * 16 + 8, startY * 16 + 16 + 2);
			float amount = Math.Abs(Main.WindForVisuals) / 1.2f;
			amount = MathHelper.Lerp(0.2f, 1f, amount);
			float num3 = -0.08f * amount;
			float windCycle = GetWindCycle(x, startY, _vineWindCounter);
			float num4 = 0f;
			float num5 = 0f;
			for (int num6 = startY; num6 > 10; num6--)
			{
				Tile tile = Main.tile[x, num6];
				if (tile != null)
				{
					ushort type = tile.type;
					if (!tile.active() || !TileID.Sets.ReverseVineThreads[type])
					{
						break;
					}
					num++;
					if (num2 >= 5)
					{
						num3 += 0.0075f * amount;
					}
					if (num2 >= 2)
					{
						num3 += 0.0025f;
					}
					if (WallID.Sets.AllowsWind[tile.wall] && (double)num6 < Main.worldSurface)
					{
						num2++;
					}
					float windGridPush = GetWindGridPush(x, num6, 40, -0.004f);
					num4 = ((windGridPush != 0f || num5 == 0f) ? (num4 - windGridPush) : (num4 * -0.78f));
					num5 = windGridPush;
					short tileFrameX = tile.frameX;
					short tileFrameY = tile.frameY;
					Color color = Lighting.GetColor(x, num6);
					GetTileDrawData(x, num6, tile, type, tileFrameX, tileFrameY, out var tileWidth, out var tileHeight, out var tileTop, out var halfBrickHeight, out var addFrX, out var addFrY, out var tileSpriteEffect, out var _, out var _, out var _);
					Vector2 position = new Vector2(-(int)screenPosition.X, -(int)screenPosition.Y) + offSet + value;
					float num7 = (float)num2 * (0f - num3) * windCycle + num4;
					Texture2D tileDrawTexture = GetTileDrawTexture(tile, x, num6);
					if (tileDrawTexture == null)
					{
						break;
					}
					Main.spriteBatch.Draw(tileDrawTexture, position, new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), color, num7, new Vector2(tileWidth / 2, halfBrickHeight - tileTop + tileHeight), 1f, tileSpriteEffect, 0f);
					value += (num7 - (float)Math.PI / 2f).ToRotationVector2() * 16f;
				}
			}
		}

		private float GetAverageWindGridPush(int topLeftX, int topLeftY, int sizeX, int sizeY, int totalPushTime, float pushForcePerFrame)
		{
			float num = 0f;
			int num2 = 0;
			for (int i = 0; i < sizeX; i++)
			{
				for (int j = 0; j < sizeY; j++)
				{
					float windGridPush = GetWindGridPush(topLeftX + i, topLeftY + j, totalPushTime, pushForcePerFrame);
					if (windGridPush != 0f)
					{
						num += windGridPush;
						num2++;
					}
				}
			}
			if (num2 == 0)
			{
				return 0f;
			}
			return num / (float)num2;
		}

		private float GetHighestWindGridPushComplex(int topLeftX, int topLeftY, int sizeX, int sizeY, int totalPushTime, float pushForcePerFrame, int loops, bool swapLoopDir)
		{
			float result = 0f;
			int num = int.MaxValue;
			for (int i = 0; i < 1; i++)
			{
				for (int j = 0; j < sizeY; j++)
				{
					_windGrid.GetWindTime(topLeftX + i + sizeX / 2, topLeftY + j, totalPushTime, out var windTimeLeft, out var _);
					float windGridPushComplex = GetWindGridPushComplex(topLeftX + i, topLeftY + j, totalPushTime, pushForcePerFrame, loops, swapLoopDir);
					if (windTimeLeft < num && windTimeLeft != 0)
					{
						result = windGridPushComplex;
						num = windTimeLeft;
					}
				}
			}
			return result;
		}

		private void DrawMultiTileVinesInWind(Vector2 screenPosition, Vector2 offSet, int topLeftX, int topLeftY, int sizeX, int sizeY)
		{
			float windCycle = GetWindCycle(topLeftX, topLeftY, _sunflowerWindCounter);
			float num = windCycle;
			int totalPushTime = 60;
			float pushForcePerFrame = 1.26f;
			float highestWindGridPushComplex = GetHighestWindGridPushComplex(topLeftX, topLeftY, sizeX, sizeY, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);
			windCycle += highestWindGridPushComplex;
			new Vector2((float)(sizeX * 16) * 0.5f, 0f);
			Vector2 value = new Vector2((float)(topLeftX * 16 - (int)screenPosition.X) + (float)sizeX * 16f * 0.5f, topLeftY * 16 - (int)screenPosition.Y) + offSet;
			float num2 = 0.07f;
			Tile tile = Main.tile[topLeftX, topLeftY];
			int type = tile.type;
			Vector2 vector = new Vector2(0f, -2f);
			value += vector;
			Texture2D texture2D = null;
			Color color = Color.Transparent;
			float? num3 = null;
			float num4 = 1f;
			float num5 = -4f;
			bool flag = false;
			num2 = 0.15f;
			switch (type)
			{
			case 34:
			case 126:
				num3 = 1f;
				num5 = 0f;
				switch (tile.frameY / 54 + tile.frameX / 108 * 37)
				{
				case 9:
					num3 = null;
					num5 = -1f;
					flag = true;
					num2 *= 0.3f;
					break;
				case 11:
					num2 *= 0.5f;
					break;
				case 12:
					num3 = null;
					num5 = -1f;
					break;
				case 18:
					num3 = null;
					num5 = -1f;
					break;
				case 21:
					num3 = null;
					num5 = -1f;
					break;
				case 23:
					num3 = 0f;
					break;
				case 25:
					num3 = null;
					num5 = -1f;
					flag = true;
					break;
				case 32:
					num2 *= 0.5f;
					break;
				case 33:
					num2 *= 0.5f;
					break;
				case 35:
					num3 = 0f;
					break;
				case 36:
					num3 = null;
					num5 = -1f;
					flag = true;
					break;
				case 37:
					num3 = null;
					num5 = -1f;
					flag = true;
					num2 *= 0.5f;
					break;
				case 39:
					num3 = null;
					num5 = -1f;
					flag = true;
					break;
				case 40:
				case 41:
				case 42:
				case 43:
					num3 = null;
					num5 = -2f;
					flag = true;
					num2 *= 0.5f;
					break;
				case 44:
					num3 = null;
					num5 = -3f;
					break;
				}
				break;
			case 42:
				num3 = 1f;
				num5 = 0f;
				switch (tile.frameY / 36)
				{
				case 0:
					num3 = null;
					num5 = -1f;
					break;
				case 9:
					num3 = 0f;
					break;
				case 12:
					num3 = null;
					num5 = -1f;
					break;
				case 14:
					num3 = null;
					num5 = -1f;
					break;
				case 28:
					num3 = null;
					num5 = -1f;
					break;
				case 30:
					num3 = 0f;
					break;
				case 32:
					num3 = 0f;
					break;
				case 33:
					num3 = 0f;
					break;
				case 34:
					num3 = null;
					num5 = -1f;
					break;
				case 35:
					num3 = 0f;
					break;
				case 38:
					num3 = null;
					num5 = -1f;
					break;
				case 39:
					num3 = null;
					num5 = -1f;
					flag = true;
					break;
				case 40:
				case 41:
				case 42:
				case 43:
					num3 = 0f;
					num3 = null;
					num5 = -1f;
					flag = true;
					break;
				}
				break;
			case 95:
			case 270:
			case 271:
			case 444:
			case 454:
			case 572:
			case 581:
				num3 = 1f;
				num5 = 0f;
				break;
			case 591:
				num4 = 0.5f;
				num5 = -2f;
				break;
			case 592:
				num4 = 0.5f;
				num5 = -2f;
				texture2D = TextureAssets.GlowMask[294].Value;
				color = new Color(255, 255, 255, 0);
				break;
			}
			if (flag)
			{
				value += new Vector2(0f, 16f);
			}
			num2 *= -1f;
			if (!InAPlaceWithWind(topLeftX, topLeftY, sizeX, sizeY))
			{
				windCycle -= num;
			}
			ulong num6 = 0uL;
			for (int i = topLeftX; i < topLeftX + sizeX; i++)
			{
				for (int j = topLeftY; j < topLeftY + sizeY; j++)
				{
					Tile tile2 = Main.tile[i, j];
					ushort type2 = tile2.type;
					if (type2 != type)
					{
						continue;
					}
					Math.Abs(((float)(i - topLeftX) + 0.5f) / (float)sizeX - 0.5f);
					short tileFrameX = tile2.frameX;
					short tileFrameY = tile2.frameY;
					float num7 = (float)(j - topLeftY + 1) / (float)sizeY;
					if (num7 == 0f)
					{
						num7 = 0.1f;
					}
					if (num3.HasValue)
					{
						num7 = num3.Value;
					}
					if (flag && j == topLeftY)
					{
						num7 = 0f;
					}
					GetTileDrawData(i, j, tile2, type2, tileFrameX, tileFrameY, out var tileWidth, out var tileHeight, out var tileTop, out var halfBrickHeight, out var addFrX, out var addFrY, out var tileSpriteEffect, out var _, out var _, out var _);
					bool flag2 = _rand.Next(4) == 0;
					Color tileLight = Lighting.GetColor(i, j);
					DrawAnimatedTile_AdjustForVisionChangers(i, j, tile2, type2, tileFrameX, tileFrameY, tileLight, flag2);
					tileLight = DrawTiles_GetLightOverride(j, i, tile2, type2, tileFrameX, tileFrameY, tileLight);
					if (_isActiveAndNotPaused && flag2)
					{
						DrawTiles_EmitParticles(j, i, tile2, type2, tileFrameX, tileFrameY, tileLight);
					}
					Vector2 value2 = new Vector2(i * 16 - (int)screenPosition.X, j * 16 - (int)screenPosition.Y + tileTop) + offSet;
					value2 += vector;
					Vector2 vector2 = new Vector2(windCycle * num4, Math.Abs(windCycle) * num5 * num7);
					Vector2 origin = value - value2;
					Texture2D tileDrawTexture = GetTileDrawTexture(tile2, i, j);
					if (tileDrawTexture != null)
					{
						Vector2 vector3 = value + new Vector2(0f, vector2.Y);
						Rectangle value3 = new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight);
						float rotation = windCycle * num2 * num7;
						Main.spriteBatch.Draw(tileDrawTexture, vector3, value3, tileLight, rotation, origin, 1f, tileSpriteEffect, 0f);
						if (texture2D != null)
						{
							Main.spriteBatch.Draw(texture2D, vector3, value3, color, rotation, origin, 1f, tileSpriteEffect, 0f);
						}
						TileFlameData tileFlameData = GetTileFlameData(i, j, type2, tileFrameY);
						if (num6 == 0L)
						{
							num6 = tileFlameData.flameSeed;
						}
						tileFlameData.flameSeed = num6;
						for (int k = 0; k < tileFlameData.flameCount; k++)
						{
							float x = (float)Utils.RandomInt(tileFlameData.flameSeed, tileFlameData.flameRangeXMin, tileFlameData.flameRangeXMax) * tileFlameData.flameRangeMultX;
							float y = (float)Utils.RandomInt(tileFlameData.flameSeed, tileFlameData.flameRangeYMin, tileFlameData.flameRangeYMax) * tileFlameData.flameRangeMultY;
							Main.spriteBatch.Draw(tileFlameData.flameTexture, vector3 + new Vector2(x, y), value3, tileFlameData.flameColor, rotation, origin, 1f, tileSpriteEffect, 0f);
						}
					}
				}
			}
		}

		private void EmitAlchemyHerbParticles(int j, int i, int style)
		{
			if (style == 0 && _rand.Next(100) == 0)
			{
				int num = Dust.NewDust(new Vector2(i * 16, j * 16 - 4), 16, 16, 19, 0f, 0f, 160, default(Color), 0.1f);
				_dust[num].velocity.X /= 2f;
				_dust[num].velocity.Y /= 2f;
				_dust[num].noGravity = true;
				_dust[num].fadeIn = 1f;
			}
			if (style == 1 && _rand.Next(100) == 0)
			{
				Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 41, 0f, 0f, 250, default(Color), 0.8f);
			}
			if (style == 3)
			{
				if (_rand.Next(200) == 0)
				{
					int num2 = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 14, 0f, 0f, 100, default(Color), 0.2f);
					_dust[num2].fadeIn = 1.2f;
				}
				if (_rand.Next(75) == 0)
				{
					int num3 = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, 27, 0f, 0f, 100);
					_dust[num3].velocity.X /= 2f;
					_dust[num3].velocity.Y /= 2f;
				}
			}
			if (style == 4 && _rand.Next(150) == 0)
			{
				int num4 = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 8, 16);
				_dust[num4].velocity.X /= 3f;
				_dust[num4].velocity.Y /= 3f;
				_dust[num4].velocity.Y -= 0.7f;
				_dust[num4].alpha = 50;
				_dust[num4].scale *= 0.1f;
				_dust[num4].fadeIn = 0.9f;
				_dust[num4].noGravity = true;
			}
			if (style == 5 && _rand.Next(40) == 0)
			{
				int num5 = Dust.NewDust(new Vector2(i * 16, j * 16 - 6), 16, 16, 6, 0f, 0f, 0, default(Color), 1.5f);
				_dust[num5].velocity.Y -= 2f;
				_dust[num5].noGravity = true;
			}
			if (style == 6 && _rand.Next(30) == 0)
			{
				int num6 = Dust.NewDust(newColor: new Color(50, 255, 255, 255), Position: new Vector2(i * 16, j * 16), Width: 16, Height: 16, Type: 43, SpeedX: 0f, SpeedY: 0f, Alpha: 254, Scale: 0.5f);
				_dust[num6].velocity *= 0f;
			}
		}

		private bool IsAlchemyPlantHarvestable(int style)
		{
			if (style == 0 && Main.dayTime)
			{
				return true;
			}
			if (style == 1 && !Main.dayTime)
			{
				return true;
			}
			if (style == 3 && !Main.dayTime && (Main.bloodMoon || Main.moonPhase == 0))
			{
				return true;
			}
			if (style == 4 && (Main.raining || Main.cloudAlpha > 0f))
			{
				return true;
			}
			if (style == 5 && !Main.raining && Main.time > 40500.0)
			{
				return true;
			}
			return false;
		}
	}
}
