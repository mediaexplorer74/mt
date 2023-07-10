using Microsoft.Xna.Framework;

namespace GameManager.GameContent.UI.Elements
{
	public interface IGroupOptionButton
	{
		void SetColorsBasedOnSelectionState(Color pickedColor, Color unpickedColor, float opacityPicked, float opacityNotPicked);

		void SetBorderColor(Color color);
	}
}
