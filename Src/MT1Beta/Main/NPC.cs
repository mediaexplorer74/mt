// Decompiled with JetBrains decompiler
// Type: GameManager.NPC
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

using Microsoft.Xna.Framework;
using System;

namespace GameManager
{
  public class NPC
  {
    public static int immuneTime = 20;
    public static int maxAI = 4;
    private static int spawnSpaceX = 4;
    private static int spawnSpaceY = 4;
    private static int spawnRangeX = (int) ((double) (Game1.screenWidth / 16) * 1.2);
    private static int spawnRangeY = (int) ((double) (Game1.screenHeight / 16) * 1.2);
    public static int safeRangeX = (int) ((double) (Game1.screenWidth / 16) * 0.55);
    public static int safeRangeY = (int) ((double) (Game1.screenHeight / 16) * 0.55);
    private static int activeRangeX = Game1.screenWidth * 2;
    private static int activeRangeY = Game1.screenHeight * 2;
    private static int townRangeX = Game1.screenWidth * 3;
    private static int townRangeY = Game1.screenHeight * 3;
    private static int activeTime = 1000;
    private static int defaultSpawnRate = 850;
    private static int defaultMaxSpawns = 4;
    public bool wet = false;
    public byte wetCount = 0;
    public bool lavaWet = false;
    public static bool downedBoss1 = false;
    public static bool downedBoss2 = false;
    public static bool downedBoss3 = false;
    private static int spawnRate = NPC.defaultSpawnRate;
    private static int maxSpawns = NPC.defaultMaxSpawns;
    public int soundDelay = 0;
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 oldPosition;
    public Vector2 oldVelocity;
    public int width;
    public int height;
    public bool active;
    public int[] immune = new int[9];
    public int direction = 1;
    public int directionY = 1;
    public int type;
    public float[] ai = new float[NPC.maxAI];
    public int aiAction = 0;
    public int aiStyle;
    public int timeLeft;
    public int target = -1;
    public int damage;
    public int defense;
    public int soundHit;
    public int soundKilled;
    public int life;
    public int lifeMax;
    public Rectangle targetRect;
    public double frameCounter;
    public Rectangle frame;
    public string name;
    public Color color;
    public int alpha;
    public float scale = 1f;
    public float knockBackResist = 1f;
    public int oldDirection = 0;
    public int oldDirectionY = 0;
    public int oldTarget = 0;
    public int whoAmI = 0;
    public float rotation = 0.0f;
    public bool noGravity = false;
    public bool noTileCollide = false;
    public bool netUpdate = false;
    public bool collideX = false;
    public bool collideY = false;
    public bool boss = false;
    public int spriteDirection = -1;
    public bool behindTiles;
    public float value;
    public bool townNPC = false;
    public bool homeless = false;
    public int homeTileX = -1;
    public int homeTileY = -1;
    public bool friendly = false;
    public bool closeDoor = false;
    public int doorX = 0;
    public int doorY = 0;
    public int friendlyRegen = 0;

    public void SetDefaults(string Name)
    {
      this.SetDefaults(0);
      switch (Name)
      {
        case "Green Slime":
          this.SetDefaults(1);
          this.name = Name;
          this.scale = 0.9f;
          this.damage = 8;
          this.defense = 2;
          this.life = 15;
          this.knockBackResist = 1.1f;
          this.color = new Color(0, 220, 40, 100);
          this.value = 3f;
          break;
        case "Baby Slime":
          this.SetDefaults(1);
          this.name = Name;
          this.scale = 0.9f;
          this.damage = 10;
          this.defense = 4;
          this.life = 30;
          this.knockBackResist = 0.95f;
          this.alpha = 120;
          this.color = new Color(0, 0, 0, 50);
          this.value = 10f;
          break;
        case "Purple Slime":
          this.SetDefaults(1);
          this.name = Name;
          this.scale = 1.2f;
          this.damage = 12;
          this.defense = 6;
          this.life = 40;
          this.knockBackResist = 0.9f;
          this.color = new Color(200, 0, (int) byte.MaxValue, 150);
          this.value = 10f;
          break;
        case "Red Slime":
          this.SetDefaults(1);
          this.name = Name;
          this.damage = 12;
          this.defense = 4;
          this.life = 35;
          this.color = new Color((int) byte.MaxValue, 30, 0, 100);
          this.value = 8f;
          break;
        case "Yellow Slime":
          this.SetDefaults(1);
          this.name = Name;
          this.scale = 1.2f;
          this.damage = 15;
          this.defense = 7;
          this.life = 45;
          this.color = new Color((int) byte.MaxValue, (int) byte.MaxValue, 0, 100);
          this.value = 10f;
          break;
        default:
          if (Name != "")
          {
            for (int Type = 1; Type < 44; ++Type)
            {
              this.SetDefaults(Type);
              if (!(this.name == Name))
              {
                if (Type == 43)
                {
                  this.SetDefaults(0);
                  this.active = false;
                }
              }
              else
                break;
            }
            break;
          }
          this.active = false;
          break;
      }
      this.lifeMax = this.life;
    }

    public void SetDefaults(int Type)
    {
      this.lavaWet = false;
      this.wetCount = (byte) 0;
      this.wet = false;
      this.townNPC = false;
      this.homeless = false;
      this.homeTileX = -1;
      this.homeTileY = -1;
      this.friendly = false;
      this.behindTiles = false;
      this.boss = false;
      this.noTileCollide = false;
      this.rotation = 0.0f;
      this.active = true;
      this.alpha = 0;
      this.color = new Color();
      this.collideX = false;
      this.collideY = false;
      this.direction = 0;
      this.oldDirection = this.direction;
      this.frameCounter = 0.0;
      this.netUpdate = false;
      this.knockBackResist = 1f;
      this.name = "";
      this.noGravity = false;
      this.scale = 1f;
      this.soundHit = 0;
      this.soundKilled = 0;
      this.spriteDirection = -1;
      this.target = 8;
      this.oldTarget = this.target;
      this.targetRect = new Rectangle();
      this.timeLeft = NPC.activeTime;
      this.type = Type;
      this.value = 0.0f;
      for (int index = 0; index < NPC.maxAI; ++index)
        this.ai[index] = 0.0f;
      if (this.type == 1)
      {
        this.name = "Blue Slime";
        this.width = 24;
        this.height = 18;
        this.aiStyle = 1;
        this.damage = 7;
        this.defense = 2;
        this.lifeMax = 25;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.alpha = 175;
        this.color = new Color(0, 80, (int) byte.MaxValue, 100);
        this.value = 25f;
      }
      if (this.type == 2)
      {
        this.name = "Demon Eye";
        this.width = 30;
        this.height = 32;
        this.aiStyle = 2;
        this.damage = 18;
        this.defense = 2;
        this.lifeMax = 60;
        this.soundHit = 1;
        this.knockBackResist = 0.8f;
        this.soundKilled = 1;
        this.value = 75f;
      }
      if (this.type == 3)
      {
        this.name = "Zombie";
        this.width = 18;
        this.height = 40;
        this.aiStyle = 3;
        this.damage = 14;
        this.defense = 6;
        this.lifeMax = 45;
        this.soundHit = 1;
        this.soundKilled = 2;
        this.knockBackResist = 0.5f;
        this.value = 60f;
      }
      if (this.type == 4)
      {
        this.name = "Eye of Cthulhu";
        this.width = 100;
        this.height = 110;
        this.aiStyle = 4;
        this.damage = 18;
        this.defense = 10;
        this.lifeMax = 4000;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.knockBackResist = 0.0f;
        this.noGravity = true;
        this.noTileCollide = true;
        this.timeLeft = NPC.activeTime * 30;
        this.boss = true;
        this.value = 30000f;
      }
      if (this.type == 5)
      {
        this.name = "Servant of Cthulhu";
        this.width = 20;
        this.height = 20;
        this.aiStyle = 5;
        this.damage = 23;
        this.defense = 0;
        this.lifeMax = 8;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.noGravity = true;
        this.noTileCollide = true;
      }
      if (this.type == 6)
      {
        this.name = "Eater of Souls";
        this.width = 30;
        this.height = 30;
        this.aiStyle = 5;
        this.damage = 15;
        this.defense = 8;
        this.lifeMax = 45;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.noGravity = true;
        this.knockBackResist = 0.5f;
        this.value = 90f;
      }
      if (this.type == 7)
      {
        this.name = "Devourer Head";
        this.width = 22;
        this.height = 22;
        this.aiStyle = 6;
        this.damage = 30;
        this.defense = 6;
        this.lifeMax = 60;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.noGravity = true;
        this.noTileCollide = true;
        this.knockBackResist = 0.0f;
        this.behindTiles = true;
        this.value = 300f;
      }
      if (this.type == 8)
      {
        this.name = "Devourer Body";
        this.width = 22;
        this.height = 22;
        this.aiStyle = 6;
        this.damage = 20;
        this.defense = 8;
        this.lifeMax = 100;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.noGravity = true;
        this.noTileCollide = true;
        this.knockBackResist = 0.0f;
        this.behindTiles = true;
        this.value = 300f;
      }
      if (this.type == 9)
      {
        this.name = "Devourer Tail";
        this.width = 22;
        this.height = 22;
        this.aiStyle = 6;
        this.damage = 15;
        this.defense = 12;
        this.lifeMax = 130;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.noGravity = true;
        this.noTileCollide = true;
        this.knockBackResist = 0.0f;
        this.behindTiles = true;
        this.value = 300f;
      }
      if (this.type == 10)
      {
        this.name = "Giant Worm Head";
        this.width = 14;
        this.height = 14;
        this.aiStyle = 6;
        this.damage = 8;
        this.defense = 0;
        this.lifeMax = 10;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.noGravity = true;
        this.noTileCollide = true;
        this.knockBackResist = 0.0f;
        this.behindTiles = true;
        this.value = 200f;
      }
      if (this.type == 11)
      {
        this.name = "Giant Worm Body";
        this.width = 14;
        this.height = 14;
        this.aiStyle = 6;
        this.damage = 4;
        this.defense = 4;
        this.lifeMax = 15;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.noGravity = true;
        this.noTileCollide = true;
        this.knockBackResist = 0.0f;
        this.behindTiles = true;
        this.value = 200f;
      }
      if (this.type == 12)
      {
        this.name = "Giant Worm Tail";
        this.width = 14;
        this.height = 14;
        this.aiStyle = 6;
        this.damage = 4;
        this.defense = 6;
        this.lifeMax = 20;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.noGravity = true;
        this.noTileCollide = true;
        this.knockBackResist = 0.0f;
        this.behindTiles = true;
        this.value = 200f;
      }
      if (this.type == 13)
      {
        this.name = "Eater of Worlds Head";
        this.width = 38;
        this.height = 38;
        this.aiStyle = 6;
        this.damage = 40;
        this.defense = 0;
        this.lifeMax = 120;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.noGravity = true;
        this.noTileCollide = true;
        this.knockBackResist = 0.0f;
        this.behindTiles = true;
        this.value = 300f;
      }
      if (this.type == 14)
      {
        this.name = "Eater of Worlds Body";
        this.width = 38;
        this.height = 38;
        this.aiStyle = 6;
        this.damage = 15;
        this.defense = 4;
        this.lifeMax = 200;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.noGravity = true;
        this.noTileCollide = true;
        this.knockBackResist = 0.0f;
        this.behindTiles = true;
        this.value = 300f;
      }
      if (this.type == 15)
      {
        this.name = "Eater of Worlds Tail";
        this.width = 38;
        this.height = 38;
        this.aiStyle = 6;
        this.damage = 10;
        this.defense = 8;
        this.lifeMax = 300;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.noGravity = true;
        this.noTileCollide = true;
        this.knockBackResist = 0.0f;
        this.behindTiles = true;
        this.value = 300f;
      }
      if (this.type == 16)
      {
        this.name = "Mother Slime";
        this.width = 36;
        this.height = 24;
        this.aiStyle = 1;
        this.damage = 15;
        this.defense = 7;
        this.lifeMax = 90;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.alpha = 120;
        this.color = new Color(0, 0, 0, 50);
        this.value = 75f;
        this.scale = 1.25f;
        this.knockBackResist = 0.6f;
      }
      if (this.type == 17)
      {
        this.townNPC = true;
        this.friendly = true;
        this.name = "Merchant";
        this.width = 18;
        this.height = 40;
        this.aiStyle = 7;
        this.damage = 10;
        this.defense = 15;
        this.lifeMax = 250;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.knockBackResist = 0.5f;
      }
      if (this.type == 18)
      {
        this.townNPC = true;
        this.friendly = true;
        this.name = "Nurse";
        this.width = 18;
        this.height = 40;
        this.aiStyle = 7;
        this.damage = 10;
        this.defense = 15;
        this.lifeMax = 250;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.knockBackResist = 0.5f;
      }
      if (this.type == 19)
      {
        this.townNPC = true;
        this.friendly = true;
        this.name = "Arms Dealer";
        this.width = 18;
        this.height = 40;
        this.aiStyle = 7;
        this.damage = 10;
        this.defense = 15;
        this.lifeMax = 250;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.knockBackResist = 0.5f;
      }
      if (this.type == 20)
      {
        this.townNPC = true;
        this.friendly = true;
        this.name = "Dryad";
        this.width = 18;
        this.height = 40;
        this.aiStyle = 7;
        this.damage = 10;
        this.defense = 15;
        this.lifeMax = 250;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.knockBackResist = 0.5f;
      }
      if (this.type == 21)
      {
        this.name = "Skeleton";
        this.width = 18;
        this.height = 40;
        this.aiStyle = 3;
        this.damage = 20;
        this.defense = 8;
        this.lifeMax = 60;
        this.soundHit = 2;
        this.soundKilled = 2;
        this.knockBackResist = 0.5f;
        this.value = 250f;
      }
      if (this.type == 22)
      {
        this.townNPC = true;
        this.friendly = true;
        this.name = "Guide";
        this.width = 18;
        this.height = 40;
        this.aiStyle = 7;
        this.damage = 10;
        this.defense = 100;
        this.lifeMax = 250;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.knockBackResist = 0.5f;
      }
      if (this.type == 23)
      {
        this.name = "Meteor Head";
        this.width = 22;
        this.height = 22;
        this.aiStyle = 5;
        this.damage = 25;
        this.defense = 10;
        this.lifeMax = 50;
        this.soundHit = 3;
        this.soundKilled = 3;
        this.noGravity = true;
        this.noTileCollide = true;
        this.value = 300f;
      }
      else if (this.type == 24)
      {
        this.name = "Fire Imp";
        this.width = 18;
        this.height = 40;
        this.aiStyle = 8;
        this.damage = 30;
        this.defense = 20;
        this.lifeMax = 80;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.knockBackResist = 0.5f;
        this.value = 800f;
      }
      if (this.type == 25)
      {
        this.name = "Burning Sphere";
        this.width = 16;
        this.height = 16;
        this.aiStyle = 9;
        this.damage = 25;
        this.defense = 0;
        this.lifeMax = 8;
        this.soundHit = 3;
        this.soundKilled = 3;
        this.noGravity = true;
        this.noTileCollide = true;
        this.alpha = 100;
      }
      if (this.type == 26)
      {
        this.name = "Goblin Peon";
        this.width = 18;
        this.height = 40;
        this.aiStyle = 3;
        this.damage = 15;
        this.defense = 5;
        this.lifeMax = 80;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.knockBackResist = 0.7f;
        this.value = 250f;
      }
      if (this.type == 27)
      {
        this.name = "Goblin Thief";
        this.width = 18;
        this.height = 40;
        this.aiStyle = 3;
        this.damage = 25;
        this.defense = 10;
        this.lifeMax = 142;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.knockBackResist = 0.5f;
        this.value = 600f;
      }
      if (this.type == 28)
      {
        this.name = "Goblin Warrior";
        this.width = 18;
        this.height = 40;
        this.aiStyle = 3;
        this.damage = 30;
        this.defense = 15;
        this.lifeMax = 150;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.knockBackResist = 0.3f;
        this.value = 500f;
      }
      else if (this.type == 29)
      {
        this.name = "Goblin Sorcerer";
        this.width = 18;
        this.height = 40;
        this.aiStyle = 8;
        this.damage = 20;
        this.defense = 5;
        this.lifeMax = 80;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.knockBackResist = 0.6f;
        this.value = 800f;
      }
      else if (this.type == 30)
      {
        this.name = "Chaos Ball";
        this.width = 16;
        this.height = 16;
        this.aiStyle = 9;
        this.damage = 20;
        this.defense = 0;
        this.lifeMax = 8;
        this.soundHit = 3;
        this.soundKilled = 3;
        this.noGravity = true;
        this.noTileCollide = true;
        this.alpha = 100;
      }
      else if (this.type == 31)
      {
        this.name = "Angry Bones";
        this.width = 18;
        this.height = 40;
        this.aiStyle = 3;
        this.damage = 30;
        this.defense = 10;
        this.lifeMax = 100;
        this.soundHit = 2;
        this.soundKilled = 2;
        this.knockBackResist = 0.7f;
        this.value = 500f;
      }
      else if (this.type == 32)
      {
        this.name = "Dark Caster";
        this.width = 18;
        this.height = 40;
        this.aiStyle = 8;
        this.damage = 20;
        this.defense = 5;
        this.lifeMax = 80;
        this.soundHit = 2;
        this.soundKilled = 2;
        this.knockBackResist = 0.6f;
        this.value = 800f;
      }
      else if (this.type == 33)
      {
        this.name = "Water Sphere";
        this.width = 16;
        this.height = 16;
        this.aiStyle = 9;
        this.damage = 20;
        this.defense = 0;
        this.lifeMax = 8;
        this.soundHit = 3;
        this.soundKilled = 3;
        this.noGravity = true;
        this.noTileCollide = true;
        this.alpha = 100;
      }
      if (this.type == 34)
      {
        this.name = "Burning Skull";
        this.width = 26;
        this.height = 28;
        this.aiStyle = 10;
        this.damage = 25;
        this.defense = 15;
        this.lifeMax = 70;
        this.soundHit = 2;
        this.soundKilled = 2;
        this.noGravity = true;
        this.value = 300f;
        this.knockBackResist = 1.2f;
      }
      if (this.type == 35)
      {
        this.name = "Skeletron Head";
        this.width = 80;
        this.height = 102;
        this.aiStyle = 11;
        this.damage = 35;
        this.defense = 30;
        this.lifeMax = 3000;
        this.soundHit = 2;
        this.soundKilled = 2;
        this.noGravity = true;
        this.noTileCollide = true;
        this.value = 50000f;
        this.knockBackResist = 0.0f;
        this.boss = true;
      }
      if (this.type == 36)
      {
        this.name = "Skeletron Hand";
        this.width = 52;
        this.height = 52;
        this.aiStyle = 12;
        this.damage = 30;
        this.defense = 35;
        this.lifeMax = 1000;
        this.soundHit = 2;
        this.soundKilled = 2;
        this.noGravity = true;
        this.noTileCollide = true;
        this.knockBackResist = 0.0f;
      }
      if (this.type == 37)
      {
        this.townNPC = true;
        this.friendly = true;
        this.name = "Old Man";
        this.width = 18;
        this.height = 40;
        this.aiStyle = 7;
        this.damage = 10;
        this.defense = 100;
        this.lifeMax = 250;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.knockBackResist = 0.5f;
      }
      if (this.type == 38)
      {
        this.townNPC = true;
        this.friendly = true;
        this.name = "Demolitionist";
        this.width = 18;
        this.height = 40;
        this.aiStyle = 7;
        this.damage = 10;
        this.defense = 15;
        this.lifeMax = 250;
        this.soundHit = 1;
        this.soundKilled = 1;
        this.knockBackResist = 0.5f;
      }
      if (this.type == 39)
      {
        this.name = "Bone Serpent Head";
        this.width = 22;
        this.height = 22;
        this.aiStyle = 6;
        this.damage = 40;
        this.defense = 10;
        this.lifeMax = 120;
        this.soundHit = 2;
        this.soundKilled = 2;
        this.noGravity = true;
        this.noTileCollide = true;
        this.knockBackResist = 0.0f;
        this.behindTiles = true;
        this.value = 1000f;
      }
      if (this.type == 40)
      {
        this.name = "Bone Serpent Body";
        this.width = 22;
        this.height = 22;
        this.aiStyle = 6;
        this.damage = 30;
        this.defense = 12;
        this.lifeMax = 150;
        this.soundHit = 2;
        this.soundKilled = 2;
        this.noGravity = true;
        this.noTileCollide = true;
        this.knockBackResist = 0.0f;
        this.behindTiles = true;
        this.value = 1000f;
      }
      if (this.type == 41)
      {
        this.name = "Bone Serpent Tail";
        this.width = 22;
        this.height = 22;
        this.aiStyle = 6;
        this.damage = 20;
        this.defense = 18;
        this.lifeMax = 200;
        this.soundHit = 2;
        this.soundKilled = 2;
        this.noGravity = true;
        this.noTileCollide = true;
        this.knockBackResist = 0.0f;
        this.behindTiles = true;
        this.value = 1000f;
      }
      if (this.type == 42)
      {
        this.name = "Hornet";
        this.width = 34;
        this.height = 32;
        this.aiStyle = 2;
        this.damage = 30;
        this.defense = 8;
        this.lifeMax = 100;
        this.soundHit = 1;
        this.knockBackResist = 0.8f;
        this.soundKilled = 1;
        this.value = 750f;
      }
      if (this.type == 43)
      {
        this.noGravity = true;
        this.name = "Man Eater";
        this.width = 30;
        this.height = 30;
        this.aiStyle = 13;
        this.damage = 35;
        this.defense = 10;
        this.lifeMax = 200;
        this.soundHit = 1;
        this.knockBackResist = 0.7f;
        this.soundKilled = 1;
        this.value = 750f;
      }
      this.frame = new Rectangle(0, 0, Game1.npcTexture[this.type].Width, Game1.npcTexture[this.type].Height / Game1.npcFrameCount[this.type]);
      this.width = (int) ((double) this.width * (double) this.scale);
      this.height = (int) ((double) this.height * (double) this.scale);
      this.life = this.lifeMax;
      if (!Game1.dumbAI)
        return;
      this.aiStyle = 0;
    }

