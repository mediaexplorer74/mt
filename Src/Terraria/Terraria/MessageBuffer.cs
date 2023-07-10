using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using GameManager.Audio;
using GameManager.Chat;
using GameManager.DataStructures;
using GameManager.Enums;
using GameManager.GameContent;
using GameManager.GameContent.Achievements;
using GameManager.GameContent.Creative;
using GameManager.GameContent.Events;
using GameManager.GameContent.Golf;
using GameManager.GameContent.Tile_Entities;
using GameManager.GameContent.UI;
using GameManager.ID;
using GameManager.Localization;
using GameManager.Net;
using GameManager.Testing;
using GameManager.UI;

namespace GameManager
{
	public class MessageBuffer
	{
		public const int readBufferMax = 131070;

		public const int writeBufferMax = 131070;

		public bool broadcast;

		public byte[] readBuffer = new byte[131070];

		public byte[] writeBuffer = new byte[131070];

		public bool writeLocked;

		public int messageLength;

		public int totalData;

		public int whoAmI;

		public int spamCount;

		public int maxSpam;

		public bool checkBytes;

		public MemoryStream readerStream;

		public MemoryStream writerStream;

		public BinaryReader reader;

		public BinaryWriter writer;

		public PacketHistory History = new PacketHistory();

		public static event TileChangeReceivedEvent OnTileChangeReceived;

		public void Reset()
		{
			Array.Clear(readBuffer, 0, readBuffer.Length);
			Array.Clear(writeBuffer, 0, writeBuffer.Length);
			writeLocked = false;
			messageLength = 0;
			totalData = 0;
			spamCount = 0;
			broadcast = false;
			checkBytes = false;
			ResetReader();
			ResetWriter();
		}

		public void ResetReader()
		{
			if (readerStream != null)
			{
				readerStream.Close();
			}
			readerStream = new MemoryStream(readBuffer);
			reader = new BinaryReader(readerStream);
		}

		public void ResetWriter()
		{
			if (writerStream != null)
			{
				writerStream.Close();
			}
			writerStream = new MemoryStream(writeBuffer);
			writer = new BinaryWriter(writerStream);
		}

