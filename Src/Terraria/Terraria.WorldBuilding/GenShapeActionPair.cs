namespace GameManager.WorldBuilding
{
	public struct GenShapeActionPair
	{
		public readonly GenShape Shape;

		public readonly GenAction Action;

		public GenShapeActionPair(GenShape shape, GenAction action)
		{
			Shape = shape;
			Action = action;
		}
	}
}
