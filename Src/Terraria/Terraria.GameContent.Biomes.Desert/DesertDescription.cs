using Microsoft.Xna.Framework;

namespace GameManager.GameContent.Biomes.Desert
{
	public class DesertDescription
	{
		public static readonly DesertDescription Invalid = new DesertDescription
		{
			IsValid = false
		};

		private static readonly Vector2 DefaultBlockScale = new Vector2(4f, 2f);

		private const int SCAN_PADDING = 5;

		public Rectangle CombinedArea
		{
			get;
			private set;
		}

		public Rectangle Desert
		{
			get;
			private set;
		}

		public Rectangle Hive
		{
			get;
			private set;
		}

		public Vector2 BlockScale
		{
			get;
			private set;
		}

		public int BlockColumnCount
		{
			get;
			private set;
		}

		public int BlockRowCount
		{
			get;
			private set;
		}

		public bool IsValid
		{
			get;
			private set;
		}

		public SurfaceMap Surface
		{
			get;
			private set;
		}

		private DesertDescription()
		{
		}

		public void UpdateSurfaceMap()
		{
			Surface = SurfaceMap.FromArea(CombinedArea.Left - 5, CombinedArea.Width + 10);
		}

		public static DesertDescription CreateFromPlacement(Point origin)
		{
			Vector2 defaultBlockScale = DefaultBlockScale;
			float num = (float)Main.maxTilesX / 4200f;
			int num2 = (int)(80f * num);
			int num3 = (int)((WorldGen.genRand.NextFloat() + 1f) * 170f * num);
			int num4 = (int)(defaultBlockScale.X * (float)num2);
			int num5 = (int)(defaultBlockScale.Y * (float)num3);
			origin.X -= num4 / 2;
			SurfaceMap surfaceMap = SurfaceMap.FromArea(origin.X - 5, num4 + 10);
			if (RowHasInvalidTiles(origin.X, surfaceMap.Bottom, num4))
			{
				return Invalid;
			}
			int num6 = (int)(surfaceMap.Average + (float)surfaceMap.Bottom) / 2;
			origin.Y = num6 + WorldGen.genRand.Next(40, 60);
			return new DesertDescription
			{
				CombinedArea = new Rectangle(origin.X, num6, num4, origin.Y + num5 - num6),
				Hive = new Rectangle(origin.X, origin.Y, num4, num5),
				Desert = new Rectangle(origin.X, num6, num4, origin.Y + num5 / 2 - num6),
				BlockScale = defaultBlockScale,
				BlockColumnCount = num2,
				BlockRowCount = num3,
				Surface = surfaceMap,
				IsValid = true
			};
		}

		private static bool RowHasInvalidTiles(int startX, int startY, int width)
		{
			if (WorldGen.skipDesertTileCheck)
			{
				return false;
			}
			for (int i = startX; i < startX + width; i++)
			{
				switch (Main.tile[i, startY].type)
				{
				case 59:
				case 60:
					return true;
				case 147:
				case 161:
					return true;
				}
			}
			return false;
		}
	}
}
