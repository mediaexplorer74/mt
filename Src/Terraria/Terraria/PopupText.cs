using Microsoft.Xna.Framework;
using GameManager.GameContent;
using GameManager.Localization;

namespace GameManager
{
	public class PopupText
	{
		public Vector2 position;

		public Vector2 velocity;

		public float alpha;

		public int alphaDir = 1;

		public string name;

		public int stack;

		public float scale = 1f;

		public float rotation;

		public Color color;

		public bool active;

		public int lifeTime;

		public static int activeTime = 60;

		public static int numActive;

		public bool NoStack;

		public bool coinText;

		public int coinValue;

		public static int sonarText = -1;

		public bool expert;

		public bool master;

		public bool sonar;

		public PopupTextContext context;

		public int npcNetID;

		public bool notActuallyAnItem => npcNetID != 0;

		public static float TargetScale => Main.UIScale / Main.GameViewMatrix.Zoom.X;

		public static void ClearSonarText()
		{
			if (sonarText >= 0 && Main.popupText[sonarText].sonar)
			{
				Main.popupText[sonarText].active = false;
				sonarText = -1;
			}
		}

		public static int NewText(PopupTextContext context, int npcNetID, Vector2 position, bool stay5TimesLonger)
		{
			if (!Main.showItemText)
			{
				return -1;
			}
			if (npcNetID == 0)
			{
				return -1;
			}
			if (Main.netMode == 2)
			{
				return -1;
			}
			int num = FindNextItemTextSlot();
			if (num >= 0)
			{
				NPC nPC = new NPC();
				nPC.SetDefaults(npcNetID);
				string typeName = nPC.TypeName;
				Vector2 value = FontAssets.MouseText.Value.MeasureString(typeName);
				PopupText popupText = Main.popupText[num];
				Main.popupText[num].alpha = 1f;
				popupText.alphaDir = -1;
				popupText.active = true;
				popupText.scale = 0f;
				popupText.NoStack = true;
				popupText.rotation = 0f;
				popupText.position = position - value / 2f;
				popupText.expert = false;
				popupText.master = false;
				popupText.name = typeName;
				popupText.stack = 1;
				popupText.velocity.Y = -7f;
				popupText.lifeTime = 60;
				popupText.context = context;
				if (stay5TimesLonger)
				{
					popupText.lifeTime *= 5;
				}
				popupText.npcNetID = npcNetID;
				popupText.coinValue = 0;
				popupText.coinText = false;
				popupText.color = Color.White;
				if (context == PopupTextContext.SonarAlert)
				{
					popupText.color = Color.Lerp(Color.White, Color.Crimson, 0.5f);
				}
			}
			return num;
		}

