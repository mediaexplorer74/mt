using System.Collections.Generic;
using GameManager.DataStructures;

namespace GameManager.GameContent.Creative
{
	public interface ICreativeItemSortStep : IEntrySortStep<int>, IComparer<int>
	{
	}
}
