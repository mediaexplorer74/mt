using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using ReLogic.Content;
using ReLogic.Content.Readers;
using ReLogic.Graphics;
using ReLogic.Utilities;
using GameManager.Audio;
using GameManager.GameContent;
using GameManager.GameContent.UI;
using GameManager.ID;
using GameManager.IO;
using GameManager.Utilities;

namespace GameManager.Initializers
{
	public static class AssetInitializer
	{
		public static void CreateAssetServices(GameServiceContainer services)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			AssetReaderCollection val = new AssetReaderCollection();
			val.RegisterReader((IAssetReader)new PngReader(XnaExtensions.Get<IGraphicsDeviceService>((IServiceProvider)services).GraphicsDevice), new string[1]
			{
				".png"
			});
			val.RegisterReader((IAssetReader)new XnbReader((IServiceProvider)services), new string[1]
			{
				".xnb"
			});
			AsyncAssetLoader val2 = new AsyncAssetLoader(val, 20, Main.content);
			val2.RequireTypeCreationOnTransfer(typeof(Texture2D));
			val2.RequireTypeCreationOnTransfer(typeof(DynamicSpriteFont));
			val2.RequireTypeCreationOnTransfer(typeof(SpriteFont));
			IAssetRepository provider = (IAssetRepository)new AssetRepository((IAssetLoader)new AssetLoader(val), (IAsyncAssetLoader)(object)val2);
			services.AddService(typeof(AssetReaderCollection), val);
			services.AddService(typeof(IAssetRepository), provider);
		}

		public static ResourcePackList CreateResourcePackList(IServiceProvider services)
		{
			GetResourcePacksFolderPathAndConfirmItExists(out var resourcePackJson, out var resourcePackFolder);
			return ResourcePackList.FromJson(resourcePackJson, services, resourcePackFolder);
		}

