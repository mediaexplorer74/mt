using GameManager.UI;

namespace GameManager.DataStructures
{
	public interface IEntryFilter<T>
	{
		bool FitsFilter(T entry);

		string GetDisplayNameKey();

		UIElement GetImage();
	}
}
