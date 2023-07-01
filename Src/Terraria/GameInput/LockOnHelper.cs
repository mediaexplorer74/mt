﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameInput.LockOnHelper
// Assembly: Terraria, Version=1.3.5.3, Culture=neutral, PublicKeyToken=null
// MVID: 68659D26-2BE6-448F-8663-74FA559E6F08
// Assembly location: H:\Steam\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.ID;

namespace Terraria.GameInput
{
  public class LockOnHelper
  {
    public static LockOnHelper.LockOnMode UseMode = LockOnHelper.LockOnMode.ThreeDS;
    private static bool _canLockOn = false;
    private static List<int> _targets = new List<int>();
    private static int _threeDSTarget = -1;
    private static float[,] _drawProgress = new float[200, 2];
    private const float LOCKON_RANGE = 2000f;
    private const int LOCKON_HOLD_LIFETIME = 40;
    private static bool _enabled;
    private static int _pickedTarget;
    private static int _lifeTimeCounter;
    private static int _lifeTimeArrowDisplay;

    public static NPC AimedTarget
    {
      get
      {
        if (LockOnHelper._pickedTarget == -1 || LockOnHelper._targets.Count < 1)
          return (NPC) null;
        return Main.npc[LockOnHelper._targets[LockOnHelper._pickedTarget]];
      }
    }

    public static Vector2 PredictedPosition
    {
      get
      {
        NPC aimedTarget = LockOnHelper.AimedTarget;
        if (aimedTarget == null)
          return Vector2.get_Zero();
        Vector2 vec = aimedTarget.Center;
        int index1;
        Vector2 pos;
        if (NPC.GetNPCLocation(LockOnHelper._targets[LockOnHelper._pickedTarget], true, false, out index1, out pos))
          vec = Vector2.op_Addition(pos, Vector2.op_Multiply(Vector2.op_Multiply(Main.npc[index1].Distance(Main.player[Main.myPlayer].Center) / 2000f, Main.npc[index1].velocity), 45f));
        Player player = Main.player[Main.myPlayer];
        for (int index2 = ItemID.Sets.LockOnAimAbove[player.inventory[player.selectedItem].type]; index2 > 0 && vec.Y > 100.0; --index2)
        {
          Point tileCoordinates = vec.ToTileCoordinates();
          __Null& local1 = @tileCoordinates.Y;
          int num1 = ^(int&) local1 - 4;
          ^(int&) local1 = num1;
          if (WorldGen.InWorld((int) tileCoordinates.X, (int) tileCoordinates.Y, 10) && !WorldGen.SolidTile((int) tileCoordinates.X, (int) tileCoordinates.Y))
          {
            __Null& local2 = @vec.Y;
            double num2 = (double) ^(float&) local2 - 16.0;
            ^(float&) local2 = (float) num2;
          }
          else
            break;
        }
        float? nullable = ItemID.Sets.LockOnAimCompensation[player.inventory[player.selectedItem].type];
        if (nullable.HasValue)
        {
          __Null& local1 = @vec.Y;
          double num1 = (double) ^(float&) local1 - (double) (aimedTarget.height / 2);
          ^(float&) local1 = (float) num1;
          Vector2 v = Vector2.op_Subtraction(vec, player.Center);
          Vector2 vector2 = v.SafeNormalize(Vector2.get_Zero());
          __Null& local2 = @vector2.Y;
          double num2 = (double) ^(float&) local2 - 1.0;
          ^(float&) local2 = (float) num2;
          float num3 = (float) Math.Pow((double) ((Vector2) @v).Length() / 700.0, 2.0) * 700f;
          __Null& local3 = @vec.Y;
          double num4 = (double) ^(float&) local3 + vector2.Y * (double) num3 * (double) nullable.Value * 1.0;
          ^(float&) local3 = (float) num4;
          __Null& local4 = @vec.X;
          double num5 = (double) ^(float&) local4 + -vector2.X * (double) num3 * (double) nullable.Value * 1.0;
          ^(float&) local4 = (float) num5;
        }
        return vec;
      }
    }

    public static bool Enabled
    {
      get
      {
        return LockOnHelper._enabled;
      }
    }

