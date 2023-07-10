using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.DataStructures;
using GameManager.GameContent;
using GameManager.ID;

namespace GameManager.Graphics.Renderers
{
	public class LegacyPlayerRenderer : IPlayerRenderer
	{
		private readonly List<DrawData> _drawData = new List<DrawData>();

		private readonly List<int> _dust = new List<int>();

		private readonly List<int> _gore = new List<int>();

		public static SamplerState MountedSamplerState
		{
			get
			{
				if (!Main.drawToScreen)
				{
					return SamplerState.AnisotropicClamp;
				}
				return SamplerState.LinearClamp;
			}
		}

		public void DrawPlayers(Camera camera, IEnumerable<Player> players)
		{
			foreach (Player player in players)
			{
				DrawPlayerFull(camera, player);
			}
		}

		public void DrawPlayerHead(Camera camera, Player drawPlayer, Vector2 position, float alpha = 1f, float scale = 1f, Color borderColor = default(Color))
		{
			_drawData.Clear();
			_dust.Clear();
			_gore.Clear();
			PlayerDrawHeadSet drawinfo = default(PlayerDrawHeadSet);
			drawinfo.BoringSetup(drawPlayer, _drawData, _dust, _gore, position.X, position.Y, alpha, scale);
			PlayerDrawHeadLayers.DrawPlayer_00_BackHelmet(drawinfo);
			PlayerDrawHeadLayers.DrawPlayer_01_FaceSkin(drawinfo);
			PlayerDrawHeadLayers.DrawPlayer_02_DrawArmorWithFullHair(drawinfo);
			PlayerDrawHeadLayers.DrawPlayer_03_HelmetHair(drawinfo);
			PlayerDrawHeadLayers.DrawPlayer_04_JungleRose(drawinfo);
			PlayerDrawHeadLayers.DrawPlayer_05_TallHats(drawinfo);
			PlayerDrawHeadLayers.DrawPlayer_06_NormalHats(drawinfo);
			PlayerDrawHeadLayers.DrawPlayer_07_JustHair(drawinfo);
			PlayerDrawHeadLayers.DrawPlayer_08_FaceAcc(drawinfo);
			CreateOutlines(alpha, scale, borderColor);
			PlayerDrawHeadLayers.DrawPlayer_RenderAllLayers(drawinfo);
		}

		private void CreateOutlines(float alpha, float scale, Color borderColor)
		{
			if (!(borderColor != Color.Transparent))
			{
				return;
			}
			List<DrawData> collection = new List<DrawData>(_drawData);
			List<DrawData> list = new List<DrawData>(_drawData);
			float num = 2f * scale;
			Color color = borderColor;
			color *= alpha * alpha;
			Color black = Color.Black;
			black *= alpha * alpha;
			int colorOnlyShaderIndex = ContentSamples.CommonlyUsedContentSamples.ColorOnlyShaderIndex;
			for (int i = 0; i < list.Count; i++)
			{
				DrawData value = list[i];
				value.shader = colorOnlyShaderIndex;
				value.color = black;
				list[i] = value;
			}
			int num2 = 2;
			Vector2 vector;
			for (int j = -num2; j <= num2; j++)
			{
				for (int k = -num2; k <= num2; k++)
				{
					if (Math.Abs(j) + Math.Abs(k) == num2)
					{
						vector = new Vector2((float)j * num, (float)k * num);
						for (int l = 0; l < list.Count; l++)
						{
							DrawData item = list[l];
							item.position += vector;
							_drawData.Add(item);
						}
					}
				}
			}
			for (int m = 0; m < list.Count; m++)
			{
				DrawData value2 = list[m];
				value2.shader = colorOnlyShaderIndex;
				value2.color = color;
				list[m] = value2;
			}
			vector = Vector2.Zero;
			num2 = 1;
			for (int n = -num2; n <= num2; n++)
			{
				for (int num3 = -num2; num3 <= num2; num3++)
				{
					if (Math.Abs(n) + Math.Abs(num3) == num2)
					{
						vector = new Vector2((float)n * num, (float)num3 * num);
						for (int num4 = 0; num4 < list.Count; num4++)
						{
							DrawData item2 = list[num4];
							item2.position += vector;
							_drawData.Add(item2);
						}
					}
				}
			}
			_drawData.AddRange(collection);
		}

