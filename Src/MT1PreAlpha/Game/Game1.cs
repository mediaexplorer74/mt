
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Diagnostics;


namespace GameManager
{

  // Game1 class Game1
  public class Game1 : Game
  {

    // Mouse support
    MouseState lastMouseState;
    Vector2 mouseposition = Vector2.Zero;

    // TouchPanel support
    TouchCollection lastTouchState;
    Vector2 touchposition = Vector2.Zero;

    public const float leftWorld = 0.0f;
    public const float rightWorld = 80000f;
    public const float topWorld = 0.0f;
    public const float bottomWorld = 40000f;
    public const int maxTilesX = 5001;
    public const int maxTilesY = 2501;
    public const int maxTileSets = 12;
    public const int maxWallTypes = 2;
    public const int maxBackgrounds = 3;
    public const int maxDust = 1000;
    public const int maxPlayers = 16;
    public const int maxItemTypes = 27;
    public const int maxItems = 1000;
    public const int maxNPCTypes = 2;
    public const int maxNPCs = 1000;
    public const int maxInventory = 40;
    public const int maxItemSounds = 2;
    public const int maxNPCHitSounds = 1;
    public const int maxNPCKilledSounds = 1;
    public const double dayLength = 40000.0;
    public const double nightLength = 30000.0;
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    // game control devices states
    public static MouseState mouseState = Mouse.GetState();
    public static MouseState oldMouseState = Mouse.GetState();
    public static KeyboardState keyState = Keyboard.GetState();

    // DEBUG (true for TEST MODE; false for GAME MODE)
    public static bool debugMode = true;//false;
    public static bool godMode = true;//false;
    public static bool dumbAI = false;
    
    public static int background = 0;
    public static Color tileColor;
    public static double worldSurface;
    public static bool dayTime = true;
    public static double time = 10000.0;
    public static int moonPhase = 0;
    public static Random rand = new Random();
    public static Texture2D playerHeadTexture;
    public static Texture2D playerBodyTexture;
    public static Texture2D playerLegTexture;
    public static Texture2D[] itemTexture = new Texture2D[27];
    public static Texture2D[] npcTexture = new Texture2D[2];
    public static Texture2D hotbarTexture;
    public static Texture2D cursorTexture;
    public static Texture2D dustTexture;
    public static Texture2D sunTexture;
    public static Texture2D moonTexture;
    public static Texture2D[] tileTexture = new Texture2D[12];
    public static Texture2D blackTileTexture;
    public static Texture2D[] wallTexture = new Texture2D[2];
    public static Texture2D[] backgroundTexture = new Texture2D[3];
    public static Texture2D heartTexture;
    public static Texture2D treeTopTexture;
    public static Texture2D treeBranchTexture;
    public static Texture2D inventoryBackTexture;
    public static SoundEffect[] soundDig = new SoundEffect[3];
    public static SoundEffectInstance[] soundInstanceDig = new SoundEffectInstance[3];
    public static SoundEffect[] soundPlayerHit = new SoundEffect[3];
    public static SoundEffectInstance[] soundInstancePlayerHit = new SoundEffectInstance[3];
    public static SoundEffect soundPlayerKilled;
    public static SoundEffectInstance soundInstancePlayerKilled;
    public static SoundEffect soundGrass;
    public static SoundEffectInstance soundInstanceGrass;
    public static SoundEffect soundGrab;
    public static SoundEffectInstance soundInstanceGrab;
    public static SoundEffect[] soundItem = new SoundEffect[3];
    public static SoundEffectInstance[] soundInstanceItem = new SoundEffectInstance[3];
    public static SoundEffect[] soundNPCHit = new SoundEffect[2];
    public static SoundEffectInstance[] soundInstanceNPCHit = new SoundEffectInstance[2];
    public static SoundEffect[] soundNPCKilled = new SoundEffect[2];
    public static SoundEffectInstance[] soundInstanceNPCKilled = new SoundEffectInstance[2];
    public static SoundEffect soundDoorOpen;
    public static SoundEffectInstance soundInstanceDoorOpen;
    public static SoundEffect soundDoorClosed;
    public static SoundEffectInstance soundInstanceDoorClosed;
    public static SoundEffect soundMenuOpen;
    public static SoundEffect soundMenuClose;
    public static SoundEffect soundMenuTick;
    public static SoundEffectInstance soundInstanceMenuTick;
    public static SpriteFont fontItemStack;
    public static SpriteFont fontMouseText;
    public static SpriteFont fontDeathText;
    public static bool[] tileSolid = new bool[12];
    public static bool[] tileNoFail = new bool[12];
    public static int[] backgroundWidth = new int[3];
    public static int[] backgroundHeight = new int[3];
    public static Tile[,] tile = new Tile[5001, 2501];
    public static Dust[] dust = new Dust[1000];
    public static Item[] item = new Item[1000];
    public static NPC[] npc = new NPC[1001];
    public static Vector2 screenPosition;
    public static int screenWidth = 800;
    public static int screenHeight = 600;
    public static float[] hotbarScale = new float[10]
    {
      1f,
      0.75f,
      0.75f,
      0.75f,
      0.75f,
      0.75f,
      0.75f,
      0.75f,
      0.75f,
      0.75f
    };
    public static byte mouseTextColor = 0;
    public static int mouseTextColorChange = 1;
    public static bool mouseLeftRelease = false;
    public static bool playerInventory = false;
    public static Item mouseItem = new Item();
    private static float inventoryScale = 0.75f;
    public static Recipe[] recipe = new Recipe[Recipe.maxRecipes];
    public static int[] availableRecipe = new int[Recipe.maxRecipes];
    public static float[] availableRecipeY = new float[Recipe.maxRecipes];
    public static int numAvailableRecipes;
    public static int focusRecipe;
    public static int myPlayer = 0;
    public static Player[] player = new Player[16];
    public static int spawnTileX;
    public static int spawnTileY;
    public bool toggleFullscreen;
    public static int[] npcFrameCount = new int[2]{ 1, 2 };
        private bool spaceDown;
        private bool gameStarted;
        private bool gameOver;

        public Game1()
    {
      this.graphics = new GraphicsDeviceManager((Game) this);
      this.Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
      this.Window.Title = "[M]icro[T]erraria: Dig Peon, Dig!"; // =)

      for (int index1 = 0; index1 < 5001; ++index1)
      {
        for (int index2 = 0; index2 < 2501; ++index2)
          Game1.tile[index1, index2] = new Tile();
      }
      Game1.tileSolid[0] = true;
      Game1.tileSolid[1] = true;
      Game1.tileSolid[2] = true;
      Game1.tileSolid[3] = false;
      Game1.tileNoFail[3] = true;
      Game1.tileSolid[4] = false;
      Game1.tileNoFail[4] = true;
      Game1.tileSolid[5] = false;
      Game1.tileSolid[6] = true;
      Game1.tileSolid[7] = true;
      Game1.tileSolid[8] = true;
      Game1.tileSolid[9] = true;
      Game1.tileSolid[10] = true;
      Game1.tileSolid[11] = false;
      for (int index = 0; index < 1000; ++index)
        Game1.dust[index] = new Dust();
      for (int index = 0; index < 1000; ++index)
        Game1.item[index] = new Item();
      for (int index = 0; index < 1000; ++index)
        Game1.npc[index] = new NPC();
      Player.SetupPlayers();
      for (int index = 0; index < Recipe.maxRecipes; ++index)
      {
        Game1.recipe[index] = new Recipe();
        Game1.availableRecipeY[index] = (float) (65 * index);
      }
      Recipe.SetupRecipes();

      this.graphics.PreferredBackBufferWidth = Game1.screenWidth;
      this.graphics.PreferredBackBufferHeight = Game1.screenHeight;
      this.graphics.ApplyChanges();

      WorldGen.generateWorld();
      base.Initialize();
    }

    protected override void LoadContent()
    {
      this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
      for (int index = 0; index < 12; ++index)
        Game1.tileTexture[index] = this.Content.Load<Texture2D>("Images\\Tiles_" + (object) index);
      for (int index = 1; index < 2; ++index)
        Game1.wallTexture[index] = this.Content.Load<Texture2D>("Images\\Wall_" + (object) index);
      for (int index = 0; index < 3; ++index)
      {
        Game1.backgroundTexture[index] = this.Content.Load<Texture2D>("Images\\Background_" + (object) index);
        Game1.backgroundWidth[index] = Game1.backgroundTexture[index].Width;
        Game1.backgroundHeight[index] = Game1.backgroundTexture[index].Height;
      }
      for (int index = 0; index < 27; ++index)
        Game1.itemTexture[index] = this.Content.Load<Texture2D>("Images\\Item_" + (object) index);
      for (int index = 0; index < 2; ++index)
        Game1.npcTexture[index] = this.Content.Load<Texture2D>("Images\\NPC_" + (object) index);
      Game1.hotbarTexture = this.Content.Load<Texture2D>("Images\\Hotbar");
      Game1.dustTexture = this.Content.Load<Texture2D>("Images\\Dust");
      Game1.sunTexture = this.Content.Load<Texture2D>("Images\\Sun");
      Game1.moonTexture = this.Content.Load<Texture2D>("Images\\Moon");
      Game1.blackTileTexture = this.Content.Load<Texture2D>("Images\\Black_Tile");
      Game1.heartTexture = this.Content.Load<Texture2D>("Images\\Heart");
      Game1.cursorTexture = this.Content.Load<Texture2D>("Images\\Cursor");
      Game1.treeTopTexture = this.Content.Load<Texture2D>("Images\\Tree_Tops");
      Game1.treeBranchTexture = this.Content.Load<Texture2D>("Images\\Tree_Branches");
      Game1.inventoryBackTexture = this.Content.Load<Texture2D>("Images\\Inventory_Back");
      Game1.playerHeadTexture = this.Content.Load<Texture2D>("Images\\Character_Heads");
      Game1.playerBodyTexture = this.Content.Load<Texture2D>("Images\\Character_Bodies");
      Game1.playerLegTexture = this.Content.Load<Texture2D>("Images\\Character_Legs");
      Game1.soundGrab = this.Content.Load<SoundEffect>("Sounds\\Grab");
      Game1.soundInstanceGrab = Game1.soundGrab.CreateInstance();
      Game1.soundDig[0] = this.Content.Load<SoundEffect>("Sounds\\Dig_0");
      Game1.soundInstanceDig[0] = Game1.soundDig[0].CreateInstance();
      Game1.soundDig[1] = this.Content.Load<SoundEffect>("Sounds\\Dig_1");
      Game1.soundInstanceDig[1] = Game1.soundDig[1].CreateInstance();
      Game1.soundDig[2] = this.Content.Load<SoundEffect>("Sounds\\Dig_2");
      Game1.soundInstanceDig[2] = Game1.soundDig[2].CreateInstance();
      Game1.soundPlayerHit[0] = this.Content.Load<SoundEffect>("Sounds\\Player_Hit_0");
      Game1.soundInstancePlayerHit[0] = Game1.soundPlayerHit[0].CreateInstance();
      Game1.soundPlayerHit[1] = this.Content.Load<SoundEffect>("Sounds\\Player_Hit_1");
      Game1.soundInstancePlayerHit[1] = Game1.soundPlayerHit[1].CreateInstance();
      Game1.soundPlayerHit[2] = this.Content.Load<SoundEffect>("Sounds\\Player_Hit_2");
      Game1.soundInstancePlayerHit[2] = Game1.soundPlayerHit[2].CreateInstance();
      Game1.soundPlayerKilled = this.Content.Load<SoundEffect>("Sounds\\Player_Killed");
      Game1.soundInstancePlayerKilled = Game1.soundPlayerKilled.CreateInstance();
      Game1.soundGrass = this.Content.Load<SoundEffect>("Sounds\\Grass");
      Game1.soundInstanceGrass = Game1.soundGrass.CreateInstance();
      Game1.soundDoorOpen = this.Content.Load<SoundEffect>("Sounds\\Door_Opened");
      Game1.soundInstanceDoorOpen = Game1.soundDoorOpen.CreateInstance();
      Game1.soundDoorClosed = this.Content.Load<SoundEffect>("Sounds\\Door_Closed");
      Game1.soundInstanceDoorClosed = Game1.soundDoorClosed.CreateInstance();
      Game1.soundMenuTick = this.Content.Load<SoundEffect>("Sounds\\Menu_Tick");
      Game1.soundInstanceMenuTick = Game1.soundMenuTick.CreateInstance();
      Game1.soundMenuOpen = this.Content.Load<SoundEffect>("Sounds\\Menu_Open");
      Game1.soundMenuClose = this.Content.Load<SoundEffect>("Sounds\\Menu_Close");
      for (int index = 1; index < 3; ++index)
      {
        Game1.soundItem[index] = this.Content.Load<SoundEffect>("Sounds\\Item_" + (object) index);
        Game1.soundInstanceItem[index] = Game1.soundItem[index].CreateInstance();
      }
      for (int index = 1; index < 2; ++index)
      {
        Game1.soundNPCHit[index] = this.Content.Load<SoundEffect>("Sounds\\NPC_Hit_" + (object) index);
        Game1.soundInstanceNPCHit[index] = Game1.soundNPCHit[index].CreateInstance();
      }
      for (int index = 1; index < 2; ++index)
      {
        Game1.soundNPCKilled[index] = this.Content.Load<SoundEffect>("Sounds\\NPC_Killed_" + (object) index);
        Game1.soundInstanceNPCKilled[index] = Game1.soundNPCKilled[index].CreateInstance();
      }
      Game1.fontItemStack = this.Content.Load<SpriteFont>("Fonts\\Item_Stack");
      Game1.fontMouseText = this.Content.Load<SpriteFont>("Fonts\\Mouse_Text");
      Game1.fontDeathText = this.Content.Load<SpriteFont>("Fonts\\Death_Text");
    }

