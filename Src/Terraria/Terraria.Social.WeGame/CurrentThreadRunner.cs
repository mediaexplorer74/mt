using System;
//using System.Windows.Threading;

namespace GameManager.Social.WeGame
{
	//RnD

	public class CurrentThreadRunner
	{
		//private Dispatcher _dsipatcher;

		public CurrentThreadRunner()
		{
			//_dsipatcher = default;//Dispatcher.CurrentDispatcher;
		}

		public void Run(Action f)
		{
			//_dsipatcher.BeginInvoke(f);
		}
	}
}