		public void DrawPlayer(Camera camera, Player drawPlayer, Vector2 position, float rotation, Vector2 rotationOrigin, float shadow = 0f, float scale = 1f)
		{
			if (drawPlayer.ShouldNotDraw)
			{
				return;
			}
			PlayerDrawSet drawinfo = default(PlayerDrawSet);
			_drawData.Clear();
			_dust.Clear();
			_gore.Clear();
			drawinfo.BoringSetup(drawPlayer, _drawData, _dust, _gore, position, shadow, rotation, rotationOrigin);
			PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(drawinfo);
			PlayerDrawLayers.DrawPlayer_01_BackHair(drawinfo);
			PlayerDrawLayers.DrawPlayer_01_2_JimsCloak(drawinfo);
			PlayerDrawLayers.DrawPlayer_01_3_BackHead(drawinfo);
			PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(drawinfo);
			PlayerDrawLayers.DrawPlayer_02_MountBehindPlayer(drawinfo);
			PlayerDrawLayers.DrawPlayer_03_Carpet(drawinfo);
			PlayerDrawLayers.DrawPlayer_03_PortableStool(drawinfo);
			PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(drawinfo);
			PlayerDrawLayers.DrawPlayer_04_ElectrifiedDebuffBack(drawinfo);
			PlayerDrawLayers.DrawPlayer_05_ForbiddenSetRing(drawinfo);
			PlayerDrawLayers.DrawPlayer_05_2_SafemanSun(drawinfo);
			PlayerDrawLayers.DrawPlayer_06_WebbedDebuffBack(drawinfo);
			PlayerDrawLayers.DrawPlayer_07_LeinforsHairShampoo(drawinfo);
			PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(drawinfo);
			PlayerDrawLayers.DrawPlayer_08_Backpacks(drawinfo);
			PlayerDrawLayers.DrawPlayer_09_BackAc(drawinfo);
			PlayerDrawLayers.DrawPlayer_10_Wings(drawinfo);
			PlayerDrawLayers.DrawPlayer_11_Balloons(drawinfo);
			if (drawinfo.weaponDrawOrder == WeaponDrawOrder.BehindBackArm)
			{
				PlayerDrawLayers.DrawPlayer_27_HeldItem(drawinfo);
			}
			PlayerDrawLayers.DrawPlayer_12_Skin(drawinfo);
			if (drawinfo.drawPlayer.wearsRobe && drawinfo.drawPlayer.body != 166)
			{
				PlayerDrawLayers.DrawPlayer_14_Shoes(drawinfo);
				PlayerDrawLayers.DrawPlayer_13_Leggings(drawinfo);
			}
			else
			{
				PlayerDrawLayers.DrawPlayer_13_Leggings(drawinfo);
				PlayerDrawLayers.DrawPlayer_14_Shoes(drawinfo);
			}
			PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(drawinfo);
			PlayerDrawLayers.DrawPlayer_15_SkinLongCoat(drawinfo);
			PlayerDrawLayers.DrawPlayer_16_ArmorLongCoat(drawinfo);
			PlayerDrawLayers.DrawPlayer_17_Torso(drawinfo);
			PlayerDrawLayers.DrawPlayer_18_OffhandAcc(drawinfo);
			PlayerDrawLayers.DrawPlayer_19_WaistAcc(drawinfo);
			PlayerDrawLayers.DrawPlayer_20_NeckAcc(drawinfo);
			PlayerDrawLayers.DrawPlayer_21_Head(drawinfo);
			PlayerDrawLayers.DrawPlayer_22_FaceAcc(drawinfo);
			if (drawinfo.drawFrontAccInNeckAccLayer)
			{
				PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(drawinfo);
				PlayerDrawLayers.DrawPlayer_32_FrontAcc(drawinfo);
				PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(drawinfo);
			}
			PlayerDrawLayers.DrawPlayer_23_MountFront(drawinfo);
			PlayerDrawLayers.DrawPlayer_24_Pulley(drawinfo);
			PlayerDrawLayers.DrawPlayer_25_Shield(drawinfo);
			PlayerDrawLayers.DrawPlayer_extra_MountPlus(drawinfo);
			PlayerDrawLayers.DrawPlayer_26_SolarShield(drawinfo);
			PlayerDrawLayers.DrawPlayer_extra_MountMinus(drawinfo);
			if (drawinfo.weaponDrawOrder == WeaponDrawOrder.BehindFrontArm)
			{
				PlayerDrawLayers.DrawPlayer_27_HeldItem(drawinfo);
			}
			PlayerDrawLayers.DrawPlayer_28_ArmOverItem(drawinfo);
			PlayerDrawLayers.DrawPlayer_29_OnhandAcc(drawinfo);
			PlayerDrawLayers.DrawPlayer_30_BladedGlove(drawinfo);
			PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(drawinfo);
			if (!drawinfo.drawFrontAccInNeckAccLayer)
			{
				PlayerDrawLayers.DrawPlayer_32_FrontAcc(drawinfo);
			}
			if (drawinfo.weaponDrawOrder == WeaponDrawOrder.OverFrontArm)
			{
				PlayerDrawLayers.DrawPlayer_27_HeldItem(drawinfo);
			}
			PlayerDrawLayers.DrawPlayer_31_ProjectileOverArm(drawinfo);
			PlayerDrawLayers.DrawPlayer_33_FrozenOrWebbedDebuff(drawinfo);
			PlayerDrawLayers.DrawPlayer_34_ElectrifiedDebuffFront(drawinfo);
			PlayerDrawLayers.DrawPlayer_35_IceBarrier(drawinfo);
			PlayerDrawLayers.DrawPlayer_36_CTG(drawinfo);
			PlayerDrawLayers.DrawPlayer_37_BeetleBuff(drawinfo);
			PlayerDrawLayers.DrawPlayer_MakeIntoFirstFractalAfterImage(drawinfo);
			PlayerDrawLayers.DrawPlayer_TransformDrawData(drawinfo);
			if (scale != 1f)
			{
				PlayerDrawLayers.DrawPlayer_ScaleDrawData(drawinfo, scale);
			}
			PlayerDrawLayers.DrawPlayer_RenderAllLayers(drawinfo);
			if (!drawinfo.drawPlayer.mount.Active || drawinfo.drawPlayer.mount.Type != 11)
			{
				return;
			}
			for (int i = 0; i < 1000; i++)
			{
				if (Main.projectile[i].active && Main.projectile[i].owner == drawinfo.drawPlayer.whoAmI && Main.projectile[i].type == 591)
				{
					Main.instance.DrawProj(i);
				}
			}
		}

