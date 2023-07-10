using System.Threading;

namespace GameManager.Social.WeGame
{
	public class IPCContent
	{
		public byte[] data;

		public CancellationToken CancelToken
		{
			get;
			set;
		}
	}
}