		public static int NewText(PopupTextContext context, Item newItem, int stack, bool noStack = false, bool longText = false)
		{
			if (!Main.showItemText)
			{
				return -1;
			}
			if (newItem.Name == null || !newItem.active)
			{
				return -1;
			}
			if (Main.netMode == 2)
			{
				return -1;
			}
			bool flag = newItem.type >= 71 && newItem.type <= 74;
			for (int i = 0; i < 20; i++)
			{
				if (!Main.popupText[i].active || Main.popupText[i].notActuallyAnItem || (!(Main.popupText[i].name == newItem.AffixName()) && (!flag || !Main.popupText[i].coinText)) || Main.popupText[i].NoStack || noStack)
				{
					continue;
				}
				string text = newItem.Name + " (" + (Main.popupText[i].stack + stack) + ")";
				string text2 = newItem.Name;
				if (Main.popupText[i].stack > 1)
				{
					text2 = text2 + " (" + Main.popupText[i].stack + ")";
				}
				Vector2 vector = FontAssets.MouseText.Value.MeasureString(text2);
				vector = FontAssets.MouseText.Value.MeasureString(text);
				if (Main.popupText[i].lifeTime < 0)
				{
					Main.popupText[i].scale = 1f;
				}
				if (Main.popupText[i].lifeTime < 60)
				{
					Main.popupText[i].lifeTime = 60;
				}
				if (flag && Main.popupText[i].coinText)
				{
					int num = 0;
					if (newItem.type == 71)
					{
						num += stack;
					}
					else if (newItem.type == 72)
					{
						num += 100 * stack;
					}
					else if (newItem.type == 73)
					{
						num += 10000 * stack;
					}
					else if (newItem.type == 74)
					{
						num += 1000000 * stack;
					}
					Main.popupText[i].coinValue += num;
					text = ValueToName(Main.popupText[i].coinValue);
					vector = FontAssets.MouseText.Value.MeasureString(text);
					Main.popupText[i].name = text;
					if (Main.popupText[i].coinValue >= 1000000)
					{
						if (Main.popupText[i].lifeTime < 300)
						{
							Main.popupText[i].lifeTime = 300;
						}
						Main.popupText[i].color = new Color(220, 220, 198);
					}
					else if (Main.popupText[i].coinValue >= 10000)
					{
						if (Main.popupText[i].lifeTime < 240)
						{
							Main.popupText[i].lifeTime = 240;
						}
						Main.popupText[i].color = new Color(224, 201, 92);
					}
					else if (Main.popupText[i].coinValue >= 100)
					{
						if (Main.popupText[i].lifeTime < 180)
						{
							Main.popupText[i].lifeTime = 180;
						}
						Main.popupText[i].color = new Color(181, 192, 193);
					}
					else if (Main.popupText[i].coinValue >= 1)
					{
						if (Main.popupText[i].lifeTime < 120)
						{
							Main.popupText[i].lifeTime = 120;
						}
						Main.popupText[i].color = new Color(246, 138, 96);
					}
				}
				Main.popupText[i].stack += stack;
				Main.popupText[i].scale = 0f;
				Main.popupText[i].rotation = 0f;
				Main.popupText[i].position.X = newItem.position.X + (float)newItem.width * 0.5f - vector.X * 0.5f;
				Main.popupText[i].position.Y = newItem.position.Y + (float)newItem.height * 0.25f - vector.Y * 0.5f;
				Main.popupText[i].velocity.Y = -7f;
				Main.popupText[i].context = context;
				Main.popupText[i].npcNetID = 0;
				if (Main.popupText[i].coinText)
				{
					Main.popupText[i].stack = 1;
				}
				return i;
			}
			int num2 = FindNextItemTextSlot();
			if (num2 >= 0)
			{
				string text3 = newItem.AffixName();
				if (stack > 1)
				{
					text3 = text3 + " (" + stack + ")";
				}
				Vector2 vector2 = FontAssets.MouseText.Value.MeasureString(text3);
				Main.popupText[num2].alpha = 1f;
				Main.popupText[num2].alphaDir = -1;
				Main.popupText[num2].active = true;
				Main.popupText[num2].scale = 0f;
				Main.popupText[num2].NoStack = noStack;
				Main.popupText[num2].rotation = 0f;
				Main.popupText[num2].position.X = newItem.position.X + (float)newItem.width * 0.5f - vector2.X * 0.5f;
				Main.popupText[num2].position.Y = newItem.position.Y + (float)newItem.height * 0.25f - vector2.Y * 0.5f;
				Main.popupText[num2].color = Color.White;
				Main.popupText[num2].master = false;
				if (newItem.rare == 1)
				{
					Main.popupText[num2].color = new Color(150, 150, 255);
				}
				else if (newItem.rare == 2)
				{
					Main.popupText[num2].color = new Color(150, 255, 150);
				}
				else if (newItem.rare == 3)
				{
					Main.popupText[num2].color = new Color(255, 200, 150);
				}
				else if (newItem.rare == 4)
				{
					Main.popupText[num2].color = new Color(255, 150, 150);
				}
				else if (newItem.rare == 5)
				{
					Main.popupText[num2].color = new Color(255, 150, 255);
				}
				else if (newItem.rare == -13)
				{
					Main.popupText[num2].master = true;
				}
				else if (newItem.rare == -11)
				{
					Main.popupText[num2].color = new Color(255, 175, 0);
				}
				else if (newItem.rare == -1)
				{
					Main.popupText[num2].color = new Color(130, 130, 130);
				}
				else if (newItem.rare == 6)
				{
					Main.popupText[num2].color = new Color(210, 160, 255);
				}
				else if (newItem.rare == 7)
				{
					Main.popupText[num2].color = new Color(150, 255, 10);
				}
				else if (newItem.rare == 8)
				{
					Main.popupText[num2].color = new Color(255, 255, 10);
				}
				else if (newItem.rare == 9)
				{
					Main.popupText[num2].color = new Color(5, 200, 255);
				}
				else if (newItem.rare == 10)
				{
					Main.popupText[num2].color = new Color(255, 40, 100);
				}
				else if (newItem.rare >= 11)
				{
					Main.popupText[num2].color = new Color(180, 40, 255);
				}
				Main.popupText[num2].expert = newItem.expert;
				Main.popupText[num2].name = newItem.AffixName();
				Main.popupText[num2].stack = stack;
				Main.popupText[num2].velocity.Y = -7f;
				Main.popupText[num2].lifeTime = 60;
				Main.popupText[num2].context = context;
				Main.popupText[num2].npcNetID = 0;
				if (longText)
				{
					Main.popupText[num2].lifeTime *= 5;
				}
				Main.popupText[num2].coinValue = 0;
				Main.popupText[num2].coinText = newItem.type >= 71 && newItem.type <= 74;
				if (Main.popupText[num2].coinText)
				{
					if (newItem.type == 71)
					{
						Main.popupText[num2].coinValue += Main.popupText[num2].stack;
					}
					else if (newItem.type == 72)
					{
						Main.popupText[num2].coinValue += 100 * Main.popupText[num2].stack;
					}
					else if (newItem.type == 73)
					{
						Main.popupText[num2].coinValue += 10000 * Main.popupText[num2].stack;
					}
					else if (newItem.type == 74)
					{
						Main.popupText[num2].coinValue += 1000000 * Main.popupText[num2].stack;
					}
					Main.popupText[num2].ValueToName();
					Main.popupText[num2].stack = 1;
					int num3 = num2;
					if (Main.popupText[num3].coinValue >= 1000000)
					{
						if (Main.popupText[num3].lifeTime < 300)
						{
							Main.popupText[num3].lifeTime = 300;
						}
						Main.popupText[num3].color = new Color(220, 220, 198);
					}
					else if (Main.popupText[num3].coinValue >= 10000)
					{
						if (Main.popupText[num3].lifeTime < 240)
						{
							Main.popupText[num3].lifeTime = 240;
						}
						Main.popupText[num3].color = new Color(224, 201, 92);
					}
					else if (Main.popupText[num3].coinValue >= 100)
					{
						if (Main.popupText[num3].lifeTime < 180)
						{
							Main.popupText[num3].lifeTime = 180;
						}
						Main.popupText[num3].color = new Color(181, 192, 193);
					}
					else if (Main.popupText[num3].coinValue >= 1)
					{
						if (Main.popupText[num3].lifeTime < 120)
						{
							Main.popupText[num3].lifeTime = 120;
						}
						Main.popupText[num3].color = new Color(246, 138, 96);
					}
				}
			}
			return num2;
		}

