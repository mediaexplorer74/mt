// Decompiled with JetBrains decompiler
// Type: GameManager.Player
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GameManager
{
  public class Player
  {
    public bool pvpDeath = false;
    public bool zoneDungeon = false;
    public bool zoneEvil = false;
    public bool zoneMeteor = false;
    public bool zoneJungle = false;
    public bool boneArmor = false;
    public int townNPCs = 0;
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 oldVelocity;
    public double headFrameCounter;
    public double bodyFrameCounter;
    public double legFrameCounter;
    public bool immune;
    public int immuneTime;
    public int immuneAlphaDirection;
    public int immuneAlpha;
    public int team = 0;
    public string chatText = "";
    public int sign = -1;
    public int chatShowTime = 0;
    public int activeNPCs;
    public bool mouseInterface;
    public int changeItem = -1;
    public int selectedItem = 0;
    public Item[] armor = new Item[8];
    public int itemAnimation;
    public int itemAnimationMax;
    public int itemTime;
    public float itemRotation;
    public int itemWidth;
    public int itemHeight;
    public Vector2 itemLocation;
    public int breathCD = 0;
    public int breathMax = 200;
    public int breath = 200;
    public string setBonus = "";
    public Item[] inventory = new Item[44];
    public Item[] bank = new Item[Chest.maxItems];
    public float headRotation;
    public float bodyRotation;
    public float legRotation;
    public Vector2 headPosition;
    public Vector2 bodyPosition;
    public Vector2 legPosition;
    public Vector2 headVelocity;
    public Vector2 bodyVelocity;
    public Vector2 legVelocity;
    public bool dead = false;
    public int respawnTimer;
    public string name = "";
    public int attackCD;
    public int potionDelay = 0;
    public bool wet = false;
    public byte wetCount = 0;
    public bool lavaWet = false;
    public int hitTile;
    public int hitTileX;
    public int hitTileY;
    public int jump;
    public int head = -1;
    public int body = -1;
    public int legs = -1;
    public Rectangle headFrame;
    public Rectangle bodyFrame;
    public Rectangle legFrame;
    public Rectangle hairFrame;
    public bool controlLeft;
    public bool controlRight;
    public bool controlUp;
    public bool controlDown;
    public bool controlJump;
    public bool controlUseItem;
    public bool controlUseTile;
    public bool releaseJump;
    public bool releaseUseItem;
    public bool releaseUseTile;
    public bool releaseInventory;
    public bool delayUseItem;
    public bool active;
    public int width = 20;
    public int height = 42;
    public int direction = 1;
    public bool showItemIcon = false;
    public int showItemIcon2 = 0;
    public int whoAmi = 0;
    public int runSoundDelay = 0;
    public float shadow = 0.0f;
    public float manaCost = 1f;
    public bool fireWalk = false;
    public Vector2[] shadowPos = new Vector2[3];
    public int shadowCount = 0;
    public bool channel = false;
    public int statDefense = 0;
    public int statAttack = 0;
    public int statLifeMax = 100;
    public int statLife = 100;
    public int statMana = 0;
    public int statManaMax = 0;
    public int lifeRegen = 0;
    public int lifeRegenCount = 0;
    public int manaRegen = 0;
    public int manaRegenCount = 0;
    public int manaRegenDelay = 0;
    public bool noKnockback = false;
    public static int tileRangeX = 5;
    public static int tileRangeY = 4;
    private static int tileTargetX;
    private static int tileTargetY;
    private static int jumpHeight = 15;
    private static float jumpSpeed = 5.01f;
    public bool[] adjTile = new bool[76];
    public bool[] oldAdjTile = new bool[76];
    private static int itemGrabRange = 38;
    private static float itemGrabSpeed = 0.4f;
    private static float itemGrabSpeedMax = 4f;
    public Color hairColor = new Color(215, 90, 55);
    public Color skinColor = new Color((int) byte.MaxValue, 125, 90);
    public Color eyeColor = new Color(105, 90, 75);
    public Color shirtColor = new Color(175, 165, 140);
    public Color underShirtColor = new Color(160, 180, 215);
    public Color pantsColor = new Color((int) byte.MaxValue, 230, 175);
    public Color shoeColor = new Color(160, 105, 60);
    public int hair = 0;
    public bool hostile = false;
    public int accWatch = 0;
    public int accDepthMeter = 0;
    public bool accFlipper = false;
    public bool doubleJump = false;
    public bool jumpAgain = false;
    public int[] grappling = new int[20];
    public int grapCount = 0;
    public int rocketDelay = 0;
    public int rocketDelay2 = 0;
    public bool rocketRelease = false;
    public bool rocketFrame = false;
    public bool rocketBoots = false;
    public bool canRocket = false;
    public bool jumpBoost = false;
    public bool noFallDmg = false;
    public int swimTime = 0;
    public int chest = -1;
    public int chestX = 0;
    public int chestY = 0;
    public int talkNPC = -1;
    public int fallStart = 0;
    public int slowCount = 0;

    public void HealEffect(int healAmount)
    {
      CombatText.NewText(new Rectangle((int) this.position.X, (int) this.position.Y, this.width, this.height), new Color(100, 100, (int) byte.MaxValue, (int) byte.MaxValue), string.Concat((object) healAmount));
      if (Game1.netMode != 1 || this.whoAmi != Game1.myPlayer)
        return;
      NetMessage.SendData(35, number: this.whoAmi, number2: (float) healAmount);
    }

    public void ManaEffect(int manaAmount)
    {
      CombatText.NewText(new Rectangle((int) this.position.X, (int) this.position.Y, this.width, this.height), new Color(180, 50, (int) byte.MaxValue, (int) byte.MaxValue), string.Concat((object) manaAmount));
      if (Game1.netMode != 1 || this.whoAmi != Game1.myPlayer)
        return;
      NetMessage.SendData(43, number: this.whoAmi, number2: (float) manaAmount);
    }

    public void UpdatePlayer(int i)
    {
      float num1 = 10f;
      float num2 = 0.4f;
      Player.jumpHeight = 15;
      Player.jumpSpeed = 5.01f;
      if (this.wet)
      {
        num2 = 0.2f;
        num1 = 5f;
        Player.jumpHeight = 30;
        Player.jumpSpeed = 6.01f;
      }
      float num3 = 3f;
      float num4 = 0.08f;
      float num5 = 0.2f;
      float num6 = num3;
      if (!this.active)
        return;
      ++this.shadowCount;
      if (this.shadowCount == 1)
        this.shadowPos[2] = this.shadowPos[1];
      else if (this.shadowCount == 2)
        this.shadowPos[1] = this.shadowPos[0];
      else if (this.shadowCount >= 3)
      {
        this.shadowCount = 0;
        this.shadowPos[0] = this.position;
      }
      this.whoAmi = i;
      if (this.runSoundDelay > 0)
        --this.runSoundDelay;
      if (this.attackCD > 0)
        --this.attackCD;
      if (this.itemAnimation == 0)
        this.attackCD = 0;
      if (this.chatShowTime > 0)
        --this.chatShowTime;
      if (this.potionDelay > 0)
        --this.potionDelay;
      if (this.dead)
      {
        if (i == Game1.myPlayer)
        {
          Game1.npcChatText = "";
          Game1.editSign = false;
        }
        this.sign = -1;
        this.talkNPC = -1;
        this.statLife = 0;
        this.channel = false;
        this.potionDelay = 0;
        this.chest = -1;
        this.changeItem = -1;
        this.itemAnimation = 0;
        this.immuneAlpha += 2;
        if (this.immuneAlpha > (int) byte.MaxValue)
          this.immuneAlpha = (int) byte.MaxValue;
        --this.respawnTimer;
        this.headPosition += this.headVelocity;
        this.bodyPosition += this.bodyVelocity;
        this.legPosition += this.legVelocity;
        this.headRotation += this.headVelocity.X * 0.1f;
        this.bodyRotation += this.bodyVelocity.X * 0.1f;
        this.legRotation += this.legVelocity.X * 0.1f;
        this.headVelocity.Y += 0.1f;
        this.bodyVelocity.Y += 0.1f;
        this.legVelocity.Y += 0.1f;
        this.headVelocity.X *= 0.99f;
        this.bodyVelocity.X *= 0.99f;
        this.legVelocity.X *= 0.99f;
        if (this.respawnTimer > 0 || Game1.myPlayer != this.whoAmi)
          return;
        this.Spawn();
      }
      else
      {
        if (i == Game1.myPlayer)
        {
          this.zoneEvil = false;
          if (Game1.evilTiles >= 500)
            this.zoneEvil = true;
          this.zoneMeteor = false;
          if (Game1.meteorTiles >= 50)
            this.zoneMeteor = true;
          this.zoneDungeon = false;
          if (Game1.dungeonTiles >= 250 && (double) this.position.Y > Game1.worldSurface * 16.0 + (double) Game1.screenHeight)
            this.zoneDungeon = true;
          this.zoneJungle = false;
          if (Game1.jungleTiles >= 200)
            this.zoneJungle = true;
          this.controlUp = false;
          this.controlLeft = false;
          this.controlDown = false;
          this.controlRight = false;
          this.controlJump = false;
          this.controlUseItem = false;
          this.controlUseTile = false;
          if (Game1.hasFocus)
          {
            if (!Game1.chatMode && !Game1.editSign)
            {
              if (Game1.keyState.IsKeyDown(/*Keys.W*/Keys.Up))
                this.controlUp = true;

              if (Game1.keyState.IsKeyDown(/*Keys.A*/Keys.Left))
                this.controlLeft = true;

              if (Game1.keyState.IsKeyDown(/*Keys.S*/Keys.Down))
                this.controlDown = true;

              if (Game1.keyState.IsKeyDown(/*Keys.D*/Keys.Right))
                this.controlRight = true;

              if (Game1.keyState.IsKeyDown(Keys.Space))
                this.controlJump = true;
            }
            if (Game1.mouseState.Count == 1//Game1.mouseState.LeftButton == ButtonState.Pressed 
            && !this.mouseInterface)
              this.controlUseItem = true;

            if (Game1.mouseState.Count > 1//Game1.mouseState.RightButton == ButtonState.Pressed 
            && !this.mouseInterface)
              this.controlUseTile = true;

            // 4-finger touch gives Escape ! =)
            if (Game1.keyState.IsKeyDown(Keys.Escape) || Game1.mouseState.Count > 3)
            {
              if (this.releaseInventory)
              {
                if (this.talkNPC >= 0)
                {
                  this.talkNPC = -1;
                  Game1.npcChatText = "";
                  Game1.PlaySound(11);
                }
                else if (this.sign >= 0)
                {
                  this.sign = -1;
                  Game1.editSign = false;
                  Game1.npcChatText = "";
                  Game1.PlaySound(11);
                }
                else if (!Game1.playerInventory)
                {
                  Recipe.FindRecipes();
                  Game1.playerInventory = true;
                  Game1.PlaySound(10);
                }
                else
                {
                  Game1.playerInventory = false;
                  Game1.PlaySound(11);
                }
              }
              this.releaseInventory = false;
            }
            else
              this.releaseInventory = true;
            if (this.delayUseItem)
            {
              if (!this.controlUseItem)
                this.delayUseItem = false;
              this.controlUseItem = false;
            }
            if (this.itemAnimation == 0 && this.itemTime == 0)
            {
              if (Game1.keyState.IsKeyDown(Keys.Q) && this.inventory[this.selectedItem].type > 0)
              {
                int num7 = Game1.chatMode ? 1 : 0;
              }

              if ((Game1.mouseState.Count == 1 //Game1.mouseState.LeftButton == ButtonState.Pressed 
                                && !this.mouseInterface && Game1.mouseLeftRelease
                                || !Game1.playerInventory ? (Game1.mouseItem.type > 0 ? 1 : 0) : 0) != 0)
              {
                Item obj = new Item();
                bool flag = false;

                if ((Game1.mouseState.Count == 1 //Game1.mouseState.LeftButton == ButtonState.Pressed 
                                    && !this.mouseInterface && Game1.mouseLeftRelease 
                                    || !Game1.playerInventory) && Game1.mouseItem.type > 0)
                {
                  obj = this.inventory[this.selectedItem];
                  this.inventory[this.selectedItem] = Game1.mouseItem;
                  this.delayUseItem = true;
                  this.controlUseItem = false;
                  flag = true;
                }
                int number = Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, this.inventory[this.selectedItem].type);
                if (!flag && this.inventory[this.selectedItem].type == 8 && this.inventory[this.selectedItem].stack > 1)
                {
                  --this.inventory[this.selectedItem].stack;
                }
                else
                {
                  this.inventory[this.selectedItem].position = Game1.item[number].position;
                  Game1.item[number] = this.inventory[this.selectedItem];
                  this.inventory[this.selectedItem] = new Item();
                }
                if (Game1.netMode == 0)
                  Game1.item[number].noGrabDelay = 100;
                Game1.item[number].velocity.Y = -2f;
                Game1.item[number].velocity.X = (float) (4 * this.direction) + this.velocity.X;

                if ((Game1.mouseState.Count == 1 //Game1.mouseState.LeftButton == ButtonState.Pressed 
                    && !this.mouseInterface || !Game1.playerInventory) && Game1.mouseItem.type > 0)
                {
                  this.inventory[this.selectedItem] = obj;
                  Game1.mouseItem = new Item();
                }
                else
                {
                  this.itemAnimation = 10;
                  this.itemAnimationMax = 10;
                }
                Recipe.FindRecipes();
                if (Game1.netMode == 1)
                  NetMessage.SendData(21, number: number);
              }
              if (!Game1.playerInventory)
              {
                int selectedItem = this.selectedItem;
                if (Game1.keyState.IsKeyDown(Keys.D1))
                  this.selectedItem = 0;
                if (Game1.keyState.IsKeyDown(Keys.D2))
                  this.selectedItem = 1;
                if (Game1.keyState.IsKeyDown(Keys.D3))
                  this.selectedItem = 2;
                if (Game1.keyState.IsKeyDown(Keys.D4))
                  this.selectedItem = 3;
                if (Game1.keyState.IsKeyDown(Keys.D5))
                  this.selectedItem = 4;
                if (Game1.keyState.IsKeyDown(Keys.D6))
                  this.selectedItem = 5;
                if (Game1.keyState.IsKeyDown(Keys.D7))
                  this.selectedItem = 6;
                if (Game1.keyState.IsKeyDown(Keys.D8))
                  this.selectedItem = 7;
                if (Game1.keyState.IsKeyDown(Keys.D9))
                  this.selectedItem = 8;
                if (Game1.keyState.IsKeyDown(Keys.D0))
                  this.selectedItem = 9;
                if (selectedItem != this.selectedItem)
                  Game1.PlaySound(12);

                //RnD: Scroll wheel
                int num8 = 0;//(Game1.mouseState.ScrollWheelValue - Game1.oldMouseState.ScrollWheelValue) / 120;

                while (num8 > 9)
                  num8 -= 10;
                while (num8 < 0)
                  num8 += 10;
                this.selectedItem -= num8;
                if (num8 != 0)
                  Game1.PlaySound(12);
                if (this.changeItem >= 0)
                {
                  if (this.selectedItem != this.changeItem)
                    Game1.PlaySound(12);
                  this.selectedItem = this.changeItem;
                  this.changeItem = -1;
                }
                while (this.selectedItem > 9)
                  this.selectedItem -= 10;
                while (this.selectedItem < 0)
                  this.selectedItem += 10;
              }
              else
              {
                int num9 = 0;//(Game1.mouseState.ScrollWheelValue - Game1.oldMouseState.ScrollWheelValue) / 120;
                Game1.focusRecipe += num9;

                if (Game1.focusRecipe > Game1.numAvailableRecipes - 1)
                  Game1.focusRecipe = Game1.numAvailableRecipes - 1;
                if (Game1.focusRecipe < 0)
                  Game1.focusRecipe = 0;
              }
            }
          }
          if (Game1.netMode == 1)
          {
            bool flag = false;
            if (this.statLife != Game1.clientPlayer.statLife || this.statLifeMax != Game1.clientPlayer.statLifeMax)
            {
              NetMessage.SendData(16, number: Game1.myPlayer);
              flag = true;
            }
            if (this.statMana != Game1.clientPlayer.statMana || this.statManaMax != Game1.clientPlayer.statManaMax)
            {
              NetMessage.SendData(42, number: Game1.myPlayer);
              flag = true;
            }
            if (this.controlUp != Game1.clientPlayer.controlUp)
              flag = true;
            if (this.controlDown != Game1.clientPlayer.controlDown)
              flag = true;
            if (this.controlLeft != Game1.clientPlayer.controlLeft)
              flag = true;
            if (this.controlRight != Game1.clientPlayer.controlRight)
              flag = true;
            if (this.controlJump != Game1.clientPlayer.controlJump)
              flag = true;
            if (this.controlUseItem != Game1.clientPlayer.controlUseItem)
              flag = true;
            if (this.selectedItem != Game1.clientPlayer.selectedItem)
              flag = true;
            if (flag)
              NetMessage.SendData(13, number: Game1.myPlayer);
          }
          if (Game1.playerInventory)
            this.AdjTiles();
          if (this.chest != -1)
          {
            int num10 = (int) (((double) this.position.X + (double) this.width * 0.5) / 16.0);
            int num11 = (int) (((double) this.position.Y + (double) this.height * 0.5) / 16.0);
            if (num10 < this.chestX - 5 || num10 > this.chestX + 6 || num11 < this.chestY - 4 || num11 > this.chestY + 5)
            {
              if (this.chest != -1)
                Game1.PlaySound(11);
              this.chest = -1;
            }
            if (!Game1.tile[this.chestX, this.chestY].active)
            {
              Game1.PlaySound(11);
              this.chest = -1;
            }
          }
          if ((double) this.velocity.Y == 0.0)
          {
            int num12 = (int) ((double) this.position.Y / 16.0) - this.fallStart;
            if (num12 > 25 && !this.noFallDmg)
            {
              int Damage = (num12 - 25) * 10;
              this.immune = false;
              this.Hurt(Damage, -this.direction);
            }
          }
          if ((double) this.velocity.Y <= 0.0 || this.rocketDelay > 0 || this.wet)
            this.fallStart = (int) ((double) this.position.Y / 16.0);
        }
        if (this.mouseInterface)
          this.delayUseItem = true;

        Player.tileTargetX = (int) (((double) Game1.mouseState[0].Position.X 
                    + (double) Game1.screenPosition.X) / 16.0);

        Player.tileTargetY = (int) (((double) Game1.mouseState[0].Position.Y 
                    + (double) Game1.screenPosition.Y) / 16.0);
        if (this.immune)
        {
          --this.immuneTime;
          if (this.immuneTime <= 0)
            this.immune = false;
          this.immuneAlpha += this.immuneAlphaDirection * 50;
          if (this.immuneAlpha <= 50)
            this.immuneAlphaDirection = 1;
          else if (this.immuneAlpha >= 205)
            this.immuneAlphaDirection = -1;
        }
        else
          this.immuneAlpha = 0;
        if (this.manaRegenDelay > 0)
          --this.manaRegenDelay;
        this.statDefense = 0;
        this.accWatch = 0;
        this.accDepthMeter = 0;
        this.lifeRegen = 0;
        this.manaCost = 1f;
        this.boneArmor = false;
        this.rocketBoots = false;
        this.fireWalk = false;
        this.noKnockback = false;
        this.jumpBoost = false;
        this.noFallDmg = false;
        this.accFlipper = false;
        this.manaRegen = this.manaRegenDelay != 0 ? 0 : this.statManaMax / 30 + 1;
        this.doubleJump = false;
        for (int index = 0; index < 8; ++index)
        {
          this.statDefense += this.armor[index].defense;
          this.lifeRegen += this.armor[index].lifeRegen;
          this.manaRegen += this.armor[index].manaRegen;
          if (this.armor[index].type == 193)
            this.fireWalk = true;
        }
        this.head = this.armor[0].headSlot;
        this.body = this.armor[1].bodySlot;
        this.legs = this.armor[2].legSlot;
        for (int index = 3; index < 8; ++index)
        {
          if (this.armor[index].type == 15 && this.accWatch < 1)
            this.accWatch = 1;
          if (this.armor[index].type == 16 && this.accWatch < 2)
            this.accWatch = 2;
          if (this.armor[index].type == 17 && this.accWatch < 3)
            this.accWatch = 3;
          if (this.armor[index].type == 18 && this.accDepthMeter < 1)
            this.accDepthMeter = 1;
          if (this.armor[index].type == 53)
            this.doubleJump = true;
          if (this.armor[index].type == 54)
            num6 = 6f;
          if (this.armor[index].type == 128)
            this.rocketBoots = true;
          if (this.armor[index].type == 156)
            this.noKnockback = true;
          if (this.armor[index].type == 158)
            this.noFallDmg = true;
          if (this.armor[index].type == 159)
            this.jumpBoost = true;
          if (this.armor[index].type == 187)
            this.accFlipper = true;
        }
        this.lifeRegenCount += this.lifeRegen;
        while (this.lifeRegenCount >= 120)
        {
          this.lifeRegenCount -= 120;
          if (this.statLife < this.statLifeMax)
            ++this.statLife;
          if (this.statLife > this.statLifeMax)
            this.statLife = this.statLifeMax;
        }
        this.manaRegenCount += this.manaRegen;
        while (this.manaRegenCount >= 120)
        {
          this.manaRegenCount -= 120;
          if (this.statMana < this.statManaMax)
            ++this.statMana;
          if (this.statMana > this.statManaMax)
            this.statMana = this.statManaMax;
        }
        if (this.head == 1)
          Lighting.addLight((int) ((double) this.position.X + (double) (this.width / 2) + (double) (8 * this.direction)) / 16, (int) ((double) this.position.Y + 2.0) / 16, 0.8f);
        if (this.jumpBoost)
        {
          Player.jumpHeight = 20;
          Player.jumpSpeed = 6.51f;
        }
        this.setBonus = "";
        if (this.head == 2 && this.body == 0 && this.legs == 0 || this.head == 3 && this.body == 1 && this.legs == 1)
        {
          this.setBonus = "1 defense";
          ++this.statDefense;
        }
        if (this.head == 5 && this.body == 3 && this.legs == 3 || this.head == 4 && this.body == 2 && this.legs == 2)
        {
          this.setBonus = "2 defense";
          this.statDefense += 2;
        }
        if (this.head == 6 && this.body == 4 && this.legs == 4)
        {
          this.setBonus = "3 defense";
          this.statDefense += 3;
          if (Game1.rand.Next(10) == 0)
            Dust.NewDust(new Vector2(this.position.X, this.position.Y), this.width, this.height, 14, Alpha: 200, Scale: 1.2f);
        }
        if (this.head == 7 && this.body == 5 && this.legs == 5)
        {
          this.setBonus = "25% reduced mana usage";
          this.manaCost *= 0.75f;
          if ((double) Math.Abs(this.velocity.X) + (double) Math.Abs(this.velocity.Y) > 1.0 && !this.rocketFrame)
          {
            for (int index1 = 0; index1 < 2; ++index1)
            {
              int index2 = Dust.NewDust(new Vector2(this.position.X - this.velocity.X * 2f, (float) ((double) this.position.Y - 2.0 - (double) this.velocity.Y * 2.0)), this.width, this.height, 6, Alpha: 100, Scale: 2f);
              Game1.dust[index2].noGravity = true;
              Game1.dust[index2].velocity.X -= this.velocity.X * 0.5f;
              Game1.dust[index2].velocity.Y -= this.velocity.Y * 0.5f;
            }
          }
        }
        if (this.head == 8 && this.body == 6 && this.legs == 6)
        {
          num4 *= 1.35f;
          num3 *= 1.35f;
          this.setBonus = "35% increased movement speed";
          this.boneArmor = true;
        }
        if (!this.doubleJump)
          this.jumpAgain = false;
        else if ((double) this.velocity.Y == 0.0)
          this.jumpAgain = true;
        if (this.grappling[0] == -1)
        {
          if (this.controlLeft && (double) this.velocity.X > -(double) num3)
          {
            if ((double) this.velocity.X > (double) num5)
              this.velocity.X -= num5;
            this.velocity.X -= num4;
            if (this.itemAnimation == 0 || this.inventory[this.selectedItem].useTurn)
              this.direction = -1;
          }
          else if (this.controlRight && (double) this.velocity.X < (double) num3)
          {
            if ((double) this.velocity.X < -(double) num5)
              this.velocity.X += num5;
            this.velocity.X += num4;
            if (this.itemAnimation == 0 || this.inventory[this.selectedItem].useTurn)
              this.direction = 1;
          }
          else if (this.controlLeft && (double) this.velocity.X > -(double) num6)
          {
            if (this.itemAnimation == 0 || this.inventory[this.selectedItem].useTurn)
              this.direction = -1;
            if ((double) this.velocity.Y == 0.0)
            {
              if ((double) this.velocity.X > (double) num5)
                this.velocity.X -= num5;
              this.velocity.X -= num4 * 0.2f;
            }
            if ((double) this.velocity.X < -((double) num6 + (double) num3) / 2.0 && (double) this.velocity.Y == 0.0)
            {
              if (this.runSoundDelay == 0 && (double) this.velocity.Y == 0.0)
              {
                Game1.PlaySound(17, (int) this.position.X, (int) this.position.Y);
                this.runSoundDelay = 9;
              }
              int index = Dust.NewDust(new Vector2(this.position.X - 4f, this.position.Y + (float) this.height), this.width + 8, 4, 16, (float) (-(double) this.velocity.X * 0.5), this.velocity.Y * 0.5f, 50, Scale: 1.5f);
              Game1.dust[index].velocity.X *= 0.2f;
              Game1.dust[index].velocity.Y *= 0.2f;
            }
          }
          else if (this.controlRight && (double) this.velocity.X < (double) num6)
          {
            if (this.itemAnimation == 0 || this.inventory[this.selectedItem].useTurn)
              this.direction = 1;
            if ((double) this.velocity.Y == 0.0)
            {
              if ((double) this.velocity.X < -(double) num5)
                this.velocity.X += num5;
              this.velocity.X += num4 * 0.2f;
            }
            if ((double) this.velocity.X > ((double) num6 + (double) num3) / 2.0 && (double) this.velocity.Y == 0.0)
            {
              if (this.runSoundDelay == 0 && (double) this.velocity.Y == 0.0)
              {
                Game1.PlaySound(17, (int) this.position.X, (int) this.position.Y);
                this.runSoundDelay = 9;
              }
              int index = Dust.NewDust(new Vector2(this.position.X - 4f, this.position.Y + (float) this.height), this.width + 8, 4, 16, (float) (-(double) this.velocity.X * 0.5), this.velocity.Y * 0.5f, 50, Scale: 1.5f);
              Game1.dust[index].velocity.X *= 0.2f;
              Game1.dust[index].velocity.Y *= 0.2f;
            }
          }
          else if ((double) this.velocity.Y == 0.0)
          {
            if ((double) this.velocity.X > (double) num5)
              this.velocity.X -= num5;
            else if ((double) this.velocity.X < -(double) num5)
              this.velocity.X += num5;
            else
              this.velocity.X = 0.0f;
          }
          else if ((double) this.velocity.X > (double) num5 * 0.5)
            this.velocity.X -= num5 * 0.5f;
          else if ((double) this.velocity.X < -(double) num5 * 0.5)
            this.velocity.X += num5 * 0.5f;
          else
            this.velocity.X = 0.0f;
          if (this.controlJump)
          {
            if (this.jump > 0)
            {
              if ((double) this.velocity.Y > -(double) Player.jumpSpeed + (double) num2 * 2.0)
              {
                this.jump = 0;
              }
              else
              {
                this.velocity.Y = -Player.jumpSpeed;
                --this.jump;
              }
            }
            else if (((double) this.velocity.Y == 0.0 || this.jumpAgain || this.wet && this.accFlipper) && this.releaseJump)
            {
              bool flag = false;
              if (this.wet && this.accFlipper)
              {
                if (this.swimTime == 0)
                  this.swimTime = 30;
                flag = true;
              }
              this.jumpAgain = false;
              this.canRocket = false;
              this.rocketRelease = false;
              if ((double) this.velocity.Y == 0.0 && this.doubleJump)
                this.jumpAgain = true;
              if ((double) this.velocity.Y == 0.0 || flag)
              {
                this.velocity.Y = -Player.jumpSpeed;
                this.jump = Player.jumpHeight;
              }
              else
              {
                Game1.PlaySound(16, (int) this.position.X, (int) this.position.Y);
                this.velocity.Y = -Player.jumpSpeed;
                this.jump = Player.jumpHeight / 2;
                for (int index3 = 0; index3 < 10; ++index3)
                {
                  int index4 = Dust.NewDust(new Vector2(this.position.X - 34f, (float) ((double) this.position.Y + (double) this.height - 16.0)), 102, 32, 16, (float) (-(double) this.velocity.X * 0.5), this.velocity.Y * 0.5f, 100, Scale: 1.5f);
                  Game1.dust[index4].velocity.X = (float) ((double) Game1.dust[index4].velocity.X * 0.5 - (double) this.velocity.X * 0.10000000149011612);
                  Game1.dust[index4].velocity.Y = (float) ((double) Game1.dust[index4].velocity.Y * 0.5 - (double) this.velocity.Y * 0.30000001192092896);
                }
                int index5 = Gore.NewGore(new Vector2((float) ((double) this.position.X + (double) (this.width / 2) - 16.0), (float) ((double) this.position.Y + (double) this.height - 16.0)), new Vector2(-this.velocity.X, -this.velocity.Y), Game1.rand.Next(11, 14));
                Game1.gore[index5].velocity.X = (float) ((double) Game1.gore[index5].velocity.X * 0.10000000149011612 - (double) this.velocity.X * 0.10000000149011612);
                Game1.gore[index5].velocity.Y = (float) ((double) Game1.gore[index5].velocity.Y * 0.10000000149011612 - (double) this.velocity.Y * 0.05000000074505806);
                int index6 = Gore.NewGore(new Vector2(this.position.X - 36f, (float) ((double) this.position.Y + (double) this.height - 16.0)), new Vector2(-this.velocity.X, -this.velocity.Y), Game1.rand.Next(11, 14));
                Game1.gore[index6].velocity.X = (float) ((double) Game1.gore[index6].velocity.X * 0.10000000149011612 - (double) this.velocity.X * 0.10000000149011612);
                Game1.gore[index6].velocity.Y = (float) ((double) Game1.gore[index6].velocity.Y * 0.10000000149011612 - (double) this.velocity.Y * 0.05000000074505806);
                int index7 = Gore.NewGore(new Vector2((float) ((double) this.position.X + (double) this.width + 4.0), (float) ((double) this.position.Y + (double) this.height - 16.0)), new Vector2(-this.velocity.X, -this.velocity.Y), Game1.rand.Next(11, 14));
                Game1.gore[index7].velocity.X = (float) ((double) Game1.gore[index7].velocity.X * 0.10000000149011612 - (double) this.velocity.X * 0.10000000149011612);
                Game1.gore[index7].velocity.Y = (float) ((double) Game1.gore[index7].velocity.Y * 0.10000000149011612 - (double) this.velocity.Y * 0.05000000074505806);
              }
            }
            this.releaseJump = false;
          }
          else
          {
            this.jump = 0;
            this.releaseJump = true;
            this.rocketRelease = true;
          }
          if (this.doubleJump && !this.jumpAgain && (double) this.velocity.Y < 0.0 && !this.rocketBoots)
          {
            int index = Dust.NewDust(new Vector2(this.position.X - 4f, this.position.Y + (float) this.height), this.width + 8, 4, 16, (float) (-(double) this.velocity.X * 0.5), this.velocity.Y * 0.5f, 100, Scale: 1.5f);
            Game1.dust[index].velocity.X = (float) ((double) Game1.dust[index].velocity.X * 0.5 - (double) this.velocity.X * 0.10000000149011612);
            Game1.dust[index].velocity.Y = (float) ((double) Game1.dust[index].velocity.Y * 0.5 - (double) this.velocity.Y * 0.30000001192092896);
          }
          if ((double) this.velocity.Y > -(double) Player.jumpSpeed && (double) this.velocity.Y != 0.0)
            this.canRocket = true;
          if (this.rocketBoots && this.controlJump && this.rocketDelay == 0 && this.canRocket && this.rocketRelease && !this.jumpAgain)
          {
            int num13 = 7;
            if (this.statMana >= (int) ((double) num13 * (double) this.manaCost))
            {
              this.manaRegenDelay = 180;
              this.statMana -= (int) ((double) num13 * (double) this.manaCost);
              this.rocketDelay = 10;
              if (this.rocketDelay2 <= 0)
              {
                Game1.PlaySound(2, (int) this.position.X, (int) this.position.Y, 13);
                this.rocketDelay2 = 30;
              }
            }
            else
              this.canRocket = false;
          }
          if (this.rocketDelay2 > 0)
            --this.rocketDelay2;
          if (this.rocketDelay == 0)
            this.rocketFrame = false;
          if (this.rocketDelay > 0)
          {
            this.rocketFrame = true;
            for (int index8 = 0; index8 < 2; ++index8)
            {
              if (index8 == 0)
              {
                int index9 = Dust.NewDust(new Vector2(this.position.X - 4f, (float) ((double) this.position.Y + (double) this.height - 10.0)), 8, 8, 6, Alpha: 100, Scale: 2.5f);
                Game1.dust[index9].noGravity = true;
                Game1.dust[index9].velocity.X = (float) ((double) Game1.dust[index9].velocity.X * 1.0 - 2.0 - (double) this.velocity.X * 0.30000001192092896);
                Game1.dust[index9].velocity.Y = (float) ((double) Game1.dust[index9].velocity.Y * 1.0 + 2.0 - (double) this.velocity.Y * 0.30000001192092896);
              }
              else
              {
                int index10 = Dust.NewDust(new Vector2((float) ((double) this.position.X + (double) this.width - 4.0), (float) ((double) this.position.Y + (double) this.height - 10.0)), 8, 8, 6, Alpha: 100, Scale: 2.5f);
                Game1.dust[index10].noGravity = true;
                Game1.dust[index10].velocity.X = (float) ((double) Game1.dust[index10].velocity.X * 1.0 + 2.0 - (double) this.velocity.X * 0.30000001192092896);
                Game1.dust[index10].velocity.Y = (float) ((double) Game1.dust[index10].velocity.Y * 1.0 + 2.0 - (double) this.velocity.Y * 0.30000001192092896);
              }
            }
            if (this.rocketDelay == 0)
              this.releaseJump = true;
            --this.rocketDelay;
            this.velocity.Y -= 0.1f;
            if ((double) this.velocity.Y > 0.0)
              this.velocity.Y -= 0.3f;
            if ((double) this.velocity.Y < -(double) Player.jumpSpeed)
              this.velocity.Y = -Player.jumpSpeed;
          }
          else
            this.velocity.Y += num2;
          if ((double) this.velocity.Y > (double) num1)
            this.velocity.Y = num1;
        }
        for (int number = 0; number < 200; ++number)
        {
          if (Game1.item[number].active && Game1.item[number].noGrabDelay == 0 && Game1.item[number].owner == i)
          {
            Rectangle rectangle = new Rectangle((int) this.position.X, (int) this.position.Y, this.width, this.height);
            if (rectangle.Intersects(new Rectangle((int) Game1.item[number].position.X, (int) Game1.item[number].position.Y, Game1.item[number].width, Game1.item[number].height)))
            {
              if (i == Game1.myPlayer && (this.inventory[this.selectedItem].type != 0 || this.itemAnimation <= 0))
              {
                if (Game1.item[number].type == 58)
                {
                  Game1.PlaySound(7, (int) this.position.X, (int) this.position.Y);
                  this.statLife += 20;
                  if (Game1.myPlayer == this.whoAmi)
                    this.HealEffect(20);
                  if (this.statLife > this.statLifeMax)
                    this.statLife = this.statLifeMax;
                  Game1.item[number] = new Item();
                  if (Game1.netMode == 1)
                    NetMessage.SendData(21, number: number);
                }
                else if (Game1.item[number].type == 184)
                {
                  Game1.PlaySound(7, (int) this.position.X, (int) this.position.Y);
                  this.statMana += 20;
                  if (Game1.myPlayer == this.whoAmi)
                    this.ManaEffect(20);
                  if (this.statMana > this.statManaMax)
                    this.statMana = this.statManaMax;
                  Game1.item[number] = new Item();
                  if (Game1.netMode == 1)
                    NetMessage.SendData(21, number: number);
                }
                else
                {
                  Game1.item[number] = this.GetItem(i, Game1.item[number]);
                  if (Game1.netMode == 1)
                    NetMessage.SendData(21, number: number);
                }
              }
            }
            else
            {
              rectangle = new Rectangle((int) this.position.X - Player.itemGrabRange, (int) this.position.Y - Player.itemGrabRange, this.width + Player.itemGrabRange * 2, this.height + Player.itemGrabRange * 2);
              if (rectangle.Intersects(new Rectangle((int) Game1.item[number].position.X, (int) Game1.item[number].position.Y, Game1.item[number].width, Game1.item[number].height)) && this.ItemSpace(Game1.item[number]))
              {
                Game1.item[number].beingGrabbed = true;
                if ((double) this.position.X + (double) this.width * 0.5 > (double) Game1.item[number].position.X + (double) Game1.item[number].width * 0.5)
                {
                  if ((double) Game1.item[number].velocity.X < (double) Player.itemGrabSpeedMax + (double) this.velocity.X)
                    Game1.item[number].velocity.X += Player.itemGrabSpeed;
                  if ((double) Game1.item[number].velocity.X < 0.0)
                    Game1.item[number].velocity.X += Player.itemGrabSpeed * 0.75f;
                }
                else
                {
                  if ((double) Game1.item[number].velocity.X > -(double) Player.itemGrabSpeedMax + (double) this.velocity.X)
                    Game1.item[number].velocity.X -= Player.itemGrabSpeed;
                  if ((double) Game1.item[number].velocity.X > 0.0)
                    Game1.item[number].velocity.X -= Player.itemGrabSpeed * 0.75f;
                }
                if ((double) this.position.Y + (double) this.height * 0.5 > (double) Game1.item[number].position.Y + (double) Game1.item[number].height * 0.5)
                {
                  if ((double) Game1.item[number].velocity.Y < (double) Player.itemGrabSpeedMax)
                    Game1.item[number].velocity.Y += Player.itemGrabSpeed;
                  if ((double) Game1.item[number].velocity.Y < 0.0)
                    Game1.item[number].velocity.Y += Player.itemGrabSpeed * 0.75f;
                }
                else
                {
                  if ((double) Game1.item[number].velocity.Y > -(double) Player.itemGrabSpeedMax)
                    Game1.item[number].velocity.Y -= Player.itemGrabSpeed;
                  if ((double) Game1.item[number].velocity.Y > 0.0)
                    Game1.item[number].velocity.Y -= Player.itemGrabSpeed * 0.75f;
                }
              }
            }
          }
        }
        if ((double) this.position.X / 16.0 - (double) Player.tileRangeX <= (double) Player.tileTargetX && ((double) this.position.X + (double) this.width) / 16.0 + (double) Player.tileRangeX - 1.0 >= (double) Player.tileTargetX && (double) this.position.Y / 16.0 - (double) Player.tileRangeY <= (double) Player.tileTargetY && ((double) this.position.Y + (double) this.height) / 16.0 + (double) Player.tileRangeY - 2.0 >= (double) Player.tileTargetY && Game1.tile[Player.tileTargetX, Player.tileTargetY].active)
        {
          if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 21)
          {
            this.showItemIcon = true;
            this.showItemIcon2 = 48;
          }
          if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 4)
          {
            this.showItemIcon = true;
            this.showItemIcon2 = 8;
          }
          if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 13)
          {
            this.showItemIcon = true;
            this.showItemIcon2 = Game1.tile[Player.tileTargetX, Player.tileTargetY].frameX != (short) 18 ? (Game1.tile[Player.tileTargetX, Player.tileTargetY].frameX != (short) 36 ? 31 : 110) : 28;
          }
          if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 29)
          {
            this.showItemIcon = true;
            this.showItemIcon2 = 87;
          }
          if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 33)
          {
            this.showItemIcon = true;
            this.showItemIcon2 = 105;
          }
          if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 49)
          {
            this.showItemIcon = true;
            this.showItemIcon2 = 148;
          }
          if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 50 && Game1.tile[Player.tileTargetX, Player.tileTargetY].frameX == (short) 90)
          {
            this.showItemIcon = true;
            this.showItemIcon2 = 165;
          }
          if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 55)
          {
            int num14 = (int) Game1.tile[Player.tileTargetX, Player.tileTargetY].frameX / 18;
            int num15 = (int) Game1.tile[Player.tileTargetX, Player.tileTargetY].frameY / 18;
            while (num14 > 1)
              num14 -= 2;
            int num16 = Player.tileTargetX - num14;
            int num17 = Player.tileTargetY - num15;
            Game1.signBubble = true;
            Game1.signX = num16 * 16 + 16;
            Game1.signY = num17 * 16;
          }
          if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 10 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 11)
          {
            this.showItemIcon = true;
            this.showItemIcon2 = 25;
          }
          if (this.controlUseTile)
          {
            if (this.releaseUseTile)
            {
              if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 4 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 13 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 33 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 49 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 50 && Game1.tile[Player.tileTargetX, Player.tileTargetY].frameX == (short) 90)
              {
                WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY);
                if (Game1.netMode == 1)
                  NetMessage.SendData(17, number2: (float) Player.tileTargetX, number3: (float) Player.tileTargetY);
              }
              else if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 55)
              {
                bool flag = true;
                if (this.sign >= 0 && Sign.ReadSign(Player.tileTargetX, Player.tileTargetY) == this.sign)
                {
                  this.sign = -1;
                  Game1.npcChatText = "";
                  Game1.editSign = false;
                  Game1.PlaySound(11);
                  flag = false;
                }
                if (flag)
                {
                  if (Game1.netMode == 0)
                  {
                    this.talkNPC = -1;
                    Game1.playerInventory = false;
                    Game1.editSign = false;
                    Game1.PlaySound(10);
                    int index = Sign.ReadSign(Player.tileTargetX, Player.tileTargetY);
                    this.sign = index;
                    Game1.npcChatText = Game1.sign[index].text;
                  }
                  else
                  {
                    int num18 = (int) Game1.tile[Player.tileTargetX, Player.tileTargetY].frameX / 18;
                    int num19 = (int) Game1.tile[Player.tileTargetX, Player.tileTargetY].frameY / 18;
                    while (num18 > 1)
                      num18 -= 2;
                    int number = Player.tileTargetX - num18;
                    int number2 = Player.tileTargetY - num19;
                    if (Game1.tile[number, number2].type == (byte) 55)
                      NetMessage.SendData(46, number: number, number2: (float) number2);
                  }
                }
              }
              else if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 10)
              {
                WorldGen.OpenDoor(Player.tileTargetX, Player.tileTargetY, this.direction);
                NetMessage.SendData(19, number2: (float) Player.tileTargetX, number3: (float) Player.tileTargetY, number4: (float) this.direction);
              }
              else if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 11)
              {
                if (WorldGen.CloseDoor(Player.tileTargetX, Player.tileTargetY))
                  NetMessage.SendData(19, number: 1, number2: (float) Player.tileTargetX, number3: (float) Player.tileTargetY, number4: (float) this.direction);
              }
              else if ((Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 21 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 29) && this.talkNPC == -1)
              {
                bool flag = false;
                int num20 = Player.tileTargetX - (int) Game1.tile[Player.tileTargetX, Player.tileTargetY].frameX / 18;
                int num21 = Player.tileTargetY - (int) Game1.tile[Player.tileTargetX, Player.tileTargetY].frameY / 18;
                if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 29)
                  flag = true;
                if (Game1.netMode == 1 && !flag)
                {
                  if (num20 == this.chestX && num21 == this.chestY && this.chest != -1)
                  {
                    this.chest = -1;
                    Game1.PlaySound(11);
                  }
                  else
                    NetMessage.SendData(31, number: num20, number2: (float) num21);
                }
                else
                {
                  int num22 = !flag ? Chest.FindChest(num20, num21) : -2;
                  if (num22 != -1)
                  {
                    if (num22 == this.chest)
                    {
                      this.chest = -1;
                      Game1.PlaySound(11);
                    }
                    else if (num22 != this.chest && this.chest == -1)
                    {
                      this.chest = num22;
                      Game1.playerInventory = true;
                      Game1.PlaySound(10);
                      this.chestX = num20;
                      this.chestY = num21;
                    }
                    else
                    {
                      this.chest = num22;
                      Game1.playerInventory = true;
                      Game1.PlaySound(12);
                      this.chestX = num20;
                      this.chestY = num21;
                    }
                  }
                }
              }
            }
            this.releaseUseTile = false;
          }
          else
            this.releaseUseTile = true;
        }
        if (Game1.myPlayer == this.whoAmi)
        {
          Rectangle rectangle1;
          if (this.talkNPC >= 0)
          {
            rectangle1 = new Rectangle((int) ((double) this.position.X + (double) (this.width / 2) - (double) (Player.tileRangeX * 16)), (int) ((double) this.position.Y + (double) (this.height / 2) - (double) (Player.tileRangeY * 16)), Player.tileRangeX * 16 * 2, Player.tileRangeY * 16 * 2);
            Rectangle rectangle2 = new Rectangle((int) Game1.npc[this.talkNPC].position.X, (int) Game1.npc[this.talkNPC].position.Y, Game1.npc[this.talkNPC].width, Game1.npc[this.talkNPC].height);
            if (!rectangle1.Intersects(rectangle2) || this.chest != -1 || !Game1.npc[this.talkNPC].active)
            {
              if (this.chest == -1)
                Game1.PlaySound(11);
              this.talkNPC = -1;
              Game1.npcChatText = "";
            }
          }
          if (this.sign >= 0)
          {
            rectangle1 = new Rectangle((int) ((double) this.position.X + (double) (this.width / 2) - (double) (Player.tileRangeX * 16)), (int) ((double) this.position.Y + (double) (this.height / 2) - (double) (Player.tileRangeY * 16)), Player.tileRangeX * 16 * 2, Player.tileRangeY * 16 * 2);
            Rectangle rectangle3 = new Rectangle(Game1.sign[this.sign].x * 16, Game1.sign[this.sign].y * 16, 32, 32);
            if (!rectangle1.Intersects(rectangle3))
            {
              Game1.PlaySound(11);
              this.sign = -1;
              Game1.editSign = false;
              Game1.npcChatText = "";
            }
          }
          if (Game1.editSign)
          {
            if (this.sign == -1)
            {
              Game1.editSign = false;
            }
            else
            {
              Game1.npcChatText = Game1.GetInputText(Game1.npcChatText);
              if (Game1.inputTextEnter)
              {
                byte[] bytes = new byte[1]{ (byte) 10 };
                Game1.npcChatText += Encoding.ASCII.GetString(bytes);
              }
            }
          }
          Rectangle rectangle4 = new Rectangle((int) this.position.X, (int) this.position.Y, this.width, this.height);
          for (int index = 0; index < 1000; ++index)
          {
            if (Game1.npc[index].active && !Game1.npc[index].friendly && rectangle4.Intersects(new Rectangle((int) Game1.npc[index].position.X, (int) Game1.npc[index].position.Y, Game1.npc[index].width, Game1.npc[index].height)))
            {
              int hitDirection = -1;
              if ((double) Game1.npc[index].position.X + (double) (Game1.npc[index].width / 2) < (double) this.position.X + (double) (this.width / 2))
                hitDirection = 1;
              this.Hurt(Game1.npc[index].damage, hitDirection);
            }
          }
          Vector2 vector2 = Collision.HurtTiles(this.position, this.velocity, this.width, this.height, this.fireWalk);
          if ((double) vector2.Y != 0.0)
            this.Hurt((int) vector2.Y, (int) vector2.X);
        }
        if (this.grappling[0] >= 0)
        {
          this.rocketDelay = 0;
          this.rocketFrame = false;
          this.canRocket = false;
          this.rocketRelease = false;
          this.fallStart = (int) ((double) this.position.Y / 16.0);
          float num23 = 0.0f;
          float num24 = 0.0f;
          for (int index = 0; index < this.grapCount; ++index)
          {
            num23 += Game1.projectile[this.grappling[index]].position.X + (float) (Game1.projectile[this.grappling[index]].width / 2);
            num24 += Game1.projectile[this.grappling[index]].position.Y + (float) (Game1.projectile[this.grappling[index]].height / 2);
          }
          float num25 = num23 / (float) this.grapCount;
          float num26 = num24 / (float) this.grapCount;
          Vector2 vector2 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
          float num27 = num25 - vector2.X;
          float num28 = num26 - vector2.Y;
          float num29 = (float) Math.Sqrt((double) num27 * (double) num27 + (double) num28 * (double) num28);
          float num30 = 11f;
          float num31 = (double) num29 <= (double) num30 ? 1f : num30 / num29;
          float num32 = num27 * num31;
          float num33 = num28 * num31;
          this.velocity.X = num32;
          this.velocity.Y = num33;
          if (this.itemAnimation == 0)
          {
            if ((double) this.velocity.X > 0.0)
              this.direction = 1;
            if ((double) this.velocity.X < 0.0)
              this.direction = -1;
          }
          if (this.controlJump)
          {
            if (this.releaseJump)
            {
              if ((double) this.velocity.Y == 0.0)
              {
                this.velocity.Y = -Player.jumpSpeed;
                this.jump = Player.jumpHeight / 2;
                this.releaseJump = false;
              }
              else
              {
                this.velocity.Y += 0.01f;
                this.releaseJump = false;
              }
              if (this.doubleJump)
                this.jumpAgain = true;
              this.grappling[0] = 0;
              this.grapCount = 0;
              for (int index = 0; index < 1000; ++index)
              {
                if (Game1.projectile[index].active && Game1.projectile[index].owner == i && Game1.projectile[index].aiStyle == 7)
                  Game1.projectile[index].Kill();
              }
            }
          }
          else
            this.releaseJump = true;
        }
        bool flag1 = Collision.DrownCollision(this.position, this.width, this.height);
        if (this.inventory[this.selectedItem].type == 186)
        {
          int index11 = (int) (((double) this.position.X + (double) (this.width / 2) + (double) (6 * this.direction)) / 16.0);
          int index12 = (int) (((double) this.position.Y - 44.0) / 16.0);
          if (Game1.tile[index11, index12].liquid < (byte) 128)
          {
            if (Game1.tile[index11, index12] == null)
              Game1.tile[index11, index12] = new Tile();
            if (!Game1.tile[index11, index12].active || !Game1.tileSolid[(int) Game1.tile[index11, index12].type] || Game1.tileSolidTop[(int) Game1.tile[index11, index12].type])
              flag1 = false;
          }
        }
        if (Game1.myPlayer == i)
        {
          if (flag1)
          {
            ++this.breathCD;
            int num34 = 5;
            if (this.inventory[this.selectedItem].type == 186)
              num34 = 10;
            if (this.breathCD >= num34)
            {
              this.breathCD = 0;
              --this.breath;
              if (this.breath <= 0)
              {
                this.breath = 0;
                this.statLife -= 4;
                if (this.statLife <= 0)
                {
                  this.statLife = 0;
                  this.KillMe(10.0, 0);
                }
              }
            }
          }
          else
          {
            this.breath += 3;
            if (this.breath > this.breathMax)
              this.breath = this.breathMax;
            this.breathCD = 0;
          }
        }
        if (flag1 && Game1.rand.Next(20) == 0)
        {
          if (this.inventory[this.selectedItem].type == 186)
            Dust.NewDust(new Vector2((float) ((double) this.position.X + (double) (10 * this.direction) + 4.0), this.position.Y - 54f), this.width - 8, 8, 34, Scale: 1.2f);
          else
            Dust.NewDust(new Vector2(this.position.X + (float) (12 * this.direction), this.position.Y + 4f), this.width - 8, 8, 34, Scale: 1.2f);
        }
        bool flag2 = Collision.LavaCollision(this.position, this.width, this.height);
        if (flag2)
        {
          if (Game1.myPlayer == i)
            this.Hurt(100, 0);
          this.lavaWet = true;
        }
        if (Collision.WetCollision(this.position, this.width, this.height))
        {
          if (!this.wet)
          {
            if (this.wetCount == (byte) 0)
            {
              this.wetCount = (byte) 10;
              if (!flag2)
              {
                for (int index13 = 0; index13 < 50; ++index13)
                {
                  int index14 = Dust.NewDust(new Vector2(this.position.X - 6f, (float) ((double) this.position.Y + (double) (this.height / 2) - 8.0)), this.width + 12, 24, 33);
                  Game1.dust[index14].velocity.Y -= 4f;
                  Game1.dust[index14].velocity.X *= 2.5f;
                  Game1.dust[index14].scale = 1.3f;
                  Game1.dust[index14].alpha = 100;
                  Game1.dust[index14].noGravity = true;
                }
                Game1.PlaySound(19, (int) this.position.X, (int) this.position.Y, 0);
              }
              else
              {
                for (int index15 = 0; index15 < 20; ++index15)
                {
                  int index16 = Dust.NewDust(new Vector2(this.position.X - 6f, (float) ((double) this.position.Y + (double) (this.height / 2) - 8.0)), this.width + 12, 24, 35);
                  Game1.dust[index16].velocity.Y -= 1.5f;
                  Game1.dust[index16].velocity.X *= 2.5f;
                  Game1.dust[index16].scale = 1.3f;
                  Game1.dust[index16].alpha = 100;
                  Game1.dust[index16].noGravity = true;
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
          if (this.jump > Player.jumpHeight / 5)
            this.jump = Player.jumpHeight / 5;
          if (this.wetCount == (byte) 0)
          {
            this.wetCount = (byte) 10;
            if (!this.lavaWet)
            {
              for (int index17 = 0; index17 < 50; ++index17)
              {
                int index18 = Dust.NewDust(new Vector2(this.position.X - 6f, this.position.Y + (float) (this.height / 2)), this.width + 12, 24, 33);
                Game1.dust[index18].velocity.Y -= 4f;
                Game1.dust[index18].velocity.X *= 2.5f;
                Game1.dust[index18].scale = 1.3f;
                Game1.dust[index18].alpha = 100;
                Game1.dust[index18].noGravity = true;
              }
              Game1.PlaySound(19, (int) this.position.X, (int) this.position.Y, 0);
            }
            else
            {
              for (int index19 = 0; index19 < 20; ++index19)
              {
                int index20 = Dust.NewDust(new Vector2(this.position.X - 6f, (float) ((double) this.position.Y + (double) (this.height / 2) - 8.0)), this.width + 12, 24, 35);
                Game1.dust[index20].velocity.Y -= 1.5f;
                Game1.dust[index20].velocity.X *= 2.5f;
                Game1.dust[index20].scale = 1.3f;
                Game1.dust[index20].alpha = 100;
                Game1.dust[index20].noGravity = true;
              }
              Game1.PlaySound(19, (int) this.position.X, (int) this.position.Y);
            }
          }
        }
        if (!this.wet)
          this.lavaWet = false;
        if (this.wetCount > (byte) 0)
          --this.wetCount;
        if (this.wet)
        {
          if (this.wet)
          {
            Vector2 velocity = this.velocity;
            this.velocity = Collision.TileCollision(this.position, this.velocity, this.width, this.height, this.controlDown);
            Vector2 vector2 = this.velocity * 0.5f;
            if ((double) this.velocity.X != (double) velocity.X)
              vector2.X = this.velocity.X;
            if ((double) this.velocity.Y != (double) velocity.Y)
              vector2.Y = this.velocity.Y;
            this.position += vector2;
          }
        }
        else
        {
          this.velocity = Collision.TileCollision(this.position, this.velocity, this.width, this.height, this.controlDown);
          this.position += this.velocity;
        }
        if ((double) this.position.X < (double) Game1.leftWorld + 336.0 + 16.0)
        {
          this.position.X = (float) ((double) Game1.leftWorld + 336.0 + 16.0);
          this.velocity.X = 0.0f;
        }
        if ((double) this.position.X + (double) this.width > (double) Game1.rightWorld - 336.0 - 32.0)
        {
          this.position.X = (float) ((double) Game1.rightWorld - 336.0 - 32.0) - (float) this.width;
          this.velocity.X = 0.0f;
        }
        if ((double) this.position.Y < (double) Game1.topWorld + 336.0 + 16.0)
        {
          this.position.Y = (float) ((double) Game1.topWorld + 336.0 + 16.0);
          this.velocity.Y = 0.0f;
        }
        if ((double) this.position.Y > (double) Game1.bottomWorld - 336.0 - 32.0 - (double) this.height)
        {
          this.position.Y = (float) ((double) Game1.bottomWorld - 336.0 - 32.0) - (float) this.height;
          this.velocity.Y = 0.0f;
        }
        this.ItemCheck(i);
        this.PlayerFrame();
        if (this.statLife > this.statLifeMax)
          this.statLife = this.statLifeMax;
        this.grappling[0] = -1;
        this.grapCount = 0;
      }
    }

    public bool SellItem(int price)
    {
      if (price <= 0)
        return false;
      Item[] objArray = new Item[44];
      for (int index = 0; index < 44; ++index)
      {
        objArray[index] = new Item();
        objArray[index] = (Item) this.inventory[index].Clone();
      }
      int num = price / 5;
      if (num < 1)
        num = 1;
      bool flag = false;
      while (num >= 1000000 && !flag)
      {
        int index = -1;
        for (int i = 43; i >= 0; --i)
        {
          if (index == -1 && (this.inventory[i].type == 0 || this.inventory[i].stack == 0))
            index = i;
          while (this.inventory[i].type == 74 && this.inventory[i].stack < this.inventory[i].maxStack && num >= 1000000)
          {
            ++this.inventory[i].stack;
            num -= 1000000;
            this.DoCoins(i);
            if (this.inventory[i].stack == 0 && index == -1)
              index = i;
          }
        }
        if (num >= 1000000)
        {
          if (index == -1)
          {
            flag = true;
          }
          else
          {
            this.inventory[index].SetDefaults(74);
            num -= 1000000;
          }
        }
      }
      while (num >= 10000 && !flag)
      {
        int index = -1;
        for (int i = 43; i >= 0; --i)
        {
          if (index == -1 && (this.inventory[i].type == 0 || this.inventory[i].stack == 0))
            index = i;
          while (this.inventory[i].type == 73 && this.inventory[i].stack < this.inventory[i].maxStack && num >= 10000)
          {
            ++this.inventory[i].stack;
            num -= 10000;
            this.DoCoins(i);
            if (this.inventory[i].stack == 0 && index == -1)
              index = i;
          }
        }
        if (num >= 10000)
        {
          if (index == -1)
          {
            flag = true;
          }
          else
          {
            this.inventory[index].SetDefaults(73);
            num -= 10000;
          }
        }
      }
      while (num >= 100 && !flag)
      {
        int index = -1;
        for (int i = 43; i >= 0; --i)
        {
          if (index == -1 && (this.inventory[i].type == 0 || this.inventory[i].stack == 0))
            index = i;
          while (this.inventory[i].type == 72 && this.inventory[i].stack < this.inventory[i].maxStack && num >= 100)
          {
            ++this.inventory[i].stack;
            num -= 100;
            this.DoCoins(i);
            if (this.inventory[i].stack == 0 && index == -1)
              index = i;
          }
        }
        if (num >= 100)
        {
          if (index == -1)
          {
            flag = true;
          }
          else
          {
            this.inventory[index].SetDefaults(72);
            num -= 100;
          }
        }
      }
      while (num >= 1 && !flag)
      {
        int index = -1;
        for (int i = 43; i >= 0; --i)
        {
          if (index == -1 && (this.inventory[i].type == 0 || this.inventory[i].stack == 0))
            index = i;
          while (this.inventory[i].type == 71 && this.inventory[i].stack < this.inventory[i].maxStack && num >= 1)
          {
            ++this.inventory[i].stack;
            --num;
            this.DoCoins(i);
            if (this.inventory[i].stack == 0 && index == -1)
              index = i;
          }
        }
        if (num >= 1)
        {
          if (index == -1)
          {
            flag = true;
          }
          else
          {
            this.inventory[index].SetDefaults(71);
            --num;
          }
        }
      }
      if (!flag)
        return true;
      for (int index = 0; index < 44; ++index)
        this.inventory[index] = (Item) objArray[index].Clone();
      return false;
    }

    public bool BuyItem(int price)
    {
      if (price == 0)
        return false;
      int num1 = 0;
      Item[] objArray = new Item[44];
      for (int index = 0; index < 44; ++index)
      {
        objArray[index] = new Item();
        objArray[index] = (Item) this.inventory[index].Clone();
        if (this.inventory[index].type == 71)
          num1 += this.inventory[index].stack;
        if (this.inventory[index].type == 72)
          num1 += this.inventory[index].stack * 100;
        if (this.inventory[index].type == 73)
          num1 += this.inventory[index].stack * 10000;
        if (this.inventory[index].type == 74)
          num1 += this.inventory[index].stack * 1000000;
      }
      if (num1 < price)
        return false;
      int num2 = price;
      while (num2 > 0)
      {
        if (num2 >= 1000000)
        {
          for (int index = 0; index < 44; ++index)
          {
            if (this.inventory[index].type == 74)
            {
              while (this.inventory[index].stack > 0 && num2 >= 1000000)
              {
                num2 -= 1000000;
                --this.inventory[index].stack;
                if (this.inventory[index].stack == 0)
                  this.inventory[index].type = 0;
              }
            }
          }
        }
        if (num2 >= 10000)
        {
          for (int index = 0; index < 44; ++index)
          {
            if (this.inventory[index].type == 73)
            {
              while (this.inventory[index].stack > 0 && num2 >= 10000)
              {
                num2 -= 10000;
                --this.inventory[index].stack;
                if (this.inventory[index].stack == 0)
                  this.inventory[index].type = 0;
              }
            }
          }
        }
        if (num2 >= 100)
        {
          for (int index = 0; index < 44; ++index)
          {
            if (this.inventory[index].type == 72)
            {
              while (this.inventory[index].stack > 0 && num2 >= 100)
              {
                num2 -= 100;
                --this.inventory[index].stack;
                if (this.inventory[index].stack == 0)
                  this.inventory[index].type = 0;
              }
            }
          }
        }
        if (num2 >= 1)
        {
          for (int index = 0; index < 44; ++index)
          {
            if (this.inventory[index].type == 71)
            {
              while (this.inventory[index].stack > 0 && num2 >= 1)
              {
                --num2;
                --this.inventory[index].stack;
                if (this.inventory[index].stack == 0)
                  this.inventory[index].type = 0;
              }
            }
          }
        }
        if (num2 > 0)
        {
          int index1 = -1;
          for (int index2 = 43; index2 >= 0; --index2)
          {
            if (this.inventory[index2].type == 0 || this.inventory[index2].stack == 0)
            {
              index1 = index2;
              break;
            }
          }
          if (index1 >= 0)
          {
            bool flag = true;
            if (num2 >= 10000)
            {
              for (int index3 = 0; index3 < 44; ++index3)
              {
                if (this.inventory[index3].type == 74 && this.inventory[index3].stack >= 1)
                {
                  --this.inventory[index3].stack;
                  if (this.inventory[index3].stack == 0)
                    this.inventory[index3].type = 0;
                  this.inventory[index1].SetDefaults(73);
                  this.inventory[index1].stack = 100;
                  flag = false;
                  break;
                }
              }
            }
            else if (num2 >= 100)
            {
              for (int index4 = 0; index4 < 44; ++index4)
              {
                if (this.inventory[index4].type == 73 && this.inventory[index4].stack >= 1)
                {
                  --this.inventory[index4].stack;
                  if (this.inventory[index4].stack == 0)
                    this.inventory[index4].type = 0;
                  this.inventory[index1].SetDefaults(72);
                  this.inventory[index1].stack = 100;
                  flag = false;
                  break;
                }
              }
            }
            else if (num2 >= 1)
            {
              for (int index5 = 0; index5 < 44; ++index5)
              {
                if (this.inventory[index5].type == 72 && this.inventory[index5].stack >= 1)
                {
                  --this.inventory[index5].stack;
                  if (this.inventory[index5].stack == 0)
                    this.inventory[index5].type = 0;
                  this.inventory[index1].SetDefaults(71);
                  this.inventory[index1].stack = 100;
                  flag = false;
                  break;
                }
              }
            }
            if (flag)
            {
              if (num2 < 10000)
              {
                for (int index6 = 0; index6 < 44; ++index6)
                {
                  if (this.inventory[index6].type == 73 && this.inventory[index6].stack >= 1)
                  {
                    --this.inventory[index6].stack;
                    if (this.inventory[index6].stack == 0)
                      this.inventory[index6].type = 0;
                    this.inventory[index1].SetDefaults(72);
                    this.inventory[index1].stack = 100;
                    flag = false;
                    break;
                  }
                }
              }
              if (flag && num2 < 1000000)
              {
                for (int index7 = 0; index7 < 44; ++index7)
                {
                  if (this.inventory[index7].type == 74 && this.inventory[index7].stack >= 1)
                  {
                    --this.inventory[index7].stack;
                    if (this.inventory[index7].stack == 0)
                      this.inventory[index7].type = 0;
                    this.inventory[index1].SetDefaults(73);
                    this.inventory[index1].stack = 100;
                    break;
                  }
                }
              }
            }
          }
          else
          {
            for (int index8 = 0; index8 < 44; ++index8)
              this.inventory[index8] = (Item) objArray[index8].Clone();
            return false;
          }
        }
      }
      return true;
    }

    public void AdjTiles()
    {
      int num1 = 4;
      int num2 = 3;
      for (int index = 0; index < 76; ++index)
      {
        this.oldAdjTile[index] = this.adjTile[index];
        this.adjTile[index] = false;
      }
      int num3 = (int) (((double) this.position.X + (double) (this.width / 2)) / 16.0);
      int num4 = (int) (((double) this.position.Y + (double) this.height) / 16.0);
      for (int index1 = num3 - num1; index1 <= num3 + num1; ++index1)
      {
        for (int index2 = num4 - num2; index2 < num4 + num2; ++index2)
        {
          if (Game1.tile[index1, index2].active)
            this.adjTile[(int) Game1.tile[index1, index2].type] = true;
        }
      }
      if (!Game1.playerInventory)
        return;
      bool flag = false;
      for (int index = 0; index < 76; ++index)
      {
        if (this.oldAdjTile[index] != this.adjTile[index])
        {
          flag = true;
          break;
        }
      }
      if (flag)
        Recipe.FindRecipes();
    }

    public void PlayerFrame()
    {
      if (this.swimTime > 0)
      {
        --this.swimTime;
        if (!this.wet)
          this.swimTime = 0;
      }
      this.head = this.armor[0].headSlot;
      this.body = this.armor[1].bodySlot;
      this.legs = this.armor[2].legSlot;
      this.hairFrame.Width = 32;
      this.hairFrame.Height = 48;
      this.headFrame.Width = 32;
      this.headFrame.Height = 48;
      this.bodyFrame.Width = 32;
      this.bodyFrame.Height = 48;
      this.legFrame.Width = 32;
      this.legFrame.Height = 48;
      this.hairFrame.X = 34 * this.hair;
      this.headFrame.X = 0;
      this.bodyFrame.X = 0;
      this.legFrame.X = 0;
      this.headFrame.Y = 0;
      if (this.itemAnimation > 0)
      {
        if (this.inventory[this.selectedItem].useStyle == 1 || this.inventory[this.selectedItem].type == 0)
          this.bodyFrame.Y = (double) this.itemAnimation >= (double) this.itemAnimationMax * 0.333 ? ((double) this.itemAnimation >= (double) this.itemAnimationMax * 0.666 ? 100 : 150) : 200;
        else if (this.inventory[this.selectedItem].useStyle == 2)
          this.bodyFrame.Y = (double) this.itemAnimation >= (double) this.itemAnimationMax * 0.5 ? 200 : 150;
        else if (this.inventory[this.selectedItem].useStyle == 3)
          this.bodyFrame.Y = (double) this.itemAnimation <= (double) this.itemAnimationMax * 0.666 ? 200 : 100;
        else if (this.inventory[this.selectedItem].useStyle == 4)
          this.bodyFrame.Y = 150;
        else if (this.inventory[this.selectedItem].useStyle == 5)
        {
          float num = this.itemRotation * (float) this.direction;
          this.bodyFrame.Y = 200;
          if ((double) num < -0.75)
            this.bodyFrame.Y = 150;
          if ((double) num > 0.6)
            this.bodyFrame.Y = 250;
        }
      }
      else if (this.inventory[this.selectedItem].holdStyle == 1)
        this.bodyFrame.Y = 200;
      else if (this.inventory[this.selectedItem].holdStyle == 2)
        this.bodyFrame.Y = 150;
      else if (this.grappling[0] >= 0)
      {
        Vector2 vector2 = new Vector2(this.position.X + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
        float num1 = 0.0f;
        float num2 = 0.0f;
        for (int index = 0; index < this.grapCount; ++index)
        {
          num1 += Game1.projectile[this.grappling[index]].position.X + (float) (Game1.projectile[this.grappling[index]].width / 2);
          num2 += Game1.projectile[this.grappling[index]].position.Y + (float) (Game1.projectile[this.grappling[index]].height / 2);
        }
        float num3 = num1 / (float) this.grapCount;
        float num4 = num2 / (float) this.grapCount;
        float num5 = num3 - vector2.X;
        float num6 = num4 - vector2.Y;
        this.bodyFrame.Y = (double) num6 >= 0.0 || (double) Math.Abs(num6) <= (double) Math.Abs(num5) ? ((double) num6 <= 0.0 || (double) Math.Abs(num6) <= (double) Math.Abs(num5) ? 200 : 250) : 150;
      }
      else
        this.bodyFrame.Y = this.swimTime <= 0 ? ((double) this.velocity.Y >= 0.0 || this.rocketFrame ? ((double) this.velocity.Y <= 0.0 || this.rocketFrame ? 0 : 50) : 50) : (this.swimTime <= 20 ? (this.swimTime <= 10 ? 0 : 50) : 0);
      if (this.swimTime > 0)
      {
        if (this.swimTime > 20)
          this.legFrame.Y = 50;
        else if (this.swimTime > 10)
          this.legFrame.Y = 0;
        else
          this.legFrame.Y = 50;
      }
      else if ((double) this.velocity.Y < 0.0 || this.grappling[0] >= 0)
        this.legFrame.Y = 100;
      else if ((double) this.velocity.Y > 0.0)
        this.legFrame.Y = 100;
      else if ((double) this.velocity.X != 0.0)
      {
        if ((this.direction >= 0 || (double)this.velocity.X <= 0.0) && (this.direction <= 0 || (double)this.velocity.X >= 0.0))
        { }

        this.legFrameCounter += 0.4 + Math.Abs((double) this.velocity.X * 0.4);
        if (this.legFrameCounter < 6.0)
          this.legFrame.Y = 0;
        else if (this.legFrameCounter < 12.0)
          this.legFrame.Y = 50;
        else if (this.legFrameCounter < 18.0)
          this.legFrame.Y = 100;
        else if (this.legFrameCounter < 24.0)
        {
          this.legFrame.Y = 50;
        }
        else
        {
          this.legFrame.Y = 0;
          this.legFrameCounter = 0.0;
        }
      }
      else
      {
        this.legFrameCounter = 6.0;
        this.legFrame.Y = 0;
      }
    }

    public void Spawn()
    {
      if (Game1.netMode == 1 && this.whoAmi == Game1.myPlayer)
      {
        NetMessage.SendData(12, number: Game1.myPlayer);
        Game1.gameMenu = false;
      }
      this.headPosition = new Vector2();
      this.bodyPosition = new Vector2();
      this.legPosition = new Vector2();
      this.headRotation = 0.0f;
      this.bodyRotation = 0.0f;
      this.legRotation = 0.0f;
      if (this.statLife <= 0)
      {
        this.statLife = 100;
        this.breath = this.breathMax;
      }
      this.immune = true;
      this.dead = false;
      this.immuneTime = 0;
      this.active = true;
      this.position.X = (float) (Game1.spawnTileX * 16 + 8 - this.width / 2);
      this.position.Y = (float) (Game1.spawnTileY * 16 - this.height);
      this.wet = Collision.WetCollision(this.position, this.width, this.height);
      this.wetCount = (byte) 0;
      this.lavaWet = false;
      this.fallStart = (int) ((double) this.position.Y / 16.0);
      this.velocity.X = 0.0f;
      this.velocity.Y = 0.0f;
      this.talkNPC = -1;
      if (this.pvpDeath)
      {
        this.pvpDeath = false;
        this.immuneTime = 300;
        this.statLife = this.statLifeMax;
      }
      for (int i = Game1.spawnTileX - 1; i < Game1.spawnTileX + 2; ++i)
      {
        for (int j = Game1.spawnTileY - 3; j < Game1.spawnTileY; ++j)
        {
          if (Game1.tileSolid[(int) Game1.tile[i, j].type])
          {
            Game1.tile[i, j].active = false;
            Game1.tile[i, j].type = (byte) 0;
            WorldGen.SquareTileFrame(i, j);
          }
        }
      }
      if (this.whoAmi != Game1.myPlayer)
        return;
      Lighting.lightCounter = Lighting.lightSkip + 1;
      Game1.screenPosition.X = this.position.X + (float) (this.width / 2) - (float) (Game1.screenWidth / 2);
      Game1.screenPosition.Y = this.position.Y + (float) (this.height / 2) - (float) (Game1.screenHeight / 2);
    }

    public double Hurt(int Damage, int hitDirection, bool pvp = false, bool quiet = false)
    {
      if (this.immune || Game1.godMode)
        return 0.0;
      int Damage1 = Damage;
      if (pvp)
        Damage1 *= 2;
      double damage = Game1.CalculateDamage(Damage1, this.statDefense);
      if (damage >= 1.0)
      {
        if (Game1.netMode == 1 && this.whoAmi == Game1.myPlayer && !quiet)
        {
          int number4 = 0;
          if (pvp)
            number4 = 1;
          NetMessage.SendData(13, number: this.whoAmi);
          NetMessage.SendData(16, number: this.whoAmi);
          NetMessage.SendData(26, number: this.whoAmi, number2: (float) hitDirection, number3: (float) Damage, number4: (float) number4);
        }
        CombatText.NewText(new Rectangle((int) this.position.X, (int) this.position.Y, this.width, this.height), new Color((int) byte.MaxValue, 80, 90, (int) byte.MaxValue), string.Concat((object) (int) damage));
        this.statLife -= (int) damage;
        this.immune = true;
        this.immuneTime = 40;
        if (pvp)
          this.immuneTime = 8;
        if (!this.noKnockback && hitDirection != 0)
        {
          this.velocity.X = 4.5f * (float) hitDirection;
          this.velocity.Y = -3.5f;
        }
        if (this.boneArmor)
          Game1.PlaySound(3, (int) this.position.X, (int) this.position.Y, 2);
        else
          Game1.PlaySound(1, (int) this.position.X, (int) this.position.Y);
        if (this.statLife > 0)
        {
          for (int index = 0; (double) index < damage / (double) this.statLifeMax * 100.0; ++index)
          {
            if (this.boneArmor)
              Dust.NewDust(this.position, this.width, this.height, 26, (float) (2 * hitDirection), -2f);
            else
              Dust.NewDust(this.position, this.width, this.height, 5, (float) (2 * hitDirection), -2f);
          }
        }
        else
        {
          this.statLife = 0;
          if (this.whoAmi == Game1.myPlayer)
            this.KillMe(damage, hitDirection, pvp);
        }
      }
      if (pvp)
        damage = Game1.CalculateDamage(Damage1, this.statDefense);
      return damage;
    }

    public void KillMe(double dmg, int hitDirection, bool pvp = false)
    {
      if (Game1.godMode && Game1.myPlayer == this.whoAmi || this.dead)
        return;
      if (pvp)
        this.pvpDeath = true;
      Game1.PlaySound(5, (int) this.position.X, (int) this.position.Y);
      this.headVelocity.Y = (float) Game1.rand.Next(-40, -10) * 0.1f;
      this.bodyVelocity.Y = (float) Game1.rand.Next(-40, -10) * 0.1f;
      this.legVelocity.Y = (float) Game1.rand.Next(-40, -10) * 0.1f;
      this.headVelocity.X = (float) Game1.rand.Next(-20, 21) * 0.1f + (float) (2 * hitDirection);
      this.bodyVelocity.X = (float) Game1.rand.Next(-20, 21) * 0.1f + (float) (2 * hitDirection);
      this.legVelocity.X = (float) Game1.rand.Next(-20, 21) * 0.1f + (float) (2 * hitDirection);
      for (int index = 0; (double) index < 20.0 + dmg / (double) this.statLifeMax * 100.0; ++index)
      {
        if (this.boneArmor)
          Dust.NewDust(this.position, this.width, this.height, 26, (float) (2 * hitDirection), -2f);
        else
          Dust.NewDust(this.position, this.width, this.height, 5, (float) (2 * hitDirection), -2f);
      }
      this.dead = true;
      this.respawnTimer = 600;
      this.immuneAlpha = 0;
      int num;
      switch (Game1.netMode)
      {
        case 1:
          num = this.whoAmi != Game1.myPlayer ? 1 : 0;
          break;
        case 2:
          NetMessage.SendData(25, text: this.name + " was slain...", number: 8, number2: 225f, number3: 25f, number4: 25f);
          return;
        default:
          num = 1;
          break;
      }
      if (num != 0)
        return;
      int number4 = 0;
      if (pvp)
        number4 = 1;
      NetMessage.SendData(44, number: this.whoAmi, number2: (float) hitDirection, number3: (float) (int) dmg, number4: (float) number4);
    }

    public bool ItemSpace(Item newItem)
    {
      if (newItem.type == 58 || newItem.type == 184)
        return true;
      int num = 40;
      if (newItem.type == 71 || newItem.type == 72 || newItem.type == 73 || newItem.type == 74)
        num = 44;
      for (int index = 0; index < num; ++index)
      {
        if (this.inventory[index].type == 0)
          return true;
      }
      for (int index = 0; index < num; ++index)
      {
        if (this.inventory[index].type > 0 && this.inventory[index].stack < this.inventory[index].maxStack && newItem.IsTheSameAs(this.inventory[index]))
          return true;
      }
      return false;
    }

    public void DoCoins(int i)
    {
      if (this.inventory[i].stack != 100 || this.inventory[i].type != 71 && this.inventory[i].type != 72 && this.inventory[i].type != 73)
        return;
      this.inventory[i].SetDefaults(this.inventory[i].type + 1);
      for (int i1 = 0; i1 < 44; ++i1)
      {
        if (this.inventory[i1].IsTheSameAs(this.inventory[i]) && i1 != i && this.inventory[i1].stack < this.inventory[i1].maxStack)
        {
          ++this.inventory[i1].stack;
          this.inventory[i].SetDefaults("");
          this.inventory[i].active = false;
          this.inventory[i].name = "";
          this.inventory[i].type = 0;
          this.inventory[i].stack = 0;
          this.DoCoins(i1);
        }
      }
    }

    public Item GetItem(int plr, Item newItem)
    {
      Item obj = newItem;
      if (newItem.noGrabDelay > 0)
        return obj;
      int num = 0;
      if (newItem.type == 71 || newItem.type == 72 || newItem.type == 73 || newItem.type == 74)
        num = -4;
      for (int index = num; index < 40; ++index)
      {
        int i = index;
        if (i < 0)
          i = 44 + index;
        if (this.inventory[i].type > 0 && this.inventory[i].stack < this.inventory[i].maxStack && obj.IsTheSameAs(this.inventory[i]))
        {
          Game1.PlaySound(7, (int) this.position.X, (int) this.position.Y);
          if (obj.stack + this.inventory[i].stack <= this.inventory[i].maxStack)
          {
            this.inventory[i].stack += obj.stack;
            this.DoCoins(i);
            if (plr == Game1.myPlayer)
              Recipe.FindRecipes();
            return new Item();
          }
          obj.stack -= this.inventory[i].maxStack - this.inventory[i].stack;
          this.inventory[i].stack = this.inventory[i].maxStack;
          this.DoCoins(i);
          if (plr == Game1.myPlayer)
            Recipe.FindRecipes();
        }
      }
      for (int index = num; index < 40; ++index)
      {
        int i = index;
        if (i < 0)
          i = 44 + index;
        if (this.inventory[i].type == 0)
        {
          this.inventory[i] = obj;
          this.DoCoins(i);
          Game1.PlaySound(7, (int) this.position.X, (int) this.position.Y);
          if (plr == Game1.myPlayer)
            Recipe.FindRecipes();
          return new Item();
        }
      }
      return obj;
    }

    public void ItemCheck(int i)
    {
      if (this.inventory[this.selectedItem].autoReuse)
      {
        this.releaseUseItem = true;
        if (this.itemAnimation == 1 && this.inventory[this.selectedItem].stack > 0)
          this.itemAnimation = 0;
      }
      if (this.controlUseItem && this.itemAnimation == 0 && this.releaseUseItem && this.inventory[this.selectedItem].useStyle > 0)
      {
        bool flag = true;
        if (this.inventory[this.selectedItem].shoot == 6 || this.inventory[this.selectedItem].shoot == 19 || this.inventory[this.selectedItem].shoot == 33)
        {
          for (int index = 0; index < 1000; ++index)
          {
            if (Game1.projectile[index].active && Game1.projectile[index].owner == Game1.myPlayer && Game1.projectile[index].type == this.inventory[this.selectedItem].shoot)
              flag = false;
          }
        }
        if (this.inventory[this.selectedItem].potion)
        {
          if (this.potionDelay <= 0)
            this.potionDelay = Item.potionDelay;
          else
            flag = false;
        }
        if (this.inventory[this.selectedItem].mana > 0)
        {
          if (this.statMana >= (int) ((double) this.inventory[this.selectedItem].mana * (double) this.manaCost))
            this.statMana -= (int) ((double) this.inventory[this.selectedItem].mana * (double) this.manaCost);
          else
            flag = false;
        }
        if (this.inventory[this.selectedItem].shoot == 17 && flag && i == Game1.myPlayer)
        {
          int index1 = (int) ((double) Game1.mouseState[0].Position.X 
                        + (double) Game1.screenPosition.X) / 16;
          int index2 = (int) ((double) Game1.mouseState[0].Position.Y 
                        + (double) Game1.screenPosition.Y) / 16;
          if (Game1.tile[index1, index2].active 
                        && (Game1.tile[index1, index2].type == (byte) 0 
                        || Game1.tile[index1, index2].type == (byte) 2 
                        || Game1.tile[index1, index2].type == (byte) 23))
          {
            WorldGen.KillTile(index1, index2, noItem: true);
            if (!Game1.tile[index1, index2].active)
            {
              if (Game1.netMode == 1)
                NetMessage.SendData(17, number: 4, number2: (float) index1, number3: (float) index2);
            }
            else
              flag = false;
          }
          else
            flag = false;
        }
        if (flag && this.inventory[this.selectedItem].useAmmo > 0)
        {
          flag = false;
          for (int index = 0; index < 44; ++index)
          {
            if (this.inventory[index].ammo == this.inventory[this.selectedItem].useAmmo && this.inventory[index].stack > 0)
            {
              flag = true;
              break;
            }
          }
        }
        if (flag)
        {
          if (this.grappling[0] > -1)
          {
            if (this.controlRight)
              this.direction = 1;
            else if (this.controlLeft)
              this.direction = -1;
          }
          this.channel = this.inventory[this.selectedItem].channel;
          this.attackCD = 0;
          this.itemAnimation = this.inventory[this.selectedItem].useAnimation;
          this.itemAnimationMax = this.itemAnimation;
          if (this.inventory[this.selectedItem].useSound > 0)
            Game1.PlaySound(2, (int) this.position.X, (int) this.position.Y, this.inventory[this.selectedItem].useSound);
        }
        if (flag && this.inventory[this.selectedItem].shoot == 18)
        {
          for (int index = 0; index < 1000; ++index)
          {
            if (Game1.projectile[index].active && Game1.projectile[index].owner == i && Game1.projectile[index].type == this.inventory[this.selectedItem].shoot)
              Game1.projectile[index].Kill();
          }
        }
      }
      if (!this.controlUseItem)
        this.channel = false;
      if (this.itemAnimation > 0)
      {
        if (this.inventory[this.selectedItem].mana > 0)
          this.manaRegenDelay = 180;
        this.itemHeight = Game1.itemTexture[this.inventory[this.selectedItem].type].Height;
        this.itemWidth = Game1.itemTexture[this.inventory[this.selectedItem].type].Width;
        --this.itemAnimation;
        if (this.inventory[this.selectedItem].useStyle == 1)
        {
          if ((double) this.itemAnimation < (double) this.inventory[this.selectedItem].useAnimation * 0.333)
          {
            float num = 4f;
            if (Game1.itemTexture[this.inventory[this.selectedItem].type].Width > 32)
              num = 14f;
            this.itemLocation.X = (float) ((double) this.position.X + (double) this.width * 0.5 + ((double) Game1.itemTexture[this.inventory[this.selectedItem].type].Width * 0.5 - (double) num) * (double) this.direction);
            this.itemLocation.Y = this.position.Y + 24f;
          }
          else if ((double) this.itemAnimation < (double) this.inventory[this.selectedItem].useAnimation * 0.666)
          {
            float num1 = 10f;
            if (Game1.itemTexture[this.inventory[this.selectedItem].type].Width > 32)
              num1 = 20f;
            this.itemLocation.X = (float) ((double) this.position.X + (double) this.width * 0.5 + ((double) Game1.itemTexture[this.inventory[this.selectedItem].type].Width * 0.5 - (double) num1) * (double) this.direction);
            float num2 = 10f;
            if (Game1.itemTexture[this.inventory[this.selectedItem].type].Height > 32)
              num2 = 14f;
            this.itemLocation.Y = this.position.Y + num2;
          }
          else
          {
            float num3 = 4f;
            if (Game1.itemTexture[this.inventory[this.selectedItem].type].Width > 32)
              num3 = 14f;
            this.itemLocation.X = (float) ((double) this.position.X + (double) this.width * 0.5 - ((double) Game1.itemTexture[this.inventory[this.selectedItem].type].Width * 0.5 - (double) num3) * (double) this.direction);
            float num4 = 6f;
            if (Game1.itemTexture[this.inventory[this.selectedItem].type].Height > 32)
              num4 = 10f;
            this.itemLocation.Y = this.position.Y + num4;
          }
          this.itemRotation = (float) (((double) this.itemAnimation / (double) this.inventory[this.selectedItem].useAnimation - 0.5) * (double) -this.direction * 3.5 - (double) this.direction * 0.30000001192092896);
        }
        else if (this.inventory[this.selectedItem].useStyle == 2)
        {
          this.itemRotation = (float) ((double) this.itemAnimation / (double) this.inventory[this.selectedItem].useAnimation * (double) this.direction * 2.0 + -1.3999999761581421 * (double) this.direction);
          if ((double) this.itemAnimation < (double) this.inventory[this.selectedItem].useAnimation * 0.5)
          {
            this.itemLocation.X = (float) ((double) this.position.X + (double) this.width * 0.5 + ((double) Game1.itemTexture[this.inventory[this.selectedItem].type].Width * 0.5 - 9.0 - (double) this.itemRotation * 12.0 * (double) this.direction) * (double) this.direction);
            this.itemLocation.Y = (float) ((double) this.position.Y + 38.0 + (double) this.itemRotation * (double) this.direction * 4.0);
          }
          else
          {
            this.itemLocation.X = (float) ((double) this.position.X + (double) this.width * 0.5 + ((double) Game1.itemTexture[this.inventory[this.selectedItem].type].Width * 0.5 - 9.0 - (double) this.itemRotation * 16.0 * (double) this.direction) * (double) this.direction);
            this.itemLocation.Y = (float) ((double) this.position.Y + 38.0 + (double) this.itemRotation * (double) this.direction);
          }
        }
        else if (this.inventory[this.selectedItem].useStyle == 3)
        {
          if ((double) this.itemAnimation > (double) this.inventory[this.selectedItem].useAnimation * 0.666)
          {
            this.itemLocation.X = -1000f;
            this.itemLocation.Y = -1000f;
            this.itemRotation = -1.3f * (float) this.direction;
          }
          else
          {
            this.itemLocation.X = (float) ((double) this.position.X + (double) this.width * 0.5 + ((double) Game1.itemTexture[this.inventory[this.selectedItem].type].Width * 0.5 - 4.0) * (double) this.direction);
            this.itemLocation.Y = this.position.Y + 24f;
            float num = (float) ((double) this.itemAnimation / (double) this.inventory[this.selectedItem].useAnimation * (double) Game1.itemTexture[this.inventory[this.selectedItem].type].Width * (double) this.direction * (double) this.inventory[this.selectedItem].scale * 1.2000000476837158) - (float) (10 * this.direction);
            if ((double) num > -4.0 && this.direction == -1)
              num = -4f;
            if ((double) num < 4.0 && this.direction == 1)
              num = 4f;
            this.itemLocation.X -= num;
            this.itemRotation = 0.8f * (float) this.direction;
          }
        }
        else if (this.inventory[this.selectedItem].useStyle == 4)
        {
          this.itemRotation = 0.0f;
          this.itemLocation.X = (float) ((double) this.position.X + (double) this.width * 0.5 + ((double) Game1.itemTexture[this.inventory[this.selectedItem].type].Width * 0.5 - 9.0 - (double) this.itemRotation * 14.0 * (double) this.direction) * (double) this.direction);
          this.itemLocation.Y = this.position.Y + (float) Game1.itemTexture[this.inventory[this.selectedItem].type].Height * 0.5f;
        }
        else if (this.inventory[this.selectedItem].useStyle == 5)
        {
          this.itemLocation.X = (float) ((double) this.position.X + (double) this.width * 0.5 - (double) Game1.itemTexture[this.inventory[this.selectedItem].type].Width * 0.5);
          this.itemLocation.Y = (float) ((double) this.position.Y + (double) this.height * 0.5 - (double) Game1.itemTexture[this.inventory[this.selectedItem].type].Height * 0.5);
        }
      }
      else if (this.inventory[this.selectedItem].holdStyle == 1)
      {
        this.itemLocation.X = (float) ((double) this.position.X + (double) this.width * 0.5 + ((double) Game1.itemTexture[this.inventory[this.selectedItem].type].Width * 0.5 + 4.0) * (double) this.direction);
        this.itemLocation.Y = this.position.Y + 24f;
        this.itemRotation = 0.0f;
      }
      else if (this.inventory[this.selectedItem].holdStyle == 2)
      {
        this.itemLocation.X = this.position.X + (float) this.width * 0.5f + (float) (6 * this.direction);
        this.itemLocation.Y = this.position.Y + 16f;
        this.itemRotation = 0.79f * (float) -this.direction;
      }
      if (this.inventory[this.selectedItem].type == 8)
      {
        int maxValue = 20;
        if (this.itemAnimation > 0)
          maxValue = 7;
        if (this.direction == -1)
        {
          if (Game1.rand.Next(maxValue) == 0)
            Dust.NewDust(new Vector2(this.itemLocation.X - 16f, this.itemLocation.Y - 14f), 4, 4, 6, Alpha: 100);
          Lighting.addLight((int) (((double) this.itemLocation.X - 16.0 + (double) this.velocity.X) / 16.0), (int) (((double) this.itemLocation.Y - 14.0) / 16.0), 1f);
        }
        else
        {
          if (Game1.rand.Next(maxValue) == 0)
            Dust.NewDust(new Vector2(this.itemLocation.X + 6f, this.itemLocation.Y - 14f), 4, 4, 6, Alpha: 100);
          Lighting.addLight((int) (((double) this.itemLocation.X + 6.0 + (double) this.velocity.X) / 16.0), (int) (((double) this.itemLocation.Y - 14.0) / 16.0), 1f);
        }
      }
      else if (this.inventory[this.selectedItem].type == 105)
      {
        int maxValue = 20;
        if (this.itemAnimation > 0)
          maxValue = 7;
        if (this.direction == -1)
        {
          if (Game1.rand.Next(maxValue) == 0)
            Dust.NewDust(new Vector2(this.itemLocation.X - 12f, this.itemLocation.Y - 20f), 4, 4, 6, Alpha: 100);
          Lighting.addLight((int) (((double) this.itemLocation.X - 16.0 + (double) this.velocity.X) / 16.0), (int) (((double) this.itemLocation.Y - 14.0) / 16.0), 1f);
        }
        else
        {
          if (Game1.rand.Next(maxValue) == 0)
            Dust.NewDust(new Vector2(this.itemLocation.X + 4f, this.itemLocation.Y - 20f), 4, 4, 6, Alpha: 100);
          Lighting.addLight((int) (((double) this.itemLocation.X + 6.0 + (double) this.velocity.X) / 16.0), (int) (((double) this.itemLocation.Y - 14.0) / 16.0), 1f);
        }
      }
      else if (this.inventory[this.selectedItem].type == 148)
      {
        int maxValue = 10;
        if (this.itemAnimation > 0)
          maxValue = 7;
        if (this.direction == -1)
        {
          if (Game1.rand.Next(maxValue) == 0)
            Dust.NewDust(new Vector2(this.itemLocation.X - 12f, this.itemLocation.Y - 20f), 4, 4, 29, Alpha: 100);
          Lighting.addLight((int) (((double) this.itemLocation.X - 16.0 + (double) this.velocity.X) / 16.0), (int) (((double) this.itemLocation.Y - 14.0) / 16.0), 1f);
        }
        else
        {
          if (Game1.rand.Next(maxValue) == 0)
            Dust.NewDust(new Vector2(this.itemLocation.X + 4f, this.itemLocation.Y - 20f), 4, 4, 29, Alpha: 100);
          Lighting.addLight((int) (((double) this.itemLocation.X + 6.0 + (double) this.velocity.X) / 16.0), (int) (((double) this.itemLocation.Y - 14.0) / 16.0), 1f);
        }
      }
      this.releaseUseItem = !this.controlUseItem;
      if (this.itemTime > 0)
        --this.itemTime;
      if (i == Game1.myPlayer)
      {
        if (this.inventory[this.selectedItem].shoot > 0 && this.itemAnimation > 0 && this.itemTime == 0)
        {
          int Type = this.inventory[this.selectedItem].shoot;
          float shootSpeed = this.inventory[this.selectedItem].shootSpeed;
          bool flag = false;
          int damage = this.inventory[this.selectedItem].damage;
          float KnockBack = this.inventory[this.selectedItem].knockBack;
          if (Type == 13 || Type == 32)
          {
            this.grappling[0] = -1;
            this.grapCount = 0;
            for (int index = 0; index < 1000; ++index)
            {
              if (Game1.projectile[index].active && Game1.projectile[index].owner == i && Game1.projectile[index].type == 13)
                Game1.projectile[index].Kill();
            }
          }
          if (this.inventory[this.selectedItem].useAmmo > 0)
          {
            for (int index = 0; index < 44; ++index)
            {
              if (this.inventory[index].ammo == this.inventory[this.selectedItem].useAmmo && this.inventory[index].stack > 0)
              {
                if (this.inventory[index].shoot > 0)
                  Type = this.inventory[index].shoot;
                shootSpeed += this.inventory[index].shootSpeed;
                damage += this.inventory[index].damage;
                KnockBack += this.inventory[index].knockBack;
                --this.inventory[index].stack;
                if (this.inventory[index].stack <= 0)
                  this.inventory[index].active = false;
                flag = true;
                break;
              }
            }
          }
          else
            flag = true;
          if (Type == 9 && (double) this.position.Y > Game1.worldSurface * 16.0 + (double) (Game1.screenHeight / 2))
            flag = false;
          if (flag)
          {
            if (Type == 1 && this.inventory[this.selectedItem].type == 120)
              Type = 2;
            this.itemTime = this.inventory[this.selectedItem].useTime;
            this.direction = (double) Game1.mouseState[0].Position.X 
                            + (double) Game1.screenPosition.X <= (double) this.position.X
                            + (double) this.width * 0.5 ? -1 : 1;

            Vector2 vector2 = new Vector2(this.position.X 
                + (float) this.width * 0.5f, this.position.Y + (float) this.height * 0.5f);
            if (Type == 9)
            {
              vector2 = new Vector2(this.position.X + (float) this.width * 0.5f 
                  + (float) (Game1.rand.Next(601) * -this.direction),
                  (float) ((double) this.position.Y + (double) this.height * 0.5 - 300.0) 
                  - (float) Game1.rand.Next(100));
              KnockBack = 0.0f;
            }
            float num5 = (float) Game1.mouseState[0].Position.X + Game1.screenPosition.X - vector2.X;
            float num6 = (float) Game1.mouseState[0].Position.Y + Game1.screenPosition.Y - vector2.Y;
            float num7 = (float) Math.Sqrt((double) num5 * (double) num5 + (double) num6 * (double) num6);
            float num8 = shootSpeed / num7;
            float SpeedX = num5 * num8;
            float SpeedY = num6 * num8;
            if (this.inventory[this.selectedItem].useStyle == 5)
            {
              this.itemRotation = (float) Math.Atan2((double) SpeedY * (double) this.direction,
                  (double) SpeedX * (double) this.direction);
              NetMessage.SendData(13, number: this.whoAmi);
              NetMessage.SendData(41, number: this.whoAmi);
            }
            if (Type == 17)
            {
              vector2.X = (float) Game1.mouseState[0].Position.X + Game1.screenPosition.X;
              vector2.Y = (float) Game1.mouseState[0].Position.Y + Game1.screenPosition.Y;
            }
            Projectile.NewProjectile(vector2.X, vector2.Y, SpeedX, SpeedY, Type, damage, KnockBack, i);
          }
          else if (this.inventory[this.selectedItem].useStyle == 5)
          {
            this.itemRotation = 0.0f;
            NetMessage.SendData(41, number: this.whoAmi);
          }
        }
        if ((this.inventory[this.selectedItem].pick > 0 || this.inventory[this.selectedItem].axe > 0 || this.inventory[this.selectedItem].hammer > 0) && (double) this.position.X / 16.0 - (double) Player.tileRangeX - (double) this.inventory[this.selectedItem].tileBoost <= (double) Player.tileTargetX && ((double) this.position.X + (double) this.width) / 16.0 + (double) Player.tileRangeX + (double) this.inventory[this.selectedItem].tileBoost - 1.0 >= (double) Player.tileTargetX && (double) this.position.Y / 16.0 - (double) Player.tileRangeY - (double) this.inventory[this.selectedItem].tileBoost <= (double) Player.tileTargetY && ((double) this.position.Y + (double) this.height) / 16.0 + (double) Player.tileRangeY + (double) this.inventory[this.selectedItem].tileBoost - 2.0 >= (double) Player.tileTargetY)
        {
          this.showItemIcon = true;
          if (Game1.tile[Player.tileTargetX, Player.tileTargetY].active && this.itemTime == 0 && this.itemAnimation > 0 && this.controlUseItem)
          {
            if (this.hitTileX != Player.tileTargetX || this.hitTileY != Player.tileTargetY)
            {
              this.hitTile = 0;
              this.hitTileX = Player.tileTargetX;
              this.hitTileY = Player.tileTargetY;
            }
            if (Game1.tileNoFail[(int) Game1.tile[Player.tileTargetX, Player.tileTargetY].type])
              this.hitTile = 100;
            if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type != (byte) 27)
            {
              if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 4 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 10 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 11 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 12 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 13 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 14 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 15 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 16 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 17 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 18 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 19 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 21 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 26 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 28 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 29 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 31 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 33 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 34 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 35 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 36 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 42 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 48 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 49 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 50 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 54 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 55)
              {
                if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 48)
                  this.hitTile += this.inventory[this.selectedItem].hammer / 3;
                else
                  this.hitTile += this.inventory[this.selectedItem].hammer;
                if (this.inventory[this.selectedItem].hammer > 0)
                {
                  if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 26)
                  {
                    this.Hurt(this.statLife / 2, -this.direction);
                    WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY, true);
                    if (Game1.netMode == 1)
                      NetMessage.SendData(17, number2: (float) Player.tileTargetX, number3: (float) Player.tileTargetY, number4: 1f);
                  }
                  else if (this.hitTile >= 100)
                  {
                    if (Game1.netMode == 1 && Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 21)
                    {
                      WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY, true);
                      NetMessage.SendData(17, number2: (float) Player.tileTargetX, number3: (float) Player.tileTargetY, number4: 1f);
                      NetMessage.SendData(34, number: Player.tileTargetX, number2: (float) Player.tileTargetY);
                    }
                    else
                    {
                      this.hitTile = 0;
                      WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY);
                      if (Game1.netMode == 1)
                        NetMessage.SendData(17, number2: (float) Player.tileTargetX, number3: (float) Player.tileTargetY);
                    }
                  }
                  else
                  {
                    WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY, true);
                    if (Game1.netMode == 1)
                      NetMessage.SendData(17, number2: (float) Player.tileTargetX, number3: (float) Player.tileTargetY, number4: 1f);
                  }
                  this.itemTime = this.inventory[this.selectedItem].useTime;
                }
              }
              else if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 5 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 30 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 72)
              {
                if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 30)
                  this.hitTile += this.inventory[this.selectedItem].axe * 5;
                else
                  this.hitTile += this.inventory[this.selectedItem].axe;
                if (this.inventory[this.selectedItem].axe > 0)
                {
                  if (this.hitTile >= 100)
                  {
                    this.hitTile = 0;
                    WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY);
                    if (Game1.netMode == 1)
                      NetMessage.SendData(17, number2: (float) Player.tileTargetX, number3: (float) Player.tileTargetY);
                  }
                  else
                  {
                    WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY, true);
                    if (Game1.netMode == 1)
                      NetMessage.SendData(17, number2: (float) Player.tileTargetX, number3: (float) Player.tileTargetY, number4: 1f);
                  }
                  this.itemTime = this.inventory[this.selectedItem].useTime;
                }
              }
              else if (this.inventory[this.selectedItem].pick > 0)
              {
                if (Game1.tileDungeon[(int) Game1.tile[Player.tileTargetX, Player.tileTargetY].type] || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 37 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 25)
                  this.hitTile += this.inventory[this.selectedItem].pick / 2;
                else
                  this.hitTile += this.inventory[this.selectedItem].pick;
                if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 25 && this.inventory[this.selectedItem].pick < 65)
                  this.hitTile = 0;
                else if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 37 && this.inventory[this.selectedItem].pick < 55)
                  this.hitTile = 0;
                if (this.hitTile >= 100)
                {
                  this.hitTile = 0;
                  WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY);
                  if (Game1.netMode == 1)
                    NetMessage.SendData(17, number2: (float) Player.tileTargetX, number3: (float) Player.tileTargetY);
                }
                else
                {
                  WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY, true);
                  if (Game1.netMode == 1)
                    NetMessage.SendData(17, number2: (float) Player.tileTargetX, number3: (float) Player.tileTargetY, number4: 1f);
                }
                this.itemTime = this.inventory[this.selectedItem].useTime;
              }
            }
          }
          if (Game1.tile[Player.tileTargetX, Player.tileTargetY].wall > (byte) 0 && this.itemTime == 0 && this.itemAnimation > 0 && this.controlUseItem && this.inventory[this.selectedItem].hammer > 0)
          {
            bool flag = true;
            if (!Game1.wallHouse[(int) Game1.tile[Player.tileTargetX, Player.tileTargetY].wall])
            {
              flag = false;
              for (int index3 = Player.tileTargetX - 1; index3 < Player.tileTargetX + 2; ++index3)
              {
                for (int index4 = Player.tileTargetY - 1; index4 < Player.tileTargetY + 2; ++index4)
                {
                  if ((int) Game1.tile[index3, index4].wall != (int) Game1.tile[Player.tileTargetX, Player.tileTargetY].wall)
                  {
                    flag = true;
                    break;
                  }
                }
              }
            }
            if (flag)
            {
              if (this.hitTileX != Player.tileTargetX || this.hitTileY != Player.tileTargetY)
              {
                this.hitTile = 0;
                this.hitTileX = Player.tileTargetX;
                this.hitTileY = Player.tileTargetY;
              }
              this.hitTile += this.inventory[this.selectedItem].hammer;
              if (this.hitTile >= 100)
              {
                this.hitTile = 0;
                WorldGen.KillWall(Player.tileTargetX, Player.tileTargetY);
                if (Game1.netMode == 1)
                  NetMessage.SendData(17, number: 2, number2: (float) Player.tileTargetX, number3: (float) Player.tileTargetY);
              }
              else
              {
                WorldGen.KillWall(Player.tileTargetX, Player.tileTargetY, true);
                if (Game1.netMode == 1)
                  NetMessage.SendData(17, number: 2, number2: (float) Player.tileTargetX, number3: (float) Player.tileTargetY, number4: 1f);
              }
              this.itemTime = this.inventory[this.selectedItem].useTime;
            }
          }
        }
        if (this.inventory[this.selectedItem].type == 29 && this.itemAnimation > 0 && this.statLifeMax < 400 && this.itemTime == 0)
        {
          this.itemTime = this.inventory[this.selectedItem].useTime;
          this.statLifeMax += 20;
          this.statLife += 20;
          if (Game1.myPlayer == this.whoAmi)
            this.HealEffect(20);
        }
        if (this.inventory[this.selectedItem].type == 109 && this.itemAnimation > 0 && this.statManaMax < 200 && this.itemTime == 0)
        {
          this.itemTime = this.inventory[this.selectedItem].useTime;
          this.statManaMax += 20;
          this.statMana += 20;
          if (Game1.myPlayer == this.whoAmi)
            this.ManaEffect(20);
        }
        if (this.inventory[this.selectedItem].createTile >= 0 && (double) this.position.X / 16.0 - (double) Player.tileRangeX - (double) this.inventory[this.selectedItem].tileBoost <= (double) Player.tileTargetX && ((double) this.position.X + (double) this.width) / 16.0 + (double) Player.tileRangeX + (double) this.inventory[this.selectedItem].tileBoost - 1.0 >= (double) Player.tileTargetX && (double) this.position.Y / 16.0 - (double) Player.tileRangeY - (double) this.inventory[this.selectedItem].tileBoost <= (double) Player.tileTargetY && ((double) this.position.Y + (double) this.height) / 16.0 + (double) Player.tileRangeY + (double) this.inventory[this.selectedItem].tileBoost - 2.0 >= (double) Player.tileTargetY)
        {
          this.showItemIcon = true;
          if ((!Game1.tile[Player.tileTargetX, Player.tileTargetY].active || this.inventory[this.selectedItem].createTile == 23 || this.inventory[this.selectedItem].createTile == 2) && this.itemTime == 0 && this.itemAnimation > 0 && this.controlUseItem)
          {
            bool flag = false;
            if (this.inventory[this.selectedItem].createTile == 23 || this.inventory[this.selectedItem].createTile == 2)
            {
              if (Game1.tile[Player.tileTargetX, Player.tileTargetY].active && Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 0)
                flag = true;
            }
            else if (this.inventory[this.selectedItem].createTile == 4)
            {
              if (Game1.tile[Player.tileTargetX + 1, Player.tileTargetY].active && !Game1.tileNoAttach[(int) Game1.tile[Player.tileTargetX + 1, Player.tileTargetY].type] || Game1.tile[Player.tileTargetX - 1, Player.tileTargetY].active && !Game1.tileNoAttach[(int) Game1.tile[Player.tileTargetX - 1, Player.tileTargetY].type] || Game1.tile[Player.tileTargetX, Player.tileTargetY + 1].active && !Game1.tileNoAttach[(int) Game1.tile[Player.tileTargetX, Player.tileTargetY + 1].type])
                flag = true;
            }
            else if (this.inventory[this.selectedItem].createTile == 13 || this.inventory[this.selectedItem].createTile == 29 || this.inventory[this.selectedItem].createTile == 33 || this.inventory[this.selectedItem].createTile == 49)
            {
              if (Game1.tile[Player.tileTargetX, Player.tileTargetY + 1].active && Game1.tileTable[(int) Game1.tile[Player.tileTargetX, Player.tileTargetY + 1].type])
                flag = true;
            }
            else if (this.inventory[this.selectedItem].createTile == 51)
            {
              if (Game1.tile[Player.tileTargetX + 1, Player.tileTargetY].active || Game1.tile[Player.tileTargetX + 1, Player.tileTargetY].wall > (byte) 0 || Game1.tile[Player.tileTargetX - 1, Player.tileTargetY].active || Game1.tile[Player.tileTargetX - 1, Player.tileTargetY].wall > (byte) 0 || Game1.tile[Player.tileTargetX, Player.tileTargetY + 1].active || Game1.tile[Player.tileTargetX, Player.tileTargetY + 1].wall > (byte) 0 || Game1.tile[Player.tileTargetX, Player.tileTargetY - 1].active || Game1.tile[Player.tileTargetX, Player.tileTargetY - 1].wall > (byte) 0)
                flag = true;
            }
            else if (Game1.tile[Player.tileTargetX + 1, Player.tileTargetY].active && Game1.tileSolid[(int) Game1.tile[Player.tileTargetX + 1, Player.tileTargetY].type] || Game1.tile[Player.tileTargetX + 1, Player.tileTargetY].wall > (byte) 0 || Game1.tile[Player.tileTargetX - 1, Player.tileTargetY].active && Game1.tileSolid[(int) Game1.tile[Player.tileTargetX - 1, Player.tileTargetY].type] || Game1.tile[Player.tileTargetX - 1, Player.tileTargetY].wall > (byte) 0 || Game1.tile[Player.tileTargetX, Player.tileTargetY + 1].active && Game1.tileSolid[(int) Game1.tile[Player.tileTargetX, Player.tileTargetY + 1].type] || Game1.tile[Player.tileTargetX, Player.tileTargetY + 1].wall > (byte) 0 || Game1.tile[Player.tileTargetX, Player.tileTargetY - 1].active && Game1.tileSolid[(int) Game1.tile[Player.tileTargetX, Player.tileTargetY - 1].type] || Game1.tile[Player.tileTargetX, Player.tileTargetY - 1].wall > (byte) 0)
              flag = true;
            if (flag && WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, this.inventory[this.selectedItem].createTile))
            {
              this.itemTime = this.inventory[this.selectedItem].useTime;
              if (Game1.netMode == 1)
                NetMessage.SendData(17, number: 1, number2: (float) Player.tileTargetX, number3: (float) Player.tileTargetY, number4: (float) this.inventory[this.selectedItem].createTile);
              if (this.inventory[this.selectedItem].createTile == 15)
              {
                if (this.direction == 1)
                {
                  Game1.tile[Player.tileTargetX, Player.tileTargetY].frameX += (short) 18;
                  Game1.tile[Player.tileTargetX, Player.tileTargetY - 1].frameX += (short) 18;
                }
                if (Game1.netMode == 1)
                  NetMessage.SendTileSquare(-1, Player.tileTargetX - 1, Player.tileTargetY - 1, 3);
              }
            }
          }
        }
        if (this.inventory[this.selectedItem].createWall >= 0)
        {
          Player.tileTargetX = (int) (((double) Game1.mouseState[0].Position.X 
                        + (double) Game1.screenPosition.X) / 16.0);
          Player.tileTargetY = (int) (((double) Game1.mouseState[0].Position.Y 
                        + (double) Game1.screenPosition.Y) / 16.0);

          if ((double) this.position.X / 16.0 - (double) Player.tileRangeX 
           - (double) this.inventory[this.selectedItem].tileBoost <= (double) Player.tileTargetX 
           && ((double) this.position.X + (double) this.width) / 16.0 
           + (double) Player.tileRangeX + (double) this.inventory[this.selectedItem].tileBoost 
           - 1.0 >= (double) Player.tileTargetX 
           && (double) this.position.Y / 16.0 
           - (double) Player.tileRangeY - (double) this.inventory[this.selectedItem].tileBoost 
           <= (double) Player.tileTargetY 
           && ((double) this.position.Y + (double) this.height) / 16.0 
           + (double) Player.tileRangeY + (double) this.inventory[this.selectedItem].tileBoost
           - 2.0 >= (double) Player.tileTargetY)
          {
            this.showItemIcon = true;
            if (this.itemTime == 0 && this.itemAnimation > 0 && this.controlUseItem 
          && (Game1.tile[Player.tileTargetX + 1, Player.tileTargetY].active 
          || Game1.tile[Player.tileTargetX + 1, Player.tileTargetY].wall > (byte) 0
          || Game1.tile[Player.tileTargetX - 1, Player.tileTargetY].active 
          || Game1.tile[Player.tileTargetX - 1, Player.tileTargetY].wall > (byte) 0 
          || Game1.tile[Player.tileTargetX, Player.tileTargetY + 1].active 
          || Game1.tile[Player.tileTargetX, Player.tileTargetY + 1].wall > (byte) 0 
          || Game1.tile[Player.tileTargetX, Player.tileTargetY - 1].active 
          || Game1.tile[Player.tileTargetX, Player.tileTargetY - 1].wall > (byte) 0) 
          && (int) Game1.tile[Player.tileTargetX, 
          Player.tileTargetY].wall != this.inventory[this.selectedItem].createWall)
            {
              WorldGen.PlaceWall(Player.tileTargetX, Player.tileTargetY, this.inventory[this.selectedItem].createWall);
              if ((int) Game1.tile[Player.tileTargetX, Player.tileTargetY].wall == this.inventory[this.selectedItem].createWall)
              {
                this.itemTime = this.inventory[this.selectedItem].useTime;
                if (Game1.netMode == 1)
                  NetMessage.SendData(17, number: 3, number2: (float) Player.tileTargetX, number3: (float) Player.tileTargetY, number4: (float) this.inventory[this.selectedItem].createWall);
              }
            }
          }
        }
      }
      if (this.inventory[this.selectedItem].damage >= 0 && this.inventory[this.selectedItem].type > 0 && !this.inventory[this.selectedItem].noMelee && this.itemAnimation > 0)
      {
        bool flag = false;
        Rectangle rectangle1 = new Rectangle((int) this.itemLocation.X, (int) this.itemLocation.Y, Game1.itemTexture[this.inventory[this.selectedItem].type].Width, Game1.itemTexture[this.inventory[this.selectedItem].type].Height);
        rectangle1.Width = (int) ((double) rectangle1.Width * (double) this.inventory[this.selectedItem].scale);
        rectangle1.Height = (int) ((double) rectangle1.Height * (double) this.inventory[this.selectedItem].scale);
        if (this.direction == -1)
          rectangle1.X -= rectangle1.Width;
        rectangle1.Y -= rectangle1.Height;
        if (this.inventory[this.selectedItem].useStyle == 1)
        {
          if ((double) this.itemAnimation < (double) this.inventory[this.selectedItem].useAnimation * 0.333)
          {
            if (this.direction == -1)
              rectangle1.X -= (int) ((double) rectangle1.Width * 1.4 - (double) rectangle1.Width);
            rectangle1.Width = (int) ((double) rectangle1.Width * 1.4);
            rectangle1.Y += (int) ((double) rectangle1.Height * 0.5);
            rectangle1.Height = (int) ((double) rectangle1.Height * 1.1);
          }
          else if ((double) this.itemAnimation >= (double) this.inventory[this.selectedItem].useAnimation * 0.666)
          {
            if (this.direction == 1)
              rectangle1.X -= (int) ((double) rectangle1.Width * 1.2);
            rectangle1.Width *= 2;
            rectangle1.Y -= (int) ((double) rectangle1.Height * 1.4 - (double) rectangle1.Height);
            rectangle1.Height = (int) ((double) rectangle1.Height * 1.4);
          }
        }
        else if (this.inventory[this.selectedItem].useStyle == 3)
        {
          if ((double) this.itemAnimation > (double) this.inventory[this.selectedItem].useAnimation * 0.666)
          {
            flag = true;
          }
          else
          {
            if (this.direction == -1)
              rectangle1.X -= (int) ((double) rectangle1.Width * 1.4 - (double) rectangle1.Width);
            rectangle1.Width = (int) ((double) rectangle1.Width * 1.4);
            rectangle1.Y += (int) ((double) rectangle1.Height * 0.6);
            rectangle1.Height = (int) ((double) rectangle1.Height * 0.6);
          }
        }
        if (!flag)
        {
          if ((this.inventory[this.selectedItem].type == 44 || this.inventory[this.selectedItem].type == 45 || this.inventory[this.selectedItem].type == 46 || this.inventory[this.selectedItem].type == 103 || this.inventory[this.selectedItem].type == 104) && Game1.rand.Next(15) == 0)
            Dust.NewDust(new Vector2((float) rectangle1.X, (float) rectangle1.Y), rectangle1.Width, rectangle1.Height, 14, (float) (this.direction * 2), Alpha: 150, Scale: 1.3f);
          if (this.inventory[this.selectedItem].type == 65)
          {
            if (Game1.rand.Next(5) == 0)
              Dust.NewDust(new Vector2((float) rectangle1.X, (float) rectangle1.Y), rectangle1.Width, rectangle1.Height, 15, Alpha: 150, Scale: 1.2f);
            if (Game1.rand.Next(10) == 0)
              Gore.NewGore(new Vector2((float) rectangle1.X, (float) rectangle1.Y), new Vector2(), Game1.rand.Next(16, 18));
          }
          if (this.inventory[this.selectedItem].type == 190)
          {
            int index = Dust.NewDust(new Vector2((float) rectangle1.X, (float) rectangle1.Y), rectangle1.Width, rectangle1.Height, 40, this.velocity.X * 0.2f + (float) (this.direction * 3), this.velocity.Y * 0.2f, Scale: 1.2f);
            Game1.dust[index].noGravity = true;
          }
          if (this.inventory[this.selectedItem].type == 121)
          {
            for (int index5 = 0; index5 < 2; ++index5)
            {
              int index6 = Dust.NewDust(new Vector2((float) rectangle1.X, (float) rectangle1.Y), rectangle1.Width, rectangle1.Height, 6, this.velocity.X * 0.2f + (float) (this.direction * 3), this.velocity.Y * 0.2f, 100, Scale: 2.5f);
              Game1.dust[index6].noGravity = true;
              Game1.dust[index6].velocity.X *= 2f;
              Game1.dust[index6].velocity.Y *= 2f;
            }
          }
          if (this.inventory[this.selectedItem].type == 122)
          {
            int index = Dust.NewDust(new Vector2((float) rectangle1.X, (float) rectangle1.Y), rectangle1.Width, rectangle1.Height, 6, this.velocity.X * 0.2f + (float) (this.direction * 3), this.velocity.Y * 0.2f, 100, Scale: 1.9f);
            Game1.dust[index].noGravity = true;
          }
          if (this.inventory[this.selectedItem].type == 155)
          {
            int index = Dust.NewDust(new Vector2((float) rectangle1.X, (float) rectangle1.Y), rectangle1.Width, rectangle1.Height, 29, this.velocity.X * 0.2f + (float) (this.direction * 3), this.velocity.Y * 0.2f, 100, Scale: 2f);
            Game1.dust[index].noGravity = true;
            Game1.dust[index].velocity.X /= 2f;
            Game1.dust[index].velocity.Y /= 2f;
          }
          if (Game1.myPlayer == i)
          {
            int num9 = rectangle1.X / 16;
            int num10 = (rectangle1.X + rectangle1.Width) / 16 + 1;
            int num11 = rectangle1.Y / 16;
            int num12 = (rectangle1.Y + rectangle1.Height) / 16 + 1;
            for (int index7 = num9; index7 < num10; ++index7)
            {
              for (int index8 = num11; index8 < num12; ++index8)
              {
                if (Game1.tile[index7, index8].type == (byte) 3 || Game1.tile[index7, index8].type == (byte) 24 || Game1.tile[index7, index8].type == (byte) 28 || Game1.tile[index7, index8].type == (byte) 32 || Game1.tile[index7, index8].type == (byte) 51 || Game1.tile[index7, index8].type == (byte) 52 || Game1.tile[index7, index8].type == (byte) 61 || Game1.tile[index7, index8].type == (byte) 62 || Game1.tile[index7, index8].type == (byte) 69 || Game1.tile[index7, index8].type == (byte) 71 || Game1.tile[index7, index8].type == (byte) 73 || Game1.tile[index7, index8].type == (byte) 74)
                {
                  WorldGen.KillTile(index7, index8);
                  if (Game1.netMode == 1)
                    NetMessage.SendData(17, number2: (float) index7, number3: (float) index8);
                }
              }
            }
            for (int number = 0; number < 1000; ++number)
            {
              if (Game1.npc[number].active && Game1.npc[number].immune[i] == 0 && this.attackCD == 0 && !Game1.npc[number].friendly)
              {
                Rectangle rectangle2 = new Rectangle((int) Game1.npc[number].position.X, (int) Game1.npc[number].position.Y, Game1.npc[number].width, Game1.npc[number].height);
                if (rectangle1.Intersects(rectangle2) && (Game1.npc[number].noTileCollide || Collision.CanHit(this.position, this.width, this.height, Game1.npc[number].position, Game1.npc[number].width, Game1.npc[number].height)))
                {
                  Game1.npc[number].StrikeNPC(this.inventory[this.selectedItem].damage, this.inventory[this.selectedItem].knockBack, this.direction);
                  if (Game1.netMode == 1)
                    NetMessage.SendData(24, number: number, number2: (float) i);
                  Game1.npc[number].immune[i] = this.itemAnimation;
                  this.attackCD = (int) ((double) this.inventory[this.selectedItem].useAnimation * 0.33);
                }
              }
            }
            if (this.hostile)
            {
              for (int number = 0; number < 8; ++number)
              {
                if (number != i && Game1.player[number].active && Game1.player[number].hostile && !Game1.player[number].immune && !Game1.player[number].dead && (Game1.player[i].team == 0 || Game1.player[i].team != Game1.player[number].team))
                {
                  Rectangle rectangle3 = new Rectangle((int) Game1.player[number].position.X, (int) Game1.player[number].position.Y, Game1.player[number].width, Game1.player[number].height);
                  if (rectangle1.Intersects(rectangle3) && Collision.CanHit(this.position, this.width, this.height, Game1.player[number].position, Game1.player[number].width, Game1.player[number].height))
                  {
                    Game1.player[number].Hurt(this.inventory[this.selectedItem].damage, this.direction, true);
                    if (Game1.netMode != 0)
                      NetMessage.SendData(26, number: number, number2: (float) this.direction, number3: (float) this.inventory[this.selectedItem].damage, number4: 1f);
                    this.attackCD = (int) ((double) this.inventory[this.selectedItem].useAnimation * 0.33);
                  }
                }
              }
            }
          }
        }
      }
      if (this.itemTime == 0 && this.inventory[this.selectedItem].healLife > 0 && this.itemAnimation > 0)
      {
        this.statLife += this.inventory[this.selectedItem].healLife;
        this.itemTime = this.inventory[this.selectedItem].useTime;
        if (Game1.myPlayer == this.whoAmi)
          this.HealEffect(this.inventory[this.selectedItem].healLife);
      }
      if (this.itemTime == 0 && this.inventory[this.selectedItem].healMana > 0 && this.itemAnimation > 0)
      {
        this.statMana += this.inventory[this.selectedItem].healMana;
        this.itemTime = this.inventory[this.selectedItem].useTime;
        if (Game1.myPlayer == this.whoAmi)
          this.ManaEffect(this.inventory[this.selectedItem].healMana);
      }
      if (this.itemTime == 0 && this.itemAnimation > 0 && (this.inventory[this.selectedItem].type == 43 || this.inventory[this.selectedItem].type == 70))
      {
        this.itemTime = this.inventory[this.selectedItem].useTime;
        bool flag = false;
        int num = 4;
        if (this.inventory[this.selectedItem].type == 43)
          num = 4;
        else if (this.inventory[this.selectedItem].type == 70)
          num = 13;
        for (int index = 0; index < 1000; ++index)
        {
          if (Game1.npc[index].active && Game1.npc[index].type == num)
          {
            flag = true;
            break;
          }
        }
        if (flag)
        {
          if (Game1.myPlayer == this.whoAmi)
            this.Hurt(this.statLife * (this.statDefense + 1), -this.direction);
        }
        else if (this.inventory[this.selectedItem].type == 43)
        {
          if (Game1.dayTime)
          {
            if (Game1.myPlayer == this.whoAmi)
              this.Hurt(this.statLife * (this.statDefense + 1), -this.direction);
          }
          else
          {
            Game1.PlaySound(15, (int) this.position.X, (int) this.position.Y, 0);
            if (Game1.netMode != 1)
              NPC.SpawnOnPlayer(i, 4);
          }
        }
        else if (this.inventory[this.selectedItem].type == 70)
        {
          if (!this.zoneEvil)
          {
            if (Game1.myPlayer == this.whoAmi)
              this.Hurt(this.statLife * (this.statDefense + 1), -this.direction);
          }
          else
          {
            Game1.PlaySound(15, (int) this.position.X, (int) this.position.Y, 0);
            if (Game1.netMode != 1)
              NPC.SpawnOnPlayer(i, 13);
          }
        }
      }
      if (this.inventory[this.selectedItem].type == 50 && this.itemAnimation > 0)
      {
        if (Game1.rand.Next(2) == 0)
          Dust.NewDust(this.position, this.width, this.height, 15, Alpha: 150, Scale: 1.1f);
        if (this.itemTime == 0)
          this.itemTime = this.inventory[this.selectedItem].useTime;
        else if (this.itemTime == this.inventory[this.selectedItem].useTime / 2)
        {
          for (int index = 0; index < 70; ++index)
            Dust.NewDust(this.position, this.width, this.height, 15, this.velocity.X * 0.5f, this.velocity.Y * 0.5f, 150, Scale: 1.5f);
          this.grappling[0] = -1;
          this.grapCount = 0;
          for (int index = 0; index < 1000; ++index)
          {
            if (Game1.projectile[index].active && Game1.projectile[index].owner == i && Game1.projectile[index].aiStyle == 7)
              Game1.projectile[index].Kill();
          }
          this.Spawn();
          for (int index = 0; index < 70; ++index)
            Dust.NewDust(this.position, this.width, this.height, 15, Alpha: 150, Scale: 1.5f);
        }
      }
      if (i != Game1.myPlayer)
        return;
      if (this.itemTime == this.inventory[this.selectedItem].useTime && this.inventory[this.selectedItem].consumable)
      {
        --this.inventory[this.selectedItem].stack;
        if (this.inventory[this.selectedItem].stack <= 0)
          this.itemTime = this.itemAnimation;
      }
      if (this.inventory[this.selectedItem].stack <= 0 && this.itemAnimation == 0)
        this.inventory[this.selectedItem] = new Item();
    }

    public Color GetImmuneAlpha(Color newColor)
    {
      float num = (float) ((int) byte.MaxValue - this.immuneAlpha) / (float) byte.MaxValue;
      if ((double) this.shadow > 0.0)
        num *= 1f - this.shadow;
      int r = (int) ((double) newColor.R * (double) num);
      int g = (int) ((double) newColor.G * (double) num);
      int b = (int) ((double) newColor.B * (double) num);
      int a = (int) ((double) newColor.A * (double) num);
      if (a < 0)
        a = 0;
      if (a > (int) byte.MaxValue)
        a = (int) byte.MaxValue;
      return new Color(r, g, b, a);
    }

    public Color GetDeathAlpha(Color newColor)
    {
      int r = (int) newColor.R + (int) ((double) this.immuneAlpha * 0.9);
      int g = (int) newColor.G + (int) ((double) this.immuneAlpha * 0.5);
      int b = (int) newColor.B + (int) ((double) this.immuneAlpha * 0.5);
      int a = (int) newColor.A + (int) ((double) this.immuneAlpha * 0.4);
      if (a < 0)
        a = 0;
      if (a > (int) byte.MaxValue)
        a = (int) byte.MaxValue;
      return new Color(r, g, b, a);
    }

    public void DropItems()
    {
      for (int index1 = 10; index1 < 44; ++index1)
      {
        int index2 = Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, this.inventory[index1].type);
        this.inventory[index1].position = Game1.item[index2].position;
        Game1.item[index2] = this.inventory[index1];
        this.inventory[index1] = new Item();
        this.selectedItem = 0;
        Game1.item[index2].velocity.Y = (float) Game1.rand.Next(-20, 1) * 0.1f;
        Game1.item[index2].velocity.X = (float) Game1.rand.Next(-20, 21) * 0.1f;
        Game1.item[index2].noGrabDelay = 100;
      }
    }

    public object Clone() => this.MemberwiseClone();

    public object clientClone()
    {
      Player player = new Player();
      player.zoneEvil = this.zoneEvil;
      player.zoneMeteor = this.zoneMeteor;
      player.zoneDungeon = this.zoneDungeon;
      player.zoneJungle = this.zoneJungle;
      player.direction = this.direction;
      player.selectedItem = this.selectedItem;
      player.controlUp = this.controlUp;
      player.controlDown = this.controlDown;
      player.controlLeft = this.controlLeft;
      player.controlRight = this.controlRight;
      player.controlJump = this.controlJump;
      player.controlUseItem = this.controlUseItem;
      player.statLife = this.statLife;
      player.statLifeMax = this.statLifeMax;
      player.statMana = this.statMana;
      player.statManaMax = this.statManaMax;
      player.position.X = this.position.X;
      player.chest = this.chest;
      player.talkNPC = this.talkNPC;
      for (int index = 0; index < 44; ++index)
      {
        player.inventory[index] = (Item) this.inventory[index].Clone();
        if (index < 8)
          player.armor[index] = (Item) this.armor[index].Clone();
      }
      return (object) player;
    }

    public static void SavePlayer(Player newPlayer, string playerPath)
    {
      if (playerPath == null)
        return;
      string destFileName = playerPath + ".bak";
      if (File.Exists(playerPath))
        File.Copy(playerPath, destFileName, true);
      using (FileStream output = new FileStream(playerPath, FileMode.Create))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) output))
        {
          binaryWriter.Write(Game1.curRelease);
          binaryWriter.Write(newPlayer.name);
          binaryWriter.Write(newPlayer.hair);
          binaryWriter.Write(newPlayer.statLife);
          binaryWriter.Write(newPlayer.statLifeMax);
          binaryWriter.Write(newPlayer.statMana);
          binaryWriter.Write(newPlayer.statManaMax);
          binaryWriter.Write(newPlayer.hairColor.R);
          binaryWriter.Write(newPlayer.hairColor.G);
          binaryWriter.Write(newPlayer.hairColor.B);
          binaryWriter.Write(newPlayer.skinColor.R);
          binaryWriter.Write(newPlayer.skinColor.G);
          binaryWriter.Write(newPlayer.skinColor.B);
          binaryWriter.Write(newPlayer.eyeColor.R);
          binaryWriter.Write(newPlayer.eyeColor.G);
          binaryWriter.Write(newPlayer.eyeColor.B);
          binaryWriter.Write(newPlayer.shirtColor.R);
          binaryWriter.Write(newPlayer.shirtColor.G);
          binaryWriter.Write(newPlayer.shirtColor.B);
          binaryWriter.Write(newPlayer.underShirtColor.R);
          binaryWriter.Write(newPlayer.underShirtColor.G);
          binaryWriter.Write(newPlayer.underShirtColor.B);
          binaryWriter.Write(newPlayer.pantsColor.R);
          binaryWriter.Write(newPlayer.pantsColor.G);
          binaryWriter.Write(newPlayer.pantsColor.B);
          binaryWriter.Write(newPlayer.shoeColor.R);
          binaryWriter.Write(newPlayer.shoeColor.G);
          binaryWriter.Write(newPlayer.shoeColor.B);
          for (int index = 0; index < 8; ++index)
          {
            if (newPlayer.armor[index].name == null)
              newPlayer.armor[index].name = "";
            binaryWriter.Write(newPlayer.armor[index].name);
          }
          for (int index = 0; index < 44; ++index)
          {
            if (newPlayer.inventory[index].name == null)
              newPlayer.inventory[index].name = "";
            binaryWriter.Write(newPlayer.inventory[index].name);
            binaryWriter.Write(newPlayer.inventory[index].stack);
          }
          for (int index = 0; index < Chest.maxItems; ++index)
          {
            if (newPlayer.bank[index].name == null)
              newPlayer.bank[index].name = "";
            binaryWriter.Write(newPlayer.bank[index].name);
            binaryWriter.Write(newPlayer.bank[index].stack);
          }
          binaryWriter.Dispose();//.Close();
        }
      }
    }

    public static Player LoadPlayer(string playerPath)
    {
      if (Game1.rand == null)
        Game1.rand = new Random((int) DateTime.Now.Ticks);
      Player player = new Player();
      using (FileStream input = new FileStream(playerPath, FileMode.Open))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) input))
        {
          try
          {
          //RnD
          int num = binaryReader.ReadInt32();//binaryReader.ReadInt32();

          player.name = binaryReader.ReadString();
          player.hair = binaryReader.ReadInt32();
          player.statLife = binaryReader.ReadInt32();
          player.statLifeMax = binaryReader.ReadInt32();
          if (num >= 25)
          {
            player.statMana = binaryReader.ReadInt32();
            player.statManaMax = binaryReader.ReadInt32();
          }
          player.hairColor.R = binaryReader.ReadByte();
          player.hairColor.G = binaryReader.ReadByte();
          player.hairColor.B = binaryReader.ReadByte();
          player.skinColor.R = binaryReader.ReadByte();
          player.skinColor.G = binaryReader.ReadByte();
          player.skinColor.B = binaryReader.ReadByte();
          player.eyeColor.R = binaryReader.ReadByte();
          player.eyeColor.G = binaryReader.ReadByte();
          player.eyeColor.B = binaryReader.ReadByte();
          player.shirtColor.R = binaryReader.ReadByte();
          player.shirtColor.G = binaryReader.ReadByte();
          player.shirtColor.B = binaryReader.ReadByte();
          player.underShirtColor.R = binaryReader.ReadByte();
          player.underShirtColor.G = binaryReader.ReadByte();
          player.underShirtColor.B = binaryReader.ReadByte();
          player.pantsColor.R = binaryReader.ReadByte();
          player.pantsColor.G = binaryReader.ReadByte();
          player.pantsColor.B = binaryReader.ReadByte();
          player.shoeColor.R = binaryReader.ReadByte();
          player.shoeColor.G = binaryReader.ReadByte();
          player.shoeColor.B = binaryReader.ReadByte();
          for (int index = 0; index < 6; ++index)
            player.armor[index].SetDefaults(binaryReader.ReadString());
          if (num >= 30)
          {
            for (int index = 6; index < 8; ++index)
              player.armor[index].SetDefaults(binaryReader.ReadString());
          }
          for (int index = 0; index < 40; ++index)
          {
            player.inventory[index].SetDefaults(binaryReader.ReadString());
            player.inventory[index].stack = binaryReader.ReadInt32();
          }
          if (num >= 30)
          {
            for (int index = 40; index < 44; ++index)
            {
              player.inventory[index].SetDefaults(binaryReader.ReadString());
              player.inventory[index].stack = binaryReader.ReadInt32();
            }
          }
          if (num >= 19)
          {
            for (int index = 0; index < Chest.maxItems; ++index)
            {
              player.bank[index].SetDefaults(binaryReader.ReadString());
              player.bank[index].stack = binaryReader.ReadInt32();
            }
          }
          }
          catch (Exception ex)
          {
             Debug.WriteLine("[ex] Player - LoadPlayer bug: " + ex.Message);
          }

          try
          {
             binaryReader.Dispose();//.Close();
          }
          catch { }
        }
      }
      player.PlayerFrame();
      return player;
    }//LoadPlayer

    public Player()
    {
      for (int index = 0; index < 44; ++index)
      {
        if (index < 8)
        {
          this.armor[index] = new Item();
          this.armor[index].name = "";
        }
        this.inventory[index] = new Item();
        this.inventory[index].name = "";
      }
      for (int index = 0; index < Chest.maxItems; ++index)
      {
        this.bank[index] = new Item();
        this.bank[index].name = "";
      }
      this.grappling[0] = -1;
      this.inventory[0].SetDefaults("Copper Pickaxe");
      this.inventory[1].SetDefaults("Copper Axe");
      for (int index = 0; index < 76; ++index)
      {
        this.adjTile[index] = false;
        this.oldAdjTile[index] = false;
      }
    }
  }
}
