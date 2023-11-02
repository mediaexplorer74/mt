// Decompiled with JetBrains decompiler
// Type: GameManager.Item
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

using Microsoft.Xna.Framework;
using System;

namespace GameManager
{
  public class Item
  {
    public static int potionDelay = 600;
    public bool wet;
    public byte wetCount = 0;
    public bool lavaWet;
    public Vector2 position;
    public Vector2 velocity;
    public int width;
    public int height;
    public bool active;
    public int noGrabDelay;
    public bool beingGrabbed;
    public int spawnTime;
    public bool wornArmor = false;
    public int ownIgnore = -1;
    public int ownTime = 0;
    public int keepTime = 0;
    public int type;
    public string name;
    public int holdStyle;
    public int useStyle;
    public bool channel = false;
    public bool accessory = false;
    public int useAnimation;
    public int useTime;
    public int stack;
    public int maxStack;
    public int pick;
    public int axe;
    public int hammer;
    public int tileBoost;
    public int createTile = -1;
    public int createWall = -1;
    public int damage;
    public float knockBack;
    public int healLife;
    public int healMana;
    public bool potion = false;
    public bool consumable;
    public bool autoReuse;
    public bool useTurn;
    public Color color;
    public int alpha;
    public float scale = 1f;
    public int useSound = 0;
    public int defense;
    public int headSlot = -1;
    public int bodySlot = -1;
    public int legSlot = -1;
    public string toolTip;
    public int owner = 8;
    public int rare = 0;
    public int shoot = 0;
    public float shootSpeed = 0.0f;
    public int ammo = 0;
    public int useAmmo = 0;
    public int lifeRegen = 0;
    public int manaRegen = 0;
    public int mana = 0;
    public bool noUseGraphic = false;
    public bool noMelee = false;
    public int release = 0;
    public int value = 0;
    public bool buy = false;

    public void SetDefaults(string ItemName)
    {
      this.name = "";
      switch (ItemName)
      {
        case "Gold Pickaxe":
          this.SetDefaults(1);
          this.color = new Color(210, 190, 0, 100);
          this.useTime = 17;
          this.pick = 55;
          this.useAnimation = 20;
          this.scale = 1.05f;
          this.damage = 6;
          this.rare = 2;
          this.value = 10000;
          break;
        case "Gold Broadsword":
          this.SetDefaults(4);
          this.color = new Color(210, 190, 0, 100);
          this.useAnimation = 20;
          this.damage = 13;
          this.scale = 1.05f;
          this.rare = 2;
          this.value = 9000;
          break;
        case "Gold Shortsword":
          this.SetDefaults(6);
          this.color = new Color(210, 190, 0, 100);
          this.damage = 11;
          this.useAnimation = 11;
          this.scale = 0.95f;
          this.rare = 2;
          this.value = 7000;
          break;
        case "Gold Axe":
          this.SetDefaults(10);
          this.color = new Color(210, 190, 0, 100);
          this.useTime = 18;
          this.axe = 11;
          this.useAnimation = 26;
          this.scale = 1.15f;
          this.damage = 7;
          this.rare = 2;
          this.value = 8000;
          break;
        case "Gold Hammer":
          this.SetDefaults(7);
          this.color = new Color(210, 190, 0, 100);
          this.useAnimation = 28;
          this.useTime = 23;
          this.scale = 1.25f;
          this.damage = 9;
          this.hammer = 55;
          this.rare = 2;
          this.value = 8000;
          break;
        case "Gold Bow":
          this.SetDefaults(99);
          this.useAnimation = 26;
          this.useTime = 26;
          this.color = new Color(210, 190, 0, 100);
          this.damage = 10;
          this.rare = 2;
          this.value = 7000;
          break;
        case "Silver Pickaxe":
          this.SetDefaults(1);
          this.color = new Color(180, 180, 180, 100);
          this.useTime = 11;
          this.pick = 45;
          this.useAnimation = 19;
          this.scale = 1.05f;
          this.damage = 6;
          this.rare = 2;
          this.value = 5000;
          break;
        case "Silver Broadsword":
          this.SetDefaults(4);
          this.color = new Color(180, 180, 180, 100);
          this.useAnimation = 21;
          this.damage = 11;
          this.rare = 2;
          this.value = 4500;
          break;
        case "Silver Shortsword":
          this.SetDefaults(6);
          this.color = new Color(180, 180, 180, 100);
          this.damage = 9;
          this.useAnimation = 12;
          this.scale = 0.95f;
          this.rare = 2;
          this.value = 3500;
          break;
        case "Silver Axe":
          this.SetDefaults(10);
          this.color = new Color(180, 180, 180, 100);
          this.useTime = 18;
          this.axe = 10;
          this.useAnimation = 26;
          this.scale = 1.15f;
          this.damage = 6;
          this.rare = 2;
          this.value = 4000;
          break;
        case "Silver Hammer":
          this.SetDefaults(7);
          this.color = new Color(180, 180, 180, 100);
          this.useAnimation = 29;
          this.useTime = 19;
          this.scale = 1.25f;
          this.damage = 9;
          this.hammer = 45;
          this.rare = 2;
          this.value = 4000;
          break;
        case "Silver Bow":
          this.SetDefaults(99);
          this.useAnimation = 27;
          this.useTime = 27;
          this.color = new Color(180, 180, 180, 100);
          this.damage = 9;
          this.rare = 2;
          this.value = 3500;
          break;
        case "Copper Pickaxe":
          this.SetDefaults(1);
          this.color = new Color(180, 100, 45, 80);
          this.useTime = 15;
          this.pick = 35;
          this.useAnimation = 23;
          this.scale = 0.9f;
          this.tileBoost = -1;
          this.damage = 2;
          this.value = 500;
          break;
        case "Copper Broadsword":
          this.SetDefaults(4);
          this.color = new Color(180, 100, 45, 80);
          this.useAnimation = 23;
          this.damage = 8;
          this.value = 450;
          break;
        case "Copper Shortsword":
          this.SetDefaults(6);
          this.color = new Color(180, 100, 45, 80);
          this.damage = 6;
          this.useAnimation = 13;
          this.scale = 0.8f;
          this.value = 350;
          break;
        case "Copper Axe":
          this.SetDefaults(10);
          this.color = new Color(180, 100, 45, 80);
          this.useTime = 21;
          this.axe = 8;
          this.useAnimation = 30;
          this.scale = 1f;
          this.damage = 3;
          this.tileBoost = -1;
          this.value = 400;
          break;
        case "Copper Hammer":
          this.SetDefaults(7);
          this.color = new Color(180, 100, 45, 80);
          this.useAnimation = 33;
          this.useTime = 23;
          this.scale = 1.1f;
          this.damage = 4;
          this.hammer = 35;
          this.tileBoost = -1;
          this.value = 400;
          break;
        case "Copper Bow":
          this.SetDefaults(99);
          this.useAnimation = 29;
          this.useTime = 29;
          this.color = new Color(180, 100, 45, 80);
          this.damage = 7;
          this.value = 350;
          break;
        default:
          if (ItemName != "")
          {
            for (int Type = 0; Type < 194; ++Type)
            {
              this.SetDefaults(Type);
              if (!(this.name == ItemName))
              {
                if (Type == 193)
                {
                  this.SetDefaults(0);
                  this.name = "";
                }
              }
              else
                break;
            }
            break;
          }
          break;
      }
      if (this.type == 0)
        return;
      this.name = ItemName;
    }