		private static int FindNextItemTextSlot()
		{
			int num = -1;
			for (int i = 0; i < 20; i++)
			{
				if (!Main.popupText[i].active)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				double num2 = Main.bottomWorld;
				for (int j = 0; j < 20; j++)
				{
					if (num2 > (double)Main.popupText[j].position.Y)
					{
						num = j;
						num2 = Main.popupText[j].position.Y;
					}
				}
			}
			return num;
		}

		private static string ValueToName(int coinValue)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			string text = "";
			int num5 = coinValue;
			while (num5 > 0)
			{
				if (num5 >= 1000000)
				{
					num5 -= 1000000;
					num++;
				}
				else if (num5 >= 10000)
				{
					num5 -= 10000;
					num2++;
				}
				else if (num5 >= 100)
				{
					num5 -= 100;
					num3++;
				}
				else if (num5 >= 1)
				{
					num5--;
					num4++;
				}
			}
			text = "";
			if (num > 0)
			{
				text = text + num + string.Format(" {0} ", Language.GetTextValue("Currency.Platinum"));
			}
			if (num2 > 0)
			{
				text = text + num2 + string.Format(" {0} ", Language.GetTextValue("Currency.Gold"));
			}
			if (num3 > 0)
			{
				text = text + num3 + string.Format(" {0} ", Language.GetTextValue("Currency.Silver"));
			}
			if (num4 > 0)
			{
				text = text + num4 + string.Format(" {0} ", Language.GetTextValue("Currency.Copper"));
			}
			if (text.Length > 1)
			{
				text = text.Substring(0, text.Length - 1);
			}
			return text;
		}

