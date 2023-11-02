
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameManager
{
  public class Player
  {
    public Vector2 position;
    public Vector2 velocity;
    public double headFrameCounter;
    public double bodyFrameCounter;
    public double legFrameCounter;
    public bool immune;
    public int immuneTime;
    public int immuneAlphaDirection;
    public int immuneAlpha;
    public int activeNPCs;
    public bool mouseInterface;
    public int changeItem = -1;
    public int selectedItem = 0;
    public Item[] armor = new Item[3];
    public int itemAnimation;
    public int itemAnimationMax;
    public int itemTime;
    public float itemRotation;
    public int itemWidth;
    public int itemHeight;
    public Vector2 itemLocation;
    public Item[] inventory = new Item[40];
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
    public int hitTile;
    public int hitTileX;
    public int hitTileY;
    public int jump;
    public int head = Game1.rand.Next(2);
    public int body = 0;
    public int legs = 0;
    public Rectangle headFrame;
    public Rectangle bodyFrame;
    public Rectangle legFrame;
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
    public int statDefense = 10;
    public int statAttack = 0;
    public int statLifeMax = 100;
    public int statLife = 100;
    private static int tileRangeX = 5;
    private static int tileRangeY = 4;
    private static int tileTargetX;
    private static int tileTargetY;
    private static float maxRunSpeed = 3f;
    private static float runAcceleration = 0.08f;
    private static float runSlowdown = 0.2f;
    private static int jumpHeight = 15;
    private static float jumpSpeed = 5.01f;
    private static float gravity = 0.4f;
    private static float maxFallSpeed = 10f;
    private static int itemGrabRange = 32;
    private static float itemGrabSpeed = 0.4f;
    private static float itemGrabSpeedMax = 4f;

    public void UpdatePlayer(int i)
    {
      if (!this.active)
        return;
      if (this.dead)
      {
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
        if (this.respawnTimer <= 0)
          this.Spawn();
      }
      else
      {
        if (i == Game1.myPlayer)
        {
          this.controlUp = false;
          this.controlLeft = false;
          this.controlDown = false;
          this.controlRight = false;
          this.controlJump = false;
          this.controlUseItem = false;
          this.controlUseTile = false;
          if (Game1.keyState.IsKeyDown(Keys.W))
            this.controlUp = true;
          if (Game1.keyState.IsKeyDown(Keys.A))
            this.controlLeft = true;
          if (Game1.keyState.IsKeyDown(Keys.S))
            this.controlDown = true;
          if (Game1.keyState.IsKeyDown(Keys.D))
            this.controlRight = true;
          if (Game1.keyState.IsKeyDown(Keys.Space))
            this.controlJump = true;
          if (Game1.mouseState.LeftButton == ButtonState.Pressed && !this.mouseInterface)
            this.controlUseItem = true;
          if (Game1.mouseState.RightButton == ButtonState.Pressed && !this.mouseInterface)
            this.controlUseTile = true;
          if (Game1.keyState.IsKeyDown(Keys.Escape))
          {
            if (this.releaseInventory)
            {
              if (!Game1.playerInventory)
              {
                Recipe.FindRecipes();
                Game1.playerInventory = true;
                Game1.soundMenuOpen.Play();
              }
              else
              {
                Game1.playerInventory = false;
                Game1.soundMenuClose.Play();
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
            if (Game1.keyState.IsKeyDown(Keys.Q) && this.inventory[this.selectedItem].type > 0 || (Game1.mouseState.LeftButton == ButtonState.Pressed && !this.mouseInterface && Game1.mouseLeftRelease || !Game1.playerInventory) && Game1.mouseItem.type > 0)
            {
              Item obj = new Item();
              bool flag = false;
              if ((Game1.mouseState.LeftButton == ButtonState.Pressed && !this.mouseInterface && Game1.mouseLeftRelease || !Game1.playerInventory) && Game1.mouseItem.type > 0)
              {
                obj = this.inventory[this.selectedItem];
                this.inventory[this.selectedItem] = Game1.mouseItem;
                this.delayUseItem = true;
                this.controlUseItem = false;
                flag = true;
              }
              int index = Item.NewItem((int) this.position.X, (int) this.position.Y, this.width, this.height, this.inventory[this.selectedItem].type);
              if (!flag && this.inventory[this.selectedItem].type == 8 && this.inventory[this.selectedItem].stack > 1)
              {
                --this.inventory[this.selectedItem].stack;
              }
              else
              {
                this.inventory[this.selectedItem].position = Game1.item[index].position;
                Game1.item[index] = this.inventory[this.selectedItem];
                this.inventory[this.selectedItem] = new Item();
              }
              Game1.item[index].noGrabDelay = 100;
              Game1.item[index].velocity.Y = -2f;
              Game1.item[index].velocity.X = (float) (4 * this.direction) + this.velocity.X;
              if ((Game1.mouseState.LeftButton == ButtonState.Pressed && !this.mouseInterface || !Game1.playerInventory) && Game1.mouseItem.type > 0)
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
                Game1.soundInstanceMenuTick.Play();
              int num = (Game1.mouseState.ScrollWheelValue - Game1.oldMouseState.ScrollWheelValue) / 120;
              while (num > 9)
                num -= 10;
              while (num < 0)
                num += 10;
              this.selectedItem -= num;
              if (num != 0)
                Game1.soundInstanceMenuTick.Play();
              if (this.changeItem >= 0)
              {
                if (this.selectedItem != this.changeItem)
                  Game1.soundInstanceMenuTick.Play();
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
              int num = (Game1.mouseState.ScrollWheelValue - Game1.oldMouseState.ScrollWheelValue) / 120;
              Game1.focusRecipe += num;
              if (Game1.focusRecipe > Game1.numAvailableRecipes - 1)
                Game1.focusRecipe = Game1.numAvailableRecipes - 1;
              if (Game1.focusRecipe < 0)
                Game1.focusRecipe = 0;
            }
          }
        }
        if (this.mouseInterface)
          this.delayUseItem = true;
        Player.tileTargetX = (int) (((double) Game1.mouseState.X + (double) Game1.screenPosition.X) / 16.0);
        Player.tileTargetY = (int) (((double) Game1.mouseState.Y + (double) Game1.screenPosition.Y) / 16.0);
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
        this.statDefense = 10;
        for (int index = 0; index < 3; ++index)
          this.statDefense += this.armor[index].defense;
        this.body = this.armor[1].bodySlot;
        this.legs = this.armor[2].legSlot;
        this.headFrame.Width = 32;
        this.headFrame.Height = 48;
        this.bodyFrame.Width = 32;
        this.bodyFrame.Height = 48;
        this.legFrame.Width = 32;
        this.legFrame.Height = 48;
        if (this.controlLeft && (double) this.velocity.X > -(double) Player.maxRunSpeed)
        {
          if ((double) this.velocity.X > (double) Player.runSlowdown)
            this.velocity.X -= Player.runSlowdown;
          this.velocity.X -= Player.runAcceleration;
          if (this.itemAnimation == 0 || this.inventory[this.selectedItem].useTurn)
            this.direction = -1;
        }
        else if (this.controlRight && (double) this.velocity.X < (double) Player.maxRunSpeed)
        {
          if ((double) this.velocity.X < -(double) Player.runSlowdown)
            this.velocity.X += Player.runSlowdown;
          this.velocity.X += Player.runAcceleration;
          if (this.itemAnimation == 0 || this.inventory[this.selectedItem].useTurn)
            this.direction = 1;
        }
        else if ((double) this.velocity.Y == 0.0)
        {
          if ((double) this.velocity.X > (double) Player.runSlowdown)
            this.velocity.X -= Player.runSlowdown;
          else if ((double) this.velocity.X < -(double) Player.runSlowdown)
            this.velocity.X += Player.runSlowdown;
          else
            this.velocity.X = 0.0f;
        }
        else if ((double) this.velocity.X > (double) Player.runSlowdown * 0.5)
          this.velocity.X -= Player.runSlowdown * 0.5f;
        else if ((double) this.velocity.X < -(double) Player.runSlowdown * 0.5)
          this.velocity.X += Player.runSlowdown * 0.5f;
        else
          this.velocity.X = 0.0f;
        if (this.controlJump)
        {
          if (this.jump > 0)
          {
            if ((double) this.velocity.Y > -(double) Player.jumpSpeed + (double) Player.gravity * 2.0)
            {
              this.jump = 0;
            }
            else
            {
              this.velocity.Y = -Player.jumpSpeed;
              --this.jump;
            }
          }
          else if ((double) this.velocity.Y == 0.0 && this.releaseJump)
          {
            this.velocity.Y = -Player.jumpSpeed;
            this.jump = Player.jumpHeight;
          }
          this.releaseJump = false;
        }
        else
        {
          this.jump = 0;
          this.releaseJump = true;
        }
        this.velocity.Y += Player.gravity;
        if ((double) this.velocity.Y > (double) Player.maxFallSpeed)
          this.velocity.Y = Player.maxFallSpeed;
        for (int index = 0; index < 1000; ++index)
        {
          if (Game1.item[index].active && Game1.item[index].noGrabDelay == 0)
          {
            Rectangle rectangle = new Rectangle((int) this.position.X, (int) this.position.Y, this.width, this.height);
            if (rectangle.Intersects(new Rectangle((int) Game1.item[index].position.X, (int) Game1.item[index].position.Y, Game1.item[index].width, Game1.item[index].height)))
            {
              if (this.inventory[this.selectedItem].type != 0 || this.itemAnimation <= 0)
                Game1.item[index] = this.GetItem(i, Game1.item[index]);
            }
            else
            {
              rectangle = new Rectangle((int) this.position.X - Player.itemGrabRange, (int) this.position.Y - Player.itemGrabRange, this.width + Player.itemGrabRange * 2, this.height + Player.itemGrabRange * 2);
              if (rectangle.Intersects(new Rectangle((int) Game1.item[index].position.X, (int) Game1.item[index].position.Y, Game1.item[index].width, Game1.item[index].height)) && this.ItemSpace(Game1.item[index]))
              {
                Game1.item[index].beingGrabbed = true;
                if ((double) this.position.X + (double) this.width * 0.5 > (double) Game1.item[index].position.X + (double) Game1.item[index].width * 0.5)
                {
                  if ((double) Game1.item[index].velocity.X < (double) Player.itemGrabSpeedMax + (double) this.velocity.X)
                    Game1.item[index].velocity.X += Player.itemGrabSpeed;
                  if ((double) Game1.item[index].velocity.X < 0.0)
                    Game1.item[index].velocity.X += Player.itemGrabSpeed * 0.75f;
                }
                else
                {
                  if ((double) Game1.item[index].velocity.X > -(double) Player.itemGrabSpeedMax + (double) this.velocity.X)
                    Game1.item[index].velocity.X -= Player.itemGrabSpeed;
                  if ((double) Game1.item[index].velocity.X > 0.0)
                    Game1.item[index].velocity.X -= Player.itemGrabSpeed * 0.75f;
                }
                if ((double) this.position.Y + (double) this.height * 0.5 > (double) Game1.item[index].position.Y + (double) Game1.item[index].height * 0.5)
                {
                  if ((double) Game1.item[index].velocity.Y < (double) Player.itemGrabSpeedMax)
                    Game1.item[index].velocity.Y += Player.itemGrabSpeed;
                  if ((double) Game1.item[index].velocity.Y < 0.0)
                    Game1.item[index].velocity.Y += Player.itemGrabSpeed * 0.75f;
                }
                else
                {
                  if ((double) Game1.item[index].velocity.Y > -(double) Player.itemGrabSpeedMax)
                    Game1.item[index].velocity.Y -= Player.itemGrabSpeed;
                  if ((double) Game1.item[index].velocity.Y > 0.0)
                    Game1.item[index].velocity.Y -= Player.itemGrabSpeed * 0.75f;
                }
              }
            }
          }
        }
        if ((double) this.position.X / 16.0 - (double) Player.tileRangeX <= (double) Player.tileTargetX && ((double) this.position.X + (double) this.width) / 16.0 + (double) Player.tileRangeX - 1.0 >= (double) Player.tileTargetX && (double) this.position.Y / 16.0 - (double) Player.tileRangeY <= (double) Player.tileTargetY && ((double) this.position.Y + (double) this.height) / 16.0 + (double) Player.tileRangeY - 2.0 >= (double) Player.tileTargetY && Game1.tile[Player.tileTargetX, Player.tileTargetY].active)
        {
          if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 4)
          {
            this.showItemIcon = true;
            this.showItemIcon2 = 8;
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
              if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 4)
                WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY);
              else if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 10)
                WorldGen.OpenDoor(Player.tileTargetX, Player.tileTargetY, this.direction);
              else if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 11)
                WorldGen.CloseDoor(Player.tileTargetX, Player.tileTargetY);
            }
            this.releaseUseTile = false;
          }
          else
            this.releaseUseTile = true;
        }
        Rectangle rectangle1 = new Rectangle((int) this.position.X, (int) this.position.Y, this.width, this.height);
        for (int index = 0; index < 1000; ++index)
        {
          if (Game1.npc[index].active && rectangle1.Intersects(new Rectangle((int) Game1.npc[index].position.X, (int) Game1.npc[index].position.Y, Game1.npc[index].width, Game1.npc[index].height)))
          {
            int hitDirection = -1;
            if ((double) Game1.npc[index].position.X + (double) (Game1.npc[index].width / 2) < (double) this.position.X + (double) (this.width / 2))
              hitDirection = 1;
            this.Hurt(Game1.npc[index].damage, hitDirection);
          }
        }
        this.velocity = Collision.TileCollision(this.position, this.velocity, this.width, this.height);
        this.position += this.velocity;
        this.ItemCheck(i);
        this.PlayerFrame();
        if (this.statLife > this.statLifeMax)
          this.statLife = this.statLifeMax;
      }
    }

    private void PlayerFrame()
    {
      this.headFrame.X = 34 * this.head;
      this.bodyFrame.X = 34 * this.body;
      this.legFrame.X = 34 * this.legs;
      this.headFrame.Y = 0;
      if (this.itemAnimation > 0)
      {
        if (this.inventory[this.selectedItem].useStyle == 1 || this.inventory[this.selectedItem].type == 0)
          this.bodyFrame.Y = (double) this.itemAnimation >= (double) this.itemAnimationMax * 0.333 ? ((double) this.itemAnimation >= (double) this.itemAnimationMax * 0.666 ? 100 : 150) : 200;
        else if (this.inventory[this.selectedItem].useStyle == 2)
          this.bodyFrame.Y = (double) this.itemAnimation >= (double) this.itemAnimationMax * 0.5 ? 200 : 150;
        else if (this.inventory[this.selectedItem].useStyle == 3)
          this.bodyFrame.Y = (double) this.itemAnimation <= (double) this.itemAnimationMax * 0.666 ? 200 : 100;
      }
      else
        this.bodyFrame.Y = this.inventory[this.selectedItem].holdStyle != 1 ? ((double) this.velocity.Y >= 0.0 ? ((double) this.velocity.Y <= 0.0 ? 0 : 50) : 50) : 200;
      if ((double) this.velocity.Y < 0.0)
        this.legFrame.Y = 100;
      else if ((double) this.velocity.Y > 0.0)
        this.legFrame.Y = 100;
      else if ((double) this.velocity.X != 0.0)
      {
        if (this.direction < 0 && (double) this.velocity.X > 0.0 || this.direction > 0 && (double) this.velocity.X < 0.0)
          this.legFrameCounter = 12.0;
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
      this.headPosition = new Vector2();
      this.bodyPosition = new Vector2();
      this.legPosition = new Vector2();
      this.headRotation = 0.0f;
      this.bodyRotation = 0.0f;
      this.legRotation = 0.0f;
      this.statLife = this.statLifeMax;
      this.immune = true;
      this.dead = false;
      this.immuneTime = 0;
      this.active = true;
      this.position.X = (float) (Game1.spawnTileX * 16 + 8 - this.width / 2);
      this.position.Y = (float) (Game1.spawnTileY * 16 - this.height);
      this.velocity.X = 0.0f;
      this.velocity.Y = 0.0f;
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
      Game1.screenPosition.X = this.position.X + (float) (this.width / 2) - (float) (Game1.screenWidth / 2);
      Game1.screenPosition.Y = this.position.Y + (float) (this.height / 2) - (float) (Game1.screenHeight / 2);
    }

    public void Hurt(int Damage, int hitDirection)
    {
      if (this.immune || Game1.godMode)
        return;
      double damage = Game1.CalculateDamage(Damage, this.statDefense);
      if (damage >= 1.0)
      {
        this.statLife -= (int) damage;
        this.immune = true;
        this.immuneTime = 40;
        this.velocity.X = 4.5f * (float) hitDirection;
        this.velocity.Y = -3.5f;
        int index1 = Game1.rand.Next(3);
        Game1.soundInstancePlayerHit[index1].Stop();
        Game1.soundInstancePlayerHit[index1] = Game1.soundPlayerHit[index1].CreateInstance();
        Game1.soundInstancePlayerHit[index1].Play();
        if (this.statLife > 0)
        {
          for (int index2 = 0; (double) index2 < damage / (double) this.statLifeMax * 100.0; ++index2)
            Dust.NewDust(this.position, this.width, this.height, 5, (float) (2 * hitDirection), -2f);
        }
        else
        {
          this.DropItems();
          Game1.soundPlayerKilled.Play();
          this.headVelocity.Y = (float) Game1.rand.Next(-40, -10) * 0.1f;
          this.bodyVelocity.Y = (float) Game1.rand.Next(-40, -10) * 0.1f;
          this.legVelocity.Y = (float) Game1.rand.Next(-40, -10) * 0.1f;
          this.headVelocity.X = (float) Game1.rand.Next(-20, 21) * 0.1f + (float) (2 * hitDirection);
          this.bodyVelocity.X = (float) Game1.rand.Next(-20, 21) * 0.1f + (float) (2 * hitDirection);
          this.legVelocity.X = (float) Game1.rand.Next(-20, 21) * 0.1f + (float) (2 * hitDirection);
          for (int index3 = 0; (double) index3 < 20.0 + damage / (double) this.statLifeMax * 100.0; ++index3)
            Dust.NewDust(this.position, this.width, this.height, 5, (float) (2 * hitDirection), -2f);
          this.dead = true;
          this.respawnTimer = 300;
          this.immuneAlpha = 0;
        }
      }
    }

    public bool ItemSpace(Item newItem)
    {
      for (int index = 0; index < 40; ++index)
      {
        if (this.inventory[index].type == 0)
          return true;
      }
      for (int index = 0; index < 40; ++index)
      {
        if (this.inventory[index].type > 0 && this.inventory[index].stack < this.inventory[index].maxStack && newItem.IsTheSameAs(this.inventory[index]))
          return true;
      }
      return false;
    }

    public Item GetItem(int plr, Item newItem)
    {
      Item obj = newItem;
      if (newItem.noGrabDelay > 0)
        return obj;
      for (int index = 0; index < 40; ++index)
      {
        if (this.inventory[index].type > 0 && this.inventory[index].stack < this.inventory[index].maxStack && obj.IsTheSameAs(this.inventory[index]))
        {
          Game1.soundInstanceGrab.Stop();
          Game1.soundInstanceGrab = Game1.soundGrab.CreateInstance();
          Game1.soundInstanceGrab.Play();
          if (obj.stack + this.inventory[index].stack <= this.inventory[index].maxStack)
          {
            this.inventory[index].stack += obj.stack;
            if (plr == Game1.myPlayer)
              Recipe.FindRecipes();
            return new Item();
          }
          obj.stack -= this.inventory[index].maxStack - this.inventory[index].stack;
          this.inventory[index].stack = this.inventory[index].maxStack;
          if (plr == Game1.myPlayer)
            Recipe.FindRecipes();
        }
      }
      for (int index = 0; index < 40; ++index)
      {
        if (this.inventory[index].type == 0)
        {
          this.inventory[index] = obj;
          Game1.soundInstanceGrab.Stop();
          Game1.soundInstanceGrab = Game1.soundGrab.CreateInstance();
          Game1.soundInstanceGrab.Play();
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
        this.releaseUseItem = true;
      if (this.controlUseItem && this.itemAnimation == 0 && this.releaseUseItem && this.inventory[this.selectedItem].useStyle > 0)
      {
        this.itemAnimation = this.inventory[this.selectedItem].useAnimation;
        this.itemAnimationMax = this.itemAnimation;
        if (this.inventory[this.selectedItem].useSound > 0)
          Game1.soundInstanceItem[this.inventory[this.selectedItem].useSound].Play();
      }
      if (this.itemAnimation > 0)
      {
        this.itemHeight = Game1.itemTexture[this.inventory[this.selectedItem].type].Height;
        this.itemWidth = Game1.itemTexture[this.inventory[this.selectedItem].type].Width;
        --this.itemAnimation;
        if (this.inventory[this.selectedItem].useStyle == 1)
        {
          if ((double) this.itemAnimation < (double) this.inventory[this.selectedItem].useAnimation * 0.333)
          {
            this.itemLocation.X = (float) ((double) this.position.X + (double) this.width * 0.5 + ((double) Game1.itemTexture[this.inventory[this.selectedItem].type].Width * 0.5 - 4.0) * (double) this.direction);
            this.itemLocation.Y = this.position.Y + 24f;
          }
          else if ((double) this.itemAnimation < (double) this.inventory[this.selectedItem].useAnimation * 0.666)
          {
            this.itemLocation.X = (float) ((double) this.position.X + (double) this.width * 0.5 + ((double) Game1.itemTexture[this.inventory[this.selectedItem].type].Width * 0.5 - 10.0) * (double) this.direction);
            this.itemLocation.Y = this.position.Y + 10f;
          }
          else
          {
            this.itemLocation.X = (float) ((double) this.position.X + (double) this.width * 0.5 - ((double) Game1.itemTexture[this.inventory[this.selectedItem].type].Width * 0.5 - 4.0) * (double) this.direction);
            this.itemLocation.Y = this.position.Y + 6f;
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
      }
      else if (this.inventory[this.selectedItem].holdStyle == 1)
      {
        this.itemLocation.X = (float) ((double) this.position.X + (double) this.width * 0.5 + ((double) Game1.itemTexture[this.inventory[this.selectedItem].type].Width * 0.5 + 4.0) * (double) this.direction);
        this.itemLocation.Y = this.position.Y + 24f;
        this.itemRotation = 0.0f;
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
          Lighting.addLight((int) (((double) this.itemLocation.X - 16.0 + (double) this.velocity.X) / 16.0), (int) (((double) this.itemLocation.Y - 14.0) / 16.0), byte.MaxValue);
        }
        else
        {
          Game1.screenPosition.X = (float) ((double) this.position.X + (double) this.width * 0.5 - (double) Game1.screenWidth * 0.5);
          Game1.screenPosition.Y = (float) ((double) this.position.Y + (double) this.height * 0.5 - (double) Game1.screenHeight * 0.5);
          if (Game1.rand.Next(maxValue) == 0)
            Dust.NewDust(new Vector2(this.itemLocation.X + 6f, this.itemLocation.Y - 14f), 4, 4, 6, Alpha: 100);
          Lighting.addLight((int) (((double) this.itemLocation.X + 6.0 + (double) this.velocity.X) / 16.0), (int) (((double) this.itemLocation.Y - 14.0) / 16.0), byte.MaxValue);
        }
      }
      this.releaseUseItem = !this.controlUseItem;
      if (this.itemTime > 0)
        --this.itemTime;
      if (i != Game1.myPlayer)
        return;
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
          if (Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 5 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 10 || Game1.tile[Player.tileTargetX, Player.tileTargetY].type == (byte) 11)
          {
            this.hitTile += this.inventory[this.selectedItem].axe;
            if (this.inventory[this.selectedItem].axe > 0)
            {
              if (this.hitTile >= 100)
              {
                this.hitTile = 0;
                WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY);
              }
              else
                WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY, true);
              this.itemTime = this.inventory[this.selectedItem].useTime;
            }
          }
          else if (this.inventory[this.selectedItem].pick > 0)
          {
            this.hitTile += this.inventory[this.selectedItem].pick;
            if (this.hitTile >= 100)
            {
              this.hitTile = 0;
              WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY);
            }
            else
              WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY, true);
            this.itemTime = this.inventory[this.selectedItem].useTime;
          }
        }
        if (Game1.tile[Player.tileTargetX, Player.tileTargetY].wall > (byte) 0 && this.itemTime == 0 && this.itemAnimation > 0 && this.controlUseItem && this.inventory[this.selectedItem].hammer > 0)
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
          }
          else
            WorldGen.KillWall(Player.tileTargetX, Player.tileTargetY, true);
          this.itemTime = this.inventory[this.selectedItem].useTime;
        }
      }
      if (this.inventory[this.selectedItem].createTile >= 0)
      {
        Player.tileTargetX = (int) (((double) Game1.mouseState.X + (double) Game1.screenPosition.X) / 16.0);
        Player.tileTargetY = (int) (((double) Game1.mouseState.Y + (double) Game1.screenPosition.Y) / 16.0);
        if ((double) this.position.X / 16.0 - (double) Player.tileRangeX - (double) this.inventory[this.selectedItem].tileBoost <= (double) Player.tileTargetX && ((double) this.position.X + (double) this.width) / 16.0 + (double) Player.tileRangeX + (double) this.inventory[this.selectedItem].tileBoost - 1.0 >= (double) Player.tileTargetX && (double) this.position.Y / 16.0 - (double) Player.tileRangeY - (double) this.inventory[this.selectedItem].tileBoost <= (double) Player.tileTargetY && ((double) this.position.Y + (double) this.height) / 16.0 + (double) Player.tileRangeY + (double) this.inventory[this.selectedItem].tileBoost - 2.0 >= (double) Player.tileTargetY)
        {
          this.showItemIcon = true;
          if (!Game1.tile[Player.tileTargetX, Player.tileTargetY].active && this.itemTime == 0 && this.itemAnimation > 0 && this.controlUseItem && (Game1.tile[Player.tileTargetX + 1, Player.tileTargetY].active || Game1.tile[Player.tileTargetX + 1, Player.tileTargetY].wall > (byte) 0 || Game1.tile[Player.tileTargetX - 1, Player.tileTargetY].active || Game1.tile[Player.tileTargetX - 1, Player.tileTargetY].wall > (byte) 0 || Game1.tile[Player.tileTargetX, Player.tileTargetY + 1].active || Game1.tile[Player.tileTargetX, Player.tileTargetY + 1].wall > (byte) 0 || Game1.tile[Player.tileTargetX, Player.tileTargetY - 1].active || Game1.tile[Player.tileTargetX, Player.tileTargetY - 1].wall > (byte) 0))
          {
            WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, this.inventory[this.selectedItem].createTile);
            if (Game1.tile[Player.tileTargetX, Player.tileTargetY].active)
              this.itemTime = this.inventory[this.selectedItem].useTime;
          }
        }
      }
      if (this.inventory[this.selectedItem].createWall >= 0)
      {
        Player.tileTargetX = (int) (((double) Game1.mouseState.X + (double) Game1.screenPosition.X) / 16.0);
        Player.tileTargetY = (int) (((double) Game1.mouseState.Y + (double) Game1.screenPosition.Y) / 16.0);
        if ((double) this.position.X / 16.0 - (double) Player.tileRangeX - (double) this.inventory[this.selectedItem].tileBoost <= (double) Player.tileTargetX && ((double) this.position.X + (double) this.width) / 16.0 + (double) Player.tileRangeX + (double) this.inventory[this.selectedItem].tileBoost - 1.0 >= (double) Player.tileTargetX && (double) this.position.Y / 16.0 - (double) Player.tileRangeY - (double) this.inventory[this.selectedItem].tileBoost <= (double) Player.tileTargetY && ((double) this.position.Y + (double) this.height) / 16.0 + (double) Player.tileRangeY + (double) this.inventory[this.selectedItem].tileBoost - 2.0 >= (double) Player.tileTargetY)
        {
          this.showItemIcon = true;
          if (this.itemTime == 0 && this.itemAnimation > 0 && this.controlUseItem && (Game1.tile[Player.tileTargetX + 1, Player.tileTargetY].active || Game1.tile[Player.tileTargetX + 1, Player.tileTargetY].wall > (byte) 0 || Game1.tile[Player.tileTargetX - 1, Player.tileTargetY].active || Game1.tile[Player.tileTargetX - 1, Player.tileTargetY].wall > (byte) 0 || Game1.tile[Player.tileTargetX, Player.tileTargetY + 1].active || Game1.tile[Player.tileTargetX, Player.tileTargetY + 1].wall > (byte) 0 || Game1.tile[Player.tileTargetX, Player.tileTargetY - 1].active || Game1.tile[Player.tileTargetX, Player.tileTargetY - 1].wall > (byte) 0) && (int) Game1.tile[Player.tileTargetX, Player.tileTargetY].wall != this.inventory[this.selectedItem].createWall)
          {
            WorldGen.PlaceWall(Player.tileTargetX, Player.tileTargetY, this.inventory[this.selectedItem].createWall);
            if ((int) Game1.tile[Player.tileTargetX, Player.tileTargetY].wall == this.inventory[this.selectedItem].createWall)
              this.itemTime = this.inventory[this.selectedItem].useTime;
          }
        }
      }
      if (this.inventory[this.selectedItem].damage >= 0 && this.inventory[this.selectedItem].type > 0 && this.itemAnimation > 0)
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
              rectangle1.X -= rectangle1.Width * 2;
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
          int num1 = rectangle1.X / 16;
          int num2 = (rectangle1.X + rectangle1.Width) / 16 + 1;
          int num3 = rectangle1.Y / 16;
          int num4 = (rectangle1.Y + rectangle1.Height) / 16 + 1;
          for (int i1 = num1; i1 < num2; ++i1)
          {
            for (int j = num3; j < num4; ++j)
            {
              if (Game1.tile[i1, j].type == (byte) 3)
                WorldGen.KillTile(i1, j);
            }
          }
          for (int index = 0; index < 1000; ++index)
          {
            if (Game1.npc[index].active && Game1.npc[index].immune[i] == 0)
            {
              Rectangle rectangle2 = new Rectangle((int) Game1.npc[index].position.X, (int) Game1.npc[index].position.Y, Game1.npc[index].width, Game1.npc[index].height);
              if (rectangle1.Intersects(rectangle2))
              {
                Game1.npc[index].StrikeNPC(this.inventory[this.selectedItem].damage, this.inventory[this.selectedItem].knockBack, this.direction);
                Game1.npc[index].immune[i] = this.itemAnimation;
              }
            }
          }
        }
      }
      if (this.itemTime == 0 && this.inventory[this.selectedItem].healLife > 0 && this.itemAnimation > 0)
      {
        this.statLife += this.inventory[this.selectedItem].healLife;
        this.itemTime = this.inventory[this.selectedItem].useTime;
      }
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
      int r = (int) newColor.R - this.immuneAlpha;
      int g = (int) newColor.G - this.immuneAlpha;
      int b = (int) newColor.B - this.immuneAlpha;
      int a = (int) newColor.A - this.immuneAlpha;
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
      for (int index1 = 10; index1 < 40; ++index1)
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

    public static void SetupPlayers()
    {
      for (int index1 = 0; index1 < 16; ++index1)
      {
        Game1.player[index1] = new Player();
        Game1.player[index1].name = "Some n00b";
        Game1.player[index1].armor[0] = new Item();
        Game1.player[index1].armor[1] = new Item();
        Game1.player[index1].armor[2] = new Item();
        for (int index2 = 0; index2 < 40; ++index2)
          Game1.player[index1].inventory[index2] = new Item();
        Game1.player[index1].inventory[0].SetDefaults("Copper Pickaxe");
        Game1.player[index1].inventory[1].SetDefaults("Copper Axe");
        Game1.player[index1].inventory[2].SetDefaults("Copper Hammer");
        Game1.player[index1].inventory[30].SetDefaults(16);
        Game1.player[index1].inventory[31].SetDefaults(18);
        Game1.player[index1].armor[1].SetDefaults(15);
        Game1.player[index1].armor[2].SetDefaults(17);
      }
    }
  }
}
