// Decompiled with JetBrains decompiler
// Type: GameManager.Projectile
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

using Microsoft.Xna.Framework;
using System;

namespace GameManager
{
  public class Projectile
  {
    public bool wet;
    public byte wetCount = 0;
    public bool lavaWet;
    public int whoAmI;
    public static int maxAI = 2;
    public Vector2 position;
    public Vector2 velocity;
    public int width;
    public int height;
    public float scale = 1f;
    public float rotation = 0.0f;
    public int type = 0;
    public int alpha;
    public int owner = 8;
    public bool active = false;
    public string name = "";
    public float[] ai = new float[Projectile.maxAI];
    public int aiStyle;
    public int timeLeft = 0;
    public int soundDelay = 0;
    public int damage = 0;
    public int direction;
    public bool hostile;
    public float knockBack = 0.0f;
    public bool friendly = false;
    public int penetrate = 1;
    public int identity = 0;
    public float light = 0.0f;
    public bool netUpdate = false;
    public int restrikeDelay = 0;
    public bool tileCollide;
    public int maxUpdates = 0;
    public int numUpdates = 0;
    public bool ignoreWater;
    public int[] playerImmune = new int[8];

    public void SetDefaults(int Type)
    {
      for (int index = 0; index < Projectile.maxAI; ++index)
        this.ai[index] = 0.0f;
      for (int index = 0; index < 8; ++index)
        this.playerImmune[index] = 0;
      this.lavaWet = false;
      this.wetCount = (byte) 0;
      this.wet = false;
      this.ignoreWater = false;
      this.hostile = false;
      this.netUpdate = false;
      this.numUpdates = 0;
      this.maxUpdates = 0;
      this.identity = 0;
      this.restrikeDelay = 0;
      this.light = 0.0f;
      this.penetrate = 1;
      this.tileCollide = true;
      this.position = new Vector2();
      this.velocity = new Vector2();
      this.aiStyle = 0;
      this.alpha = 0;
      this.type = Type;
      this.active = true;
      this.rotation = 0.0f;
      this.scale = 1f;
      this.owner = 8;
      this.timeLeft = 3600;
      this.name = "";
      this.friendly = false;
      this.damage = 0;
      this.knockBack = 0.0f;
      if (this.type == 1)
      {
        this.name = "Wooden Arrow";
        this.width = 10;
        this.height = 10;
        this.aiStyle = 1;
        this.friendly = true;
      }
      else if (this.type == 2)
      {
        this.name = "Fire Arrow";
        this.width = 10;
        this.height = 10;
        this.aiStyle = 1;
        this.friendly = true;
        this.light = 1f;
      }
      else if (this.type == 3)
      {
        this.name = "Shuriken";
        this.width = 22;
        this.height = 22;
        this.aiStyle = 2;
        this.friendly = true;
        this.penetrate = 4;
      }
      else if (this.type == 4)
      {
        this.name = "Unholy Arrow";
        this.width = 10;
        this.height = 10;
        this.aiStyle = 1;
        this.friendly = true;
        this.light = 0.2f;
        this.penetrate = 3;
      }
      else if (this.type == 5)
      {
        this.name = "Jester's Arrow";
        this.width = 10;
        this.height = 10;
        this.aiStyle = 1;
        this.friendly = true;
        this.light = 0.4f;
        this.penetrate = -1;
        this.timeLeft = 40;
        this.alpha = 100;
        this.ignoreWater = true;
      }
      else if (this.type == 6)
      {
        this.name = "Enchanted Boomerang";
        this.width = 22;
        this.height = 22;
        this.aiStyle = 3;
        this.friendly = true;
        this.penetrate = -1;
      }
      else if (this.type == 7 || this.type == 8)
      {
        this.name = "Vilethorn";
        this.width = 28;
        this.height = 28;
        this.aiStyle = 4;
        this.friendly = true;
        this.penetrate = -1;
        this.tileCollide = false;
        this.alpha = (int) byte.MaxValue;
        this.ignoreWater = true;
      }
      else if (this.type == 9)
      {
        this.name = "Starfury";
        this.width = 24;
        this.height = 24;
        this.aiStyle = 5;
        this.friendly = true;
        this.penetrate = 2;
        this.alpha = 50;
        this.scale = 0.8f;
        this.light = 1f;
      }
      else if (this.type == 10)
      {
        this.name = "Purification Powder";
        this.width = 64;
        this.height = 64;
        this.aiStyle = 6;
        this.friendly = true;
        this.tileCollide = false;
        this.penetrate = -1;
        this.alpha = (int) byte.MaxValue;
        this.ignoreWater = true;
      }
      else if (this.type == 11)
      {
        this.name = "Vile Powder";
        this.width = 48;
        this.height = 48;
        this.aiStyle = 6;
        this.friendly = true;
        this.tileCollide = false;
        this.penetrate = -1;
        this.alpha = (int) byte.MaxValue;
        this.ignoreWater = true;
      }
      else if (this.type == 12)
      {
        this.name = "Fallen Star";
        this.width = 16;
        this.height = 16;
        this.aiStyle = 5;
        this.friendly = true;
        this.penetrate = 2;
        this.alpha = 50;
        this.light = 1f;
      }
      else if (this.type == 13)
      {
        this.name = "Hook";
        this.width = 18;
        this.height = 18;
        this.aiStyle = 7;
        this.friendly = true;
        this.penetrate = -1;
        this.tileCollide = false;
      }
      else if (this.type == 14)
      {
        this.name = "Musket Ball";
        this.width = 4;
        this.height = 4;
        this.aiStyle = 1;
        this.friendly = true;
        this.penetrate = 1;
        this.light = 0.5f;
        this.alpha = (int) byte.MaxValue;
        this.maxUpdates = 1;
        this.scale = 1.2f;
        this.timeLeft = 600;
      }
      else if (this.type == 15)
      {
        this.name = "Ball of Fire";
        this.width = 16;
        this.height = 16;
        this.aiStyle = 8;
        this.friendly = true;
        this.light = 0.8f;
        this.alpha = 100;
      }
      else if (this.type == 16)
      {
        this.name = "Magic Missile";
        this.width = 10;
        this.height = 10;
        this.aiStyle = 9;
        this.friendly = true;
        this.light = 0.8f;
        this.alpha = 100;
      }
      else if (this.type == 17)
      {
        this.name = "Dirt Ball";
        this.width = 10;
        this.height = 10;
        this.aiStyle = 10;
        this.friendly = true;
      }
      else if (this.type == 18)
      {
        this.name = "Orb of Light";
        this.width = 32;
        this.height = 32;
        this.aiStyle = 11;
        this.friendly = true;
        this.light = 1f;
        this.alpha = 150;
        this.tileCollide = false;
        this.penetrate = -1;
        this.timeLeft *= 5;
        this.ignoreWater = true;
      }
      else if (this.type == 19)
      {
        this.name = "Flamarang";
        this.width = 22;
        this.height = 22;
        this.aiStyle = 3;
        this.friendly = true;
        this.penetrate = -1;
        this.light = 1f;
      }
      else if (this.type == 20)
      {
        this.name = "Green Laser";
        this.width = 4;
        this.height = 4;
        this.aiStyle = 1;
        this.friendly = true;
        this.penetrate = -1;
        this.light = 0.75f;
        this.alpha = (int) byte.MaxValue;
        this.maxUpdates = 2;
        this.scale = 1.4f;
        this.timeLeft = 600;
      }
      else if (this.type == 21)
      {
        this.name = "Bone";
        this.width = 16;
        this.height = 16;
        this.aiStyle = 2;
        this.scale = 1.2f;
        this.friendly = true;
      }
      else if (this.type == 22)
      {
        this.name = "Water Stream";
        this.width = 12;
        this.height = 12;
        this.aiStyle = 12;
        this.friendly = true;
        this.alpha = (int) byte.MaxValue;
        this.penetrate = -1;
        this.maxUpdates = 1;
        this.ignoreWater = true;
      }
      else if (this.type == 23)
      {
        this.name = "Harpoon";
        this.width = 4;
        this.height = 4;
        this.aiStyle = 13;
        this.friendly = true;
        this.penetrate = -1;
        this.alpha = (int) byte.MaxValue;
      }
      else if (this.type == 24)
      {
        this.name = "Spiky Ball";
        this.width = 14;
        this.height = 14;
        this.aiStyle = 14;
        this.friendly = true;
        this.penetrate = 3;
      }
      else if (this.type == 25)
      {
        this.name = "Ball 'O Hurt";
        this.width = 22;
        this.height = 22;
        this.aiStyle = 15;
        this.friendly = true;
        this.penetrate = -1;
      }
      else if (this.type == 26)
      {
        this.name = "Blue Moon";
        this.width = 22;
        this.height = 22;
        this.aiStyle = 15;
        this.friendly = true;
        this.penetrate = -1;
      }
      else if (this.type == 27)
      {
        this.name = "Water Bolt";
        this.width = 16;
        this.height = 16;
        this.aiStyle = 8;
        this.friendly = true;
        this.light = 0.8f;
        this.alpha = 200;
        this.timeLeft /= 2;
        this.penetrate = 10;
      }
      else if (this.type == 28)
      {
        this.name = "Bomb";
        this.width = 22;
        this.height = 22;
        this.aiStyle = 16;
        this.friendly = true;
        this.penetrate = -1;
      }
      else if (this.type == 29)
      {
        this.name = "Dynamite";
        this.width = 10;
        this.height = 10;
        this.aiStyle = 16;
        this.friendly = true;
        this.penetrate = -1;
      }
      else if (this.type == 30)
      {
        this.name = "Grenade";
        this.width = 14;
        this.height = 14;
        this.aiStyle = 16;
        this.friendly = true;
        this.penetrate = -1;
      }
      else if (this.type == 31)
      {
        this.name = "Sand Ball";
        this.knockBack = 6f;
        this.width = 10;
        this.height = 10;
        this.aiStyle = 10;
        this.friendly = true;
        this.hostile = true;
      }
      else if (this.type == 32)
      {
        this.name = "Ivy Whip";
        this.width = 18;
        this.height = 18;
        this.aiStyle = 7;
        this.friendly = true;
        this.penetrate = -1;
        this.tileCollide = false;
      }
      else if (this.type == 33)
      {
        this.name = "Thorn Chakrum";
        this.width = 28;
        this.height = 28;
        this.aiStyle = 3;
        this.friendly = true;
        this.scale = 0.9f;
        this.penetrate = -1;
      }
      else
        this.active = false;
      this.width = (int) ((double) this.width * (double) this.scale);
      this.height = (int) ((double) this.height * (double) this.scale);
    }

