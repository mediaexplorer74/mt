namespace GameManager.GameContent.ObjectInteractions
{
	public class PotionOfReturnSmartInteractCandidateProvider : ISmartInteractCandidateProvider
	{
		private class ReusableCandidate : ISmartInteractCandidate
		{
			public float DistanceFromCursor
			{
				get;
				private set;
			}

			public void WinCandidacy()
			{
				Main.SmartInteractPotionOfReturn = true;
				Main.SmartInteractShowingGenuine = true;
			}

			public void Reuse(float distanceFromCursor)
			{
				DistanceFromCursor = distanceFromCursor;
			}
		}

		private ReusableCandidate _candidate = new ReusableCandidate();

		public void ClearSelfAndPrepareForCheck()
		{
			Main.SmartInteractPotionOfReturn = false;
		}

		public bool ProvideCandidate(SmartInteractScanSettings settings, out ISmartInteractCandidate candidate)
		{
			candidate = null;
			if (!PotionOfReturnHelper.TryGetGateHitbox(settings.player, out var homeHitbox))
			{
				return false;
			}
			float distanceFromCursor = homeHitbox.ClosestPointInRect(settings.mousevec).Distance(settings.mousevec);
			_candidate.Reuse(distanceFromCursor);
			candidate = _candidate;
			return true;
		}
	}
}
