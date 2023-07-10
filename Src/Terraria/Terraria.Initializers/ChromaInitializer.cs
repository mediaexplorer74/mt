using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using ReLogic.Peripherals.RGB;
using ReLogic.Peripherals.RGB.Corsair;
using ReLogic.Peripherals.RGB.Logitech;
using ReLogic.Peripherals.RGB.Razer;
using GameManager.GameContent.RGB;
using GameManager.Graphics.Effects;
using GameManager.IO;

namespace GameManager.Initializers
{
	public static class ChromaInitializer
	{
		private static ChromaEngine _engine;

		private static void AddDevices()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected O, but got Unknown
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Expected O, but got Unknown
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Expected O, but got Unknown
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Expected O, but got Unknown
			VendorColorProfile razerColorProfile = Main.Configuration.Get<VendorColorProfile>("RazerColorProfile", new VendorColorProfile(new Vector3(1f, 0.765f, 0.568f)));
			VendorColorProfile corsairColorProfile = Main.Configuration.Get<VendorColorProfile>("CorsairColorProfile", new VendorColorProfile());
			VendorColorProfile logitechColorProfile = Main.Configuration.Get<VendorColorProfile>("LogitechColorProfile", new VendorColorProfile());
			_engine.AddDeviceGroup("Razer", (RgbDeviceGroup)new RazerDeviceGroup(razerColorProfile));
			_engine.AddDeviceGroup("Corsair", (RgbDeviceGroup)new CorsairDeviceGroup(corsairColorProfile));
			_engine.AddDeviceGroup("Logitech", (RgbDeviceGroup)new LogitechDeviceGroup(logitechColorProfile));
			bool useRazer = Main.Configuration.Get("UseRazerRGB", defaultValue: true);
			bool useCorsair = Main.Configuration.Get("UseCorsairRGB", defaultValue: true);
			bool useLogitech = Main.Configuration.Get("UseLogitechRGB", defaultValue: true);
			float rgbUpdateRate = Main.Configuration.Get("RGBUpdatesPerSecond", 45f);
			if (rgbUpdateRate <= 1E-07f)
			{
				rgbUpdateRate = 45f;
			}
			_engine.FrameTimeInSeconds = (1f / rgbUpdateRate);
			Main.Configuration.OnSave += delegate(Preferences config)
			{
				config.Put("UseRazerRGB", useRazer);
				config.Put("UseCorsairRGB", useCorsair);
				config.Put("UseLogitechRGB", useLogitech);
				config.Put("RazerColorProfile", razerColorProfile);
				config.Put("CorsairColorProfile", corsairColorProfile);
				config.Put("LogitechColorProfile", logitechColorProfile);
				config.Put("RGBUpdatesPerSecond", rgbUpdateRate);
			};
			if (useRazer)
			{
				_engine.EnableDeviceGroup("Razer");
			}
			if (useCorsair)
			{
				_engine.EnableDeviceGroup("Corsair");
			}
			if (useLogitech)
			{
				_engine.EnableDeviceGroup("Logitech");
			}
			AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
		}

		private static void OnProcessExit(object sender, EventArgs e)
		{
			_engine.DisableAllDeviceGroups();
		}

