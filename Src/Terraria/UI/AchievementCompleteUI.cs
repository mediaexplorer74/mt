﻿// Decompiled with JetBrains decompiler
// Type: Terraria.UI.AchievementCompleteUI
// Assembly: Terraria, Version=1.3.5.3, Culture=neutral, PublicKeyToken=null
// MVID: 68659D26-2BE6-448F-8663-74FA559E6F08
// Assembly location: H:\Steam\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.Achievements;
using Terraria.GameInput;
using Terraria.Graphics;

namespace Terraria.UI
{
  public class AchievementCompleteUI
  {
    private static List<AchievementCompleteUI.DrawCache> caches = new List<AchievementCompleteUI.DrawCache>();
    private static Texture2D AchievementsTexture;
    private static Texture2D AchievementsTextureBorder;

    public static void LoadContent()
    {
      AchievementCompleteUI.AchievementsTexture = TextureManager.Load("Images/UI/Achievements");
      AchievementCompleteUI.AchievementsTextureBorder = TextureManager.Load("Images/UI/Achievement_Borders");
    }

    public static void Initialize()
    {
      Main.Achievements.OnAchievementCompleted += new Achievement.AchievementCompleted(AchievementCompleteUI.AddCompleted);
    }

    public static void Draw(SpriteBatch sb)
    {
      float num = (float) (Main.screenHeight - 40);
      if (PlayerInput.UsingGamepad)
        num -= 25f;
      Vector2 center;
      // ISSUE: explicit reference operation
      ((Vector2) @center).\u002Ector((float) (Main.screenWidth / 2), num);
      foreach (AchievementCompleteUI.DrawCache cach in AchievementCompleteUI.caches)
      {
        AchievementCompleteUI.DrawAchievement(sb, ref center, cach);
        if (center.Y < -100.0)
          break;
      }
    }

    public static void AddCompleted(Achievement achievement)
    {
      if (Main.netMode == 2)
        return;
      AchievementCompleteUI.caches.Add(new AchievementCompleteUI.DrawCache(achievement));
    }

    public static void Clear()
    {
      AchievementCompleteUI.caches.Clear();
    }

    public static void Update()
    {
      foreach (AchievementCompleteUI.DrawCache cach in AchievementCompleteUI.caches)
        cach.Update();
      for (int index = 0; index < AchievementCompleteUI.caches.Count; ++index)
      {
        if (AchievementCompleteUI.caches[index].TimeLeft == 0)
        {
          AchievementCompleteUI.caches.Remove(AchievementCompleteUI.caches[index]);
          --index;
        }
      }
    }

    private static void DrawAchievement(SpriteBatch sb, ref Vector2 center, AchievementCompleteUI.DrawCache ach)
    {
      float alpha = ach.Alpha;
      if ((double) alpha > 0.0)
      {
        string title = ach.Title;
        Vector2 center1 = center;
        Vector2 vector2_1 = Main.fontItemStack.MeasureString(title);
        float num1 = ach.Scale * 1.1f;
        Vector2 size = Vector2.op_Multiply(Vector2.op_Addition(vector2_1, new Vector2(58f, 10f)), num1);
        Rectangle rectangle = Utils.CenteredRectangle(center1, size);
        Vector2 mouseScreen = Main.MouseScreen;
        // ISSUE: explicit reference operation
        int num2 = ((Rectangle) @rectangle).Contains(mouseScreen.ToPoint()) ? 1 : 0;
        Color c = num2 != 0 ? Color.op_Multiply(new Color(64, 109, 164), 0.75f) : Color.op_Multiply(new Color(64, 109, 164), 0.5f);
        Utils.DrawInvBG(sb, rectangle, c);
        float num3 = num1 * 0.3f;
        Color color;
        // ISSUE: explicit reference operation
        ((Color) @color).\u002Ector((int) Main.mouseTextColor, (int) Main.mouseTextColor, (int) Main.mouseTextColor / 5, (int) Main.mouseTextColor);
        Vector2 vector2_2 = Vector2.op_Subtraction(rectangle.Right(), Vector2.op_Multiply(Vector2.op_Multiply(Vector2.get_UnitX(), num1), (float) (12.0 + (double) num3 * (double) (float) ach.Frame.Width)));
        sb.Draw(AchievementCompleteUI.AchievementsTexture, vector2_2, new Rectangle?(ach.Frame), Color.op_Multiply(Color.get_White(), alpha), 0.0f, new Vector2(0.0f, (float) (ach.Frame.Height / 2)), num3, (SpriteEffects) 0, 0.0f);
        sb.Draw(AchievementCompleteUI.AchievementsTextureBorder, vector2_2, new Rectangle?(), Color.op_Multiply(Color.get_White(), alpha), 0.0f, new Vector2(0.0f, (float) (ach.Frame.Height / 2)), num3, (SpriteEffects) 0, 0.0f);
        Utils.DrawBorderString(sb, title, Vector2.op_Subtraction(vector2_2, Vector2.op_Multiply(Vector2.get_UnitX(), 10f)), Color.op_Multiply(color, alpha), num1 * 0.9f, 1f, 0.4f, -1);
        if (num2 != 0 && !PlayerInput.IgnoreMouseInterface)
        {
          Main.player[Main.myPlayer].mouseInterface = true;
          if (Main.mouseLeft && Main.mouseLeftRelease)
          {
            IngameFancyUI.OpenAchievementsAndGoto(ach.theAchievement);
            ach.TimeLeft = 0;
          }
        }
      }
      ach.ApplyHeight(ref center);
    }

    public class DrawCache
    {
      public Achievement theAchievement;
      private const int _iconSize = 64;
      private const int _iconSizeWithSpace = 66;
      private const int _iconsPerRow = 8;
      public int IconIndex;
      public Rectangle Frame;
      public string Title;
      public int TimeLeft;

      public float Scale
      {
        get
        {
          if (this.TimeLeft < 30)
            return MathHelper.Lerp(0.0f, 1f, (float) this.TimeLeft / 30f);
          if (this.TimeLeft > 285)
            return MathHelper.Lerp(1f, 0.0f, (float) (((double) this.TimeLeft - 285.0) / 15.0));
          return 1f;
        }
      }

      public float Alpha
      {
        get
        {
          float scale = this.Scale;
          if ((double) scale <= 0.5)
            return 0.0f;
          return (float) (((double) scale - 0.5) / 0.5);
        }
      }

      public DrawCache(Achievement achievement)
      {
        this.theAchievement = achievement;
        this.Title = achievement.FriendlyName.Value;
        int iconIndex = Main.Achievements.GetIconIndex(achievement.Name);
        this.IconIndex = iconIndex;
        this.Frame = new Rectangle(iconIndex % 8 * 66, iconIndex / 8 * 66, 64, 64);
        this.TimeLeft = 300;
      }

      public void Update()
      {
        this.TimeLeft = this.TimeLeft - 1;
        if (this.TimeLeft >= 0)
          return;
        this.TimeLeft = 0;
      }

      public void ApplyHeight(ref Vector2 v)
      {
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        __Null& local = @v.Y;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        double num = (double) ^(float&) local - 50.0 * (double) this.Alpha;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(float&) local = (float) num;
      }
    }
  }
}
