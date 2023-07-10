using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.GameContent;

namespace GameManager.Graphics.Renderers
{
	public class PrettySparkleParticle : ABasicParticle
	{
		public Color ColorTint;

		public float Opacity;

		private float _timeSinceSpawn;

		public override void FetchFromPool()
		{
			base.FetchFromPool();
			ColorTint = Color.Transparent;
			_timeSinceSpawn = 0f;
			Opacity = 0f;
		}

		public override void Update(ParticleRendererSettings settings)
		{
			base.Update(settings);
			_timeSinceSpawn += 1f;
			Opacity = Utils.GetLerpValue(0f, 0.05f, _timeSinceSpawn / 60f, clamped: true) * Utils.GetLerpValue(1f, 0.9f, _timeSinceSpawn / 60f, clamped: true);
			if (_timeSinceSpawn >= 60f)
			{
				base.ShouldBeRemovedFromRenderer = true;
			}
		}

		public override void Draw(ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			Color value = Color.White * Opacity * 0.9f;
			value.A /= 2;
			Texture2D value2 = TextureAssets.Extra[98].Value;
			Color color = ColorTint * Opacity * 0.5f;
			color.A = 0;
			Vector2 origin = value2.Size() / 2f;
			Color color2 = value * 0.5f;
			float num = Utils.GetLerpValue(0f, 20f, _timeSinceSpawn, clamped: true) * Utils.GetLerpValue(45f, 30f, _timeSinceSpawn, clamped: true);
			Vector2 vector = new Vector2(0.3f, 2f) * num * Scale;
			Vector2 vector2 = new Vector2(0.3f, 1f) * num * Scale;
			color *= num;
			color2 *= num;
			Vector2 position = settings.AnchorPosition + LocalPosition;
			SpriteEffects effects = SpriteEffects.None;
			spritebatch.Draw(value2, position, null, color, (float)Math.PI / 2f + Rotation, origin, vector, effects, 0f);
			spritebatch.Draw(value2, position, null, color, 0f + Rotation, origin, vector2, effects, 0f);
			spritebatch.Draw(value2, position, null, color2, (float)Math.PI / 2f + Rotation, origin, vector * 0.6f, effects, 0f);
			spritebatch.Draw(value2, position, null, color2, 0f + Rotation, origin, vector2 * 0.6f, effects, 0f);
		}
	}
}
