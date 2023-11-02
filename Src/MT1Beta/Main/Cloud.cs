// Decompiled with JetBrains decompiler
// Type: GameManager.Cloud
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

using Microsoft.Xna.Framework;
using System;

namespace GameManager
{
  public class Cloud
  {
    public Vector2 position;
    public float scale;
    public float rotation;
    public float rSpeed;
    public float sSpeed;
    public bool active = false;
    public int type = 0;
    public int width = 0;
    public int height = 0;
    private static Random rand = new Random();

    public static void resetClouds()
    {
      if (Game1.cloudLimit < 10)
        return;
      Game1.numClouds = Cloud.rand.Next(10, Game1.cloudLimit);
      Game1.windSpeed = 0.0f;
      while ((double) Game1.windSpeed == 0.0)
        Game1.windSpeed = (float) Cloud.rand.Next(-100, 101) * 0.01f;
      for (int index = 0; index < 100; ++index)
        Game1.cloud[index].active = false;
      for (int index = 0; index < Game1.numClouds; ++index)
        Cloud.addCloud();
      for (int index = 0; index < Game1.numClouds; ++index)
      {
        if ((double) Game1.windSpeed < 0.0)
          Game1.cloud[index].position.X -= (float) (Game1.screenWidth * 2);
        else
          Game1.cloud[index].position.X += (float) (Game1.screenWidth * 2);
      }
    }

    public static void addCloud()
    {
      int index1 = -1;
      for (int index2 = 0; index2 < 100; ++index2)
      {
        if (!Game1.cloud[index2].active)
        {
          index1 = index2;
          break;
        }
      }
      if (index1 < 0)
        return;
      Game1.cloud[index1].rSpeed = 0.0f;
      Game1.cloud[index1].sSpeed = 0.0f;
      Game1.cloud[index1].type = Cloud.rand.Next(4);
      Game1.cloud[index1].scale = (float) Cloud.rand.Next(8, 13) * 0.1f;
      Game1.cloud[index1].rotation = (float) Cloud.rand.Next(-10, 11) * 0.01f;
      Game1.cloud[index1].width = (int) ((double) Game1.cloudTexture[Game1.cloud[index1].type].Width * (double) Game1.cloud[index1].scale);
      Game1.cloud[index1].height = (int) ((double) Game1.cloudTexture[Game1.cloud[index1].type].Height * (double) Game1.cloud[index1].scale);
      Game1.cloud[index1].position.X = (double) Game1.windSpeed <= 0.0 ? (float) (Game1.screenWidth + Game1.cloudTexture[Game1.cloud[index1].type].Width + Cloud.rand.Next(Game1.screenWidth * 2)) : (float) (-Game1.cloud[index1].width - Game1.cloudTexture[Game1.cloud[index1].type].Width - Cloud.rand.Next(Game1.screenWidth * 2));
      Game1.cloud[index1].position.Y = (float) Cloud.rand.Next((int) ((double) -Game1.screenHeight * 0.25), (int) ((double) Game1.screenHeight * 1.25));
      Game1.cloud[index1].position.Y -= (float) Cloud.rand.Next((int) ((double) Game1.screenHeight * 0.25));
      Game1.cloud[index1].position.Y -= (float) Cloud.rand.Next((int) ((double) Game1.screenHeight * 0.25));
      Game1.cloud[index1].scale *= 2.2f - (float) (((double) Game1.cloud[index1].position.Y + (double) Game1.screenHeight * 0.25) / ((double) Game1.screenHeight * 1.5) + 0.699999988079071);
      if ((double) Game1.cloud[index1].scale > 1.4)
        Game1.cloud[index1].scale = 1.4f;
      if ((double) Game1.cloud[index1].scale < 0.6)
        Game1.cloud[index1].scale = 0.6f;
      Game1.cloud[index1].active = true;
      Rectangle rectangle1 = new Rectangle((int) Game1.cloud[index1].position.X, (int) Game1.cloud[index1].position.Y, Game1.cloud[index1].width, Game1.cloud[index1].height);
      for (int index3 = 0; index3 < 100; ++index3)
      {
        if (index1 != index3 && Game1.cloud[index3].active)
        {
          Rectangle rectangle2 = new Rectangle((int) Game1.cloud[index3].position.X, (int) Game1.cloud[index3].position.Y, Game1.cloud[index3].width, Game1.cloud[index3].height);
          if (rectangle1.Intersects(rectangle2))
            Game1.cloud[index1].active = false;
        }
      }
    }

    public object Clone() => this.MemberwiseClone();

    public static void UpdateClouds()
    {
      int num = 0;
      for (int index = 0; index < 100; ++index)
      {
        if (Game1.cloud[index].active)
        {
          Game1.cloud[index].Update();
          ++num;
        }
      }
      for (int index = 0; index < 100; ++index)
      {
        if (Game1.cloud[index].active)
        {
          if (index > 1 && (!Game1.cloud[index - 1].active || (double) Game1.cloud[index - 1].scale > (double) Game1.cloud[index].scale + 0.02))
          {
            Cloud cloud = (Cloud) Game1.cloud[index - 1].Clone();
            Game1.cloud[index - 1] = (Cloud) Game1.cloud[index].Clone();
            Game1.cloud[index] = cloud;
          }
          if (index < 99 && (!Game1.cloud[index].active || (double) Game1.cloud[index + 1].scale < (double) Game1.cloud[index].scale - 0.02))
          {
            Cloud cloud = (Cloud) Game1.cloud[index + 1].Clone();
            Game1.cloud[index + 1] = (Cloud) Game1.cloud[index].Clone();
            Game1.cloud[index] = cloud;
          }
        }
      }
      if (num >= Game1.numClouds)
        return;
      Cloud.addCloud();
    }

    public void Update()
    {
      if (Game1.gameMenu)
        this.position.X += (float) ((double) Game1.windSpeed * (double) this.scale * 3.0);
      else
        this.position.X += (Game1.windSpeed - Game1.player[Game1.myPlayer].velocity.X * 0.1f) * this.scale;
      if ((double) Game1.windSpeed > 0.0)
      {
        if ((double) this.position.X - (double) Game1.cloudTexture[this.type].Width > (double) Game1.screenWidth)
          this.active = false;
      }
      else if ((double) this.position.X + (double) this.width + (double) Game1.cloudTexture[this.type].Width < 0.0)
        this.active = false;
      this.rSpeed += (float) Cloud.rand.Next(-10, 11) * 2E-05f;
      if ((double) this.rSpeed > 0.0007)
        this.rSpeed = 0.0007f;
      if ((double) this.rSpeed < -0.0007)
        this.rSpeed = -0.0007f;
      if ((double) this.rotation > 0.05)
        this.rotation = 0.05f;
      if ((double) this.rotation < -0.05)
        this.rotation = -0.05f;
      this.sSpeed += (float) Cloud.rand.Next(-10, 11) * 2E-05f;
      if ((double) this.sSpeed > 0.0007)
        this.sSpeed = 0.0007f;
      if ((double) this.sSpeed < -0.0007)
        this.sSpeed = -0.0007f;
      if ((double) this.scale > 1.4)
        this.scale = 1.4f;
      if ((double) this.scale < 0.6)
        this.scale = 0.6f;
      this.rotation += this.rSpeed;
      this.scale += this.sSpeed;
      this.width = (int) ((double) Game1.cloudTexture[this.type].Width * (double) this.scale);
      this.height = (int) ((double) Game1.cloudTexture[this.type].Height * (double) this.scale);
    }
  }
}