		private void ValueToName()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = coinValue;
			while (num5 > 0)
			{
				if (num5 >= 1000000)
				{
					num5 -= 1000000;
					num++;
				}
				else if (num5 >= 10000)
				{
					num5 -= 10000;
					num2++;
				}
				else if (num5 >= 100)
				{
					num5 -= 100;
					num3++;
				}
				else if (num5 >= 1)
				{
					num5--;
					num4++;
				}
			}
			name = "";
			if (num > 0)
			{
				name = name + num + string.Format(" {0} ", Language.GetTextValue("Currency.Platinum"));
			}
			if (num2 > 0)
			{
				name = name + num2 + string.Format(" {0} ", Language.GetTextValue("Currency.Gold"));
			}
			if (num3 > 0)
			{
				name = name + num3 + string.Format(" {0} ", Language.GetTextValue("Currency.Silver"));
			}
			if (num4 > 0)
			{
				name = name + num4 + string.Format(" {0} ", Language.GetTextValue("Currency.Copper"));
			}
			if (name.Length > 1)
			{
				name = name.Substring(0, name.Length - 1);
			}
		}

		public void Update(int whoAmI)
		{
			if (!active)
			{
				return;
			}
			float targetScale = TargetScale;
			alpha += (float)alphaDir * 0.01f;
			if ((double)alpha <= 0.7)
			{
				alpha = 0.7f;
				alphaDir = 1;
			}
			if (alpha >= 1f)
			{
				alpha = 1f;
				alphaDir = -1;
			}
			if (expert)
			{
				color = new Color((byte)Main.DiscoR, (byte)Main.DiscoG, (byte)Main.DiscoB, Main.mouseTextColor);
			}
			else if (master)
			{
				color = new Color(255, (int)(Main.masterColor * 200f), 0, (int)Main.mouseTextColor);
			}
			bool flag = false;
			string text = name;
			if (stack > 1)
			{
				text = text + " (" + stack + ")";
			}
			Vector2 vector = FontAssets.MouseText.Value.MeasureString(text);
			vector *= scale;
			vector.Y *= 0.8f;
			Rectangle rectangle = new Rectangle((int)(position.X - vector.X / 2f), (int)(position.Y - vector.Y / 2f), (int)vector.X, (int)vector.Y);
			for (int i = 0; i < 20; i++)
			{
				if (!Main.popupText[i].active || i == whoAmI)
				{
					continue;
				}
				string text2 = Main.popupText[i].name;
				if (Main.popupText[i].stack > 1)
				{
					text2 = text2 + " (" + Main.popupText[i].stack + ")";
				}
				Vector2 vector2 = FontAssets.MouseText.Value.MeasureString(text2);
				vector2 *= Main.popupText[i].scale;
				vector2.Y *= 0.8f;
				Rectangle value = new Rectangle((int)(Main.popupText[i].position.X - vector2.X / 2f), (int)(Main.popupText[i].position.Y - vector2.Y / 2f), (int)vector2.X, (int)vector2.Y);
				if (rectangle.Intersects(value) && (position.Y < Main.popupText[i].position.Y || (position.Y == Main.popupText[i].position.Y && whoAmI < i)))
				{
					flag = true;
					int num = numActive;
					if (num > 3)
					{
						num = 3;
					}
					Main.popupText[i].lifeTime = activeTime + 15 * num;
					lifeTime = activeTime + 15 * num;
				}
			}
			if (!flag)
			{
				velocity.Y *= 0.86f;
				if (scale == targetScale)
				{
					velocity.Y *= 0.4f;
				}
			}
			else if (velocity.Y > -6f)
			{
				velocity.Y -= 0.2f;
			}
			else
			{
				velocity.Y *= 0.86f;
			}
			velocity.X *= 0.93f;
			position += velocity;
			lifeTime--;
			if (lifeTime <= 0)
			{
				scale -= 0.03f * targetScale;
				if ((double)scale < 0.1 * (double)targetScale)
				{
					active = false;
					if (sonarText == whoAmI)
					{
						sonarText = -1;
					}
				}
				lifeTime = 0;
			}
			else
			{
				if (scale < targetScale)
				{
					scale += 0.1f * targetScale;
				}
				if (scale > targetScale)
				{
					scale = targetScale;
				}
			}
		}

		public static void UpdateItemText()
		{
			int num = 0;
			for (int i = 0; i < 20; i++)
			{
				if (Main.popupText[i].active)
				{
					num++;
					Main.popupText[i].Update(i);
				}
			}
			numActive = num;
		}
	}
}