    public static void CycleUseModes()
    {
      switch (LockOnHelper.UseMode)
      {
        case LockOnHelper.LockOnMode.FocusTarget:
          LockOnHelper.UseMode = LockOnHelper.LockOnMode.TargetClosest;
          break;
        case LockOnHelper.LockOnMode.TargetClosest:
          LockOnHelper.UseMode = LockOnHelper.LockOnMode.ThreeDS;
          break;
        case LockOnHelper.LockOnMode.ThreeDS:
          LockOnHelper.UseMode = LockOnHelper.LockOnMode.TargetClosest;
          break;
      }
    }

    public static void Update()
    {
      LockOnHelper._canLockOn = false;
      if (!PlayerInput.UsingGamepad)
      {
        LockOnHelper.SetActive(false);
      }
      else
      {
        if (--LockOnHelper._lifeTimeArrowDisplay < 0)
          LockOnHelper._lifeTimeArrowDisplay = 0;
        LockOnHelper.Handle3DSTarget();
        if (PlayerInput.Triggers.JustPressed.LockOn && !PlayerInput.WritingText)
        {
          LockOnHelper._lifeTimeCounter = 40;
          LockOnHelper._lifeTimeArrowDisplay = 30;
          LockOnHelper.HandlePressing();
        }
        if (!LockOnHelper._enabled)
          return;
        if (LockOnHelper.UseMode == LockOnHelper.LockOnMode.FocusTarget && PlayerInput.Triggers.Current.LockOn)
        {
          if (LockOnHelper._lifeTimeCounter <= 0)
          {
            LockOnHelper.SetActive(false);
            return;
          }
          --LockOnHelper._lifeTimeCounter;
        }
        NPC aimedTarget = LockOnHelper.AimedTarget;
        if (!LockOnHelper.ValidTarget(aimedTarget))
          LockOnHelper.SetActive(false);
        if (LockOnHelper.UseMode == LockOnHelper.LockOnMode.TargetClosest)
        {
          LockOnHelper.SetActive(false);
          LockOnHelper.SetActive(LockOnHelper.CanEnable());
        }
        if (!LockOnHelper._enabled)
          return;
        Player p = Main.player[Main.myPlayer];
        Vector2 predictedPosition = LockOnHelper.PredictedPosition;
        bool flag = false;
        if (LockOnHelper.ShouldLockOn(p) && (ItemID.Sets.LockOnIgnoresCollision[p.inventory[p.selectedItem].type] || Collision.CanHit(p.Center, 0, 0, predictedPosition, 0, 0) || (Collision.CanHitLine(p.Center, 0, 0, predictedPosition, 0, 0) || Collision.CanHit(p.Center, 0, 0, aimedTarget.Center, 0, 0)) || Collision.CanHitLine(p.Center, 0, 0, aimedTarget.Center, 0, 0)))
          flag = true;
        if (!flag)
          return;
        LockOnHelper._canLockOn = true;
      }
    }

    public static void SetUP()
    {
      if (!LockOnHelper._canLockOn)
        return;
      NPC aimedTarget = LockOnHelper.AimedTarget;
      LockOnHelper.SetLockPosition(Main.ReverseGravitySupport(Vector2.op_Subtraction(LockOnHelper.PredictedPosition, Main.screenPosition), 0.0f));
    }

    public static void SetDOWN()
    {
      if (!LockOnHelper._canLockOn)
        return;
      LockOnHelper.ResetLockPosition();
    }

    private static bool ShouldLockOn(Player p)
    {
      return p.inventory[p.selectedItem].type != 496;
    }

    public static void Toggle(bool forceOff = false)
    {
      LockOnHelper._lifeTimeCounter = 40;
      LockOnHelper._lifeTimeArrowDisplay = 30;
      LockOnHelper.HandlePressing();
      if (!forceOff)
        return;
      LockOnHelper._enabled = false;
    }

    private static void Handle3DSTarget()
    {
      LockOnHelper._threeDSTarget = -1;
      if (LockOnHelper.UseMode != LockOnHelper.LockOnMode.ThreeDS || !PlayerInput.UsingGamepad)
        return;
      List<int> t1_1 = new List<int>();
      int t1_2 = -1;
      Utils.Swap<List<int>>(ref t1_1, ref LockOnHelper._targets);
      Utils.Swap<int>(ref t1_2, ref LockOnHelper._pickedTarget);
      LockOnHelper.RefreshTargets(Main.MouseWorld, 2000f);
      LockOnHelper.GetClosestTarget(Main.MouseWorld);
      Utils.Swap<List<int>>(ref t1_1, ref LockOnHelper._targets);
      Utils.Swap<int>(ref t1_2, ref LockOnHelper._pickedTarget);
      if (t1_2 >= 0)
        LockOnHelper._threeDSTarget = t1_1[t1_2];
      t1_1.Clear();
    }

