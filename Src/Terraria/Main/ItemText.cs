﻿/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Microsoft.Xna.Framework;

namespace GameManager
{
    public class ItemText
    {
        public static int activeTime = 60;
        public int alphaDir = 1;
        public float scale = 1f;
        public Vector2 position;
        public Vector2 velocity;
        public float alpha;
        public string name;
        public int stack;
        public float rotation;
        public Color color;
        public bool active;
        public int lifeTime;
        public static int numActive;
        public bool NoStack;
        public bool coinText;
        public int coinValue;
        public bool expert;

        public static void NewText(Item newItem, int stack, bool noStack = false, bool longText = false)
        {
            bool flag = newItem.itemId >= 71 && newItem.itemId <= 74;
            if (!Main.showItemText || newItem.name == null || (!newItem.active || Main.netMode == 2))
                return;
            for (int index = 0; index < 20; ++index)
            {
                if (Main.itemText[index].active && (Main.itemText[index].name == newItem.AffixName() || flag && Main.itemText[index].coinText) && (!Main.itemText[index].NoStack && !noStack))
                {
                    string text1 = string.Concat(new object[4]
          {
            (object) newItem.name,
            (object) " (",
            (object) (Main.itemText[index].stack + stack),
            (object) ")"
          });
                    string text2 = newItem.name;
                    if (Main.itemText[index].stack > 1)
                        text2 = string.Concat(new object[4]
            {
              (object) text2,
              (object) " (",
              (object) Main.itemText[index].stack,
              (object) ")"
            });
                    Main.fontMouseText.MeasureString(text2);
                    Vector2 vector2 = Main.fontMouseText.MeasureString(text1);
                    if (Main.itemText[index].lifeTime < 0)
                        Main.itemText[index].scale = 1f;
                    if (Main.itemText[index].lifeTime < 60)
                        Main.itemText[index].lifeTime = 60;
                    if (flag && Main.itemText[index].coinText)
                    {
                        int num = 0;
                        if (newItem.itemId == 71)
                            num += newItem.stack;
                        else if (newItem.itemId == 72)
                            num += 100 * newItem.stack;
                        else if (newItem.itemId == 73)
                            num += 10000 * newItem.stack;
                        else if (newItem.itemId == 74)
                            num += 1000000 * newItem.stack;
                        Main.itemText[index].coinValue += num;
                        string text3 = ItemText.ValueToName(Main.itemText[index].coinValue);
                        vector2 = Main.fontMouseText.MeasureString(text3);
                        Main.itemText[index].name = text3;
                        if (Main.itemText[index].coinValue >= 1000000)
                        {
                            if (Main.itemText[index].lifeTime < 300)
                                Main.itemText[index].lifeTime = 300;
                            Main.itemText[index].color = new Color(220, 220, 198);
                        }
                        else if (Main.itemText[index].coinValue >= 10000)
                        {
                            if (Main.itemText[index].lifeTime < 240)
                                Main.itemText[index].lifeTime = 240;
                            Main.itemText[index].color = new Color(224, 201, 92);
                        }
                        else if (Main.itemText[index].coinValue >= 100)
                        {
                            if (Main.itemText[index].lifeTime < 180)
                                Main.itemText[index].lifeTime = 180;
                            Main.itemText[index].color = new Color(181, 192, 193);
                        }
                        else if (Main.itemText[index].coinValue >= 1)
                        {
                            if (Main.itemText[index].lifeTime < 120)
                                Main.itemText[index].lifeTime = 120;
                            Main.itemText[index].color = new Color(246, 138, 96);
                        }
                    }
                    Main.itemText[index].stack += stack;
                    Main.itemText[index].scale = 0.0f;
                    Main.itemText[index].rotation = 0.0f;
                    Main.itemText[index].position.X = (float)((double)newItem.position.X + (double)newItem.width * 0.5 - (double)vector2.X * 0.5);
                    Main.itemText[index].position.Y = (float)((double)newItem.position.Y + (double)newItem.height * 0.25 - (double)vector2.Y * 0.5);
                    Main.itemText[index].velocity.Y = -7f;
                    if (!Main.itemText[index].coinText)
                        return;
                    Main.itemText[index].stack = 1;
                    return;
                }
            }
            int index1 = -1;
            for (int index2 = 0; index2 < 20; ++index2)
            {
                if (!Main.itemText[index2].active)
                {
                    index1 = index2;
                    break;
                }
            }
            if (index1 == -1)
            {
                double num = (double)Main.bottomWorld;
                for (int index2 = 0; index2 < 20; ++index2)
                {
                    if (num > (double)Main.itemText[index2].position.Y)
                    {
                        index1 = index2;
                        num = (double)Main.itemText[index2].position.Y;
                    }
                }
            }
            if (index1 < 0)
                return;
            string text = newItem.AffixName();
            if (stack > 1)
                text = string.Concat(new object[4]
        {
          (object) text,
          (object) " (",
          (object) stack,
          (object) ")"
        });
            Vector2 vector2_1 = Main.fontMouseText.MeasureString(text);
            Main.itemText[index1].alpha = 1f;
            Main.itemText[index1].alphaDir = -1;
            Main.itemText[index1].active = true;
            Main.itemText[index1].scale = 0.0f;
            Main.itemText[index1].NoStack = noStack;
            Main.itemText[index1].rotation = 0.0f;
            Main.itemText[index1].position.X = (float)((double)newItem.position.X + (double)newItem.width * 0.5 - (double)vector2_1.X * 0.5);
            Main.itemText[index1].position.Y = (float)((double)newItem.position.Y + (double)newItem.height * 0.25 - (double)vector2_1.Y * 0.5);
            Main.itemText[index1].color = Color.White;
            if (newItem.rare == 1)
                Main.itemText[index1].color = new Color(150, 150, (int)byte.MaxValue);
            else if (newItem.rare == 2)
                Main.itemText[index1].color = new Color(150, (int)byte.MaxValue, 150);
            else if (newItem.rare == 3)
                Main.itemText[index1].color = new Color((int)byte.MaxValue, 200, 150);
            else if (newItem.rare == 4)
                Main.itemText[index1].color = new Color((int)byte.MaxValue, 150, 150);
            else if (newItem.rare == 5)
                Main.itemText[index1].color = new Color((int)byte.MaxValue, 150, (int)byte.MaxValue);
            else if (newItem.rare == -11)
                Main.itemText[index1].color = new Color((int)byte.MaxValue, 175, 0);
            else if (newItem.rare == -1)
                Main.itemText[index1].color = new Color(130, 130, 130);
            else if (newItem.rare == 6)
                Main.itemText[index1].color = new Color(210, 160, (int)byte.MaxValue);
            else if (newItem.rare == 7)
                Main.itemText[index1].color = new Color(150, (int)byte.MaxValue, 10);
            else if (newItem.rare == 8)
                Main.itemText[index1].color = new Color((int)byte.MaxValue, (int)byte.MaxValue, 10);
            else if (newItem.rare == 9)
                Main.itemText[index1].color = new Color(5, 200, (int)byte.MaxValue);
            else if (newItem.rare == 10)
                Main.itemText[index1].color = new Color((int)byte.MaxValue, 40, 100);
            else if (newItem.rare >= 11)
                Main.itemText[index1].color = new Color(180, 40, (int)byte.MaxValue);
            Main.itemText[index1].expert = newItem.expert;
            Main.itemText[index1].name = newItem.AffixName();
            Main.itemText[index1].stack = stack;
            Main.itemText[index1].velocity.Y = -7f;
            Main.itemText[index1].lifeTime = 60;
            if (longText)
                Main.itemText[index1].lifeTime *= 5;
            Main.itemText[index1].coinValue = 0;
            Main.itemText[index1].coinText = newItem.itemId >= 71 && newItem.itemId <= 74;
            if (!Main.itemText[index1].coinText)
                return;
            if (newItem.itemId == 71)
                Main.itemText[index1].coinValue += Main.itemText[index1].stack;
            else if (newItem.itemId == 72)
                Main.itemText[index1].coinValue += 100 * Main.itemText[index1].stack;
            else if (newItem.itemId == 73)
                Main.itemText[index1].coinValue += 10000 * Main.itemText[index1].stack;
            else if (newItem.itemId == 74)
                Main.itemText[index1].coinValue += 1000000 * Main.itemText[index1].stack;
            Main.itemText[index1].ValueToName();
            Main.itemText[index1].stack = 1;
            int index3 = index1;
            if (Main.itemText[index3].coinValue >= 1000000)
            {
                if (Main.itemText[index3].lifeTime < 300)
                    Main.itemText[index3].lifeTime = 300;
                Main.itemText[index3].color = new Color(220, 220, 198);
            }
            else if (Main.itemText[index3].coinValue >= 10000)
            {
                if (Main.itemText[index3].lifeTime < 240)
                    Main.itemText[index3].lifeTime = 240;
                Main.itemText[index3].color = new Color(224, 201, 92);
            }
            else if (Main.itemText[index3].coinValue >= 100)
            {
                if (Main.itemText[index3].lifeTime < 180)
                    Main.itemText[index3].lifeTime = 180;
                Main.itemText[index3].color = new Color(181, 192, 193);
            }
            else
            {
                if (Main.itemText[index3].coinValue < 1)
                    return;
                if (Main.itemText[index3].lifeTime < 120)
                    Main.itemText[index3].lifeTime = 120;
                Main.itemText[index3].color = new Color(246, 138, 96);
            }
        }

