using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.ID;

namespace GameManager.DataStructures
{
	public struct PlayerDrawHeadSet
	{
		public List<DrawData> DrawData;

		public List<int> Dust;

		public List<int> Gore;

		public Player drawPlayer;

		public int cHead;

		public int cFace;

		public int cUnicornHorn;

		public int skinVar;

		public int hairShaderPacked;

		public int skinDyePacked;

		public float scale;

		public Color colorEyeWhites;

		public Color colorEyes;

		public Color colorHair;

		public Color colorHead;

		public Color colorArmorHead;

		public SpriteEffects playerEffect;

		public Vector2 headVect;

		public Rectangle bodyFrameMemory;

		public bool fullHair;

		public bool hatHair;

		public bool hideHair;

		public bool helmetIsTall;

		public bool helmetIsOverFullHair;

		public bool helmetIsNormal;

		public bool drawUnicornHorn;

		public Vector2 Position;

		public Vector2 helmetOffset;

		public Rectangle HairFrame
		{
			get
			{
				Rectangle result = bodyFrameMemory;
				result.Height--;
				return result;
			}
		}

		public void BoringSetup(Player drawPlayer2, List<DrawData> drawData, List<int> dust, List<int> gore, float X, float Y, float Alpha, float Scale)
		{
			DrawData = drawData;
			Dust = dust;
			Gore = gore;
			drawPlayer = drawPlayer2;
			Position = drawPlayer.position;
			cHead = 0;
			cFace = 0;
			cUnicornHorn = 0;
			drawUnicornHorn = false;
			skinVar = drawPlayer.skinVariant;
			hairShaderPacked = PlayerDrawHelper.PackShader(drawPlayer.hairDye, PlayerDrawHelper.ShaderConfiguration.HairShader);
			if (drawPlayer.head == 0 && drawPlayer.hairDye == 0)
			{
				hairShaderPacked = PlayerDrawHelper.PackShader(1, PlayerDrawHelper.ShaderConfiguration.HairShader);
			}
			skinDyePacked = drawPlayer.skinDyePacked;
			if (drawPlayer.face > 0 && drawPlayer.face < 16)
			{
				Main.instance.LoadAccFace(drawPlayer.face);
			}
			cHead = drawPlayer.cHead;
			cFace = drawPlayer.cFace;
			cUnicornHorn = drawPlayer.cUnicornHorn;
			drawUnicornHorn = drawPlayer.hasUnicornHorn;
			Main.instance.LoadHair(drawPlayer.hair);
			scale = Scale;
			colorEyeWhites = Main.quickAlpha(Color.White, Alpha);
			colorEyes = Main.quickAlpha(drawPlayer.eyeColor, Alpha);
			colorHair = Main.quickAlpha(drawPlayer.GetHairColor(useLighting: false), Alpha);
			colorHead = Main.quickAlpha(drawPlayer.skinColor, Alpha);
			colorArmorHead = Main.quickAlpha(Color.White, Alpha);
			playerEffect = SpriteEffects.None;
			if (drawPlayer.direction < 0)
			{
				playerEffect = SpriteEffects.FlipHorizontally;
			}
			headVect = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.4f);
			bodyFrameMemory = drawPlayer.bodyFrame;
			bodyFrameMemory.Y = 0;
			Position = Main.screenPosition;
			Position.X += X;
			Position.Y += Y;
			Position.X -= 6f;
			Position.Y -= 4f;
			Position.Y -= drawPlayer.HeightMapOffset;
			if (drawPlayer.head > 0 && drawPlayer.head < 266)
			{
				Main.instance.LoadArmorHead(drawPlayer.head);
				int num = ArmorIDs.Head.Sets.FrontToBackID[drawPlayer.head];
				if (num >= 0)
				{
					Main.instance.LoadArmorHead(num);
				}
			}
			if (drawPlayer.face > 0 && drawPlayer.face < 16)
			{
				Main.instance.LoadAccFace(drawPlayer.face);
			}
			helmetOffset = drawPlayer.GetHelmetDrawOffset();
			drawPlayer.GetHairSettings(out fullHair, out hatHair, out hideHair, out var _, out helmetIsOverFullHair);
			helmetIsTall = drawPlayer.head == 14 || drawPlayer.head == 56 || drawPlayer.head == 158;
			helmetIsNormal = !helmetIsTall && !helmetIsOverFullHair && drawPlayer.head > 0 && drawPlayer.head < 266 && drawPlayer.head != 28;
		}
	}
}
