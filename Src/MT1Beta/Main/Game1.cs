// Decompiled with JetBrains decompiler
// Type: GameManager.Game1
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Windows.Storage;

namespace GameManager
{
  public class Game1 : Game
  {
    private const int MF_BYPOSITION = 1024;
    public const int sectionWidth = 200;
    public const int sectionHeight = 150;
    public const int maxTileSets = 76;
    public const int maxWallTypes = 13;
    public const int maxBackgrounds = 7;
    public const int maxDust = 2000;
    public const int maxCombatText = 100;
    public const int maxPlayers = 8;
    public const int maxChests = 1000;
    public const int maxItemTypes = 194;
    public const int maxItems = 200;
    public const int maxProjectileTypes = 34;
    public const int maxProjectiles = 1000;
    public const int maxNPCTypes = 44;
    public const int maxNPCs = 1000;
    public const int maxGoreTypes = 73;
    public const int maxGore = 200;
    public const int maxInventory = 44;
    public const int maxItemSounds = 14;
    public const int maxNPCHitSounds = 3;
    public const int maxNPCKilledSounds = 3;
    public const int maxLiquidTypes = 2;
    public const int maxMusic = 6;
    public const double dayLength = 54000.0;
    public const double nightLength = 32400.0;
    public const int maxStars = 130;
    public const int maxStarTypes = 5;
    public const int maxClouds = 100;
    public const int maxCloudTypes = 4;

    //RnD
    public static bool grabSun = true;//false;
    
    public static bool debugMode = true;//set True for/to debug
    
    public static bool godMode = true;//false;
    
    public static bool stopSpawns = false;
    
    public static bool dumbAI = false;

    public static bool skipMenu = false;//false;

    public static bool lightTiles = false;
    public static bool verboseNetplay = false;
    public static bool stopTimeOuts = false;
    public static bool showSpam = false;
    public static bool showItemOwner = false;
    public static string defaultIP = "74.132.0.65";

    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    public static MouseState mouseState1 = Mouse.GetState();
    public static TouchCollection mouseState = TouchPanel.GetState();//Experimental

    // public static MouseState oldMouseState = Mouse.GetState();
    public static MouseState oldMouseState1 = Mouse.GetState();
    public static TouchCollection oldMouseState = TouchPanel.GetState();//Experimental

    public static KeyboardState keyState = Keyboard.GetState();
    public static int updateTime = 0;
    public static int drawTime = 0;
    public static int frameRate = 0;
    public static bool frameRelease = false;
    public static bool showFrameRate = false;
    public static int magmaBGFrame = 0;
    public static int magmaBGFrameCounter = 0;
    public static float leftWorld = 0.0f;
    public static float rightWorld = 134400f;
    public static float topWorld = 0.0f;
    public static float bottomWorld = 38400f;
    public static int maxTilesX = (int) Game1.rightWorld / 16 + 1;
    public static int maxTilesY = (int) Game1.bottomWorld / 16 + 1;
    public static int maxSectionsX = Game1.maxTilesX / 200;
    public static int maxSectionsY = Game1.maxTilesY / 150;
    public static int maxHair = 3;
    public static int dungeonX;
    public static int dungeonY;
    public static Liquid[] liquid = new Liquid[Liquid.resLiquid];
    public static LiquidBuffer[] liquidBuffer = new LiquidBuffer[10000];
    public int curMusic = 0;
    public int newMusic = 0;
    public static string statusText = "";
    public static string worldName = "";
    public static int background = 0;
    public static Color tileColor;
    public static double worldSurface;
    public static double rockLayer;
    public static Color[] teamColor = new Color[5];
    public static bool dayTime = true;
    public static double time = 13500.0;
    public static int moonPhase = 0;
    public static short sunModY = 0;
    public static short moonModY = 0;
    public static bool grabSky = false;
    public static bool bloodMoon = false;
    public static int checkForSpawns = 0;
    public static int helpText = 0;
    public static int numStars;
    public static int cloudLimit = 100;
    public static int numClouds = Game1.cloudLimit;
    public static float windSpeed = 0.0f;
    public static float windSpeedSpeed = 0.0f;
    public static Cloud[] cloud = new Cloud[100];
    public static bool resetClouds = true;
    public static int evilTiles;
    public static int meteorTiles;
    public static int jungleTiles;
    public static int dungeonTiles;
    [ThreadStatic]
    public static Random rand;
    public static Texture2D armorHeadTexture;
    public static Texture2D armorBodyTexture;
    public static Texture2D armorBody2Texture;
    public static Texture2D armorLegTexture;
    public static Texture2D chainTexture;
    public static Texture2D chain2Texture;
    public static Texture2D chain3Texture;
    public static Texture2D chain4Texture;
    public static Texture2D chain5Texture;
    public static Texture2D boneArmTexture;
    public static Texture2D[] itemTexture = new Texture2D[194];
    public static Texture2D[] npcTexture = new Texture2D[44];
    public static Texture2D[] projectileTexture = new Texture2D[34];
    public static Texture2D[] goreTexture = new Texture2D[73];
    public static Texture2D cursorTexture;
    public static Texture2D dustTexture;
    public static Texture2D sunTexture;
    public static Texture2D moonTexture;
    public static Texture2D[] tileTexture = new Texture2D[76];
    public static Texture2D blackTileTexture;
    public static Texture2D[] wallTexture = new Texture2D[13];
    public static Texture2D[] backgroundTexture = new Texture2D[7];
    public static Texture2D[] cloudTexture = new Texture2D[4];
    public static Texture2D[] starTexture = new Texture2D[5];
    public static Texture2D[] liquidTexture = new Texture2D[2];
    public static Texture2D heartTexture;
    public static Texture2D manaTexture;
    public static Texture2D bubbleTexture;
    public static Texture2D treeTopTexture;
    public static Texture2D shroomCapTexture;
    public static Texture2D treeBranchTexture;
    public static Texture2D inventoryBackTexture;
    public static Texture2D logoTexture;
    public static Texture2D textBackTexture;
    public static Texture2D chatTexture;
    public static Texture2D chat2Texture;
    public static Texture2D chatBackTexture;
    public static Texture2D teamTexture;
    public static Texture2D playerEyeWhitesTexture;
    public static Texture2D playerEyesTexture;
    public static Texture2D playerHairTexture;
    public static Texture2D playerHandsTexture;
    public static Texture2D playerHands2Texture;
    public static Texture2D playerHeadTexture;
    public static Texture2D playerPantsTexture;
    public static Texture2D playerShirtTexture;
    public static Texture2D playerShoesTexture;
    public static Texture2D playerBeltTexture;
    public static Texture2D playerUnderShirtTexture;
    public static Texture2D playerUnderShirt2Texture;
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
    public static SoundEffect[] soundItem = new SoundEffect[15];
    public static SoundEffectInstance[] soundInstanceItem = new SoundEffectInstance[15];
    public static SoundEffect[] soundNPCHit = new SoundEffect[4];
    public static SoundEffectInstance[] soundInstanceNPCHit = new SoundEffectInstance[4];
    public static SoundEffect[] soundNPCKilled = new SoundEffect[4];
    public static SoundEffectInstance[] soundInstanceNPCKilled = new SoundEffectInstance[4];
    public static SoundEffect soundDoorOpen;
    public static SoundEffectInstance soundInstanceDoorOpen;
    public static SoundEffect soundDoorClosed;
    public static SoundEffectInstance soundInstanceDoorClosed;
    public static SoundEffect soundMenuOpen;
    public static SoundEffectInstance soundInstanceMenuOpen;
    public static SoundEffect soundMenuClose;
    public static SoundEffectInstance soundInstanceMenuClose;
    public static SoundEffect soundMenuTick;
    public static SoundEffectInstance soundInstanceMenuTick;
    public static SoundEffect soundShatter;
    public static SoundEffectInstance soundInstanceShatter;
    public static SoundEffect[] soundZombie = new SoundEffect[3];
    public static SoundEffectInstance[] soundInstanceZombie = new SoundEffectInstance[3];
    public static SoundEffect[] soundRoar = new SoundEffect[2];
    public static SoundEffectInstance[] soundInstanceRoar = new SoundEffectInstance[2];
    public static SoundEffect[] soundSplash = new SoundEffect[2];
    public static SoundEffectInstance[] soundInstanceSplash = new SoundEffectInstance[2];
    public static SoundEffect soundDoubleJump;
    public static SoundEffectInstance soundInstanceDoubleJump;
    public static SoundEffect soundRun;
    public static SoundEffectInstance soundInstanceRun;
    public static SoundEffect soundCoins;
    public static SoundEffectInstance soundInstanceCoins;
    public static AudioEngine engine;
    public static SoundBank soundBank;
    public static WaveBank waveBank;
    public static Cue[] music = new Cue[6];
    public static float[] musicFade = new float[6];
    public static float musicVolume = 0.75f;
    public static float soundVolume = 1f;
    public static SpriteFont fontItemStack;
    public static SpriteFont fontMouseText;
    public static SpriteFont fontDeathText;
    public static SpriteFont fontCombatText;
    public static bool[] wallHouse = new bool[13];
    public static bool[] tileStone = new bool[76];
    public static bool[] tileWaterDeath = new bool[76];
    public static bool[] tileLavaDeath = new bool[76];
    public static bool[] tileTable = new bool[76];
    public static bool[] tileBlockLight = new bool[76];
    public static bool[] tileDungeon = new bool[76];
    public static bool[] tileSolidTop = new bool[76];
    public static bool[] tileSolid = new bool[76];
    public static bool[] tileNoAttach = new bool[76];
    public static bool[] tileNoFail = new bool[76];
    public static bool[] tileFrameImportant = new bool[76];
    public static int[] backgroundWidth = new int[7];
    public static int[] backgroundHeight = new int[7];
    public static bool tilesLoaded = false;
    public static Tile[,] tile = new Tile[Game1.maxTilesX, Game1.maxTilesY];
    public static Dust[] dust = new Dust[2000];
    public static Star[] star = new Star[130];
    public static Item[] item = new Item[201];
    public static NPC[] npc = new NPC[1001];
    public static Gore[] gore = new Gore[201];
    public static Projectile[] projectile = new Projectile[1001];
    public static CombatText[] combatText = new CombatText[100];
    public static Chest[] chest = new Chest[1000];
    public static Sign[] sign = new Sign[1000];
    public static Vector2 screenPosition;
    public static Vector2 screenLastPosition;
    public static int screenWidth = 800;
    public static int screenHeight = 600;
    public static int chatLength = 600;
    public static bool chatMode = false;
    public static bool chatRelease = false;
    public static int numChatLines = 7;
    public static string chatText = "";
    public static ChatLine[] chatLine = new ChatLine[Game1.numChatLines];
    public static bool inputTextEnter = false;
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
    public static bool mouseRightRelease = false;
    public static bool playerInventory = false;
    public static int stackSplit;
    public static int stackCounter = 0;
    public static int stackDelay = 7;
    public static Item mouseItem = new Item();
    private static float inventoryScale = 0.75f;
    public static bool hasFocus = true;
    public static Recipe[] recipe = new Recipe[Recipe.maxRecipes];
    public static int[] availableRecipe = new int[Recipe.maxRecipes];
    public static float[] availableRecipeY = new float[Recipe.maxRecipes];
    public static int numAvailableRecipes;
    public static int focusRecipe;
    public static int myPlayer = 0;
    public static Player[] player = new Player[9];
    public static int spawnTileX;
    public static int spawnTileY;
    public static bool npcChatRelease = false;
    public static bool editSign = false;
    public static string signText = "";
    public static string npcChatText = "";
    public static bool npcChatFocus1 = false;
    public static bool npcChatFocus2 = false;
    public static int npcShop = 0;
    public Chest[] shop = new Chest[5];
    private static Item toolTip = new Item();
    private static int backSpaceCount = 0;
    public bool toggleFullscreen;
    public static bool gameMenu = true;
    public static Player[] loadPlayer = new Player[5];
    public static string[] loadPlayerPath = new string[5];
    private static int numLoadPlayers = 0;
    public static string playerPathName;
    public static string[] loadWorld = new string[5];
    public static string[] loadWorldPath = new string[5];
    private static int numLoadWorlds = 0;
    public static string worldPathName;

    public static string SavePath = ApplicationData.Current.LocalFolder.Path;//Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\My Games\\Terraria";
    
    public static string WorldPath = Game1.SavePath + "\\Worlds";
    public static string PlayerPath = Game1.SavePath + "\\Players";
    private static KeyboardState inputText;
    private static KeyboardState oldInputText;
    public static int invasionType = 0;
    public static double invasionX = 0.0;
    public static int invasionSize = 0;
    public static int invasionDelay = 0;
    public static int invasionWarn = 0;
    public static int[] npcFrameCount = new int[44]
    {
      1,
      2,
      2,
      3,
      6,
      2,
      2,
      1,
      1,
      1,
      1,
      1,
      1,
      1,
      1,
      1,
      2,
      3,
      5,
      3,
      3,
      3,
      5,
      2,
      6,
      1,
      3,
      3,
      3,
      3,
      1,
      3,
      3,
      1,
      3,
      1,
      1,
      3,
      3,
      1,
      1,
      1,
      3,
      3
    };
    private static bool mouseExit = false;
    private static float exitScale = 0.8f;
    public static Player clientPlayer = new Player();
    public static string getIP = Game1.defaultIP;
    public static bool menuMultiplayer = false;
    public static int netMode = 0;
    public static int timeOut = 120;
    public static int netPlayCounter;
    public static int lastNPCUpdate;
    public static int lastItemUpdate;
    public static int maxNPCUpdates = 15;
    public static int maxItemUpdates = 10;
    public static Color mouseColor = new Color((int) byte.MaxValue, 50, 95);
    public static Color cursorColor = Color.White;
    public static int cursorColorDirection = 1;
    public static float cursorAlpha = 0.0f;
    public static float cursorScale = 0.0f;
    public static bool signBubble = false;
    public static int signX = 0;
    public static int signY = 0;
    public static bool hideUI = false;
    public static bool releaseUI = false;
    public static int curRelease = 38;
    private float logoRotation = 0.0f;
    private float logoRotationDirection = 1f;
    private float logoRotationSpeed = 1f;
    private float logoScale = 1f;
    private float logoScaleDirection = 1f;
    private float logoScaleSpeed = 1f;
    private static int maxMenuItems = 11;

    private float[] menuItemScale = new float[] 
    {   1, 1, 1,
        1,1,1,1,
        1,1,1,1,
        1,1
    };// new float[Game1.maxMenuItems];

    private int focusMenu = -1;
    private int selectedMenu = -1;
    private int selectedPlayer = 0;
    private int selectedWorld = 0;
    public static int menuMode = 0;
    private int textBlinkerCount;
    private int textBlinkerState;
    public static string newWorldName = "";
    private Color selColor = Color.White;
    private int focusColor = 0;
    private int colorDelay = 0;

    //[DllImport("User32")]
    private static /*extern*/ int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags)
    {
       return default;
    }

    //[DllImport("User32")]
    private static /*extern*/ IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert)
    { 
      return default;
    }

    //[DllImport("User32")]
    private static /*extern*/ int GetMenuItemCount(IntPtr hWnd)
    {
        int menuItemsCount = 11;
        return menuItemsCount;//default;
    }

    public static void LoadWorlds()
    {
      Directory.CreateDirectory(Game1.WorldPath);
      string[] files = Directory.GetFiles(Game1.WorldPath, "*.wld");
      int num = files.Length;
      if (num > 5)
        num = 5;
      for (int index = 0; index < num; ++index)
      {
        Game1.loadWorldPath[index] = files[index];
        using (FileStream input = new FileStream(Game1.loadWorldPath[index], FileMode.Open))
        {
          using (BinaryReader binaryReader = new BinaryReader((Stream) input))
          {
            binaryReader.ReadInt32();
            Game1.loadWorld[index] = binaryReader.ReadString();
            binaryReader.Dispose();//.Close();
          }
        }
      }
      Game1.numLoadWorlds = num;
    }

    private static void LoadPlayers()
    {
      Directory.CreateDirectory(Game1.PlayerPath);
      string[] files = Directory.GetFiles(Game1.PlayerPath, "*.plr");
      int num = files.Length;
      if (num > 5)
        num = 5;
      for (int index = 0; index < 5; ++index)
      {
        Game1.loadPlayer[index] = new Player();
        if (index < num)
        {
          Game1.loadPlayerPath[index] = files[index];
          Game1.loadPlayer[index] = Player.LoadPlayer(Game1.loadPlayerPath[index]);
        }
      }
      Game1.numLoadPlayers = num;
    }

    protected void SaveSettings()
    {
      using (FileStream output = new FileStream(Game1.SavePath + "\\config.dat", FileMode.Create))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) output))
        {
          binaryWriter.Write(Game1.curRelease);
          binaryWriter.Write(this.graphics.IsFullScreen);
          binaryWriter.Write(Game1.mouseColor.R);
          binaryWriter.Write(Game1.mouseColor.G);
          binaryWriter.Write(Game1.mouseColor.B);
          binaryWriter.Write(Game1.soundVolume);
          binaryWriter.Write(Game1.musicVolume);
          binaryWriter.Dispose();//.Close();
        }
      }
    }

    protected void OpenSettings()
    {
      if (!File.Exists(Game1.SavePath + "\\config.dat"))
        return;
      using (FileStream input = new FileStream(Game1.SavePath + "\\config.dat", FileMode.Open))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) input))
        {
          int num = binaryReader.ReadInt32();
          bool flag = binaryReader.ReadBoolean();
          Game1.mouseColor.R = binaryReader.ReadByte();
          Game1.mouseColor.G = binaryReader.ReadByte();
          Game1.mouseColor.B = binaryReader.ReadByte();
          if (num >= 38)
          {
            Game1.soundVolume = binaryReader.ReadSingle();
            Game1.musicVolume = binaryReader.ReadSingle();
          }
          binaryReader.Dispose();//.Close();
          if (flag && !this.graphics.IsFullScreen)
            this.graphics.ToggleFullScreen();
        }
      }
    }

    private static void ErasePlayer(int i)
    {
      File.Delete(Game1.loadPlayerPath[i]);
      Game1.LoadPlayers();
    }

    private static void EraseWorld(int i)
    {
      File.Delete(Game1.loadWorldPath[i]);
      Game1.LoadWorlds();
    }

    private static string nextLoadPlayer()
    {
      int num = 1;
      while (true)
      {
        if (File.Exists(Game1.PlayerPath + "\\player" + (object) num + ".plr"))
          ++num;
        else
          break;
      }
      return Game1.PlayerPath + "\\player" + (object) num + ".plr";
    }

    private static string nextLoadWorld()
    {
      int num = 1;
      while (true)
      {
        if (File.Exists(Game1.WorldPath + "\\world" + (object) num + ".wld"))
          ++num;
        else
          break;
      }
      return Game1.WorldPath + "\\world" + (object) num + ".wld";
    }

    public Game1()
    {
      this.graphics = new GraphicsDeviceManager((Game) this);
      this.Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
      if (Game1.rand == null)
        Game1.rand = new Random((int) DateTime.Now.Ticks);
      if (WorldGen.genRand == null)
        WorldGen.genRand = new Random((int) DateTime.Now.Ticks);
      this.OpenSettings();
      int num = Game1.rand.Next(3);
      if (num == 0)
        this.Window.Title = "Terraria: Dig Peon, Dig!";
      if (num == 1)
        this.Window.Title = "Terraria: Epic Dirt";
      else
        this.Window.Title = "Terraria: Shut Up and Dig Gaiden!";
      Game1.tileSolid[0] = true;
      Game1.tileBlockLight[0] = true;
      Game1.tileSolid[1] = true;
      Game1.tileBlockLight[1] = true;
      Game1.tileSolid[2] = true;
      Game1.tileBlockLight[2] = true;
      Game1.tileSolid[3] = false;
      Game1.tileNoAttach[3] = true;
      Game1.tileNoFail[3] = true;
      Game1.tileSolid[4] = false;
      Game1.tileNoAttach[4] = true;
      Game1.tileNoFail[4] = true;
      Game1.tileNoFail[24] = true;
      Game1.tileSolid[5] = false;
      Game1.tileSolid[6] = true;
      Game1.tileBlockLight[6] = true;
      Game1.tileSolid[7] = true;
      Game1.tileBlockLight[7] = true;
      Game1.tileSolid[8] = true;
      Game1.tileBlockLight[8] = true;
      Game1.tileSolid[9] = true;
      Game1.tileBlockLight[9] = true;
      Game1.tileBlockLight[10] = true;
      Game1.tileSolid[10] = true;
      Game1.tileNoAttach[10] = true;
      Game1.tileBlockLight[10] = true;
      Game1.tileSolid[11] = false;
      Game1.tileSolidTop[19] = true;
      Game1.tileSolid[19] = true;
      Game1.tileSolid[22] = true;
      Game1.tileSolid[23] = true;
      Game1.tileSolid[25] = true;
      Game1.tileSolid[30] = true;
      Game1.tileNoFail[32] = true;
      Game1.tileBlockLight[32] = true;
      Game1.tileSolid[37] = true;
      Game1.tileBlockLight[37] = true;
      Game1.tileSolid[38] = true;
      Game1.tileBlockLight[38] = true;
      Game1.tileSolid[39] = true;
      Game1.tileBlockLight[39] = true;
      Game1.tileSolid[40] = true;
      Game1.tileBlockLight[40] = true;
      Game1.tileSolid[41] = true;
      Game1.tileBlockLight[41] = true;
      Game1.tileSolid[43] = true;
      Game1.tileBlockLight[43] = true;
      Game1.tileSolid[44] = true;
      Game1.tileBlockLight[44] = true;
      Game1.tileSolid[45] = true;
      Game1.tileBlockLight[45] = true;
      Game1.tileSolid[46] = true;
      Game1.tileBlockLight[46] = true;
      Game1.tileSolid[47] = true;
      Game1.tileBlockLight[47] = true;
      Game1.tileSolid[48] = true;
      Game1.tileBlockLight[48] = true;
      Game1.tileSolid[53] = true;
      Game1.tileBlockLight[53] = true;
      Game1.tileSolid[54] = true;
      Game1.tileBlockLight[52] = true;
      Game1.tileSolid[56] = true;
      Game1.tileBlockLight[56] = true;
      Game1.tileSolid[57] = true;
      Game1.tileBlockLight[57] = true;
      Game1.tileSolid[58] = true;
      Game1.tileBlockLight[58] = true;
      Game1.tileSolid[59] = true;
      Game1.tileBlockLight[59] = true;
      Game1.tileSolid[60] = true;
      Game1.tileBlockLight[60] = true;
      Game1.tileSolid[63] = true;
      Game1.tileBlockLight[63] = true;
      Game1.tileStone[63] = true;
      Game1.tileSolid[64] = true;
      Game1.tileBlockLight[64] = true;
      Game1.tileStone[64] = true;
      Game1.tileSolid[65] = true;
      Game1.tileBlockLight[65] = true;
      Game1.tileStone[65] = true;
      Game1.tileSolid[66] = true;
      Game1.tileBlockLight[66] = true;
      Game1.tileStone[66] = true;
      Game1.tileSolid[67] = true;
      Game1.tileBlockLight[67] = true;
      Game1.tileStone[67] = true;
      Game1.tileSolid[68] = true;
      Game1.tileBlockLight[68] = true;
      Game1.tileStone[68] = true;
      Game1.tileSolid[75] = true;
      Game1.tileBlockLight[75] = true;
      Game1.tileSolid[70] = true;
      Game1.tileBlockLight[70] = true;
      Game1.tileBlockLight[51] = true;
      Game1.tileNoFail[50] = true;
      Game1.tileNoAttach[50] = true;
      Game1.tileDungeon[41] = true;
      Game1.tileDungeon[43] = true;
      Game1.tileDungeon[44] = true;
      Game1.tileBlockLight[30] = true;
      Game1.tileBlockLight[25] = true;
      Game1.tileBlockLight[23] = true;
      Game1.tileBlockLight[22] = true;
      Game1.tileBlockLight[62] = true;
      Game1.tileSolidTop[18] = true;
      Game1.tileSolidTop[14] = true;
      Game1.tileSolidTop[16] = true;
      Game1.tileNoAttach[20] = true;
      Game1.tileNoAttach[19] = true;
      Game1.tileNoAttach[13] = true;
      Game1.tileNoAttach[14] = true;
      Game1.tileNoAttach[15] = true;
      Game1.tileNoAttach[16] = true;
      Game1.tileNoAttach[17] = true;
      Game1.tileNoAttach[18] = true;
      Game1.tileNoAttach[19] = true;
      Game1.tileNoAttach[21] = true;
      Game1.tileNoAttach[27] = true;
      Game1.tileFrameImportant[3] = true;
      Game1.tileFrameImportant[5] = true;
      Game1.tileFrameImportant[10] = true;
      Game1.tileFrameImportant[11] = true;
      Game1.tileFrameImportant[12] = true;
      Game1.tileFrameImportant[13] = true;
      Game1.tileFrameImportant[14] = true;
      Game1.tileFrameImportant[15] = true;
      Game1.tileFrameImportant[16] = true;
      Game1.tileFrameImportant[17] = true;
      Game1.tileFrameImportant[18] = true;
      Game1.tileFrameImportant[20] = true;
      Game1.tileFrameImportant[21] = true;
      Game1.tileFrameImportant[24] = true;
      Game1.tileFrameImportant[26] = true;
      Game1.tileFrameImportant[27] = true;
      Game1.tileFrameImportant[28] = true;
      Game1.tileFrameImportant[29] = true;
      Game1.tileFrameImportant[31] = true;
      Game1.tileFrameImportant[33] = true;
      Game1.tileFrameImportant[34] = true;
      Game1.tileFrameImportant[35] = true;
      Game1.tileFrameImportant[36] = true;
      Game1.tileFrameImportant[42] = true;
      Game1.tileFrameImportant[50] = true;
      Game1.tileFrameImportant[55] = true;
      Game1.tileFrameImportant[61] = true;
      Game1.tileFrameImportant[71] = true;
      Game1.tileFrameImportant[72] = true;
      Game1.tileFrameImportant[73] = true;
      Game1.tileFrameImportant[74] = true;
      Game1.tileTable[14] = true;
      Game1.tileTable[18] = true;
      Game1.tileTable[19] = true;
      Game1.tileWaterDeath[4] = true;
      Game1.tileWaterDeath[51] = true;
      Game1.tileLavaDeath[3] = true;
      Game1.tileLavaDeath[5] = true;
      Game1.tileLavaDeath[10] = true;
      Game1.tileLavaDeath[11] = true;
      Game1.tileLavaDeath[12] = true;
      Game1.tileLavaDeath[13] = true;
      Game1.tileLavaDeath[14] = true;
      Game1.tileLavaDeath[15] = true;
      Game1.tileLavaDeath[16] = true;
      Game1.tileLavaDeath[17] = true;
      Game1.tileLavaDeath[18] = true;
      Game1.tileLavaDeath[19] = true;
      Game1.tileLavaDeath[20] = true;
      Game1.tileLavaDeath[27] = true;
      Game1.tileLavaDeath[28] = true;
      Game1.tileLavaDeath[29] = true;
      Game1.tileLavaDeath[32] = true;
      Game1.tileLavaDeath[33] = true;
      Game1.tileLavaDeath[34] = true;
      Game1.tileLavaDeath[35] = true;
      Game1.tileLavaDeath[36] = true;
      Game1.tileLavaDeath[42] = true;
      Game1.tileLavaDeath[49] = true;
      Game1.tileLavaDeath[50] = true;
      Game1.tileLavaDeath[52] = true;
      Game1.tileLavaDeath[55] = true;
      Game1.tileLavaDeath[61] = true;
      Game1.tileLavaDeath[62] = true;
      Game1.tileLavaDeath[69] = true;
      Game1.tileLavaDeath[71] = true;
      Game1.tileLavaDeath[72] = true;
      Game1.tileLavaDeath[73] = true;
      Game1.tileLavaDeath[74] = true;
      Game1.wallHouse[1] = true;
      Game1.wallHouse[4] = true;
      Game1.wallHouse[5] = true;
      Game1.wallHouse[6] = true;
      Game1.wallHouse[10] = true;
      Game1.wallHouse[11] = true;
      Game1.wallHouse[12] = true;

      for (int index = 0; index < Game1.maxMenuItems; ++index)
        this.menuItemScale[index] = 0.8f;

      for (int index = 0; index < 2000; ++index)
        Game1.dust[index] = new Dust();
      for (int index = 0; index < 201; ++index)
        Game1.item[index] = new Item();
      for (int index = 0; index < 1001; ++index)
      {
        Game1.npc[index] = new NPC();
        Game1.npc[index].whoAmI = index;
      }
      for (int index = 0; index < 9; ++index)
        Game1.player[index] = new Player();
      for (int index = 0; index < 1001; ++index)
        Game1.projectile[index] = new Projectile();
      for (int index = 0; index < 201; ++index)
        Game1.gore[index] = new Gore();
      for (int index = 0; index < 100; ++index)
        Game1.cloud[index] = new Cloud();
      for (int index = 0; index < 100; ++index)
        Game1.combatText[index] = new CombatText();
      for (int index = 0; index < Recipe.maxRecipes; ++index)
      {
        Game1.recipe[index] = new Recipe();
        Game1.availableRecipeY[index] = (float) (65 * index);
      }
      Recipe.SetupRecipes();
      for (int index = 0; index < Game1.numChatLines; ++index)
        Game1.chatLine[index] = new ChatLine();
      for (int index = 0; index < Liquid.resLiquid; ++index)
        Game1.liquid[index] = new Liquid();
      for (int index = 0; index < 10000; ++index)
        Game1.liquidBuffer[index] = new LiquidBuffer();
      this.graphics.PreferredBackBufferWidth = Game1.screenWidth;
      this.graphics.PreferredBackBufferHeight = Game1.screenHeight;
      this.graphics.ApplyChanges();
      this.shop[0] = new Chest();
      this.shop[1] = new Chest();
      this.shop[1].SetupShop(1);
      this.shop[2] = new Chest();
      this.shop[2].SetupShop(2);
      this.shop[3] = new Chest();
      this.shop[3].SetupShop(3);
      this.shop[4] = new Chest();
      this.shop[4].SetupShop(4);
      Game1.teamColor[0] = Color.White;
      Game1.teamColor[1] = new Color(230, 40, 20);
      Game1.teamColor[2] = new Color(20, 200, 30);
      Game1.teamColor[3] = new Color(75, 90, (int) byte.MaxValue);
      Game1.teamColor[4] = new Color(200, 180, 0);
      Netplay.Init();

      if (Game1.skipMenu)
      {
        WorldGen.clearWorld();
        Game1.gameMenu = false;
        Game1.LoadPlayers();
        Game1.player[Game1.myPlayer] = (Player) Game1.loadPlayer[0].Clone();
        Game1.PlayerPath = Game1.loadPlayerPath[0];
        Game1.LoadWorlds();
        WorldGen.generateWorld();
        WorldGen.EveryTileFrame();
        Game1.player[Game1.myPlayer].Spawn();
        //IntPtr systemMenu = Game1.GetSystemMenu(this.Window.Handle, false);
        //int menuItemCount = Game1.GetMenuItemCount(systemMenu);
        //Game1.RemoveMenu(systemMenu, menuItemCount - 1, 1024);
      }
      else
      {
        IntPtr systemMenu = Game1.GetSystemMenu(this.Window.Handle, false);
        int menuItemCount = Game1.GetMenuItemCount(systemMenu);
        Game1.RemoveMenu(systemMenu, menuItemCount - 1, 1024);
      }

      base.Initialize();
      Star.SpawnStars();
    }

    protected override void LoadContent()
    {
      Game1.engine = new AudioEngine("Content\\TerrariaMusic.xgs");
      Game1.soundBank = new SoundBank(Game1.engine, "Content\\Sound Bank.xsb");
      Game1.waveBank = new WaveBank(Game1.engine, "Content\\Wave Bank.xwb");

      for (int index = 1; index < 6; ++index)
        Game1.music[index] = Game1.soundBank.GetCue("Music_" + (object) index);
      
      this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
      for (int index = 0; index < 76; ++index)
        Game1.tileTexture[index] = this.Content.Load<Texture2D>("Images\\Tiles_" + (object) index);
      for (int index = 1; index < 13; ++index)
        Game1.wallTexture[index] = this.Content.Load<Texture2D>("Images\\Wall_" + (object) index);
      for (int index = 0; index < 7; ++index)
      {
        Game1.backgroundTexture[index] = this.Content.Load<Texture2D>("Images\\Background_" + (object) index);
        Game1.backgroundWidth[index] = Game1.backgroundTexture[index].Width;
        Game1.backgroundHeight[index] = Game1.backgroundTexture[index].Height;
      }
      for (int index = 0; index < 194; ++index)
        Game1.itemTexture[index] = this.Content.Load<Texture2D>("Images\\Item_" + (object) index);
      for (int index = 0; index < 44; ++index)
        Game1.npcTexture[index] = this.Content.Load<Texture2D>("Images\\NPC_" + (object) index);
      for (int index = 0; index < 34; ++index)
        Game1.projectileTexture[index] = this.Content.Load<Texture2D>("Images\\Projectile_" + (object) index);
      for (int index = 1; index < 73; ++index)
        Game1.goreTexture[index] = this.Content.Load<Texture2D>("Images\\Gore_" + (object) index);
      for (int index = 0; index < 4; ++index)
        Game1.cloudTexture[index] = this.Content.Load<Texture2D>("Images\\Cloud_" + (object) index);
      for (int index = 0; index < 5; ++index)
        Game1.starTexture[index] = this.Content.Load<Texture2D>("Images\\Star_" + (object) index);
      for (int index = 0; index < 2; ++index)
        Game1.liquidTexture[index] = this.Content.Load<Texture2D>("Images\\Liquid_" + (object) index);
      Game1.logoTexture = this.Content.Load<Texture2D>("Images\\Logo");
      Game1.dustTexture = this.Content.Load<Texture2D>("Images\\Dust");
      Game1.sunTexture = this.Content.Load<Texture2D>("Images\\Sun");
      Game1.moonTexture = this.Content.Load<Texture2D>("Images\\Moon");
      Game1.blackTileTexture = this.Content.Load<Texture2D>("Images\\Black_Tile");
      Game1.heartTexture = this.Content.Load<Texture2D>("Images\\Heart");
      Game1.bubbleTexture = this.Content.Load<Texture2D>("Images\\Bubble");
      Game1.manaTexture = this.Content.Load<Texture2D>("Images\\Mana");
      Game1.cursorTexture = this.Content.Load<Texture2D>("Images\\Cursor");
      Game1.treeTopTexture = this.Content.Load<Texture2D>("Images\\Tree_Tops");
      Game1.treeBranchTexture = this.Content.Load<Texture2D>("Images\\Tree_Branches");
      Game1.shroomCapTexture = this.Content.Load<Texture2D>("Images\\Shroom_Tops");
      Game1.inventoryBackTexture = this.Content.Load<Texture2D>("Images\\Inventory_Back");
      Game1.textBackTexture = this.Content.Load<Texture2D>("Images\\Text_Back");
      Game1.chatTexture = this.Content.Load<Texture2D>("Images\\Chat");
      Game1.chat2Texture = this.Content.Load<Texture2D>("Images\\Chat2");
      Game1.chatBackTexture = this.Content.Load<Texture2D>("Images\\Chat_Back");
      Game1.teamTexture = this.Content.Load<Texture2D>("Images\\Team");
      Game1.armorHeadTexture = this.Content.Load<Texture2D>("Images\\Armor_Head");
      Game1.armorBodyTexture = this.Content.Load<Texture2D>("Images\\Armor_Body");
      Game1.armorBody2Texture = this.Content.Load<Texture2D>("Images\\Armor_Body2");
      Game1.armorLegTexture = this.Content.Load<Texture2D>("Images\\Armor_Legs");
      Game1.playerEyeWhitesTexture = this.Content.Load<Texture2D>("Images\\Player_Eye_Whites");
      Game1.playerEyesTexture = this.Content.Load<Texture2D>("Images\\Player_Eyes");
      Game1.playerHairTexture = this.Content.Load<Texture2D>("Images\\Player_Hair");
      Game1.playerHandsTexture = this.Content.Load<Texture2D>("Images\\Player_Hands");
      Game1.playerHands2Texture = this.Content.Load<Texture2D>("Images\\Player_Hands2");
      Game1.playerHeadTexture = this.Content.Load<Texture2D>("Images\\Player_Head");
      Game1.playerPantsTexture = this.Content.Load<Texture2D>("Images\\Player_Pants");
      Game1.playerShirtTexture = this.Content.Load<Texture2D>("Images\\Player_Shirt");
      Game1.playerBeltTexture = this.Content.Load<Texture2D>("Images\\Player_Belt");
      Game1.playerShoesTexture = this.Content.Load<Texture2D>("Images\\Player_Shoes");
      Game1.playerUnderShirtTexture = this.Content.Load<Texture2D>("Images\\Player_Undershirt");
      Game1.playerUnderShirt2Texture = this.Content.Load<Texture2D>("Images\\Player_Undershirt2");
      Game1.chainTexture = this.Content.Load<Texture2D>("Images\\Chain");
      Game1.chain2Texture = this.Content.Load<Texture2D>("Images\\Chain2");
      Game1.chain3Texture = this.Content.Load<Texture2D>("Images\\Chain3");
      Game1.chain4Texture = this.Content.Load<Texture2D>("Images\\Chain4");
      Game1.chain5Texture = this.Content.Load<Texture2D>("Images\\Chain5");
      Game1.boneArmTexture = this.Content.Load<Texture2D>("Images\\Arm_Bone");
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
      Game1.soundInstanceMenuOpen = Game1.soundMenuOpen.CreateInstance();
      Game1.soundMenuClose = this.Content.Load<SoundEffect>("Sounds\\Menu_Close");
      Game1.soundInstanceMenuClose = Game1.soundMenuClose.CreateInstance();
      Game1.soundShatter = this.Content.Load<SoundEffect>("Sounds\\Shatter");
      Game1.soundInstanceShatter = Game1.soundShatter.CreateInstance();
      Game1.soundZombie[0] = this.Content.Load<SoundEffect>("Sounds\\Zombie_0");
      Game1.soundInstanceZombie[0] = Game1.soundZombie[0].CreateInstance();
      Game1.soundZombie[1] = this.Content.Load<SoundEffect>("Sounds\\Zombie_1");
      Game1.soundInstanceZombie[1] = Game1.soundZombie[1].CreateInstance();
      Game1.soundZombie[2] = this.Content.Load<SoundEffect>("Sounds\\Zombie_2");
      Game1.soundInstanceZombie[2] = Game1.soundZombie[2].CreateInstance();
      Game1.soundRoar[0] = this.Content.Load<SoundEffect>("Sounds\\Roar_0");
      Game1.soundInstanceRoar[0] = Game1.soundRoar[0].CreateInstance();
      Game1.soundRoar[1] = this.Content.Load<SoundEffect>("Sounds\\Roar_1");
      Game1.soundInstanceRoar[1] = Game1.soundRoar[1].CreateInstance();
      Game1.soundSplash[0] = this.Content.Load<SoundEffect>("Sounds\\Splash_0");
      Game1.soundInstanceSplash[0] = Game1.soundRoar[0].CreateInstance();
      Game1.soundSplash[1] = this.Content.Load<SoundEffect>("Sounds\\Splash_1");
      Game1.soundInstanceSplash[1] = Game1.soundSplash[1].CreateInstance();
      Game1.soundDoubleJump = this.Content.Load<SoundEffect>("Sounds\\Double_Jump");
      Game1.soundInstanceDoubleJump = Game1.soundRoar[0].CreateInstance();
      Game1.soundRun = this.Content.Load<SoundEffect>("Sounds\\Run");
      Game1.soundInstanceRun = Game1.soundRun.CreateInstance();
      Game1.soundCoins = this.Content.Load<SoundEffect>("Sounds\\Coins");
      Game1.soundInstanceCoins = Game1.soundCoins.CreateInstance();
      for (int index = 1; index < 15; ++index)
      {
        Game1.soundItem[index] = this.Content.Load<SoundEffect>("Sounds\\Item_" + (object) index);
        Game1.soundInstanceItem[index] = Game1.soundItem[index].CreateInstance();
      }
      for (int index = 1; index < 4; ++index)
      {
        Game1.soundNPCHit[index] = this.Content.Load<SoundEffect>("Sounds\\NPC_Hit_" + (object) index);
        Game1.soundInstanceNPCHit[index] = Game1.soundNPCHit[index].CreateInstance();
      }
      for (int index = 1; index < 4; ++index)
      {
        Game1.soundNPCKilled[index] = this.Content.Load<SoundEffect>("Sounds\\NPC_Killed_" + (object) index);
        Game1.soundInstanceNPCKilled[index] = Game1.soundNPCKilled[index].CreateInstance();
      }
      Game1.fontItemStack = this.Content.Load<SpriteFont>("Fonts\\Item_Stack");
      Game1.fontMouseText = this.Content.Load<SpriteFont>("Fonts\\Mouse_Text");
      Game1.fontDeathText = this.Content.Load<SpriteFont>("Fonts\\Death_Text");
      Game1.fontCombatText = this.Content.Load<SpriteFont>("Fonts\\Combat_Text");
    }

    protected override void UnloadContent()
    {
    }

    protected void UpdateMusic()
    {
      bool flag = false;
      for (int index = 0; index < 1000; ++index)
      {
        if (Game1.npc[index].active && (Game1.npc[index].boss || Game1.npc[index].type == 13 || Game1.npc[index].type == 14 || Game1.npc[index].type == 15))
        {
          int num = 5000;
          if (new Rectangle((int) Game1.screenPosition.X, (int) Game1.screenPosition.Y, Game1.screenWidth, Game1.screenHeight).Intersects(new Rectangle((int) ((double) Game1.npc[index].position.X + (double) (Game1.npc[index].width / 2)) - num, (int) ((double) Game1.npc[index].position.Y + (double) (Game1.npc[index].height / 2)) - num, num * 2, num * 2)))
          {
            flag = true;
            break;
          }
        }
      }
      
      if ((double) Game1.musicVolume == 0.0)
        this.newMusic = 0;
      else if (Game1.gameMenu)
        this.newMusic = Game1.netMode == 2 ? 0 : 1;
      else if (flag)
        this.newMusic = 5;
      else if (Game1.player[Game1.myPlayer].zoneEvil || Game1.player[Game1.myPlayer].zoneMeteor || Game1.player[Game1.myPlayer].zoneDungeon)
        this.newMusic = 2;
      else if ((double) Game1.player[Game1.myPlayer].position.Y > (double) ((Game1.maxTilesY - 200) * 16))
        this.newMusic = 2;
      else if ((double) Game1.player[Game1.myPlayer].position.Y > Game1.worldSurface * 16.0 + (double) Game1.screenHeight)
        this.newMusic = 4;
      else if (Game1.dayTime)
        this.newMusic = 1;
      else if (!Game1.dayTime)
        this.newMusic = !Game1.bloodMoon ? 3 : 2;

            this.curMusic = this.newMusic;

      for (int index = 1; index < 6; ++index)
      {
        if (index == this.curMusic)
        {
          if (!Game1.music[index].IsPlaying)
          {
            Game1.music[index] = Game1.soundBank.GetCue("Music_" + (object) index);
            Game1.music[index].Play();
            Game1.music[index].SetVariable("Volume", Game1.musicFade[index] * Game1.musicVolume);
          }
          else
          {
            Game1.musicFade[index] += 0.005f;
            if ((double) Game1.musicFade[index] > 1.0)
              Game1.musicFade[index] = 1f;
            Game1.music[index].SetVariable("Volume", Game1.musicFade[index] * Game1.musicVolume);
          }
        }
        else if (Game1.music[index].IsPlaying)
        {
          if ((double) Game1.musicFade[this.curMusic] > 0.25)
            Game1.musicFade[index] -= 0.005f;
          else if (this.curMusic == 0)
            Game1.musicFade[index] = 0.0f;
          if ((double) Game1.musicFade[index] <= 0.0)
          {
            Game1.musicFade[index] -= 0.0f;
            Game1.music[index].Stop(AudioStopOptions.Immediate);
          }
          else
            Game1.music[index].SetVariable("Volume", Game1.musicFade[index] * Game1.musicVolume);
        }
        else
          Game1.musicFade[index] = 0.0f;
      }
    }

    protected override void Update(GameTime gameTime)
    {
      this.UpdateMusic();
      if (Game1.rand.Next(99999) == 0)
        Game1.rand = new Random((int) DateTime.Now.Ticks);
      ++Game1.updateTime;
      if (Game1.updateTime >= 60)
      {
        Game1.frameRate = Game1.drawTime;
        Game1.updateTime = 0;
        Game1.drawTime = 0;
        if (Game1.frameRate == 60)
        {
          Lighting.lightPasses = 2;
          Lighting.lightSkip = 0;
          Game1.cloudLimit = 100;
          Gore.goreTime = 1200;
        }
        else if (Game1.frameRate >= 58)
        {
          Lighting.lightPasses = 2;
          Lighting.lightSkip = 0;
          Game1.cloudLimit = 100;
          Gore.goreTime = 600;
        }
        else if (Game1.frameRate >= 43)
        {
          Lighting.lightPasses = 2;
          Lighting.lightSkip = 1;
          Game1.cloudLimit = 75;
          Gore.goreTime = 300;
        }
        else if (Game1.frameRate >= 28)
        {
          if (!Game1.gameMenu)
          {
            Liquid.maxLiquid = 3000;
            Liquid.cycles = 6;
          }
          Lighting.lightPasses = 2;
          Lighting.lightSkip = 2;
          Game1.cloudLimit = 50;
          Gore.goreTime = 180;
        }
        else
        {
          Lighting.lightPasses = 2;
          Lighting.lightSkip = 4;
          Game1.cloudLimit = 0;
          Gore.goreTime = 0;
        }
        if (Liquid.quickSettle)
        {
          Liquid.maxLiquid = Liquid.resLiquid;
          Liquid.cycles = 1;
        }
        else if (Game1.frameRate == 60)
        {
          Liquid.maxLiquid = 5000;
          Liquid.cycles = 7;
        }
        else if (Game1.frameRate >= 58)
        {
          Liquid.maxLiquid = 5000;
          Liquid.cycles = 12;
        }
        else if (Game1.frameRate >= 43)
        {
          Liquid.maxLiquid = 4000;
          Liquid.cycles = 13;
        }
        else if (Game1.frameRate >= 28)
        {
          Liquid.maxLiquid = 3500;
          Liquid.cycles = 15;
        }
        else
        {
          Liquid.maxLiquid = 3000;
          Liquid.cycles = 17;
        }
        if (Game1.netMode == 2)
          Game1.cloudLimit = 0;
      }
      Game1.hasFocus = this.IsActive;
      if (!this.IsActive && Game1.netMode == 0)
      {
        this.IsMouseVisible = true;
        if (Game1.netMode != 2 && Game1.myPlayer >= 0)
          Game1.player[Game1.myPlayer].delayUseItem = true;
        Game1.mouseLeftRelease = false;
        Game1.mouseRightRelease = false;
        if (!Game1.gameMenu)
          return;
        Game1.UpdateMenu();
      }
      else
      {
        this.IsMouseVisible = false;
        if (Game1.keyState.IsKeyDown(Keys.F) && !Game1.chatMode)
        {
          if (Game1.frameRelease)
          {
            Game1.PlaySound(12);
            Game1.showFrameRate = !Game1.showFrameRate;
          }
          Game1.frameRelease = false;
        }
        else
          Game1.frameRelease = true;
        if (Game1.keyState.IsKeyDown(Keys.F11))
        {
          if (Game1.releaseUI)
            Game1.hideUI = !Game1.hideUI;
          Game1.releaseUI = false;
        }
        else
          Game1.releaseUI = true;
        if ((Game1.keyState.IsKeyDown(Keys.LeftAlt) || Game1.keyState.IsKeyDown(Keys.RightAlt)) && Game1.keyState.IsKeyDown(Keys.Enter))
        {
          if (this.toggleFullscreen)
          {
            this.graphics.ToggleFullScreen();
            Game1.chatRelease = false;
          }
          this.toggleFullscreen = false;
        }
        else
          this.toggleFullscreen = true;

        Game1.oldMouseState = Game1.mouseState;
        Game1.mouseState = TouchPanel.GetState();//Mouse.GetState();

        Game1.keyState = Keyboard.GetState();
        if (Game1.editSign)
          Game1.chatMode = false;
        if (Game1.chatMode)
        {
          string chatText = Game1.chatText;
          Game1.chatText = Game1.GetInputText(Game1.chatText);
          while ((double) Game1.fontMouseText.MeasureString(Game1.chatText).X > 470.0)
            Game1.chatText = Game1.chatText.Substring(0, Game1.chatText.Length - 1);
          if (chatText != Game1.chatText)
            Game1.PlaySound(12);
          if (Game1.inputTextEnter && Game1.chatRelease)
          {
            if (Game1.chatText != "")
              NetMessage.SendData(25, text: Game1.chatText, number: Game1.myPlayer);
            Game1.chatText = "";
            Game1.chatMode = false;
            Game1.chatRelease = false;
            Game1.PlaySound(11);
          }
        }
        if (Game1.keyState.IsKeyDown(Keys.Enter) && Game1.netMode == 1)
        {
          if (Game1.chatRelease && !Game1.chatMode && !Game1.editSign)
          {
            Game1.PlaySound(10);
            Game1.chatMode = true;
            Game1.chatText = "";
          }
          Game1.chatRelease = false;
        }
        else
          Game1.chatRelease = true;
        if (Game1.gameMenu)
        {
          Game1.UpdateMenu();
          if (Game1.netMode != 2)
            return;
        }
        
        if (Game1.debugMode)
          Game1.UpdateDebug();

        if (Game1.netMode == 1)
        {
          for (int number2 = 0; number2 < 44; ++number2)
          {
            if (Game1.player[Game1.myPlayer].inventory[number2].IsNotTheSameAs(Game1.clientPlayer.inventory[number2]))
              NetMessage.SendData(5, text: Game1.player[Game1.myPlayer].inventory[number2].name, number: Game1.myPlayer, number2: (float) number2);
          }
          if (Game1.player[Game1.myPlayer].armor[0].IsNotTheSameAs(Game1.clientPlayer.armor[0]))
            NetMessage.SendData(5, text: Game1.player[Game1.myPlayer].armor[0].name, number: Game1.myPlayer, number2: 44f);
          if (Game1.player[Game1.myPlayer].armor[1].IsNotTheSameAs(Game1.clientPlayer.armor[1]))
            NetMessage.SendData(5, text: Game1.player[Game1.myPlayer].armor[1].name, number: Game1.myPlayer, number2: 45f);
          if (Game1.player[Game1.myPlayer].armor[2].IsNotTheSameAs(Game1.clientPlayer.armor[2]))
            NetMessage.SendData(5, text: Game1.player[Game1.myPlayer].armor[2].name, number: Game1.myPlayer, number2: 46f);
          if (Game1.player[Game1.myPlayer].armor[3].IsNotTheSameAs(Game1.clientPlayer.armor[3]))
            NetMessage.SendData(5, text: Game1.player[Game1.myPlayer].armor[3].name, number: Game1.myPlayer, number2: 47f);
          if (Game1.player[Game1.myPlayer].armor[4].IsNotTheSameAs(Game1.clientPlayer.armor[4]))
            NetMessage.SendData(5, text: Game1.player[Game1.myPlayer].armor[4].name, number: Game1.myPlayer, number2: 48f);
          if (Game1.player[Game1.myPlayer].armor[5].IsNotTheSameAs(Game1.clientPlayer.armor[5]))
            NetMessage.SendData(5, text: Game1.player[Game1.myPlayer].armor[5].name, number: Game1.myPlayer, number2: 49f);
          if (Game1.player[Game1.myPlayer].chest != Game1.clientPlayer.chest)
            NetMessage.SendData(33, number: Game1.player[Game1.myPlayer].chest);
          if (Game1.player[Game1.myPlayer].talkNPC != Game1.clientPlayer.talkNPC)
            NetMessage.SendData(40, number: Game1.myPlayer);
          if (Game1.player[Game1.myPlayer].zoneEvil != Game1.clientPlayer.zoneEvil)
            NetMessage.SendData(36, number: Game1.myPlayer);
          if (Game1.player[Game1.myPlayer].zoneMeteor != Game1.clientPlayer.zoneMeteor)
            NetMessage.SendData(36, number: Game1.myPlayer);
          if (Game1.player[Game1.myPlayer].zoneDungeon != Game1.clientPlayer.zoneDungeon)
            NetMessage.SendData(36, number: Game1.myPlayer);
          if (Game1.player[Game1.myPlayer].zoneJungle != Game1.clientPlayer.zoneJungle)
            NetMessage.SendData(36, number: Game1.myPlayer);
        }
        if (Game1.netMode == 1)
          Game1.clientPlayer = (Player) Game1.player[Game1.myPlayer].clientClone();
        for (int i = 0; i < 8; ++i)
          Game1.player[i].UpdatePlayer(i);
        if (Game1.netMode != 1)
          NPC.SpawnNPC();
        for (int index = 0; index < 8; ++index)
        {
          Game1.player[index].activeNPCs = 0;
          Game1.player[index].townNPCs = 0;
        }
        for (int i = 0; i < 1000; ++i)
          Game1.npc[i].UpdateNPC(i);
        for (int index = 0; index < 200; ++index)
          Game1.gore[index].Update();
        for (int i = 0; i < 1000; ++i)
          Game1.projectile[i].Update(i);
        for (int i = 0; i < 200; ++i)
          Game1.item[i].UpdateItem(i);
        Dust.UpdateDust();
        CombatText.UpdateCombatText();
        Game1.UpdateTime();
        if (Game1.netMode != 1)
        {
          WorldGen.UpdateWorld();
          Game1.UpdateInvasion();
        }
        if (Game1.netMode == 2)
          Game1.UpdateServer();
        if (Game1.netMode == 1)
          Game1.UpdateClient();
        for (int index = 0; index < Game1.numChatLines; ++index)
        {
          if (Game1.chatLine[index].showTime > 0)
            --Game1.chatLine[index].showTime;
        }
        base.Update(gameTime);
      }
    }

    private static void UpdateMenu()
    {
      Game1.playerInventory = false;
      Game1.exitScale = 0.8f;
      switch (Game1.netMode)
      {
        case 0:
          if (Game1.grabSky)
            break;
          Game1.time += 86.4;
          if (!Game1.dayTime)
          {
            if (Game1.time > 32400.0)
            {
              Game1.bloodMoon = false;
              Game1.time = 0.0;
              Game1.dayTime = true;
              ++Game1.moonPhase;
              if (Game1.moonPhase >= 8)
                Game1.moonPhase = 0;
            }
          }
          else if (Game1.time > 54000.0)
          {
            Game1.time = 0.0;
            Game1.dayTime = false;
          }
          break;
        case 1:
          Game1.UpdateTime();
          break;
      }
    }

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        // extern
        public static short GetKeyState(int keyCode)
        {
            return default;
        }

    public static string GetInputText(string oldString)
    {
      if (!Game1.hasFocus)
        return oldString;
            
            Game1.inputTextEnter = false;
            string inputText = oldString ?? "";
            
            Game1.oldInputText = Game1.inputText;
            
            //RnD
            /*
            Game1.inputText = Keyboard.GetState();
            bool flag1 = ((int) (ushort) Game1.GetKeyState(20) & (int) ushort.MaxValue) != 0;
            bool flag2 = false;
            if (Game1.inputText.IsKeyDown(Keys.LeftShift) || Game1.inputText.IsKeyDown(Keys.RightShift))
              flag2 = true;
            Keys[] pressedKeys1 = Game1.inputText.GetPressedKeys();
            Keys[] pressedKeys2 = Game1.oldInputText.GetPressedKeys();
            bool flag3 = false;
            if (Game1.inputText.IsKeyDown(Keys.Back) && Game1.oldInputText.IsKeyDown(Keys.Back))
            {
              if (Game1.backSpaceCount == 0)
              {
                Game1.backSpaceCount = 7;
                flag3 = true;
              }
              --Game1.backSpaceCount;
            }
            else
              Game1.backSpaceCount = 15;
            for (int index1 = 0; index1 < pressedKeys1.Length; ++index1)
            {
              bool flag4 = true;
              for (int index2 = 0; index2 < pressedKeys2.Length; ++index2)
              {
                if (pressedKeys1[index1] == pressedKeys2[index2])
                  flag4 = false;
              }
              string str = string.Concat((object) pressedKeys1[index1]);
              if (str == "Back" && (flag4 || flag3))
              {
                if (inputText.Length > 0)
                  inputText = inputText.Substring(0, inputText.Length - 1);
              }
              else if (flag4)
              {
                if (str == "Space")
                  str = " ";
                else if (str.Length == 1)
                {
                  int int32 = Convert.ToInt32(Convert.ToChar(str));
                  if (int32 >= 65 && int32 <= 90 && (!flag2 && !flag1 || flag2 && flag1))
                    str = string.Concat((object) Convert.ToChar(int32 + 32));
                }
                else if (str.Length == 2 && str.Substring(0, 1) == "D")
                {
                  str = str.Substring(1, 1);
                  if (flag2)
                  {
                    if (str == "1")
                      str = "!";
                    if (str == "2")
                      str = "@";
                    if (str == "3")
                      str = "#";
                    if (str == "4")
                      str = "$";
                    if (str == "5")
                      str = "%";
                    if (str == "6")
                      str = "^";
                    if (str == "7")
                      str = "&";
                    if (str == "8")
                      str = "*";
                    if (str == "9")
                      str = "(";
                    if (str == "0")
                      str = ")";
                  }
                }
                else if (str.Length == 7 && str.Substring(0, 6) == "NumPad")
                {
                  str = str.Substring(6, 1);
                }
                else
                {
                  switch (str)
                  {
                    case "Divide":
                      str = "/";
                      goto label_82;
                    case "Multiply":
                      str = "*";
                      goto label_82;
                    case "Subtract":
                      str = "-";
                      goto label_82;
                    case "Add":
                      str = "+";
                      goto label_82;
                    case "Decimal":
                      str = ".";
                      goto label_82;
                    case "OemSemicolon":
                      str = ";";
                      break;
                    case "OemPlus":
                      str = "=";
                      break;
                    case "OemComma":
                      str = ",";
                      break;
                    case "OemMinus":
                      str = "-";
                      break;
                    case "OemPeriod":
                      str = ".";
                      break;
                    case "OemQuestion":
                      str = "/";
                      break;
                    case "OemTilde":
                      str = "`";
                      break;
                    case "OemOpenBrackets":
                      str = "[";
                      break;
                    case "OemPipe":
                      str = "\\";
                      break;
                    case "OemCloseBrackets":
                      str = "]";
                      break;
                    case "OemQuotes":
                      str = "'";
                      break;
                    case "OemBackslash":
                      str = "\\";
                      break;
                  }
                  if (flag2)
                  {
                    switch (str)
                    {
                      case ";":
                        str = ":";
                        break;
                      case "=":
                        str = "+";
                        break;
                      case ",":
                        str = "<";
                        break;
                      case "-":
                        str = "_";
                        break;
                      case ".":
                        str = ">";
                        break;
                      case "/":
                        str = "?";
                        break;
                      case "`":
                        str = "~";
                        break;
                      case "[":
                        str = "{";
                        break;
                      case "\\":
                        str = "|";
                        break;
                      case "]":
                        str = "}";
                        break;
                      case "'":
                        str = "\"";
                        break;
                    }
                  }
                }
      label_82:
                if (str == "Enter")
                  Game1.inputTextEnter = true;
                if (str.Length == 1)
                  inputText += str;
              }
            }
            */
      inputText = "TEST";
      return inputText;
    }

    protected void MouseText(string cursorText, int rare = 0)
    {
      int x1 = (int)Game1.mouseState[0].Position.X + 10;
      int y1 = (int)Game1.mouseState[0].Position.Y + 10;
      Color color1 = new Color((int) Game1.mouseTextColor, 
          (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor);
      if (Game1.toolTip.type > 0)
      {
        rare = Game1.toolTip.rare;
        int length = 20;
        int index1 = 1;
        string[] strArray1 = new string[length];
        strArray1[0] = Game1.toolTip.name;
        if (Game1.toolTip.stack > 1)
        {
          string[] strArray2;
          string str = (strArray2 = strArray1)[0] + " (" + (object) Game1.toolTip.stack + ")";
          strArray2[0] = str;
        }
        if (Game1.toolTip.damage > 0)
        {
          strArray1[index1] = Game1.toolTip.damage.ToString() + " damage";
          ++index1;
          if (Game1.toolTip.useStyle > 0)
          {
            strArray1[index1] = Game1.toolTip.useAnimation > 8 ? (Game1.toolTip.useAnimation > 15 ? (Game1.toolTip.useAnimation > 20 ? (Game1.toolTip.useAnimation > 25 ? (Game1.toolTip.useAnimation > 30 ? (Game1.toolTip.useAnimation > 40 ? (Game1.toolTip.useAnimation > 50 ? "Snail" : "Extremly slow") : "Very slow") : "Slow") : "Average") : "Fast") : "Very fast") : "Insanely fast";
            string[] strArray3;
            IntPtr index2;
             //RnD
            //(strArray3 = strArray1)[(int) (index2 = (IntPtr) index1)] = strArray3[index2] + " speed";
            ++index1;
          }
        }
        if (Game1.toolTip.headSlot > 0 || Game1.toolTip.bodySlot > 0 || Game1.toolTip.legSlot > 0 || Game1.toolTip.accessory)
        {
          strArray1[index1] = "Equipable";
          ++index1;
        }
        if (Game1.toolTip.defense > 0)
        {
          strArray1[index1] = Game1.toolTip.defense.ToString() + " defense";
          ++index1;
        }
        if (Game1.toolTip.pick > 0)
        {
          strArray1[index1] = Game1.toolTip.pick.ToString() + "% pickaxe power";
          ++index1;
        }
        if (Game1.toolTip.axe > 0)
        {
          strArray1[index1] = Game1.toolTip.axe.ToString() + "% axe power";
          ++index1;
        }
        if (Game1.toolTip.hammer > 0)
        {
          strArray1[index1] = Game1.toolTip.hammer.ToString() + "% hammer power";
          ++index1;
        }
        if (Game1.toolTip.healLife > 0)
        {
          strArray1[index1] = "Restores " + (object) Game1.toolTip.healLife + " life";
          ++index1;
        }
        if (Game1.toolTip.healMana > 0)
        {
          strArray1[index1] = "Restores " + (object) Game1.toolTip.healMana + " mana";
          ++index1;
        }
        if (Game1.toolTip.mana > 0)
        {
          strArray1[index1] = "Uses " + (object) (int) ((double) Game1.toolTip.mana * (double) Game1.player[Game1.myPlayer].manaCost) + " mana";
          ++index1;
        }
        if (Game1.toolTip.createWall > 0 || Game1.toolTip.createTile > -1)
        {
          strArray1[index1] = "Can be placed";
          ++index1;
        }
        if (Game1.toolTip.consumable)
        {
          strArray1[index1] = "Consumable";
          ++index1;
        }
        if (Game1.toolTip.toolTip != null)
        {
          strArray1[index1] = Game1.toolTip.toolTip;
          ++index1;
        }
        if (Game1.toolTip.wornArmor && Game1.player[Game1.myPlayer].setBonus != "")
        {
          strArray1[index1] = "Set bonus: " + Game1.player[Game1.myPlayer].setBonus;
          ++index1;
        }
        if (Game1.npcShop > 0)
        {
          if (Game1.toolTip.value > 0)
          {
            string str = "";
            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = Game1.toolTip.value * Game1.toolTip.stack;
            if (!Game1.toolTip.buy)
              num5 /= 5;
            if (num5 < 1)
              num5 = 1;
            if (num5 >= 1000000)
            {
              num1 = num5 / 1000000;
              num5 -= num1 * 1000000;
            }
            if (num5 >= 10000)
            {
              num2 = num5 / 10000;
              num5 -= num2 * 10000;
            }
            if (num5 >= 100)
            {
              num3 = num5 / 100;
              num5 -= num3 * 100;
            }
            if (num5 >= 1)
              num4 = num5;
            if (num1 > 0)
              str = str + (object) num1 + " platinum ";
            if (num2 > 0)
              str = str + (object) num2 + " gold ";
            if (num3 > 0)
              str = str + (object) num3 + " silver ";
            if (num4 > 0)
              str = str + (object) num4 + " copper ";
            strArray1[index1] = Game1.toolTip.buy ? "Buy price: " + str : "Sell price: " + str;
            ++index1;
            float num6 = (float) Game1.mouseTextColor / (float) byte.MaxValue;
            if (num1 > 0)
              color1 = new Color((int) (byte) (220.0 * (double) num6), (int) (byte) (220.0 * (double) num6), (int) (byte) (198.0 * (double) num6), (int) Game1.mouseTextColor);
            else if (num2 > 0)
              color1 = new Color((int) (byte) (224.0 * (double) num6), (int) (byte) (201.0 * (double) num6), (int) (byte) (92.0 * (double) num6), (int) Game1.mouseTextColor);
            else if (num3 > 0)
              color1 = new Color((int) (byte) (181.0 * (double) num6), (int) (byte) (192.0 * (double) num6), (int) (byte) (193.0 * (double) num6), (int) Game1.mouseTextColor);
            else if (num4 > 0)
              color1 = new Color((int) (byte) (246.0 * (double) num6), (int) (byte) (138.0 * (double) num6), (int) (byte) (96.0 * (double) num6), (int) Game1.mouseTextColor);
          }
          else
          {
            float num = (float) Game1.mouseTextColor / (float) byte.MaxValue;
            strArray1[index1] = "No value";
            ++index1;
            color1 = new Color((int) (byte) (120.0 * (double) num), (int) (byte) (120.0 * (double) num), (int) (byte) (120.0 * (double) num), (int) Game1.mouseTextColor);
          }
        }
        Vector2 vector2_1 = new Vector2();
        int num7 = 0;
        for (int index3 = 0; index3 < index1; ++index3)
        {
          Vector2 vector2_2 = Game1.fontMouseText.MeasureString(strArray1[index3]);
          if ((double) vector2_2.X > (double) vector2_1.X)
            vector2_1.X = vector2_2.X;
          vector2_1.Y += vector2_2.Y + (float) num7;
        }
        if ((double) x1 + (double) vector2_1.X + 4.0 > (double) Game1.screenWidth)
          x1 = (int) ((double) Game1.screenWidth - (double) vector2_1.X - 4.0);
        if ((double) y1 + (double) vector2_1.Y + 4.0 > (double) Game1.screenHeight)
          y1 = (int) ((double) Game1.screenHeight - (double) vector2_1.Y - 4.0);
        int num8 = 0;
        float num9 = (float) Game1.mouseTextColor / (float) byte.MaxValue;
        for (int index4 = 0; index4 < index1; ++index4)
        {
          for (int index5 = 0; index5 < 5; ++index5)
          {
            int x2 = x1;
            int y2 = y1 + num8;
            Color color2 = Color.Black;
            if (index5 == 0)
              x2 -= 2;
            else if (index5 == 1)
              x2 += 2;
            else if (index5 == 2)
              y2 -= 2;
            else if (index5 == 3)
            {
              y2 += 2;
            }
            else
            {
              color2 = new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor);
              if (index4 == 0)
              {
                if (rare == 1)
                  color2 = new Color((int) (byte) (150.0 * (double) num9), (int) (byte) (150.0 * (double) num9), (int) (byte) ((double) byte.MaxValue * (double) num9), (int) Game1.mouseTextColor);
                if (rare == 2)
                  color2 = new Color((int) (byte) (150.0 * (double) num9), (int) (byte) ((double) byte.MaxValue * (double) num9), (int) (byte) (150.0 * (double) num9), (int) Game1.mouseTextColor);
                if (rare == 3)
                  color2 = new Color((int) (byte) ((double) byte.MaxValue * (double) num9), (int) (byte) (200.0 * (double) num9), (int) (byte) (150.0 * (double) num9), (int) Game1.mouseTextColor);
                if (rare == 4)
                  color2 = new Color((int) (byte) ((double) byte.MaxValue * (double) num9), (int) (byte) (150.0 * (double) num9), (int) (byte) (150.0 * (double) num9), (int) Game1.mouseTextColor);
              }
              else if (index4 == index1 - 1)
                color2 = color1;
            }
            this.spriteBatch.DrawString(Game1.fontMouseText, strArray1[index4], new Vector2((float) x2, (float) y2), color2, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
          }
          num8 += (int) ((double) Game1.fontMouseText.MeasureString(strArray1[index4]).Y + (double) num7);
        }
      }
      else
      {
        Vector2 vector2 = Game1.fontMouseText.MeasureString(cursorText);
        if ((double) x1 + (double) vector2.X + 4.0 > (double) Game1.screenWidth)
          x1 = (int) ((double) Game1.screenWidth - (double) vector2.X - 4.0);
        if ((double) y1 + (double) vector2.Y + 4.0 > (double) Game1.screenHeight)
          y1 = (int) ((double) Game1.screenHeight - (double) vector2.Y - 4.0);
        this.spriteBatch.DrawString(Game1.fontMouseText, cursorText, new Vector2((float) x1, (float) (y1 - 2)), Color.Black, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
        this.spriteBatch.DrawString(Game1.fontMouseText, cursorText, new Vector2((float) x1, (float) (y1 + 2)), Color.Black, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
        this.spriteBatch.DrawString(Game1.fontMouseText, cursorText, new Vector2((float) (x1 - 2), (float) y1), Color.Black, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
        this.spriteBatch.DrawString(Game1.fontMouseText, cursorText, new Vector2((float) (x1 + 2), (float) y1), Color.Black, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
        float num = (float) Game1.mouseTextColor / (float) byte.MaxValue;
        Color color3 = new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor);
        if (rare == 1)
          color3 = new Color((int) (byte) (150.0 * (double) num), (int) (byte) (150.0 * (double) num), (int) (byte) ((double) byte.MaxValue * (double) num), (int) Game1.mouseTextColor);
        if (rare == 2)
          color3 = new Color((int) (byte) (150.0 * (double) num), (int) (byte) ((double) byte.MaxValue * (double) num), (int) (byte) (150.0 * (double) num), (int) Game1.mouseTextColor);
        if (rare == 3)
          color3 = new Color((int) (byte) ((double) byte.MaxValue * (double) num), (int) (byte) (200.0 * (double) num), (int) (byte) (150.0 * (double) num), (int) Game1.mouseTextColor);
        if (rare == 4)
          color3 = new Color((int) (byte) ((double) byte.MaxValue * (double) num), (int) (byte) (150.0 * (double) num), (int) (byte) (150.0 * (double) num), (int) Game1.mouseTextColor);
        this.spriteBatch.DrawString(Game1.fontMouseText, cursorText, new Vector2((float) x1, (float) y1), color3, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
      }
    }

    protected void DrawFPS()
    {
      if (!Game1.showFrameRate)
        return;
      string text = string.Concat((object) Game1.frameRate);
      if (Game1.netMode == 1 && Game1.showSpam)
        text = text + " (" + (object) NetMessage.buffer[9].maxSpam + ")";
      if (Game1.netMode != 1)
        text = text + " (" + (object) (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer) + ")";
      this.spriteBatch.DrawString(Game1.fontMouseText, text, new Vector2(4f, (float) (Game1.screenHeight - 24)), new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
    }

    protected void DrawTiles(bool solidOnly = true)
    {
      int num1 = (int) ((double) Game1.screenPosition.X / 16.0 - 1.0);
      int num2 = (int) (((double) Game1.screenPosition.X + (double) Game1.screenWidth) / 16.0) + 2;
      int num3 = (int) ((double) Game1.screenPosition.Y / 16.0 - 1.0);
      int num4 = (int) (((double) Game1.screenPosition.Y + (double) Game1.screenHeight) / 16.0) + 2;
      if (num1 < 0)
        num1 = 0;
      if (num2 > Game1.maxTilesX)
        num2 = Game1.maxTilesX;
      if (num3 < 0)
        num3 = 0;
      if (num4 > Game1.maxTilesY)
        num4 = Game1.maxTilesY;
      for (int y = num3; y < num4 + 4; ++y)
      {
        int num5 = y - num3 + 21;
        for (int x = num1 - 2; x < num2 + 2; ++x)
        {
          int num6 = x - num1 + 21;
          if (Game1.tile[x, y].active && Game1.tileSolid[(int) Game1.tile[x, y].type] == solidOnly)
          {
            int num7 = 0;
            if (Game1.tile[x, y].type == (byte) 33 || Game1.tile[x, y].type == (byte) 49)
              num7 = -4;
            int height = Game1.tile[x, y].type != (byte) 3 && Game1.tile[x, y].type != (byte) 4 && Game1.tile[x, y].type != (byte) 5 && Game1.tile[x, y].type != (byte) 24 && Game1.tile[x, y].type != (byte) 33 && Game1.tile[x, y].type != (byte) 49 && Game1.tile[x, y].type != (byte) 61 && Game1.tile[x, y].type != (byte) 71 ? (Game1.tile[x, y].type != (byte) 15 && Game1.tile[x, y].type != (byte) 14 && Game1.tile[x, y].type != (byte) 16 && Game1.tile[x, y].type != (byte) 17 && Game1.tile[x, y].type != (byte) 18 && Game1.tile[x, y].type != (byte) 20 && Game1.tile[x, y].type != (byte) 21 && Game1.tile[x, y].type != (byte) 26 && Game1.tile[x, y].type != (byte) 27 && Game1.tile[x, y].type != (byte) 32 && Game1.tile[x, y].type != (byte) 69 && Game1.tile[x, y].type != (byte) 72 ? 16 : 18) : 20;
            int width = Game1.tile[x, y].type != (byte) 4 && Game1.tile[x, y].type != (byte) 5 ? 16 : 20;
            if (Game1.tile[x, y].type == (byte) 73 || Game1.tile[x, y].type == (byte) 74)
            {
              num7 -= 12;
              height = 32;
            }
            if (Game1.tile[x, y].type == (byte) 4 && Game1.rand.Next(40) == 0 && this.IsActive)
            {
              if (Game1.tile[x, y].frameX == (short) 22)
                Dust.NewDust(new Vector2((float) (x * 16 + 6), (float) (y * 16)), 4, 4, 6, Alpha: 100);
              if (Game1.tile[x, y].frameX == (short) 44)
                Dust.NewDust(new Vector2((float) (x * 16 + 2), (float) (y * 16)), 4, 4, 6, Alpha: 100);
              else
                Dust.NewDust(new Vector2((float) (x * 16 + 4), (float) (y * 16)), 4, 4, 6, Alpha: 100);
            }
            if (Game1.tile[x, y].type == (byte) 33 && Game1.rand.Next(40) == 0 && this.IsActive)
              Dust.NewDust(new Vector2((float) (x * 16 + 4), (float) (y * 16 - 4)), 4, 4, 6, Alpha: 100);
            if (Game1.tile[x, y].type == (byte) 49 && Game1.rand.Next(20) == 0 && this.IsActive)
              Dust.NewDust(new Vector2((float) (x * 16 + 4), (float) (y * 16 - 4)), 4, 4, 29, Alpha: 100);
            if ((Game1.tile[x, y].type == (byte) 34 || Game1.tile[x, y].type == (byte) 35 || Game1.tile[x, y].type == (byte) 36) && Game1.rand.Next(40) == 0 && this.IsActive && Game1.tile[x, y].frameY == (short) 18 && (Game1.tile[x, y].frameX == (short) 0 || Game1.tile[x, y].frameX == (short) 36))
              Dust.NewDust(new Vector2((float) (x * 16), (float) (y * 16 + 2)), 14, 6, 6, Alpha: 100);
            if (Game1.tile[x, y].type == (byte) 22 && Game1.rand.Next(400) == 0 && this.IsActive)
              Dust.NewDust(new Vector2((float) (x * 16), (float) (y * 16)), 16, 16, 14);
            else if ((Game1.tile[x, y].type == (byte) 23 || Game1.tile[x, y].type == (byte) 24 || Game1.tile[x, y].type == (byte) 32) && Game1.rand.Next(500) == 0 && this.IsActive)
              Dust.NewDust(new Vector2((float) (x * 16), (float) (y * 16)), 16, 16, 14);
            else if (Game1.tile[x, y].type == (byte) 25 && Game1.rand.Next(700) == 0 && this.IsActive)
              Dust.NewDust(new Vector2((float) (x * 16), (float) (y * 16)), 16, 16, 14);
            else if (Game1.tile[x, y].type == (byte) 31 && Game1.rand.Next(20) == 0 && this.IsActive)
              Dust.NewDust(new Vector2((float) (x * 16), (float) (y * 16)), 16, 16, 14, Alpha: 100);
            else if ((Game1.tile[x, y].type == (byte) 71 || Game1.tile[x, y].type == (byte) 72) && Game1.rand.Next(500) == 0 && this.IsActive)
              Dust.NewDust(new Vector2((float) (x * 16), (float) (y * 16)), 16, 16, 41, Alpha: 250, Scale: 0.8f);
            else if (Game1.tile[x, y].type == (byte) 17 && Game1.rand.Next(40) == 0 && this.IsActive)
            {
              if (Game1.tile[x, y].frameX == (short) 18 & Game1.tile[x, y].frameY == (short) 18)
                Dust.NewDust(new Vector2((float) (x * 16 + 2), (float) (y * 16)), 8, 6, 6, Alpha: 100);
            }
            else if ((Game1.tile[x, y].type == (byte) 37 || Game1.tile[x, y].type == (byte) 58) && Game1.rand.Next(200) == 0 && this.IsActive)
            {
              int index = Dust.NewDust(new Vector2((float) (x * 16), (float) (y * 16)), 16, 16, 6, Scale: (float) Game1.rand.Next(3));
              if ((double) Game1.dust[index].scale > 1.0)
                Game1.dust[index].noGravity = true;
            }
            if (Game1.tile[x, y].type == (byte) 5 && Game1.tile[x, y].frameY >= (short) 198 && Game1.tile[x, y].frameX >= (short) 22)
            {
              int num8 = 0;
              if (Game1.tile[x, y].frameX == (short) 22)
              {
                if (Game1.tile[x, y].frameY == (short) 220)
                  num8 = 1;
                else if (Game1.tile[x, y].frameY == (short) 242)
                  num8 = 2;
                this.spriteBatch.Draw(Game1.treeTopTexture, new Vector2((float) (x * 16 - (int) Game1.screenPosition.X - 32), (float) (y * 16 - (int) Game1.screenPosition.Y - 64)), new Rectangle?(new Rectangle(num8 * 82, 0, 80, 80)), Lighting.GetColor(x, y), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
              }
              else if (Game1.tile[x, y].frameX == (short) 44)
              {
                if (Game1.tile[x, y].frameY == (short) 220)
                  num8 = 1;
                else if (Game1.tile[x, y].frameY == (short) 242)
                  num8 = 2;
                this.spriteBatch.Draw(Game1.treeBranchTexture, new Vector2((float) (x * 16 - (int) Game1.screenPosition.X - 24), (float) (y * 16 - (int) Game1.screenPosition.Y - 12)), new Rectangle?(new Rectangle(0, num8 * 42, 40, 40)), Lighting.GetColor(x, y), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
              }
              else if (Game1.tile[x, y].frameX == (short) 66)
              {
                if (Game1.tile[x, y].frameY == (short) 220)
                  num8 = 1;
                else if (Game1.tile[x, y].frameY == (short) 242)
                  num8 = 2;
                this.spriteBatch.Draw(Game1.treeBranchTexture, new Vector2((float) (x * 16 - (int) Game1.screenPosition.X), (float) (y * 16 - (int) Game1.screenPosition.Y - 12)), new Rectangle?(new Rectangle(42, num8 * 42, 40, 40)), Lighting.GetColor(x, y), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
              }
            }
            if (Game1.tile[x, y].type == (byte) 72 && Game1.tile[x, y].frameX >= (short) 36)
            {
              int num9 = 0;
              if (Game1.tile[x, y].frameY == (short) 18)
                num9 = 1;
              else if (Game1.tile[x, y].frameY == (short) 36)
                num9 = 2;
              this.spriteBatch.Draw(Game1.shroomCapTexture, new Vector2((float) (x * 16 - (int) Game1.screenPosition.X - 22), (float) (y * 16 - (int) Game1.screenPosition.Y - 26)), new Rectangle?(new Rectangle(num9 * 62, 0, 60, 42)), Lighting.GetColor(x, y), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
            }
            if ((double) Lighting.Brightness(x, y) > 0.0)
            {
              Color color;
              if (solidOnly && (Game1.tile[x - 1, y].liquid > (byte) 0 || Game1.tile[x + 1, y].liquid > (byte) 0 || Game1.tile[x, y - 1].liquid > (byte) 0 || Game1.tile[x, y + 1].liquid > (byte) 0))
              {
                color = Lighting.GetColor(x, y);
                int num10 = 0;
                bool flag1 = false;
                bool flag2 = false;
                bool flag3 = false;
                bool flag4 = false;
                int index = 0;
                bool flag5 = false;
                if ((int) Game1.tile[x - 1, y].liquid > num10)
                {
                  num10 = (int) Game1.tile[x - 1, y].liquid;
                  flag1 = true;
                }
                else if (Game1.tile[x - 1, y].liquid > (byte) 0)
                  flag1 = true;
                if ((int) Game1.tile[x + 1, y].liquid > num10)
                {
                  num10 = (int) Game1.tile[x + 1, y].liquid;
                  flag2 = true;
                }
                else if (Game1.tile[x + 1, y].liquid > (byte) 0)
                {
                  num10 = (int) Game1.tile[x + 1, y].liquid;
                  flag2 = true;
                }
                if (Game1.tile[x, y - 1].liquid > (byte) 0)
                  flag3 = true;
                if (Game1.tile[x, y + 1].liquid > (byte) 240)
                  flag4 = true;
                if (Game1.tile[x - 1, y].liquid > (byte) 0)
                {
                  if (Game1.tile[x - 1, y].lava)
                    index = 1;
                  else
                    flag5 = true;
                }
                if (Game1.tile[x + 1, y].liquid > (byte) 0)
                {
                  if (Game1.tile[x + 1, y].lava)
                    index = 1;
                  else
                    flag5 = true;
                }
                if (Game1.tile[x, y - 1].liquid > (byte) 0)
                {
                  if (Game1.tile[x, y - 1].lava)
                    index = 1;
                  else
                    flag5 = true;
                }
                if (Game1.tile[x, y + 1].liquid > (byte) 0)
                {
                  if (Game1.tile[x, y + 1].lava)
                    index = 1;
                  else
                    flag5 = true;
                }
                if (!flag5 || index != 1)
                {
                  Vector2 vector2 = new Vector2((float) (x * 16), (float) (y * 16));
                  Rectangle rectangle = new Rectangle(0, 4, 16, 16);
                  if (flag4 && (flag1 || flag2))
                  {
                    flag1 = true;
                    flag2 = true;
                  }
                  if ((!flag3 || !flag1 && !flag2) && (!flag4 || !flag3))
                  {
                    if (flag3)
                      rectangle = new Rectangle(0, 4, 16, 4);
                    else if (flag4 && !flag1 && !flag2)
                    {
                      vector2 = new Vector2((float) (x * 16), (float) (y * 16 + 12));
                      rectangle = new Rectangle(0, 4, 16, 4);
                    }
                    else
                    {
                      float num11 = (float) (256 - num10) / 32f;
                      if (flag1 && flag2)
                      {
                        vector2 = new Vector2((float) (x * 16), (float) (y * 16 + (int) num11 * 2));
                        rectangle = new Rectangle(0, 4, 16, 16 - (int) num11 * 2);
                      }
                      else if (flag1)
                      {
                        vector2 = new Vector2((float) (x * 16), (float) (y * 16 + (int) num11 * 2));
                        rectangle = new Rectangle(0, 4, 4, 16 - (int) num11 * 2);
                      }
                      else
                      {
                        vector2 = new Vector2((float) (x * 16 + 12), (float) (y * 16 + (int) num11 * 2));
                        rectangle = new Rectangle(0, 4, 4, 16 - (int) num11 * 2);
                      }
                    }
                  }
                  float num12 = 0.5f;
                  if (index == 1)
                    num12 *= 1.6f;
                  if ((double) y < Game1.worldSurface || (double) num12 > 1.0)
                    num12 = 1f;
                  color = new Color((int) (byte) ((float) color.R * num12), (int) (byte) ((float) color.G * num12), (int) (byte) ((float) color.B * num12), (int) (byte) ((float) color.A * num12));
                  this.spriteBatch.Draw(Game1.liquidTexture[index], vector2 - Game1.screenPosition, new Rectangle?(rectangle), color, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
                }
              }
              if (Game1.tile[x, y].type == (byte) 51)
              {
                color = Lighting.GetColor(x, y);
                float num13 = 0.5f;
                color = new Color((int) (byte) ((float) color.R * num13), (int) (byte) ((float) color.G * num13), (int) (byte) ((float) color.B * num13), (int) (byte) ((float) color.A * num13));
                this.spriteBatch.Draw(Game1.tileTexture[(int) Game1.tile[x, y].type], new Vector2((float) (x * 16 - (int) Game1.screenPosition.X) - (float) (((double) width - 16.0) / 2.0), (float) (y * 16 - (int) Game1.screenPosition.Y + num7)), new Rectangle?(new Rectangle((int) Game1.tile[x, y].frameX, (int) Game1.tile[x, y].frameY, width, height)), color, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
              }
              else
                this.spriteBatch.Draw(Game1.tileTexture[(int) Game1.tile[x, y].type], new Vector2((float) (x * 16 - (int) Game1.screenPosition.X) - (float) (((double) width - 16.0) / 2.0), (float) (y * 16 - (int) Game1.screenPosition.Y + num7)), new Rectangle?(new Rectangle((int) Game1.tile[x, y].frameX, (int) Game1.tile[x, y].frameY, width, height)), Lighting.GetColor(x, y), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
            }
          }
        }
      }
    }

    protected void DrawWater(bool bg = false)
    {
      int num1 = (int) ((double) Game1.screenPosition.X / 16.0 - 1.0);
      int num2 = (int) (((double) Game1.screenPosition.X + (double) Game1.screenWidth) / 16.0) + 2;
      int num3 = (int) ((double) Game1.screenPosition.Y / 16.0 - 1.0);
      int num4 = (int) (((double) Game1.screenPosition.Y + (double) Game1.screenHeight) / 16.0) + 2;
      if (num1 < 0)
        num1 = 0;
      if (num2 > Game1.maxTilesX)
        num2 = Game1.maxTilesX;
      if (num3 < 0)
        num3 = 0;
      if (num4 > Game1.maxTilesY)
        num4 = Game1.maxTilesY;
      for (int y = num3; y < num4 + 4; ++y)
      {
        for (int x = num1 - 2; x < num2 + 2; ++x)
        {
          if (Game1.tile[x, y].liquid > (byte) 0 && (double) Lighting.Brightness(x, y) > 0.0)
          {
            Color color = Lighting.GetColor(x, y);
            float num5 = (float) (256 - (int) Game1.tile[x, y].liquid) / 32f;
            int index1 = 0;
            if (Game1.tile[x, y].lava)
              index1 = 1;
            float num6 = 0.5f;
            if (bg)
              num6 = 1f;
            Vector2 vector2 = new Vector2((float) (x * 16), (float) (y * 16 + (int) num5 * 2));
            Rectangle rectangle = new Rectangle(0, 0, 16, 16 - (int) num5 * 2);
            if (Game1.tile[x, y + 1].liquid < (byte) 245 && (!Game1.tile[x, y + 1].active || !Game1.tileSolid[(int) Game1.tile[x, y + 1].type] || Game1.tileSolidTop[(int) Game1.tile[x, y + 1].type]))
            {
              float num7 = (float) (256 - (int) Game1.tile[x, y + 1].liquid) / 32f;
              num6 = (float) (0.5 * (8.0 - (double) num5) / 4.0);
              if ((double) num6 > 0.55)
                num6 = 0.55f;
              if ((double) num6 < 0.35)
                num6 = 0.35f;
              float num8 = num5 / 2f;
              if (Game1.tile[x, y + 1].liquid < (byte) 200)
              {
                if (!bg)
                {
                  if (Game1.tile[x, y - 1].liquid > (byte) 0 && Game1.tile[x, y - 1].liquid > (byte) 0)
                  {
                    rectangle = new Rectangle(0, 4, 16, 16);
                    num6 = 0.5f;
                  }
                  else if (Game1.tile[x, y - 1].liquid > (byte) 0)
                  {
                    vector2 = new Vector2((float) (x * 16), (float) (y * 16 + 4));
                    rectangle = new Rectangle(0, 4, 16, 12);
                    num6 = 0.5f;
                  }
                  else if (Game1.tile[x, y + 1].liquid > (byte) 0)
                  {
                    vector2 = new Vector2((float) (x * 16), (float) (y * 16 + (int) num5 * 2 + (int) num7 * 2));
                    rectangle = new Rectangle(0, 4, 16, 16 - (int) num5 * 2);
                  }
                  else
                  {
                    vector2 = new Vector2((float) (x * 16 + (int) num8), (float) (y * 16 + (int) num8 * 2 + (int) num7 * 2));
                    rectangle = new Rectangle(0, 4, 16 - (int) num8 * 2, 16 - (int) num8 * 2);
                  }
                }
                else
                  continue;
              }
              else
              {
                num6 = 0.5f;
                rectangle = new Rectangle(0, 4, 16, 16 - (int) num5 * 2 + (int) num7 * 2);
              }
            }
            else if (Game1.tile[x, y - 1].liquid > (byte) 32)
              rectangle = new Rectangle(0, 4, rectangle.Width, rectangle.Height);
            else if ((double) num5 < 1.0 && Game1.tile[x, y - 1].active && Game1.tileSolid[(int) Game1.tile[x, y - 1].type] && !Game1.tileSolidTop[(int) Game1.tile[x, y - 1].type])
            {
              vector2 = new Vector2((float) (x * 16), (float) (y * 16));
              rectangle = new Rectangle(0, 4, 16, 16);
            }
            else
            {
              bool flag = true;
              for (int index2 = y + 1; index2 < y + 6 && (!Game1.tile[x, index2].active || !Game1.tileSolid[(int) Game1.tile[x, index2].type] || Game1.tileSolidTop[(int) Game1.tile[x, index2].type]); ++index2)
              {
                if (Game1.tile[x, index2].liquid < (byte) 200)
                {
                  flag = false;
                  break;
                }
              }
              if (!flag)
              {
                num6 = 0.5f;
                rectangle = new Rectangle(0, 4, 16, 16);
              }
              else if (Game1.tile[x, y - 1].liquid > (byte) 0)
                rectangle = new Rectangle(0, 2, rectangle.Width, rectangle.Height);
            }
            if (Game1.tile[x, y].lava)
            {
              num6 *= 1.6f;
              if ((double) num6 > 1.0)
                num6 = 1f;
              if (this.IsActive)
              {
                if (Game1.tile[x, y].liquid > (byte) 200 && Game1.rand.Next(700) == 0)
                  Dust.NewDust(new Vector2((float) (x * 16), (float) (y * 16)), 16, 16, 35);
                if (rectangle.Y == 0 && this.IsActive && Game1.rand.Next(300) == 0)
                {
                  int index3 = Dust.NewDust(new Vector2((float) (x * 16), (float) ((double) (y * 16) + (double) num5 * 2.0 - 8.0)), 16, 8, 35, Alpha: 50, Scale: 1.5f);
                  Game1.dust[index3].velocity *= 0.8f;
                  Game1.dust[index3].velocity.X *= 2f;
                  Game1.dust[index3].velocity.Y -= (float) Game1.rand.Next(1, 7) * 0.1f;
                  if (Game1.rand.Next(10) == 0)
                    Game1.dust[index3].velocity.Y *= (float) Game1.rand.Next(2, 5);
                  Game1.dust[index3].noGravity = true;
                }
              }
            }
            color = new Color((int) (byte) ((float) color.R * num6), (int) (byte) ((float) color.G * num6), (int) (byte) ((float) color.B * num6), (int) (byte) ((float) color.A * num6));
            this.spriteBatch.Draw(Game1.liquidTexture[index1], vector2 - Game1.screenPosition, new Rectangle?(rectangle), color, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
          }
        }
      }
    }

    protected void DrawGore()
    {
      for (int index = 0; index < 200; ++index)
      {
        if (Game1.gore[index].active && Game1.gore[index].type > 0)
        {
          Color alpha = Game1.gore[index].GetAlpha(Lighting.GetColor((int) ((double) Game1.gore[index].position.X + (double) Game1.goreTexture[Game1.gore[index].type].Width * 0.5) / 16, (int) (((double) Game1.gore[index].position.Y + (double) Game1.goreTexture[Game1.gore[index].type].Height * 0.5) / 16.0)));
          this.spriteBatch.Draw(Game1.goreTexture[Game1.gore[index].type], new Vector2(Game1.gore[index].position.X - Game1.screenPosition.X + (float) (Game1.goreTexture[Game1.gore[index].type].Width / 2), Game1.gore[index].position.Y - Game1.screenPosition.Y + (float) (Game1.goreTexture[Game1.gore[index].type].Height / 2)), new Rectangle?(new Rectangle(0, 0, Game1.goreTexture[Game1.gore[index].type].Width, Game1.goreTexture[Game1.gore[index].type].Height)), alpha, Game1.gore[index].rotation, new Vector2((float) (Game1.goreTexture[Game1.gore[index].type].Width / 2), (float) (Game1.goreTexture[Game1.gore[index].type].Height / 2)), Game1.gore[index].scale, SpriteEffects.None, 0.0f);
        }
      }
    }

    protected void DrawNPCs(bool behindTiles = false)
    {
      Rectangle rectangle = new Rectangle((int) Game1.screenPosition.X - 300, (int) Game1.screenPosition.Y - 300, Game1.screenWidth + 600, Game1.screenHeight + 600);
      for (int index1 = 999; index1 >= 0; --index1)
      {
        if (Game1.npc[index1].active && Game1.npc[index1].type > 0 && Game1.npc[index1].behindTiles == behindTiles && rectangle.Intersects(new Rectangle((int) Game1.npc[index1].position.X, (int) Game1.npc[index1].position.Y, Game1.npc[index1].width, Game1.npc[index1].height)))
        {
          Vector2 vector2;
          if (Game1.npc[index1].aiStyle == 13)
          {
            vector2 = new Vector2(Game1.npc[index1].position.X + (float) (Game1.npc[index1].width / 2), Game1.npc[index1].position.Y + (float) (Game1.npc[index1].height / 2));
            float x = (float) ((double) Game1.npc[index1].ai[0] * 16.0 + 8.0) - vector2.X;
            float y = (float) ((double) Game1.npc[index1].ai[1] * 16.0 + 8.0) - vector2.Y;
            float rotation = (float) Math.Atan2((double) y, (double) x) - 1.57f;
            bool flag = true;
            while (flag)
            {
              int height = 28;
              float num1 = (float) Math.Sqrt((double) x * (double) x + (double) y * (double) y);
              if ((double) num1 < 40.0)
              {
                height = (int) num1 - 40 + 28;
                flag = false;
              }
              float num2 = 28f / num1;
              float num3 = x * num2;
              float num4 = y * num2;
              vector2.X += num3;
              vector2.Y += num4;
              x = (float) ((double) Game1.npc[index1].ai[0] * 16.0 + 8.0) - vector2.X;
              y = (float) ((double) Game1.npc[index1].ai[1] * 16.0 + 8.0) - vector2.Y;
              Color color = Lighting.GetColor((int) vector2.X / 16, (int) ((double) vector2.Y / 16.0));
              this.spriteBatch.Draw(Game1.chain4Texture, new Vector2(vector2.X - Game1.screenPosition.X, vector2.Y - Game1.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, Game1.chain4Texture.Width, height)), color, rotation, new Vector2((float) Game1.chain4Texture.Width * 0.5f, (float) Game1.chain4Texture.Height * 0.5f), 1f, SpriteEffects.None, 0.0f);
            }
          }
          if (Game1.npc[index1].type == 36)
          {
            vector2 = new Vector2((float) ((double) Game1.npc[index1].position.X + (double) Game1.npc[index1].width * 0.5 - 5.0 * (double) Game1.npc[index1].ai[0]), Game1.npc[index1].position.Y + 20f);
            for (int index2 = 0; index2 < 2; ++index2)
            {
              float num5 = Game1.npc[(int) Game1.npc[index1].ai[1]].position.X + (float) (Game1.npc[(int) Game1.npc[index1].ai[1]].width / 2) - vector2.X;
              float num6 = Game1.npc[(int) Game1.npc[index1].ai[1]].position.Y + (float) (Game1.npc[(int) Game1.npc[index1].ai[1]].height / 2) - vector2.Y;
              float x;
              float y;
              float num7;
              if (index2 == 0)
              {
                x = num5 - 200f * Game1.npc[index1].ai[0];
                y = num6 + 130f;
                num7 = 92f / (float) Math.Sqrt((double) x * (double) x + (double) y * (double) y);
                vector2.X += x * num7;
                vector2.Y += y * num7;
              }
              else
              {
                x = num5 - 50f * Game1.npc[index1].ai[0];
                y = num6 + 80f;
                num7 = 60f / (float) Math.Sqrt((double) x * (double) x + (double) y * (double) y);
                vector2.X += x * num7;
                vector2.Y += y * num7;
              }
              float rotation = (float) Math.Atan2((double) y, (double) x) - 1.57f;
              Color color = Lighting.GetColor((int) vector2.X / 16, (int) ((double) vector2.Y / 16.0));
              this.spriteBatch.Draw(Game1.boneArmTexture, new Vector2(vector2.X - Game1.screenPosition.X, vector2.Y - Game1.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, Game1.boneArmTexture.Width, Game1.boneArmTexture.Height)), color, rotation, new Vector2((float) Game1.boneArmTexture.Width * 0.5f, (float) Game1.boneArmTexture.Height * 0.5f), 1f, SpriteEffects.None, 0.0f);
              if (index2 == 0)
              {
                vector2.X += (float) ((double) x * (double) num7 / 2.0);
                vector2.Y += (float) ((double) y * (double) num7 / 2.0);
              }
              else if (this.IsActive)
              {
                vector2.X += (float) ((double) x * (double) num7 - 16.0);
                vector2.Y += (float) ((double) y * (double) num7 - 6.0);
                int index3 = Dust.NewDust(new Vector2(vector2.X, vector2.Y), 30, 10, 5, x * 0.02f, y * 0.02f, Scale: 2f);
                Game1.dust[index3].noGravity = true;
              }
            }
          }
          float num8 = 0.0f;
          Vector2 origin = new Vector2((float) (Game1.npcTexture[Game1.npc[index1].type].Width / 2), (float) (Game1.npcTexture[Game1.npc[index1].type].Height / Game1.npcFrameCount[Game1.npc[index1].type] / 2));
          if (Game1.npc[index1].type == 4)
            origin = new Vector2(55f, 107f);
          if (Game1.npc[index1].type == 6)
            num8 = 26f;
          if (Game1.npc[index1].type == 7 || Game1.npc[index1].type == 8 || Game1.npc[index1].type == 9)
            num8 = 13f;
          if (Game1.npc[index1].type == 10 || Game1.npc[index1].type == 11 || Game1.npc[index1].type == 12)
            num8 = 8f;
          if (Game1.npc[index1].type == 13 || Game1.npc[index1].type == 14 || Game1.npc[index1].type == 15)
            num8 = 26f;
          float num9 = num8 * Game1.npc[index1].scale;
          Color newColor = Lighting.GetColor((int) ((double) Game1.npc[index1].position.X + (double) Game1.npc[index1].width * 0.5) / 16, (int) (((double) Game1.npc[index1].position.Y + (double) Game1.npc[index1].height * 0.5) / 16.0));
          if (Game1.npc[index1].aiStyle == 10)
            newColor = Color.White;
          SpriteEffects effects = SpriteEffects.None;
          if (Game1.npc[index1].spriteDirection == 1)
            effects = SpriteEffects.FlipHorizontally;
          this.spriteBatch.Draw(Game1.npcTexture[Game1.npc[index1].type], new Vector2((float) ((double) Game1.npc[index1].position.X - (double) Game1.screenPosition.X + (double) (Game1.npc[index1].width / 2) - (double) Game1.npcTexture[Game1.npc[index1].type].Width * (double) Game1.npc[index1].scale / 2.0 + (double) origin.X * (double) Game1.npc[index1].scale), (float) ((double) Game1.npc[index1].position.Y - (double) Game1.screenPosition.Y + (double) Game1.npc[index1].height - (double) Game1.npcTexture[Game1.npc[index1].type].Height * (double) Game1.npc[index1].scale / (double) Game1.npcFrameCount[Game1.npc[index1].type] + 4.0 + (double) origin.Y * (double) Game1.npc[index1].scale) + num9), new Rectangle?(Game1.npc[index1].frame), Game1.npc[index1].GetAlpha(newColor), Game1.npc[index1].rotation, origin, Game1.npc[index1].scale, effects, 0.0f);
          if (Game1.npc[index1].color != new Color())
            this.spriteBatch.Draw(Game1.npcTexture[Game1.npc[index1].type], new Vector2((float) ((double) Game1.npc[index1].position.X - (double) Game1.screenPosition.X + (double) (Game1.npc[index1].width / 2) - (double) Game1.npcTexture[Game1.npc[index1].type].Width * (double) Game1.npc[index1].scale / 2.0 + (double) origin.X * (double) Game1.npc[index1].scale), (float) ((double) Game1.npc[index1].position.Y - (double) Game1.screenPosition.Y + (double) Game1.npc[index1].height - (double) Game1.npcTexture[Game1.npc[index1].type].Height * (double) Game1.npc[index1].scale / (double) Game1.npcFrameCount[Game1.npc[index1].type] + 4.0 + (double) origin.Y * (double) Game1.npc[index1].scale) + num9), new Rectangle?(Game1.npc[index1].frame), Game1.npc[index1].GetColor(newColor), Game1.npc[index1].rotation, origin, Game1.npc[index1].scale, effects, 0.0f);
        }
      }
    }

    protected void DrawPlayer(Player drawPlayer)
    {
      Color immuneAlpha1 = drawPlayer.GetImmuneAlpha(Lighting.GetColor((int) ((double) drawPlayer.position.X + (double) drawPlayer.width * 0.5) / 16, (int) (((double) drawPlayer.position.Y + (double) drawPlayer.height * 0.25) / 16.0), Color.White));
      Color immuneAlpha2 = drawPlayer.GetImmuneAlpha(Lighting.GetColor((int) ((double) drawPlayer.position.X + (double) drawPlayer.width * 0.5) / 16, (int) (((double) drawPlayer.position.Y + (double) drawPlayer.height * 0.25) / 16.0), drawPlayer.eyeColor));
      Color immuneAlpha3 = drawPlayer.GetImmuneAlpha(Lighting.GetColor((int) ((double) drawPlayer.position.X + (double) drawPlayer.width * 0.5) / 16, (int) (((double) drawPlayer.position.Y + (double) drawPlayer.height * 0.25) / 16.0), drawPlayer.hairColor));
      Color immuneAlpha4 = drawPlayer.GetImmuneAlpha(Lighting.GetColor((int) ((double) drawPlayer.position.X + (double) drawPlayer.width * 0.5) / 16, (int) (((double) drawPlayer.position.Y + (double) drawPlayer.height * 0.25) / 16.0), drawPlayer.skinColor));
      Color immuneAlpha5 = drawPlayer.GetImmuneAlpha(Lighting.GetColor((int) ((double) drawPlayer.position.X + (double) drawPlayer.width * 0.5) / 16, (int) (((double) drawPlayer.position.Y + (double) drawPlayer.height * 0.5) / 16.0), drawPlayer.skinColor));
      Color immuneAlpha6 = drawPlayer.GetImmuneAlpha(Lighting.GetColor((int) ((double) drawPlayer.position.X + (double) drawPlayer.width * 0.5) / 16, (int) (((double) drawPlayer.position.Y + (double) drawPlayer.height * 0.5) / 16.0), drawPlayer.shirtColor));
      Color immuneAlpha7 = drawPlayer.GetImmuneAlpha(Lighting.GetColor((int) ((double) drawPlayer.position.X + (double) drawPlayer.width * 0.5) / 16, (int) (((double) drawPlayer.position.Y + (double) drawPlayer.height * 0.5) / 16.0), drawPlayer.underShirtColor));
      Color immuneAlpha8 = drawPlayer.GetImmuneAlpha(Lighting.GetColor((int) ((double) drawPlayer.position.X + (double) drawPlayer.width * 0.5) / 16, (int) (((double) drawPlayer.position.Y + (double) drawPlayer.height * 0.75) / 16.0), drawPlayer.pantsColor));
      Color immuneAlpha9 = drawPlayer.GetImmuneAlpha(Lighting.GetColor((int) ((double) drawPlayer.position.X + (double) drawPlayer.width * 0.5) / 16, (int) (((double) drawPlayer.position.Y + (double) drawPlayer.height * 0.75) / 16.0), drawPlayer.shoeColor));
      SpriteEffects effects = drawPlayer.direction != -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
      if (drawPlayer.legs >= 0)
      {
        this.spriteBatch.Draw(Game1.armorLegTexture, new Vector2((float) (int) ((double) drawPlayer.position.X - (double) Game1.screenPosition.X - (double) (drawPlayer.bodyFrame.Width / 2) + (double) (drawPlayer.width / 2)), (float) (int) ((double) drawPlayer.position.Y - (double) Game1.screenPosition.Y + (double) drawPlayer.height - (double) drawPlayer.bodyFrame.Height + 2.0)) + drawPlayer.bodyPosition + new Vector2(16f, 28f), new Rectangle?(new Rectangle(34 * drawPlayer.legs, drawPlayer.legFrame.Y, 32, 48)), drawPlayer.GetImmuneAlpha(Lighting.GetColor((int) ((double) drawPlayer.position.X + (double) drawPlayer.width * 0.5) / 16, (int) (((double) drawPlayer.position.Y + (double) drawPlayer.height * 0.75) / 16.0), Color.White)), drawPlayer.bodyRotation, new Vector2(16f, 28f), 1f, effects, 0.0f);
      }
      else
      {
        this.spriteBatch.Draw(Game1.playerPantsTexture, new Vector2((float) (int) ((double) drawPlayer.position.X - (double) Game1.screenPosition.X - (double) (drawPlayer.legFrame.Width / 2) + (double) (drawPlayer.width / 2)), (float) (int) ((double) drawPlayer.position.Y - (double) Game1.screenPosition.Y + (double) drawPlayer.height - (double) drawPlayer.legFrame.Height + 2.0)) + drawPlayer.legPosition + new Vector2(16f, 40f), new Rectangle?(drawPlayer.legFrame), immuneAlpha8, drawPlayer.legRotation, new Vector2(16f, 40f), 1f, effects, 0.0f);
        this.spriteBatch.Draw(Game1.playerShoesTexture, new Vector2((float) (int) ((double) drawPlayer.position.X - (double) Game1.screenPosition.X - (double) (drawPlayer.legFrame.Width / 2) + (double) (drawPlayer.width / 2)), (float) (int) ((double) drawPlayer.position.Y - (double) Game1.screenPosition.Y + (double) drawPlayer.height - (double) drawPlayer.legFrame.Height + 2.0)) + drawPlayer.legPosition + new Vector2(16f, 40f), new Rectangle?(drawPlayer.legFrame), immuneAlpha9, drawPlayer.legRotation, new Vector2(16f, 40f), 1f, effects, 0.0f);
      }
      if (drawPlayer.body < 0 || drawPlayer.body == 6)
      {
        this.spriteBatch.Draw(Game1.playerUnderShirtTexture, new Vector2((float) (int) ((double) drawPlayer.position.X - (double) Game1.screenPosition.X - (double) (drawPlayer.bodyFrame.Width / 2) + (double) (drawPlayer.width / 2)), (float) (int) ((double) drawPlayer.position.Y - (double) Game1.screenPosition.Y + (double) drawPlayer.height - (double) drawPlayer.bodyFrame.Height + 2.0)) + drawPlayer.bodyPosition + new Vector2(16f, 28f), new Rectangle?(drawPlayer.bodyFrame), immuneAlpha7, drawPlayer.bodyRotation, new Vector2(16f, 28f), 1f, effects, 0.0f);
        this.spriteBatch.Draw(Game1.playerShirtTexture, new Vector2((float) (int) ((double) drawPlayer.position.X - (double) Game1.screenPosition.X - (double) (drawPlayer.bodyFrame.Width / 2) + (double) (drawPlayer.width / 2)), (float) (int) ((double) drawPlayer.position.Y - (double) Game1.screenPosition.Y + (double) drawPlayer.height - (double) drawPlayer.bodyFrame.Height + 2.0)) + drawPlayer.bodyPosition + new Vector2(16f, 28f), new Rectangle?(drawPlayer.bodyFrame), immuneAlpha6, drawPlayer.bodyRotation, new Vector2(16f, 28f), 1f, effects, 0.0f);
        this.spriteBatch.Draw(Game1.playerBeltTexture, new Vector2((float) (int) ((double) drawPlayer.position.X - (double) Game1.screenPosition.X - (double) (drawPlayer.bodyFrame.Width / 2) + (double) (drawPlayer.width / 2)), (float) (int) ((double) drawPlayer.position.Y - (double) Game1.screenPosition.Y + (double) drawPlayer.height - (double) drawPlayer.bodyFrame.Height + 2.0)) + drawPlayer.bodyPosition + new Vector2(16f, 28f), new Rectangle?(drawPlayer.bodyFrame), immuneAlpha9, drawPlayer.bodyRotation, new Vector2(16f, 28f), 1f, effects, 0.0f);
        if (drawPlayer.body != 6)
          this.spriteBatch.Draw(Game1.playerHandsTexture, new Vector2((float) (int) ((double) drawPlayer.position.X - (double) Game1.screenPosition.X - (double) (drawPlayer.bodyFrame.Width / 2) + (double) (drawPlayer.width / 2)), (float) (int) ((double) drawPlayer.position.Y - (double) Game1.screenPosition.Y + (double) drawPlayer.height - (double) drawPlayer.bodyFrame.Height + 2.0)) + drawPlayer.bodyPosition + new Vector2(16f, 28f), new Rectangle?(drawPlayer.bodyFrame), immuneAlpha5, drawPlayer.bodyRotation, new Vector2(16f, 28f), 1f, effects, 0.0f);
      }
      if (drawPlayer.body >= 0)
        this.spriteBatch.Draw(Game1.armorBodyTexture, new Vector2((float) (int) ((double) drawPlayer.position.X - (double) Game1.screenPosition.X - (double) (drawPlayer.bodyFrame.Width / 2) + (double) (drawPlayer.width / 2)), (float) (int) ((double) drawPlayer.position.Y - (double) Game1.screenPosition.Y + (double) drawPlayer.height - (double) drawPlayer.bodyFrame.Height + 2.0)) + drawPlayer.bodyPosition + new Vector2(16f, 28f), new Rectangle?(new Rectangle(34 * drawPlayer.body, drawPlayer.bodyFrame.Y, 32, 48)), drawPlayer.GetImmuneAlpha(Lighting.GetColor((int) ((double) drawPlayer.position.X + (double) drawPlayer.width * 0.5) / 16, (int) (((double) drawPlayer.position.Y + (double) drawPlayer.height * 0.5) / 16.0), Color.White)), drawPlayer.bodyRotation, new Vector2(16f, 28f), 1f, effects, 0.0f);
      if (drawPlayer.head != 6)
      {
        if (drawPlayer.head != 8)
          this.spriteBatch.Draw(Game1.playerHeadTexture, new Vector2((float) (int) ((double) drawPlayer.position.X - (double) Game1.screenPosition.X - (double) (drawPlayer.headFrame.Width / 2) + (double) (drawPlayer.width / 2)), (float) (int) ((double) drawPlayer.position.Y - (double) Game1.screenPosition.Y + (double) drawPlayer.height - (double) drawPlayer.headFrame.Height + 2.0)) + drawPlayer.headPosition + new Vector2(16f, 14f), new Rectangle?(new Rectangle(0, 0, 32, 48)), immuneAlpha4, drawPlayer.headRotation, new Vector2(16f, 14f), 1f, effects, 0.0f);
        this.spriteBatch.Draw(Game1.playerEyeWhitesTexture, new Vector2((float) (int) ((double) drawPlayer.position.X - (double) Game1.screenPosition.X - (double) (drawPlayer.headFrame.Width / 2) + (double) (drawPlayer.width / 2)), (float) (int) ((double) drawPlayer.position.Y - (double) Game1.screenPosition.Y + (double) drawPlayer.height - (double) drawPlayer.headFrame.Height + 2.0)) + drawPlayer.headPosition + new Vector2(16f, 14f), new Rectangle?(new Rectangle(0, 0, 32, 48)), immuneAlpha1, drawPlayer.headRotation, new Vector2(16f, 14f), 1f, effects, 0.0f);
        this.spriteBatch.Draw(Game1.playerEyesTexture, new Vector2((float) (int) ((double) drawPlayer.position.X - (double) Game1.screenPosition.X - (double) (drawPlayer.headFrame.Width / 2) + (double) (drawPlayer.width / 2)), (float) (int) ((double) drawPlayer.position.Y - (double) Game1.screenPosition.Y + (double) drawPlayer.height - (double) drawPlayer.headFrame.Height + 2.0)) + drawPlayer.headPosition + new Vector2(16f, 14f), new Rectangle?(new Rectangle(0, 0, 32, 48)), immuneAlpha2, drawPlayer.headRotation, new Vector2(16f, 14f), 1f, effects, 0.0f);
      }
      if (drawPlayer.head == 0)
        this.spriteBatch.Draw(Game1.armorHeadTexture, new Vector2((float) (int) ((double) drawPlayer.position.X - (double) Game1.screenPosition.X - (double) (drawPlayer.headFrame.Width / 2) + (double) (drawPlayer.width / 2)), (float) (int) ((double) drawPlayer.position.Y - (double) Game1.screenPosition.Y + (double) drawPlayer.height - (double) drawPlayer.headFrame.Height + 2.0)) + drawPlayer.headPosition + new Vector2(16f, 14f), new Rectangle?(new Rectangle(34 * drawPlayer.head, 0, 32, 48)), drawPlayer.GetImmuneAlpha(Lighting.GetColor((int) ((double) drawPlayer.position.X + (double) drawPlayer.width * 0.5) / 16, (int) (((double) drawPlayer.position.Y + (double) drawPlayer.height * 0.25) / 16.0), Color.White)), drawPlayer.headRotation, new Vector2(16f, 14f), 1f, effects, 0.0f);
      if (drawPlayer.head > 0)
        this.spriteBatch.Draw(Game1.armorHeadTexture, new Vector2((float) (int) ((double) drawPlayer.position.X - (double) Game1.screenPosition.X - (double) (drawPlayer.headFrame.Width / 2) + (double) (drawPlayer.width / 2)), (float) (int) ((double) drawPlayer.position.Y - (double) Game1.screenPosition.Y + (double) drawPlayer.height - (double) drawPlayer.headFrame.Height + 2.0)) + drawPlayer.headPosition + new Vector2(16f, 14f), new Rectangle?(new Rectangle(34 * drawPlayer.head, 0, 32, 48)), drawPlayer.GetImmuneAlpha(Lighting.GetColor((int) ((double) drawPlayer.position.X + (double) drawPlayer.width * 0.5) / 16, (int) (((double) drawPlayer.position.Y + (double) drawPlayer.height * 0.25) / 16.0), Color.White)), drawPlayer.headRotation, new Vector2(16f, 14f), 1f, effects, 0.0f);
      else
        this.spriteBatch.Draw(Game1.playerHairTexture, new Vector2((float) (int) ((double) drawPlayer.position.X - (double) Game1.screenPosition.X - (double) (drawPlayer.headFrame.Width / 2) + (double) (drawPlayer.width / 2)), (float) (int) ((double) drawPlayer.position.Y - (double) Game1.screenPosition.Y + (double) drawPlayer.height - (double) drawPlayer.headFrame.Height + 2.0)) + drawPlayer.headPosition + new Vector2(16f, 14f), new Rectangle?(drawPlayer.hairFrame), immuneAlpha3, drawPlayer.headRotation, new Vector2(16f, 14f), 1f, effects, 0.0f);
      Color color = Lighting.GetColor((int) ((double) drawPlayer.position.X + (double) drawPlayer.width * 0.5) / 16, (int) (((double) drawPlayer.position.Y + (double) drawPlayer.height * 0.5) / 16.0));
      if ((drawPlayer.itemAnimation > 0 || drawPlayer.inventory[drawPlayer.selectedItem].holdStyle > 0) && drawPlayer.inventory[drawPlayer.selectedItem].type > 0 && !drawPlayer.dead && !drawPlayer.inventory[drawPlayer.selectedItem].noUseGraphic)
      {
        if (drawPlayer.inventory[drawPlayer.selectedItem].useStyle == 5)
        {
          int num = 10;
          Vector2 vector2 = new Vector2((float) (Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type].Width / 2), (float) (Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type].Height / 2));
          if (drawPlayer.inventory[drawPlayer.selectedItem].type == 95)
          {
            num = 10;
            vector2.Y += 2f;
          }
          else if (drawPlayer.inventory[drawPlayer.selectedItem].type == 96)
            num = -5;
          else if (drawPlayer.inventory[drawPlayer.selectedItem].type == 98)
          {
            num = -5;
            vector2.Y -= 2f;
          }
          else if (drawPlayer.inventory[drawPlayer.selectedItem].type == 126)
          {
            num = 4;
            vector2.Y += 4f;
          }
          else if (drawPlayer.inventory[drawPlayer.selectedItem].type == (int) sbyte.MaxValue)
          {
            num = 4;
            vector2.Y += 2f;
          }
          else if (drawPlayer.inventory[drawPlayer.selectedItem].type == 157)
          {
            num = 6;
            vector2.Y += 2f;
          }
          else if (drawPlayer.inventory[drawPlayer.selectedItem].type == 160)
            num = -8;
          else if (drawPlayer.inventory[drawPlayer.selectedItem].type == 164)
          {
            num = 2;
            vector2.Y += 4f;
          }
          else if (drawPlayer.inventory[drawPlayer.selectedItem].type == 165)
          {
            num = 12;
            vector2.Y += 6f;
          }
          Vector2 origin = new Vector2((float) -num, (float) (Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type].Height / 2));
          if (drawPlayer.direction == -1)
            origin = new Vector2((float) (Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type].Width + num), (float) (Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type].Height / 2));
          this.spriteBatch.Draw(Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type], new Vector2((float) (int) ((double) drawPlayer.itemLocation.X - (double) Game1.screenPosition.X + (double) vector2.X), (float) (int) ((double) drawPlayer.itemLocation.Y - (double) Game1.screenPosition.Y + (double) vector2.Y)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type].Width, Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type].Height)), drawPlayer.inventory[drawPlayer.selectedItem].GetAlpha(color), drawPlayer.itemRotation, origin, drawPlayer.inventory[drawPlayer.selectedItem].scale, effects, 0.0f);
          if (drawPlayer.inventory[drawPlayer.selectedItem].color != new Color())
            this.spriteBatch.Draw(Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type], new Vector2((float) (int) ((double) drawPlayer.itemLocation.X - (double) Game1.screenPosition.X + (double) vector2.X), (float) (int) ((double) drawPlayer.itemLocation.Y - (double) Game1.screenPosition.Y + (double) vector2.Y)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type].Width, Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type].Height)), drawPlayer.inventory[drawPlayer.selectedItem].GetColor(color), drawPlayer.itemRotation, origin, drawPlayer.inventory[drawPlayer.selectedItem].scale, effects, 0.0f);
        }
        else
        {
          this.spriteBatch.Draw(Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type], new Vector2((float) (int) ((double) drawPlayer.itemLocation.X - (double) Game1.screenPosition.X), (float) (int) ((double) drawPlayer.itemLocation.Y - (double) Game1.screenPosition.Y)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type].Width, Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type].Height)), drawPlayer.inventory[drawPlayer.selectedItem].GetAlpha(color), drawPlayer.itemRotation, new Vector2((float) ((double) Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type].Width * 0.5 - (double) Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type].Width * 0.5 * (double) drawPlayer.direction), (float) Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type].Height), drawPlayer.inventory[drawPlayer.selectedItem].scale, effects, 0.0f);
          if (drawPlayer.inventory[drawPlayer.selectedItem].color != new Color())
            this.spriteBatch.Draw(Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type], new Vector2((float) (int) ((double) drawPlayer.itemLocation.X - (double) Game1.screenPosition.X), (float) (int) ((double) drawPlayer.itemLocation.Y - (double) Game1.screenPosition.Y)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type].Width, Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type].Height)), drawPlayer.inventory[drawPlayer.selectedItem].GetColor(color), drawPlayer.itemRotation, new Vector2((float) ((double) Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type].Width * 0.5 - (double) Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type].Width * 0.5 * (double) drawPlayer.direction), (float) Game1.itemTexture[drawPlayer.inventory[drawPlayer.selectedItem].type].Height), drawPlayer.inventory[drawPlayer.selectedItem].scale, effects, 0.0f);
        }
      }
      if (drawPlayer.body < 0 || drawPlayer.body == 6)
      {
        this.spriteBatch.Draw(Game1.playerUnderShirt2Texture, new Vector2((float) (int) ((double) drawPlayer.position.X - (double) Game1.screenPosition.X - (double) (drawPlayer.bodyFrame.Width / 2) + (double) (drawPlayer.width / 2)), (float) (int) ((double) drawPlayer.position.Y - (double) Game1.screenPosition.Y + (double) drawPlayer.height - (double) drawPlayer.bodyFrame.Height + 2.0)) + drawPlayer.bodyPosition + new Vector2(16f, 28f), new Rectangle?(drawPlayer.bodyFrame), immuneAlpha7, drawPlayer.bodyRotation, new Vector2(16f, 28f), 1f, effects, 0.0f);
        this.spriteBatch.Draw(Game1.playerHands2Texture, new Vector2((float) (int) ((double) drawPlayer.position.X - (double) Game1.screenPosition.X - (double) (drawPlayer.bodyFrame.Width / 2) + (double) (drawPlayer.width / 2)), (float) (int) ((double) drawPlayer.position.Y - (double) Game1.screenPosition.Y + (double) drawPlayer.height - (double) drawPlayer.bodyFrame.Height + 2.0)) + drawPlayer.bodyPosition + new Vector2(16f, 28f), new Rectangle?(drawPlayer.bodyFrame), immuneAlpha5, drawPlayer.bodyRotation, new Vector2(16f, 28f), 1f, effects, 0.0f);
      }
      if (drawPlayer.body < 0)
        return;
      this.spriteBatch.Draw(Game1.armorBody2Texture, new Vector2((float) (int) ((double) drawPlayer.position.X - (double) Game1.screenPosition.X - (double) (drawPlayer.bodyFrame.Width / 2) + (double) (drawPlayer.width / 2)), (float) (int) ((double) drawPlayer.position.Y - (double) Game1.screenPosition.Y + (double) drawPlayer.height - (double) drawPlayer.bodyFrame.Height + 2.0)) + drawPlayer.bodyPosition + new Vector2(16f, 28f), new Rectangle?(new Rectangle(34 * drawPlayer.body, drawPlayer.bodyFrame.Y, 32, 48)), drawPlayer.GetImmuneAlpha(Lighting.GetColor((int) ((double) drawPlayer.position.X + (double) drawPlayer.width * 0.5) / 16, (int) (((double) drawPlayer.position.Y + (double) drawPlayer.height * 0.5) / 16.0), Color.White)), drawPlayer.bodyRotation, new Vector2(16f, 28f), 1f, effects, 0.0f);
    }

    private static void HelpText()
    {
      bool flag1 = false;
      if (Game1.player[Game1.myPlayer].statLifeMax > 100)
        flag1 = true;
      bool flag2 = false;
      if (Game1.player[Game1.myPlayer].statManaMax > 0)
        flag2 = true;
      bool flag3 = true;
      bool flag4 = false;
      bool flag5 = false;
      bool flag6 = false;
      bool flag7 = false;
      bool flag8 = false;
      bool flag9 = false;
      bool flag10 = false;
      for (int index = 0; index < 44; ++index)
      {
        if (Game1.player[Game1.myPlayer].inventory[index].pick > 0 && Game1.player[Game1.myPlayer].inventory[index].name != "Copper Pickaxe")
          flag3 = false;
        if (Game1.player[Game1.myPlayer].inventory[index].axe > 0 && Game1.player[Game1.myPlayer].inventory[index].name != "Copper Axe")
          flag3 = false;
        if (Game1.player[Game1.myPlayer].inventory[index].hammer > 0)
          flag3 = false;
        if (Game1.player[Game1.myPlayer].inventory[index].type == 11 || Game1.player[Game1.myPlayer].inventory[index].type == 12 || Game1.player[Game1.myPlayer].inventory[index].type == 13 || Game1.player[Game1.myPlayer].inventory[index].type == 14)
          flag4 = true;
        if (Game1.player[Game1.myPlayer].inventory[index].type == 19 || Game1.player[Game1.myPlayer].inventory[index].type == 20 || Game1.player[Game1.myPlayer].inventory[index].type == 21 || Game1.player[Game1.myPlayer].inventory[index].type == 22)
          flag5 = true;
        if (Game1.player[Game1.myPlayer].inventory[index].type == 75)
          flag6 = true;
        if (Game1.player[Game1.myPlayer].inventory[index].type == 75)
          flag8 = true;
        if (Game1.player[Game1.myPlayer].inventory[index].type == 68 || Game1.player[Game1.myPlayer].inventory[index].type == 70)
          flag9 = true;
        if (Game1.player[Game1.myPlayer].inventory[index].type == 84)
          flag10 = true;
        if (Game1.player[Game1.myPlayer].inventory[index].type == 117)
          flag7 = true;
      }
      bool flag11 = false;
      bool flag12 = false;
      bool flag13 = false;
      bool flag14 = false;
      for (int index = 0; index < 1000; ++index)
      {
        if (Game1.npc[index].active)
        {
          if (Game1.npc[index].type == 17)
            flag11 = true;
          if (Game1.npc[index].type == 18)
            flag12 = true;
          if (Game1.npc[index].type == 19)
            flag14 = true;
          if (Game1.npc[index].type == 20)
            flag13 = true;
        }
      }
      while (true)
      {
        ++Game1.helpText;
        if (flag3)
        {
          switch (Game1.helpText)
          {
            case 1:
              goto label_41;
            case 2:
              goto label_42;
            case 3:
              goto label_43;
            case 4:
              goto label_44;
            case 5:
              goto label_45;
          }
        }
        if (!flag3 || flag4 || flag5 || Game1.helpText != 11)
        {
          if (flag3 && flag4 && !flag5)
          {
            if (Game1.helpText != 21)
            {
              if (Game1.helpText == 22)
                goto label_52;
            }
            else
              goto label_50;
          }
          if (flag3 && flag5)
          {
            if (Game1.helpText != 31)
            {
              if (Game1.helpText == 32)
                goto label_57;
            }
            else
              goto label_55;
          }
          if (flag1 || Game1.helpText != 41)
          {
            if (flag2 || Game1.helpText != 42)
            {
              if (flag2 || flag6 || Game1.helpText != 43)
              {
                if (!flag11 && !flag12)
                {
                  switch (Game1.helpText)
                  {
                    case 51:
                      goto label_66;
                    case 52:
                      goto label_67;
                    case 53:
                      goto label_68;
                  }
                }
                if (flag11 || Game1.helpText != 61)
                {
                  if (flag12 || Game1.helpText != 62)
                  {
                    if (flag14 || Game1.helpText != 63)
                    {
                      if (flag13 || Game1.helpText != 64)
                      {
                        if (!flag8 || Game1.helpText != 71)
                        {
                          if (!flag9 || Game1.helpText != 72)
                          {
                            if (!flag8 && !flag9 || Game1.helpText != 80)
                            {
                              if (flag10 || Game1.helpText != 201)
                              {
                                if (!flag7 || Game1.helpText != 202)
                                {
                                  switch (Game1.helpText)
                                  {
                                    case 1000:
                                      goto label_88;
                                    case 1001:
                                      goto label_89;
                                    case 1002:
                                      goto label_90;
                                    default:
                                      if (Game1.helpText > 1100)
                                        Game1.helpText = 0;
                                      continue;
                                  }
                                }
                                else
                                  goto label_86;
                              }
                              else
                                goto label_84;
                            }
                            else
                              goto label_82;
                          }
                          else
                            goto label_80;
                        }
                        else
                          goto label_78;
                      }
                      else
                        goto label_76;
                    }
                    else
                      goto label_74;
                  }
                  else
                    goto label_72;
                }
                else
                  goto label_70;
              }
              else
                goto label_63;
            }
            else
              goto label_61;
          }
          else
            goto label_59;
        }
        else
          goto label_47;
      }
label_41:
      Game1.npcChatText = "You can use your pickaxe to dig through dirt, and your axe to chop down trees. Just place your cursor over the tile and click!";
      return;
label_42:
      Game1.npcChatText = "If you want to survive, you will need to create weapons and shelter. Start by chopping down trees and gathering wood.";
      return;
label_43:
      Game1.npcChatText = "Press ESC to access your crafting menu. When you have enough wood, create a workbench. This will allow you to create more complicated things, as long as you are standing close to it.";
      return;
label_44:
      Game1.npcChatText = "You can build a shelter by placing wood or other blocks in the world. Don't forget to create and place walls.";
      return;
label_45:
      Game1.npcChatText = "Once you have a wooden sword, you might try to gather some gel from the slimes. Combine wood and gel to make a torch!";
      return;
label_47:
      Game1.npcChatText = "You should do some mining to find metal ore. You can craft very usefull things with it.";
      return;
label_50:
      Game1.npcChatText = "Now that you have some ore, you will need to turn it into a bar in order to make items with it. This requires a furnace!";
      return;
label_52:
      Game1.npcChatText = "You can create a furnace out of torches, wood, and stone. Make sure you are standing near a work bench.";
      return;
label_55:
      Game1.npcChatText = "You will need an anvil to make most things out of metal bars.";
      return;
label_57:
      Game1.npcChatText = "Anvils can be crafted out of iron, or purchased from a merchant.";
      return;
label_59:
      Game1.npcChatText = "Underground are crystal hearts that can be used to increase your max life. You will need a hammer to obtain them.";
      return;
label_61:
      Game1.npcChatText = "If you gather 10 fallen stars, they can be combined to create an item that will increase your magic capacity.";
      return;
label_63:
      Game1.npcChatText = "Stars fall all over the world at night. They can be used for all sorts of usefull things. If you see one, be sure to grab it because they disappear after sunrise.";
      return;
label_66:
      Game1.npcChatText = "There are many different ways you can attract people to move in to our town. They will of course need a home to live in.";
      return;
label_67:
      Game1.npcChatText = "In order for a room to be considered a home, it needs to have a door, chair, table, and a light source.  Make sure the house has walls as well.";
      return;
label_68:
      Game1.npcChatText = "Two people will not live in the same home. Also, if their home is destroyed, they will look for a new place to live.";
      return;
label_70:
      Game1.npcChatText = "If you want a merchant to move in, you will need to gather plenty of money. 50 silver coins should do the trick!";
      return;
label_72:
      Game1.npcChatText = "For a nurse to move in, you might want to increase your maximum life.";
      return;
label_74:
      Game1.npcChatText = "If you had a gun, I bet an arms dealer might show up to sell you some ammo!";
      return;
label_76:
      Game1.npcChatText = "You should prove yourself by defeating a strong monster. That will get the attention of a dryad.";
      return;
label_78:
      Game1.npcChatText = "If you combine lenses at a demon alter, you might be able to find a way to summon a powerful monster. You will want to wait until night before using it, though.";
      return;
label_80:
      Game1.npcChatText = "You can create worm bait with rotten chunks and vile powder. Make sure you are in a corrupt area before using it.";
      return;
label_82:
      Game1.npcChatText = "Demonic alters are hidden all over the world. You will need to be near them to craft some items.";
      return;
label_84:
      Game1.npcChatText = "You can make a grappling hook from a hook and 3 chains. Skeletons found deep underground usually carry hooks, and chains can be made from iron bars.";
      return;
label_86:
      Game1.npcChatText = "You can craft a space gun using a flintlock pistol, 10 fallen stars, and 30 meteorite bars.";
      return;
label_88:
      Game1.npcChatText = "If you see a pot, be sure to smash it open. They contain all sorts of useful supplies.";
      return;
label_89:
      Game1.npcChatText = "There is treasure hidden all over the world. Some amazing things can be found deep underground!";
      return;
label_90:
      Game1.npcChatText = "Smashing a shadow orb will cause a meteor to fall out of the sky. Shadow orbs can usually be found in the chasms around corrupt areas.";
    }

    protected void DrawChat()
    {
      if (Game1.player[Game1.myPlayer].talkNPC < 0 && Game1.player[Game1.myPlayer].sign == -1)
      {
        Game1.npcChatText = "";
      }
      else
      {
        Color color1 = new Color(200, 200, 200, 200);
        int num1 = ((int) Game1.mouseTextColor * 2 + (int) byte.MaxValue) / 3;
        Color color2 = new Color(num1, num1, num1, num1);
        int length = 10;
        int index1 = 0;
        string[] strArray1 = new string[length];
        int startIndex1 = 0;
        int num2 = 0;
        if (Game1.npcChatText == null)
          Game1.npcChatText = "";
        for (int startIndex2 = 0; startIndex2 < Game1.npcChatText.Length; ++startIndex2)
        {
          if (Encoding.ASCII.GetBytes(Game1.npcChatText.Substring(startIndex2, 1))[0] == (byte) 10)
          {
            strArray1[index1] = Game1.npcChatText.Substring(startIndex1, startIndex2 - startIndex1);
            ++index1;
            startIndex1 = startIndex2 + 1;
            num2 = startIndex2 + 1;
          }
          else if (Game1.npcChatText.Substring(startIndex2, 1) == " " || startIndex2 == Game1.npcChatText.Length - 1)
          {
            if ((double) Game1.fontMouseText.MeasureString(Game1.npcChatText.Substring(startIndex1, startIndex2 - startIndex1)).X > 470.0)
            {
              strArray1[index1] = Game1.npcChatText.Substring(startIndex1, num2 - startIndex1);
              ++index1;
              startIndex1 = num2 + 1;
            }
            num2 = startIndex2;
          }
          if (index1 == 9)
            break;
        }
        if (index1 < 10)
          strArray1[index1] = Game1.npcChatText.Substring(startIndex1, Game1.npcChatText.Length - startIndex1);
        if (Game1.editSign)
        {
          ++this.textBlinkerCount;
          if (this.textBlinkerCount >= 20)
          {
            this.textBlinkerState = this.textBlinkerState != 0 ? 0 : 1;
            this.textBlinkerCount = 0;
          }
          if (this.textBlinkerState == 1)
          {
            string[] strArray2;
            IntPtr index2;
                        //RnD
            //(strArray2 = strArray1)[(int) (index2 = (IntPtr) index1)] = strArray2[index2] + "|";
          }
        }
        int num3 = index1 + 1;
        this.spriteBatch.Draw(Game1.chatBackTexture, new Vector2((float) (Game1.screenWidth / 2 - Game1.chatBackTexture.Width / 2), 100f), new Rectangle?(new Rectangle(0, 0, Game1.chatBackTexture.Width, (num3 + 1) * 30)), color1, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
        this.spriteBatch.Draw(Game1.chatBackTexture, new Vector2((float) (Game1.screenWidth / 2 - Game1.chatBackTexture.Width / 2), (float) (100 + (num3 + 1) * 30)), new Rectangle?(new Rectangle(0, Game1.chatBackTexture.Height - 30, Game1.chatBackTexture.Width, 30)), color1, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
        for (int index3 = 0; index3 < num3; ++index3)
        {
          for (int index4 = 0; index4 < 5; ++index4)
          {
            Color color3 = Color.Black;
            int x = 170;
            int y = 120 + index3 * 30;
            if (index4 == 0)
              x -= 2;
            if (index4 == 1)
              x += 2;
            if (index4 == 2)
              y -= 2;
            if (index4 == 3)
              y += 2;
            if (index4 == 4)
              color3 = color2;
            this.spriteBatch.DrawString(Game1.fontMouseText, strArray1[index3], new Vector2((float) x, (float) y), color3, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
          }
        }
        int mouseTextColor = (int) Game1.mouseTextColor;
        color2 = new Color(mouseTextColor, (int) ((double) mouseTextColor / 1.1), mouseTextColor / 2, mouseTextColor);
        string text = "";
        int price = Game1.player[Game1.myPlayer].statLifeMax - Game1.player[Game1.myPlayer].statLife;
        if (Game1.player[Game1.myPlayer].sign > -1)
          text = !Game1.editSign ? "Edit" : "Save";
        else if (Game1.npc[Game1.player[Game1.myPlayer].talkNPC].type == 17 || Game1.npc[Game1.player[Game1.myPlayer].talkNPC].type == 19 || Game1.npc[Game1.player[Game1.myPlayer].talkNPC].type == 20 || Game1.npc[Game1.player[Game1.myPlayer].talkNPC].type == 38)
          text = "Shop";
        else if (Game1.npc[Game1.player[Game1.myPlayer].talkNPC].type == 22)
          text = "Help";
        else if (Game1.npc[Game1.player[Game1.myPlayer].talkNPC].type == 18)
        {
          string str = "";
          int num4 = 0;
          int num5 = 0;
          int num6 = 0;
          int num7 = 0;
          int num8 = price;
          if (num8 > 0)
          {
            num8 = (int) ((double) num8 * 0.75);
            if (num8 < 1)
              num8 = 1;
          }
          if (num8 < 0)
            num8 = 0;
          if (num8 >= 1000000)
          {
            num4 = num8 / 1000000;
            num8 -= num4 * 1000000;
          }
          if (num8 >= 10000)
          {
            num5 = num8 / 10000;
            num8 -= num5 * 10000;
          }
          if (num8 >= 100)
          {
            num6 = num8 / 100;
            num8 -= num6 * 100;
          }
          if (num8 >= 1)
            num7 = num8;
          if (num4 > 0)
            str = str + (object) num4 + " platinum ";
          if (num5 > 0)
            str = str + (object) num5 + " gold ";
          if (num6 > 0)
            str = str + (object) num6 + " silver ";
          if (num7 > 0)
            str = str + (object) num7 + " copper ";
          float num9 = (float) Game1.mouseTextColor / (float) byte.MaxValue;
          if (num4 > 0)
            color2 = new Color((int) (byte) (220.0 * (double) num9), (int) (byte) (220.0 * (double) num9), (int) (byte) (198.0 * (double) num9), (int) Game1.mouseTextColor);
          else if (num5 > 0)
            color2 = new Color((int) (byte) (224.0 * (double) num9), (int) (byte) (201.0 * (double) num9), (int) (byte) (92.0 * (double) num9), (int) Game1.mouseTextColor);
          else if (num6 > 0)
            color2 = new Color((int) (byte) (181.0 * (double) num9), (int) (byte) (192.0 * (double) num9), (int) (byte) (193.0 * (double) num9), (int) Game1.mouseTextColor);
          else if (num7 > 0)
            color2 = new Color((int) (byte) (246.0 * (double) num9), (int) (byte) (138.0 * (double) num9), (int) (byte) (96.0 * (double) num9), (int) Game1.mouseTextColor);
          text = "Heal (" + str + ")";
          if (num8 == 0)
            text = "Heal";
        }
        int num10 = 180;
        int num11 = 130 + num3 * 30;
        float scale1 = 0.9f;

        if (Game1.mouseState[0].Position.X > num10 && (double) Game1.mouseState[0].Position.X 
                    < (double) num10 + (double) Game1.fontMouseText.MeasureString(text).X 
                    && Game1.mouseState[0].Position.Y > num11 
                    && (double) Game1.mouseState[0].Position.Y < (double) num11 
                    + (double) Game1.fontMouseText.MeasureString(text).Y)
        {
          Game1.player[Game1.myPlayer].mouseInterface = true;
          scale1 = 1.1f;
          if (!Game1.npcChatFocus2)
            Game1.PlaySound(12);
          Game1.npcChatFocus2 = true;
          Game1.player[Game1.myPlayer].releaseUseItem = false;
        }
        else
        {
          if (Game1.npcChatFocus2)
            Game1.PlaySound(12);
          Game1.npcChatFocus2 = false;
        }

        Vector2 origin;
        for (int index5 = 0; index5 < 5; ++index5)
        {
          int num12 = num10;
          int num13 = num11;
          Color color4 = Color.Black;
          if (index5 == 0)
            num12 -= 2;
          if (index5 == 1)
            num12 += 2;
          if (index5 == 2)
            num13 -= 2;
          if (index5 == 3)
            num13 += 2;
          if (index5 == 4)
            color4 = color2;
          origin = Game1.fontMouseText.MeasureString(text);
          origin *= 0.5f;
          this.spriteBatch.DrawString(Game1.fontMouseText, text, new Vector2((float) num12 + origin.X, (float) num13 + origin.Y), color4, 0.0f, origin, scale1, SpriteEffects.None, 0.0f);
        }
        color2 = new Color(mouseTextColor, (int) ((double) mouseTextColor / 1.1), mouseTextColor / 2, mouseTextColor);
        int num14 = num10 + (int) Game1.fontMouseText.MeasureString(text).X + 20;
        int num15 = 130 + num3 * 30;
        float scale2 = 0.9f;
        if (Game1.mouseState[0].Position.X > num14
                    && (double) Game1.mouseState[0].Position.X 
                    < (double) num14 + (double) Game1.fontMouseText.MeasureString("Close").X 
                    && Game1.mouseState[0].Position.Y > num15 
                    && (double) Game1.mouseState[0].Position.Y < (double) num15
                    + (double) Game1.fontMouseText.MeasureString("Close").Y)
        {
          scale2 = 1.1f;
          if (!Game1.npcChatFocus1)
            Game1.PlaySound(12);
          Game1.npcChatFocus1 = true;
          Game1.player[Game1.myPlayer].releaseUseItem = false;
        }
        else
        {
          if (Game1.npcChatFocus1)
            Game1.PlaySound(12);
          Game1.npcChatFocus1 = false;
        }
        for (int index6 = 0; index6 < 5; ++index6)
        {
          int num16 = num14;
          int num17 = num15;
          Color color5 = Color.Black;
          if (index6 == 0)
            num16 -= 2;
          if (index6 == 1)
            num16 += 2;
          if (index6 == 2)
            num17 -= 2;
          if (index6 == 3)
            num17 += 2;
          if (index6 == 4)
            color5 = color2;
          origin = Game1.fontMouseText.MeasureString("Close");
          origin *= 0.5f;
          this.spriteBatch.DrawString(Game1.fontMouseText, 
              "Close", new Vector2((float) num16 + origin.X, (float) num17 + origin.Y),
              color5, 0.0f, origin, scale2, SpriteEffects.None, 0.0f);
        }
        if (/*Game1.mouseState.LeftButton != ButtonState.Pressed */
                    Game1.mouseState.Count == 0 || !Game1.mouseLeftRelease)
          return;
        Game1.mouseLeftRelease = false;
        Game1.player[Game1.myPlayer].releaseUseItem = false;
        Game1.player[Game1.myPlayer].mouseInterface = true;
        if (Game1.npcChatFocus1)
        {
          Game1.player[Game1.myPlayer].talkNPC = -1;
          Game1.player[Game1.myPlayer].sign = -1;
          Game1.editSign = false;
          Game1.npcChatText = "";
          Game1.PlaySound(11);
        }
        else if (Game1.npcChatFocus2)
        {
          if (Game1.player[Game1.myPlayer].sign != -1)
          {
            if (Game1.editSign)
            {
              Game1.PlaySound(12);
              int sign = Game1.player[Game1.myPlayer].sign;
              Sign.TextSign(sign, Game1.npcChatText);
              Game1.editSign = false;
              if (Game1.netMode == 1)
                NetMessage.SendData(47, number: sign);
            }
            else
            {
              Game1.PlaySound(12);
              Game1.editSign = true;
            }
          }
          else if (Game1.npc[Game1.player[Game1.myPlayer].talkNPC].type == 17)
          {
            Game1.playerInventory = true;
            Game1.npcChatText = "";
            Game1.npcShop = 1;
            Game1.PlaySound(12);
          }
          else if (Game1.npc[Game1.player[Game1.myPlayer].talkNPC].type == 19)
          {
            Game1.playerInventory = true;
            Game1.npcChatText = "";
            Game1.npcShop = 2;
            Game1.PlaySound(12);
          }
          else if (Game1.npc[Game1.player[Game1.myPlayer].talkNPC].type == 20)
          {
            Game1.playerInventory = true;
            Game1.npcChatText = "";
            Game1.npcShop = 3;
            Game1.PlaySound(12);
          }
          else if (Game1.npc[Game1.player[Game1.myPlayer].talkNPC].type == 38)
          {
            Game1.playerInventory = true;
            Game1.npcChatText = "";
            Game1.npcShop = 4;
            Game1.PlaySound(12);
          }
          else if (Game1.npc[Game1.player[Game1.myPlayer].talkNPC].type == 22)
          {
            Game1.PlaySound(12);
            Game1.HelpText();
          }
          else if (Game1.npc[Game1.player[Game1.myPlayer].talkNPC].type == 18)
          {
            Game1.PlaySound(12);
            if (price > 0)
            {
              if (Game1.player[Game1.myPlayer].BuyItem(price))
              {
                Game1.PlaySound(2, style: 4);
                Game1.player[Game1.myPlayer].HealEffect(Game1.player[Game1.myPlayer].statLifeMax - Game1.player[Game1.myPlayer].statLife);
                Game1.npcChatText = (double) Game1.player[Game1.myPlayer].statLife >= (double) Game1.player[Game1.myPlayer].statLifeMax * 0.25 ? ((double) Game1.player[Game1.myPlayer].statLife >= (double) Game1.player[Game1.myPlayer].statLifeMax * 0.5 ? ((double) Game1.player[Game1.myPlayer].statLife >= (double) Game1.player[Game1.myPlayer].statLifeMax * 0.75 ? "That didn't hurt too bad, now did it?" : "All better. I don't want to see you jumping off anymore cliffs.") : "That's probably going to leave a scar.") : "I managed to sew your face back on. Be more careful next time.";
                Game1.player[Game1.myPlayer].statLife = Game1.player[Game1.myPlayer].statLifeMax;
              }
              else
              {
                int num18 = Game1.rand.Next(3);
                if (num18 == 0)
                  Game1.npcChatText = "I'm sorry, but you can't afford me.";
                if (num18 == 1)
                  Game1.npcChatText = "I'm gonna need more gold then that.";
                if (num18 == 2)
                  Game1.npcChatText = "I don't work for free you know.";
              }
            }
            else
            {
              int num19 = Game1.rand.Next(3);
              if (num19 == 0)
                Game1.npcChatText = "I don't give happy endings.";
              if (num19 == 1)
                Game1.npcChatText = "I can't do anymore for you without plastic surgery.";
              if (num19 == 2)
                Game1.npcChatText = "Quit wasting my time.";
            }
          }
        }
      }
    }

    protected void DrawInterface()
    {
      if (Game1.hideUI)
        return;
      if (Game1.signBubble)
      {
        int num1 = (int) ((double) Game1.signX - (double) Game1.screenPosition.X);
        int num2 = (int) ((double) Game1.signY - (double) Game1.screenPosition.Y);
        SpriteEffects effects = SpriteEffects.None;
        int x;
        if ((double) Game1.signX > (double) Game1.player[Game1.myPlayer].position.X + (double) Game1.player[Game1.myPlayer].width)
        {
          effects = SpriteEffects.FlipHorizontally;
          x = num1 + (-8 - Game1.chat2Texture.Width);
        }
        else
          x = num1 + 8;
        int y = num2 - 22;
        this.spriteBatch.Draw(Game1.chat2Texture, new Vector2((float) x, (float) y), new Rectangle?(new Rectangle(0, 0, Game1.chat2Texture.Width, Game1.chat2Texture.Height)), new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor), 0.0f, new Vector2(), 1f, effects, 0.0f);
        Game1.signBubble = false;
      }
      Rectangle rectangle1;
      for (int index = 0; index < 8; ++index)
      {
        if (Game1.player[index].active && Game1.myPlayer != index && !Game1.player[index].dead)
        {
          rectangle1 = new Rectangle((int) ((double) Game1.player[index].position.X + (double) Game1.player[index].width * 0.5 - 16.0), (int) ((double) Game1.player[index].position.Y + (double) Game1.player[index].height - 48.0), 32, 48);
          if (Game1.player[Game1.myPlayer].team > 0 && Game1.player[Game1.myPlayer].team == Game1.player[index].team)
          {
            Rectangle rectangle2 = new Rectangle((int) Game1.screenPosition.X, (int) Game1.screenPosition.Y, Game1.screenWidth, Game1.screenHeight);
            string text1 = Game1.player[index].name;
            if (Game1.player[index].statLife < Game1.player[index].statLifeMax)
              text1 = text1 + ": " + (object) Game1.player[index].statLife + "/" + (object) Game1.player[index].statLifeMax;
            Vector2 position1 = Game1.fontMouseText.MeasureString(text1);
            float num3 = 0.0f;
            if (Game1.player[index].chatShowTime > 0)
              num3 = -position1.Y;
            float num4 = 0.0f;
            float num5 = (float) Game1.mouseTextColor / (float) byte.MaxValue;
            Color color = new Color((int) (byte) ((double) Game1.teamColor[Game1.player[index].team].R * (double) num5), (int) (byte) ((double) Game1.teamColor[Game1.player[index].team].G * (double) num5), (int) (byte) ((double) Game1.teamColor[Game1.player[index].team].B * (double) num5), (int) Game1.mouseTextColor);
            Vector2 vector2 = new Vector2((float) (Game1.screenWidth / 2) + Game1.screenPosition.X, (float) (Game1.screenHeight / 2) + Game1.screenPosition.Y);
            float num6 = Game1.player[index].position.X + (float) (Game1.player[index].width / 2) - vector2.X;
            float num7 = (float) ((double) Game1.player[index].position.Y - (double) position1.Y - 2.0) + num3 - vector2.Y;
            float num8 = (float) Math.Sqrt((double) num6 * (double) num6 + (double) num7 * (double) num7);
            if ((double) num8 < 270.0)
            {
              position1.X = (float) ((double) Game1.player[index].position.X + (double) (Game1.player[index].width / 2) - (double) position1.X / 2.0) - Game1.screenPosition.X;
              position1.Y = (float) ((double) Game1.player[index].position.Y - (double) position1.Y - 2.0) + num3 - Game1.screenPosition.Y;
            }
            else
            {
              num4 = num8;
              float num9 = 270f / num8;
              position1.X = (float) ((double) (Game1.screenWidth / 2) + (double) num6 * (double) num9 - (double) position1.X / 2.0);
              position1.Y = (float) (Game1.screenHeight / 2) + num7 * num9;
            }
            if ((double) num4 > 0.0)
            {
              string text2 = "(" + (object) (int) ((double) num4 / 16.0 * 2.0) + " ft)";
              Vector2 position2 = Game1.fontMouseText.MeasureString(text2);
              position2.X = (float) ((double) position1.X + (double) Game1.fontMouseText.MeasureString(text1).X / 2.0 - (double) position2.X / 2.0);
              position2.Y = (float) ((double) position1.Y + (double) Game1.fontMouseText.MeasureString(text1).Y / 2.0 - (double) position2.Y / 2.0 - 20.0);
              this.spriteBatch.DrawString(Game1.fontMouseText, text2, new Vector2(position2.X - 2f, position2.Y), Color.Black, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
              this.spriteBatch.DrawString(Game1.fontMouseText, text2, new Vector2(position2.X + 2f, position2.Y), Color.Black, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
              this.spriteBatch.DrawString(Game1.fontMouseText, text2, new Vector2(position2.X, position2.Y - 2f), Color.Black, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
              this.spriteBatch.DrawString(Game1.fontMouseText, text2, new Vector2(position2.X, position2.Y + 2f), Color.Black, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
              this.spriteBatch.DrawString(Game1.fontMouseText, text2, position2, color, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
            }
            this.spriteBatch.DrawString(Game1.fontMouseText, text1, new Vector2(position1.X - 2f, position1.Y), Color.Black, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
            this.spriteBatch.DrawString(Game1.fontMouseText, text1, new Vector2(position1.X + 2f, position1.Y), Color.Black, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
            this.spriteBatch.DrawString(Game1.fontMouseText, text1, new Vector2(position1.X, position1.Y - 2f), Color.Black, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
            this.spriteBatch.DrawString(Game1.fontMouseText, text1, new Vector2(position1.X, position1.Y + 2f), Color.Black, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
            this.spriteBatch.DrawString(Game1.fontMouseText, text1, position1, color, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
          }
        }
      }
      if (Game1.npcChatText != "" || Game1.player[Game1.myPlayer].sign != -1)
        this.DrawChat();
      Color color1 = new Color(200, 200, 200, 200);
      bool flag1 = false;
      int rare1 = 0;
      int num10 = Game1.player[Game1.myPlayer].statLifeMax / 20;
      if (num10 >= 10)
        num10 = 10;
      this.spriteBatch.DrawString(Game1.fontMouseText, "Life", new Vector2((float) (500 + 13 * num10) - Game1.fontMouseText.MeasureString("Life").X * 0.5f, 6f), new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
      int num11 = 20;
      for (int index = 1; index < Game1.player[Game1.myPlayer].statLifeMax / num11 + 1; ++index)
      {
        float scale = 1f;
        int num12;
        if (Game1.player[Game1.myPlayer].statLife >= index * num11)
        {
          num12 = (int) byte.MaxValue;
        }
        else
        {
          float num13 = (float) (Game1.player[Game1.myPlayer].statLife - (index - 1) * num11) / (float) num11;
          num12 = (int) (30.0 + 225.0 * (double) num13);
          if (num12 < 30)
            num12 = 30;
          scale = (float) ((double) num13 / 4.0 + 0.75);
          if ((double) scale < 0.75)
            scale = 0.75f;
        }
        int num14 = 0;
        int num15 = 0;
        if (index > 10)
        {
          num14 -= 260;
          num15 += 26;
        }
        this.spriteBatch.Draw(Game1.heartTexture, new Vector2((float) (500 + 26 * (index - 1) + num14), (float) (32.0 + ((double) Game1.heartTexture.Height - (double) Game1.heartTexture.Height * (double) scale) / 2.0) + (float) num15), new Rectangle?(new Rectangle(0, 0, Game1.heartTexture.Width, Game1.heartTexture.Height)), new Color(num12, num12, num12, num12), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
      }
      int num16 = 20;
      if (Game1.player[Game1.myPlayer].statManaMax > 0)
      {
        int num17 = Game1.player[Game1.myPlayer].statManaMax / 20;
        this.spriteBatch.DrawString(Game1.fontMouseText, "Mana", new Vector2(750f, 6f), new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
        for (int index = 1; index < Game1.player[Game1.myPlayer].statManaMax / num16 + 1; ++index)
        {
          float scale = 1f;
          int num18;
          if (Game1.player[Game1.myPlayer].statMana >= index * num16)
          {
            num18 = (int) byte.MaxValue;
          }
          else
          {
            float num19 = (float) (Game1.player[Game1.myPlayer].statMana - (index - 1) * num16) / (float) num16;
            num18 = (int) (30.0 + 225.0 * (double) num19);
            if (num18 < 30)
              num18 = 30;
            scale = (float) ((double) num19 / 4.0 + 0.75);
            if ((double) scale < 0.75)
              scale = 0.75f;
          }
          this.spriteBatch.Draw(Game1.manaTexture, new Vector2(775f, (float) (30 + Game1.manaTexture.Height / 2) + (float) (((double) Game1.manaTexture.Height - (double) Game1.manaTexture.Height * (double) scale) / 2.0) + (float) (28 * (index - 1))), new Rectangle?(new Rectangle(0, 0, Game1.manaTexture.Width, Game1.manaTexture.Height)), new Color(num18, num18, num18, num18), 0.0f, new Vector2((float) (Game1.manaTexture.Width / 2), (float) (Game1.manaTexture.Height / 2)), scale, SpriteEffects.None, 0.0f);
        }
      }
      if (Game1.player[Game1.myPlayer].breath < Game1.player[Game1.myPlayer].breathMax)
      {
        int num20 = 76;
        int num21 = Game1.player[Game1.myPlayer].breathMax / 20;
        this.spriteBatch.DrawString(Game1.fontMouseText, "Breath", new Vector2((float) (500 + 13 * num10) - Game1.fontMouseText.MeasureString("Breath").X * 0.5f, (float) (6 + num20)), new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
        int num22 = 20;
        for (int index = 1; index < Game1.player[Game1.myPlayer].breathMax / num22 + 1; ++index)
        {
          float scale = 1f;
          int num23;
          if (Game1.player[Game1.myPlayer].breath >= index * num22)
          {
            num23 = (int) byte.MaxValue;
          }
          else
          {
            float num24 = (float) (Game1.player[Game1.myPlayer].breath - (index - 1) * num22) / (float) num22;
            num23 = (int) (30.0 + 225.0 * (double) num24);
            if (num23 < 30)
              num23 = 30;
            scale = (float) ((double) num24 / 4.0 + 0.75);
            if ((double) scale < 0.75)
              scale = 0.75f;
          }
          int num25 = 0;
          int num26 = 0;
          if (index > 10)
          {
            num25 -= 260;
            num26 += 26;
          }
          this.spriteBatch.Draw(Game1.bubbleTexture, new Vector2((float) (500 + 26 * (index - 1) + num25), (float) (32.0 + ((double) Game1.bubbleTexture.Height - (double) Game1.bubbleTexture.Height * (double) scale) / 2.0) + (float) num26 + (float) num20), new Rectangle?(new Rectangle(0, 0, Game1.bubbleTexture.Width, Game1.bubbleTexture.Height)), new Color(num23, num23, num23, num23), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
        }
      }
      if (Game1.player[Game1.myPlayer].dead)
        Game1.playerInventory = false;
      if (!Game1.playerInventory)
        Game1.player[Game1.myPlayer].chest = -1;
      string cursorText1 = "";
      if (Game1.playerInventory)
      {
        if (Game1.netMode == 1)
        {
          int num27 = 675;
          int y = 134;
          if (Game1.player[Game1.myPlayer].hostile)
          {
            this.spriteBatch.Draw(Game1.itemTexture[4], new Vector2((float) (num27 - 2), (float) y), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[4].Width, Game1.itemTexture[4].Height)), Game1.teamColor[Game1.player[Game1.myPlayer].team], 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
            this.spriteBatch.Draw(Game1.itemTexture[4], new Vector2((float) (num27 + 2), (float) y), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[4].Width, Game1.itemTexture[4].Height)), Game1.teamColor[Game1.player[Game1.myPlayer].team], 0.0f, new Vector2(), 1f, SpriteEffects.FlipHorizontally, 0.0f);
          }
          else
          {
            this.spriteBatch.Draw(Game1.itemTexture[4], new Vector2((float) (num27 - 16), (float) (y + 14)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[4].Width, Game1.itemTexture[4].Height)), Game1.teamColor[Game1.player[Game1.myPlayer].team], -0.785f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
            this.spriteBatch.Draw(Game1.itemTexture[4], new Vector2((float) (num27 + 2), (float) (y + 14)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[4].Width, Game1.itemTexture[4].Height)), Game1.teamColor[Game1.player[Game1.myPlayer].team], -0.785f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
          }
          if (Game1.mouseState[0].Position.X > num27 
                        && Game1.mouseState[0].Position.X < num27 + 34 && 
                        Game1.mouseState[0].Position.Y > y - 2 
                        && Game1.mouseState[0].Position.Y < y + 34)
          {
            Game1.player[Game1.myPlayer].mouseInterface = true;
            if (Game1.mouseState.Count == 1 //Game1.mouseState.LeftButton == ButtonState.Pressed
               && Game1.mouseLeftRelease)
            {
              Game1.PlaySound(12);
              Game1.player[Game1.myPlayer].hostile = !Game1.player[Game1.myPlayer].hostile;
              NetMessage.SendData(30, number: Game1.myPlayer);
            }
          }
          int num28 = num27 - 3;
          Rectangle rectangle3 = new Rectangle(
              (int)Game1.mouseState[0].Position.X, 
              (int)Game1.mouseState[0].Position.Y, 1, 1);
          int width = Game1.teamTexture.Width;
          int height = Game1.teamTexture.Height;
          for (int index = 0; index < 5; ++index)
          {
            Rectangle rectangle4 = new Rectangle();
            if (index == 0)
              rectangle4 = new Rectangle(num28 + 50, y - 20, width, height);
            if (index == 1)
              rectangle4 = new Rectangle(num28 + 40, y, width, height);
            if (index == 2)
              rectangle4 = new Rectangle(num28 + 60, y, width, height);
            if (index == 3)
              rectangle4 = new Rectangle(num28 + 40, y + 20, width, height);
            if (index == 4)
              rectangle4 = new Rectangle(num28 + 60, y + 20, width, height);
            if (rectangle4.Intersects(rectangle3))
            {
              Game1.player[Game1.myPlayer].mouseInterface = true;
              if (  Game1.mouseState.Count == 1
                                //Game1.mouseState.LeftButton == ButtonState.Pressed 
                                && Game1.mouseLeftRelease 
                                && Game1.player[Game1.myPlayer].team != index)
              {
                Game1.PlaySound(12);
                Game1.player[Game1.myPlayer].team = index;
                NetMessage.SendData(45, number: Game1.myPlayer);
              }
            }
          }
          this.spriteBatch.Draw(Game1.teamTexture, new Vector2((float) (num28 + 50),
              (float) (y - 20)), new Rectangle?(new Rectangle(0, 0, Game1.teamTexture.Width, 
              Game1.teamTexture.Height)), Game1.teamColor[0], 0.0f, new Vector2(), 1f, 
              SpriteEffects.None, 0.0f);
          this.spriteBatch.Draw(Game1.teamTexture, new Vector2((float) (num28 + 40), 
              (float) y), new Rectangle?(new Rectangle(0, 0, Game1.teamTexture.Width, 
              Game1.teamTexture.Height)), Game1.teamColor[1], 0.0f, new Vector2(), 1f, 
              SpriteEffects.None, 0.0f);
          this.spriteBatch.Draw(Game1.teamTexture, new Vector2((float) (num28 + 60),
              (float) y), new Rectangle?(new Rectangle(0, 0, Game1.teamTexture.Width, 
              Game1.teamTexture.Height)), Game1.teamColor[2], 0.0f, new Vector2(), 1f, 
              SpriteEffects.None, 0.0f);
          this.spriteBatch.Draw(Game1.teamTexture, new Vector2((float) (num28 + 40),
              (float) (y + 20)), new Rectangle?(new Rectangle(0, 0, Game1.teamTexture.Width, 
              Game1.teamTexture.Height)), Game1.teamColor[3], 0.0f, new Vector2(), 1f,
              SpriteEffects.None, 0.0f);
          this.spriteBatch.Draw(Game1.teamTexture, new Vector2((float) (num28 + 60), 
              (float) (y + 20)), new Rectangle?(new Rectangle(0, 0, Game1.teamTexture.Width,
              Game1.teamTexture.Height)), Game1.teamColor[4], 0.0f, new Vector2(), 1f, 
              SpriteEffects.None, 0.0f);
        }
        string text3 = "Save & Exit";
        if (Game1.netMode != 0)
          text3 = "Disconnect";
        Vector2 vector2_1 = Game1.fontDeathText.MeasureString(text3);
        int num29 = Game1.screenWidth - 110;
        int num30 = Game1.screenHeight - 20;
        if (Game1.mouseExit)
        {
          if ((double) Game1.exitScale < 1.0)
            Game1.exitScale += 0.02f;
        }
        else if ((double) Game1.exitScale > 0.8)
          Game1.exitScale -= 0.02f;
        for (int index = 0; index < 5; ++index)
        {
          int num31 = 0;
          int num32 = 0;
          Color color2 = Color.Black;
          if (index == 0)
            num31 = -2;
          if (index == 1)
            num31 = 2;
          if (index == 2)
            num32 = -2;
          if (index == 3)
            num32 = 2;
          if (index == 4)
            color2 = Color.White;
          this.spriteBatch.DrawString(Game1.fontDeathText, text3, new Vector2((float) (num29 + num31), (float) (num30 + num32)), color2, 0.0f, new Vector2(vector2_1.X / 2f, vector2_1.Y / 2f), Game1.exitScale - 0.2f, SpriteEffects.None, 0.0f);
        }
        if ((double) Game1.mouseState[0].Position.X >
            (double) num29 - (double) vector2_1.X / 2.0 
        && (double) Game1.mouseState[0].Position.X < (double) num29 + (double) vector2_1.X / 2.0 
        && (double) Game1.mouseState[0].Position.Y > (double) num30 - (double) vector2_1.Y / 2.0 
        && (double) Game1.mouseState[0].Position.Y < (double) num30 + (double) vector2_1.Y / 2.0 - 10.0)
        {
          if (!Game1.mouseExit)
            Game1.PlaySound(12);
          Game1.mouseExit = true;
          Game1.player[Game1.myPlayer].mouseInterface = true;
          if (Game1.mouseLeftRelease && //Game1.mouseState.LeftButton == ButtonState.Pressed
                Game1.mouseState.Count == 1)
          {
            Game1.menuMode = 10;
            WorldGen.SaveAndQuit();
          }
        }
        else
          Game1.mouseExit = false;
        this.spriteBatch.DrawString(Game1.fontMouseText, "Inventory", new Vector2(40f, 0.0f), new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
        Game1.inventoryScale = 0.85f;
        Color color3;
        for (int index1 = 0; index1 < 10; ++index1)
        {
          for (int index2 = 0; index2 < 4; ++index2)
          {
            int x = (int) (20.0 + (double) (index1 * 56) * (double) Game1.inventoryScale);
            int y = (int) (20.0 + (double) (index2 * 56) * (double) Game1.inventoryScale);
            int index3 = index1 + index2 * 10;
            color3 = new Color(100, 100, 100, 100);
            if (Game1.mouseState[0].Position.X >= x && 
                            (double) Game1.mouseState[0].Position.X <= (double) x
                            + (double) Game1.inventoryBackTexture.Width 
                            * (double) Game1.inventoryScale 
                            && Game1.mouseState[0].Position.Y >= y 
                            && (double) Game1.mouseState[0].Position.Y <= (double) y 
                             + (double) Game1.inventoryBackTexture.Height * (double) Game1.inventoryScale)
            {
              Game1.player[Game1.myPlayer].mouseInterface = true;
              if (Game1.mouseLeftRelease 
                                && Game1.mouseState.Count == 1/*Game1.mouseState.LeftButton == ButtonState.Pressed*/)
              {
                if (Game1.player[Game1.myPlayer].selectedItem != index3 || Game1.player[Game1.myPlayer].itemAnimation <= 0)
                {
                  Item mouseItem = Game1.mouseItem;
                  Game1.mouseItem = Game1.player[Game1.myPlayer].inventory[index3];
                  Game1.player[Game1.myPlayer].inventory[index3] = mouseItem;
                  if (Game1.player[Game1.myPlayer].inventory[index3].type == 0 || Game1.player[Game1.myPlayer].inventory[index3].stack < 1)
                    Game1.player[Game1.myPlayer].inventory[index3] = new Item();
                  if (Game1.mouseItem.IsTheSameAs(Game1.player[Game1.myPlayer].inventory[index3]) && Game1.player[Game1.myPlayer].inventory[index3].stack != Game1.player[Game1.myPlayer].inventory[index3].maxStack && Game1.mouseItem.stack != Game1.mouseItem.maxStack)
                  {
                    if (Game1.mouseItem.stack + Game1.player[Game1.myPlayer].inventory[index3].stack <= Game1.mouseItem.maxStack)
                    {
                      Game1.player[Game1.myPlayer].inventory[index3].stack += Game1.mouseItem.stack;
                      Game1.mouseItem.stack = 0;
                    }
                    else
                    {
                      int num33 = Game1.mouseItem.maxStack - Game1.player[Game1.myPlayer].inventory[index3].stack;
                      Game1.player[Game1.myPlayer].inventory[index3].stack += num33;
                      Game1.mouseItem.stack -= num33;
                    }
                  }
                  if (Game1.mouseItem.type == 0 || Game1.mouseItem.stack < 1)
                    Game1.mouseItem = new Item();
                  if (Game1.mouseItem.type > 0 || Game1.player[Game1.myPlayer].inventory[index3].type > 0)
                  {
                    Recipe.FindRecipes();
                    Game1.PlaySound(7);
                  }
                }
              }
              else if (Game1.stackSplit <= 1 
                   && Game1.mouseState.Count > 1//Game1.mouseState.RightButton == ButtonState.Pressed 
                   && (Game1.mouseItem.IsTheSameAs(Game1.player[Game1.myPlayer].inventory[index3])
                   || Game1.mouseItem.type == 0) 
                   && (Game1.mouseItem.stack < Game1.mouseItem.maxStack || Game1.mouseItem.type == 0))
              {
                if (Game1.mouseItem.type == 0)
                {
                  Game1.mouseItem = (Item) Game1.player[Game1.myPlayer].inventory[index3].Clone();
                  Game1.mouseItem.stack = 0;
                }
                ++Game1.mouseItem.stack;
                --Game1.player[Game1.myPlayer].inventory[index3].stack;
                if (Game1.player[Game1.myPlayer].inventory[index3].stack <= 0)
                  Game1.player[Game1.myPlayer].inventory[index3] = new Item();
                Recipe.FindRecipes();
                Game1.soundInstanceMenuTick.Stop();
                Game1.soundInstanceMenuTick = Game1.soundMenuTick.CreateInstance();
                Game1.PlaySound(12);
                Game1.stackSplit = Game1.stackSplit != 0 ? Game1.stackDelay : 15;
              }
              cursorText1 = Game1.player[Game1.myPlayer].inventory[index3].name;
              Game1.toolTip = (Item) Game1.player[Game1.myPlayer].inventory[index3].Clone();
              if (Game1.player[Game1.myPlayer].inventory[index3].stack > 1)
                cursorText1 = cursorText1 + " (" + (object) Game1.player[Game1.myPlayer].inventory[index3].stack + ")";
            }
            this.spriteBatch.Draw(Game1.inventoryBackTexture, new Vector2((float) x, (float) y), new Rectangle?(new Rectangle(0, 0, Game1.inventoryBackTexture.Width, Game1.inventoryBackTexture.Height)), color1, 0.0f, new Vector2(), Game1.inventoryScale, SpriteEffects.None, 0.0f);
            color3 = Color.White;
            if (Game1.player[Game1.myPlayer].inventory[index3].type > 0 && Game1.player[Game1.myPlayer].inventory[index3].stack > 0)
            {
              float num34 = 1f;
              if (Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index3].type].Width > 32 || Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index3].type].Height > 32)
                num34 = Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index3].type].Width <= Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index3].type].Height ? 32f / (float) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index3].type].Height : 32f / (float) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index3].type].Width;
              float scale = num34 * Game1.inventoryScale;
              this.spriteBatch.Draw(Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index3].type], new Vector2((float) ((double) x + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index3].type].Width * 0.5 * (double) scale), (float) ((double) y + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index3].type].Height * 0.5 * (double) scale)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index3].type].Width, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index3].type].Height)), Game1.player[Game1.myPlayer].inventory[index3].GetAlpha(color3), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
              if (Game1.player[Game1.myPlayer].inventory[index3].color != new Color())
                this.spriteBatch.Draw(Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index3].type], new Vector2((float) ((double) x + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index3].type].Width * 0.5 * (double) scale), (float) ((double) y + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index3].type].Height * 0.5 * (double) scale)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index3].type].Width, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index3].type].Height)), Game1.player[Game1.myPlayer].inventory[index3].GetColor(color3), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
              if (Game1.player[Game1.myPlayer].inventory[index3].stack > 1)
                this.spriteBatch.DrawString(Game1.fontItemStack, string.Concat((object) Game1.player[Game1.myPlayer].inventory[index3].stack), new Vector2((float) x + 10f * Game1.inventoryScale, (float) y + 26f * Game1.inventoryScale), color3, 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
            }
          }
        }
        for (int index = 0; index < 8; ++index)
        {
          int x = Game1.screenWidth - 64 - 28;
          int y = (int) (174.0 + (double) (index * 56) * (double) Game1.inventoryScale);
          color3 = new Color(100, 100, 100, 100);
          string text4 = "";
          if (index == 0)
            text4 = "Helmet";
          else if (index == 1)
            text4 = "Shirt";
          else if (index == 2)
            text4 = "Pants";
          else if (index == 3)
            text4 = "Accessories";
          Vector2 vector2_2 = Game1.fontMouseText.MeasureString(text4);
          this.spriteBatch.DrawString(Game1.fontMouseText, text4, new Vector2((float) ((double) x 
              - (double) vector2_2.X - 10.0), (float) ((double) y 
              + (double) Game1.inventoryBackTexture.Height * 0.5 
              - (double) vector2_2.Y * 0.5)), new Color((int) Game1.mouseTextColor,
              (int) Game1.mouseTextColor, (int) Game1.mouseTextColor,
              (int) Game1.mouseTextColor), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
          if (Game1.mouseState[0].Position.X >= x && (double) Game1.mouseState[0].Position.X <= (double) x 
                        + (double) Game1.inventoryBackTexture.Width 
                        * (double) Game1.inventoryScale && Game1.mouseState[0].Position.Y >= y 
                        && (double) Game1.mouseState[0].Position.Y <= (double) y + (double) Game1.inventoryBackTexture.Height
                        * (double) Game1.inventoryScale)
          {
            Game1.player[Game1.myPlayer].mouseInterface = true;
            if (Game1.mouseLeftRelease 
                            && Game1.mouseState.Count == 1//Game1.mouseState.LeftButton == ButtonState.Pressed 
                            && (Game1.mouseItem.type == 0 
                            || Game1.mouseItem.headSlot > -1 && index == 0 
                            || Game1.mouseItem.bodySlot > -1 && index == 1
                            || Game1.mouseItem.legSlot > -1 && index == 2 
                            || Game1.mouseItem.accessory && index > 2))
            {
              Item mouseItem = Game1.mouseItem;
              Game1.mouseItem = Game1.player[Game1.myPlayer].armor[index];
              Game1.player[Game1.myPlayer].armor[index] = mouseItem;
              if (Game1.player[Game1.myPlayer].armor[index].type == 0 || Game1.player[Game1.myPlayer].armor[index].stack < 1)
                Game1.player[Game1.myPlayer].armor[index] = new Item();
              if (Game1.mouseItem.type == 0 || Game1.mouseItem.stack < 1)
                Game1.mouseItem = new Item();
              if (Game1.mouseItem.type > 0 || Game1.player[Game1.myPlayer].armor[index].type > 0)
              {
                Recipe.FindRecipes();
                Game1.PlaySound(7);
              }
            }
            cursorText1 = Game1.player[Game1.myPlayer].armor[index].name;
            Game1.toolTip = (Item) Game1.player[Game1.myPlayer].armor[index].Clone();
            if (index <= 2)
              Game1.toolTip.wornArmor = true;
            if (Game1.player[Game1.myPlayer].armor[index].stack > 1)
              cursorText1 = cursorText1 + " (" + (object) Game1.player[Game1.myPlayer].armor[index].stack + ")";
          }
          this.spriteBatch.Draw(Game1.inventoryBackTexture, new Vector2((float) x, (float) y), new Rectangle?(new Rectangle(0, 0, Game1.inventoryBackTexture.Width, Game1.inventoryBackTexture.Height)), color1, 0.0f, new Vector2(), Game1.inventoryScale, SpriteEffects.None, 0.0f);
          color3 = Color.White;
          if (Game1.player[Game1.myPlayer].armor[index].type > 0 && Game1.player[Game1.myPlayer].armor[index].stack > 0)
          {
            float num35 = 1f;
            if (Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Width > 32 || Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Height > 32)
              num35 = Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Width <= Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Height ? 32f / (float) Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Height : 32f / (float) Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Width;
            float scale = num35 * Game1.inventoryScale;
            this.spriteBatch.Draw(Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type], new Vector2((float) ((double) x + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Width * 0.5 * (double) scale), (float) ((double) y + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Height * 0.5 * (double) scale)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Width, Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Height)), Game1.player[Game1.myPlayer].armor[index].GetAlpha(color3), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
            if (Game1.player[Game1.myPlayer].armor[index].color != new Color())
              this.spriteBatch.Draw(Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type], new Vector2((float) ((double) x + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Width * 0.5 * (double) scale), (float) ((double) y + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Height * 0.5 * (double) scale)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Width, Game1.itemTexture[Game1.player[Game1.myPlayer].armor[index].type].Height)), Game1.player[Game1.myPlayer].armor[index].GetColor(color3), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
            if (Game1.player[Game1.myPlayer].armor[index].stack > 1)
              this.spriteBatch.DrawString(Game1.fontItemStack, string.Concat((object) Game1.player[Game1.myPlayer].armor[index].stack), new Vector2((float) x + 10f * Game1.inventoryScale, (float) y + 26f * Game1.inventoryScale), color3, 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
          }
        }
        this.spriteBatch.DrawString(Game1.fontMouseText, "Crafting", new Vector2(76f, 414f), new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
        for (int index = 0; index < Recipe.maxRecipes; ++index)
        {
          Game1.inventoryScale = (float) (100.0 / ((double) Math.Abs(Game1.availableRecipeY[index]) + 100.0));
          if ((double) Game1.inventoryScale < 0.75)
            Game1.inventoryScale = 0.75f;
          if ((double) Game1.availableRecipeY[index] < (double) ((index - Game1.focusRecipe) * 65))
          {
            if ((double) Game1.availableRecipeY[index] == 0.0)
              Game1.PlaySound(12);
            Game1.availableRecipeY[index] += 6.5f;
          }
          else if ((double) Game1.availableRecipeY[index] > (double) ((index - Game1.focusRecipe) * 65))
          {
            if ((double) Game1.availableRecipeY[index] == 0.0)
              Game1.PlaySound(12);
            Game1.availableRecipeY[index] -= 6.5f;
          }
          if (index < Game1.numAvailableRecipes 
                        && (double) Math.Abs(Game1.availableRecipeY[index]) <= 250.0)
          {
            int x = (int) (46.0 - 26.0 * (double) Game1.inventoryScale);
            int y = (int) (410.0 + (double) Game1.availableRecipeY[index] 
                            * (double) Game1.inventoryScale - 30.0 * (double) Game1.inventoryScale);
            double num36 = (double) ((int) color1.A + 50);
            double num37 = (double) byte.MaxValue;
            if ((double) Math.Abs(Game1.availableRecipeY[index]) > 150.0)
            {
              num36 = 150.0 * (100.0 - ((double) Math.Abs(Game1.availableRecipeY[index]) - 150.0)) * 0.01;
              num37 = (double) byte.MaxValue * (100.0 - ((double) 
                                Math.Abs(Game1.availableRecipeY[index]) - 150.0)) * 0.01;
            }
            color3 = new Color((int) (byte) num36, (int) (byte) num36, 
                (int) (byte) num36, (int) (byte) num36);
            Color newColor = new Color((int) (byte) num37, (int) (byte) num37, 
                (int) (byte) num37, (int) (byte) num37);
            if (Game1.mouseState[0].Position.X >= x && (double) Game1.mouseState[0].Position.X <= (double) x 
                            + (double) Game1.inventoryBackTexture.Width 
                            * (double) Game1.inventoryScale && Game1.mouseState[0].Position.Y >= y 
                            && (double) Game1.mouseState[0].Position.Y <= (double) y 
                            + (double) Game1.inventoryBackTexture.Height * (double) Game1.inventoryScale)
            {
              Game1.player[Game1.myPlayer].mouseInterface = true;
              if (Game1.mouseLeftRelease && 
                   Game1.mouseState.Count == 1/*Game1.mouseState.LeftButton == ButtonState.Pressed*/)
              {
                if (Game1.focusRecipe == index)
                {
                  if (Game1.mouseItem.type == 0 || Game1.mouseItem.IsTheSameAs(Game1.recipe[Game1.availableRecipe[index]].createItem) && Game1.mouseItem.stack + Game1.recipe[Game1.availableRecipe[index]].createItem.stack <= Game1.mouseItem.maxStack)
                  {
                    int stack = Game1.mouseItem.stack;
                    Game1.mouseItem = (Item) Game1.recipe[Game1.availableRecipe[index]].createItem.Clone();
                    Game1.mouseItem.stack += stack;
                    Game1.recipe[Game1.availableRecipe[index]].Create();
                    if (Game1.mouseItem.type > 0 || Game1.recipe[Game1.availableRecipe[index]].createItem.type > 0)
                      Game1.PlaySound(7);
                  }
                }
                else
                  Game1.focusRecipe = index;
              }
              cursorText1 = Game1.recipe[Game1.availableRecipe[index]].createItem.name;
              Game1.toolTip = (Item) Game1.recipe[Game1.availableRecipe[index]].createItem.Clone();
              if (Game1.recipe[Game1.availableRecipe[index]].createItem.stack > 1)
                cursorText1 = cursorText1 + " (" + (object) Game1.recipe[Game1.availableRecipe[index]].createItem.stack + ")";
            }
            if (Game1.numAvailableRecipes > 0)
            {
              double num38 = num36 - 50.0;
              if (num38 < 0.0)
                num38 = 0.0;
              this.spriteBatch.Draw(Game1.inventoryBackTexture, new Vector2((float) x, (float) y), new Rectangle?(new Rectangle(0, 0, Game1.inventoryBackTexture.Width, Game1.inventoryBackTexture.Height)), new Color((int) (byte) num38, (int) (byte) num38, (int) (byte) num38, (int) (byte) num38), 0.0f, new Vector2(), Game1.inventoryScale, SpriteEffects.None, 0.0f);
              if (Game1.recipe[Game1.availableRecipe[index]].createItem.type > 0 && Game1.recipe[Game1.availableRecipe[index]].createItem.stack > 0)
              {
                float num39 = 1f;
                if (Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Width > 32 || Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Height > 32)
                  num39 = Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Width <= Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Height ? 32f / (float) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Height : 32f / (float) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Width;
                float scale = num39 * Game1.inventoryScale;
                this.spriteBatch.Draw(Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type], new Vector2((float) ((double) x + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Width * 0.5 * (double) scale), (float) ((double) y + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Height * 0.5 * (double) scale)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Width, Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Height)), Game1.recipe[Game1.availableRecipe[index]].createItem.GetAlpha(newColor), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
                if (Game1.recipe[Game1.availableRecipe[index]].createItem.color != new Color())
                  this.spriteBatch.Draw(Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type], new Vector2((float) ((double) x + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Width * 0.5 * (double) scale), (float) ((double) y + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Height * 0.5 * (double) scale)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Width, Game1.itemTexture[Game1.recipe[Game1.availableRecipe[index]].createItem.type].Height)), Game1.recipe[Game1.availableRecipe[index]].createItem.GetColor(newColor), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
                if (Game1.recipe[Game1.availableRecipe[index]].createItem.stack > 1)
                  this.spriteBatch.DrawString(Game1.fontItemStack, string.Concat((object) Game1.recipe[Game1.availableRecipe[index]].createItem.stack), new Vector2((float) x + 10f * Game1.inventoryScale, (float) y + 26f * Game1.inventoryScale), color3, 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
              }
            }
          }
        }
        if (Game1.numAvailableRecipes > 0)
        {
          for (int index = 0; index < Recipe.maxRequirements && Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type != 0; ++index)
          {
            int x = 80 + index * 40;
            int y = 380;
            double num40 = (double) ((int) color1.A + 50);
            color3 = Color.White;
            Color white = Color.White;
            double num41 = (double) ((int) color1.A + 50) - (double) Math.Abs(Game1.availableRecipeY[Game1.focusRecipe]) * 2.0;
            double num42 = (double) byte.MaxValue - (double) Math.Abs(Game1.availableRecipeY[Game1.focusRecipe]) * 2.0;
            if (num41 < 0.0)
              num41 = 0.0;
            if (num42 < 0.0)
              num42 = 0.0;
            color3.R = (byte) num41;
            color3.G = (byte) num41;
            color3.B = (byte) num41;
            color3.A = (byte) num41;
            white.R = (byte) num42;
            white.G = (byte) num42;
            white.B = (byte) num42;
            white.A = (byte) num42;
            Game1.inventoryScale = 0.6f;
            if (num41 != 0.0)
            {
              if (Game1.mouseState[0].Position.X >= x 
                                && (double) Game1.mouseState[0].Position.X <= (double) x + (double) Game1.inventoryBackTexture.Width 
                                * (double) Game1.inventoryScale && (double) Game1.mouseState[0].Position.Y >= y 
                                    && (double) Game1.mouseState[0].Position.Y <= (double) y 
                                    + (double) Game1.inventoryBackTexture.Height * (double) Game1.inventoryScale)
              {
                Game1.player[Game1.myPlayer].mouseInterface = true;
                cursorText1 = Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].name;
                Game1.toolTip = (Item) Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].Clone();
                if (Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].stack > 1)
                  cursorText1 = cursorText1 + " (" + (object) Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].stack + ")";
              }
              double num43 = num41 - 50.0;
              if (num43 < 0.0)
                num43 = 0.0;
              this.spriteBatch.Draw(Game1.inventoryBackTexture, new Vector2((float) x, (float) y), new Rectangle?(new Rectangle(0, 0, Game1.inventoryBackTexture.Width, Game1.inventoryBackTexture.Height)), new Color((int) (byte) num43, (int) (byte) num43, (int) (byte) num43, (int) (byte) num43), 0.0f, new Vector2(), Game1.inventoryScale, SpriteEffects.None, 0.0f);
              if (Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type > 0 && Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].stack > 0)
              {
                float num44 = 1f;
                if (Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Width > 32 || Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Height > 32)
                  num44 = Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Width <= Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Height ? 32f / (float) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Height : 32f / (float) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Width;
                float scale = num44 * Game1.inventoryScale;
                this.spriteBatch.Draw(Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type], new Vector2((float) ((double) x + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Width * 0.5 * (double) scale), (float) ((double) y + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Height * 0.5 * (double) scale)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Width, Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Height)), Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].GetAlpha(white), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
                if (Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].color != new Color())
                  this.spriteBatch.Draw(Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type], new Vector2((float) ((double) x + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Width * 0.5 * (double) scale), (float) ((double) y + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Height * 0.5 * (double) scale)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Width, Game1.itemTexture[Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].type].Height)), Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].GetColor(white), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
                if (Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].stack > 1)
                  this.spriteBatch.DrawString(Game1.fontItemStack, string.Concat((object) Game1.recipe[Game1.availableRecipe[Game1.focusRecipe]].requiredItem[index].stack), new Vector2((float) x + 10f * Game1.inventoryScale, (float) y + 26f * Game1.inventoryScale), color3, 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
              }
            }
            else
              break;
          }
        }
        this.spriteBatch.DrawString(Game1.fontMouseText, "Coins", new Vector2(528f, 84f), new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor), 0.0f, new Vector2(), 0.8f, SpriteEffects.None, 0.0f);
        Game1.inventoryScale = 0.55f;
        for (int index4 = 0; index4 < 4; ++index4)
        {
          int x = 497;
          int y = (int) (85.0 + (double) (index4 * 56) * (double) Game1.inventoryScale);
          int index5 = index4 + 40;
          color3 = new Color(100, 100, 100, 100);
          if (Game1.mouseState[0].Position.X >= x 
                        && (double) Game1.mouseState[0].Position.X 
                        <= (double) x + (double) Game1.inventoryBackTexture.Width
                        * (double) Game1.inventoryScale
                        && Game1.mouseState[0].Position.Y >= y 
                        && (double) Game1.mouseState[0].Position.Y <= (double) y 
                        + (double) Game1.inventoryBackTexture.Height * (double) Game1.inventoryScale)
          {
            Game1.player[Game1.myPlayer].mouseInterface = true;
            if (Game1.mouseLeftRelease
                            && Game1.mouseState.Count==1//Game1.mouseState.LeftButton == ButtonState.Pressed
               )
            {
              if ((Game1.player[Game1.myPlayer].selectedItem != index5
                                || Game1.player[Game1.myPlayer].itemAnimation <= 0)
                                && (Game1.mouseItem.type == 0 || Game1.mouseItem.type == 71 
                                || Game1.mouseItem.type == 72 || Game1.mouseItem.type == 73 
                                || Game1.mouseItem.type == 74))
              {
                Item mouseItem = Game1.mouseItem;
                Game1.mouseItem = Game1.player[Game1.myPlayer].inventory[index5];
                Game1.player[Game1.myPlayer].inventory[index5] = mouseItem;
                if (Game1.player[Game1.myPlayer].inventory[index5].type == 0 
                                    || Game1.player[Game1.myPlayer].inventory[index5].stack < 1)
                  Game1.player[Game1.myPlayer].inventory[index5] = new Item();
                if (Game1.mouseItem.IsTheSameAs(Game1.player[Game1.myPlayer].inventory[index5])
                                    && Game1.player[Game1.myPlayer].inventory[index5].stack != Game1.player[Game1.myPlayer].inventory[index5].maxStack && Game1.mouseItem.stack != Game1.mouseItem.maxStack)
                {
                  if (Game1.mouseItem.stack + Game1.player[Game1.myPlayer].inventory[index5].stack 
                                        <= Game1.mouseItem.maxStack)
                  {
                    Game1.player[Game1.myPlayer].inventory[index5].stack += Game1.mouseItem.stack;
                    Game1.mouseItem.stack = 0;
                  }
                  else
                  {
                    int num45 = Game1.mouseItem.maxStack
                                            - Game1.player[Game1.myPlayer].inventory[index5].stack;
                    Game1.player[Game1.myPlayer].inventory[index5].stack += num45;
                    Game1.mouseItem.stack -= num45;
                  }
                }
                if (Game1.mouseItem.type == 0 || Game1.mouseItem.stack < 1)
                  Game1.mouseItem = new Item();
                if (Game1.mouseItem.type > 0 || Game1.player[Game1.myPlayer].inventory[index5].type > 0)
                  Game1.PlaySound(7);
              }
            }
            else if (Game1.stackSplit <= 1 
                            && Game1.mouseState.Count > 1//Game1.mouseState.RightButton == ButtonState.Pressed 
                            && (Game1.mouseItem.IsTheSameAs(Game1.player[Game1.myPlayer].inventory[index5]) 
                            || Game1.mouseItem.type == 0) 
                            && (Game1.mouseItem.stack < Game1.mouseItem.maxStack 
                            || Game1.mouseItem.type == 0))
            {
              if (Game1.mouseItem.type == 0)
              {
                Game1.mouseItem = (Item) Game1.player[Game1.myPlayer].inventory[index5].Clone();
                Game1.mouseItem.stack = 0;
              }
              ++Game1.mouseItem.stack;
              --Game1.player[Game1.myPlayer].inventory[index5].stack;
              if (Game1.player[Game1.myPlayer].inventory[index5].stack <= 0)
                Game1.player[Game1.myPlayer].inventory[index5] = new Item();
              Recipe.FindRecipes();
              Game1.soundInstanceMenuTick.Stop();
              Game1.soundInstanceMenuTick = Game1.soundMenuTick.CreateInstance();
              Game1.PlaySound(12);
              Game1.stackSplit = Game1.stackSplit != 0 ? Game1.stackDelay : 15;
            }
            cursorText1 = Game1.player[Game1.myPlayer].inventory[index5].name;
            Game1.toolTip = (Item) Game1.player[Game1.myPlayer].inventory[index5].Clone();
            if (Game1.player[Game1.myPlayer].inventory[index5].stack > 1)
              cursorText1 = cursorText1 + " (" + (object) Game1.player[Game1.myPlayer].inventory[index5].stack + ")";
          }
          this.spriteBatch.Draw(Game1.inventoryBackTexture, new Vector2((float) x, (float) y), new Rectangle?(new Rectangle(0, 0, Game1.inventoryBackTexture.Width, Game1.inventoryBackTexture.Height)), color1, 0.0f, new Vector2(), Game1.inventoryScale, SpriteEffects.None, 0.0f);
          color3 = Color.White;
          if (Game1.player[Game1.myPlayer].inventory[index5].type > 0 && Game1.player[Game1.myPlayer].inventory[index5].stack > 0)
          {
            float num46 = 1f;
            if (Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index5].type].Width > 32 || Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index5].type].Height > 32)
              num46 = Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index5].type].Width <= Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index5].type].Height ? 32f / (float) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index5].type].Height : 32f / (float) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index5].type].Width;
            float scale = num46 * Game1.inventoryScale;
            this.spriteBatch.Draw(Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index5].type], new Vector2((float) ((double) x + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index5].type].Width * 0.5 * (double) scale), (float) ((double) y + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index5].type].Height * 0.5 * (double) scale)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index5].type].Width, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index5].type].Height)), Game1.player[Game1.myPlayer].inventory[index5].GetAlpha(color3), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
            if (Game1.player[Game1.myPlayer].inventory[index5].color != new Color())
              this.spriteBatch.Draw(Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index5].type], new Vector2((float) ((double) x + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index5].type].Width * 0.5 * (double) scale), (float) ((double) y + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index5].type].Height * 0.5 * (double) scale)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index5].type].Width, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index5].type].Height)), Game1.player[Game1.myPlayer].inventory[index5].GetColor(color3), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
            if (Game1.player[Game1.myPlayer].inventory[index5].stack > 1)
              this.spriteBatch.DrawString(Game1.fontItemStack, string.Concat((object) Game1.player[Game1.myPlayer].inventory[index5].stack), new Vector2((float) x + 10f * Game1.inventoryScale, (float) y + 26f * Game1.inventoryScale), color3, 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
          }
        }
        if (Game1.npcShop > 0 && (!Game1.playerInventory || Game1.player[Game1.myPlayer].talkNPC == -1))
          Game1.npcShop = 0;
        if (Game1.npcShop > 0)
        {
          this.spriteBatch.DrawString(Game1.fontMouseText, "Shop", new Vector2(284f, 210f), new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
          Game1.inventoryScale = 0.75f;
          for (int index6 = 0; index6 < 5; ++index6)
          {
            for (int index7 = 0; index7 < 4; ++index7)
            {
              int x = (int) (73.0 + (double) (index6 * 56) * (double) Game1.inventoryScale);
              int y = (int) (210.0 + (double) (index7 * 56) * (double) Game1.inventoryScale);
              int index8 = index6 + index7 * 5;
              color3 = new Color(100, 100, 100, 100);
              if (Game1.mouseState[0].Position.X >= x 
                && (double) Game1.mouseState[0].Position.X <= (double) x 
                + (double) Game1.inventoryBackTexture.Width 
                * (double) Game1.inventoryScale 
                && Game1.mouseState[0].Position.Y >= y 
                && (double) Game1.mouseState[0].Position.Y <= (double) y 
                + (double) Game1.inventoryBackTexture.Height * (double) Game1.inventoryScale)
              {
                Game1.player[Game1.myPlayer].mouseInterface = true;
                if (Game1.mouseLeftRelease && Game1.mouseState.Count==1//Game1.mouseState.LeftButton == ButtonState.Pressed
                )
                {
                  if (Game1.mouseItem.type == 0)
                  {
                    if ((Game1.player[Game1.myPlayer].selectedItem != index8 || Game1.player[Game1.myPlayer].itemAnimation <= 0) && Game1.player[Game1.myPlayer].BuyItem(this.shop[Game1.npcShop].item[index8].value))
                    {
                      Game1.mouseItem.SetDefaults(this.shop[Game1.npcShop].item[index8].name);
                      Game1.PlaySound(18);
                    }
                  }
                  else if (Game1.player[Game1.myPlayer].SellItem(Game1.mouseItem.value * Game1.mouseItem.stack))
                  {
                    Game1.mouseItem.stack = 0;
                    Game1.mouseItem.type = 0;
                    Game1.PlaySound(18);
                  }
                  else if (Game1.mouseItem.value == 0)
                  {
                    Game1.mouseItem.stack = 0;
                    Game1.mouseItem.type = 0;
                    Game1.PlaySound(7);
                  }
                }
                else if (Game1.stackSplit <= 1 
                    && Game1.mouseState.Count > 1//Game1.mouseState.RightButton == ButtonState.Pressed 
                    && (Game1.mouseItem.IsTheSameAs(this.shop[Game1.npcShop].item[index8])
                    || Game1.mouseItem.type == 0) 
                    && (Game1.mouseItem.stack < Game1.mouseItem.maxStack 
                    || Game1.mouseItem.type == 0) 
                    && Game1.player[Game1.myPlayer].BuyItem(this.shop[Game1.npcShop].item[index8].value))
                {
                  Game1.PlaySound(18);
                  if (Game1.mouseItem.type == 0)
                  {
                    Game1.mouseItem = (Item) this.shop[Game1.npcShop].item[index8].Clone();
                    Game1.mouseItem.stack = 0;
                  }
                  ++Game1.mouseItem.stack;
                  Game1.stackSplit = Game1.stackSplit != 0 ? Game1.stackDelay : 15;
                }
                cursorText1 = this.shop[Game1.npcShop].item[index8].name;
                Game1.toolTip = (Item) this.shop[Game1.npcShop].item[index8].Clone();
                Game1.toolTip.buy = true;
                if (this.shop[Game1.npcShop].item[index8].stack > 1)
                  cursorText1 = cursorText1 + " (" + (object) this.shop[Game1.npcShop].item[index8].stack + ")";
              }
              this.spriteBatch.Draw(Game1.inventoryBackTexture, new Vector2((float) x, (float) y), new Rectangle?(new Rectangle(0, 0, Game1.inventoryBackTexture.Width, Game1.inventoryBackTexture.Height)), color1, 0.0f, new Vector2(), Game1.inventoryScale, SpriteEffects.None, 0.0f);
              color3 = Color.White;
              if (this.shop[Game1.npcShop].item[index8].type > 0 && this.shop[Game1.npcShop].item[index8].stack > 0)
              {
                float num47 = 1f;
                if (Game1.itemTexture[this.shop[Game1.npcShop].item[index8].type].Width > 32 || Game1.itemTexture[this.shop[Game1.npcShop].item[index8].type].Height > 32)
                  num47 = Game1.itemTexture[this.shop[Game1.npcShop].item[index8].type].Width <= Game1.itemTexture[this.shop[Game1.npcShop].item[index8].type].Height ? 32f / (float) Game1.itemTexture[this.shop[Game1.npcShop].item[index8].type].Height : 32f / (float) Game1.itemTexture[this.shop[Game1.npcShop].item[index8].type].Width;
                float scale = num47 * Game1.inventoryScale;
                this.spriteBatch.Draw(Game1.itemTexture[this.shop[Game1.npcShop].item[index8].type], new Vector2((float) ((double) x + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[this.shop[Game1.npcShop].item[index8].type].Width * 0.5 * (double) scale), (float) ((double) y + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[this.shop[Game1.npcShop].item[index8].type].Height * 0.5 * (double) scale)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[this.shop[Game1.npcShop].item[index8].type].Width, Game1.itemTexture[this.shop[Game1.npcShop].item[index8].type].Height)), this.shop[Game1.npcShop].item[index8].GetAlpha(color3), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
                if (this.shop[Game1.npcShop].item[index8].color != new Color())
                  this.spriteBatch.Draw(Game1.itemTexture[this.shop[Game1.npcShop].item[index8].type], new Vector2((float) ((double) x + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[this.shop[Game1.npcShop].item[index8].type].Width * 0.5 * (double) scale), (float) ((double) y + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[this.shop[Game1.npcShop].item[index8].type].Height * 0.5 * (double) scale)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[this.shop[Game1.npcShop].item[index8].type].Width, Game1.itemTexture[this.shop[Game1.npcShop].item[index8].type].Height)), this.shop[Game1.npcShop].item[index8].GetColor(color3), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
                if (this.shop[Game1.npcShop].item[index8].stack > 1)
                  this.spriteBatch.DrawString(Game1.fontItemStack, string.Concat((object) this.shop[Game1.npcShop].item[index8].stack), new Vector2((float) x + 10f * Game1.inventoryScale, (float) y + 26f * Game1.inventoryScale), color3, 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
              }
            }
          }
        }
        if (Game1.player[Game1.myPlayer].chest > -1)
        {
          this.spriteBatch.DrawString(Game1.fontMouseText, "Chest", new Vector2(284f, 210f), new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
          Game1.inventoryScale = 0.75f;
          for (int index9 = 0; index9 < 5; ++index9)
          {
            for (int index10 = 0; index10 < 4; ++index10)
            {
              int x = (int) (73.0 + (double) (index9 * 56) * (double) Game1.inventoryScale);
              int y = (int) (210.0 + (double) (index10 * 56) * (double) Game1.inventoryScale);
              int number2 = index9 + index10 * 5;
              color3 = new Color(100, 100, 100, 100);
              if (Game1.mouseState[0].Position.X >= x
                                && (double) Game1.mouseState[0].Position.X <= (double) x 
                                + (double) Game1.inventoryBackTexture.Width 
                                * (double) Game1.inventoryScale 
                                && Game1.mouseState[0].Position.Y >= y 
                                && (double) Game1.mouseState[0].Position.Y <= (double) y 
                                + (double) Game1.inventoryBackTexture.Height 
                                * (double) Game1.inventoryScale)
              {
                Game1.player[Game1.myPlayer].mouseInterface = true;
                if (Game1.mouseLeftRelease && Game1.mouseState.Count==1//Game1.mouseState.LeftButton == ButtonState.Pressed
                 )
                {
                  if (Game1.player[Game1.myPlayer].selectedItem != number2 || Game1.player[Game1.myPlayer].itemAnimation <= 0)
                  {
                    Item mouseItem = Game1.mouseItem;
                    Game1.mouseItem = Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2];
                    Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2] = mouseItem;
                    if (Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].type == 0 || Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].stack < 1)
                      Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2] = new Item();
                    if (Game1.mouseItem.IsTheSameAs(Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2]) && Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].stack != Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].maxStack && Game1.mouseItem.stack != Game1.mouseItem.maxStack)
                    {
                      if (Game1.mouseItem.stack + Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].stack <= Game1.mouseItem.maxStack)
                      {
                        Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].stack += Game1.mouseItem.stack;
                        Game1.mouseItem.stack = 0;
                      }
                      else
                      {
                        int num48 = Game1.mouseItem.maxStack - Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].stack;
                        Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].stack += num48;
                        Game1.mouseItem.stack -= num48;
                      }
                    }
                    if (Game1.mouseItem.type == 0 || Game1.mouseItem.stack < 1)
                      Game1.mouseItem = new Item();
                    if (Game1.mouseItem.type > 0 || Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].type > 0)
                    {
                      Recipe.FindRecipes();
                      Game1.PlaySound(7);
                    }
                    if (Game1.netMode == 1)
                      NetMessage.SendData(32, number: Game1.player[Game1.myPlayer].chest, number2: (float) number2);
                  }
                }
                else if (Game1.stackSplit <= 1 
                 && Game1.mouseState.Count > 1//Game1.mouseState.RightButton == ButtonState.Pressed 
               && (Game1.mouseItem.IsTheSameAs(Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2]) 
               || Game1.mouseItem.type == 0) && (Game1.mouseItem.stack < Game1.mouseItem.maxStack 
               || Game1.mouseItem.type == 0))
                {
                  if (Game1.mouseItem.type == 0)
                  {
                    Game1.mouseItem = (Item) Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].Clone();
                    Game1.mouseItem.stack = 0;
                  }
                  ++Game1.mouseItem.stack;
                  --Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].stack;
                  if (Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].stack <= 0)
                    Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2] = new Item();
                  Recipe.FindRecipes();
                  Game1.soundInstanceMenuTick.Stop();
                  Game1.soundInstanceMenuTick = Game1.soundMenuTick.CreateInstance();
                  Game1.PlaySound(12);
                  Game1.stackSplit = Game1.stackSplit != 0 ? Game1.stackDelay : 15;
                  if (Game1.netMode == 1)
                    NetMessage.SendData(32, number: Game1.player[Game1.myPlayer].chest, number2: (float) number2);
                }
                cursorText1 = Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].name;
                Game1.toolTip = (Item) Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].Clone();
                if (Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].stack > 1)
                  cursorText1 = cursorText1 + " (" + (object) Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].stack + ")";
              }
              this.spriteBatch.Draw(Game1.inventoryBackTexture, new Vector2((float) x, (float) y), new Rectangle?(new Rectangle(0, 0, Game1.inventoryBackTexture.Width, Game1.inventoryBackTexture.Height)), color1, 0.0f, new Vector2(), Game1.inventoryScale, SpriteEffects.None, 0.0f);
              color3 = Color.White;
              if (Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].type > 0 && Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].stack > 0)
              {
                float num49 = 1f;
                if (Game1.itemTexture[Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].type].Width > 32 || Game1.itemTexture[Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].type].Height > 32)
                  num49 = Game1.itemTexture[Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].type].Width <= Game1.itemTexture[Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].type].Height ? 32f / (float) Game1.itemTexture[Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].type].Height : 32f / (float) Game1.itemTexture[Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].type].Width;
                float scale = num49 * Game1.inventoryScale;
                this.spriteBatch.Draw(Game1.itemTexture[Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].type], new Vector2((float) ((double) x + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].type].Width * 0.5 * (double) scale), (float) ((double) y + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].type].Height * 0.5 * (double) scale)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].type].Width, Game1.itemTexture[Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].type].Height)), Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].GetAlpha(color3), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
                if (Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].color != new Color())
                  this.spriteBatch.Draw(Game1.itemTexture[Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].type], new Vector2((float) ((double) x + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].type].Width * 0.5 * (double) scale), (float) ((double) y + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].type].Height * 0.5 * (double) scale)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].type].Width, Game1.itemTexture[Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].type].Height)), Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].GetColor(color3), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
                if (Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].stack > 1)
                  this.spriteBatch.DrawString(Game1.fontItemStack, string.Concat((object) Game1.chest[Game1.player[Game1.myPlayer].chest].item[number2].stack), new Vector2((float) x + 10f * Game1.inventoryScale, (float) y + 26f * Game1.inventoryScale), color3, 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
              }
            }
          }
        }
        if (Game1.player[Game1.myPlayer].chest == -2)
        {
          this.spriteBatch.DrawString(Game1.fontMouseText, "Piggy Bank", new Vector2(284f, 210f), new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
          Game1.inventoryScale = 0.75f;
          for (int index11 = 0; index11 < 5; ++index11)
          {
            for (int index12 = 0; index12 < 4; ++index12)
            {
              int x = (int) (73.0 + (double) (index11 * 56) * (double) Game1.inventoryScale);
              int y = (int) (210.0 + (double) (index12 * 56) * (double) Game1.inventoryScale);
              int index13 = index11 + index12 * 5;
              color3 = new Color(100, 100, 100, 100);
              if (Game1.mouseState[0].Position.X >= x 
                                && (double) Game1.mouseState[0].Position.X 
                                <= (double) x + (double) Game1.inventoryBackTexture.Width
                                * (double) Game1.inventoryScale 
                                && Game1.mouseState[0].Position.Y >= y 
                                && (double) Game1.mouseState[0].Position.Y <= 
                                (double) y + (double) Game1.inventoryBackTexture.Height
                                * (double) Game1.inventoryScale)
              {
                Game1.player[Game1.myPlayer].mouseInterface = true;
                if (Game1.mouseLeftRelease && Game1.mouseState.Count==1//Game1.mouseState.LeftButton == ButtonState.Pressed
                )
                {
                  if (Game1.player[Game1.myPlayer].selectedItem != index13 || Game1.player[Game1.myPlayer].itemAnimation <= 0)
                  {
                    Item mouseItem = Game1.mouseItem;
                    Game1.mouseItem = Game1.player[Game1.myPlayer].bank[index13];
                    Game1.player[Game1.myPlayer].bank[index13] = mouseItem;
                    if (Game1.player[Game1.myPlayer].bank[index13].type == 0 || Game1.player[Game1.myPlayer].bank[index13].stack < 1)
                      Game1.player[Game1.myPlayer].bank[index13] = new Item();
                    if (Game1.mouseItem.IsTheSameAs(Game1.player[Game1.myPlayer].bank[index13]) && Game1.player[Game1.myPlayer].bank[index13].stack != Game1.player[Game1.myPlayer].bank[index13].maxStack && Game1.mouseItem.stack != Game1.mouseItem.maxStack)
                    {
                      if (Game1.mouseItem.stack + Game1.player[Game1.myPlayer].bank[index13].stack <= Game1.mouseItem.maxStack)
                      {
                        Game1.player[Game1.myPlayer].bank[index13].stack += Game1.mouseItem.stack;
                        Game1.mouseItem.stack = 0;
                      }
                      else
                      {
                        int num50 = Game1.mouseItem.maxStack - Game1.player[Game1.myPlayer].bank[index13].stack;
                        Game1.player[Game1.myPlayer].bank[index13].stack += num50;
                        Game1.mouseItem.stack -= num50;
                      }
                    }
                    if (Game1.mouseItem.type == 0 || Game1.mouseItem.stack < 1)
                      Game1.mouseItem = new Item();
                    if (Game1.mouseItem.type > 0 || Game1.player[Game1.myPlayer].bank[index13].type > 0)
                    {
                      Recipe.FindRecipes();
                      Game1.PlaySound(7);
                    }
                  }
                }
                else if (Game1.stackSplit <= 1 
                                    && Game1.mouseState.Count > 1//Game1.mouseState.RightButton == ButtonState.Pressed 
                                    && (Game1.mouseItem.IsTheSameAs(
                                        Game1.player[Game1.myPlayer].bank[index13]) 
                                        || Game1.mouseItem.type == 0) 
                                        && (Game1.mouseItem.stack < Game1.mouseItem.maxStack 
                                        || Game1.mouseItem.type == 0))
                {
                  if (Game1.mouseItem.type == 0)
                  {
                    Game1.mouseItem = (Item) Game1.player[Game1.myPlayer].bank[index13].Clone();
                    Game1.mouseItem.stack = 0;
                  }
                  ++Game1.mouseItem.stack;
                  --Game1.player[Game1.myPlayer].bank[index13].stack;
                  if (Game1.player[Game1.myPlayer].bank[index13].stack <= 0)
                    Game1.player[Game1.myPlayer].bank[index13] = new Item();
                  Recipe.FindRecipes();
                  Game1.soundInstanceMenuTick.Stop();
                  Game1.soundInstanceMenuTick = Game1.soundMenuTick.CreateInstance();
                  Game1.PlaySound(12);
                  Game1.stackSplit = Game1.stackSplit != 0 ? Game1.stackDelay : 15;
                }
                cursorText1 = Game1.player[Game1.myPlayer].bank[index13].name;
                Game1.toolTip = (Item) Game1.player[Game1.myPlayer].bank[index13].Clone();
                if (Game1.player[Game1.myPlayer].bank[index13].stack > 1)
                  cursorText1 = cursorText1 + " (" + (object) Game1.player[Game1.myPlayer].bank[index13].stack + ")";
              }
              this.spriteBatch.Draw(Game1.inventoryBackTexture, new Vector2((float) x, (float) y), new Rectangle?(new Rectangle(0, 0, Game1.inventoryBackTexture.Width, Game1.inventoryBackTexture.Height)), color1, 0.0f, new Vector2(), Game1.inventoryScale, SpriteEffects.None, 0.0f);
              color3 = Color.White;
              if (Game1.player[Game1.myPlayer].bank[index13].type > 0 && Game1.player[Game1.myPlayer].bank[index13].stack > 0)
              {
                float num51 = 1f;
                if (Game1.itemTexture[Game1.player[Game1.myPlayer].bank[index13].type].Width > 32 || Game1.itemTexture[Game1.player[Game1.myPlayer].bank[index13].type].Height > 32)
                  num51 = Game1.itemTexture[Game1.player[Game1.myPlayer].bank[index13].type].Width <= Game1.itemTexture[Game1.player[Game1.myPlayer].bank[index13].type].Height ? 32f / (float) Game1.itemTexture[Game1.player[Game1.myPlayer].bank[index13].type].Height : 32f / (float) Game1.itemTexture[Game1.player[Game1.myPlayer].bank[index13].type].Width;
                float scale = num51 * Game1.inventoryScale;
                this.spriteBatch.Draw(Game1.itemTexture[Game1.player[Game1.myPlayer].bank[index13].type], new Vector2((float) ((double) x + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].bank[index13].type].Width * 0.5 * (double) scale), (float) ((double) y + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].bank[index13].type].Height * 0.5 * (double) scale)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.player[Game1.myPlayer].bank[index13].type].Width, Game1.itemTexture[Game1.player[Game1.myPlayer].bank[index13].type].Height)), Game1.player[Game1.myPlayer].bank[index13].GetAlpha(color3), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
                if (Game1.player[Game1.myPlayer].bank[index13].color != new Color())
                  this.spriteBatch.Draw(Game1.itemTexture[Game1.player[Game1.myPlayer].bank[index13].type], new Vector2((float) ((double) x + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].bank[index13].type].Width * 0.5 * (double) scale), (float) ((double) y + 26.0 * (double) Game1.inventoryScale - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].bank[index13].type].Height * 0.5 * (double) scale)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.player[Game1.myPlayer].bank[index13].type].Width, Game1.itemTexture[Game1.player[Game1.myPlayer].bank[index13].type].Height)), Game1.player[Game1.myPlayer].bank[index13].GetColor(color3), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
                if (Game1.player[Game1.myPlayer].bank[index13].stack > 1)
                  this.spriteBatch.DrawString(Game1.fontItemStack, string.Concat((object) Game1.player[Game1.myPlayer].bank[index13].stack), new Vector2((float) x + 10f * Game1.inventoryScale, (float) y + 26f * Game1.inventoryScale), color3, 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
              }
            }
          }
        }
      }
      else
      {
        bool flag2 = false;
        bool flag3 = false;
        for (int index14 = 0; index14 < 3; ++index14)
        {
          string text = "";
          if (Game1.player[Game1.myPlayer].accDepthMeter > 0 && !flag3)
          {
            int num52 = (int) (((double) Game1.player[Game1.myPlayer].position.Y + (double) Game1.player[Game1.myPlayer].height) * 2.0 / 16.0 - Game1.worldSurface * 2.0);
            if (num52 > 0)
            {
              text = "Depth: " + (object) num52 + " feet below";
              if (num52 == 1)
                text = "Depth: " + (object) num52 + " foot below";
            }
            else if (num52 < 0)
            {
              int num53 = num52 * -1;
              text = "Depth: " + (object) num53 + " feet above";
              if (num53 == 1)
                text = "Depth: " + (object) num53 + " foot above";
            }
            else
              text = "Depth: Level";
            flag3 = true;
          }
          else if (Game1.player[Game1.myPlayer].accWatch > 0 && !flag2)
          {
            string str1 = "AM";
            double time = Game1.time;
            if (!Game1.dayTime)
              time += 54000.0;
            double num54 = time / 86400.0 * 24.0 - 7.5 - 12.0;
            if (num54 < 0.0)
              num54 += 24.0;
            if (num54 >= 12.0)
              str1 = "PM";
            int num55 = (int) num54;
            double num56 = (double) (int) ((num54 - (double) num55) * 60.0);
            string str2 = string.Concat((object) num56);
            if (num56 < 10.0)
              str2 = "0" + str2;
            if (num55 > 12)
              num55 -= 12;
            if (num55 == 0)
              num55 = 12;
            if (Game1.player[Game1.myPlayer].accWatch == 1)
              str2 = "00";
            else if (Game1.player[Game1.myPlayer].accWatch == 2)
              str2 = num56 >= 30.0 ? "30" : "00";
            text = "Time: " + (object) num55 + ":" + str2 + " " + str1;
            flag2 = true;
          }
          if (text != "")
          {
            for (int index15 = 0; index15 < 5; ++index15)
            {
              int num57 = 0;
              int num58 = 0;
              Color color4 = Color.Black;
              if (index15 == 0)
                num57 = -2;
              if (index15 == 1)
                num57 = 2;
              if (index15 == 2)
                num58 = -2;
              if (index15 == 3)
                num58 = 2;
              if (index15 == 4)
                color4 = new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor);
              this.spriteBatch.DrawString(Game1.fontMouseText, text, new Vector2((float) (22 + num57), (float) (74 + 22 * index14 + num58)), color4, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
            }
          }
        }
      }
      if (!Game1.playerInventory)
      {
        this.spriteBatch.DrawString(Game1.fontMouseText, "Items", new Vector2(215f, 0.0f), 
            new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, 
            (int) Game1.mouseTextColor, (int) Game1.mouseTextColor), 0.0f, new Vector2(), 
            1f, SpriteEffects.None, 0.0f);
        int x = 20;

        for (int index = 0; index < 10; ++index)
        {
          if (index == Game1.player[Game1.myPlayer].selectedItem)
          {
            if ((double) Game1.hotbarScale[index] < 1.0)
              Game1.hotbarScale[index] += 0.05f;
          }
          else if ((double) Game1.hotbarScale[index] > 0.75)
            Game1.hotbarScale[index] -= 0.05f;

          int y = (int) (20.0 + 22.0 * (1.0 - (double) Game1.hotbarScale[index]));

          Color color5 = new Color((int) byte.MaxValue, (int) byte.MaxValue, 
              (int) byte.MaxValue, (int) (75.0 + 150.0 * (double) Game1.hotbarScale[index]));

          this.spriteBatch.Draw(Game1.inventoryBackTexture, new Vector2((float) x, (float) y), 
              new Rectangle?(new Rectangle(0, 0, Game1.inventoryBackTexture.Width, 
              Game1.inventoryBackTexture.Height)), new Color(100, 100, 100, 100),
              0.0f, new Vector2(), Game1.hotbarScale[index], SpriteEffects.None, 0.0f);
          if (Game1.mouseState[0].Position.X >= x 
                        && (double) Game1.mouseState[0].Position.X <= (double) x 
                        + (double) Game1.inventoryBackTexture.Width 
                        * (double) Game1.hotbarScale[index]
                        && Game1.mouseState[0].Position.Y >= y 
                        && (double) Game1.mouseState[0].Position.Y <= (double) y 
                        + (double) Game1.inventoryBackTexture.Height 
                        * (double) Game1.hotbarScale[index] && !Game1.player[Game1.myPlayer].channel)
          {
            Game1.player[Game1.myPlayer].mouseInterface = true;
            if (Game1.mouseState.Count==1//Game1.mouseState.LeftButton == ButtonState.Pressed
            )
              Game1.player[Game1.myPlayer].changeItem = index;
            Game1.player[Game1.myPlayer].showItemIcon = false;
            cursorText1 = Game1.player[Game1.myPlayer].inventory[index].name;
            if (Game1.player[Game1.myPlayer].inventory[index].stack > 1)
              cursorText1 = cursorText1 + " (" + (object) Game1.player[Game1.myPlayer].inventory[index].stack + ")";
            rare1 = Game1.player[Game1.myPlayer].inventory[index].rare;
          }
          if (Game1.player[Game1.myPlayer].inventory[index].type > 0 && Game1.player[Game1.myPlayer].inventory[index].stack > 0)
          {
            float num59 = 1f;
            if (Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Width > 32 || Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Height > 32)
              num59 = Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Width <= Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Height ? 32f / (float) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Height : 32f / (float) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Width;
            float scale = num59 * Game1.hotbarScale[index];
            this.spriteBatch.Draw(Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type], new Vector2((float) ((double) x + 26.0 * (double) Game1.hotbarScale[index] - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Width * 0.5 * (double) scale), (float) ((double) y + 26.0 * (double) Game1.hotbarScale[index] - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Height * 0.5 * (double) scale)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Width, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Height)), Game1.player[Game1.myPlayer].inventory[index].GetAlpha(color5), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
            if (Game1.player[Game1.myPlayer].inventory[index].color != new Color())
              this.spriteBatch.Draw(Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type], new Vector2((float) ((double) x + 26.0 * (double) Game1.hotbarScale[index] - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Width * 0.5 * (double) scale), (float) ((double) y + 26.0 * (double) Game1.hotbarScale[index] - (double) Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Height * 0.5 * (double) scale)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Width, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[index].type].Height)), Game1.player[Game1.myPlayer].inventory[index].GetColor(color5), 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
            if (Game1.player[Game1.myPlayer].inventory[index].stack > 1)
              this.spriteBatch.DrawString(Game1.fontItemStack, string.Concat((object) Game1.player[Game1.myPlayer].inventory[index].stack), new Vector2((float) x + 10f * Game1.hotbarScale[index], (float) y + 26f * Game1.hotbarScale[index]), color5, 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
          }
          x += (int) ((double) Game1.inventoryBackTexture.Width * (double) Game1.hotbarScale[index]) + 4;
        }
      }
      if (cursorText1 != null && cursorText1 != "" && Game1.mouseItem.type == 0)
      {
        Game1.player[Game1.myPlayer].showItemIcon = false;
        this.MouseText(cursorText1, rare1);
        flag1 = true;
      }
      if (Game1.chatMode)
      {
        ++this.textBlinkerCount;
        if (this.textBlinkerCount >= 20)
        {
          this.textBlinkerState = this.textBlinkerState != 0 ? 0 : 1;
          this.textBlinkerCount = 0;
        }
        string chatText = Game1.chatText;
        if (this.textBlinkerState == 1)
          chatText += "|";
        this.spriteBatch.Draw(Game1.textBackTexture, new Vector2(78f, (float) (Game1.screenHeight - 36)), new Rectangle?(new Rectangle(0, 0, Game1.textBackTexture.Width, Game1.textBackTexture.Height)), new Color(100, 100, 100, 100), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
        for (int index = 0; index < 5; ++index)
        {
          int num60 = 0;
          int num61 = 0;
          Color color6 = Color.Black;
          if (index == 0)
            num60 = -2;
          if (index == 1)
            num60 = 2;
          if (index == 2)
            num61 = -2;
          if (index == 3)
            num61 = 2;
          if (index == 4)
            color6 = new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor);
          this.spriteBatch.DrawString(Game1.fontMouseText, chatText, new Vector2((float) (88 + num60), (float) (Game1.screenHeight - 30 + num61)), color6, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
        }
      }
      for (int index16 = 0; index16 < Game1.numChatLines; ++index16)
      {
        if (Game1.chatMode || Game1.chatLine[index16].showTime > 0)
        {
          float num62 = (float) Game1.mouseTextColor / (float) byte.MaxValue;
          for (int index17 = 0; index17 < 5; ++index17)
          {
            int num63 = 0;
            int num64 = 0;
            Color color7 = Color.Black;
            if (index17 == 0)
              num63 = -2;
            if (index17 == 1)
              num63 = 2;
            if (index17 == 2)
              num64 = -2;
            if (index17 == 3)
              num64 = 2;
            if (index17 == 4)
              color7 = 
                new Color((int) (byte) ((double) Game1.chatLine[index16].color.R 
                  * (double) num62), 
                 (int) (byte) ((double) Game1.chatLine[index16].color.G 
                  * (double) num62), 
                 (int) (byte) ((double) Game1.chatLine[index16].color.B 
                  * (double) num62), 
                 (int) Game1.mouseTextColor);

            this.spriteBatch.DrawString(Game1.fontMouseText,
                Game1.chatLine[index16].text, new Vector2((float) (88 + num63), (float) (Game1.screenHeight - 30 + num64 - 28 - index16 * 21)), color7, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
          }
        }
      }
      if (Game1.player[Game1.myPlayer].dead)
      {
        string text = Game1.player[Game1.myPlayer].name + " was slain...";
        this.spriteBatch.DrawString(Game1.fontDeathText, text, 
            new Vector2((float) (Game1.screenWidth / 2 - text.Length * 10), 
            (float) (Game1.screenHeight / 2 - 20)), 
            Game1.player[Game1.myPlayer].GetDeathAlpha(new Color(0, 0, 0, 0)),
            0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
      }

      this.spriteBatch.Draw(Game1.cursorTexture, new Vector2((float) (
          Game1.mouseState[0].Position.X + 1),
          (float) (Game1.mouseState[0].Position.Y + 1)), 
          new Rectangle?(new Rectangle(0, 0, Game1.cursorTexture.Width, 
          Game1.cursorTexture.Height)), 
          new Color((int) ((double) Game1.cursorColor.R * 0.20000000298023224), 
          (int) ((double) Game1.cursorColor.G * 0.20000000298023224), 
          (int) ((double) Game1.cursorColor.B * 0.20000000298023224),
          (int) ((double) Game1.cursorColor.A * 0.5)), 0.0f, 
          new Vector2(), Game1.cursorScale * 1.1f, SpriteEffects.None, 0.0f);

      this.spriteBatch.Draw(Game1.cursorTexture, 
          new Vector2((float) Game1.mouseState[0].Position.X, 
          (float) Game1.mouseState[0].Position.Y), 
          new Rectangle?(new Rectangle(0, 0, Game1.cursorTexture.Width, Game1.cursorTexture.Height)), 
          Game1.cursorColor, 0.0f, new Vector2(), Game1.cursorScale, SpriteEffects.None, 0.0f);
      if (Game1.mouseItem.type > 0 && Game1.mouseItem.stack > 0)
      {
        Game1.player[Game1.myPlayer].showItemIcon = false;
        Game1.player[Game1.myPlayer].showItemIcon2 = 0;
        flag1 = true;
        float num65 = 1f;
        if (Game1.itemTexture[Game1.mouseItem.type].Width > 32 || 
                    Game1.itemTexture[Game1.mouseItem.type].Height > 32)
          num65 = Game1.itemTexture[Game1.mouseItem.type].Width <= 
                        Game1.itemTexture[Game1.mouseItem.type].Height 
                        ? 32f / (float) Game1.itemTexture[Game1.mouseItem.type].Height : 32f 
                        / (float) Game1.itemTexture[Game1.mouseItem.type].Width;
        float num66 = 1f;
        Color white = Color.White;
        float scale = num65 * num66;

        this.spriteBatch.Draw(Game1.itemTexture[Game1.mouseItem.type], 
            new Vector2((float) ((double) Game1.mouseState[0].Position.X + 26.0 * (double) num66 - 
            (double) Game1.itemTexture[Game1.mouseItem.type].Width * 0.5 * (double) scale),
            (float) ((double) Game1.mouseState[0].Position.Y + 26.0 * (double) num66 - 
            (double) Game1.itemTexture[Game1.mouseItem.type].Height * 0.5 * (double) scale)), 
            new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.mouseItem.type].Width,
            Game1.itemTexture[Game1.mouseItem.type].Height)), Game1.mouseItem.GetAlpha(white), 0.0f, 
            new Vector2(), scale, SpriteEffects.None, 0.0f);

        if (Game1.mouseItem.color != new Color())
          this.spriteBatch.Draw(Game1.itemTexture[Game1.mouseItem.type], new Vector2((float) (
              (double) Game1.mouseState[0].Position.X + 26.0 * (double) num66 -
              (double) Game1.itemTexture[Game1.mouseItem.type].Width * 0.5 * (double) scale), 
              (float) ((double) Game1.mouseState[0].Position.Y + 26.0 * (double) num66 - 
              (double) Game1.itemTexture[Game1.mouseItem.type].Height * 0.5 * (double) scale)), 
              new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.mouseItem.type].Width, 
              Game1.itemTexture[Game1.mouseItem.type].Height)), Game1.mouseItem.GetColor(white), 0.0f, 
              new Vector2(), scale, SpriteEffects.None, 0.0f);

        if (Game1.mouseItem.stack > 1)
          this.spriteBatch.DrawString(Game1.fontItemStack, string.Concat((object) Game1.mouseItem.stack), 
              new Vector2((float) Game1.mouseState[0].Position.X + 10f * num66, 
              (float) Game1.mouseState[0].Position.Y + 26f * num66),
              white, 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
      }
      Rectangle rectangle5 = new Rectangle((int) ((double) Game1.mouseState[0].Position.X + 
          (double) Game1.screenPosition.X), (int) ((double) Game1.mouseState[0].Position.Y +
          (double) Game1.screenPosition.Y), 1, 1);
      if (!flag1)
      {
        int num67 = 26 * Game1.player[Game1.myPlayer].statLifeMax / num11;
        int num68 = 0;
        if (Game1.player[Game1.myPlayer].statLifeMax > 200)
        {
          num67 = 260;
          num68 += 26;
        }
        if (Game1.mouseState[0].Position.X > 500 
                    && Game1.mouseState[0].Position.X < 500 + num67 
                    && Game1.mouseState[0].Position.Y > 32 
                    && Game1.mouseState[0].Position.Y < 32 + Game1.heartTexture.Height + num68)
        {
          Game1.player[Game1.myPlayer].showItemIcon = false;
          this.MouseText(Game1.player[Game1.myPlayer].statLife.ToString() 
              + "/" + (object) Game1.player[Game1.myPlayer].statLifeMax);
          flag1 = true;
        }
      }
      if (!flag1)
      {
        int num69 = 24;
        int num70 = 28 * Game1.player[Game1.myPlayer].statManaMax / num16;

        if (Game1.mouseState[0].Position.X > 762 
                    && Game1.mouseState[0].Position.X < 762 + 
                    num69 && Game1.mouseState[0].Position.Y > 30 
                    && Game1.mouseState[0].Position.Y < 30 + num70)
        {
          Game1.player[Game1.myPlayer].showItemIcon = false;
          this.MouseText(Game1.player[Game1.myPlayer].statMana.ToString() 
              + "/" + (object) Game1.player[Game1.myPlayer].statManaMax);
          flag1 = true;
        }
      }
      if (!flag1)
      {
        for (int index = 0; index < 200; ++index)
        {
          if (Game1.item[index].active)
          {
            Rectangle rectangle6 = new Rectangle((int) (
                (double) Game1.item[index].position.X + 
                (double) Game1.item[index].width * 0.5 
                - (double) Game1.itemTexture[Game1.item[index].type].Width * 0.5), 
                (int) ((double) Game1.item[index].position.Y 
                + (double) Game1.item[index].height - 
                (double) Game1.itemTexture[Game1.item[index].type].Height), 
                Game1.itemTexture[Game1.item[index].type].Width,
                Game1.itemTexture[Game1.item[index].type].Height);

            if (rectangle5.Intersects(rectangle6))
            {
              Game1.player[Game1.myPlayer].showItemIcon = false;
              string cursorText2 = Game1.item[index].name;
              if (Game1.item[index].stack > 1)
                cursorText2 = cursorText2 + " (" + (object) Game1.item[index].stack + ")";
              if (Game1.item[index].owner < 8 && Game1.showItemOwner)
                cursorText2 = cursorText2 + " <" + Game1.player[Game1.item[index].owner].name + ">";
              int rare2 = Game1.item[index].rare;
              this.MouseText(cursorText2, rare2);
              flag1 = true;
              break;
            }
          }
        }
      }
      for (int index = 0; index < 8; ++index)
      {
        if (Game1.player[index].active && Game1.myPlayer != index && !Game1.player[index].dead)
        {
          rectangle1 = new Rectangle((int) ((double) Game1.player[index].position.X + (double) Game1.player[index].width * 0.5 - 16.0), (int) ((double) Game1.player[index].position.Y + (double) Game1.player[index].height - 48.0), 32, 48);
          if (!flag1 && rectangle5.Intersects(rectangle1))
          {
            Game1.player[Game1.myPlayer].showItemIcon = false;
            string cursorText3 = Game1.player[index].name + ": " + (object) Game1.player[index].statLife + "/" + (object) Game1.player[index].statLifeMax;
            if (Game1.player[index].hostile)
              cursorText3 += " (PvP)";
            this.MouseText(cursorText3);
          }
        }
      }
      if (!flag1)
      {
        for (int index = 0; index < 1000; ++index)
        {
          if (Game1.npc[index].active)
          {
            Rectangle rectangle7 = new Rectangle((int) ((double) Game1.npc[index].position.X + (double) Game1.npc[index].width * 0.5 - (double) Game1.npcTexture[Game1.npc[index].type].Width * 0.5), (int) ((double) Game1.npc[index].position.Y + (double) Game1.npc[index].height - (double) (Game1.npcTexture[Game1.npc[index].type].Height / Game1.npcFrameCount[Game1.npc[index].type])), Game1.npcTexture[Game1.npc[index].type].Width, Game1.npcTexture[Game1.npc[index].type].Height / Game1.npcFrameCount[Game1.npc[index].type]);
            if (rectangle5.Intersects(rectangle7))
            {
              bool flag4 = false;
              if (Game1.npc[index].townNPC && new Rectangle((int) ((double) Game1.player[Game1.myPlayer].position.X + (double) (Game1.player[Game1.myPlayer].width / 2) - (double) (Player.tileRangeX * 16)), (int) ((double) Game1.player[Game1.myPlayer].position.Y + (double) (Game1.player[Game1.myPlayer].height / 2) - (double) (Player.tileRangeY * 16)), Player.tileRangeX * 16 * 2, Player.tileRangeY * 16 * 2).Intersects(new Rectangle((int) Game1.npc[index].position.X, (int) Game1.npc[index].position.Y, Game1.npc[index].width, Game1.npc[index].height)))
                flag4 = true;
              if (flag4)
              {
                int num71 = -(Game1.npc[index].width / 2 + 8);
                SpriteEffects effects = SpriteEffects.None;
                if (Game1.npc[index].spriteDirection == -1)
                {
                  effects = SpriteEffects.FlipHorizontally;
                  num71 = Game1.npc[index].width / 2 + 8;
                }
                this.spriteBatch.Draw(Game1.chatTexture, 
                    new Vector2(Game1.npc[index].position.X + (float) (Game1.npc[index].width / 2) 
                    - Game1.screenPosition.X - (float) (Game1.chatTexture.Width / 2) 
                    - (float) num71, Game1.npc[index].position.Y -
                    (float) Game1.chatTexture.Height - Game1.screenPosition.Y), 
                    new Rectangle?(new Rectangle(0, 0, Game1.chatTexture.Width, Game1.chatTexture.Height)),
                    new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, 
                    (int) Game1.mouseTextColor, (int) Game1.mouseTextColor), 0.0f, 
                    new Vector2(), 1f, effects, 0.0f);

                if (Game1.mouseState.Count > 1 //Game1.mouseState.RightButton == ButtonState.Pressed 
                                    && Game1.npcChatRelease)
                {
                  Game1.npcChatRelease = false;
                  if (Game1.player[Game1.myPlayer].talkNPC != index)
                  {
                    Game1.player[Game1.myPlayer].sign = -1;
                    Game1.editSign = false;
                    Game1.player[Game1.myPlayer].talkNPC = index;
                    Game1.playerInventory = false;
                    Game1.player[Game1.myPlayer].chest = -1;
                    Game1.npcChatText = Game1.npc[index].GetChat();
                    Game1.PlaySound(10);
                  }
                }
              }
              Game1.player[Game1.myPlayer].showItemIcon = false;
              this.MouseText(Game1.npc[index].name + ": " + (object) Game1.npc[index].life + "/" + (object) Game1.npc[index].lifeMax);
              break;
            }
          }
        }
      }

      Game1.npcChatRelease = (Game1.mouseState.Count < 2);//Game1.mouseState.RightButton != ButtonState.Pressed;

      if (Game1.player[Game1.myPlayer].showItemIcon 
                && (Game1.player[Game1.myPlayer].inventory[Game1.player[Game1.myPlayer].selectedItem].type > 0 
                || Game1.player[Game1.myPlayer].showItemIcon2 > 0))
      {
        int index = Game1.player[Game1.myPlayer].inventory[Game1.player[Game1.myPlayer].selectedItem].type;
        Color color8 = Game1.player[Game1.myPlayer].inventory[Game1.player[Game1.myPlayer].selectedItem].GetAlpha(Color.White);
        Color color9 = Game1.player[Game1.myPlayer].inventory[Game1.player[Game1.myPlayer].selectedItem].GetColor(Color.White);
        if (Game1.player[Game1.myPlayer].showItemIcon2 > 0)
        {
          index = Game1.player[Game1.myPlayer].showItemIcon2;
          color8 = Color.White;
          color9 = new Color();
        }
        this.spriteBatch.Draw(Game1.itemTexture[index], 
            new Vector2((float) (Game1.mouseState[0].Position.X + 10),
            (float) (Game1.mouseState[0].Position.Y + 10)), 
            new Rectangle?(
                new Rectangle(0, 0, Game1.itemTexture[index].Width, Game1.itemTexture[index].Height)),
            color8, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
        if (Game1.player[Game1.myPlayer].showItemIcon2 == 0 && Game1.player[Game1.myPlayer].inventory[Game1.player[Game1.myPlayer].selectedItem].color != new Color())
          this.spriteBatch.Draw(Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[Game1.player[Game1.myPlayer].selectedItem].type], 
              new Vector2((float) (Game1.mouseState[0].Position.X + 10), 
              (float) (Game1.mouseState[0].Position.Y + 10)), 
              new Rectangle?(new Rectangle(0, 0,
              Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[Game1.player[Game1.myPlayer].selectedItem].type].Width, Game1.itemTexture[Game1.player[Game1.myPlayer].inventory[Game1.player[Game1.myPlayer].selectedItem].type].Height)), color9, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
      }
      Game1.player[Game1.myPlayer].showItemIcon = false;
      Game1.player[Game1.myPlayer].showItemIcon2 = 0;
    }

    protected void DrawMenu()
    {
      Game1.evilTiles = 0;
      Game1.chatMode = false;
      for (int index = 0; index < Game1.numChatLines; ++index)
        Game1.chatLine[index] = new ChatLine();
      this.DrawFPS();
      Game1.screenPosition.Y = (float) (Game1.worldSurface * 16.0) - (float) Game1.screenHeight;
      Game1.background = 0;
      byte num1 = (byte) (((int) byte.MaxValue + (int) Game1.tileColor.R * 2) / 3);
      Color color1 = new Color((int) num1, (int) num1, (int) num1, (int) byte.MaxValue);
      this.logoRotation += this.logoRotationSpeed * 3E-05f;
      if ((double) this.logoRotation > 0.1)
        this.logoRotationDirection = -1f;
      else if ((double) this.logoRotation < -0.1)
        this.logoRotationDirection = 1f;
      if ((double) this.logoRotationSpeed < 20.0 & (double) this.logoRotationDirection == 1.0)
        ++this.logoRotationSpeed;
      else if ((double) this.logoRotationSpeed > -20.0 & (double) this.logoRotationDirection == -1.0)
        --this.logoRotationSpeed;
      this.logoScale += this.logoScaleSpeed * 1E-05f;
      if ((double) this.logoScale > 1.1)
        this.logoScaleDirection = -1f;
      else if ((double) this.logoScale < 0.9)
        this.logoScaleDirection = 1f;
      if ((double) this.logoScaleSpeed < 50.0 & (double) this.logoScaleDirection == 1.0)
        ++this.logoScaleSpeed;
      else if ((double) this.logoScaleSpeed > -50.0 & (double) this.logoScaleDirection == -1.0)
        --this.logoScaleSpeed;
      this.spriteBatch.Draw(Game1.logoTexture, new Vector2((float) (Game1.screenWidth / 2), 100f), new Rectangle?(new Rectangle(0, 0, Game1.logoTexture.Width, Game1.logoTexture.Height)), color1, this.logoRotation, new Vector2((float) (Game1.logoTexture.Width / 2), (float) (Game1.logoTexture.Height / 2)), this.logoScale, SpriteEffects.None, 0.0f);
      int num2 = 250;
      int num3 = Game1.screenWidth / 2;
      int num4 = 80;
      int num5 = 0;
      int menuMode = Game1.menuMode;
      int index1 = -1;
      int num6 = 0;
      int num7 = 0;
      bool flag1 = false;
      bool flag2 = false;
      int num8 = 0;
      bool[] flagArray1 = new bool[Game1.maxMenuItems];
      bool[] flagArray2 = new bool[Game1.maxMenuItems];
      int[] numArray1 = new int[Game1.maxMenuItems];
      int[] numArray2 = new int[Game1.maxMenuItems];
      for (int index2 = 0; index2 < Game1.maxMenuItems; ++index2)
      {
        flagArray1[index2] = false;
        flagArray2[index2] = false;
        numArray1[index2] = 0;
        numArray2[index2] = 0;
      }
      string[] strArray1 = new string[Game1.maxMenuItems];
      if (Game1.menuMode == -1)
        Game1.menuMode = 0;
      if (Game1.netMode == 2)
      {
        for (int index3 = 0; index3 < 9; ++index3)
        {
          try
          {
            strArray1[index3] = Netplay.serverSock[index3].statusText;
            if (Netplay.serverSock[index3].active && Game1.showSpam)
            {
              string[] strArray2 = default;
              int index4 = 0;
              //RnD
              string str = "tttt";//(strArray2 = strArray1)[(IntPtr) (index4 = index3)] + " (" + (object) NetMessage.buffer[index3].spamCount + ")";
              strArray2[index4] = str;
            }
          }
          catch
          {
            strArray1[index3] = "";
          }
          flagArray1[index3] = true;
        }
        num5 = 11;
        strArray1[9] = Game1.statusText;
        flagArray1[9] = true;
        num2 = 170;
        num4 = 30;
        numArray1[9] = 20;
        numArray1[10] = 40;
        strArray1[10] = "Cancel";
        if (this.selectedMenu == 10)
        {
          Netplay.disconnect = true;
          Game1.PlaySound(11);
        }
      }
      else if (Game1.menuMode == 31)
      {
        string password = Netplay.password;
        Netplay.password = Game1.GetInputText(Netplay.password);
        if (password != Netplay.password)
          Game1.PlaySound(12);
        strArray1[0] = "Server Requires Password:";
        ++this.textBlinkerCount;
        if (this.textBlinkerCount >= 20)
        {
          this.textBlinkerState = this.textBlinkerState != 0 ? 0 : 1;
          this.textBlinkerCount = 0;
        }
        strArray1[1] = Netplay.password;
        if (this.textBlinkerState == 1)
        {
          string[] strArray3;
          (strArray3 = strArray1)[1] = strArray3[1] + "|";
          numArray2[1] = 1;
        }
        else
        {
          string[] strArray4;
          (strArray4 = strArray1)[1] = strArray4[1] + " ";
        }
        flagArray1[0] = true;
        flagArray1[1] = true;
        numArray1[1] = -20;
        numArray1[2] = 20;
        strArray1[2] = "Accept";
        strArray1[3] = "Back";
        num5 = 4;
        if (this.selectedMenu == 3)
        {
          Game1.PlaySound(11);
          Game1.menuMode = 0;
          Netplay.disconnect = true;
          Netplay.password = "";
        }
        else if (this.selectedMenu == 2 || Game1.inputTextEnter)
        {
          NetMessage.SendData(38, text: Netplay.password);
          Game1.menuMode = 14;
        }
      }
      else if (Game1.netMode == 1 || Game1.menuMode == 14)
      {
        num5 = 2;
        strArray1[0] = Game1.statusText;
        flagArray1[0] = true;
        num2 = 300;
        strArray1[1] = "Cancel";
        if (this.selectedMenu == 1)
        {
          Netplay.disconnect = true;
          //RnD
          Netplay.clientSock.tcpClient.Dispose();//.Close();
          Game1.PlaySound(11);
          Game1.menuMode = 0;
          Game1.netMode = 0;
        }
      }
      else
      {
        switch (Game1.menuMode)
        {
          case 0:
            Game1.menuMultiplayer = false;
            Game1.netMode = 0;
            strArray1[0] = "Single Player";
            strArray1[1] = "Multiplayer";
            strArray1[2] = "Settings";
            strArray1[3] = "Exit";
            num5 = 4;
            if (this.selectedMenu == 3)
              this.Exit();
            if (this.selectedMenu == 1)
            {
              Game1.PlaySound(10);
              Game1.menuMode = 12;
            }
            if (this.selectedMenu == 2)
            {
              Game1.PlaySound(10);
              Game1.menuMode = 11;
            }
            if (this.selectedMenu == 0)
            {
              Game1.PlaySound(10);
              Game1.menuMode = 1;
              Game1.LoadPlayers();
              break;
            }
            break;
          case 1:
            num2 = 190;
            num4 = 50;
            strArray1[5] = "Create Character";
            strArray1[6] = "Delete";
            switch (Game1.numLoadPlayers)
            {
              case 0:
                flagArray2[6] = true;
                strArray1[6] = "";
                break;
              case 5:
                flagArray2[5] = true;
                strArray1[5] = "";
                break;
            }
            strArray1[7] = "Back";
            for (int index5 = 0; index5 < 5; ++index5)
              strArray1[index5] = index5 >= Game1.numLoadPlayers ? (string) null : Game1.loadPlayer[index5].name;
            num5 = 8;
            if (this.focusMenu >= 0 && this.focusMenu < Game1.numLoadPlayers)
            {
              index1 = this.focusMenu;
              Vector2 vector2 = Game1.fontDeathText.MeasureString(strArray1[index1]);
              num6 = (int) ((double) (Game1.screenWidth / 2) + (double) vector2.X * 0.5 + 10.0);
              num7 = num2 + num4 * this.focusMenu + 4;
            }
            if (this.selectedMenu == 7)
            {
              Game1.PlaySound(11);
              Game1.menuMode = !Game1.menuMultiplayer ? 0 : 12;
              Game1.menuMultiplayer = false;
              break;
            }
            if (this.selectedMenu == 5)
            {
              Game1.loadPlayer[Game1.numLoadPlayers] = new Player();
              Game1.PlaySound(10);
              Game1.menuMode = 2;
              break;
            }
            if (this.selectedMenu == 6)
            {
              Game1.PlaySound(10);
              Game1.menuMode = 4;
              break;
            }
            if (this.selectedMenu >= 0)
            {
              if (Game1.menuMultiplayer)
              {
                this.selectedPlayer = this.selectedMenu;
                Game1.player[Game1.myPlayer] = (Player) Game1.loadPlayer[this.selectedPlayer].Clone();
                Game1.playerPathName = Game1.loadPlayerPath[this.selectedPlayer];
                Game1.PlaySound(10);
                Game1.menuMode = 13;
              }
              else
              {
                Game1.myPlayer = 0;
                this.selectedPlayer = this.selectedMenu;
                Game1.player[Game1.myPlayer] = (Player) Game1.loadPlayer[this.selectedPlayer].Clone();
                Game1.playerPathName = Game1.loadPlayerPath[this.selectedPlayer];
                Game1.LoadWorlds();
                Game1.PlaySound(10);
                Game1.menuMode = 6;
              }
              break;
            }
            break;
          case 2:
            if (this.selectedMenu == 0)
            {
              Game1.menuMode = 17;
              Game1.PlaySound(10);
              this.selColor = Game1.loadPlayer[Game1.numLoadPlayers].hairColor;
            }
            if (this.selectedMenu == 1)
            {
              Game1.menuMode = 18;
              Game1.PlaySound(10);
              this.selColor = Game1.loadPlayer[Game1.numLoadPlayers].eyeColor;
            }
            if (this.selectedMenu == 2)
            {
              Game1.menuMode = 19;
              Game1.PlaySound(10);
              this.selColor = Game1.loadPlayer[Game1.numLoadPlayers].skinColor;
            }
            if (this.selectedMenu == 3)
            {
              Game1.menuMode = 20;
              Game1.PlaySound(10);
            }
            strArray1[0] = "Hair";
            strArray1[1] = "Eyes";
            strArray1[2] = "Skin";
            strArray1[3] = "Clothes";
            num2 = 260;
            num4 = 50;
            numArray1[4] = 20;
            numArray1[5] = 20;
            strArray1[4] = "Create";
            strArray1[5] = "Back";
            num5 = 6;
            index1 = Game1.numLoadPlayers;
            num6 = Game1.screenWidth / 2 - 16;
            num7 = 210;
            if (this.selectedMenu == 5)
            {
              Game1.PlaySound(11);
              Game1.menuMode = 1;
              break;
            }
            if (this.selectedMenu == 4)
            {
              Game1.PlaySound(10);
              Game1.loadPlayer[Game1.numLoadPlayers].name = "";
              Game1.menuMode = 3;
              break;
            }
            break;
          case 3:
            string name = Game1.loadPlayer[Game1.numLoadPlayers].name;
            Game1.loadPlayer[Game1.numLoadPlayers].name = Game1.GetInputText(Game1.loadPlayer[Game1.numLoadPlayers].name);
            if (Game1.loadPlayer[Game1.numLoadPlayers].name.Length > 20)
              Game1.loadPlayer[Game1.numLoadPlayers].name = Game1.loadPlayer[Game1.numLoadPlayers].name.Substring(0, 20);
            if (name != Game1.loadPlayer[Game1.numLoadPlayers].name)
              Game1.PlaySound(12);
            strArray1[0] = "Enter Character Name:";
            flagArray2[2] = true;
            if (Game1.loadPlayer[Game1.numLoadPlayers].name != "")
            {
              if (Game1.loadPlayer[Game1.numLoadPlayers].name.Substring(0, 1) == " ")
                Game1.loadPlayer[Game1.numLoadPlayers].name = "";
              for (int startIndex = 0; startIndex < Game1.loadPlayer[Game1.numLoadPlayers].name.Length; ++startIndex)
              {
                if (Game1.loadPlayer[Game1.numLoadPlayers].name.Substring(startIndex, 1) != " ")
                  flagArray2[2] = false;
              }
            }
            ++this.textBlinkerCount;
            if (this.textBlinkerCount >= 20)
            {
              this.textBlinkerState = this.textBlinkerState != 0 ? 0 : 1;
              this.textBlinkerCount = 0;
            }
            strArray1[1] = Game1.loadPlayer[Game1.numLoadPlayers].name;
            if (this.textBlinkerState == 1)
            {
              string[] strArray5;
              (strArray5 = strArray1)[1] = strArray5[1] + "|";
              numArray2[1] = 1;
            }
            else
            {
              string[] strArray6;
              (strArray6 = strArray1)[1] = strArray6[1] + " ";
            }
            flagArray1[0] = true;
            flagArray1[1] = true;
            numArray1[1] = -20;
            numArray1[2] = 20;
            strArray1[2] = "Accept";
            strArray1[3] = "Back";
            num5 = 4;
            if (this.selectedMenu == 3)
            {
              Game1.PlaySound(11);
              Game1.menuMode = 2;
            }
            if (this.selectedMenu == 2 || !flagArray2[2] && Game1.inputTextEnter)
            {
              Game1.loadPlayerPath[Game1.numLoadPlayers] = Game1.nextLoadPlayer();
              Player.SavePlayer(Game1.loadPlayer[Game1.numLoadPlayers], Game1.loadPlayerPath[Game1.numLoadPlayers]);
              Game1.LoadPlayers();
              Game1.PlaySound(10);
              Game1.menuMode = 1;
              break;
            }
            break;
          case 4:
            num2 = 220;
            num4 = 60;
            strArray1[5] = "Back";
            for (int index6 = 0; index6 < 5; ++index6)
              strArray1[index6] = index6 >= Game1.numLoadPlayers ? (string) null : Game1.loadPlayer[index6].name;
            num5 = 6;
            if (this.focusMenu >= 0 && this.focusMenu < Game1.numLoadPlayers)
            {
              index1 = this.focusMenu;
              Vector2 vector2 = Game1.fontDeathText.MeasureString(strArray1[index1]);
              num6 = (int) ((double) (Game1.screenWidth / 2) + (double) vector2.X * 0.5 + 10.0);
              num7 = num2 + num4 * this.focusMenu + 4;
            }
            if (this.selectedMenu == 5)
            {
              Game1.PlaySound(11);
              Game1.menuMode = 1;
              break;
            }
            if (this.selectedMenu >= 0)
            {
              this.selectedPlayer = this.selectedMenu;
              Game1.PlaySound(10);
              Game1.menuMode = 5;
              break;
            }
            break;
          case 5:
            strArray1[0] = "Delete " + Game1.loadPlayer[this.selectedPlayer].name + "?";
            flagArray1[0] = true;
            strArray1[1] = "Yes";
            strArray1[2] = "No";
            num5 = 3;
            if (this.selectedMenu == 1)
            {
              Game1.ErasePlayer(this.selectedPlayer);
              Game1.PlaySound(10);
              Game1.menuMode = 1;
              break;
            }
            if (this.selectedMenu == 2)
            {
              Game1.PlaySound(11);
              Game1.menuMode = 1;
              break;
            }
            break;
          case 6:
            num2 = 190;
            num4 = 50;
            strArray1[5] = "Create World";
            strArray1[6] = "Delete";
            switch (Game1.numLoadWorlds)
            {
              case 0:
                flagArray2[6] = true;
                strArray1[6] = "";
                break;
              case 5:
                flagArray2[5] = true;
                strArray1[5] = "";
                break;
            }
            strArray1[7] = "Back";
            for (int index7 = 0; index7 < 5; ++index7)
              strArray1[index7] = index7 >= Game1.numLoadWorlds ? (string) null : Game1.loadWorld[index7];
            num5 = 8;
            if (this.selectedMenu == 7)
            {
              Game1.menuMode = !Game1.menuMultiplayer ? 1 : 12;
              Game1.PlaySound(11);
              break;
            }
            if (this.selectedMenu == 5)
            {
              Game1.PlaySound(10);
              Game1.menuMode = 16;
              Game1.newWorldName = "World " + (object) (Game1.numLoadWorlds + 1);
              break;
            }
            if (this.selectedMenu == 6)
            {
              Game1.PlaySound(10);
              Game1.menuMode = 8;
              break;
            }
            if (this.selectedMenu >= 0)
            {
              if (Game1.menuMultiplayer)
              {
                Game1.PlaySound(10);
                Game1.worldPathName = Game1.loadWorldPath[this.selectedMenu];
                Game1.menuMode = 30;
              }
              else
              {
                Game1.PlaySound(10);
                Game1.worldPathName = Game1.loadWorldPath[this.selectedMenu];
                WorldGen.playWorld();
                Game1.menuMode = 10;
              }
              break;
            }
            break;
          case 7:
            string newWorldName = Game1.newWorldName;
            Game1.newWorldName = Game1.GetInputText(Game1.newWorldName);
            if (Game1.newWorldName.Length > 20)
              Game1.newWorldName = Game1.newWorldName.Substring(0, 20);
            if (newWorldName != Game1.newWorldName)
              Game1.PlaySound(12);
            strArray1[0] = "Enter World Name:";
            flagArray2[2] = true;
            if (Game1.newWorldName != "")
            {
              if (Game1.newWorldName.Substring(0, 1) == " ")
                Game1.newWorldName = "";
              for (int index8 = 0; index8 < Game1.newWorldName.Length; ++index8)
              {
                if (Game1.newWorldName != " ")
                  flagArray2[2] = false;
              }
            }
            ++this.textBlinkerCount;
            if (this.textBlinkerCount >= 20)
            {
              this.textBlinkerState = this.textBlinkerState != 0 ? 0 : 1;
              this.textBlinkerCount = 0;
            }
            strArray1[1] = Game1.newWorldName;
            if (this.textBlinkerState == 1)
            {
              string[] strArray7;
              (strArray7 = strArray1)[1] = strArray7[1] + "|";
              numArray2[1] = 1;
            }
            else
            {
              string[] strArray8;
              (strArray8 = strArray1)[1] = strArray8[1] + " ";
            }
            flagArray1[0] = true;
            flagArray1[1] = true;
            numArray1[1] = -20;
            numArray1[2] = 20;
            strArray1[2] = "Accept";
            strArray1[3] = "Back";
            num5 = 4;
            if (this.selectedMenu == 3)
            {
              Game1.PlaySound(11);
              Game1.menuMode = 16;
            }
            if (this.selectedMenu == 2 || !flagArray2[2] && Game1.inputTextEnter)
            {
              Game1.menuMode = 10;
              Game1.worldName = Game1.newWorldName;
              Game1.worldPathName = Game1.nextLoadWorld();
              WorldGen.CreateNewWorld();
              break;
            }
            break;
          case 8:
            num2 = 220;
            num4 = 60;
            strArray1[5] = "Back";
            for (int index9 = 0; index9 < 5; ++index9)
              strArray1[index9] = index9 >= Game1.numLoadWorlds ? (string) null : Game1.loadWorld[index9];
            num5 = 6;
            if (this.selectedMenu == 5)
            {
              Game1.PlaySound(11);
              Game1.menuMode = 1;
              break;
            }
            if (this.selectedMenu >= 0)
            {
              this.selectedWorld = this.selectedMenu;
              Game1.PlaySound(10);
              Game1.menuMode = 9;
              break;
            }
            break;
          case 9:
            strArray1[0] = "Delete " + Game1.loadWorld[this.selectedWorld] + "?";
            flagArray1[0] = true;
            strArray1[1] = "Yes";
            strArray1[2] = "No";
            num5 = 3;
            if (this.selectedMenu == 1)
            {
              Game1.EraseWorld(this.selectedWorld);
              Game1.PlaySound(10);
              Game1.menuMode = 6;
              break;
            }
            if (this.selectedMenu == 2)
            {
              Game1.PlaySound(11);
              Game1.menuMode = 6;
              break;
            }
            break;
          case 10:
            num5 = 1;
            strArray1[0] = Game1.statusText;
            flagArray1[0] = true;
            num2 = 300;
            break;
          case 11:
            num5 = 4;
            strArray1[0] = !this.graphics.IsFullScreen ? "Windowed Mode" : "Fullscreen Mode";
            strArray1[1] = "Cursor Color";
            strArray1[2] = "Volume";
            strArray1[3] = "Back";
            if (this.selectedMenu == 3)
            {
              Game1.PlaySound(11);
              this.SaveSettings();
              Game1.menuMode = 0;
            }
            if (this.selectedMenu == 2)
            {
              Game1.PlaySound(11);
              Game1.menuMode = 26;
            }
            if (this.selectedMenu == 1)
            {
              Game1.PlaySound(10);
              this.selColor = Game1.mouseColor;
              Game1.menuMode = 25;
            }
            if (this.selectedMenu == 0)
            {
              this.graphics.ToggleFullScreen();
              break;
            }
            break;
          case 12:
            strArray1[0] = "Join";
            strArray1[1] = "Start Server";
            strArray1[2] = "Back";
            if (this.selectedMenu == 0)
            {
              Game1.LoadPlayers();
              Game1.menuMultiplayer = true;
              Game1.PlaySound(10);
              Game1.menuMode = 1;
            }
            else if (this.selectedMenu == 1)
            {
              Game1.LoadWorlds();
              Game1.PlaySound(10);
              Game1.menuMode = 6;
              Game1.menuMultiplayer = true;
            }
            if (this.selectedMenu == 2)
            {
              Game1.PlaySound(11);
              Game1.menuMode = 0;
            }
            num5 = 3;
            break;
          case 13:
            string getIp = Game1.getIP;
            Game1.getIP = Game1.GetInputText(Game1.getIP);
            if (getIp != Game1.getIP)
              Game1.PlaySound(12);
            strArray1[0] = "Enter Server IP Address:";
            flagArray2[2] = true;
            if (Game1.getIP != "")
            {
              if (Game1.getIP.Substring(0, 1) == " ")
                Game1.getIP = "";
              for (int index10 = 0; index10 < Game1.getIP.Length; ++index10)
              {
                if (Game1.getIP != " ")
                  flagArray2[2] = false;
              }
            }
            ++this.textBlinkerCount;
            if (this.textBlinkerCount >= 20)
            {
              this.textBlinkerState = this.textBlinkerState != 0 ? 0 : 1;
              this.textBlinkerCount = 0;
            }
            strArray1[1] = Game1.getIP;
            if (this.textBlinkerState == 1)
            {
              string[] strArray9;
              (strArray9 = strArray1)[1] = strArray9[1] + "|";
              numArray2[1] = 1;
            }
            else
            {
              string[] strArray10;
              (strArray10 = strArray1)[1] = strArray10[1] + " ";
            }
            flagArray1[0] = true;
            flagArray1[1] = true;
            numArray1[1] = -20;
            numArray1[2] = 20;
            strArray1[2] = "Accept";
            strArray1[3] = "Back";
            num5 = 4;
            if (this.selectedMenu == 3)
            {
              Game1.PlaySound(11);
              Game1.menuMode = 1;
            }
            if (this.selectedMenu == 2 || !flagArray2[2] && Game1.inputTextEnter)
            {
              Game1.menuMode = 10;
              Netplay.SetIP(Game1.getIP);
              Netplay.StartClient();
              break;
            }
            break;
          case 15:
            num5 = 2;
            strArray1[0] = Game1.statusText;
            flagArray1[0] = true;
            num2 = 80;
            num4 = 400;
            strArray1[1] = "Back";
            if (this.selectedMenu == 1)
            {
              Netplay.disconnect = true;
              Game1.PlaySound(11);
              Game1.menuMode = 0;
              Game1.netMode = 0;
              break;
            }
            break;
          case 16:
            num2 = 200;
            num4 = 60;
            numArray1[1] = 30;
            numArray1[2] = 30;
            numArray1[3] = 30;
            numArray1[4] = 70;
            strArray1[0] = "Choose world size:";
            flagArray1[0] = true;
            strArray1[1] = "Small (200MB)";
            strArray1[2] = "Medium (300MB)";
            strArray1[3] = "Large (500MB)";
            strArray1[4] = "Back";
            num5 = 5;
            if (this.selectedMenu == 4)
            {
              Game1.menuMode = 6;
              Game1.PlaySound(11);
            }
            else if (this.selectedMenu > 0)
            {
              if (this.selectedMenu == 1)
              {
                Game1.maxTilesX = 4200;
                Game1.maxTilesY = 1200;
              }
              else if (this.selectedMenu == 2)
              {
                Game1.maxTilesX = 6300;
                Game1.maxTilesY = 1800;
              }
              else
              {
                Game1.maxTilesX = 8400;
                Game1.maxTilesY = 2400;
              }
              Game1.menuMode = 7;
              Game1.PlaySound(10);
              WorldGen.setWorldSize();
            }
            break;
          case 17:
            index1 = Game1.numLoadPlayers;
            num6 = Game1.screenWidth / 2 - 16;
            num7 = 210;
            flag1 = true;
            num8 = 390;
            num2 = 260;
            num4 = 60;
            Game1.loadPlayer[index1].hairColor = this.selColor;
            num5 = 3;
            strArray1[0] = "Hair " + (object) (Game1.loadPlayer[index1].hair + 1);
            strArray1[1] = "Hair Color";
            flagArray1[1] = true;
            numArray1[2] = 150;
            numArray1[1] = 10;
            strArray1[2] = "Back";
            if (this.selectedMenu == 0)
            {
              Game1.PlaySound(12);
              ++Game1.loadPlayer[index1].hair;
              if (Game1.loadPlayer[index1].hair >= Game1.maxHair)
                Game1.loadPlayer[index1].hair = 0;
            }
            if (this.selectedMenu == 2)
            {
              Game1.menuMode = 2;
              Game1.PlaySound(11);
              break;
            }
            break;
          case 18:
            index1 = Game1.numLoadPlayers;
            num6 = Game1.screenWidth / 2 - 16;
            num7 = 210;
            flag1 = true;
            num8 = 370;
            num2 = 240;
            num4 = 60;
            Game1.loadPlayer[index1].eyeColor = this.selColor;
            num5 = 3;
            strArray1[0] = "";
            strArray1[1] = "Eye Color";
            flagArray1[1] = true;
            numArray1[2] = 170;
            numArray1[1] = 10;
            strArray1[2] = "Back";
            if (this.selectedMenu == 2)
            {
              Game1.menuMode = 2;
              Game1.PlaySound(11);
              break;
            }
            break;
          case 19:
            index1 = Game1.numLoadPlayers;
            num6 = Game1.screenWidth / 2 - 16;
            num7 = 210;
            flag1 = true;
            num8 = 370;
            num2 = 240;
            num4 = 60;
            Game1.loadPlayer[index1].skinColor = this.selColor;
            num5 = 3;
            strArray1[0] = "";
            strArray1[1] = "Skin Color";
            flagArray1[1] = true;
            numArray1[2] = 170;
            numArray1[1] = 10;
            strArray1[2] = "Back";
            if (this.selectedMenu == 2)
            {
              Game1.menuMode = 2;
              Game1.PlaySound(11);
              break;
            }
            break;
          case 20:
            if (this.selectedMenu == 0)
            {
              Game1.menuMode = 21;
              Game1.PlaySound(10);
              this.selColor = Game1.loadPlayer[Game1.numLoadPlayers].shirtColor;
            }
            if (this.selectedMenu == 1)
            {
              Game1.menuMode = 22;
              Game1.PlaySound(10);
              this.selColor = Game1.loadPlayer[Game1.numLoadPlayers].underShirtColor;
            }
            if (this.selectedMenu == 2)
            {
              Game1.menuMode = 23;
              Game1.PlaySound(10);
              this.selColor = Game1.loadPlayer[Game1.numLoadPlayers].pantsColor;
            }
            if (this.selectedMenu == 3)
            {
              this.selColor = Game1.loadPlayer[Game1.numLoadPlayers].shoeColor;
              Game1.menuMode = 24;
              Game1.PlaySound(10);
            }
            strArray1[0] = "Shirt";
            strArray1[1] = "Undershirt";
            strArray1[2] = "Pants";
            strArray1[3] = "Misc";
            num2 = 260;
            num4 = 50;
            numArray1[5] = 20;
            strArray1[5] = "Back";
            num5 = 6;
            index1 = Game1.numLoadPlayers;
            num6 = Game1.screenWidth / 2 - 16;
            num7 = 210;
            if (this.selectedMenu == 5)
            {
              Game1.PlaySound(11);
              Game1.menuMode = 2;
              break;
            }
            break;
          case 21:
            index1 = Game1.numLoadPlayers;
            num6 = Game1.screenWidth / 2 - 16;
            num7 = 210;
            flag1 = true;
            num8 = 370;
            num2 = 240;
            num4 = 60;
            Game1.loadPlayer[index1].shirtColor = this.selColor;
            num5 = 3;
            strArray1[0] = "";
            strArray1[1] = "Shirt Color";
            flagArray1[1] = true;
            numArray1[2] = 170;
            numArray1[1] = 10;
            strArray1[2] = "Back";
            if (this.selectedMenu == 2)
            {
              Game1.menuMode = 20;
              Game1.PlaySound(11);
              break;
            }
            break;
          case 22:
            index1 = Game1.numLoadPlayers;
            num6 = Game1.screenWidth / 2 - 16;
            num7 = 210;
            flag1 = true;
            num8 = 370;
            num2 = 240;
            num4 = 60;
            Game1.loadPlayer[index1].underShirtColor = this.selColor;
            num5 = 3;
            strArray1[0] = "";
            strArray1[1] = "Undershirt Color";
            flagArray1[1] = true;
            numArray1[2] = 170;
            numArray1[1] = 10;
            strArray1[2] = "Back";
            if (this.selectedMenu == 2)
            {
              Game1.menuMode = 20;
              Game1.PlaySound(11);
              break;
            }
            break;
          case 23:
            index1 = Game1.numLoadPlayers;
            num6 = Game1.screenWidth / 2 - 16;
            num7 = 210;
            flag1 = true;
            num8 = 370;
            num2 = 240;
            num4 = 60;
            Game1.loadPlayer[index1].pantsColor = this.selColor;
            num5 = 3;
            strArray1[0] = "";
            strArray1[1] = "Pants Color";
            flagArray1[1] = true;
            numArray1[2] = 170;
            numArray1[1] = 10;
            strArray1[2] = "Back";
            if (this.selectedMenu == 2)
            {
              Game1.menuMode = 20;
              Game1.PlaySound(11);
              break;
            }
            break;
          case 24:
            index1 = Game1.numLoadPlayers;
            num6 = Game1.screenWidth / 2 - 16;
            num7 = 210;
            flag1 = true;
            num8 = 370;
            num2 = 240;
            num4 = 60;
            Game1.loadPlayer[index1].shoeColor = this.selColor;
            num5 = 3;
            strArray1[0] = "";
            strArray1[1] = "Misc Color";
            flagArray1[1] = true;
            numArray1[2] = 170;
            numArray1[1] = 10;
            strArray1[2] = "Back";
            if (this.selectedMenu == 2)
            {
              Game1.menuMode = 20;
              Game1.PlaySound(11);
              break;
            }
            break;
          case 25:
            flag1 = true;
            num8 = 370;
            num2 = 240;
            num4 = 60;
            Game1.mouseColor = this.selColor;
            num5 = 3;
            strArray1[0] = "";
            strArray1[1] = "Cursor Color";
            flagArray1[1] = true;
            numArray1[2] = 170;
            numArray1[1] = 10;
            strArray1[2] = "Back";
            if (this.selectedMenu == 2)
            {
              Game1.menuMode = 11;
              Game1.PlaySound(11);
              break;
            }
            break;
          case 26:
            flag2 = true;
            num2 = 240;
            num4 = 60;
            num5 = 3;
            strArray1[0] = "";
            strArray1[1] = "Volume";
            flagArray1[1] = true;
            numArray1[2] = 170;
            numArray1[1] = 10;
            strArray1[2] = "Back";
            if (this.selectedMenu == 2)
            {
              Game1.menuMode = 11;
              Game1.PlaySound(11);
              break;
            }
            break;
          case 30:
            string password1 = Netplay.password;
            Netplay.password = Game1.GetInputText(Netplay.password);
            if (password1 != Netplay.password)
              Game1.PlaySound(12);
            strArray1[0] = "Enter Server Password:";
            ++this.textBlinkerCount;
            if (this.textBlinkerCount >= 20)
            {
              this.textBlinkerState = this.textBlinkerState != 0 ? 0 : 1;
              this.textBlinkerCount = 0;
            }
            strArray1[1] = Netplay.password;
            if (this.textBlinkerState == 1)
            {
              string[] strArray11;
              (strArray11 = strArray1)[1] = strArray11[1] + "|";
              numArray2[1] = 1;
            }
            else
            {
              string[] strArray12;
              (strArray12 = strArray1)[1] = strArray12[1] + " ";
            }
            flagArray1[0] = true;
            flagArray1[1] = true;
            numArray1[1] = -20;
            numArray1[2] = 20;
            strArray1[2] = "Accept";
            strArray1[3] = "Back";
            num5 = 4;
            if (this.selectedMenu == 3)
            {
              Game1.PlaySound(11);
              Game1.menuMode = 6;
              Netplay.password = "";
              break;
            }
            if (this.selectedMenu == 2 || Game1.inputTextEnter)
            {
              WorldGen.serverLoadWorld();
              Game1.menuMode = 10;
              break;
            }
            break;
        }
      }
      if (Game1.menuMode != menuMode)
      {
        num5 = 0;
        for (int index11 = 0; index11 < Game1.maxMenuItems; ++index11)
          this.menuItemScale[index11] = 0.8f;
      }
      int focusMenu = this.focusMenu;
      this.selectedMenu = -1;
      this.focusMenu = -1;
      for (int index12 = 0; index12 < num5; ++index12)
      {
        if (strArray1[index12] != null)
        {
          Color color2;
          if (flag1)
          {
            string text1 = "";
            for (int index13 = 0; index13 < 6; ++index13)
            {
              int num9 = num8;
              int num10 = 370;
              if (index13 == 0)
                text1 = "Red:";
              if (index13 == 1)
              {
                text1 = "Green:";
                num9 += 30;
              }
              if (index13 == 2)
              {
                text1 = "Blue:";
                num9 += 60;
              }
              if (index13 == 3)
              {
                text1 = string.Concat((object) this.selColor.R);
                num10 += 90;
              }
              if (index13 == 4)
              {
                text1 = string.Concat((object) this.selColor.G);
                num10 += 90;
                num9 += 30;
              }
              if (index13 == 5)
              {
                text1 = string.Concat((object) this.selColor.B);
                num10 += 90;
                num9 += 60;
              }
              for (int index14 = 0; index14 < 5; ++index14)
              {
                color2 = Color.Black;
                if (index14 == 4)
                {
                  color2 = color1;
                  color2.R = (byte) (((int) byte.MaxValue + (int) color2.R) / 2);
                  color2.G = (byte) (((int) byte.MaxValue + (int) color2.R) / 2);
                  color2.B = (byte) (((int) byte.MaxValue + (int) color2.R) / 2);
                }
                int maxValue = (int) byte.MaxValue;
                int num11 = (int) color2.R - ((int) byte.MaxValue - maxValue);
                if (num11 < 0)
                  num11 = 0;
                color2 = new Color((int) (byte) num11, (int) (byte) num11, (int) (byte) num11, (int) (byte) maxValue);
                int num12 = 0;
                int num13 = 0;
                if (index14 == 0)
                  num12 = -2;
                if (index14 == 1)
                  num12 = 2;
                if (index14 == 2)
                  num13 = -2;
                if (index14 == 3)
                  num13 = 2;
                this.spriteBatch.DrawString(Game1.fontDeathText, text1, new Vector2((float) (num10 + num12), (float) (num9 + num13)), color2, 0.0f, new Vector2(), 0.5f, SpriteEffects.None, 0.0f);
              }
            }
            bool flag3 = false;
            for (int index15 = 0; index15 < 2; ++index15)
            {
              for (int index16 = 0; index16 < 3; ++index16)
              {
                int num14 = num8 + index16 * 30 - 12;
                int num15 = 360;
                float scale = 0.9f;
                int num16;
                if (index15 == 0)
                {
                  num16 = num15 - 70;
                  num14 += 2;
                }
                else
                  num16 = num15 - 40;
                string text2 = "-";
                if (index15 == 1)
                  text2 = "+";
                Vector2 vector2 = new Vector2(24f, 24f);
                int a = 142;
                if (Game1.mouseState[0].Position.X > num16 
                  && (double) Game1.mouseState[0].Position.X < (double) num16 + (double) vector2.X 
                  && Game1.mouseState[0].Position.Y > num14 + 13 
                  && (double) Game1.mouseState[0].Position.Y < (double) (num14 + 13) + (double) vector2.Y)
                {
                  if (this.focusColor != (index15 + 1) * (index16 + 10))
                    Game1.PlaySound(12);
                  this.focusColor = (index15 + 1) * (index16 + 10);
                  flag3 = true;
                  a = (int) byte.MaxValue;
                  if (Game1.mouseState.Count==1//Game1.mouseState.LeftButton == ButtonState.Pressed
                  )
                  {
                    if (this.colorDelay <= 1)
                    {
                      this.colorDelay = this.colorDelay != 0 ? 3 : 40;
                      int num17 = index15;
                      if (index15 == 0)
                      {
                        num17 = -1;
                        if ((int) this.selColor.R + (int) this.selColor.G + (int) this.selColor.B < (int) byte.MaxValue)
                          num17 = 0;
                      }
                      if (index16 == 0 && (int) this.selColor.R + num17 >= 0 && (int) this.selColor.R + num17 <= (int) byte.MaxValue)
                        this.selColor.R += (byte) num17;
                      if (index16 == 1 && (int) this.selColor.G + num17 >= 0 && (int) this.selColor.G + num17 <= (int) byte.MaxValue)
                        this.selColor.G += (byte) num17;
                      if (index16 == 2 && (int) this.selColor.B + num17 >= 0 && (int) this.selColor.B + num17 <= (int) byte.MaxValue)
                        this.selColor.B += (byte) num17;
                    }
                    --this.colorDelay;
                  }
                  else
                    this.colorDelay = 0;
                }
                for (int index17 = 0; index17 < 5; ++index17)
                {
                  color2 = Color.Black;
                  if (index17 == 4)
                  {
                    color2 = color1;
                    color2.R = (byte) (((int) byte.MaxValue + (int) color2.R) / 2);
                    color2.G = (byte) (((int) byte.MaxValue + (int) color2.R) / 2);
                    color2.B = (byte) (((int) byte.MaxValue + (int) color2.R) / 2);
                  }
                  int num18 = (int) color2.R - ((int) byte.MaxValue - a);
                  if (num18 < 0)
                    num18 = 0;
                  color2 = new Color((int) (byte) num18, (int) (byte) num18, (int) (byte) num18, (int) (byte) a);
                  int num19 = 0;
                  int num20 = 0;
                  if (index17 == 0)
                    num19 = -2;
                  if (index17 == 1)
                    num19 = 2;
                  if (index17 == 2)
                    num20 = -2;
                  if (index17 == 3)
                    num20 = 2;
                  this.spriteBatch.DrawString(Game1.fontDeathText, text2, new Vector2((float) (num16 + num19), (float) (num14 + num20)), color2, 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
                }
              }
            }
            if (!flag3)
            {
              this.focusColor = 0;
              this.colorDelay = 0;
            }
          }
          if (flag2)
          {
            int num21 = 400;
            string text3 = "";
            for (int index18 = 0; index18 < 4; ++index18)
            {
              int num22 = num21;
              int num23 = 370;
              if (index18 == 0)
                text3 = "Sound:";
              if (index18 == 1)
              {
                text3 = "Music:";
                num22 += 30;
              }
              if (index18 == 2)
              {
                text3 = Math.Round((double) Game1.soundVolume * 100.0).ToString() + "%";
                num23 += 90;
              }
              if (index18 == 3)
              {
                text3 = Math.Round((double) Game1.musicVolume * 100.0).ToString() + "%";
                num23 += 90;
                num22 += 30;
              }
              for (int index19 = 0; index19 < 5; ++index19)
              {
                color2 = Color.Black;
                if (index19 == 4)
                {
                  color2 = color1;
                  color2.R = (byte) (((int) byte.MaxValue + (int) color2.R) / 2);
                  color2.G = (byte) (((int) byte.MaxValue + (int) color2.R) / 2);
                  color2.B = (byte) (((int) byte.MaxValue + (int) color2.R) / 2);
                }
                int maxValue = (int) byte.MaxValue;
                int num24 = (int) color2.R - ((int) byte.MaxValue - maxValue);
                if (num24 < 0)
                  num24 = 0;
                color2 = new Color((int) (byte) num24, (int) (byte) num24, (int) (byte) num24, (int) (byte) maxValue);
                int num25 = 0;
                int num26 = 0;
                if (index19 == 0)
                  num25 = -2;
                if (index19 == 1)
                  num25 = 2;
                if (index19 == 2)
                  num26 = -2;
                if (index19 == 3)
                  num26 = 2;
                this.spriteBatch.DrawString(Game1.fontDeathText, text3, new Vector2((float) (num23 + num25), (float) (num22 + num26)), color2, 0.0f, new Vector2(), 0.5f, SpriteEffects.None, 0.0f);
              }
            }
            bool flag4 = false;
            for (int index20 = 0; index20 < 2; ++index20)
            {
              for (int index21 = 0; index21 < 2; ++index21)
              {
                int num27 = num21 + index21 * 30 - 12;
                int num28 = 360;
                float scale = 0.9f;
                int num29;
                if (index20 == 0)
                {
                  num29 = num28 - 70;
                  num27 += 2;
                }
                else
                  num29 = num28 - 40;
                string text4 = "-";
                if (index20 == 1)
                  text4 = "+";
                Vector2 vector2 = new Vector2(24f, 24f);
                int a = 142;
                if (Game1.mouseState[0].Position.X > num29 
                      && (double) Game1.mouseState[0].Position.X 
                                               < (double) num29 + (double) vector2.X 
                      && Game1.mouseState[0].Position.Y > num27 + 13 
                      && (double) Game1.mouseState[0].Position.Y 
                                                < (double) (num27 + 13) + (double) vector2.Y)
                {
                  if (this.focusColor != (index20 + 1) * (index21 + 10))
                    Game1.PlaySound(12);
                  this.focusColor = (index20 + 1) * (index21 + 10);
                  flag4 = true;
                  a = (int) byte.MaxValue;

                  if (Game1.mouseState.Count == 1//Game1.mouseState.LeftButton == ButtonState.Pressed
                  )
                  {
                    if (this.colorDelay <= 1)
                    {
                      this.colorDelay = this.colorDelay != 0 ? 3 : 40;
                      int num30 = index20;
                      if (index20 == 0)
                        num30 = -1;
                      if (index21 == 0)
                      {
                        Game1.soundVolume += (float) num30 * 0.01f;
                        if ((double) Game1.soundVolume > 1.0)
                          Game1.soundVolume = 1f;
                        if ((double) Game1.soundVolume < 0.0)
                          Game1.soundVolume = 0.0f;
                      }
                      if (index21 == 1)
                      {
                        Game1.musicVolume += (float) num30 * 0.01f;
                        if ((double) Game1.musicVolume > 1.0)
                          Game1.musicVolume = 1f;
                        if ((double) Game1.musicVolume < 0.0)
                          Game1.musicVolume = 0.0f;
                      }
                    }
                    --this.colorDelay;
                  }
                  else
                    this.colorDelay = 0;
                }
                for (int index22 = 0; index22 < 5; ++index22)
                {
                  color2 = Color.Black;
                  if (index22 == 4)
                  {
                    color2 = color1;
                    color2.R = (byte) (((int) byte.MaxValue + (int) color2.R) / 2);
                    color2.G = (byte) (((int) byte.MaxValue + (int) color2.R) / 2);
                    color2.B = (byte) (((int) byte.MaxValue + (int) color2.R) / 2);
                  }
                  int num31 = (int) color2.R - ((int) byte.MaxValue - a);
                  if (num31 < 0)
                    num31 = 0;
                  color2 = new Color((int) (byte) num31, (int) (byte) num31, (int) (byte) num31, (int) (byte) a);
                  int num32 = 0;
                  int num33 = 0;
                  if (index22 == 0)
                    num32 = -2;
                  if (index22 == 1)
                    num32 = 2;
                  if (index22 == 2)
                    num33 = -2;
                  if (index22 == 3)
                    num33 = 2;
                  this.spriteBatch.DrawString(Game1.fontDeathText, text4, new Vector2((float) (num29 + num32), (float) (num27 + num33)), color2, 0.0f, new Vector2(), scale, SpriteEffects.None, 0.0f);
                }
              }
            }
            if (!flag4)
            {
              this.focusColor = 0;
              this.colorDelay = 0;
            }
          }
          for (int index23 = 0; index23 < 5; ++index23)
          {
            color2 = Color.Black;
            if (index23 == 4)
            {
              color2 = color1;
              color2.R = (byte) (((int) byte.MaxValue + (int) color2.R) / 2);
              color2.G = (byte) (((int) byte.MaxValue + (int) color2.R) / 2);
              color2.B = (byte) (((int) byte.MaxValue + (int) color2.R) / 2);
            }
            int a = (int) ((double) byte.MaxValue * ((double) this.menuItemScale[index12] * 2.0 - 1.0));
            if (flagArray1[index12])
              a = (int) byte.MaxValue;
            int num34 = (int) color2.R - ((int) byte.MaxValue - a);
            if (num34 < 0)
              num34 = 0;
            color2 = new Color((int) (byte) num34, (int) (byte) num34, (int) (byte) num34, (int) (byte) a);
            int num35 = 0;
            int num36 = 0;
            if (index23 == 0)
              num35 = -2;
            if (index23 == 1)
              num35 = 2;
            if (index23 == 2)
              num36 = -2;
            if (index23 == 3)
              num36 = 2;
            Vector2 origin = Game1.fontDeathText.MeasureString(strArray1[index12]);
            origin.X *= 0.5f;
            origin.Y *= 0.5f;
            float scale = this.menuItemScale[index12];
            if (Game1.menuMode == 15 && index12 == 0)
              scale *= 0.35f;
            else if (Game1.netMode == 2)
              scale *= 0.5f;
            this.spriteBatch.DrawString(
                Game1.fontDeathText, strArray1[index12], 
                new Vector2((float) (num3 + num35 + numArray2[index12]),
                (float) (num2 + num4 * index12 + num36) + origin.Y 
                + (float) numArray1[index12]), color2, 0.0f,
                origin, scale, SpriteEffects.None, 0.0f);
          }

          if (Game1.mouseState[0].Position.X > num3 - strArray1[index12].Length * 10 + numArray2[index12] 
                        && Game1.mouseState[0].Position.X < num3 
                        + strArray1[index12].Length * 10 + numArray2[index12] 
                        && Game1.mouseState[0].Position.Y > num2 + num4 * index12 
                        + numArray1[index12] 
                        && Game1.mouseState[0].Position.Y < num2 + num4 * index12 + 50 + numArray1[index12] 
                        && Game1.hasFocus)
          {
            this.focusMenu = index12;
            if (flagArray1[index12] || flagArray2[index12])
            {
              this.focusMenu = -1;
            }
            else
            {
              if (focusMenu != this.focusMenu)
                Game1.PlaySound(12);
              if (Game1.mouseLeftRelease && Game1.mouseState.Count==1//Game1.mouseState.LeftButton == ButtonState.Pressed
              )
                this.selectedMenu = index12;
            }
          }
        }
      }
      for (int index24 = 0; index24 < Game1.maxMenuItems; ++index24)
      {
        if (index24 == this.focusMenu)
        {
          if ((double) this.menuItemScale[index24] < 1.0)
            this.menuItemScale[index24] += 0.02f;
          if ((double) this.menuItemScale[index24] > 1.0)
            this.menuItemScale[index24] = 1f;
        }
        else if ((double) this.menuItemScale[index24] > 0.8)
          this.menuItemScale[index24] -= 0.02f;
      }
      if (index1 >= 0)
      {
        Game1.loadPlayer[index1].PlayerFrame();
        Game1.loadPlayer[index1].position.X = (float) num6 + Game1.screenPosition.X;
        Game1.loadPlayer[index1].position.Y = (float) num7 + Game1.screenPosition.Y;
        this.DrawPlayer(Game1.loadPlayer[index1]);
      }
      this.spriteBatch.Draw(Game1.cursorTexture, 
          new Vector2((float) (Game1.mouseState[0].Position.X + 1), 
          (float) (Game1.mouseState[0].Position.Y + 1)), new Rectangle?(
              new Rectangle(0, 0, Game1.cursorTexture.Width, Game1.cursorTexture.Height)), 
          new Color((int) ((double) Game1.cursorColor.R * 0.20000000298023224),
          (int) ((double) Game1.cursorColor.G * 0.20000000298023224),
          (int) ((double) Game1.cursorColor.B * 0.20000000298023224),
          (int) ((double) Game1.cursorColor.A * 0.5)), 0.0f, 
          new Vector2(), Game1.cursorScale * 1.1f, SpriteEffects.None, 0.0f);

      this.spriteBatch.Draw(Game1.cursorTexture, 
          new Vector2((float) Game1.mouseState[0].Position.X,
          (float) Game1.mouseState[0].Position.Y), new Rectangle?(
              new Rectangle(0, 0, Game1.cursorTexture.Width, Game1.cursorTexture.Height)),
          Game1.cursorColor, 0.0f, new Vector2(), Game1.cursorScale, SpriteEffects.None, 0.0f);
      this.spriteBatch.End();
            Game1.mouseLeftRelease = (Game1.mouseState.Count == 0);//Game1.mouseState.LeftButton != ButtonState.Pressed;
      if (
           Game1.mouseState.Count > 1 // Game1.mouseState.RightButton == ButtonState.Pressed
      )
        Game1.mouseRightRelease = false;
      else
        Game1.mouseRightRelease = true;
    }

    public static void CursorColor()
    {
      Game1.cursorAlpha += (float) Game1.cursorColorDirection * 0.015f;
      if ((double) Game1.cursorAlpha >= 1.0)
      {
        Game1.cursorAlpha = 1f;
        Game1.cursorColorDirection = -1;
      }
      if ((double) Game1.cursorAlpha <= 0.6)
      {
        Game1.cursorAlpha = 0.6f;
        Game1.cursorColorDirection = 1;
      }
      float num = (float) ((double) Game1.cursorAlpha * 0.30000001192092896 + 0.699999988079071);
      Game1.cursorColor = new Color((int) (byte) ((double) Game1.mouseColor.R * (double) Game1.cursorAlpha), (int) (byte) ((double) Game1.mouseColor.G * (double) Game1.cursorAlpha), (int) (byte) ((double) Game1.mouseColor.B * (double) Game1.cursorAlpha), (int) (byte) ((double) byte.MaxValue * (double) num));
      Game1.cursorScale = (float) ((double) Game1.cursorAlpha * 0.30000001192092896 + 0.699999988079071 + 0.10000000149011612);
    }

    protected override void Draw(GameTime gameTime)
    {
      if (!this.IsActive)
        return;
      Game1.CursorColor();
      ++Game1.drawTime;
      Game1.screenLastPosition = Game1.screenPosition;
      if (Game1.stackSplit == 0)
      {
        Game1.stackCounter = 0;
        Game1.stackDelay = 7;
      }
      else
      {
        ++Game1.stackCounter;
        if (Game1.stackCounter >= 30)
        {
          --Game1.stackDelay;
          if (Game1.stackDelay < 2)
            Game1.stackDelay = 2;
          Game1.stackCounter = 0;
        }
      }
      Game1.mouseTextColor += (byte) Game1.mouseTextColorChange;
      if (Game1.mouseTextColor >= (byte) 250)
        Game1.mouseTextColorChange = -4;
      if (Game1.mouseTextColor <= (byte) 175)
        Game1.mouseTextColorChange = 4;
      if (Game1.myPlayer >= 0)
        Game1.player[Game1.myPlayer].mouseInterface = false;
      Game1.toolTip = new Item();

      // screen panning
      if (/*!Game1.debugMode && */ !Game1.gameMenu && Game1.netMode != 2)
      {
        int num1 = (int)Game1.mouseState[0].Position.X;
        int num2 = (int)Game1.mouseState[0].Position.Y;
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
        Game1.screenPosition.X = (float) ((double) Game1.player[Game1.myPlayer].position.X 
                    + (double) Game1.player[Game1.myPlayer].width * 0.5 
                    - (double) Game1.screenWidth * 0.5);
        Game1.screenPosition.Y = (float) ((double) Game1.player[Game1.myPlayer].position.Y 
                    + (double) Game1.player[Game1.myPlayer].height * 0.5 
                    - (double) Game1.screenHeight * 0.5);
        Game1.screenPosition.X = (float) (int) Game1.screenPosition.X;
        Game1.screenPosition.Y = (float) (int) Game1.screenPosition.Y;
      }
      if (!Game1.gameMenu && Game1.netMode != 2)
      {
        if ((double) Game1.screenPosition.X < (double) Game1.leftWorld + 336.0 + 16.0)
          Game1.screenPosition.X = (float) ((double) Game1.leftWorld + 336.0 + 16.0);
        else if ((double) Game1.screenPosition.X + (double) Game1.screenWidth > (double) Game1.rightWorld - 336.0 - 32.0)
          Game1.screenPosition.X = (float) ((double) Game1.rightWorld - (double) Game1.screenWidth - 336.0 - 32.0);
        if ((double) Game1.screenPosition.Y < (double) Game1.topWorld + 336.0 + 16.0)
          Game1.screenPosition.Y = (float) ((double) Game1.topWorld + 336.0 + 16.0);
        else if ((double) Game1.screenPosition.Y + (double) Game1.screenHeight > (double) Game1.bottomWorld - 336.0 - 32.0)
          Game1.screenPosition.Y = (float) ((double) Game1.bottomWorld - (double) Game1.screenHeight - 336.0 - 32.0);
      }
     
      this.GraphicsDevice.Clear(Color.Black);
      base.Draw(gameTime);
      this.spriteBatch.Begin();
      double num3 = 0.5;
      int num4 = (int) (-Math.IEEERemainder((double) Game1.screenPosition.X * num3, 
          (double) Game1.backgroundWidth[Game1.background]) - (double) (Game1.backgroundWidth[Game1.background] / 2));
      int num5 = Game1.screenWidth / Game1.backgroundWidth[Game1.background] + 2;
      int y1 = (int) (-(double) Game1.screenPosition.Y / (Game1.worldSurface * 16.0 -
                (double) Game1.screenHeight) * (double) (Game1.backgroundHeight[Game1.background] - Game1.screenHeight));
     
      if (Game1.gameMenu || Game1.netMode == 2)
        y1 = -200;
      Color white1 = Color.White;
      int x1 = (int) (Game1.time / 54000.0 * (double) (Game1.screenWidth + Game1.sunTexture.Width * 2)) - Game1.sunTexture.Width;
      int num6 = 0;
      Color white2 = Color.White;
      float scale1 = 1f;
      float rotation1 = (float) (Game1.time / 54000.0 * 2.0 - 7.3000001907348633);
      int x2 = (int) (Game1.time / 32400.0 * (double) (Game1.screenWidth + Game1.moonTexture.Width * 2)) - Game1.moonTexture.Width;
      int num7 = 0;
      Color white3 = Color.White;
      float scale2 = 1f;
      float rotation2 = (float) (Game1.time / 32400.0 * 2.0 - 7.3000001907348633);
      if (Game1.dayTime)
      {
        double num8;
        if (Game1.time < 27000.0)
        {
          num8 = Math.Pow(1.0 - Game1.time / 54000.0 * 2.0, 2.0);
          num6 = (int) ((double) y1 + num8 * 250.0 + 180.0);
        }
        else
        {
          num8 = Math.Pow((Game1.time / 54000.0 - 0.5) * 2.0, 2.0);
          num6 = (int) ((double) y1 + num8 * 250.0 + 180.0);
        }
        scale1 = (float) (1.2 - num8 * 0.4);
      }
      else
      {
        double num9;
        if (Game1.time < 16200.0)
        {
          num9 = Math.Pow(1.0 - Game1.time / 32400.0 * 2.0, 2.0);
          num7 = (int) ((double) y1 + num9 * 250.0 + 180.0);
        }
        else
        {
          num9 = Math.Pow((Game1.time / 32400.0 - 0.5) * 2.0, 2.0);
          num7 = (int) ((double) y1 + num9 * 250.0 + 180.0);
        }
        scale2 = (float) (1.2 - num9 * 0.4);
      }
      if (Game1.dayTime)
      {
        if (Game1.time < 13500.0)
        {
          float num10 = (float) (Game1.time / 13500.0);
          white2.R = (byte) ((double) num10 * 200.0 + 55.0);
          white2.G = (byte) ((double) num10 * 180.0 + 75.0);
          white2.B = (byte) ((double) num10 * 250.0 + 5.0);
          white1.R = (byte) ((double) num10 * 200.0 + 55.0);
          white1.G = (byte) ((double) num10 * 200.0 + 55.0);
          white1.B = (byte) ((double) num10 * 200.0 + 55.0);
        }
        if (Game1.time > 45900.0)
        {
          float num11 = (float) (1.0 - (Game1.time / 54000.0 - 0.85) * (20.0 / 3.0));
          white2.R = (byte) ((double) num11 * 120.0 + 55.0);
          white2.G = (byte) ((double) num11 * 100.0 + 25.0);
          white2.B = (byte) ((double) num11 * 120.0 + 55.0);
          white1.R = (byte) ((double) num11 * 200.0 + 55.0);
          white1.G = (byte) ((double) num11 * 85.0 + 55.0);
          white1.B = (byte) ((double) num11 * 135.0 + 55.0);
        }
        else if (Game1.time > 37800.0)
        {
          float num12 = (float) (1.0 - (Game1.time / 54000.0 - 0.7) * (20.0 / 3.0));
          white2.R = (byte) ((double) num12 * 80.0 + 175.0);
          white2.G = (byte) ((double) num12 * 130.0 + 125.0);
          white2.B = (byte) ((double) num12 * 100.0 + 155.0);
          white1.R = (byte) ((double) num12 * 0.0 + (double) byte.MaxValue);
          white1.G = (byte) ((double) num12 * 115.0 + 140.0);
          white1.B = (byte) ((double) num12 * 75.0 + 180.0);
        }
      }
      if (!Game1.dayTime)
      {
        if (Game1.bloodMoon)
        {
          if (Game1.time < 16200.0)
          {
            float num13 = (float) (1.0 - Game1.time / 16200.0);
            white3.R = (byte) ((double) num13 * 10.0 + 205.0);
            white3.G = (byte) ((double) num13 * 170.0 + 55.0);
            white3.B = (byte) ((double) num13 * 200.0 + 55.0);
            white1.R = (byte) (60.0 - (double) num13 * 60.0 + 55.0);
            white1.G = (byte) ((double) num13 * 40.0 + 15.0);
            white1.B = (byte) ((double) num13 * 40.0 + 15.0);
          }
          else if (Game1.time >= 16200.0)
          {
            float num14 = (float) ((Game1.time / 32400.0 - 0.5) * 2.0);
            white3.R = (byte) ((double) num14 * 50.0 + 205.0);
            white3.G = (byte) ((double) num14 * 100.0 + 155.0);
            white3.B = (byte) ((double) num14 * 100.0 + 155.0);
            white3.R = (byte) ((double) num14 * 10.0 + 205.0);
            white3.G = (byte) ((double) num14 * 170.0 + 55.0);
            white3.B = (byte) ((double) num14 * 200.0 + 55.0);
            white1.R = (byte) (60.0 - (double) num14 * 60.0 + 55.0);
            white1.G = (byte) ((double) num14 * 40.0 + 15.0);
            white1.B = (byte) ((double) num14 * 40.0 + 15.0);
          }
        }
        else if (Game1.time < 16200.0)
        {
          float num15 = (float) (1.0 - Game1.time / 16200.0);
          white3.R = (byte) ((double) num15 * 10.0 + 205.0);
          white3.G = (byte) ((double) num15 * 70.0 + 155.0);
          white3.B = (byte) ((double) num15 * 100.0 + 155.0);
          white1.R = (byte) ((double) num15 * 40.0 + 15.0);
          white1.G = (byte) ((double) num15 * 40.0 + 15.0);
          white1.B = (byte) ((double) num15 * 40.0 + 15.0);
        }
        else if (Game1.time >= 16200.0)
        {
          float num16 = (float) ((Game1.time / 32400.0 - 0.5) * 2.0);
          white3.R = (byte) ((double) num16 * 50.0 + 205.0);
          white3.G = (byte) ((double) num16 * 100.0 + 155.0);
          white3.B = (byte) ((double) num16 * 100.0 + 155.0);
          white1.R = (byte) ((double) num16 * 40.0 + 15.0);
          white1.G = (byte) ((double) num16 * 40.0 + 15.0);
          white1.B = (byte) ((double) num16 * 40.0 + 15.0);
        }
      }
      if (Game1.gameMenu || Game1.netMode == 2)
      {
        y1 = 0;
        if (!Game1.dayTime)
        {
          white1.R = (byte) 55;
          white1.G = (byte) 55;
          white1.B = (byte) 55;
        }
      }
      if (Game1.evilTiles > 0)
      {
        float num17 = (float) Game1.evilTiles / 500f;
        if ((double) num17 > 1.0)
          num17 = 1f;
        int r1 = (int) white1.R;
        int g1 = (int) white1.G;
        int b1 = (int) white1.B;
        int num18 = r1 + (int) (10.0 * (double) num17);
        int num19 = g1 - (int) (90.0 * (double) num17 * ((double) white1.G / (double) byte.MaxValue));
        int num20 = b1 - (int) (190.0 * (double) num17 * ((double) white1.B / (double) byte.MaxValue));
        if (num18 > (int) byte.MaxValue)
          num18 = (int) byte.MaxValue;
        if (num19 < 15)
          num19 = 15;
        if (num20 < 15)
          num20 = 15;
        white1.R = (byte) num18;
        white1.G = (byte) num19;
        white1.B = (byte) num20;
        int r2 = (int) white2.R;
        int g2 = (int) white2.G;
        int b2 = (int) white2.B;
        int num21 = r2 - (int) (100.0 * (double) num17 * ((double) white2.R / (double) byte.MaxValue));
        int num22 = g2 - (int) (160.0 * (double) num17 * ((double) white2.G / (double) byte.MaxValue));
        int num23 = b2 - (int) (170.0 * (double) num17 * ((double) white2.B / (double) byte.MaxValue));
        if (num21 < 15)
          num21 = 15;
        if (num22 < 15)
          num22 = 15;
        if (num23 < 15)
          num23 = 15;
        white2.R = (byte) num21;
        white2.G = (byte) num22;
        white2.B = (byte) num23;
        int r3 = (int) white3.R;
        int g3 = (int) white3.G;
        int b3 = (int) white3.B;
        int num24 = r3 - (int) (140.0 * (double) num17 * ((double) white3.R / (double) byte.MaxValue));
        int num25 = g3 - (int) (170.0 * (double) num17 * ((double) white3.G / (double) byte.MaxValue));
        int num26 = b3 - (int) (190.0 * (double) num17 * ((double) white3.B / (double) byte.MaxValue));
        if (num24 < 15)
          num24 = 15;
        if (num25 < 15)
          num25 = 15;
        if (num26 < 15)
          num26 = 15;
        white3.R = (byte) num24;
        white3.G = (byte) num25;
        white3.B = (byte) num26;
      }
      Game1.tileColor.A = byte.MaxValue;
      Game1.tileColor.R = (byte) (((int) white1.R + (int) white1.B + (int) white1.G) / 3);
      Game1.tileColor.G = (byte) (((int) white1.R + (int) white1.B + (int) white1.G) / 3);
      Game1.tileColor.B = (byte) (((int) white1.R + (int) white1.B + (int) white1.G) / 3);
      if ((double) Game1.screenPosition.Y < Game1.worldSurface * 16.0 + 16.0)
      {
        for (int index = 0; index < num5; ++index)
          this.spriteBatch.Draw(Game1.backgroundTexture[Game1.background], new Rectangle(num4 + Game1.backgroundWidth[Game1.background] * index, y1, Game1.backgroundWidth[Game1.background], Game1.backgroundHeight[Game1.background]), white1);
      }
      if ((double) Game1.screenPosition.Y < Game1.worldSurface * 16.0 + 16.0 && (int) byte.MaxValue - (int) Game1.tileColor.R - 100 > 0 && Game1.netMode != 2)
      {
        Star.UpdateStars();
        for (int index = 0; index < Game1.numStars; ++index)
        {
          Color color = new Color();
          float num27 = (float) Game1.evilTiles / 500f;
          if ((double) num27 > 1.0)
            num27 = 1f;
          float num28 = (float) (1.0 - (double) num27 * 0.5);
          if (Game1.evilTiles <= 0)
            num28 = 1f;
          int num29 = (int) ((double) ((int) byte.MaxValue - (int) Game1.tileColor.R - 100) * (double) Game1.star[index].twinkle * (double) num28);
          int num30 = (int) ((double) ((int) byte.MaxValue - (int) Game1.tileColor.G - 100) * (double) Game1.star[index].twinkle * (double) num28);
          int num31 = (int) ((double) ((int) byte.MaxValue - (int) Game1.tileColor.B - 100) * (double) Game1.star[index].twinkle * (double) num28);
          if (num29 < 0)
            num29 = 0;
          if (num30 < 0)
            num30 = 0;
          if (num31 < 0)
            num31 = 0;
          color.R = (byte) num29;
          color.G = (byte) ((double) num30 * (double) num28);
          color.B = (byte) ((double) num31 * (double) num28);
          this.spriteBatch.Draw(Game1.starTexture[Game1.star[index].type], new Vector2(Game1.star[index].position.X + (float) Game1.starTexture[Game1.star[index].type].Width * 0.5f, Game1.star[index].position.Y + (float) Game1.starTexture[Game1.star[index].type].Height * 0.5f + (float) y1), new Rectangle?(new Rectangle(0, 0, Game1.starTexture[Game1.star[index].type].Width, Game1.starTexture[Game1.star[index].type].Height)), color, Game1.star[index].rotation, new Vector2((float) Game1.starTexture[Game1.star[index].type].Width * 0.5f, (float) Game1.starTexture[Game1.star[index].type].Height * 0.5f), Game1.star[index].scale * Game1.star[index].twinkle, SpriteEffects.None, 0.0f);
        }
      }
      if (Game1.dayTime)
        this.spriteBatch.Draw(Game1.sunTexture, 
            new Vector2((float) x1, (float) (num6 + (int) Game1.sunModY)), 
            new Rectangle?(new Rectangle(0, 0, Game1.sunTexture.Width, Game1.sunTexture.Height)), 
            white2, rotation1, new Vector2((float) (Game1.sunTexture.Width / 2), 
            (float) (Game1.sunTexture.Height / 2)), scale1, SpriteEffects.None, 0.0f);
      if (!Game1.dayTime)
        this.spriteBatch.Draw(Game1.moonTexture, 
            new Vector2((float) x2, (float) (num7 + (int) Game1.moonModY)), 
            new Rectangle?(new Rectangle(0, Game1.moonTexture.Width * Game1.moonPhase, 
            Game1.moonTexture.Width, Game1.moonTexture.Width)), white3, rotation2, 
            new Vector2((float) (Game1.moonTexture.Width / 2), 
            (float) (Game1.moonTexture.Width / 2)), scale2, SpriteEffects.None, 0.0f);

      Rectangle rectangle1 = !Game1.dayTime 
                ? new Rectangle((int) ((double) x2 - (double) Game1.moonTexture.Width * 0.5 * (double) scale2), (int) ((double) num7 - (double) Game1.moonTexture.Width * 0.5 * (double) scale2 + (double) Game1.moonModY), (int) ((double) Game1.moonTexture.Width * (double) scale2), (int) ((double) Game1.moonTexture.Width * (double) scale2)) : new Rectangle((int) ((double) x1 - (double) Game1.sunTexture.Width * 0.5 * (double) scale1), (int) ((double) num6 - (double) Game1.sunTexture.Height * 0.5 * (double) scale1 + (double) Game1.sunModY), (int) ((double) Game1.sunTexture.Width * (double) scale1), (int) ((double) Game1.sunTexture.Width * (double) scale1));

      Rectangle rectangle2 = new Rectangle(
          (int)Game1.mouseState[0].Position.X, 
          (int)Game1.mouseState[0].Position.Y, 1, 1);
      Game1.sunModY = (short) ((double) Game1.sunModY * 0.999);
      Game1.moonModY = (short) ((double) Game1.moonModY * 0.999);
      if (Game1.gameMenu && Game1.netMode != 1 || Game1.grabSun)
      {
        if (Game1.mouseState.Count == 1 //Game1.mouseState.LeftButton == ButtonState.Pressed 
        && Game1.hasFocus)
        {
          if (rectangle2.Intersects(rectangle1) || Game1.grabSky)
          {
            if (Game1.dayTime)
            {
              Game1.time = 54000.0 * 
                                ((double) (Game1.mouseState[0].Position.X + Game1.sunTexture.Width) 
                                / ((double) Game1.screenWidth + (double) (Game1.sunTexture.Width * 2)));
              Game1.sunModY = (short) (Game1.mouseState[0].Position.Y - num6);
              if (Game1.time > 53990.0)
                Game1.time = 53990.0;
            }
            else
            {
              Game1.time = 32400.0 * ((double) (Game1.mouseState[0].Position.X + Game1.moonTexture.Width)
                                / ((double) Game1.screenWidth + (double) (Game1.moonTexture.Width * 2)));

              Game1.moonModY = (short) (Game1.mouseState[0].Position.Y - num7);
              if (Game1.time > 32390.0)
                Game1.time = 32390.0;
            }
            if (Game1.time < 10.0)
              Game1.time = 10.0;
            if (Game1.netMode != 0)
              NetMessage.SendData(18);
            Game1.grabSky = true;
          }
        }
        else
          Game1.grabSky = false;
      }
      if (Game1.resetClouds)
      {
        Cloud.resetClouds();
        Game1.resetClouds = false;
      }
      if (this.IsActive || Game1.netMode != 0)
      {
        Cloud.UpdateClouds();
        Game1.windSpeedSpeed += (float) Game1.rand.Next(-10, 11) * 0.0001f;
        if ((double) Game1.windSpeedSpeed < -0.002)
          Game1.windSpeedSpeed = -1f / 500f;
        if ((double) Game1.windSpeedSpeed > 0.002)
          Game1.windSpeedSpeed = 1f / 500f;
        Game1.windSpeed += Game1.windSpeedSpeed;
        if ((double) Game1.windSpeed < -0.3)
          Game1.windSpeed = -0.3f;
        if ((double) Game1.windSpeed > 0.3)
          Game1.windSpeed = 0.3f;
        Game1.numClouds += Game1.rand.Next(-1, 2);
        if (Game1.numClouds < 0)
          Game1.numClouds = 0;
        if (Game1.numClouds > Game1.cloudLimit)
          Game1.numClouds = Game1.cloudLimit;
      }
      if ((double) Game1.screenPosition.Y < Game1.worldSurface * 16.0 + 16.0)
      {
        for (int index = 0; index < 100; ++index)
        {
          if (Game1.cloud[index].active)
          {
            int num32 = (int) (40.0 * (2.0 - (double) Game1.cloud[index].scale));
            Color color = new Color();
            int num33 = (int) white1.R - num32;
            if (num33 <= 0)
              num33 = 0;
            color.R = (byte) num33;
            int num34 = (int) white1.G - num32;
            if (num34 <= 0)
              num34 = 0;
            color.G = (byte) num34;
            int num35 = (int) white1.B - num32;
            if (num35 <= 0)
              num35 = 0;
            color.B = (byte) num35;
            color.A = (byte) ((int) byte.MaxValue - num32);
            this.spriteBatch.Draw(Game1.cloudTexture[Game1.cloud[index].type], new Vector2(Game1.cloud[index].position.X + (float) Game1.cloudTexture[Game1.cloud[index].type].Width * 0.5f, Game1.cloud[index].position.Y + (float) Game1.cloudTexture[Game1.cloud[index].type].Height * 0.5f + (float) y1), new Rectangle?(new Rectangle(0, 0, Game1.cloudTexture[Game1.cloud[index].type].Width, Game1.cloudTexture[Game1.cloud[index].type].Height)), color, Game1.cloud[index].rotation, new Vector2((float) Game1.cloudTexture[Game1.cloud[index].type].Width * 0.5f, (float) Game1.cloudTexture[Game1.cloud[index].type].Height * 0.5f), Game1.cloud[index].scale, SpriteEffects.None, 0.0f);
          }
        }
      }
      if (Game1.gameMenu || Game1.netMode == 2)
      {
        this.DrawMenu();
      }
      else
      {
        int firstX = (int) ((double) Game1.screenPosition.X / 16.0 - 1.0);
        int lastX = (int) (((double) Game1.screenPosition.X + (double) Game1.screenWidth) / 16.0) + 2;
        int firstY = (int) ((double) Game1.screenPosition.Y / 16.0 - 1.0);
        int lastY = (int) (((double) Game1.screenPosition.Y + (double) Game1.screenHeight) / 16.0) + 2;
        if (firstX < 0)
          firstX = 0;
        if (lastX > Game1.maxTilesX)
          lastX = Game1.maxTilesX;
        if (firstY < 0)
          firstY = 0;
        if (lastY > Game1.maxTilesY)
          lastY = Game1.maxTilesY;

        Lighting.LightTiles(firstX, lastX, firstY, lastY);
        
        Color white4 = Color.White;
        this.DrawWater(true);
        
        double num36 = 1.0;
        int num37 = (int) (-Math.IEEERemainder((double) Game1.screenPosition.X * num36, (double) Game1.backgroundWidth[1]) - (double) (Game1.backgroundWidth[1] / 2));
        int num38 = Game1.screenWidth / Game1.backgroundWidth[1] + 2;
        int y2 = (int) ((double) ((int) Game1.worldSurface * 16 - Game1.backgroundHeight[1]) - (double) Game1.screenPosition.Y + 16.0);
        for (int index1 = 0; index1 < num38; ++index1)
        {
          for (int index2 = 0; index2 < 6; ++index2)
          {
            int num39 = (num37 + Game1.backgroundWidth[1] * index1 + index2 * 16) / 16;
            int num40 = y2 / 16;
            Color color = Lighting.GetColor(num39 + (int) ((double) Game1.screenPosition.X / 16.0), num40 + (int) ((double) Game1.screenPosition.Y / 16.0));
            this.spriteBatch.Draw(Game1.backgroundTexture[1], new Vector2((float) (num37 + Game1.backgroundWidth[1] * index1 + 16 * index2), (float) y2), new Rectangle?(new Rectangle(16 * index2, 0, 16, 16)), color);
          }
        }
        double num41 = (double) ((int) (((double) (Game1.maxTilesY - 230) - Game1.worldSurface) / 6.0) * 6);
        double num42 = Game1.worldSurface + num41 - 5.0;
        bool flag1 = false;
        bool flag2 = false;
        int x3 = (int) ((double) ((int) Game1.worldSurface * 16) - (double) Game1.screenPosition.Y + 16.0);
        if (Game1.worldSurface * 16.0 <= (double) Game1.screenPosition.Y + (double) Game1.screenHeight)
        {
          double num43 = 1.0;
          int num44 = (int) (-Math.IEEERemainder(100.0 + (double) Game1.screenPosition.X * num43, (double) Game1.backgroundWidth[2]) - (double) (Game1.backgroundWidth[2] / 2));
          int num45 = Game1.screenWidth / Game1.backgroundWidth[2] + 2;
          int num46;
          int num47;
          if (Game1.worldSurface * 16.0 < (double) Game1.screenPosition.Y - 16.0)
          {
            num46 = (int) (Math.IEEERemainder((double) x3, (double) Game1.backgroundHeight[2]) - (double) Game1.backgroundHeight[2]);
            num47 = Game1.screenHeight / Game1.backgroundHeight[2] + 2;
          }
          else
          {
            num46 = x3;
            num47 = (Game1.screenHeight - x3) / Game1.backgroundHeight[2] + 1;
          }
          if (Game1.rockLayer * 16.0 < (double) Game1.screenPosition.Y + (double) Game1.screenHeight)
          {
            num47 = (int) (Game1.rockLayer * 16.0 - (double) Game1.screenPosition.Y + (double) Game1.screenHeight - (double) num46) / Game1.backgroundHeight[2];
            flag2 = true;
          }
          for (int index3 = 0; index3 < num45; ++index3)
          {
            for (int index4 = 0; index4 < num47; ++index4)
              this.spriteBatch.Draw(Game1.backgroundTexture[2], new Rectangle(num44 + Game1.backgroundWidth[2] * index3, num46 + Game1.backgroundHeight[2] * index4, Game1.backgroundWidth[2], Game1.backgroundHeight[2]), Color.White);
          }
          if (flag2)
          {
            double num48 = 1.0;
            int num49 = (int) (-Math.IEEERemainder((double) Game1.screenPosition.X * num48, (double) Game1.backgroundWidth[1]) - (double) (Game1.backgroundWidth[1] / 2));
            int num50 = Game1.screenWidth / Game1.backgroundWidth[1] + 2;
            int y3 = num46 + num47 * Game1.backgroundHeight[2];
            for (int index5 = 0; index5 < num50; ++index5)
            {
              for (int index6 = 0; index6 < 6; ++index6)
              {
                int num51 = (num49 + Game1.backgroundWidth[4] * index5 + index6 * 16) / 16;
                int num52 = y3 / 16;
                this.spriteBatch.Draw(Game1.backgroundTexture[4], new Vector2((float) (num49 + Game1.backgroundWidth[4] * index5 + 16 * index6), (float) y3), new Rectangle?(new Rectangle(16 * index6, 0, 16, 24)), Color.White);
              }
            }
          }
        }
        int x4 = (int) ((double) ((int) Game1.rockLayer * 16) - (double) Game1.screenPosition.Y + 16.0 + (double) Game1.screenHeight);
        if (Game1.rockLayer * 16.0 <= (double) Game1.screenPosition.Y + (double) Game1.screenHeight)
        {
          double num53 = 1.0;
          int num54 = (int) (-Math.IEEERemainder(100.0 + (double) Game1.screenPosition.X * num53, (double) Game1.backgroundWidth[3]) - (double) (Game1.backgroundWidth[3] / 2));
          int num55 = Game1.screenWidth / Game1.backgroundWidth[3] + 2;
          int num56;
          int num57;
          if (Game1.rockLayer * 16.0 + (double) Game1.screenHeight < (double) Game1.screenPosition.Y - 16.0)
          {
            num56 = (int) (Math.IEEERemainder((double) x4, (double) Game1.backgroundHeight[3]) - (double) Game1.backgroundHeight[3]);
            num57 = Game1.screenHeight / Game1.backgroundHeight[3] + 2;
          }
          else
          {
            num56 = x4;
            num57 = (Game1.screenHeight - x4) / Game1.backgroundHeight[3] + 1;
          }
          if (num42 * 16.0 < (double) Game1.screenPosition.Y + (double) Game1.screenHeight)
          {
            num57 = (int) (num42 * 16.0 - (double) Game1.screenPosition.Y + (double) Game1.screenHeight - (double) num56) / Game1.backgroundHeight[2];
            flag1 = true;
          }
          for (int index7 = 0; index7 < num55; ++index7)
          {
            for (int index8 = 0; index8 < num57; ++index8)
              this.spriteBatch.Draw(Game1.backgroundTexture[3], new Rectangle(num54 + Game1.backgroundWidth[2] * index7, num56 + Game1.backgroundHeight[2] * index8, Game1.backgroundWidth[2], Game1.backgroundHeight[2]), Color.White);
          }
          if (flag1)
          {
            double num58 = 1.0;
            int num59 = (int) (-Math.IEEERemainder((double) Game1.screenPosition.X * num58, (double) Game1.backgroundWidth[1]) - (double) (Game1.backgroundWidth[1] / 2) - 4.0);
            int num60 = Game1.screenWidth / Game1.backgroundWidth[1] + 2;
            int y4 = num56 + num57 * Game1.backgroundHeight[2];
            for (int index9 = 0; index9 < num60; ++index9)
            {
              for (int index10 = 0; index10 < 6; ++index10)
              {
                int num61 = (num59 + Game1.backgroundWidth[1] * index9 + index10 * 16) / 16;
                int num62 = y4 / 16;
                Lighting.GetColor(num61 + (int) ((double) Game1.screenPosition.X / 16.0), num62 + (int) ((double) Game1.screenPosition.Y / 16.0));
                this.spriteBatch.Draw(Game1.backgroundTexture[6], new Vector2((float) (num59 + Game1.backgroundWidth[1] * index9 + 16 * index10), (float) y4), new Rectangle?(new Rectangle(16 * index10, Game1.magmaBGFrame * 24, 16, 24)), Color.White);
              }
            }
          }
        }
        int x5 = (int) ((double) ((int) num42 * 16) - (double) Game1.screenPosition.Y + 16.0 + (double) Game1.screenHeight) + 8;
        if (num42 * 16.0 <= (double) Game1.screenPosition.Y + (double) Game1.screenHeight)
        {
          double num63 = 1.0;
          int num64 = (int) (-Math.IEEERemainder(100.0 + (double) Game1.screenPosition.X * num63, (double) Game1.backgroundWidth[3]) - (double) (Game1.backgroundWidth[3] / 2));
          int num65 = Game1.screenWidth / Game1.backgroundWidth[3] + 2;
          int num66;
          int num67;
          if (num42 * 16.0 + (double) Game1.screenHeight < (double) Game1.screenPosition.Y - 16.0)
          {
            num66 = (int) (Math.IEEERemainder((double) x5, (double) Game1.backgroundHeight[3]) - (double) Game1.backgroundHeight[3]);
            num67 = Game1.screenHeight / Game1.backgroundHeight[3] + 2;
          }
          else
          {
            num66 = x5;
            num67 = (Game1.screenHeight - x5) / Game1.backgroundHeight[3] + 1;
          }
          for (int index11 = 0; index11 < num65; ++index11)
          {
            for (int index12 = 0; index12 < num67; ++index12)
              this.spriteBatch.Draw(Game1.backgroundTexture[5], new Vector2((float) (num64 + Game1.backgroundWidth[2] * index11), (float) (num66 + Game1.backgroundHeight[2] * index12)), new Rectangle?(new Rectangle(0, Game1.backgroundHeight[2] * Game1.magmaBGFrame, Game1.backgroundWidth[2], Game1.backgroundHeight[2])), Color.White, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
          }
        }
        ++Game1.magmaBGFrameCounter;
        if (Game1.magmaBGFrameCounter >= 8)
        {
          Game1.magmaBGFrameCounter = 0;
          ++Game1.magmaBGFrame;
          if (Game1.magmaBGFrame >= 3)
            Game1.magmaBGFrame = 0;
        }
        int num68;
        int num69;
        for (int y5 = firstY; y5 < lastY + 4; ++y5)
        {
          num68 = y5 - firstY + 21;
          for (int x6 = firstX - 2; x6 < lastX + 2; ++x6)
          {
            if (Game1.tile[x6, y5] == null)
              Game1.tile[x6, y5] = new Tile();
            num69 = x6 - firstX + 21;
            if ((double) Lighting.Brightness(x6, y5) * (double) byte.MaxValue < (double) ((int) Game1.tileColor.R - 12) || (double) y5 > Game1.worldSurface)
              this.spriteBatch.Draw(Game1.blackTileTexture, new Vector2((float) (x6 * 16 - (int) Game1.screenPosition.X), (float) (y5 * 16 - (int) Game1.screenPosition.Y)), new Rectangle?(new Rectangle((int) Game1.tile[x6, y5].frameX, (int) Game1.tile[x6, y5].frameY, 16, 16)), Lighting.GetBlackness(x6, y5), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
          }
        }
        for (int y6 = firstY; y6 < lastY + 4; ++y6)
        {
          num68 = y6 - firstY + 21;
          for (int x7 = firstX - 2; x7 < lastX + 2; ++x7)
          {
            num69 = x7 - firstX + 21;
            if (Game1.tile[x7, y6].wall > (byte) 0 && (double) Lighting.Brightness(x7, y6) > 0.0)
            {
              if (Game1.tile[x7, y6].wallFrameY == (byte) 18 && Game1.tile[x7, y6].wallFrameX >= (byte) 18)
              {
                int wallFrameY = (int) Game1.tile[x7, y6].wallFrameY;
              }
              Rectangle rectangle3 = new Rectangle((int) Game1.tile[x7, y6].wallFrameX * 2, (int) Game1.tile[x7, y6].wallFrameY * 2, 32, 32);
              this.spriteBatch.Draw(Game1.wallTexture[(int) Game1.tile[x7, y6].wall], new Vector2((float) (x7 * 16 - (int) Game1.screenPosition.X - 8), (float) (y6 * 16 - (int) Game1.screenPosition.Y - 8)), new Rectangle?(rectangle3), Lighting.GetColor(x7, y6), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
            }
          }
        }
        this.DrawTiles(false);
        this.DrawNPCs(true);
        this.DrawTiles();
        this.DrawGore();
        this.DrawNPCs();
        for (int index = 0; index < 1000; ++index)
        {
          if (Game1.projectile[index].active && Game1.projectile[index].type > 0)
          {
            Vector2 vector2;
            if (Game1.projectile[index].type == 32)
            {
              vector2 = new Vector2(Game1.projectile[index].position.X + (float) Game1.projectile[index].width * 0.5f, Game1.projectile[index].position.Y + (float) Game1.projectile[index].height * 0.5f);
              float x8 = Game1.player[Game1.projectile[index].owner].position.X + (float) (Game1.player[Game1.projectile[index].owner].width / 2) - vector2.X;
              float y7 = Game1.player[Game1.projectile[index].owner].position.Y + (float) (Game1.player[Game1.projectile[index].owner].height / 2) - vector2.Y;
              float rotation3 = (float) Math.Atan2((double) y7, (double) x8) - 1.57f;
              bool flag3 = true;
              if ((double) x8 == 0.0 && (double) y7 == 0.0)
              {
                flag3 = false;
              }
              else
              {
                float num70 = 8f / (float) Math.Sqrt((double) x8 * (double) x8 + (double) y7 * (double) y7);
                float num71 = x8 * num70;
                float num72 = y7 * num70;
                vector2.X -= num71;
                vector2.Y -= num72;
                x8 = Game1.player[Game1.projectile[index].owner].position.X + (float) (Game1.player[Game1.projectile[index].owner].width / 2) - vector2.X;
                y7 = Game1.player[Game1.projectile[index].owner].position.Y + (float) (Game1.player[Game1.projectile[index].owner].height / 2) - vector2.Y;
              }
              while (flag3)
              {
                float num73 = (float) Math.Sqrt((double) x8 * (double) x8 + (double) y7 * (double) y7);
                if ((double) num73 < 28.0)
                {
                  flag3 = false;
                }
                else
                {
                  float num74 = 28f / num73;
                  float num75 = x8 * num74;
                  float num76 = y7 * num74;
                  vector2.X += num75;
                  vector2.Y += num76;
                  x8 = Game1.player[Game1.projectile[index].owner].position.X + (float) (Game1.player[Game1.projectile[index].owner].width / 2) - vector2.X;
                  y7 = Game1.player[Game1.projectile[index].owner].position.Y + (float) (Game1.player[Game1.projectile[index].owner].height / 2) - vector2.Y;
                  Color color = Lighting.GetColor((int) vector2.X / 16, (int) ((double) vector2.Y / 16.0));
                  this.spriteBatch.Draw(Game1.chain5Texture, new Vector2(vector2.X - Game1.screenPosition.X, vector2.Y - Game1.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, Game1.chain5Texture.Width, Game1.chain5Texture.Height)), color, rotation3, new Vector2((float) Game1.chain5Texture.Width * 0.5f, (float) Game1.chain5Texture.Height * 0.5f), 1f, SpriteEffects.None, 0.0f);
                }
              }
            }
            else if (Game1.projectile[index].aiStyle == 7)
            {
              vector2 = new Vector2(Game1.projectile[index].position.X + (float) Game1.projectile[index].width * 0.5f, Game1.projectile[index].position.Y + (float) Game1.projectile[index].height * 0.5f);
              float x9 = Game1.player[Game1.projectile[index].owner].position.X + (float) (Game1.player[Game1.projectile[index].owner].width / 2) - vector2.X;
              float y8 = Game1.player[Game1.projectile[index].owner].position.Y + (float) (Game1.player[Game1.projectile[index].owner].height / 2) - vector2.Y;
              float rotation4 = (float) Math.Atan2((double) y8, (double) x9) - 1.57f;
              bool flag4 = true;
              while (flag4)
              {
                float num77 = (float) Math.Sqrt((double) x9 * (double) x9 + (double) y8 * (double) y8);
                if ((double) num77 < 25.0)
                {
                  flag4 = false;
                }
                else
                {
                  float num78 = 12f / num77;
                  float num79 = x9 * num78;
                  float num80 = y8 * num78;
                  vector2.X += num79;
                  vector2.Y += num80;
                  x9 = Game1.player[Game1.projectile[index].owner].position.X + (float) (Game1.player[Game1.projectile[index].owner].width / 2) - vector2.X;
                  y8 = Game1.player[Game1.projectile[index].owner].position.Y + (float) (Game1.player[Game1.projectile[index].owner].height / 2) - vector2.Y;
                  Color color = Lighting.GetColor((int) vector2.X / 16, (int) ((double) vector2.Y / 16.0));
                  this.spriteBatch.Draw(Game1.chainTexture, new Vector2(vector2.X - Game1.screenPosition.X, vector2.Y - Game1.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, Game1.chainTexture.Width, Game1.chainTexture.Height)), color, rotation4, new Vector2((float) Game1.chainTexture.Width * 0.5f, (float) Game1.chainTexture.Height * 0.5f), 1f, SpriteEffects.None, 0.0f);
                }
              }
            }
            else if (Game1.projectile[index].aiStyle == 13)
            {
              float num81 = Game1.projectile[index].position.X + 8f;
              float num82 = Game1.projectile[index].position.Y + 2f;
              float x10 = Game1.projectile[index].velocity.X;
              float y9 = Game1.projectile[index].velocity.Y;
              float num83 = 20f / (float) Math.Sqrt((double) x10 * (double) x10 + (double) y9 * (double) y9);
              float x11;
              float y10;
              if ((double) Game1.projectile[index].ai[0] == 0.0)
              {
                x11 = num81 - Game1.projectile[index].velocity.X * num83;
                y10 = num82 - Game1.projectile[index].velocity.Y * num83;
              }
              else
              {
                x11 = num81 + Game1.projectile[index].velocity.X * num83;
                y10 = num82 + Game1.projectile[index].velocity.Y * num83;
              }
              vector2 = new Vector2(x11, y10);
              float x12 = Game1.player[Game1.projectile[index].owner].position.X + (float) (Game1.player[Game1.projectile[index].owner].width / 2) - vector2.X;
              float y11 = Game1.player[Game1.projectile[index].owner].position.Y + (float) (Game1.player[Game1.projectile[index].owner].height / 2) - vector2.Y;
              float rotation5 = (float) Math.Atan2((double) y11, (double) x12) - 1.57f;
              if (Game1.projectile[index].alpha == 0)
                Game1.player[Game1.projectile[index].owner].itemRotation = Game1.player[Game1.projectile[index].owner].direction != 1 ? rotation5 + 1.57f : rotation5 - 1.57f;
              bool flag5 = true;
              while (flag5)
              {
                float num84 = (float) Math.Sqrt((double) x12 * (double) x12 + (double) y11 * (double) y11);
                if ((double) num84 < 25.0)
                {
                  flag5 = false;
                }
                else
                {
                  float num85 = 12f / num84;
                  float num86 = x12 * num85;
                  float num87 = y11 * num85;
                  vector2.X += num86;
                  vector2.Y += num87;
                  x12 = Game1.player[Game1.projectile[index].owner].position.X + (float) (Game1.player[Game1.projectile[index].owner].width / 2) - vector2.X;
                  y11 = Game1.player[Game1.projectile[index].owner].position.Y + (float) (Game1.player[Game1.projectile[index].owner].height / 2) - vector2.Y;
                  Color color = Lighting.GetColor((int) vector2.X / 16, (int) ((double) vector2.Y / 16.0));
                  this.spriteBatch.Draw(Game1.chainTexture, new Vector2(vector2.X - Game1.screenPosition.X, vector2.Y - Game1.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, Game1.chainTexture.Width, Game1.chainTexture.Height)), color, rotation5, new Vector2((float) Game1.chainTexture.Width * 0.5f, (float) Game1.chainTexture.Height * 0.5f), 1f, SpriteEffects.None, 0.0f);
                }
              }
            }
            else if (Game1.projectile[index].aiStyle == 15)
            {
              vector2 = new Vector2(Game1.projectile[index].position.X + (float) Game1.projectile[index].width * 0.5f, Game1.projectile[index].position.Y + (float) Game1.projectile[index].height * 0.5f);
              float x13 = Game1.player[Game1.projectile[index].owner].position.X + (float) (Game1.player[Game1.projectile[index].owner].width / 2) - vector2.X;
              float y12 = Game1.player[Game1.projectile[index].owner].position.Y + (float) (Game1.player[Game1.projectile[index].owner].height / 2) - vector2.Y;
              float rotation6 = (float) Math.Atan2((double) y12, (double) x13) - 1.57f;
              if (Game1.projectile[index].alpha == 0)
                Game1.player[Game1.projectile[index].owner].itemRotation = Game1.player[Game1.projectile[index].owner].direction != 1 ? rotation6 + 1.57f : rotation6 - 1.57f;
              bool flag6 = true;
              while (flag6)
              {
                float num88 = (float) Math.Sqrt((double) x13 * (double) x13 + (double) y12 * (double) y12);
                if ((double) num88 < 25.0)
                {
                  flag6 = false;
                }
                else
                {
                  float num89 = 12f / num88;
                  float num90 = x13 * num89;
                  float num91 = y12 * num89;
                  vector2.X += num90;
                  vector2.Y += num91;
                  x13 = Game1.player[Game1.projectile[index].owner].position.X + (float) (Game1.player[Game1.projectile[index].owner].width / 2) - vector2.X;
                  y12 = Game1.player[Game1.projectile[index].owner].position.Y + (float) (Game1.player[Game1.projectile[index].owner].height / 2) - vector2.Y;
                  Color color = Lighting.GetColor((int) vector2.X / 16, (int) ((double) vector2.Y / 16.0));
                  if (Game1.projectile[index].type == 25)
                    this.spriteBatch.Draw(Game1.chain2Texture, new Vector2(vector2.X - Game1.screenPosition.X, vector2.Y - Game1.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, Game1.chain2Texture.Width, Game1.chain2Texture.Height)), color, rotation6, new Vector2((float) Game1.chain2Texture.Width * 0.5f, (float) Game1.chain2Texture.Height * 0.5f), 1f, SpriteEffects.None, 0.0f);
                  else
                    this.spriteBatch.Draw(Game1.chain3Texture, new Vector2(vector2.X - Game1.screenPosition.X, vector2.Y - Game1.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, Game1.chain3Texture.Width, Game1.chain3Texture.Height)), color, rotation6, new Vector2((float) Game1.chain3Texture.Width * 0.5f, (float) Game1.chain3Texture.Height * 0.5f), 1f, SpriteEffects.None, 0.0f);
                }
              }
            }
            Color newColor = Lighting.GetColor((int) ((double) Game1.projectile[index].position.X + (double) Game1.projectile[index].width * 0.5) / 16, (int) (((double) Game1.projectile[index].position.Y + (double) Game1.projectile[index].height * 0.5) / 16.0));
            if (Game1.projectile[index].type == 14)
              newColor = Color.White;
            int num92 = 0;
            if (Game1.projectile[index].type == 16)
              num92 = 6;
            if (Game1.projectile[index].type == 17 || Game1.projectile[index].type == 31)
              num92 = 2;
            if (Game1.projectile[index].type == 25 || Game1.projectile[index].type == 26 || Game1.projectile[index].type == 30)
              num92 = 6;
            if (Game1.projectile[index].type == 28)
              num92 = 8;
            if (Game1.projectile[index].type == 29)
              num92 = 11;
            float x14 = (float) ((double) (Game1.projectileTexture[Game1.projectile[index].type].Width - Game1.projectile[index].width) * 0.5 + (double) Game1.projectile[index].width * 0.5);
            this.spriteBatch.Draw(Game1.projectileTexture[Game1.projectile[index].type], new Vector2(Game1.projectile[index].position.X - Game1.screenPosition.X + x14, Game1.projectile[index].position.Y - Game1.screenPosition.Y + (float) (Game1.projectile[index].height / 2)), new Rectangle?(new Rectangle(0, 0, Game1.projectileTexture[Game1.projectile[index].type].Width, Game1.projectileTexture[Game1.projectile[index].type].Height)), Game1.projectile[index].GetAlpha(newColor), Game1.projectile[index].rotation, new Vector2(x14, (float) (Game1.projectile[index].height / 2 + num92)), Game1.projectile[index].scale, SpriteEffects.None, 0.0f);
          }
        }
        for (int index = 0; index < 8; ++index)
        {
          if (Game1.player[index].active)
          {
            if (Game1.player[index].head == 6 && Game1.player[index].body == 4 && Game1.player[index].legs == 4 || Game1.player[index].head == 8 && Game1.player[index].body == 6 && Game1.player[index].legs == 6)
            {
              Vector2 position = Game1.player[index].position;
              Game1.player[index].position = Game1.player[index].shadowPos[0];
              Game1.player[index].shadow = 0.5f;
              this.DrawPlayer(Game1.player[index]);
              Game1.player[index].position = Game1.player[index].shadowPos[1];
              Game1.player[index].shadow = 0.7f;
              this.DrawPlayer(Game1.player[index]);
              Game1.player[index].position = Game1.player[index].shadowPos[2];
              Game1.player[index].shadow = 0.9f;
              this.DrawPlayer(Game1.player[index]);
              Game1.player[index].position = position;
              Game1.player[index].shadow = 0.0f;
            }
            this.DrawPlayer(Game1.player[index]);
          }
        }
        for (int index = 0; index < 200; ++index)
        {
          if (Game1.item[index].active && Game1.item[index].type > 0)
          {
            int num93 = (int) ((double) Game1.item[index].position.X + (double) Game1.item[index].width * 0.5) / 16 - firstX + 21;
            int num94 = (int) ((double) Game1.item[index].position.Y + (double) Game1.item[index].height * 0.5) / 16 - firstY + 21;
            Color color = Lighting.GetColor((int) ((double) Game1.item[index].position.X + (double) Game1.item[index].width * 0.5) / 16, (int) ((double) Game1.item[index].position.Y + (double) Game1.item[index].height * 0.5) / 16);
            this.spriteBatch.Draw(Game1.itemTexture[Game1.item[index].type], new Vector2(Game1.item[index].position.X - Game1.screenPosition.X + (float) (Game1.item[index].width / 2) - (float) (Game1.itemTexture[Game1.item[index].type].Width / 2), Game1.item[index].position.Y - Game1.screenPosition.Y + (float) (Game1.item[index].height / 2) - (float) (Game1.itemTexture[Game1.item[index].type].Height / 2)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.item[index].type].Width, Game1.itemTexture[Game1.item[index].type].Height)), Game1.item[index].GetAlpha(color), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
            if (Game1.item[index].color != new Color())
              this.spriteBatch.Draw(Game1.itemTexture[Game1.item[index].type], new Vector2(Game1.item[index].position.X - Game1.screenPosition.X + (float) (Game1.item[index].width / 2) - (float) (Game1.itemTexture[Game1.item[index].type].Width / 2), Game1.item[index].position.Y - Game1.screenPosition.Y + (float) (Game1.item[index].height / 2) - (float) (Game1.itemTexture[Game1.item[index].type].Height / 2)), new Rectangle?(new Rectangle(0, 0, Game1.itemTexture[Game1.item[index].type].Width, Game1.itemTexture[Game1.item[index].type].Height)), Game1.item[index].GetColor(color), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
          }
        }
        Rectangle rectangle4 = new Rectangle((int) Game1.screenPosition.X - 50, (int) Game1.screenPosition.Y - 50, Game1.screenWidth + 100, Game1.screenHeight + 100);
        for (int index = 0; index < 2000; ++index)
        {
          if (Game1.dust[index].active)
          {
            if (new Rectangle((int) Game1.dust[index].position.X, (int) Game1.dust[index].position.Y, 4, 4).Intersects(rectangle4))
            {
              Color newColor = Lighting.GetColor((int) ((double) Game1.dust[index].position.X + 4.0) / 16, (int) ((double) Game1.dust[index].position.Y + 4.0) / 16);
              if (Game1.dust[index].type == 6 || Game1.dust[index].type == 15 || Game1.dust[index].noLight)
                newColor = Color.White;
              this.spriteBatch.Draw(Game1.dustTexture, Game1.dust[index].position - Game1.screenPosition, new Rectangle?(Game1.dust[index].frame), Game1.dust[index].GetAlpha(newColor), Game1.dust[index].rotation, new Vector2(4f, 4f), Game1.dust[index].scale, SpriteEffects.None, 0.0f);
              if (Game1.dust[index].color != new Color())
                this.spriteBatch.Draw(Game1.dustTexture, Game1.dust[index].position - Game1.screenPosition, new Rectangle?(Game1.dust[index].frame), Game1.dust[index].GetColor(newColor), Game1.dust[index].rotation, new Vector2(4f, 4f), Game1.dust[index].scale, SpriteEffects.None, 0.0f);
            }
            else
              Game1.dust[index].active = false;
          }
        }
        this.DrawWater();
        if (!Game1.hideUI)
        {
          for (int index13 = 0; index13 < 8; ++index13)
          {
            if (Game1.player[index13].active && Game1.player[index13].chatShowTime > 0 && index13 != Game1.myPlayer && !Game1.player[index13].dead)
            {
              Vector2 vector2_1 = Game1.fontMouseText.MeasureString(Game1.player[index13].chatText);
              Vector2 vector2_2;
              vector2_2.X = (float) ((double) Game1.player[index13].position.X + (double) (Game1.player[index13].width / 2) - (double) vector2_1.X / 2.0);
              vector2_2.Y = (float) ((double) Game1.player[index13].position.Y - (double) vector2_1.Y - 2.0);
              for (int index14 = 0; index14 < 5; ++index14)
              {
                int num95 = 0;
                int num96 = 0;
                Color color = Color.Black;
                if (index14 == 0)
                  num95 = -2;
                if (index14 == 1)
                  num95 = 2;
                if (index14 == 2)
                  num96 = -2;
                if (index14 == 3)
                  num96 = 2;
                if (index14 == 4)
                  color = new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor);
                this.spriteBatch.DrawString(Game1.fontMouseText, Game1.player[index13].chatText, new Vector2(vector2_2.X + (float) num95 - Game1.screenPosition.X, vector2_2.Y + (float) num96 - Game1.screenPosition.Y), color, 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
              }
            }
          }
          for (int index15 = 0; index15 < 100; ++index15)
          {
            if (Game1.combatText[index15].active)
            {
              Vector2 vector2 = Game1.fontCombatText.MeasureString(Game1.combatText[index15].text);
              Vector2 origin = new Vector2(vector2.X * 0.5f, vector2.Y * 0.5f);
              int num97 = (int) ((double) byte.MaxValue - (double) byte.MaxValue * (double) Game1.combatText[index15].scale);
              float r4 = (float) Game1.combatText[index15].color.R;
              float g4 = (float) Game1.combatText[index15].color.G;
              float b4 = (float) Game1.combatText[index15].color.B;
              float a1 = (float) Game1.combatText[index15].color.A;
              float r5 = r4 * (float) ((double) Game1.combatText[index15].scale * (double) Game1.combatText[index15].alpha * 0.30000001192092896);
              float b5 = b4 * (float) ((double) Game1.combatText[index15].scale * (double) Game1.combatText[index15].alpha * 0.30000001192092896);
              float g5 = g4 * (float) ((double) Game1.combatText[index15].scale * (double) Game1.combatText[index15].alpha * 0.30000001192092896);
              float a2 = a1 * (Game1.combatText[index15].scale * Game1.combatText[index15].alpha);
              Color color = new Color((int) r5, (int) g5, (int) b5, (int) a2);
              for (int index16 = 0; index16 < 5; ++index16)
              {
                int num98 = 0;
                int num99 = 0;
                if (index16 == 0)
                  --num98;
                else if (index16 == 1)
                  ++num98;
                else if (index16 == 2)
                  --num99;
                else if (index16 == 3)
                {
                  ++num99;
                }
                else
                {
                  float r6 = (float) Game1.combatText[index15].color.R * Game1.combatText[index15].scale * Game1.combatText[index15].alpha;
                  float b6 = (float) Game1.combatText[index15].color.B * Game1.combatText[index15].scale * Game1.combatText[index15].alpha;
                  float g6 = (float) Game1.combatText[index15].color.G * Game1.combatText[index15].scale * Game1.combatText[index15].alpha;
                  float a3 = (float) Game1.combatText[index15].color.A * Game1.combatText[index15].scale * Game1.combatText[index15].alpha;
                  color = new Color((int) r6, (int) g6, (int) b6, (int) a3);
                }
                this.spriteBatch.DrawString(Game1.fontCombatText, Game1.combatText[index15].text, new Vector2(Game1.combatText[index15].position.X - Game1.screenPosition.X + (float) num98 + origin.X, Game1.combatText[index15].position.Y - Game1.screenPosition.Y + (float) num99 + origin.Y), color, Game1.combatText[index15].rotation, origin, Game1.combatText[index15].scale, SpriteEffects.None, 0.0f);
              }
            }
          }
          if (Game1.netMode == 1 && Netplay.clientSock.statusText != "" && Netplay.clientSock.statusText != null)
          {
            string text = Netplay.clientSock.statusText + ": " + (object) (int) ((double) Netplay.clientSock.statusCount / (double) Netplay.clientSock.statusMax * 100.0) + "%";
            this.spriteBatch.DrawString(Game1.fontMouseText, text, new Vector2((float) (628.0 - (double) Game1.fontMouseText.MeasureString(text).X * 0.5), 84f), new Color((int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor, (int) Game1.mouseTextColor), 0.0f, new Vector2(), 1f, SpriteEffects.None, 0.0f);
          }
          this.DrawFPS();
          this.DrawInterface();
        }
        this.spriteBatch.End();
        
        //RnD
        Game1.mouseLeftRelease = (Game1.mouseState.Count == 0);//Game1.mouseState.LeftButton != ButtonState.Pressed;
        Game1.mouseRightRelease = (Game1.mouseState.Count == 0);//Game1.mouseState.RightButton != ButtonState.Pressed;
        if (Game1.mouseState.Count < 2//Game1.mouseState.RightButton != ButtonState.Pressed
        )
          Game1.stackSplit = 0;
        if (Game1.stackSplit <= 0)
          return;
        --Game1.stackSplit;
      }
    }

    private static void UpdateInvasion()
    {
      if (Game1.invasionType <= 0)
        return;
      if (Game1.invasionSize <= 0)
      {
        Game1.InvasionWarning();
        Game1.invasionType = 0;
        Game1.invasionDelay = 7;
      }
      if (Game1.invasionX != (double) Game1.spawnTileX)
      {
        float num = 0.2f;
        if (Game1.invasionX > (double) Game1.spawnTileX)
        {
          Game1.invasionX -= (double) num;
          if (Game1.invasionX <= (double) Game1.spawnTileX)
          {
            Game1.invasionX = (double) Game1.spawnTileX;
            Game1.InvasionWarning();
          }
          else
            --Game1.invasionWarn;
        }
        else if (Game1.invasionX < (double) Game1.spawnTileX)
        {
          Game1.invasionX += (double) num;
          if (Game1.invasionX >= (double) Game1.spawnTileX)
          {
            Game1.invasionX = (double) Game1.spawnTileX;
            Game1.InvasionWarning();
          }
          else
            --Game1.invasionWarn;
        }
        if (Game1.invasionWarn <= 0)
        {
          Game1.invasionWarn = 3600;
          Game1.InvasionWarning();
        }
      }
    }

    private static void InvasionWarning()
    {
      if (Game1.invasionType == 0)
        return;
      string str = Game1.invasionSize > 0 ? (Game1.invasionX >= (double) Game1.spawnTileX ? (Game1.invasionX <= (double) Game1.spawnTileX ? "The goblin army has arrived!" : "A goblin army is approaching from the east!") : "A goblin army is approaching from the west!") : "The goblin army has been defeated!";
      if (Game1.netMode == 0)
      {
        Game1.NewText(str, (byte) 175, (byte) 75);
      }
      else
      {
        if (Game1.netMode != 2)
          return;
        NetMessage.SendData(25, text: str, number: 8, number2: 175f, number3: 75f, number4: (float) byte.MaxValue);
      }
    }

    private static void StartInvasion()
    {
      if (!WorldGen.shadowOrbSmashed || Game1.invasionType != 0 || Game1.invasionDelay != 0)
        return;
      int num = 0;
      for (int index = 0; index < 8; ++index)
      {
        if (Game1.player[index].active && Game1.player[index].statLife >= 200)
          ++num;
      }
      if (num > 0)
      {
        Game1.invasionType = 1;
        Game1.invasionSize = 100 + 50 * num;
        Game1.invasionWarn = 0;
        Game1.invasionX = Game1.rand.Next(2) != 0 ? (double) Game1.maxTilesX : 0.0;
      }
    }

    private static void UpdateClient()
    {
      if (Game1.myPlayer == 8)
        Netplay.disconnect = true;
      ++Game1.netPlayCounter;
      if (Game1.netPlayCounter > 3600)
        Game1.netPlayCounter = 0;
      if (Math.IEEERemainder((double) Game1.netPlayCounter, 300.0) == 0.0)
      {
        NetMessage.SendData(13, number: Game1.myPlayer);
        NetMessage.SendData(36, number: Game1.myPlayer);
      }
      if (Math.IEEERemainder((double) Game1.netPlayCounter, 600.0) == 0.0)
      {
        NetMessage.SendData(16, number: Game1.myPlayer);
        NetMessage.SendData(40, number: Game1.myPlayer);
      }
      if (Netplay.clientSock.active)
      {
        ++Netplay.clientSock.timeOut;
        if (!Game1.stopTimeOuts && Netplay.clientSock.timeOut > 60 * Game1.timeOut)
        {
          Game1.statusText = "Connection timed out";
          Netplay.disconnect = true;
        }
      }
      for (int whoAmI = 0; whoAmI < 200; ++whoAmI)
      {
        if (Game1.item[whoAmI].active && Game1.item[whoAmI].owner == Game1.myPlayer)
          Game1.item[whoAmI].FindOwner(whoAmI);
      }
    }

    private static void UpdateServer()
    {
      ++Game1.netPlayCounter;
      if (Game1.netPlayCounter > 3600)
      {
        NetMessage.SendData(7);
        NetMessage.syncPlayers();
        Game1.netPlayCounter = 0;
      }
      Math.IEEERemainder((double) Game1.netPlayCounter, 60.0);
      if (Math.IEEERemainder((double) Game1.netPlayCounter, 360.0) == 0.0)
      {
        bool flag = true;
        int number = Game1.lastItemUpdate;
        int num = 0;
        while (flag)
        {
          ++number;
          if (number >= 200)
            number = 0;
          ++num;
          if (!Game1.item[number].active || Game1.item[number].owner == 8)
            NetMessage.SendData(21, number: number);
          if (num >= Game1.maxItemUpdates || number == Game1.lastItemUpdate)
            flag = false;
        }
        Game1.lastItemUpdate = number;
      }
      for (int whoAmI = 0; whoAmI < 200; ++whoAmI)
      {
        if (Game1.item[whoAmI].active && (Game1.item[whoAmI].owner == 8 || !Game1.player[Game1.item[whoAmI].owner].active))
          Game1.item[whoAmI].FindOwner(whoAmI);
      }
      for (int index1 = 0; index1 < 8; ++index1)
      {
        if (Netplay.serverSock[index1].active)
        {
          ++Netplay.serverSock[index1].timeOut;
          if (!Game1.stopTimeOuts && Netplay.serverSock[index1].timeOut > 60 * Game1.timeOut)
            Netplay.serverSock[index1].kill = true;
        }
        if (Game1.player[index1].active)
        {
          int sectionX = Netplay.GetSectionX((int) ((double) Game1.player[index1].position.X / 16.0));
          int sectionY = Netplay.GetSectionY((int) ((double) Game1.player[index1].position.Y / 16.0));
          int num = 0;
          for (int index2 = sectionX - 1; index2 < sectionX + 2; ++index2)
          {
            for (int index3 = sectionY - 1; index3 < sectionY + 2; ++index3)
            {
              if (index2 >= 0 && index2 < Game1.maxSectionsX && index3 >= 0 && index3 < Game1.maxSectionsY && !Netplay.serverSock[index1].tileSection[index2, index3])
                ++num;
            }
          }
          if (num > 0)
          {
            int number = num * 150;
            NetMessage.SendData(9, index1, text: "Recieving tile data", number: number);
            Netplay.serverSock[index1].statusText2 = "is recieving tile data";
            Netplay.serverSock[index1].statusMax += number;
            for (int index4 = sectionX - 1; index4 < sectionX + 2; ++index4)
            {
              for (int index5 = sectionY - 1; index5 < sectionY + 2; ++index5)
              {
                if (index4 >= 0 && index4 < Game1.maxSectionsX && index5 >= 0 && index5 < Game1.maxSectionsY && !Netplay.serverSock[index1].tileSection[index4, index5])
                {
                  NetMessage.SendSection(index1, index4, index5);
                  NetMessage.SendData(11, index1, number: index4, number2: (float) index5, number3: (float) index4, number4: (float) index5);
                }
              }
            }
          }
        }
      }
    }

    public static void NewText(string newText, byte R = 255, byte G = 255, byte B = 255)
    {
      for (int index = Game1.numChatLines - 1; index > 0; --index)
      {
        Game1.chatLine[index].text = Game1.chatLine[index - 1].text;
        Game1.chatLine[index].showTime = Game1.chatLine[index - 1].showTime;
        Game1.chatLine[index].color = Game1.chatLine[index - 1].color;
      }
      int num = R != (byte) 0 || G != (byte) 0 ? 1 : (B != (byte) 0 ? 1 : 0);
      Game1.chatLine[0].color = num != 0 ? new Color((int) R, (int) G, (int) B) : Color.White;
      Game1.chatLine[0].text = newText;
      Game1.chatLine[0].showTime = Game1.chatLength;
      Game1.PlaySound(12);
    }

    private static void UpdateTime()
    {
      ++Game1.time;
      if (!Game1.dayTime)
      {
        if (Game1.time > 32400.0)
        {
          if (Game1.invasionDelay > 0)
            --Game1.invasionDelay;
          WorldGen.spawnNPC = 0;
          Game1.checkForSpawns = 0;
          Game1.time = 0.0;
          Game1.bloodMoon = false;
          Game1.dayTime = true;
          ++Game1.moonPhase;
          if (Game1.moonPhase >= 8)
            Game1.moonPhase = 0;
          if (Game1.netMode == 2)
          {
            NetMessage.SendData(7);
            WorldGen.saveAndPlay();
          }
          if (Game1.netMode != 1 && Game1.rand.Next(15) == 0)
            Game1.StartInvasion();
        }
        if (Game1.time <= 16200.0 || !WorldGen.spawnMeteor)
          return;
        WorldGen.spawnMeteor = false;
        WorldGen.dropMeteor();
      }
      else
      {
        if (Game1.time > 54000.0)
        {
          WorldGen.spawnNPC = 0;
          Game1.checkForSpawns = 0;
          if (Game1.rand.Next(50) == 0 && Game1.netMode != 1 && WorldGen.shadowOrbSmashed)
            WorldGen.spawnMeteor = true;
          if (Game1.moonPhase != 4 && Game1.rand.Next(7) == 0 && Game1.netMode != 1)
          {
            Game1.bloodMoon = true;
            if (Game1.netMode == 0)
              Game1.NewText("The Blood Moon is rising...", (byte) 50, B: (byte) 130);
            else if (Game1.netMode == 2)
              NetMessage.SendData(25, text: "The Blood Moon is rising...", number: 8, number2: 50f, number3: (float) byte.MaxValue, number4: 130f);
          }
          Game1.time = 0.0;
          Game1.dayTime = false;
          if (Game1.netMode == 2)
            NetMessage.SendData(7);
        }
        if (Game1.netMode != 1)
        {
          ++Game1.checkForSpawns;
          if (Game1.checkForSpawns >= 7200)
          {
            Game1.checkForSpawns = 0;
            WorldGen.spawnNPC = 0;
            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            int num6 = 0;
            int num7 = 0;
            int num8 = 0;
            for (int npc = 0; npc < 1000; ++npc)
            {
              if (Game1.npc[npc].active && Game1.npc[npc].townNPC)
              {
                if (Game1.npc[npc].type != 37 && !Game1.npc[npc].homeless)
                  WorldGen.QuickFindHome(npc);
                else
                  ++num6;
                if (Game1.npc[npc].type == 17)
                  ++num1;
                if (Game1.npc[npc].type == 18)
                  ++num2;
                if (Game1.npc[npc].type == 19)
                  ++num4;
                if (Game1.npc[npc].type == 20)
                  ++num3;
                if (Game1.npc[npc].type == 22)
                  ++num5;
                if (Game1.npc[npc].type == 38)
                  ++num7;
                ++num8;
              }
            }
            if (WorldGen.spawnNPC == 0)
            {
              int num9 = 0;
              bool flag1 = false;
              int num10 = 0;
              bool flag2 = false;
              bool flag3 = false;
              for (int index1 = 0; index1 < 8; ++index1)
              {
                if (Game1.player[index1].active)
                {
                  for (int index2 = 0; index2 < 44; ++index2)
                  {
                    if (Game1.player[index1].inventory[index2] != null & Game1.player[index1].inventory[index2].stack > 0)
                    {
                      if (Game1.player[index1].inventory[index2].type == 71)
                        num9 += Game1.player[index1].inventory[index2].stack;
                      if (Game1.player[index1].inventory[index2].type == 72)
                        num9 += Game1.player[index1].inventory[index2].stack * 100;
                      if (Game1.player[index1].inventory[index2].type == 73)
                        num9 += Game1.player[index1].inventory[index2].stack * 10000;
                      if (Game1.player[index1].inventory[index2].type == 74)
                        num9 += Game1.player[index1].inventory[index2].stack * 1000000;
                      if (Game1.player[index1].inventory[index2].type == 95 || Game1.player[index1].inventory[index2].type == 96 || Game1.player[index1].inventory[index2].type == 97 || Game1.player[index1].inventory[index2].type == 98 || Game1.player[index1].inventory[index2].useAmmo == 14)
                        flag2 = true;
                      if (Game1.player[index1].inventory[index2].type == 166 || Game1.player[index1].inventory[index2].type == 167)
                        flag3 = true;
                    }
                  }
                  int num11 = Game1.player[index1].statLifeMax / 20;
                  if (num11 > 5)
                    flag1 = true;
                  num10 += num11;
                }
              }
              if (WorldGen.spawnNPC == 0 && num5 < 1)
                WorldGen.spawnNPC = 22;
              if (WorldGen.spawnNPC == 0 && (double) num9 > 5000.0 && num1 < 1)
                WorldGen.spawnNPC = 17;
              if (WorldGen.spawnNPC == 0 && flag1 && num2 < 1)
                WorldGen.spawnNPC = 18;
              if (WorldGen.spawnNPC == 0 && flag2 && num4 < 1)
                WorldGen.spawnNPC = 19;
              if (WorldGen.spawnNPC == 0 && (NPC.downedBoss1 || NPC.downedBoss2) && num3 < 1)
                WorldGen.spawnNPC = 20;
              if (WorldGen.spawnNPC == 0 && flag3 && num1 > 0 && num7 < 1)
                WorldGen.spawnNPC = 38;
              if (WorldGen.spawnNPC == 0 && num9 > 100000 && num1 < 2)
                WorldGen.spawnNPC = 17;
              if (WorldGen.spawnNPC == 0 && num10 >= 20 && num2 < 2)
                WorldGen.spawnNPC = 18;
              if (WorldGen.spawnNPC == 0 && num9 > 5000000 && num1 < 3)
                WorldGen.spawnNPC = 17;
              if (!NPC.downedBoss3 && num6 == 0)
              {
                int index = NPC.NewNPC(Game1.dungeonX * 16 + 8, Game1.dungeonY * 16, 37);
                Game1.npc[index].homeless = false;
                Game1.npc[index].homeTileX = Game1.dungeonX;
                Game1.npc[index].homeTileY = Game1.dungeonY;
              }
            }
          }
        }
      }
    }

    public static double CalculateDamage(int Damage, int Defense)
    {
      double damage = (double) Damage - (double) Defense * 0.5;
      if (damage < 1.0)
        damage = 1.0;
      return damage;
    }

    public static void PlaySound(int type, int x = -1, int y = -1, int style = 1)
    {
      if ((double) Game1.soundVolume == 0.0)
        return;
      bool flag = false;
      float num1 = 1f;
      float num2 = 0.0f;
      if (x == -1 || y == -1)
      {
        flag = true;
      }
      else
      {
        if (WorldGen.gen || Game1.netMode == 2)
          return;
        Rectangle rectangle1 = new Rectangle((int) ((double) Game1.screenPosition.X - (double) (Game1.screenWidth * 2)), (int) ((double) Game1.screenPosition.Y - (double) (Game1.screenHeight * 2)), Game1.screenWidth * 5, Game1.screenHeight * 5);
        Rectangle rectangle2 = new Rectangle(x, y, 1, 1);
        Vector2 vector2 = new Vector2(Game1.screenPosition.X + (float) Game1.screenWidth * 0.5f, Game1.screenPosition.Y + (float) Game1.screenHeight * 0.5f);
        if (rectangle2.Intersects(rectangle1))
          flag = true;
        if (flag)
        {
          num2 = (float) (((double) x - (double) vector2.X) / ((double) Game1.screenWidth * 0.5));
          float num3 = Math.Abs((float) x - vector2.X);
          float num4 = Math.Abs((float) y - vector2.Y);
          num1 = (float) (1.0 - Math.Sqrt((double) num3 * (double) num3 + (double) num4 * (double) num4) / ((double) Game1.screenWidth * 1.5));
        }
      }
      if ((double) num2 < -1.0)
        num2 = -1f;
      if ((double) num2 > 1.0)
        num2 = 1f;
      if ((double) num1 > 1.0)
        num1 = 1f;
      if ((double) num1 <= 0.0 || !flag)
        return;
      float num5 = num1 * Game1.soundVolume;
      switch (type)
      {
        case 0:
          int index1 = Game1.rand.Next(3);
          Game1.soundInstanceDig[index1].Stop();
          Game1.soundInstanceDig[index1] = Game1.soundDig[index1].CreateInstance();
          Game1.soundInstanceDig[index1].Volume = num5;
          Game1.soundInstanceDig[index1].Pan = num2;
          Game1.soundInstanceDig[index1].Play();
          break;
        case 1:
          int index2 = Game1.rand.Next(3);
          Game1.soundInstancePlayerHit[index2].Stop();
          Game1.soundInstancePlayerHit[index2] = Game1.soundPlayerHit[index2].CreateInstance();
          Game1.soundInstancePlayerHit[index2].Volume = num5;
          Game1.soundInstancePlayerHit[index2].Pan = num2;
          Game1.soundInstancePlayerHit[index2].Play();
          break;
        case 2:
          if (style != 9 && style != 10)
            Game1.soundInstanceItem[style].Stop();
          Game1.soundInstanceItem[style] = Game1.soundItem[style].CreateInstance();
          Game1.soundInstanceItem[style].Volume = num5;
          Game1.soundInstanceItem[style].Pan = num2;
          Game1.soundInstanceItem[style].Play();
          break;
        case 3:
          Game1.soundInstanceNPCHit[style].Stop();
          Game1.soundInstanceNPCHit[style] = Game1.soundNPCHit[style].CreateInstance();
          Game1.soundInstanceNPCHit[style].Volume = num5;
          Game1.soundInstanceNPCHit[style].Pan = num2;
          Game1.soundInstanceNPCHit[style].Play();
          break;
        case 4:
          Game1.soundInstanceNPCKilled[style] = Game1.soundNPCKilled[style].CreateInstance();
          Game1.soundInstanceNPCKilled[style].Volume = num5;
          Game1.soundInstanceNPCKilled[style].Pan = num2;
          Game1.soundInstanceNPCKilled[style].Play();
          break;
        case 5:
          Game1.soundInstancePlayerKilled.Stop();
          Game1.soundInstancePlayerKilled = Game1.soundPlayerKilled.CreateInstance();
          Game1.soundInstancePlayerKilled.Volume = num5;
          Game1.soundInstancePlayerKilled.Pan = num2;
          Game1.soundInstancePlayerKilled.Play();
          break;
        case 6:
          Game1.soundInstanceGrass.Stop();
          Game1.soundInstanceGrass = Game1.soundGrass.CreateInstance();
          Game1.soundInstanceGrass.Volume = num5;
          Game1.soundInstanceGrass.Pan = num2;
          Game1.soundInstanceGrass.Play();
          break;
        case 7:
          Game1.soundInstanceGrab.Stop();
          Game1.soundInstanceGrab = Game1.soundGrab.CreateInstance();
          Game1.soundInstanceGrab.Volume = num5;
          Game1.soundInstanceGrab.Pan = num2;
          Game1.soundInstanceGrab.Play();
          break;
        case 8:
          Game1.soundInstanceDoorOpen.Stop();
          Game1.soundInstanceDoorOpen = Game1.soundDoorOpen.CreateInstance();
          Game1.soundInstanceDoorOpen.Volume = num5;
          Game1.soundInstanceDoorOpen.Pan = num2;
          Game1.soundInstanceDoorOpen.Play();
          break;
        case 9:
          Game1.soundInstanceDoorClosed.Stop();
          Game1.soundInstanceDoorClosed = Game1.soundDoorClosed.CreateInstance();
          Game1.soundInstanceDoorClosed.Volume = num5;
          Game1.soundInstanceDoorClosed.Pan = num2;
          Game1.soundInstanceDoorClosed.Play();
          break;
        case 10:
          Game1.soundInstanceMenuOpen.Stop();
          Game1.soundInstanceMenuOpen = Game1.soundMenuOpen.CreateInstance();
          Game1.soundInstanceMenuOpen.Volume = num5;
          Game1.soundInstanceMenuOpen.Pan = num2;
          Game1.soundInstanceMenuOpen.Play();
          break;
        case 11:
          Game1.soundInstanceMenuClose.Stop();
          Game1.soundInstanceMenuClose = Game1.soundMenuClose.CreateInstance();
          Game1.soundInstanceMenuClose.Volume = num5;
          Game1.soundInstanceMenuClose.Pan = num2;
          Game1.soundInstanceMenuClose.Play();
          break;
        case 12:
          Game1.soundInstanceMenuTick.Stop();
          Game1.soundInstanceMenuTick = Game1.soundMenuTick.CreateInstance();
          Game1.soundInstanceMenuTick.Volume = num5;
          Game1.soundInstanceMenuTick.Pan = num2;
          Game1.soundInstanceMenuTick.Play();
          break;
        case 13:
          Game1.soundInstanceShatter.Stop();
          Game1.soundInstanceShatter = Game1.soundShatter.CreateInstance();
          Game1.soundInstanceShatter.Volume = num5;
          Game1.soundInstanceShatter.Pan = num2;
          Game1.soundInstanceShatter.Play();
          break;
        case 14:
          int index3 = Game1.rand.Next(3);
          Game1.soundInstanceZombie[index3] = Game1.soundZombie[index3].CreateInstance();
          Game1.soundInstanceZombie[index3].Volume = num5 * 0.4f;
          Game1.soundInstanceZombie[index3].Pan = num2;
          Game1.soundInstanceZombie[index3].Play();
          break;
        case 15:
          Game1.soundInstanceRoar[style] = Game1.soundRoar[style].CreateInstance();
          Game1.soundInstanceRoar[style].Volume = num5;
          Game1.soundInstanceRoar[style].Pan = num2;
          Game1.soundInstanceRoar[style].Play();
          break;
        case 16:
          Game1.soundInstanceDoubleJump.Stop();
          Game1.soundInstanceDoubleJump = Game1.soundDoubleJump.CreateInstance();
          Game1.soundInstanceDoubleJump.Volume = num5;
          Game1.soundInstanceDoubleJump.Pan = num2;
          Game1.soundInstanceDoubleJump.Play();
          break;
        case 17:
          Game1.soundInstanceRun.Stop();
          Game1.soundInstanceRun = Game1.soundRun.CreateInstance();
          Game1.soundInstanceRun.Volume = num5;
          Game1.soundInstanceRun.Pan = num2;
          Game1.soundInstanceRun.Play();
          break;
        case 18:
          Game1.soundInstanceCoins = Game1.soundCoins.CreateInstance();
          Game1.soundInstanceCoins.Volume = num5;
          Game1.soundInstanceCoins.Pan = num2;
          Game1.soundInstanceCoins.Play();
          break;
        case 19:
          Game1.soundInstanceSplash[style] = Game1.soundSplash[style].CreateInstance();
          Game1.soundInstanceSplash[style].Volume = num5;
          Game1.soundInstanceSplash[style].Pan = num2;
          Game1.soundInstanceSplash[style].Play();
          break;
      }
    }

    private static void UpdateDebug()
    {
      if (Game1.netMode == 2)
        return;

      // Panning control via WASD keys
      if (Game1.keyState.IsKeyDown(/*Keys.Up*/Keys.W))
        Game1.screenPosition.Y -= 8f;//32f;

      if (Game1.keyState.IsKeyDown(/*Keys.Left*/Keys.A))
        Game1.screenPosition.X -= 8f;//32f;

      if (Game1.keyState.IsKeyDown(/*Keys.Down*/Keys.S))
        Game1.screenPosition.Y += 8f;//32f;

      if (Game1.keyState.IsKeyDown(/*Keys.Right*/Keys.D))
        Game1.screenPosition.X += 8f;//32f;

      //RnD
      if (Game1.mouseState.Count == 0)
        return;

      int i = (int) (((double) Game1.mouseState[0].Position.X + (double) Game1.screenPosition.X) / 16.0);
      int j = (int) (((double) Game1.mouseState[0].Position.Y + (double) Game1.screenPosition.Y) / 16.0)-1;
     
      if 
       (Game1.mouseState[0].Position.X >= Game1.screenWidth 
       || Game1.mouseState[0].Position.Y >= Game1.screenHeight 
       || i < 0 || j < 0 
       || i >= Game1.maxTilesX || j >= Game1.maxTilesY
      )
        return;

      // RnD: Light ?
      Lighting.addLight(i, j, 1f);
      
      //if (Game1.mouseState.RightButton != ButtonState.Pressed 
      //|| Game1.mouseState.LeftButton != ButtonState.Pressed)
      //{}

      if (Game1.mouseState.Count == 2//Game1.mouseState.RightButton == ButtonState.Pressed
      )
      {
        int player = Game1.myPlayer;
        int number = Game1.rand.Next(8);
        if (Game1.player[number].active)
        {
          Game1.player[number].position.X = (float) (i * 16);
          Game1.player[number].position.Y = (float) (j * 16);
          Game1.player[number].fallStart = (int) ((double) Game1.player[number].position.Y / 16.0);

          NetMessage.SendData(13, number: number);
        }

        for (int index = -1; index < 2; ++index)
        {
          int num = -1;
          while (num < 2)
            ++num;
        }
      }
      else if (Game1.mouseState.Count == 3//Game1.mouseState.LeftButton == ButtonState.Pressed
      )
      {
        WorldGen.KillTile(i, j);
        for (int index1 = -5; index1 <= 5; ++index1)
        {
          for (int index2 = 5; index2 >= -5; --index2)
          {
            if (Game1.netMode != 1)
              Liquid.AddWater(i + index1, j + index2);
          }
        }
      }
      

    }//UpdateDebug

  }//Game1 class end

}