		private void DrawPlayerFull(Camera camera, Player drawPlayer)
		{
			SpriteBatch spriteBatch = camera.SpriteBatch;
			SamplerState samplerState = camera.Sampler;
			if (drawPlayer.mount.Active && drawPlayer.fullRotation != 0f)
			{
				samplerState = MountedSamplerState;
			}
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, samplerState, DepthStencilState.None, camera.Rasterizer, null, camera.GameViewMatrix.TransformationMatrix);
			if (Main.gamePaused)
			{
				drawPlayer.PlayerFrame();
			}
			if (drawPlayer.ghost)
			{
				for (int i = 0; i < 3; i++)
				{
					Vector2 vector = drawPlayer.shadowPos[i];
					vector = drawPlayer.position - drawPlayer.velocity * (2 + i * 2);
					DrawGhost(camera, drawPlayer, vector, 0.5f + 0.2f * (float)i);
				}
				DrawGhost(camera, drawPlayer, drawPlayer.position);
			}
			else
			{
				if (drawPlayer.inventory[drawPlayer.selectedItem].flame || drawPlayer.head == 137 || drawPlayer.wings == 22)
				{
					drawPlayer.itemFlameCount--;
					if (drawPlayer.itemFlameCount <= 0)
					{
						drawPlayer.itemFlameCount = 5;
						for (int j = 0; j < 7; j++)
						{
							drawPlayer.itemFlamePos[j].X = (float)Main.rand.Next(-10, 11) * 0.15f;
							drawPlayer.itemFlamePos[j].Y = (float)Main.rand.Next(-10, 1) * 0.35f;
						}
					}
				}
				if (drawPlayer.armorEffectDrawShadowEOCShield)
				{
					int num = drawPlayer.eocDash / 4;
					if (num > 3)
					{
						num = 3;
					}
					for (int k = 0; k < num; k++)
					{
						DrawPlayer(camera, drawPlayer, drawPlayer.shadowPos[k], drawPlayer.shadowRotation[k], drawPlayer.shadowOrigin[k], 0.5f + 0.2f * (float)k);
					}
				}
				Vector2 position = default(Vector2);
				if (drawPlayer.invis)
				{
					drawPlayer.armorEffectDrawOutlines = false;
					drawPlayer.armorEffectDrawShadow = false;
					drawPlayer.armorEffectDrawShadowSubtle = false;
					position = drawPlayer.position;
					if (drawPlayer.aggro <= -750)
					{
						DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, 1f);
					}
					else
					{
						drawPlayer.invis = false;
						DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin);
						drawPlayer.invis = true;
					}
				}
				if (drawPlayer.armorEffectDrawOutlines)
				{
					_ = drawPlayer.position;
					if (!Main.gamePaused)
					{
						drawPlayer.ghostFade += drawPlayer.ghostDir * 0.075f;
					}
					if ((double)drawPlayer.ghostFade < 0.1)
					{
						drawPlayer.ghostDir = 1f;
						drawPlayer.ghostFade = 0.1f;
					}
					else if ((double)drawPlayer.ghostFade > 0.9)
					{
						drawPlayer.ghostDir = -1f;
						drawPlayer.ghostFade = 0.9f;
					}
					float num2 = drawPlayer.ghostFade * 5f;
					for (int l = 0; l < 4; l++)
					{
						float num3;
						float num4;
						switch (l)
						{
						default:
							num3 = num2;
							num4 = 0f;
							break;
						case 1:
							num3 = 0f - num2;
							num4 = 0f;
							break;
						case 2:
							num3 = 0f;
							num4 = num2;
							break;
						case 3:
							num3 = 0f;
							num4 = 0f - num2;
							break;
						}
						position = new Vector2(drawPlayer.position.X + num3, drawPlayer.position.Y + drawPlayer.gfxOffY + num4);
						DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, drawPlayer.ghostFade);
					}
				}
				if (drawPlayer.armorEffectDrawOutlinesForbidden)
				{
					_ = drawPlayer.position;
					if (!Main.gamePaused)
					{
						drawPlayer.ghostFade += drawPlayer.ghostDir * 0.025f;
					}
					if ((double)drawPlayer.ghostFade < 0.1)
					{
						drawPlayer.ghostDir = 1f;
						drawPlayer.ghostFade = 0.1f;
					}
					else if ((double)drawPlayer.ghostFade > 0.9)
					{
						drawPlayer.ghostDir = -1f;
						drawPlayer.ghostFade = 0.9f;
					}
					float num5 = drawPlayer.ghostFade * 5f;
					for (int m = 0; m < 4; m++)
					{
						float num6;
						float num7;
						switch (m)
						{
						default:
							num6 = num5;
							num7 = 0f;
							break;
						case 1:
							num6 = 0f - num5;
							num7 = 0f;
							break;
						case 2:
							num6 = 0f;
							num7 = num5;
							break;
						case 3:
							num6 = 0f;
							num7 = 0f - num5;
							break;
						}
						position = new Vector2(drawPlayer.position.X + num6, drawPlayer.position.Y + drawPlayer.gfxOffY + num7);
						DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, drawPlayer.ghostFade);
					}
				}
				if (drawPlayer.armorEffectDrawShadowBasilisk)
				{
					int num8 = (int)(drawPlayer.basiliskCharge * 3f);
					for (int n = 0; n < num8; n++)
					{
						DrawPlayer(camera, drawPlayer, drawPlayer.shadowPos[n], drawPlayer.shadowRotation[n], drawPlayer.shadowOrigin[n], 0.5f + 0.2f * (float)n);
					}
				}
				else if (drawPlayer.armorEffectDrawShadow)
				{
					for (int num9 = 0; num9 < 3; num9++)
					{
						DrawPlayer(camera, drawPlayer, drawPlayer.shadowPos[num9], drawPlayer.shadowRotation[num9], drawPlayer.shadowOrigin[num9], 0.5f + 0.2f * (float)num9);
					}
				}
				if (drawPlayer.armorEffectDrawShadowLokis)
				{
					for (int num10 = 0; num10 < 3; num10++)
					{
						DrawPlayer(camera, drawPlayer, Vector2.Lerp(drawPlayer.shadowPos[num10], drawPlayer.position + new Vector2(0f, drawPlayer.gfxOffY), 0.5f), drawPlayer.shadowRotation[num10], drawPlayer.shadowOrigin[num10], MathHelper.Lerp(1f, 0.5f + 0.2f * (float)num10, 0.5f));
					}
				}
				if (drawPlayer.armorEffectDrawShadowSubtle)
				{
					for (int num11 = 0; num11 < 4; num11++)
					{
						position.X = drawPlayer.position.X + (float)Main.rand.Next(-20, 21) * 0.1f;
						position.Y = drawPlayer.position.Y + (float)Main.rand.Next(-20, 21) * 0.1f + drawPlayer.gfxOffY;
						DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, 0.9f);
					}
				}
				if (drawPlayer.shadowDodge)
				{
					drawPlayer.shadowDodgeCount += 1f;
					if (drawPlayer.shadowDodgeCount > 30f)
					{
						drawPlayer.shadowDodgeCount = 30f;
					}
				}
				else
				{
					drawPlayer.shadowDodgeCount -= 1f;
					if (drawPlayer.shadowDodgeCount < 0f)
					{
						drawPlayer.shadowDodgeCount = 0f;
					}
				}
				if (drawPlayer.shadowDodgeCount > 0f)
				{
					_ = drawPlayer.position;
					position.X = drawPlayer.position.X + drawPlayer.shadowDodgeCount;
					position.Y = drawPlayer.position.Y + drawPlayer.gfxOffY;
					DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, 0.5f + (float)Main.rand.Next(-10, 11) * 0.005f);
					position.X = drawPlayer.position.X - drawPlayer.shadowDodgeCount;
					DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, 0.5f + (float)Main.rand.Next(-10, 11) * 0.005f);
				}
				if (drawPlayer.brainOfConfusionDodgeAnimationCounter > 0)
				{
					Vector2 value = drawPlayer.position + new Vector2(0f, drawPlayer.gfxOffY);
					float lerpValue = Utils.GetLerpValue(300f, 270f, drawPlayer.brainOfConfusionDodgeAnimationCounter);
					float y = MathHelper.Lerp(2f, 120f, lerpValue);
					if (lerpValue >= 0f && lerpValue <= 1f)
					{
						for (float num12 = 0f; num12 < (float)Math.PI * 2f; num12 += (float)Math.PI / 3f)
						{
							position = value + new Vector2(0f, y).RotatedBy((float)Math.PI * 2f * lerpValue * 0.5f + num12);
							DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, lerpValue);
						}
					}
				}
				position = drawPlayer.position;
				position.Y += drawPlayer.gfxOffY;
				if (drawPlayer.stoned)
				{
					DrawPlayerStoned(camera, drawPlayer, position);
				}
				else if (!drawPlayer.invis)
				{
					DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin);
				}
			}
			spriteBatch.End();
		}

		private void DrawPlayerStoned(Camera camera, Player drawPlayer, Vector2 position)
		{
			if (!drawPlayer.dead)
			{
				SpriteEffects spriteEffects = SpriteEffects.None;
				spriteEffects = ((drawPlayer.direction != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				camera.SpriteBatch.Draw(TextureAssets.Extra[37].Value, new Vector2((int)(position.X - camera.UnscaledPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2)), (int)(position.Y - camera.UnscaledPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 8f)) + drawPlayer.bodyPosition + new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2), null, Lighting.GetColor((int)((double)position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)position.Y + (double)drawPlayer.height * 0.5) / 16, Color.White), 0f, new Vector2(TextureAssets.Extra[37].Width() / 2, TextureAssets.Extra[37].Height() / 2), 1f, spriteEffects, 0f);
			}
		}

		private void DrawGhost(Camera camera, Player drawPlayer, Vector2 position, float shadow = 0f)
		{
			byte mouseTextColor = Main.mouseTextColor;
			SpriteEffects effects = ((drawPlayer.direction != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			Color immuneAlpha = drawPlayer.GetImmuneAlpha(Lighting.GetColor((int)((double)drawPlayer.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawPlayer.position.Y + (double)drawPlayer.height * 0.5) / 16, new Color((int)mouseTextColor / 2 + 100, (int)mouseTextColor / 2 + 100, (int)mouseTextColor / 2 + 100, (int)mouseTextColor / 2 + 100)), shadow);
			immuneAlpha.A = (byte)((float)(int)immuneAlpha.A * (1f - Math.Max(0.5f, shadow - 0.5f)));
			Rectangle value = new Rectangle(0, TextureAssets.Ghost.Height() / 4 * drawPlayer.ghostFrame, TextureAssets.Ghost.Width(), TextureAssets.Ghost.Height() / 4);
			Vector2 origin = new Vector2((float)value.Width * 0.5f, (float)value.Height * 0.5f);
			camera.SpriteBatch.Draw(TextureAssets.Ghost.Value, new Vector2((int)(position.X - camera.UnscaledPosition.X + (float)(value.Width / 2)), (int)(position.Y - camera.UnscaledPosition.Y + (float)(value.Height / 2))), value, immuneAlpha, 0f, origin, 1f, effects, 0f);
		}
	}
}
