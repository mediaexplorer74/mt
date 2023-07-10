using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.GameContent.Golf;
using GameManager.Graphics.Shaders;
using GameManager.ID;

namespace GameManager.DataStructures
{
	public struct PlayerDrawSet
	{
		public List<DrawData> DrawDataCache;

		public List<int> DustCache;

		public List<int> GoreCache;

		public Player drawPlayer;

		public float shadow;

		public Vector2 Position;

		public int projectileDrawPosition;

		public Vector2 ItemLocation;

		public int armorAdjust;

		public bool missingHand;

		public bool missingArm;

		public bool heldProjOverHand;

		public int skinVar;

		public bool fullHair;

		public bool drawsBackHairWithoutHeadgear;

		public bool hatHair;

		public bool hideHair;

		public int hairDyePacked;

		public int skinDyePacked;

		public float mountOffSet;

		public int cHead;

		public int cBody;

		public int cLegs;

		public int cHandOn;

		public int cHandOff;

		public int cBack;

		public int cFront;

		public int cShoe;

		public int cWaist;

		public int cShield;

		public int cNeck;

		public int cFace;

		public int cBalloon;

		public int cWings;

		public int cCarpet;

		public int cPortableStool;

		public int cFloatingTube;

		public int cUnicornHorn;

		public int cLeinShampoo;

		public SpriteEffects playerEffect;

		public SpriteEffects itemEffect;

		public Color colorHair;

		public Color colorEyeWhites;

		public Color colorEyes;

		public Color colorHead;

		public Color colorBodySkin;

		public Color colorLegs;

		public Color colorShirt;

		public Color colorUnderShirt;

		public Color colorPants;

		public Color colorShoes;

		public Color colorArmorHead;

		public Color colorArmorBody;

		public Color colorMount;

		public Color colorArmorLegs;

		public Color colorElectricity;

		public int headGlowMask;

		public int bodyGlowMask;

		public int armGlowMask;

		public int legsGlowMask;

		public Color headGlowColor;

		public Color bodyGlowColor;

		public Color armGlowColor;

		public Color legsGlowColor;

		public Color ArkhalisColor;

		public float stealth;

		public Vector2 legVect;

		public Vector2 bodyVect;

		public Vector2 headVect;

		public Color selectionGlowColor;

		public float torsoOffset;

		public bool hidesTopSkin;

		public bool hidesBottomSkin;

		public float rotation;

		public Vector2 rotationOrigin;

		public Rectangle hairFrame;

		public bool backHairDraw;

		public bool backPack;

		public Color itemColor;

		public bool usesCompositeTorso;

		public bool usesCompositeFrontHandAcc;

		public bool usesCompositeBackHandAcc;

		public bool compShoulderOverFrontArm;

		public Rectangle compBackShoulderFrame;

		public Rectangle compFrontShoulderFrame;

		public Rectangle compBackArmFrame;

		public Rectangle compFrontArmFrame;

		public Rectangle compTorsoFrame;

		public float compositeBackArmRotation;

		public float compositeFrontArmRotation;

		public bool hideCompositeShoulders;

		public Vector2 frontShoulderOffset;

		public Vector2 backShoulderOffset;

		public WeaponDrawOrder weaponDrawOrder;

		public bool weaponOverFrontArm;

		public bool isSitting;

		public bool isSleeping;

		public float seatYOffset;

		public int sittingIndex;

		public bool drawFrontAccInNeckAccLayer;

		public Item heldItem;

		public bool drawFloatingTube;

		public bool drawUnicornHorn;

		public Color floatingTubeColor;

		public Vector2 helmetOffset;

		public Vector2 Center => new Vector2(Position.X + (float)(drawPlayer.width / 2), Position.Y + (float)(drawPlayer.height / 2));