    public void SetDefaults(int Type)
    {
      this.owner = Game1.netMode != 1 && Game1.netMode != 2 ? Game1.myPlayer : 8;
      this.mana = 0;
      this.wet = false;
      this.wetCount = (byte) 0;
      this.lavaWet = false;
      this.channel = false;
      this.manaRegen = 0;
      this.release = 0;
      this.noMelee = false;
      this.noUseGraphic = false;
      this.lifeRegen = 0;
      this.shootSpeed = 0.0f;
      this.active = true;
      this.alpha = 0;
      this.ammo = 0;
      this.useAmmo = 0;
      this.autoReuse = false;
      this.accessory = false;
      this.axe = 0;
      this.healMana = 0;
      this.bodySlot = -1;
      this.legSlot = -1;
      this.headSlot = -1;
      this.potion = false;
      this.color = new Color();
      this.consumable = false;
      this.createTile = -1;
      this.createWall = -1;
      this.damage = -1;
      this.defense = 0;
      this.hammer = 0;
      this.healLife = 0;
      this.holdStyle = 0;
      this.knockBack = 0.0f;
      this.maxStack = 1;
      this.pick = 0;
      this.rare = 0;
      this.scale = 1f;
      this.shoot = 0;
      this.stack = 1;
      this.toolTip = (string) null;
      this.tileBoost = 0;
      this.type = Type;
      this.useStyle = 0;
      this.useSound = 0;
      this.useTime = 100;
      this.useAnimation = 100;
      this.value = 0;
      this.useTurn = false;
      this.buy = false;
      if (this.type == 1)
      {
        this.name = "Iron Pickaxe";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 20;
        this.useTime = 13;
        this.autoReuse = true;
        this.width = 24;
        this.height = 28;
        this.damage = 5;
        this.pick = 45;
        this.useSound = 1;
        this.knockBack = 2f;
        this.rare = 1;
        this.value = 2000;
      }
      else if (this.type == 2)
      {
        this.name = "Dirt Block";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 0;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 3)
      {
        this.name = "Stone Block";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 1;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 4)
      {
        this.name = "Iron Broadsword";
        this.useStyle = 1;
        this.useTurn = false;
        this.useAnimation = 21;
        this.useTime = 21;
        this.width = 24;
        this.height = 28;
        this.damage = 10;
        this.knockBack = 5f;
        this.useSound = 1;
        this.scale = 1f;
        this.rare = 1;
        this.value = 1800;
      }
      else if (this.type == 5)
      {
        this.name = "Mushroom";
        this.useStyle = 2;
        this.useSound = 2;
        this.useTurn = false;
        this.useAnimation = 17;
        this.useTime = 17;
        this.width = 16;
        this.height = 18;
        this.healLife = 20;
        this.maxStack = 99;
        this.consumable = true;
        this.potion = true;
        this.value = 50;
      }
      else if (this.type == 6)
      {
        this.name = "Iron Shortsword";
        this.useStyle = 3;
        this.useTurn = false;
        this.useAnimation = 12;
        this.useTime = 12;
        this.width = 24;
        this.height = 28;
        this.damage = 8;
        this.knockBack = 4f;
        this.scale = 0.9f;
        this.useSound = 1;
        this.useTurn = true;
        this.rare = 1;
        this.value = 1400;
      }
      else if (this.type == 7)
      {
        this.name = "Iron Hammer";
        this.autoReuse = true;
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 30;
        this.useTime = 20;
        this.hammer = 45;
        this.width = 24;
        this.height = 28;
        this.damage = 7;
        this.knockBack = 5.5f;
        this.scale = 1.2f;
        this.useSound = 1;
        this.rare = 1;
        this.value = 1600;
      }
      else if (this.type == 8)
      {
        this.name = "Torch";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.holdStyle = 1;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 4;
        this.width = 10;
        this.height = 12;
        this.toolTip = "Provides light";
        this.value = 50;
      }
      else if (this.type == 9)
      {
        this.name = "Wood";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 30;
        this.width = 8;
        this.height = 10;
      }
      else if (this.type == 10)
      {
        this.name = "Iron Axe";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 27;
        this.knockBack = 4.5f;
        this.useTime = 19;
        this.autoReuse = true;
        this.width = 24;
        this.height = 28;
        this.damage = 5;
        this.axe = 9;
        this.scale = 1.1f;
        this.useSound = 1;
        this.rare = 1;
        this.value = 1600;
      }
      else if (this.type == 11)
      {
        this.name = "Iron Ore";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 6;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 12)
      {
        this.name = "Copper Ore";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 7;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 13)
      {
        this.name = "Gold Ore";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 8;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 14)
      {
        this.name = "Silver Ore";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 9;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 15)
      {
        this.name = "Copper Watch";
        this.width = 24;
        this.height = 28;
        this.accessory = true;
        this.rare = 1;
        this.toolTip = "Tells the time";
        this.value = 1000;
      }
      else if (this.type == 16)
      {
        this.name = "Silver Watch";
        this.width = 24;
        this.height = 28;
        this.accessory = true;
        this.rare = 2;
        this.toolTip = "Tells the time";
        this.value = 5000;
      }
      else if (this.type == 17)
      {
        this.name = "Gold Watch";
        this.width = 24;
        this.height = 28;
        this.accessory = true;
        this.rare = 3;
        this.toolTip = "Tells the time";
        this.value = 10000;
      }
      else if (this.type == 18)
      {
        this.name = "Depth Meter";
        this.width = 24;
        this.height = 18;
        this.accessory = true;
        this.rare = 3;
        this.toolTip = "Shows depth";
        this.value = 10000;
      }
      else if (this.type == 19)
      {
        this.name = "Gold Bar";
        this.width = 20;
        this.height = 20;
        this.maxStack = 99;
        this.rare = 3;
        this.value = 10000;
      }
      else if (this.type == 20)
      {
        this.name = "Copper Bar";
        this.width = 20;
        this.height = 20;
        this.maxStack = 99;
        this.value = 1000;
      }
      else if (this.type == 21)
      {
        this.name = "Silver Bar";
        this.width = 20;
        this.height = 20;
        this.maxStack = 99;
        this.rare = 2;
        this.value = 5000;
      }
      else if (this.type == 22)
      {
        this.name = "Iron Bar";
        this.width = 20;
        this.height = 20;
        this.maxStack = 99;
        this.rare = 1;
        this.value = 2500;
      }
      else if (this.type == 23)
      {
        this.name = "Gel";
        this.width = 10;
        this.height = 12;
        this.maxStack = 99;
        this.alpha = 175;
        this.color = new Color(0, 80, (int) byte.MaxValue, 100);
        this.toolTip = "'Both tasty and flammable'";
        this.value = 5;
      }
      else if (this.type == 24)
      {
        this.name = "Wooden Sword";
        this.useStyle = 1;
        this.useTurn = false;
        this.useAnimation = 25;
        this.width = 24;
        this.height = 28;
        this.damage = 7;
        this.knockBack = 4f;
        this.scale = 0.95f;
        this.useSound = 1;
        this.value = 100;
      }
      else if (this.type == 25)
      {
        this.name = "Wooden Door";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 10;
        this.width = 14;
        this.height = 28;
        this.value = 200;
      }
      else if (this.type == 26)
      {
        this.name = "Stone Wall";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createWall = 1;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 27)
      {
        this.name = "Acorn";
        this.useTurn = true;
        this.useStyle = 1;
        this.useAnimation = 15;
        this.useTime = 10;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 20;
        this.width = 18;
        this.height = 18;
        this.value = 10;
      }
      else if (this.type == 28)
      {
        this.name = "Lesser Healing Potion";
        this.useSound = 3;
        this.healLife = 100;
        this.useStyle = 2;
        this.useTurn = true;
        this.useAnimation = 17;
        this.useTime = 17;
        this.maxStack = 99;
        this.consumable = true;
        this.width = 14;
        this.height = 24;
        this.rare = 1;
        this.potion = true;
        this.value = 200;
      }
      else if (this.type == 29)
      {
        this.name = "Life Crystal";
        this.maxStack = 99;
        this.consumable = true;
        this.width = 18;
        this.height = 18;
        this.useStyle = 4;
        this.useTime = 30;
        this.useSound = 4;
        this.useAnimation = 30;
        this.toolTip = "Increases maximum life";
        this.rare = 4;
      }
      else if (this.type == 30)
      {
        this.name = "Dirt Wall";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createWall = 2;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 31)
      {
        this.name = "Bottle";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 13;
        this.width = 16;
        this.height = 24;
        this.value = 100;
      }
      else if (this.type == 32)
      {
        this.name = "Wooden Table";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 14;
        this.width = 26;
        this.height = 20;
        this.value = 300;
      }
      else if (this.type == 33)
      {
        this.name = "Furnace";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 17;
        this.width = 26;
        this.height = 24;
        this.value = 300;
      }
      else if (this.type == 34)
      {
        this.name = "Wooden Chair";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 15;
        this.width = 12;
        this.height = 30;
        this.value = 150;
      }
      else if (this.type == 35)
      {
        this.name = "Iron Anvil";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 16;
        this.width = 28;
        this.height = 14;
        this.value = 5000;
      }
      else if (this.type == 36)
      {
        this.name = "Work Bench";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 18;
        this.width = 28;
        this.height = 14;
        this.value = 150;
      }
      else if (this.type == 37)
      {
        this.name = "Goggles";
        this.width = 28;
        this.height = 12;
        this.defense = 2;
        this.headSlot = 0;
        this.rare = 2;
        this.value = 1000;
      }
      else if (this.type == 38)
      {
        this.name = "Lens";
        this.width = 12;
        this.height = 20;
        this.maxStack = 99;
        this.value = 500;
      }
      else if (this.type == 39)
      {
        this.useStyle = 5;
        this.useAnimation = 30;
        this.useTime = 30;
        this.name = "Wooden Bow";
        this.width = 12;
        this.height = 28;
        this.shoot = 1;
        this.useAmmo = 1;
        this.useSound = 5;
        this.damage = 6;
        this.shootSpeed = 6.5f;
        this.noMelee = true;
        this.value = 100;
      }
      else if (this.type == 40)
      {
        this.name = "Wooden Arrow";
        this.shootSpeed = 3f;
        this.shoot = 1;
        this.damage = 4;
        this.width = 10;
        this.height = 28;
        this.maxStack = 250;
        this.consumable = true;
        this.ammo = 1;
        this.knockBack = 2f;
        this.value = 10;
      }
      else if (this.type == 41)
      {
        this.name = "Flaming Arrow";
        this.shootSpeed = 3.5f;
        this.shoot = 2;
        this.damage = 6;
        this.width = 10;
        this.height = 28;
        this.maxStack = 250;
        this.consumable = true;
        this.ammo = 1;
        this.knockBack = 2f;
        this.value = 15;
      }
      else if (this.type == 42)
      {
        this.useStyle = 1;
        this.name = "Shuriken";
        this.shootSpeed = 9f;
        this.shoot = 3;
        this.damage = 12;
        this.width = 18;
        this.height = 20;
        this.maxStack = 250;
        this.consumable = true;
        this.useSound = 1;
        this.useAnimation = 15;
        this.useTime = 15;
        this.noUseGraphic = true;
        this.noMelee = true;
        this.value = 20;
      }
      else if (this.type == 43)
      {
        this.useStyle = 4;
        this.name = "Suspicious Looking Eye";
        this.width = 22;
        this.height = 14;
        this.consumable = true;
        this.useAnimation = 45;
        this.useTime = 45;
        this.toolTip = "May cause terrible things to occur";
      }
      else if (this.type == 44)
      {
        this.useStyle = 5;
        this.useAnimation = 25;
        this.useTime = 25;
        this.name = "Demon Bow";
        this.width = 12;
        this.height = 28;
        this.shoot = 1;
        this.useAmmo = 1;
        this.useSound = 5;
        this.damage = 12;
        this.shootSpeed = 6.7f;
        this.knockBack = 1f;
        this.alpha = 30;
        this.rare = 3;
        this.noMelee = true;
        this.value = 18000;
      }
      else if (this.type == 45)
      {
        this.name = "War Axe of the Night";
        this.autoReuse = true;
        this.useStyle = 1;
        this.useAnimation = 30;
        this.knockBack = 6f;
        this.useTime = 15;
        this.width = 24;
        this.height = 28;
        this.damage = 21;
        this.axe = 15;
        this.scale = 1.2f;
        this.useSound = 1;
        this.rare = 3;
        this.value = 13500;
      }
      else if (this.type == 46)
      {
        this.name = "Light's Bane";
        this.useStyle = 1;
        this.useAnimation = 20;
        this.knockBack = 5f;
        this.width = 24;
        this.height = 28;
        this.damage = 16;
        this.scale = 1.1f;
        this.useSound = 1;
        this.rare = 3;
        this.value = 13500;
      }
      else if (this.type == 47)
      {
        this.name = "Unholy Arrow";
        this.shootSpeed = 3.4f;
        this.shoot = 4;
        this.damage = 7;
        this.width = 10;
        this.height = 28;
        this.maxStack = 250;
        this.consumable = true;
        this.ammo = 1;
        this.knockBack = 3f;
        this.alpha = 30;
        this.rare = 3;
        this.value = 40;
      }
      else if (this.type == 48)
      {
        this.name = "Chest";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 21;
        this.width = 26;
        this.height = 22;
        this.value = 500;
      }
      else if (this.type == 49)
      {
        this.name = "Band of Regeneration";
        this.width = 22;
        this.height = 22;
        this.accessory = true;
        this.lifeRegen = 1;
        this.rare = 3;
        this.toolTip = "Slowly regenerates life";
        this.value = 50000;
      }
      else if (this.type == 50)
      {
        this.name = "Magic Mirror";
        this.useTurn = true;
        this.width = 20;
        this.height = 20;
        this.useStyle = 4;
        this.useTime = 90;
        this.useSound = 6;
        this.useAnimation = 90;
        this.toolTip = "Gaze in the mirror to return home";
        this.rare = 4;
        this.value = 50000;
      }
      else if (this.type == 51)
      {
        this.name = "Jester's Arrow";
        this.shootSpeed = 0.5f;
        this.shoot = 5;
        this.damage = 8;
        this.width = 10;
        this.height = 28;
        this.maxStack = 250;
        this.consumable = true;
        this.ammo = 1;
        this.knockBack = 4f;
        this.rare = 3;
        this.value = 100;
      }
      else if (this.type == 52)
      {
        this.name = "Angel Statue";
        this.width = 24;
        this.height = 28;
        this.toolTip = "It doesn't do anything";
        this.value = 1;
      }
      else if (this.type == 53)
      {
        this.name = "Cloud in a Bottle";
        this.width = 16;
        this.height = 24;
        this.accessory = true;
        this.rare = 3;
        this.toolTip = "Allows the holder to double jump";
        this.value = 50000;
      }
      else if (this.type == 54)
      {
        this.name = "Hermes Boots";
        this.width = 28;
        this.height = 24;
        this.accessory = true;
        this.rare = 3;
        this.toolTip = "The wearer can run super fast";
        this.value = 50000;
      }
      else if (this.type == 55)
      {
        this.noMelee = true;
        this.useStyle = 1;
        this.name = "Enchanted Boomerang";
        this.shootSpeed = 10f;
        this.shoot = 6;
        this.damage = 13;
        this.knockBack = 8f;
        this.width = 14;
        this.height = 28;
        this.useSound = 1;
        this.useAnimation = 15;
        this.useTime = 15;
        this.noUseGraphic = true;
        this.rare = 3;
        this.value = 50000;
      }
      else if (this.type == 56)
      {
        this.name = "Demonite Ore";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 22;
        this.width = 12;
        this.height = 12;
        this.rare = 2;
        this.toolTip = "Pulsing with dark energy";
      }
      else if (this.type == 57)
      {
        this.name = "Demonite Bar";
        this.width = 20;
        this.height = 20;
        this.maxStack = 99;
        this.rare = 3;
        this.toolTip = "Pulsing with dark energy";
        this.value = 15000;
      }
      else if (this.type == 58)
      {
        this.name = "Heart";
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 59)
      {
        this.name = "Corrupt Seeds";
        this.useTurn = true;
        this.useStyle = 1;
        this.useAnimation = 15;
        this.useTime = 10;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 23;
        this.width = 14;
        this.height = 14;
        this.value = 500;
      }
      else if (this.type == 60)
      {
        this.name = "Vile Mushroom";
        this.width = 16;
        this.height = 18;
        this.maxStack = 99;
        this.value = 50;
      }
      else if (this.type == 61)
      {
        this.name = "Ebonstone Block";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 25;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 62)
      {
        this.name = "Grass Seeds";
        this.useTurn = true;
        this.useStyle = 1;
        this.useAnimation = 15;
        this.useTime = 10;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 2;
        this.width = 14;
        this.height = 14;
        this.value = 20;
      }
      else if (this.type == 63)
      {
        this.name = "Sunflower";
        this.useTurn = true;
        this.useStyle = 1;
        this.useAnimation = 15;
        this.useTime = 10;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 27;
        this.width = 26;
        this.height = 26;
        this.value = 200;
      }
      else if (this.type == 64)
      {
        this.mana = 5;
        this.damage = 8;
        this.useStyle = 1;
        this.name = "Vilethorn";
        this.shootSpeed = 32f;
        this.shoot = 7;
        this.width = 26;
        this.height = 28;
        this.useSound = 8;
        this.useAnimation = 30;
        this.useTime = 30;
        this.rare = 3;
        this.noMelee = true;
        this.toolTip = "Summons a vile thorn";
        this.value = 10000;
      }
      else if (this.type == 65)
      {
        this.mana = 5;
        this.knockBack = 5f;
        this.alpha = 100;
        this.color = new Color(150, 150, 150, 0);
        this.damage = 12;
        this.useStyle = 1;
        this.scale = 1.15f;
        this.name = "Starfury";
        this.shootSpeed = 12f;
        this.shoot = 9;
        this.width = 14;
        this.height = 28;
        this.useSound = 9;
        this.useAnimation = 25;
        this.useTime = 10;
        this.rare = 3;
        this.toolTip = "Forged with the fury of heaven";
        this.value = 50000;
      }
      else if (this.type == 66)
      {
        this.useStyle = 1;
        this.name = "Purification Powder";
        this.shootSpeed = 4f;
        this.shoot = 10;
        this.width = 16;
        this.height = 24;
        this.maxStack = 99;
        this.consumable = true;
        this.useSound = 1;
        this.useAnimation = 15;
        this.useTime = 15;
        this.noMelee = true;
        this.value = 75;
      }
      else if (this.type == 67)
      {
        this.damage = 8;
        this.useStyle = 1;
        this.name = "Vile Powder";
        this.shootSpeed = 4f;
        this.shoot = 11;
        this.width = 16;
        this.height = 24;
        this.maxStack = 99;
        this.consumable = true;
        this.useSound = 1;
        this.useAnimation = 15;
        this.useTime = 15;
        this.noMelee = true;
        this.value = 100;
      }
      else if (this.type == 68)
      {
        this.name = "Rotten Chunk";
        this.width = 18;
        this.height = 20;
        this.maxStack = 99;
        this.toolTip = "Looks tasty!";
        this.value = 10;
      }
      else if (this.type == 69)
      {
        this.name = "Worm Tooth";
        this.width = 8;
        this.height = 20;
        this.maxStack = 99;
        this.value = 100;
      }
      else if (this.type == 70)
      {
        this.useStyle = 4;
        this.consumable = true;
        this.useAnimation = 45;
        this.useTime = 45;
        this.name = "Worm Food";
        this.width = 28;
        this.height = 28;
        this.toolTip = "May attract giant worms";
      }
      else if (this.type == 71)
      {
        this.name = "Copper Coin";
        this.width = 10;
        this.height = 12;
        this.maxStack = 100;
      }
      else if (this.type == 72)
      {
        this.name = "Silver Coin";
        this.width = 10;
        this.height = 12;
        this.maxStack = 100;
      }
      else if (this.type == 73)
      {
        this.name = "Gold Coin";
        this.width = 10;
        this.height = 12;
        this.maxStack = 100;
      }
      else if (this.type == 74)
      {
        this.name = "Platinum Coin";
        this.width = 10;
        this.height = 12;
        this.maxStack = 100;
      }
      else if (this.type == 75)
      {
        this.name = "Fallen Star";
        this.width = 18;
        this.height = 20;
        this.maxStack = 100;
        this.alpha = 75;
        this.toolTip = "Disappears after the sunrise";
        this.value = 500;
        this.useStyle = 4;
        this.useSound = 4;
        this.useTurn = false;
        this.useAnimation = 17;
        this.useTime = 17;
        this.healMana = 20;
        this.consumable = true;
        this.rare = 1;
        this.potion = true;
      }
      else if (this.type == 76)
      {
        this.name = "Copper Greaves";
        this.width = 18;
        this.height = 28;
        this.defense = 1;
        this.legSlot = 0;
        this.value = 750;
      }
      else if (this.type == 77)
      {
        this.name = "Iron Greaves";
        this.width = 18;
        this.height = 28;
        this.defense = 2;
        this.legSlot = 1;
        this.rare = 1;
        this.value = 3000;
      }
      else if (this.type == 78)
      {
        this.name = "Silver Greaves";
        this.width = 18;
        this.height = 28;
        this.defense = 3;
        this.legSlot = 2;
        this.rare = 2;
        this.value = 7500;
      }
      else if (this.type == 79)
      {
        this.name = "Gold Greaves";
        this.width = 18;
        this.height = 28;
        this.defense = 4;
        this.legSlot = 3;
        this.rare = 2;
        this.value = 15000;
      }
      else if (this.type == 80)
      {
        this.name = "Copper Chainmail";
        this.width = 26;
        this.height = 28;
        this.defense = 2;
        this.bodySlot = 0;
        this.value = 1000;
      }
      else if (this.type == 81)
      {
        this.name = "Iron Chainmail";
        this.width = 26;
        this.height = 28;
        this.defense = 3;
        this.bodySlot = 1;
        this.rare = 1;
        this.value = 4000;
      }
      else if (this.type == 82)
      {
        this.name = "Silver Chainmail";
        this.width = 26;
        this.height = 28;
        this.defense = 4;
        this.bodySlot = 2;
        this.rare = 2;
        this.value = 10000;
      }
      else if (this.type == 83)
      {
        this.name = "Gold Chainmail";
        this.width = 26;
        this.height = 28;
        this.defense = 5;
        this.bodySlot = 3;
        this.rare = 2;
        this.value = 20000;
      }
      else if (this.type == 84)
      {
        this.noUseGraphic = true;
        this.damage = 5;
        this.knockBack = 7f;
        this.useStyle = 5;
        this.name = "Grappling Hook";
        this.shootSpeed = 11f;
        this.shoot = 13;
        this.width = 18;
        this.height = 28;
        this.useSound = 1;
        this.useAnimation = 20;
        this.useTime = 20;
        this.rare = 3;
        this.noMelee = true;
        this.value = 20000;
      }
      else if (this.type == 85)
      {
        this.name = "Iron Chain";
        this.width = 14;
        this.height = 20;
        this.maxStack = 99;
        this.value = 1000;
      }
      else if (this.type == 86)
      {
        this.name = "Shadow Scale";
        this.width = 14;
        this.height = 18;
        this.maxStack = 99;
        this.rare = 3;
        this.value = 500;
      }
      else if (this.type == 87)
      {
        this.name = "Piggy Bank";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 29;
        this.width = 20;
        this.height = 12;
        this.value = 10000;
      }
      else if (this.type == 88)
      {
        this.name = "Mining Helmet";
        this.width = 22;
        this.height = 16;
        this.defense = 1;
        this.headSlot = 1;
        this.rare = 2;
        this.value = 100000;
        this.toolTip = "Provides light when worn";
      }
      else if (this.type == 89)
      {
        this.name = "Copper Helmet";
        this.width = 22;
        this.height = 22;
        this.defense = 1;
        this.headSlot = 2;
        this.value = 1250;
      }
      else if (this.type == 90)
      {
        this.name = "Iron Helmet";
        this.width = 22;
        this.height = 22;
        this.defense = 2;
        this.headSlot = 3;
        this.rare = 1;
        this.value = 5000;
      }
      else if (this.type == 91)
      {
        this.name = "Silver Helmet";
        this.width = 22;
        this.height = 22;
        this.defense = 3;
        this.headSlot = 4;
        this.rare = 2;
        this.value = 12500;
      }
      else if (this.type == 92)
      {
        this.name = "Gold Helmet";
        this.width = 22;
        this.height = 22;
        this.defense = 4;
        this.headSlot = 5;
        this.rare = 2;
        this.value = 25000;
      }
      else if (this.type == 93)
      {
        this.name = "Wood Wall";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createWall = 4;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 94)
      {
        this.name = "Wood Platform";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 19;
        this.width = 8;
        this.height = 10;
      }
      else if (this.type == 95)
      {
        this.useStyle = 5;
        this.useAnimation = 20;
        this.useTime = 20;
        this.name = "Flintlock Pistol";
        this.width = 24;
        this.height = 28;
        this.shoot = 14;
        this.useAmmo = 14;
        this.useSound = 11;
        this.damage = 4;
        this.shootSpeed = 5f;
        this.noMelee = true;
        this.value = 50000;
        this.scale = 0.9f;
        this.rare = 2;
      }
      else if (this.type == 96)
      {
        this.useStyle = 5;
        this.autoReuse = true;
        this.useAnimation = 45;
        this.useTime = 45;
        this.name = "Musket";
        this.width = 44;
        this.height = 14;
        this.shoot = 10;
        this.useAmmo = 14;
        this.useSound = 11;
        this.damage = 10;
        this.shootSpeed = 8f;
        this.noMelee = true;
        this.value = 100000;
        this.knockBack = 3f;
        this.rare = 2;
      }
      else if (this.type == 97)
      {
        this.name = "Musket Ball";
        this.shootSpeed = 4f;
        this.shoot = 14;
        this.damage = 6;
        this.width = 8;
        this.height = 8;
        this.maxStack = 250;
        this.consumable = true;
        this.ammo = 14;
        this.knockBack = 2f;
        this.value = 10;
      }
      else if (this.type == 98)
      {
        this.useStyle = 5;
        this.autoReuse = true;
        this.useAnimation = 8;
        this.useTime = 8;
        this.name = "Minishark";
        this.width = 50;
        this.height = 18;
        this.shoot = 10;
        this.useAmmo = 14;
        this.useSound = 11;
        this.damage = 6;
        this.shootSpeed = 7f;
        this.noMelee = true;
        this.value = 500000;
        this.rare = 4;
        this.toolTip = "Half shark, half gun, completely awesome.";
      }
      else if (this.type == 99)
      {
        this.useStyle = 5;
        this.useAnimation = 28;
        this.useTime = 28;
        this.name = "Iron Bow";
        this.width = 12;
        this.height = 28;
        this.shoot = 1;
        this.useAmmo = 1;
        this.useSound = 5;
        this.damage = 8;
        this.shootSpeed = 6.6f;
        this.noMelee = true;
        this.value = 1400;
        this.rare = 1;
      }
      else if (this.type == 100)
      {
        this.name = "Shadow Greaves";
        this.width = 18;
        this.height = 28;
        this.defense = 5;
        this.legSlot = 4;
        this.rare = 3;
        this.value = 22500;
      }
      else if (this.type == 101)
      {
        this.name = "Shadow Scalemail";
        this.width = 26;
        this.height = 28;
        this.defense = 6;
        this.bodySlot = 4;
        this.rare = 3;
        this.value = 30000;
      }
      else if (this.type == 102)
      {
        this.name = "Shadow Helmet";
        this.width = 22;
        this.height = 22;
        this.defense = 5;
        this.headSlot = 6;
        this.rare = 3;
        this.value = 37500;
      }
      else if (this.type == 103)
      {
        this.name = "Nightmare Pickaxe";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 20;
        this.useTime = 15;
        this.autoReuse = true;
        this.width = 24;
        this.height = 28;
        this.damage = 11;
        this.pick = 65;
        this.useSound = 1;
        this.knockBack = 3f;
        this.rare = 3;
        this.value = 18000;
        this.scale = 1.15f;
      }
      else if (this.type == 104)
      {
        this.name = "The Breaker";
        this.autoReuse = true;
        this.useStyle = 1;
        this.useAnimation = 40;
        this.useTime = 19;
        this.hammer = 55;
        this.width = 24;
        this.height = 28;
        this.damage = 28;
        this.knockBack = 7f;
        this.scale = 1.3f;
        this.useSound = 1;
        this.rare = 3;
        this.value = 15000;
      }
      else if (this.type == 105)
      {
        this.name = "Candle";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 33;
        this.width = 8;
        this.height = 18;
        this.holdStyle = 1;
      }
      else if (this.type == 106)
      {
        this.name = "Copper Chandelier";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 34;
        this.width = 26;
        this.height = 26;
      }
      else if (this.type == 107)
      {
        this.name = "Silver Chandelier";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 35;
        this.width = 26;
        this.height = 26;
      }
      else if (this.type == 108)
      {
        this.name = "Gold Chandelier";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 36;
        this.width = 26;
        this.height = 26;
      }
      else if (this.type == 109)
      {
        this.name = "Mana Crystal";
        this.maxStack = 99;
        this.consumable = true;
        this.width = 18;
        this.height = 18;
        this.useStyle = 4;
        this.useTime = 30;
        this.useSound = 4;
        this.useAnimation = 30;
        this.toolTip = "Increases maximum mana";
        this.rare = 4;
      }
      else if (this.type == 110)
      {
        this.name = "Lesser Mana Potion";
        this.useSound = 3;
        this.healMana = 100;
        this.useStyle = 2;
        this.useTurn = true;
        this.useAnimation = 17;
        this.useTime = 17;
        this.maxStack = 99;
        this.consumable = true;
        this.width = 14;
        this.height = 24;
        this.rare = 1;
        this.potion = true;
        this.value = 1000;
      }
      else if (this.type == 111)
      {
        this.name = "Band of Starpower";
        this.width = 22;
        this.height = 22;
        this.accessory = true;
        this.manaRegen = 3;
        this.rare = 3;
        this.toolTip = "Slowly regenerates mana";
        this.value = 50000;
      }
      else if (this.type == 112)
      {
        this.mana = 10;
        this.damage = 20;
        this.useStyle = 1;
        this.name = "Flower of Fire";
        this.shootSpeed = 6f;
        this.shoot = 15;
        this.width = 26;
        this.height = 28;
        this.useSound = 8;
        this.useAnimation = 30;
        this.useTime = 30;
        this.rare = 3;
        this.noMelee = true;
        this.knockBack = 5f;
        this.toolTip = "Throws balls of fire";
        this.value = 10000;
      }
      else if (this.type == 113)
      {
        this.mana = 20;
        this.channel = true;
        this.damage = 30;
        this.useStyle = 1;
        this.name = "Magic Missile";
        this.shootSpeed = 6f;
        this.shoot = 16;
        this.width = 26;
        this.height = 28;
        this.useSound = 9;
        this.useAnimation = 20;
        this.useTime = 20;
        this.rare = 3;
        this.noMelee = true;
        this.knockBack = 5f;
        this.toolTip = "Casts a controllable missile";
        this.value = 10000;
      }
      else if (this.type == 114)
      {
        this.mana = 5;
        this.channel = true;
        this.damage = 0;
        this.useStyle = 1;
        this.name = "Dirt Rod";
        this.shoot = 17;
        this.width = 26;
        this.height = 28;
        this.useSound = 8;
        this.useAnimation = 20;
        this.useTime = 20;
        this.rare = 3;
        this.noMelee = true;
        this.knockBack = 5f;
        this.toolTip = "Magically move dirt";
        this.value = 200000;
      }
      else if (this.type == 115)
      {
        this.mana = 40;
        this.channel = true;
        this.damage = 0;
        this.useStyle = 4;
        this.name = "Orb of Light";
        this.shoot = 18;
        this.width = 24;
        this.height = 24;
        this.useSound = 8;
        this.useAnimation = 20;
        this.useTime = 20;
        this.rare = 3;
        this.noMelee = true;
        this.toolTip = "Creates a magical orb of light";
        this.value = 10000;
      }
      else if (this.type == 116)
      {
        this.name = "Meteorite";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 37;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 117)
      {
        this.name = "Meteorite Bar";
        this.width = 20;
        this.height = 20;
        this.maxStack = 99;
        this.rare = 3;
        this.toolTip = "Warm to the touch";
        this.value = 20000;
      }
      else if (this.type == 118)
      {
        this.name = "Hook";
        this.maxStack = 99;
        this.width = 18;
        this.height = 18;
        this.value = 1000;
        this.toolTip = "Combine with chains to making a grappling hook";
      }
      else if (this.type == 119)
      {
        this.noMelee = true;
        this.useStyle = 1;
        this.name = "Flamarang";
        this.shootSpeed = 11f;
        this.shoot = 19;
        this.damage = 30;
        this.knockBack = 8f;
        this.width = 14;
        this.height = 28;
        this.useSound = 1;
        this.useAnimation = 15;
        this.useTime = 15;
        this.noUseGraphic = true;
        this.rare = 3;
        this.value = 100000;
      }
      else if (this.type == 120)
      {
        this.useStyle = 5;
        this.useAnimation = 25;
        this.useTime = 25;
        this.name = "Molten Fury";
        this.width = 14;
        this.height = 32;
        this.shoot = 1;
        this.useAmmo = 1;
        this.useSound = 5;
        this.damage = 25;
        this.shootSpeed = 8f;
        this.knockBack = 2f;
        this.alpha = 30;
        this.rare = 3;
        this.noMelee = true;
        this.scale = 1.1f;
        this.value = 27000;
        this.toolTip = "Lights wooden arrows ablaze";
      }
      else if (this.type == 121)
      {
        this.name = "Fiery Greatsword";
        this.useStyle = 1;
        this.useAnimation = 35;
        this.knockBack = 6.5f;
        this.width = 24;
        this.height = 28;
        this.damage = 30;
        this.scale = 1.3f;
        this.useSound = 1;
        this.rare = 3;
        this.value = 27000;
        this.toolTip = "It's made out of fire!";
      }
      if (this.type == 122)
      {
        this.name = "Molten Pickaxe";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 25;
        this.useTime = 25;
        this.autoReuse = true;
        this.width = 24;
        this.height = 28;
        this.damage = 15;
        this.pick = 100;
        this.scale = 1.15f;
        this.useSound = 1;
        this.knockBack = 2f;
        this.rare = 3;
        this.value = 27000;
      }
      else if (this.type == 123)
      {
        this.name = "Meteor Helmet";
        this.width = 22;
        this.height = 22;
        this.defense = 5;
        this.headSlot = 7;
        this.rare = 3;
        this.value = 45000;
        this.manaRegen = 3;
        this.toolTip = "Slowly regenerates mana";
      }
      else if (this.type == 124)
      {
        this.name = "Meteor Suit";
        this.width = 26;
        this.height = 28;
        this.defense = 6;
        this.bodySlot = 5;
        this.rare = 3;
        this.value = 30000;
        this.manaRegen = 3;
        this.toolTip = "Slowly regenerates mana";
      }
      else if (this.type == 125)
      {
        this.name = "Meteor Leggings";
        this.width = 18;
        this.height = 28;
        this.defense = 5;
        this.legSlot = 5;
        this.rare = 3;
        this.value = 30000;
      }
      else if (this.type == 126)
      {
        this.autoReuse = true;
        this.useStyle = 5;
        this.useAnimation = 8;
        this.useTime = 8;
        this.name = "Zapinator";
        this.width = 24;
        this.height = 28;
        this.shoot = 20;
        this.useSound = 12;
        this.knockBack = 3f;
        this.damage = 50;
        this.shootSpeed = 12f;
        this.noMelee = true;
        this.scale = 0.8f;
        this.rare = 4;
      }
      else if (this.type == (int) sbyte.MaxValue)
      {
        this.autoReuse = true;
        this.useStyle = 5;
        this.useAnimation = 15;
        this.useTime = 15;
        this.name = "Space Gun";
        this.width = 24;
        this.height = 28;
        this.shoot = 20;
        this.mana = 7;
        this.useSound = 12;
        this.knockBack = 3f;
        this.damage = 25;
        this.shootSpeed = 10f;
        this.noMelee = true;
        this.scale = 0.8f;
        this.rare = 3;
      }
      else if (this.type == 128)
      {
        this.mana = 7;
        this.name = "Rocket Boots";
        this.width = 28;
        this.height = 24;
        this.accessory = true;
        this.rare = 3;
        this.toolTip = "Allows flight";
        this.value = 50000;
      }
      else if (this.type == 129)
      {
        this.name = "Gray Brick";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 38;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 130)
      {
        this.name = "Gray Brick Wall";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createWall = 5;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 131)
      {
        this.name = "Red Brick";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 39;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 132)
      {
        this.name = "Red Brick Wall";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createWall = 6;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 133)
      {
        this.name = "Clay Block";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 40;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 134)
      {
        this.name = "Blue Brick";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 41;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 135)
      {
        this.name = "Blue Brick Wall";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createWall = 7;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 136)
      {
        this.name = "Chain Lantern";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 42;
        this.width = 12;
        this.height = 28;
      }
      else if (this.type == 137)
      {
        this.name = "Green Brick";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 43;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 138)
      {
        this.name = "Green Brick Wall";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createWall = 8;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 139)
      {
        this.name = "Pink Brick";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 44;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 140)
      {
        this.name = "Pink Brick Wall";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createWall = 9;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 141)
      {
        this.name = "Gold Brick";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 45;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 142)
      {
        this.name = "Gold Brick Wall";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createWall = 10;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 143)
      {
        this.name = "Silver Brick";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 46;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 144)
      {
        this.name = "Silver Brick Wall";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createWall = 11;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 145)
      {
        this.name = "Copper Brick";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 47;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 146)
      {
        this.name = "Copper Brick Wall";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createWall = 12;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 147)
      {
        this.name = "Spike";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 48;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 148)
      {
        this.name = "Water Candle";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 49;
        this.width = 8;
        this.height = 18;
        this.holdStyle = 1;
        this.toolTip = "Holding this may attract unwanted attention";
      }
      else if (this.type == 149)
      {
        this.name = "Book";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createTile = 50;
        this.width = 24;
        this.height = 28;
        this.toolTip = "It contains strange symbols";
      }
      else if (this.type == 150)
      {
        this.name = "Cobweb";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 51;
        this.width = 20;
        this.height = 24;
        this.alpha = 100;
      }
      else if (this.type == 151)
      {
        this.name = "Necro Helmet";
        this.width = 22;
        this.height = 22;
        this.defense = 5;
        this.headSlot = 8;
        this.rare = 2;
        this.value = 45000;
      }
      else if (this.type == 152)
      {
        this.name = "Necro Breastplate";
        this.width = 26;
        this.height = 28;
        this.defense = 6;
        this.bodySlot = 6;
        this.rare = 2;
        this.value = 30000;
      }
      else if (this.type == 153)
      {
        this.name = "Necro Grieves";
        this.width = 18;
        this.height = 28;
        this.defense = 5;
        this.legSlot = 6;
        this.rare = 2;
        this.value = 30000;
      }
      else if (this.type == 154)
      {
        this.name = "Bone";
        this.maxStack = 99;
        this.consumable = true;
        this.width = 12;
        this.height = 14;
        this.value = 50;
        this.useAnimation = 12;
        this.useTime = 12;
        this.useStyle = 1;
        this.useSound = 1;
        this.shootSpeed = 8f;
        this.noUseGraphic = true;
        this.damage = 22;
        this.knockBack = 4f;
        this.shoot = 21;
      }
      else if (this.type == 155)
      {
        this.autoReuse = true;
        this.useTurn = true;
        this.name = "Muramasa";
        this.useStyle = 1;
        this.useAnimation = 20;
        this.knockBack = 3f;
        this.width = 40;
        this.height = 40;
        this.damage = 25;
        this.scale = 1.2f;
        this.useSound = 1;
        this.rare = 3;
        this.value = 27000;
      }
      else if (this.type == 156)
      {
        this.name = "Cobalt Shield";
        this.width = 24;
        this.height = 28;
        this.rare = 3;
        this.value = 27000;
        this.accessory = true;
        this.defense = 2;
        this.toolTip = "Grants immunity to knockback";
      }
      else if (this.type == 157)
      {
        this.mana = 5;
        this.autoReuse = true;
        this.name = "Aqua Scepter";
        this.useStyle = 5;
        this.useAnimation = 30;
        this.useTime = 5;
        this.knockBack = 3f;
        this.width = 38;
        this.height = 10;
        this.damage = 15;
        this.scale = 1f;
        this.shoot = 22;
        this.shootSpeed = 10f;
        this.useSound = 13;
        this.rare = 3;
        this.value = 27000;
        this.toolTip = "Sprays out a shower of water";
      }
      else if (this.type == 158)
      {
        this.name = "Lucky Horseshoe";
        this.width = 20;
        this.height = 22;
        this.rare = 3;
        this.value = 27000;
        this.accessory = true;
        this.toolTip = "Mitigates fall damage";
      }
      else if (this.type == 159)
      {
        this.name = "Shiney Red Ballon";
        this.width = 14;
        this.height = 28;
        this.rare = 3;
        this.value = 27000;
        this.accessory = true;
        this.toolTip = "Increases jump height";
      }
      else if (this.type == 160)
      {
        this.autoReuse = true;
        this.name = "Harpoonitron";
        this.useStyle = 5;
        this.useAnimation = 30;
        this.useTime = 30;
        this.knockBack = 6f;
        this.width = 30;
        this.height = 10;
        this.damage = 15;
        this.scale = 1.1f;
        this.shoot = 23;
        this.shootSpeed = 10f;
        this.useSound = 10;
        this.rare = 3;
        this.value = 27000;
      }
      else if (this.type == 161)
      {
        this.useStyle = 1;
        this.name = "Spiky Ball";
        this.shootSpeed = 5f;
        this.shoot = 24;
        this.knockBack = 1f;
        this.damage = 12;
        this.width = 10;
        this.height = 10;
        this.maxStack = 250;
        this.consumable = true;
        this.useSound = 1;
        this.useAnimation = 15;
        this.useTime = 15;
        this.noUseGraphic = true;
        this.noMelee = true;
        this.value = 20;
      }
      else if (this.type == 162)
      {
        this.name = "Ball 'O Hurt";
        this.useStyle = 5;
        this.useAnimation = 30;
        this.useTime = 30;
        this.knockBack = 7f;
        this.width = 30;
        this.height = 10;
        this.damage = 15;
        this.scale = 1.1f;
        this.noUseGraphic = true;
        this.shoot = 25;
        this.shootSpeed = 12f;
        this.useSound = 1;
        this.rare = 3;
        this.value = 27000;
      }
      else if (this.type == 163)
      {
        this.name = "Blue Moon";
        this.useStyle = 5;
        this.useAnimation = 30;
        this.useTime = 30;
        this.knockBack = 7f;
        this.width = 30;
        this.height = 10;
        this.damage = 30;
        this.scale = 1.1f;
        this.noUseGraphic = true;
        this.shoot = 26;
        this.shootSpeed = 12f;
        this.useSound = 1;
        this.rare = 3;
        this.value = 27000;
      }
      else if (this.type == 164)
      {
        this.autoReuse = false;
        this.useStyle = 5;
        this.useAnimation = 10;
        this.useTime = 10;
        this.name = "Handgun";
        this.width = 24;
        this.height = 28;
        this.shoot = 14;
        this.knockBack = 3f;
        this.useAmmo = 14;
        this.useSound = 11;
        this.damage = 20;
        this.shootSpeed = 10f;
        this.noMelee = true;
        this.value = 50000;
        this.scale = 0.9f;
        this.rare = 2;
      }
      else if (this.type == 165)
      {
        this.rare = 3;
        this.mana = 20;
        this.useSound = 8;
        this.name = "Water Bolt";
        this.useStyle = 5;
        this.damage = 15;
        this.useAnimation = 20;
        this.useTime = 20;
        this.width = 24;
        this.height = 28;
        this.shoot = 27;
        this.scale = 0.8f;
        this.shootSpeed = 4f;
        this.knockBack = 5f;
        this.toolTip = "Casts a slow moving bolt of water";
      }
      else if (this.type == 166)
      {
        this.useStyle = 1;
        this.name = "Bomb";
        this.shootSpeed = 5f;
        this.shoot = 28;
        this.width = 20;
        this.height = 20;
        this.maxStack = 20;
        this.consumable = true;
        this.useSound = 1;
        this.useAnimation = 25;
        this.useTime = 25;
        this.noUseGraphic = true;
        this.noMelee = true;
        this.value = 1000;
        this.damage = 100;
        this.toolTip = "A small explosion that will destroy some tiles";
      }
      else if (this.type == 167)
      {
        this.useStyle = 1;
        this.name = "Dynamite";
        this.shootSpeed = 4f;
        this.shoot = 29;
        this.width = 8;
        this.height = 28;
        this.consumable = true;
        this.useSound = 1;
        this.useAnimation = 40;
        this.useTime = 40;
        this.noUseGraphic = true;
        this.noMelee = true;
        this.value = 10000;
        this.rare = 1;
        this.damage = 250;
        this.toolTip = "A large explosion that will destroy most tiles";
      }
      else if (this.type == 168)
      {
        this.useStyle = 1;
        this.name = "Grenade";
        this.shootSpeed = 5.5f;
        this.shoot = 30;
        this.width = 20;
        this.height = 20;
        this.maxStack = 20;
        this.consumable = true;
        this.useSound = 1;
        this.useAnimation = 60;
        this.useTime = 60;
        this.noUseGraphic = true;
        this.noMelee = true;
        this.value = 1000;
        this.damage = 60;
        this.knockBack = 8f;
        this.toolTip = "A small explosion that will not destroy tiles";
      }
      else if (this.type == 169)
      {
        this.name = "Sand Block";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 53;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 170)
      {
        this.name = "Glass";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 54;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 171)
      {
        this.name = "Sign";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 55;
        this.width = 28;
        this.height = 28;
      }
      else if (this.type == 172)
      {
        this.name = "Ash Block";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 57;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 173)
      {
        this.name = "Obsidian";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 56;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 174)
      {
        this.name = "Hellstone";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 58;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 175)
      {
        this.name = "Hellstone Bar";
        this.width = 20;
        this.height = 20;
        this.maxStack = 99;
        this.rare = 3;
        this.toolTip = "Warm to the touch";
        this.value = 20000;
      }
      else if (this.type == 176)
      {
        this.name = "Mud Block";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 59;
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 177)
      {
        this.name = "Sapphire";
        this.maxStack = 99;
        this.alpha = 50;
        this.width = 10;
        this.height = 14;
        this.value = 100000;
      }
      else if (this.type == 178)
      {
        this.name = "Ruby";
        this.maxStack = 99;
        this.alpha = 50;
        this.width = 10;
        this.height = 14;
        this.value = 500000;
      }
      else if (this.type == 179)
      {
        this.name = "Emerald";
        this.maxStack = 99;
        this.alpha = 50;
        this.width = 10;
        this.height = 14;
        this.value = 250000;
      }
      else if (this.type == 180)
      {
        this.name = "Topaz";
        this.maxStack = 99;
        this.alpha = 50;
        this.width = 10;
        this.height = 14;
        this.value = 50000;
      }
      else if (this.type == 181)
      {
        this.name = "Amethyst";
        this.maxStack = 99;
        this.alpha = 50;
        this.width = 10;
        this.height = 14;
        this.value = 25000;
      }
      else if (this.type == 182)
      {
        this.name = "Diamond";
        this.maxStack = 99;
        this.alpha = 50;
        this.width = 10;
        this.height = 14;
        this.value = 1000000;
      }
      else if (this.type == 183)
      {
        this.name = "Glowing Mushroom";
        this.useStyle = 2;
        this.useSound = 2;
        this.useTurn = false;
        this.useAnimation = 17;
        this.useTime = 17;
        this.width = 16;
        this.height = 18;
        this.healLife = 50;
        this.maxStack = 99;
        this.consumable = true;
        this.potion = true;
        this.value = 50;
      }
      else if (this.type == 184)
      {
        this.name = "Star";
        this.width = 12;
        this.height = 12;
      }
      else if (this.type == 185)
      {
        this.noUseGraphic = true;
        this.damage = 20;
        this.knockBack = 7f;
        this.useStyle = 5;
        this.name = "Ivy Whip";
        this.shootSpeed = 13f;
        this.shoot = 32;
        this.width = 18;
        this.height = 28;
        this.useSound = 1;
        this.useAnimation = 20;
        this.useTime = 20;
        this.rare = 4;
        this.noMelee = true;
        this.value = 20000;
      }
      else if (this.type == 186)
      {
        this.name = "Breathing Reed";
        this.width = 44;
        this.height = 44;
        this.rare = 2;
        this.value = 10000;
        this.holdStyle = 2;
      }
      else if (this.type == 187)
      {
        this.name = "Flipper";
        this.width = 28;
        this.height = 28;
        this.rare = 2;
        this.value = 10000;
        this.accessory = true;
        this.toolTip = "Grants the ability to swim";
      }
      else if (this.type == 188)
      {
        this.name = "Healing Potion";
        this.useSound = 3;
        this.healLife = 200;
        this.useStyle = 2;
        this.useTurn = true;
        this.useAnimation = 17;
        this.useTime = 17;
        this.maxStack = 99;
        this.consumable = true;
        this.width = 14;
        this.height = 24;
        this.rare = 1;
        this.potion = true;
        this.value = 1000;
      }
      else if (this.type == 189)
      {
        this.name = "Mana Potion";
        this.useSound = 3;
        this.healMana = 200;
        this.useStyle = 2;
        this.useTurn = true;
        this.useAnimation = 17;
        this.useTime = 17;
        this.maxStack = 99;
        this.consumable = true;
        this.width = 14;
        this.height = 24;
        this.rare = 1;
        this.potion = true;
        this.value = 1000;
      }
      else if (this.type == 190)
      {
        this.name = "Blade of Grass";
        this.useStyle = 1;
        this.useAnimation = 30;
        this.knockBack = 3f;
        this.width = 40;
        this.height = 40;
        this.damage = 35;
        this.scale = 1.4f;
        this.useSound = 1;
        this.rare = 3;
        this.value = 27000;
      }
      else if (this.type == 191)
      {
        this.noMelee = true;
        this.useStyle = 1;
        this.name = "Thorn Chakrum";
        this.shootSpeed = 11f;
        this.shoot = 33;
        this.damage = 40;
        this.knockBack = 8f;
        this.width = 14;
        this.height = 28;
        this.useSound = 1;
        this.useAnimation = 15;
        this.useTime = 15;
        this.noUseGraphic = true;
        this.rare = 3;
        this.value = 50000;
      }
      else if (this.type == 192)
      {
        this.name = "Obsidian Block";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 250;
        this.consumable = true;
        this.createTile = 75;
        this.width = 12;
        this.height = 12;
      }
      else
      {
        if (this.type != 193)
          return;
        this.name = "Obsidian Skull";
        this.width = 20;
        this.height = 22;
        this.rare = 3;
        this.value = 27000;
        this.accessory = true;
        this.defense = 2;
        this.toolTip = "Grants immunity to fire blocks";
      }
    }

