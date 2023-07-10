using Microsoft.Xna.Framework.Graphics;

namespace GameManager.Graphics.Renderers
{
	public interface IParticle
	{
		bool ShouldBeRemovedFromRenderer
		{
			get;
		}

		void Update(ParticleRendererSettings settings);

		void Draw(ParticleRendererSettings settings, SpriteBatch spritebatch);
	}
}
