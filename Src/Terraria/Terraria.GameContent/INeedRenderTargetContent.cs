using Microsoft.Xna.Framework.Graphics;

namespace GameManager.GameContent
{
	public interface INeedRenderTargetContent
	{
		bool IsReady
		{
			get;
		}

		void PrepareRenderTarget(GraphicsDevice device, SpriteBatch spriteBatch);
	}
}
