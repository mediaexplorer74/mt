// Decompiled with JetBrains decompiler
// Type: GameManager.Chest
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

namespace GameManager
{
  public class Chest
  {
    public static int maxItems = 20;
    public Item[] item = new Item[Chest.maxItems];
    public int x;
    public int y;

    public static int UsingChest(int i)
    {
      if (Game1.chest[i] != null)
      {
        for (int index = 0; index < 8; ++index)
        {
          if (Game1.player[index].chest == i)
            return index;
        }
      }
      return -1;
    }

    public static int FindChest(int X, int Y)
    {
      for (int chest = 0; chest < 1000; ++chest)
      {
        if (Game1.chest[chest] != null && Game1.chest[chest].x == X && Game1.chest[chest].y == Y)
          return chest;
      }
      return -1;
    }

    public static int CreateChest(int X, int Y)
    {
      for (int index = 0; index < 1000; ++index)
      {
        if (Game1.chest[index] != null && Game1.chest[index].x == X && Game1.chest[index].y == Y)
          return -1;
      }
      for (int chest = 0; chest < 1000; ++chest)
      {
        if (Game1.chest[chest] == null)
        {
          Game1.chest[chest] = new Chest();
          Game1.chest[chest].x = X;
          Game1.chest[chest].y = Y;
          for (int index = 0; index < Chest.maxItems; ++index)
            Game1.chest[chest].item[index] = new Item();
          return chest;
        }
      }
      return -1;
    }

    public static bool DestroyChest(int X, int Y)
    {
      for (int index1 = 0; index1 < 1000; ++index1)
      {
        if (Game1.chest[index1] != null && Game1.chest[index1].x == X && Game1.chest[index1].y == Y)
        {
          for (int index2 = 0; index2 < Chest.maxItems; ++index2)
          {
            if (Game1.chest[index1].item[index2].type > 0 && Game1.chest[index1].item[index2].stack > 0)
              return false;
          }
          Game1.chest[index1] = (Chest) null;
          return true;
        }
      }
      return true;
    }

    public void SetupShop(int type)
    {
      for (int index = 0; index < Chest.maxItems; ++index)
        this.item[index] = new Item();
      int num;
      switch (type)
      {
        case 1:
          int index1 = 0;
          this.item[index1].SetDefaults("Mining Helmet");
          int index2 = index1 + 1;
          this.item[index2].SetDefaults("Piggy Bank");
          int index3 = index2 + 1;
          this.item[index3].SetDefaults("Iron Anvil");
          int index4 = index3 + 1;
          this.item[index4].SetDefaults("Copper Pickaxe");
          int index5 = index4 + 1;
          this.item[index5].SetDefaults("Copper Axe");
          int index6 = index5 + 1;
          this.item[index6].SetDefaults("Torch");
          int index7 = index6 + 1;
          this.item[index7].SetDefaults("Lesser Healing Potion");
          int index8 = index7 + 1;
          this.item[index8].SetDefaults("Wooden Arrow");
          int index9 = index8 + 1;
          this.item[index9].SetDefaults("Shuriken");
          num = index9 + 1;
          break;
        case 2:
          int index10 = 0;
          this.item[index10].SetDefaults("Musket Ball");
          int index11 = index10 + 1;
          this.item[index11].SetDefaults("Flintlock Pistol");
          int index12 = index11 + 1;
          this.item[index12].SetDefaults("Minishark");
          num = index12 + 1;
          break;
        case 3:
          int index13 = 0;
          this.item[index13].SetDefaults("Purification Powder");
          int index14 = index13 + 1;
          this.item[index14].SetDefaults("Acorn");
          int index15 = index14 + 1;
          this.item[index15].SetDefaults("Grass Seeds");
          int index16 = index15 + 1;
          this.item[index16].SetDefaults("Sunflower");
          int index17 = index16 + 1;
          this.item[index17].SetDefaults(114);
          num = index17 + 1;
          break;
        case 4:
          int index18 = 0;
          this.item[index18].SetDefaults("Grenade");
          int index19 = index18 + 1;
          this.item[index19].SetDefaults("Bomb");
          int index20 = index19 + 1;
          this.item[index20].SetDefaults("Dynamite");
          num = index20 + 1;
          break;
      }
    }
  }
}