		public void GetData(int start, int length, out int messageType)
		{
			if (whoAmI < 256)
			{
				Netplay.Clients[whoAmI].TimeOutTimer = 0;
			}
			else
			{
				Netplay.Connection.TimeOutTimer = 0;
			}
			byte b = 0;
			int num = 0;
			num = start + 1;
			b = (byte)(messageType = readBuffer[start]);
			if (b >= 140)
			{
				return;
			}
			Main.ActiveNetDiagnosticsUI.CountReadMessage(b, length);
			if (Main.netMode == 1 && Netplay.Connection.StatusMax > 0)
			{
				Netplay.Connection.StatusCount++;
			}
			if (Main.verboseNetplay)
			{
				for (int i = start; i < start + length; i++)
				{
				}
				for (int j = start; j < start + length; j++)
				{
					_ = readBuffer[j];
				}
			}
			if (Main.netMode == 2 && b != 38 && Netplay.Clients[whoAmI].State == -1)
			{
				NetMessage.TrySendData(2, whoAmI, -1, Lang.mp[1].ToNetworkText());
				return;
			}
			if (Main.netMode == 2)
			{
				if (Netplay.Clients[whoAmI].State < 10 && b > 12 && b != 93 && b != 16 && b != 42 && b != 50 && b != 38 && b != 68)
				{
					NetMessage.BootPlayer(whoAmI, Lang.mp[2].ToNetworkText());
				}
				if (Netplay.Clients[whoAmI].State == 0 && b != 1)
				{
					NetMessage.BootPlayer(whoAmI, Lang.mp[2].ToNetworkText());
				}
			}
			if (reader == null)
			{
				ResetReader();
			}
			reader.BaseStream.Position = num;
			NPCSpawnParams spawnparams;
			switch (b)
			{
			case 1:
				if (Main.netMode != 2)
				{
					break;
				}
				if (Main.dedServ && Netplay.IsBanned(Netplay.Clients[whoAmI].Socket.GetRemoteAddress()))
				{
					NetMessage.TrySendData(2, whoAmI, -1, Lang.mp[3].ToNetworkText());
				}
				else
				{
					if (Netplay.Clients[whoAmI].State != 0)
					{
						break;
					}
					if (reader.ReadString() == "Terraria" + 230)
					{
						if (string.IsNullOrEmpty(Netplay.ServerPassword))
						{
							Netplay.Clients[whoAmI].State = 1;
							NetMessage.TrySendData(3, whoAmI);
						}
						else
						{
							Netplay.Clients[whoAmI].State = -1;
							NetMessage.TrySendData(37, whoAmI);
						}
					}
					else
					{
						NetMessage.TrySendData(2, whoAmI, -1, Lang.mp[4].ToNetworkText());
					}
				}
				break;
			case 2:
				if (Main.netMode == 1)
				{
					Netplay.Disconnect = true;
					Main.statusText = NetworkText.Deserialize(reader).ToString();
				}
				break;
			case 3:
				if (Main.netMode == 1)
				{
					if (Netplay.Connection.State == 1)
					{
						Netplay.Connection.State = 2;
					}
					int num228 = reader.ReadByte();
					if (num228 != Main.myPlayer)
					{
						Main.player[num228] = Main.ActivePlayerFileData.Player;
						Main.player[Main.myPlayer] = new Player();
					}
					Main.player[num228].whoAmI = num228;
					Main.myPlayer = num228;
					Player player12 = Main.player[num228];
					NetMessage.TrySendData(4, -1, -1, null, num228);
					NetMessage.TrySendData(68, -1, -1, null, num228);
					NetMessage.TrySendData(16, -1, -1, null, num228);
					NetMessage.TrySendData(42, -1, -1, null, num228);
					NetMessage.TrySendData(50, -1, -1, null, num228);
					for (int num229 = 0; num229 < 59; num229++)
					{
						NetMessage.TrySendData(5, -1, -1, null, num228, num229, (int)player12.inventory[num229].prefix);
					}
					for (int num230 = 0; num230 < player12.armor.Length; num230++)
					{
						NetMessage.TrySendData(5, -1, -1, null, num228, 59 + num230, (int)player12.armor[num230].prefix);
					}
					for (int num231 = 0; num231 < player12.dye.Length; num231++)
					{
						NetMessage.TrySendData(5, -1, -1, null, num228, 58 + player12.armor.Length + 1 + num231, (int)player12.dye[num231].prefix);
					}
					for (int num232 = 0; num232 < player12.miscEquips.Length; num232++)
					{
						NetMessage.TrySendData(5, -1, -1, null, num228, 58 + player12.armor.Length + player12.dye.Length + 1 + num232, (int)player12.miscEquips[num232].prefix);
					}
					for (int num233 = 0; num233 < player12.miscDyes.Length; num233++)
					{
						NetMessage.TrySendData(5, -1, -1, null, num228, 58 + player12.armor.Length + player12.dye.Length + player12.miscEquips.Length + 1 + num233, (int)player12.miscDyes[num233].prefix);
					}
					for (int num234 = 0; num234 < player12.bank.item.Length; num234++)
					{
						NetMessage.TrySendData(5, -1, -1, null, num228, 58 + player12.armor.Length + player12.dye.Length + player12.miscEquips.Length + player12.miscDyes.Length + 1 + num234, (int)player12.bank.item[num234].prefix);
					}
					for (int num235 = 0; num235 < player12.bank2.item.Length; num235++)
					{
						NetMessage.TrySendData(5, -1, -1, null, num228, 58 + player12.armor.Length + player12.dye.Length + player12.miscEquips.Length + player12.miscDyes.Length + player12.bank.item.Length + 1 + num235, (int)player12.bank2.item[num235].prefix);
					}
					NetMessage.TrySendData(5, -1, -1, null, num228, 58 + player12.armor.Length + player12.dye.Length + player12.miscEquips.Length + player12.miscDyes.Length + player12.bank.item.Length + player12.bank2.item.Length + 1, (int)player12.trashItem.prefix);
					for (int num236 = 0; num236 < player12.bank3.item.Length; num236++)
					{
						NetMessage.TrySendData(5, -1, -1, null, num228, 58 + player12.armor.Length + player12.dye.Length + player12.miscEquips.Length + player12.miscDyes.Length + player12.bank.item.Length + player12.bank2.item.Length + 2 + num236, (int)player12.bank3.item[num236].prefix);
					}
					for (int num237 = 0; num237 < player12.bank4.item.Length; num237++)
					{
						NetMessage.TrySendData(5, -1, -1, null, num228, 58 + player12.armor.Length + player12.dye.Length + player12.miscEquips.Length + player12.miscDyes.Length + player12.bank.item.Length + player12.bank2.item.Length + player12.bank3.item.Length + 2 + num237, (int)player12.bank4.item[num237].prefix);
					}
					NetMessage.TrySendData(6);
					if (Netplay.Connection.State == 2)
					{
						Netplay.Connection.State = 3;
					}
				}
				break;
			case 4:
			{
				int num212 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num212 = whoAmI;
				}
				if (num212 == Main.myPlayer && !Main.ServerSideCharacter)
				{
					break;
				}
				Player player10 = Main.player[num212];
				player10.whoAmI = num212;
				player10.skinVariant = reader.ReadByte();
				player10.skinVariant = (int)MathHelper.Clamp(player10.skinVariant, 0f, 11f);
				player10.hair = reader.ReadByte();
				if (player10.hair >= 162)
				{
					player10.hair = 0;
				}
				player10.name = reader.ReadString().Trim().Trim();
				player10.hairDye = reader.ReadByte();
				BitsByte bitsByte24 = reader.ReadByte();
				for (int num213 = 0; num213 < 8; num213++)
				{
					player10.hideVisibleAccessory[num213] = bitsByte24[num213];
				}
				bitsByte24 = reader.ReadByte();
				for (int num214 = 0; num214 < 2; num214++)
				{
					player10.hideVisibleAccessory[num214 + 8] = bitsByte24[num214];
				}
				player10.hideMisc = reader.ReadByte();
				player10.hairColor = reader.ReadRGB();
				player10.skinColor = reader.ReadRGB();
				player10.eyeColor = reader.ReadRGB();
				player10.shirtColor = reader.ReadRGB();
				player10.underShirtColor = reader.ReadRGB();
				player10.pantsColor = reader.ReadRGB();
				player10.shoeColor = reader.ReadRGB();
				BitsByte bitsByte25 = reader.ReadByte();
				player10.difficulty = 0;
				if (bitsByte25[0])
				{
					player10.difficulty = 1;
				}
				if (bitsByte25[1])
				{
					player10.difficulty = 2;
				}
				if (bitsByte25[3])
				{
					player10.difficulty = 3;
				}
				if (player10.difficulty > 3)
				{
					player10.difficulty = 3;
				}
				player10.extraAccessory = bitsByte25[2];
				BitsByte bitsByte26 = reader.ReadByte();
				player10.UsingBiomeTorches = bitsByte26[0];
				player10.happyFunTorchTime = bitsByte26[1];
				if (Main.netMode != 2)
				{
					break;
				}
				bool flag13 = false;
				if (Netplay.Clients[whoAmI].State < 10)
				{
					for (int num215 = 0; num215 < 255; num215++)
					{
						if (num215 != num212 && player10.name == Main.player[num215].name && Netplay.Clients[num215].IsActive)
						{
							flag13 = true;
						}
					}
				}
				if (flag13)
				{
					NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey(Lang.mp[5].Key, player10.name));
				}
				else if (player10.name.Length > Player.nameLen)
				{
					NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.NameTooLong"));
				}
				else if (player10.name == "")
				{
					NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.EmptyName"));
				}
				else if (player10.difficulty == 3 && !Main.GameModeInfo.IsJourneyMode)
				{
					NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.PlayerIsCreativeAndWorldIsNotCreative"));
				}
				else if (player10.difficulty != 3 && Main.GameModeInfo.IsJourneyMode)
				{
					NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.PlayerIsNotCreativeAndWorldIsCreative"));
				}
				else
				{
					Netplay.Clients[whoAmI].Name = player10.name;
					Netplay.Clients[whoAmI].Name = player10.name;
					NetMessage.TrySendData(4, -1, whoAmI, null, num212);
				}
				break;
			}
			case 5:
			{
				int num223 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num223 = whoAmI;
				}
				if (num223 == Main.myPlayer && !Main.ServerSideCharacter && !Main.player[num223].HasLockedInventory())
				{
					break;
				}
				Player player11 = Main.player[num223];
				lock (player11)
				{
					int num224 = reader.ReadInt16();
					int stack6 = reader.ReadInt16();
					int num225 = reader.ReadByte();
					int type14 = reader.ReadInt16();
					Item[] array3 = null;
					Item[] array4 = null;
					int num226 = 0;
					bool flag14 = false;
					if (num224 > 58 + player11.armor.Length + player11.dye.Length + player11.miscEquips.Length + player11.miscDyes.Length + player11.bank.item.Length + player11.bank2.item.Length + player11.bank3.item.Length + 1)
					{
						num226 = num224 - 58 - (player11.armor.Length + player11.dye.Length + player11.miscEquips.Length + player11.miscDyes.Length + player11.bank.item.Length + player11.bank2.item.Length + player11.bank3.item.Length + 1) - 1;
						array3 = player11.bank4.item;
						array4 = Main.clientPlayer.bank4.item;
					}
					else if (num224 > 58 + player11.armor.Length + player11.dye.Length + player11.miscEquips.Length + player11.miscDyes.Length + player11.bank.item.Length + player11.bank2.item.Length + 1)
					{
						num226 = num224 - 58 - (player11.armor.Length + player11.dye.Length + player11.miscEquips.Length + player11.miscDyes.Length + player11.bank.item.Length + player11.bank2.item.Length + 1) - 1;
						array3 = player11.bank3.item;
						array4 = Main.clientPlayer.bank3.item;
					}
					else if (num224 > 58 + player11.armor.Length + player11.dye.Length + player11.miscEquips.Length + player11.miscDyes.Length + player11.bank.item.Length + player11.bank2.item.Length)
					{
						flag14 = true;
					}
					else if (num224 > 58 + player11.armor.Length + player11.dye.Length + player11.miscEquips.Length + player11.miscDyes.Length + player11.bank.item.Length)
					{
						num226 = num224 - 58 - (player11.armor.Length + player11.dye.Length + player11.miscEquips.Length + player11.miscDyes.Length + player11.bank.item.Length) - 1;
						array3 = player11.bank2.item;
						array4 = Main.clientPlayer.bank2.item;
					}
					else if (num224 > 58 + player11.armor.Length + player11.dye.Length + player11.miscEquips.Length + player11.miscDyes.Length)
					{
						num226 = num224 - 58 - (player11.armor.Length + player11.dye.Length + player11.miscEquips.Length + player11.miscDyes.Length) - 1;
						array3 = player11.bank.item;
						array4 = Main.clientPlayer.bank.item;
					}
					else if (num224 > 58 + player11.armor.Length + player11.dye.Length + player11.miscEquips.Length)
					{
						num226 = num224 - 58 - (player11.armor.Length + player11.dye.Length + player11.miscEquips.Length) - 1;
						array3 = player11.miscDyes;
						array4 = Main.clientPlayer.miscDyes;
					}
					else if (num224 > 58 + player11.armor.Length + player11.dye.Length)
					{
						num226 = num224 - 58 - (player11.armor.Length + player11.dye.Length) - 1;
						array3 = player11.miscEquips;
						array4 = Main.clientPlayer.miscEquips;
					}
					else if (num224 > 58 + player11.armor.Length)
					{
						num226 = num224 - 58 - player11.armor.Length - 1;
						array3 = player11.dye;
						array4 = Main.clientPlayer.dye;
					}
					else if (num224 > 58)
					{
						num226 = num224 - 58 - 1;
						array3 = player11.armor;
						array4 = Main.clientPlayer.armor;
					}
					else
					{
						num226 = num224;
						array3 = player11.inventory;
						array4 = Main.clientPlayer.inventory;
					}
					if (flag14)
					{
						player11.trashItem = new Item();
						player11.trashItem.netDefaults(type14);
						player11.trashItem.stack = stack6;
						player11.trashItem.Prefix(num225);
						if (num223 == Main.myPlayer && !Main.ServerSideCharacter)
						{
							Main.clientPlayer.trashItem = player11.trashItem.Clone();
						}
					}
					else if (num224 <= 58)
					{
						int type15 = array3[num226].type;
						int stack7 = array3[num226].stack;
						array3[num226] = new Item();
						array3[num226].netDefaults(type14);
						array3[num226].stack = stack6;
						array3[num226].Prefix(num225);
						if (num223 == Main.myPlayer && !Main.ServerSideCharacter)
						{
							array4[num226] = array3[num226].Clone();
						}
						if (num223 == Main.myPlayer && num226 == 58)
						{
							Main.mouseItem = array3[num226].Clone();
						}
						if (num223 == Main.myPlayer && Main.netMode == 1)
						{
							Main.player[num223].inventoryChestStack[num224] = false;
							if (array3[num226].stack != stack7 || array3[num226].type != type15)
							{
								Recipe.FindRecipes(canDelayCheck: true);
								SoundEngine.PlaySound(7);
							}
						}
					}
					else
					{
						array3[num226] = new Item();
						array3[num226].netDefaults(type14);
						array3[num226].stack = stack6;
						array3[num226].Prefix(num225);
						if (num223 == Main.myPlayer && !Main.ServerSideCharacter)
						{
							array4[num226] = array3[num226].Clone();
						}
					}
					if (Main.netMode == 2 && num223 == whoAmI && num224 <= 58 + player11.armor.Length + player11.dye.Length + player11.miscEquips.Length + player11.miscDyes.Length)
					{
						NetMessage.TrySendData(5, -1, whoAmI, null, num223, num224, num225);
					}
				}
				break;
			}
			case 6:
				if (Main.netMode == 2)
				{
					if (Netplay.Clients[whoAmI].State == 1)
					{
						Netplay.Clients[whoAmI].State = 2;
					}
					NetMessage.TrySendData(7, whoAmI);
					Main.SyncAnInvasion(whoAmI);
				}
				break;
			case 7:
				if (Main.netMode == 1)
				{
					Main.time = reader.ReadInt32();
					BitsByte bitsByte14 = reader.ReadByte();
					Main.dayTime = bitsByte14[0];
					Main.bloodMoon = bitsByte14[1];
					Main.eclipse = bitsByte14[2];
					Main.moonPhase = reader.ReadByte();
					Main.maxTilesX = reader.ReadInt16();
					Main.maxTilesY = reader.ReadInt16();
					Main.spawnTileX = reader.ReadInt16();
					Main.spawnTileY = reader.ReadInt16();
					Main.worldSurface = reader.ReadInt16();
					Main.rockLayer = reader.ReadInt16();
					Main.worldID = reader.ReadInt32();
					Main.worldName = reader.ReadString();
					Main.GameMode = reader.ReadByte();
					Main.ActiveWorldFileData.UniqueId = new Guid(reader.ReadBytes(16));
					Main.ActiveWorldFileData.WorldGeneratorVersion = reader.ReadUInt64();
					Main.moonType = reader.ReadByte();
					WorldGen.setBG(0, reader.ReadByte());
					WorldGen.setBG(10, reader.ReadByte());
					WorldGen.setBG(11, reader.ReadByte());
					WorldGen.setBG(12, reader.ReadByte());
					WorldGen.setBG(1, reader.ReadByte());
					WorldGen.setBG(2, reader.ReadByte());
					WorldGen.setBG(3, reader.ReadByte());
					WorldGen.setBG(4, reader.ReadByte());
					WorldGen.setBG(5, reader.ReadByte());
					WorldGen.setBG(6, reader.ReadByte());
					WorldGen.setBG(7, reader.ReadByte());
					WorldGen.setBG(8, reader.ReadByte());
					WorldGen.setBG(9, reader.ReadByte());
					Main.iceBackStyle = reader.ReadByte();
					Main.jungleBackStyle = reader.ReadByte();
					Main.hellBackStyle = reader.ReadByte();
					Main.windSpeedTarget = reader.ReadSingle();
					Main.numClouds = reader.ReadByte();
					for (int num191 = 0; num191 < 3; num191++)
					{
						Main.treeX[num191] = reader.ReadInt32();
					}
					for (int num192 = 0; num192 < 4; num192++)
					{
						Main.treeStyle[num192] = reader.ReadByte();
					}
					for (int num193 = 0; num193 < 3; num193++)
					{
						Main.caveBackX[num193] = reader.ReadInt32();
					}
					for (int num194 = 0; num194 < 4; num194++)
					{
						Main.caveBackStyle[num194] = reader.ReadByte();
					}
					WorldGen.TreeTops.SyncReceive(reader);
					WorldGen.BackgroundsCache.UpdateCache();
					Main.maxRaining = reader.ReadSingle();
					Main.raining = Main.maxRaining > 0f;
					BitsByte bitsByte15 = reader.ReadByte();
					WorldGen.shadowOrbSmashed = bitsByte15[0];
					NPC.downedBoss1 = bitsByte15[1];
					NPC.downedBoss2 = bitsByte15[2];
					NPC.downedBoss3 = bitsByte15[3];
					Main.hardMode = bitsByte15[4];
					NPC.downedClown = bitsByte15[5];
					Main.ServerSideCharacter = bitsByte15[6];
					NPC.downedPlantBoss = bitsByte15[7];
					BitsByte bitsByte16 = reader.ReadByte();
					NPC.downedMechBoss1 = bitsByte16[0];
					NPC.downedMechBoss2 = bitsByte16[1];
					NPC.downedMechBoss3 = bitsByte16[2];
					NPC.downedMechBossAny = bitsByte16[3];
					Main.cloudBGActive = (bitsByte16[4] ? 1 : 0);
					WorldGen.crimson = bitsByte16[5];
					Main.pumpkinMoon = bitsByte16[6];
					Main.snowMoon = bitsByte16[7];
					BitsByte bitsByte17 = reader.ReadByte();
					Main.fastForwardTime = bitsByte17[1];
					Main.UpdateTimeRate();
					bool num195 = bitsByte17[2];
					NPC.downedSlimeKing = bitsByte17[3];
					NPC.downedQueenBee = bitsByte17[4];
					NPC.downedFishron = bitsByte17[5];
					NPC.downedMartians = bitsByte17[6];
					NPC.downedAncientCultist = bitsByte17[7];
					BitsByte bitsByte18 = reader.ReadByte();
					NPC.downedMoonlord = bitsByte18[0];
					NPC.downedHalloweenKing = bitsByte18[1];
					NPC.downedHalloweenTree = bitsByte18[2];
					NPC.downedChristmasIceQueen = bitsByte18[3];
					NPC.downedChristmasSantank = bitsByte18[4];
					NPC.downedChristmasTree = bitsByte18[5];
					NPC.downedGolemBoss = bitsByte18[6];
					BirthdayParty.ManualParty = bitsByte18[7];
					BitsByte bitsByte19 = reader.ReadByte();
					NPC.downedPirates = bitsByte19[0];
					NPC.downedFrost = bitsByte19[1];
					NPC.downedGoblins = bitsByte19[2];
					Sandstorm.Happening = bitsByte19[3];
					DD2Event.Ongoing = bitsByte19[4];
					DD2Event.DownedInvasionT1 = bitsByte19[5];
					DD2Event.DownedInvasionT2 = bitsByte19[6];
					DD2Event.DownedInvasionT3 = bitsByte19[7];
					BitsByte bitsByte20 = reader.ReadByte();
					NPC.combatBookWasUsed = bitsByte20[0];
					LanternNight.ManualLanterns = bitsByte20[1];
					NPC.downedTowerSolar = bitsByte20[2];
					NPC.downedTowerVortex = bitsByte20[3];
					NPC.downedTowerNebula = bitsByte20[4];
					NPC.downedTowerStardust = bitsByte20[5];
					Main.forceHalloweenForToday = bitsByte20[6];
					Main.forceXMasForToday = bitsByte20[7];
					BitsByte bitsByte21 = reader.ReadByte();
					NPC.boughtCat = bitsByte21[0];
					NPC.boughtDog = bitsByte21[1];
					NPC.boughtBunny = bitsByte21[2];
					NPC.freeCake = bitsByte21[3];
					Main.drunkWorld = bitsByte21[4];
					NPC.downedEmpressOfLight = bitsByte21[5];
					NPC.downedQueenSlime = bitsByte21[6];
					Main.getGoodWorld = bitsByte21[7];
					WorldGen.SavedOreTiers.Copper = reader.ReadInt16();
					WorldGen.SavedOreTiers.Iron = reader.ReadInt16();
					WorldGen.SavedOreTiers.Silver = reader.ReadInt16();
					WorldGen.SavedOreTiers.Gold = reader.ReadInt16();
					WorldGen.SavedOreTiers.Cobalt = reader.ReadInt16();
					WorldGen.SavedOreTiers.Mythril = reader.ReadInt16();
					WorldGen.SavedOreTiers.Adamantite = reader.ReadInt16();
					if (num195)
					{
						Main.StartSlimeRain();
					}
					else
					{
						Main.StopSlimeRain();
					}
					Main.invasionType = reader.ReadSByte();
					Main.LobbyId = reader.ReadUInt64();
					Sandstorm.IntendedSeverity = reader.ReadSingle();
					if (Netplay.Connection.State == 3)
					{
						Main.windSpeedCurrent = Main.windSpeedTarget;
						Netplay.Connection.State = 4;
					}
					Main.checkHalloween();
					Main.checkXMas();
				}
				break;
			case 8:
			{
				if (Main.netMode != 2)
				{
					break;
				}
				int num100 = reader.ReadInt32();
				int num101 = reader.ReadInt32();
				bool flag7 = true;
				if (num100 == -1 || num101 == -1)
				{
					flag7 = false;
				}
				else if (num100 < 10 || num100 > Main.maxTilesX - 10)
				{
					flag7 = false;
				}
				else if (num101 < 10 || num101 > Main.maxTilesY - 10)
				{
					flag7 = false;
				}
				int num102 = Netplay.GetSectionX(Main.spawnTileX) - 2;
				int num103 = Netplay.GetSectionY(Main.spawnTileY) - 1;
				int num104 = num102 + 5;
				int num105 = num103 + 3;
				if (num102 < 0)
				{
					num102 = 0;
				}
				if (num104 >= Main.maxSectionsX)
				{
					num104 = Main.maxSectionsX - 1;
				}
				if (num103 < 0)
				{
					num103 = 0;
				}
				if (num105 >= Main.maxSectionsY)
				{
					num105 = Main.maxSectionsY - 1;
				}
				int num106 = (num104 - num102) * (num105 - num103);
				List<Point> list = new List<Point>();
				for (int num107 = num102; num107 < num104; num107++)
				{
					for (int num108 = num103; num108 < num105; num108++)
					{
						list.Add(new Point(num107, num108));
					}
				}
				int num109 = -1;
				int num110 = -1;
				if (flag7)
				{
					num100 = Netplay.GetSectionX(num100) - 2;
					num101 = Netplay.GetSectionY(num101) - 1;
					num109 = num100 + 5;
					num110 = num101 + 3;
					if (num100 < 0)
					{
						num100 = 0;
					}
					if (num109 >= Main.maxSectionsX)
					{
						num109 = Main.maxSectionsX - 1;
					}
					if (num101 < 0)
					{
						num101 = 0;
					}
					if (num110 >= Main.maxSectionsY)
					{
						num110 = Main.maxSectionsY - 1;
					}
					for (int num111 = num100; num111 < num109; num111++)
					{
						for (int num112 = num101; num112 < num110; num112++)
						{
							if (num111 < num102 || num111 >= num104 || num112 < num103 || num112 >= num105)
							{
								list.Add(new Point(num111, num112));
								num106++;
							}
						}
					}
				}
				int num113 = 1;
				PortalHelper.SyncPortalsOnPlayerJoin(whoAmI, 1, list, out var portals, out var portalCenters);
				num106 += portals.Count;
				if (Netplay.Clients[whoAmI].State == 2)
				{
					Netplay.Clients[whoAmI].State = 3;
				}
				NetMessage.TrySendData(9, whoAmI, -1, Lang.inter[44].ToNetworkText(), num106);
				Netplay.Clients[whoAmI].StatusText2 = Language.GetTextValue("Net.IsReceivingTileData");
				Netplay.Clients[whoAmI].StatusMax += num106;
				for (int num114 = num102; num114 < num104; num114++)
				{
					for (int num115 = num103; num115 < num105; num115++)
					{
						NetMessage.SendSection(whoAmI, num114, num115);
					}
				}
				NetMessage.TrySendData(11, whoAmI, -1, null, num102, num103, num104 - 1, num105 - 1);
				if (flag7)
				{
					for (int num116 = num100; num116 < num109; num116++)
					{
						for (int num117 = num101; num117 < num110; num117++)
						{
							NetMessage.SendSection(whoAmI, num116, num117, skipSent: true);
						}
					}
					NetMessage.TrySendData(11, whoAmI, -1, null, num100, num101, num109 - 1, num110 - 1);
				}
				for (int num118 = 0; num118 < portals.Count; num118++)
				{
					NetMessage.SendSection(whoAmI, portals[num118].X, portals[num118].Y, skipSent: true);
				}
				for (int num119 = 0; num119 < portalCenters.Count; num119++)
				{
					NetMessage.TrySendData(11, whoAmI, -1, null, portalCenters[num119].X - num113, portalCenters[num119].Y - num113, portalCenters[num119].X + num113 + 1, portalCenters[num119].Y + num113 + 1);
				}
				for (int num120 = 0; num120 < 400; num120++)
				{
					if (Main.item[num120].active)
					{
						NetMessage.TrySendData(21, whoAmI, -1, null, num120);
						NetMessage.TrySendData(22, whoAmI, -1, null, num120);
					}
				}
				for (int num121 = 0; num121 < 200; num121++)
				{
					if (Main.npc[num121].active)
					{
						NetMessage.TrySendData(23, whoAmI, -1, null, num121);
					}
				}
				for (int num122 = 0; num122 < 1000; num122++)
				{
					if (Main.projectile[num122].active && (Main.projPet[Main.projectile[num122].type] || Main.projectile[num122].netImportant))
					{
						NetMessage.TrySendData(27, whoAmI, -1, null, num122);
					}
				}
				for (int num123 = 0; num123 < 289; num123++)
				{
					NetMessage.TrySendData(83, whoAmI, -1, null, num123);
				}
				NetMessage.TrySendData(49, whoAmI);
				NetMessage.TrySendData(57, whoAmI);
				NetMessage.TrySendData(7, whoAmI);
				NetMessage.TrySendData(103, -1, -1, null, NPC.MoonLordCountdown);
				NetMessage.TrySendData(101, whoAmI);
				NetMessage.TrySendData(136, whoAmI);
				Main.BestiaryTracker.OnPlayerJoining(whoAmI);
				CreativePowerManager.Instance.SyncThingsToJoiningPlayer(whoAmI);
				Main.PylonSystem.OnPlayerJoining(whoAmI);
				break;
			}
			case 9:
				if (Main.netMode == 1)
				{
					Netplay.Connection.StatusMax += reader.ReadInt32();
					Netplay.Connection.StatusText = NetworkText.Deserialize(reader).ToString();
					Netplay.Connection.StatusTextFlags = reader.ReadByte();
				}
				break;
			case 10:
				if (Main.netMode == 1)
				{
					NetMessage.DecompressTileBlock(readBuffer, num, length);
				}
				break;
			case 11:
				if (Main.netMode == 1)
				{
					WorldGen.SectionTileFrame(reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16());
				}
				break;
			case 12:
			{
				int num144 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num144 = whoAmI;
				}
				Player player7 = Main.player[num144];
				player7.SpawnX = reader.ReadInt16();
				player7.SpawnY = reader.ReadInt16();
				player7.respawnTimer = reader.ReadInt32();
				if (player7.respawnTimer > 0)
				{
					player7.dead = true;
				}
				PlayerSpawnContext playerSpawnContext = (PlayerSpawnContext)reader.ReadByte();
				player7.Spawn(playerSpawnContext);
				if (num144 == Main.myPlayer && Main.netMode != 2)
				{
					Main.ActivePlayerFileData.StartPlayTimer();
					Player.Hooks.EnterWorld(Main.myPlayer);
				}
				if (Main.netMode != 2 || Netplay.Clients[whoAmI].State < 3)
				{
					break;
				}
				if (Netplay.Clients[whoAmI].State == 3)
				{
					Netplay.Clients[whoAmI].State = 10;
					NetMessage.buffer[whoAmI].broadcast = true;
					NetMessage.SyncConnectedPlayer(whoAmI);
					bool flag9 = NetMessage.DoesPlayerSlotCountAsAHost(whoAmI);
					Main.countsAsHostForGameplay[whoAmI] = flag9;
					if (NetMessage.DoesPlayerSlotCountAsAHost(whoAmI))
					{
						NetMessage.TrySendData(139, whoAmI, -1, null, whoAmI, flag9.ToInt());
					}
					NetMessage.TrySendData(12, -1, whoAmI, null, whoAmI, (int)(byte)playerSpawnContext);
					NetMessage.TrySendData(74, whoAmI, -1, NetworkText.FromLiteral(Main.player[whoAmI].name), Main.anglerQuest);
					NetMessage.TrySendData(129, whoAmI);
					NetMessage.greetPlayer(whoAmI);
				}
				else
				{
					NetMessage.TrySendData(12, -1, whoAmI, null, whoAmI, (int)(byte)playerSpawnContext);
				}
				break;
			}
			case 13:
			{
				int num163 = reader.ReadByte();
				if (num163 != Main.myPlayer || Main.ServerSideCharacter)
				{
					if (Main.netMode == 2)
					{
						num163 = whoAmI;
					}
					Player player8 = Main.player[num163];
					BitsByte bitsByte9 = reader.ReadByte();
					BitsByte bitsByte10 = reader.ReadByte();
					BitsByte bitsByte11 = reader.ReadByte();
					BitsByte bitsByte12 = reader.ReadByte();
					player8.controlUp = bitsByte9[0];
					player8.controlDown = bitsByte9[1];
					player8.controlLeft = bitsByte9[2];
					player8.controlRight = bitsByte9[3];
					player8.controlJump = bitsByte9[4];
					player8.controlUseItem = bitsByte9[5];
					player8.direction = (bitsByte9[6] ? 1 : (-1));
					if (bitsByte10[0])
					{
						player8.pulley = true;
						player8.pulleyDir = (byte)((!bitsByte10[1]) ? 1u : 2u);
					}
					else
					{
						player8.pulley = false;
					}
					player8.vortexStealthActive = bitsByte10[3];
					player8.gravDir = (bitsByte10[4] ? 1 : (-1));
					player8.TryTogglingShield(bitsByte10[5]);
					player8.ghost = bitsByte10[6];
					player8.selectedItem = reader.ReadByte();
					player8.position = reader.ReadVector2();
					if (bitsByte10[2])
					{
						player8.velocity = reader.ReadVector2();
					}
					else
					{
						player8.velocity = Vector2.Zero;
					}
					if (bitsByte11[6])
					{
						player8.PotionOfReturnOriginalUsePosition = reader.ReadVector2();
						player8.PotionOfReturnHomePosition = reader.ReadVector2();
					}
					else
					{
						player8.PotionOfReturnOriginalUsePosition = null;
						player8.PotionOfReturnHomePosition = null;
					}
					player8.tryKeepingHoveringUp = bitsByte11[0];
					player8.IsVoidVaultEnabled = bitsByte11[1];
					player8.sitting.isSitting = bitsByte11[2];
					player8.downedDD2EventAnyDifficulty = bitsByte11[3];
					player8.isPettingAnimal = bitsByte11[4];
					player8.isTheAnimalBeingPetSmall = bitsByte11[5];
					player8.tryKeepingHoveringDown = bitsByte11[7];
					player8.sleeping.SetIsSleepingAndAdjustPlayerRotation(player8, bitsByte12[0]);
					if (Main.netMode == 2 && Netplay.Clients[whoAmI].State == 10)
					{
						NetMessage.TrySendData(13, -1, whoAmI, null, num163);
					}
				}
				break;
			}
			case 14:
			{
				int num22 = reader.ReadByte();
				int num23 = reader.ReadByte();
				if (Main.netMode != 1)
				{
					break;
				}
				bool active = Main.player[num22].active;
				if (num23 == 1)
				{
					if (!Main.player[num22].active)
					{
						Main.player[num22] = new Player();
					}
					Main.player[num22].active = true;
				}
				else
				{
					Main.player[num22].active = false;
				}
				if (active != Main.player[num22].active)
				{
					if (Main.player[num22].active)
					{
						Player.Hooks.PlayerConnect(num22);
					}
					else
					{
						Player.Hooks.PlayerDisconnect(num22);
					}
				}
				break;
			}
			case 16:
			{
				int num252 = reader.ReadByte();
				if (num252 != Main.myPlayer || Main.ServerSideCharacter)
				{
					if (Main.netMode == 2)
					{
						num252 = whoAmI;
					}
					Player player14 = Main.player[num252];
					player14.statLife = reader.ReadInt16();
					player14.statLifeMax = reader.ReadInt16();
					if (player14.statLifeMax < 100)
					{
						player14.statLifeMax = 100;
					}
					player14.dead = player14.statLife <= 0;
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(16, -1, whoAmI, null, num252);
					}
				}
				break;
			}
			case 17:
			{
				byte b8 = reader.ReadByte();
				int num96 = reader.ReadInt16();
				int num97 = reader.ReadInt16();
				short num98 = reader.ReadInt16();
				int num99 = reader.ReadByte();
				bool flag6 = num98 == 1;
				if (!WorldGen.InWorld(num96, num97, 3))
				{
					break;
				}
				if (Main.tile[num96, num97] == null)
				{
					Main.tile[num96, num97] = new Tile();
				}
				if (Main.netMode == 2)
				{
					if (!flag6)
					{
						if (b8 == 0 || b8 == 2 || b8 == 4)
						{
							Netplay.Clients[whoAmI].SpamDeleteBlock += 1f;
						}
						if (b8 == 1 || b8 == 3)
						{
							Netplay.Clients[whoAmI].SpamAddBlock += 1f;
						}
					}
					if (!Netplay.Clients[whoAmI].TileSections[Netplay.GetSectionX(num96), Netplay.GetSectionY(num97)])
					{
						flag6 = true;
					}
				}
				if (b8 == 0)
				{
					WorldGen.KillTile(num96, num97, flag6);
					if (Main.netMode == 1 && !flag6)
					{
						HitTile.ClearAllTilesAtThisLocation(num96, num97);
					}
				}
				if (b8 == 1)
				{
					WorldGen.PlaceTile(num96, num97, num98, mute: false, forced: true, -1, num99);
				}
				if (b8 == 2)
				{
					WorldGen.KillWall(num96, num97, flag6);
				}
				if (b8 == 3)
				{
					WorldGen.PlaceWall(num96, num97, num98);
				}
				if (b8 == 4)
				{
					WorldGen.KillTile(num96, num97, flag6, effectOnly: false, noItem: true);
				}
				if (b8 == 5)
				{
					WorldGen.PlaceWire(num96, num97);
				}
				if (b8 == 6)
				{
					WorldGen.KillWire(num96, num97);
				}
				if (b8 == 7)
				{
					WorldGen.PoundTile(num96, num97);
				}
				if (b8 == 8)
				{
					WorldGen.PlaceActuator(num96, num97);
				}
				if (b8 == 9)
				{
					WorldGen.KillActuator(num96, num97);
				}
				if (b8 == 10)
				{
					WorldGen.PlaceWire2(num96, num97);
				}
				if (b8 == 11)
				{
					WorldGen.KillWire2(num96, num97);
				}
				if (b8 == 12)
				{
					WorldGen.PlaceWire3(num96, num97);
				}
				if (b8 == 13)
				{
					WorldGen.KillWire3(num96, num97);
				}
				if (b8 == 14)
				{
					WorldGen.SlopeTile(num96, num97, num98);
				}
				if (b8 == 15)
				{
					Minecart.FrameTrack(num96, num97, pound: true);
				}
				if (b8 == 16)
				{
					WorldGen.PlaceWire4(num96, num97);
				}
				if (b8 == 17)
				{
					WorldGen.KillWire4(num96, num97);
				}
				switch (b8)
				{
				case 18:
					Wiring.SetCurrentUser(whoAmI);
					Wiring.PokeLogicGate(num96, num97);
					Wiring.SetCurrentUser();
					return;
				case 19:
					Wiring.SetCurrentUser(whoAmI);
					Wiring.Actuate(num96, num97);
					Wiring.SetCurrentUser();
					return;
				case 20:
					if (WorldGen.InWorld(num96, num97, 2))
					{
						int type8 = Main.tile[num96, num97].type;
						WorldGen.KillTile(num96, num97, flag6);
						num98 = (short)((Main.tile[num96, num97].type == type8) ? 1 : 0);
						if (Main.netMode == 2)
						{
							NetMessage.TrySendData(17, -1, -1, null, b8, num96, num97, num98, num99);
						}
					}
					return;
				case 21:
					WorldGen.ReplaceTile(num96, num97, (ushort)num98, num99);
					break;
				}
				if (b8 == 22)
				{
					WorldGen.ReplaceWall(num96, num97, (ushort)num98);
				}
				if (b8 == 23)
				{
					WorldGen.SlopeTile(num96, num97, num98);
					WorldGen.PoundTile(num96, num97);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(17, -1, whoAmI, null, b8, num96, num97, num98, num99);
					if ((b8 == 1 || b8 == 21) && TileID.Sets.Falling[num98])
					{
						NetMessage.SendTileSquare(-1, num96, num97, 1);
					}
				}
				break;
			}
			case 18:
				if (Main.netMode == 1)
				{
					Main.dayTime = reader.ReadByte() == 1;
					Main.time = reader.ReadInt32();
					Main.sunModY = reader.ReadInt16();
					Main.moonModY = reader.ReadInt16();
				}
				break;
			case 19:
			{
				byte b12 = reader.ReadByte();
				int num205 = reader.ReadInt16();
				int num206 = reader.ReadInt16();
				if (WorldGen.InWorld(num205, num206, 3))
				{
					int num207 = ((reader.ReadByte() != 0) ? 1 : (-1));
					switch (b12)
					{
					case 0:
						WorldGen.OpenDoor(num205, num206, num207);
						break;
					case 1:
						WorldGen.CloseDoor(num205, num206, forced: true);
						break;
					case 2:
						WorldGen.ShiftTrapdoor(num205, num206, num207 == 1, 1);
						break;
					case 3:
						WorldGen.ShiftTrapdoor(num205, num206, num207 == 1, 0);
						break;
					case 4:
						WorldGen.ShiftTallGate(num205, num206, closing: false, forced: true);
						break;
					case 5:
						WorldGen.ShiftTallGate(num205, num206, closing: true, forced: true);
						break;
					}
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(19, -1, whoAmI, null, b12, num205, num206, (num207 == 1) ? 1 : 0);
					}
				}
				break;
			}
			case 20:
			{
				ushort num67 = reader.ReadUInt16();
				short num68 = (short)(num67 & 0x7FFF);
				bool num69 = (num67 & 0x8000) != 0;
				byte b6 = 0;
				if (num69)
				{
					b6 = reader.ReadByte();
				}
				int num70 = reader.ReadInt16();
				int num71 = reader.ReadInt16();
				if (!WorldGen.InWorld(num70, num71, 3))
				{
					break;
				}
				TileChangeType type6 = TileChangeType.None;
				if (Enum.IsDefined(typeof(TileChangeType), b6))
				{
					type6 = (TileChangeType)b6;
				}
				if (MessageBuffer.OnTileChangeReceived != null)
				{
					MessageBuffer.OnTileChangeReceived(num70, num71, num68, type6);
				}
				BitsByte bitsByte6 = (byte)0;
				BitsByte bitsByte7 = (byte)0;
				Tile tile4 = null;
				for (int num72 = num70; num72 < num70 + num68; num72++)
				{
					for (int num73 = num71; num73 < num71 + num68; num73++)
					{
						if (Main.tile[num72, num73] == null)
						{
							Main.tile[num72, num73] = new Tile();
						}
						tile4 = Main.tile[num72, num73];
						bool flag4 = tile4.active();
						bitsByte6 = reader.ReadByte();
						bitsByte7 = reader.ReadByte();
						tile4.active(bitsByte6[0]);
						tile4.wall = (byte)(bitsByte6[2] ? 1u : 0u);
						bool flag5 = bitsByte6[3];
						if (Main.netMode != 2)
						{
							tile4.liquid = (byte)(flag5 ? 1u : 0u);
						}
						tile4.wire(bitsByte6[4]);
						tile4.halfBrick(bitsByte6[5]);
						tile4.actuator(bitsByte6[6]);
						tile4.inActive(bitsByte6[7]);
						tile4.wire2(bitsByte7[0]);
						tile4.wire3(bitsByte7[1]);
						if (bitsByte7[2])
						{
							tile4.color(reader.ReadByte());
						}
						if (bitsByte7[3])
						{
							tile4.wallColor(reader.ReadByte());
						}
						if (tile4.active())
						{
							int type7 = tile4.type;
							tile4.type = reader.ReadUInt16();
							if (Main.tileFrameImportant[tile4.type])
							{
								tile4.frameX = reader.ReadInt16();
								tile4.frameY = reader.ReadInt16();
							}
							else if (!flag4 || tile4.type != type7)
							{
								tile4.frameX = -1;
								tile4.frameY = -1;
							}
							byte b7 = 0;
							if (bitsByte7[4])
							{
								b7 = (byte)(b7 + 1);
							}
							if (bitsByte7[5])
							{
								b7 = (byte)(b7 + 2);
							}
							if (bitsByte7[6])
							{
								b7 = (byte)(b7 + 4);
							}
							tile4.slope(b7);
						}
						tile4.wire4(bitsByte7[7]);
						if (tile4.wall > 0)
						{
							tile4.wall = reader.ReadUInt16();
						}
						if (flag5)
						{
							tile4.liquid = reader.ReadByte();
							tile4.liquidType(reader.ReadByte());
						}
					}
				}
				WorldGen.RangeFrame(num70, num71, num70 + num68, num71 + num68);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(b, -1, whoAmI, null, num68, num70, num71);
				}
				break;
			}
			case 21:
			case 90:
			{
				int num2 = reader.ReadInt16();
				Vector2 position = reader.ReadVector2();
				Vector2 velocity = reader.ReadVector2();
				int stack = reader.ReadInt16();
				int pre = reader.ReadByte();
				int num3 = reader.ReadByte();
				int num4 = reader.ReadInt16();
				if (Main.netMode == 1)
				{
					if (num4 == 0)
					{
						Main.item[num2].active = false;
						break;
					}
					int num5 = num2;
					Item item = Main.item[num5];
					ItemSyncPersistentStats itemSyncPersistentStats = default(ItemSyncPersistentStats);
					itemSyncPersistentStats.CopyFrom(item);
					bool newAndShiny = (item.newAndShiny || item.netID != num4) && ItemSlot.Options.HighlightNewItems && (num4 < 0 || num4 >= 5045 || !ItemID.Sets.NeverAppearsAsNewInInventory[num4]);
					item.netDefaults(num4);
					item.newAndShiny = newAndShiny;
					item.Prefix(pre);
					item.stack = stack;
					item.position = position;
					item.velocity = velocity;
					item.active = true;
					if (b == 90)
					{
						item.instanced = true;
						item.playerIndexTheItemIsReservedFor = Main.myPlayer;
						item.keepTime = 600;
					}
					item.wet = Collision.WetCollision(item.position, item.width, item.height);
					itemSyncPersistentStats.PasteInto(item);
				}
				else
				{
					if (Main.timeItemSlotCannotBeReusedFor[num2] > 0)
					{
						break;
					}
					if (num4 == 0)
					{
						if (num2 < 400)
						{
							Main.item[num2].active = false;
							NetMessage.TrySendData(21, -1, -1, null, num2);
						}
						break;
					}
					bool flag = false;
					if (num2 == 400)
					{
						flag = true;
					}
					if (flag)
					{
						Item item2 = new Item();
						item2.netDefaults(num4);
						num2 = Item.NewItem((int)position.X, (int)position.Y, item2.width, item2.height, item2.type, stack, noBroadcast: true);
					}
					Item obj = Main.item[num2];
					obj.netDefaults(num4);
					obj.Prefix(pre);
					obj.stack = stack;
					obj.position = position;
					obj.velocity = velocity;
					obj.active = true;
					obj.playerIndexTheItemIsReservedFor = Main.myPlayer;
					if (flag)
					{
						NetMessage.TrySendData(21, -1, -1, null, num2);
						if (num3 == 0)
						{
							Main.item[num2].ownIgnore = whoAmI;
							Main.item[num2].ownTime = 100;
						}
						Main.item[num2].FindOwner(num2);
					}
					else
					{
						NetMessage.TrySendData(21, -1, whoAmI, null, num2);
					}
				}
				break;
			}
			case 22:
			{
				int num184 = reader.ReadInt16();
				int num185 = reader.ReadByte();
				if (Main.netMode != 2 || Main.item[num184].playerIndexTheItemIsReservedFor == whoAmI)
				{
					Main.item[num184].playerIndexTheItemIsReservedFor = num185;
					if (num185 == Main.myPlayer)
					{
						Main.item[num184].keepTime = 15;
					}
					else
					{
						Main.item[num184].keepTime = 0;
					}
					if (Main.netMode == 2)
					{
						Main.item[num184].playerIndexTheItemIsReservedFor = 255;
						Main.item[num184].keepTime = 15;
						NetMessage.TrySendData(22, -1, -1, null, num184);
					}
				}
				break;
			}
			case 23:
			{
				if (Main.netMode != 1)
				{
					break;
				}
				int num48 = reader.ReadInt16();
				Vector2 vector4 = reader.ReadVector2();
				Vector2 velocity2 = reader.ReadVector2();
				int num49 = reader.ReadUInt16();
				if (num49 == 65535)
				{
					num49 = 0;
				}
				BitsByte bitsByte4 = reader.ReadByte();
				BitsByte bitsByte5 = reader.ReadByte();
				float[] array = new float[NPC.maxAI];
				for (int num50 = 0; num50 < NPC.maxAI; num50++)
				{
					if (bitsByte4[num50 + 2])
					{
						array[num50] = reader.ReadSingle();
					}
					else
					{
						array[num50] = 0f;
					}
				}
				int num51 = reader.ReadInt16();
				int? playerCountForMultiplayerDifficultyOverride = 1;
				if (bitsByte5[0])
				{
					playerCountForMultiplayerDifficultyOverride = reader.ReadByte();
				}
				float value5 = 1f;
				if (bitsByte5[2])
				{
					value5 = reader.ReadSingle();
				}
				int num52 = 0;
				if (!bitsByte4[7])
				{
					num52 = reader.ReadByte() switch
					{
						2 => reader.ReadInt16(), 
						4 => reader.ReadInt32(), 
						_ => reader.ReadSByte(), 
					};
				}
				int num53 = -1;
				NPC nPC = Main.npc[num48];
				if (nPC.active && Main.multiplayerNPCSmoothingRange > 0 && Vector2.DistanceSquared(nPC.position, vector4) < 640000f)
				{
					nPC.netOffset += nPC.position - vector4;
				}
				if (!nPC.active || nPC.netID != num51)
				{
					nPC.netOffset *= 0f;
					if (nPC.active)
					{
						num53 = nPC.type;
					}
					nPC.active = true;
					spawnparams = new NPCSpawnParams
					{
						playerCountForMultiplayerDifficultyOverride = playerCountForMultiplayerDifficultyOverride,
						strengthMultiplierOverride = value5
					};
					nPC.SetDefaults(num51, spawnparams);
				}
				nPC.position = vector4;
				nPC.velocity = velocity2;
				nPC.target = num49;
				nPC.direction = (bitsByte4[0] ? 1 : (-1));
				nPC.directionY = (bitsByte4[1] ? 1 : (-1));
				nPC.spriteDirection = (bitsByte4[6] ? 1 : (-1));
				if (bitsByte4[7])
				{
					num52 = (nPC.life = nPC.lifeMax);
				}
				else
				{
					nPC.life = num52;
				}
				if (num52 <= 0)
				{
					nPC.active = false;
				}
				nPC.SpawnedFromStatue = bitsByte5[0];
				if (nPC.SpawnedFromStatue)
				{
					nPC.value = 0f;
				}
				for (int num54 = 0; num54 < NPC.maxAI; num54++)
				{
					nPC.ai[num54] = array[num54];
				}
				if (num53 > -1 && num53 != nPC.type)
				{
					nPC.TransformVisuals(num53, nPC.type);
				}
				if (num51 == 262)
				{
					NPC.plantBoss = num48;
				}
				if (num51 == 245)
				{
					NPC.golemBoss = num48;
				}
				if (nPC.type >= 0 && nPC.type < 663 && Main.npcCatchable[nPC.type])
				{
					nPC.releaseOwner = reader.ReadByte();
				}
				break;
			}
			case 24:
			{
				int num256 = reader.ReadInt16();
				int num257 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num257 = whoAmI;
				}
				Player player16 = Main.player[num257];
				Main.npc[num256].StrikeNPC(player16.inventory[player16.selectedItem].damage, player16.inventory[player16.selectedItem].knockBack, player16.direction);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(24, -1, whoAmI, null, num256, num257);
					NetMessage.TrySendData(23, -1, -1, null, num256);
				}
				break;
			}
			case 27:
			{
				int num147 = reader.ReadInt16();
				Vector2 position3 = reader.ReadVector2();
				Vector2 velocity4 = reader.ReadVector2();
				int num148 = reader.ReadByte();
				int num149 = reader.ReadInt16();
				BitsByte bitsByte8 = reader.ReadByte();
				float[] array2 = new float[Projectile.maxAI];
				for (int num150 = 0; num150 < Projectile.maxAI; num150++)
				{
					if (bitsByte8[num150])
					{
						array2[num150] = reader.ReadSingle();
					}
					else
					{
						array2[num150] = 0f;
					}
				}
				int damage = (bitsByte8[4] ? reader.ReadInt16() : 0);
				float knockBack = (bitsByte8[5] ? reader.ReadSingle() : 0f);
				int originalDamage = (bitsByte8[6] ? reader.ReadInt16() : 0);
				int num151 = (bitsByte8[7] ? reader.ReadInt16() : (-1));
				if (num151 >= 1000)
				{
					num151 = -1;
				}
				if (Main.netMode == 2)
				{
					if (num149 == 949)
					{
						num148 = 255;
					}
					else
					{
						num148 = whoAmI;
						if (Main.projHostile[num149])
						{
							break;
						}
					}
				}
				int num152 = 1000;
				for (int num153 = 0; num153 < 1000; num153++)
				{
					if (Main.projectile[num153].owner == num148 && Main.projectile[num153].identity == num147 && Main.projectile[num153].active)
					{
						num152 = num153;
						break;
					}
				}
				if (num152 == 1000)
				{
					for (int num154 = 0; num154 < 1000; num154++)
					{
						if (!Main.projectile[num154].active)
						{
							num152 = num154;
							break;
						}
					}
				}
				if (num152 == 1000)
				{
					num152 = Projectile.FindOldestProjectile();
				}
				Projectile projectile = Main.projectile[num152];
				if (!projectile.active || projectile.type != num149)
				{
					projectile.SetDefaults(num149);
					if (Main.netMode == 2)
					{
						Netplay.Clients[whoAmI].SpamProjectile += 1f;
					}
				}
				projectile.identity = num147;
				projectile.position = position3;
				projectile.velocity = velocity4;
				projectile.type = num149;
				projectile.damage = damage;
				projectile.originalDamage = originalDamage;
				projectile.knockBack = knockBack;
				projectile.owner = num148;
				for (int num155 = 0; num155 < Projectile.maxAI; num155++)
				{
					projectile.ai[num155] = array2[num155];
				}
				if (num151 >= 0)
				{
					projectile.projUUID = num151;
					Main.projectileIdentity[num148, num151] = num152;
				}
				projectile.ProjectileFixDesperation();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(27, -1, whoAmI, null, num152);
				}
				break;
			}
			case 28:
			{
				int num258 = reader.ReadInt16();
				int num259 = reader.ReadInt16();
				float num260 = reader.ReadSingle();
				int num261 = reader.ReadByte() - 1;
				byte b13 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					if (num259 < 0)
					{
						num259 = 0;
					}
					Main.npc[num258].PlayerInteraction(whoAmI);
				}
				if (num259 >= 0)
				{
					Main.npc[num258].StrikeNPC(num259, num260, num261, b13 == 1, noEffect: false, fromNet: true);
				}
				else
				{
					Main.npc[num258].life = 0;
					Main.npc[num258].HitEffect();
					Main.npc[num258].active = false;
				}
				if (Main.netMode != 2)
				{
					break;
				}
				NetMessage.TrySendData(28, -1, whoAmI, null, num258, num259, num260, num261, b13);
				if (Main.npc[num258].life <= 0)
				{
					NetMessage.TrySendData(23, -1, -1, null, num258);
				}
				else
				{
					Main.npc[num258].netUpdate = true;
				}
				if (Main.npc[num258].realLife >= 0)
				{
					if (Main.npc[Main.npc[num258].realLife].life <= 0)
					{
						NetMessage.TrySendData(23, -1, -1, null, Main.npc[num258].realLife);
					}
					else
					{
						Main.npc[Main.npc[num258].realLife].netUpdate = true;
					}
				}
				break;
			}
			case 29:
			{
				int num188 = reader.ReadInt16();
				int num189 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num189 = whoAmI;
				}
				for (int num190 = 0; num190 < 1000; num190++)
				{
					if (Main.projectile[num190].owner == num189 && Main.projectile[num190].identity == num188 && Main.projectile[num190].active)
					{
						Main.projectile[num190].Kill();
						break;
					}
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(29, -1, whoAmI, null, num188, num189);
				}
				break;
			}
			case 30:
			{
				int num199 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num199 = whoAmI;
				}
				bool flag11 = reader.ReadBoolean();
				Main.player[num199].hostile = flag11;
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(30, -1, whoAmI, null, num199);
					LocalizedText obj7 = (flag11 ? Lang.mp[11] : Lang.mp[12]);
					ChatHelper.BroadcastChatMessage(color: Main.teamColor[Main.player[num199].team], text: NetworkText.FromKey(obj7.Key, Main.player[num199].name));
				}
				break;
			}
			case 31:
			{
				if (Main.netMode != 2)
				{
					break;
				}
				int num44 = reader.ReadInt16();
				int num45 = reader.ReadInt16();
				int num46 = Chest.FindChest(num44, num45);
				if (num46 > -1 && Chest.UsingChest(num46) == -1)
				{
					for (int num47 = 0; num47 < 40; num47++)
					{
						NetMessage.TrySendData(32, whoAmI, -1, null, num46, num47);
					}
					NetMessage.TrySendData(33, whoAmI, -1, null, num46);
					Main.player[whoAmI].chest = num46;
					if (Main.myPlayer == whoAmI)
					{
						Main.recBigList = false;
					}
					NetMessage.TrySendData(80, -1, whoAmI, null, whoAmI, num46);
					if (Main.netMode == 2 && WorldGen.IsChestRigged(num44, num45))
					{
						Wiring.SetCurrentUser(whoAmI);
						Wiring.HitSwitch(num44, num45);
						Wiring.SetCurrentUser();
						NetMessage.TrySendData(59, -1, whoAmI, null, num44, num45);
					}
				}
				break;
			}
			case 32:
			{
				int num245 = reader.ReadInt16();
				int num246 = reader.ReadByte();
				int stack8 = reader.ReadInt16();
				int pre3 = reader.ReadByte();
				int type16 = reader.ReadInt16();
				if (num245 >= 0 && num245 < 8000)
				{
					if (Main.chest[num245] == null)
					{
						Main.chest[num245] = new Chest();
					}
					if (Main.chest[num245].item[num246] == null)
					{
						Main.chest[num245].item[num246] = new Item();
					}
					Main.chest[num245].item[num246].netDefaults(type16);
					Main.chest[num245].item[num246].Prefix(pre3);
					Main.chest[num245].item[num246].stack = stack8;
					Recipe.FindRecipes(canDelayCheck: true);
				}
				break;
			}
			case 33:
			{
				int num59 = reader.ReadInt16();
				int num60 = reader.ReadInt16();
				int num61 = reader.ReadInt16();
				int num62 = reader.ReadByte();
				string name = string.Empty;
				if (num62 != 0)
				{
					if (num62 <= 20)
					{
						name = reader.ReadString();
					}
					else if (num62 != 255)
					{
						num62 = 0;
					}
				}
				if (Main.netMode == 1)
				{
					Player player5 = Main.player[Main.myPlayer];
					if (player5.chest == -1)
					{
						Main.playerInventory = true;
						SoundEngine.PlaySound(10);
					}
					else if (player5.chest != num59 && num59 != -1)
					{
						Main.playerInventory = true;
						SoundEngine.PlaySound(12);
						Main.recBigList = false;
					}
					else if (player5.chest != -1 && num59 == -1)
					{
						SoundEngine.PlaySound(11);
						Main.recBigList = false;
					}
					player5.chest = num59;
					player5.chestX = num60;
					player5.chestY = num61;
					Recipe.FindRecipes(canDelayCheck: true);
					if (Main.tile[num60, num61].frameX >= 36 && Main.tile[num60, num61].frameX < 72)
					{
						AchievementsHelper.HandleSpecialEvent(Main.player[Main.myPlayer], 16);
					}
				}
				else
				{
					if (num62 != 0)
					{
						int chest = Main.player[whoAmI].chest;
						Chest chest2 = Main.chest[chest];
						chest2.name = name;
						NetMessage.TrySendData(69, -1, whoAmI, null, chest, chest2.x, chest2.y);
					}
					Main.player[whoAmI].chest = num59;
					Recipe.FindRecipes(canDelayCheck: true);
					NetMessage.TrySendData(80, -1, whoAmI, null, whoAmI, num59);
				}
				break;
			}
			case 34:
			{
				byte b2 = reader.ReadByte();
				int num12 = reader.ReadInt16();
				int num13 = reader.ReadInt16();
				int num14 = reader.ReadInt16();
				int num15 = reader.ReadInt16();
				if (Main.netMode == 2)
				{
					num15 = 0;
				}
				if (Main.netMode == 2)
				{
					switch (b2)
					{
					case 0:
					{
						int num18 = WorldGen.PlaceChest(num12, num13, 21, notNearOtherChests: false, num14);
						if (num18 == -1)
						{
							NetMessage.TrySendData(34, whoAmI, -1, null, b2, num12, num13, num14, num18);
							Item.NewItem(num12 * 16, num13 * 16, 32, 32, Chest.chestItemSpawn[num14], 1, noBroadcast: true);
						}
						else
						{
							NetMessage.TrySendData(34, -1, -1, null, b2, num12, num13, num14, num18);
						}
						break;
					}
					case 1:
						if (Main.tile[num12, num13].type == 21)
						{
							Tile tile = Main.tile[num12, num13];
							if (tile.frameX % 36 != 0)
							{
								num12--;
							}
							if (tile.frameY % 36 != 0)
							{
								num13--;
							}
							int number = Chest.FindChest(num12, num13);
							WorldGen.KillTile(num12, num13);
							if (!tile.active())
							{
								NetMessage.TrySendData(34, -1, -1, null, b2, num12, num13, 0f, number);
							}
							break;
						}
						goto default;
					default:
						switch (b2)
						{
						case 2:
						{
							int num16 = WorldGen.PlaceChest(num12, num13, 88, notNearOtherChests: false, num14);
							if (num16 == -1)
							{
								NetMessage.TrySendData(34, whoAmI, -1, null, b2, num12, num13, num14, num16);
								Item.NewItem(num12 * 16, num13 * 16, 32, 32, Chest.dresserItemSpawn[num14], 1, noBroadcast: true);
							}
							else
							{
								NetMessage.TrySendData(34, -1, -1, null, b2, num12, num13, num14, num16);
							}
							break;
						}
						case 3:
							if (Main.tile[num12, num13].type == 88)
							{
								Tile tile2 = Main.tile[num12, num13];
								num12 -= tile2.frameX % 54 / 18;
								if (tile2.frameY % 36 != 0)
								{
									num13--;
								}
								int number2 = Chest.FindChest(num12, num13);
								WorldGen.KillTile(num12, num13);
								if (!tile2.active())
								{
									NetMessage.TrySendData(34, -1, -1, null, b2, num12, num13, 0f, number2);
								}
								break;
							}
							goto default;
						default:
							switch (b2)
							{
							case 4:
							{
								int num17 = WorldGen.PlaceChest(num12, num13, 467, notNearOtherChests: false, num14);
								if (num17 == -1)
								{
									NetMessage.TrySendData(34, whoAmI, -1, null, b2, num12, num13, num14, num17);
									Item.NewItem(num12 * 16, num13 * 16, 32, 32, Chest.chestItemSpawn2[num14], 1, noBroadcast: true);
								}
								else
								{
									NetMessage.TrySendData(34, -1, -1, null, b2, num12, num13, num14, num17);
								}
								break;
							}
							case 5:
								if (Main.tile[num12, num13].type == 467)
								{
									Tile tile3 = Main.tile[num12, num13];
									if (tile3.frameX % 36 != 0)
									{
										num12--;
									}
									if (tile3.frameY % 36 != 0)
									{
										num13--;
									}
									int number3 = Chest.FindChest(num12, num13);
									WorldGen.KillTile(num12, num13);
									if (!tile3.active())
									{
										NetMessage.TrySendData(34, -1, -1, null, b2, num12, num13, 0f, number3);
									}
								}
								break;
							}
							break;
						}
						break;
					}
					break;
				}
				switch (b2)
				{
				case 0:
					if (num15 == -1)
					{
						WorldGen.KillTile(num12, num13);
						break;
					}
					SoundEngine.PlaySound(0, num12 * 16, num13 * 16);
					WorldGen.PlaceChestDirect(num12, num13, 21, num14, num15);
					break;
				case 2:
					if (num15 == -1)
					{
						WorldGen.KillTile(num12, num13);
						break;
					}
					SoundEngine.PlaySound(0, num12 * 16, num13 * 16);
					WorldGen.PlaceDresserDirect(num12, num13, 88, num14, num15);
					break;
				case 4:
					if (num15 == -1)
					{
						WorldGen.KillTile(num12, num13);
						break;
					}
					SoundEngine.PlaySound(0, num12 * 16, num13 * 16);
					WorldGen.PlaceChestDirect(num12, num13, 467, num14, num15);
					break;
				default:
					Chest.DestroyChestDirect(num12, num13, num15);
					WorldGen.KillTile(num12, num13);
					break;
				}
				break;
			}
			case 35:
			{
				int num241 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num241 = whoAmI;
				}
				int num242 = reader.ReadInt16();
				if (num241 != Main.myPlayer || Main.ServerSideCharacter)
				{
					Main.player[num241].HealEffect(num242);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(35, -1, whoAmI, null, num241, num242);
				}
				break;
			}
			case 36:
			{
				int num222 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num222 = whoAmI;
				}
				Player obj8 = Main.player[num222];
				obj8.zone1 = reader.ReadByte();
				obj8.zone2 = reader.ReadByte();
				obj8.zone3 = reader.ReadByte();
				obj8.zone4 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(36, -1, whoAmI, null, num222);
				}
				break;
			}
			case 37:
				if (Main.netMode == 1)
				{
					if (Main.autoPass)
					{
						NetMessage.TrySendData(38);
						Main.autoPass = false;
					}
					else
					{
						Netplay.ServerPassword = "";
						Main.menuMode = 31;
					}
				}
				break;
			case 38:
				if (Main.netMode == 2)
				{
					if (reader.ReadString() == Netplay.ServerPassword)
					{
						Netplay.Clients[whoAmI].State = 1;
						NetMessage.TrySendData(3, whoAmI);
					}
					else
					{
						NetMessage.TrySendData(2, whoAmI, -1, Lang.mp[1].ToNetworkText());
					}
				}
				break;
			case 39:
				if (Main.netMode == 1)
				{
					int num164 = reader.ReadInt16();
					Main.item[num164].playerIndexTheItemIsReservedFor = 255;
					NetMessage.TrySendData(22, -1, -1, null, num164);
				}
				break;
			case 40:
			{
				int num159 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num159 = whoAmI;
				}
				int npcIndex = reader.ReadInt16();
				Main.player[num159].SetTalkNPC(npcIndex, fromNet: true);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(40, -1, whoAmI, null, num159);
				}
				break;
			}
			case 41:
			{
				int num91 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num91 = whoAmI;
				}
				Player player6 = Main.player[num91];
				float itemRotation = reader.ReadSingle();
				int itemAnimation = reader.ReadInt16();
				player6.itemRotation = itemRotation;
				player6.itemAnimation = itemAnimation;
				player6.channel = player6.inventory[player6.selectedItem].channel;
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(41, -1, whoAmI, null, num91);
				}
				break;
			}
			case 42:
			{
				int num58 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num58 = whoAmI;
				}
				else if (Main.myPlayer == num58 && !Main.ServerSideCharacter)
				{
					break;
				}
				int statMana = reader.ReadInt16();
				int statManaMax = reader.ReadInt16();
				Main.player[num58].statMana = statMana;
				Main.player[num58].statManaMax = statManaMax;
				break;
			}
			case 43:
			{
				int num7 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num7 = whoAmI;
				}
				int num8 = reader.ReadInt16();
				if (num7 != Main.myPlayer)
				{
					Main.player[num7].ManaEffect(num8);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(43, -1, whoAmI, null, num7, num8);
				}
				break;
			}
			case 45:
			{
				int num238 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num238 = whoAmI;
				}
				int num239 = reader.ReadByte();
				Player player13 = Main.player[num238];
				int team = player13.team;
				player13.team = num239;
				Color color4 = Main.teamColor[num239];
				if (Main.netMode != 2)
				{
					break;
				}
				NetMessage.TrySendData(45, -1, whoAmI, null, num238);
				LocalizedText localizedText = Lang.mp[13 + num239];
				if (num239 == 5)
				{
					localizedText = Lang.mp[22];
				}
				for (int num240 = 0; num240 < 255; num240++)
				{
					if (num240 == whoAmI || (team > 0 && Main.player[num240].team == team) || (num239 > 0 && Main.player[num240].team == num239))
					{
						ChatHelper.SendChatMessageToClient(NetworkText.FromKey(localizedText.Key, player13.name), color4, num240);
					}
				}
				break;
			}
			case 46:
				if (Main.netMode == 2)
				{
					short i3 = reader.ReadInt16();
					int j3 = reader.ReadInt16();
					int num227 = Sign.ReadSign(i3, j3);
					if (num227 >= 0)
					{
						NetMessage.TrySendData(47, whoAmI, -1, null, num227, whoAmI);
					}
				}
				break;
			case 47:
			{
				int num172 = reader.ReadInt16();
				int x9 = reader.ReadInt16();
				int y8 = reader.ReadInt16();
				string text3 = reader.ReadString();
				int num173 = reader.ReadByte();
				BitsByte bitsByte13 = reader.ReadByte();
				if (num172 >= 0 && num172 < 1000)
				{
					string a = null;
					if (Main.sign[num172] != null)
					{
						a = Main.sign[num172].text;
					}
					Main.sign[num172] = new Sign();
					Main.sign[num172].x = x9;
					Main.sign[num172].y = y8;
					Sign.TextSign(num172, text3);
					if (Main.netMode == 2 && a != text3)
					{
						num173 = whoAmI;
						NetMessage.TrySendData(47, -1, whoAmI, null, num172, num173);
					}
					if (Main.netMode == 1 && num173 == Main.myPlayer && Main.sign[num172] != null && !bitsByte13[0])
					{
						Main.playerInventory = false;
						Main.player[Main.myPlayer].SetTalkNPC(-1, fromNet: true);
						Main.npcChatCornerItem = 0;
						Main.editSign = false;
						SoundEngine.PlaySound(10);
						Main.player[Main.myPlayer].sign = num172;
						Main.npcChatText = Main.sign[num172].text;
					}
				}
				break;
			}
			case 48:
			{
				int num74 = reader.ReadInt16();
				int num75 = reader.ReadInt16();
				byte liquid = reader.ReadByte();
				byte liquidType = reader.ReadByte();
				if (Main.netMode == 2 && Netplay.SpamCheck)
				{
					int num76 = whoAmI;
					int num77 = (int)(Main.player[num76].position.X + (float)(Main.player[num76].width / 2));
					int num78 = (int)(Main.player[num76].position.Y + (float)(Main.player[num76].height / 2));
					int num79 = 10;
					int num80 = num77 - num79;
					int num81 = num77 + num79;
					int num82 = num78 - num79;
					int num83 = num78 + num79;
					if (num74 < num80 || num74 > num81 || num75 < num82 || num75 > num83)
					{
						NetMessage.BootPlayer(whoAmI, NetworkText.FromKey("Net.CheatingLiquidSpam"));
						break;
					}
				}
				if (Main.tile[num74, num75] == null)
				{
					Main.tile[num74, num75] = new Tile();
				}
				lock (Main.tile[num74, num75])
				{
					Main.tile[num74, num75].liquid = liquid;
					Main.tile[num74, num75].liquidType(liquidType);
					if (Main.netMode == 2)
					{
						WorldGen.SquareTileFrame(num74, num75);
					}
				}
				break;
			}
			case 49:
				if (Netplay.Connection.State == 6)
				{
					Netplay.Connection.State = 10;
					Main.ActivePlayerFileData.StartPlayTimer();
					Player.Hooks.EnterWorld(Main.myPlayer);
					Main.player[Main.myPlayer].Spawn(PlayerSpawnContext.SpawningIntoWorld);
				}
				break;
			case 50:
			{
				int num42 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num42 = whoAmI;
				}
				else if (num42 == Main.myPlayer && !Main.ServerSideCharacter)
				{
					break;
				}
				Player player4 = Main.player[num42];
				for (int num43 = 0; num43 < 22; num43++)
				{
					player4.buffType[num43] = reader.ReadUInt16();
					if (player4.buffType[num43] > 0)
					{
						player4.buffTime[num43] = 60;
					}
					else
					{
						player4.buffTime[num43] = 0;
					}
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(50, -1, whoAmI, null, num42);
				}
				break;
			}
			case 51:
			{
				byte b3 = reader.ReadByte();
				byte b4 = reader.ReadByte();
				switch (b4)
				{
				case 1:
					NPC.SpawnSkeletron();
					break;
				case 2:
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(51, -1, whoAmI, null, b3, (int)b4);
					}
					else
					{
						SoundEngine.PlaySound(SoundID.Item1, (int)Main.player[b3].position.X, (int)Main.player[b3].position.Y);
					}
					break;
				case 3:
					if (Main.netMode == 2)
					{
						Main.Sundialing();
					}
					break;
				case 4:
					Main.npc[b3].BigMimicSpawnSmoke();
					break;
				}
				break;
			}
			case 52:
			{
				int num262 = reader.ReadByte();
				int num263 = reader.ReadInt16();
				int num264 = reader.ReadInt16();
				if (num262 == 1)
				{
					Chest.Unlock(num263, num264);
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(52, -1, whoAmI, null, 0, num262, num263, num264);
						NetMessage.SendTileSquare(-1, num263, num264, 2);
					}
				}
				if (num262 == 2)
				{
					WorldGen.UnlockDoor(num263, num264);
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(52, -1, whoAmI, null, 0, num262, num263, num264);
						NetMessage.SendTileSquare(-1, num263, num264, 2);
					}
				}
				break;
			}
			case 53:
			{
				int num265 = reader.ReadInt16();
				int type18 = reader.ReadUInt16();
				int time2 = reader.ReadInt16();
				Main.npc[num265].AddBuff(type18, time2, quiet: true);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(54, -1, -1, null, num265);
				}
				break;
			}
			case 54:
				if (Main.netMode == 1)
				{
					int num243 = reader.ReadInt16();
					NPC nPC4 = Main.npc[num243];
					for (int num244 = 0; num244 < 5; num244++)
					{
						nPC4.buffType[num244] = reader.ReadUInt16();
						nPC4.buffTime[num244] = reader.ReadInt16();
					}
				}
				break;
			case 55:
			{
				int num219 = reader.ReadByte();
				int num220 = reader.ReadUInt16();
				int num221 = reader.ReadInt32();
				if (Main.netMode != 2 || num219 == whoAmI || Main.pvpBuff[num220])
				{
					if (Main.netMode == 1 && num219 == Main.myPlayer)
					{
						Main.player[num219].AddBuff(num220, num221);
					}
					else if (Main.netMode == 2)
					{
						NetMessage.TrySendData(55, num219, -1, null, num219, num220, num221);
					}
				}
				break;
			}
			case 56:
			{
				int num208 = reader.ReadInt16();
				if (num208 >= 0 && num208 < 200)
				{
					if (Main.netMode == 1)
					{
						string givenName = reader.ReadString();
						Main.npc[num208].GivenName = givenName;
						int townNpcVariationIndex = reader.ReadInt32();
						Main.npc[num208].townNpcVariationIndex = townNpcVariationIndex;
					}
					else if (Main.netMode == 2)
					{
						NetMessage.TrySendData(56, whoAmI, -1, null, num208);
					}
				}
				break;
			}
			case 57:
				if (Main.netMode == 1)
				{
					WorldGen.tGood = reader.ReadByte();
					WorldGen.tEvil = reader.ReadByte();
					WorldGen.tBlood = reader.ReadByte();
				}
				break;
			case 58:
			{
				int num186 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num186 = whoAmI;
				}
				float num187 = reader.ReadSingle();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(58, -1, whoAmI, null, whoAmI, num187);
					break;
				}
				Player player9 = Main.player[num186];
				int type11 = player9.inventory[player9.selectedItem].type;
				switch (type11)
				{
				case 4057:
				case 4372:
				case 4715:
					player9.PlayGuitarChord(num187);
					break;
				case 4673:
					player9.PlayDrums(num187);
					break;
				default:
				{
					Main.musicPitch = num187;
					LegacySoundStyle type12 = SoundID.Item26;
					if (type11 == 507)
					{
						type12 = SoundID.Item35;
					}
					if (type11 == 1305)
					{
						type12 = SoundID.Item47;
					}
					SoundEngine.PlaySound(type12, player9.position);
					break;
				}
				}
				break;
			}
			case 59:
			{
				int num170 = reader.ReadInt16();
				int num171 = reader.ReadInt16();
				Wiring.SetCurrentUser(whoAmI);
				Wiring.HitSwitch(num170, num171);
				Wiring.SetCurrentUser();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(59, -1, whoAmI, null, num170, num171);
				}
				break;
			}
			case 60:
			{
				int num141 = reader.ReadInt16();
				int num142 = reader.ReadInt16();
				int num143 = reader.ReadInt16();
				byte b11 = reader.ReadByte();
				if (num141 >= 200)
				{
					NetMessage.BootPlayer(whoAmI, NetworkText.FromKey("Net.CheatingInvalid"));
				}
				else if (Main.netMode == 1)
				{
					Main.npc[num141].homeless = b11 == 1;
					Main.npc[num141].homeTileX = num142;
					Main.npc[num141].homeTileY = num143;
					switch (b11)
					{
					case 1:
						WorldGen.TownManager.KickOut(Main.npc[num141].type);
						break;
					case 2:
						WorldGen.TownManager.SetRoom(Main.npc[num141].type, num142, num143);
						break;
					}
				}
				else if (b11 == 1)
				{
					WorldGen.kickOut(num141);
				}
				else
				{
					WorldGen.moveRoom(num142, num143, num141);
				}
				break;
			}
			case 61:
			{
				int plr = reader.ReadInt16();
				int num182 = reader.ReadInt16();
				if (Main.netMode != 2)
				{
					break;
				}
				if (num182 >= 0 && num182 < 663 && NPCID.Sets.MPAllowedEnemies[num182])
				{
					if (!NPC.AnyNPCs(num182))
					{
						NPC.SpawnOnPlayer(plr, num182);
					}
				}
				else if (num182 == -4)
				{
					if (!Main.dayTime && !DD2Event.Ongoing)
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[31].Key), new Color(50, 255, 130));
						Main.startPumpkinMoon();
						NetMessage.TrySendData(7);
						NetMessage.TrySendData(78, -1, -1, null, 0, 1f, 2f, 1f);
					}
				}
				else if (num182 == -5)
				{
					if (!Main.dayTime && !DD2Event.Ongoing)
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[34].Key), new Color(50, 255, 130));
						Main.startSnowMoon();
						NetMessage.TrySendData(7);
						NetMessage.TrySendData(78, -1, -1, null, 0, 1f, 1f, 1f);
					}
				}
				else if (num182 == -6)
				{
					if (Main.dayTime && !Main.eclipse)
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[20].Key), new Color(50, 255, 130));
						Main.eclipse = true;
						NetMessage.TrySendData(7);
					}
				}
				else if (num182 == -7)
				{
					Main.invasionDelay = 0;
					Main.StartInvasion(4);
					NetMessage.TrySendData(7);
					NetMessage.TrySendData(78, -1, -1, null, 0, 1f, Main.invasionType + 3);
				}
				else if (num182 == -8)
				{
					if (NPC.downedGolemBoss && Main.hardMode && !NPC.AnyDanger() && !NPC.AnyoneNearCultists())
					{
						WorldGen.StartImpendingDoom();
						NetMessage.TrySendData(7);
					}
				}
				else if (num182 == -10)
				{
					if (!Main.dayTime && !Main.bloodMoon)
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[8].Key), new Color(50, 255, 130));
						Main.bloodMoon = true;
						if (Main.GetMoonPhase() == MoonPhase.Empty)
						{
							Main.moonPhase = 5;
						}
						AchievementsHelper.NotifyProgressionEvent(4);
						NetMessage.TrySendData(7);
					}
				}
				else if (num182 == -11)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.CombatBookUsed"), new Color(50, 255, 130));
					NPC.combatBookWasUsed = true;
					NetMessage.TrySendData(7);
				}
				else if (num182 == -12)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.LicenseCatUsed"), new Color(50, 255, 130));
					NPC.boughtCat = true;
					NetMessage.TrySendData(7);
				}
				else if (num182 == -13)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.LicenseDogUsed"), new Color(50, 255, 130));
					NPC.boughtDog = true;
					NetMessage.TrySendData(7);
				}
				else if (num182 == -14)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.LicenseBunnyUsed"), new Color(50, 255, 130));
					NPC.boughtBunny = true;
					NetMessage.TrySendData(7);
				}
				else if (num182 < 0)
				{
					int num183 = 1;
					if (num182 > -5)
					{
						num183 = -num182;
					}
					if (num183 > 0 && Main.invasionType == 0)
					{
						Main.invasionDelay = 0;
						Main.StartInvasion(num183);
					}
					NetMessage.TrySendData(78, -1, -1, null, 0, 1f, Main.invasionType + 3);
				}
				break;
			}
			case 62:
			{
				int num139 = reader.ReadByte();
				int num140 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num139 = whoAmI;
				}
				if (num140 == 1)
				{
					Main.player[num139].NinjaDodge();
				}
				if (num140 == 2)
				{
					Main.player[num139].ShadowDodge();
				}
				if (num140 == 4)
				{
					Main.player[num139].BrainOfConfusionDodge();
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(62, -1, whoAmI, null, num139, num140);
				}
				break;
			}
			case 63:
			{
				int num135 = reader.ReadInt16();
				int num136 = reader.ReadInt16();
				byte b10 = reader.ReadByte();
				WorldGen.paintTile(num135, num136, b10);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(63, -1, whoAmI, null, num135, num136, (int)b10);
				}
				break;
			}
			case 64:
			{
				int num133 = reader.ReadInt16();
				int num134 = reader.ReadInt16();
				byte b9 = reader.ReadByte();
				WorldGen.paintWall(num133, num134, b9);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(64, -1, whoAmI, null, num133, num134, (int)b9);
				}
				break;
			}
			case 65:
			{
				BitsByte bitsByte3 = reader.ReadByte();
				int num27 = reader.ReadInt16();
				if (Main.netMode == 2)
				{
					num27 = whoAmI;
				}
				Vector2 vector = reader.ReadVector2();
				int num28 = 0;
				num28 = reader.ReadByte();
				int num29 = 0;
				if (bitsByte3[0])
				{
					num29++;
				}
				if (bitsByte3[1])
				{
					num29 += 2;
				}
				bool flag3 = false;
				if (bitsByte3[2])
				{
					flag3 = true;
				}
				int num30 = 0;
				if (bitsByte3[3])
				{
					num30 = reader.ReadInt32();
				}
				if (flag3)
				{
					vector = Main.player[num27].position;
				}
				switch (num29)
				{
				case 0:
					Main.player[num27].Teleport(vector, num28, num30);
					break;
				case 1:
					Main.npc[num27].Teleport(vector, num28, num30);
					break;
				case 2:
				{
					Main.player[num27].Teleport(vector, num28, num30);
					if (Main.netMode != 2)
					{
						break;
					}
					RemoteClient.CheckSection(whoAmI, vector);
					NetMessage.TrySendData(65, -1, -1, null, 0, num27, vector.X, vector.Y, num28, flag3.ToInt(), num30);
					int num31 = -1;
					float num32 = 9999f;
					for (int m = 0; m < 255; m++)
					{
						if (Main.player[m].active && m != whoAmI)
						{
							Vector2 vector2 = Main.player[m].position - Main.player[whoAmI].position;
							if (vector2.Length() < num32)
							{
								num32 = vector2.Length();
								num31 = m;
							}
						}
					}
					if (num31 >= 0)
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Game.HasTeleportedTo", Main.player[whoAmI].name, Main.player[num31].name), new Color(250, 250, 0));
					}
					break;
				}
				}
				if (Main.netMode == 2 && num29 == 0)
				{
					NetMessage.TrySendData(65, -1, whoAmI, null, num29, num27, vector.X, vector.Y, num28, flag3.ToInt(), num30);
				}
				break;
			}
			case 66:
			{
				int num10 = reader.ReadByte();
				int num11 = reader.ReadInt16();
				if (num11 > 0)
				{
					Player player = Main.player[num10];
					player.statLife += num11;
					if (player.statLife > player.statLifeMax2)
					{
						player.statLife = player.statLifeMax2;
					}
					player.HealEffect(num11, broadcast: false);
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(66, -1, whoAmI, null, num10, num11);
					}
				}
				break;
			}
			case 68:
				reader.ReadString();
				break;
			case 69:
			{
				int num216 = reader.ReadInt16();
				int num217 = reader.ReadInt16();
				int num218 = reader.ReadInt16();
				if (Main.netMode == 1)
				{
					if (num216 >= 0 && num216 < 8000)
					{
						Chest chest3 = Main.chest[num216];
						if (chest3 == null)
						{
							chest3 = new Chest();
							chest3.x = num217;
							chest3.y = num218;
							Main.chest[num216] = chest3;
						}
						else if (chest3.x != num217 || chest3.y != num218)
						{
							break;
						}
						chest3.name = reader.ReadString();
					}
				}
				else
				{
					if (num216 < -1 || num216 >= 8000)
					{
						break;
					}
					if (num216 == -1)
					{
						num216 = Chest.FindChest(num217, num218);
						if (num216 == -1)
						{
							break;
						}
					}
					Chest chest4 = Main.chest[num216];
					if (chest4.x == num217 && chest4.y == num218)
					{
						NetMessage.TrySendData(69, whoAmI, -1, null, num216, num217, num218);
					}
				}
				break;
			}
			case 70:
				if (Main.netMode == 2)
				{
					int num201 = reader.ReadInt16();
					int who = reader.ReadByte();
					if (Main.netMode == 2)
					{
						who = whoAmI;
					}
					if (num201 < 200 && num201 >= 0)
					{
						NPC.CatchNPC(num201, who);
					}
				}
				break;
			case 71:
				if (Main.netMode == 2)
				{
					int x11 = reader.ReadInt32();
					int y10 = reader.ReadInt32();
					int type13 = reader.ReadInt16();
					byte style3 = reader.ReadByte();
					NPC.ReleaseNPC(x11, y10, type13, style3, whoAmI);
				}
				break;
			case 72:
				if (Main.netMode == 1)
				{
					for (int num196 = 0; num196 < 40; num196++)
					{
						Main.travelShop[num196] = reader.ReadInt16();
					}
				}
				break;
			case 73:
				switch (reader.ReadByte())
				{
				case 0:
					Main.player[whoAmI].TeleportationPotion();
					break;
				case 1:
					Main.player[whoAmI].MagicConch();
					break;
				case 2:
					Main.player[whoAmI].DemonConch();
					break;
				}
				break;
			case 74:
				if (Main.netMode == 1)
				{
					Main.anglerQuest = reader.ReadByte();
					Main.anglerQuestFinished = reader.ReadBoolean();
				}
				break;
			case 75:
				if (Main.netMode == 2)
				{
					string name2 = Main.player[whoAmI].name;
					if (!Main.anglerWhoFinishedToday.Contains(name2))
					{
						Main.anglerWhoFinishedToday.Add(name2);
					}
				}
				break;
			case 76:
			{
				int num169 = reader.ReadByte();
				if (num169 != Main.myPlayer || Main.ServerSideCharacter)
				{
					if (Main.netMode == 2)
					{
						num169 = whoAmI;
					}
					Player obj6 = Main.player[num169];
					obj6.anglerQuestsFinished = reader.ReadInt32();
					obj6.golferScoreAccumulated = reader.ReadInt32();
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(76, -1, whoAmI, null, num169);
					}
				}
				break;
			}
			case 77:
			{
				short type10 = reader.ReadInt16();
				ushort tileType = reader.ReadUInt16();
				short x8 = reader.ReadInt16();
				short y7 = reader.ReadInt16();
				Animation.NewTemporaryAnimation(type10, tileType, x8, y7);
				break;
			}
			case 78:
				if (Main.netMode == 1)
				{
					Main.ReportInvasionProgress(reader.ReadInt32(), reader.ReadInt32(), reader.ReadSByte(), reader.ReadSByte());
				}
				break;
			case 79:
			{
				int x6 = reader.ReadInt16();
				int y5 = reader.ReadInt16();
				short type9 = reader.ReadInt16();
				int style2 = reader.ReadInt16();
				int num138 = reader.ReadByte();
				int random = reader.ReadSByte();
				int direction = (reader.ReadBoolean() ? 1 : (-1));
				if (Main.netMode == 2)
				{
					Netplay.Clients[whoAmI].SpamAddBlock += 1f;
					if (!WorldGen.InWorld(x6, y5, 10) || !Netplay.Clients[whoAmI].TileSections[Netplay.GetSectionX(x6), Netplay.GetSectionY(y5)])
					{
						break;
					}
				}
				WorldGen.PlaceObject(x6, y5, type9, mute: false, style2, num138, random, direction);
				if (Main.netMode == 2)
				{
					NetMessage.SendObjectPlacment(whoAmI, x6, y5, type9, style2, num138, random, direction);
				}
				break;
			}
			case 80:
				if (Main.netMode == 1)
				{
					int num127 = reader.ReadByte();
					int num128 = reader.ReadInt16();
					if (num128 >= -3 && num128 < 8000)
					{
						Main.player[num127].chest = num128;
						Recipe.FindRecipes(canDelayCheck: true);
					}
				}
				break;
			case 81:
				if (Main.netMode == 1)
				{
					int x5 = (int)reader.ReadSingle();
					int y4 = (int)reader.ReadSingle();
					CombatText.NewText(color: reader.ReadRGB(), amount: reader.ReadInt32(), location: new Rectangle(x5, y4, 0, 0));
				}
				break;
			case 119:
				if (Main.netMode == 1)
				{
					int x4 = (int)reader.ReadSingle();
					int y3 = (int)reader.ReadSingle();
					CombatText.NewText(color: reader.ReadRGB(), text: NetworkText.Deserialize(reader).ToString(), location: new Rectangle(x4, y3, 0, 0));
				}
				break;
			case 82:
				NetManager.Instance.Read(reader, whoAmI, length);
				break;
			case 83:
				if (Main.netMode == 1)
				{
					int num89 = reader.ReadInt16();
					int num90 = reader.ReadInt32();
					if (num89 >= 0 && num89 < 289)
					{
						NPC.killCount[num89] = num90;
					}
				}
				break;
			case 84:
			{
				int num63 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num63 = whoAmI;
				}
				float stealth = reader.ReadSingle();
				Main.player[num63].stealth = stealth;
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(84, -1, whoAmI, null, num63);
				}
				break;
			}
			case 85:
			{
				int num57 = whoAmI;
				byte b5 = reader.ReadByte();
				if (Main.netMode == 2 && num57 < 255 && b5 < 58)
				{
					Chest.ServerPlaceItem(whoAmI, b5);
				}
				break;
			}
			case 86:
			{
				if (Main.netMode != 1)
				{
					break;
				}
				int num33 = reader.ReadInt32();
				if (!reader.ReadBoolean())
				{
					if (TileEntity.ByID.TryGetValue(num33, out var value2))
					{
						TileEntity.ByID.Remove(num33);
						TileEntity.ByPosition.Remove(value2.Position);
					}
				}
				else
				{
					TileEntity tileEntity = TileEntity.Read(reader, networkSend: true);
					tileEntity.ID = num33;
					TileEntity.ByID[tileEntity.ID] = tileEntity;
					TileEntity.ByPosition[tileEntity.Position] = tileEntity;
				}
				break;
			}
			case 87:
				if (Main.netMode == 2)
				{
					int x = reader.ReadInt16();
					int y = reader.ReadInt16();
					int type2 = reader.ReadByte();
					if (WorldGen.InWorld(x, y) && !TileEntity.ByPosition.ContainsKey(new Point16(x, y)))
					{
						TileEntity.PlaceEntityNet(x, y, type2);
					}
				}
				break;
			case 88:
			{
				if (Main.netMode != 1)
				{
					break;
				}
				int num200 = reader.ReadInt16();
				if (num200 < 0 || num200 > 400)
				{
					break;
				}
				Item item4 = Main.item[num200];
				BitsByte bitsByte22 = reader.ReadByte();
				if (bitsByte22[0])
				{
					item4.color.PackedValue = reader.ReadUInt32();
				}
				if (bitsByte22[1])
				{
					item4.damage = reader.ReadUInt16();
				}
				if (bitsByte22[2])
				{
					item4.knockBack = reader.ReadSingle();
				}
				if (bitsByte22[3])
				{
					item4.useAnimation = reader.ReadUInt16();
				}
				if (bitsByte22[4])
				{
					item4.useTime = reader.ReadUInt16();
				}
				if (bitsByte22[5])
				{
					item4.shoot = reader.ReadInt16();
				}
				if (bitsByte22[6])
				{
					item4.shootSpeed = reader.ReadSingle();
				}
				if (bitsByte22[7])
				{
					bitsByte22 = reader.ReadByte();
					if (bitsByte22[0])
					{
						item4.width = reader.ReadInt16();
					}
					if (bitsByte22[1])
					{
						item4.height = reader.ReadInt16();
					}
					if (bitsByte22[2])
					{
						item4.scale = reader.ReadSingle();
					}
					if (bitsByte22[3])
					{
						item4.ammo = reader.ReadInt16();
					}
					if (bitsByte22[4])
					{
						item4.useAmmo = reader.ReadInt16();
					}
					if (bitsByte22[5])
					{
						item4.notAmmo = reader.ReadBoolean();
					}
				}
				break;
			}
			case 89:
				if (Main.netMode == 2)
				{
					short x10 = reader.ReadInt16();
					int y9 = reader.ReadInt16();
					int netid3 = reader.ReadInt16();
					int prefix3 = reader.ReadByte();
					int stack5 = reader.ReadInt16();
					TEItemFrame.TryPlacing(x10, y9, netid3, prefix3, stack5);
				}
				break;
			case 91:
			{
				if (Main.netMode != 1)
				{
					break;
				}
				int num177 = reader.ReadInt32();
				int num178 = reader.ReadByte();
				if (num178 == 255)
				{
					if (EmoteBubble.byID.ContainsKey(num177))
					{
						EmoteBubble.byID.Remove(num177);
					}
					break;
				}
				int num179 = reader.ReadUInt16();
				int num180 = reader.ReadUInt16();
				int num181 = reader.ReadByte();
				int metadata = 0;
				if (num181 < 0)
				{
					metadata = reader.ReadInt16();
				}
				WorldUIAnchor worldUIAnchor = EmoteBubble.DeserializeNetAnchor(num178, num179);
				if (num178 == 1)
				{
					Main.player[num179].emoteTime = 360;
				}
				lock (EmoteBubble.byID)
				{
					if (!EmoteBubble.byID.ContainsKey(num177))
					{
						EmoteBubble.byID[num177] = new EmoteBubble(num181, worldUIAnchor, num180);
					}
					else
					{
						EmoteBubble.byID[num177].lifeTime = num180;
						EmoteBubble.byID[num177].lifeTimeStart = num180;
						EmoteBubble.byID[num177].emote = num181;
						EmoteBubble.byID[num177].anchor = worldUIAnchor;
					}
					EmoteBubble.byID[num177].ID = num177;
					EmoteBubble.byID[num177].metadata = metadata;
					EmoteBubble.OnBubbleChange(num177);
				}
				break;
			}
			case 92:
			{
				int num165 = reader.ReadInt16();
				int num166 = reader.ReadInt32();
				float num167 = reader.ReadSingle();
				float num168 = reader.ReadSingle();
				if (num165 >= 0 && num165 <= 200)
				{
					if (Main.netMode == 1)
					{
						Main.npc[num165].moneyPing(new Vector2(num167, num168));
						Main.npc[num165].extraValue = num166;
					}
					else
					{
						Main.npc[num165].extraValue += num166;
						NetMessage.TrySendData(92, -1, -1, null, num165, Main.npc[num165].extraValue, num167, num168);
					}
				}
				break;
			}
			case 95:
			{
				ushort num160 = reader.ReadUInt16();
				int num161 = reader.ReadByte();
				if (Main.netMode != 2)
				{
					break;
				}
				for (int num162 = 0; num162 < 1000; num162++)
				{
					if (Main.projectile[num162].owner == num160 && Main.projectile[num162].active && Main.projectile[num162].type == 602 && Main.projectile[num162].ai[1] == (float)num161)
					{
						Main.projectile[num162].Kill();
						NetMessage.TrySendData(29, -1, -1, null, Main.projectile[num162].identity, (int)num160);
						break;
					}
				}
				break;
			}
			case 96:
			{
				int num156 = reader.ReadByte();
				Player obj5 = Main.player[num156];
				int num157 = reader.ReadInt16();
				Vector2 newPos2 = reader.ReadVector2();
				Vector2 velocity5 = reader.ReadVector2();
				int num158 = (obj5.lastPortalColorIndex = num157 + ((num157 % 2 == 0) ? 1 : (-1)));
				obj5.Teleport(newPos2, 4, num157);
				obj5.velocity = velocity5;
				if (Main.netMode == 2)
				{
					NetMessage.SendData(96, -1, -1, null, num156, newPos2.X, newPos2.Y, num157);
				}
				break;
			}
			case 97:
				if (Main.netMode == 1)
				{
					AchievementsHelper.NotifyNPCKilledDirect(Main.player[Main.myPlayer], reader.ReadInt16());
				}
				break;
			case 98:
				if (Main.netMode == 1)
				{
					AchievementsHelper.NotifyProgressionEvent(reader.ReadInt16());
				}
				break;
			case 99:
			{
				int num137 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num137 = whoAmI;
				}
				Main.player[num137].MinionRestTargetPoint = reader.ReadVector2();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(99, -1, whoAmI, null, num137);
				}
				break;
			}
			case 115:
			{
				int num132 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num132 = whoAmI;
				}
				Main.player[num132].MinionAttackTargetNPC = reader.ReadInt16();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(115, -1, whoAmI, null, num132);
				}
				break;
			}
			case 100:
			{
				int num124 = reader.ReadUInt16();
				NPC obj4 = Main.npc[num124];
				int num125 = reader.ReadInt16();
				Vector2 newPos = reader.ReadVector2();
				Vector2 velocity3 = reader.ReadVector2();
				int num126 = (obj4.lastPortalColorIndex = num125 + ((num125 % 2 == 0) ? 1 : (-1)));
				obj4.Teleport(newPos, 4, num125);
				obj4.velocity = velocity3;
				obj4.netOffset *= 0f;
				break;
			}
			case 101:
				if (Main.netMode != 2)
				{
					NPC.ShieldStrengthTowerSolar = reader.ReadUInt16();
					NPC.ShieldStrengthTowerVortex = reader.ReadUInt16();
					NPC.ShieldStrengthTowerNebula = reader.ReadUInt16();
					NPC.ShieldStrengthTowerStardust = reader.ReadUInt16();
					if (NPC.ShieldStrengthTowerSolar < 0)
					{
						NPC.ShieldStrengthTowerSolar = 0;
					}
					if (NPC.ShieldStrengthTowerVortex < 0)
					{
						NPC.ShieldStrengthTowerVortex = 0;
					}
					if (NPC.ShieldStrengthTowerNebula < 0)
					{
						NPC.ShieldStrengthTowerNebula = 0;
					}
					if (NPC.ShieldStrengthTowerStardust < 0)
					{
						NPC.ShieldStrengthTowerStardust = 0;
					}
					if (NPC.ShieldStrengthTowerSolar > NPC.LunarShieldPowerExpert)
					{
						NPC.ShieldStrengthTowerSolar = NPC.LunarShieldPowerExpert;
					}
					if (NPC.ShieldStrengthTowerVortex > NPC.LunarShieldPowerExpert)
					{
						NPC.ShieldStrengthTowerVortex = NPC.LunarShieldPowerExpert;
					}
					if (NPC.ShieldStrengthTowerNebula > NPC.LunarShieldPowerExpert)
					{
						NPC.ShieldStrengthTowerNebula = NPC.LunarShieldPowerExpert;
					}
					if (NPC.ShieldStrengthTowerStardust > NPC.LunarShieldPowerExpert)
					{
						NPC.ShieldStrengthTowerStardust = NPC.LunarShieldPowerExpert;
					}
				}
				break;
			case 102:
			{
				int num34 = reader.ReadByte();
				ushort num35 = reader.ReadUInt16();
				Vector2 other = reader.ReadVector2();
				if (Main.netMode == 2)
				{
					num34 = whoAmI;
					NetMessage.TrySendData(102, -1, -1, null, num34, (int)num35, other.X, other.Y);
					break;
				}
				Player player2 = Main.player[num34];
				for (int n = 0; n < 255; n++)
				{
					Player player3 = Main.player[n];
					if (!player3.active || player3.dead || (player2.team != 0 && player2.team != player3.team) || !(player3.Distance(other) < 700f))
					{
						continue;
					}
					Vector2 value3 = player2.Center - player3.Center;
					Vector2 vector3 = Vector2.Normalize(value3);
					if (!vector3.HasNaNs())
					{
						int type3 = 90;
						float num36 = 0f;
						float num37 = (float)Math.PI / 15f;
						Vector2 spinningpoint = new Vector2(0f, -8f);
						Vector2 value4 = new Vector2(-3f);
						float num38 = 0f;
						float num39 = 0.005f;
						switch (num35)
						{
						case 179:
							type3 = 86;
							break;
						case 173:
							type3 = 90;
							break;
						case 176:
							type3 = 88;
							break;
						}
						for (int num40 = 0; (float)num40 < value3.Length() / 6f; num40++)
						{
							Vector2 position2 = player3.Center + 6f * (float)num40 * vector3 + spinningpoint.RotatedBy(num36) + value4;
							num36 += num37;
							int num41 = Dust.NewDust(position2, 6, 6, type3, 0f, 0f, 100, default(Color), 1.5f);
							Main.dust[num41].noGravity = true;
							Main.dust[num41].velocity = Vector2.Zero;
							num38 = (Main.dust[num41].fadeIn = num38 + num39);
							Main.dust[num41].velocity += vector3 * 1.5f;
						}
					}
					player3.NebulaLevelup(num35);
				}
				break;
			}
			case 103:
				if (Main.netMode == 1)
				{
					NPC.MoonLordCountdown = reader.ReadInt32();
				}
				break;
			case 104:
				if (Main.netMode == 1 && Main.npcShop > 0)
				{
					Item[] item3 = Main.instance.shop[Main.npcShop].item;
					int num21 = reader.ReadByte();
					int type = reader.ReadInt16();
					int stack2 = reader.ReadInt16();
					int pre2 = reader.ReadByte();
					int value = reader.ReadInt32();
					BitsByte bitsByte = reader.ReadByte();
					if (num21 < item3.Length)
					{
						item3[num21] = new Item();
						item3[num21].netDefaults(type);
						item3[num21].stack = stack2;
						item3[num21].Prefix(pre2);
						item3[num21].value = value;
						item3[num21].buyOnce = bitsByte[0];
					}
				}
				break;
			case 105:
				if (Main.netMode != 1)
				{
					short i2 = reader.ReadInt16();
					int j2 = reader.ReadInt16();
					bool on = reader.ReadBoolean();
					WorldGen.ToggleGemLock(i2, j2, on);
				}
				break;
			case 106:
				if (Main.netMode == 1)
				{
					HalfVector2 halfVector = default(HalfVector2);
					halfVector.PackedValue = reader.ReadUInt32();
					Utils.PoofOfSmoke(halfVector.ToVector2());
				}
				break;
			case 107:
				if (Main.netMode == 1)
				{
					Color c = reader.ReadRGB();
					string text = NetworkText.Deserialize(reader).ToString();
					int widthLimit = reader.ReadInt16();
					Main.NewTextMultiline(text, force: false, c, widthLimit);
				}
				break;
			case 108:
				if (Main.netMode == 1)
				{
					int damage3 = reader.ReadInt16();
					float knockBack2 = reader.ReadSingle();
					int x15 = reader.ReadInt16();
					int y14 = reader.ReadInt16();
					int angle = reader.ReadInt16();
					int ammo = reader.ReadInt16();
					int num267 = reader.ReadByte();
					if (num267 == Main.myPlayer)
					{
						WorldGen.ShootFromCannon(x15, y14, angle, ammo, damage3, knockBack2, num267);
					}
				}
				break;
			case 109:
				if (Main.netMode == 2)
				{
					short x13 = reader.ReadInt16();
					int y12 = reader.ReadInt16();
					int x14 = reader.ReadInt16();
					int y13 = reader.ReadInt16();
					byte toolMode = reader.ReadByte();
					int num266 = whoAmI;
					WiresUI.Settings.MultiToolMode toolMode2 = WiresUI.Settings.ToolMode;
					WiresUI.Settings.ToolMode = (WiresUI.Settings.MultiToolMode)toolMode;
					Wiring.MassWireOperation(new Point(x13, y12), new Point(x14, y13), Main.player[num266]);
					WiresUI.Settings.ToolMode = toolMode2;
				}
				break;
			case 110:
			{
				if (Main.netMode != 1)
				{
					break;
				}
				int type17 = reader.ReadInt16();
				int num253 = reader.ReadInt16();
				int num254 = reader.ReadByte();
				if (num254 == Main.myPlayer)
				{
					Player player15 = Main.player[num254];
					for (int num255 = 0; num255 < num253; num255++)
					{
						player15.ConsumeItem(type17);
					}
					player15.wireOperationsCooldown = 0;
				}
				break;
			}
			case 111:
				if (Main.netMode == 2)
				{
					BirthdayParty.ToggleManualParty();
				}
				break;
			case 112:
			{
				int num247 = reader.ReadByte();
				int num248 = reader.ReadInt32();
				int num249 = reader.ReadInt32();
				int num250 = reader.ReadByte();
				int num251 = reader.ReadInt16();
				switch (num247)
				{
				case 1:
					if (Main.netMode == 1)
					{
						WorldGen.TreeGrowFX(num248, num249, num250, num251);
					}
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(b, -1, -1, null, num247, num248, num249, num250, num251);
					}
					break;
				case 2:
					NPC.FairyEffects(new Vector2(num248, num249), num250);
					break;
				}
				break;
			}
			case 113:
			{
				int x12 = reader.ReadInt16();
				int y11 = reader.ReadInt16();
				if (Main.netMode == 2 && !Main.snowMoon && !Main.pumpkinMoon)
				{
					if (DD2Event.WouldFailSpawningHere(x12, y11))
					{
						DD2Event.FailureMessage(whoAmI);
					}
					DD2Event.SummonCrystal(x12, y11);
				}
				break;
			}
			case 114:
				if (Main.netMode == 1)
				{
					DD2Event.WipeEntities();
				}
				break;
			case 116:
				if (Main.netMode == 1)
				{
					DD2Event.TimeLeftBetweenWaves = reader.ReadInt32();
				}
				break;
			case 117:
			{
				int num209 = reader.ReadByte();
				if (Main.netMode != 2 || whoAmI == num209 || (Main.player[num209].hostile && Main.player[whoAmI].hostile))
				{
					PlayerDeathReason playerDeathReason2 = PlayerDeathReason.FromReader(reader);
					int damage2 = reader.ReadInt16();
					int num210 = reader.ReadByte() - 1;
					BitsByte bitsByte23 = reader.ReadByte();
					bool flag12 = bitsByte23[0];
					bool pvp2 = bitsByte23[1];
					int num211 = reader.ReadSByte();
					Main.player[num209].Hurt(playerDeathReason2, damage2, num210, pvp2, quiet: true, flag12, num211);
					if (Main.netMode == 2)
					{
						NetMessage.SendPlayerHurt(num209, playerDeathReason2, damage2, num210, flag12, pvp2, num211, -1, whoAmI);
					}
				}
				break;
			}
			case 118:
			{
				int num202 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num202 = whoAmI;
				}
				PlayerDeathReason playerDeathReason = PlayerDeathReason.FromReader(reader);
				int num203 = reader.ReadInt16();
				int num204 = reader.ReadByte() - 1;
				bool pvp = ((BitsByte)reader.ReadByte())[0];
				Main.player[num202].KillMe(playerDeathReason, num203, num204, pvp);
				if (Main.netMode == 2)
				{
					NetMessage.SendPlayerDeath(num202, playerDeathReason, num203, num204, pvp, -1, whoAmI);
				}
				break;
			}
			case 120:
			{
				int num197 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num197 = whoAmI;
				}
				int num198 = reader.ReadByte();
				if (num198 >= 0 && num198 < 145 && Main.netMode == 2)
				{
					EmoteBubble.NewBubble(num198, new WorldUIAnchor(Main.player[num197]), 360);
					EmoteBubble.CheckForNPCsToReactToEmoteBubble(num198, Main.player[num197]);
				}
				break;
			}
			case 121:
			{
				int num174 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num174 = whoAmI;
				}
				int num175 = reader.ReadInt32();
				int num176 = reader.ReadByte();
				bool flag10 = false;
				if (num176 >= 8)
				{
					flag10 = true;
					num176 -= 8;
				}
				if (!TileEntity.ByID.TryGetValue(num175, out var value9))
				{
					reader.ReadInt32();
					reader.ReadByte();
					break;
				}
				if (num176 >= 8)
				{
					value9 = null;
				}
				TEDisplayDoll tEDisplayDoll = value9 as TEDisplayDoll;
				if (tEDisplayDoll != null)
				{
					tEDisplayDoll.ReadItem(num176, reader, flag10);
				}
				else
				{
					reader.ReadInt32();
					reader.ReadByte();
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(b, -1, num174, null, num174, num175, num176, flag10.ToInt());
				}
				break;
			}
			case 122:
			{
				int num145 = reader.ReadInt32();
				int num146 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num146 = whoAmI;
				}
				if (Main.netMode == 2)
				{
					if (num145 == -1)
					{
						Main.player[num146].tileEntityAnchor.Clear();
						NetMessage.TrySendData(b, -1, -1, null, num145, num146);
						break;
					}
					if (!TileEntity.IsOccupied(num145, out var _) && TileEntity.ByID.TryGetValue(num145, out var value7))
					{
						Main.player[num146].tileEntityAnchor.Set(num145, value7.Position.X, value7.Position.Y);
						NetMessage.TrySendData(b, -1, -1, null, num145, num146);
					}
				}
				if (Main.netMode == 1)
				{
					TileEntity value8;
					if (num145 == -1)
					{
						Main.player[num146].tileEntityAnchor.Clear();
					}
					else if (TileEntity.ByID.TryGetValue(num145, out value8))
					{
						TileEntity.SetInteractionAnchor(Main.player[num146], value8.Position.X, value8.Position.Y, num145);
					}
				}
				break;
			}
			case 123:
				if (Main.netMode == 2)
				{
					short x7 = reader.ReadInt16();
					int y6 = reader.ReadInt16();
					int netid2 = reader.ReadInt16();
					int prefix2 = reader.ReadByte();
					int stack4 = reader.ReadInt16();
					TEWeaponsRack.TryPlacing(x7, y6, netid2, prefix2, stack4);
				}
				break;
			case 124:
			{
				int num129 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num129 = whoAmI;
				}
				int num130 = reader.ReadInt32();
				int num131 = reader.ReadByte();
				bool flag8 = false;
				if (num131 >= 2)
				{
					flag8 = true;
					num131 -= 2;
				}
				if (!TileEntity.ByID.TryGetValue(num130, out var value6))
				{
					reader.ReadInt32();
					reader.ReadByte();
					break;
				}
				if (num131 >= 2)
				{
					value6 = null;
				}
				TEHatRack tEHatRack = value6 as TEHatRack;
				if (tEHatRack != null)
				{
					tEHatRack.ReadItem(num131, reader, flag8);
				}
				else
				{
					reader.ReadInt32();
					reader.ReadByte();
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(b, -1, num129, null, num129, num130, num131, flag8.ToInt());
				}
				break;
			}
			case 125:
			{
				int num92 = reader.ReadByte();
				int num93 = reader.ReadInt16();
				int num94 = reader.ReadInt16();
				int num95 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num92 = whoAmI;
				}
				if (Main.netMode == 1)
				{
					Main.player[Main.myPlayer].GetOtherPlayersPickTile(num93, num94, num95);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(125, -1, num92, null, num92, num93, num94, num95);
				}
				break;
			}
			case 126:
				if (Main.netMode == 1)
				{
					NPC.RevengeManager.AddMarkerFromReader(reader);
				}
				break;
			case 127:
			{
				int markerUniqueID = reader.ReadInt32();
				if (Main.netMode == 1)
				{
					NPC.RevengeManager.DestroyMarker(markerUniqueID);
				}
				break;
			}
			case 128:
			{
				int num84 = reader.ReadByte();
				int num85 = reader.ReadUInt16();
				int num86 = reader.ReadUInt16();
				int num87 = reader.ReadUInt16();
				int num88 = reader.ReadUInt16();
				if (Main.netMode == 2)
				{
					NetMessage.SendData(128, -1, num84, null, num84, num87, num88, 0f, num85, num86);
				}
				else
				{
					GolfHelper.ContactListener.PutBallInCup_TextAndEffects(new Point(num85, num86), num84, num87, num88);
				}
				break;
			}
			case 129:
				if (Main.netMode == 1)
				{
					Main.FixUIScale();
					Main.TrySetPreparationState(Main.WorldPreparationState.ProcessingData);
				}
				break;
			case 130:
				if (Main.netMode == 2)
				{
					ushort num64 = reader.ReadUInt16();
					int num65 = reader.ReadUInt16();
					int type4 = reader.ReadInt16();
					int x3 = num64 * 16;
					num65 *= 16;
					NPC nPC3 = new NPC();
					spawnparams = default(NPCSpawnParams);
					nPC3.SetDefaults(type4, spawnparams);
					int type5 = nPC3.type;
					int netID = nPC3.netID;
					int num66 = NPC.NewNPC(x3, num65, type4);
					if (netID != type5)
					{
						NPC obj3 = Main.npc[num66];
						spawnparams = default(NPCSpawnParams);
						obj3.SetDefaults(netID, spawnparams);
						NetMessage.TrySendData(23, -1, -1, null, num66);
					}
				}
				break;
			case 131:
				if (Main.netMode == 1)
				{
					int num55 = reader.ReadUInt16();
					NPC nPC2 = null;
					nPC2 = ((num55 >= 200) ? new NPC() : Main.npc[num55]);
					int num56 = reader.ReadByte();
					if (num56 == 1)
					{
						int time = reader.ReadInt32();
						int fromWho = reader.ReadInt16();
						nPC2.GetImmuneTime(fromWho, time);
					}
				}
				break;
			case 132:
				if (Main.netMode == 1)
				{
					Point point = reader.ReadVector2().ToPoint();
					ushort key = reader.ReadUInt16();
					LegacySoundStyle legacySoundStyle = SoundID.SoundByIndex[key];
					BitsByte bitsByte2 = reader.ReadByte();
					int num24 = -1;
					float num25 = 1f;
					float num26 = 0f;
					SoundEngine.PlaySound(Style: (!bitsByte2[0]) ? legacySoundStyle.Style : reader.ReadInt32(), volumeScale: (!bitsByte2[1]) ? legacySoundStyle.Volume : MathHelper.Clamp(reader.ReadSingle(), 0f, 1f), pitchOffset: (!bitsByte2[2]) ? legacySoundStyle.GetRandomPitch() : MathHelper.Clamp(reader.ReadSingle(), -1f, 1f), type: legacySoundStyle.SoundId, x: point.X, y: point.Y);
				}
				break;
			case 133:
				if (Main.netMode == 2)
				{
					short x2 = reader.ReadInt16();
					int y2 = reader.ReadInt16();
					int netid = reader.ReadInt16();
					int prefix = reader.ReadByte();
					int stack3 = reader.ReadInt16();
					TEFoodPlatter.TryPlacing(x2, y2, netid, prefix, stack3);
				}
				break;
			case 134:
			{
				int num20 = reader.ReadByte();
				int ladyBugLuckTimeLeft = reader.ReadInt32();
				float torchLuck = reader.ReadSingle();
				byte luckPotion = reader.ReadByte();
				bool hasGardenGnomeNearby = reader.ReadBoolean();
				if (Main.netMode == 2)
				{
					num20 = whoAmI;
				}
				Player obj2 = Main.player[num20];
				obj2.ladyBugLuckTimeLeft = ladyBugLuckTimeLeft;
				obj2.torchLuck = torchLuck;
				obj2.luckPotion = luckPotion;
				obj2.HasGardenGnomeNearby = hasGardenGnomeNearby;
				obj2.RecalculateLuck();
				if (Main.netMode == 2)
				{
					NetMessage.SendData(134, -1, num20, null, num20);
				}
				break;
			}
			case 135:
			{
				int num19 = reader.ReadByte();
				if (Main.netMode == 1)
				{
					Main.player[num19].immuneAlpha = 255;
				}
				break;
			}
			case 136:
			{
				for (int k = 0; k < 2; k++)
				{
					for (int l = 0; l < 3; l++)
					{
						NPC.cavernMonsterType[k, l] = reader.ReadUInt16();
					}
				}
				break;
			}
			case 137:
				if (Main.netMode == 2)
				{
					int num9 = reader.ReadInt16();
					int buffTypeToRemove = reader.ReadUInt16();
					if (num9 >= 0 && num9 < 200)
					{
						Main.npc[num9].RequestBuffRemoval(buffTypeToRemove);
					}
				}
				break;
			case 139:
				if (Main.netMode != 2)
				{
					int num6 = reader.ReadByte();
					bool flag2 = reader.ReadBoolean();
					Main.countsAsHostForGameplay[num6] = flag2;
				}
				break;
			default:
				if (Netplay.Clients[whoAmI].State == 0)
				{
					NetMessage.BootPlayer(whoAmI, Lang.mp[2].ToNetworkText());
				}
				break;
			case 15:
			case 25:
			case 26:
			case 44:
			case 67:
			case 93:
				break;
			}
		}
	}
}