        private static string ValueToName(int coinValue)
        {
            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = coinValue;
            while (num5 > 0)
            {
                if (num5 >= 1000000)
                {
                    num5 -= 1000000;
                    ++num1;
                }
                else if (num5 >= 10000)
                {
                    num5 -= 10000;
                    ++num2;
                }
                else if (num5 >= 100)
                {
                    num5 -= 100;
                    ++num3;
                }
                else if (num5 >= 1)
                {
                    --num5;
                    ++num4;
                }
            }
            string str = "";
            if (num1 > 0)
                str = str + (object)num1 + " Platinum ";
            if (num2 > 0)
                str = str + (object)num2 + " Gold ";
            if (num3 > 0)
                str = str + (object)num3 + " Silver ";
            if (num4 > 0)
                str = str + (object)num4 + " Copper ";
            if (str.Length > 1)
                str = str.Substring(0, str.Length - 1);
            return str;
        }

        private void ValueToName()
        {
            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = this.coinValue;
            while (num5 > 0)
            {
                if (num5 >= 1000000)
                {
                    num5 -= 1000000;
                    ++num1;
                }
                else if (num5 >= 10000)
                {
                    num5 -= 10000;
                    ++num2;
                }
                else if (num5 >= 100)
                {
                    num5 -= 100;
                    ++num3;
                }
                else if (num5 >= 1)
                {
                    --num5;
                    ++num4;
                }
            }
            this.name = "";
            if (num1 > 0)
            {
                ItemText itemText = this;
                string str = itemText.name + (object)num1 + " Platinum ";
                itemText.name = str;
            }
            if (num2 > 0)
            {
                ItemText itemText = this;
                string str = itemText.name + (object)num2 + " Gold ";
                itemText.name = str;
            }
            if (num3 > 0)
            {
                ItemText itemText = this;
                string str = itemText.name + (object)num3 + " Silver ";
                itemText.name = str;
            }
            if (num4 > 0)
            {
                ItemText itemText = this;
                string str = itemText.name + (object)num4 + " Copper ";
                itemText.name = str;
            }
            if (this.name.Length <= 1)
                return;
            this.name = this.name.Substring(0, this.name.Length - 1);
        }

