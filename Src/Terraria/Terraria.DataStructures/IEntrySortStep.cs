using System.Collections.Generic;

namespace GameManager.DataStructures
{
	public interface IEntrySortStep<T> : IComparer<T>
	{
		string GetDisplayNameKey();
	}
}