    public static int NewProjectile(
      float X,
      float Y,
      float SpeedX,
      float SpeedY,
      int Type,
      int Damage,
      float KnockBack,
      int Owner = 8)
    {
      int number = 1000;
      for (int index = 0; index < 1000; ++index)
      {
        if (!Game1.projectile[index].active)
        {
          number = index;
          break;
        }
      }
      if (number == 1000)
        return number;
      Game1.projectile[number].SetDefaults(Type);
      Game1.projectile[number].position.X = X - (float) Game1.projectile[number].width * 0.5f;
      Game1.projectile[number].position.Y = Y - (float) Game1.projectile[number].height * 0.5f;
      Game1.projectile[number].owner = Owner;
      Game1.projectile[number].velocity.X = SpeedX;
      Game1.projectile[number].velocity.Y = SpeedY;
      Game1.projectile[number].damage = Damage;
      Game1.projectile[number].knockBack = KnockBack;
      Game1.projectile[number].identity = number;
      Game1.projectile[number].wet = Collision.WetCollision(Game1.projectile[number].position, Game1.projectile[number].width, Game1.projectile[number].height);
      if (Game1.netMode != 0 && Owner == Game1.myPlayer)
        NetMessage.SendData(27, number: number);
      if (Owner == Game1.myPlayer)
      {
        if (Type == 28)
          Game1.projectile[number].timeLeft = 180;
        if (Type == 29)
          Game1.projectile[number].timeLeft = 300;
        if (Type == 30)
          Game1.projectile[number].timeLeft = 180;
      }
      return number;
    }