    private static void HandlePressing()
    {
      if (LockOnHelper.UseMode == LockOnHelper.LockOnMode.TargetClosest)
        LockOnHelper.SetActive(!LockOnHelper._enabled);
      else if (LockOnHelper.UseMode == LockOnHelper.LockOnMode.ThreeDS)
      {
        if (!LockOnHelper._enabled)
          LockOnHelper.SetActive(true);
        else
          LockOnHelper.CycleTargetThreeDS();
      }
      else if (!LockOnHelper._enabled)
        LockOnHelper.SetActive(true);
      else
        LockOnHelper.CycleTargetFocus();
    }

    private static void CycleTargetFocus()
    {
      int target = LockOnHelper._targets[LockOnHelper._pickedTarget];
      LockOnHelper.RefreshTargets(Main.MouseWorld, 2000f);
      if (LockOnHelper._targets.Count < 1 || LockOnHelper._targets.Count == 1 && target == LockOnHelper._targets[0])
      {
        LockOnHelper.SetActive(false);
      }
      else
      {
        LockOnHelper._pickedTarget = 0;
        for (int index = 0; index < LockOnHelper._targets.Count; ++index)
        {
          if (LockOnHelper._targets[index] > target)
          {
            LockOnHelper._pickedTarget = index;
            break;
          }
        }
      }
    }

    private static void CycleTargetThreeDS()
    {
      int target = LockOnHelper._targets[LockOnHelper._pickedTarget];
      LockOnHelper.RefreshTargets(Main.MouseWorld, 2000f);
      LockOnHelper.GetClosestTarget(Main.MouseWorld);
      if (LockOnHelper._targets.Count >= 1 && (LockOnHelper._targets.Count != 1 || target != LockOnHelper._targets[0]) && target != LockOnHelper._targets[LockOnHelper._pickedTarget])
        return;
      LockOnHelper.SetActive(false);
    }

    private static bool CanEnable()
    {
      return !Main.player[Main.myPlayer].dead;
    }

    private static void SetActive(bool on)
    {
      if (on)
      {
        if (!LockOnHelper.CanEnable())
          return;
        LockOnHelper.RefreshTargets(Main.MouseWorld, 2000f);
        LockOnHelper.GetClosestTarget(Main.MouseWorld);
        if (LockOnHelper._pickedTarget < 0)
          return;
        LockOnHelper._enabled = true;
      }
      else
      {
        LockOnHelper._enabled = false;
        LockOnHelper._targets.Clear();
        LockOnHelper._lifeTimeCounter = 0;
      }
    }

    private static void RefreshTargets(Vector2 position, float radius)
    {
      LockOnHelper._targets.Clear();
      Rectangle rectangle = Utils.CenteredRectangle(Main.player[Main.myPlayer].Center, new Vector2(1920f, 1200f));
      Vector2 center = Main.player[Main.myPlayer].Center;
      Vector2 vector2 = Main.player[Main.myPlayer].DirectionTo(Main.MouseWorld);
      for (int index = 0; index < Main.npc.Length; ++index)
      {
        NPC n = Main.npc[index];
        // ISSUE: explicit reference operation
        if (LockOnHelper.ValidTarget(n) && (double) n.Distance(position) <= (double) radius && ((Rectangle) @rectangle).Intersects(n.Hitbox))
        {
          Vector3 subLight = Lighting.GetSubLight(n.Center);
          // ISSUE: explicit reference operation
          if ((double) ((Vector3) @subLight).Length() / 3.0 >= 0.00999999977648258 && (LockOnHelper.UseMode != LockOnHelper.LockOnMode.ThreeDS || (double) Vector2.Dot(n.DirectionFrom(center), vector2) >= 0.649999976158142))
            LockOnHelper._targets.Add(index);
        }
      }
    }

