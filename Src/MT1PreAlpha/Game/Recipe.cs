

namespace GameManager
{
  public class Recipe
  {
    public static int maxRequirements = 10;
    public static int maxRecipes = 100;
    public static int numRecipes = 0;
    private static Recipe newRecipe = new Recipe();
    public Item createItem = new Item();
    public Item[] requiredItem = new Item[Recipe.maxRecipes];

    public Recipe()
    {
      for (int index = 0; index < Recipe.maxRequirements; ++index)
        this.requiredItem[index] = new Item();
    }

    public void Create()
    {
      for (int index1 = 0; index1 < Recipe.maxRequirements && this.requiredItem[index1].type != 0; ++index1)
      {
        int num = this.requiredItem[index1].stack;
        for (int index2 = 0; index2 < 40; ++index2)
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
        bool flag = true;
        for (int index2 = 0; index2 < Recipe.maxRequirements && Game1.recipe[index1].requiredItem[index2].type != 0; ++index2)
        {
          int stack = Game1.recipe[index1].requiredItem[index2].stack;
          for (int index3 = 0; index3 < 40; ++index3)
          {
            if (Game1.player[Game1.myPlayer].inventory[index3].IsTheSameAs(Game1.recipe[index1].requiredItem[index2]))
              stack -= Game1.player[Game1.myPlayer].inventory[index3].stack;
            if (stack <= 0)
              break;
          }
          if (stack > 0)
          {
            flag = false;
            break;
          }
        }
        if (flag)
        {
          Game1.availableRecipe[Game1.numAvailableRecipes] = index1;
          ++Game1.numAvailableRecipes;
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
      Recipe.newRecipe.createItem.SetDefaults(8);
      Recipe.newRecipe.createItem.stack = 3;
      Recipe.newRecipe.requiredItem[0].SetDefaults(23);
      Recipe.newRecipe.requiredItem[0].stack = 2;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(26);
      Recipe.newRecipe.createItem.stack = 4;
      Recipe.newRecipe.requiredItem[0].SetDefaults(3);
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(25);
      Recipe.newRecipe.requiredItem[0].SetDefaults(9);
      Recipe.newRecipe.requiredItem[0].stack = 5;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(24);
      Recipe.newRecipe.requiredItem[0].SetDefaults(9);
      Recipe.newRecipe.requiredItem[0].stack = 7;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(20);
      Recipe.newRecipe.requiredItem[0].SetDefaults(12);
      Recipe.newRecipe.requiredItem[0].stack = 3;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Copper Pickaxe");
      Recipe.newRecipe.requiredItem[0].SetDefaults(20);
      Recipe.newRecipe.requiredItem[0].stack = 12;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 4;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Copper Axe");
      Recipe.newRecipe.requiredItem[0].SetDefaults(20);
      Recipe.newRecipe.requiredItem[0].stack = 9;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 3;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Copper Hammer");
      Recipe.newRecipe.requiredItem[0].SetDefaults(20);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 3;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Copper Broadsword");
      Recipe.newRecipe.requiredItem[0].SetDefaults(20);
      Recipe.newRecipe.requiredItem[0].stack = 8;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Copper Shortsword");
      Recipe.newRecipe.requiredItem[0].SetDefaults(20);
      Recipe.newRecipe.requiredItem[0].stack = 7;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(19);
      Recipe.newRecipe.requiredItem[0].SetDefaults(13);
      Recipe.newRecipe.requiredItem[0].stack = 3;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Gold Pickaxe");
      Recipe.newRecipe.requiredItem[0].SetDefaults(19);
      Recipe.newRecipe.requiredItem[0].stack = 12;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 4;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Gold Axe");
      Recipe.newRecipe.requiredItem[0].SetDefaults(19);
      Recipe.newRecipe.requiredItem[0].stack = 9;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 3;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Gold Hammer");
      Recipe.newRecipe.requiredItem[0].SetDefaults(19);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 3;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Gold Broadsword");
      Recipe.newRecipe.requiredItem[0].SetDefaults(19);
      Recipe.newRecipe.requiredItem[0].stack = 8;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Gold Shortsword");
      Recipe.newRecipe.requiredItem[0].SetDefaults(19);
      Recipe.newRecipe.requiredItem[0].stack = 7;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(22);
      Recipe.newRecipe.requiredItem[0].SetDefaults(11);
      Recipe.newRecipe.requiredItem[0].stack = 3;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(1);
      Recipe.newRecipe.requiredItem[0].SetDefaults(22);
      Recipe.newRecipe.requiredItem[0].stack = 12;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 3;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(10);
      Recipe.newRecipe.requiredItem[0].SetDefaults(22);
      Recipe.newRecipe.requiredItem[0].stack = 9;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 3;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(7);
      Recipe.newRecipe.requiredItem[0].SetDefaults(22);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 3;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(4);
      Recipe.newRecipe.requiredItem[0].SetDefaults(22);
      Recipe.newRecipe.requiredItem[0].stack = 8;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(6);
      Recipe.newRecipe.requiredItem[0].SetDefaults(22);
      Recipe.newRecipe.requiredItem[0].stack = 7;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults(21);
      Recipe.newRecipe.requiredItem[0].SetDefaults(14);
      Recipe.newRecipe.requiredItem[0].stack = 3;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Silver Pickaxe");
      Recipe.newRecipe.requiredItem[0].SetDefaults(21);
      Recipe.newRecipe.requiredItem[0].stack = 12;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 4;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Silver Axe");
      Recipe.newRecipe.requiredItem[0].SetDefaults(21);
      Recipe.newRecipe.requiredItem[0].stack = 9;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 3;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Silver Hammer");
      Recipe.newRecipe.requiredItem[0].SetDefaults(21);
      Recipe.newRecipe.requiredItem[0].stack = 10;
      Recipe.newRecipe.requiredItem[1].SetDefaults(9);
      Recipe.newRecipe.requiredItem[1].stack = 3;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Silver Broadsword");
      Recipe.newRecipe.requiredItem[0].SetDefaults(21);
      Recipe.newRecipe.requiredItem[0].stack = 8;
      Recipe.addRecipe();
      Recipe.newRecipe.createItem.SetDefaults("Silver Shortsword");
      Recipe.newRecipe.requiredItem[0].SetDefaults(21);
      Recipe.newRecipe.requiredItem[0].stack = 7;
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