		public static void Load()
		{
			_engine = Main.Chroma;
			AddDevices();
			Color color = new Color(46, 23, 12);
			RegisterShader("Base", (ChromaShader)(object)new SurfaceBiomeShader(Color.Green, color), CommonConditions.InMenu, (ShaderLayer)9);
			RegisterShader("Surface Mushroom", (ChromaShader)(object)new SurfaceBiomeShader(Color.DarkBlue, new Color(33, 31, 27)), CommonConditions.DrunkMenu, (ShaderLayer)9);
			RegisterShader("Sky", (ChromaShader)(object)new SkyShader(new Color(34, 51, 128), new Color(5, 5, 5)), CommonConditions.Depth.Sky, (ShaderLayer)1);
			RegisterShader("Surface", (ChromaShader)(object)new SurfaceBiomeShader(Color.Green, color), CommonConditions.Depth.Surface, (ShaderLayer)1);
			RegisterShader("Vines", (ChromaShader)(object)new VineShader(), CommonConditions.Depth.Vines, (ShaderLayer)1);
			RegisterShader("Underground", (ChromaShader)(object)new CavernShader(new Color(122, 62, 32), new Color(25, 13, 7), 0.5f), CommonConditions.Depth.Underground, (ShaderLayer)1);
			RegisterShader("Caverns", (ChromaShader)(object)new CavernShader(color, new Color(25, 25, 25), 0.5f), CommonConditions.Depth.Caverns, (ShaderLayer)1);
			RegisterShader("Magma", (ChromaShader)(object)new CavernShader(new Color(181, 17, 0), new Color(25, 25, 25), 0.5f), CommonConditions.Depth.Magma, (ShaderLayer)1);
			RegisterShader("Underworld", (ChromaShader)(object)new UnderworldShader(Color.Red, new Color(1f, 0.5f, 0f), 1f), CommonConditions.Depth.Underworld, (ShaderLayer)1);
			RegisterShader("Surface Desert", (ChromaShader)(object)new SurfaceBiomeShader(new Color(84, 49, 0), new Color(245, 225, 33)), CommonConditions.SurfaceBiome.Desert, (ShaderLayer)2);
			RegisterShader("Surface Jungle", (ChromaShader)(object)new SurfaceBiomeShader(Color.Green, Color.Teal), CommonConditions.SurfaceBiome.Jungle, (ShaderLayer)2);
			RegisterShader("Surface Ocean", (ChromaShader)(object)new SurfaceBiomeShader(Color.SkyBlue, Color.Blue), CommonConditions.SurfaceBiome.Ocean, (ShaderLayer)2);
			RegisterShader("Surface Snow", (ChromaShader)(object)new SurfaceBiomeShader(new Color(0, 10, 50), new Color(0.5f, 0.75f, 1f)), CommonConditions.SurfaceBiome.Snow, (ShaderLayer)2);
			RegisterShader("Surface Mushroom", (ChromaShader)(object)new SurfaceBiomeShader(Color.DarkBlue, new Color(33, 31, 27)), CommonConditions.SurfaceBiome.Mushroom, (ShaderLayer)2);
			RegisterShader("Surface Hallow", (ChromaShader)(object)new HallowSurfaceShader(), CommonConditions.SurfaceBiome.Hallow, (ShaderLayer)3);
			RegisterShader("Surface Crimson", (ChromaShader)(object)new CorruptSurfaceShader(Color.Red, new Color(25, 25, 40)), CommonConditions.SurfaceBiome.Crimson, (ShaderLayer)3);
			RegisterShader("Surface Corruption", (ChromaShader)(object)new CorruptSurfaceShader(new Color(73, 0, 255), new Color(15, 15, 27)), CommonConditions.SurfaceBiome.Corruption, (ShaderLayer)3);
			RegisterShader("Hive", (ChromaShader)(object)new DrippingShader(new Color(0.05f, 0.01f, 0f), new Color(255, 150, 0), 0.5f), CommonConditions.UndergroundBiome.Hive, (ShaderLayer)3);
			RegisterShader("Underground Mushroom", (ChromaShader)(object)new UndergroundMushroomShader(), CommonConditions.UndergroundBiome.Mushroom, (ShaderLayer)2);
			RegisterShader("Underground Corrutpion", (ChromaShader)(object)new UndergroundCorruptionShader(), CommonConditions.UndergroundBiome.Corrupt, (ShaderLayer)2);
			RegisterShader("Underground Crimson", (ChromaShader)(object)new DrippingShader(new Color(0.05f, 0f, 0f), new Color(255, 0, 0)), CommonConditions.UndergroundBiome.Crimson, (ShaderLayer)2);
			RegisterShader("Underground Hallow", (ChromaShader)(object)new UndergroundHallowShader(), CommonConditions.UndergroundBiome.Hallow, (ShaderLayer)2);
			RegisterShader("Meteorite", (ChromaShader)(object)new MeteoriteShader(), CommonConditions.MiscBiome.Meteorite, (ShaderLayer)3);
			RegisterShader("Temple", (ChromaShader)(object)new TempleShader(), CommonConditions.UndergroundBiome.Temple, (ShaderLayer)3);
			RegisterShader("Dungeon", (ChromaShader)(object)new DungeonShader(), CommonConditions.UndergroundBiome.Dungeon, (ShaderLayer)3);
			RegisterShader("Granite", (ChromaShader)(object)new CavernShader(new Color(14, 19, 46), new Color(5, 0, 30), 0.5f), CommonConditions.UndergroundBiome.Granite, (ShaderLayer)3);
			RegisterShader("Marble", (ChromaShader)(object)new CavernShader(new Color(100, 100, 100), new Color(20, 20, 20), 0.5f), CommonConditions.UndergroundBiome.Marble, (ShaderLayer)3);
			RegisterShader("Gem Cave", (ChromaShader)(object)new GemCaveShader(color, new Color(25, 25, 25)), CommonConditions.UndergroundBiome.GemCave, (ShaderLayer)3);
			RegisterShader("Underground Jungle", (ChromaShader)(object)new JungleShader(), CommonConditions.UndergroundBiome.Jungle, (ShaderLayer)2);
			RegisterShader("Underground Ice", (ChromaShader)(object)new IceShader(new Color(0, 10, 50), new Color(0.5f, 0.75f, 1f)), CommonConditions.UndergroundBiome.Ice, (ShaderLayer)2);
			RegisterShader("Corrupt Ice", (ChromaShader)(object)new IceShader(new Color(5, 0, 25), new Color(152, 102, 255)), CommonConditions.UndergroundBiome.CorruptIce, (ShaderLayer)3);
			RegisterShader("Crimson Ice", (ChromaShader)(object)new IceShader(new Color(0.1f, 0f, 0f), new Color(1f, 0.45f, 0.4f)), CommonConditions.UndergroundBiome.CrimsonIce, (ShaderLayer)3);
			RegisterShader("Hallow Ice", (ChromaShader)(object)new IceShader(new Color(0.2f, 0f, 0.1f), new Color(1f, 0.7f, 0.7f)), CommonConditions.UndergroundBiome.HallowIce, (ShaderLayer)3);
			RegisterShader("Underground Desert", (ChromaShader)(object)new DesertShader(new Color(60, 10, 0), new Color(255, 165, 0)), CommonConditions.UndergroundBiome.Desert, (ShaderLayer)2);
			RegisterShader("Corrupt Desert", (ChromaShader)(object)new DesertShader(new Color(15, 0, 15), new Color(116, 103, 255)), CommonConditions.UndergroundBiome.CorruptDesert, (ShaderLayer)3);
			RegisterShader("Crimson Desert", (ChromaShader)(object)new DesertShader(new Color(20, 10, 0), new Color(195, 0, 0)), CommonConditions.UndergroundBiome.CrimsonDesert, (ShaderLayer)3);
			RegisterShader("Hallow Desert", (ChromaShader)(object)new DesertShader(new Color(29, 0, 56), new Color(255, 221, 255)), CommonConditions.UndergroundBiome.HallowDesert, (ShaderLayer)3);
			RegisterShader("Pumpkin Moon", (ChromaShader)(object)new MoonShader(new Color(13, 0, 26), Color.Orange), CommonConditions.Events.PumpkinMoon, (ShaderLayer)4);
			RegisterShader("Blood Moon", (ChromaShader)(object)new MoonShader(new Color(10, 0, 0), Color.Red, Color.Red, new Color(255, 150, 125)), CommonConditions.Events.BloodMoon, (ShaderLayer)4);
			RegisterShader("Frost Moon", (ChromaShader)(object)new MoonShader(new Color(0, 4, 13), new Color(255, 255, 255)), CommonConditions.Events.FrostMoon, (ShaderLayer)4);
			RegisterShader("Solar Eclipse", (ChromaShader)(object)new MoonShader(new Color(0.02f, 0.02f, 0.02f), Color.Orange, Color.Black), CommonConditions.Events.SolarEclipse, (ShaderLayer)4);
			RegisterShader("Pirate Invasion", (ChromaShader)(object)new PirateInvasionShader(new Color(173, 173, 173), new Color(101, 101, 255), Color.Blue, Color.Black), CommonConditions.Events.PirateInvasion, (ShaderLayer)4);
			RegisterShader("DD2 Event", (ChromaShader)(object)new DD2Shader(new Color(222, 94, 245), Color.White), CommonConditions.Events.DD2Event, (ShaderLayer)4);
			RegisterShader("Goblin Army", (ChromaShader)(object)new GoblinArmyShader(new Color(14, 0, 79), new Color(176, 0, 144)), CommonConditions.Events.GoblinArmy, (ShaderLayer)4);
			RegisterShader("Frost Legion", (ChromaShader)(object)new FrostLegionShader(Color.White, new Color(27, 80, 201)), CommonConditions.Events.FrostLegion, (ShaderLayer)4);
			RegisterShader("Martian Madness", (ChromaShader)(object)new MartianMadnessShader(new Color(64, 64, 64), new Color(64, 113, 122), new Color(255, 255, 0), new Color(3, 3, 18)), CommonConditions.Events.MartianMadness, (ShaderLayer)4);
			RegisterShader("Solar Pillar", (ChromaShader)(object)new PillarShader(Color.Red, Color.Orange), CommonConditions.Events.SolarPillar, (ShaderLayer)4);
			RegisterShader("Nebula Pillar", (ChromaShader)(object)new PillarShader(new Color(255, 144, 209), new Color(100, 0, 76)), CommonConditions.Events.NebulaPillar, (ShaderLayer)4);
			RegisterShader("Vortex Pillar", (ChromaShader)(object)new PillarShader(Color.Green, Color.Black), CommonConditions.Events.VortexPillar, (ShaderLayer)4);
			RegisterShader("Stardust Pillar", (ChromaShader)(object)new PillarShader(new Color(46, 63, 255), Color.White), CommonConditions.Events.StardustPillar, (ShaderLayer)4);
			RegisterShader("Eater of Worlds", (ChromaShader)(object)new WormShader(new Color(14, 0, 15), new Color(47, 51, 59), new Color(20, 25, 11)), CommonConditions.Boss.EaterOfWorlds, (ShaderLayer)5);
			RegisterShader("Eye of Cthulhu", (ChromaShader)(object)new EyeOfCthulhuShader(new Color(145, 145, 126), new Color(138, 0, 0), new Color(3, 3, 18)), CommonConditions.Boss.EyeOfCthulhu, (ShaderLayer)5);
			RegisterShader("Skeletron", (ChromaShader)(object)new SkullShader(new Color(110, 92, 47), new Color(36, 32, 51), new Color(0, 0, 0)), CommonConditions.Boss.Skeletron, (ShaderLayer)5);
			RegisterShader("Brain Of Cthulhu", (ChromaShader)(object)new BrainShader(new Color(54, 0, 0), new Color(186, 137, 139)), CommonConditions.Boss.BrainOfCthulhu, (ShaderLayer)5);
			RegisterShader("Empress of Light", (ChromaShader)(object)new EmpressShader(), CommonConditions.Boss.Empress, (ShaderLayer)5);
			RegisterShader("Queen Slime", (ChromaShader)(object)new QueenSlimeShader(new Color(72, 41, 130), new Color(126, 220, 255)), CommonConditions.Boss.QueenSlime, (ShaderLayer)5);
			RegisterShader("King Slime", (ChromaShader)(object)new KingSlimeShader(new Color(41, 70, 130), Color.White), CommonConditions.Boss.KingSlime, (ShaderLayer)5);
			RegisterShader("Queen Bee", (ChromaShader)(object)new QueenBeeShader(new Color(5, 5, 0), new Color(255, 235, 0)), CommonConditions.Boss.QueenBee, (ShaderLayer)5);
			RegisterShader("Wall of Flesh", (ChromaShader)(object)new WallOfFleshShader(new Color(112, 48, 60), new Color(5, 0, 0)), CommonConditions.Boss.WallOfFlesh, (ShaderLayer)5);
			RegisterShader("Destroyer", (ChromaShader)(object)new WormShader(new Color(25, 25, 25), new Color(192, 0, 0), new Color(10, 0, 0)), CommonConditions.Boss.Destroyer, (ShaderLayer)5);
			RegisterShader("Skeletron Prime", (ChromaShader)(object)new SkullShader(new Color(110, 92, 47), new Color(79, 0, 0), new Color(255, 29, 0)), CommonConditions.Boss.SkeletronPrime, (ShaderLayer)5);
			RegisterShader("The Twins", (ChromaShader)(object)new TwinsShader(new Color(145, 145, 126), new Color(138, 0, 0), new Color(138, 0, 0), new Color(20, 20, 20), new Color(65, 140, 0), new Color(3, 3, 18)), CommonConditions.Boss.TheTwins, (ShaderLayer)5);
			RegisterShader("Duke Fishron", (ChromaShader)(object)new DukeFishronShader(new Color(0, 0, 122), new Color(100, 254, 194)), CommonConditions.Boss.DukeFishron, (ShaderLayer)5);
			RegisterShader("Plantera", (ChromaShader)(object)new PlanteraShader(new Color(255, 0, 220), new Color(0, 255, 0), new Color(12, 4, 0)), CommonConditions.Boss.Plantera, (ShaderLayer)5);
			RegisterShader("Golem", (ChromaShader)(object)new GolemShader(new Color(255, 144, 0), new Color(255, 198, 0), new Color(10, 10, 0)), CommonConditions.Boss.Golem, (ShaderLayer)5);
			RegisterShader("Cultist", (ChromaShader)(object)new CultistShader(), CommonConditions.Boss.Cultist, (ShaderLayer)5);
			RegisterShader("Moon Lord", (ChromaShader)(object)new EyeballShader(isSpawning: false), CommonConditions.Boss.MoonLord, (ShaderLayer)5);
			RegisterShader("Rain", (ChromaShader)(object)new RainShader(), CommonConditions.Weather.Rain, (ShaderLayer)6);
			RegisterShader("Snowstorm", (ChromaShader)(object)new BlizzardShader(), CommonConditions.Weather.Blizzard, (ShaderLayer)6);
			RegisterShader("Sandstorm", (ChromaShader)(object)new SandstormShader(), CommonConditions.Weather.Sandstorm, (ShaderLayer)6);
			RegisterShader("Slime Rain", (ChromaShader)(object)new SlimeRainShader(), CommonConditions.Weather.SlimeRain, (ShaderLayer)6);
			RegisterShader("Drowning", (ChromaShader)(object)new DrowningShader(), CommonConditions.Alert.Drowning, (ShaderLayer)7);
			RegisterShader("Keybinds", (ChromaShader)(object)new KeybindsMenuShader(), CommonConditions.Alert.Keybinds, (ShaderLayer)7);
			RegisterShader("Lava Indicator", (ChromaShader)(object)new LavaIndicatorShader(Color.Black, Color.Red, new Color(255, 188, 0)), CommonConditions.Alert.LavaIndicator, (ShaderLayer)7);
			RegisterShader("Moon Lord Spawn", (ChromaShader)(object)new EyeballShader(isSpawning: true), CommonConditions.Alert.MoonlordComing, (ShaderLayer)7);
			RegisterShader("Low Life", (ChromaShader)(object)new LowLifeShader(), CommonConditions.CriticalAlert.LowLife, (ShaderLayer)8);
			RegisterShader("Death", (ChromaShader)(object)new DeathShader(new Color(36, 0, 10), new Color(158, 28, 53)), CommonConditions.CriticalAlert.Death, (ShaderLayer)8);
		}

		private static void RegisterShader(string name, ChromaShader shader, ChromaCondition condition, ShaderLayer layer)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_engine.RegisterShader(shader, condition, layer);
		}

		[Conditional("DEBUG")]
		private static void AddDebugDraw()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			new BasicDebugDrawer(Main.instance.GraphicsDevice);
			Filters.Scene.OnPostDraw += delegate
			{
			};
		}
	}
}
