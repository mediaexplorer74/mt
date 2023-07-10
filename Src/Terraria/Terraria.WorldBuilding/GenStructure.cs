using Microsoft.Xna.Framework;

namespace GameManager.WorldBuilding
{
	public abstract class GenStructure : GenBase
	{
		public abstract bool Place(Point origin, StructureMap structures);
	}
}
