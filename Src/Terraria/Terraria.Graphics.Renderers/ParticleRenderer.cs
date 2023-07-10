using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager.Graphics.Renderers
{
	public class ParticleRenderer
	{
		public ParticleRendererSettings Settings;

		public List<IParticle> Particles = new List<IParticle>();

		public ParticleRenderer()
		{
			Settings = default(ParticleRendererSettings);
		}

		public void Add(IParticle particle)
		{
			Particles.Add(particle);
		}

		public void Update()
		{
			for (int i = 0; i < Particles.Count; i++)
			{
				if (Particles[i].ShouldBeRemovedFromRenderer)
				{
					(Particles[i] as IPooledParticle)?.RestInPool();
					Particles.RemoveAt(i);
					i--;
				}
				else
				{
					Particles[i].Update(Settings);
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < Particles.Count; i++)
			{
				if (!Particles[i].ShouldBeRemovedFromRenderer)
				{
					Particles[i].Draw(Settings, spriteBatch);
				}
			}
		}
	}
}
