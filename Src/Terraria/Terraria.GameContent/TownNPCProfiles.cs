using System.Collections.Generic;

namespace GameManager.GameContent
{
	public class TownNPCProfiles
	{
		private const string DefaultNPCFileFolderPath = "Images/TownNPCs/";

		private static readonly int[] CatHeadIDs = new int[6]
		{
			27,
			28,
			29,
			30,
			31,
			32
		};

		private static readonly int[] DogHeadIDs = new int[6]
		{
			33,
			34,
			35,
			36,
			37,
			38
		};

		private static readonly int[] BunnyHeadIDs = new int[6]
		{
			39,
			40,
			41,
			42,
			43,
			44
		};

		private Dictionary<int, ITownNPCProfile> _townNPCProfiles = new Dictionary<int, ITownNPCProfile>
		{
			{
				369,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/Angler", 22)
			},
			{
				633,
				new Profiles.TransformableNPCProfile("Images/TownNPCs/BestiaryGirl", 26)
			},
			{
				54,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/Clothier", 7)
			},
			{
				209,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/Cyborg", 16)
			},
			{
				38,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/Demolitionist", 4)
			},
			{
				207,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/DyeTrader", 14)
			},
			{
				588,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/Golfer", 25)
			},
			{
				124,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/Mechanic", 8)
			},
			{
				17,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/Merchant", 2)
			},
			{
				18,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/Nurse", 3)
			},
			{
				227,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/Painter", 17)
			},
			{
				229,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/Pirate", 19)
			},
			{
				142,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/Santa", 11)
			},
			{
				453,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/SkeletonMerchant", -1)
			},
			{
				178,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/Steampunker", 13)
			},
			{
				353,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/Stylist", 20)
			},
			{
				441,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/TaxCollector", 23)
			},
			{
				368,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/TravelingMerchant", 21)
			},
			{
				108,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/Wizard", 10)
			},
			{
				637,
				new Profiles.VariantNPCProfile("Images/TownNPCs/Cat", "Cat", CatHeadIDs, "Siamese", "Black", "OrangeTabby", "RussianBlue", "Silver", "White")
			},
			{
				638,
				new Profiles.VariantNPCProfile("Images/TownNPCs/Dog", "Dog", DogHeadIDs, "Labrador", "PitBull", "Beagle", "Corgi", "Dalmation", "Husky")
			},
			{
				656,
				new Profiles.VariantNPCProfile("Images/TownNPCs/Bunny", "Bunny", BunnyHeadIDs, "White", "Angora", "Dutch", "Flemish", "Lop", "Silver")
			}
		};

		public static TownNPCProfiles Instance = new TownNPCProfiles();

		public bool GetProfile(int npcId, out ITownNPCProfile profile)
		{
			return _townNPCProfiles.TryGetValue(npcId, out profile);
		}
	}
}
