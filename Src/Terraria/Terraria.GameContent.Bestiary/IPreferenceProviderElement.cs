namespace GameManager.GameContent.Bestiary
{
	public interface IPreferenceProviderElement : IBestiaryInfoElement
	{
		IBestiaryBackgroundImagePathAndColorProvider GetPreferredProvider();

		bool Matches(IBestiaryBackgroundImagePathAndColorProvider provider);
	}
}