		public static void GetResourcePacksFolderPathAndConfirmItExists(out JArray resourcePackJson, out string resourcePackFolder)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			resourcePackJson = Main.Configuration.Get<JArray>("ResourcePacks", new JArray());
			resourcePackFolder = Path.Combine(Main.SavePath, "ResourcePacks");
			Utils.TryCreatingDirectory(resourcePackFolder);
		}

		public static void LoadSplashAssets(bool asyncLoadForSounds)
		{
			TextureAssets.SplashTexture16x9 = LoadAsset<Texture2D>("Images\\SplashScreens\\Splash_1", Main.content, (AssetRequestMode)1);
			TextureAssets.SplashTexture4x3 = LoadAsset<Texture2D>("Images\\logo_" + new UnifiedRandom().Next(1, 9), Main.content, (AssetRequestMode)1);
			TextureAssets.SplashTextureLegoResonanace = LoadAsset<Texture2D>("Images\\SplashScreens\\ResonanceArray", Main.content, (AssetRequestMode)1);
			int num = new UnifiedRandom().Next(1, 10);
			TextureAssets.SplashTextureLegoBack = LoadAsset<Texture2D>("Images\\SplashScreens\\Splash_" + num + "_0", Main.content, (AssetRequestMode)1);
			TextureAssets.SplashTextureLegoTree = LoadAsset<Texture2D>("Images\\SplashScreens\\Splash_" + num + "_1", Main.content, (AssetRequestMode)1);
			TextureAssets.SplashTextureLegoFront = LoadAsset<Texture2D>("Images\\SplashScreens\\Splash_" + num + "_2", Main.content, (AssetRequestMode)1);
			TextureAssets.Item[75] = LoadAsset<Texture2D>("Images\\Item_" + (short)75, Main.content, (AssetRequestMode)1);
			TextureAssets.LoadingSunflower = LoadAsset<Texture2D>("Images\\UI\\Sunflower_Loading", Main.content, (AssetRequestMode)1);
		}

		public static void LoadAssetsWhileInInitialBlackScreen()
		{
			LoadFonts((AssetRequestMode)1);
			LoadTextures((AssetRequestMode)1);
			LoadRenderTargetAssets((AssetRequestMode)1);
			LoadSounds((AssetRequestMode)1);
		}

		public static void Load(bool asyncLoad)
		{
		}

		private static void LoadFonts(AssetRequestMode mode)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			FontAssets.ItemStack = LoadAsset<DynamicSpriteFont>("Fonts/Item_Stack", Main.content, mode);
			FontAssets.MouseText = LoadAsset<DynamicSpriteFont>("Fonts/Mouse_Text", Main.content, mode);
			FontAssets.DeathText = LoadAsset<DynamicSpriteFont>("Fonts/Death_Text", Main.content, mode);
			FontAssets.CombatText[0] = LoadAsset<DynamicSpriteFont>("Fonts/Combat_Text", Main.content, mode);
			FontAssets.CombatText[1] = LoadAsset<DynamicSpriteFont>("Fonts/Combat_Crit", Main.content, mode);
		}

		private static void LoadSounds(AssetRequestMode mode)
		{
			SoundEngine.Load(Main.instance.Services);
		}

		private static void LoadRenderTargetAssets(AssetRequestMode mode)
		{
			RegisterRenderTargetAsset(TextureAssets.RenderTargets.PlayerRainbowWings = new PlayerRainbowWingsTextureContent());
			RegisterRenderTargetAsset(TextureAssets.RenderTargets.PlayerTitaniumStormBuff = new PlayerTitaniumStormBuffTextureContent());
			RegisterRenderTargetAsset(TextureAssets.RenderTargets.QueenSlimeMount = new PlayerQueenSlimeMountTextureContent());
		}

		private static void RegisterRenderTargetAsset(INeedRenderTargetContent content)
		{
			Main.ContentThatNeedsRenderTargets.Add(content);
		}

		private static void LoadTextures(AssetRequestMode mode)
		{
			//IL_0624: Unknown result type (might be due to invalid IL or missing references)
			//IL_063a: Unknown result type (might be due to invalid IL or missing references)
			//IL_064c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0658: Unknown result type (might be due to invalid IL or missing references)
			//IL_0668: Unknown result type (might be due to invalid IL or missing references)
			//IL_0678: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_071a: Unknown result type (might be due to invalid IL or missing references)
			//IL_074f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0784: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0823: Unknown result type (might be due to invalid IL or missing references)
			//IL_0858: Unknown result type (might be due to invalid IL or missing references)
			//IL_088d: Unknown result type (might be due to invalid IL or missing references)
			//IL_08aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_08da: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_090a: Unknown result type (might be due to invalid IL or missing references)
			//IL_091a: Unknown result type (might be due to invalid IL or missing references)
			//IL_092a: Unknown result type (might be due to invalid IL or missing references)
			//IL_093a: Unknown result type (might be due to invalid IL or missing references)
			//IL_094a: Unknown result type (might be due to invalid IL or missing references)
			//IL_095a: Unknown result type (might be due to invalid IL or missing references)
			//IL_096a: Unknown result type (might be due to invalid IL or missing references)
			//IL_097a: Unknown result type (might be due to invalid IL or missing references)
			//IL_098a: Unknown result type (might be due to invalid IL or missing references)
			//IL_099a: Unknown result type (might be due to invalid IL or missing references)
			//IL_09aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_09da: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_09fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a0a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a34: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a4b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a5b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a6b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a7b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a8b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a9b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aab: Unknown result type (might be due to invalid IL or missing references)
			//IL_0abb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0acb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0adb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aeb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b15: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b32: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b42: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b52: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b62: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b72: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b82: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b92: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bba: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bf7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c2c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c76: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c86: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c96: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cbe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cf3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d22: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d57: Unknown result type (might be due to invalid IL or missing references)
			//IL_0da9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dbb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dcd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dd9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0de9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0df9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e09: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e19: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e29: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e39: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e49: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e59: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e81: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eb6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eeb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f20: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f52: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f7a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f97: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fa7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fb7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fc7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fd7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fe7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ffd: Unknown result type (might be due to invalid IL or missing references)
			//IL_100f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1021: Unknown result type (might be due to invalid IL or missing references)
			//IL_1033: Unknown result type (might be due to invalid IL or missing references)
			//IL_103f: Unknown result type (might be due to invalid IL or missing references)
			//IL_104f: Unknown result type (might be due to invalid IL or missing references)
			//IL_105f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1075: Unknown result type (might be due to invalid IL or missing references)
			//IL_1087: Unknown result type (might be due to invalid IL or missing references)
			//IL_1099: Unknown result type (might be due to invalid IL or missing references)
			//IL_10ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_10bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_10cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_10e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_10f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_1105: Unknown result type (might be due to invalid IL or missing references)
			//IL_1117: Unknown result type (might be due to invalid IL or missing references)
			//IL_1129: Unknown result type (might be due to invalid IL or missing references)
			//IL_113b: Unknown result type (might be due to invalid IL or missing references)
			//IL_114d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1171: Unknown result type (might be due to invalid IL or missing references)
			//IL_11a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_11d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_1205: Unknown result type (might be due to invalid IL or missing references)
			//IL_1224: Unknown result type (might be due to invalid IL or missing references)
			//IL_1251: Unknown result type (might be due to invalid IL or missing references)
			//IL_1263: Unknown result type (might be due to invalid IL or missing references)
			//IL_1275: Unknown result type (might be due to invalid IL or missing references)
			//IL_1287: Unknown result type (might be due to invalid IL or missing references)
			//IL_1299: Unknown result type (might be due to invalid IL or missing references)
			//IL_12ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_12b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_12c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_12d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_12e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_12f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_1307: Unknown result type (might be due to invalid IL or missing references)
			//IL_1317: Unknown result type (might be due to invalid IL or missing references)
			//IL_1327: Unknown result type (might be due to invalid IL or missing references)
			//IL_1337: Unknown result type (might be due to invalid IL or missing references)
			//IL_1347: Unknown result type (might be due to invalid IL or missing references)
			//IL_1357: Unknown result type (might be due to invalid IL or missing references)
			//IL_1367: Unknown result type (might be due to invalid IL or missing references)
			//IL_1377: Unknown result type (might be due to invalid IL or missing references)
			//IL_1387: Unknown result type (might be due to invalid IL or missing references)
			//IL_1397: Unknown result type (might be due to invalid IL or missing references)
			//IL_13a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_13b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_13df: Unknown result type (might be due to invalid IL or missing references)
			//IL_13fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_140c: Unknown result type (might be due to invalid IL or missing references)
			//IL_141c: Unknown result type (might be due to invalid IL or missing references)
			//IL_142c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1442: Unknown result type (might be due to invalid IL or missing references)
			//IL_1454: Unknown result type (might be due to invalid IL or missing references)
			//IL_1466: Unknown result type (might be due to invalid IL or missing references)
			//IL_1478: Unknown result type (might be due to invalid IL or missing references)
			//IL_148a: Unknown result type (might be due to invalid IL or missing references)
			//IL_149c: Unknown result type (might be due to invalid IL or missing references)
			//IL_14ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_14ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_14ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_14da: Unknown result type (might be due to invalid IL or missing references)
			//IL_1502: Unknown result type (might be due to invalid IL or missing references)
			//IL_1537: Unknown result type (might be due to invalid IL or missing references)
			//IL_156c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1589: Unknown result type (might be due to invalid IL or missing references)
			//IL_1599: Unknown result type (might be due to invalid IL or missing references)
			//IL_15a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_15b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_15c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_15d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_15e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_15f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_1609: Unknown result type (might be due to invalid IL or missing references)
			//IL_1619: Unknown result type (might be due to invalid IL or missing references)
			//IL_1629: Unknown result type (might be due to invalid IL or missing references)
			//IL_1639: Unknown result type (might be due to invalid IL or missing references)
			//IL_1649: Unknown result type (might be due to invalid IL or missing references)
			//IL_1659: Unknown result type (might be due to invalid IL or missing references)
			//IL_1669: Unknown result type (might be due to invalid IL or missing references)
			//IL_1679: Unknown result type (might be due to invalid IL or missing references)
			//IL_1689: Unknown result type (might be due to invalid IL or missing references)
			//IL_1699: Unknown result type (might be due to invalid IL or missing references)
			//IL_16a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_16b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_16c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_16d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_16e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_16f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_1709: Unknown result type (might be due to invalid IL or missing references)
			//IL_1719: Unknown result type (might be due to invalid IL or missing references)
			//IL_1729: Unknown result type (might be due to invalid IL or missing references)
			//IL_1739: Unknown result type (might be due to invalid IL or missing references)
			//IL_174e: Unknown result type (might be due to invalid IL or missing references)
			//IL_175e: Unknown result type (might be due to invalid IL or missing references)
			//IL_176e: Unknown result type (might be due to invalid IL or missing references)
			//IL_177e: Unknown result type (might be due to invalid IL or missing references)
			//IL_178e: Unknown result type (might be due to invalid IL or missing references)
			//IL_179e: Unknown result type (might be due to invalid IL or missing references)
			//IL_17ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_17be: Unknown result type (might be due to invalid IL or missing references)
			//IL_17ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_17de: Unknown result type (might be due to invalid IL or missing references)
			//IL_17ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_1816: Unknown result type (might be due to invalid IL or missing references)
			//IL_182e: Unknown result type (might be due to invalid IL or missing references)
			//IL_183e: Unknown result type (might be due to invalid IL or missing references)
			//IL_184e: Unknown result type (might be due to invalid IL or missing references)
			//IL_185e: Unknown result type (might be due to invalid IL or missing references)
			//IL_186e: Unknown result type (might be due to invalid IL or missing references)
			//IL_187e: Unknown result type (might be due to invalid IL or missing references)
			//IL_188e: Unknown result type (might be due to invalid IL or missing references)
			//IL_189e: Unknown result type (might be due to invalid IL or missing references)
			//IL_18ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_18be: Unknown result type (might be due to invalid IL or missing references)
			//IL_18ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_18de: Unknown result type (might be due to invalid IL or missing references)
			//IL_18ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_18fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_190e: Unknown result type (might be due to invalid IL or missing references)
			//IL_191e: Unknown result type (might be due to invalid IL or missing references)
			//IL_192e: Unknown result type (might be due to invalid IL or missing references)
			//IL_193e: Unknown result type (might be due to invalid IL or missing references)
			//IL_194e: Unknown result type (might be due to invalid IL or missing references)
			//IL_195e: Unknown result type (might be due to invalid IL or missing references)
			//IL_196e: Unknown result type (might be due to invalid IL or missing references)
			//IL_197e: Unknown result type (might be due to invalid IL or missing references)
			//IL_198e: Unknown result type (might be due to invalid IL or missing references)
			//IL_199e: Unknown result type (might be due to invalid IL or missing references)
			//IL_19ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_19be: Unknown result type (might be due to invalid IL or missing references)
			//IL_19ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_19de: Unknown result type (might be due to invalid IL or missing references)
			//IL_19ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_19fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a0e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a1e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a2e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a3e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a4e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a5e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a6e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a7e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a8e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a9e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1aae: Unknown result type (might be due to invalid IL or missing references)
			//IL_1abe: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ace: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ade: Unknown result type (might be due to invalid IL or missing references)
			//IL_1aee: Unknown result type (might be due to invalid IL or missing references)
			//IL_1afe: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b0e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b1e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b2e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b3e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b66: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b9b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1bb8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1bc8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1bd8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1be8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1bf8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c08: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c18: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c28: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c33: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c39: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < TextureAssets.Item.Length; i++)
			{
				int num = ItemID.Sets.TextureCopyLoad[i];
				if (num != -1)
				{
					TextureAssets.Item[i] = TextureAssets.Item[num];
				}
				else
				{
					TextureAssets.Item[i] = LoadAsset<Texture2D>("Images/Item_" + i, Main.content, (AssetRequestMode)0);
				}
			}
			for (int j = 0; j < TextureAssets.Npc.Length; j++)
			{
				TextureAssets.Npc[j] = LoadAsset<Texture2D>("Images/NPC_" + j, Main.content, (AssetRequestMode)0);
			}
			for (int k = 0; k < TextureAssets.Projectile.Length; k++)
			{
				TextureAssets.Projectile[k] = LoadAsset<Texture2D>("Images/Projectile_" + k, Main.content, (AssetRequestMode)0);
			}
			for (int l = 0; l < TextureAssets.Gore.Length; l++)
			{
				TextureAssets.Gore[l] = LoadAsset<Texture2D>("Images/Gore_" + l, Main.content, (AssetRequestMode)0);
			}
			for (int m = 0; m < TextureAssets.Wall.Length; m++)
			{
				TextureAssets.Wall[m] = LoadAsset<Texture2D>("Images/Wall_" + m, Main.content, (AssetRequestMode)0);
			}
			for (int n = 0; n < TextureAssets.Tile.Length; n++)
			{
				TextureAssets.Tile[n] = LoadAsset<Texture2D>("Images/Tiles_" + n, Main.content, (AssetRequestMode)0);
			}
			for (int num2 = 0; num2 < TextureAssets.ItemFlame.Length; num2++)
			{
				TextureAssets.ItemFlame[num2] = LoadAsset<Texture2D>("Images/ItemFlame_" + num2, Main.content, (AssetRequestMode)0);
			}
			for (int num3 = 0; num3 < TextureAssets.Wings.Length; num3++)
			{
				TextureAssets.Wings[num3] = LoadAsset<Texture2D>("Images/Wings_" + num3, Main.content, (AssetRequestMode)0);
			}
			for (int num4 = 0; num4 < TextureAssets.PlayerHair.Length; num4++)
			{
				TextureAssets.PlayerHair[num4] = LoadAsset<Texture2D>("Images/Player_Hair_" + (num4 + 1), Main.content, (AssetRequestMode)0);
			}
			for (int num5 = 0; num5 < TextureAssets.PlayerHairAlt.Length; num5++)
			{
				TextureAssets.PlayerHairAlt[num5] = LoadAsset<Texture2D>("Images/Player_HairAlt_" + (num5 + 1), Main.content, (AssetRequestMode)0);
			}
			for (int num6 = 0; num6 < TextureAssets.ArmorHead.Length; num6++)
			{
				TextureAssets.ArmorHead[num6] = LoadAsset<Texture2D>("Images/Armor_Head_" + num6, Main.content, (AssetRequestMode)0);
			}
			for (int num7 = 0; num7 < TextureAssets.FemaleBody.Length; num7++)
			{
				TextureAssets.FemaleBody[num7] = LoadAsset<Texture2D>("Images/Female_Body_" + num7, Main.content, (AssetRequestMode)0);
			}
			for (int num8 = 0; num8 < TextureAssets.ArmorBody.Length; num8++)
			{
				TextureAssets.ArmorBody[num8] = LoadAsset<Texture2D>("Images/Armor_Body_" + num8, Main.content, (AssetRequestMode)0);
			}
			for (int num9 = 0; num9 < TextureAssets.ArmorBodyComposite.Length; num9++)
			{
				TextureAssets.ArmorBodyComposite[num9] = LoadAsset<Texture2D>("Images/Armor/Armor_" + num9, Main.content, (AssetRequestMode)0);
			}
			for (int num10 = 0; num10 < TextureAssets.ArmorArm.Length; num10++)
			{
				TextureAssets.ArmorArm[num10] = LoadAsset<Texture2D>("Images/Armor_Arm_" + num10, Main.content, (AssetRequestMode)0);
			}
			for (int num11 = 0; num11 < TextureAssets.ArmorLeg.Length; num11++)
			{
				TextureAssets.ArmorLeg[num11] = LoadAsset<Texture2D>("Images/Armor_Legs_" + num11, Main.content, (AssetRequestMode)0);
			}
			for (int num12 = 0; num12 < TextureAssets.AccHandsOn.Length; num12++)
			{
				TextureAssets.AccHandsOn[num12] = LoadAsset<Texture2D>("Images/Acc_HandsOn_" + num12, Main.content, (AssetRequestMode)0);
			}
			for (int num13 = 0; num13 < TextureAssets.AccHandsOff.Length; num13++)
			{
				TextureAssets.AccHandsOff[num13] = LoadAsset<Texture2D>("Images/Acc_HandsOff_" + num13, Main.content, (AssetRequestMode)0);
			}
			for (int num14 = 0; num14 < TextureAssets.AccHandsOnComposite.Length; num14++)
			{
				TextureAssets.AccHandsOnComposite[num14] = LoadAsset<Texture2D>("Images/Accessories/Acc_HandsOn_" + num14, Main.content, (AssetRequestMode)0);
			}
			for (int num15 = 0; num15 < TextureAssets.AccHandsOffComposite.Length; num15++)
			{
				TextureAssets.AccHandsOffComposite[num15] = LoadAsset<Texture2D>("Images/Accessories/Acc_HandsOff_" + num15, Main.content, (AssetRequestMode)0);
			}
			for (int num16 = 0; num16 < TextureAssets.AccBack.Length; num16++)
			{
				TextureAssets.AccBack[num16] = LoadAsset<Texture2D>("Images/Acc_Back_" + num16, Main.content, (AssetRequestMode)0);
			}
			for (int num17 = 0; num17 < TextureAssets.AccFront.Length; num17++)
			{
				TextureAssets.AccFront[num17] = LoadAsset<Texture2D>("Images/Acc_Front_" + num17, Main.content, (AssetRequestMode)0);
			}
			for (int num18 = 0; num18 < TextureAssets.AccShoes.Length; num18++)
			{
				TextureAssets.AccShoes[num18] = LoadAsset<Texture2D>("Images/Acc_Shoes_" + num18, Main.content, (AssetRequestMode)0);
			}
			for (int num19 = 0; num19 < TextureAssets.AccWaist.Length; num19++)
			{
				TextureAssets.AccWaist[num19] = LoadAsset<Texture2D>("Images/Acc_Waist_" + num19, Main.content, (AssetRequestMode)0);
			}
			for (int num20 = 0; num20 < TextureAssets.AccShield.Length; num20++)
			{
				TextureAssets.AccShield[num20] = LoadAsset<Texture2D>("Images/Acc_Shield_" + num20, Main.content, (AssetRequestMode)0);
			}
			for (int num21 = 0; num21 < TextureAssets.AccNeck.Length; num21++)
			{
				TextureAssets.AccNeck[num21] = LoadAsset<Texture2D>("Images/Acc_Neck_" + num21, Main.content, (AssetRequestMode)0);
			}
			for (int num22 = 0; num22 < TextureAssets.AccFace.Length; num22++)
			{
				TextureAssets.AccFace[num22] = LoadAsset<Texture2D>("Images/Acc_Face_" + num22, Main.content, (AssetRequestMode)0);
			}
			for (int num23 = 0; num23 < TextureAssets.AccBalloon.Length; num23++)
			{
				TextureAssets.AccBalloon[num23] = LoadAsset<Texture2D>("Images/Acc_Balloon_" + num23, Main.content, (AssetRequestMode)0);
			}
			for (int num24 = 0; num24 < TextureAssets.Background.Length; num24++)
			{
				TextureAssets.Background[num24] = LoadAsset<Texture2D>("Images/Background_" + num24, Main.content, (AssetRequestMode)0);
			}
			TextureAssets.FlameRing = LoadAsset<Texture2D>("Images/FlameRing", Main.content, (AssetRequestMode)0);
			TextureAssets.TileCrack = LoadAsset<Texture2D>("Images\\TileCracks", Main.content, mode);
			TextureAssets.ChestStack[0] = LoadAsset<Texture2D>("Images\\ChestStack_0", Main.content, mode);
			TextureAssets.ChestStack[1] = LoadAsset<Texture2D>("Images\\ChestStack_1", Main.content, mode);
			TextureAssets.SmartDig = LoadAsset<Texture2D>("Images\\SmartDig", Main.content, mode);
			TextureAssets.IceBarrier = LoadAsset<Texture2D>("Images\\IceBarrier", Main.content, mode);
			TextureAssets.Frozen = LoadAsset<Texture2D>("Images\\Frozen", Main.content, mode);
			for (int num25 = 0; num25 < TextureAssets.Pvp.Length; num25++)
			{
				TextureAssets.Pvp[num25] = LoadAsset<Texture2D>("Images\\UI\\PVP_" + num25, Main.content, mode);
			}
			for (int num26 = 0; num26 < TextureAssets.EquipPage.Length; num26++)
			{
				TextureAssets.EquipPage[num26] = LoadAsset<Texture2D>("Images\\UI\\DisplaySlots_" + num26, Main.content, mode);
			}
			TextureAssets.HouseBanner = LoadAsset<Texture2D>("Images\\UI\\House_Banner", Main.content, mode);
			for (int num27 = 0; num27 < TextureAssets.CraftToggle.Length; num27++)
			{
				TextureAssets.CraftToggle[num27] = LoadAsset<Texture2D>("Images\\UI\\Craft_Toggle_" + num27, Main.content, mode);
			}
			for (int num28 = 0; num28 < TextureAssets.InventorySort.Length; num28++)
			{
				TextureAssets.InventorySort[num28] = LoadAsset<Texture2D>("Images\\UI\\Sort_" + num28, Main.content, mode);
			}
			for (int num29 = 0; num29 < TextureAssets.TextGlyph.Length; num29++)
			{
				TextureAssets.TextGlyph[num29] = LoadAsset<Texture2D>("Images\\UI\\Glyphs_" + num29, Main.content, mode);
			}
			for (int num30 = 0; num30 < TextureAssets.HotbarRadial.Length; num30++)
			{
				TextureAssets.HotbarRadial[num30] = LoadAsset<Texture2D>("Images\\UI\\HotbarRadial_" + num30, Main.content, mode);
			}
			for (int num31 = 0; num31 < TextureAssets.InfoIcon.Length; num31++)
			{
				TextureAssets.InfoIcon[num31] = LoadAsset<Texture2D>("Images\\UI\\InfoIcon_" + num31, Main.content, mode);
			}
			for (int num32 = 0; num32 < TextureAssets.Reforge.Length; num32++)
			{
				TextureAssets.Reforge[num32] = LoadAsset<Texture2D>("Images\\UI\\Reforge_" + num32, Main.content, mode);
			}
			for (int num33 = 0; num33 < TextureAssets.Camera.Length; num33++)
			{
				TextureAssets.Camera[num33] = LoadAsset<Texture2D>("Images\\UI\\Camera_" + num33, Main.content, mode);
			}
			for (int num34 = 0; num34 < TextureAssets.WireUi.Length; num34++)
			{
				TextureAssets.WireUi[num34] = LoadAsset<Texture2D>("Images\\UI\\Wires_" + num34, Main.content, mode);
			}
			TextureAssets.BuilderAcc = LoadAsset<Texture2D>("Images\\UI\\BuilderIcons", Main.content, mode);
			TextureAssets.QuicksIcon = LoadAsset<Texture2D>("Images\\UI\\UI_quickicon1", Main.content, mode);
			TextureAssets.CraftUpButton = LoadAsset<Texture2D>("Images\\RecUp", Main.content, mode);
			TextureAssets.CraftDownButton = LoadAsset<Texture2D>("Images\\RecDown", Main.content, mode);
			TextureAssets.ScrollLeftButton = LoadAsset<Texture2D>("Images\\RecLeft", Main.content, mode);
			TextureAssets.ScrollRightButton = LoadAsset<Texture2D>("Images\\RecRight", Main.content, mode);
			TextureAssets.OneDropLogo = LoadAsset<Texture2D>("Images\\OneDropLogo", Main.content, mode);
			TextureAssets.Pulley = LoadAsset<Texture2D>("Images\\PlayerPulley", Main.content, mode);
			TextureAssets.Timer = LoadAsset<Texture2D>("Images\\Timer", Main.content, mode);
			TextureAssets.EmoteMenuButton = LoadAsset<Texture2D>("Images\\UI\\Emotes", Main.content, mode);
			TextureAssets.BestiaryMenuButton = LoadAsset<Texture2D>("Images\\UI\\Bestiary", Main.content, mode);
			TextureAssets.Wof = LoadAsset<Texture2D>("Images\\WallOfFlesh", Main.content, mode);
			TextureAssets.WallOutline = LoadAsset<Texture2D>("Images\\Wall_Outline", Main.content, mode);
			TextureAssets.Fade = LoadAsset<Texture2D>("Images\\fade-out", Main.content, mode);
			TextureAssets.Ghost = LoadAsset<Texture2D>("Images\\Ghost", Main.content, mode);
			TextureAssets.EvilCactus = LoadAsset<Texture2D>("Images\\Evil_Cactus", Main.content, mode);
			TextureAssets.GoodCactus = LoadAsset<Texture2D>("Images\\Good_Cactus", Main.content, mode);
			TextureAssets.CrimsonCactus = LoadAsset<Texture2D>("Images\\Crimson_Cactus", Main.content, mode);
			TextureAssets.WraithEye = LoadAsset<Texture2D>("Images\\Wraith_Eyes", Main.content, mode);
			TextureAssets.Firefly = LoadAsset<Texture2D>("Images\\Firefly", Main.content, mode);
			TextureAssets.FireflyJar = LoadAsset<Texture2D>("Images\\FireflyJar", Main.content, mode);
			TextureAssets.Lightningbug = LoadAsset<Texture2D>("Images\\LightningBug", Main.content, mode);
			TextureAssets.LightningbugJar = LoadAsset<Texture2D>("Images\\LightningBugJar", Main.content, mode);
			for (int num35 = 1; num35 <= 3; num35++)
			{
				TextureAssets.JellyfishBowl[num35 - 1] = LoadAsset<Texture2D>("Images\\jellyfishBowl" + num35, Main.content, mode);
			}
			TextureAssets.GlowSnail = LoadAsset<Texture2D>("Images\\GlowSnail", Main.content, mode);
			TextureAssets.IceQueen = LoadAsset<Texture2D>("Images\\IceQueen", Main.content, mode);
			TextureAssets.SantaTank = LoadAsset<Texture2D>("Images\\SantaTank", Main.content, mode);
			TextureAssets.JackHat = LoadAsset<Texture2D>("Images\\JackHat", Main.content, mode);
			TextureAssets.TreeFace = LoadAsset<Texture2D>("Images\\TreeFace", Main.content, mode);
			TextureAssets.PumpkingFace = LoadAsset<Texture2D>("Images\\PumpkingFace", Main.content, mode);
			TextureAssets.ReaperEye = LoadAsset<Texture2D>("Images\\Reaper_Eyes", Main.content, mode);
			TextureAssets.MapDeath = LoadAsset<Texture2D>("Images\\MapDeath", Main.content, mode);
			TextureAssets.DukeFishron = LoadAsset<Texture2D>("Images\\DukeFishron", Main.content, mode);
			TextureAssets.MiniMinotaur = LoadAsset<Texture2D>("Images\\MiniMinotaur", Main.content, mode);
			TextureAssets.Map = LoadAsset<Texture2D>("Images\\Map", Main.content, mode);
			for (int num36 = 0; num36 < TextureAssets.MapBGs.Length; num36++)
			{
				TextureAssets.MapBGs[num36] = LoadAsset<Texture2D>("Images\\MapBG" + (num36 + 1), Main.content, mode);
			}
			TextureAssets.Hue = LoadAsset<Texture2D>("Images\\Hue", Main.content, mode);
			TextureAssets.ColorSlider = LoadAsset<Texture2D>("Images\\ColorSlider", Main.content, mode);
			TextureAssets.ColorBar = LoadAsset<Texture2D>("Images\\ColorBar", Main.content, mode);
			TextureAssets.ColorBlip = LoadAsset<Texture2D>("Images\\ColorBlip", Main.content, mode);
			TextureAssets.ColorHighlight = LoadAsset<Texture2D>("Images\\UI\\Slider_Highlight", Main.content, mode);
			TextureAssets.LockOnCursor = LoadAsset<Texture2D>("Images\\UI\\LockOn_Cursor", Main.content, mode);
			TextureAssets.Rain = LoadAsset<Texture2D>("Images\\Rain", Main.content, mode);
			for (int num37 = 0; num37 < 301; num37++)
			{
				TextureAssets.GlowMask[num37] = LoadAsset<Texture2D>("Images\\Glow_" + num37, Main.content, mode);
			}
			for (int num38 = 0; num38 < TextureAssets.HighlightMask.Length; num38++)
			{
				if (TileID.Sets.HasOutlines[num38])
				{
					TextureAssets.HighlightMask[num38] = LoadAsset<Texture2D>("Images\\Misc\\TileOutlines\\Tiles_" + num38, Main.content, mode);
				}
			}
			for (int num39 = 0; num39 < 212; num39++)
			{
				TextureAssets.Extra[num39] = LoadAsset<Texture2D>("Images\\Extra_" + num39, Main.content, mode);
			}
			for (int num40 = 0; num40 < 4; num40++)
			{
				TextureAssets.Coin[num40] = LoadAsset<Texture2D>("Images\\Coin_" + num40, Main.content, mode);
			}
			TextureAssets.MagicPixel = LoadAsset<Texture2D>("Images\\MagicPixel", Main.content, mode);
			TextureAssets.SettingsPanel = LoadAsset<Texture2D>("Images\\UI\\Settings_Panel", Main.content, mode);
			TextureAssets.SettingsPanel2 = LoadAsset<Texture2D>("Images\\UI\\Settings_Panel_2", Main.content, mode);
			for (int num41 = 0; num41 < TextureAssets.XmasTree.Length; num41++)
			{
				TextureAssets.XmasTree[num41] = LoadAsset<Texture2D>("Images\\Xmas_" + num41, Main.content, mode);
			}
			for (int num42 = 0; num42 < 6; num42++)
			{
				TextureAssets.Clothes[num42] = LoadAsset<Texture2D>("Images\\Clothes_" + num42, Main.content, mode);
			}
			for (int num43 = 0; num43 < TextureAssets.Flames.Length; num43++)
			{
				TextureAssets.Flames[num43] = LoadAsset<Texture2D>("Images\\Flame_" + num43, Main.content, mode);
			}
			for (int num44 = 0; num44 < 8; num44++)
			{
				TextureAssets.MapIcon[num44] = LoadAsset<Texture2D>("Images\\Map_" + num44, Main.content, mode);
			}
			for (int num45 = 0; num45 < TextureAssets.Underworld.Length; num45++)
			{
				TextureAssets.Underworld[num45] = LoadAsset<Texture2D>("Images/Backgrounds/Underworld " + num45, Main.content, (AssetRequestMode)0);
			}
			TextureAssets.Dest[0] = LoadAsset<Texture2D>("Images\\Dest1", Main.content, mode);
			TextureAssets.Dest[1] = LoadAsset<Texture2D>("Images\\Dest2", Main.content, mode);
			TextureAssets.Dest[2] = LoadAsset<Texture2D>("Images\\Dest3", Main.content, mode);
			TextureAssets.Actuator = LoadAsset<Texture2D>("Images\\Actuator", Main.content, mode);
			TextureAssets.Wire = LoadAsset<Texture2D>("Images\\Wires", Main.content, mode);
			TextureAssets.Wire2 = LoadAsset<Texture2D>("Images\\Wires2", Main.content, mode);
			TextureAssets.Wire3 = LoadAsset<Texture2D>("Images\\Wires3", Main.content, mode);
			TextureAssets.Wire4 = LoadAsset<Texture2D>("Images\\Wires4", Main.content, mode);
			TextureAssets.WireNew = LoadAsset<Texture2D>("Images\\WiresNew", Main.content, mode);
			TextureAssets.FlyingCarpet = LoadAsset<Texture2D>("Images\\FlyingCarpet", Main.content, mode);
			TextureAssets.Hb1 = LoadAsset<Texture2D>("Images\\HealthBar1", Main.content, mode);
			TextureAssets.Hb2 = LoadAsset<Texture2D>("Images\\HealthBar2", Main.content, mode);
			for (int num46 = 0; num46 < TextureAssets.NpcHead.Length; num46++)
			{
				TextureAssets.NpcHead[num46] = LoadAsset<Texture2D>("Images\\NPC_Head_" + num46, Main.content, mode);
			}
			for (int num47 = 0; num47 < TextureAssets.NpcHeadBoss.Length; num47++)
			{
				TextureAssets.NpcHeadBoss[num47] = LoadAsset<Texture2D>("Images\\NPC_Head_Boss_" + num47, Main.content, mode);
			}
			for (int num48 = 1; num48 < TextureAssets.BackPack.Length; num48++)
			{
				TextureAssets.BackPack[num48] = LoadAsset<Texture2D>("Images\\BackPack_" + num48, Main.content, mode);
			}
			for (int num49 = 1; num49 < 323; num49++)
			{
				TextureAssets.Buff[num49] = LoadAsset<Texture2D>("Images\\Buff_" + num49, Main.content, mode);
			}
			Main.instance.LoadBackground(0);
			Main.instance.LoadBackground(49);
			TextureAssets.MinecartMount = LoadAsset<Texture2D>("Images\\Mount_Minecart", Main.content, mode);
			for (int num50 = 0; num50 < TextureAssets.RudolphMount.Length; num50++)
			{
				TextureAssets.RudolphMount[num50] = LoadAsset<Texture2D>("Images\\Rudolph_" + num50, Main.content, mode);
			}
			TextureAssets.BunnyMount = LoadAsset<Texture2D>("Images\\Mount_Bunny", Main.content, mode);
			TextureAssets.PigronMount = LoadAsset<Texture2D>("Images\\Mount_Pigron", Main.content, mode);
			TextureAssets.SlimeMount = LoadAsset<Texture2D>("Images\\Mount_Slime", Main.content, mode);
			TextureAssets.TurtleMount = LoadAsset<Texture2D>("Images\\Mount_Turtle", Main.content, mode);
			TextureAssets.UnicornMount = LoadAsset<Texture2D>("Images\\Mount_Unicorn", Main.content, mode);
			TextureAssets.BasiliskMount = LoadAsset<Texture2D>("Images\\Mount_Basilisk", Main.content, mode);
			TextureAssets.MinecartMechMount[0] = LoadAsset<Texture2D>("Images\\Mount_MinecartMech", Main.content, mode);
			TextureAssets.MinecartMechMount[1] = LoadAsset<Texture2D>("Images\\Mount_MinecartMechGlow", Main.content, mode);
			TextureAssets.CuteFishronMount[0] = LoadAsset<Texture2D>("Images\\Mount_CuteFishron1", Main.content, mode);
			TextureAssets.CuteFishronMount[1] = LoadAsset<Texture2D>("Images\\Mount_CuteFishron2", Main.content, mode);
			TextureAssets.MinecartWoodMount = LoadAsset<Texture2D>("Images\\Mount_MinecartWood", Main.content, mode);
			TextureAssets.DesertMinecartMount = LoadAsset<Texture2D>("Images\\Mount_MinecartDesert", Main.content, mode);
			TextureAssets.FishMinecartMount = LoadAsset<Texture2D>("Images\\Mount_MinecartMineCarp", Main.content, mode);
			TextureAssets.BeeMount[0] = LoadAsset<Texture2D>("Images\\Mount_Bee", Main.content, mode);
			TextureAssets.BeeMount[1] = LoadAsset<Texture2D>("Images\\Mount_BeeWings", Main.content, mode);
			TextureAssets.UfoMount[0] = LoadAsset<Texture2D>("Images\\Mount_UFO", Main.content, mode);
			TextureAssets.UfoMount[1] = LoadAsset<Texture2D>("Images\\Mount_UFOGlow", Main.content, mode);
			TextureAssets.DrillMount[0] = LoadAsset<Texture2D>("Images\\Mount_DrillRing", Main.content, mode);
			TextureAssets.DrillMount[1] = LoadAsset<Texture2D>("Images\\Mount_DrillSeat", Main.content, mode);
			TextureAssets.DrillMount[2] = LoadAsset<Texture2D>("Images\\Mount_DrillDiode", Main.content, mode);
			TextureAssets.DrillMount[3] = LoadAsset<Texture2D>("Images\\Mount_Glow_DrillRing", Main.content, mode);
			TextureAssets.DrillMount[4] = LoadAsset<Texture2D>("Images\\Mount_Glow_DrillSeat", Main.content, mode);
			TextureAssets.DrillMount[5] = LoadAsset<Texture2D>("Images\\Mount_Glow_DrillDiode", Main.content, mode);
			TextureAssets.ScutlixMount[0] = LoadAsset<Texture2D>("Images\\Mount_Scutlix", Main.content, mode);
			TextureAssets.ScutlixMount[1] = LoadAsset<Texture2D>("Images\\Mount_ScutlixEyes", Main.content, mode);
			TextureAssets.ScutlixMount[2] = LoadAsset<Texture2D>("Images\\Mount_ScutlixEyeGlow", Main.content, mode);
			for (int num51 = 0; num51 < TextureAssets.Gem.Length; num51++)
			{
				TextureAssets.Gem[num51] = LoadAsset<Texture2D>("Images\\Gem_" + num51, Main.content, mode);
			}
			for (int num52 = 0; num52 < 37; num52++)
			{
				TextureAssets.Cloud[num52] = LoadAsset<Texture2D>("Images\\Cloud_" + num52, Main.content, mode);
			}
			for (int num53 = 0; num53 < 4; num53++)
			{
				TextureAssets.Star[num53] = LoadAsset<Texture2D>("Images\\Star_" + num53, Main.content, mode);
			}
			for (int num54 = 0; num54 < 13; num54++)
			{
				TextureAssets.Liquid[num54] = LoadAsset<Texture2D>("Images\\Liquid_" + num54, Main.content, mode);
				TextureAssets.LiquidSlope[num54] = LoadAsset<Texture2D>("Images\\LiquidSlope_" + num54, Main.content, mode);
			}
			Main.instance.waterfallManager.LoadContent();
			TextureAssets.NpcToggle[0] = LoadAsset<Texture2D>("Images\\House_1", Main.content, mode);
			TextureAssets.NpcToggle[1] = LoadAsset<Texture2D>("Images\\House_2", Main.content, mode);
			TextureAssets.HbLock[0] = LoadAsset<Texture2D>("Images\\Lock_0", Main.content, mode);
			TextureAssets.HbLock[1] = LoadAsset<Texture2D>("Images\\Lock_1", Main.content, mode);
			TextureAssets.blockReplaceIcon[0] = LoadAsset<Texture2D>("Images\\UI\\BlockReplace_0", Main.content, mode);
			TextureAssets.blockReplaceIcon[1] = LoadAsset<Texture2D>("Images\\UI\\BlockReplace_1", Main.content, mode);
			TextureAssets.Grid = LoadAsset<Texture2D>("Images\\Grid", Main.content, mode);
			TextureAssets.Trash = LoadAsset<Texture2D>("Images\\Trash", Main.content, mode);
			TextureAssets.Cd = LoadAsset<Texture2D>("Images\\CoolDown", Main.content, mode);
			TextureAssets.Logo = LoadAsset<Texture2D>("Images\\Logo", Main.content, mode);
			TextureAssets.Logo2 = LoadAsset<Texture2D>("Images\\Logo2", Main.content, mode);
			TextureAssets.Logo3 = LoadAsset<Texture2D>("Images\\Logo3", Main.content, mode);
			TextureAssets.Logo4 = LoadAsset<Texture2D>("Images\\Logo4", Main.content, mode);
			TextureAssets.Dust = LoadAsset<Texture2D>("Images\\Dust", Main.content, mode);
			TextureAssets.Sun = LoadAsset<Texture2D>("Images\\Sun", Main.content, mode);
			TextureAssets.Sun2 = LoadAsset<Texture2D>("Images\\Sun2", Main.content, mode);
			TextureAssets.Sun3 = LoadAsset<Texture2D>("Images\\Sun3", Main.content, mode);
			TextureAssets.BlackTile = LoadAsset<Texture2D>("Images\\Black_Tile", Main.content, mode);
			TextureAssets.Heart = LoadAsset<Texture2D>("Images\\Heart", Main.content, mode);
			TextureAssets.Heart2 = LoadAsset<Texture2D>("Images\\Heart2", Main.content, mode);
			TextureAssets.Bubble = LoadAsset<Texture2D>("Images\\Bubble", Main.content, mode);
			TextureAssets.Flame = LoadAsset<Texture2D>("Images\\Flame", Main.content, mode);
			TextureAssets.Mana = LoadAsset<Texture2D>("Images\\Mana", Main.content, mode);
			for (int num55 = 0; num55 < TextureAssets.Cursors.Length; num55++)
			{
				TextureAssets.Cursors[num55] = LoadAsset<Texture2D>("Images\\UI\\Cursor_" + num55, Main.content, mode);
			}
			TextureAssets.CursorRadial = LoadAsset<Texture2D>("Images\\UI\\Radial", Main.content, mode);
			TextureAssets.Ninja = LoadAsset<Texture2D>("Images\\Ninja", Main.content, mode);
			TextureAssets.AntLion = LoadAsset<Texture2D>("Images\\AntlionBody", Main.content, mode);
			TextureAssets.SpikeBase = LoadAsset<Texture2D>("Images\\Spike_Base", Main.content, mode);
			TextureAssets.Wood[0] = LoadAsset<Texture2D>("Images\\Tiles_5_0", Main.content, mode);
			TextureAssets.Wood[1] = LoadAsset<Texture2D>("Images\\Tiles_5_1", Main.content, mode);
			TextureAssets.Wood[2] = LoadAsset<Texture2D>("Images\\Tiles_5_2", Main.content, mode);
			TextureAssets.Wood[3] = LoadAsset<Texture2D>("Images\\Tiles_5_3", Main.content, mode);
			TextureAssets.Wood[4] = LoadAsset<Texture2D>("Images\\Tiles_5_4", Main.content, mode);
			TextureAssets.Wood[5] = LoadAsset<Texture2D>("Images\\Tiles_5_5", Main.content, mode);
			TextureAssets.Wood[6] = LoadAsset<Texture2D>("Images\\Tiles_5_6", Main.content, mode);
			TextureAssets.SmileyMoon = LoadAsset<Texture2D>("Images\\Moon_Smiley", Main.content, mode);
			TextureAssets.PumpkinMoon = LoadAsset<Texture2D>("Images\\Moon_Pumpkin", Main.content, mode);
			TextureAssets.SnowMoon = LoadAsset<Texture2D>("Images\\Moon_Snow", Main.content, mode);
			for (int num56 = 0; num56 < TextureAssets.Moon.Length; num56++)
			{
				TextureAssets.Moon[num56] = LoadAsset<Texture2D>("Images\\Moon_" + num56, Main.content, mode);
			}
			for (int num57 = 0; num57 < TextureAssets.TreeTop.Length; num57++)
			{
				TextureAssets.TreeTop[num57] = LoadAsset<Texture2D>("Images\\Tree_Tops_" + num57, Main.content, mode);
			}
			for (int num58 = 0; num58 < TextureAssets.TreeBranch.Length; num58++)
			{
				TextureAssets.TreeBranch[num58] = LoadAsset<Texture2D>("Images\\Tree_Branches_" + num58, Main.content, mode);
			}
			TextureAssets.ShroomCap = LoadAsset<Texture2D>("Images\\Shroom_Tops", Main.content, mode);
			TextureAssets.InventoryBack = LoadAsset<Texture2D>("Images\\Inventory_Back", Main.content, mode);
			TextureAssets.InventoryBack2 = LoadAsset<Texture2D>("Images\\Inventory_Back2", Main.content, mode);
			TextureAssets.InventoryBack3 = LoadAsset<Texture2D>("Images\\Inventory_Back3", Main.content, mode);
			TextureAssets.InventoryBack4 = LoadAsset<Texture2D>("Images\\Inventory_Back4", Main.content, mode);
			TextureAssets.InventoryBack5 = LoadAsset<Texture2D>("Images\\Inventory_Back5", Main.content, mode);
			TextureAssets.InventoryBack6 = LoadAsset<Texture2D>("Images\\Inventory_Back6", Main.content, mode);
			TextureAssets.InventoryBack7 = LoadAsset<Texture2D>("Images\\Inventory_Back7", Main.content, mode);
			TextureAssets.InventoryBack8 = LoadAsset<Texture2D>("Images\\Inventory_Back8", Main.content, mode);
			TextureAssets.InventoryBack9 = LoadAsset<Texture2D>("Images\\Inventory_Back9", Main.content, mode);
			TextureAssets.InventoryBack10 = LoadAsset<Texture2D>("Images\\Inventory_Back10", Main.content, mode);
			TextureAssets.InventoryBack11 = LoadAsset<Texture2D>("Images\\Inventory_Back11", Main.content, mode);
			TextureAssets.InventoryBack12 = LoadAsset<Texture2D>("Images\\Inventory_Back12", Main.content, mode);
			TextureAssets.InventoryBack13 = LoadAsset<Texture2D>("Images\\Inventory_Back13", Main.content, mode);
			TextureAssets.InventoryBack14 = LoadAsset<Texture2D>("Images\\Inventory_Back14", Main.content, mode);
			TextureAssets.InventoryBack15 = LoadAsset<Texture2D>("Images\\Inventory_Back15", Main.content, mode);
			TextureAssets.InventoryBack16 = LoadAsset<Texture2D>("Images\\Inventory_Back16", Main.content, mode);
			TextureAssets.InventoryBack17 = LoadAsset<Texture2D>("Images\\Inventory_Back17", Main.content, mode);
			TextureAssets.InventoryBack18 = LoadAsset<Texture2D>("Images\\Inventory_Back18", Main.content, mode);
			TextureAssets.HairStyleBack = LoadAsset<Texture2D>("Images\\HairStyleBack", Main.content, mode);
			TextureAssets.ClothesStyleBack = LoadAsset<Texture2D>("Images\\ClothesStyleBack", Main.content, mode);
			TextureAssets.InventoryTickOff = LoadAsset<Texture2D>("Images\\Inventory_Tick_Off", Main.content, mode);
			TextureAssets.InventoryTickOn = LoadAsset<Texture2D>("Images\\Inventory_Tick_On", Main.content, mode);
			TextureAssets.TextBack = LoadAsset<Texture2D>("Images\\Text_Back", Main.content, mode);
			TextureAssets.Chat = LoadAsset<Texture2D>("Images\\Chat", Main.content, mode);
			TextureAssets.Chat2 = LoadAsset<Texture2D>("Images\\Chat2", Main.content, mode);
			TextureAssets.ChatBack = LoadAsset<Texture2D>("Images\\Chat_Back", Main.content, mode);
			TextureAssets.Team = LoadAsset<Texture2D>("Images\\Team", Main.content, mode);
			PlayerDataInitializer.Load();
			TextureAssets.Chaos = LoadAsset<Texture2D>("Images\\Chaos", Main.content, mode);
			TextureAssets.EyeLaser = LoadAsset<Texture2D>("Images\\Eye_Laser", Main.content, mode);
			TextureAssets.BoneEyes = LoadAsset<Texture2D>("Images\\Bone_Eyes", Main.content, mode);
			TextureAssets.BoneLaser = LoadAsset<Texture2D>("Images\\Bone_Laser", Main.content, mode);
			TextureAssets.LightDisc = LoadAsset<Texture2D>("Images\\Light_Disc", Main.content, mode);
			TextureAssets.Confuse = LoadAsset<Texture2D>("Images\\Confuse", Main.content, mode);
			TextureAssets.Probe = LoadAsset<Texture2D>("Images\\Probe", Main.content, mode);
			TextureAssets.SunOrb = LoadAsset<Texture2D>("Images\\SunOrb", Main.content, mode);
			TextureAssets.SunAltar = LoadAsset<Texture2D>("Images\\SunAltar", Main.content, mode);
			TextureAssets.XmasLight = LoadAsset<Texture2D>("Images\\XmasLight", Main.content, mode);
			TextureAssets.Beetle = LoadAsset<Texture2D>("Images\\BeetleOrb", Main.content, mode);
			for (int num59 = 0; num59 < 17; num59++)
			{
				TextureAssets.Chains[num59] = LoadAsset<Texture2D>("Images\\Chains_" + num59, Main.content, mode);
			}
			TextureAssets.Chain20 = LoadAsset<Texture2D>("Images\\Chain20", Main.content, mode);
			TextureAssets.FishingLine = LoadAsset<Texture2D>("Images\\FishingLine", Main.content, mode);
			TextureAssets.Chain = LoadAsset<Texture2D>("Images\\Chain", Main.content, mode);
			TextureAssets.Chain2 = LoadAsset<Texture2D>("Images\\Chain2", Main.content, mode);
			TextureAssets.Chain3 = LoadAsset<Texture2D>("Images\\Chain3", Main.content, mode);
			TextureAssets.Chain4 = LoadAsset<Texture2D>("Images\\Chain4", Main.content, mode);
			TextureAssets.Chain5 = LoadAsset<Texture2D>("Images\\Chain5", Main.content, mode);
			TextureAssets.Chain6 = LoadAsset<Texture2D>("Images\\Chain6", Main.content, mode);
			TextureAssets.Chain7 = LoadAsset<Texture2D>("Images\\Chain7", Main.content, mode);
			TextureAssets.Chain8 = LoadAsset<Texture2D>("Images\\Chain8", Main.content, mode);
			TextureAssets.Chain9 = LoadAsset<Texture2D>("Images\\Chain9", Main.content, mode);
			TextureAssets.Chain10 = LoadAsset<Texture2D>("Images\\Chain10", Main.content, mode);
			TextureAssets.Chain11 = LoadAsset<Texture2D>("Images\\Chain11", Main.content, mode);
			TextureAssets.Chain12 = LoadAsset<Texture2D>("Images\\Chain12", Main.content, mode);
			TextureAssets.Chain13 = LoadAsset<Texture2D>("Images\\Chain13", Main.content, mode);
			TextureAssets.Chain14 = LoadAsset<Texture2D>("Images\\Chain14", Main.content, mode);
			TextureAssets.Chain15 = LoadAsset<Texture2D>("Images\\Chain15", Main.content, mode);
			TextureAssets.Chain16 = LoadAsset<Texture2D>("Images\\Chain16", Main.content, mode);
			TextureAssets.Chain17 = LoadAsset<Texture2D>("Images\\Chain17", Main.content, mode);
			TextureAssets.Chain18 = LoadAsset<Texture2D>("Images\\Chain18", Main.content, mode);
			TextureAssets.Chain19 = LoadAsset<Texture2D>("Images\\Chain19", Main.content, mode);
			TextureAssets.Chain20 = LoadAsset<Texture2D>("Images\\Chain20", Main.content, mode);
			TextureAssets.Chain21 = LoadAsset<Texture2D>("Images\\Chain21", Main.content, mode);
			TextureAssets.Chain22 = LoadAsset<Texture2D>("Images\\Chain22", Main.content, mode);
			TextureAssets.Chain23 = LoadAsset<Texture2D>("Images\\Chain23", Main.content, mode);
			TextureAssets.Chain24 = LoadAsset<Texture2D>("Images\\Chain24", Main.content, mode);
			TextureAssets.Chain25 = LoadAsset<Texture2D>("Images\\Chain25", Main.content, mode);
			TextureAssets.Chain26 = LoadAsset<Texture2D>("Images\\Chain26", Main.content, mode);
			TextureAssets.Chain27 = LoadAsset<Texture2D>("Images\\Chain27", Main.content, mode);
			TextureAssets.Chain28 = LoadAsset<Texture2D>("Images\\Chain28", Main.content, mode);
			TextureAssets.Chain29 = LoadAsset<Texture2D>("Images\\Chain29", Main.content, mode);
			TextureAssets.Chain30 = LoadAsset<Texture2D>("Images\\Chain30", Main.content, mode);
			TextureAssets.Chain31 = LoadAsset<Texture2D>("Images\\Chain31", Main.content, mode);
			TextureAssets.Chain32 = LoadAsset<Texture2D>("Images\\Chain32", Main.content, mode);
			TextureAssets.Chain33 = LoadAsset<Texture2D>("Images\\Chain33", Main.content, mode);
			TextureAssets.Chain34 = LoadAsset<Texture2D>("Images\\Chain34", Main.content, mode);
			TextureAssets.Chain35 = LoadAsset<Texture2D>("Images\\Chain35", Main.content, mode);
			TextureAssets.Chain36 = LoadAsset<Texture2D>("Images\\Chain36", Main.content, mode);
			TextureAssets.Chain37 = LoadAsset<Texture2D>("Images\\Chain37", Main.content, mode);
			TextureAssets.Chain38 = LoadAsset<Texture2D>("Images\\Chain38", Main.content, mode);
			TextureAssets.Chain39 = LoadAsset<Texture2D>("Images\\Chain39", Main.content, mode);
			TextureAssets.Chain40 = LoadAsset<Texture2D>("Images\\Chain40", Main.content, mode);
			TextureAssets.Chain41 = LoadAsset<Texture2D>("Images\\Chain41", Main.content, mode);
			TextureAssets.Chain42 = LoadAsset<Texture2D>("Images\\Chain42", Main.content, mode);
			TextureAssets.Chain43 = LoadAsset<Texture2D>("Images\\Chain43", Main.content, mode);
			TextureAssets.EyeLaserSmall = LoadAsset<Texture2D>("Images\\Eye_Laser_Small", Main.content, mode);
			TextureAssets.BoneArm = LoadAsset<Texture2D>("Images\\Arm_Bone", Main.content, mode);
			TextureAssets.PumpkingArm = LoadAsset<Texture2D>("Images\\PumpkingArm", Main.content, mode);
			TextureAssets.PumpkingCloak = LoadAsset<Texture2D>("Images\\PumpkingCloak", Main.content, mode);
			TextureAssets.BoneArm2 = LoadAsset<Texture2D>("Images\\Arm_Bone_2", Main.content, mode);
			for (int num60 = 1; num60 < TextureAssets.GemChain.Length; num60++)
			{
				TextureAssets.GemChain[num60] = LoadAsset<Texture2D>("Images\\GemChain_" + num60, Main.content, mode);
			}
			for (int num61 = 1; num61 < TextureAssets.Golem.Length; num61++)
			{
				TextureAssets.Golem[num61] = LoadAsset<Texture2D>("Images\\GolemLights" + num61, Main.content, mode);
			}
			TextureAssets.GolfSwingBarFill = LoadAsset<Texture2D>("Images\\UI\\GolfSwingBarFill", Main.content, mode);
			TextureAssets.GolfSwingBarPanel = LoadAsset<Texture2D>("Images\\UI\\GolfSwingBarPanel", Main.content, mode);
			TextureAssets.SpawnPoint = LoadAsset<Texture2D>("Images\\UI\\SpawnPoint", Main.content, mode);
			TextureAssets.SpawnBed = LoadAsset<Texture2D>("Images\\UI\\SpawnBed", Main.content, mode);
			TextureAssets.MapPing = LoadAsset<Texture2D>("Images\\UI\\MapPing", Main.content, mode);
			TextureAssets.GolfBallArrow = LoadAsset<Texture2D>("Images\\UI\\GolfBall_Arrow", Main.content, mode);
			TextureAssets.GolfBallArrowShadow = LoadAsset<Texture2D>("Images\\UI\\GolfBall_Arrow_Shadow", Main.content, mode);
			TextureAssets.GolfBallOutline = LoadAsset<Texture2D>("Images\\Misc\\GolfBallOutline", Main.content, mode);
			LoadMinimapFrames(mode);
			LoadPlayerResourceSets(mode);
			Main.AchievementAdvisor.LoadContent();
		}

		private static void LoadMinimapFrames(AssetRequestMode mode)
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			float num = 2f;
			float num2 = 6f;
			LoadMinimap("Default", new Vector2(-8f, -15f), new Vector2(148f + num, 234f + num2), new Vector2(200f + num, 234f + num2), new Vector2(174f + num, 234f + num2), mode);
			LoadMinimap("Golden", new Vector2(-10f, -10f), new Vector2(136f, 248f), new Vector2(96f, 248f), new Vector2(116f, 248f), mode);
			LoadMinimap("Remix", new Vector2(-10f, -10f), new Vector2(200f, 234f), new Vector2(148f, 234f), new Vector2(174f, 234f), mode);
			LoadMinimap("Sticks", new Vector2(-10f, -10f), new Vector2(148f, 234f), new Vector2(200f, 234f), new Vector2(174f, 234f), mode);
			LoadMinimap("StoneGold", new Vector2(-15f, -15f), new Vector2(220f, 244f), new Vector2(244f, 188f), new Vector2(244f, 216f), mode);
			LoadMinimap("TwigLeaf", new Vector2(-20f, -20f), new Vector2(206f, 242f), new Vector2(162f, 242f), new Vector2(184f, 242f), mode);
			LoadMinimap("Leaf", new Vector2(-20f, -20f), new Vector2(212f, 244f), new Vector2(168f, 246f), new Vector2(190f, 246f), mode);
			LoadMinimap("Retro", new Vector2(-10f, -10f), new Vector2(150f, 236f), new Vector2(202f, 236f), new Vector2(176f, 236f), mode);
			LoadMinimap("Valkyrie", new Vector2(-10f, -10f), new Vector2(154f, 242f), new Vector2(206f, 240f), new Vector2(180f, 244f), mode);
			string frameName = Main.Configuration.Get("MinimapFrame", "Default");
			Main.ActiveMinimapFrame = Main.MinimapFrames.FirstOrDefault((KeyValuePair<string, MinimapFrame> pair) => pair.Key == frameName).Value;
			if (Main.ActiveMinimapFrame == null)
			{
				Main.ActiveMinimapFrame = Main.MinimapFrames.Values.First();
			}
			Main.Configuration.OnSave += Configuration_OnSave_MinimapFrame;
		}

		private static void Configuration_OnSave_MinimapFrame(Preferences obj)
		{
			string text = Main.MinimapFrames.FirstOrDefault((KeyValuePair<string, MinimapFrame> pair) => pair.Value == Main.ActiveMinimapFrame).Key;
			if (text == null)
			{
				text = "Default";
			}
			obj.Put("MinimapFrame", text);
		}

		private static void LoadMinimap(string name, Vector2 frameOffset, Vector2 resetPosition, Vector2 zoomInPosition, Vector2 zoomOutPosition, AssetRequestMode mode)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			MinimapFrame minimapFrame = new MinimapFrame(LoadAsset<Texture2D>("Images\\UI\\Minimap\\" + name + "\\MinimapFrame", Main.content, mode), frameOffset);
			minimapFrame.SetResetButton(LoadAsset<Texture2D>("Images\\UI\\Minimap\\" + name + "\\MinimapButton_Reset", Main.content, mode), resetPosition);
			minimapFrame.SetZoomOutButton(LoadAsset<Texture2D>("Images\\UI\\Minimap\\" + name + "\\MinimapButton_ZoomOut", Main.content, mode), zoomOutPosition);
			minimapFrame.SetZoomInButton(LoadAsset<Texture2D>("Images\\UI\\Minimap\\" + name + "\\MinimapButton_ZoomIn", Main.content, mode), zoomInPosition);
			Main.MinimapFrames[name] = minimapFrame;
		}

		private static void LoadPlayerResourceSets(AssetRequestMode mode)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			Main.PlayerResourcesSets["Default"] = new ClassicPlayerResourcesDisplaySet();
			Main.PlayerResourcesSets["New"] = new FancyClassicPlayerResourcesDisplaySet("FancyClassic", mode);
			Main.PlayerResourcesSets["HorizontalBars"] = new HorizontalBarsPlayerReosurcesDisplaySet("HorizontalBars", mode);
			string frameName = Main.Configuration.Get("PlayerResourcesSet", "New");
			Main.ActivePlayerResourcesSet = Main.PlayerResourcesSets.FirstOrDefault((KeyValuePair<string, IPlayerResourcesDisplaySet> pair) => pair.Key == frameName).Value;
			if (Main.ActivePlayerResourcesSet == null)
			{
				Main.ActivePlayerResourcesSet = Main.PlayerResourcesSets.Values.First();
			}
			Main.Configuration.OnSave += Configuration_OnSave_PlayerResourcesSet;
		}

		private static void Configuration_OnSave_PlayerResourcesSet(Preferences obj)
		{
			string text = Main.PlayerResourcesSets.FirstOrDefault((KeyValuePair<string, IPlayerResourcesDisplaySet> pair) => pair.Value == Main.ActivePlayerResourcesSet).Key;
			if (text == null)
			{
				text = "New";
			}
			obj.Put("PlayerResourcesSet", text);
		}

		private static Asset<T> LoadAsset<T>(string assetName, ContentManager content, AssetRequestMode mode) where T : class
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return Main.Assets.Request<T>(assetName, Main.content, mode);
		}
	}
}
