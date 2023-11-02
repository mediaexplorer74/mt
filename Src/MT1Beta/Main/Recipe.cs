// Decompiled with JetBrains decompiler
// Type: GameManager.Recipe
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

namespace GameManager
{
  public class Recipe
  {
    public static int maxRequirements = 10;
    public static int maxRecipes = 200;
    public static int numRecipes = 0;
    private static Recipe newRecipe = new Recipe();
    public Item createItem = new Item();
    public Item[] requiredItem = new Item[Recipe.maxRequirements];
    public int[] requiredTile = new int[Recipe.maxRequirements];

    public Recipe()
    {
      for (int index = 0; index < Recipe.maxRequirements; ++index)
      {
        this.requiredItem[index] = new Item();
        this.requiredTile[index] = -1;
      }
    }

    public void Create()
    {
      for (int index1 = 0; index1 < Recipe.maxRequirements && this.requiredItem[index1].type != 0; ++index1)
      {
        int num = this.requiredItem[index1].stack;
        for (int index2 = 0; index2 < 44; ++index2)
        {
          if (Game1.player[Game1.myPlayer].inventory[index2].IsTheSameAs(this.requiredItem[index1]))
          {
            if (Game1.player[Game1.myPlayer].inventory[index2].stack > num)
            {
              Game1.player[Game1.myPlayer].inventory[index2].stack -= num;
              num = 0;
            }
            else
            {
              num -= Game1.player[Game1.myPlayer].inventory[index2].stack;
              Game1.player[Game1.myPlayer].inventory[index2] = new Item();
            }
          }
          if (num <= 0)
            break;
        }
      }
      Recipe.FindRecipes();
    }

    public static void FindRecipes()
    {
      int num1 = Game1.availableRecipe[Game1.focusRecipe];
      float num2 = Game1.availableRecipeY[Game1.focusRecipe];
      for (int index = 0; index < Recipe.maxRecipes; ++index)
        Game1.availableRecipe[index] = 0;
      Game1.numAvailableRecipes = 0;
      for (int index1 = 0; index1 < Recipe.maxRecipes && Game1.recipe[index1].createItem.type != 0; ++index1)
      {
        bool flag1 = true;
        for (int index2 = 0; index2 < Recipe.maxRequirements && Game1.recipe[index1].requiredItem[index2].type != 0; ++index2)
        {
          int stack = Game1.recipe[index1].requiredItem[index2].stack;
          for (int index3 = 0; index3 < 44; ++index3)
          {
            if (Game1.player[Game1.myPlayer].inventory[index3].IsTheSameAs(Game1.recipe[index1].requiredItem[index2]))
              stack -= Game1.player[Game1.myPlayer].inventory[index3].stack;
            if (stack <= 0)
              break;
          }
          if (stack > 0)
          {
            flag1 = false;
            break;
          }
        }
        if (flag1)
        {
          bool flag2 = true;
          for (int index4 = 0; index4 < Recipe.maxRequirements && Game1.recipe[index1].requiredTile[index4] != -1; ++index4)
          {
            if (!Game1.player[Game1.myPlayer].adjTile[Game1.recipe[index1].requiredTile[index4]])
            {
              flag2 = false;
              break;
            }
          }
          if (flag2)
          {
            Game1.availableRecipe[Game1.numAvailableRecipes] = index1;
            ++Game1.numAvailableRecipes;
          }
        }
      }
      for (int index = 0; index < Game1.numAvailableRecipes; ++index)
      {
        if (num1 == Game1.availableRecipe[index])
        {
          Game1.focusRecipe = index;
          break;
        }
      }
      if (Game1.focusRecipe >= Game1.numAvailableRecipes)
        Game1.focusRecipe = Game1.numAvailableRecipes - 1;
      if (Game1.focusRecipe < 0)
        Game1.focusRecipe = 0;
      float num3 = Game1.availableRecipeY[Game1.focusRecipe] - num2;
      for (int index = 0; index < Recipe.maxRecipes; ++index)
        Game1.availableRecipeY[index] -= num3;
    }

