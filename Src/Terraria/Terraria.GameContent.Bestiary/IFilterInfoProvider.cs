using GameManager.UI;

namespace GameManager.GameContent.Bestiary
{
	public interface IFilterInfoProvider
	{
		UIElement GetFilterImage();

		string GetDisplayNameKey();
	}
}