        public void Update(int whoAmI)
        {
            if (!this.active)
                return;
            this.alpha += (float)this.alphaDir * 0.01f;
            if ((double)this.alpha <= 0.7)
            {
                this.alpha = 0.7f;
                this.alphaDir = 1;
            }
            if ((double)this.alpha >= 1.0)
            {
                this.alpha = 1f;
                this.alphaDir = -1;
            }
            if (this.expert && this.expert)
                this.color = new Color((int)(byte)Main.DiscoR, (int)(byte)Main.DiscoG, (int)(byte)Main.DiscoB, (int)Main.mouseTextColor);
            bool flag = false;
            string text1 = this.name;
            if (this.stack > 1)
                text1 = string.Concat(new object[4]
        {
          (object) text1,
          (object) " (",
          (object) this.stack,
          (object) ")"
        });
            Vector2 vector2_1 = Main.fontMouseText.MeasureString(text1) * this.scale;
            vector2_1.Y *= 0.8f;
            Rectangle rectangle1 = new Rectangle((int)((double)this.position.X - (double)vector2_1.X / 2.0), (int)((double)this.position.Y - (double)vector2_1.Y / 2.0), (int)vector2_1.X, (int)vector2_1.Y);
            for (int index = 0; index < 20; ++index)
            {
                if (Main.itemText[index].active && index != whoAmI)
                {
                    string text2 = Main.itemText[index].name;
                    if (Main.itemText[index].stack > 1)
                        text2 = string.Concat(new object[4]
            {
              (object) text2,
              (object) " (",
              (object) Main.itemText[index].stack,
              (object) ")"
            });
                    Vector2 vector2_2 = Main.fontMouseText.MeasureString(text2);
                    vector2_2 *= Main.itemText[index].scale;
                    vector2_2.Y *= 0.8f;
                    Rectangle rectangle2 = new Rectangle((int)((double)Main.itemText[index].position.X - (double)vector2_2.X / 2.0), (int)((double)Main.itemText[index].position.Y - (double)vector2_2.Y / 2.0), (int)vector2_2.X, (int)vector2_2.Y);
                    if (rectangle1.Intersects(rectangle2) && ((double)this.position.Y < (double)Main.itemText[index].position.Y || (double)this.position.Y == (double)Main.itemText[index].position.Y && whoAmI < index))
                    {
                        flag = true;
                        int num = ItemText.numActive;
                        if (num > 3)
                            num = 3;
                        Main.itemText[index].lifeTime = ItemText.activeTime + 15 * num;
                        this.lifeTime = ItemText.activeTime + 15 * num;
                    }
                }
            }
            if (!flag)
            {
                this.velocity.Y *= 0.86f;
                if ((double)this.scale == 1.0)
                    this.velocity.Y *= 0.4f;
            }
            else if ((double)this.velocity.Y > -6.0)
                this.velocity.Y -= 0.2f;
            else
                this.velocity.Y *= 0.86f;
            this.velocity.X *= 0.93f;
            this.position += this.velocity;
            --this.lifeTime;
            if (this.lifeTime <= 0)
            {
                this.scale -= 0.03f;
                if ((double)this.scale < 0.1)
                    this.active = false;
                this.lifeTime = 0;
            }
            else
            {
                if ((double)this.scale < 1.0)
                    this.scale += 0.1f;
                if ((double)this.scale <= 1.0)
                    return;
                this.scale = 1f;
            }
        }

        public static void UpdateItemText()
        {
            int num = 0;
            for (int whoAmI = 0; whoAmI < 20; ++whoAmI)
            {
                if (Main.itemText[whoAmI].active)
                {
                    ++num;
                    Main.itemText[whoAmI].Update(whoAmI);
                }
            }
            ItemText.numActive = num;
        }
    }
}
