using Microsoft.Xna.Framework;
using GameManager.Audio;
using GameManager.DataStructures;
using GameManager.Enums;
using GameManager.Graphics.Shaders;
using GameManager.ID;

namespace GameManager
{
	public static class DelegateMethods
	{
		public static class Minecart
		{
			public static Vector2 rotationOrigin;

			public static float rotation;

			public static void Sparks(Vector2 dustPosition)
			{
				dustPosition += new Vector2((Main.rand.Next(2) == 0) ? 13 : (-13), 0f).RotatedBy(rotation);
				int num = Dust.NewDust(dustPosition, 1, 1, 213, Main.rand.Next(-2, 3), Main.rand.Next(-2, 3));
				Main.dust[num].noGravity = true;
				Main.dust[num].fadeIn = Main.dust[num].scale + 1f + 0.01f * (float)Main.rand.Next(0, 51);
				Main.dust[num].noGravity = true;
				Main.dust[num].velocity *= (float)Main.rand.Next(15, 51) * 0.01f;
				Main.dust[num].velocity.X *= (float)Main.rand.Next(25, 101) * 0.01f;
				Main.dust[num].velocity.Y -= (float)Main.rand.Next(15, 31) * 0.1f;
				Main.dust[num].position.Y -= 4f;
				if (Main.rand.Next(3) != 0)
				{
					Main.dust[num].noGravity = false;
				}
				else
				{
					Main.dust[num].scale *= 0.6f;
				}
			}

			public static void LandingSound(Vector2 Position, int Width, int Height)
			{
				SoundEngine.PlaySound(SoundID.Item53, (int)Position.X + Width / 2, (int)Position.Y + Height / 2);
			}

			public static void BumperSound(Vector2 Position, int Width, int Height)
			{
				SoundEngine.PlaySound(SoundID.Item56, (int)Position.X + Width / 2, (int)Position.Y + Height / 2);
			}

			public static void SparksMech(Vector2 dustPosition)
			{
				dustPosition += new Vector2((Main.rand.Next(2) == 0) ? 13 : (-13), 0f).RotatedBy(rotation);
				int num = Dust.NewDust(dustPosition, 1, 1, 260, Main.rand.Next(-2, 3), Main.rand.Next(-2, 3));
				Main.dust[num].noGravity = true;
				Main.dust[num].fadeIn = Main.dust[num].scale + 0.5f + 0.01f * (float)Main.rand.Next(0, 51);
				Main.dust[num].noGravity = true;
				Main.dust[num].velocity *= (float)Main.rand.Next(15, 51) * 0.01f;
				Main.dust[num].velocity.X *= (float)Main.rand.Next(25, 101) * 0.01f;
				Main.dust[num].velocity.Y -= (float)Main.rand.Next(15, 31) * 0.1f;
				Main.dust[num].position.Y -= 4f;
				if (Main.rand.Next(3) != 0)
				{
					Main.dust[num].noGravity = false;
				}
				else
				{
					Main.dust[num].scale *= 0.6f;
				}
			}

			public static void SparksMeow(Vector2 dustPosition)
			{
				dustPosition += new Vector2((Main.rand.Next(2) == 0) ? 13 : (-13), 0f).RotatedBy(rotation);
				int num = Dust.NewDust(dustPosition, 1, 1, 213, Main.rand.Next(-2, 3), Main.rand.Next(-2, 3));
				Main.dust[num].shader = GameShaders.Armor.GetShaderFromItemId(2870);
				Main.dust[num].noGravity = true;
				Main.dust[num].fadeIn = Main.dust[num].scale + 1f + 0.01f * (float)Main.rand.Next(0, 51);
				Main.dust[num].noGravity = true;
				Main.dust[num].velocity *= (float)Main.rand.Next(15, 51) * 0.01f;
				Main.dust[num].velocity.X *= (float)Main.rand.Next(25, 101) * 0.01f;
				Main.dust[num].velocity.Y -= (float)Main.rand.Next(15, 31) * 0.1f;
				Main.dust[num].position.Y -= 4f;
				if (Main.rand.Next(3) != 0)
				{
					Main.dust[num].noGravity = false;
				}
				else
				{
					Main.dust[num].scale *= 0.6f;
				}
			}
		}

		public static Vector3 v3_1 = Vector3.Zero;

		public static Vector2 v2_1 = Vector2.Zero;

		public static float f_1 = 0f;

