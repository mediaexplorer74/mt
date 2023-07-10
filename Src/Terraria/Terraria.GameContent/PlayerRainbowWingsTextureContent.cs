using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using GameManager.DataStructures;
using GameManager.Graphics.Shaders;

namespace GameManager.GameContent
{
	public class PlayerRainbowWingsTextureContent : ARenderTargetContentByRequest
	{
		protected override void HandleUseReqest(GraphicsDevice device, SpriteBatch spriteBatch)
		{
			Asset<Texture2D> val = TextureAssets.Extra[171];
			PrepareARenderTarget_AndListenToEvents(_target, device, val.Width(), val.Height(), RenderTargetUsage.PreserveContents);
			device.SetRenderTarget(_target);
			device.Clear(Color.Transparent);
			DrawData value = new DrawData(val.Value, Vector2.Zero, Color.White);
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			GameShaders.Misc["HallowBoss"].Apply(value);
			value.Draw(spriteBatch);
			spriteBatch.End();
			device.SetRenderTarget(null);
			_wasPrepared = true;
		}
	}
}
