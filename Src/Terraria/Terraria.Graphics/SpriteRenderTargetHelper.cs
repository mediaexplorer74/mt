using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using GameManager.DataStructures;

namespace GameManager.Graphics
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct SpriteRenderTargetHelper
	{
		public static void GetDrawBoundary(List<DrawData> playerDrawData, out Vector2 lowest, out Vector2 highest)
		{
			lowest = Vector2.Zero;
			highest = Vector2.Zero;
			for (int i = 0; i <= playerDrawData.Count; i++)
			{
				if (i != playerDrawData.Count)
				{
					DrawData cdd = playerDrawData[i];
					if (i == 0)
					{
						lowest = cdd.position;
						highest = cdd.position;
					}
					GetHighsAndLowsOf(lowest, highest, cdd);
				}
			}
		}

		public static void GetHighsAndLowsOf(Vector2 lowest, Vector2 highest, DrawData cdd)
		{
			Vector2 origin = cdd.origin;
			Rectangle rectangle = cdd.destinationRectangle;
			if (cdd.sourceRect.HasValue)
			{
				rectangle = cdd.sourceRect.Value;
			}
			if (!cdd.sourceRect.HasValue)
			{
				rectangle = cdd.texture.Frame();
			}
			rectangle.X = 0;
			rectangle.Y = 0;
			Vector2 pos = cdd.position;
			GetHighsAndLowsOf(lowest, highest, cdd, pos, origin, new Vector2(0f, 0f));
			GetHighsAndLowsOf(lowest, highest, cdd, pos, origin, new Vector2(rectangle.Width, 0f));
			GetHighsAndLowsOf(lowest, highest, cdd, pos, origin, new Vector2(0f, rectangle.Height));
			GetHighsAndLowsOf(lowest, highest, cdd, pos, origin, new Vector2(rectangle.Width, rectangle.Height));
		}

		public static void GetHighsAndLowsOf(Vector2 lowest, Vector2 highest, DrawData cdd, Vector2 pos, Vector2 origin, Vector2 corner)
		{
			Vector2 corner2 = GetCorner(cdd, pos, origin, corner);
			lowest = Vector2.Min(lowest, corner2);
			highest = Vector2.Max(highest, corner2);
		}

		public static Vector2 GetCorner(DrawData cdd, Vector2 pos, Vector2 origin, Vector2 corner)
		{
			Vector2 spinningpoint = corner - origin;
			return pos + spinningpoint.RotatedBy(cdd.rotation) * cdd.scale;
		}
	}
}
