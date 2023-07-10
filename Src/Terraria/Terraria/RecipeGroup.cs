using System;
using System.Collections.Generic;

namespace GameManager
{
	public class RecipeGroup
	{
		public Func<string> GetText;

		public HashSet<int> ValidItems;

		public int IconicItemId;

		public static Dictionary<int, RecipeGroup> recipeGroups = new Dictionary<int, RecipeGroup>();

		public static Dictionary<string, int> recipeGroupIDs = new Dictionary<string, int>();

		public static int nextRecipeGroupIndex;

		public RecipeGroup(Func<string> getName, params int[] validItems)
		{
			GetText = getName;
			ValidItems = new HashSet<int>(validItems);
			IconicItemId = validItems[0];
		}

		public static int RegisterGroup(string name, RecipeGroup rec)
		{
			int num = nextRecipeGroupIndex++;
			recipeGroups.Add(num, rec);
			recipeGroupIDs.Add(name, num);
			return num;
		}
	}
}