    public static void SetupRecipes()
    {
      Recipe.newRecipe.createItem.SetDefaults(28);
      Recipe.newRecipe.createItem.stack = 2;
      Recipe.newRecipe.requiredItem[0].SetDefaults(5);
      Recipe.newRecipe.requiredItem[1].SetDefaults(23);
      Recipe.newRecipe.requiredItem[1].stack = 2;
      Recipe.newRecipe.requiredItem[2].SetDefaults(31);
      Recipe.newRecipe.requiredTile[0] = 13;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Healing Potion");
      Recipe.newRecipe.createItem.stack = 2;
      Recipe.newRecipe.requiredItem[0].SetDefaults(28);
      Recipe.newRecipe.requiredItem[1].SetDefaults(183);
      Recipe.newRecipe.requiredTile[0] = 13;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(110);
      Recipe.newRecipe.requiredItem[0].SetDefaults(75);
      Recipe.newRecipe.requiredItem[1].SetDefaults(23);
      Recipe.newRecipe.requiredItem[1].stack = 2;
      Recipe.newRecipe.requiredItem[2].SetDefaults(31);
      Recipe.newRecipe.requiredTile[0] = 13;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Mana Potion");
      Recipe.newRecipe.requiredItem[0].SetDefaults(110);
      Recipe.newRecipe.requiredItem[1].SetDefaults(183);
      Recipe.newRecipe.requiredTile[0] = 13;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Bottle");
      Recipe.newRecipe.createItem.stack = 3;
      Recipe.newRecipe.requiredItem[0].SetDefaults("Glass");
      Recipe.newRecipe.requiredTile[0] = 17;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(67);
      Recipe.newRecipe.createItem.stack = 5;
      Recipe.newRecipe.requiredItem[0].SetDefaults(60);
      Recipe.newRecipe.requiredTile[0] = 13;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(8);
      Recipe.newRecipe.createItem.stack = 3;
      Recipe.newRecipe.requiredItem[0].SetDefaults(23);
      Recipe.newRecipe.requiredItem[0].stack = 1;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Glass");
      Recipe.newRecipe.createItem.stack = 1;
      Recipe.newRecipe.requiredItem[0].SetDefaults(169);
      Recipe.newRecipe.requiredItem[0].stack = 2;
      Recipe.newRecipe.requiredTile[0] = 17;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Gray Brick");
      Recipe.newRecipe.requiredItem[0].SetDefaults(3);
      Recipe.newRecipe.requiredItem[0].stack = 2;
      Recipe.newRecipe.requiredTile[0] = 17;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Gray Brick Wall");
      Recipe.newRecipe.createItem.stack = 4;
      Recipe.newRecipe.requiredItem[0].SetDefaults("Gray Brick");
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Red Brick");
      Recipe.newRecipe.requiredItem[0].SetDefaults(133);
      Recipe.newRecipe.requiredItem[0].stack = 2;
      Recipe.newRecipe.requiredTile[0] = 17;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Red Brick Wall");
      Recipe.newRecipe.createItem.stack = 4;
      Recipe.newRecipe.requiredItem[0].SetDefaults("Red Brick");
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Copper Brick");
      Recipe.newRecipe.requiredItem[0].SetDefaults(3);
      Recipe.newRecipe.requiredItem[0].stack = 1;
      Recipe.newRecipe.requiredItem[1].SetDefaults("Copper Ore");
      Recipe.newRecipe.requiredItem[1].stack = 1;
      Recipe.newRecipe.requiredTile[0] = 17;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Copper Brick Wall");
      Recipe.newRecipe.createItem.stack = 4;
      Recipe.newRecipe.requiredItem[0].SetDefaults("Copper Brick");
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Silver Brick Wall");
      Recipe.newRecipe.createItem.stack = 4;
      Recipe.newRecipe.requiredItem[0].SetDefaults("Silver Brick");
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Silver Brick");
      Recipe.newRecipe.requiredItem[0].SetDefaults(3);
      Recipe.newRecipe.requiredItem[0].stack = 1;
      Recipe.newRecipe.requiredItem[1].SetDefaults("Silver Ore");
      Recipe.newRecipe.requiredItem[1].stack = 1;
      Recipe.newRecipe.requiredTile[0] = 17;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Gold Brick Wall");
      Recipe.newRecipe.createItem.stack = 4;
      Recipe.newRecipe.requiredItem[0].SetDefaults("Gold Brick");
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Gold Brick");
      Recipe.newRecipe.requiredItem[0].SetDefaults(3);
      Recipe.newRecipe.requiredItem[0].stack = 1;
      Recipe.newRecipe.requiredItem[1].SetDefaults("Gold Ore");
      Recipe.newRecipe.requiredItem[1].stack = 1;
      Recipe.newRecipe.requiredTile[0] = 17;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(192);
      Recipe.newRecipe.requiredItem[0].SetDefaults(173);
      Recipe.newRecipe.requiredItem[0].stack = 2;
      Recipe.newRecipe.requiredTile[0] = 17;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(193);
      Recipe.newRecipe.requiredItem[0].SetDefaults(173);
      Recipe.newRecipe.requiredItem[0].stack = 20;
      Recipe.newRecipe.requiredTile[0] = 17;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(30);
      Recipe.newRecipe.createItem.stack = 4;
      Recipe.newRecipe.requiredItem[0].SetDefaults(2);
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(26);
      Recipe.newRecipe.createItem.stack = 4;
      Recipe.newRecipe.requiredItem[0].SetDefaults(3);
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(93);
      Recipe.newRecipe.createItem.stack = 4;
      Recipe.newRecipe.requiredItem[0].SetDefaults(9);
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(94);
      Recipe.newRecipe.requiredItem[0].SetDefaults(9);
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(25);
      Recipe.newRecipe.requiredItem[0].SetDefaults(9);
      Recipe.newRecipe.requiredItem[0].stack = 5;
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(34);
      Recipe.newRecipe.requiredItem[0].SetDefaults(9);
      Recipe.newRecipe.requiredItem[0].stack = 3;
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Sign");
      Recipe.newRecipe.requiredItem[0].SetDefaults(9);
      Recipe.newRecipe.requiredItem[0].stack = 5;
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(48);
      Recipe.newRecipe.requiredItem[0].SetDefaults(9);
      Recipe.newRecipe.requiredItem[0].stack = 7;
      Recipe.newRecipe.requiredItem[1].SetDefaults(22);
      Recipe.newRecipe.requiredItem[1].stack = 3;
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(32);
      Recipe.newRecipe.requiredItem[0].SetDefaults(9);
      Recipe.newRecipe.requiredItem[0].stack = 8;
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(36);
      Recipe.newRecipe.requiredItem[0].SetDefaults(9);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(24);
      Recipe.newRecipe.requiredItem[0].SetDefaults(9);
      Recipe.newRecipe.requiredItem[0].stack = 7;
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(39);
      Recipe.newRecipe.requiredItem[0].SetDefaults(9);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Flaming Arrow");
      Recipe.newRecipe.createItem.stack = 5;
      Recipe.newRecipe.requiredItem[0].SetDefaults(40);
      Recipe.newRecipe.requiredItem[0].stack = 5;
      Recipe.newRecipe.requiredItem[1].SetDefaults(8);
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(33);
      Recipe.newRecipe.requiredItem[0].SetDefaults(3);
      Recipe.newRecipe.requiredItem[0].stack = 20;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 4;
      Recipe.newRecipe.requiredItem[2].SetDefaults(8);
      Recipe.newRecipe.requiredItem[2].stack = 3;
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(20);
      Recipe.newRecipe.requiredItem[0].SetDefaults(12);
      Recipe.newRecipe.requiredItem[0].stack = 3;
      Recipe.newRecipe.requiredTile[0] = 17;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Copper Pickaxe");
      Recipe.newRecipe.requiredItem[0].SetDefaults(20);
      Recipe.newRecipe.requiredItem[0].stack = 12;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 4;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Copper Axe");
      Recipe.newRecipe.requiredItem[0].SetDefaults(20);
      Recipe.newRecipe.requiredItem[0].stack = 9;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 3;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Copper Hammer");
      Recipe.newRecipe.requiredItem[0].SetDefaults(20);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 3;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Copper Broadsword");
      Recipe.newRecipe.requiredItem[0].SetDefaults(20);
      Recipe.newRecipe.requiredItem[0].stack = 8;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Copper Shortsword");
      Recipe.newRecipe.requiredItem[0].SetDefaults(20);
      Recipe.newRecipe.requiredItem[0].stack = 7;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Copper Bow");
      Recipe.newRecipe.requiredItem[0].SetDefaults(20);
      Recipe.newRecipe.requiredItem[0].stack = 7;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Copper Greaves");
      Recipe.newRecipe.requiredItem[0].SetDefaults(20);
      Recipe.newRecipe.requiredItem[0].stack = 15;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Copper Chainmail");
      Recipe.newRecipe.requiredItem[0].SetDefaults(20);
      Recipe.newRecipe.requiredItem[0].stack = 25;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Copper Helmet");
      Recipe.newRecipe.requiredItem[0].SetDefaults(20);
      Recipe.newRecipe.requiredItem[0].stack = 30;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Copper Watch");
      Recipe.newRecipe.requiredItem[0].SetDefaults(20);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.newRecipe.requiredItem[1].SetDefaults(85);
      Recipe.newRecipe.requiredTile[0] = 14;
      Recipe.newRecipe.requiredTile[1] = 15;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Copper Chandelier");
      Recipe.newRecipe.requiredItem[0].SetDefaults(20);
      Recipe.newRecipe.requiredItem[0].stack = 4;
      Recipe.newRecipe.requiredItem[1].SetDefaults(8);
      Recipe.newRecipe.requiredItem[1].stack = 4;
      Recipe.newRecipe.requiredItem[2].SetDefaults(85);
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(22);
      Recipe.newRecipe.requiredItem[0].SetDefaults(11);
      Recipe.newRecipe.requiredItem[0].stack = 3;
      Recipe.newRecipe.requiredTile[0] = 17;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(35);
      Recipe.newRecipe.requiredItem[0].SetDefaults(22);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(1);
      Recipe.newRecipe.requiredItem[0].SetDefaults(22);
      Recipe.newRecipe.requiredItem[0].stack = 12;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 3;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(10);
      Recipe.newRecipe.requiredItem[0].SetDefaults(22);
      Recipe.newRecipe.requiredItem[0].stack = 9;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 3;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(7);
      Recipe.newRecipe.requiredItem[0].SetDefaults(22);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 3;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(4);
      Recipe.newRecipe.requiredItem[0].SetDefaults(22);
      Recipe.newRecipe.requiredItem[0].stack = 8;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(6);
      Recipe.newRecipe.requiredItem[0].SetDefaults(22);
      Recipe.newRecipe.requiredItem[0].stack = 7;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Iron Bow");
      Recipe.newRecipe.requiredItem[0].SetDefaults(22);
      Recipe.newRecipe.requiredItem[0].stack = 7;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Iron Greaves");
      Recipe.newRecipe.requiredItem[0].SetDefaults(22);
      Recipe.newRecipe.requiredItem[0].stack = 20;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Iron Chainmail");
      Recipe.newRecipe.requiredItem[0].SetDefaults(22);
      Recipe.newRecipe.requiredItem[0].stack = 30;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Iron Helmet");
      Recipe.newRecipe.requiredItem[0].SetDefaults(22);
      Recipe.newRecipe.requiredItem[0].stack = 35;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Iron Chain");
      Recipe.newRecipe.requiredItem[0].SetDefaults(22);
      Recipe.newRecipe.requiredItem[0].stack = 3;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(21);
      Recipe.newRecipe.requiredItem[0].SetDefaults(14);
      Recipe.newRecipe.requiredItem[0].stack = 3;
      Recipe.newRecipe.requiredTile[0] = 17;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Silver Pickaxe");
      Recipe.newRecipe.requiredItem[0].SetDefaults(21);
      Recipe.newRecipe.requiredItem[0].stack = 12;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 4;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Silver Axe");
      Recipe.newRecipe.requiredItem[0].SetDefaults(21);
      Recipe.newRecipe.requiredItem[0].stack = 9;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 3;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Silver Hammer");
      Recipe.newRecipe.requiredItem[0].SetDefaults(21);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 3;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Silver Broadsword");
      Recipe.newRecipe.requiredItem[0].SetDefaults(21);
      Recipe.newRecipe.requiredItem[0].stack = 8;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Silver Greaves");
      Recipe.newRecipe.requiredItem[0].SetDefaults(21);
      Recipe.newRecipe.requiredItem[0].stack = 20;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Silver Bow");
      Recipe.newRecipe.requiredItem[0].SetDefaults(21);
      Recipe.newRecipe.requiredItem[0].stack = 7;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Silver Chainmail");
      Recipe.newRecipe.requiredItem[0].SetDefaults(21);
      Recipe.newRecipe.requiredItem[0].stack = 30;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Silver Helmet");
      Recipe.newRecipe.requiredItem[0].SetDefaults(21);
      Recipe.newRecipe.requiredItem[0].stack = 35;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Silver Watch");
      Recipe.newRecipe.requiredItem[0].SetDefaults(21);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.newRecipe.requiredItem[1].SetDefaults(85);
      Recipe.newRecipe.requiredTile[0] = 14;
      Recipe.newRecipe.requiredTile[1] = 15;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Silver Chandelier");
      Recipe.newRecipe.requiredItem[0].SetDefaults(21);
      Recipe.newRecipe.requiredItem[0].stack = 4;
      Recipe.newRecipe.requiredItem[1].SetDefaults(8);
      Recipe.newRecipe.requiredItem[1].stack = 4;
      Recipe.newRecipe.requiredItem[2].SetDefaults(85);
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(19);
      Recipe.newRecipe.requiredItem[0].SetDefaults(13);
      Recipe.newRecipe.requiredItem[0].stack = 4;
      Recipe.newRecipe.requiredTile[0] = 17;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Gold Pickaxe");
      Recipe.newRecipe.requiredItem[0].SetDefaults(19);
      Recipe.newRecipe.requiredItem[0].stack = 12;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 4;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Gold Axe");
      Recipe.newRecipe.requiredItem[0].SetDefaults(19);
      Recipe.newRecipe.requiredItem[0].stack = 9;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 3;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Gold Hammer");
      Recipe.newRecipe.requiredItem[0].SetDefaults(19);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 3;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Gold Broadsword");
      Recipe.newRecipe.requiredItem[0].SetDefaults(19);
      Recipe.newRecipe.requiredItem[0].stack = 8;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Gold Shortsword");
      Recipe.newRecipe.requiredItem[0].SetDefaults(19);
      Recipe.newRecipe.requiredItem[0].stack = 7;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Gold Bow");
      Recipe.newRecipe.requiredItem[0].SetDefaults(19);
      Recipe.newRecipe.requiredItem[0].stack = 7;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Gold Greaves");
      Recipe.newRecipe.requiredItem[0].SetDefaults(19);
      Recipe.newRecipe.requiredItem[0].stack = 25;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Gold Chainmail");
      Recipe.newRecipe.requiredItem[0].SetDefaults(19);
      Recipe.newRecipe.requiredItem[0].stack = 35;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Gold Helmet");
      Recipe.newRecipe.requiredItem[0].SetDefaults(19);
      Recipe.newRecipe.requiredItem[0].stack = 40;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Gold Watch");
      Recipe.newRecipe.requiredItem[0].SetDefaults(19);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.newRecipe.requiredItem[1].SetDefaults(85);
      Recipe.newRecipe.requiredTile[0] = 14;
      Recipe.newRecipe.requiredTile[1] = 15;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Gold Chandelier");
      Recipe.newRecipe.requiredItem[0].SetDefaults(19);
      Recipe.newRecipe.requiredItem[0].stack = 4;
      Recipe.newRecipe.requiredItem[1].SetDefaults(8);
      Recipe.newRecipe.requiredItem[1].stack = 4;
      Recipe.newRecipe.requiredItem[2].SetDefaults(85);
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Candle");
      Recipe.newRecipe.requiredItem[0].SetDefaults(19);
      Recipe.newRecipe.requiredItem[1].SetDefaults(8);
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(57);
      Recipe.newRecipe.requiredItem[0].SetDefaults(56);
      Recipe.newRecipe.requiredItem[0].stack = 4;
      Recipe.newRecipe.requiredTile[0] = 17;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(44);
      Recipe.newRecipe.requiredItem[0].SetDefaults(57);
      Recipe.newRecipe.requiredItem[0].stack = 8;
      Recipe.newRecipe.requiredTile[0] = 26;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(45);
      Recipe.newRecipe.requiredItem[0].SetDefaults(57);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.newRecipe.requiredTile[0] = 26;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(46);
      Recipe.newRecipe.requiredItem[0].SetDefaults(57);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.newRecipe.requiredTile[0] = 26;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(47);
      Recipe.newRecipe.createItem.stack = 20;
      Recipe.newRecipe.requiredItem[0].SetDefaults(57);
      Recipe.newRecipe.requiredItem[0].stack = 1;
      Recipe.newRecipe.requiredTile[0] = 26;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Shadow Greaves");
      Recipe.newRecipe.requiredItem[0].SetDefaults(57);
      Recipe.newRecipe.requiredItem[0].stack = 25;
      Recipe.newRecipe.requiredItem[1].SetDefaults(86);
      Recipe.newRecipe.requiredItem[1].stack = 25;
      Recipe.newRecipe.requiredTile[0] = 26;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Shadow Scalemail");
      Recipe.newRecipe.requiredItem[0].SetDefaults(57);
      Recipe.newRecipe.requiredItem[0].stack = 35;
      Recipe.newRecipe.requiredItem[1].SetDefaults(86);
      Recipe.newRecipe.requiredItem[1].stack = 35;
      Recipe.newRecipe.requiredTile[0] = 26;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Shadow Helmet");
      Recipe.newRecipe.requiredItem[0].SetDefaults(57);
      Recipe.newRecipe.requiredItem[0].stack = 40;
      Recipe.newRecipe.requiredItem[1].SetDefaults(86);
      Recipe.newRecipe.requiredItem[1].stack = 40;
      Recipe.newRecipe.requiredTile[0] = 26;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Nightmare Pickaxe");
      Recipe.newRecipe.requiredItem[0].SetDefaults(57);
      Recipe.newRecipe.requiredItem[0].stack = 12;
      Recipe.newRecipe.requiredItem[1].SetDefaults(86);
      Recipe.newRecipe.requiredItem[1].stack = 6;
      Recipe.newRecipe.requiredTile[0] = 26;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("The Breaker");
      Recipe.newRecipe.requiredItem[0].SetDefaults(57);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.newRecipe.requiredItem[1].SetDefaults(86);
      Recipe.newRecipe.requiredItem[1].stack = 5;
      Recipe.newRecipe.requiredTile[0] = 26;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Grappling Hook");
      Recipe.newRecipe.requiredItem[0].SetDefaults(85);
      Recipe.newRecipe.requiredItem[0].stack = 3;
      Recipe.newRecipe.requiredItem[1].SetDefaults(118);
      Recipe.newRecipe.requiredItem[1].stack = 1;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(117);
      Recipe.newRecipe.requiredItem[0].SetDefaults(116);
      Recipe.newRecipe.requiredItem[0].stack = 6;
      Recipe.newRecipe.requiredTile[0] = 17;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(119);
      Recipe.newRecipe.requiredItem[0].SetDefaults(117);
      Recipe.newRecipe.requiredItem[0].stack = 15;
      Recipe.newRecipe.requiredItem[1].SetDefaults(55);
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(120);
      Recipe.newRecipe.requiredItem[0].SetDefaults(117);
      Recipe.newRecipe.requiredItem[0].stack = 25;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(121);
      Recipe.newRecipe.requiredItem[0].SetDefaults(117);
      Recipe.newRecipe.requiredItem[0].stack = 35;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(122);
      Recipe.newRecipe.requiredItem[0].SetDefaults(117);
      Recipe.newRecipe.requiredItem[0].stack = 35;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults((int) sbyte.MaxValue);
      Recipe.newRecipe.requiredItem[0].SetDefaults(95);
      Recipe.newRecipe.requiredItem[1].SetDefaults(117);
      Recipe.newRecipe.requiredItem[1].stack = 30;
      Recipe.newRecipe.requiredItem[2].SetDefaults(75);
      Recipe.newRecipe.requiredItem[2].stack = 10;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Meteor Leggings");
      Recipe.newRecipe.requiredItem[0].SetDefaults(117);
      Recipe.newRecipe.requiredItem[0].stack = 40;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Meteor Suit");
      Recipe.newRecipe.requiredItem[0].SetDefaults(117);
      Recipe.newRecipe.requiredItem[0].stack = 45;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Meteor Helmet");
      Recipe.newRecipe.requiredItem[0].SetDefaults(117);
      Recipe.newRecipe.requiredItem[0].stack = 50;
      Recipe.newRecipe.requiredTile[0] = 16;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(151);
      Recipe.newRecipe.requiredItem[0].SetDefaults(154);
      Recipe.newRecipe.requiredItem[0].stack = 40;
      Recipe.newRecipe.requiredItem[1].SetDefaults(150);
      Recipe.newRecipe.requiredItem[1].stack = 100;
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(152);
      Recipe.newRecipe.requiredItem[0].SetDefaults(154);
      Recipe.newRecipe.requiredItem[0].stack = 35;
      Recipe.newRecipe.requiredItem[1].SetDefaults(150);
      Recipe.newRecipe.requiredItem[1].stack = 100;
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(153);
      Recipe.newRecipe.requiredItem[0].SetDefaults(154);
      Recipe.newRecipe.requiredItem[0].stack = 30;
      Recipe.newRecipe.requiredItem[1].SetDefaults(150);
      Recipe.newRecipe.requiredItem[1].stack = 100;
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Depth Meter");
      Recipe.newRecipe.requiredItem[0].SetDefaults(20);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.newRecipe.requiredItem[1].SetDefaults(21);
      Recipe.newRecipe.requiredItem[1].stack = 8;
      Recipe.newRecipe.requiredItem[2].SetDefaults(19);
      Recipe.newRecipe.requiredItem[2].stack = 6;
      Recipe.newRecipe.requiredTile[0] = 14;
      Recipe.newRecipe.requiredTile[1] = 15;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Goggles");
      Recipe.newRecipe.requiredItem[0].SetDefaults(38);
      Recipe.newRecipe.requiredItem[0].stack = 2;
      Recipe.newRecipe.requiredItem[1].SetDefaults(22);
      Recipe.newRecipe.requiredItem[1].stack = 5;
      Recipe.newRecipe.requiredTile[0] = 18;
      Recipe.newRecipe.requiredTile[1] = 15;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Vilethorn");
      Recipe.newRecipe.requiredItem[0].SetDefaults(69);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.newRecipe.requiredItem[1].SetDefaults(67);
      Recipe.newRecipe.requiredItem[1].stack = 50;
      Recipe.newRecipe.requiredTile[0] = 26;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Mana Crystal");
      Recipe.newRecipe.requiredItem[0].SetDefaults(75);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(43);
      Recipe.newRecipe.requiredItem[0].SetDefaults(38);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.newRecipe.requiredTile[0] = 26;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(70);
      Recipe.newRecipe.requiredItem[0].SetDefaults(67);
      Recipe.newRecipe.requiredItem[0].stack = 30;
      Recipe.newRecipe.requiredItem[1].SetDefaults(68);
      Recipe.newRecipe.requiredItem[1].stack = 15;
      Recipe.newRecipe.requiredTile[0] = 26;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(71);
      Recipe.newRecipe.createItem.stack = 100;
      Recipe.newRecipe.requiredItem[0].SetDefaults(72);
      Recipe.newRecipe.requiredItem[0].stack = 1;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(72);
      Recipe.newRecipe.createItem.stack = 1;
      Recipe.newRecipe.requiredItem[0].SetDefaults(71);
      Recipe.newRecipe.requiredItem[0].stack = 100;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(72);
      Recipe.newRecipe.createItem.stack = 100;
      Recipe.newRecipe.requiredItem[0].SetDefaults(73);
      Recipe.newRecipe.requiredItem[0].stack = 1;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(73);
      Recipe.newRecipe.createItem.stack = 1;
      Recipe.newRecipe.requiredItem[0].SetDefaults(72);
      Recipe.newRecipe.requiredItem[0].stack = 100;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(73);
      Recipe.newRecipe.createItem.stack = 100;
      Recipe.newRecipe.requiredItem[0].SetDefaults(74);
      Recipe.newRecipe.requiredItem[0].stack = 1;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(74);
      Recipe.newRecipe.createItem.stack = 1;
      Recipe.newRecipe.requiredItem[0].SetDefaults(73);
      Recipe.newRecipe.requiredItem[0].stack = 100;
      Recipe.addRecipe();
    }

    private static void addRecipe()
    {
      Game1.recipe[Recipe.numRecipes] = Recipe.newRecipe;
      Recipe.newRecipe = new Recipe();
      ++Recipe.numRecipes;
    }
  }
}
