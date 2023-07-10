using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using GameManager.UI;

namespace GameManager.GameContent.UI.Elements
{
	public class UIImage : UIElement
	{
		private Asset<Texture2D> _texture;

		public float ImageScale = 1f;

		public float Rotation;

		public bool ScaleToFit;

		public Color Color = Color.White;

		public Vector2 NormalizedOrigin = Vector2.Zero;

		public bool RemoveFloatingPointsFromDrawPosition;

		public UIImage(Asset<Texture2D> texture)
		{
			_texture = texture;
			Width.Set(_texture.Width(), 0f);
			Height.Set(_texture.Height(), 0f);
		}

		public void SetImage(Asset<Texture2D> texture)
		{
			_texture = texture;
			Width.Set(_texture.Width(), 0f);
			Height.Set(_texture.Height(), 0f);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			if (ScaleToFit)
			{
				spriteBatch.Draw(_texture.Value, dimensions.ToRectangle(), Color);
				return;
			}
			Vector2 vector = _texture.Value.Size();
			Vector2 vector2 = dimensions.Position() + vector * (1f - ImageScale) / 2f + vector * NormalizedOrigin;
			if (RemoveFloatingPointsFromDrawPosition)
			{
				vector2 = vector2;
			}
			spriteBatch.Draw(_texture.Value, vector2, null, Color, Rotation, vector * NormalizedOrigin, ImageScale, SpriteEffects.None, 0f);
		}
	}
}
