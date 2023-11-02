// Decompiled with JetBrains decompiler
// Type: GameManager.Tile
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

namespace GameManager
{
  public class Tile
  {
    public bool active = false;
    public bool lighted = false;
    public byte type;
    public byte wall;
    public byte wallFrameX;
    public byte wallFrameY;
    public byte wallFrameNumber;
    public byte liquid;
    public bool checkingLiquid = false;
    public bool skipLiquid = false;
    public bool lava = false;
    public byte frameNumber;
    public short frameX;
    public short frameY;
  }
}
