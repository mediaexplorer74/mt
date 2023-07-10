namespace GameManager.Map
{
	public interface IMapLayer
	{
		void Draw(MapOverlayDrawContext context, string text);
	}
}
