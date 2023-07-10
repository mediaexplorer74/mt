using System;
using Microsoft.Xna.Framework;

namespace GameManager.GameContent.Biomes.Desert
{
	public static class PitEntrance
	{
		public static void Place(DesertDescription description)
		{
			int holeRadius = WorldGen.genRand.Next(6, 9);
			Point center = description.CombinedArea.Center;
			center.Y = description.Surface[center.X];
			PlaceAt(description, center, holeRadius);
		}

		private static void PlaceAt(DesertDescription description, Point position, int holeRadius)
		{
			for (int i = -holeRadius - 3; i < holeRadius + 3; i++)
			{
				for (int j = description.Surface[i + position.X]; j <= description.Hive.Top + 10; j++)
				{
					float value = (float)(j - description.Surface[i + position.X]) / (float)(description.Hive.Top - description.Desert.Top);
					value = MathHelper.Clamp(value, 0f, 1f);
					int num = (int)(GetHoleRadiusScaleAt(value) * (float)holeRadius);
					if (Math.Abs(i) < num)
					{
						Main.tile[i + position.X, j].ClearEverything();
					}
					else if (Math.Abs(i) < num + 3 && value > 0.35f)
					{
						Main.tile[i + position.X, j].ResetToType(397);
					}
					float num2 = Math.Abs((float)i / (float)holeRadius);
					num2 *= num2;
					if (Math.Abs(i) < num + 3 && (float)(j - position.Y) > 15f - 3f * num2)
					{
						Main.tile[i + position.X, j].wall = 187;
						WorldGen.SquareWallFrame(i + position.X, j - 1);
						WorldGen.SquareWallFrame(i + position.X, j);
					}
				}
			}
			holeRadius += 4;
			for (int k = -holeRadius; k < holeRadius; k++)
			{
				int num3 = holeRadius - Math.Abs(k);
				num3 = Math.Min(10, num3 * num3);
				for (int l = 0; l < num3; l++)
				{
					Main.tile[k + position.X, l + description.Surface[k + position.X]].ClearEverything();
				}
			}
		}

		private static float GetHoleRadiusScaleAt(float yProgress)
		{
			if (yProgress < 0.6f)
			{
				return 1f;
			}
			return (1f - SmootherStep((yProgress - 0.6f) / 0.4f)) * 0.5f + 0.5f;
		}

		private static float SmootherStep(float delta)
		{
			delta = MathHelper.Clamp(delta, 0f, 1f);
			return 1f - (float)Math.Cos(delta * (float)Math.PI) * 0.5f - 0.5f;
		}
	}
}