    public Color GetAlpha(Color newColor)
    {
      int r = (int) newColor.R - this.alpha;
      int g = (int) newColor.G - this.alpha;
      int b = (int) newColor.B - this.alpha;
      int a = (int) newColor.A - this.alpha;
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

    public void UpdateItem(int i)
    {
      if (!this.active)
        return;
      float num1 = 0.1f;
      float num2 = 10f;
      if (this.wet)
      {
        num2 = 5f;
        num1 = 0.08f;
      }
      Vector2 vector2 = this.velocity * 0.5f;
      if (this.ownTime > 0)
        --this.ownTime;
      else
        this.ownIgnore = -1;
      if (this.keepTime > 0)
        --this.keepTime;
      if (!this.beingGrabbed)
      {
        this.velocity.Y += num1;
        if ((double) this.velocity.Y > (double) num2)
          this.velocity.Y = num2;
        this.velocity.X *= 0.95f;
        if ((double) this.velocity.X < 0.1 && (double) this.velocity.X > -0.1)
          this.velocity.X = 0.0f;
        bool flag = Collision.LavaCollision(this.position, this.width, this.height);
        if (flag)
          this.lavaWet = true;
        if (Collision.WetCollision(this.position, this.width, this.height))
        {
          if (!this.wet)
          {
            if (this.wetCount == (byte) 0)
            {
              this.wetCount = (byte) 20;
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
                for (int index3 = 0; index3 < 5; ++index3)
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
          this.wet = false;
        if (!this.wet)
          this.lavaWet = false;
        if (this.wetCount > (byte) 0)
          --this.wetCount;
        if (this.wet)
        {
          if (this.wet)
          {
            Vector2 velocity = this.velocity;
            this.velocity = Collision.TileCollision(this.position, this.velocity, this.width, this.height);
            if ((double) this.velocity.X != (double) velocity.X)
              vector2.X = this.velocity.X;
            if ((double) this.velocity.Y != (double) velocity.Y)
              vector2.Y = this.velocity.Y;
          }
        }
        else
          this.velocity = Collision.TileCollision(this.position, this.velocity, this.width, this.height);
        if (this.owner == Game1.myPlayer && this.lavaWet)
        {
          this.active = false;
          this.type = 0;
          this.name = "";
          this.stack = 0;
          if (Game1.netMode != 0)
            NetMessage.SendData(21, number: i);
        }
        if (this.type == 75 && Game1.dayTime)
        {
          for (int index = 0; index < 10; ++index)
            Dust.NewDust(this.position, this.width, this.height, 15, this.velocity.X, this.velocity.Y, 150, Scale: 1.2f);
          for (int index = 0; index < 3; ++index)
            Gore.NewGore(this.position, new Vector2(this.velocity.X, this.velocity.Y), Game1.rand.Next(16, 18));
          this.active = false;
          this.type = 0;
          this.stack = 0;
          if (Game1.netMode == 2)
            NetMessage.SendData(21, number: i);
        }
      }
      else
        this.beingGrabbed = false;
      if (this.type == 8 || this.type == 41 || this.type == 75 || this.type == 105 || this.type == 116)
        Lighting.addLight((int) (((double) this.position.X - 7.0) / 16.0), (int) (((double) this.position.Y - 7.0) / 16.0), 1f);
      else if (this.type == 183)
        Lighting.addLight((int) (((double) this.position.X - 7.0) / 16.0), (int) (((double) this.position.Y - 7.0) / 16.0), 0.5f);
      if (this.type == 75)
      {
        if (Game1.rand.Next(25) == 0)
          Dust.NewDust(this.position, this.width, this.height, 15, this.velocity.X * 0.5f, this.velocity.Y * 0.5f, 150, Scale: 1.2f);
        if (Game1.rand.Next(50) == 0)
          Gore.NewGore(this.position, new Vector2(this.velocity.X * 0.2f, this.velocity.Y * 0.2f), Game1.rand.Next(16, 18));
      }
      if (this.spawnTime < 2147483646)
        ++this.spawnTime;
      if (Game1.netMode == 2 && this.owner != Game1.myPlayer)
      {
        ++this.release;
        if (this.release >= 300)
        {
          this.release = 0;
          NetMessage.SendData(39, this.owner, number: i);
        }
      }
      if (this.wet)
        this.position += vector2;
      else
        this.position += this.velocity;
      if (this.noGrabDelay > 0)
        --this.noGrabDelay;
    }

    public static int NewItem(
      int X,
      int Y,
      int Width,
      int Height,
      int Type,
      int Stack = 1,
      bool noBroadcast = false)
    {
      if (WorldGen.gen)
        return 0;
      int index1 = 200;
      Game1.item[200] = new Item();
      if (Game1.netMode != 1)
      {
        for (int index2 = 0; index2 < 200; ++index2)
        {
          if (!Game1.item[index2].active)
          {
            index1 = index2;
            break;
          }
        }
      }
      if (index1 == 200 && Game1.netMode != 1)
      {
        int num = 0;
        for (int index3 = 0; index3 < 200; ++index3)
        {
          if (Game1.item[index3].spawnTime > num)
          {
            num = Game1.item[index3].spawnTime;
            index1 = index3;
          }
        }
      }
      Game1.item[index1] = new Item();
      Game1.item[index1].SetDefaults(Type);
      Game1.item[index1].position.X = (float) (X + Width / 2 - Game1.item[index1].width / 2);
      Game1.item[index1].position.Y = (float) (Y + Height / 2 - Game1.item[index1].height / 2);
      Game1.item[index1].wet = Collision.WetCollision(Game1.item[index1].position, Game1.item[index1].width, Game1.item[index1].height);
      Game1.item[index1].velocity.X = (float) Game1.rand.Next(-20, 21) * 0.1f;
      Game1.item[index1].velocity.Y = (float) Game1.rand.Next(-30, -10) * 0.1f;
      Game1.item[index1].active = true;
      Game1.item[index1].spawnTime = 0;
      Game1.item[index1].stack = Stack;
      if (Game1.netMode == 2 && !noBroadcast)
      {
        NetMessage.SendData(21, number: index1);
        Game1.item[index1].FindOwner(index1);
      }
      else if (Game1.netMode == 0)
        Game1.item[index1].owner = Game1.myPlayer;
      return index1;
    }

    public void FindOwner(int whoAmI)
    {
      if (this.keepTime > 0)
        return;
      int owner = this.owner;
      this.owner = 8;
      float num1 = -1f;
      for (int index = 0; index < 8; ++index)
      {
        if (this.ownIgnore != index && Game1.player[index].active && Game1.player[index].ItemSpace(Game1.item[whoAmI]))
        {
          float num2 = Math.Abs(Game1.player[index].position.X + (float) (Game1.player[index].width / 2) - this.position.X - (float) (this.width / 2)) + Math.Abs(Game1.player[index].position.Y + (float) (Game1.player[index].height / 2) - this.position.Y - (float) this.height);
          if ((double) num2 < (double) (Game1.screenWidth / 2 + Game1.screenHeight / 2) && ((double) num1 == -1.0 || (double) num2 < (double) num1))
          {
            num1 = num2;
            this.owner = index;
          }
        }
      }
      if (this.owner == owner || (owner != Game1.myPlayer || Game1.netMode != 1) && (owner != 8 || Game1.netMode != 2) && Game1.player[owner].active)
        return;
      NetMessage.SendData(21, number: whoAmI);
      if (this.active)
        NetMessage.SendData(22, number: whoAmI);
    }

    public object Clone() => this.MemberwiseClone();

    public bool IsTheSameAs(Item compareItem) => this.name == compareItem.name;

    public bool IsNotTheSameAs(Item compareItem) => this.name != compareItem.name || this.stack != compareItem.stack;
  }
}
