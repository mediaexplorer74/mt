using GameManager.UI;

namespace GameManager.GameContent.Bestiary
{
	public interface IBestiaryInfoElement
	{
		UIElement ProvideUIElement(BestiaryUICollectionInfo info);
	}
}