    private static void GetClosestTarget(Vector2 position)
    {
      LockOnHelper._pickedTarget = -1;
      float num1 = -1f;
      if (LockOnHelper.UseMode == LockOnHelper.LockOnMode.ThreeDS)
      {
        Vector2 center = Main.player[Main.myPlayer].Center;
        Vector2 vector2 = Main.player[Main.myPlayer].DirectionTo(Main.MouseWorld);
        for (int index = 0; index < LockOnHelper._targets.Count; ++index)
        {
          int target = LockOnHelper._targets[index];
          NPC n = Main.npc[target];
          Vector2 Source = center;
          float num2 = Vector2.Dot(n.DirectionFrom(Source), vector2);
          if (LockOnHelper.ValidTarget(n) && (LockOnHelper._pickedTarget == -1 || (double) num2 > (double) num1))
          {
            LockOnHelper._pickedTarget = index;
            num1 = num2;
          }
        }
      }
      else
      {
        for (int index = 0; index < LockOnHelper._targets.Count; ++index)
        {
          int target = LockOnHelper._targets[index];
          NPC n = Main.npc[target];
          if (LockOnHelper.ValidTarget(n) && (LockOnHelper._pickedTarget == -1 || (double) n.Distance(position) < (double) num1))
          {
            LockOnHelper._pickedTarget = index;
            num1 = n.Distance(position);
          }
        }
      }
    }

    private static bool ValidTarget(NPC n)
    {
      return n != null && n.active && (!n.dontTakeDamage && !n.friendly) && (!n.townNPC && n.life >= 1 && !n.immortal) && (n.aiStyle != 25 || (double) n.ai[0] != 0.0);
    }

    private static void SetLockPosition(Vector2 position)
    {
      PlayerInput.LockOnCachePosition();
      Main.mouseX = PlayerInput.MouseX = (int) position.X;
      Main.mouseY = PlayerInput.MouseY = (int) position.Y;
    }

