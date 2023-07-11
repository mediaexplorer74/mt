using System;
//using System.Windows.Threading;

namespace GameManager.Social.WeGame
{
	public class CurrentThreadRunner
	{
		private Dispatcher _dsipatcher;

		public CurrentThreadRunner()
		{
			_dsipatcher = Dispatcher.CurrentDispatcher;
		}

		public void Run(Action f)
		{
			_dsipatcher.BeginInvoke(f);
		}
	}
}