		public static Color c_1 = Color.Transparent;

		public static int i_1;

		public static TileCuttingContext tilecut_0 = TileCuttingContext.Unknown;

		public static Color ColorLerp_BlackToWhite(float percent)
		{
			return Color.Lerp(Color.Black, Color.White, percent);
		}

		public static Color ColorLerp_HSL_H(float percent)
		{
			return Main.hslToRgb(percent, 1f, 0.5f);
		}

		public static Color ColorLerp_HSL_S(float percent)
		{
			return Main.hslToRgb(v3_1.X, percent, v3_1.Z);
		}

		public static Color ColorLerp_HSL_L(float percent)
		{
			return Main.hslToRgb(v3_1.X, v3_1.Y, 0.15f + 0.85f * percent);
		}

		public static Color ColorLerp_HSL_O(float percent)
		{
			return Color.Lerp(Color.White, Main.hslToRgb(v3_1.X, v3_1.Y, v3_1.Z), percent);
		}

		public static bool SpreadDirt(int x, int y)
		{
			if (Vector2.Distance(v2_1, new Vector2(x, y)) > f_1)
			{
				return false;
			}
			if (WorldGen.PlaceTile(x, y, 0))
			{
				if (Main.netMode != 0)
				{
					NetMessage.SendData(17, -1, -1, null, 1, x, y);
				}
				Vector2 position = new Vector2(x * 16, y * 16);
				int type = 0;
				for (int i = 0; i < 3; i++)
				{
					Dust dust = Dust.NewDustDirect(position, 16, 16, type, 0f, 0f, 100, Color.Transparent, 2.2f);
					dust.noGravity = true;
					dust.velocity.Y -= 1.2f;
					dust.velocity *= 4f;
					Dust dust2 = Dust.NewDustDirect(position, 16, 16, type, 0f, 0f, 100, Color.Transparent, 1.3f);
					dust2.velocity.Y -= 1.2f;
					dust2.velocity *= 2f;
				}
				int num = y + 1;
				if (Main.tile[x, num] != null && !TileID.Sets.Platforms[Main.tile[x, num].type] && (Main.tile[x, num].topSlope() || Main.tile[x, num].halfBrick()))
				{
					WorldGen.SlopeTile(x, num);
					if (Main.netMode != 0)
					{
						NetMessage.SendData(17, -1, -1, null, 14, x, num);
					}
				}
				num = y - 1;
				if (Main.tile[x, num] != null && !TileID.Sets.Platforms[Main.tile[x, num].type] && Main.tile[x, num].bottomSlope())
				{
					WorldGen.SlopeTile(x, num);
					if (Main.netMode != 0)
					{
						NetMessage.SendData(17, -1, -1, null, 14, x, num);
					}
				}
				return true;
			}
			return false;
		}

		public static bool SpreadWater(int x, int y)
		{
			if (Vector2.Distance(v2_1, new Vector2(x, y)) > f_1)
			{
				return false;
			}
			if (WorldGen.PlaceLiquid(x, y, 0, byte.MaxValue))
			{
				Vector2 position = new Vector2(x * 16, y * 16);
				int type = Dust.dustWater();
				for (int i = 0; i < 3; i++)
				{
					Dust dust = Dust.NewDustDirect(position, 16, 16, type, 0f, 0f, 100, Color.Transparent, 2.2f);
					dust.noGravity = true;
					dust.velocity.Y -= 1.2f;
					dust.velocity *= 7f;
					Dust dust2 = Dust.NewDustDirect(position, 16, 16, type, 0f, 0f, 100, Color.Transparent, 1.3f);
					dust2.velocity.Y -= 1.2f;
					dust2.velocity *= 4f;
				}
				return true;
			}
			return false;
		}

		public static bool SpreadHoney(int x, int y)
		{
			if (Vector2.Distance(v2_1, new Vector2(x, y)) > f_1)
			{
				return false;
			}
			if (WorldGen.PlaceLiquid(x, y, 2, byte.MaxValue))
			{
				Vector2 position = new Vector2(x * 16, y * 16);
				int type = 152;
				for (int i = 0; i < 3; i++)
				{
					Dust dust = Dust.NewDustDirect(position, 16, 16, type, 0f, 0f, 100, Color.Transparent, 2.2f);
					dust.velocity.Y -= 1.2f;
					dust.velocity *= 7f;
					Dust dust2 = Dust.NewDustDirect(position, 16, 16, type, 0f, 0f, 100, Color.Transparent, 1.3f);
					dust2.velocity.Y -= 1.2f;
					dust2.velocity *= 4f;
				}
				return true;
			}
			return false;
		}

