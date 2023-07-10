using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.GameContent.Events;
using GameManager.Graphics.Effects;
using GameManager.Utilities;

namespace GameManager.GameContent.Skies
{
	public class SandstormSky : CustomSky
	{
		private UnifiedRandom _random = new UnifiedRandom();

		private bool _isActive;

		private bool _isLeaving;

		private float _opacity;

		public override void OnLoad()
		{
		}

		public override void Update(GameTime gameTime)
		{
			if (Main.gamePaused || !Main.hasFocus)
			{
				return;
			}
			if (_isLeaving)
			{
				_opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (_opacity < 0f)
				{
					_isActive = false;
					_opacity = 0f;
				}
			}
			else
			{
				_opacity += (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (_opacity > 1f)
				{
					_opacity = 1f;
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (minDepth < 1f || maxDepth == float.MaxValue)
			{
				float scale = Math.Min(1f, Sandstorm.Severity * 1.5f);
				Color color = new Color(new Vector4(0.85f, 0.66f, 0.33f, 1f) * 0.8f * Main.ColorOfTheSkies.ToVector4()) * _opacity * scale;
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), color);
			}
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			_isActive = true;
			_isLeaving = false;
		}

		public override void Deactivate(params object[] args)
		{
			_isLeaving = true;
		}

		public override void Reset()
		{
			_opacity = 0f;
			_isActive = false;
		}

		public override bool IsActive()
		{
			return _isActive;
		}
	}
}
