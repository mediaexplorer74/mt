using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameManager.Utilities;
using GameManager.WorldBuilding;

namespace GameManager.GameContent.Biomes.Desert
{
	public static class ChambersEntrance
	{
		private struct PathConnection
		{
			public readonly Vector2 Position;

			public readonly float Direction;

			public PathConnection(Point position, int direction)
			{
				Position = new Vector2(position.X, position.Y);
				Direction = direction;
			}
		}

		public static void Place(DesertDescription description)
		{
			int num = description.Desert.Center.X + WorldGen.genRand.Next(-40, 41);
			Point position = new Point(num, description.Surface[num]);
			PlaceAt(description, position);
		}

		private static void PlaceAt(DesertDescription description, Point position)
		{
			ShapeData shapeData = new ShapeData();
			Point origin = new Point(position.X, position.Y + 2);
			WorldUtils.Gen(origin, new Shapes.Circle(24, 12), Actions.Chain(new Modifiers.Blotches(), new Actions.SetTile(53).Output(shapeData)));
			UnifiedRandom genRand = WorldGen.genRand;
			ShapeData data = new ShapeData();
			int num = description.Hive.Top - position.Y;
			int num2 = ((genRand.Next(2) != 0) ? 1 : (-1));
			List<PathConnection> list = new List<PathConnection>
			{
				new PathConnection(new Point(position.X + -num2 * 26, position.Y - 8), num2)
			};
			int num3 = genRand.Next(2, 4);
			for (int i = 0; i < num3; i++)
			{
				int num4 = (int)((float)(i + 1) / (float)num3 * (float)num) + genRand.Next(-8, 9);
				int num5 = num2 * genRand.Next(20, 41);
				int num6 = genRand.Next(18, 29);
				WorldUtils.Gen(position, new Shapes.Circle(num6 / 2, 3), Actions.Chain(new Modifiers.Offset(num5, num4), new Modifiers.Blotches(), new Actions.Clear().Output(data), new Actions.PlaceWall(187)));
				list.Add(new PathConnection(new Point(num5 + num6 / 2 * -num2 + position.X, num4 + position.Y), -num2));
				num2 *= -1;
			}
			WorldUtils.Gen(position, new ModShapes.OuterOutline(data), Actions.Chain(new Modifiers.Expand(1), new Modifiers.OnlyTiles(53), new Actions.SetTile(397), new Actions.PlaceWall(187)));
			GenShapeActionPair pair = new GenShapeActionPair(new Shapes.Rectangle(2, 4), Actions.Chain(new Modifiers.IsSolid(), new Modifiers.Blotches(), new Actions.Clear(), new Modifiers.Expand(1), new Actions.PlaceWall(187), new Modifiers.OnlyTiles(53), new Actions.SetTile(397)));
			for (int j = 1; j < list.Count; j++)
			{
				PathConnection pathConnection = list[j - 1];
				PathConnection pathConnection2 = list[j];
				float num7 = Math.Abs(pathConnection2.Position.X - pathConnection.Position.X) * 1.5f;
				for (float num8 = 0f; num8 <= 1f; num8 += 0.02f)
				{
					Vector2 value = new Vector2(pathConnection.Position.X + pathConnection.Direction * num7 * num8, pathConnection.Position.Y);
					Vector2 value2 = new Vector2(pathConnection2.Position.X + pathConnection2.Direction * num7 * (1f - num8), pathConnection2.Position.Y);
					Vector2 vector = Vector2.Lerp(pathConnection.Position, pathConnection2.Position, num8);
					Vector2 value3 = Vector2.Lerp(value, vector, num8);
					Vector2 value4 = Vector2.Lerp(vector, value2, num8);
					WorldUtils.Gen(Vector2.Lerp(value3, value4, num8).ToPoint(), pair);
				}
			}
			WorldUtils.Gen(origin, new Shapes.Rectangle(new Rectangle(-29, -12, 58, 12)), Actions.Chain(new Modifiers.NotInShape(shapeData), new Modifiers.Expand(1), new Actions.PlaceWall(0)));
		}
	}
}
