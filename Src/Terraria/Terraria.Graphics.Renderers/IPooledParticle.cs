namespace GameManager.Graphics.Renderers
{
	public interface IPooledParticle : IParticle
	{
		bool IsRestingInPool
		{
			get;
		}

		void RestInPool();

		void FetchFromPool();
	}
}
