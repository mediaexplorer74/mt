// Decompiled with JetBrains decompiler
// Type: GameManager.CombatText
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

using Microsoft.Xna.Framework;

namespace GameManager
{
  public class CombatText
  {
    public Vector2 position;
    public Vector2 velocity;
    public float alpha;
    public int alphaDir = 1;
    public string text;
    public float scale = 1f;
    public float rotation;
    public Color color;
    public bool active;
    public int lifeTime = 0;

    public static void NewText(Rectangle location, Color color, string text)
    {
      for (int index = 0; index < 100; ++index)
      {
        if (!Game1.combatText[index].active)
        {
          Vector2 vector2 = Game1.fontCombatText.MeasureString(text);
          Game1.combatText[index].alpha = 1f;
          Game1.combatText[index].alphaDir = -1;
          Game1.combatText[index].active = true;
          Game1.combatText[index].scale = 0.0f;
          Game1.combatText[index].rotation = 0.0f;
          Game1.combatText[index].position.X = (float) ((double) location.X + (double) location.Width * 0.5 - (double) vector2.X * 0.5);
          Game1.combatText[index].position.Y = (float) ((double) location.Y + (double) location.Height * 0.25 - (double) vector2.Y * 0.5);
          Game1.combatText[index].position.X += (float) Game1.rand.Next(-(int) ((double) location.Width * 0.5), (int) ((double) location.Width * 0.5) + 1);
          Game1.combatText[index].position.Y += (float) Game1.rand.Next(-(int) ((double) location.Height * 0.5), (int) ((double) location.Height * 0.5) + 1);
          Game1.combatText[index].color = color;
          Game1.combatText[index].text = text;
          Game1.combatText[index].velocity.Y = -7f;
          Game1.combatText[index].lifeTime = 60;
          break;
        }
      }
    }

    public void Update()
    {
      if (!this.active)
        return;
      this.alpha += (float) this.alphaDir * 0.05f;
      if ((double) this.alpha <= 0.6)
      {
        this.alpha = 0.6f;
        this.alphaDir = 1;
      }
      if ((double) this.alpha >= 1.0)
      {
        this.alpha = 1f;
        this.alphaDir = -1;
      }
      this.velocity.Y *= 0.92f;
      this.velocity.X *= 0.93f;
      this.position += this.velocity;
      --this.lifeTime;
      if (this.lifeTime <= 0)
      {
        this.scale -= 0.1f;
        if ((double) this.scale < 0.1)
          this.active = false;
        this.lifeTime = 0;
      }
      else
      {
        if ((double) this.scale < 1.0)
          this.scale += 0.1f;
        if ((double) this.scale > 1.0)
          this.scale = 1f;
      }
    }

    public static void UpdateCombatText()
    {
      for (int index = 0; index < 100; ++index)
      {
        if (Game1.combatText[index].active)
          Game1.combatText[index].Update();
      }
    }
  }
}
