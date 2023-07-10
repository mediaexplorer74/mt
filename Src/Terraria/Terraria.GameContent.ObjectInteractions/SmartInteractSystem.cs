using System.Collections.Generic;

namespace GameManager.GameContent.ObjectInteractions
{
	public class SmartInteractSystem
	{
		private List<ISmartInteractCandidateProvider> _candidateProvidersByOrderOfPriority = new List<ISmartInteractCandidateProvider>();

		private List<ISmartInteractBlockReasonProvider> _blockProviders = new List<ISmartInteractBlockReasonProvider>();

		private List<ISmartInteractCandidate> _candidates = new List<ISmartInteractCandidate>();

		public SmartInteractSystem()
		{
			_candidateProvidersByOrderOfPriority.Add(new PotionOfReturnSmartInteractCandidateProvider());
			_candidateProvidersByOrderOfPriority.Add(new ProjectileSmartInteractCandidateProvider());
			_candidateProvidersByOrderOfPriority.Add(new NPCSmartInteractCandidateProvider());
			_candidateProvidersByOrderOfPriority.Add(new TileSmartInteractCandidateProvider());
			_blockProviders.Add(new BlockBecauseYouAreOverAnImportantTile());
		}

		public void RunQuery(SmartInteractScanSettings settings)
		{
			_candidates.Clear();
			foreach (ISmartInteractBlockReasonProvider blockProvider in _blockProviders)
			{
				if (blockProvider.ShouldBlockSmartInteract(settings))
				{
					return;
				}
			}
			foreach (ISmartInteractCandidateProvider item in _candidateProvidersByOrderOfPriority)
			{
				item.ClearSelfAndPrepareForCheck();
			}
			foreach (ISmartInteractCandidateProvider item2 in _candidateProvidersByOrderOfPriority)
			{
				if (item2.ProvideCandidate(settings, out var candidate))
				{
					_candidates.Add(candidate);
					if (candidate.DistanceFromCursor == 0f)
					{
						break;
					}
				}
			}
			ISmartInteractCandidate smartInteractCandidate = null;
			foreach (ISmartInteractCandidate candidate2 in _candidates)
			{
				if (smartInteractCandidate == null || smartInteractCandidate.DistanceFromCursor > candidate2.DistanceFromCursor)
				{
					smartInteractCandidate = candidate2;
				}
			}
			smartInteractCandidate?.WinCandidacy();
		}
	}
}
