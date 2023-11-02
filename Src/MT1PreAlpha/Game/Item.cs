
using Microsoft.Xna.Framework;

namespace GameManager
{
  public class Item
  {
    public static float gravity = 0.1f;
    public static float maxFallSpeed = 10f;
    public Vector2 position;
    public Vector2 velocity;
    public int width;
    public int height;
    public bool active;
    public int noGrabDelay;
    public bool beingGrabbed;
    public int type;
    public string name;
    public int holdStyle;
    public int useStyle;
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
    public bool consumable;
    public bool autoReuse;
    public bool useTurn;
    public Color color;
    public int alpha;
    public float scale = 1f;
    public int useSound = 0;
    public int defense;
    public int headSlot;
    public int bodySlot;
    public int legSlot;

    public void SetDefaults(string ItemName)
    {
      if (ItemName == "Gold Pickaxe")
      {
        this.SetDefaults(1);
        this.color = new Color(210, 190, 0, 100);
        this.useTime = 13;
        this.pick = 65;
        this.useAnimation = 19;
        this.scale = 1.1f;
        this.damage = 7;
      }
      if (ItemName == "Gold Broadsword")
      {
        this.SetDefaults(4);
        this.color = new Color(210, 190, 0, 100);
        this.useAnimation = 19;
        this.damage = 15;
        this.scale = 1.1f;
      }
      if (ItemName == "Gold Shortsword")
      {
        this.SetDefaults(6);
        this.color = new Color(210, 190, 0, 100);
        this.damage = 12;
        this.useAnimation = 10;
        this.scale = 1f;
      }
      if (ItemName == "Gold Axe")
      {
        this.SetDefaults(10);
        this.color = new Color(210, 190, 0, 100);
        this.useTime = 17;
        this.axe = 13;
        this.useAnimation = 25;
        this.scale = 1.2f;
        this.damage = 8;
      }
      if (ItemName == "Gold Hammer")
      {
        this.SetDefaults(7);
        this.color = new Color(210, 190, 0, 100);
        this.useAnimation = 27;
        this.useTime = 17;
        this.hammer = 65;
        this.scale = 1.3f;
        this.damage = 10;
      }
      if (ItemName == "Silver Pickaxe")
      {
        this.SetDefaults(1);
        this.color = new Color(180, 180, 180, 100);
        this.useTime = 17;
        this.pick = 55;
        this.useAnimation = 20;
        this.scale = 1.05f;
        this.damage = 6;
      }
      if (ItemName == "Silver Broadsword")
      {
        this.SetDefaults(4);
        this.color = new Color(180, 180, 180, 100);
        this.useAnimation = 20;
        this.damage = 13;
        this.scale = 1.05f;
      }
      if (ItemName == "Silver Shortsword")
      {
        this.SetDefaults(6);
        this.color = new Color(180, 180, 180, 100);
        this.damage = 11;
        this.useAnimation = 11;
        this.scale = 0.95f;
      }
      if (ItemName == "Silver Axe")
      {
        this.SetDefaults(10);
        this.color = new Color(180, 180, 180, 100);
        this.useTime = 18;
        this.axe = 11;
        this.useAnimation = 26;
        this.scale = 1.15f;
        this.damage = 7;
      }
      if (ItemName == "Silver Hammer")
      {
        this.SetDefaults(7);
        this.color = new Color(180, 180, 180, 100);
        this.useAnimation = 28;
        this.useTime = 23;
        this.scale = 1.25f;
        this.damage = 9;
        this.hammer = 55;
      }
      if (ItemName == "Copper Pickaxe")
      {
        this.SetDefaults(1);
        this.color = new Color(180, 100, 45, 80);
        this.useTime = 15;
        this.pick = 35;
        this.useAnimation = 23;
        this.scale = 0.9f;
        this.tileBoost = -1;
        this.damage = 2;
      }
      if (ItemName == "Copper Broadsword")
      {
        this.SetDefaults(4);
        this.color = new Color(180, 100, 45, 80);
        this.useAnimation = 23;
        this.damage = 8;
        this.scale = 0.9f;
      }
      if (ItemName == "Copper Shortsword")
      {
        this.SetDefaults(6);
        this.color = new Color(180, 100, 45, 80);
        this.damage = 6;
        this.useAnimation = 13;
        this.scale = 0.8f;
      }
      if (ItemName == "Copper Axe")
      {
        this.SetDefaults(10);
        this.color = new Color(180, 100, 45, 80);
        this.useTime = 21;
        this.axe = 8;
        this.useAnimation = 30;
        this.scale = 1f;
        this.damage = 3;
        this.tileBoost = -1;
      }
      if (ItemName == "Copper Hammer")
      {
        this.SetDefaults(7);
        this.color = new Color(180, 100, 45, 80);
        this.useAnimation = 33;
        this.useTime = 23;
        this.scale = 1.1f;
        this.damage = 4;
        this.hammer = 35;
        this.tileBoost = -1;
      }
      this.name = ItemName;
    }