    protected override void UnloadContent()
    {
      
    }


    // Handle user input from the keyboard
    public void MouseAndKeyboardHandler()
    {
        bool SpecialDebugEventHandlingNeeded = false;
        float PosX = 0;
        float PosY = 0;

        // ++++++++++++++ TOUCH PANEL ++++++++++++++++++++
        TouchCollection currentTouchState = TouchPanel.GetState();
        // Touch detected
        if (currentTouchState.Count > 0)
        {
            PosX = currentTouchState[0].Position.X;
            PosY = currentTouchState[0].Position.Y;

#region Future
            if 
            (    currentTouchState[0].Position.X != lastTouchState[0].Position.X
                || currentTouchState[0].Position.Y != lastTouchState[0].Position.Y
            )
            {
                Debug.WriteLine("[i] Event: Touchscreen touched!");
                SpecialDebugEventHandlingNeeded = true;

  #region D1
                    //DEBUG
                    //touchposition = new Vector2(currentTouchState[0].Position.X,
                    //    currentTouchState[0].Position.Y);

                    if (PosX < lastTouchState[0].Position.X)
                {
                    //dino.dX = dinoSpeedX * -1;

                }
                else if (PosX > lastTouchState[0].Position.X)
                {
                    //dino.dX = dinoSpeedX * 1;
                    //Game1.keyState.IsKeyDown...
                }
                else
                {
                    //dino.dX = 0;
                }
                    #endregion

                    // Experimental autopanning mode 
                    if (PosX < lastMouseState.X)
                        Game1.screenPosition.X -= 2f;

                    if (PosX > lastMouseState.X)
                        Game1.screenPosition.X += 2f;

                    if (PosY < lastMouseState.Y)
                        Game1.screenPosition.Y -= 2f;

                    if (PosY > lastMouseState.Y)
                        Game1.screenPosition.Y += 2f;

                }//if
#endregion
            }//if (currentTouchState.Count...


            // ============== MOUSE CONTROL ==================


            PosX = Game1.mouseState.Position.X;
            PosY = Game1.mouseState.Position.Y;

            // ******Experimental -- autopanning on mouse move =) ************

            if (PosX < lastMouseState.X)
                Game1.screenPosition.X -= 2f;

            if (PosX > lastMouseState.X)
                Game1.screenPosition.X += 2f;

            if (PosY < lastMouseState.Y)
                Game1.screenPosition.Y -= 2f;

            if (PosY > lastMouseState.Y)
                Game1.screenPosition.Y += 2f;

            // *******************************************

            //// Both mouse button pressed
            if (    Game1.mouseState.RightButton == ButtonState.Pressed
                 && Game1.mouseState.LeftButton  == ButtonState.Pressed  
                 && ((PosX != lastMouseState.X)  || (PosY != lastMouseState.Y))
                 )
            {
                Debug.WriteLine("[i] Event: Both left+right mouse button pressed!"); 
                SpecialDebugEventHandlingNeeded = true;          
            }
        // ===============================================


            // ============ KEYBOARD CONTOL ===================

            // Panning control via WASD keys
            if (Game1.keyState.IsKeyDown(/*Keys.Up*/Keys.W))
                Game1.screenPosition.Y -= 4f;//32f;

            if (Game1.keyState.IsKeyDown(/*Keys.Left*/Keys.A))
                Game1.screenPosition.X -= 4f;//32f;

            if (Game1.keyState.IsKeyDown(/*Keys.Down*/Keys.S))
                Game1.screenPosition.Y += 4f;//32f;

            if (Game1.keyState.IsKeyDown(/*Keys.Right*/Keys.D))
                Game1.screenPosition.X += 4f;//32f;

            // ===============================================


            // ++++++++++++++++++++++++++++++++++++++++++++++

            //// SpecialDebugEvent handler

            if (SpecialDebugEventHandlingNeeded)
        {
            Debug.WriteLine("[i] SpecialDebugEvent handled!");

            if (true)//(Game1.player[Game1.myPlayer].releaseUseItem)
            {
                //RnD
                Game1.player[Game1.myPlayer].releaseUseItem = true;

                int index = NPC.NewNPC
                (
                    (int)((double)PosX  + (double)Game1.screenPosition.X), 
                    (int)((double)PosY + (double)Game1.screenPosition.Y), 
                    1
                );
                Game1.dayTime = true;
                Game1.npc[index].name = "Yellow Slime";
                Game1.npc[index].scale = 1.2f;
                Game1.npc[index].damage = 15;
                Game1.npc[index].defense = 15;
                Game1.npc[index].life = 50;
                Game1.npc[index].lifeMax = Game1.npc[index].life;
                Game1.npc[index].color = new Color((int)byte.MaxValue, 200, 0, 100);
            }
        }
        // ++++++++++++++++++++++++++++++++++++++++++++++

        lastMouseState = Game1.mouseState;
        lastTouchState = currentTouchState;
        SpecialDebugEventHandlingNeeded = false;

    }//MouseAndKeyboardHandler


    // Update
     protected override void Update(GameTime gameTime)
    {
      if (!this.IsActive)
      {
        // Game windows is not active

        this.IsMouseVisible = true;        


        //RnD
        Game1.player[Game1.myPlayer].delayUseItem = true;
        
        //RnD
        Game1.mouseLeftRelease = false;
      }
      else
      {
        // Game windows is ACTIVE

        this.IsMouseVisible = false;

        //Experimental 
        MouseAndKeyboardHandler(); // Handle mouse&keyboard input

        Game1.oldMouseState = Game1.mouseState;
        Game1.mouseState = Mouse.GetState();
        Game1.keyState = Keyboard.GetState();

                //RnD -----------------------------------------------------------------
                #region RnD01
                /*
                if
                (
                    //Game1.keyState.IsKeyDown(Keys.LeftAlt) // not processing... so strange
                    //||
                    //Game1.keyState.IsKeyDown(Keys.RightAlt)
                    //&&
                    Game1.keyState.IsKeyDown(Keys.Enter)
                )
                {
                    Debug.WriteLine("[i] Event: FullScreen mode toggling on Enter key press");

                    this.graphics.ToggleFullScreen();
                    if (this.toggleFullscreen)
                    {
                        Debug.WriteLine("[i] Event: FullScreen mode ON");
                        //this.toggleFullscreen = false;
                    }
                    else
                    {
                        Debug.WriteLine("[i] Event: FullScreen mode OFF");
                        //this.graphics.ToggleFullScreen();
                        //this.toggleFullscreen = true;
                    }

                }
                */
                #endregion
                //--------------------------------------------------------------


        // RnD: check debug mode state
        if (Game1.debugMode)
        {
            //TEMP
            //Game1.UpdateDebug();
        }

        // update players 0-15
        for (int i = 0; i < 16; ++i)
        {
            Game1.player[i].UpdatePlayer(i);
        }

        NPC.SpawnNPC();

        for (int index = 0; index < 16; ++index)
        {
            Game1.player[index].activeNPCs = 0;
        }

        for (int i = 0; i < 1000; ++i)
        {
            Game1.npc[i].UpdateNPC(i);
        }
               
        for (int i = 0; i < 1000; ++i)
        {
            Game1.item[i].UpdateItem(i);
        }
        
        Dust.UpdateDust();
        
        Game1.UpdateTime();

        WorldGen.UpdateWorld();

        base.Update(gameTime);
      }
    }//Update


