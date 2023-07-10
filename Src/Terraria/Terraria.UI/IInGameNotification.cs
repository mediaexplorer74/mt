using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager.UI
{
	public interface IInGameNotification
	{
		object CreationObject
		{
			get;
		}

		bool ShouldBeRemoved
		{
			get;
		}

		void Update();

		void DrawInGame(SpriteBatch spriteBatch, Vector2 bottomAnchorPosition);

		void PushAnchor(Vector2 positionAnchorBottom);

		void DrawInNotificationsArea(SpriteBatch spriteBatch, Rectangle area, int gamepadPointLocalIndexTouse);
	}
}
