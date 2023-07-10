namespace GameManager.GameContent.ObjectInteractions
{
	public interface ISmartInteractBlockReasonProvider
	{
		bool ShouldBlockSmartInteract(SmartInteractScanSettings settings);
	}
}
