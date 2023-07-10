using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.GameContent;
using GameManager.Graphics;
using GameManager.ID;

namespace GameManager.DataStructures
{
	public static class PlayerDrawHeadLayers
	{
		public static void DrawPlayer_0_(PlayerDrawHeadSet drawinfo)
		{
		}

		public static void DrawPlayer_00_BackHelmet(PlayerDrawHeadSet drawinfo)
		{
			if (drawinfo.drawPlayer.head >= 0 && drawinfo.drawPlayer.head < 266)
			{
				int num = ArmorIDs.Head.Sets.FrontToBackID[drawinfo.drawPlayer.head];
				if (num >= 0)
				{
					Rectangle hairFrame = drawinfo.HairFrame;
					QuickCDD(drawinfo.DrawData, drawinfo.cHead, TextureAssets.ArmorHead[num].Value, drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, hairFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				}
			}
		}

		public static void DrawPlayer_01_FaceSkin(PlayerDrawHeadSet drawinfo)
		{
			if (drawinfo.drawPlayer.head != 38 && drawinfo.drawPlayer.head != 135 && !drawinfo.drawPlayer.isHatRackDoll)
			{
				QuickCDD(drawinfo.DrawData, drawinfo.skinDyePacked, TextureAssets.Players[drawinfo.skinVar, 0].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				QuickCDD(drawinfo.DrawData, TextureAssets.Players[drawinfo.skinVar, 1].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorEyeWhites, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				QuickCDD(drawinfo.DrawData, TextureAssets.Players[drawinfo.skinVar, 2].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorEyes, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				if (drawinfo.drawPlayer.yoraiz0rDarkness)
				{
					QuickCDD(drawinfo.DrawData, drawinfo.skinDyePacked, TextureAssets.Extra[67].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				}
			}
		}

		public static void DrawPlayer_02_DrawArmorWithFullHair(PlayerDrawHeadSet drawinfo)
		{
			if (!drawinfo.fullHair)
			{
				return;
			}
			QuickCDD(drawinfo.DrawData, drawinfo.cHead, TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.HairFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			if (!drawinfo.drawPlayer.invis && !drawinfo.hideHair)
			{
				Rectangle hairFrame = drawinfo.HairFrame;
				hairFrame.Y -= 336;
				if (hairFrame.Y < 0)
				{
					hairFrame.Y = 0;
				}
				QuickCDD(drawinfo.DrawData, drawinfo.hairShaderPacked, TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, hairFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
		}

		public static void DrawPlayer_03_HelmetHair(PlayerDrawHeadSet drawinfo)
		{
			if (!drawinfo.hideHair && drawinfo.hatHair)
			{
				Rectangle hairFrame = drawinfo.HairFrame;
				hairFrame.Y -= 336;
				if (hairFrame.Y < 0)
				{
					hairFrame.Y = 0;
				}
				if (!drawinfo.drawPlayer.invis)
				{
					QuickCDD(drawinfo.DrawData, drawinfo.hairShaderPacked, TextureAssets.PlayerHairAlt[drawinfo.drawPlayer.hair].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, hairFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				}
			}
		}

		public static void DrawPlayer_04_RabbitOrder(PlayerDrawHeadSet drawinfo)
		{
			int verticalFrames = 27;
			Texture2D value = TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value;
			Rectangle rectangle = value.Frame(1, verticalFrames, 0, drawinfo.drawPlayer.rabbitOrderFrame.DisplayFrame);
			Vector2 origin = rectangle.Size() / 2f;
			int usedGravDir = 1;
			Vector2 value2 = DrawPlayer_04_GetHatDrawPosition(drawinfo, new Vector2(1f, -26f), usedGravDir);
			int num = DrawPlayer_04_GetHatStacks(drawinfo, 4955);
			float num2 = (float)Math.PI / 60f;
			float num3 = num2 * drawinfo.drawPlayer.position.X % ((float)Math.PI * 2f);
			for (int num4 = num - 1; num4 >= 0; num4--)
			{
				float x = Vector2.UnitY.RotatedBy(num3 + num2 * (float)num4).X * ((float)num4 / 30f) * 2f - (float)(num4 * 2 * drawinfo.drawPlayer.direction);
				QuickCDD(drawinfo.DrawData, drawinfo.cHead, value, value2 + new Vector2(x, (float)(num4 * -14) * drawinfo.scale), rectangle, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, origin, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
			if (!drawinfo.drawPlayer.invis && !drawinfo.hideHair)
			{
				Rectangle hairFrame = drawinfo.HairFrame;
				hairFrame.Y -= 336;
				if (hairFrame.Y < 0)
				{
					hairFrame.Y = 0;
				}
				QuickCDD(drawinfo.DrawData, drawinfo.hairShaderPacked, TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, hairFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
		}

		public static void DrawPlayer_04_BadgersHat(PlayerDrawHeadSet drawinfo)
		{
			int verticalFrames = 6;
			Texture2D value = TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value;
			Rectangle rectangle = value.Frame(1, verticalFrames, 0, drawinfo.drawPlayer.rabbitOrderFrame.DisplayFrame);
			Vector2 origin = rectangle.Size() / 2f;
			int num = 1;
			Vector2 value2 = DrawPlayer_04_GetHatDrawPosition(drawinfo, new Vector2(0f, -9f), num);
			int num2 = DrawPlayer_04_GetHatStacks(drawinfo, 5004);
			float num3 = (float)Math.PI / 60f;
			float num4 = num3 * drawinfo.drawPlayer.position.X % ((float)Math.PI * 2f);
			int num5 = num2 * 4 + 2;
			int num6 = 0;
			bool flag = (Main.GlobalTimeWrappedHourly + 180f) % 3600f < 60f;
			for (int num7 = num5 - 1; num7 >= 0; num7--)
			{
				int num8 = 0;
				if (num7 == num5 - 1)
				{
					rectangle.Y = 0;
					num8 = 2;
				}
				else if (num7 == 0)
				{
					rectangle.Y = rectangle.Height * 5;
				}
				else
				{
					rectangle.Y = rectangle.Height * (num6++ % 4 + 1);
				}
				if (!(rectangle.Y == rectangle.Height * 3 && flag))
				{
					float x = Vector2.UnitY.RotatedBy(num4 + num3 * (float)num7).X * ((float)num7 / 10f) * 4f - (float)num7 * 0.1f * (float)drawinfo.drawPlayer.direction;
					QuickCDD(drawinfo.DrawData, drawinfo.cHead, value, value2 + new Vector2(x, (num7 * -4 + num8) * num) * drawinfo.scale, rectangle, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, origin, drawinfo.scale, drawinfo.playerEffect, 0f);
				}
			}
		}

		private static Vector2 DrawPlayer_04_GetHatDrawPosition(PlayerDrawHeadSet drawinfo, Vector2 hatOffset, int usedGravDir)
		{
			Vector2 value = new Vector2(drawinfo.drawPlayer.direction, usedGravDir);
			return drawinfo.Position - Main.screenPosition + new Vector2(-drawinfo.bodyFrameMemory.Width / 2 + drawinfo.drawPlayer.width / 2, drawinfo.drawPlayer.height - drawinfo.bodyFrameMemory.Height + 4) + hatOffset * value * drawinfo.scale + (drawinfo.drawPlayer.headPosition + drawinfo.headVect);
		}

		private static int DrawPlayer_04_GetHatStacks(PlayerDrawHeadSet drawinfo, int itemId)
		{
			int num = 0;
			int num2 = 0;
			if (drawinfo.drawPlayer.armor[num2] != null && drawinfo.drawPlayer.armor[num2].type == itemId && drawinfo.drawPlayer.armor[num2].stack > 0)
			{
				num += drawinfo.drawPlayer.armor[num2].stack;
			}
			num2 = 10;
			if (drawinfo.drawPlayer.armor[num2] != null && drawinfo.drawPlayer.armor[num2].type == itemId && drawinfo.drawPlayer.armor[num2].stack > 0)
			{
				num += drawinfo.drawPlayer.armor[num2].stack;
			}
			return num;
		}

		public static void DrawPlayer_04_JungleRose(PlayerDrawHeadSet drawinfo)
		{
			if (drawinfo.drawPlayer.head == 259)
			{
				DrawPlayer_04_RabbitOrder(drawinfo);
			}
			else
			{
				if (!drawinfo.helmetIsOverFullHair)
				{
					return;
				}
				if (!drawinfo.drawPlayer.invis && !drawinfo.hideHair)
				{
					Rectangle hairFrame = drawinfo.HairFrame;
					hairFrame.Y -= 336;
					if (hairFrame.Y < 0)
					{
						hairFrame.Y = 0;
					}
					QuickCDD(drawinfo.DrawData, drawinfo.hairShaderPacked, TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, hairFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				}
				if (drawinfo.drawPlayer.head != 0)
				{
					QuickCDD(drawinfo.DrawData, drawinfo.cHead, TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				}
			}
		}

		public static void DrawPlayer_05_TallHats(PlayerDrawHeadSet drawinfo)
		{
			if (drawinfo.helmetIsTall)
			{
				Rectangle hairFrame = drawinfo.HairFrame;
				if (drawinfo.drawPlayer.head == 158)
				{
					hairFrame.Height -= 2;
				}
				int num = 0;
				if (hairFrame.Y == hairFrame.Height * 6)
				{
					hairFrame.Height -= 2;
				}
				else if (hairFrame.Y == hairFrame.Height * 7)
				{
					num = -2;
				}
				else if (hairFrame.Y == hairFrame.Height * 8)
				{
					num = -2;
				}
				else if (hairFrame.Y == hairFrame.Height * 9)
				{
					num = -2;
				}
				else if (hairFrame.Y == hairFrame.Height * 10)
				{
					num = -2;
				}
				else if (hairFrame.Y == hairFrame.Height * 13)
				{
					hairFrame.Height -= 2;
				}
				else if (hairFrame.Y == hairFrame.Height * 14)
				{
					num = -2;
				}
				else if (hairFrame.Y == hairFrame.Height * 15)
				{
					num = -2;
				}
				else if (hairFrame.Y == hairFrame.Height * 16)
				{
					num = -2;
				}
				hairFrame.Y += num;
				QuickCDD(drawinfo.DrawData, drawinfo.cHead, TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f + (float)num) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, hairFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
		}

		public static void DrawPlayer_06_NormalHats(PlayerDrawHeadSet drawinfo)
		{
			if (drawinfo.drawPlayer.head == 265)
			{
				DrawPlayer_04_BadgersHat(drawinfo);
			}
			else if (drawinfo.helmetIsNormal)
			{
				Rectangle hairFrame = drawinfo.HairFrame;
				QuickCDD(drawinfo.DrawData, drawinfo.cHead, TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, hairFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
		}

		public static void DrawPlayer_07_JustHair(PlayerDrawHeadSet drawinfo)
		{
			if (!drawinfo.helmetIsNormal && !drawinfo.helmetIsOverFullHair && !drawinfo.helmetIsTall && !drawinfo.hideHair)
			{
				if (drawinfo.drawPlayer.face == 5)
				{
					QuickCDD(drawinfo.DrawData, drawinfo.cFace, TextureAssets.AccFace[drawinfo.drawPlayer.face].Value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				}
				Rectangle hairFrame = drawinfo.HairFrame;
				hairFrame.Y -= 336;
				if (hairFrame.Y < 0)
				{
					hairFrame.Y = 0;
				}
				QuickCDD(drawinfo.DrawData, drawinfo.hairShaderPacked, TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, hairFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
		}

		public static void DrawPlayer_08_FaceAcc(PlayerDrawHeadSet drawinfo)
		{
			if (drawinfo.drawPlayer.face > 0 && drawinfo.drawPlayer.face < 16 && drawinfo.drawPlayer.face != 5)
			{
				if (drawinfo.drawPlayer.face == 7)
				{
					new Color(200, 200, 200, 150);
					QuickCDD(drawinfo.DrawData, drawinfo.cFace, TextureAssets.AccFace[drawinfo.drawPlayer.face].Value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, new Color(200, 200, 200, 200), drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				}
				else
				{
					QuickCDD(drawinfo.DrawData, drawinfo.cFace, TextureAssets.AccFace[drawinfo.drawPlayer.face].Value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				}
			}
			if (drawinfo.drawUnicornHorn)
			{
				QuickCDD(drawinfo.DrawData, drawinfo.cUnicornHorn, TextureAssets.Extra[143].Value, new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
		}

		public static void DrawPlayer_RenderAllLayers(PlayerDrawHeadSet drawinfo)
		{
			List<DrawData> drawData = drawinfo.DrawData;
			Effect pixelShader = Main.pixelShader;
			_ = Main.projectile;
			SpriteBatch spriteBatch = Main.spriteBatch;
			for (int i = 0; i < drawData.Count; i++)
			{
				DrawData cdd = drawData[i];
				if (!cdd.sourceRect.HasValue)
				{
					cdd.sourceRect = cdd.texture.Frame();
				}
				PlayerDrawHelper.SetShaderForData(drawinfo.drawPlayer, drawinfo.cHead, cdd);
				if (cdd.texture != null)
				{
					cdd.Draw(spriteBatch);
				}
			}
			pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		public static void DrawPlayer_DrawSelectionRect(PlayerDrawHeadSet drawinfo)
		{
			SpriteRenderTargetHelper.GetDrawBoundary(drawinfo.DrawData, out var lowest, out var highest);
			Utils.DrawRect(Main.spriteBatch, lowest + Main.screenPosition, highest + Main.screenPosition, Color.White);
		}

		public static void QuickCDD(List<DrawData> drawData, Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
		{
			drawData.Add(new DrawData(texture, position, sourceRectangle, color, rotation, origin, scale, effects, 0));
		}

		public static void QuickCDD(List<DrawData> drawData, int shaderTechnique, Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
		{
			DrawData item = new DrawData(texture, position, sourceRectangle, color, rotation, origin, scale, effects, 0);
			item.shader = shaderTechnique;
			drawData.Add(item);
		}
	}
}
