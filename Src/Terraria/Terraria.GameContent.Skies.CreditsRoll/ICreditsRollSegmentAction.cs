namespace GameManager.GameContent.Skies.CreditsRoll
{
	public interface ICreditsRollSegmentAction<T>
	{
		int ExpectedLengthOfActionInFrames
		{
			get;
		}

		void BindTo(T obj);

		void ApplyTo(T obj, float localTimeForObj);

		void SetDelay(float delay);
	}
}