		public void BoringSetup(Player player, List<DrawData> drawData, List<int> dust, List<int> gore, Vector2 drawPosition, float shadowOpacity, float rotation, Vector2 rotationOrigin)
		{
			DrawDataCache = drawData;
			DustCache = dust;
			GoreCache = gore;
			drawPlayer = player;
			shadow = shadowOpacity;
			this.rotation = rotation;
			this.rotationOrigin = rotationOrigin;
			heldItem = player.lastVisualizedSelectedItem;
			cHead = drawPlayer.cHead;
			cBody = drawPlayer.cBody;
			cLegs = drawPlayer.cLegs;
			if (drawPlayer.wearsRobe)
			{
				cLegs = cBody;
			}
			cHandOn = drawPlayer.cHandOn;
			cHandOff = drawPlayer.cHandOff;
			cBack = drawPlayer.cBack;
			cFront = drawPlayer.cFront;
			cShoe = drawPlayer.cShoe;
			cWaist = drawPlayer.cWaist;
			cShield = drawPlayer.cShield;
			cNeck = drawPlayer.cNeck;
			cFace = drawPlayer.cFace;
			cBalloon = drawPlayer.cBalloon;
			cWings = drawPlayer.cWings;
			cCarpet = drawPlayer.cCarpet;
			cPortableStool = drawPlayer.cPortalbeStool;
			cFloatingTube = drawPlayer.cFloatingTube;
			cUnicornHorn = drawPlayer.cUnicornHorn;
			cLeinShampoo = drawPlayer.cLeinShampoo;
			isSitting = drawPlayer.sitting.isSitting;
			seatYOffset = 0f;
			sittingIndex = 0;
			Vector2 posOffset = Vector2.Zero;
			drawPlayer.sitting.GetSittingOffsetInfo(drawPlayer, out posOffset, out seatYOffset);
			if (isSitting)
			{
				sittingIndex = drawPlayer.sitting.sittingIndex;
			}
			if (drawPlayer.mount.Active && drawPlayer.mount.Type == 17)
			{
				isSitting = true;
			}
			if (drawPlayer.mount.Active && drawPlayer.mount.Type == 23)
			{
				isSitting = true;
			}
			if (drawPlayer.mount.Active && drawPlayer.mount.Type == 45)
			{
				isSitting = true;
			}
			isSleeping = drawPlayer.sleeping.isSleeping;
			Position = drawPosition;
			if (isSitting)
			{
				torsoOffset = seatYOffset;
				Position += posOffset;
			}
			else
			{
				sittingIndex = -1;
			}
			if (isSleeping)
			{
				this.rotationOrigin = player.Size / 2f;
				drawPlayer.sleeping.GetSleepingOffsetInfo(drawPlayer, out var posOffset2);
				Position += posOffset2;
			}
			weaponDrawOrder = WeaponDrawOrder.BehindFrontArm;
			if (heldItem.type == 4952)
			{
				weaponDrawOrder = WeaponDrawOrder.BehindBackArm;
			}
			if (GolfHelper.IsPlayerHoldingClub(player) && player.itemAnimation > player.itemAnimationMax)
			{
				weaponDrawOrder = WeaponDrawOrder.OverFrontArm;
			}
			projectileDrawPosition = -1;
			ItemLocation = Position + (drawPlayer.itemLocation - drawPlayer.position);
			armorAdjust = 0;
			missingHand = false;
			missingArm = false;
			heldProjOverHand = false;
			skinVar = drawPlayer.skinVariant;
			if (drawPlayer.body == 77 || drawPlayer.body == 103 || drawPlayer.body == 41 || drawPlayer.body == 100 || drawPlayer.body == 10 || drawPlayer.body == 11 || drawPlayer.body == 12 || drawPlayer.body == 13 || drawPlayer.body == 14 || drawPlayer.body == 43 || drawPlayer.body == 15 || drawPlayer.body == 16 || drawPlayer.body == 20 || drawPlayer.body == 39 || drawPlayer.body == 50 || drawPlayer.body == 38 || drawPlayer.body == 40 || drawPlayer.body == 57 || drawPlayer.body == 44 || drawPlayer.body == 52 || drawPlayer.body == 53 || drawPlayer.body == 68 || drawPlayer.body == 81 || drawPlayer.body == 85 || drawPlayer.body == 88 || drawPlayer.body == 98 || drawPlayer.body == 86 || drawPlayer.body == 87 || drawPlayer.body == 99 || drawPlayer.body == 165 || drawPlayer.body == 166 || drawPlayer.body == 167 || drawPlayer.body == 171 || drawPlayer.body == 45 || drawPlayer.body == 168 || drawPlayer.body == 169 || drawPlayer.body == 42 || drawPlayer.body == 180 || drawPlayer.body == 181 || drawPlayer.body == 183 || drawPlayer.body == 186 || drawPlayer.body == 187 || drawPlayer.body == 188 || drawPlayer.body == 64 || drawPlayer.body == 189 || drawPlayer.body == 191 || drawPlayer.body == 192 || drawPlayer.body == 198 || drawPlayer.body == 199 || drawPlayer.body == 202 || drawPlayer.body == 203 || drawPlayer.body == 58 || drawPlayer.body == 59 || drawPlayer.body == 60 || drawPlayer.body == 61 || drawPlayer.body == 62 || drawPlayer.body == 63 || drawPlayer.body == 36 || drawPlayer.body == 104 || drawPlayer.body == 184 || drawPlayer.body == 74 || drawPlayer.body == 78 || drawPlayer.body == 185 || drawPlayer.body == 196 || drawPlayer.body == 197 || drawPlayer.body == 182 || drawPlayer.body == 87 || drawPlayer.body == 76 || drawPlayer.body == 209 || drawPlayer.body == 168 || drawPlayer.body == 210 || drawPlayer.body == 211 || drawPlayer.body == 213)
			{
				missingHand = true;
			}
			int body = drawPlayer.body;
			if (body == 83)
			{
				missingArm = false;
			}
			else
			{
				missingArm = true;
			}
			if (drawPlayer.heldProj >= 0 && shadow == 0f)
			{
				body = Main.projectile[drawPlayer.heldProj].type;
				if (body == 460 || body == 535 || body == 600)
				{
					heldProjOverHand = true;
				}
			}
			drawPlayer.GetHairSettings(out fullHair, out hatHair, out hideHair, out backHairDraw, out drawsBackHairWithoutHeadgear);
			hairDyePacked = PlayerDrawHelper.PackShader(drawPlayer.hairDye, PlayerDrawHelper.ShaderConfiguration.HairShader);
			if (drawPlayer.head == 0 && drawPlayer.hairDye == 0)
			{
				hairDyePacked = PlayerDrawHelper.PackShader(1, PlayerDrawHelper.ShaderConfiguration.HairShader);
			}
			skinDyePacked = player.skinDyePacked;
			if (drawPlayer.isDisplayDollOrInanimate)
			{
				Point point = Center.ToTileCoordinates();
				if (Main.InSmartCursorHighlightArea(point.X, point.Y, out var actuallySelected))
				{
					Color color = Lighting.GetColor(point.X, point.Y);
					int num = (color.R + color.G + color.B) / 3;
					if (num > 10)
					{
						selectionGlowColor = Colors.GetSelectionGlowColor(actuallySelected, num);
					}
				}
			}
			mountOffSet = drawPlayer.HeightOffsetVisual;
			Position.Y -= mountOffSet;
			if (drawPlayer.mount.Active)
			{
				Mount.currentShader = (drawPlayer.mount.Cart ? drawPlayer.cMinecart : drawPlayer.cMount);
			}
			else
			{
				Mount.currentShader = 0;
			}
			playerEffect = SpriteEffects.None;
			itemEffect = SpriteEffects.FlipHorizontally;
			colorHair = drawPlayer.GetImmuneAlpha(drawPlayer.GetHairColor(), shadow);
			colorEyeWhites = drawPlayer.GetImmuneAlpha(Lighting.GetColor((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.25) / 16.0), Color.White), shadow);
			colorEyes = drawPlayer.GetImmuneAlpha(Lighting.GetColor((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.25) / 16.0), drawPlayer.eyeColor), shadow);
			colorHead = drawPlayer.GetImmuneAlpha(Lighting.GetColor((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.25) / 16.0), drawPlayer.skinColor), shadow);
			colorBodySkin = drawPlayer.GetImmuneAlpha(Lighting.GetColor((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.5) / 16.0), drawPlayer.skinColor), shadow);
			colorLegs = drawPlayer.GetImmuneAlpha(Lighting.GetColor((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.75) / 16.0), drawPlayer.skinColor), shadow);
			colorShirt = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.5) / 16.0), drawPlayer.shirtColor), shadow);
			colorUnderShirt = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.5) / 16.0), drawPlayer.underShirtColor), shadow);
			colorPants = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.75) / 16.0), drawPlayer.pantsColor), shadow);
			colorShoes = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.75) / 16.0), drawPlayer.shoeColor), shadow);
			colorArmorHead = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)Position.Y + (double)drawPlayer.height * 0.25) / 16, Color.White), shadow);
			colorArmorBody = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)Position.Y + (double)drawPlayer.height * 0.5) / 16, Color.White), shadow);
			colorMount = colorArmorBody;
			colorArmorLegs = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)Position.Y + (double)drawPlayer.height * 0.75) / 16, Color.White), shadow);
			floatingTubeColor = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)Position.Y + (double)drawPlayer.height * 0.75) / 16, Color.White), shadow);
			colorElectricity = new Color(255, 255, 255, 100);
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			headGlowMask = -1;
			bodyGlowMask = -1;
			armGlowMask = -1;
			legsGlowMask = -1;
			headGlowColor = Color.Transparent;
			bodyGlowColor = Color.Transparent;
			armGlowColor = Color.Transparent;
			legsGlowColor = Color.Transparent;
			switch (drawPlayer.head)
			{
			case 169:
				num2++;
				break;
			case 170:
				num3++;
				break;
			case 171:
				num4++;
				break;
			case 189:
				num5++;
				break;
			}
			switch (drawPlayer.body)
			{
			case 175:
				num2++;
				break;
			case 176:
				num3++;
				break;
			case 177:
				num4++;
				break;
			case 190:
				num5++;
				break;
			}
			switch (drawPlayer.legs)
			{
			case 110:
				num2++;
				break;
			case 111:
				num3++;
				break;
			case 112:
				num4++;
				break;
			case 130:
				num5++;
				break;
			}
			num2 = 3;
			num3 = 3;
			num4 = 3;
			num5 = 3;
			ArkhalisColor = drawPlayer.underShirtColor;
			ArkhalisColor.A = 180;
			if (drawPlayer.head == 169)
			{
				headGlowMask = 15;
				byte b = (byte)(62.5f * (float)(1 + num2));
				headGlowColor = new Color((int)b, (int)b, (int)b, 0);
			}
			else if (drawPlayer.head == 216)
			{
				headGlowMask = 256;
				byte b2 = 127;
				headGlowColor = new Color((int)b2, (int)b2, (int)b2, 0);
			}
			else if (drawPlayer.head == 210)
			{
				headGlowMask = 242;
				byte b3 = 127;
				headGlowColor = new Color((int)b3, (int)b3, (int)b3, 0);
			}
			else if (drawPlayer.head == 214)
			{
				headGlowMask = 245;
				headGlowColor = ArkhalisColor;
			}
			else if (drawPlayer.head == 240)
			{
				headGlowMask = 273;
				headGlowColor = new Color(230, 230, 230, 60);
			}
			else if (drawPlayer.head == 170)
			{
				headGlowMask = 16;
				byte b4 = (byte)(62.5f * (float)(1 + num3));
				headGlowColor = new Color((int)b4, (int)b4, (int)b4, 0);
			}
			else if (drawPlayer.head == 189)
			{
				headGlowMask = 184;
				byte b5 = (byte)(62.5f * (float)(1 + num5));
				headGlowColor = new Color((int)b5, (int)b5, (int)b5, 0);
				colorArmorHead = drawPlayer.GetImmuneAlphaPure(new Color((int)b5, (int)b5, (int)b5, 255), shadow);
			}
			else if (drawPlayer.head == 171)
			{
				byte b6 = (byte)(62.5f * (float)(1 + num4));
				colorArmorHead = drawPlayer.GetImmuneAlphaPure(new Color((int)b6, (int)b6, (int)b6, 255), shadow);
			}
			else if (drawPlayer.head == 175)
			{
				headGlowMask = 41;
				headGlowColor = new Color(255, 255, 255, 0);
			}
			else if (drawPlayer.head == 193)
			{
				headGlowMask = 209;
				headGlowColor = new Color(255, 255, 255, 127);
			}
			else if (drawPlayer.head == 109)
			{
				headGlowMask = 208;
				headGlowColor = new Color(255, 255, 255, 0);
			}
			else if (drawPlayer.head == 178)
			{
				headGlowMask = 96;
				headGlowColor = new Color(255, 255, 255, 0);
			}
			if (drawPlayer.body == 175)
			{
				if (drawPlayer.Male)
				{
					bodyGlowMask = 13;
				}
				else
				{
					bodyGlowMask = 18;
				}
				byte b7 = (byte)(62.5f * (float)(1 + num2));
				bodyGlowColor = new Color((int)b7, (int)b7, (int)b7, 0);
			}
			else if (drawPlayer.body == 208)
			{
				if (drawPlayer.Male)
				{
					bodyGlowMask = 246;
				}
				else
				{
					bodyGlowMask = 247;
				}
				armGlowMask = 248;
				bodyGlowColor = ArkhalisColor;
				armGlowColor = ArkhalisColor;
			}
			else if (drawPlayer.body == 227)
			{
				bodyGlowColor = new Color(230, 230, 230, 60);
				armGlowColor = new Color(230, 230, 230, 60);
			}
			else if (drawPlayer.body == 190)
			{
				if (drawPlayer.Male)
				{
					bodyGlowMask = 185;
				}
				else
				{
					bodyGlowMask = 186;
				}
				armGlowMask = 188;
				byte b8 = (byte)(62.5f * (float)(1 + num5));
				bodyGlowColor = new Color((int)b8, (int)b8, (int)b8, 0);
				armGlowColor = new Color((int)b8, (int)b8, (int)b8, 0);
				colorArmorBody = drawPlayer.GetImmuneAlphaPure(new Color((int)b8, (int)b8, b8, 255), shadow);
			}
			else if (drawPlayer.body == 176)
			{
				if (drawPlayer.Male)
				{
					bodyGlowMask = 14;
				}
				else
				{
					bodyGlowMask = 19;
				}
				armGlowMask = 12;
				byte b9 = (byte)(62.5f * (float)(1 + num3));
				bodyGlowColor = new Color((int)b9, (int)b9, (int)b9, 0);
				armGlowColor = new Color((int)b9, (int)b9, (int)b9, 0);
			}
			else if (drawPlayer.body == 194)
			{
				bodyGlowMask = 210;
				armGlowMask = 211;
				bodyGlowColor = new Color(255, 255, 255, 127);
				armGlowColor = new Color(255, 255, 255, 127);
			}
			else if (drawPlayer.body == 177)
			{
				byte b10 = (byte)(62.5f * (float)(1 + num4));
				colorArmorBody = drawPlayer.GetImmuneAlphaPure(new Color((int)b10, (int)b10, (int)b10, 255), shadow);
			}
			else if (drawPlayer.body == 179)
			{
				if (drawPlayer.Male)
				{
					bodyGlowMask = 42;
				}
				else
				{
					bodyGlowMask = 43;
				}
				armGlowMask = 44;
				bodyGlowColor = new Color(255, 255, 255, 0);
				armGlowColor = new Color(255, 255, 255, 0);
			}
			if (drawPlayer.legs == 111)
			{
				legsGlowMask = 17;
				byte b11 = (byte)(62.5f * (float)(1 + num3));
				legsGlowColor = new Color((int)b11, (int)b11, (int)b11, 0);
			}
			else if (drawPlayer.legs == 157)
			{
				legsGlowMask = 249;
				legsGlowColor = ArkhalisColor;
			}
			else if (drawPlayer.legs == 158)
			{
				legsGlowMask = 250;
				legsGlowColor = ArkhalisColor;
			}
			else if (drawPlayer.legs == 210)
			{
				legsGlowMask = 274;
				legsGlowColor = new Color(230, 230, 230, 60);
			}
			else if (drawPlayer.legs == 110)
			{
				legsGlowMask = 199;
				byte b12 = (byte)(62.5f * (float)(1 + num2));
				legsGlowColor = new Color((int)b12, (int)b12, (int)b12, 0);
			}
			else if (drawPlayer.legs == 112)
			{
				byte b13 = (byte)(62.5f * (float)(1 + num4));
				colorArmorLegs = drawPlayer.GetImmuneAlphaPure(new Color((int)b13, (int)b13, (int)b13, 255), shadow);
			}
			else if (drawPlayer.legs == 134)
			{
				legsGlowMask = 212;
				legsGlowColor = new Color(255, 255, 255, 127);
			}
			else if (drawPlayer.legs == 130)
			{
				byte b14 = (byte)(127 * (1 + num5));
				legsGlowMask = 187;
				legsGlowColor = new Color((int)b14, (int)b14, (int)b14, 0);
				colorArmorLegs = drawPlayer.GetImmuneAlphaPure(new Color((int)b14, (int)b14, (int)b14, 255), shadow);
			}
			float alphaReduction = shadow;
			headGlowColor = drawPlayer.GetImmuneAlphaPure(headGlowColor, alphaReduction);
			bodyGlowColor = drawPlayer.GetImmuneAlphaPure(bodyGlowColor, alphaReduction);
			armGlowColor = drawPlayer.GetImmuneAlphaPure(armGlowColor, alphaReduction);
			legsGlowColor = drawPlayer.GetImmuneAlphaPure(legsGlowColor, alphaReduction);
			if (drawPlayer.head > 0 && drawPlayer.head < 266)
			{
				Main.instance.LoadArmorHead(drawPlayer.head);
				int num6 = ArmorIDs.Head.Sets.FrontToBackID[drawPlayer.head];
				if (num6 >= 0)
				{
					Main.instance.LoadArmorHead(num6);
				}
			}
			if (drawPlayer.body > 0 && drawPlayer.body < 235)
			{
				Main.instance.LoadArmorBody(drawPlayer.body);
			}
			if (drawPlayer.legs > 0 && drawPlayer.legs < 218)
			{
				Main.instance.LoadArmorLegs(drawPlayer.legs);
			}
			if (drawPlayer.handon > 0 && drawPlayer.handon < 22)
			{
				Main.instance.LoadAccHandsOn(drawPlayer.handon);
			}
			if (drawPlayer.handoff > 0 && drawPlayer.handoff < 14)
			{
				Main.instance.LoadAccHandsOff(drawPlayer.handoff);
			}
			if (drawPlayer.back > 0 && drawPlayer.back < 30)
			{
				Main.instance.LoadAccBack(drawPlayer.back);
			}
			if (drawPlayer.front > 0 && drawPlayer.front < 9)
			{
				Main.instance.LoadAccFront(drawPlayer.front);
			}
			if (drawPlayer.shoe > 0 && drawPlayer.shoe < 25)
			{
				Main.instance.LoadAccShoes(drawPlayer.shoe);
			}
			if (drawPlayer.waist > 0 && drawPlayer.waist < 17)
			{
				Main.instance.LoadAccWaist(drawPlayer.waist);
			}
			if (drawPlayer.shield > 0 && drawPlayer.shield < 10)
			{
				Main.instance.LoadAccShield(drawPlayer.shield);
			}
			if (drawPlayer.neck > 0 && drawPlayer.neck < 11)
			{
				Main.instance.LoadAccNeck(drawPlayer.neck);
			}
			if (drawPlayer.face > 0 && drawPlayer.face < 16)
			{
				Main.instance.LoadAccFace(drawPlayer.face);
			}
			if (drawPlayer.balloon > 0 && drawPlayer.balloon < 18)
			{
				Main.instance.LoadAccBalloon(drawPlayer.balloon);
			}
			Main.instance.LoadHair(drawPlayer.hair);
			if (drawPlayer.isHatRackDoll)
			{
				colorLegs = Color.Transparent;
				colorBodySkin = Color.Transparent;
				colorHead = Color.Transparent;
				colorHair = Color.Transparent;
				colorEyes = Color.Transparent;
				colorEyeWhites = Color.Transparent;
			}
			if (drawPlayer.isDisplayDollOrInanimate)
			{
				PlayerDrawHelper.UnpackShader(skinDyePacked, out var localShaderIndex, out var shaderType);
				if (shaderType == PlayerDrawHelper.ShaderConfiguration.TilePaintID && localShaderIndex == 31)
				{
					colorHead = Color.White;
					colorBodySkin = Color.White;
					colorLegs = Color.White;
					colorEyes = Color.White;
					colorEyeWhites = Color.White;
					colorArmorHead = Color.White;
					colorArmorBody = Color.White;
					colorArmorLegs = Color.White;
				}
			}
			if (!drawPlayer.isDisplayDollOrInanimate)
			{
				if ((drawPlayer.head == 78 || drawPlayer.head == 79 || drawPlayer.head == 80) && drawPlayer.body == 51 && drawPlayer.legs == 47)
				{
					float num7 = (float)(int)Main.mouseTextColor / 200f - 0.3f;
					if (shadow != 0f)
					{
						num7 = 0f;
					}
					colorArmorHead.R = (byte)((float)(int)colorArmorHead.R * num7);
					colorArmorHead.G = (byte)((float)(int)colorArmorHead.G * num7);
					colorArmorHead.B = (byte)((float)(int)colorArmorHead.B * num7);
					colorArmorBody.R = (byte)((float)(int)colorArmorBody.R * num7);
					colorArmorBody.G = (byte)((float)(int)colorArmorBody.G * num7);
					colorArmorBody.B = (byte)((float)(int)colorArmorBody.B * num7);
					colorArmorLegs.R = (byte)((float)(int)colorArmorLegs.R * num7);
					colorArmorLegs.G = (byte)((float)(int)colorArmorLegs.G * num7);
					colorArmorLegs.B = (byte)((float)(int)colorArmorLegs.B * num7);
				}
				if (drawPlayer.head == 193 && drawPlayer.body == 194 && drawPlayer.legs == 134)
				{
					float num8 = 0.6f - drawPlayer.ghostFade * 0.3f;
					if (shadow != 0f)
					{
						num8 = 0f;
					}
					colorArmorHead.R = (byte)((float)(int)colorArmorHead.R * num8);
					colorArmorHead.G = (byte)((float)(int)colorArmorHead.G * num8);
					colorArmorHead.B = (byte)((float)(int)colorArmorHead.B * num8);
					colorArmorBody.R = (byte)((float)(int)colorArmorBody.R * num8);
					colorArmorBody.G = (byte)((float)(int)colorArmorBody.G * num8);
					colorArmorBody.B = (byte)((float)(int)colorArmorBody.B * num8);
					colorArmorLegs.R = (byte)((float)(int)colorArmorLegs.R * num8);
					colorArmorLegs.G = (byte)((float)(int)colorArmorLegs.G * num8);
					colorArmorLegs.B = (byte)((float)(int)colorArmorLegs.B * num8);
				}
				if (shadow > 0f)
				{
					colorLegs = Color.Transparent;
					colorBodySkin = Color.Transparent;
					colorHead = Color.Transparent;
					colorHair = Color.Transparent;
					colorEyes = Color.Transparent;
					colorEyeWhites = Color.Transparent;
				}
			}
			float num9 = 1f;
			float num10 = 1f;
			float num11 = 1f;
			float num12 = 1f;
			if (drawPlayer.honey && Main.rand.Next(30) == 0 && shadow == 0f)
			{
				Dust dust2 = Dust.NewDustDirect(Position, drawPlayer.width, drawPlayer.height, 152, 0f, 0f, 150);
				dust2.velocity.Y = 0.3f;
				dust2.velocity.X *= 0.1f;
				dust2.scale += (float)Main.rand.Next(3, 4) * 0.1f;
				dust2.alpha = 100;
				dust2.noGravity = true;
				dust2.velocity += drawPlayer.velocity * 0.1f;
				DustCache.Add(dust2.dustIndex);
			}
			if (drawPlayer.dryadWard && drawPlayer.velocity.X != 0f && Main.rand.Next(4) == 0)
			{
				Dust dust3 = Dust.NewDustDirect(new Vector2(drawPlayer.position.X - 2f, drawPlayer.position.Y + (float)drawPlayer.height - 2f), drawPlayer.width + 4, 4, 163, 0f, 0f, 100, default(Color), 1.5f);
				dust3.noGravity = true;
				dust3.noLight = true;
				dust3.velocity *= 0f;
				DustCache.Add(dust3.dustIndex);
			}
			if (drawPlayer.poisoned)
			{
				if (Main.rand.Next(50) == 0 && shadow == 0f)
				{
					Dust dust4 = Dust.NewDustDirect(Position, drawPlayer.width, drawPlayer.height, 46, 0f, 0f, 150, default(Color), 0.2f);
					dust4.noGravity = true;
					dust4.fadeIn = 1.9f;
					DustCache.Add(dust4.dustIndex);
				}
				num9 *= 0.65f;
				num11 *= 0.75f;
			}
			if (drawPlayer.venom)
			{
				if (Main.rand.Next(10) == 0 && shadow == 0f)
				{
					Dust dust5 = Dust.NewDustDirect(Position, drawPlayer.width, drawPlayer.height, 171, 0f, 0f, 100, default(Color), 0.5f);
					dust5.noGravity = true;
					dust5.fadeIn = 1.5f;
					DustCache.Add(dust5.dustIndex);
				}
				num10 *= 0.45f;
				num9 *= 0.75f;
			}
			if (drawPlayer.onFire)
			{
				if (Main.rand.Next(4) == 0 && shadow == 0f)
				{
					Dust dust6 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 6, drawPlayer.velocity.X * 0.4f, drawPlayer.velocity.Y * 0.4f, 100, default(Color), 3f);
					dust6.noGravity = true;
					dust6.velocity *= 1.8f;
					dust6.velocity.Y -= 0.5f;
					DustCache.Add(dust6.dustIndex);
				}
				num11 *= 0.6f;
				num10 *= 0.7f;
			}
			if (drawPlayer.dripping && shadow == 0f && Main.rand.Next(4) != 0)
			{
				Vector2 position = Position;
				position.X -= 2f;
				position.Y -= 2f;
				if (Main.rand.Next(2) == 0)
				{
					Dust dust7 = Dust.NewDustDirect(position, drawPlayer.width + 4, drawPlayer.height + 2, 211, 0f, 0f, 50, default(Color), 0.8f);
					if (Main.rand.Next(2) == 0)
					{
						dust7.alpha += 25;
					}
					if (Main.rand.Next(2) == 0)
					{
						dust7.alpha += 25;
					}
					dust7.noLight = true;
					dust7.velocity *= 0.2f;
					dust7.velocity.Y += 0.2f;
					dust7.velocity += drawPlayer.velocity;
					DustCache.Add(dust7.dustIndex);
				}
				else
				{
					Dust dust8 = Dust.NewDustDirect(position, drawPlayer.width + 8, drawPlayer.height + 8, 211, 0f, 0f, 50, default(Color), 1.1f);
					if (Main.rand.Next(2) == 0)
					{
						dust8.alpha += 25;
					}
					if (Main.rand.Next(2) == 0)
					{
						dust8.alpha += 25;
					}
					dust8.noLight = true;
					dust8.noGravity = true;
					dust8.velocity *= 0.2f;
					dust8.velocity.Y += 1f;
					dust8.velocity += drawPlayer.velocity;
					DustCache.Add(dust8.dustIndex);
				}
			}
			if (drawPlayer.drippingSlime)
			{
				int alpha = 175;
				Color newColor = new Color(0, 80, 255, 100);
				if (Main.rand.Next(4) != 0 && shadow == 0f)
				{
					Vector2 position2 = Position;
					position2.X -= 2f;
					position2.Y -= 2f;
					if (Main.rand.Next(2) == 0)
					{
						Dust dust9 = Dust.NewDustDirect(position2, drawPlayer.width + 4, drawPlayer.height + 2, 4, 0f, 0f, alpha, newColor, 1.4f);
						if (Main.rand.Next(2) == 0)
						{
							dust9.alpha += 25;
						}
						if (Main.rand.Next(2) == 0)
						{
							dust9.alpha += 25;
						}
						dust9.noLight = true;
						dust9.velocity *= 0.2f;
						dust9.velocity.Y += 0.2f;
						dust9.velocity += drawPlayer.velocity;
						DustCache.Add(dust9.dustIndex);
					}
				}
				num9 *= 0.8f;
				num10 *= 0.8f;
			}
			if (drawPlayer.drippingSparkleSlime)
			{
				int alpha2 = 100;
				if (Main.rand.Next(4) != 0 && shadow == 0f)
				{
					Vector2 position3 = Position;
					position3.X -= 2f;
					position3.Y -= 2f;
					if (Main.rand.Next(4) == 0)
					{
						Color newColor2 = Main.hslToRgb(0.7f + 0.2f * Main.rand.NextFloat(), 1f, 0.5f);
						newColor2.A /= 2;
						Dust dust10 = Dust.NewDustDirect(position3, drawPlayer.width + 4, drawPlayer.height + 2, 4, 0f, 0f, alpha2, newColor2, 0.65f);
						if (Main.rand.Next(2) == 0)
						{
							dust10.alpha += 25;
						}
						if (Main.rand.Next(2) == 0)
						{
							dust10.alpha += 25;
						}
						dust10.noLight = true;
						dust10.velocity *= 0.2f;
						dust10.velocity += drawPlayer.velocity * 0.7f;
						dust10.fadeIn = 0.8f;
						DustCache.Add(dust10.dustIndex);
					}
					if (Main.rand.Next(30) == 0)
					{
						Color color2 = Main.hslToRgb(0.7f + 0.2f * Main.rand.NextFloat(), 1f, 0.5f);
						color2.A /= 2;
						Dust dust11 = Dust.NewDustDirect(position3, drawPlayer.width + 4, drawPlayer.height + 2, 43, 0f, 0f, 254, new Color(127, 127, 127, 0), 0.45f);
						dust11.noLight = true;
						dust11.velocity.X *= 0f;
						dust11.velocity *= 0.03f;
						dust11.fadeIn = 0.6f;
						DustCache.Add(dust11.dustIndex);
					}
				}
				num9 *= 0.94f;
				num10 *= 0.82f;
			}
			if (drawPlayer.ichor)
			{
				num11 = 0f;
			}
			if (drawPlayer.electrified && shadow == 0f && Main.rand.Next(3) == 0)
			{
				Dust dust12 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 226, 0f, 0f, 100, default(Color), 0.5f);
				dust12.velocity *= 1.6f;
				dust12.velocity.Y -= 1f;
				dust12.position = Vector2.Lerp(dust12.position, drawPlayer.Center, 0.5f);
				DustCache.Add(dust12.dustIndex);
			}
			if (drawPlayer.burned)
			{
				if (shadow == 0f)
				{
					Dust dust13 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 6, drawPlayer.velocity.X * 0.4f, drawPlayer.velocity.Y * 0.4f, 100, default(Color), 2f);
					dust13.noGravity = true;
					dust13.velocity *= 1.8f;
					dust13.velocity.Y -= 0.75f;
					DustCache.Add(dust13.dustIndex);
				}
				num9 = 1f;
				num11 *= 0.6f;
				num10 *= 0.7f;
			}
			if (drawPlayer.onFrostBurn)
			{
				if (Main.rand.Next(4) == 0 && shadow == 0f)
				{
					Dust dust14 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 135, drawPlayer.velocity.X * 0.4f, drawPlayer.velocity.Y * 0.4f, 100, default(Color), 3f);
					dust14.noGravity = true;
					dust14.velocity *= 1.8f;
					dust14.velocity.Y -= 0.5f;
					DustCache.Add(dust14.dustIndex);
				}
				num9 *= 0.5f;
				num10 *= 0.7f;
			}
			if (drawPlayer.onFire2)
			{
				if (Main.rand.Next(4) == 0 && shadow == 0f)
				{
					Dust dust15 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 75, drawPlayer.velocity.X * 0.4f, drawPlayer.velocity.Y * 0.4f, 100, default(Color), 3f);
					dust15.noGravity = true;
					dust15.velocity *= 1.8f;
					dust15.velocity.Y -= 0.5f;
					DustCache.Add(dust15.dustIndex);
				}
				num11 *= 0.6f;
				num10 *= 0.7f;
			}
			if (drawPlayer.noItems)
			{
				num10 *= 0.8f;
				num9 *= 0.65f;
			}
			if (drawPlayer.blind)
			{
				num10 *= 0.65f;
				num9 *= 0.7f;
			}
			if (drawPlayer.bleed)
			{
				num10 *= 0.9f;
				num11 *= 0.9f;
				if (!drawPlayer.dead && Main.rand.Next(30) == 0 && shadow == 0f)
				{
					Dust dust16 = Dust.NewDustDirect(Position, drawPlayer.width, drawPlayer.height, 5);
					dust16.velocity.Y += 0.5f;
					dust16.velocity *= 0.25f;
					DustCache.Add(dust16.dustIndex);
				}
			}
			if (shadow == 0f && drawPlayer.palladiumRegen && drawPlayer.statLife < drawPlayer.statLifeMax2 && Main.instance.IsActive && !Main.gamePaused && drawPlayer.miscCounter % 10 == 0 && shadow == 0f)
			{
				Vector2 position4 = default(Vector2);
				position4.X = Position.X + (float)Main.rand.Next(drawPlayer.width);
				position4.Y = Position.Y + (float)Main.rand.Next(drawPlayer.height);
				position4.X = Position.X + (float)(drawPlayer.width / 2) - 6f;
				position4.Y = Position.Y + (float)(drawPlayer.height / 2) - 6f;
				position4.X -= Main.rand.Next(-10, 11);
				position4.Y -= Main.rand.Next(-20, 21);
				int item = Gore.NewGore(position4, new Vector2((float)Main.rand.Next(-10, 11) * 0.1f, (float)Main.rand.Next(-20, -10) * 0.1f), 331, (float)Main.rand.Next(80, 120) * 0.01f);
				GoreCache.Add(item);
			}
			if (shadow == 0f && drawPlayer.loveStruck && Main.instance.IsActive && !Main.gamePaused && Main.rand.Next(5) == 0)
			{
				Vector2 value = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
				value.Normalize();
				value.X *= 0.66f;
				int num13 = Gore.NewGore(Position + new Vector2(Main.rand.Next(drawPlayer.width + 1), Main.rand.Next(drawPlayer.height + 1)), value * Main.rand.Next(3, 6) * 0.33f, 331, (float)Main.rand.Next(40, 121) * 0.01f);
				Main.gore[num13].sticky = false;
				Main.gore[num13].velocity *= 0.4f;
				Main.gore[num13].velocity.Y -= 0.6f;
				GoreCache.Add(num13);
			}
			if (drawPlayer.stinky && Main.instance.IsActive && !Main.gamePaused)
			{
				num9 *= 0.7f;
				num11 *= 0.55f;
				if (Main.rand.Next(5) == 0 && shadow == 0f)
				{
					Vector2 value2 = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
					value2.Normalize();
					value2.X *= 0.66f;
					value2.Y = Math.Abs(value2.Y);
					Vector2 vector = value2 * Main.rand.Next(3, 5) * 0.25f;
					int num14 = Dust.NewDust(Position, drawPlayer.width, drawPlayer.height, 188, vector.X, vector.Y * 0.5f, 100, default(Color), 1.5f);
					Main.dust[num14].velocity *= 0.1f;
					Main.dust[num14].velocity.Y -= 0.5f;
					DustCache.Add(num14);
				}
			}
			if (drawPlayer.slowOgreSpit && Main.instance.IsActive && !Main.gamePaused)
			{
				num9 *= 0.6f;
				num11 *= 0.45f;
				if (Main.rand.Next(5) == 0 && shadow == 0f)
				{
					int type = Utils.SelectRandom<int>(Main.rand, 4, 256);
					Dust dust17 = Main.dust[Dust.NewDust(Position, drawPlayer.width, drawPlayer.height, type, 0f, 0f, 100)];
					dust17.scale = 0.8f + Main.rand.NextFloat() * 0.6f;
					dust17.fadeIn = 0.5f;
					dust17.velocity *= 0.05f;
					dust17.noLight = true;
					if (dust17.type == 4)
					{
						dust17.color = new Color(80, 170, 40, 120);
					}
					DustCache.Add(dust17.dustIndex);
				}
				if (Main.rand.Next(5) == 0 && shadow == 0f)
				{
					int num15 = Gore.NewGore(Position + new Vector2(Main.rand.NextFloat(), Main.rand.NextFloat()) * drawPlayer.Size, Vector2.Zero, Utils.SelectRandom<int>(Main.rand, 1024, 1025, 1026), 0.65f);
					Main.gore[num15].velocity *= 0.05f;
					GoreCache.Add(num15);
				}
			}
			if (Main.instance.IsActive && !Main.gamePaused && shadow == 0f)
			{
				float num16 = (float)drawPlayer.miscCounter / 180f;
				float num17 = 0f;
				float scaleFactor = 10f;
				int type2 = 90;
				int num18 = 0;
				for (int i = 0; i < 3; i++)
				{
					switch (i)
					{
					case 0:
						if (drawPlayer.nebulaLevelLife < 1)
						{
							continue;
						}
						num17 = (float)Math.PI * 2f / (float)drawPlayer.nebulaLevelLife;
						num18 = drawPlayer.nebulaLevelLife;
						break;
					case 1:
						if (drawPlayer.nebulaLevelMana < 1)
						{
							continue;
						}
						num17 = (float)Math.PI * -2f / (float)drawPlayer.nebulaLevelMana;
						num18 = drawPlayer.nebulaLevelMana;
						num16 = (float)(-drawPlayer.miscCounter) / 180f;
						scaleFactor = 20f;
						type2 = 88;
						break;
					case 2:
						if (drawPlayer.nebulaLevelDamage < 1)
						{
							continue;
						}
						num17 = (float)Math.PI * 2f / (float)drawPlayer.nebulaLevelDamage;
						num18 = drawPlayer.nebulaLevelDamage;
						num16 = (float)drawPlayer.miscCounter / 180f;
						scaleFactor = 30f;
						type2 = 86;
						break;
					}
					for (int j = 0; j < num18; j++)
					{
						Dust dust18 = Dust.NewDustDirect(Position, drawPlayer.width, drawPlayer.height, type2, 0f, 0f, 100, default(Color), 1.5f);
						dust18.noGravity = true;
						dust18.velocity = Vector2.Zero;
						dust18.position = drawPlayer.Center + Vector2.UnitY * drawPlayer.gfxOffY + (num16 * ((float)Math.PI * 2f) + num17 * (float)j).ToRotationVector2() * scaleFactor;
						dust18.customData = drawPlayer;
						DustCache.Add(dust18.dustIndex);
					}
				}
			}
			if (drawPlayer.witheredArmor && Main.instance.IsActive && !Main.gamePaused)
			{
				num10 *= 0.5f;
				num9 *= 0.75f;
			}
			if (drawPlayer.witheredWeapon && drawPlayer.itemAnimation > 0 && heldItem.damage > 0 && Main.instance.IsActive && !Main.gamePaused && Main.rand.Next(3) == 0)
			{
				Dust dust19 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 272, 0f, 0f, 50, default(Color), 0.5f);
				dust19.velocity *= 1.6f;
				dust19.velocity.Y -= 1f;
				dust19.position = Vector2.Lerp(dust19.position, drawPlayer.Center, 0.5f);
				DustCache.Add(dust19.dustIndex);
			}
			if (num9 != 1f || num10 != 1f || num11 != 1f || num12 != 1f)
			{
				if (drawPlayer.onFire || drawPlayer.onFire2 || drawPlayer.onFrostBurn)
				{
					colorEyeWhites = drawPlayer.GetImmuneAlpha(Color.White, shadow);
					colorEyes = drawPlayer.GetImmuneAlpha(drawPlayer.eyeColor, shadow);
					colorHair = drawPlayer.GetImmuneAlpha(drawPlayer.GetHairColor(useLighting: false), shadow);
					colorHead = drawPlayer.GetImmuneAlpha(drawPlayer.skinColor, shadow);
					colorBodySkin = drawPlayer.GetImmuneAlpha(drawPlayer.skinColor, shadow);
					colorShirt = drawPlayer.GetImmuneAlpha(drawPlayer.shirtColor, shadow);
					colorUnderShirt = drawPlayer.GetImmuneAlpha(drawPlayer.underShirtColor, shadow);
					colorPants = drawPlayer.GetImmuneAlpha(drawPlayer.pantsColor, shadow);
					colorLegs = drawPlayer.GetImmuneAlpha(drawPlayer.skinColor, shadow);
					colorShoes = drawPlayer.GetImmuneAlpha(drawPlayer.shoeColor, shadow);
					colorArmorHead = drawPlayer.GetImmuneAlpha(Color.White, shadow);
					colorArmorBody = drawPlayer.GetImmuneAlpha(Color.White, shadow);
					colorArmorLegs = drawPlayer.GetImmuneAlpha(Color.White, shadow);
				}
				else
				{
					colorEyeWhites = Main.buffColor(colorEyeWhites, num9, num10, num11, num12);
					colorEyes = Main.buffColor(colorEyes, num9, num10, num11, num12);
					colorHair = Main.buffColor(colorHair, num9, num10, num11, num12);
					colorHead = Main.buffColor(colorHead, num9, num10, num11, num12);
					colorBodySkin = Main.buffColor(colorBodySkin, num9, num10, num11, num12);
					colorShirt = Main.buffColor(colorShirt, num9, num10, num11, num12);
					colorUnderShirt = Main.buffColor(colorUnderShirt, num9, num10, num11, num12);
					colorPants = Main.buffColor(colorPants, num9, num10, num11, num12);
					colorLegs = Main.buffColor(colorLegs, num9, num10, num11, num12);
					colorShoes = Main.buffColor(colorShoes, num9, num10, num11, num12);
					colorArmorHead = Main.buffColor(colorArmorHead, num9, num10, num11, num12);
					colorArmorBody = Main.buffColor(colorArmorBody, num9, num10, num11, num12);
					colorArmorLegs = Main.buffColor(colorArmorLegs, num9, num10, num11, num12);
				}
			}
			if (drawPlayer.socialGhost)
			{
				colorEyeWhites = Color.Transparent;
				colorEyes = Color.Transparent;
				colorHair = Color.Transparent;
				colorHead = Color.Transparent;
				colorBodySkin = Color.Transparent;
				colorShirt = Color.Transparent;
				colorUnderShirt = Color.Transparent;
				colorPants = Color.Transparent;
				colorShoes = Color.Transparent;
				colorLegs = Color.Transparent;
				if (colorArmorHead.A > Main.gFade)
				{
					colorArmorHead.A = Main.gFade;
				}
				if (colorArmorBody.A > Main.gFade)
				{
					colorArmorBody.A = Main.gFade;
				}
				if (colorArmorLegs.A > Main.gFade)
				{
					colorArmorLegs.A = Main.gFade;
				}
			}
			if (drawPlayer.socialIgnoreLight)
			{
				float scale = 1.2f;
				colorEyeWhites = Color.White * scale;
				colorEyes = drawPlayer.eyeColor * scale;
				colorHair = GameShaders.Hair.GetColor(drawPlayer.hairDye, drawPlayer, Color.White);
				colorHead = drawPlayer.skinColor * scale;
				colorBodySkin = drawPlayer.skinColor * scale;
				colorShirt = drawPlayer.shirtColor * scale;
				colorUnderShirt = drawPlayer.underShirtColor * scale;
				colorPants = drawPlayer.pantsColor * scale;
				colorShoes = drawPlayer.shoeColor * scale;
				colorLegs = drawPlayer.skinColor * scale;
			}
			stealth = 1f;
			if (heldItem.type == 3106)
			{
				float num19 = drawPlayer.stealth;
				if ((double)num19 < 0.03)
				{
					num19 = 0.03f;
				}
				float num20 = (1f + num19 * 10f) / 11f;
				if (num19 < 0f)
				{
					num19 = 0f;
				}
				if (!(num19 < 1f - shadow) && shadow > 0f)
				{
					num19 = shadow * 0.5f;
				}
				stealth = num20;
				colorArmorHead = new Color((byte)((float)(int)colorArmorHead.R * num19), (byte)((float)(int)colorArmorHead.G * num19), (byte)((float)(int)colorArmorHead.B * num20), (byte)((float)(int)colorArmorHead.A * num19));
				colorArmorBody = new Color((byte)((float)(int)colorArmorBody.R * num19), (byte)((float)(int)colorArmorBody.G * num19), (byte)((float)(int)colorArmorBody.B * num20), (byte)((float)(int)colorArmorBody.A * num19));
				colorArmorLegs = new Color((byte)((float)(int)colorArmorLegs.R * num19), (byte)((float)(int)colorArmorLegs.G * num19), (byte)((float)(int)colorArmorLegs.B * num20), (byte)((float)(int)colorArmorLegs.A * num19));
				num19 *= num19;
				colorEyeWhites = Color.Multiply(colorEyeWhites, num19);
				colorEyes = Color.Multiply(colorEyes, num19);
				colorHair = Color.Multiply(colorHair, num19);
				colorHead = Color.Multiply(colorHead, num19);
				colorBodySkin = Color.Multiply(colorBodySkin, num19);
				colorShirt = Color.Multiply(colorShirt, num19);
				colorUnderShirt = Color.Multiply(colorUnderShirt, num19);
				colorPants = Color.Multiply(colorPants, num19);
				colorShoes = Color.Multiply(colorShoes, num19);
				colorLegs = Color.Multiply(colorLegs, num19);
				colorMount = Color.Multiply(colorMount, num19);
				headGlowColor = Color.Multiply(headGlowColor, num19);
				bodyGlowColor = Color.Multiply(bodyGlowColor, num19);
				armGlowColor = Color.Multiply(armGlowColor, num19);
				legsGlowColor = Color.Multiply(legsGlowColor, num19);
			}
			else if (drawPlayer.shroomiteStealth)
			{
				float num21 = drawPlayer.stealth;
				if ((double)num21 < 0.03)
				{
					num21 = 0.03f;
				}
				float num22 = (1f + num21 * 10f) / 11f;
				if (num21 < 0f)
				{
					num21 = 0f;
				}
				if (!(num21 < 1f - shadow) && shadow > 0f)
				{
					num21 = shadow * 0.5f;
				}
				stealth = num22;
				colorArmorHead = new Color((byte)((float)(int)colorArmorHead.R * num21), (byte)((float)(int)colorArmorHead.G * num21), (byte)((float)(int)colorArmorHead.B * num22), (byte)((float)(int)colorArmorHead.A * num21));
				colorArmorBody = new Color((byte)((float)(int)colorArmorBody.R * num21), (byte)((float)(int)colorArmorBody.G * num21), (byte)((float)(int)colorArmorBody.B * num22), (byte)((float)(int)colorArmorBody.A * num21));
				colorArmorLegs = new Color((byte)((float)(int)colorArmorLegs.R * num21), (byte)((float)(int)colorArmorLegs.G * num21), (byte)((float)(int)colorArmorLegs.B * num22), (byte)((float)(int)colorArmorLegs.A * num21));
				num21 *= num21;
				colorEyeWhites = Color.Multiply(colorEyeWhites, num21);
				colorEyes = Color.Multiply(colorEyes, num21);
				colorHair = Color.Multiply(colorHair, num21);
				colorHead = Color.Multiply(colorHead, num21);
				colorBodySkin = Color.Multiply(colorBodySkin, num21);
				colorShirt = Color.Multiply(colorShirt, num21);
				colorUnderShirt = Color.Multiply(colorUnderShirt, num21);
				colorPants = Color.Multiply(colorPants, num21);
				colorShoes = Color.Multiply(colorShoes, num21);
				colorLegs = Color.Multiply(colorLegs, num21);
				colorMount = Color.Multiply(colorMount, num21);
				headGlowColor = Color.Multiply(headGlowColor, num21);
				bodyGlowColor = Color.Multiply(bodyGlowColor, num21);
				armGlowColor = Color.Multiply(armGlowColor, num21);
				legsGlowColor = Color.Multiply(legsGlowColor, num21);
			}
			else if (drawPlayer.setVortex)
			{
				float num23 = drawPlayer.stealth;
				if ((double)num23 < 0.03)
				{
					num23 = 0.03f;
				}
				if (num23 < 0f)
				{
					num23 = 0f;
				}
				if (!(num23 < 1f - shadow) && shadow > 0f)
				{
					num23 = shadow * 0.5f;
				}
				stealth = num23;
				Color secondColor = new Color(Vector4.Lerp(Vector4.One, new Vector4(0f, 0.12f, 0.16f, 0f), 1f - num23));
				colorArmorHead = colorArmorHead.MultiplyRGBA(secondColor);
				colorArmorBody = colorArmorBody.MultiplyRGBA(secondColor);
				colorArmorLegs = colorArmorLegs.MultiplyRGBA(secondColor);
				num23 *= num23;
				colorEyeWhites = Color.Multiply(colorEyeWhites, num23);
				colorEyes = Color.Multiply(colorEyes, num23);
				colorHair = Color.Multiply(colorHair, num23);
				colorHead = Color.Multiply(colorHead, num23);
				colorBodySkin = Color.Multiply(colorBodySkin, num23);
				colorShirt = Color.Multiply(colorShirt, num23);
				colorUnderShirt = Color.Multiply(colorUnderShirt, num23);
				colorPants = Color.Multiply(colorPants, num23);
				colorShoes = Color.Multiply(colorShoes, num23);
				colorLegs = Color.Multiply(colorLegs, num23);
				colorMount = Color.Multiply(colorMount, num23);
				headGlowColor = Color.Multiply(headGlowColor, num23);
				bodyGlowColor = Color.Multiply(bodyGlowColor, num23);
				armGlowColor = Color.Multiply(armGlowColor, num23);
				legsGlowColor = Color.Multiply(legsGlowColor, num23);
			}
			if (drawPlayer.gravDir == 1f)
			{
				if (drawPlayer.direction == 1)
				{
					playerEffect = SpriteEffects.None;
					itemEffect = SpriteEffects.None;
				}
				else
				{
					playerEffect = SpriteEffects.FlipHorizontally;
					itemEffect = SpriteEffects.FlipHorizontally;
				}
				if (!drawPlayer.dead)
				{
					drawPlayer.legPosition.Y = 0f;
					drawPlayer.headPosition.Y = 0f;
					drawPlayer.bodyPosition.Y = 0f;
				}
			}
			else
			{
				if (drawPlayer.direction == 1)
				{
					playerEffect = SpriteEffects.FlipVertically;
					itemEffect = SpriteEffects.FlipVertically;
				}
				else
				{
					playerEffect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
					itemEffect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
				}
				if (!drawPlayer.dead)
				{
					drawPlayer.legPosition.Y = 6f;
					drawPlayer.headPosition.Y = 6f;
					drawPlayer.bodyPosition.Y = 6f;
				}
			}
			body = heldItem.type;
			if (body == 3182 || (uint)(body - 3184) <= 1u || body == 3782)
			{
				itemEffect ^= SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
			}
			legVect = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.75f);
			bodyVect = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);
			headVect = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.4f);
			if ((drawPlayer.merman || drawPlayer.forceMerman) && !drawPlayer.hideMerman)
			{
				drawPlayer.headRotation = drawPlayer.velocity.Y * (float)drawPlayer.direction * 0.1f;
				if ((double)drawPlayer.headRotation < -0.3)
				{
					drawPlayer.headRotation = -0.3f;
				}
				if ((double)drawPlayer.headRotation > 0.3)
				{
					drawPlayer.headRotation = 0.3f;
				}
			}
			else if (!drawPlayer.dead)
			{
				drawPlayer.headRotation = 0f;
			}
			hairFrame = drawPlayer.bodyFrame;
			hairFrame.Y -= 336;
			if (hairFrame.Y < 0)
			{
				hairFrame.Y = 0;
			}
			if (hideHair)
			{
				hairFrame.Height = 0;
			}
			hidesTopSkin = drawPlayer.body == 82 || drawPlayer.body == 83 || drawPlayer.body == 93 || drawPlayer.body == 21 || drawPlayer.body == 22;
			hidesBottomSkin = drawPlayer.body == 93 || drawPlayer.legs == 20 || drawPlayer.legs == 21;
			drawFloatingTube = drawPlayer.hasFloatingTube;
			drawUnicornHorn = drawPlayer.hasUnicornHorn;
			drawFrontAccInNeckAccLayer = false;
			if (drawPlayer.bodyFrame.Y / drawPlayer.bodyFrame.Height == 5)
			{
				drawFrontAccInNeckAccLayer = drawPlayer.front > 0 && drawPlayer.front < 9 && ArmorIDs.Front.Sets.DrawsInNeckLayer[drawPlayer.front];
			}
			helmetOffset = drawPlayer.GetHelmetDrawOffset();
			CreateCompositeData();
		}

		private void CreateCompositeData()
		{
			frontShoulderOffset = Vector2.Zero;
			backShoulderOffset = Vector2.Zero;
			usesCompositeTorso = drawPlayer.body > 0 && drawPlayer.body < 235 && ArmorIDs.Body.Sets.UsesNewFramingCode[drawPlayer.body];
			usesCompositeFrontHandAcc = drawPlayer.handon > 0 && drawPlayer.handon < 22 && ArmorIDs.HandOn.Sets.UsesNewFramingCode[drawPlayer.handon];
			usesCompositeBackHandAcc = drawPlayer.handoff > 0 && drawPlayer.handoff < 14 && ArmorIDs.HandOff.Sets.UsesNewFramingCode[drawPlayer.handoff];
			if (drawPlayer.body < 1)
			{
				usesCompositeTorso = true;
			}
			if (!usesCompositeTorso)
			{
				return;
			}
			Point pt = new Point(1, 1);
			Point pt2 = new Point(0, 1);
			Point pt3 = default(Point);
			Point frameIndex = default(Point);
			Point frameIndex2 = default(Point);
			int num = drawPlayer.bodyFrame.Y / drawPlayer.bodyFrame.Height;
			compShoulderOverFrontArm = true;
			hideCompositeShoulders = false;
			bool flag = true;
			if (drawPlayer.body > 0)
			{
				flag = ArmorIDs.Body.Sets.showsShouldersWhileJumping[drawPlayer.body];
			}
			bool flag2 = false;
			if (drawPlayer.handon > 0)
			{
				flag2 = ArmorIDs.HandOn.Sets.UsesOldFramingTexturesForWalking[drawPlayer.handon];
			}
			bool flag3 = !flag2;
			switch (num)
			{
			case 0:
				frameIndex2.X = 2;
				flag3 = true;
				break;
			case 1:
				frameIndex2.X = 3;
				compShoulderOverFrontArm = false;
				flag3 = true;
				break;
			case 2:
				frameIndex2.X = 4;
				compShoulderOverFrontArm = false;
				flag3 = true;
				break;
			case 3:
				frameIndex2.X = 5;
				compShoulderOverFrontArm = true;
				flag3 = true;
				break;
			case 4:
				frameIndex2.X = 6;
				compShoulderOverFrontArm = true;
				flag3 = true;
				break;
			case 5:
				frameIndex2.X = 2;
				frameIndex2.Y = 1;
				pt3.X = 1;
				compShoulderOverFrontArm = false;
				flag3 = true;
				if (!flag)
				{
					hideCompositeShoulders = true;
				}
				break;
			case 6:
				frameIndex2.X = 3;
				frameIndex2.Y = 1;
				break;
			case 7:
			case 8:
			case 9:
			case 10:
				frameIndex2.X = 4;
				frameIndex2.Y = 1;
				break;
			case 11:
			case 12:
			case 13:
				frameIndex2.X = 3;
				frameIndex2.Y = 1;
				break;
			case 14:
				frameIndex2.X = 5;
				frameIndex2.Y = 1;
				break;
			case 15:
			case 16:
				frameIndex2.X = 6;
				frameIndex2.Y = 1;
				break;
			case 17:
				frameIndex2.X = 5;
				frameIndex2.Y = 1;
				break;
			case 18:
			case 19:
				frameIndex2.X = 3;
				frameIndex2.Y = 1;
				break;
			}
			CreateCompositeData_DetermineShoulderOffsets(drawPlayer.body, num);
			backShoulderOffset *= new Vector2(drawPlayer.direction, drawPlayer.gravDir);
			frontShoulderOffset *= new Vector2(drawPlayer.direction, drawPlayer.gravDir);
			if (drawPlayer.body > 0 && ArmorIDs.Body.Sets.shouldersAreAlwaysInTheBack[drawPlayer.body])
			{
				compShoulderOverFrontArm = false;
			}
			usesCompositeFrontHandAcc = flag3;
			frameIndex.X = frameIndex2.X;
			frameIndex.Y = frameIndex2.Y + 2;
			UpdateCompositeArm(drawPlayer.compositeFrontArm, compositeFrontArmRotation, frameIndex2, 7);
			UpdateCompositeArm(drawPlayer.compositeBackArm, compositeBackArmRotation, frameIndex, 8);
			if (!drawPlayer.Male)
			{
				pt.Y += 2;
				pt2.Y += 2;
				pt3.Y += 2;
			}
			compBackShoulderFrame = CreateCompositeFrameRect(pt);
			compFrontShoulderFrame = CreateCompositeFrameRect(pt2);
			compBackArmFrame = CreateCompositeFrameRect(frameIndex);
			compFrontArmFrame = CreateCompositeFrameRect(frameIndex2);
			compTorsoFrame = CreateCompositeFrameRect(pt3);
		}

		private void CreateCompositeData_DetermineShoulderOffsets(int armor, int targetFrameNumber)
		{
			int num = 0;
			switch (armor)
			{
			case 55:
				num = 1;
				break;
			case 71:
				num = 2;
				break;
			case 204:
				num = 3;
				break;
			case 183:
				num = 4;
				break;
			case 201:
				num = 5;
				break;
			case 101:
				num = 6;
				break;
			case 207:
				num = 7;
				break;
			}
			switch (num)
			{
			case 1:
				switch (targetFrameNumber)
				{
				case 6:
					frontShoulderOffset.X = -2f;
					break;
				case 7:
				case 8:
				case 9:
				case 10:
					frontShoulderOffset.X = -4f;
					break;
				case 11:
				case 12:
				case 13:
				case 14:
					frontShoulderOffset.X = -2f;
					break;
				case 18:
				case 19:
					frontShoulderOffset.X = -2f;
					break;
				case 15:
				case 16:
				case 17:
					break;
				}
				break;
			case 2:
				switch (targetFrameNumber)
				{
				case 6:
					frontShoulderOffset.X = -2f;
					break;
				case 7:
				case 8:
				case 9:
				case 10:
					frontShoulderOffset.X = -4f;
					break;
				case 11:
				case 12:
				case 13:
				case 14:
					frontShoulderOffset.X = -2f;
					break;
				case 18:
				case 19:
					frontShoulderOffset.X = -2f;
					break;
				case 15:
				case 16:
				case 17:
					break;
				}
				break;
			case 3:
				switch (targetFrameNumber)
				{
				case 7:
				case 8:
				case 9:
					frontShoulderOffset.X = -2f;
					break;
				case 15:
				case 16:
				case 17:
					frontShoulderOffset.X = 2f;
					break;
				}
				break;
			case 4:
				switch (targetFrameNumber)
				{
				case 6:
					frontShoulderOffset.X = -2f;
					break;
				case 7:
				case 8:
				case 9:
				case 10:
					frontShoulderOffset.X = -4f;
					break;
				case 11:
				case 12:
				case 13:
					frontShoulderOffset.X = -2f;
					break;
				case 15:
				case 16:
					frontShoulderOffset.X = 2f;
					break;
				case 18:
				case 19:
					frontShoulderOffset.X = -2f;
					break;
				case 14:
				case 17:
					break;
				}
				break;
			case 5:
				switch (targetFrameNumber)
				{
				case 7:
				case 8:
				case 9:
				case 10:
					frontShoulderOffset.X = -2f;
					break;
				case 15:
				case 16:
					frontShoulderOffset.X = 2f;
					break;
				}
				break;
			case 6:
				switch (targetFrameNumber)
				{
				case 7:
				case 8:
				case 9:
				case 10:
					frontShoulderOffset.X = -2f;
					break;
				case 14:
				case 15:
				case 16:
				case 17:
					frontShoulderOffset.X = 2f;
					break;
				}
				break;
			case 7:
				switch (targetFrameNumber)
				{
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
					frontShoulderOffset.X = -2f;
					break;
				case 11:
				case 12:
				case 13:
				case 14:
					frontShoulderOffset.X = -2f;
					break;
				case 18:
				case 19:
					frontShoulderOffset.X = -2f;
					break;
				case 15:
				case 16:
				case 17:
					break;
				}
				break;
			}
		}

		private Rectangle CreateCompositeFrameRect(Point pt)
		{
			return new Rectangle(pt.X * 40, pt.Y * 56, 40, 56);
		}

		private void UpdateCompositeArm(Player.CompositeArmData data, float rotation, Point frameIndex, int targetX)
		{
			if (data.enabled)
			{
				rotation = data.rotation;
				switch (data.stretch)
				{
				case Player.CompositeArmStretchAmount.Full:
					frameIndex.X = targetX;
					frameIndex.Y = 0;
					break;
				case Player.CompositeArmStretchAmount.ThreeQuarters:
					frameIndex.X = targetX;
					frameIndex.Y = 1;
					break;
				case Player.CompositeArmStretchAmount.Quarter:
					frameIndex.X = targetX;
					frameIndex.Y = 2;
					break;
				case Player.CompositeArmStretchAmount.None:
					frameIndex.X = targetX;
					frameIndex.Y = 3;
					break;
				}
			}
			else
			{
				rotation = 0f;
			}
		}
	}
}