		public static bool SpreadLava(int x, int y)
		{
			if (Vector2.Distance(v2_1, new Vector2(x, y)) > f_1)
			{
				return false;
			}
			if (WorldGen.PlaceLiquid(x, y, 1, byte.MaxValue))
			{
				Vector2 position = new Vector2(x * 16, y * 16);
				int type = 35;
				for (int i = 0; i < 3; i++)
				{
					Dust.NewDustDirect(position, 16, 16, type, 0f, 0f, 100, Color.Transparent, 1.2f).velocity *= 7f;
					Dust.NewDustDirect(position, 16, 16, type, 0f, 0f, 100, Color.Transparent, 0.8f).velocity *= 4f;
				}
				return true;
			}
			return false;
		}

		public static bool SpreadDry(int x, int y)
		{
			if (Vector2.Distance(v2_1, new Vector2(x, y)) > f_1)
			{
				return false;
			}
			if (WorldGen.EmptyLiquid(x, y))
			{
				Vector2 position = new Vector2(x * 16, y * 16);
				int type = 31;
				for (int i = 0; i < 3; i++)
				{
					Dust dust = Dust.NewDustDirect(position, 16, 16, type, 0f, 0f, 100, Color.Transparent, 1.2f);
					dust.noGravity = true;
					dust.velocity *= 7f;
					Dust.NewDustDirect(position, 16, 16, type, 0f, 0f, 100, Color.Transparent, 0.8f).velocity *= 4f;
				}
				return true;
			}
			return false;
		}

		public static bool SpreadTest(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			if (WorldGen.SolidTile(x, y) || tile.wall != 0)
			{
				tile.active();
				return false;
			}
			return true;
		}