    // Draw
    protected override void Draw(GameTime gameTime)
    {
      Game1.player[Game1.myPlayer].mouseInterface = false;
      if (!this.IsActive)
        return;
      bool flag = false;
      if (!Game1.debugMode)
      {
        int num1 = Game1.mouseState.X;
        int num2 = Game1.mouseState.Y;

        if (num1 < 0)
          num1 = 0;

        if (num1 > Game1.screenWidth)
        {
          int screenWidth = Game1.screenWidth;
        }
        if (num2 < 0)
          num2 = 0;
        if (num2 > Game1.screenHeight)
        {
          int screenHeight = Game1.screenHeight;
        }
        Game1.screenPosition.X = (float) ((double) Game1.player[Game1.myPlayer].position.X + (double) Game1.player[Game1.myPlayer].width * 0.5 - (double) Game1.screenWidth * 0.5);
        Game1.screenPosition.Y = (float) ((double) Game1.player[Game1.myPlayer].position.Y + (double) Game1.player[Game1.myPlayer].height * 0.5 - (double) Game1.screenHeight * 0.5);
      }
      Game1.screenPosition.X = (float) (int) Game1.screenPosition.X;
      Game1.screenPosition.Y = (float) (int) Game1.screenPosition.Y;
      if ((double) Game1.screenPosition.X < 0.0)
        Game1.screenPosition.X = 0.0f;
      else if ((double) Game1.screenPosition.X + (double) Game1.screenWidth > 80000.0)
        Game1.screenPosition.X = 80000f - (float) Game1.screenWidth;
      if ((double) Game1.screenPosition.Y < 0.0)
        Game1.screenPosition.Y = 0.0f;
      else if ((double) Game1.screenPosition.Y + (double) Game1.screenHeight > 40000.0)
        Game1.screenPosition.Y = 40000f - (float) Game1.screenHeight;
      this.GraphicsDevice.Clear(Color.Black);
      base.Draw(gameTime);
      this.spriteBatch.Begin();
      double num3 = 0.5;
      int num4 = (int) (-Math.IEEERemainder((double) Game1.screenPosition.X * num3, (double) Game1.backgroundWidth[Game1.background]) - (double) (Game1.backgroundWidth[Game1.background] / 2));
      int num5 = Game1.screenWidth / Game1.backgroundWidth[Game1.background] + 2;
      int y1 = (int) (-(double) Game1.screenPosition.Y / (Game1.worldSurface * 16.0 - (double) Game1.screenHeight) * (double) (Game1.backgroundHeight[Game1.background] - Game1.screenHeight));
      Color white1 = Color.White;
      int x1 = (int) (Game1.time / 40000.0 * (double) (Game1.screenWidth + Game1.sunTexture.Width * 2)) - Game1.sunTexture.Width;
      int y2 = 0;
      Color white2 = Color.White;
      float scale1 = 1f;
      float rotation1 = (float) (Game1.time / 40000.0 * 2.0 - 7.3000001907348633);
      int x2 = (int) (Game1.time / 30000.0 * (double) (Game1.screenWidth + Game1.moonTexture.Width * 2)) - Game1.moonTexture.Width;
      int y3 = 0;
      Color white3 = Color.White;
      float scale2 = 1f;
      float rotation2 = (float) (Game1.time / 30000.0 * 2.0 - 7.3000001907348633);
      if (Game1.dayTime)
      {
        double num6;
        if (Game1.time < 20000.0)
        {
          num6 = Math.Pow(1.0 - Game1.time / 40000.0 * 2.0, 2.0);
          y2 = (int) ((double) y1 + num6 * 250.0 + 180.0);
        }
        else
        {
          num6 = Math.Pow((Game1.time / 40000.0 - 0.5) * 2.0, 2.0);
          y2 = (int) ((double) y1 + num6 * 250.0 + 180.0);
        }
        scale1 = (float) (1.2 - num6 * 0.4);
      }
      else
      {
        double num7;
        if (Game1.time < 15000.0)
        {
          num7 = Math.Pow(1.0 - Game1.time / 30000.0 * 2.0, 2.0);
          y3 = (int) ((double) y1 + num7 * 250.0 + 180.0);
        }
        else
        {
          num7 = Math.Pow((Game1.time / 30000.0 - 0.5) * 2.0, 2.0);
          y3 = (int) ((double) y1 + num7 * 250.0 + 180.0);
        }
        scale2 = (float) (1.2 - num7 * 0.4);
      }
      if (Game1.dayTime)
      {
        if (Game1.time < 10000.0)
        {
          float num8 = (float) (Game1.time / 10000.0);
          white2.R = (byte) ((double) num8 * 200.0 + 55.0);
          white2.G = (byte) ((double) num8 * 180.0 + 75.0);
          white2.B = (byte) ((double) num8 * 250.0 + 5.0);
          white1.R = (byte) ((double) num8 * 200.0 + 55.0);
          white1.G = (byte) ((double) num8 * 200.0 + 55.0);
          white1.B = (byte) ((double) num8 * 200.0 + 55.0);
        }
        if (Game1.time > 34000.0)
        {
          float num9 = (float) (1.0 - (Game1.time / 40000.0 - 0.85) * (20.0 / 3.0));
          white2.R = (byte) ((double) num9 * 120.0 + 55.0);
          white2.G = (byte) ((double) num9 * 100.0 + 25.0);
          white2.B = (byte) ((double) num9 * 120.0 + 55.0);
          white1.R = (byte) ((double) num9 * 200.0 + 55.0);
          white1.G = (byte) ((double) num9 * 85.0 + 55.0);
          white1.B = (byte) ((double) num9 * 135.0 + 55.0);
        }
        else if (Game1.time > 28000.0)
        {
          float num10 = (float) (1.0 - (Game1.time / 40000.0 - 0.7) * (20.0 / 3.0));
          white2.R = (byte) ((double) num10 * 80.0 + 175.0);
          white2.G = (byte) ((double) num10 * 130.0 + 125.0);
          white2.B = (byte) ((double) num10 * 100.0 + 155.0);
          white1.R = (byte) ((double) num10 * 0.0 + (double) byte.MaxValue);
          white1.G = (byte) ((double) num10 * 115.0 + 140.0);
          white1.B = (byte) ((double) num10 * 75.0 + 180.0);
        }
      }
      if (!Game1.dayTime)
      {
        if (Game1.time < 15000.0)
        {
          float num11 = (float) (1.0 - Game1.time / 15000.0);
          white3.R = (byte) ((double) num11 * 10.0 + 205.0);
          white3.G = (byte) ((double) num11 * 70.0 + 155.0);
          white3.B = (byte) ((double) num11 * 100.0 + 155.0);
          white1.R = (byte) ((double) num11 * 40.0 + 15.0);
          white1.G = (byte) ((double) num11 * 40.0 + 15.0);
          white1.B = (byte) ((double) num11 * 40.0 + 15.0);
        }
        else if (Game1.time >= 15000.0)
        {
          float num12 = (float) ((Game1.time / 30000.0 - 0.5) * 2.0);
          white3.R = (byte) ((double) num12 * 50.0 + 205.0);
          white3.G = (byte) ((double) num12 * 100.0 + 155.0);
          white3.B = (byte) ((double) num12 * 100.0 + 155.0);
          white1.R = (byte) ((double) num12 * 40.0 + 15.0);
          white1.G = (byte) ((double) num12 * 40.0 + 15.0);
          white1.B = (byte) ((double) num12 * 40.0 + 15.0);
        }
      }
      Game1.tileColor.A = byte.MaxValue;
      Game1.tileColor.R = (byte) (((int) white1.R + (int) white1.B + (int) white1.G) / 3);
      Game1.tileColor.G = (byte) (((int) white1.R + (int) white1.B + (int) white1.G) / 3);
      Game1.tileColor.B = (byte) (((int) white1.R + (int) white1.B + (int) white1.G) / 3);
      for (int index = 0; index < num5; ++index)
        this.spriteBatch.Draw(Game1.backgroundTexture[Game1.background], new Rectangle(num4 + Game1.backgroundWidth[Game1.background] * index, y1, Game1.backgroundWidth[Game1.background], Game1.backgroundHeight[Game1.background]), white1);
      if (Game1.dayTime)
        this.spriteBatch.Draw(Game1.sunTexture, new Vector2((float) x1, (float) y2), new Rectangle?(new Rectangle(0, 0, Game1.sunTexture.Width, Game1.sunTexture.Height)), white2, rotation1, new Vector2((float) (Game1.sunTexture.Width / 2), (float) (Game1.sunTexture.Height / 2)), scale1, SpriteEffects.None, 0.0f);
      if (!Game1.dayTime)
        this.spriteBatch.Draw(Game1.moonTexture, new Vector2((float) x2, (float) y3), new Rectangle?(new Rectangle(0, Game1.moonTexture.Width * Game1.moonPhase, Game1.moonTexture.Width, Game1.moonTexture.Width)), white3, rotation2, new Vector2((float) (Game1.moonTexture.Width / 2), (float) (Game1.moonTexture.Width / 2)), scale2, SpriteEffects.None, 0.0f);
      int firstX = (int) ((double) Game1.screenPosition.X / 16.0 - 1.0);
      int lastX = (int) (((double) Game1.screenPosition.X + (double) Game1.screenWidth) / 16.0) + 2;
      int firstY = (int) ((double) Game1.screenPosition.Y / 16.0 - 1.0);
      int lastY = (int) (((double) Game1.screenPosition.Y + (double) Game1.screenHeight) / 16.0) + 2;
      if (firstX < 0)
        firstX = 0;
      if (lastX > 5001)
        lastX = 5001;
      if (firstY < 0)
        firstY = 0;
      if (lastY > 2501)
        lastY = 2501;
      Lighting.LightTiles(firstX, lastX, firstY, lastY);
      Color white4 = Color.White;
      double num13 = 1.0;
      int num14 = (int) (-Math.IEEERemainder((double) Game1.screenPosition.X * num13, (double) Game1.backgroundWidth[1]) - (double) (Game1.backgroundWidth[1] / 2));
      int num15 = Game1.screenWidth / Game1.backgroundWidth[1] + 2;
      int y4 = (int) ((double) ((int) Game1.worldSurface * 16 - Game1.backgroundHeight[1]) - (double) Game1.screenPosition.Y + 16.0);
      for (int index1 = 0; index1 < num15; ++index1)
      {
        for (int index2 = 0; index2 < 6; ++index2)
        {
          int index3 = (int) (((double) (num14 + Game1.backgroundWidth[1] * index1) + (double) Game1.screenPosition.X + (double) (index2 * 16)) / 16.0 - (double) firstX + 21.0);
          int index4 = (int) (((double) y4 + (double) Game1.screenPosition.Y) / 16.0 - (double) firstY + 21.0);
          if (index3 < 0)
            index3 = 0;
          if (index3 >= Game1.screenWidth / 16 + 42 + 10)
            index3 = Game1.screenWidth / 16 + 42 + 10 - 1;
          if (index4 < 0)
            index4 = 0;
          if (index4 >= Game1.screenHeight / 16 + 42 + 10)
            index4 = Game1.screenHeight / 16 + 42 + 10 - 1;
          Color color = Lighting.color[index3, index4];
          this.spriteBatch.Draw(Game1.backgroundTexture[1], new Vector2((float) (num14 + Game1.backgroundWidth[1] * index1 + 16 * index2), (float) y4), new Rectangle?(new Rectangle(16 * index2, 0, 16, 16)), color);
        }
      }
      int x3 = (int) ((double) ((int) Game1.worldSurface * 16) - (double) Game1.screenPosition.Y + 16.0);
      if (Game1.worldSurface * 16.0 <= (double) Game1.screenPosition.Y + (double) Game1.screenHeight)
      {
        double num16 = 1.0;
        int num17 = (int) (-Math.IEEERemainder(100.0 + (double) Game1.screenPosition.X * num16, (double) Game1.backgroundWidth[2]) - (double) (Game1.backgroundWidth[2] / 2));
        int num18 = Game1.screenWidth / Game1.backgroundWidth[2] + 2;
        int num19;
        int num20;
        if (Game1.worldSurface * 16.0 < (double) Game1.screenPosition.Y)
        {
          num19 = (int) (Math.IEEERemainder((double) x3, (double) Game1.backgroundHeight[2]) - (double) Game1.backgroundHeight[2]);
          num20 = Game1.screenHeight / Game1.backgroundHeight[2] + 2;
        }
        else
        {
          num19 = x3;
          num20 = (Game1.screenHeight - x3) / Game1.backgroundHeight[2] + 1;
        }
        for (int index5 = 0; index5 < num18; ++index5)
        {
          for (int index6 = 0; index6 < num20; ++index6)
            this.spriteBatch.Draw(Game1.backgroundTexture[2], new Rectangle(num17 + Game1.backgroundWidth[2] * index5, num19 + Game1.backgroundHeight[2] * index6, Game1.backgroundWidth[2], Game1.backgroundHeight[2]), Color.White);
        }
      }
      for (int index7 = firstY; index7 < lastY + 4; ++index7)
      {
        for (int index8 = firstX - 2; index8 < lastX + 2; ++index8)
        {
          if ((int) Lighting.color[index8 - firstX + 21, index7 - firstY + 21].R < (int) Game1.tileColor.R - 10 || (double) index7 > Game1.worldSurface)
          {
            int num21 = (int) byte.MaxValue - (int) Lighting.color[index8 - firstX + 21, index7 - firstY + 21].R;
            if (num21 < 0)
              num21 = 0;
            if (num21 > (int) byte.MaxValue)
              num21 = (int) byte.MaxValue;
            white4.A = (byte) num21;
            this.spriteBatch.Draw(Game1.blackTileTexture, new Vector2((float) (index8 * 16 - (int) Game1.screenPosition.X), (float) (index7 * 16 - (int) Game1.screenPosition.Y)), new Rectangle?(new Rectangle((int) Game1.tile[index8, index7].frameX, (int) Game1.tile[index8, index7].frameY, 16, 16)), white4, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
          }
          if (Game1.tile[index8, index7].wall > (byte) 0)
            this.spriteBatch.Draw(Game1.wallTexture[(int) Game1.tile[index8, index7].wall], new Vector2((float) (index8 * 16 - (int) Game1.screenPosition.X - 8), (float) (index7 * 16 - (int) Game1.screenPosition.Y - 8)), new Rectangle?(new Rectangle((int) Game1.tile[index8, index7].wallFrameX * 2, (int) Game1.tile[index8, index7].wallFrameY * 2, 32, 32)), Lighting.color[index8 - firstX + 21, index7 - firstY + 21], 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
        }
      }
      for (int index9 = firstY; index9 < lastY + 4; ++index9)
      {
        for (int index10 = firstX - 2; index10 < lastX + 2; ++index10)
        {
          if (Game1.tile[index10, index9].active)
          {
            int height = Game1.tile[index10, index9].type != (byte) 3 && Game1.tile[index10, index9].type != (byte) 4 && Game1.tile[index10, index9].type != (byte) 5 ? 16 : 20;
            int width = Game1.tile[index10, index9].type != (byte) 4 && Game1.tile[index10, index9].type != (byte) 5 ? 16 : 20;
            if (Game1.tile[index10, index9].type == (byte) 4 && Game1.rand.Next(40) == 0)
            {
              if (Game1.tile[index10, index9].frameX == (short) 22)
                Dust.NewDust(new Vector2((float) (index10 * 16 + 6), (float) (index9 * 16)), 4, 4, 6, Alpha: 100);
              if (Game1.tile[index10, index9].frameX == (short) 44)
                Dust.NewDust(new Vector2((float) (index10 * 16 + 2), (float) (index9 * 16)), 4, 4, 6, Alpha: 100);
              else
                Dust.NewDust(new Vector2((float) (index10 * 16 + 4), (float) (index9 * 16)), 4, 4, 6, Alpha: 100);
            }
            if (Game1.tile[index10, index9].type == (byte) 5 && Game1.tile[index10, index9].frameY >= (short) 198 && Game1.tile[index10, index9].frameX >= (short) 22)
            {
              int num22 = 0;
              if (Game1.tile[index10, index9].frameX == (short) 22)
              {
                if (Game1.tile[index10, index9].frameY == (short) 220)
                  num22 = 1;
                else if (Game1.tile[index10, index9].frameY == (short) 242)
                  num22 = 2;
                this.spriteBatch.Draw(Game1.treeTopTexture, new Vector2((float) (index10 * 16 - (int) Game1.screenPosition.X - 32), (float) (index9 * 16 - (int) Game1.screenPosition.Y - 64)), new Rectangle?(new Rectangle(num22 * 82, 0, 80, 80)), Lighting.color[index10 - firstX + 21, index9 - firstY + 21], 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
              }
              else if (Game1.tile[index10, index9].frameX == (short) 44)
              {
                if (Game1.tile[index10, index9].frameY == (short) 220)
                  num22 = 1;
                else if (Game1.tile[index10, index9].frameY == (short) 242)
                  num22 = 2;
                this.spriteBatch.Draw(Game1.treeBranchTexture, new Vector2((float) (index10 * 16 - (int) Game1.screenPosition.X - 24), (float) (index9 * 16 - (int) Game1.screenPosition.Y - 12)), new Rectangle?(new Rectangle(0, num22 * 42, 40, 40)), Lighting.color[index10 - firstX + 21, index9 - firstY + 21], 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
              }
              else if (Game1.tile[index10, index9].frameX == (short) 66)
              {
                if (Game1.tile[index10, index9].frameY == (short) 220)
                  num22 = 1;
                else if (Game1.tile[index10, index9].frameY == (short) 242)
                  num22 = 2;
                this.spriteBatch.Draw(Game1.treeBranchTexture, new Vector2((float) (index10 * 16 - (int) Game1.screenPosition.X), (float) (index9 * 16 - (int) Game1.screenPosition.Y - 12)), new Rectangle?(new Rectangle(42, num22 * 42, 40, 40)), Lighting.color[index10 - firstX + 21, index9 - firstY + 21], 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
              }
            }
            this.spriteBatch.Draw(Game1.tileTexture[(int) Game1.tile[index10, index9].type], new Vector2((float) (index10 * 16 - (int) Game1.screenPosition.X) - (float) (((double) width - 16.0) / 2.0), (float) (index9 * 16 - (int) Game1.screenPosition.Y)), new Rectangle?(new Rectangle((int) Game1.tile[index10, index9].frameX, (int) Game1.tile[index10, index9].frameY, width, height)), Lighting.color[index10 - firstX + 21, index9 - firstY + 21], 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
          }
        }
      }
      for (int index = 0; index < 16; ++index)
      {
        if (Game1.player[index].active)
        {
          SpriteEffects effects = Game1.player[index].direction != -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
          Color newColor1 = Lighting.color[Lighting.LightingX((int) ((double) Game1.player[index].position.X + (double) Game1.player[index].width * 0.5) / 16 - firstX + 21), Lighting.LightingY((int) ((double) Game1.player[index].position.Y + (double) Game1.player[index].height * 0.25) / 16 - firstY + 21)];
          this.spriteBatch.Draw(Game1.playerHeadTexture, new Vector2((float) (int) ((double) Game1.player[index].position.X - (double) Game1.screenPosition.X - (double) (Game1.player[index].headFrame.Width / 2) + (double) (Game1.player[index].width / 2)), (float) (int) ((double) Game1.player[index].position.Y - (double) Game1.screenPosition.Y + (double) Game1.player[index].height - (double) Game1.player[index].headFrame.Height + 2.0)) + Game1.player[index].headPosition + new Vector2(16f, 14f), new Rectangle?(Game1.player[index].headFrame), Game1.player[index].GetImmuneAlpha(newColor1), Game1.player[index].headRotation, new Vector2(16f, 14f), 1f, effects, 0.0f);
          Color newColor2 = Lighting.color[Lighting.LightingX((int) ((double) Game1.player[index].position.X + (double) Game1.player[index].width * 0.5) / 16 - firstX + 21), Lighting.LightingY((int) ((double) Game1.player[index].position.Y + (double) Game1.player[index].height * 0.5) / 16 - firstY + 21)];
          if ((Game1.player[index].itemAnimation > 0 || Game1.player[index].inventory[Game1.player[index].selectedItem].holdStyle > 0) && Game1.player[index].inventory[Game1.player[index].selectedItem].type > 0)
          {
            this.spriteBatch.Draw(Game1.itemTexture[Game1.player[index].inventory[Game1.player[index].selectedItem].type], new Vector2((float) (int) ((double) Game1.player[index].itemLocation.X - (double) Game1.screenPosition.X), (float) (int) ((double) Game1.player[index].itemLocation.Y - (double) Game1.screenPosition.Y)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.player[index].inventory[Game1.player[index].selectedItem].type].Width, Game1.itemTexture[Game1.player[index].inventory[Game1.player[index].selectedItem].type].Height)), Game1.player[index].inventory[Game1.player[index].selectedItem].GetAlpha(newColor2), Game1.player[index].itemRotation, new Vector2((float) ((double) Game1.itemTexture[Game1.player[index].inventory[Game1.player[index].selectedItem].type].Width * 0.5 - (double) Game1.itemTexture[Game1.player[index].inventory[Game1.player[index].selectedItem].type].Width * 0.5 * (double) Game1.player[index].direction), (float) Game1.itemTexture[Game1.player[index].inventory[Game1.player[index].selectedItem].type].Height), Game1.player[index].inventory[Game1.player[index].selectedItem].scale, effects, 0.0f);
            if (Game1.player[index].inventory[Game1.player[index].selectedItem].color != new Color())
              this.spriteBatch.Draw(Game1.itemTexture[Game1.player[index].inventory[Game1.player[index].selectedItem].type], new Vector2((float) (int) ((double) Game1.player[index].itemLocation.X - (double) Game1.screenPosition.X), (float) (int) ((double) Game1.player[index].itemLocation.Y - (double) Game1.screenPosition.Y)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.player[index].inventory[Game1.player[index].selectedItem].type].Width, Game1.itemTexture[Game1.player[index].inventory[Game1.player[index].selectedItem].type].Height)), Game1.player[index].inventory[Game1.player[index].selectedItem].GetColor(newColor2), Game1.player[index].itemRotation, new Vector2((float) ((double) Game1.itemTexture[Game1.player[index].inventory[Game1.player[index].selectedItem].type].Width * 0.5 - (double) Game1.itemTexture[Game1.player[index].inventory[Game1.player[index].selectedItem].type].Width * 0.5 * (double) Game1.player[index].direction), (float) Game1.itemTexture[Game1.player[index].inventory[Game1.player[index].selectedItem].type].Height), Game1.player[index].inventory[Game1.player[index].selectedItem].scale, effects, 0.0f);
          }
          this.spriteBatch.Draw(Game1.playerBodyTexture, new Vector2((float) (int) ((double) Game1.player[index].position.X - (double) Game1.screenPosition.X - (double) (Game1.player[index].bodyFrame.Width / 2) + (double) (Game1.player[index].width / 2)), (float) (int) ((double) Game1.player[index].position.Y - (double) Game1.screenPosition.Y + (double) Game1.player[index].height - (double) Game1.player[index].bodyFrame.Height + 2.0)) + Game1.player[index].bodyPosition + new Vector2(16f, 28f), new Rectangle?(Game1.player[index].bodyFrame), Game1.player[index].GetImmuneAlpha(newColor2), Game1.player[index].bodyRotation, new Vector2(16f, 28f), 1f, effects, 0.0f);
          Color newColor3 = Lighting.color[Lighting.LightingX((int) ((double) Game1.player[index].position.X + (double) Game1.player[index].width * 0.5) / 16 - firstX + 21), Lighting.LightingY((int) ((double) Game1.player[index].position.Y + (double) Game1.player[index].height * 0.75) / 16 - firstY + 21)];
          this.spriteBatch.Draw(Game1.playerLegTexture, new Vector2((float) (int) ((double) Game1.player[index].position.X - (double) Game1.screenPosition.X - (double) (Game1.player[index].legFrame.Width / 2) + (double) (Game1.player[index].width / 2)), (float) (int) ((double) Game1.player[index].position.Y - (double) Game1.screenPosition.Y + (double) Game1.player[index].height - (double) Game1.player[index].legFrame.Height + 2.0)) + Game1.player[index].legPosition + new Vector2(16f, 40f), new Rectangle?(Game1.player[index].legFrame), Game1.player[index].GetImmuneAlpha(newColor3), Game1.player[index].legRotation, new Vector2(16f, 40f), 1f, effects, 0.0f);
        }
      }
      Rectangle rectangle1 = new Rectangle((int) Game1.screenPosition.X, (int) Game1.screenPosition.Y, Game1.screenWidth, Game1.screenHeight);
      for (int index = 0; index < 1000; ++index)
      {
        if (rectangle1.Intersects(new Rectangle((int) Game1.npc[index].position.X, (int) Game1.npc[index].position.Y, Game1.npc[index].width, Game1.npc[index].height)))
        {
          Color newColor = Lighting.color[(int) ((double) Game1.npc[index].position.X + (double) Game1.npc[index].width * 0.5) / 16 - firstX + 21, (int) ((double) Game1.npc[index].position.Y + (double) Game1.npc[index].height * 0.5) / 16 - firstY + 21];
          if (Game1.npc[index].active && Game1.npc[index].type > 0)
          {
            this.spriteBatch.Draw(Game1.npcTexture[Game1.npc[index].type], new Vector2((float) ((double) Game1.npc[index].position.X - (double) Game1.screenPosition.X + (double) (Game1.npc[index].width / 2) - (double) Game1.npcTexture[Game1.npc[index].type].Width * (double) Game1.npc[index].scale / 2.0), (float) ((double) Game1.npc[index].position.Y - (double) Game1.screenPosition.Y + (double) Game1.npc[index].height - (double) Game1.npcTexture[Game1.npc[index].type].Height * (double) Game1.npc[index].scale / (double) Game1.npcFrameCount[Game1.npc[index].type] + 4.0)), new Rectangle?(Game1.npc[index].frame), Game1.npc[index].GetAlpha(newColor), 0.0f, new Vector2(), Game1.npc[index].scale, SpriteEffects.None, 0.0f);
            if (Game1.npc[index].color != new Color())
              this.spriteBatch.Draw(Game1.npcTexture[Game1.npc[index].type], new Vector2((float) ((double) Game1.npc[index].position.X - (double) Game1.screenPosition.X + (double) (Game1.npc[index].width / 2) - (double) Game1.npcTexture[Game1.npc[index].type].Width * (double) Game1.npc[index].scale / 2.0), (float) ((double) Game1.npc[index].position.Y - (double) Game1.screenPosition.Y + (double) Game1.npc[index].height - (double) Game1.npcTexture[Game1.npc[index].type].Height * (double) Game1.npc[index].scale / (double) Game1.npcFrameCount[Game1.npc[index].type] + 4.0)), new Rectangle?(Game1.npc[index].frame), Game1.npc[index].GetColor(newColor), 0.0f, new Vector2(), Game1.npc[index].scale, SpriteEffects.None, 0.0f);
          }
        }
      }
      for (int index = 0; index < 1000; ++index)
      {
        if (Game1.item[index].active && Game1.item[index].type > 0)
        {
          int lightX = (int) ((double) Game1.item[index].position.X + (double) Game1.item[index].width * 0.5) / 16 - firstX + 21;
          int lightY = (int) ((double) Game1.item[index].position.Y + (double) Game1.item[index].height * 0.5) / 16 - firstY + 21;
          Color newColor = Lighting.color[Lighting.LightingX(lightX), Lighting.LightingY(lightY)];
          this.spriteBatch.Draw(Game1.itemTexture[Game1.item[index].type], new Vector2(Game1.item[index].position.X - Game1.screenPosition.X + (float) (Game1.item[index].width / 2) - (float) (Game1.itemTexture[Game1.item[index].type].Width / 2), Game1.item[index].position.Y - Game1.screenPosition.Y + (float) (Game1.item[index].height / 2) - (float) (Game1.itemTexture[Game1.item[index].type].Height / 2)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.item[index].type].Width, Game1.itemTexture[Game1.item[index].type].Height)), Game1.item[index].GetAlpha(newColor), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
          if (Game1.item[index].color != new Color())
            this.spriteBatch.Draw(Game1.itemTexture[Game1.item[index].type], new Vector2(Game1.item[index].position.X - Game1.screenPosition.X + (float) (Game1.item[index].width / 2) - (float) (Game1.itemTexture[Game1.item[index].type].Width / 2), Game1.item[index].position.Y - Game1.screenPosition.Y + (float) (Game1.item[index].height / 2) - (float) (Game1.itemTexture[Game1.item[index].type].Height / 2)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.item[index].type].Width, Game1.itemTexture[Game1.item[index].type].Height)), Game1.item[index].GetColor(newColor), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
        }
      }
      for (int index = 0; index < 1000; ++index)
      {
        if (Game1.dust[index].active)
        {
          Color white5 = Lighting.color[Lighting.LightingX((int) ((double) Game1.dust[index].position.X + 4.0) / 16 - firstX + 21), Lighting.LightingY((int) ((double) Game1.dust[index].position.Y + 4.0) / 16 - firstY + 21)];
          if (Game1.dust[index].type == 6)
            white5 = Color.White;
          this.spriteBatch.Draw(Game1.dustTexture, Game1.dust[index].position - Game1.screenPosition, new Rectangle?(Game1.dust[index].frame), Game1.dust[index].GetAlpha(white5), Game1.dust[index].rotation, new Vector2(4f, 4f), Game1.dust[index].scale, SpriteEffects.None, 0.0f);
          if (Game1.dust[index].color != new Color())
            this.spriteBatch.Draw(Game1.dustTexture, Game1.dust[index].position - Game1.screenPosition, new Rectangle?(Game1.dust[index].frame), Game1.dust[index].GetColor(white5), Game1.dust[index].rotation, new Vector2(4f, 4f), Game1.dust[index].scale, SpriteEffects.None, 0.0f);
        }
      }
      int num23 = 20;
      for (int index = 1; index < Game1.player[Game1.myPlayer].statLifeMax / num23 + 1; ++index)
      {
        float scale3 = 1f;
        int num24;
        if (Game1.player[Game1.myPlayer].statLife >= index * num23)
        {
          num24 = (int) byte.MaxValue;
        }
        else
        {
          float num25 = (float) (Game1.player[Game1.myPlayer].statLife - (index - 1) * num23) / (float) num23;
          num24 = (int) (30.0 + 225.0 * (double) num25);
          if (num24 < 30)
            num24 = 30;
          scale3 = (float) ((double) num25 / 4.0 + 0.75);
          if ((double) scale3 < 0.75)
            scale3 = 0.75f;
        }
        int num26 = 0;
        int num27 = 0;
        if (index > 10)
        {
          num26 -= 260;
          num27 += 26;
        }
        this.spriteBatch.Draw(Game1.heartTexture, new Vector2((float) (500 + 26 * (index - 1) + num26), (float) (32.0 + ((double) Game1.heartTexture.Height - (double) Game1.heartTexture.Height * (double) scale3) / 2.0) + (float) num27), new Rectangle?(new Rectangle(0, 0, Game1.heartTexture.Width, Game1.heartTexture.Height)), new Color(num24, num24, num24, num24), 0.0f, new Vector2(), scale3, SpriteEffects.None, 0.0f);
      }
      string text1 = "";
      if (Game1.playerInventory)
      {
        Game1.inventoryScale = 0.75f;
        Color color;
        for (int index11 = 0; index11 < 10; ++index11)
        {
          for (int index12 = 0; index12 < 4; ++index12)
          {
            int x4 = (int) (20.0 + (double) (index11 * 56) * (double) Game1.inventoryScale);
            int y5 = (int) (20.0 + (double) (index12 * 56) * (double) Game1.inventoryScale);
            int index13 = index11 + index12 * 10;
            color = new Color((int) byte.MaxValue, 
                (int) byte.MaxValue, 
                (int) byte.MaxValue,
                (int) byte.MaxValue);

            if 
            ( 
             Game1.mouseState.X >= x4 
             && (double) Game1.mouseState.X <= (double)x4 +(double)Game1.hotbarTexture.Width*(double)Game1.inventoryScale 
             && Game1.mouseState.Y >= y5 
             && (double) Game1.mouseState.Y <= (double)y5 +(double)Game1.hotbarTexture.Height*(double)Game1.inventoryScale
            )
            {
              Game1.player[Game1.myPlayer].mouseInterface = true;
              if 
              (
               Game1.mouseLeftRelease
               && Game1.mouseState.LeftButton == ButtonState.Pressed 
               && (Game1.player[Game1.myPlayer].selectedItem != index13 
               || Game1.player[Game1.myPlayer].itemAnimation <= 0)
              )
              {
                Item mouseItem = Game1.mouseItem;
                Game1.mouseItem = Game1.player[Game1.myPlayer].inventory[index13];
                Game1.player[Game1.myPlayer].inventory[index13] = mouseItem;
                if (Game1.player[Game1.myPlayer].inventory[index13].type == 0 
                                    || Game1.player[Game1.myPlayer].inventory[index13].stack < 1)
                  Game1.player[Game1.myPlayer].inventory[index13] = new Item();
                if 
                (
                 Game1.mouseItem.IsTheSameAs( Game1.player[Game1.myPlayer].inventory[index13] )
                 && Game1.player[Game1.myPlayer].inventory[index13].stack != Game1.player[Game1.myPlayer].inventory[index13].maxStack 
                 && Game1.mouseItem.stack != Game1.mouseItem.maxStack 
                )
                {
                  if (Game1.mouseItem.stack + Game1.player[Game1.myPlayer].inventory[index13].stack <= Game1.mouseItem.maxStack)
                  {
                    Game1.player[Game1.myPlayer].inventory[index13].stack += Game1.mouseItem.stack;
                    Game1.mouseItem.stack = 0;
                  }
                  else
                  {
                    int num28 = Game1.mouseItem.maxStack 
                                            - Game1.player[Game1.myPlayer].inventory[index13].stack;

                    Game1.player[Game1.myPlayer].inventory[index13].stack += num28;
                    Game1.mouseItem.stack -= num28;
                  }
                }
                if (Game1.mouseItem.type == 0 || Game1.mouseItem.stack < 1)
                  Game1.mouseItem = new Item();

                if (Game1.mouseItem.type > 0 || Game1.player[Game1.myPlayer].inventory[index13].type > 0)
                {
                  Recipe.FindRecipes();
                  Game1.soundInstanceGrab.Stop();
                  Game1.soundInstanceGrab = Game1.soundGrab.CreateInstance();
                  Game1.soundInstanceGrab.Play();
                }
              }
              text1 = Game1.player[Game1.myPlayer].inventory[index13].name;
              if (Game1.player[Game1.myPlayer].inventory[index13].stack > 1)
                text1 = text1 + " (" + (object) Game1.player[Game1.myPlayer].inventory[index13].stack + ")";
            }

            this.spriteBatch.Draw(Game1.hotbarTexture, new Vector2((float) x4, (float) y5), new Rectangle?(new Rectangle(0, 0, Game1.hotbarTexture.Width, Game1.hotbarTexture.Height)), color, 0.0f, new Vector2(), Game1.inventoryScale, SpriteEffects.None, 0.0f);
            this.spriteBatch.Draw(Game1.inventoryBackTexture, new Vector2((float) x4, (float) y5), new Rectangle?(new Rectangle(0, 0, Game1.inventoryBackTexture.Width, Game1.inventoryBackTexture.Height)), new Color(200, 200, 200, 200), 0.0f, new Vector2(), Game1.inventoryScale, SpriteEffects.None, 0.0f);
            color = Color.White;
            
            if (Game1.player[Game1.myPlayer].inventory[index13].type > 0 && Game1.player[Game1.myPlayer].inventory[index13].stack > 0)
            {
              float num29 = 1f;
              if (Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index13].type].Width > 32 || Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index13].type].Height > 32)
                num29 = Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index13].type].Width <= Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index13].type].Height ? 32f / (float) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index13].type].Height : 32f / (float) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index13].type].Width;
              float scale4 = num29 * Game1.inventoryScale;
              this.spriteBatch.Draw(Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index13].type], new Vector2((float) ((double) x4 + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index13].type].Width * 0.5 * (double) scale4), (float) ((double) y5 + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index13].type].Height * 0.5 * (double) scale4)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index13].type].Width, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index13].type].Height)), Game1.player[Game1.myPlayer].inventory[index13].GetAlpha(color), 0.0f, new Vector2(), scale4, SpriteEffects.None, 0.0f);
              if (Game1.player[Game1.myPlayer].inventory[index13].color != new Color())
                this.spriteBatch.Draw(Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index13].type], new Vector2((float) ((double) x4 + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index13].type].Width * 0.5 * (double) scale4), (float) ((double) y5 + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index13].type].Height * 0.5 * (double) scale4)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index13].type].Width, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index13].type].Height)), Game1.player[Game1.myPlayer].inventory[index13].GetColor(color), 0.0f, new Vector2(), scale4, SpriteEffects.None, 0.0f);
              if (Game1.player[Game1.myPlayer].inventory[index13].stack > 1)
                this.spriteBatch.DrawString(Game1.fontItemStack, string.Concat((object) Game1.player[Game1.myPlayer].inventory[index13].stack), new Vector2((float) x4 + 10f * Game1.inventoryScale, (float) y5 + 26f * Game1.inventoryScale), color, 0.0f, new Vector2(), scale4, SpriteEffects.None, 0.0f);
            }
          }
        }
        for (int index = 0; index < 3; ++index)
        {
          int x5 = 330;
          int y6 = (int) (238.0 + (double) (index * 56) * (double) Game1.inventoryScale);
          color = new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          if (Game1.mouseState.X >= x5 && (double) Game1.mouseState.X <= (double) x5 + (double) Game1.hotbarTexture.Width * (double) Game1.inventoryScale && Game1.mouseState.Y >= y6 && (double) Game1.mouseState.Y <= (double) y6 + (double) Game1.hotbarTexture.Height * (double) Game1.inventoryScale)
          {
            Game1.player[Game1.myPlayer].mouseInterface = true;
            if 
            (
              Game1.mouseLeftRelease 
              && Game1.mouseState.LeftButton == ButtonState.Pressed 
              && (Game1.mouseItem.type == 0 
              || Game1.mouseItem.headSlot > 0 && index == 0 
              || Game1.mouseItem.bodySlot > 0 && index == 1 
              || Game1.mouseItem.legSlot > 0 && index == 2))
            {
              Item mouseItem = Game1.mouseItem;
              Game1.mouseItem = Game1.player[Game1.myPlayer].armor[index];
              Game1.player[Game1.myPlayer].armor[index] = mouseItem;

              if 
              (
               Game1.player[Game1.myPlayer].armor[index].type == 0 
               || Game1.player[Game1.myPlayer].armor[index].stack < 1
              )
                Game1.player[Game1.myPlayer].armor[index] = new Item();
              
              if (Game1.mouseItem.type == 0 || Game1.mouseItem.stack < 1)
                Game1.mouseItem = new Item();

              if (Game1.mouseItem.type > 0 || Game1.player[Game1.myPlayer].armor[index].type > 0)
              {
                Recipe.FindRecipes();
                Game1.soundInstanceGrab.Stop();
                Game1.soundInstanceGrab = Game1.soundGrab.CreateInstance();
                Game1.soundInstanceGrab.Play();
              }
            }
            text1 = Game1.player[Game1.myPlayer].armor[index].name;
            if (Game1.player[Game1.myPlayer].armor[index].stack > 1)
              text1 = text1 + " (" + (object) Game1.player[Game1.myPlayer].armor[index].stack + ")";
          }

          this.spriteBatch.Draw(Game1.hotbarTexture, new Vector2((float) x5, (float) y6),
              new Rectangle?(new Rectangle(0, 0, Game1.hotbarTexture.Width, Game1.hotbarTexture.Height)),
              color, 0.0f, new Vector2(), Game1.inventoryScale, SpriteEffects.None, 0.0f);

          this.spriteBatch.Draw(Game1.inventoryBackTexture, new Vector2((float) x5, (float) y6),
              new Rectangle?(new Rectangle(0, 0, Game1.inventoryBackTexture.Width,
              Game1.inventoryBackTexture.Height)), new Color(200, 200, 200, 200), 0.0f, 
              new Vector2(), Game1.inventoryScale, SpriteEffects.None, 0.0f);

          color = Color.White;
          if ( Game1.player[Game1.myPlayer].armor[index].type > 0
                        && Game1.player[Game1.myPlayer].armor[index].stack > 0 )
          {
            float num30 = 1f;
            if (Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Width > 32
                            || Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Height > 32)
              num30 = Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Width <= Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Height ? 32f / (float) Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Height : 32f / (float) Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Width;
            float scale5 = num30 * Game1.inventoryScale;

            this.spriteBatch.Draw(Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type], new Vector2((float) ((double) x5 + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Width * 0.5 * (double) scale5), (float) ((double) y6 + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Height * 0.5 * (double) scale5)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Width, Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Height)), Game1.player[Game1.myPlayer].armor[index].GetAlpha(color), 0.0f, new Vector2(), scale5, SpriteEffects.None, 0.0f);
            if (Game1.player[Game1.myPlayer].armor[index].color != new Color())
              this.spriteBatch.Draw(Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type], new Vector2((float) ((double) x5 + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Width * 0.5 * (double) scale5), (float) ((double) y6 + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Height * 0.5 * (double) scale5)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Width, Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Height)), Game1.player[Game1.myPlayer].armor[index].GetColor(color), 0.0f, new Vector2(), scale5, SpriteEffects.None, 0.0f);
            if (Game1.player[Game1.myPlayer].armor[index].stack > 1)
              this.spriteBatch.DrawString(Game1.fontItemStack, string.Concat((object) Game1.player[Game1.myPlayer].armor[index].stack), new Vector2((float) x5 + 10f * Game1.inventoryScale, (float) y6 + 26f * Game1.inventoryScale), color, 0.0f, new Vector2(), scale5, SpriteEffects.None, 0.0f);
          }
        }
        Color white6;
        for (int index = 0; index < Recipe.maxRecipes; ++index)
        {
          Game1.inventoryScale = (float) (100.0 / ((double) Math.Abs(Game1.availableRecipeY[index]) + 100.0));
          if ((double) Game1.inventoryScale < 0.75)
            Game1.inventoryScale = 0.75f;
          if ((double) Game1.availableRecipeY[index] < (double) ((index - Game1.focusRecipe) * 65))
          {
            if ((double) Game1.availableRecipeY[index] == 0.0)
              Game1.soundInstanceMenuTick.Play();
            Game1.availableRecipeY[index] += 6.5f;
          }
          else if ((double) Game1.availableRecipeY[index] > (double) ((index - Game1.focusRecipe) * 65))
          {
            if ((double) Game1.availableRecipeY[index] == 0.0)
              Game1.soundInstanceMenuTick.Play();
            Game1.availableRecipeY[index] -= 6.5f;
          }
          if (index < Game1.numAvailableRecipes && (double) Math.Abs(Game1.availableRecipeY[index]) <= 250.0)
          {
            int x6 = (int) (46.0 - 26.0 * (double) Game1.inventoryScale);
            int y7 = (int) (400.0 + (double) Game1.availableRecipeY[index] * (double) Game1.inventoryScale - 30.0 * (double) Game1.inventoryScale);
            double num31 = (double) byte.MaxValue;

            if ((double) Math.Abs(Game1.availableRecipeY[index]) > 150.0)
              num31 = (double) byte.MaxValue * (100.0 
                                - ((double) Math.Abs(Game1.availableRecipeY[index]) - 150.0)) * 0.01;
            white6 = new Color//.White with
            {
              R = (byte) num31,
              G = (byte) num31,
              B = (byte) num31,
              A = (byte) num31
            };
            if (Game1.mouseState.X >= x6 && (double) Game1.mouseState.X <= (double) x6 + (double) Game1.hotbarTexture.Width * (double) Game1.inventoryScale && Game1.mouseState.Y >= y7 && (double) Game1.mouseState.Y <= (double) y7 + (double) Game1.hotbarTexture.Height * (double) Game1.inventoryScale)
            {
              Game1.player[Game1.myPlayer].mouseInterface = true;
              if (Game1.mouseLeftRelease && Game1.mouseState.LeftButton == ButtonState.Pressed)
              {
                if (Game1.focusRecipe == index)
                {
                  if ( Game1.mouseItem.type == 0 
                        || Game1.mouseItem.IsTheSameAs(Game1.recipe[Game1.availableRecipe[index]].createItem )
                        && Game1.mouseItem.stack + Game1.recipe[Game1.availableRecipe[index]].createItem.stack
                                                                                  <= Game1.mouseItem.maxStack )
                  {
                    int stack = Game1.mouseItem.stack;
                    
                    Game1.mouseItem = 
                         (Item) Game1.recipe[Game1.availableRecipe[index]].createItem.Clone();
                    
                    Game1.mouseItem.stack += stack;
                    Game1.recipe[Game1.availableRecipe[index]].Create();
                    if (Game1.mouseItem.type > 0 || Game1.recipe[Game1.availableRecipe[index]].createItem.type > 0)
                    {
                      Game1.soundInstanceGrab.Stop();
                      Game1.soundInstanceGrab = Game1.soundGrab.CreateInstance();
                      Game1.soundInstanceGrab.Play();
                    }
                  }
                }
                else
                  Game1.focusRecipe = index;
              }
              text1 = Game1.recipe[Game1.availableRecipe[index]].createItem.name;
              if (Game1.recipe[Game1.availableRecipe[index]].createItem.stack > 1)
                text1 = text1 + " (" + (object) Game1.recipe[Game1.availableRecipe[index]].createItem.stack + ")";
            }
            this.spriteBatch.Draw(Game1.hotbarTexture, new Vector2((float) x6, (float) y7), 
                new Rectangle?(new Rectangle(0, 0, Game1.hotbarTexture.Width, Game1.hotbarTexture.Height)), 
                white6, 0.0f, new Vector2(), Game1.inventoryScale, SpriteEffects.None, 0.0f);

            double num32 = num31 - 50.0;
            if (num32 < 0.0)
              num32 = 0.0;
            this.spriteBatch.Draw(Game1.inventoryBackTexture, new Vector2((float) x6, (float) y7),
                new Rectangle?(new Rectangle(0, 0, Game1.inventoryBackTexture.Width,
                Game1.inventoryBackTexture.Height)), new Color((int) (byte) num32, (int) (byte) num32, 
                (int) (byte) num32, (int) (byte) num32), 0.0f, new Vector2(), 
                Game1.inventoryScale, SpriteEffects.None, 0.0f);

            if (Game1.recipe[Game1.availableRecipe[index]].createItem.type > 0 
                            && Game1.recipe[Game1.availableRecipe[index]].createItem.stack > 0)
            {
              float num33 = 1f;
              if (Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Width > 32 || Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Height > 32)
                num33 = Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Width <= Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Height ? 32f / (float) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Height : 32f / (float) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Width;
              float scale6 = num33 * Game1.inventoryScale;
              this.spriteBatch.Draw(Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type], new Vector2((float) ((double) x6 + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Width * 0.5 * (double) scale6), (float) ((double) y7 + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Height * 0.5 * (double) scale6)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Width, Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Height)), Game1.recipe[Game1.availableRecipe[index]].createItem.GetAlpha(white6), 0.0f, new Vector2(), scale6, SpriteEffects.None, 0.0f);
              if (Game1.recipe[Game1.availableRecipe[index]].createItem.color != new Color())
                this.spriteBatch.Draw(Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type], new Vector2((float) ((double) x6 + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Width * 0.5 * (double) scale6), (float) ((double) y7 + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Height * 0.5 * (double) scale6)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Width, Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Height)), Game1.recipe[Game1.availableRecipe[index]].createItem.GetColor(white6), 0.0f, new Vector2(), scale6, SpriteEffects.None, 0.0f);
              if (Game1.recipe[Game1.availableRecipe[index]].createItem.stack > 1)
                this.spriteBatch.DrawString(Game1.fontItemStack, string.Concat((object) Game1.recipe[Game1.availableRecipe[index]].createItem.stack), new Vector2((float) x6 + 10f * Game1.inventoryScale, (float) y7 + 26f * Game1.inventoryScale), white6, 0.0f, new Vector2(), scale6, SpriteEffects.None, 0.0f);
            }
          }
        }
        if (Game1.numAvailableRecipes > 0)
        {
          for (int index = 0; index < Recipe.maxRequirements && Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type != 0; ++index)
          {
            int x7 = 80 + index * 40;
            int y8 = 380;
            white6 = Color.White;
            double num34 = (double) byte.MaxValue - (double) Math.Abs(Game1.availableRecipeY[Game1.focusRecipe]) * 3.0;
            if (num34 < 0.0)
              num34 = 0.0;
            white6.R = (byte) num34;
            white6.G = (byte) num34;
            white6.B = (byte) num34;
            white6.A = (byte) num34;
            Game1.inventoryScale = 0.6f;
            if (num34 != 0.0)
            {
              if (Game1.mouseState.X >= x7 && (double) Game1.mouseState.X <= (double) x7 + (double) Game1.hotbarTexture.Width * (double) Game1.inventoryScale && Game1.mouseState.Y >= y8 && (double) Game1.mouseState.Y <= (double) y8 + (double) Game1.hotbarTexture.Height * (double) Game1.inventoryScale)
              {
                Game1.player[Game1.myPlayer].mouseInterface = true;
                text1 = Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].name;
                if (Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].stack > 1)
                  text1 = text1 + " (" + (object) Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].stack + ")";
              }
              this.spriteBatch.Draw(Game1.hotbarTexture, new Vector2((float) x7, (float) y8), new Rectangle?(new Rectangle(0, 0, Game1.hotbarTexture.Width, Game1.hotbarTexture.Height)), white6, 0.0f, new Vector2(), Game1.inventoryScale, SpriteEffects.None, 0.0f);
              double num35 = num34 - 50.0;
              if (num35 < 0.0)
                num35 = 0.0;
              this.spriteBatch.Draw(Game1.inventoryBackTexture, new Vector2((float) x7, (float) y8), new Rectangle?(new Rectangle(0, 0, Game1.inventoryBackTexture.Width, Game1.inventoryBackTexture.Height)), new Color((int) (byte) num35, (int) (byte) num35, (int) (byte) num35, (int) (byte) num35), 0.0f, new Vector2(), Game1.inventoryScale, SpriteEffects.None, 0.0f);
              if (Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type > 0 && Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].stack > 0)
              {
                float num36 = 1f;
                if (Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Width > 32 || Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Height > 32)
                  num36 = Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Width <= Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Height ? 32f / (float) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Height : 32f / (float) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Width;
                float scale7 = num36 * Game1.inventoryScale;
                this.spriteBatch.Draw(Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type], new Vector2((float) ((double) x7 + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Width * 0.5 * (double) scale7), (float) ((double) y8 + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Height * 0.5 * (double) scale7)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Width, Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Height)), Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].GetAlpha(white6), 0.0f, new Vector2(), scale7, SpriteEffects.None, 0.0f);
                if (Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].color != new Color())
                  this.spriteBatch.Draw(Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type], new Vector2((float) ((double) x7 + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Width * 0.5 * (double) scale7), (float) ((double) y8 + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Height * 0.5 * (double) scale7)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Width, Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Height)), Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].GetColor(white6), 0.0f, new Vector2(), scale7, SpriteEffects.None, 0.0f);
                if (Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].stack > 1)
                  this.spriteBatch.DrawString(Game1.fontItemStack, string.Concat((object) Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].stack), new Vector2((float) x7 + 10f * Game1.inventoryScale, (float) y8 + 26f * Game1.inventoryScale), white6, 0.0f, new Vector2(), scale7, SpriteEffects.None, 0.0f);
              }
            }
            else
              break;
          }
        }
      }
      if (!Game1.playerInventory)
      {
        int x8 = 20;
        for (int index = 0; index < 10; ++index)
        {
          if (index == Game1.player[Game1.myPlayer].selectedItem)
          {
            if ((double) Game1.hotbarScale[index] < 1.0)
              Game1.hotbarScale[index] += 0.05f;
          }
          else if ((double) Game1.hotbarScale[index] > 0.75)
            Game1.hotbarScale[index] -= 0.05f;
          int y9 = (int) (20.0 + 22.0 * (1.0 - (double) Game1.hotbarScale[index]));
          Color color = new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) (75.0 + 150.0 * (double) Game1.hotbarScale[index]));
          this.spriteBatch.Draw(Game1.hotbarTexture, new Vector2((float) x8, (float) y9), new Rectangle?(new Rectangle(0, 0, Game1.hotbarTexture.Width, Game1.hotbarTexture.Height)), color, 0.0f, new Vector2(), Game1.hotbarScale[index], SpriteEffects.None, 0.0f);
          if (Game1.mouseState.X >= x8 && (double) Game1.mouseState.X <= (double) x8 + (double) Game1.hotbarTexture.Width * (double) Game1.hotbarScale[index] && Game1.mouseState.Y >= y9 && (double) Game1.mouseState.Y <= (double) y9 + (double) Game1.hotbarTexture.Height * (double) Game1.hotbarScale[index])
          {
            Game1.player[Game1.myPlayer].mouseInterface = true;
            if (Game1.mouseState.LeftButton == ButtonState.Pressed)
              Game1.player[Game1.myPlayer].changeItem = index;
            Game1.player[Game1.myPlayer].showItemIcon = false;
            text1 = Game1.player[Game1.myPlayer].inventory[index].name;
            if (Game1.player[Game1.myPlayer].inventory[index].stack > 1)
              text1 = text1 + " (" + (object) Game1.player[Game1.myPlayer].inventory[index].stack + ")";
          }
          if (Game1.player[Game1.myPlayer].inventory[index].type > 0 && Game1.player[Game1.myPlayer].inventory[index].stack > 0)
          {
            float num37 = 1f;
            if (Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Width > 32 || Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Height > 32)
              num37 = Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Width <= Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Height ? 32f / (float) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Height : 32f / (float) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Width;
            float scale8 = num37 * Game1.hotbarScale[index];
            this.spriteBatch.Draw(Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type], new Vector2((float) ((double) x8 + 26.0 * (double) Game1.hotbarScale[index] - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Width * 0.5 * (double) scale8), (float) ((double) y9 + 26.0 * (double) Game1.hotbarScale[index] - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Height * 0.5 * (double) scale8)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Width, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Height)), Game1.player[Game1.myPlayer].inventory[index].GetAlpha(color), 0.0f, new Vector2(), scale8, SpriteEffects.None, 0.0f);
            if (Game1.player[Game1.myPlayer].inventory[index].color != new Color())
              this.spriteBatch.Draw(Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type], new Vector2((float) ((double) x8 + 26.0 * (double) Game1.hotbarScale[index] - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Width * 0.5 * (double) scale8), (float) ((double) y9 + 26.0 * (double) Game1.hotbarScale[index] - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Height * 0.5 * (double) scale8)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Width, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Height)), Game1.player[Game1.myPlayer].inventory[index].GetColor(color), 0.0f, new Vector2(), scale8, SpriteEffects.None, 0.0f);
            if (Game1.player[Game1.myPlayer].inventory[index].stack > 1)
              this.spriteBatch.DrawString(Game1.fontItemStack, string.Concat((object) Game1.player[Game1.myPlayer].inventory[index].stack), new Vector2((float) x8 + 10f * Game1.hotbarScale[index], (float) y9 + 26f * Game1.hotbarScale[index]), color, 0.0f, new Vector2(), scale8, SpriteEffects.None, 0.0f);
          }
          x8 += (int) ((double) Game1.hotbarTexture.Width * (double) Game1.hotbarScale[index]) + 4;
        }
      }
      if (text1 != null && text1 != "" && Game1.mouseItem.type == 0)
      {
        Game1.player[Game1.myPlayer].showItemIcon = false;
        this.spriteBatch.DrawString(Game1.fontMouseText, text1, new Vector2((float) (Game1.mouseState.X + 10), (float) (Game1.mouseState.Y + 10)), new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
        flag = true;
      }
      if (Game1.player[Game1.myPlayer].dead)
      {
        string text2 = Game1.player[Game1.myPlayer].name + " was slain...";
        this.spriteBatch.DrawString(Game1.fontDeathText, text2, new Vector2((float) (Game1.screenWidth / 2 - text2.Length * 10), (float) (Game1.screenHeight / 2 - 20)), Game1.player[Game1.myPlayer].GetDeathAlpha(new Color(0, 0, 0, 0)), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
      }
      this.spriteBatch.Draw(Game1.cursorTexture, new Vector2((float) Game1.mouseState.X, (float) Game1.mouseState.Y), new Rectangle?(new Rectangle(0, 0, Game1.cursorTexture.Width, Game1.cursorTexture.Height)), Color.White, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
      if (Game1.mouseItem.type > 0 && Game1.mouseItem.stack > 0)
      {
        Game1.player[Game1.myPlayer].showItemIcon = false;
        Game1.player[Game1.myPlayer].showItemIcon2 = 0;
        flag = true;
        float num38 = 1f;
        if (Game1.itemTexture[Game1.mouseItem.type].Width > 32 || Game1.itemTexture[Game1.mouseItem.type].Height > 32)
          num38 = Game1.itemTexture[Game1.mouseItem.type].Width <= Game1.itemTexture[Game1.mouseItem.type].Height ? 32f / (float) Game1.itemTexture[Game1.mouseItem.type].Height : 32f / (float) Game1.itemTexture[Game1.mouseItem.type].Width;
        float num39 = 1f;
        Color white7 = Color.White;
        float scale9 = num38 * num39;
        this.spriteBatch.Draw(Game1.itemTexture[Game1.mouseItem.type], new Vector2((float) ((double) Game1.mouseState.X + 26.0 * (double) num39 - (double) Game1.itemTexture[Game1.mouseItem.type].Width * 0.5 * (double) scale9), (float) ((double) Game1.mouseState.Y + 26.0 * (double) num39 - (double) Game1.itemTexture[Game1.mouseItem.type].Height * 0.5 * (double) scale9)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.mouseItem.type].Width, Game1.itemTexture[Game1.mouseItem.type].Height)), Game1.mouseItem.GetAlpha(white7), 0.0f, new Vector2(), scale9, SpriteEffects.None, 0.0f);
        if (Game1.mouseItem.color != new Color())
          this.spriteBatch.Draw(Game1.itemTexture[Game1.mouseItem.type], new Vector2((float) ((double) Game1.mouseState.X + 26.0 * (double) num39 - (double) Game1.itemTexture[Game1.mouseItem.type].Width * 0.5 * (double) scale9), (float) ((double) Game1.mouseState.Y + 26.0 * (double) num39 - (double) Game1.itemTexture[Game1.mouseItem.type].Height * 0.5 * (double) scale9)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.mouseItem.type].Width, Game1.itemTexture[Game1.mouseItem.type].Height)), Game1.mouseItem.GetColor(white7), 0.0f, new Vector2(), scale9, SpriteEffects.None, 0.0f);
        if (Game1.mouseItem.stack > 1)
          this.spriteBatch.DrawString(Game1.fontItemStack, string.Concat((object) Game1.mouseItem.stack), new Vector2((float) Game1.mouseState.X + 10f * num39, (float) Game1.mouseState.Y + 26f * num39), white7, 0.0f, new Vector2(), scale9, SpriteEffects.None, 0.0f);
      }
      Rectangle rectangle2 = new Rectangle((int) ((double) Game1.mouseState.X + (double) Game1.screenPosition.X), (int) ((double) Game1.mouseState.Y + (double) Game1.screenPosition.Y), 1, 1);
      Game1.mouseTextColor += (byte) Game1.mouseTextColorChange;
      if (Game1.mouseTextColor >= (byte) 250)
        Game1.mouseTextColorChange = -4;
      if (Game1.mouseTextColor <= (byte) 175)
        Game1.mouseTextColorChange = 4;
      if (!flag)
      {
        int num40 = 26 * Game1.player[Game1.myPlayer].statLifeMax / num23;
        int num41 = 0;
        if (Game1.player[Game1.myPlayer].statLifeMax > 200)
        {
          num40 -= 260;
          num41 += 26;
        }
        if (Game1.mouseState.X > 500 && Game1.mouseState.X < 500 + num40 && Game1.mouseState.Y > 32 && Game1.mouseState.Y < 32 + Game1.heartTexture.Height + num41)
        {
          Game1.player[Game1.myPlayer].showItemIcon = false;
          string text3 = "Life: " + (object) Game1.player[Game1.myPlayer].statLife + "/" + (object) Game1.player[Game1.myPlayer].statLifeMax;
          this.spriteBatch.DrawString(Game1.fontMouseText, text3, new Vector2((float) (Game1.mouseState.X + 10), (float) (Game1.mouseState.Y + 10)), new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
          flag = true;
        }
      }
      if (!flag)
      {
        for (int index = 0; index < 1000; ++index)
        {
          if (Game1.item[index].active)
          {
            Rectangle rectangle3 = new Rectangle((int) ((double) Game1.item[index].position.X + (double) Game1.item[index].width * 0.5 - (double) Game1.itemTexture[Game1.item[index].type].Width * 0.5), (int) ((double) Game1.item[index].position.Y + (double) Game1.item[index].height - (double) Game1.itemTexture[Game1.item[index].type].Height), Game1.itemTexture[Game1.item[index].type].Width, Game1.itemTexture[Game1.item[index].type].Height);
            if (rectangle2.Intersects(rectangle3))
            {
              Game1.player[Game1.myPlayer].showItemIcon = false;
              string text4 = Game1.item[index].name;
              if (Game1.item[index].stack > 1)
                text4 = text4 + " (" + (object) Game1.item[index].stack + ")";
              this.spriteBatch.DrawString(Game1.fontMouseText, text4, new Vector2((float) (Game1.mouseState.X + 10), (float) (Game1.mouseState.Y + 10)), new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
              flag = true;
              break;
            }
          }
        }
      }
      if (!flag)
      {
        for (int index = 0; index < 1000; ++index)
        {
          if (Game1.npc[index].active)
          {
            Rectangle rectangle4 = new Rectangle((int) ((double) Game1.npc[index].position.X + (double) Game1.npc[index].width * 0.5 - (double) Game1.npcTexture[Game1.npc[index].type].Width * 0.5), (int) ((double) Game1.npc[index].position.Y + (double) Game1.npc[index].height - (double) (Game1.npcTexture[Game1.npc[index].type].Height / Game1.npcFrameCount[Game1.npc[index].type])), Game1.npcTexture[Game1.npc[index].type].Width, Game1.npcTexture[Game1.npc[index].type].Height / Game1.npcFrameCount[Game1.npc[index].type]);
            if (rectangle2.Intersects(rectangle4))
            {
              Game1.player[Game1.myPlayer].showItemIcon = false;
              string text5 = Game1.npc[index].name + ": " + (object) Game1.npc[index].life + "/" + (object) Game1.npc[index].lifeMax;
              this.spriteBatch.DrawString(Game1.fontMouseText, text5, new Vector2((float) (Game1.mouseState.X + 10), (float) (Game1.mouseState.Y + 10)), new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
              break;
            }
          }
        }
      }
      if (Game1.player[Game1.myPlayer].showItemIcon && (Game1.player[Game1.myPlayer].inventory[Game1.player[Game1.myPlayer].selectedItem].type > 0 || Game1.player[Game1.myPlayer].showItemIcon2 > 0))
      {
        int index = Game1.player[Game1.myPlayer].inventory[Game1.player[Game1.myPlayer].selectedItem].type;
        Color color1 = Game1.player[Game1.myPlayer].inventory[Game1.player[Game1.myPlayer].selectedItem].GetAlpha(Color.White);
        Color color2 = Game1.player[Game1.myPlayer].inventory[Game1.player[Game1.myPlayer].selectedItem].GetColor(Color.White);
        if (Game1.player[Game1.myPlayer].showItemIcon2 > 0)
        {
          index = Game1.player[Game1.myPlayer].showItemIcon2;
          color1 = Color.White;
          color2 = new Color();
        }
        this.spriteBatch.Draw(Game1.itemTexture[index], new Vector2((float) (Game1.mouseState.X + 10), (float) (Game1.mouseState.Y + 10)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[index].Width, Game1.itemTexture[index].Height)), color1, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
        if (Game1.player[Game1.myPlayer].showItemIcon2 == 0 && Game1.player[Game1.myPlayer].inventory[Game1.player[Game1.myPlayer].selectedItem].color != new Color())
          this.spriteBatch.Draw(Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[Game1.player[Game1.myPlayer].selectedItem].type], new Vector2((float) (Game1.mouseState.X + 10), (float) (Game1.mouseState.Y + 10)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[Game1.player[Game1.myPlayer].selectedItem].type].Width, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[Game1.player[Game1.myPlayer].selectedItem].type].Height)), color2, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
      }
      Game1.player[Game1.myPlayer].showItemIcon = false;
      Game1.player[Game1.myPlayer].showItemIcon2 = 0;
      this.spriteBatch.End();
      if (Game1.mouseState.LeftButton == ButtonState.Pressed)
        Game1.mouseLeftRelease = false;
      else
        Game1.mouseLeftRelease = true;
    }

    private static void UpdateTime()
    {
      ++Game1.time;
      if (!Game1.dayTime)
      {
        if (Game1.time <= 30000.0)
          return;
        Game1.time = 0.0;
        Game1.dayTime = true;
        ++Game1.moonPhase;
        if (Game1.moonPhase >= 8)
          Game1.moonPhase = 0;
      }
      else if (Game1.time > 40000.0)
      {
        Game1.time = 0.0;
        Game1.dayTime = false;
      }
    }

    public static double CalculateDamage(int Damage, int Defense)
    {
        return (double)Damage / ((double)Defense * 0.1);
    }

    private static void UpdateDebug()
    {
      if (Game1.keyState.IsKeyDown(Keys.Left))
      {
        Game1.screenPosition.X -= 10f;//8f;
      }

      if (Game1.keyState.IsKeyDown(Keys.Right))
      {
        Game1.screenPosition.X += 10f;//8f;
      }

      if (Game1.keyState.IsKeyDown(Keys.Up))
      {
        Game1.screenPosition.Y -= 10f;//8f;
      }

      if (Game1.keyState.IsKeyDown(Keys.Down))
      {
        Game1.screenPosition.Y += 10f;//8f;
      }

      int i = (int) (((double) Game1.mouseState.X + (double) Game1.screenPosition.X) / 16.0);
      int j = (int) (((double) Game1.mouseState.Y + (double) Game1.screenPosition.Y) / 16.0);

      if 
      (
        Game1.mouseState.X >= Game1.screenWidth || Game1.mouseState.Y >= Game1.screenHeight
        || i < 0 || j < 0 || i >= 5001 || j >= 2501
      )
      {
            return;
      }

            if (Game1.mouseState.RightButton == ButtonState.Pressed
                      && Game1.mouseState.LeftButton == ButtonState.Pressed)
            {
                Debug.WriteLine("[i] Event: Both mouse button pressed!");

                if (Game1.player[Game1.myPlayer].releaseUseItem)
                {
                    int index = NPC.NewNPC((int)((double)Game1.mouseState.X
                        + (double)Game1.screenPosition.X), (int)((double)
                        Game1.mouseState.Y + (double)Game1.screenPosition.Y), 1);
                    Game1.dayTime = true;
                    Game1.npc[index].name = "Yellow Slime";
                    Game1.npc[index].scale = 1.2f;
                    Game1.npc[index].damage = 15;
                    Game1.npc[index].defense = 15;
                    Game1.npc[index].life = 50;
                    Game1.npc[index].lifeMax = Game1.npc[index].life;
                    Game1.npc[index].color = new Color((int)byte.MaxValue, 200, 0, 100);
                }
            }
            else if (Game1.mouseState.RightButton == ButtonState.Pressed)
            {
                Debug.WriteLine("[i] " + "MouseState: Right Button pressed");
                if (!Game1.tile[i, j].active)
                {
                    WorldGen.PlaceTile(i, j, 4);
                }
                //TODO
            }
            else if (Game1.mouseState.LeftButton == ButtonState.Pressed/*Game1.mouseState.LeftButton != ButtonState.Pressed*/)
            {
                Debug.WriteLine("[i] " + "MouseState: Left Button pressed");
                //TODO
            }
            else
            {
                //Debug.WriteLine("[i] " + "Some other/another event appeared");
            }
        }//UpdatingDebug
  }
}
