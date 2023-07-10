using System.Collections.Generic;
using GameManager.DataStructures;

namespace GameManager.GameContent.Bestiary
{
	public interface IBestiarySortStep : IEntrySortStep<BestiaryEntry>, IComparer<BestiaryEntry>
	{
		bool HiddenFromSortOptions
		{
			get;
		}
	}
}