    public void Update(int i)
    {
      if (!this.active)
        return;
      Vector2 vector2 = this.velocity;
      if ((double) this.position.X <= (double) Game1.leftWorld || (double) this.position.X + (double) this.width >= (double) Game1.rightWorld || (double) this.position.Y <= (double) Game1.topWorld || (double) this.position.Y + (double) this.height >= (double) Game1.bottomWorld)
      {
        this.Kill();
      }
      else
      {
        this.whoAmI = i;
        if (this.soundDelay > 0)
          --this.soundDelay;
        this.netUpdate = false;
        for (int index = 0; index < 8; ++index)
        {
          if (this.playerImmune[index] > 0)
            --this.playerImmune[index];
        }
        this.AI();
        if (this.owner < 8 && !Game1.player[this.owner].active)
          this.Kill();
        if (!this.ignoreWater)
        {
          bool flag = Collision.LavaCollision(this.position, this.width, this.height);
          if (flag)
            this.lavaWet = true;
          if (Collision.WetCollision(this.position, this.width, this.height))
          {
            if (this.wetCount == (byte) 0)
            {
              this.wetCount = (byte) 10;
              if (!this.wet)
              {
                if (!flag)
                {
                  for (int index1 = 0; index1 < 10; ++index1)
                  {
                    int index2 = Dust.NewDust(new Vector2(this.position.X - 6f, (float) ((double) this.position.Y + (double) (this.height / 2) - 8.0)), this.width + 12, 24, 33);
                    Game1.dust[index2].velocity.Y -= 4f;
                    Game1.dust[index2].velocity.X *= 2.5f;
                    Game1.dust[index2].scale = 1.3f;
                    Game1.dust[index2].alpha = 100;
                    Game1.dust[index2].noGravity = true;
                  }
                  Game1.PlaySound(19, (int) this.position.X, (int) this.position.Y);
                }
                else
                {
                  for (int index3 = 0; index3 < 10; ++index3)
                  {
                    int index4 = Dust.NewDust(new Vector2(this.position.X - 6f, (float) ((double) this.position.Y + (double) (this.height / 2) - 8.0)), this.width + 12, 24, 35);
                    Game1.dust[index4].velocity.Y -= 1.5f;
                    Game1.dust[index4].velocity.X *= 2.5f;
                    Game1.dust[index4].scale = 1.3f;
                    Game1.dust[index4].alpha = 100;
                    Game1.dust[index4].noGravity = true;
                  }
                  Game1.PlaySound(19, (int) this.position.X, (int) this.position.Y);
                }
              }
              this.wet = true;
            }
          }
          else if (this.wet)
          {
            this.wet = false;
            if (this.wetCount == (byte) 0)
            {
              this.wetCount = (byte) 10;
              if (!this.lavaWet)
              {
                for (int index5 = 0; index5 < 10; ++index5)
                {
                  int index6 = Dust.NewDust(new Vector2(this.position.X - 6f, this.position.Y + (float) (this.height / 2)), this.width + 12, 24, 33);
                  Game1.dust[index6].velocity.Y -= 4f;
                  Game1.dust[index6].velocity.X *= 2.5f;
                  Game1.dust[index6].scale = 1.3f;
                  Game1.dust[index6].alpha = 100;
                  Game1.dust[index6].noGravity = true;
                }
                Game1.PlaySound(19, (int) this.position.X, (int) this.position.Y);
              }
              else
              {
                for (int index7 = 0; index7 < 10; ++index7)
                {
                  int index8 = Dust.NewDust(new Vector2(this.position.X - 6f, (float) ((double) this.position.Y + (double) (this.height / 2) - 8.0)), this.width + 12, 24, 35);
                  Game1.dust[index8].velocity.Y -= 1.5f;
                  Game1.dust[index8].velocity.X *= 2.5f;
                  Game1.dust[index8].scale = 1.3f;
                  Game1.dust[index8].alpha = 100;
                  Game1.dust[index8].noGravity = true;
                }
                Game1.PlaySound(19, (int) this.position.X, (int) this.position.Y);
              }
            }
          }
          if (!this.wet)
            this.lavaWet = false;
          if (this.wetCount > (byte) 0)
            --this.wetCount;
        }
        if (this.tileCollide)
        {
          Vector2 velocity1 = this.velocity;
          bool flag = true;
          if (this.type == 9 || this.type == 12 || this.type == 15 || this.type == 13 || this.type == 31)
            flag = false;
          if (this.aiStyle == 10)
            this.velocity = Collision.AnyCollision(this.position, this.velocity, this.width, this.height);
          else if (this.wet)
          {
            Vector2 velocity2 = this.velocity;
            this.velocity = Collision.TileCollision(this.position, this.velocity, this.width, this.height, flag, flag);
            vector2 = this.velocity * 0.5f;
            if ((double) this.velocity.X != (double) velocity2.X)
              vector2.X = this.velocity.X;
            if ((double) this.velocity.Y != (double) velocity2.Y)
              vector2.Y = this.velocity.Y;
          }
          else
            this.velocity = Collision.TileCollision(this.position, this.velocity, this.width, this.height, flag, flag);
          if (velocity1 != this.velocity)
          {
            if (this.aiStyle == 3 || this.aiStyle == 13 || this.aiStyle == 15)
            {
              Collision.HitTiles(this.position, this.velocity, this.width, this.height);
              if (this.type == 33)
              {
                if ((double) this.velocity.X != (double) velocity1.X)
                  this.velocity.X = -velocity1.X;
                if ((double) this.velocity.Y != (double) velocity1.Y)
                  this.velocity.Y = -velocity1.Y;
              }
              else
              {
                this.ai[0] = 1f;
                if (this.aiStyle == 3)
                {
                  this.velocity.X = -velocity1.X;
                  this.velocity.Y = -velocity1.Y;
                }
              }
              this.netUpdate = true;
              Game1.PlaySound(0, (int) this.position.X, (int) this.position.Y);
            }
            else if (this.aiStyle == 8)
            {
              Game1.PlaySound(2, (int) this.position.X, (int) this.position.Y, 10);
              ++this.ai[0];
              if ((double) this.ai[0] >= 5.0)
              {
                this.position += this.velocity;
                this.Kill();
              }
              else
              {
                if ((double) this.velocity.Y != (double) velocity1.Y)
                  this.velocity.Y = -velocity1.Y;
                if ((double) this.velocity.X != (double) velocity1.X)
                  this.velocity.X = -velocity1.X;
              }
            }
            else if (this.aiStyle == 14)
            {
              if ((double) this.velocity.X != (double) velocity1.X)
                this.velocity.X = velocity1.X * -0.5f;
              if ((double) this.velocity.Y != (double) velocity1.Y && (double) velocity1.Y > 1.0)
                this.velocity.Y = velocity1.Y * -0.5f;
            }
            else if (this.aiStyle == 16)
            {
              if ((double) this.velocity.X != (double) velocity1.X)
              {
                this.velocity.X = velocity1.X * -0.4f;
                if (this.type == 29)
                  this.velocity.X *= 0.8f;
              }
              if ((double) this.velocity.Y != (double) velocity1.Y && (double) velocity1.Y > 0.7)
              {
                this.velocity.Y = velocity1.Y * -0.4f;
                if (this.type == 29)
                  this.velocity.Y *= 0.8f;
              }
            }
            else
            {
              this.position += this.velocity;
              this.Kill();
            }
          }
        }
        if (this.type != 7 && this.type != 8)
        {
          if (this.wet)
            this.position += vector2;
          else
            this.position += this.velocity;
        }
        if ((this.aiStyle != 3 || (double) this.ai[0] != 1.0) && (this.aiStyle != 7 || (double) this.ai[0] != 1.0) && (this.aiStyle != 13 || (double) this.ai[0] != 1.0) && (this.aiStyle != 15 || (double) this.ai[0] != 1.0))
          this.direction = (double) this.velocity.X >= 0.0 ? 1 : -1;
        if (!this.active)
          return;
        if ((double) this.light > 0.0)
          Lighting.addLight((int) (((double) this.position.X + (double) (this.width / 2)) / 16.0), (int) (((double) this.position.Y + (double) (this.height / 2)) / 16.0), this.light);
        if (this.type == 2)
          Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, Alpha: 100);
        else if (this.type == 4)
        {
          if (Game1.rand.Next(5) == 0)
            Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 14, Alpha: 150, Scale: 1.1f);
        }
        else if (this.type == 5)
          Dust.NewDust(this.position, this.width, this.height, 15, this.velocity.X * 0.5f, this.velocity.Y * 0.5f, 150, Scale: 1.2f);
        Rectangle rectangle1 = new Rectangle((int) this.position.X, (int) this.position.Y, this.width, this.height);
        Rectangle rectangle2;
        if (this.hostile && Game1.myPlayer < 8 && this.damage > 0)
        {
          int player = Game1.myPlayer;
          if (Game1.player[player].active && !Game1.player[player].dead && !Game1.player[player].immune)
          {
            rectangle2 = new Rectangle((int) Game1.player[player].position.X, (int) Game1.player[player].position.Y, Game1.player[player].width, Game1.player[player].height);
            if (rectangle1.Intersects(rectangle2))
            {
              int direction = this.direction;
              int hitDirection = (double) Game1.player[player].position.X + (double) (Game1.player[player].width / 2) >= (double) this.position.X + (double) (this.width / 2) ? 1 : -1;
              Game1.player[player].Hurt(this.damage * 2, hitDirection);
              if (Game1.netMode != 0)
                NetMessage.SendData(26, number: player, number2: (float) this.direction, number3: (float) (this.damage * 2));
            }
          }
        }
        if (this.friendly && this.type != 18 && this.owner == Game1.myPlayer)
        {
          if (this.aiStyle == 16 && (double) this.ai[1] > 0.0)
          {
            for (int number = 0; number < 8; ++number)
            {
              if (Game1.player[number].active && !Game1.player[number].dead && !Game1.player[number].immune)
              {
                rectangle2 = new Rectangle((int) Game1.player[number].position.X, (int) Game1.player[number].position.Y, Game1.player[number].width, Game1.player[number].height);
                if (rectangle1.Intersects(rectangle2))
                {
                  this.direction = (double) Game1.player[number].position.X + (double) (Game1.player[number].width / 2) >= (double) this.position.X + (double) (this.width / 2) ? 1 : -1;
                  Game1.player[number].Hurt(this.damage, this.direction, true);
                  if (Game1.netMode != 0)
                    NetMessage.SendData(26, number: number, number2: (float) this.direction, number3: (float) this.damage, number4: 1f);
                }
              }
            }
          }
          int num1 = (int) ((double) this.position.X / 16.0);
          int num2 = (int) (((double) this.position.X + (double) this.width) / 16.0) + 1;
          int num3 = (int) ((double) this.position.Y / 16.0);
          int num4 = (int) (((double) this.position.Y + (double) this.height) / 16.0) + 1;
          if (num1 < 0)
            num1 = 0;
          if (num2 > Game1.maxTilesX)
            num2 = Game1.maxTilesX;
          if (num3 < 0)
            num3 = 0;
          if (num4 > Game1.maxTilesY)
            num4 = Game1.maxTilesY;
          for (int index9 = num1; index9 < num2; ++index9)
          {
            for (int index10 = num3; index10 < num4; ++index10)
            {
              if (Game1.tile[index9, index10] != null && (Game1.tile[index9, index10].type == (byte) 3 || Game1.tile[index9, index10].type == (byte) 24 || Game1.tile[index9, index10].type == (byte) 28 || Game1.tile[index9, index10].type == (byte) 32 || Game1.tile[index9, index10].type == (byte) 51 || Game1.tile[index9, index10].type == (byte) 52 || Game1.tile[index9, index10].type == (byte) 61 || Game1.tile[index9, index10].type == (byte) 62 || Game1.tile[index9, index10].type == (byte) 69 || Game1.tile[index9, index10].type == (byte) 71 || Game1.tile[index9, index10].type == (byte) 73 || Game1.tile[index9, index10].type == (byte) 74))
              {
                WorldGen.KillTile(index9, index10);
                if (Game1.netMode == 1)
                  NetMessage.SendData(17, number2: (float) index9, number3: (float) index10);
              }
            }
          }
          if (this.damage > 0)
          {
            for (int number = 0; number < 1000; ++number)
            {
              if (Game1.npc[number].active && !Game1.npc[number].friendly && (this.owner < 0 || Game1.npc[number].immune[this.owner] == 0))
              {
                Rectangle rectangle3 = new Rectangle((int) Game1.npc[number].position.X, (int) Game1.npc[number].position.Y, Game1.npc[number].width, Game1.npc[number].height);
                if (rectangle1.Intersects(rectangle3))
                {
                  if (this.aiStyle == 3)
                  {
                    if ((double) this.ai[0] == 0.0)
                    {
                      this.velocity.X = -this.velocity.X;
                      this.velocity.Y = -this.velocity.Y;
                      this.netUpdate = true;
                    }
                    this.ai[0] = 1f;
                  }
                  else if (this.aiStyle == 16)
                  {
                    if (this.timeLeft > 3)
                      this.timeLeft = 3;
                    this.direction = (double) Game1.npc[number].position.X + (double) (Game1.npc[number].width / 2) >= (double) this.position.X + (double) (this.width / 2) ? 1 : -1;
                  }
                  Game1.npc[number].StrikeNPC(this.damage, this.knockBack, this.direction);
                  if (Game1.netMode != 0)
                    NetMessage.SendData(28, number: number, number2: (float) this.damage, number3: this.knockBack, number4: (float) this.direction);
                  if (this.penetrate != 1)
                    Game1.npc[number].immune[this.owner] = 10;
                  if (this.penetrate > 0)
                  {
                    --this.penetrate;
                    if (this.penetrate == 0)
                      break;
                  }
                  if (this.aiStyle == 7)
                  {
                    this.ai[0] = 1f;
                    this.damage = 0;
                    this.netUpdate = true;
                  }
                  else if (this.aiStyle == 13)
                  {
                    this.ai[0] = 1f;
                    this.netUpdate = true;
                  }
                }
              }
            }
          }
          if (this.damage > 0 && Game1.player[Game1.myPlayer].hostile)
          {
            for (int number = 0; number < 8; ++number)
            {
              if (number != this.owner && Game1.player[number].active && !Game1.player[number].dead && !Game1.player[number].immune && Game1.player[number].hostile && this.playerImmune[number] <= 0 && (Game1.player[Game1.myPlayer].team == 0 || Game1.player[Game1.myPlayer].team != Game1.player[number].team))
              {
                rectangle2 = new Rectangle((int) Game1.player[number].position.X, (int) Game1.player[number].position.Y, Game1.player[number].width, Game1.player[number].height);
                if (rectangle1.Intersects(rectangle2))
                {
                  if (this.aiStyle == 3)
                  {
                    if ((double) this.ai[0] == 0.0)
                    {
                      this.velocity.X = -this.velocity.X;
                      this.velocity.Y = -this.velocity.Y;
                      this.netUpdate = true;
                    }
                    this.ai[0] = 1f;
                  }
                  else if (this.aiStyle == 16)
                  {
                    if (this.timeLeft > 3)
                      this.timeLeft = 3;
                    this.direction = (double) Game1.player[number].position.X + (double) (Game1.player[number].width / 2) >= (double) this.position.X + (double) (this.width / 2) ? 1 : -1;
                  }
                  Game1.player[number].Hurt(this.damage, this.direction, true);
                  if (Game1.netMode != 0)
                    NetMessage.SendData(26, number: number, number2: (float) this.direction, number3: (float) this.damage, number4: 1f);
                  this.playerImmune[number] = 40;
                  if (this.penetrate > 0)
                  {
                    --this.penetrate;
                    if (this.penetrate == 0)
                      break;
                  }
                  if (this.aiStyle == 7)
                  {
                    this.ai[0] = 1f;
                    this.damage = 0;
                    this.netUpdate = true;
                  }
                  else if (this.aiStyle == 13)
                  {
                    this.ai[0] = 1f;
                    this.netUpdate = true;
                  }
                }
              }
            }
          }
        }
        --this.timeLeft;
        if (this.timeLeft <= 0)
          this.Kill();
        if (this.penetrate == 0)
          this.Kill();
        if (this.active && this.netUpdate && this.owner == Game1.myPlayer)
          NetMessage.SendData(27, number: i);
        if (this.active && this.maxUpdates > 0)
        {
          --this.numUpdates;
          if (this.numUpdates >= 0)
            this.Update(i);
          else
            this.numUpdates = this.maxUpdates;
        }
        this.netUpdate = false;
      }
    }

    public void AI()
    {
      float num1;
      if (this.aiStyle == 1)
      {
        if (this.type == 20 || this.type == 14)
        {
          if (this.alpha > 0)
            this.alpha -= 15;
          if (this.alpha < 0)
            this.alpha = 0;
        }
        if (this.type != 5 && this.type != 14 && this.type != 20)
          ++this.ai[0];
        if ((double) this.ai[0] >= 15.0)
        {
          this.ai[0] = 15f;
          this.velocity.Y += 0.1f;
        }
        this.rotation = (float) Math.Atan2((double) this.velocity.Y, (double) this.velocity.X) + 1.57f;
        if ((double) this.velocity.Y <= 16.0)
          return;
        this.velocity.Y = 16f;
      }
      else if (this.aiStyle == 2)
      {
        ++this.ai[0];
        if ((double) this.ai[0] >= 20.0)
        {
          this.velocity.Y += 0.4f;
          this.velocity.X *= 0.97f;
        }
        this.rotation += (float) (((double) Math.Abs(this.velocity.X) + (double) Math.Abs(this.velocity.Y)) * 0.029999999329447746) * (float) this.direction;
        if ((double) this.velocity.Y <= 16.0)
          return;
        this.velocity.Y = 16f;
      }
      else if (this.aiStyle == 3)
      {
        if (this.soundDelay == 0)
        {
          this.soundDelay = 8;
          Game1.PlaySound(2, (int) this.position.X, (int) this.position.Y, 7);
        }
        if (this.type == 19)
        {
          for (int index1 = 0; index1 < 2; ++index1)
          {
            int index2 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, this.velocity.X * 0.2f, this.velocity.Y * 0.2f, 100, Scale: 2f);
            Game1.dust[index2].noGravity = true;
            Game1.dust[index2].velocity.X *= 0.3f;
            Game1.dust[index2].velocity.Y *= 0.3f;
          }
        }
        else if (this.type == 33)
        {
          if (Game1.rand.Next(1) == 0)
          {
            int index = Dust.NewDust(this.position, this.width, this.height, 40, this.velocity.X * 0.25f, this.velocity.Y * 0.25f, Scale: 1.4f);
            Game1.dust[index].noGravity = true;
          }
        }
        else if (Game1.rand.Next(5) == 0)
          Dust.NewDust(this.position, this.width, this.height, 15, this.velocity.X * 0.5f, this.velocity.Y * 0.5f, 150, Scale: 0.9f);
        if ((double) this.ai[0] == 0.0)
        {
          ++this.ai[1];
          if ((double) this.ai[1] >= 30.0)
          {
            this.ai[0] = 1f;
            this.ai[1] = 0.0f;
            this.netUpdate = true;
          }
        }
        else
        {
          this.tileCollide = false;
          float num2 = 9f;
          float num3 = 0.4f;
          if (this.type == 19)
          {
            num2 = 13f;
            num3 = 0.6f;
          }
          else if (this.type == 33)
          {
            num2 = 15f;
            num3 = 0.8f;
          }
          Vector2 vector2 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
          float num4 = Game1.player[this.owner].position.X + (float) (Game1.player[this.owner].width / 2) - vector2.X;
          float num5 = Game1.player[this.owner].position.Y + (float) (Game1.player[this.owner].height / 2) - vector2.Y;
          float num6 = (float) Math.Sqrt((double) num4 * (double) num4 + (double) num5 * (double) num5);
          float num7 = num2 / num6;
          float num8 = num4 * num7;
          float num9 = num5 * num7;
          if ((double) this.velocity.X < (double) num8)
          {
            this.velocity.X += num3;
            if ((double) this.velocity.X < 0.0 && (double) num8 > 0.0)
              this.velocity.X += num3;
          }
          else if ((double) this.velocity.X > (double) num8)
          {
            this.velocity.X -= num3;
            if ((double) this.velocity.X > 0.0 && (double) num8 < 0.0)
              this.velocity.X -= num3;
          }
          if ((double) this.velocity.Y < (double) num9)
          {
            this.velocity.Y += num3;
            if ((double) this.velocity.Y < 0.0 && (double) num9 > 0.0)
              this.velocity.Y += num3;
          }
          else if ((double) this.velocity.Y > (double) num9)
          {
            this.velocity.Y -= num3;
            if ((double) this.velocity.Y > 0.0 && (double) num9 < 0.0)
              this.velocity.Y -= num3;
          }
          if (Game1.myPlayer == this.owner && new Rectangle((int) this.position.X, (int) this.position.Y, this.width, this.height).Intersects(new Rectangle((int) Game1.player[this.owner].position.X, (int) Game1.player[this.owner].position.Y, Game1.player[this.owner].width, Game1.player[this.owner].height)))
            this.Kill();
        }
        this.rotation += 0.4f * (float) this.direction;
      }
      else if (this.aiStyle == 4)
      {
        this.rotation = (float) Math.Atan2((double) this.velocity.Y, (double) this.velocity.X) + 1.57f;
        if ((double) this.ai[0] == 0.0)
        {
          this.alpha -= 50;
          if (this.alpha > 0)
            return;
          this.alpha = 0;
          this.ai[0] = 1f;
          if ((double) this.ai[1] == 0.0)
          {
            ++this.ai[1];
            this.position += this.velocity * 1f;
          }
          if (this.type == 7 && Game1.myPlayer == this.owner)
          {
            int type = this.type;
            if ((double) this.ai[1] >= 6.0)
              ++type;
            int number = Projectile.NewProjectile(this.position.X + this.velocity.X + (float) (this.width / 2), this.position.Y + this.velocity.Y + (float) (this.height / 2), this.velocity.X, this.velocity.Y, type, this.damage, this.knockBack, this.owner);
            Game1.projectile[number].damage = this.damage;
            Game1.projectile[number].ai[1] = this.ai[1] + 1f;
            NetMessage.SendData(27, number: number);
          }
        }
        else
        {
          if (this.alpha < 170 && this.alpha + 5 >= 170)
          {
            for (int index = 0; index < 3; ++index)
              Dust.NewDust(this.position, this.width, this.height, 18, this.velocity.X * 0.025f, this.velocity.Y * 0.025f, 170, Scale: 1.2f);
            Dust.NewDust(this.position, this.width, this.height, 14, Alpha: 170, Scale: 1.1f);
          }
          this.alpha += 5;
          if (this.alpha >= (int) byte.MaxValue)
            this.Kill();
        }
      }
      else if (this.aiStyle == 5)
      {
        if (this.soundDelay == 0)
        {
          this.soundDelay = 20 + Game1.rand.Next(40);
          Game1.PlaySound(2, (int) this.position.X, (int) this.position.Y, 9);
        }
        if ((double) this.ai[0] == 0.0)
          this.ai[0] = 1f;
        this.alpha += (int) (25.0 * (double) this.ai[0]);
        if (this.alpha > 200)
        {
          this.alpha = 200;
          this.ai[0] = -1f;
        }
        if (this.alpha < 0)
        {
          this.alpha = 0;
          this.ai[0] = 1f;
        }
        this.rotation += (float) (((double) Math.Abs(this.velocity.X) + (double) Math.Abs(this.velocity.Y)) * 0.0099999997764825821) * (float) this.direction;
        if (Game1.rand.Next(10) == 0)
          Dust.NewDust(this.position, this.width, this.height, 15, this.velocity.X * 0.5f, this.velocity.Y * 0.5f, 150, Scale: 1.2f);
        if (Game1.rand.Next(20) != 0)
          return;
        Gore.NewGore(this.position, new Vector2(this.velocity.X * 0.2f, this.velocity.Y * 0.2f), Game1.rand.Next(16, 18));
      }
      else if (this.aiStyle == 6)
      {
        this.velocity *= 0.95f;
        ++this.ai[0];
        if ((double) this.ai[0] == 180.0)
          this.Kill();
        if ((double) this.ai[1] == 0.0)
        {
          this.ai[1] = 1f;
          for (int index = 0; index < 30; ++index)
            Dust.NewDust(this.position, this.width, this.height, 10 + this.type, this.velocity.X, this.velocity.Y, 50);
        }
        if (this.type != 10)
          return;
        int num10 = (int) ((double) this.position.X / 16.0) - 1;
        int num11 = (int) (((double) this.position.X + (double) this.width) / 16.0) + 2;
        int num12 = (int) ((double) this.position.Y / 16.0) - 1;
        int num13 = (int) (((double) this.position.Y + (double) this.height) / 16.0) + 2;
        if (num10 < 0)
          num10 = 0;
        if (num11 > Game1.maxTilesX)
          num11 = Game1.maxTilesX;
        if (num12 < 0)
          num12 = 0;
        if (num13 > Game1.maxTilesY)
          num13 = Game1.maxTilesY;
        for (int i = num10; i < num11; ++i)
        {
          for (int j = num12; j < num13; ++j)
          {
            Vector2 vector2;
            vector2.X = (float) (i * 16);
            vector2.Y = (float) (j * 16);
            if ((double) this.position.X + (double) this.width > (double) vector2.X && (double) this.position.X < (double) vector2.X + 16.0 && (double) this.position.Y + (double) this.height > (double) vector2.Y && (double) this.position.Y < (double) vector2.Y + 16.0 && Game1.myPlayer == this.owner && Game1.tile[i, j].active)
            {
              if (Game1.tile[i, j].type == (byte) 23)
              {
                Game1.tile[i, j].type = (byte) 2;
                WorldGen.SquareTileFrame(i, j);
                if (Game1.netMode == 1)
                  NetMessage.SendTileSquare(-1, i - 1, j - 1, 3);
              }
              if (Game1.tile[i, j].type == (byte) 25)
              {
                Game1.tile[i, j].type = (byte) 1;
                WorldGen.SquareTileFrame(i, j);
                if (Game1.netMode == 1)
                  NetMessage.SendTileSquare(-1, i - 1, j - 1, 3);
              }
            }
          }
        }
      }
      else if (this.aiStyle == 7)
      {
        if (Game1.player[this.owner].dead)
        {
          this.Kill();
        }
        else
        {
          Vector2 vector2_1 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
          float x = Game1.player[this.owner].position.X + (float) (Game1.player[this.owner].width / 2) - vector2_1.X;
          float y = Game1.player[this.owner].position.Y + (float) (Game1.player[this.owner].height / 2) - vector2_1.Y;
          float num14 = (float) Math.Sqrt((double) x * (double) x + (double) y * (double) y);
          this.rotation = (float) Math.Atan2((double) y, (double) x) - 1.57f;
          if ((double) this.ai[0] == 0.0)
          {
            if ((double) num14 > 300.0 && this.type == 13 || (double) num14 > 400.0 && this.type == 32)
              this.ai[0] = 1f;
            int num15 = (int) ((double) this.position.X / 16.0) - 1;
            int num16 = (int) (((double) this.position.X + (double) this.width) / 16.0) + 2;
            int num17 = (int) ((double) this.position.Y / 16.0) - 1;
            int num18 = (int) (((double) this.position.Y + (double) this.height) / 16.0) + 2;
            if (num15 < 0)
              num15 = 0;
            if (num16 > Game1.maxTilesX)
              num16 = Game1.maxTilesX;
            if (num17 < 0)
              num17 = 0;
            if (num18 > Game1.maxTilesY)
              num18 = Game1.maxTilesY;
            for (int i = num15; i < num16; ++i)
            {
              for (int j = num17; j < num18; ++j)
              {
                if (Game1.tile[i, j] == null)
                  Game1.tile[i, j] = new Tile();
                Vector2 vector2_2;
                vector2_2.X = (float) (i * 16);
                vector2_2.Y = (float) (j * 16);
                if ((double) this.position.X + (double) this.width > (double) vector2_2.X && (double) this.position.X < (double) vector2_2.X + 16.0 && (double) this.position.Y + (double) this.height > (double) vector2_2.Y && (double) this.position.Y < (double) vector2_2.Y + 16.0 && Game1.tile[i, j].active && Game1.tileSolid[(int) Game1.tile[i, j].type])
                {
                  if (Game1.player[this.owner].grapCount < 10)
                  {
                    Game1.player[this.owner].grappling[Game1.player[this.owner].grapCount] = this.whoAmI;
                    ++Game1.player[this.owner].grapCount;
                  }
                  if (Game1.myPlayer == this.owner)
                  {
                    int num19 = 0;
                    int index3 = -1;
                    int num20 = 100000;
                    for (int index4 = 0; index4 < 1000; ++index4)
                    {
                      if (Game1.projectile[index4].active && Game1.projectile[index4].owner == this.owner && Game1.projectile[index4].aiStyle == 7)
                      {
                        if (Game1.projectile[index4].timeLeft < num20)
                        {
                          index3 = index4;
                          num20 = Game1.projectile[index4].timeLeft;
                        }
                        ++num19;
                      }
                    }
                    if (num19 > 3)
                      Game1.projectile[index3].Kill();
                  }
                  WorldGen.KillTile(i, j, true, true);
                  Game1.PlaySound(0, i * 16, j * 16);
                  this.velocity.X = 0.0f;
                  this.velocity.Y = 0.0f;
                  this.ai[0] = 2f;
                  this.position.X = (float) (i * 16 + 8 - this.width / 2);
                  this.position.Y = (float) (j * 16 + 8 - this.height / 2);
                  this.damage = 0;
                  this.netUpdate = true;
                  if (Game1.myPlayer == this.owner)
                  {
                    NetMessage.SendData(13, number: this.owner);
                    break;
                  }
                  break;
                }
              }
              if ((double) this.ai[0] == 2.0)
                break;
            }
          }
          else if ((double) this.ai[0] == 1.0)
          {
            float num21 = 11f;
            if (this.type == 32)
              num21 = 15f;
            if ((double) num14 < 24.0)
              this.Kill();
            float num22 = num21 / num14;
            float num23 = x * num22;
            float num24 = y * num22;
            this.velocity.X = num23;
            this.velocity.Y = num24;
          }
          else
          {
            if ((double) this.ai[0] != 2.0)
              return;
            int num25 = (int) ((double) this.position.X / 16.0) - 1;
            int num26 = (int) (((double) this.position.X + (double) this.width) / 16.0) + 2;
            int num27 = (int) ((double) this.position.Y / 16.0) - 1;
            int num28 = (int) (((double) this.position.Y + (double) this.height) / 16.0) + 2;
            if (num25 < 0)
              num25 = 0;
            if (num26 > Game1.maxTilesX)
              num26 = Game1.maxTilesX;
            if (num27 < 0)
              num27 = 0;
            if (num28 > Game1.maxTilesY)
              num28 = Game1.maxTilesY;
            bool flag = true;
            for (int index5 = num25; index5 < num26; ++index5)
            {
              for (int index6 = num27; index6 < num28; ++index6)
              {
                if (Game1.tile[index5, index6] == null)
                  Game1.tile[index5, index6] = new Tile();
                Vector2 vector2_3;
                vector2_3.X = (float) (index5 * 16);
                vector2_3.Y = (float) (index6 * 16);
                if ((double) this.position.X + (double) (this.width / 2) > (double) vector2_3.X && (double) this.position.X + (double) (this.width / 2) < (double) vector2_3.X + 16.0 && (double) this.position.Y + (double) (this.height / 2) > (double) vector2_3.Y && (double) this.position.Y + (double) (this.height / 2) < (double) vector2_3.Y + 16.0 && Game1.tile[index5, index6].active && Game1.tileSolid[(int) Game1.tile[index5, index6].type])
                  flag = false;
              }
            }
            if (flag)
              this.ai[0] = 1f;
            else if (Game1.player[this.owner].grapCount < 10)
            {
              Game1.player[this.owner].grappling[Game1.player[this.owner].grapCount] = this.whoAmI;
              ++Game1.player[this.owner].grapCount;
            }
          }
        }
      }
      else if (this.aiStyle == 8)
      {
        if (this.type == 27)
        {
          int index = Dust.NewDust(new Vector2(this.position.X + this.velocity.X, this.position.Y + this.velocity.Y), this.width, this.height, 29, this.velocity.X, this.velocity.Y, 100, Scale: 3f);
          Game1.dust[index].noGravity = true;
          Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 29, this.velocity.X, this.velocity.Y, 100, Scale: 1.5f);
        }
        else
        {
          for (int index7 = 0; index7 < 2; ++index7)
          {
            int index8 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, this.velocity.X * 0.2f, this.velocity.Y * 0.2f, 100, Scale: 2f);
            Game1.dust[index8].noGravity = true;
            Game1.dust[index8].velocity.X *= 0.3f;
            Game1.dust[index8].velocity.Y *= 0.3f;
          }
        }
        if (this.type != 27)
          ++this.ai[1];
        if ((double) this.ai[1] >= 20.0)
          this.velocity.Y += 0.2f;
        this.rotation += 0.3f * (float) this.direction;
        if ((double) this.velocity.Y <= 16.0)
          return;
        this.velocity.Y = 16f;
      }
      else if (this.aiStyle == 9)
      {
        if (this.soundDelay == 0 && (double) Math.Abs(this.velocity.X) + (double) Math.Abs(this.velocity.Y) > 2.0)
        {
          this.soundDelay = 10;
          Game1.PlaySound(2, (int) this.position.X, (int) this.position.Y, 9);
        }
        int index = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 15, Alpha: 100, Scale: 2f);
        Game1.dust[index].velocity *= 0.3f;
        Game1.dust[index].position.X = (float) ((double) this.position.X + (double) (this.width / 2) + 4.0) + (float) Game1.rand.Next(-4, 5);
        Game1.dust[index].position.Y = this.position.Y + (float) (this.height / 2) + (float) Game1.rand.Next(-4, 5);
        Game1.dust[index].noGravity = true;
        if (Game1.myPlayer == this.owner && (double) this.ai[0] == 0.0)
        {
          if (Game1.player[this.owner].channel)
          {
            float num29 = 12f;
            Vector2 vector2 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
            float num30 = (float) Game1.mouseState[0].Position.X + Game1.screenPosition.X - vector2.X;
            float num31 = (float) Game1.mouseState[0].Position.Y + Game1.screenPosition.Y - vector2.Y;
            num1 = (float) Math.Sqrt((double) num30 * (double) num30 + (double) num31 * (double) num31);
            float num32 = (float) Math.Sqrt((double) num30 * (double) num30 + (double) num31 * (double) num31);
            if ((double) num32 > (double) num29)
            {
              float num33 = num29 / num32;
              float num34 = num30 * num33;
              float num35 = num31 * num33;
              if ((double) num34 != (double) this.velocity.X || (double) num35 != (double) this.velocity.Y)
                this.netUpdate = true;
              this.velocity.X = num34;
              this.velocity.Y = num35;
            }
            else
            {
              if ((double) num30 != (double) this.velocity.X || (double) num31 != (double) this.velocity.Y)
                this.netUpdate = true;
              this.velocity.X = num30;
              this.velocity.Y = num31;
            }
          }
          else
            this.Kill();
        }
        if ((double) this.velocity.X != 0.0 || (double) this.velocity.Y != 0.0)
          this.rotation = (float) Math.Atan2((double) this.velocity.Y, (double) this.velocity.X) - 2.355f;
        if ((double) this.velocity.Y <= 16.0)
          return;
        this.velocity.Y = 16f;
      }
      else if (this.aiStyle == 10)
      {
        if (this.type == 31)
        {
          if (Game1.rand.Next(2) == 0)
          {
            int index = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 32, SpeedY: this.velocity.Y / 2f);
            Game1.dust[index].velocity.X *= 0.4f;
          }
        }
        else if (Game1.rand.Next(20) == 0)
          Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 0);
        if (Game1.myPlayer == this.owner && (double) this.ai[0] == 0.0)
        {
          if (Game1.player[this.owner].channel)
          {
            float num36 = 12f;
            Vector2 vector2 = new Vector2(this.position.X + (float) this.width * 0.5f, 
                this.position.Y + (float) this.height * 0.5f);

            float num37 = (float) Game1.mouseState[0].Position.X + Game1.screenPosition.X - vector2.X;
            float num38 = (float) Game1.mouseState[0].Position.Y + Game1.screenPosition.Y - vector2.Y;
            num1 = (float) Math.Sqrt((double) num37 * (double) num37 + (double) num38 * (double) num38);

            float num39 = (float) Math.Sqrt((double) num37 * (double) num37 
                + (double) num38 * (double) num38);
            if ((double) num39 > (double) num36)
            {
              float num40 = num36 / num39;
              float num41 = num37 * num40;
              float num42 = num38 * num40;
              if ((double) num41 != (double) this.velocity.X || (double) num42 != (double) this.velocity.Y)
                this.netUpdate = true;
              this.velocity.X = num41;
              this.velocity.Y = num42;
            }
            else
            {
              if ((double) num37 != (double) this.velocity.X || (double) num38 != (double) this.velocity.Y)
                this.netUpdate = true;
              this.velocity.X = num37;
              this.velocity.Y = num38;
            }
          }
          else
          {
            this.ai[0] = 1f;
            this.netUpdate = true;
          }
        }
        if ((double) this.ai[0] == 1.0)
          this.velocity.Y += 0.41f;
        this.rotation += 0.1f;
        if ((double) this.velocity.Y <= 10.0)
          return;
        this.velocity.Y = 10f;
      }
      else if (this.aiStyle == 11)
      {
        this.rotation += 0.02f;
        if (Game1.myPlayer != this.owner)
          return;
        if (!Game1.player[this.owner].dead)
        {
          float num43 = 4f;
          Vector2 vector2 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
          float num44 = Game1.player[this.owner].position.X + (float) (Game1.player[this.owner].width / 2) - vector2.X;
          float num45 = Game1.player[this.owner].position.Y + (float) (Game1.player[this.owner].height / 2) - vector2.Y;
          num1 = (float) Math.Sqrt((double) num44 * (double) num44 + (double) num45 * (double) num45);
          float num46 = (float) Math.Sqrt((double) num44 * (double) num44 + (double) num45 * (double) num45);
          if ((double) num46 > 64.0)
          {
            float num47 = num43 / num46;
            float num48 = num44 * num47;
            float num49 = num45 * num47;
            if ((double) num48 != (double) this.velocity.X || (double) num49 != (double) this.velocity.Y)
              this.netUpdate = true;
            this.velocity.X = num48;
            this.velocity.Y = num49;
          }
          else
          {
            if ((double) this.velocity.X != 0.0 || (double) this.velocity.Y != 0.0)
              this.netUpdate = true;
            this.velocity.X = 0.0f;
            this.velocity.Y = 0.0f;
          }
        }
        else
          this.Kill();
      }
      else if (this.aiStyle == 12)
      {
        this.scale -= 0.05f;
        if ((double) this.scale <= 0.0)
          this.Kill();
        if ((double) this.ai[0] > 4.0)
        {
          this.alpha = 150;
          this.light = 0.8f;
          int index = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 29, this.velocity.X, this.velocity.Y, 100, Scale: 2.5f);
          Game1.dust[index].noGravity = true;
          Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 29, this.velocity.X, this.velocity.Y, 100, Scale: 1.5f);
        }
        else
          ++this.ai[0];
        this.rotation += 0.3f * (float) this.direction;
      }
      else if (this.aiStyle == 13)
      {
        if (Game1.player[this.owner].dead)
        {
          this.Kill();
        }
        else
        {
          Game1.player[this.owner].itemAnimation = 5;
          Game1.player[this.owner].itemTime = 5;
          Game1.player[this.owner].direction = (double) this.position.X + (double) (this.width / 2) <= (double) Game1.player[this.owner].position.X + (double) (Game1.player[this.owner].width / 2) ? -1 : 1;
          Vector2 vector2 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
          float x = Game1.player[this.owner].position.X + (float) (Game1.player[this.owner].width / 2) - vector2.X;
          float y = Game1.player[this.owner].position.Y + (float) (Game1.player[this.owner].height / 2) - vector2.Y;
          float num50 = (float) Math.Sqrt((double) x * (double) x + (double) y * (double) y);
          if ((double) this.ai[0] == 0.0)
          {
            if ((double) num50 > 600.0)
              this.ai[0] = 1f;
            this.rotation = (float) Math.Atan2((double) this.velocity.Y, (double) this.velocity.X) + 1.57f;
            ++this.ai[1];
            if ((double) this.ai[1] > 2.0)
              this.alpha = 0;
            if ((double) this.ai[1] < 10.0)
              return;
            this.ai[1] = 15f;
            this.velocity.Y += 0.3f;
          }
          else
          {
            if ((double) this.ai[0] != 1.0)
              return;
            this.tileCollide = false;
            this.rotation = (float) Math.Atan2((double) y, (double) x) - 1.57f;
            float num51 = 11f;
            if ((double) num50 < 50.0)
              this.Kill();
            float num52 = num51 / num50;
            float num53 = x * num52;
            float num54 = y * num52;
            this.velocity.X = num53;
            this.velocity.Y = num54;
          }
        }
      }
      else if (this.aiStyle == 14)
      {
        ++this.ai[0];
        if ((double) this.ai[0] > 5.0)
        {
          this.ai[0] = 5f;
          if ((double) this.velocity.Y == 0.0 && (double) this.velocity.X != 0.0)
          {
            this.velocity.X *= 0.97f;
            if ((double) this.velocity.X > -0.01 && (double) this.velocity.X < 0.01)
            {
              this.velocity.X = 0.0f;
              this.netUpdate = true;
            }
          }
          this.velocity.Y += 0.2f;
        }
        this.rotation += this.velocity.X * 0.1f;
      }
      else if (this.aiStyle == 15)
      {
        if (this.type == 25)
        {
          if (Game1.rand.Next(15) == 0)
            Dust.NewDust(this.position, this.width, this.height, 14, Alpha: 150, Scale: 1.3f);
        }
        else if (this.type == 26)
        {
          int index = Dust.NewDust(this.position, this.width, this.height, 29, this.velocity.X * 0.4f, this.velocity.Y * 0.4f, 100, Scale: 2.5f);
          Game1.dust[index].noGravity = true;
          Game1.dust[index].velocity.X /= 2f;
          Game1.dust[index].velocity.Y /= 2f;
        }
        if (Game1.player[this.owner].dead)
        {
          this.Kill();
        }
        else
        {
          Game1.player[this.owner].itemAnimation = 5;
          Game1.player[this.owner].itemTime = 5;
          Game1.player[this.owner].direction = (double) this.position.X + (double) (this.width / 2) <= (double) Game1.player[this.owner].position.X + (double) (Game1.player[this.owner].width / 2) ? -1 : 1;
          Vector2 vector2 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
          float num55 = Game1.player[this.owner].position.X + (float) (Game1.player[this.owner].width / 2) - vector2.X;
          float num56 = Game1.player[this.owner].position.Y + (float) (Game1.player[this.owner].height / 2) - vector2.Y;
          float num57 = (float) Math.Sqrt((double) num55 * (double) num55 + (double) num56 * (double) num56);
          if ((double) this.ai[0] == 0.0)
          {
            this.tileCollide = true;
            if ((double) num57 > 300.0)
            {
              this.ai[0] = 1f;
            }
            else
            {
              ++this.ai[1];
              if ((double) this.ai[1] > 2.0)
                this.alpha = 0;
              if ((double) this.ai[1] >= 5.0)
              {
                this.ai[1] = 15f;
                this.velocity.Y += 0.5f;
                this.velocity.X *= 0.95f;
              }
            }
          }
          else if ((double) this.ai[0] == 1.0)
          {
            this.tileCollide = false;
            float num58 = 11f;
            if ((double) num57 < 20.0)
              this.Kill();
            float num59 = num58 / num57;
            float num60 = num55 * num59;
            float num61 = num56 * num59;
            this.velocity.X = num60;
            this.velocity.Y = num61;
          }
          this.rotation += this.velocity.X * 0.03f;
        }
      }
      else
      {
        if (this.aiStyle != 16)
          return;
        if ((double) this.ai[1] > 0.0)
        {
          this.alpha = (int) byte.MaxValue;
          if (this.type == 28)
          {
            this.position.X += (float) (this.width / 2);
            this.position.Y += (float) (this.height / 2);
            this.width = 128;
            this.height = 128;
            this.position.X -= (float) (this.width / 2);
            this.position.Y -= (float) (this.height / 2);
            this.damage = 100;
            this.knockBack = 8f;
          }
          else if (this.type == 29)
          {
            this.position.X += (float) (this.width / 2);
            this.position.Y += (float) (this.height / 2);
            this.width = 250;
            this.height = 250;
            this.position.X -= (float) (this.width / 2);
            this.position.Y -= (float) (this.height / 2);
            this.damage = 250;
            this.knockBack = 10f;
          }
          else if (this.type == 30)
          {
            this.position.X += (float) (this.width / 2);
            this.position.Y += (float) (this.height / 2);
            this.width = 128;
            this.height = 128;
            this.position.X -= (float) (this.width / 2);
            this.position.Y -= (float) (this.height / 2);
            this.knockBack = 8f;
          }
        }
        else if (this.type != 30 && Game1.rand.Next(4) == 0)
        {
          if (this.type != 30)
            this.damage = 0;
          Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, Alpha: 100);
        }
        if (this.owner == Game1.myPlayer && this.timeLeft <= 3 && (double) this.ai[1] == 0.0)
        {
          this.ai[1] = 1f;
          this.netUpdate = true;
        }
        ++this.ai[0];
        if (this.type == 30 && (double) this.ai[0] > 10.0 || this.type != 30 && (double) this.ai[0] > 5.0)
        {
          this.ai[0] = 10f;
          if ((double) this.velocity.Y == 0.0 && (double) this.velocity.X != 0.0)
          {
            this.velocity.X *= 0.97f;
            if (this.type == 29)
              this.velocity.X *= 0.99f;
            if ((double) this.velocity.X > -0.01 && (double) this.velocity.X < 0.01)
            {
              this.velocity.X = 0.0f;
              this.netUpdate = true;
            }
          }
          this.velocity.Y += 0.2f;
        }
        this.rotation += this.velocity.X * 0.1f;
      }
    }

    public void Kill()
    {
      if (!this.active)
        return;
      if (this.type == 1)
      {
        Game1.PlaySound(0, (int) this.position.X, (int) this.position.Y);
        for (int index = 0; index < 10; ++index)
          Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 7);
      }
      else if (this.type == 2)
      {
        Game1.PlaySound(0, (int) this.position.X, (int) this.position.Y);
        for (int index = 0; index < 20; ++index)
          Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, Alpha: 100);
      }
      else if (this.type == 3)
      {
        Game1.PlaySound(0, (int) this.position.X, (int) this.position.Y);
        for (int index = 0; index < 10; ++index)
          Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 1, this.velocity.X * 0.1f, this.velocity.Y * 0.1f, Scale: 0.75f);
      }
      else if (this.type == 4)
      {
        Game1.PlaySound(0, (int) this.position.X, (int) this.position.Y);
        for (int index = 0; index < 10; ++index)
          Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 14, Alpha: 150, Scale: 1.1f);
      }
      else if (this.type == 5)
      {
        Game1.PlaySound(2, (int) this.position.X, (int) this.position.Y, 10);
        for (int index = 0; index < 60; ++index)
          Dust.NewDust(this.position, this.width, this.height, 15, this.velocity.X * 0.5f, this.velocity.Y * 0.5f, 150, Scale: 1.5f);
      }
      else if (this.type == 9 || this.type == 12)
      {
        Game1.PlaySound(2, (int) this.position.X, (int) this.position.Y, 10);
        for (int index = 0; index < 10; ++index)
          Dust.NewDust(this.position, this.width, this.height, 15, this.velocity.X * 0.1f, this.velocity.Y * 0.1f, 150, Scale: 1.2f);
        for (int index = 0; index < 3; ++index)
          Gore.NewGore(this.position, new Vector2(this.velocity.X * 0.05f, this.velocity.Y * 0.05f), Game1.rand.Next(16, 18));
      }
      else if (this.type == 14 || this.type == 20)
      {
        Collision.HitTiles(this.position, this.velocity, this.width, this.height);
        Game1.PlaySound(2, (int) this.position.X, (int) this.position.Y, 10);
      }
      else if (this.type == 15)
      {
        Game1.PlaySound(2, (int) this.position.X, (int) this.position.Y, 10);
        for (int index1 = 0; index1 < 20; ++index1)
        {
          int index2 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, (float) (-(double) this.velocity.X * 0.20000000298023224), (float) (-(double) this.velocity.Y * 0.20000000298023224), 100, Scale: 2f);
          Game1.dust[index2].noGravity = true;
          Game1.dust[index2].velocity *= 2f;
          int index3 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, (float) (-(double) this.velocity.X * 0.20000000298023224), (float) (-(double) this.velocity.Y * 0.20000000298023224), 100);
          Game1.dust[index3].velocity *= 2f;
        }
      }
      else if (this.type == 16)
      {
        Game1.PlaySound(2, (int) this.position.X, (int) this.position.Y, 10);
        for (int index4 = 0; index4 < 20; ++index4)
        {
          int index5 = Dust.NewDust(new Vector2(this.position.X - this.velocity.X, this.position.Y - this.velocity.Y), this.width, this.height, 15, Alpha: 100, Scale: 2f);
          Game1.dust[index5].noGravity = true;
          Game1.dust[index5].velocity *= 2f;
          Dust.NewDust(new Vector2(this.position.X - this.velocity.X, this.position.Y - this.velocity.Y), this.width, this.height, 15, Alpha: 100);
        }
      }
      else if (this.type == 17)
      {
        Game1.PlaySound(0, (int) this.position.X, (int) this.position.Y);
        for (int index = 0; index < 5; ++index)
          Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 0);
      }
      else if (this.type == 31)
      {
        Game1.PlaySound(0, (int) this.position.X, (int) this.position.Y);
        for (int index6 = 0; index6 < 5; ++index6)
        {
          int index7 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 32);
          Game1.dust[index7].velocity *= 0.6f;
        }
      }
      else if (this.type == 21)
      {
        Game1.PlaySound(0, (int) this.position.X, (int) this.position.Y);
        for (int index = 0; index < 10; ++index)
          Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 26, Scale: 0.8f);
      }
      else if (this.type == 24)
      {
        for (int index = 0; index < 10; ++index)
          Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 1, this.velocity.X * 0.1f, this.velocity.Y * 0.1f, Scale: 0.75f);
      }
      else if (this.type == 27)
      {
        Game1.PlaySound(2, (int) this.position.X, (int) this.position.Y, 10);
        for (int index8 = 0; index8 < 30; ++index8)
        {
          int index9 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 29, this.velocity.X * 0.1f, this.velocity.Y * 0.1f, 100, Scale: 3f);
          Game1.dust[index9].noGravity = true;
          Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 29, this.velocity.X * 0.1f, this.velocity.Y * 0.1f, 100, Scale: 2f);
        }
      }
      else if (this.type == 28 || this.type == 30)
      {
        Game1.PlaySound(2, (int) this.position.X, (int) this.position.Y, 14);
        this.position.X += (float) (this.width / 2);
        this.position.Y += (float) (this.height / 2);
        this.width = 22;
        this.height = 22;
        this.position.X -= (float) (this.width / 2);
        this.position.Y -= (float) (this.height / 2);
        for (int index10 = 0; index10 < 20; ++index10)
        {
          int index11 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 31, Alpha: 100, Scale: 1.5f);
          Game1.dust[index11].velocity *= 1.4f;
        }
        for (int index12 = 0; index12 < 10; ++index12)
        {
          int index13 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, Alpha: 100, Scale: 2.5f);
          Game1.dust[index13].noGravity = true;
          Game1.dust[index13].velocity *= 5f;
          int index14 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, Alpha: 100, Scale: 1.5f);
          Game1.dust[index14].velocity *= 3f;
        }
        int index15 = Gore.NewGore(new Vector2(this.position.X, this.position.Y), new Vector2(), Game1.rand.Next(61, 64));
        Game1.gore[index15].velocity *= 0.4f;
        ++Game1.gore[index15].velocity.X;
        ++Game1.gore[index15].velocity.Y;
        int index16 = Gore.NewGore(new Vector2(this.position.X, this.position.Y), new Vector2(), Game1.rand.Next(61, 64));
        Game1.gore[index16].velocity *= 0.4f;
        --Game1.gore[index16].velocity.X;
        ++Game1.gore[index16].velocity.Y;
        int index17 = Gore.NewGore(new Vector2(this.position.X, this.position.Y), new Vector2(), Game1.rand.Next(61, 64));
        Game1.gore[index17].velocity *= 0.4f;
        ++Game1.gore[index17].velocity.X;
        --Game1.gore[index17].velocity.Y;
        int index18 = Gore.NewGore(new Vector2(this.position.X, this.position.Y), new Vector2(), Game1.rand.Next(61, 64));
        Game1.gore[index18].velocity *= 0.4f;
        --Game1.gore[index18].velocity.X;
        --Game1.gore[index18].velocity.Y;
      }
      else if (this.type == 29)
      {
        Game1.PlaySound(2, (int) this.position.X, (int) this.position.Y, 14);
        this.position.X += (float) (this.width / 2);
        this.position.Y += (float) (this.height / 2);
        this.width = 200;
        this.height = 200;
        this.position.X -= (float) (this.width / 2);
        this.position.Y -= (float) (this.height / 2);
        for (int index19 = 0; index19 < 50; ++index19)
        {
          int index20 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 31, Alpha: 100, Scale: 2f);
          Game1.dust[index20].velocity *= 1.4f;
        }
        for (int index21 = 0; index21 < 80; ++index21)
        {
          int index22 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, Alpha: 100, Scale: 3f);
          Game1.dust[index22].noGravity = true;
          Game1.dust[index22].velocity *= 5f;
          int index23 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, Alpha: 100, Scale: 2f);
          Game1.dust[index23].velocity *= 3f;
        }
        for (int index24 = 0; index24 < 2; ++index24)
        {
          int index25 = Gore.NewGore(new Vector2((float) ((double) this.position.X + (double) (this.width / 2) - 24.0), (float) ((double) this.position.Y + (double) (this.height / 2) - 24.0)), new Vector2(), Game1.rand.Next(61, 64));
          Game1.gore[index25].scale = 1.5f;
          Game1.gore[index25].velocity.X += 1.5f;
          Game1.gore[index25].velocity.Y += 1.5f;
          int index26 = Gore.NewGore(new Vector2((float) ((double) this.position.X + (double) (this.width / 2) - 24.0), (float) ((double) this.position.Y + (double) (this.height / 2) - 24.0)), new Vector2(), Game1.rand.Next(61, 64));
          Game1.gore[index26].scale = 1.5f;
          Game1.gore[index26].velocity.X -= 1.5f;
          Game1.gore[index26].velocity.Y += 1.5f;
          int index27 = Gore.NewGore(new Vector2((float) ((double) this.position.X + (double) (this.width / 2) - 24.0), (float) ((double) this.position.Y + (double) (this.height / 2) - 24.0)), new Vector2(), Game1.rand.Next(61, 64));
          Game1.gore[index27].scale = 1.5f;
          Game1.gore[index27].velocity.X += 1.5f;
          Game1.gore[index27].velocity.Y -= 1.5f;
          int index28 = Gore.NewGore(new Vector2((float) ((double) this.position.X + (double) (this.width / 2) - 24.0), (float) ((double) this.position.Y + (double) (this.height / 2) - 24.0)), new Vector2(), Game1.rand.Next(61, 64));
          Game1.gore[index28].scale = 1.5f;
          Game1.gore[index28].velocity.X -= 1.5f;
          Game1.gore[index28].velocity.Y -= 1.5f;
        }
        this.position.X += (float) (this.width / 2);
        this.position.Y += (float) (this.height / 2);
        this.width = 10;
        this.height = 10;
        this.position.X -= (float) (this.width / 2);
        this.position.Y -= (float) (this.height / 2);
      }
      if (this.owner == Game1.myPlayer)
      {
        if (this.type == 28 || this.type == 29)
        {
          int num1 = 3;
          if (this.type == 29)
            num1 = 7;
          int num2 = (int) ((double) this.position.X / 16.0 - (double) num1);
          int num3 = (int) ((double) this.position.X / 16.0 + (double) num1);
          int num4 = (int) ((double) this.position.Y / 16.0 - (double) num1);
          int num5 = (int) ((double) this.position.Y / 16.0 + (double) num1);
          if (num2 < 0)
            num2 = 0;
          if (num3 > Game1.maxTilesX)
            num3 = Game1.maxTilesX;
          if (num4 < 0)
            num4 = 0;
          if (num5 > Game1.maxTilesY)
            num5 = Game1.maxTilesY;
          bool flag1 = false;
          for (int index29 = num2; index29 <= num3; ++index29)
          {
            for (int index30 = num4; index30 <= num5; ++index30)
            {
              float num6 = Math.Abs((float) index29 - this.position.X / 16f);
              float num7 = Math.Abs((float) index30 - this.position.Y / 16f);
              if (Math.Sqrt((double) num6 * (double) num6 + (double) num7 * (double) num7) < (double) num1 && Game1.tile[index29, index30] != null && Game1.tile[index29, index30].wall == (byte) 0)
              {
                flag1 = true;
                break;
              }
            }
          }
          for (int index31 = num2; index31 <= num3; ++index31)
          {
            for (int index32 = num4; index32 <= num5; ++index32)
            {
              float num8 = Math.Abs((float) index31 - this.position.X / 16f);
              float num9 = Math.Abs((float) index32 - this.position.Y / 16f);
              if (Math.Sqrt((double) num8 * (double) num8 + (double) num9 * (double) num9) < (double) num1)
              {
                bool flag2 = true;
                if (Game1.tile[index31, index32] != null && Game1.tile[index31, index32].active)
                {
                  flag2 = false;
                  if (this.type == 28)
                  {
                    if (!Game1.tileSolid[(int) Game1.tile[index31, index32].type] || Game1.tileSolidTop[(int) Game1.tile[index31, index32].type] || Game1.tile[index31, index32].type == (byte) 0 || Game1.tile[index31, index32].type == (byte) 1 || Game1.tile[index31, index32].type == (byte) 2 || Game1.tile[index31, index32].type == (byte) 23 || Game1.tile[index31, index32].type == (byte) 30 || Game1.tile[index31, index32].type == (byte) 40 || Game1.tile[index31, index32].type == (byte) 6 || Game1.tile[index31, index32].type == (byte) 7 || Game1.tile[index31, index32].type == (byte) 8 || Game1.tile[index31, index32].type == (byte) 9 || Game1.tile[index31, index32].type == (byte) 10 || Game1.tile[index31, index32].type == (byte) 53 || Game1.tile[index31, index32].type == (byte) 54 || Game1.tile[index31, index32].type == (byte) 57 || Game1.tile[index31, index32].type == (byte) 59 || Game1.tile[index31, index32].type == (byte) 60 || Game1.tile[index31, index32].type == (byte) 63 || Game1.tile[index31, index32].type == (byte) 64 || Game1.tile[index31, index32].type == (byte) 65 || Game1.tile[index31, index32].type == (byte) 66 || Game1.tile[index31, index32].type == (byte) 67 || Game1.tile[index31, index32].type == (byte) 68 || Game1.tile[index31, index32].type == (byte) 70)
                      flag2 = true;
                  }
                  else if (this.type == 29)
                    flag2 = true;
                  if (Game1.tileDungeon[(int) Game1.tile[index31, index32].type] || Game1.tile[index31, index32].type == (byte) 26 || Game1.tile[index31, index32].type == (byte) 37)
                    flag2 = false;
                  if (flag2)
                  {
                    WorldGen.KillTile(index31, index32);
                    if (!Game1.tile[index31, index32].active && Game1.netMode == 1)
                      NetMessage.SendData(17, number2: (float) index31, number3: (float) index32);
                  }
                }
                if (flag2 && Game1.tile[index31, index32] != null && Game1.tile[index31, index32].wall > (byte) 0 && flag1)
                {
                  WorldGen.KillWall(index31, index32);
                  if (Game1.tile[index31, index32].wall == (byte) 0 && Game1.netMode == 1)
                    NetMessage.SendData(17, number: 2, number2: (float) index31, number3: (float) index32);
                }
              }
            }
          }
        }
        if (Game1.netMode != 0)
          NetMessage.SendData(29, number: this.identity, number2: (float) this.owner);
        int number = -1;
        if (this.aiStyle == 10)
        {
          int index33 = (int) ((double) this.position.X + (double) (this.width / 2)) / 16;
          int index34 = (int) ((double) this.position.Y + (double) (this.width / 2)) / 16;
          int num = 0;
          int Type = 2;
          if (this.type == 31)
          {
            num = 53;
            Type = 169;
          }
          if (!Game1.tile[index33, index34].active)
          {
            WorldGen.PlaceTile(index33, index34, num, forced: true);
            if (Game1.tile[index33, index34].active && (int) Game1.tile[index33, index34].type == num)
              NetMessage.SendData(17, number: 1, number2: (float) index33, number3: (float) index34, number4: (float) num);
            else
              number = Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, Type);
          }
          else
            number = Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, Type);
        }
        if (this.type == 1 && Game1.rand.Next(3) < 2)
          number = Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 40);
        if (this.type == 2 && Game1.rand.Next(5) < 4)
          number = Game1.rand.Next(4) != 0 ? Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 40) : Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 41);
        if (this.type == 3 && Game1.rand.Next(5) < 4)
          number = Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 42);
        if (this.type == 4 && Game1.rand.Next(5) < 4)
          number = Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 47);
        if (this.type == 12)
          number = Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 75);
        if (this.type == 21 && Game1.rand.Next(5) < 4)
          number = Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 154);
        if (Game1.netMode == 1 && number >= 0)
          NetMessage.SendData(21, number: number);
      }
      this.active = false;
    }

    public Color GetAlpha(Color newColor)
    {
      int r;
      int g;
      int b;
      if (this.type == 9 || this.type == 15)
      {
        r = (int) newColor.R - this.alpha / 3;
        g = (int) newColor.G - this.alpha / 3;
        b = (int) newColor.B - this.alpha / 3;
      }
      else if (this.type == 16 || this.type == 18)
      {
        r = (int) newColor.R;
        g = (int) newColor.G;
        b = (int) newColor.B;
      }
      else
      {
        r = (int) newColor.R - this.alpha;
        g = (int) newColor.G - this.alpha;
        b = (int) newColor.B - this.alpha;
      }
      int a = (int) newColor.A - this.alpha;
      if (a < 0)
        a = 0;
      if (a > (int) byte.MaxValue)
        a = (int) byte.MaxValue;
      return new Color(r, g, b, a);
    }
  }
}