    public void AI()
    {
      if (this.aiStyle == 0)
      {
        this.velocity.X *= 0.93f;
        if ((double) this.velocity.X <= -0.1 || (double) this.velocity.X >= 0.1)
          return;
        this.velocity.X = 0.0f;
      }
      else if (this.aiStyle == 1)
      {
        this.aiAction = 0;
        if ((double) this.ai[2] == 0.0)
        {
          this.ai[0] = -100f;
          this.ai[2] = 1f;
          this.TargetClosest();
        }
        if ((double) this.velocity.Y == 0.0)
        {
          if ((double) this.ai[3] == (double) this.position.X)
            this.direction *= -1;
          this.ai[3] = 0.0f;
          this.velocity.X *= 0.8f;
          if ((double) this.velocity.X > -0.1 && (double) this.velocity.X < 0.1)
            this.velocity.X = 0.0f;
          if (!Game1.dayTime || this.life != this.lifeMax || (double) this.position.Y > Game1.worldSurface * 16.0)
            ++this.ai[0];
          ++this.ai[0];
          if ((double) this.ai[0] >= 0.0)
          {
            if (!Game1.dayTime || this.life != this.lifeMax || (double) this.position.Y > Game1.worldSurface * 16.0)
              this.TargetClosest();
            if ((double) this.ai[1] == 2.0)
            {
              this.velocity.Y = -8f;
              this.velocity.X += (float) (3 * this.direction);
              this.ai[0] = -200f;
              this.ai[1] = 0.0f;
              this.ai[3] = this.position.X;
            }
            else
            {
              this.velocity.Y = -6f;
              this.velocity.X += (float) (2 * this.direction);
              this.ai[0] = -120f;
              ++this.ai[1];
            }
          }
          else
          {
            if ((double) this.ai[0] < -30.0)
              return;
            this.aiAction = 1;
          }
        }
        else
        {
          if (this.target >= 8 || (this.direction != 1 || (double) this.velocity.X >= 3.0) && (this.direction != -1 || (double) this.velocity.X <= -3.0))
            return;
          if (this.direction == -1 && (double) this.velocity.X < 0.1 || this.direction == 1 && (double) this.velocity.X > -0.1)
            this.velocity.X += 0.2f * (float) this.direction;
          else
            this.velocity.X *= 0.93f;
        }
      }
      else if (this.aiStyle == 2)
      {
        this.noGravity = true;
        if (this.collideX)
        {
          this.velocity.X = this.oldVelocity.X * -0.5f;
          if (this.direction == -1 && (double) this.velocity.X > 0.0 && (double) this.velocity.X < 2.0)
            this.velocity.X = 2f;
          if (this.direction == 1 && (double) this.velocity.X < 0.0 && (double) this.velocity.X > -2.0)
            this.velocity.X = -2f;
        }
        if (this.collideY)
        {
          this.velocity.Y = this.oldVelocity.Y * -0.5f;
          if ((double) this.velocity.Y > 0.0 && (double) this.velocity.Y < 1.0)
            this.velocity.Y = 1f;
          if ((double) this.velocity.Y < 0.0 && (double) this.velocity.Y > -1.0)
            this.velocity.Y = -1f;
        }
        if (Game1.dayTime && (double) this.position.Y <= Game1.worldSurface * 16.0 && this.type == 2)
        {
          if (this.timeLeft > 10)
            this.timeLeft = 10;
          this.directionY = -1;
          if ((double) this.velocity.Y > 0.0)
            this.direction = 1;
          this.direction = -1;
          if ((double) this.velocity.X > 0.0)
            this.direction = 1;
        }
        else
          this.TargetClosest();
        if (this.direction == -1 && (double) this.velocity.X > -4.0)
        {
          this.velocity.X -= 0.1f;
          if ((double) this.velocity.X > 4.0)
            this.velocity.X -= 0.1f;
          else if ((double) this.velocity.X > 0.0)
            this.velocity.X += 0.05f;
          if ((double) this.velocity.X < -4.0)
            this.velocity.X = -4f;
        }
        else if (this.direction == 1 && (double) this.velocity.X < 4.0)
        {
          this.velocity.X += 0.1f;
          if ((double) this.velocity.X < -4.0)
            this.velocity.X += 0.1f;
          else if ((double) this.velocity.X < 0.0)
            this.velocity.X -= 0.05f;
          if ((double) this.velocity.X > 4.0)
            this.velocity.X = 4f;
        }
        if (this.directionY == -1 && (double) this.velocity.Y > -1.5)
        {
          this.velocity.Y -= 0.04f;
          if ((double) this.velocity.Y > 1.5)
            this.velocity.Y -= 0.05f;
          else if ((double) this.velocity.Y > 0.0)
            this.velocity.Y += 0.03f;
          if ((double) this.velocity.Y < -1.5)
            this.velocity.Y = -1.5f;
        }
        else if (this.directionY == 1 && (double) this.velocity.Y < 1.5)
        {
          this.velocity.Y += 0.04f;
          if ((double) this.velocity.Y < -1.5)
            this.velocity.Y += 0.05f;
          else if ((double) this.velocity.Y < 0.0)
            this.velocity.Y -= 0.03f;
          if ((double) this.velocity.Y > 1.5)
            this.velocity.Y = 1.5f;
        }
        if (this.type != 2 || Game1.rand.Next(40) != 0)
          return;
        int index = Dust.NewDust(new Vector2(this.position.X, this.position.Y + (float) this.height * 0.25f), this.width, (int) ((double) this.height * 0.5), 5, this.velocity.X, 2f);
        Game1.dust[index].velocity.X *= 0.5f;
        Game1.dust[index].velocity.Y *= 0.1f;
      }
      else if (this.aiStyle == 3)
      {
        int num = 60;
        bool flag1 = false;
        if ((double) this.velocity.Y == 0.0 && ((double) this.velocity.X > 0.0 && this.direction < 0 || (double) this.velocity.X < 0.0 && this.direction > 0))
          flag1 = true;
        if ((double) this.position.X == (double) this.oldPosition.X || (double) this.ai[3] >= (double) num || flag1)
          ++this.ai[3];
        else if ((double) Math.Abs(this.velocity.X) > 0.9 && (double) this.ai[3] > 0.0)
          --this.ai[3];
        if ((double) this.ai[3] > (double) (num * 10))
          this.ai[3] = 0.0f;
        if ((double) this.ai[3] == (double) num)
          this.netUpdate = true;
        if ((!Game1.dayTime || (double) this.position.Y > Game1.worldSurface * 16.0 || this.type == 26 || this.type == 27 || this.type == 28 || this.type == 31) && (double) this.ai[3] < (double) num)
        {
          if ((this.type == 3 || this.type == 21 || this.type == 31) && Game1.rand.Next(1000) == 0)
            Game1.PlaySound(14, (int) this.position.X, (int) this.position.Y);
          this.TargetClosest();
        }
        else
        {
          if (this.timeLeft > 10)
            this.timeLeft = 10;
          if ((double) this.velocity.X == 0.0)
          {
            if ((double) this.velocity.Y == 0.0)
            {
              ++this.ai[0];
              if ((double) this.ai[0] >= 2.0)
              {
                this.direction *= -1;
                this.spriteDirection = this.direction;
                this.ai[0] = 0.0f;
              }
            }
          }
          else
            this.ai[0] = 0.0f;
          if (this.direction == 0)
            this.direction = 1;
        }
        if (this.type == 27)
        {
          if ((double) this.velocity.X < -2.0 || (double) this.velocity.X > 2.0)
          {
            if ((double) this.velocity.Y == 0.0)
              this.velocity *= 0.8f;
          }
          else if ((double) this.velocity.X < 2.0 && this.direction == 1)
          {
            this.velocity.X += 0.07f;
            if ((double) this.velocity.X > 2.0)
              this.velocity.X = 2f;
          }
          else if ((double) this.velocity.X > -2.0 && this.direction == -1)
          {
            this.velocity.X -= 0.07f;
            if ((double) this.velocity.X < -2.0)
              this.velocity.X = -2f;
          }
        }
        else if (this.type == 21 || this.type == 26 || this.type == 31)
        {
          if ((double) this.velocity.X < -1.5 || (double) this.velocity.X > 1.5)
          {
            if ((double) this.velocity.Y == 0.0)
              this.velocity *= 0.8f;
          }
          else if ((double) this.velocity.X < 1.5 && this.direction == 1)
          {
            this.velocity.X += 0.07f;
            if ((double) this.velocity.X > 1.5)
              this.velocity.X = 1.5f;
          }
          else if ((double) this.velocity.X > -1.5 && this.direction == -1)
          {
            this.velocity.X -= 0.07f;
            if ((double) this.velocity.X < -1.5)
              this.velocity.X = -1.5f;
          }
        }
        else if ((double) this.velocity.X < -1.0 || (double) this.velocity.X > 1.0)
        {
          if ((double) this.velocity.Y == 0.0)
            this.velocity *= 0.8f;
        }
        else if ((double) this.velocity.X < 1.0 && this.direction == 1)
        {
          this.velocity.X += 0.07f;
          if ((double) this.velocity.X > 1.0)
            this.velocity.X = 1f;
        }
        else if ((double) this.velocity.X > -1.0 && this.direction == -1)
        {
          this.velocity.X -= 0.07f;
          if ((double) this.velocity.X < -1.0)
            this.velocity.X = -1f;
        }
        if ((double) this.velocity.Y == 0.0)
        {
          int index1 = (int) (((double) this.position.X + (double) (this.width / 2) + (double) (15 * this.direction)) / 16.0);
          int index2 = (int) (((double) this.position.Y + (double) this.height - 16.0) / 16.0);
          if (Game1.tile[index1, index2] == null)
            Game1.tile[index1, index2] = new Tile();
          if (Game1.tile[index1, index2 - 1] == null)
            Game1.tile[index1, index2 - 1] = new Tile();
          if (Game1.tile[index1, index2 - 2] == null)
            Game1.tile[index1, index2 - 2] = new Tile();
          if (Game1.tile[index1, index2 - 3] == null)
            Game1.tile[index1, index2 - 3] = new Tile();
          if (Game1.tile[index1, index2 + 1] == null)
            Game1.tile[index1, index2 + 1] = new Tile();
          if (Game1.tile[index1 + this.direction, index2 - 1] == null)
            Game1.tile[index1 + this.direction, index2 - 1] = new Tile();
          if (Game1.tile[index1 + this.direction, index2 + 1] == null)
            Game1.tile[index1 + this.direction, index2 + 1] = new Tile();
          if (Game1.tile[index1, index2 - 1].active && Game1.tile[index1, index2 - 1].type == (byte) 10)
          {
            ++this.ai[2];
            this.ai[3] = 0.0f;
            if ((double) this.ai[2] < 60.0)
              return;
            if (!Game1.bloodMoon && this.type == 3)
              this.ai[1] = 0.0f;
            this.velocity.X = 0.5f * (float) -this.direction;
            ++this.ai[1];
            if (this.type == 27)
              ++this.ai[1];
            if (this.type == 31)
              this.ai[1] += 6f;
            this.ai[2] = 0.0f;
            bool flag2 = false;
            if ((double) this.ai[1] >= 10.0)
            {
              flag2 = true;
              this.ai[1] = 10f;
            }
            WorldGen.KillTile(index1, index2 - 1, true);
            if ((Game1.netMode != 1 || !flag2) && flag2 && Game1.netMode != 1)
            {
              if (this.type == 26)
              {
                WorldGen.KillTile(index1, index2 - 1);
                if (Game1.netMode == 2)
                  NetMessage.SendData(17, number2: (float) index1, number3: (float) (index2 - 1));
              }
              else
              {
                bool flag3 = WorldGen.OpenDoor(index1, index2, this.direction);
                if (!flag3)
                {
                  this.ai[3] = (float) num;
                  this.netUpdate = true;
                }
                if (Game1.netMode == 2 && flag3)
                  NetMessage.SendData(19, number2: (float) index1, number3: (float) index2, number4: (float) this.direction);
              }
            }
          }
          else
          {
            if ((double) this.velocity.X < 0.0 && this.spriteDirection == -1 || (double) this.velocity.X > 0.0 && this.spriteDirection == 1)
            {
              if (Game1.tile[index1, index2 - 2].active && Game1.tileSolid[(int) Game1.tile[index1, index2 - 2].type])
              {
                if (Game1.tile[index1, index2 - 3].active && Game1.tileSolid[(int) Game1.tile[index1, index2 - 3].type])
                {
                  this.velocity.Y = -8f;
                  this.netUpdate = true;
                }
                else
                {
                  this.velocity.Y = -7f;
                  this.netUpdate = true;
                }
              }
              else if (Game1.tile[index1, index2 - 1].active && Game1.tileSolid[(int) Game1.tile[index1, index2 - 1].type])
              {
                this.velocity.Y = -6f;
                this.netUpdate = true;
              }
              else if (Game1.tile[index1, index2].active && Game1.tileSolid[(int) Game1.tile[index1, index2].type])
              {
                this.velocity.Y = -5f;
                this.netUpdate = true;
              }
              else if (this.directionY < 0 && (!Game1.tile[index1, index2 + 1].active || !Game1.tileSolid[(int) Game1.tile[index1, index2 + 1].type]) && (!Game1.tile[index1 + this.direction, index2 + 1].active || !Game1.tileSolid[(int) Game1.tile[index1 + this.direction, index2 + 1].type]))
              {
                this.velocity.Y = -8f;
                this.velocity.X *= 1.5f;
                this.netUpdate = true;
              }
              else
              {
                this.ai[1] = 0.0f;
                this.ai[2] = 0.0f;
              }
            }
            if (this.type == 31 && (double) this.velocity.Y == 0.0 && (double) Math.Abs((float) ((double) this.position.X + (double) (this.width / 2) - ((double) Game1.player[this.target].position.X - (double) (Game1.player[this.target].width / 2)))) < 100.0 && (double) Math.Abs((float) ((double) this.position.Y + (double) (this.height / 2) - ((double) Game1.player[this.target].position.Y - (double) (Game1.player[this.target].height / 2)))) < 50.0 && (this.direction > 0 && (double) this.velocity.X > 1.0 || this.direction < 0 && (double) this.velocity.X < -1.0))
            {
              this.velocity.X *= 2f;
              if ((double) this.velocity.X > 3.0)
                this.velocity.X = 3f;
              if ((double) this.velocity.X < -3.0)
                this.velocity.X = -3f;
              this.velocity.Y = -4f;
              this.netUpdate = true;
            }
          }
        }
        else
        {
          this.ai[1] = 0.0f;
          this.ai[2] = 0.0f;
        }
      }
      else if (this.aiStyle == 4)
      {
        if (this.target < 0 || this.target == 8 || Game1.player[this.target].dead || !Game1.player[this.target].active)
          this.TargetClosest();
        float x = this.position.X + (float) (this.width / 2) - Game1.player[this.target].position.X - (float) (Game1.player[this.target].width / 2);
        float num1 = (float) Math.Atan2((double) ((float) ((double) this.position.Y + (double) this.height - 59.0) - Game1.player[this.target].position.Y - (float) (Game1.player[this.target].height / 2)), (double) x) + 1.57f;
        if ((double) num1 < 0.0)
          num1 += 6.283f;
        else if ((double) num1 > 6.283)
          num1 -= 6.283f;
        float num2 = 0.0f;
        if ((double) this.ai[0] == 0.0 && (double) this.ai[1] == 0.0)
          num2 = 0.02f;
        if ((double) this.ai[0] == 0.0 && (double) this.ai[1] == 2.0 && (double) this.ai[2] > 40.0)
          num2 = 0.05f;
        if ((double) this.ai[0] == 3.0 && (double) this.ai[1] == 0.0)
          num2 = 0.05f;
        if ((double) this.ai[0] == 3.0 && (double) this.ai[1] == 2.0 && (double) this.ai[2] > 40.0)
          num2 = 0.08f;
        if ((double) this.rotation < (double) num1)
        {
          if ((double) num1 - (double) this.rotation > 3.1415)
            this.rotation -= num2;
          else
            this.rotation += num2;
        }
        else if ((double) this.rotation > (double) num1)
        {
          if ((double) this.rotation - (double) num1 > 3.1415)
            this.rotation += num2;
          else
            this.rotation -= num2;
        }
        if ((double) this.rotation > (double) num1 - (double) num2 && (double) this.rotation < (double) num1 + (double) num2)
          this.rotation = num1;
        if ((double) this.rotation < 0.0)
          this.rotation += 6.283f;
        else if ((double) this.rotation > 6.283)
          this.rotation -= 6.283f;
        if ((double) this.rotation > (double) num1 - (double) num2 && (double) this.rotation < (double) num1 + (double) num2)
          this.rotation = num1;
        if (Game1.rand.Next(5) == 0)
        {
          int index = Dust.NewDust(new Vector2(this.position.X, this.position.Y + (float) this.height * 0.25f), this.width, (int) ((double) this.height * 0.5), 5, this.velocity.X, 2f);
          Game1.dust[index].velocity.X *= 0.5f;
          Game1.dust[index].velocity.Y *= 0.1f;
        }
        if (Game1.dayTime)
        {
          this.velocity.Y -= 0.04f;
          if (this.timeLeft <= 10)
            return;
          this.timeLeft = 10;
        }
        else if ((double) this.ai[0] == 0.0)
        {
          if ((double) this.ai[1] == 0.0)
          {
            float num3 = 5f;
            float num4 = 0.04f;
            Vector2 vector2_1 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
            float num5 = Game1.player[this.target].position.X + (float) (Game1.player[this.target].width / 2) - vector2_1.X;
            float num6 = (float) ((double) Game1.player[this.target].position.Y + (double) (Game1.player[this.target].height / 2) - 200.0) - vector2_1.Y;
            float num7 = (float) Math.Sqrt((double) num5 * (double) num5 + (double) num6 * (double) num6);
            float num8 = num7;
            float num9 = num3 / num7;
            float num10 = num5 * num9;
            float num11 = num6 * num9;
            if ((double) this.velocity.X < (double) num10)
            {
              this.velocity.X += num4;
              if ((double) this.velocity.X < 0.0 && (double) num10 > 0.0)
                this.velocity.X += num4;
            }
            else if ((double) this.velocity.X > (double) num10)
            {
              this.velocity.X -= num4;
              if ((double) this.velocity.X > 0.0 && (double) num10 < 0.0)
                this.velocity.X -= num4;
            }
            if ((double) this.velocity.Y < (double) num11)
            {
              this.velocity.Y += num4;
              if ((double) this.velocity.Y < 0.0 && (double) num11 > 0.0)
                this.velocity.Y += num4;
            }
            else if ((double) this.velocity.Y > (double) num11)
            {
              this.velocity.Y -= num4;
              if ((double) this.velocity.Y > 0.0 && (double) num11 < 0.0)
                this.velocity.Y -= num4;
            }
            ++this.ai[2];
            if ((double) this.ai[2] >= 600.0)
            {
              this.ai[1] = 1f;
              this.ai[2] = 0.0f;
              this.ai[3] = 0.0f;
              this.target = 8;
              this.netUpdate = true;
            }
            else if ((double) this.position.Y + (double) this.height < (double) Game1.player[this.target].position.Y && (double) num8 < 500.0)
            {
              if (!Game1.player[this.target].dead)
                ++this.ai[3];
              if ((double) this.ai[3] >= 90.0)
              {
                this.ai[3] = 0.0f;
                this.rotation = num1;
                float num12 = 5f;
                float num13 = Game1.player[this.target].position.X + (float) (Game1.player[this.target].width / 2) - vector2_1.X;
                float num14 = Game1.player[this.target].position.Y + (float) (Game1.player[this.target].height / 2) - vector2_1.Y;
                float num15 = (float) Math.Sqrt((double) num13 * (double) num13 + (double) num14 * (double) num14);
                float num16 = num12 / num15;
                Vector2 Position = vector2_1;
                Vector2 vector2_2;
                vector2_2.X = num13 * num16;
                vector2_2.Y = num14 * num16;
                Position.X += vector2_2.X * 10f;
                Position.Y += vector2_2.Y * 10f;
                if (Game1.netMode != 1)
                {
                  int number = NPC.NewNPC((int) Position.X, (int) Position.Y, 5);
                  Game1.npc[number].velocity.X = vector2_2.X;
                  Game1.npc[number].velocity.Y = vector2_2.Y;
                  if (Game1.netMode == 2 && number < 1000)
                    NetMessage.SendData(23, number: number);
                }
                Game1.PlaySound(3, (int) Position.X, (int) Position.Y);
                for (int index = 0; index < 10; ++index)
                  Dust.NewDust(Position, 20, 20, 5, vector2_2.X * 0.4f, vector2_2.Y * 0.4f);
              }
            }
          }
          else if ((double) this.ai[1] == 1.0)
          {
            this.rotation = num1;
            float num17 = 7f;
            Vector2 vector2 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
            float num18 = Game1.player[this.target].position.X + (float) (Game1.player[this.target].width / 2) - vector2.X;
            float num19 = Game1.player[this.target].position.Y + (float) (Game1.player[this.target].height / 2) - vector2.Y;
            float num20 = (float) Math.Sqrt((double) num18 * (double) num18 + (double) num19 * (double) num19);
            float num21 = num17 / num20;
            this.velocity.X = num18 * num21;
            this.velocity.Y = num19 * num21;
            this.ai[1] = 2f;
          }
          else if ((double) this.ai[1] == 2.0)
          {
            ++this.ai[2];
            if ((double) this.ai[2] >= 40.0)
            {
              this.velocity.X *= 0.98f;
              this.velocity.Y *= 0.98f;
              if ((double) this.velocity.X > -0.1 && (double) this.velocity.X < 0.1)
                this.velocity.X = 0.0f;
              if ((double) this.velocity.Y > -0.1 && (double) this.velocity.Y < 0.1)
                this.velocity.Y = 0.0f;
            }
            else
              this.rotation = (float) Math.Atan2((double) this.velocity.Y, (double) this.velocity.X) - 1.57f;
            if ((double) this.ai[2] >= 120.0)
            {
              ++this.ai[3];
              this.ai[2] = 0.0f;
              this.target = 8;
              this.rotation = num1;
              if ((double) this.ai[3] >= 3.0)
              {
                this.ai[1] = 0.0f;
                this.ai[3] = 0.0f;
              }
              else
                this.ai[1] = 1f;
            }
          }
          if ((double) this.life >= (double) this.lifeMax * 0.5)
            return;
          this.ai[0] = 1f;
          this.ai[1] = 0.0f;
          this.ai[2] = 0.0f;
          this.ai[3] = 0.0f;
          this.netUpdate = true;
        }
        else if ((double) this.ai[0] == 1.0 || (double) this.ai[0] == 2.0)
        {
          if ((double) this.ai[0] == 1.0)
          {
            this.ai[2] += 0.005f;
            if ((double) this.ai[2] > 0.5)
              this.ai[2] = 0.5f;
          }
          else
          {
            this.ai[2] -= 0.005f;
            if ((double) this.ai[2] < 0.0)
              this.ai[2] = 0.0f;
          }
          this.rotation += this.ai[2];
          ++this.ai[1];
          if ((double) this.ai[1] == 100.0)
          {
            ++this.ai[0];
            this.ai[1] = 0.0f;
            if ((double) this.ai[0] == 3.0)
            {
              this.ai[2] = 0.0f;
            }
            else
            {
              Game1.PlaySound(3, (int) this.position.X, (int) this.position.Y);
              for (int index = 0; index < 2; ++index)
              {
                Gore.NewGore(this.position, new Vector2((float) Game1.rand.Next(-30, 31) * 0.2f, (float) Game1.rand.Next(-30, 31) * 0.2f), 8);
                Gore.NewGore(this.position, new Vector2((float) Game1.rand.Next(-30, 31) * 0.2f, (float) Game1.rand.Next(-30, 31) * 0.2f), 7);
                Gore.NewGore(this.position, new Vector2((float) Game1.rand.Next(-30, 31) * 0.2f, (float) Game1.rand.Next(-30, 31) * 0.2f), 6);
              }
              for (int index = 0; index < 20; ++index)
                Dust.NewDust(this.position, this.width, this.height, 5, (float) Game1.rand.Next(-30, 31) * 0.2f, (float) Game1.rand.Next(-30, 31) * 0.2f);
              Game1.PlaySound(15, (int) this.position.X, (int) this.position.Y, 0);
            }
          }
          Dust.NewDust(this.position, this.width, this.height, 5, (float) Game1.rand.Next(-30, 31) * 0.2f, (float) Game1.rand.Next(-30, 31) * 0.2f);
          this.velocity.X *= 0.98f;
          this.velocity.Y *= 0.98f;
          if ((double) this.velocity.X > -0.1 && (double) this.velocity.X < 0.1)
            this.velocity.X = 0.0f;
          if ((double) this.velocity.Y <= -0.1 || (double) this.velocity.Y >= 0.1)
            return;
          this.velocity.Y = 0.0f;
        }
        else
        {
          this.damage = 36;
          if ((double) this.ai[1] == 0.0)
          {
            float num22 = 6f;
            float num23 = 0.07f;
            Vector2 vector2 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
            float num24 = Game1.player[this.target].position.X + (float) (Game1.player[this.target].width / 2) - vector2.X;
            float num25 = (float) ((double) Game1.player[this.target].position.Y + (double) (Game1.player[this.target].height / 2) - 120.0) - vector2.Y;
            float num26 = (float) Math.Sqrt((double) num24 * (double) num24 + (double) num25 * (double) num25);
            float num27 = num22 / num26;
            float num28 = num24 * num27;
            float num29 = num25 * num27;
            if ((double) this.velocity.X < (double) num28)
            {
              this.velocity.X += num23;
              if ((double) this.velocity.X < 0.0 && (double) num28 > 0.0)
                this.velocity.X += num23;
            }
            else if ((double) this.velocity.X > (double) num28)
            {
              this.velocity.X -= num23;
              if ((double) this.velocity.X > 0.0 && (double) num28 < 0.0)
                this.velocity.X -= num23;
            }
            if ((double) this.velocity.Y < (double) num29)
            {
              this.velocity.Y += num23;
              if ((double) this.velocity.Y < 0.0 && (double) num29 > 0.0)
                this.velocity.Y += num23;
            }
            else if ((double) this.velocity.Y > (double) num29)
            {
              this.velocity.Y -= num23;
              if ((double) this.velocity.Y > 0.0 && (double) num29 < 0.0)
                this.velocity.Y -= num23;
            }
            ++this.ai[2];
            if ((double) this.ai[2] >= 200.0)
            {
              this.ai[1] = 1f;
              this.ai[2] = 0.0f;
              this.ai[3] = 0.0f;
              this.target = 8;
              this.netUpdate = true;
            }
          }
          else if ((double) this.ai[1] == 1.0)
          {
            Game1.PlaySound(15, (int) this.position.X, (int) this.position.Y, 0);
            this.rotation = num1;
            float num30 = 8f;
            Vector2 vector2 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
            float num31 = Game1.player[this.target].position.X + (float) (Game1.player[this.target].width / 2) - vector2.X;
            float num32 = Game1.player[this.target].position.Y + (float) (Game1.player[this.target].height / 2) - vector2.Y;
            float num33 = (float) Math.Sqrt((double) num31 * (double) num31 + (double) num32 * (double) num32);
            float num34 = num30 / num33;
            this.velocity.X = num31 * num34;
            this.velocity.Y = num32 * num34;
            this.ai[1] = 2f;
          }
          else if ((double) this.ai[1] == 2.0)
          {
            ++this.ai[2];
            if ((double) this.ai[2] >= 40.0)
            {
              this.velocity.X *= 0.97f;
              this.velocity.Y *= 0.97f;
              if ((double) this.velocity.X > -0.1 && (double) this.velocity.X < 0.1)
                this.velocity.X = 0.0f;
              if ((double) this.velocity.Y > -0.1 && (double) this.velocity.Y < 0.1)
                this.velocity.Y = 0.0f;
            }
            else
              this.rotation = (float) Math.Atan2((double) this.velocity.Y, (double) this.velocity.X) - 1.57f;
            if ((double) this.ai[2] >= 100.0)
            {
              ++this.ai[3];
              this.ai[2] = 0.0f;
              this.target = 8;
              this.rotation = num1;
              if ((double) this.ai[3] >= 3.0)
              {
                this.ai[1] = 0.0f;
                this.ai[3] = 0.0f;
              }
              else
                this.ai[1] = 1f;
            }
          }
        }
      }
      else if (this.aiStyle == 5)
      {
        if (this.target < 0 || this.target == 8 || Game1.player[this.target].dead)
          this.TargetClosest();
        float num35 = 6f;
        float num36 = 0.05f;
        if (this.type == 6)
        {
          num35 = 4f;
          num36 = 0.02f;
        }
        else if (this.type == 23)
        {
          num35 = 4f;
          num36 = 0.03f;
        }
        Vector2 vector2 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
        float num37 = Game1.player[this.target].position.X + (float) (Game1.player[this.target].width / 2) - vector2.X;
        float num38 = Game1.player[this.target].position.Y + (float) (Game1.player[this.target].height / 2) - vector2.Y;
        float num39 = (float) Math.Sqrt((double) num37 * (double) num37 + (double) num38 * (double) num38);
        float num40 = num35 / num39;
        float num41 = num37 * num40;
        float num42 = num38 * num40;
        if ((double) this.velocity.X < (double) num41)
        {
          this.velocity.X += num36;
          if ((double) this.velocity.X < 0.0 && (double) num41 > 0.0)
            this.velocity.X += num36;
        }
        else if ((double) this.velocity.X > (double) num41)
        {
          this.velocity.X -= num36;
          if ((double) this.velocity.X > 0.0 && (double) num41 < 0.0)
            this.velocity.X -= num36;
        }
        if ((double) this.velocity.Y < (double) num42)
        {
          this.velocity.Y += num36;
          if ((double) this.velocity.Y < 0.0 && (double) num42 > 0.0)
            this.velocity.Y += num36;
        }
        else if ((double) this.velocity.Y > (double) num42)
        {
          this.velocity.Y -= num36;
          if ((double) this.velocity.Y > 0.0 && (double) num42 < 0.0)
            this.velocity.Y -= num36;
        }
        this.rotation = this.type != 23 ? (float) Math.Atan2((double) this.velocity.Y, (double) this.velocity.X) - 1.57f : (float) Math.Atan2((double) this.velocity.Y, (double) this.velocity.X);
        if (this.type == 6 || this.type == 23)
        {
          if (this.collideX)
          {
            this.netUpdate = true;
            this.velocity.X = this.oldVelocity.X * -0.7f;
            if (this.direction == -1 && (double) this.velocity.X > 0.0 && (double) this.velocity.X < 2.0)
              this.velocity.X = 2f;
            if (this.direction == 1 && (double) this.velocity.X < 0.0 && (double) this.velocity.X > -2.0)
              this.velocity.X = -2f;
          }
          if (this.collideY)
          {
            this.netUpdate = true;
            this.velocity.Y = this.oldVelocity.Y * -0.7f;
            if ((double) this.velocity.Y > 0.0 && (double) this.velocity.Y < 2.0)
              this.velocity.Y = 2f;
            if ((double) this.velocity.Y < 0.0 && (double) this.velocity.Y > -2.0)
              this.velocity.Y = -2f;
          }
          if (this.type == 23)
          {
            int index = Dust.NewDust(new Vector2(this.position.X - this.velocity.X, this.position.Y - this.velocity.Y), this.width, this.height, 6, this.velocity.X * 0.2f, this.velocity.Y * 0.2f, 100, Scale: 2f);
            Game1.dust[index].noGravity = true;
            Game1.dust[index].velocity.X *= 0.3f;
            Game1.dust[index].velocity.Y *= 0.3f;
          }
          else if (Game1.rand.Next(20) == 0)
          {
            int index = Dust.NewDust(new Vector2(this.position.X, this.position.Y + (float) this.height * 0.25f), this.width, (int) ((double) this.height * 0.5), 18, this.velocity.X, 2f, this.alpha, this.color, this.scale);
            Game1.dust[index].velocity.X *= 0.5f;
            Game1.dust[index].velocity.Y *= 0.1f;
          }
        }
        else if (Game1.rand.Next(40) == 0)
        {
          int index = Dust.NewDust(new Vector2(this.position.X, this.position.Y + (float) this.height * 0.25f), this.width, (int) ((double) this.height * 0.5), 5, this.velocity.X, 2f);
          Game1.dust[index].velocity.X *= 0.5f;
          Game1.dust[index].velocity.Y *= 0.1f;
        }
        if ((!Game1.dayTime || this.type == 6 || this.type == 23) && !Game1.player[this.target].dead)
          return;
        this.velocity.Y -= num36 * 2f;
        if (this.timeLeft > 10)
          this.timeLeft = 10;
      }
      else if (this.aiStyle == 6)
      {
        if (this.target < 0 || this.target == 8 || Game1.player[this.target].dead)
          this.TargetClosest();
        if (Game1.netMode != 1)
        {
          if ((this.type == 7 || this.type == 8 || this.type == 10 || this.type == 11 || this.type == 13 || this.type == 14 || this.type == 39 || this.type == 40) && (double) this.ai[0] == 0.0)
          {
            if (this.type == 7 || this.type == 10 || this.type == 13 || this.type == 39)
            {
              this.ai[2] = 10f;
              if (this.type == 10)
                this.ai[2] = 5f;
              if (this.type == 13)
                this.ai[2] = 50f;
              if (this.type == 39)
                this.ai[2] = 15f;
              this.ai[0] = (float) NPC.NewNPC((int) this.position.X, (int) this.position.Y, this.type + 1, this.whoAmI);
            }
            else
              this.ai[0] = this.type != 8 && this.type != 11 && this.type != 14 && this.type != 40 || (double) this.ai[2] <= 0.0 ? (float) NPC.NewNPC((int) this.position.X, (int) this.position.Y, this.type + 1, this.whoAmI) : (float) NPC.NewNPC((int) this.position.X, (int) this.position.Y, this.type, this.whoAmI);
            Game1.npc[(int) this.ai[0]].ai[1] = (float) this.whoAmI;
            Game1.npc[(int) this.ai[0]].ai[2] = this.ai[2] - 1f;
            this.netUpdate = true;
          }
          if ((this.type == 8 || this.type == 9 || this.type == 11 || this.type == 12 || this.type == 40 || this.type == 41) && !Game1.npc[(int) this.ai[1]].active)
          {
            this.life = 0;
            this.HitEffect();
            this.active = false;
          }
          if ((this.type == 7 || this.type == 8 || this.type == 10 || this.type == 11 || this.type == 39 || this.type == 40) && !Game1.npc[(int) this.ai[0]].active)
          {
            this.life = 0;
            this.HitEffect();
            this.active = false;
          }
          if (this.type == 13 || this.type == 14 || this.type == 15)
          {
            if (!Game1.npc[(int) this.ai[1]].active && !Game1.npc[(int) this.ai[0]].active)
            {
              this.life = 0;
              this.HitEffect();
              this.active = false;
            }
            if (this.type == 13 && !Game1.npc[(int) this.ai[0]].active)
            {
              this.life = 0;
              this.HitEffect();
              this.active = false;
            }
            if (this.type == 15 && !Game1.npc[(int) this.ai[1]].active)
            {
              this.life = 0;
              this.HitEffect();
              this.active = false;
            }
            if (this.type == 14 && !Game1.npc[(int) this.ai[1]].active)
            {
              this.type = 13;
              int whoAmI = this.whoAmI;
              int life = this.life;
              float num = this.ai[0];
              this.SetDefaults(this.type);
              this.life = life;
              if (this.life > this.lifeMax)
                this.life = this.lifeMax;
              this.ai[0] = num;
              this.TargetClosest();
              this.netUpdate = true;
              this.whoAmI = whoAmI;
            }
            if (this.type == 14 && !Game1.npc[(int) this.ai[0]].active)
            {
              int life = this.life;
              int whoAmI = this.whoAmI;
              float num = this.ai[1];
              this.SetDefaults(this.type);
              this.life = life;
              if (this.life > this.lifeMax)
                this.life = this.lifeMax;
              this.ai[1] = num;
              this.TargetClosest();
              this.netUpdate = true;
              this.whoAmI = whoAmI;
            }
            if (this.life == 0)
            {
              bool flag = true;
              for (int index = 0; index < 1000; ++index)
              {
                if (Game1.npc[index].active && (Game1.npc[index].type == 13 || Game1.npc[index].type == 14 || Game1.npc[index].type == 15))
                {
                  flag = false;
                  break;
                }
              }
              if (flag)
              {
                this.boss = true;
                this.NPCLoot();
              }
            }
          }
          if (!this.active && Game1.netMode == 2)
            NetMessage.SendData(28, number: this.whoAmI, number2: -1f);
        }
        int num43 = (int) ((double) this.position.X / 16.0) - 1;
        int num44 = (int) (((double) this.position.X + (double) this.width) / 16.0) + 2;
        int num45 = (int) ((double) this.position.Y / 16.0) - 1;
        int num46 = (int) (((double) this.position.Y + (double) this.height) / 16.0) + 2;
        if (num43 < 0)
          num43 = 0;
        if (num44 > Game1.maxTilesX)
          num44 = Game1.maxTilesX;
        if (num45 < 0)
          num45 = 0;
        if (num46 > Game1.maxTilesY)
          num46 = Game1.maxTilesY;
        bool flag4 = false;
        for (int i = num43; i < num44; ++i)
        {
          for (int j = num45; j < num46; ++j)
          {
            if (Game1.tile[i, j] != null && (Game1.tile[i, j].active && (Game1.tileSolid[(int) Game1.tile[i, j].type] || Game1.tileSolidTop[(int) Game1.tile[i, j].type] && Game1.tile[i, j].frameY == (short) 0) || Game1.tile[i, j].liquid > (byte) 64))
            {
              Vector2 vector2;
              vector2.X = (float) (i * 16);
              vector2.Y = (float) (j * 16);
              if ((double) this.position.X + (double) this.width > (double) vector2.X && (double) this.position.X < (double) vector2.X + 16.0 && (double) this.position.Y + (double) this.height > (double) vector2.Y && (double) this.position.Y < (double) vector2.Y + 16.0)
              {
                flag4 = true;
                if (Game1.rand.Next(40) == 0 && Game1.tile[i, j].active)
                  WorldGen.KillTile(i, j, true, true);

                if (Game1.netMode == 1 || Game1.tile[i, j].type != (byte)2 || Game1.tile[i, j - 1].type == (byte)27)
                { }
              }
            }
          }
        }
        float num47 = 8f;
        float num48 = 0.07f;
        if (this.type == 10)
        {
          num47 = 6f;
          num48 = 0.05f;
        }
        if (this.type == 13)
        {
          num47 = 11f;
          num48 = 0.08f;
        }
        Vector2 vector2_3 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
        float num49 = Game1.player[this.target].position.X + (float) (Game1.player[this.target].width / 2) - vector2_3.X;
        float num50 = Game1.player[this.target].position.Y + (float) (Game1.player[this.target].height / 2) - vector2_3.Y;
        float num51 = (float) Math.Sqrt((double) num49 * (double) num49 + (double) num50 * (double) num50);
        if ((double) this.ai[1] > 0.0)
        {
          float x = Game1.npc[(int) this.ai[1]].position.X + (float) (Game1.npc[(int) this.ai[1]].width / 2) - vector2_3.X;
          float y = Game1.npc[(int) this.ai[1]].position.Y + (float) (Game1.npc[(int) this.ai[1]].height / 2) - vector2_3.Y;
          this.rotation = (float) Math.Atan2((double) y, (double) x) + 1.57f;
          float num52 = (float) Math.Sqrt((double) x * (double) x + (double) y * (double) y);
          float num53 = (num52 - (float) this.width) / num52;
          float num54 = x * num53;
          float num55 = y * num53;
          this.velocity = new Vector2();
          this.position.X += num54;
          this.position.Y += num55;
        }
        else
        {
          if (!flag4)
          {
            this.TargetClosest();
            this.velocity.Y += 0.11f;
            if ((double) this.velocity.Y > (double) num47)
              this.velocity.Y = num47;
            if ((double) Math.Abs(this.velocity.X) + (double) Math.Abs(this.velocity.Y) < (double) num47 * 0.4)
            {
              if ((double) this.velocity.X < 0.0)
                this.velocity.X -= num48 * 1.1f;
              else
                this.velocity.X += num48 * 1.1f;
            }
            else if ((double) this.velocity.Y == (double) num47)
            {
              if ((double) this.velocity.X < (double) num49)
                this.velocity.X += num48;
              else if ((double) this.velocity.X > (double) num49)
                this.velocity.X -= num48;
            }
            else if ((double) this.velocity.Y > 4.0)
            {
              if ((double) this.velocity.X < 0.0)
                this.velocity.X += num48 * 0.9f;
              else
                this.velocity.X -= num48 * 0.9f;
            }
          }
          else
          {
            if (this.soundDelay == 0)
            {
              float num56 = num51 / 40f;
              if ((double) num56 < 10.0)
                num56 = 10f;
              if ((double) num56 > 20.0)
                num56 = 20f;
              this.soundDelay = (int) num56;
              Game1.PlaySound(15, (int) this.position.X, (int) this.position.Y);
            }
            float num57 = (float) Math.Sqrt((double) num49 * (double) num49 + (double) num50 * (double) num50);
            float num58 = Math.Abs(num49);
            float num59 = Math.Abs(num50);
            float num60 = num47 / num57;
            float num61 = num49 * num60;
            float num62 = num50 * num60;
            if ((double) this.velocity.X > 0.0 && (double) num61 > 0.0 || (double) this.velocity.X < 0.0 && (double) num61 < 0.0 || (double) this.velocity.Y > 0.0 && (double) num62 > 0.0 || (double) this.velocity.Y < 0.0 && (double) num62 < 0.0)
            {
              if ((double) this.velocity.X < (double) num61)
                this.velocity.X += num48;
              else if ((double) this.velocity.X > (double) num61)
                this.velocity.X -= num48;
              if ((double) this.velocity.Y < (double) num62)
                this.velocity.Y += num48;
              else if ((double) this.velocity.Y > (double) num62)
                this.velocity.Y -= num48;
            }
            else if ((double) num58 > (double) num59)
            {
              if ((double) this.velocity.X < (double) num61)
                this.velocity.X += num48 * 1.1f;
              else if ((double) this.velocity.X > (double) num61)
                this.velocity.X -= num48 * 1.1f;
              if ((double) Math.Abs(this.velocity.X) + (double) Math.Abs(this.velocity.Y) < (double) num47 * 0.5)
              {
                if ((double) this.velocity.Y > 0.0)
                  this.velocity.Y += num48;
                else
                  this.velocity.Y -= num48;
              }
            }
            else
            {
              if ((double) this.velocity.Y < (double) num62)
                this.velocity.Y += num48 * 1.1f;
              else if ((double) this.velocity.Y > (double) num62)
                this.velocity.Y -= num48 * 1.1f;
              if ((double) Math.Abs(this.velocity.X) + (double) Math.Abs(this.velocity.Y) < (double) num47 * 0.5)
              {
                if ((double) this.velocity.X > 0.0)
                  this.velocity.X += num48;
                else
                  this.velocity.X -= num48;
              }
            }
          }
          this.rotation = (float) Math.Atan2((double) this.velocity.Y, (double) this.velocity.X) + 1.57f;
        }
      }
      else if (this.aiStyle == 7)
      {
        int num63 = (int) ((double) this.position.X + (double) (this.width / 2)) / 16;
        int num64 = (int) ((double) this.position.Y + (double) this.height + 1.0) / 16;
        bool flag5 = false;
        this.directionY = -1;
        if (this.direction == 0)
          this.direction = 1;
        for (int index = 0; index < 8; ++index)
        {
          if (Game1.player[index].active && Game1.player[index].talkNPC == this.whoAmI)
          {
            flag5 = true;
            if ((double) this.ai[0] != 0.0)
              this.netUpdate = true;
            this.ai[0] = 0.0f;
            this.ai[1] = 300f;
            this.ai[2] = 100f;
            this.direction = (double) Game1.player[index].position.X + (double) (Game1.player[index].width / 2) >= (double) this.position.X + (double) (this.width / 2) ? 1 : -1;
          }
        }
        if ((double) this.ai[3] > 0.0)
        {
          this.life = -1;
          this.HitEffect();
          this.active = false;
          if (this.type == 37)
            Game1.PlaySound(15, (int) this.position.X, (int) this.position.Y, 0);
        }
        if (this.type == 37 && Game1.netMode != 1)
        {
          this.homeless = false;
          this.homeTileX = Game1.dungeonX;
          this.homeTileY = Game1.dungeonY;
          if (NPC.downedBoss3)
          {
            this.ai[3] = 1f;
            this.netUpdate = true;
          }
          if (!Game1.dayTime && flag5 && (double) this.ai[3] == 0.0)
          {
            bool flag6 = true;
            for (int index = 0; index < 1000; ++index)
            {
              if (Game1.npc[index].active && Game1.npc[index].type == 35)
              {
                flag6 = false;
                break;
              }
            }
            if (flag6)
            {
              int index = NPC.NewNPC((int) this.position.X + this.width / 2, (int) this.position.Y + this.height / 2, 35);
              Game1.npc[index].netUpdate = true;
              string str = "Skeletron";
              if (Game1.netMode == 0)
                Game1.NewText(str + " has awoken!", (byte) 175, (byte) 75);
              else if (Game1.netMode == 2)
                NetMessage.SendData(25, text: str + " has awoken!", number: 8, number2: 175f, number3: 75f, number4: (float) byte.MaxValue);
            }
            this.ai[3] = 1f;
            this.netUpdate = true;
          }
        }
        if (Game1.netMode != 1 && !Game1.dayTime && (num63 != this.homeTileX || num64 != this.homeTileY) && !this.homeless)
        {
          bool flag7 = true;
          for (int index3 = 0; index3 < 2; ++index3)
          {
            Rectangle rectangle = new Rectangle((int) ((double) this.position.X + (double) (this.width / 2) - (double) (Game1.screenWidth / 2) - (double) NPC.safeRangeX), (int) ((double) this.position.Y + (double) (this.height / 2) - (double) (Game1.screenHeight / 2) - (double) NPC.safeRangeY), Game1.screenWidth + NPC.safeRangeX * 2, Game1.screenHeight + NPC.safeRangeY * 2);
            if (index3 == 1)
              rectangle = new Rectangle(this.homeTileX * 16 + 8 - Game1.screenWidth / 2 - NPC.safeRangeX, this.homeTileY * 16 + 8 - Game1.screenHeight / 2 - NPC.safeRangeY, Game1.screenWidth + NPC.safeRangeX * 2, Game1.screenHeight + NPC.safeRangeY * 2);
            for (int index4 = 0; index4 < 8; ++index4)
            {
              if (Game1.player[index4].active && new Rectangle((int) Game1.player[index4].position.X, (int) Game1.player[index4].position.Y, Game1.player[index4].width, Game1.player[index4].height).Intersects(rectangle))
              {
                flag7 = false;
                break;
              }
              if (!flag7)
                break;
            }
          }
          if (flag7)
          {
            if (this.type == 37 || !Collision.SolidTiles(this.homeTileX - 1, this.homeTileX + 1, this.homeTileY - 3, this.homeTileY - 1))
            {
              this.velocity.X = 0.0f;
              this.velocity.Y = 0.0f;
              this.position.X = (float) (this.homeTileX * 16 + 8 - this.width / 2);
              this.position.Y = (float) (this.homeTileY * 16 - this.height) - 0.1f;
              this.netUpdate = true;
            }
            else
            {
              this.homeless = true;
              WorldGen.QuickFindHome(this.whoAmI);
            }
          }
        }
        if ((double) this.ai[0] == 0.0)
        {
          if ((double) this.ai[2] > 0.0)
            --this.ai[2];
          if (!Game1.dayTime && !flag5)
          {
            if (Game1.netMode != 1)
            {
              if (num63 == this.homeTileX && num64 == this.homeTileY)
              {
                if ((double) this.velocity.X != 0.0)
                  this.netUpdate = true;
                if ((double) this.velocity.X > 0.1)
                  this.velocity.X -= 0.1f;
                else if ((double) this.velocity.X < -0.1)
                  this.velocity.X += 0.1f;
                else
                  this.velocity.X = 0.0f;
              }
              else if (!flag5)
              {
                this.direction = num63 <= this.homeTileX ? 1 : -1;
                this.ai[0] = 1f;
                this.ai[1] = (float) (200 + Game1.rand.Next(200));
                this.ai[2] = 0.0f;
                this.netUpdate = true;
              }
            }
          }
          else
          {
            if ((double) this.velocity.X > 0.1)
              this.velocity.X -= 0.1f;
            else if ((double) this.velocity.X < -0.1)
              this.velocity.X += 0.1f;
            else
              this.velocity.X = 0.0f;
            if ((double) this.ai[1] > 0.0)
              --this.ai[1];
            if ((double) this.ai[1] <= 0.0)
            {
              this.ai[0] = 1f;
              this.ai[1] = (float) (200 + Game1.rand.Next(200));
              this.ai[2] = 0.0f;
              this.netUpdate = true;
            }
          }
          if (Game1.netMode == 1 || !Game1.dayTime && (num63 != this.homeTileX || num64 != this.homeTileY))
            return;
          if (num63 < this.homeTileX - 25 || num63 > this.homeTileX + 25)
          {
            if ((double) this.ai[2] == 0.0)
            {
              if (num63 < this.homeTileX - 50 && this.direction == -1)
              {
                this.direction = 1;
                this.netUpdate = true;
              }
              else if (num63 > this.homeTileX + 50 && this.direction == 1)
              {
                this.direction = -1;
                this.netUpdate = true;
              }
            }
          }
          else if (Game1.rand.Next(80) == 0 && (double) this.ai[2] == 0.0)
          {
            this.ai[2] = 200f;
            this.direction *= -1;
            this.netUpdate = true;
          }
        }
        else
        {
          if ((double) this.ai[0] != 1.0)
            return;
          if (Game1.netMode != 1 && !Game1.dayTime && num63 == this.homeTileX && num64 == this.homeTileY)
          {
            this.ai[0] = 0.0f;
            this.ai[1] = (float) (200 + Game1.rand.Next(200));
            this.ai[2] = 60f;
            this.netUpdate = true;
          }
          else
          {
            if (Game1.netMode != 1 && !this.homeless && (num63 < this.homeTileX - 35 || num63 > this.homeTileX + 35))
            {
              if ((double) this.position.X < (double) (this.homeTileX * 16) && this.direction == -1)
              {
                this.direction = 1;
                this.velocity.X = 0.1f;
                this.netUpdate = true;
              }
              else if ((double) this.position.X > (double) (this.homeTileX * 16) && this.direction == 1)
              {
                this.direction = -1;
                this.velocity.X = -0.1f;
                this.netUpdate = true;
              }
            }
            --this.ai[1];
            if ((double) this.ai[1] <= 0.0)
            {
              this.ai[0] = 0.0f;
              this.ai[1] = (float) (200 + Game1.rand.Next(200));
              this.ai[2] = 60f;
              this.netUpdate = true;
            }
            if (this.closeDoor && (((double) this.position.X + (double) (this.width / 2)) / 16.0 > (double) (this.doorX + 2) || ((double) this.position.X + (double) (this.width / 2)) / 16.0 < (double) (this.doorX - 2)))
            {
              if (WorldGen.CloseDoor(this.doorX, this.doorY))
              {
                this.closeDoor = false;
                NetMessage.SendData(19, number: 1, number2: (float) this.doorX, number3: (float) this.doorY, number4: (float) this.direction);
              }
              if (((double) this.position.X + (double) (this.width / 2)) / 16.0 > (double) (this.doorX + 4) || ((double) this.position.X + (double) (this.width / 2)) / 16.0 < (double) (this.doorX - 4) || ((double) this.position.Y + (double) (this.height / 2)) / 16.0 > (double) (this.doorY + 4) || ((double) this.position.Y + (double) (this.height / 2)) / 16.0 < (double) (this.doorY - 4))
                this.closeDoor = false;
            }
            if ((double) this.velocity.X < -1.0 || (double) this.velocity.X > 1.0)
            {
              if ((double) this.velocity.Y == 0.0)
                this.velocity *= 0.8f;
            }
            else if ((double) this.velocity.X < 1.15 && this.direction == 1)
            {
              this.velocity.X += 0.07f;
              if ((double) this.velocity.X > 1.0)
                this.velocity.X = 1f;
            }
            else if ((double) this.velocity.X > -1.0 && this.direction == -1)
            {
              this.velocity.X -= 0.07f;
              if ((double) this.velocity.X > 1.0)
                this.velocity.X = 1f;
            }
            if ((double) this.velocity.Y == 0.0)
            {
              if ((double) this.position.X == (double) this.ai[2])
                this.direction *= -1;
              this.ai[2] = -1f;
              int index5 = (int) (((double) this.position.X + (double) (this.width / 2) + (double) (15 * this.direction)) / 16.0);
              int index6 = (int) (((double) this.position.Y + (double) this.height - 16.0) / 16.0);
              if (Game1.tile[index5, index6] == null)
                Game1.tile[index5, index6] = new Tile();
              if (Game1.tile[index5, index6 - 1] == null)
                Game1.tile[index5, index6 - 1] = new Tile();
              if (Game1.tile[index5, index6 - 2] == null)
                Game1.tile[index5, index6 - 2] = new Tile();
              if (Game1.tile[index5, index6 - 3] == null)
                Game1.tile[index5, index6 - 3] = new Tile();
              if (Game1.tile[index5, index6 + 1] == null)
                Game1.tile[index5, index6 + 1] = new Tile();
              if (Game1.tile[index5 + this.direction, index6 - 1] == null)
                Game1.tile[index5 + this.direction, index6 - 1] = new Tile();
              if (Game1.tile[index5 + this.direction, index6 + 1] == null)
                Game1.tile[index5 + this.direction, index6 + 1] = new Tile();
              if (Game1.tile[index5, index6 - 2].active && Game1.tile[index5, index6 - 2].type == (byte) 10)
              {
                if (Game1.netMode != 1)
                {
                  if (WorldGen.OpenDoor(index5, index6 - 2, this.direction))
                  {
                    this.closeDoor = true;
                    this.doorX = index5;
                    this.doorY = index6 - 2;
                    NetMessage.SendData(19, number2: (float) index5, number3: (float) (index6 - 2), number4: (float) this.direction);
                    this.netUpdate = true;
                    this.ai[1] += 80f;
                  }
                  else if (WorldGen.OpenDoor(index5, index6 - 2, -this.direction))
                  {
                    this.closeDoor = true;
                    this.doorX = index5;
                    this.doorY = index6 - 2;
                    NetMessage.SendData(19, number2: (float) index5, number3: (float) (index6 - 2), number4: (float) -this.direction);
                    this.netUpdate = true;
                    this.ai[1] += 80f;
                  }
                  else
                  {
                    this.direction *= -1;
                    this.netUpdate = true;
                  }
                }
              }
              else if ((double) this.velocity.X < 0.0 && this.spriteDirection == -1 || (double) this.velocity.X > 0.0 && this.spriteDirection == 1)
              {
                if (Game1.tile[index5, index6 - 2].active && Game1.tileSolid[(int) Game1.tile[index5, index6 - 2].type] && !Game1.tileSolidTop[(int) Game1.tile[index5, index6 - 2].type])
                {
                  if (this.direction == 1 && !Collision.SolidTiles(index5 - 2, index5 - 1, index6 - 5, index6 - 1) || this.direction == -1 && !Collision.SolidTiles(index5 + 1, index5 + 2, index6 - 5, index6 - 1))
                  {
                    if (!Collision.SolidTiles(index5, index5, index6 - 5, index6 - 3))
                    {
                      this.velocity.Y = -6f;
                      this.netUpdate = true;
                    }
                    else
                    {
                      this.direction *= -1;
                      this.netUpdate = true;
                    }
                  }
                  else
                  {
                    this.direction *= -1;
                    this.netUpdate = true;
                  }
                }
                else if (Game1.tile[index5, index6 - 1].active && Game1.tileSolid[(int) Game1.tile[index5, index6 - 1].type] && !Game1.tileSolidTop[(int) Game1.tile[index5, index6 - 1].type])
                {
                  if (this.direction == 1 && !Collision.SolidTiles(index5 - 2, index5 - 1, index6 - 4, index6 - 1) || this.direction == -1 && !Collision.SolidTiles(index5 + 1, index5 + 2, index6 - 4, index6 - 1))
                  {
                    if (!Collision.SolidTiles(index5, index5, index6 - 4, index6 - 2))
                    {
                      this.velocity.Y = -5f;
                      this.netUpdate = true;
                    }
                    else
                    {
                      this.direction *= -1;
                      this.netUpdate = true;
                    }
                  }
                  else
                  {
                    this.direction *= -1;
                    this.netUpdate = true;
                  }
                }
                else if (Game1.tile[index5, index6].active && Game1.tileSolid[(int) Game1.tile[index5, index6].type] && !Game1.tileSolidTop[(int) Game1.tile[index5, index6].type])
                {
                  if (this.direction == 1 && !Collision.SolidTiles(index5 - 2, index5, index6 - 3, index6 - 1) || this.direction == -1 && !Collision.SolidTiles(index5, index5 + 2, index6 - 3, index6 - 1))
                  {
                    this.velocity.Y = -3.6f;
                    this.netUpdate = true;
                  }
                  else
                  {
                    this.direction *= -1;
                    this.netUpdate = true;
                  }
                }
                else if (Game1.netMode != 1 && num63 >= this.homeTileX - 35 && num63 <= this.homeTileX + 35 && (!Game1.tile[index5, index6 + 1].active || !Game1.tileSolid[(int) Game1.tile[index5, index6 + 1].type]) && (!Game1.tile[index5 - this.direction, index6 + 1].active || !Game1.tileSolid[(int) Game1.tile[index5 - this.direction, index6 + 1].type]) && (!Game1.tile[index5, index6 + 2].active || !Game1.tileSolid[(int) Game1.tile[index5, index6 + 2].type]) && (!Game1.tile[index5 - this.direction, index6 + 2].active || !Game1.tileSolid[(int) Game1.tile[index5 - this.direction, index6 + 2].type]) && (!Game1.tile[index5, index6 + 3].active || !Game1.tileSolid[(int) Game1.tile[index5, index6 + 3].type]) && (!Game1.tile[index5 - this.direction, index6 + 3].active || !Game1.tileSolid[(int) Game1.tile[index5 - this.direction, index6 + 3].type]) && (!Game1.tile[index5, index6 + 4].active || !Game1.tileSolid[(int) Game1.tile[index5, index6 + 4].type]) && (!Game1.tile[index5 - this.direction, index6 + 4].active || !Game1.tileSolid[(int) Game1.tile[index5 - this.direction, index6 + 4].type]))
                {
                  this.direction *= -1;
                  this.velocity.X *= -1f;
                  this.netUpdate = true;
                }
                if ((double) this.velocity.Y < 0.0)
                  this.ai[2] = this.position.X;
              }
            }
          }
        }
      }
      else if (this.aiStyle == 8)
      {
        this.TargetClosest();
        this.velocity.X *= 0.93f;
        if ((double) this.velocity.X > -0.1 && (double) this.velocity.X < 0.1)
          this.velocity.X = 0.0f;
        if ((double) this.ai[0] == 0.0)
          this.ai[0] = 500f;
        if ((double) this.ai[2] != 0.0 && (double) this.ai[3] != 0.0)
        {
          Game1.PlaySound(2, (int) this.position.X, (int) this.position.Y, 8);
          for (int index7 = 0; index7 < 50; ++index7)
          {
            if (this.type == 29)
            {
              int index8 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 27, Alpha: 100, Scale: (float) Game1.rand.Next(1, 3));
              Game1.dust[index8].velocity *= 3f;
              if ((double) Game1.dust[index8].scale > 1.0)
                Game1.dust[index8].noGravity = true;
            }
            else if (this.type == 32)
            {
              int index9 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 29, Alpha: 100, Scale: 2.5f);
              Game1.dust[index9].velocity *= 3f;
              Game1.dust[index9].noGravity = true;
            }
            else
            {
              int index10 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, Alpha: 100, Scale: 2.5f);
              Game1.dust[index10].velocity *= 3f;
              Game1.dust[index10].noGravity = true;
            }
          }
          this.position.X = (float) ((double) this.ai[2] * 16.0 - (double) (this.width / 2) + 8.0);
          this.position.Y = this.ai[3] * 16f - (float) this.height;
          this.velocity.X = 0.0f;
          this.velocity.Y = 0.0f;
          this.ai[2] = 0.0f;
          this.ai[3] = 0.0f;
          Game1.PlaySound(2, (int) this.position.X, (int) this.position.Y, 8);
          for (int index11 = 0; index11 < 50; ++index11)
          {
            if (this.type == 29)
            {
              int index12 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 27, Alpha: 100, Scale: (float) Game1.rand.Next(1, 3));
              Game1.dust[index12].velocity *= 3f;
              if ((double) Game1.dust[index12].scale > 1.0)
                Game1.dust[index12].noGravity = true;
            }
            else if (this.type == 32)
            {
              int index13 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 29, Alpha: 100, Scale: 2.5f);
              Game1.dust[index13].velocity *= 3f;
              Game1.dust[index13].noGravity = true;
            }
            else
            {
              int index14 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, Alpha: 100, Scale: 2.5f);
              Game1.dust[index14].velocity *= 3f;
              Game1.dust[index14].noGravity = true;
            }
          }
        }
        ++this.ai[0];
        if ((double) this.ai[0] == 75.0 || (double) this.ai[0] == 150.0 || (double) this.ai[0] == 225.0)
        {
          this.ai[1] = 30f;
          this.netUpdate = true;
        }
        else if ((double) this.ai[0] >= 450.0 && Game1.netMode != 1)
        {
          this.ai[0] = 1f;
          int num65 = (int) Game1.player[this.target].position.X / 16;
          int num66 = (int) Game1.player[this.target].position.Y / 16;
          int num67 = (int) this.position.X / 16;
          int num68 = (int) this.position.Y / 16;
          int num69 = 20;
          int num70 = 0;
          bool flag8 = false;
          if ((double) Math.Abs(this.position.X - Game1.player[this.target].position.X) + (double) Math.Abs(this.position.Y - Game1.player[this.target].position.Y) > 2000.0)
          {
            num70 = 100;
            flag8 = true;
          }
          while (!flag8 && num70 < 100)
          {
            ++num70;
            int index15 = Game1.rand.Next(num65 - num69, num65 + num69);
            for (int index16 = Game1.rand.Next(num66 - num69, num66 + num69); index16 < num66 + num69; ++index16)
            {
              if ((index16 < num66 - 4 || index16 > num66 + 4 || index15 < num65 - 4 || index15 > num65 + 4) && (index16 < num68 - 1 || index16 > num68 + 1 || index15 < num67 - 1 || index15 > num67 + 1) && Game1.tile[index15, index16].active)
              {
                bool flag9 = true;
                if (this.type == 32 && Game1.tile[index15, index16 - 1].wall == (byte) 0)
                  flag9 = false;
                else if (Game1.tile[index15, index16 - 1].lava)
                  flag9 = false;
                if (flag9 && Game1.tileSolid[(int) Game1.tile[index15, index16].type] && !Collision.SolidTiles(index15 - 1, index15 + 1, index16 - 4, index16 - 1))
                {
                  this.ai[1] = 20f;
                  this.ai[2] = (float) index15;
                  this.ai[3] = (float) index16;
                  flag8 = true;
                  break;
                }
              }
            }
          }
          this.netUpdate = true;
        }
        if ((double) this.ai[1] > 0.0)
        {
          --this.ai[1];
          if ((double) this.ai[1] == 25.0)
          {
            Game1.PlaySound(2, (int) this.position.X, (int) this.position.Y, 8);
            if (Game1.netMode != 1)
            {
              if (this.type == 29)
                NPC.NewNPC((int) this.position.X + this.width / 2, (int) this.position.Y - 8, 30);
              else if (this.type == 32)
                NPC.NewNPC((int) this.position.X + this.width / 2, (int) this.position.Y - 8, 33);
              else
                NPC.NewNPC((int) this.position.X + this.width / 2, (int) this.position.Y - 8, 25);
            }
          }
        }
        if (this.type == 29)
        {
          if (Game1.rand.Next(5) != 0)
            return;
          int index = Dust.NewDust(new Vector2(this.position.X, this.position.Y + 2f), this.width, this.height, 27, this.velocity.X * 0.2f, this.velocity.Y * 0.2f, 100, Scale: 1.5f);
          Game1.dust[index].noGravity = true;
          Game1.dust[index].velocity.X *= 0.5f;
          Game1.dust[index].velocity.Y = -2f;
        }
        else if (this.type == 32)
        {
          if (Game1.rand.Next(2) != 0)
            return;
          int index = Dust.NewDust(new Vector2(this.position.X, this.position.Y + 2f), this.width, this.height, 29, this.velocity.X * 0.2f, this.velocity.Y * 0.2f, 100, Scale: 2f);
          Game1.dust[index].noGravity = true;
          Game1.dust[index].velocity.X *= 1f;
          Game1.dust[index].velocity.Y *= 1f;
        }
        else if (Game1.rand.Next(2) == 0)
        {
          int index = Dust.NewDust(new Vector2(this.position.X, this.position.Y + 2f), this.width, this.height, 6, this.velocity.X * 0.2f, this.velocity.Y * 0.2f, 100, Scale: 2f);
          Game1.dust[index].noGravity = true;
          Game1.dust[index].velocity.X *= 1f;
          Game1.dust[index].velocity.Y *= 1f;
        }
      }
      else if (this.aiStyle == 9)
      {
        if (this.target == 8)
        {
          this.TargetClosest();
          float num71 = 6f;
          if (this.type == 30)
            NPC.maxSpawns = 8;
          Vector2 vector2 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
          float num72 = Game1.player[this.target].position.X + (float) (Game1.player[this.target].width / 2) - vector2.X;
          float num73 = Game1.player[this.target].position.Y + (float) (Game1.player[this.target].height / 2) - vector2.Y;
          float num74 = (float) Math.Sqrt((double) num72 * (double) num72 + (double) num73 * (double) num73);
          float num75 = num71 / num74;
          this.velocity.X = num72 * num75;
          this.velocity.Y = num73 * num75;
        }
        if (this.timeLeft > 100)
          this.timeLeft = 100;
        for (int index17 = 0; index17 < 2; ++index17)
        {
          if (this.type == 30)
          {
            int index18 = Dust.NewDust(new Vector2(this.position.X, this.position.Y + 2f), this.width, this.height, 27, this.velocity.X * 0.2f, this.velocity.Y * 0.2f, 100, Scale: 2f);
            Game1.dust[index18].noGravity = true;
            Game1.dust[index18].velocity *= 0.3f;
            Game1.dust[index18].velocity.X -= this.velocity.X * 0.2f;
            Game1.dust[index18].velocity.Y -= this.velocity.Y * 0.2f;
          }
          else if (this.type == 33)
          {
            int index19 = Dust.NewDust(new Vector2(this.position.X, this.position.Y + 2f), this.width, this.height, 29, this.velocity.X * 0.2f, this.velocity.Y * 0.2f, 100, Scale: 2f);
            Game1.dust[index19].noGravity = true;
            Game1.dust[index19].velocity.X *= 0.3f;
            Game1.dust[index19].velocity.Y *= 0.3f;
          }
          else
          {
            int index20 = Dust.NewDust(new Vector2(this.position.X, this.position.Y + 2f), this.width, this.height, 6, this.velocity.X * 0.2f, this.velocity.Y * 0.2f, 100, Scale: 2f);
            Game1.dust[index20].noGravity = true;
            Game1.dust[index20].velocity.X *= 0.3f;
            Game1.dust[index20].velocity.Y *= 0.3f;
          }
        }
        this.rotation += 0.4f * (float) this.direction;
      }
      else if (this.aiStyle == 10)
      {
        if (this.collideX)
        {
          this.velocity.X = this.oldVelocity.X * -0.5f;
          if (this.direction == -1 && (double) this.velocity.X > 0.0 && (double) this.velocity.X < 2.0)
            this.velocity.X = 2f;
          if (this.direction == 1 && (double) this.velocity.X < 0.0 && (double) this.velocity.X > -2.0)
            this.velocity.X = -2f;
        }
        if (this.collideY)
        {
          this.velocity.Y = this.oldVelocity.Y * -0.5f;
          if ((double) this.velocity.Y > 0.0 && (double) this.velocity.Y < 1.0)
            this.velocity.Y = 1f;
          if ((double) this.velocity.Y < 0.0 && (double) this.velocity.Y > -1.0)
            this.velocity.Y = -1f;
        }
        this.TargetClosest();
        if (this.direction == -1 && (double) this.velocity.X > -4.0)
        {
          this.velocity.X -= 0.1f;
          if ((double) this.velocity.X > 4.0)
            this.velocity.X -= 0.1f;
          else if ((double) this.velocity.X > 0.0)
            this.velocity.X += 0.05f;
          if ((double) this.velocity.X < -4.0)
            this.velocity.X = -4f;
        }
        else if (this.direction == 1 && (double) this.velocity.X < 4.0)
        {
          this.velocity.X += 0.1f;
          if ((double) this.velocity.X < -4.0)
            this.velocity.X += 0.1f;
          else if ((double) this.velocity.X < 0.0)
            this.velocity.X -= 0.05f;
          if ((double) this.velocity.X > 4.0)
            this.velocity.X = 4f;
        }
        if (this.directionY == -1 && (double) this.velocity.Y > -1.5)
        {
          this.velocity.Y -= 0.04f;
          if ((double) this.velocity.Y > 1.5)
            this.velocity.Y -= 0.05f;
          else if ((double) this.velocity.Y > 0.0)
            this.velocity.Y += 0.03f;
          if ((double) this.velocity.Y < -1.5)
            this.velocity.Y = -1.5f;
        }
        else if (this.directionY == 1 && (double) this.velocity.Y < 1.5)
        {
          this.velocity.Y += 0.04f;
          if ((double) this.velocity.Y < -1.5)
            this.velocity.Y += 0.05f;
          else if ((double) this.velocity.Y < 0.0)
            this.velocity.Y -= 0.03f;
          if ((double) this.velocity.Y > 1.5)
            this.velocity.Y = 1.5f;
        }
        this.rotation = (float) Math.Atan2((double) this.velocity.Y, (double) this.velocity.X) - 1.57f;
        int index = Dust.NewDust(new Vector2(this.position.X - this.velocity.X, this.position.Y - this.velocity.Y), this.width, this.height, 6, this.velocity.X * 0.2f, this.velocity.Y * 0.2f, 100, Scale: 2f);
        Game1.dust[index].noGravity = true;
        Game1.dust[index].noLight = true;
        Game1.dust[index].velocity.X *= 0.3f;
        Game1.dust[index].velocity.Y *= 0.3f;
      }
      else if (this.aiStyle == 11)
      {
        if ((double) this.ai[0] == 0.0 && Game1.netMode != 1)
        {
          this.TargetClosest();
          this.ai[0] = 1f;
          int index21 = NPC.NewNPC((int) ((double) this.position.X + (double) (this.width / 2)), (int) this.position.Y + this.height / 2, 36, this.whoAmI);
          Game1.npc[index21].ai[0] = -1f;
          Game1.npc[index21].ai[1] = (float) this.whoAmI;
          Game1.npc[index21].target = this.target;
          Game1.npc[index21].netUpdate = true;
          int index22 = NPC.NewNPC((int) ((double) this.position.X + (double) (this.width / 2)), (int) this.position.Y + this.height / 2, 36, this.whoAmI);
          Game1.npc[index22].ai[0] = 1f;
          Game1.npc[index22].ai[1] = (float) this.whoAmI;
          Game1.npc[index22].ai[3] = 150f;
          Game1.npc[index22].target = this.target;
          Game1.npc[index22].netUpdate = true;
        }
        if (Game1.player[this.target].dead || (double) Math.Abs(this.position.X - Game1.player[this.target].position.X) > 2000.0 || (double) Math.Abs(this.position.Y - Game1.player[this.target].position.Y) > 2000.0)
        {
          this.TargetClosest();
          if (Game1.player[this.target].dead || (double) Math.Abs(this.position.X - Game1.player[this.target].position.X) > 2000.0 || (double) Math.Abs(this.position.Y - Game1.player[this.target].position.Y) > 2000.0)
            this.ai[1] = 3f;
        }
        if (Game1.dayTime && (double) this.ai[1] != 3.0 && (double) this.ai[1] != 2.0)
        {
          this.ai[1] = 2f;
          Game1.PlaySound(15, (int) this.position.X, (int) this.position.Y, 0);
        }
        if ((double) this.ai[1] == 0.0)
        {
          ++this.ai[2];
          if ((double) this.ai[2] >= 800.0)
          {
            this.ai[2] = 0.0f;
            this.ai[1] = 1f;
            this.TargetClosest();
            this.netUpdate = true;
          }
          this.rotation = this.velocity.X / 15f;
          if ((double) this.position.Y > (double) Game1.player[this.target].position.Y - 250.0)
          {
            if ((double) this.velocity.Y > 0.0)
              this.velocity.Y *= 0.98f;
            this.velocity.Y -= 0.02f;
            if ((double) this.velocity.Y > 2.0)
              this.velocity.Y = 2f;
          }
          else if ((double) this.position.Y < (double) Game1.player[this.target].position.Y - 250.0)
          {
            if ((double) this.velocity.Y < 0.0)
              this.velocity.Y *= 0.98f;
            this.velocity.Y += 0.02f;
            if ((double) this.velocity.Y < -2.0)
              this.velocity.Y = -2f;
          }
          if ((double) this.position.X + (double) (this.width / 2) > (double) Game1.player[this.target].position.X + (double) (Game1.player[this.target].width / 2))
          {
            if ((double) this.velocity.X > 0.0)
              this.velocity.X *= 0.98f;
            this.velocity.X -= 0.05f;
            if ((double) this.velocity.X > 8.0)
              this.velocity.X = 8f;
          }
          if ((double) this.position.X + (double) (this.width / 2) < (double) Game1.player[this.target].position.X + (double) (Game1.player[this.target].width / 2))
          {
            if ((double) this.velocity.X < 0.0)
              this.velocity.X *= 0.98f;
            this.velocity.X += 0.05f;
            if ((double) this.velocity.X < -8.0)
              this.velocity.X = -8f;
          }
        }
        else if ((double) this.ai[1] == 1.0)
        {
          ++this.ai[2];
          if ((double) this.ai[2] == 2.0)
            Game1.PlaySound(15, (int) this.position.X, (int) this.position.Y, 0);
          if ((double) this.ai[2] >= 400.0)
          {
            this.ai[2] = 0.0f;
            this.ai[1] = 0.0f;
          }
          this.rotation += (float) this.direction * 0.3f;
          Vector2 vector2 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
          float num76 = Game1.player[this.target].position.X + (float) (Game1.player[this.target].width / 2) - vector2.X;
          float num77 = Game1.player[this.target].position.Y + (float) (Game1.player[this.target].height / 2) - vector2.Y;
          float num78 = 2.5f / (float) Math.Sqrt((double) num76 * (double) num76 + (double) num77 * (double) num77);
          this.velocity.X = num76 * num78;
          this.velocity.Y = num77 * num78;
        }
        else if ((double) this.ai[1] == 2.0)
        {
          this.damage = 9999;
          this.defense = 9999;
          this.rotation += (float) this.direction * 0.3f;
          Vector2 vector2 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
          float num79 = Game1.player[this.target].position.X + (float) (Game1.player[this.target].width / 2) - vector2.X;
          float num80 = Game1.player[this.target].position.Y + (float) (Game1.player[this.target].height / 2) - vector2.Y;
          float num81 = 8f / (float) Math.Sqrt((double) num79 * (double) num79 + (double) num80 * (double) num80);
          this.velocity.X = num79 * num81;
          this.velocity.Y = num80 * num81;
        }
        else if ((double) this.ai[1] == 3.0)
        {
          this.velocity.Y -= 0.1f;
          if ((double) this.velocity.Y > 0.0)
            this.velocity.Y *= 0.95f;
          this.velocity.X *= 0.95f;
          if (this.timeLeft > 50)
            this.timeLeft = 50;
        }
        int index23 = Dust.NewDust(new Vector2((float) ((double) this.position.X + (double) (this.width / 2) - 15.0 - (double) this.velocity.X * 5.0), (float) ((double) this.position.Y + (double) this.height - 2.0)), 30, 10, 5, (float) (-(double) this.velocity.X * 0.20000000298023224), 3f, Scale: 2f);
        Game1.dust[index23].noGravity = true;
        Game1.dust[index23].velocity.X *= 1.3f;
        Game1.dust[index23].velocity.X += this.velocity.X * 0.4f;
        Game1.dust[index23].velocity.Y += 2f + this.velocity.Y;
        for (int index24 = 0; index24 < 2; ++index24)
        {
          int index25 = Dust.NewDust(new Vector2(this.position.X, this.position.Y + 120f), this.width, 60, 5, this.velocity.X, this.velocity.Y, Scale: 2f);
          Game1.dust[index25].noGravity = true;
          Game1.dust[index25].velocity -= this.velocity;
          Game1.dust[index25].velocity.Y += 5f;
        }
      }
      else if (this.aiStyle == 12)
      {
        this.spriteDirection = -(int) this.ai[0];
        if (!Game1.npc[(int) this.ai[1]].active || Game1.npc[(int) this.ai[1]].aiStyle != 11)
        {
          this.ai[2] += 10f;
          if ((double) this.ai[2] > 50.0 || Game1.netMode != 2)
          {
            this.life = -1;
            this.HitEffect();
            this.active = false;
          }
        }
        float num82;
        if ((double) this.ai[2] == 0.0 || (double) this.ai[2] == 3.0)
        {
          if ((double) Game1.npc[(int) this.ai[1]].ai[1] == 3.0 && this.timeLeft > 10)
            this.timeLeft = 10;
          if ((double) Game1.npc[(int) this.ai[1]].ai[1] != 0.0)
          {
            if ((double) this.position.Y > (double) Game1.npc[(int) this.ai[1]].position.Y - 100.0)
            {
              if ((double) this.velocity.Y > 0.0)
                this.velocity.Y *= 0.96f;
              this.velocity.Y -= 0.07f;
              if ((double) this.velocity.Y > 6.0)
                this.velocity.Y = 6f;
            }
            else if ((double) this.position.Y < (double) Game1.npc[(int) this.ai[1]].position.Y - 100.0)
            {
              if ((double) this.velocity.Y < 0.0)
                this.velocity.Y *= 0.96f;
              this.velocity.Y += 0.07f;
              if ((double) this.velocity.Y < -6.0)
                this.velocity.Y = -6f;
            }
            if ((double) this.position.X + (double) (this.width / 2) > (double) Game1.npc[(int) this.ai[1]].position.X + (double) (Game1.npc[(int) this.ai[1]].width / 2) - 120.0 * (double) this.ai[0])
            {
              if ((double) this.velocity.X > 0.0)
                this.velocity.X *= 0.96f;
              this.velocity.X -= 0.1f;
              if ((double) this.velocity.X > 8.0)
                this.velocity.X = 8f;
            }
            if ((double) this.position.X + (double) (this.width / 2) < (double) Game1.npc[(int) this.ai[1]].position.X + (double) (Game1.npc[(int) this.ai[1]].width / 2) - 120.0 * (double) this.ai[0])
            {
              if ((double) this.velocity.X < 0.0)
                this.velocity.X *= 0.96f;
              this.velocity.X += 0.1f;
              if ((double) this.velocity.X < -8.0)
                this.velocity.X = -8f;
            }
          }
          else
          {
            ++this.ai[3];
            if ((double) this.ai[3] >= 300.0)
            {
              ++this.ai[2];
              this.ai[3] = 0.0f;
              this.netUpdate = true;
            }
            if ((double) this.position.Y > (double) Game1.npc[(int) this.ai[1]].position.Y + 230.0)
            {
              if ((double) this.velocity.Y > 0.0)
                this.velocity.Y *= 0.96f;
              this.velocity.Y -= 0.04f;
              if ((double) this.velocity.Y > 3.0)
                this.velocity.Y = 3f;
            }
            else if ((double) this.position.Y < (double) Game1.npc[(int) this.ai[1]].position.Y + 230.0)
            {
              if ((double) this.velocity.Y < 0.0)
                this.velocity.Y *= 0.96f;
              this.velocity.Y += 0.04f;
              if ((double) this.velocity.Y < -3.0)
                this.velocity.Y = -3f;
            }
            if ((double) this.position.X + (double) (this.width / 2) > (double) Game1.npc[(int) this.ai[1]].position.X + (double) (Game1.npc[(int) this.ai[1]].width / 2) - 200.0 * (double) this.ai[0])
            {
              if ((double) this.velocity.X > 0.0)
                this.velocity.X *= 0.96f;
              this.velocity.X -= 0.07f;
              if ((double) this.velocity.X > 8.0)
                this.velocity.X = 8f;
            }
            if ((double) this.position.X + (double) (this.width / 2) < (double) Game1.npc[(int) this.ai[1]].position.X + (double) (Game1.npc[(int) this.ai[1]].width / 2) - 200.0 * (double) this.ai[0])
            {
              if ((double) this.velocity.X < 0.0)
                this.velocity.X *= 0.96f;
              this.velocity.X += 0.07f;
              if ((double) this.velocity.X < -8.0)
                this.velocity.X = -8f;
            }
          }
          Vector2 vector2 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
          float x = (float) ((double) Game1.npc[(int) this.ai[1]].position.X + (double) (Game1.npc[(int) this.ai[1]].width / 2) - 200.0 * (double) this.ai[0]) - vector2.X;
          float y = Game1.npc[(int) this.ai[1]].position.Y + 230f - vector2.Y;
          num82 = (float) Math.Sqrt((double) x * (double) x + (double) y * (double) y);
          this.rotation = (float) Math.Atan2((double) y, (double) x) + 1.57f;
        }
        else if ((double) this.ai[2] == 1.0)
        {
          Vector2 vector2 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
          float x = (float) ((double) Game1.npc[(int) this.ai[1]].position.X + (double) (Game1.npc[(int) this.ai[1]].width / 2) - 200.0 * (double) this.ai[0]) - vector2.X;
          float y = Game1.npc[(int) this.ai[1]].position.Y + 230f - vector2.Y;
          num82 = (float) Math.Sqrt((double) x * (double) x + (double) y * (double) y);
          this.rotation = (float) Math.Atan2((double) y, (double) x) + 1.57f;
          this.velocity.X *= 0.95f;
          this.velocity.Y -= 0.1f;
          if ((double) this.velocity.Y < -8.0)
            this.velocity.Y = -8f;
          if ((double) this.position.Y >= (double) Game1.npc[(int) this.ai[1]].position.Y - 200.0)
            return;
          this.TargetClosest();
          this.ai[2] = 2f;
          vector2 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
          float num83 = Game1.player[this.target].position.X + (float) (Game1.player[this.target].width / 2) - vector2.X;
          float num84 = Game1.player[this.target].position.Y + (float) (Game1.player[this.target].height / 2) - vector2.Y;
          float num85 = 20f / (float) Math.Sqrt((double) num83 * (double) num83 + (double) num84 * (double) num84);
          this.velocity.X = num83 * num85;
          this.velocity.Y = num84 * num85;
          this.netUpdate = true;
        }
        else if ((double) this.ai[2] == 2.0)
        {
          if ((double) this.position.Y <= (double) Game1.player[this.target].position.Y && (double) this.velocity.Y >= 0.0)
            return;
          this.ai[2] = 3f;
        }
        else if ((double) this.ai[2] == 4.0)
        {
          Vector2 vector2 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
          float x = (float) ((double) Game1.npc[(int) this.ai[1]].position.X + (double) (Game1.npc[(int) this.ai[1]].width / 2) - 200.0 * (double) this.ai[0]) - vector2.X;
          float y = Game1.npc[(int) this.ai[1]].position.Y + 230f - vector2.Y;
          num82 = (float) Math.Sqrt((double) x * (double) x + (double) y * (double) y);
          this.rotation = (float) Math.Atan2((double) y, (double) x) + 1.57f;
          this.velocity.Y *= 0.95f;
          this.velocity.X += (float) (0.10000000149011612 * -(double) this.ai[0]);
          if ((double) this.velocity.X < -8.0)
            this.velocity.X = -8f;
          if ((double) this.velocity.X > 8.0)
            this.velocity.X = 8f;
          if ((double) this.position.X + (double) (this.width / 2) >= (double) Game1.npc[(int) this.ai[1]].position.X + (double) (Game1.npc[(int) this.ai[1]].width / 2) - 500.0 && (double) this.position.X + (double) (this.width / 2) <= (double) Game1.npc[(int) this.ai[1]].position.X + (double) (Game1.npc[(int) this.ai[1]].width / 2) + 500.0)
            return;
          this.TargetClosest();
          this.ai[2] = 5f;
          vector2 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
          float num86 = Game1.player[this.target].position.X + (float) (Game1.player[this.target].width / 2) - vector2.X;
          float num87 = Game1.player[this.target].position.Y + (float) (Game1.player[this.target].height / 2) - vector2.Y;
          float num88 = 20f / (float) Math.Sqrt((double) num86 * (double) num86 + (double) num87 * (double) num87);
          this.velocity.X = num86 * num88;
          this.velocity.Y = num87 * num88;
          this.netUpdate = true;
        }
        else
        {
          if ((double) this.ai[2] != 5.0 || ((double) this.velocity.X <= 0.0 || (double) this.position.X + (double) (this.width / 2) <= (double) Game1.player[this.target].position.X + (double) (Game1.player[this.target].width / 2)) && ((double) this.velocity.X >= 0.0 || (double) this.position.X + (double) (this.width / 2) >= (double) Game1.player[this.target].position.X + (double) (Game1.player[this.target].width / 2)))
            return;
          this.ai[2] = 0.0f;
        }
      }
      else
      {
        if (this.aiStyle != 13)
          return;
        if (!Game1.tile[(int) this.ai[0], (int) this.ai[1]].active)
        {
          this.life = -1;
          this.HitEffect();
          this.active = false;
        }
        else
        {
          this.TargetClosest();
          float num89 = 0.05f;
          Vector2 vector2 = new Vector2((float) ((double) this.ai[0] * 16.0 + 8.0), (float) ((double) this.ai[1] * 16.0 + 8.0));
          float x = Game1.player[this.target].position.X + (float) (Game1.player[this.target].width / 2) - (float) (this.width / 2) - vector2.X;
          float y = Game1.player[this.target].position.Y + (float) (Game1.player[this.target].height / 2) - (float) (this.height / 2) - vector2.Y;
          float num90 = (float) Math.Sqrt((double) x * (double) x + (double) y * (double) y);
          if ((double) num90 > 150.0)
          {
            float num91 = 150f / num90;
            x *= num91;
            y *= num91;
          }
          if ((double) this.position.X < (double) this.ai[0] * 16.0 + 8.0 + (double) x)
          {
            this.velocity.X += num89;
            if ((double) this.velocity.X < 0.0 && (double) x > 0.0)
              this.velocity.X += num89 * 2f;
          }
          else if ((double) this.position.X > (double) this.ai[0] * 16.0 + 8.0 + (double) x)
          {
            this.velocity.X -= num89;
            if ((double) this.velocity.X > 0.0 && (double) x < 0.0)
              this.velocity.X -= num89 * 2f;
          }
          if ((double) this.position.Y < (double) this.ai[1] * 16.0 + 8.0 + (double) y)
          {
            this.velocity.Y += num89;
            if ((double) this.velocity.Y < 0.0 && (double) y > 0.0)
              this.velocity.Y += num89 * 2f;
          }
          else if ((double) this.position.Y > (double) this.ai[1] * 16.0 + 8.0 + (double) y)
          {
            this.velocity.Y -= num89;
            if ((double) this.velocity.Y > 0.0 && (double) y < 0.0)
              this.velocity.Y -= num89 * 2f;
          }
          if ((double) this.velocity.X > 2.0)
            this.velocity.X = 2f;
          if ((double) this.velocity.X < -2.0)
            this.velocity.X = -2f;
          if ((double) this.velocity.Y > 2.0)
            this.velocity.Y = 2f;
          if ((double) this.velocity.Y < -2.0)
            this.velocity.Y = -2f;
          if ((double) x > 0.0)
          {
            this.spriteDirection = 1;
            this.rotation = (float) Math.Atan2((double) y, (double) x);
          }
          if ((double) x < 0.0)
          {
            this.spriteDirection = -1;
            this.rotation = (float) Math.Atan2((double) y, (double) x) + 3.14f;
          }
          if (this.collideX)
          {
            this.netUpdate = true;
            this.velocity.X = this.oldVelocity.X * -0.7f;
            if ((double) this.velocity.X > 0.0 && (double) this.velocity.X < 2.0)
              this.velocity.X = 2f;
            if ((double) this.velocity.X < 0.0 && (double) this.velocity.X > -2.0)
              this.velocity.X = -2f;
          }
          if (this.collideY)
          {
            this.netUpdate = true;
            this.velocity.Y = this.oldVelocity.Y * -0.7f;
            if ((double) this.velocity.Y > 0.0 && (double) this.velocity.Y < 2.0)
              this.velocity.Y = 2f;
            if ((double) this.velocity.Y < 0.0 && (double) this.velocity.Y > -2.0)
              this.velocity.Y = -2f;
          }
        }
      }
    }

    public void FindFrame()
    {
      int num1 = Game1.npcTexture[this.type].Height / Game1.npcFrameCount[this.type];
      int num2 = 0;
      if (this.aiAction == 0)
        num2 = (double) this.velocity.Y >= 0.0 ? ((double) this.velocity.Y <= 0.0 ? ((double) this.velocity.X == 0.0 ? 0 : 1) : 3) : 2;
      else if (this.aiAction == 1)
        num2 = 4;
      if (this.type == 1 || this.type == 16)
      {
        ++this.frameCounter;
        if (num2 > 0)
          ++this.frameCounter;
        if (num2 == 4)
          ++this.frameCounter;
        if (this.frameCounter >= 8.0)
        {
          this.frame.Y += num1;
          this.frameCounter = 0.0;
        }
        if (this.frame.Y >= num1 * Game1.npcFrameCount[this.type])
          this.frame.Y = 0;
      }
      if (this.type == 2 || this.type == 23)
      {
        if ((double) this.velocity.X > 0.0)
        {
          this.spriteDirection = 1;
          this.rotation = (float) Math.Atan2((double) this.velocity.Y, (double) this.velocity.X);
        }
        if ((double) this.velocity.X < 0.0)
        {
          this.spriteDirection = -1;
          this.rotation = (float) Math.Atan2((double) this.velocity.Y, (double) this.velocity.X) + 3.14f;
        }
        ++this.frameCounter;
        if (this.frameCounter >= 8.0)
        {
          this.frame.Y += num1;
          this.frameCounter = 0.0;
        }
        if (this.frame.Y >= num1 * Game1.npcFrameCount[this.type])
          this.frame.Y = 0;
      }
      if (this.type == 42)
      {
        if ((double) this.velocity.X > 0.0)
        {
          this.spriteDirection = 1;
          this.rotation = (float) Math.Atan2((double) this.velocity.Y, (double) this.velocity.X);
        }
        if ((double) this.velocity.X < 0.0)
        {
          this.spriteDirection = -1;
          this.rotation = (float) Math.Atan2((double) this.velocity.Y, (double) this.velocity.X) + 3.14f;
        }
        ++this.frameCounter;
        if (this.frameCounter < 4.0)
          this.frame.Y = 0;
        else if (this.frameCounter < 8.0)
          this.frame.Y = num1;
        else if (this.frameCounter < 12.0)
          this.frame.Y = num1 * 2;
        else if (this.frameCounter < 16.0)
          this.frame.Y = num1;
        if (this.frameCounter == 15.0)
          this.frameCounter = 0.0;
      }
      if (this.type == 43)
      {
        ++this.frameCounter;
        if (this.frameCounter < 6.0)
          this.frame.Y = 0;
        else if (this.frameCounter < 12.0)
          this.frame.Y = num1;
        else if (this.frameCounter < 18.0)
          this.frame.Y = num1 * 2;
        else if (this.frameCounter < 24.0)
          this.frame.Y = num1;
        if (this.frameCounter == 23.0)
          this.frameCounter = 0.0;
      }
      if (this.type == 18 || this.type == 22)
      {
        if ((double) this.velocity.Y == 0.0)
        {
          if (this.direction == 1)
            this.spriteDirection = 1;
          if (this.direction == -1)
            this.spriteDirection = -1;
          if ((double) this.velocity.X == 0.0)
          {
            this.frame.Y = 0;
            this.frameCounter = 0.0;
          }
          else
          {
            this.frameCounter += (double) Math.Abs(this.velocity.X);
            if (this.frameCounter < 8.0)
              this.frame.Y = num1;
            else if (this.frameCounter < 16.0)
              this.frame.Y = num1 * 2;
            else if (this.frameCounter < 24.0)
              this.frame.Y = num1 * 3;
            else if (this.frameCounter < 32.0)
              this.frame.Y = num1 * 4;
            else
              this.frameCounter = 0.0;
          }
        }
        else
        {
          this.frameCounter = 0.0;
          this.frame.Y = num1 * 3;
        }
      }
      else if (this.type == 3 || this.townNPC || this.type == 21 || this.type == 26 || this.type == 27 || this.type == 28 || this.type == 31)
      {
        if ((double) this.velocity.Y == 0.0)
        {
          if (this.direction == 1)
            this.spriteDirection = 1;
          if (this.direction == -1)
            this.spriteDirection = -1;
        }
        if ((double) this.velocity.Y != 0.0 || this.direction == -1 && (double) this.velocity.X > 0.0 || this.direction == 1 && (double) this.velocity.X < 0.0)
        {
          this.frameCounter = 0.0;
          this.frame.Y = num1 * 2;
        }
        else if ((double) this.velocity.X == 0.0)
        {
          this.frameCounter = 0.0;
          this.frame.Y = 0;
        }
        else
        {
          this.frameCounter += (double) Math.Abs(this.velocity.X);
          if (this.frameCounter < 8.0)
            this.frame.Y = 0;
          else if (this.frameCounter < 16.0)
            this.frame.Y = num1;
          else if (this.frameCounter < 24.0)
            this.frame.Y = num1 * 2;
          else if (this.frameCounter < 32.0)
            this.frame.Y = num1;
          else
            this.frameCounter = 0.0;
        }
      }
      else if (this.type == 4)
      {
        ++this.frameCounter;
        if (this.frameCounter < 7.0)
          this.frame.Y = 0;
        else if (this.frameCounter < 14.0)
          this.frame.Y = num1;
        else if (this.frameCounter < 21.0)
        {
          this.frame.Y = num1 * 2;
        }
        else
        {
          this.frameCounter = 0.0;
          this.frame.Y = 0;
        }
        if ((double) this.ai[0] > 1.0)
          this.frame.Y += num1 * 3;
      }
      else if (this.type == 5)
      {
        ++this.frameCounter;
        if (this.frameCounter >= 8.0)
        {
          this.frame.Y += num1;
          this.frameCounter = 0.0;
        }
        if (this.frame.Y >= num1 * Game1.npcFrameCount[this.type])
          this.frame.Y = 0;
      }
      else if (this.type == 6)
      {
        ++this.frameCounter;
        if (this.frameCounter >= 8.0)
        {
          this.frame.Y += num1;
          this.frameCounter = 0.0;
        }
        if (this.frame.Y >= num1 * Game1.npcFrameCount[this.type])
          this.frame.Y = 0;
      }
      else if (this.type == 24)
      {
        if ((double) this.velocity.Y == 0.0)
        {
          if (this.direction == 1)
            this.spriteDirection = 1;
          if (this.direction == -1)
            this.spriteDirection = -1;
        }
        ++this.frameCounter;
        if (this.frameCounter >= 4.0)
        {
          this.frame.Y = num1;
          if (this.frameCounter >= 7.0)
            this.frameCounter = 0.0;
        }
        else
          this.frame.Y = 0;
        if ((double) this.velocity.Y != 0.0)
          this.frame.Y += num1 * 2;
        else if ((double) this.ai[1] > 0.0)
          this.frame.Y += num1 * 4;
      }
      else if (this.type == 29 || this.type == 32)
      {
        if ((double) this.velocity.Y == 0.0)
        {
          if (this.direction == 1)
            this.spriteDirection = 1;
          if (this.direction == -1)
            this.spriteDirection = -1;
        }
        this.frame.Y = 0;
        if ((double) this.velocity.Y != 0.0)
          this.frame.Y += num1;
        else if ((double) this.ai[1] > 0.0)
          this.frame.Y += num1 * 2;
      }
      if (this.type != 34)
        return;
      if ((double) this.velocity.X > 0.0)
      {
        this.spriteDirection = -1;
        this.rotation = (float) Math.Atan2((double) this.velocity.Y, (double) this.velocity.X);
      }
      if ((double) this.velocity.X < 0.0)
      {
        this.spriteDirection = 1;
        this.rotation = (float) Math.Atan2((double) this.velocity.Y, (double) this.velocity.X) + 3.14f;
      }
      ++this.frameCounter;
      if (this.frameCounter >= 4.0)
      {
        this.frame.Y += num1;
        this.frameCounter = 0.0;
      }
      if (this.frame.Y >= num1 * Game1.npcFrameCount[this.type])
        this.frame.Y = 0;
    }

    public void TargetClosest()
    {
      float num = -1f;
      for (int index = 0; index < 8; ++index)
      {
        if (Game1.player[index].active && !Game1.player[index].dead && ((double) num == -1.0 || (double) Math.Abs(Game1.player[index].position.X + (float) (Game1.player[index].width / 2) - this.position.X + (float) (this.width / 2)) + (double) Math.Abs(Game1.player[index].position.Y + (float) (Game1.player[index].height / 2) - this.position.Y + (float) (this.height / 2)) < (double) num))
        {
          num = Math.Abs(Game1.player[index].position.X + (float) (Game1.player[index].width / 2) - this.position.X + (float) (this.width / 2)) + Math.Abs(Game1.player[index].position.Y + (float) (Game1.player[index].height / 2) - this.position.Y + (float) (this.height / 2));
          this.target = index;
        }
      }
      if (this.target < 0 || this.target >= 8)
        this.target = 0;
      this.targetRect = new Rectangle((int) Game1.player[this.target].position.X, (int) Game1.player[this.target].position.Y, Game1.player[this.target].width, Game1.player[this.target].height);
      this.direction = 1;
      if ((double) (this.targetRect.X + this.targetRect.Width / 2) < (double) this.position.X + (double) (this.width / 2))
        this.direction = -1;
      this.directionY = 1;
      if ((double) (this.targetRect.Y + this.targetRect.Height / 2) < (double) this.position.Y + (double) (this.height / 2))
        this.directionY = -1;
      if (this.direction == this.oldDirection && this.directionY == this.oldDirectionY && this.target == this.oldTarget)
        return;
      this.netUpdate = true;
    }

    public void CheckActive()
    {
      if (!this.active || this.type == 8 || this.type == 9 || this.type == 11 || this.type == 12 || this.type == 14 || this.type == 15 || this.type == 40 || this.type == 41)
        return;
      if (this.townNPC)
      {
        if ((double) this.position.Y >= Game1.worldSurface * 18.0)
          return;
        Rectangle rectangle = new Rectangle((int) ((double) this.position.X + (double) (this.width / 2) - (double) NPC.townRangeX), (int) ((double) this.position.Y + (double) (this.height / 2) - (double) NPC.townRangeY), NPC.townRangeX * 2, NPC.townRangeY * 2);
        for (int index = 0; index < 8; ++index)
        {
          if (Game1.player[index].active && rectangle.Intersects(new Rectangle((int) Game1.player[index].position.X, (int) Game1.player[index].position.Y, Game1.player[index].width, Game1.player[index].height)))
            ++Game1.player[index].townNPCs;
        }
      }
      else
      {
        bool flag = false;
        Rectangle rectangle1 = new Rectangle((int) ((double) this.position.X + (double) (this.width / 2) - (double) NPC.activeRangeX), (int) ((double) this.position.Y + (double) (this.height / 2) - (double) NPC.activeRangeY), NPC.activeRangeX * 2, NPC.activeRangeY * 2);
        Rectangle rectangle2 = new Rectangle((int) ((double) this.position.X + (double) (this.width / 2) - (double) Game1.screenWidth * 0.5 - (double) this.width), (int) ((double) this.position.Y + (double) (this.height / 2) - (double) Game1.screenHeight * 0.5 - (double) this.height), Game1.screenWidth + this.width * 2, Game1.screenHeight + this.height * 2);
        for (int index = 0; index < 8; ++index)
        {
          if (Game1.player[index].active)
          {
            if (rectangle1.Intersects(new Rectangle((int) Game1.player[index].position.X, (int) Game1.player[index].position.Y, Game1.player[index].width, Game1.player[index].height)))
            {
              flag = true;
              if (this.type != 25 && this.type != 30 && this.type != 33)
                ++Game1.player[index].activeNPCs;
            }
            if (rectangle2.Intersects(new Rectangle((int) Game1.player[index].position.X, (int) Game1.player[index].position.Y, Game1.player[index].width, Game1.player[index].height)))
              this.timeLeft = NPC.activeTime;
            if (this.type == 7 || this.type == 10 || this.type == 13)
              flag = true;
            if (this.boss || this.type == 35 || this.type == 36)
              flag = true;
          }
        }
        --this.timeLeft;
        if (this.timeLeft <= 0)
          flag = false;
        if (!flag && Game1.netMode != 1)
        {
          this.active = false;
          if (Game1.netMode == 2)
          {
            this.life = 0;
            NetMessage.SendData(23, number: this.whoAmI);
          }
        }
      }
    }

    public static void SpawnNPC()
    {
      if (Game1.stopSpawns)
        return;
      bool flag1 = false;
      int index1 = 0;
      int index2 = 0;
      int num1 = 0;
      int num2 = 0;
      for (int index3 = 0; index3 < 8; ++index3)
      {
        if (Game1.player[index3].active)
          ++num2;
      }
      for (int index4 = 0; index4 < 8; ++index4)
      {
        bool flag2 = false;
        if (Game1.player[index4].active && Game1.invasionType > 0 && Game1.invasionDelay == 0 && Game1.invasionSize > 0 && (double) Game1.player[index4].position.Y < Game1.worldSurface * 16.0 + (double) Game1.screenHeight)
        {
          int num3 = 3000;
          if ((double) Game1.player[index4].position.X > Game1.invasionX * 16.0 - (double) num3 && (double) Game1.player[index4].position.X < Game1.invasionX * 16.0 + (double) num3)
            flag2 = true;
        }
        bool flag3 = false;
        NPC.spawnRate = NPC.defaultSpawnRate;
        NPC.maxSpawns = NPC.defaultMaxSpawns;
        if ((double) Game1.player[index4].position.Y > (double) ((Game1.maxTilesY - 200) * 16))
        {
          NPC.spawnRate = (int) ((double) NPC.spawnRate * 1.5);
          NPC.maxSpawns = (int) ((double) NPC.maxSpawns * 0.5);
        }
        else if ((double) Game1.player[index4].position.Y > Game1.rockLayer * 16.0 + (double) Game1.screenHeight)
        {
          NPC.spawnRate = (int) ((double) NPC.spawnRate * 0.65);
          NPC.maxSpawns = (int) ((double) NPC.maxSpawns * 1.3999999761581421);
        }
        else if ((double) Game1.player[index4].position.Y > Game1.worldSurface * 16.0 + (double) Game1.screenHeight)
        {
          NPC.spawnRate = (int) ((double) NPC.spawnRate * 0.8);
          NPC.maxSpawns = (int) ((double) NPC.maxSpawns * 1.1000000238418579);
        }
        else if (!Game1.dayTime)
        {
          NPC.spawnRate = (int) ((double) NPC.spawnRate * 0.7);
          NPC.maxSpawns = (int) ((double) NPC.maxSpawns * 1.2000000476837158);
          if (Game1.bloodMoon)
          {
            NPC.spawnRate = (int) ((double) NPC.spawnRate * 0.3);
            NPC.maxSpawns = (int) ((double) NPC.maxSpawns * 1.7999999523162842);
          }
        }
        if (Game1.player[index4].zoneDungeon)
        {
          NPC.spawnRate = (int) ((double) NPC.defaultSpawnRate * 0.1);
          NPC.maxSpawns = (int) ((double) NPC.defaultMaxSpawns * 2.5);
        }
        else if (Game1.player[index4].zoneEvil)
        {
          NPC.spawnRate = (int) ((double) NPC.spawnRate * 0.2);
          NPC.maxSpawns = (int) ((double) NPC.maxSpawns * 1.3999999761581421);
        }
        else if (Game1.player[index4].zoneMeteor)
          NPC.spawnRate = (int) ((double) NPC.spawnRate * 0.3);
        else if (Game1.player[index4].zoneJungle)
        {
          NPC.spawnRate = (int) ((double) NPC.spawnRate * 0.3);
          NPC.maxSpawns = (int) ((double) NPC.maxSpawns * 1.6000000238418579);
        }
        if ((double) NPC.spawnRate < (double) NPC.defaultSpawnRate * 0.1)
          NPC.spawnRate = (int) ((double) NPC.defaultSpawnRate * 0.1);
        if ((double) NPC.maxSpawns > (double) NPC.defaultMaxSpawns * 2.5)
          NPC.maxSpawns = (int) ((double) NPC.defaultMaxSpawns * 2.5);
        if (Game1.player[index4].inventory[Game1.player[index4].selectedItem].type == 49)
        {
          NPC.spawnRate = (int) ((double) NPC.spawnRate * 0.75);
          NPC.maxSpawns = (int) ((double) NPC.maxSpawns * 1.5);
        }
        if (flag2)
        {
          NPC.maxSpawns = (int) ((double) NPC.defaultMaxSpawns * (1.0 + 0.5 * (double) num2));
          NPC.spawnRate = 30;
        }
        if (!flag2 && (!Game1.bloodMoon || Game1.dayTime) && !Game1.player[index4].zoneDungeon && !Game1.player[index4].zoneEvil && !Game1.player[index4].zoneMeteor)
        {
          if (Game1.player[index4].townNPCs == 1)
          {
            NPC.maxSpawns /= 2;
            NPC.spawnRate *= 2;
          }
          else if (Game1.player[index4].townNPCs >= 2)
          {
            NPC.maxSpawns = 0;
            NPC.spawnRate = 99999;
          }
        }
        if (Game1.player[index4].active && !Game1.player[index4].dead && Game1.player[index4].activeNPCs < NPC.maxSpawns && Game1.rand.Next(NPC.spawnRate) == 0)
        {
          int minValue1 = (int) ((double) Game1.player[index4].position.X / 16.0) - NPC.spawnRangeX;
          int maxValue1 = (int) ((double) Game1.player[index4].position.X / 16.0) + NPC.spawnRangeX;
          int minValue2 = (int) ((double) Game1.player[index4].position.Y / 16.0) - NPC.spawnRangeY;
          int maxValue2 = (int) ((double) Game1.player[index4].position.Y / 16.0) + NPC.spawnRangeY;
          int num4 = (int) ((double) Game1.player[index4].position.X / 16.0) - NPC.safeRangeX;
          int num5 = (int) ((double) Game1.player[index4].position.X / 16.0) + NPC.safeRangeX;
          int num6 = (int) ((double) Game1.player[index4].position.Y / 16.0) - NPC.safeRangeY;
          int num7 = (int) ((double) Game1.player[index4].position.Y / 16.0) + NPC.safeRangeY;
          if (minValue1 < 0)
            minValue1 = 0;
          if (maxValue1 > Game1.maxTilesX)
            maxValue1 = Game1.maxTilesX;
          if (minValue2 < 0)
            minValue2 = 0;
          if (maxValue2 > Game1.maxTilesY)
            maxValue2 = Game1.maxTilesY;
          for (int index5 = 0; index5 < 100; ++index5)
          {
            int index6 = Game1.rand.Next(minValue1, maxValue1);
            int index7 = Game1.rand.Next(minValue2, maxValue2);
            if (!Game1.tile[index6, index7].active || !Game1.tileSolid[(int) Game1.tile[index6, index7].type])
            {
              if (!Game1.wallHouse[(int) Game1.tile[index6, index7].wall])
              {
                for (int index8 = index7; index8 < Game1.maxTilesY; ++index8)
                {
                  if (Game1.tile[index6, index8].active && Game1.tileSolid[(int) Game1.tile[index6, index8].type])
                  {
                    if (index6 < num4 || index6 > num5 || index8 < num6 || index8 > num7)
                    {
                      num1 = (int) Game1.tile[index6, index8].type;
                      index1 = index6;
                      index2 = index8;
                      flag3 = true;
                      break;
                    }
                    break;
                  }
                }
                if (flag3)
                {
                  int num8 = index1 - NPC.spawnSpaceX / 2;
                  int num9 = index1 + NPC.spawnSpaceX / 2;
                  int num10 = index2 - NPC.spawnSpaceY;
                  int num11 = index2;
                  if (num8 < 0)
                    flag3 = false;
                  if (num9 > Game1.maxTilesX)
                    flag3 = false;
                  if (num10 < 0)
                    flag3 = false;
                  if (num11 > Game1.maxTilesY)
                    flag3 = false;
                  if (flag3)
                  {
                    for (int index9 = num8; index9 < num9; ++index9)
                    {
                      for (int index10 = num10; index10 < num11; ++index10)
                      {
                        if (Game1.tile[index9, index10].active && Game1.tileSolid[(int) Game1.tile[index9, index10].type])
                        {
                          flag3 = false;
                          break;
                        }
                        if (Game1.tile[index9, index10].lava && index10 < Game1.maxTilesY - 200)
                        {
                          flag3 = false;
                          break;
                        }
                      }
                    }
                  }
                }
              }
              else
                continue;
            }
            if (flag3 || flag3)
              break;
          }
        }
        if (flag3)
        {
          Rectangle rectangle1 = new Rectangle(index1 * 16, index2 * 16, 16, 16);
          for (int index11 = 0; index11 < 8; ++index11)
          {
            if (Game1.player[index11].active)
            {
              Rectangle rectangle2 = new Rectangle((int) ((double) Game1.player[index11].position.X + (double) (Game1.player[index11].width / 2) - (double) (Game1.screenWidth / 2) - (double) NPC.safeRangeX), (int) ((double) Game1.player[index11].position.Y + (double) (Game1.player[index11].height / 2) - (double) (Game1.screenHeight / 2) - (double) NPC.safeRangeY), Game1.screenWidth + NPC.safeRangeX * 2, Game1.screenHeight + NPC.safeRangeY * 2);
              if (rectangle1.Intersects(rectangle2))
                flag3 = false;
            }
          }
        }
        if (flag3 && Game1.player[index4].zoneDungeon && (!Game1.tileDungeon[(int) Game1.tile[index1, index2].type] || Game1.tile[index1, index2 - 1].wall == (byte) 0))
          flag3 = false;
        if (flag3)
        {
          flag1 = false;
          int type = (int) Game1.tile[index1, index2].type;
          int number = 1000;
          if (flag2)
          {
            if (Game1.rand.Next(9) == 0)
              NPC.NewNPC(index1 * 16 + 8, index2 * 16, 29);
            else if (Game1.rand.Next(5) == 0)
              NPC.NewNPC(index1 * 16 + 8, index2 * 16, 26);
            else if (Game1.rand.Next(3) == 0)
              NPC.NewNPC(index1 * 16 + 8, index2 * 16, 27);
            else
              NPC.NewNPC(index1 * 16 + 8, index2 * 16, 28);
          }
          else if (Game1.player[index4].zoneDungeon)
          {
            if (!NPC.downedBoss3)
            {
              number = NPC.NewNPC(index1 * 16 + 8, index2 * 16, 35);
              Game1.npc[number].ai[0] = 1f;
              Game1.npc[number].ai[2] = 2f;
            }
            else
              number = Game1.rand.Next(4) != 0 ? (Game1.rand.Next(5) != 0 ? NPC.NewNPC(index1 * 16 + 8, index2 * 16, 31) : NPC.NewNPC(index1 * 16 + 8, index2 * 16, 32)) : NPC.NewNPC(index1 * 16 + 8, index2 * 16, 34);
          }
          else if (Game1.player[index4].zoneMeteor)
            number = NPC.NewNPC(index1 * 16 + 8, index2 * 16, 23);
          else if (Game1.player[index4].zoneEvil && Game1.rand.Next(20) == 0)
            number = NPC.NewNPC(index1 * 16 + 8, index2 * 16, 7, 1);
          else if (type == 60)
          {
            if (Game1.rand.Next(3) == 0)
            {
              number = NPC.NewNPC(index1 * 16 + 8, index2 * 16, 43);
              Game1.npc[number].ai[0] = (float) index1;
              Game1.npc[number].ai[1] = (float) index2;
              Game1.npc[number].netUpdate = true;
            }
            else
              number = NPC.NewNPC(index1 * 16 + 8, index2 * 16, 42);
          }
          else if ((double) index2 <= Game1.worldSurface)
          {
            if (type == 23 || type == 25)
              number = NPC.NewNPC(index1 * 16 + 8, index2 * 16, 6);
            else if (Game1.dayTime)
            {
              number = NPC.NewNPC(index1 * 16 + 8, index2 * 16, 1);
              if (Game1.rand.Next(3) == 0)
                Game1.npc[number].SetDefaults("Green Slime");
              else if (Game1.rand.Next(10) == 0)
                Game1.npc[number].SetDefaults("Purple Slime");
            }
            else if (Game1.rand.Next(6) == 0 || Game1.moonPhase == 4 && Game1.rand.Next(2) == 0)
              number = NPC.NewNPC(index1 * 16 + 8, index2 * 16, 2);
            else
              NPC.NewNPC(index1 * 16 + 8, index2 * 16, 3);
          }
          else if ((double) index2 <= Game1.rockLayer)
          {
            if (Game1.rand.Next(30) == 0)
            {
              number = NPC.NewNPC(index1 * 16 + 8, index2 * 16, 10, 1);
            }
            else
            {
              number = NPC.NewNPC(index1 * 16 + 8, index2 * 16, 1);
              if (Game1.rand.Next(5) == 0)
                Game1.npc[number].SetDefaults("Yellow Slime");
              else if (Game1.rand.Next(2) == 0)
                Game1.npc[number].SetDefaults("Blue Slime");
              else
                Game1.npc[number].SetDefaults("Red Slime");
            }
          }
          else
            number = index2 <= Game1.maxTilesY - 190 ? (Game1.rand.Next(15) != 0 ? (Game1.rand.Next(3) != 0 ? NPC.NewNPC(index1 * 16 + 8, index2 * 16, 21) : NPC.NewNPC(index1 * 16 + 8, index2 * 16, 16)) : NPC.NewNPC(index1 * 16 + 8, index2 * 16, 10, 1)) : (Game1.rand.Next(5) != 0 ? NPC.NewNPC(index1 * 16 + 8, index2 * 16, 24) : NPC.NewNPC(index1 * 16 + 8, index2 * 16, 39, 1));
          if (Game1.netMode != 2 || number >= 1000)
            break;
          NetMessage.SendData(23, number: number);
          break;
        }
      }
    }

    public static void SpawnOnPlayer(int plr, int Type)
    {
      bool flag = false;
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int minValue1 = (int) ((double) Game1.player[plr].position.X / 16.0) - NPC.spawnRangeX * 3;
      int maxValue1 = (int) ((double) Game1.player[plr].position.X / 16.0) + NPC.spawnRangeX * 3;
      int minValue2 = (int) ((double) Game1.player[plr].position.Y / 16.0) - NPC.spawnRangeY * 3;
      int maxValue2 = (int) ((double) Game1.player[plr].position.Y / 16.0) + NPC.spawnRangeY * 3;
      int num4 = (int) ((double) Game1.player[plr].position.X / 16.0) - NPC.safeRangeX;
      int num5 = (int) ((double) Game1.player[plr].position.X / 16.0) + NPC.safeRangeX;
      int num6 = (int) ((double) Game1.player[plr].position.Y / 16.0) - NPC.safeRangeY;
      int num7 = (int) ((double) Game1.player[plr].position.Y / 16.0) + NPC.safeRangeY;
      if (minValue1 < 0)
        minValue1 = 0;
      if (maxValue1 > Game1.maxTilesX)
        maxValue1 = Game1.maxTilesX;
      if (minValue2 < 0)
        minValue2 = 0;
      if (maxValue2 > Game1.maxTilesY)
        maxValue2 = Game1.maxTilesY;
      for (int index1 = 0; index1 < 1000; ++index1)
      {
        for (int index2 = 0; index2 < 100; ++index2)
        {
          int index3 = Game1.rand.Next(minValue1, maxValue1);
          int index4 = Game1.rand.Next(minValue2, maxValue2);
          if (!Game1.tile[index3, index4].active || !Game1.tileSolid[(int) Game1.tile[index3, index4].type])
          {
            if (Game1.tile[index3, index4].wall != (byte) 1)
            {
              for (int index5 = index4; index5 < Game1.maxTilesY; ++index5)
              {
                if (Game1.tile[index3, index5].active && Game1.tileSolid[(int) Game1.tile[index3, index5].type])
                {
                  if (index3 < num4 || index3 > num5 || index5 < num6 || index5 > num7)
                  {
                    num3 = (int) Game1.tile[index3, index5].type;
                    num1 = index3;
                    num2 = index5;
                    flag = true;
                    break;
                  }
                  break;
                }
              }
              if (flag)
              {
                int num8 = num1 - NPC.spawnSpaceX / 2;
                int num9 = num1 + NPC.spawnSpaceX / 2;
                int num10 = num2 - NPC.spawnSpaceY;
                int num11 = num2;
                if (num8 < 0)
                  flag = false;
                if (num9 > Game1.maxTilesX)
                  flag = false;
                if (num10 < 0)
                  flag = false;
                if (num11 > Game1.maxTilesY)
                  flag = false;
                if (flag)
                {
                  for (int index6 = num8; index6 < num9; ++index6)
                  {
                    for (int index7 = num10; index7 < num11; ++index7)
                    {
                      if (Game1.tile[index6, index7].active && Game1.tileSolid[(int) Game1.tile[index6, index7].type])
                      {
                        flag = false;
                        break;
                      }
                    }
                  }
                }
              }
            }
            else
              continue;
          }
          if (flag || flag)
            break;
        }
        if (flag)
        {
          Rectangle rectangle1 = new Rectangle(num1 * 16, num2 * 16, 16, 16);
          for (int index8 = 0; index8 < 8; ++index8)
          {
            if (Game1.player[index8].active)
            {
              Rectangle rectangle2 = new Rectangle((int) ((double) Game1.player[index8].position.X + (double) (Game1.player[index8].width / 2) - (double) (Game1.screenWidth / 2) - (double) NPC.safeRangeX), (int) ((double) Game1.player[index8].position.Y + (double) (Game1.player[index8].height / 2) - (double) (Game1.screenHeight / 2) - (double) NPC.safeRangeY), Game1.screenWidth + NPC.safeRangeX * 2, Game1.screenHeight + NPC.safeRangeY * 2);
              if (rectangle1.Intersects(rectangle2))
                flag = false;
            }
          }
        }
        if (flag)
          break;
      }
      if (!flag)
        return;
      int number = NPC.NewNPC(num1 * 16 + 8, num2 * 16, Type, 1);
      Game1.npc[number].target = plr;
      string str = Game1.npc[number].name;
      if (Game1.npc[number].type == 13)
        str = "Eater of Worlds";
      if (Game1.netMode == 2 && number < 1000)
        NetMessage.SendData(23, number: number);
      switch (Game1.netMode)
      {
        case 0:
          Game1.NewText(str + " has awoken!", (byte) 175, (byte) 75);
          break;
        case 2:
          NetMessage.SendData(25, text: str + " has awoken!", number: 8, number2: 175f, number3: 75f, number4: (float) byte.MaxValue);
          break;
      }
    }

    public static int NewNPC(int X, int Y, int Type, int Start = 0)
    {
      int index1 = -1;
      for (int index2 = Start; index2 < 1000; ++index2)
      {
        if (!Game1.npc[index2].active)
        {
          index1 = index2;
          break;
        }
      }
      if (index1 < 0)
        return 1000;
      Game1.npc[index1] = new NPC();
      Game1.npc[index1].SetDefaults(Type);
      Game1.npc[index1].position.X = (float) (X - Game1.npc[index1].width / 2);
      Game1.npc[index1].position.Y = (float) (Y - Game1.npc[index1].height);
      Game1.npc[index1].active = true;
      Game1.npc[index1].timeLeft = (int) ((double) NPC.activeTime * 1.25);
      Game1.npc[index1].wet = Collision.WetCollision(Game1.npc[index1].position, Game1.npc[index1].width, Game1.npc[index1].height);
      return index1;
    }

    public double StrikeNPC(int Damage, float knockBack, int hitDirection)
    {
      if (!this.active || this.life <= 0)
        return 0.0;
      double damage = Game1.CalculateDamage(Damage, this.defense);
      if (this.friendly)
        CombatText.NewText(new Rectangle((int) this.position.X, (int) this.position.Y, this.width, this.height), new Color((int) byte.MaxValue, 80, 90, (int) byte.MaxValue), string.Concat((object) (int) damage));
      else
        CombatText.NewText(new Rectangle((int) this.position.X, (int) this.position.Y, this.width, this.height), new Color((int) byte.MaxValue, 160, 80, (int) byte.MaxValue), string.Concat((object) (int) damage));
      if (damage < 1.0)
        return 0.0;
      if (this.townNPC)
      {
        this.ai[0] = 1f;
        this.ai[1] = (float) (300 + Game1.rand.Next(300));
        this.ai[2] = 0.0f;
        this.direction = hitDirection;
        this.netUpdate = true;
      }
      if (this.aiStyle == 8 && Game1.netMode != 1)
      {
        this.ai[0] = 400f;
        this.TargetClosest();
      }
      this.life -= (int) damage;
      if ((double) knockBack > 0.0 && (double) this.knockBackResist > 0.0)
      {
        this.velocity.Y = this.noGravity ? (float) (-(double) knockBack * 0.5) * this.knockBackResist : (float) (-(double) knockBack * 0.75) * this.knockBackResist;
        this.velocity.X = knockBack * (float) hitDirection * this.knockBackResist;
      }
      this.HitEffect(hitDirection, damage);
      if (this.soundHit > 0)
        Game1.PlaySound(3, (int) this.position.X, (int) this.position.Y, this.soundHit);
      if (this.life <= 0)
      {
        if (this.townNPC && this.type != 37)
        {
          if (Game1.netMode == 0)
            Game1.NewText(this.name + " was slain...", G: (byte) 25, B: (byte) 25);
          else if (Game1.netMode == 2)
            NetMessage.SendData(25, text: this.name + " was slain...", number: 8, number2: (float) byte.MaxValue, number3: 25f, number4: 25f);
        }
        if (this.townNPC && Game1.netMode != 1 && this.homeless && WorldGen.spawnNPC == this.type)
          WorldGen.spawnNPC = 0;
        if (this.soundKilled > 0)
          Game1.PlaySound(4, (int) this.position.X, (int) this.position.Y, this.soundKilled);
        this.NPCLoot();
        this.active = false;
        if (this.type == 26 || this.type == 27 || this.type == 28 || this.type == 29)
          --Game1.invasionSize;
      }
      return damage;
    }

    public void NPCLoot()
    {
      if ((this.type == 1 || this.type == 16) && Game1.rand.Next(2) == 0)
        Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 23);
      if (this.type == 2 && Game1.rand.Next(3) == 0)
        Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 38);
      if (this.type == 4)
      {
        Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 47, Game1.rand.Next(30) + 20);
        Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 56, Game1.rand.Next(20) + 10);
        Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 56, Game1.rand.Next(20) + 10);
        Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 56, Game1.rand.Next(20) + 10);
        Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 59, Game1.rand.Next(3) + 1);
      }
      if (this.type == 6 && Game1.rand.Next(3) == 0)
        Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 68);
      if ((this.type == 7 || this.type == 8 || this.type == 9) && Game1.rand.Next(3) <= 1)
        Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 69);
      if (this.type == 13 || this.type == 14 || this.type == 15)
      {
        Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 86, Game1.rand.Next(2) + 1);
        if (Game1.rand.Next(2) == 0)
          Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 56, Game1.rand.Next(3) + 1);
        if (this.boss)
        {
          Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 56, Game1.rand.Next(20) + 10);
          Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 56, Game1.rand.Next(20) + 10);
        }
      }
      if (this.type == 21 && Game1.rand.Next(15) == 0)
        Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 118);
      if (this.type == 23 && Game1.rand.Next(3) == 0)
        Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 116);
      if (this.type == 24 && Game1.rand.Next(50) == 0)
        Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 112);
      if (this.type == 31 || this.type == 32)
        Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 154);
      if (this.type == 26 || this.type == 27 || this.type == 28 || this.type == 29)
      {
        if (Game1.rand.Next(400) == 0)
          Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 128);
        else if (Game1.rand.Next(200) == 0)
          Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 160);
        else if (Game1.rand.Next(2) == 0)
          Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 161, Game1.rand.Next(1, 6));
      }
      if (this.type == 42 || this.type == 43)
      {
        if (Game1.rand.Next(200) == 0)
          Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 190);
        else if (Game1.rand.Next(200) == 0)
          Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 191);
      }
      if (this.type == 43 && Game1.rand.Next(50) == 0)
        Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 185);
      if (this.boss)
      {
        if (this.type == 4)
          NPC.downedBoss1 = true;
        if (this.type == 13 || this.type == 14 || this.type == 15)
        {
          NPC.downedBoss2 = true;
          this.name = "Eater of Worlds";
        }
        if (this.type == 35)
        {
          NPC.downedBoss3 = true;
          this.name = "Skeletron";
        }
        Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 29);
        Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 28, Game1.rand.Next(15) + 15);
        int num = Game1.rand.Next(5) + 5;
        for (int index = 0; index < num; ++index)
          Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 58);
        if (Game1.netMode == 0)
          Game1.NewText(this.name + " has been defeated!", (byte) 175, (byte) 75);
        else if (Game1.netMode == 2)
          NetMessage.SendData(25, text: this.name + " has been defeated!", number: 8, number2: 175f, number3: 75f, number4: (float) byte.MaxValue);
      }
      if (Game1.rand.Next(6) == 0)
      {
        if (Game1.rand.Next(2) == 0)
          Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 58);
        else
          Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 184);
      }
      float num1 = this.value * (float) (1.0 + (double) Game1.rand.Next(-20, 21) * 0.0099999997764825821);
      if (Game1.rand.Next(5) == 0)
        num1 *= (float) (1.0 + (double) Game1.rand.Next(5, 11) * 0.0099999997764825821);
      if (Game1.rand.Next(10) == 0)
        num1 *= (float) (1.0 + (double) Game1.rand.Next(10, 21) * 0.0099999997764825821);
      if (Game1.rand.Next(15) == 0)
        num1 *= (float) (1.0 + (double) Game1.rand.Next(15, 31) * 0.0099999997764825821);
      if (Game1.rand.Next(20) == 0)
        num1 *= (float) (1.0 + (double) Game1.rand.Next(20, 41) * 0.0099999997764825821);
      while ((int) num1 > 0)
      {
        if ((double) num1 > 1000000.0)
        {
          int Stack = (int) ((double) num1 / 1000000.0);
          if (Stack > 50 && Game1.rand.Next(2) == 0)
            Stack /= Game1.rand.Next(3) + 1;
          if (Game1.rand.Next(2) == 0)
            Stack /= Game1.rand.Next(3) + 1;
          num1 -= (float) (1000000 * Stack);
          Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 74, Stack);
        }
        else if ((double) num1 > 10000.0)
        {
          int Stack = (int) ((double) num1 / 10000.0);
          if (Stack > 50 && Game1.rand.Next(2) == 0)
            Stack /= Game1.rand.Next(3) + 1;
          if (Game1.rand.Next(2) == 0)
            Stack /= Game1.rand.Next(3) + 1;
          num1 -= (float) (10000 * Stack);
          Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 73, Stack);
        }
        else if ((double) num1 > 100.0)
        {
          int Stack = (int) ((double) num1 / 100.0);
          if (Stack > 50 && Game1.rand.Next(2) == 0)
            Stack /= Game1.rand.Next(3) + 1;
          if (Game1.rand.Next(2) == 0)
            Stack /= Game1.rand.Next(3) + 1;
          num1 -= (float) (100 * Stack);
          Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 72, Stack);
        }
        else
        {
          int Stack = (int) num1;
          if (Stack > 50 && Game1.rand.Next(2) == 0)
            Stack /= Game1.rand.Next(3) + 1;
          if (Game1.rand.Next(2) == 0)
            Stack /= Game1.rand.Next(4) + 1;
          if (Stack < 1)
            Stack = 1;
          num1 -= (float) Stack;
          Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, 71, Stack);
        }
      }
    }

    public void HitEffect(int hitDirection = 0, double dmg = 10.0)
    {
      if (this.type == 1 || this.type == 16)
      {
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 100.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 4, (float) hitDirection, -1f, this.alpha, this.color);
        }
        else
        {
          for (int index = 0; index < 50; ++index)
            Dust.NewDust(this.position, this.width, this.height, 4, (float) (2 * hitDirection), -2f, this.alpha, this.color);
          if (Game1.netMode != 1 && this.type == 16)
          {
            int num = Game1.rand.Next(2) + 2;
            for (int index = 0; index < num; ++index)
            {
              int number = NPC.NewNPC((int) ((double) this.position.X + (double) (this.width / 2)), (int) ((double) this.position.Y + (double) this.height), 1);
              Game1.npc[number].SetDefaults("Baby Slime");
              Game1.npc[number].velocity.X = this.velocity.X * 2f;
              Game1.npc[number].velocity.Y = this.velocity.Y;
              Game1.npc[number].velocity.X += (float) ((double) Game1.rand.Next(-20, 20) * 0.10000000149011612 + (double) (index * this.direction) * 0.30000001192092896);
              Game1.npc[number].velocity.Y -= (float) Game1.rand.Next(0, 10) * 0.1f + (float) index;
              Game1.npc[number].ai[1] = (float) index;
              if (Game1.netMode == 2 && number < 1000)
                NetMessage.SendData(23, number: number);
            }
          }
        }
      }
      else if (this.type == 2)
      {
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 100.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, (float) hitDirection, -1f);
        }
        else
        {
          for (int index = 0; index < 50; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, (float) (2 * hitDirection), -2f);
          Gore.NewGore(this.position, this.velocity, 1);
          Gore.NewGore(new Vector2(this.position.X + 14f, this.position.Y), this.velocity, 2);
        }
      }
      else if (this.type == 3)
      {
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 100.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, (float) hitDirection, -1f);
        }
        else
        {
          for (int index = 0; index < 50; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, 2.5f * (float) hitDirection, -2.5f);
          Gore.NewGore(this.position, this.velocity, 3);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 20f), this.velocity, 4);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 20f), this.velocity, 4);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 34f), this.velocity, 5);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 34f), this.velocity, 5);
        }
      }
      else if (this.type == 4)
      {
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 100.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, (float) hitDirection, -1f);
        }
        else
        {
          for (int index = 0; index < 150; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, (float) (2 * hitDirection), -2f);
          for (int index = 0; index < 2; ++index)
          {
            Gore.NewGore(this.position, new Vector2((float) Game1.rand.Next(-30, 31) * 0.2f, (float) Game1.rand.Next(-30, 31) * 0.2f), 2);
            Gore.NewGore(this.position, new Vector2((float) Game1.rand.Next(-30, 31) * 0.2f, (float) Game1.rand.Next(-30, 31) * 0.2f), 7);
            Gore.NewGore(this.position, new Vector2((float) Game1.rand.Next(-30, 31) * 0.2f, (float) Game1.rand.Next(-30, 31) * 0.2f), 9);
            Gore.NewGore(this.position, new Vector2((float) Game1.rand.Next(-30, 31) * 0.2f, (float) Game1.rand.Next(-30, 31) * 0.2f), 10);
          }
          Game1.PlaySound(15, (int) this.position.X, (int) this.position.Y, 0);
        }
      }
      else if (this.type == 5)
      {
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 50.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, (float) hitDirection, -1f);
        }
        else
        {
          for (int index = 0; index < 20; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, (float) (2 * hitDirection), -2f);
          Gore.NewGore(this.position, this.velocity, 6);
          Gore.NewGore(this.position, this.velocity, 7);
        }
      }
      else if (this.type == 6)
      {
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 100.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 18, (float) hitDirection, -1f, this.alpha, this.color, this.scale);
        }
        else
        {
          for (int index = 0; index < 50; ++index)
            Dust.NewDust(this.position, this.width, this.height, 18, (float) hitDirection, -2f, this.alpha, this.color, this.scale);
          int index1 = Gore.NewGore(this.position, this.velocity, 14);
          Game1.gore[index1].alpha = this.alpha;
          int index2 = Gore.NewGore(this.position, this.velocity, 15);
          Game1.gore[index2].alpha = this.alpha;
        }
      }
      else if (this.type == 7 || this.type == 8 || this.type == 9)
      {
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 100.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 18, (float) hitDirection, -1f, this.alpha, this.color, this.scale);
        }
        else
        {
          for (int index = 0; index < 50; ++index)
            Dust.NewDust(this.position, this.width, this.height, 18, (float) hitDirection, -2f, this.alpha, this.color, this.scale);
          int index3 = Gore.NewGore(this.position, this.velocity, this.type - 7 + 18);
          Game1.gore[index3].alpha = this.alpha;
        }
      }
      else if (this.type == 10 || this.type == 11 || this.type == 12)
      {
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 50.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, (float) hitDirection, -1f);
        }
        else
        {
          for (int index = 0; index < 10; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, 2.5f * (float) hitDirection, -2.5f);
          Gore.NewGore(this.position, this.velocity, this.type - 7 + 18);
        }
      }
      else if (this.type == 13 || this.type == 14 || this.type == 15)
      {
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 100.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 18, (float) hitDirection, -1f, this.alpha, this.color, this.scale);
        }
        else
        {
          for (int index = 0; index < 50; ++index)
            Dust.NewDust(this.position, this.width, this.height, 18, (float) hitDirection, -2f, this.alpha, this.color, this.scale);
          if (this.type == 13)
          {
            Gore.NewGore(this.position, this.velocity, 24);
            Gore.NewGore(this.position, this.velocity, 25);
          }
          else if (this.type == 14)
          {
            Gore.NewGore(this.position, this.velocity, 26);
            Gore.NewGore(this.position, this.velocity, 27);
          }
          else
          {
            Gore.NewGore(this.position, this.velocity, 28);
            Gore.NewGore(this.position, this.velocity, 29);
          }
        }
      }
      else if (this.type == 17)
      {
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 100.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, (float) hitDirection, -1f);
        }
        else
        {
          for (int index = 0; index < 50; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, 2.5f * (float) hitDirection, -2.5f);
          Gore.NewGore(this.position, this.velocity, 30);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 20f), this.velocity, 31);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 20f), this.velocity, 31);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 34f), this.velocity, 32);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 34f), this.velocity, 32);
        }
      }
      else if (this.type == 37)
      {
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 100.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, (float) hitDirection, -1f);
        }
        else
        {
          for (int index = 0; index < 50; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, 2.5f * (float) hitDirection, -2.5f);
          Gore.NewGore(this.position, this.velocity, 58);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 20f), this.velocity, 59);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 20f), this.velocity, 59);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 34f), this.velocity, 60);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 34f), this.velocity, 60);
        }
      }
      else if (this.type == 18)
      {
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 100.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, (float) hitDirection, -1f);
        }
        else
        {
          for (int index = 0; index < 50; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, 2.5f * (float) hitDirection, -2.5f);
          Gore.NewGore(this.position, this.velocity, 33);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 20f), this.velocity, 34);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 20f), this.velocity, 34);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 34f), this.velocity, 35);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 34f), this.velocity, 35);
        }
      }
      else if (this.type == 19)
      {
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 100.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, (float) hitDirection, -1f);
        }
        else
        {
          for (int index = 0; index < 50; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, 2.5f * (float) hitDirection, -2.5f);
          Gore.NewGore(this.position, this.velocity, 36);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 20f), this.velocity, 37);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 20f), this.velocity, 37);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 34f), this.velocity, 38);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 34f), this.velocity, 38);
        }
      }
      else if (this.type == 38)
      {
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 100.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, (float) hitDirection, -1f);
        }
        else
        {
          for (int index = 0; index < 50; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, 2.5f * (float) hitDirection, -2.5f);
          Gore.NewGore(this.position, this.velocity, 64);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 20f), this.velocity, 65);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 20f), this.velocity, 65);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 34f), this.velocity, 66);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 34f), this.velocity, 66);
        }
      }
      else if (this.type == 20)
      {
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 100.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, (float) hitDirection, -1f);
        }
        else
        {
          for (int index = 0; index < 50; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, 2.5f * (float) hitDirection, -2.5f);
          Gore.NewGore(this.position, this.velocity, 39);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 20f), this.velocity, 40);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 20f), this.velocity, 40);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 34f), this.velocity, 41);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 34f), this.velocity, 41);
        }
      }
      else if (this.type == 21 || this.type == 31 || this.type == 32)
      {
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 50.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 26, (float) hitDirection, -1f);
        }
        else
        {
          for (int index = 0; index < 20; ++index)
            Dust.NewDust(this.position, this.width, this.height, 26, 2.5f * (float) hitDirection, -2.5f);
          Gore.NewGore(this.position, this.velocity, 42);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 20f), this.velocity, 43);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 20f), this.velocity, 43);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 34f), this.velocity, 44);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 34f), this.velocity, 44);
        }
      }
      else if (this.type == 39 || this.type == 40 || this.type == 41)
      {
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 50.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 26, (float) hitDirection, -1f);
        }
        else
        {
          for (int index = 0; index < 20; ++index)
            Dust.NewDust(this.position, this.width, this.height, 26, 2.5f * (float) hitDirection, -2.5f);
          Gore.NewGore(this.position, this.velocity, this.type - 39 + 67);
        }
      }
      else if (this.type == 34)
      {
        if (this.life > 0)
        {
          for (int index4 = 0; (double) index4 < dmg / (double) this.lifeMax * 50.0; ++index4)
          {
            int index5 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, (float) (-(double) this.velocity.X * 0.20000000298023224), (float) (-(double) this.velocity.Y * 0.20000000298023224), 100, Scale: 3f);
            Game1.dust[index5].noLight = true;
            Game1.dust[index5].noGravity = true;
            Game1.dust[index5].velocity *= 2f;
            int index6 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, (float) (-(double) this.velocity.X * 0.20000000298023224), (float) (-(double) this.velocity.Y * 0.20000000298023224), 100, Scale: 2f);
            Game1.dust[index6].noLight = true;
            Game1.dust[index6].velocity *= 2f;
          }
        }
        else
        {
          for (int index7 = 0; index7 < 20; ++index7)
          {
            int index8 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, (float) (-(double) this.velocity.X * 0.20000000298023224), (float) (-(double) this.velocity.Y * 0.20000000298023224), 100, Scale: 3f);
            Game1.dust[index8].noLight = true;
            Game1.dust[index8].noGravity = true;
            Game1.dust[index8].velocity *= 2f;
            int index9 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, (float) (-(double) this.velocity.X * 0.20000000298023224), (float) (-(double) this.velocity.Y * 0.20000000298023224), 100, Scale: 2f);
            Game1.dust[index9].noLight = true;
            Game1.dust[index9].velocity *= 2f;
          }
        }
      }
      else if (this.type == 35 || this.type == 36)
      {
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 100.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 26, (float) hitDirection, -1f);
        }
        else
        {
          for (int index = 0; index < 150; ++index)
            Dust.NewDust(this.position, this.width, this.height, 26, 2.5f * (float) hitDirection, -2.5f);
          if (this.type == 35)
          {
            Gore.NewGore(this.position, this.velocity, 54);
            Gore.NewGore(this.position, this.velocity, 55);
          }
          else
          {
            Gore.NewGore(this.position, this.velocity, 56);
            Gore.NewGore(this.position, this.velocity, 57);
            Gore.NewGore(this.position, this.velocity, 57);
            Gore.NewGore(this.position, this.velocity, 57);
          }
        }
      }
      else if (this.type == 23)
      {
        if (this.life > 0)
        {
          for (int index10 = 0; (double) index10 < dmg / (double) this.lifeMax * 100.0; ++index10)
          {
            int Type = 25;
            if (Game1.rand.Next(2) == 0)
              Type = 6;
            Dust.NewDust(this.position, this.width, this.height, Type, (float) hitDirection, -1f);
            int index11 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, this.velocity.X * 0.2f, this.velocity.Y * 0.2f, 100, Scale: 2f);
            Game1.dust[index11].noGravity = true;
          }
        }
        else
        {
          for (int index = 0; index < 50; ++index)
          {
            int Type = 25;
            if (Game1.rand.Next(2) == 0)
              Type = 6;
            Dust.NewDust(this.position, this.width, this.height, Type, (float) (2 * hitDirection), -2f);
          }
          for (int index12 = 0; index12 < 50; ++index12)
          {
            int index13 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, this.velocity.X * 0.2f, this.velocity.Y * 0.2f, 100, Scale: 2.5f);
            Game1.dust[index13].velocity *= 6f;
            Game1.dust[index13].noGravity = true;
          }
        }
      }
      else if (this.type == 24)
      {
        if (this.life > 0)
        {
          for (int index14 = 0; (double) index14 < dmg / (double) this.lifeMax * 100.0; ++index14)
          {
            int index15 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, this.velocity.X, this.velocity.Y, 100, Scale: 2.5f);
            Game1.dust[index15].noGravity = true;
          }
        }
        else
        {
          for (int index16 = 0; index16 < 50; ++index16)
          {
            int index17 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, this.velocity.X, this.velocity.Y, 100, Scale: 2.5f);
            Game1.dust[index17].noGravity = true;
            Game1.dust[index17].velocity *= 2f;
          }
          Gore.NewGore(this.position, this.velocity, 45);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 20f), this.velocity, 46);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 20f), this.velocity, 46);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 34f), this.velocity, 47);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 34f), this.velocity, 47);
        }
      }
      else if (this.type == 25)
      {
        Game1.PlaySound(2, (int) this.position.X, (int) this.position.Y, 10);
        for (int index18 = 0; index18 < 20; ++index18)
        {
          int index19 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, (float) (-(double) this.velocity.X * 0.20000000298023224), (float) (-(double) this.velocity.Y * 0.20000000298023224), 100, Scale: 2f);
          Game1.dust[index19].noGravity = true;
          Game1.dust[index19].velocity *= 2f;
          int index20 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 6, (float) (-(double) this.velocity.X * 0.20000000298023224), (float) (-(double) this.velocity.Y * 0.20000000298023224), 100);
          Game1.dust[index20].velocity *= 2f;
        }
      }
      else if (this.type == 33)
      {
        Game1.PlaySound(2, (int) this.position.X, (int) this.position.Y, 10);
        for (int index21 = 0; index21 < 20; ++index21)
        {
          int index22 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 29, (float) (-(double) this.velocity.X * 0.20000000298023224), (float) (-(double) this.velocity.Y * 0.20000000298023224), 100, Scale: 2f);
          Game1.dust[index22].noGravity = true;
          Game1.dust[index22].velocity *= 2f;
          int index23 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 29, (float) (-(double) this.velocity.X * 0.20000000298023224), (float) (-(double) this.velocity.Y * 0.20000000298023224), 100);
          Game1.dust[index23].velocity *= 2f;
        }
      }
      else if (this.type == 26 || this.type == 27 || this.type == 28 || this.type == 29)
      {
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 100.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, (float) hitDirection, -1f);
        }
        else
        {
          for (int index = 0; index < 50; ++index)
            Dust.NewDust(this.position, this.width, this.height, 5, 2.5f * (float) hitDirection, -2.5f);
          Gore.NewGore(this.position, this.velocity, 48);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 20f), this.velocity, 49);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 20f), this.velocity, 49);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 34f), this.velocity, 50);
          Gore.NewGore(new Vector2(this.position.X, this.position.Y + 34f), this.velocity, 50);
        }
      }
      else if (this.type == 30)
      {
        Game1.PlaySound(2, (int) this.position.X, (int) this.position.Y, 10);
        for (int index24 = 0; index24 < 20; ++index24)
        {
          int index25 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 27, (float) (-(double) this.velocity.X * 0.20000000298023224), (float) (-(double) this.velocity.Y * 0.20000000298023224), 100, Scale: 2f);
          Game1.dust[index25].noGravity = true;
          Game1.dust[index25].velocity *= 2f;
          int index26 = Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 27, (float) (-(double) this.velocity.X * 0.20000000298023224), (float) (-(double) this.velocity.Y * 0.20000000298023224), 100);
          Game1.dust[index26].velocity *= 2f;
        }
      }
      else if (this.type == 42)
      {
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 100.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 18, (float) hitDirection, -1f, this.alpha, this.color, this.scale);
        }
        else
        {
          for (int index = 0; index < 50; ++index)
            Dust.NewDust(this.position, this.width, this.height, 18, (float) hitDirection, -2f, this.alpha, this.color, this.scale);
          Gore.NewGore(this.position, this.velocity, 70);
          Gore.NewGore(this.position, this.velocity, 71);
        }
      }
      else
      {
        if (this.type != 43)
          return;
        if (this.life > 0)
        {
          for (int index = 0; (double) index < dmg / (double) this.lifeMax * 100.0; ++index)
            Dust.NewDust(this.position, this.width, this.height, 40, (float) hitDirection, -1f, this.alpha, this.color, 1.2f);
        }
        else
        {
          for (int index = 0; index < 50; ++index)
            Dust.NewDust(this.position, this.width, this.height, 40, (float) hitDirection, -2f, this.alpha, this.color, 1.2f);
          Gore.NewGore(this.position, this.velocity, 72);
          Gore.NewGore(this.position, this.velocity, 72);
        }
      }
    }

    public void UpdateNPC(int i)
    {
      this.whoAmI = i;
      if (!this.active)
        return;
      float num1 = 10f;
      float num2 = 0.3f;
      if (this.wet)
      {
        num2 = 0.2f;
        num1 = 7f;
      }
      if (this.soundDelay > 0)
        --this.soundDelay;
      if (this.life <= 0)
        this.active = false;
      this.oldTarget = this.target;
      this.oldDirection = this.direction;
      this.oldDirectionY = this.directionY;
      this.AI();
      for (int index = 0; index < 9; ++index)
      {
        if (this.immune[index] > 0)
          --this.immune[index];
      }
      if (!this.noGravity)
      {
        this.velocity.Y += num2;
        if ((double) this.velocity.Y > (double) num1)
          this.velocity.Y = num1;
      }
      if ((double) this.velocity.X < 0.005 && (double) this.velocity.X > -0.005)
        this.velocity.X = 0.0f;
      if (Game1.netMode != 1 && this.friendly && this.type != 22 && this.type != 37)
      {
        if (this.life < this.lifeMax)
        {
          ++this.friendlyRegen;
          if (this.friendlyRegen > 300)
          {
            this.friendlyRegen = 0;
            ++this.life;
            this.netUpdate = true;
          }
        }
        if (this.immune[8] == 0)
        {
          Rectangle rectangle1 = new Rectangle((int) this.position.X, (int) this.position.Y, this.width, this.height);
          for (int index = 0; index < 1000; ++index)
          {
            if (Game1.npc[index].active && !Game1.npc[index].friendly)
            {
              Rectangle rectangle2 = new Rectangle((int) Game1.npc[index].position.X, (int) Game1.npc[index].position.Y, Game1.npc[index].width, Game1.npc[index].height);
              if (rectangle1.Intersects(rectangle2))
              {
                int damage = Game1.npc[index].damage;
                int num3 = 6;
                int num4 = 1;
                if ((double) Game1.npc[index].position.X + (double) (Game1.npc[index].width / 2) > (double) this.position.X + (double) (this.width / 2))
                  num4 = -1;
                Game1.npc[i].StrikeNPC(damage, (float) num3, num4);
                if (Game1.netMode != 0)
                  NetMessage.SendData(28, number: i, number2: (float) damage, number3: (float) num3, number4: (float) num4);
                this.netUpdate = true;
                this.immune[8] = 30;
              }
            }
          }
        }
      }
      if (!this.noTileCollide)
      {
        bool flag1 = Collision.LavaCollision(this.position, this.width, this.height);
        if (flag1)
        {
          this.lavaWet = true;
          if (Game1.netMode != 1 && this.immune[8] == 0)
          {
            this.immune[8] = 30;
            this.StrikeNPC(50, 0.0f, 0);
            if (Game1.netMode == 2 && Game1.netMode != 0)
              NetMessage.SendData(28, number: this.whoAmI, number2: 50f);
          }
        }
        if (Collision.WetCollision(this.position, this.width, this.height))
        {
          if (!this.wet && this.wetCount == (byte) 0)
          {
            this.wetCount = (byte) 10;
            if (!flag1)
            {
              for (int index1 = 0; index1 < 50; ++index1)
              {
                int index2 = Dust.NewDust(new Vector2(this.position.X - 6f, (float) ((double) this.position.Y + (double) (this.height / 2) - 8.0)), this.width + 12, 24, 33);
                Game1.dust[index2].velocity.Y -= 4f;
                Game1.dust[index2].velocity.X *= 2.5f;
                Game1.dust[index2].scale = 1.3f;
                Game1.dust[index2].alpha = 100;
                Game1.dust[index2].noGravity = true;
              }
              Game1.PlaySound(19, (int) this.position.X, (int) this.position.Y, 0);
            }
            else
            {
              for (int index3 = 0; index3 < 20; ++index3)
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
        else if (this.wet)
        {
          this.velocity.X *= 0.5f;
          this.wet = false;
          if (this.wetCount == (byte) 0)
          {
            this.wetCount = (byte) 10;
            if (!this.lavaWet)
            {
              for (int index5 = 0; index5 < 50; ++index5)
              {
                int index6 = Dust.NewDust(new Vector2(this.position.X - 6f, this.position.Y + (float) (this.height / 2)), this.width + 12, 24, 33);
                Game1.dust[index6].velocity.Y -= 4f;
                Game1.dust[index6].velocity.X *= 2.5f;
                Game1.dust[index6].scale = 1.3f;
                Game1.dust[index6].alpha = 100;
                Game1.dust[index6].noGravity = true;
              }
              Game1.PlaySound(19, (int) this.position.X, (int) this.position.Y, 0);
            }
            else
            {
              for (int index7 = 0; index7 < 20; ++index7)
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
        bool flag2 = false;
        if (this.aiStyle == 10)
          flag2 = true;
        if (this.aiStyle == 3 && this.directionY == 1)
          flag2 = true;
        this.oldVelocity = this.velocity;
        this.collideX = false;
        this.collideY = false;
        if (this.wet)
        {
          Vector2 velocity = this.velocity;
          this.velocity = Collision.TileCollision(this.position, this.velocity, this.width, this.height, flag2, flag2);
          Vector2 vector2 = this.velocity * 0.5f;
          if ((double) this.velocity.X != (double) velocity.X)
          {
            vector2.X = this.velocity.X;
            this.collideX = true;
          }
          if ((double) this.velocity.Y != (double) velocity.Y)
          {
            vector2.Y = this.velocity.Y;
            this.collideY = true;
          }
          this.oldPosition = this.position;
          this.position += vector2;
        }
        else
        {
          this.velocity = Collision.TileCollision(this.position, this.velocity, this.width, this.height, flag2, flag2);
          if ((double) this.oldVelocity.X != (double) this.velocity.X)
            this.collideX = true;
          if ((double) this.oldVelocity.Y != (double) this.velocity.Y)
            this.collideY = true;
          this.oldPosition = this.position;
          this.position += this.velocity;
        }
      }
      else
      {
        this.oldPosition = this.position;
        this.position += this.velocity;
      }
      if (!this.active)
        this.netUpdate = true;
      if (Game1.netMode == 2 && this.netUpdate)
        NetMessage.SendData(23, number: i);
      this.FindFrame();
      this.CheckActive();
      this.netUpdate = false;
    }

    public Color GetAlpha(Color newColor)
    {
      int r = (int) newColor.R - this.alpha;
      int g = (int) newColor.G - this.alpha;
      int b = (int) newColor.B - this.alpha;
      int a = (int) newColor.A - this.alpha;
      if (this.type == 25 || this.type == 30 || this.type == 33)
      {
        r = (int) newColor.R;
        g = (int) newColor.G;
        b = (int) newColor.B;
      }
      if (a < 0)
        a = 0;
      if (a > (int) byte.MaxValue)
        a = (int) byte.MaxValue;
      return new Color(r, g, b, a);
    }

    public Color GetColor(Color newColor)
    {
      int r = (int) this.color.R - ((int) byte.MaxValue - (int) newColor.R);
      int g = (int) this.color.G - ((int) byte.MaxValue - (int) newColor.G);
      int b = (int) this.color.B - ((int) byte.MaxValue - (int) newColor.B);
      int a = (int) this.color.A - ((int) byte.MaxValue - (int) newColor.A);
      if (r < 0)
        r = 0;
      if (r > (int) byte.MaxValue)
        r = (int) byte.MaxValue;
      if (g < 0)
        g = 0;
      if (g > (int) byte.MaxValue)
        g = (int) byte.MaxValue;
      if (b < 0)
        b = 0;
      if (b > (int) byte.MaxValue)
        b = (int) byte.MaxValue;
      if (a < 0)
        a = 0;
      if (a > (int) byte.MaxValue)
        a = (int) byte.MaxValue;
      return new Color(r, g, b, a);
    }

    public string GetChat()
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      bool flag6 = false;
      for (int index = 0; index < 1000; ++index)
      {
        if (Game1.npc[index].type == 17)
          flag1 = true;
        else if (Game1.npc[index].type == 18)
          flag2 = true;
        else if (Game1.npc[index].type == 19)
          flag3 = true;
        else if (Game1.npc[index].type == 20)
          flag4 = true;
        else if (Game1.npc[index].type == 37)
          flag5 = true;
        else if (Game1.npc[index].type == 38)
          flag6 = true;
      }
      string chat = "";
      string str;
      if (this.type == 17)
      {
        if (Game1.dayTime)
        {
          if (Game1.time < 16200.0)
            chat = Game1.rand.Next(2) != 0 ? "Lovely morning, wouldn't you say? Was there something you needed?" : "Sword beats paper, get one today.";
          else if (Game1.time > 37800.0)
          {
            chat = Game1.rand.Next(2) != 0 ? "Ah, they will tell tales of " + Game1.player[Game1.myPlayer].name + " some day... good ones I'm sure." : "Night be upon us soon, friend. Make your choices while you can.";
          }
          else
          {
            int num = Game1.rand.Next(3);
            if (num == 0)
              str = "Check out my dirt blocks, they are extra dirty.";
            chat = num != 1 ? "The sun is high, but my prices are not." : "Boy, that sun is hot! I do have some perfectly ventilated armor.";
          }
        }
        else if (Game1.bloodMoon)
          chat = Game1.rand.Next(2) != 0 ? "Keep your eye on the prize, buy a lense!" : "Have you seen Chith...Shith.. Chat... The big eye?";
        else if (Game1.time < 9720.0)
          chat = Game1.rand.Next(2) != 0 ? Game1.player[Game1.myPlayer].name + " is it? I've heard good things, friend!" : "Kosh, kapleck Mog. Oh sorry, thats klingon for 'Buy something or die.'";
        else if (Game1.time > 22680.0)
        {
          chat = Game1.rand.Next(2) != 0 ? "Angel Statue you say? I'm sorry, I'm not a junk dealer." : "I hear there's a secret treasure... oh never mind.";
        }
        else
        {
          int num = Game1.rand.Next(3);
          if (num == 0)
            str = "The last guy who was here left me some junk..er I mean.. treasures!";
          chat = num != 1 ? "Did you say gold?  I'll take that off of ya'." : "I wonder if the moon is made of cheese...huh, what? Oh yes, buy something!";
        }
      }
      else if (this.type == 18)
      {
        if (flag6 && Game1.rand.Next(4) == 0)
          chat = "I wish that bomb maker would be more careful.  I'm getting tired of having to sew his limbs back on every day.";
        else if ((double) Game1.player[Game1.myPlayer].statLife < (double) Game1.player[Game1.myPlayer].statLifeMax * 0.33)
        {
          switch (Game1.rand.Next(3))
          {
            case 0:
              chat = "I think you look better this way.";
              break;
            case 1:
              chat = "Eww.. What happened to your face?";
              break;
            default:
              chat = "You left your arm over there. Let me get that for you..";
              break;
          }
        }
        else if ((double) Game1.player[Game1.myPlayer].statLife < (double) Game1.player[Game1.myPlayer].statLifeMax * 0.66)
        {
          switch (Game1.rand.Next(3))
          {
            case 0:
              chat = "Quit being such a baby! I've seen worse.";
              break;
            case 1:
              chat = "That's gonna need stitches!";
              break;
            default:
              chat = "You look half digested. Have you been chasing slimes again?";
              break;
          }
        }
        else
        {
          switch (Game1.rand.Next(4))
          {
            case 0:
              chat = "Turn your head and cough.";
              break;
            case 1:
              chat = "Thats not the biggest I've ever seen... Yes, I've seen bigger wounds for sure.";
              break;
            case 2:
              chat = "Bend over and grab your ankles.";
              break;
            default:
              chat = "Show me where it hurts.";
              break;
          }
        }
      }
      else if (this.type == 19)
      {
        if (flag2 && Game1.rand.Next(4) == 0)
          chat = "Make it quick! I've got a date with the nurse in an hour.";
        else if (flag4 && Game1.rand.Next(4) == 0)
          chat = "That dryad is a looker.  Too bad she's such a prude.";
        else if (flag6 && Game1.rand.Next(4) == 0)
          chat = "Don't bother with that firework vendor, I've got all you need right here.";
        else if (Game1.bloodMoon)
        {
          chat = "I love nights like tonight.  There is never a shortage of things to kill!";
        }
        else
        {
          switch (Game1.rand.Next(2))
          {
            case 0:
              chat = "I see you're eyeballin' the Minishark.. You really don't want to know how it was made.";
              break;
            case 1:
              chat = "Keep your hands off my gun, buddy!";
              break;
          }
        }
      }
      else if (this.type == 20)
      {
        if (flag3 && Game1.rand.Next(4) == 0)
          chat = "I wish that gun seller would stop talking to me. Doesn't he realize I'm 500 years old?";
        else if (flag1 && Game1.rand.Next(4) == 0)
          chat = "That merchant keeps trying to sell me an angel statue. Everyone knows that they don't do anything.";
        else if (flag5 && Game1.rand.Next(4) == 0)
          chat = "Have you seen that old man walk around the temp? He doesn't look well at all...";
        else if (Game1.bloodMoon)
        {
          chat = "It is an evil moon tonight. Be careful.";
        }
        else
        {
          switch (Game1.rand.Next(2))
          {
            case 0:
              chat = "You must cleanse the world of this corruption.";
              break;
            case 1:
              chat = "Be safe; Terraria needs you!";
              break;
          }
        }
      }
      else if (this.type == 22)
      {
        if (Game1.bloodMoon)
          chat = "You can tell a Blood Moon is out when the sky turns red. There is something about it that causes monsters to swarm.";
        else if (!Game1.dayTime)
        {
          chat = "You should stay indoors at night. It is very dangerous to be wondering around in the dark.";
        }
        else
        {
          switch (Game1.rand.Next(2))
          {
            case 0:
              chat = "Greetings, " + Game1.player[Game1.myPlayer].name + ". Is there something I can help you with?";
              break;
            case 1:
              chat = "I am here to give you advice on what to do next.  It is recommended that you talk with me anytime you get stuck.";
              break;
          }
        }
      }
      else if (this.type == 37)
      {
        if (Game1.dayTime)
        {
          switch (Game1.rand.Next(2))
          {
            case 0:
              chat = "I cannot let you enter until you free me of my curse.";
              break;
            case 1:
              chat = "Come back at night if you wish to enter.";
              break;
          }
        }
      }
      else if (this.type == 38)
      {
        if (Game1.bloodMoon)
          chat = "I've got something for them zombies alright!";
        else if (flag3 && Game1.rand.Next(4) == 0)
          chat = "Even the gun dealer wants what I'm selling!";
        else if (flag2 && Game1.rand.Next(4) == 0)
          chat = "I'm sure the nurse will help if you accidentally lose a limb to these.";
        else if (flag4 && Game1.rand.Next(4) == 0)
        {
          chat = "Why purify the world when you can just blow it up?";
        }
        else
        {
          switch (Game1.rand.Next(2))
          {
            case 0:
              chat = "Explosives are da' bomb these days.  Buy some now!";
              break;
            case 1:
              chat = "Check out my goods; they have explosive prices!";
              break;
          }
        }
      }
      return chat;
    }
  }
}
