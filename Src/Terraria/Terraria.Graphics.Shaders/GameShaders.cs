using System.Collections.Generic;

namespace GameManager.Graphics.Shaders
{
	public class GameShaders
	{
		public static ArmorShaderDataSet Armor = new ArmorShaderDataSet();

		public static HairShaderDataSet Hair = new HairShaderDataSet();

		public static Dictionary<string, MiscShaderData> Misc = new Dictionary<string, MiscShaderData>();
	}
}
