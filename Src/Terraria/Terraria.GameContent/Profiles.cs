using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using GameManager.Localization;

namespace GameManager.GameContent
{
	public class Profiles
	{
		public class LegacyNPCProfile : ITownNPCProfile
		{
			private string _rootFilePath;

			private int _defaultVariationHeadIndex;

			private Asset<Texture2D> _defaultNoAlt;

			private Asset<Texture2D> _defaultParty;

			public LegacyNPCProfile(string npcFileTitleFilePath, int defaultHeadIndex)
			{
				_rootFilePath = npcFileTitleFilePath;
				_defaultVariationHeadIndex = defaultHeadIndex;
				_defaultNoAlt = Main.Assets.Request<Texture2D>(npcFileTitleFilePath + "_Default", Main.content, (AssetRequestMode)0);
				_defaultParty = Main.Assets.Request<Texture2D>(npcFileTitleFilePath + "_Default_Party", Main.content, (AssetRequestMode)0);
			}

			public int RollVariation()
			{
				return 0;
			}

			public string GetNameForVariant(NPC npc)
			{
				return NPC.getNewNPCName(npc.type);
			}

			public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
			{
				if (npc.IsABestiaryIconDummy)
				{
					return _defaultNoAlt;
				}
				if (npc.altTexture == 1)
				{
					return _defaultParty;
				}
				return _defaultNoAlt;
			}

			public int GetHeadTextureIndex(NPC npc)
			{
				return _defaultVariationHeadIndex;
			}
		}

		public class TransformableNPCProfile : ITownNPCProfile
		{
			private string _rootFilePath;

			private int _defaultVariationHeadIndex;

			private Asset<Texture2D> _defaultNoAlt;

			private Asset<Texture2D> _defaultTransformed;

			public TransformableNPCProfile(string npcFileTitleFilePath, int defaultHeadIndex)
			{
				_rootFilePath = npcFileTitleFilePath;
				_defaultVariationHeadIndex = defaultHeadIndex;
				_defaultNoAlt = Main.Assets.Request<Texture2D>(npcFileTitleFilePath + "_Default", Main.content, (AssetRequestMode)0);
				_defaultTransformed = Main.Assets.Request<Texture2D>(npcFileTitleFilePath + "_Default_Transformed", Main.content, (AssetRequestMode)0);
			}

			public int RollVariation()
			{
				return 0;
			}

			public string GetNameForVariant(NPC npc)
			{
				return NPC.getNewNPCName(npc.type);
			}

			public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
			{
				if (npc.IsABestiaryIconDummy)
				{
					return _defaultNoAlt;
				}
				if (npc.altTexture == 2)
				{
					return _defaultTransformed;
				}
				return _defaultNoAlt;
			}

			public int GetHeadTextureIndex(NPC npc)
			{
				return _defaultVariationHeadIndex;
			}
		}

		public class VariantNPCProfile : ITownNPCProfile
		{
			private string _rootFilePath;

			private string _npcBaseName;

			private int[] _variantHeadIDs;

			private string[] _variants;

			private Dictionary<string, Asset<Texture2D>> _variantTextures = new Dictionary<string, Asset<Texture2D>>();

			public VariantNPCProfile(string npcFileTitleFilePath, string npcBaseName, int[] variantHeadIds, params string[] variantTextureNames)
			{
				_rootFilePath = npcFileTitleFilePath;
				_npcBaseName = npcBaseName;
				_variantHeadIDs = variantHeadIds;
				_variants = variantTextureNames;
				string[] variants = _variants;
				foreach (string str in variants)
				{
					string text = _rootFilePath + "_" + str;
					_variantTextures[text] = Main.Assets.Request<Texture2D>(text, Main.content, (AssetRequestMode)0);
				}
			}

			public int RollVariation()
			{
				return Main.rand.Next(_variants.Length);
			}

			public string GetNameForVariant(NPC npc)
			{
				return Language.RandomFromCategory(_npcBaseName + "Names_" + _variants[npc.townNpcVariationIndex], WorldGen.genRand).Value;
			}

			public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
			{
				string text = _rootFilePath + "_" + _variants[npc.townNpcVariationIndex];
				if (npc.IsABestiaryIconDummy)
				{
					return _variantTextures[text];
				}
				if (npc.altTexture == 1 && _variantTextures.ContainsKey(text + "_Party"))
				{
					return _variantTextures[text + "_Party"];
				}
				return _variantTextures[text];
			}

			public int GetHeadTextureIndex(NPC npc)
			{
				return _variantHeadIDs[npc.townNpcVariationIndex];
			}
		}
	}
}