    public void SetDefaults(int Type)
    {
      this.active = true;
      this.alpha = 0;
      this.autoReuse = false;
      this.axe = 0;
      this.color = new Color();
      this.consumable = false;
      this.createTile = -1;
      this.createWall = -1;
      this.damage = -1;
      this.holdStyle = 0;
      this.knockBack = 0.0f;
      this.maxStack = 1;
      this.pick = 0;
      this.scale = 1f;
      this.stack = 1;
      this.tileBoost = 0;
      this.type = Type;
      this.useStyle = 0;
      this.useTurn = false;

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

        //RnD
        this.damage = 50;//5;

        this.pick = 45;
        this.useSound = 1;
      }
      else if (this.type == 2)
      {
        this.name = "Dirt Block";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
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
        this.maxStack = 99;
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

        //RnD
        this.damage = 100;//8;

        this.knockBack = 4f;
        this.scale = 0.9f;
        this.useSound = 1;
        this.useTurn = true;
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
        this.knockBack = 6.5f;
        this.scale = 1.2f;
        this.useSound = 1;
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
        this.damage = 1;
      }
      else if (this.type == 9)
      {
        this.name = "Wood";
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.width = 8;
        this.height = 10;
      }
      else if (this.type == 10)
      {
        this.name = "Iron Axe";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 27;
        this.knockBack = 5.5f;
        this.useTime = 19;
        this.autoReuse = true;
        this.width = 24;
        this.height = 28;
        this.damage = 5;
        this.axe = 9;
        this.scale = 1.1f;
        this.useSound = 1;
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
        this.name = "Green Tunic";
        this.width = 24;
        this.height = 28;
        this.bodySlot = 1;
        this.defense = 2;
      }
      else if (this.type == 16)
      {
        this.name = "Gray Tunic";
        this.width = 24;
        this.height = 28;
        this.bodySlot = 2;
        this.defense = 2;
      }
      else if (this.type == 17)
      {
        this.name = "White Pants";
        this.width = 24;
        this.height = 28;
        this.legSlot = 1;
        this.defense = 1;
      }
      else if (this.type == 18)
      {
        this.name = "Blue Pants";
        this.width = 24;
        this.height = 28;
        this.legSlot = 2;
        this.defense = 1;
      }
      else if (this.type == 19)
      {
        this.name = "Gold Bar";
        this.width = 20;
        this.height = 20;
        this.maxStack = 99;
      }
      else if (this.type == 20)
      {
        this.name = "Copper Bar";
        this.width = 20;
        this.height = 20;
        this.maxStack = 99;
      }
      else if (this.type == 21)
      {
        this.name = "Silver Bar";
        this.width = 20;
        this.height = 20;
        this.maxStack = 99;
      }
      else if (this.type == 22)
      {
        this.name = "Iron Bar";
        this.width = 20;
        this.height = 20;
        this.maxStack = 99;
      }
      else if (this.type == 23)
      {
        this.name = "Gel";
        this.width = 20;
        this.height = 12;
        this.maxStack = 12;
        this.alpha = 175;
        this.color = new Color(0, 80, (int) byte.MaxValue, 100);
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
      }
      else
      {
        if (this.type != 26)
          return;
        this.name = "Stone Wall";
        this.useStyle = 1;
        this.useTurn = true;
        this.useAnimation = 15;
        this.useTime = 10;
        this.autoReuse = true;
        this.maxStack = 99;
        this.consumable = true;
        this.createWall = 1;
        this.width = 12;
        this.height = 12;
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
      if (!this.beingGrabbed)
      {
        this.velocity.Y += Item.gravity;
        if ((double) this.velocity.Y > (double) Item.maxFallSpeed)
          this.velocity.Y = Item.maxFallSpeed;
        this.velocity.X *= 0.95f;
        if ((double) this.velocity.X < 0.1 && (double) this.velocity.X > -0.1)
          this.velocity.X = 0.0f;
        this.velocity = Collision.TileCollision(this.position, this.velocity, this.width, this.height);
      }
      else
        this.beingGrabbed = false;
      if (this.type == 8)
        Lighting.addLight((int) (((double) this.position.X - 7.0) / 16.0), (int) (((double) this.position.Y - 7.0) / 16.0), byte.MaxValue);
      this.position += this.velocity;
      if (this.noGrabDelay <= 0)
        return;
      --this.noGrabDelay;
    }

    public static int NewItem(int X, int Y, int Width, int Height, int Type)
    {
      int index1 = -1;
      for (int index2 = 0; index2 < 1000; ++index2)
      {
        if (!Game1.item[index2].active)
        {
          index1 = index2;
          break;
        }
      }
      if (index1 == -1)
      {
        index1 = 999;
        for (int index3 = 0; index3 < 999; ++index3)
        {
          Game1.item[index3] = new Item();
          Game1.item[index3] = Game1.item[index3 + 1];
        }
      }
      Game1.item[index1] = new Item();
      Game1.item[index1].SetDefaults(Type);
      Game1.item[index1].position.X = (float) (X + Width / 2 - Game1.item[index1].width / 2);
      Game1.item[index1].position.Y = (float) (Y + Height / 2 - Game1.item[index1].height / 2);
      Game1.item[index1].velocity.X = (float) Game1.rand.Next(-20, 21) * 0.1f;
      Game1.item[index1].velocity.Y = (float) Game1.rand.Next(-30, -10) * 0.1f;
      Game1.item[index1].active = true;
      return index1;
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public bool IsTheSameAs(Item compareItem)
    {
        return this.name == compareItem.name;
    }
  }
}