		public static bool TestDust(int x, int y)
		{
			if (x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
			{
				return false;
			}
			int num = Dust.NewDust(new Vector2(x, y) * 16f + new Vector2(8f), 0, 0, 6);
			Main.dust[num].noGravity = true;
			Main.dust[num].noLight = true;
			return true;
		}

		public static bool CastLight(int x, int y)
		{
			if (x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
			{
				return false;
			}
			if (Main.tile[x, y] == null)
			{
				return false;
			}
			Lighting.AddLight(x, y, v3_1.X, v3_1.Y, v3_1.Z);
			return true;
		}

		public static bool CastLightOpen(int x, int y)
		{
			if (x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
			{
				return false;
			}
			if (Main.tile[x, y] == null)
			{
				return false;
			}
			if (!Main.tile[x, y].active() || Main.tile[x, y].inActive() || Main.tileSolidTop[Main.tile[x, y].type] || !Main.tileSolid[Main.tile[x, y].type])
			{
				Lighting.AddLight(x, y, v3_1.X, v3_1.Y, v3_1.Z);
			}
			return true;
		}

		public static bool CastLightOpen_StopForSolids_ScaleWithDistance(int x, int y)
		{
			if (x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
			{
				return false;
			}
			if (Main.tile[x, y] == null)
			{
				return false;
			}
			if (!Main.tile[x, y].active() || Main.tile[x, y].inActive() || Main.tileSolidTop[Main.tile[x, y].type] || !Main.tileSolid[Main.tile[x, y].type])
			{
				Vector3 vector = v3_1;
				float num = Vector2.Distance(value2: new Vector2(x, y), value1: v2_1);
				vector *= MathHelper.Lerp(0.65f, 1f, num / f_1);
				Lighting.AddLight(x, y, vector.X, vector.Y, vector.Z);
				return true;
			}
			return false;
		}

		public static bool EmitGolfCartDust_StopForSolids(int x, int y)
		{
			if (x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
			{
				return false;
			}
			if (Main.tile[x, y] == null)
			{
				return false;
			}
			if (!Main.tile[x, y].active() || Main.tile[x, y].inActive() || Main.tileSolidTop[Main.tile[x, y].type] || !Main.tileSolid[Main.tile[x, y].type])
			{
				Dust.NewDustPerfect(new Vector2(x * 16 + 8, y * 16 + 8), 260, Vector2.UnitY * -0.2f);
				return true;
			}
			return false;
		}

		public static bool NotDoorStand(int x, int y)
		{
			if (Main.tile[x, y] != null && Main.tile[x, y].active() && Main.tile[x, y].type == 11)
			{
				if (Main.tile[x, y].frameX >= 18)
				{
					return Main.tile[x, y].frameX < 54;
				}
				return false;
			}
			return true;
		}

		public static bool CutTiles(int x, int y)
		{
			if (!WorldGen.InWorld(x, y, 1))
			{
				return false;
			}
			if (Main.tile[x, y] == null)
			{
				return false;
			}
			if (!Main.tileCut[Main.tile[x, y].type])
			{
				return true;
			}
			if (WorldGen.CanCutTile(x, y, tilecut_0))
			{
				WorldGen.KillTile(x, y);
				if (Main.netMode != 0)
				{
					NetMessage.SendData(17, -1, -1, null, 0, x, y);
				}
			}
			return true;
		}

		public static bool SearchAvoidedByNPCs(int x, int y)
		{
			if (!WorldGen.InWorld(x, y, 1))
			{
				return false;
			}
			if (Main.tile[x, y] == null)
			{
				return false;
			}
			if (!Main.tile[x, y].active() || !TileID.Sets.AvoidedByNPCs[Main.tile[x, y].type])
			{
				return true;
			}
			return false;
		}

		public static void RainbowLaserDraw(int stage, Vector2 currentPosition, float distanceLeft, Rectangle lastFrame, out float distCovered, out Rectangle frame, out Vector2 origin, out Color color)
		{
			color = c_1;
			switch (stage)
			{
			case 0:
				distCovered = 33f;
				frame = new Rectangle(0, 0, 26, 22);
				origin = frame.Size() / 2f;
				break;
			case 1:
				frame = new Rectangle(0, 25, 26, 28);
				distCovered = frame.Height;
				origin = new Vector2(frame.Width / 2, 0f);
				break;
			case 2:
				distCovered = 22f;
				frame = new Rectangle(0, 56, 26, 22);
				origin = new Vector2(frame.Width / 2, 1f);
				break;
			default:
				distCovered = 9999f;
				frame = Rectangle.Empty;
				origin = Vector2.Zero;
				color = Color.Transparent;
				break;
			}
		}

		public static void TurretLaserDraw(int stage, Vector2 currentPosition, float distanceLeft, Rectangle lastFrame, out float distCovered, out Rectangle frame, out Vector2 origin, out Color color)
		{
			color = c_1;
			switch (stage)
			{
			case 0:
				distCovered = 32f;
				frame = new Rectangle(0, 0, 22, 20);
				origin = frame.Size() / 2f;
				break;
			case 1:
			{
				i_1++;
				int num = i_1 % 5;
				frame = new Rectangle(0, 22 * (num + 1), 22, 20);
				distCovered = frame.Height - 1;
				origin = new Vector2(frame.Width / 2, 0f);
				break;
			}
			case 2:
				frame = new Rectangle(0, 154, 22, 30);
				distCovered = frame.Height;
				origin = new Vector2(frame.Width / 2, 1f);
				break;
			default:
				distCovered = 9999f;
				frame = Rectangle.Empty;
				origin = Vector2.Zero;
				color = Color.Transparent;
				break;
			}
		}

		public static void LightningLaserDraw(int stage, Vector2 currentPosition, float distanceLeft, Rectangle lastFrame, out float distCovered, out Rectangle frame, out Vector2 origin, out Color color)
		{
			color = c_1 * f_1;
			switch (stage)
			{
			case 0:
				distCovered = 0f;
				frame = new Rectangle(0, 0, 21, 8);
				origin = frame.Size() / 2f;
				break;
			case 1:
				frame = new Rectangle(0, 8, 21, 6);
				distCovered = frame.Height;
				origin = new Vector2(frame.Width / 2, 0f);
				break;
			case 2:
				distCovered = 8f;
				frame = new Rectangle(0, 14, 21, 8);
				origin = new Vector2(frame.Width / 2, 2f);
				break;
			default:
				distCovered = 9999f;
				frame = Rectangle.Empty;
				origin = Vector2.Zero;
				color = Color.Transparent;
				break;
			}
		}

		public static int CompareYReverse(Point a, Point b)
		{
			return b.Y.CompareTo(a.Y);
		}

		public static int CompareDrawSorterByYScale(DrawData a, DrawData b)
		{
			return a.scale.Y.CompareTo(b.scale.Y);
		}
	}
}
