using GameManager.GameContent.UI.Elements;

namespace GameManager.GameContent.Creative
{
	public interface IPowerSubcategoryElement
	{
		GroupOptionButton<int> GetOptionButton(CreativePowerUIElementRequestInfo info, int optionIndex, int currentOptionIndex);
	}
}
