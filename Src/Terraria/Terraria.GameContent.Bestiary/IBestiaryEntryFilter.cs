using GameManager.DataStructures;

namespace GameManager.GameContent.Bestiary
{
	public interface IBestiaryEntryFilter : IEntryFilter<BestiaryEntry>
	{
		bool? ForcedDisplay
		{
			get;
		}
	}
}