    private static void ResetLockPosition()
    {
      PlayerInput.LockOnUnCachePosition();
      Main.mouseX = PlayerInput.MouseX;
      Main.mouseY = PlayerInput.MouseY;
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
      if (Main.gameMenu)
        return;
      Texture2D lockOnCursorTexture = Main.LockOnCursorTexture;
      Rectangle r1;
      // ISSUE: explicit reference operation
      ((Rectangle) @r1).\u002Ector(0, 0, lockOnCursorTexture.get_Width(), 12);
      Rectangle r2;
      // ISSUE: explicit reference operation
      ((Rectangle) @r2).\u002Ector(0, 16, lockOnCursorTexture.get_Width(), 12);
      Color color1 = Main.OurFavoriteColor.MultiplyRGBA(new Color(0.75f, 0.75f, 0.75f, 1f));
      // ISSUE: explicit reference operation
      ((Color) @color1).set_A((byte) 220);
      Color t2 = Main.OurFavoriteColor;
      // ISSUE: explicit reference operation
      ((Color) @t2).set_A((byte) 220);
      float num1 = (float) (0.939999997615814 + Math.Sin((double) Main.GlobalTime * 6.28318548202515) * 0.0599999986588955);
      t2 = Color.op_Multiply(t2, num1);
      Color t1 = Color.op_Multiply(color1, num1);
      Utils.Swap<Color>(ref t1, ref t2);
      Color color2 = t1.MultiplyRGBA(new Color(0.8f, 0.8f, 0.8f, 0.8f));
      Color color3 = t1.MultiplyRGBA(new Color(0.8f, 0.8f, 0.8f, 0.8f));
      float gravDir = Main.player[Main.myPlayer].gravDir;
      float num2 = 1f;
      float num3 = 0.1f;
      float num4 = 0.8f;
      float num5 = 1f;
      float num6 = 10f;
      float num7 = 10f;
      bool flag = false;
      for (int i = 0; i < LockOnHelper._drawProgress.GetLength(0); ++i)
      {
        int num8 = 0;
        if (LockOnHelper._pickedTarget != -1 && LockOnHelper._targets.Count > 0 && i == LockOnHelper._targets[LockOnHelper._pickedTarget])
          num8 = 2;
        else if (flag && LockOnHelper._targets.Contains(i) || LockOnHelper.UseMode == LockOnHelper.LockOnMode.ThreeDS && LockOnHelper._threeDSTarget == i)
          num8 = 1;
        LockOnHelper._drawProgress[i, 0] = MathHelper.Clamp(LockOnHelper._drawProgress[i, 0] + (num8 == 1 ? num3 : -num3), 0.0f, 1f);
        LockOnHelper._drawProgress[i, 1] = MathHelper.Clamp(LockOnHelper._drawProgress[i, 1] + (num8 == 2 ? num3 : -num3), 0.0f, 1f);
        float num9 = LockOnHelper._drawProgress[i, 0];
        if ((double) num9 > 0.0)
        {
          float num10 = (float) (1.0 - (double) num9 * (double) num9);
          Vector2 vector2 = Main.ReverseGravitySupport(Vector2.op_Subtraction(Vector2.op_Addition(Main.npc[i].Top, new Vector2(0.0f, (float) (-(double) num7 - (double) num10 * (double) num6))), Main.screenPosition), (float) Main.npc[i].height);
          spriteBatch.Draw(lockOnCursorTexture, vector2, new Rectangle?(r1), Color.op_Multiply(color2, num9), 0.0f, Vector2.op_Division(r1.Size(), 2f), Vector2.op_Division(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.op_Multiply(new Vector2(0.58f, 1f), num2), num4), 1f + num9), 2f), (SpriteEffects) 0, 0.0f);
          spriteBatch.Draw(lockOnCursorTexture, vector2, new Rectangle?(r2), Color.op_Multiply(Color.op_Multiply(color3, num9), num9), 0.0f, Vector2.op_Division(r2.Size(), 2f), Vector2.op_Division(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.op_Multiply(new Vector2(0.58f, 1f), num2), num4), 1f + num9), 2f), (SpriteEffects) 0, 0.0f);
        }
        float num11 = LockOnHelper._drawProgress[i, 1];
        if ((double) num11 > 0.0)
        {
          int num10 = Main.npc[i].width;
          if (Main.npc[i].height > num10)
            num10 = Main.npc[i].height;
          int num12 = num10 + 20;
          if ((double) num12 < 70.0)
            num5 *= (float) num12 / 70f;
          float num13 = 3f;
          Vector2 vector2_1 = Main.npc[i].Center;
          int index1;
          Vector2 pos;
          if (LockOnHelper._targets.Count >= 0 && LockOnHelper._pickedTarget >= 0 && (LockOnHelper._pickedTarget < LockOnHelper._targets.Count && i == LockOnHelper._targets[LockOnHelper._pickedTarget]) && NPC.GetNPCLocation(i, true, false, out index1, out pos))
            vector2_1 = pos;
          for (int index2 = 0; (double) index2 < (double) num13; ++index2)
          {
            float num14 = (float) (6.28318548202515 / (double) num13 * (double) index2 + (double) Main.GlobalTime * 6.28318548202515 * 0.25);
            Vector2 vector2_2 = new Vector2(0.0f, (float) (num12 / 2)).RotatedBy((double) num14, (Vector2) null);
            Vector2 vector2_3 = Main.ReverseGravitySupport(Vector2.op_Subtraction(Vector2.op_Addition(vector2_1, vector2_2), Main.screenPosition), 0.0f);
            float num15 = (float) ((double) num14 * ((double) gravDir == 1.0 ? 1.0 : -1.0) + 3.14159274101257 * ((double) gravDir == 1.0 ? 1.0 : 0.0));
            spriteBatch.Draw(lockOnCursorTexture, vector2_3, new Rectangle?(r1), Color.op_Multiply(t1, num11), num15, Vector2.op_Division(r1.Size(), 2f), Vector2.op_Division(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.op_Multiply(new Vector2(0.58f, 1f), num2), num5), 1f + num11), 2f), (SpriteEffects) 0, 0.0f);
            spriteBatch.Draw(lockOnCursorTexture, vector2_3, new Rectangle?(r2), Color.op_Multiply(Color.op_Multiply(t2, num11), num11), num15, Vector2.op_Division(r2.Size(), 2f), Vector2.op_Division(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.op_Multiply(new Vector2(0.58f, 1f), num2), num5), 1f + num11), 2f), (SpriteEffects) 0, 0.0f);
          }
        }
      }
    }

    public enum LockOnMode
    {
      FocusTarget,
      TargetClosest,
      ThreeDS,
    }
  }
}
