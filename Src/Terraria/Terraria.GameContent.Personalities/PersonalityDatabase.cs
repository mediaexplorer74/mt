using System.Collections.Generic;

namespace GameManager.GameContent.Personalities
{
	public class PersonalityDatabase
	{
		private Dictionary<int, PersonalityProfile> _personalityProfiles;

		public PersonalityDatabase()
		{
			_personalityProfiles = new Dictionary<int, PersonalityProfile>();
		}

		private void Register(IShopPersonalityTrait trait, int npcId)
		{
			if (!_personalityProfiles.ContainsKey(npcId))
			{
				_personalityProfiles[npcId] = new PersonalityProfile();
			}
			_personalityProfiles[npcId].ShopModifiers.Add(trait);
		}

		private void Register(IShopPersonalityTrait trait, params int[] npcIds)
		{
			for (int i = 0; i < npcIds.Length; i++)
			{
				Register(trait, npcIds[i]);
			}
		}
	}
}
