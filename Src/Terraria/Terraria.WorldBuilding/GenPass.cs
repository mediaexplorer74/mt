using System;
using GameManager.IO;

namespace GameManager.WorldBuilding
{
	public abstract class GenPass : GenBase
	{
		public string Name;

		public float Weight;

		private Action<GenPass> _onComplete;

		private Action<GenPass> _onBegin;

		public GenPass(string name, float loadWeight)
		{
			Name = name;
			Weight = loadWeight;
		}

		protected abstract void ApplyPass(GenerationProgress progress, GameConfiguration configuration);

		public void Apply(GenerationProgress progress, GameConfiguration configuration)
		{
			if (_onBegin != null)
			{
				_onBegin(this);
			}
			ApplyPass(progress, configuration);
			if (_onComplete != null)
			{
				_onComplete(this);
			}
		}

		public GenPass OnBegin(Action<GenPass> beginAction)
		{
			_onBegin = beginAction;
			return this;
		}

		public GenPass OnComplete(Action<GenPass> completionAction)
		{
			_onComplete = completionAction;
			return this;
		}
	}
}
