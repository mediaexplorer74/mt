using Microsoft.Xna.Framework;

namespace GameManager.UI.Chat
{
	public interface ITagHandler
	{
		TextSnippet Parse(string text, Color baseColor = default(Color), string options = null);
	}
}
