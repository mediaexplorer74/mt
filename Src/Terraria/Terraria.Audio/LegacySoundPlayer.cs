using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Content;
using ReLogic.Utilities;
using GameManager.ID;

namespace GameManager.Audio
{
	public class LegacySoundPlayer
	{
		private Asset<SoundEffect>[] _soundDrip = new Asset<SoundEffect>[3];

		private SoundEffectInstance[] _soundInstanceDrip = new SoundEffectInstance[3];

		private Asset<SoundEffect>[] _soundLiquid = new Asset<SoundEffect>[2];

		private SoundEffectInstance[] _soundInstanceLiquid = new SoundEffectInstance[2];

		private Asset<SoundEffect>[] _soundMech = new Asset<SoundEffect>[1];

		private SoundEffectInstance[] _soundInstanceMech = new SoundEffectInstance[1];

		private Asset<SoundEffect>[] _soundDig = new Asset<SoundEffect>[3];

		private SoundEffectInstance[] _soundInstanceDig = new SoundEffectInstance[3];

		private Asset<SoundEffect>[] _soundThunder = new Asset<SoundEffect>[7];

		private SoundEffectInstance[] _soundInstanceThunder = new SoundEffectInstance[7];

		private Asset<SoundEffect>[] _soundResearch = new Asset<SoundEffect>[4];

		private SoundEffectInstance[] _soundInstanceResearch = new SoundEffectInstance[4];

		private Asset<SoundEffect>[] _soundTink = new Asset<SoundEffect>[3];

		private SoundEffectInstance[] _soundInstanceTink = new SoundEffectInstance[3];

		private Asset<SoundEffect>[] _soundCoin = new Asset<SoundEffect>[5];

		private SoundEffectInstance[] _soundInstanceCoin = new SoundEffectInstance[5];

		private Asset<SoundEffect>[] _soundPlayerHit = new Asset<SoundEffect>[3];

		private SoundEffectInstance[] _soundInstancePlayerHit = new SoundEffectInstance[3];

		private Asset<SoundEffect>[] _soundFemaleHit = new Asset<SoundEffect>[3];

		private SoundEffectInstance[] _soundInstanceFemaleHit = new SoundEffectInstance[3];

		private Asset<SoundEffect> _soundPlayerKilled;

		private SoundEffectInstance _soundInstancePlayerKilled;

		private Asset<SoundEffect> _soundGrass;

		private SoundEffectInstance _soundInstanceGrass;

		private Asset<SoundEffect> _soundGrab;

		private SoundEffectInstance _soundInstanceGrab;

		private Asset<SoundEffect> _soundPixie;

		private SoundEffectInstance _soundInstancePixie;

		private Asset<SoundEffect>[] _soundItem = new Asset<SoundEffect>[SoundID.ItemSoundCount];

		private SoundEffectInstance[] _soundInstanceItem = new SoundEffectInstance[SoundID.ItemSoundCount];

		private Asset<SoundEffect>[] _soundNpcHit = new Asset<SoundEffect>[58];

		private SoundEffectInstance[] _soundInstanceNpcHit = new SoundEffectInstance[58];

		private Asset<SoundEffect>[] _soundNpcKilled = new Asset<SoundEffect>[SoundID.NPCDeathCount];

		private SoundEffectInstance[] _soundInstanceNpcKilled = new SoundEffectInstance[SoundID.NPCDeathCount];

		private SoundEffectInstance _soundInstanceMoonlordCry;

		private Asset<SoundEffect> _soundDoorOpen;

		private SoundEffectInstance _soundInstanceDoorOpen;

		private Asset<SoundEffect> _soundDoorClosed;

		private SoundEffectInstance _soundInstanceDoorClosed;

		private Asset<SoundEffect> _soundMenuOpen;

		private SoundEffectInstance _soundInstanceMenuOpen;

		private Asset<SoundEffect> _soundMenuClose;

		private SoundEffectInstance _soundInstanceMenuClose;

		private Asset<SoundEffect> _soundMenuTick;

		private SoundEffectInstance _soundInstanceMenuTick;

		private Asset<SoundEffect> _soundShatter;

		private SoundEffectInstance _soundInstanceShatter;

		private Asset<SoundEffect> _soundCamera;

		private SoundEffectInstance _soundInstanceCamera;

		private Asset<SoundEffect>[] _soundZombie = new Asset<SoundEffect>[118];

		private SoundEffectInstance[] _soundInstanceZombie = new SoundEffectInstance[118];

		private Asset<SoundEffect>[] _soundRoar = new Asset<SoundEffect>[3];

		private SoundEffectInstance[] _soundInstanceRoar = new SoundEffectInstance[3];

		private Asset<SoundEffect>[] _soundSplash = new Asset<SoundEffect>[2];

		private SoundEffectInstance[] _soundInstanceSplash = new SoundEffectInstance[2];

		private Asset<SoundEffect> _soundDoubleJump;

		private SoundEffectInstance _soundInstanceDoubleJump;

		private Asset<SoundEffect> _soundRun;

		private SoundEffectInstance _soundInstanceRun;

		private Asset<SoundEffect> _soundCoins;

		private SoundEffectInstance _soundInstanceCoins;

		private Asset<SoundEffect> _soundUnlock;

		private SoundEffectInstance _soundInstanceUnlock;

		private Asset<SoundEffect> _soundChat;

		private SoundEffectInstance _soundInstanceChat;

		private Asset<SoundEffect> _soundMaxMana;

		private SoundEffectInstance _soundInstanceMaxMana;

		private Asset<SoundEffect> _soundDrown;

		private SoundEffectInstance _soundInstanceDrown;

		private Asset<SoundEffect>[] _trackableSounds;

		private SoundEffectInstance[] _trackableSoundInstances;

		private readonly IServiceProvider _services;

		public LegacySoundPlayer(IServiceProvider services)
		{
			_services = services;
			LoadAll();
		}

		private void LoadAll()
		{
			_soundMech[0] = Load("Sounds/Mech_0");
			_soundGrab = Load("Sounds/Grab");
			_soundPixie = Load("Sounds/Pixie");
			_soundDig[0] = Load("Sounds/Dig_0");
			_soundDig[1] = Load("Sounds/Dig_1");
			_soundDig[2] = Load("Sounds/Dig_2");
			_soundThunder[0] = Load("Sounds/Thunder_0");
			_soundThunder[1] = Load("Sounds/Thunder_1");
			_soundThunder[2] = Load("Sounds/Thunder_2");
			_soundThunder[3] = Load("Sounds/Thunder_3");
			_soundThunder[4] = Load("Sounds/Thunder_4");
			_soundThunder[5] = Load("Sounds/Thunder_5");
			_soundThunder[6] = Load("Sounds/Thunder_6");
			_soundResearch[0] = Load("Sounds/Research_0");
			_soundResearch[1] = Load("Sounds/Research_1");
			_soundResearch[2] = Load("Sounds/Research_2");
			_soundResearch[3] = Load("Sounds/Research_3");
			_soundTink[0] = Load("Sounds/Tink_0");
			_soundTink[1] = Load("Sounds/Tink_1");
			_soundTink[2] = Load("Sounds/Tink_2");
			_soundPlayerHit[0] = Load("Sounds/Player_Hit_0");
			_soundPlayerHit[1] = Load("Sounds/Player_Hit_1");
			_soundPlayerHit[2] = Load("Sounds/Player_Hit_2");
			_soundFemaleHit[0] = Load("Sounds/Female_Hit_0");
			_soundFemaleHit[1] = Load("Sounds/Female_Hit_1");
			_soundFemaleHit[2] = Load("Sounds/Female_Hit_2");
			_soundPlayerKilled = Load("Sounds/Player_Killed");
			_soundChat = Load("Sounds/Chat");
			_soundGrass = Load("Sounds/Grass");
			_soundDoorOpen = Load("Sounds/Door_Opened");
			_soundDoorClosed = Load("Sounds/Door_Closed");
			_soundMenuTick = Load("Sounds/Menu_Tick");
			_soundMenuOpen = Load("Sounds/Menu_Open");
			_soundMenuClose = Load("Sounds/Menu_Close");
			_soundShatter = Load("Sounds/Shatter");
			_soundCamera = Load("Sounds/Camera");
			for (int i = 0; i < _soundCoin.Length; i++)
			{
				_soundCoin[i] = Load("Sounds/Coin_" + i);
			}
			for (int j = 0; j < _soundDrip.Length; j++)
			{
				_soundDrip[j] = Load("Sounds/Drip_" + j);
			}
			for (int k = 0; k < _soundZombie.Length; k++)
			{
				_soundZombie[k] = Load("Sounds/Zombie_" + k);
			}
			for (int l = 0; l < _soundLiquid.Length; l++)
			{
				_soundLiquid[l] = Load("Sounds/Liquid_" + l);
			}
			for (int m = 0; m < _soundRoar.Length; m++)
			{
				_soundRoar[m] = Load("Sounds/Roar_" + m);
			}
			_soundSplash[0] = Load("Sounds/Splash_0");
			_soundSplash[1] = Load("Sounds/Splash_1");
			_soundDoubleJump = Load("Sounds/Double_Jump");
			_soundRun = Load("Sounds/Run");
			_soundCoins = Load("Sounds/Coins");
			_soundUnlock = Load("Sounds/Unlock");
			_soundMaxMana = Load("Sounds/MaxMana");
			_soundDrown = Load("Sounds/Drown");
			for (int n = 1; n < _soundItem.Length; n++)
			{
				_soundItem[n] = Load("Sounds/Item_" + n);
			}
			for (int num = 1; num < _soundNpcHit.Length; num++)
			{
				_soundNpcHit[num] = Load("Sounds/NPC_Hit_" + num);
			}
			for (int num2 = 1; num2 < _soundNpcKilled.Length; num2++)
			{
				_soundNpcKilled[num2] = Load("Sounds/NPC_Killed_" + num2);
			}
			_trackableSounds = new Asset<SoundEffect>[SoundID.TrackableLegacySoundCount];
			_trackableSoundInstances = new SoundEffectInstance[_trackableSounds.Length];
			for (int num3 = 0; num3 < _trackableSounds.Length; num3++)
			{
				_trackableSounds[num3] = Load("Sounds/Custom" + Path.DirectorySeparatorChar + SoundID.GetTrackableLegacySoundPath(num3));
			}
		}

		public void CreateAllSoundInstances()
		{
			_soundInstanceMech[0] = _soundMech[0].Value.CreateInstance();
			_soundInstanceGrab = _soundGrab.Value.CreateInstance();
			_soundInstancePixie = _soundGrab.Value.CreateInstance();
			_soundInstanceDig[0] = _soundDig[0].Value.CreateInstance();
			_soundInstanceDig[1] = _soundDig[1].Value.CreateInstance();
			_soundInstanceDig[2] = _soundDig[2].Value.CreateInstance();
			_soundInstanceTink[0] = _soundTink[0].Value.CreateInstance();
			_soundInstanceTink[1] = _soundTink[1].Value.CreateInstance();
			_soundInstanceTink[2] = _soundTink[2].Value.CreateInstance();
			_soundInstancePlayerHit[0] = _soundPlayerHit[0].Value.CreateInstance();
			_soundInstancePlayerHit[1] = _soundPlayerHit[1].Value.CreateInstance();
			_soundInstancePlayerHit[2] = _soundPlayerHit[2].Value.CreateInstance();
			_soundInstanceFemaleHit[0] = _soundFemaleHit[0].Value.CreateInstance();
			_soundInstanceFemaleHit[1] = _soundFemaleHit[1].Value.CreateInstance();
			_soundInstanceFemaleHit[2] = _soundFemaleHit[2].Value.CreateInstance();
			_soundInstancePlayerKilled = _soundPlayerKilled.Value.CreateInstance();
			_soundInstanceChat = _soundChat.Value.CreateInstance();
			_soundInstanceGrass = _soundGrass.Value.CreateInstance();
			_soundInstanceDoorOpen = _soundDoorOpen.Value.CreateInstance();
			_soundInstanceDoorClosed = _soundDoorClosed.Value.CreateInstance();
			_soundInstanceMenuTick = _soundMenuTick.Value.CreateInstance();
			_soundInstanceMenuOpen = _soundMenuOpen.Value.CreateInstance();
			_soundInstanceMenuClose = _soundMenuClose.Value.CreateInstance();
			_soundInstanceShatter = _soundShatter.Value.CreateInstance();
			_soundInstanceCamera = _soundCamera.Value.CreateInstance();
			for (int i = 0; i < _soundThunder.Length; i++)
			{
				_soundInstanceThunder[i] = _soundThunder[i].Value.CreateInstance();
			}
			for (int j = 0; j < _soundResearch.Length; j++)
			{
				_soundInstanceResearch[j] = _soundResearch[j].Value.CreateInstance();
			}
			for (int k = 0; k < _soundCoin.Length; k++)
			{
				_soundInstanceCoin[k] = _soundCoin[k].Value.CreateInstance();
			}
			for (int l = 0; l < _soundDrip.Length; l++)
			{
				_soundInstanceDrip[l] = _soundDrip[l].Value.CreateInstance();
			}
			for (int m = 0; m < _soundZombie.Length; m++)
			{
				_soundInstanceZombie[m] = _soundZombie[m].Value.CreateInstance();
			}
			for (int n = 0; n < _soundLiquid.Length; n++)
			{
				_soundInstanceLiquid[n] = _soundLiquid[n].Value.CreateInstance();
			}
			for (int num = 0; num < _soundRoar.Length; num++)
			{
				_soundInstanceRoar[num] = _soundRoar[num].Value.CreateInstance();
			}
			_soundInstanceSplash[0] = _soundRoar[0].Value.CreateInstance();
			_soundInstanceSplash[1] = _soundSplash[1].Value.CreateInstance();
			_soundInstanceDoubleJump = _soundRoar[0].Value.CreateInstance();
			_soundInstanceRun = _soundRun.Value.CreateInstance();
			_soundInstanceCoins = _soundCoins.Value.CreateInstance();
			_soundInstanceUnlock = _soundUnlock.Value.CreateInstance();
			_soundInstanceMaxMana = _soundMaxMana.Value.CreateInstance();
			_soundInstanceDrown = _soundDrown.Value.CreateInstance();
			for (int num2 = 1; num2 < _soundItem.Length; num2++)
			{
				_soundInstanceItem[num2] = _soundItem[num2].Value.CreateInstance();
			}
			for (int num3 = 1; num3 < _soundNpcHit.Length; num3++)
			{
				_soundInstanceNpcHit[num3] = _soundNpcHit[num3].Value.CreateInstance();
			}
			for (int num4 = 1; num4 < _soundNpcKilled.Length; num4++)
			{
				_soundInstanceNpcKilled[num4] = _soundNpcKilled[num4].Value.CreateInstance();
			}
			for (int num5 = 0; num5 < _trackableSounds.Length; num5++)
			{
				_trackableSoundInstances[num5] = _trackableSounds[num5].Value.CreateInstance();
			}
			_soundInstanceMoonlordCry = _soundNpcKilled[10].Value.CreateInstance();
		}

		private Asset<SoundEffect> Load(string assetName)
		{
			return XnaExtensions.Get<IAssetRepository>(_services).Request<SoundEffect>(assetName, Main.content, (AssetRequestMode)2);
		}

		public SoundEffectInstance PlaySound(int type, int x = -1, int y = -1, int Style = 1, float volumeScale = 1f, float pitchOffset = 0f)
		{
			int num = Style;
			try
			{
				if (Main.dedServ)
				{
					return null;
				}
				if (Main.soundVolume == 0f && (type < 30 || type > 35))
				{
					return null;
				}
				bool flag = false;
				float num2 = 1f;
				float num3 = 0f;
				if (x == -1 || y == -1)
				{
					flag = true;
				}
				else
				{
					if (WorldGen.gen)
					{
						return null;
					}
					if (Main.netMode == 2)
					{
						return null;
					}
					Vector2 vector = new Vector2(Main.screenPosition.X + (float)Main.screenWidth * 0.5f, Main.screenPosition.Y + (float)Main.screenHeight * 0.5f);
					float num4 = Math.Abs((float)x - vector.X);
					float num5 = Math.Abs((float)y - vector.Y);
					float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
					int num7 = 2500;
					if (num6 < (float)num7)
					{
						flag = true;
						num3 = ((type != 43) ? (((float)x - vector.X) / ((float)Main.screenWidth * 0.5f)) : (((float)x - vector.X) / 900f));
						num2 = 1f - num6 / (float)num7;
					}
				}
				if (num3 < -1f)
				{
					num3 = -1f;
				}
				if (num3 > 1f)
				{
					num3 = 1f;
				}
				if (num2 > 1f)
				{
					num2 = 1f;
				}
				if (num2 <= 0f && (type < 34 || type > 35 || type > 39))
				{
					return null;
				}
				if (flag)
				{
					if ((type >= 30 && type <= 35) || type == 39)
					{
						num2 *= Main.ambientVolume * (float)((!Main.gameInactive) ? 1 : 0);
						if (Main.gameMenu)
						{
							num2 = 0f;
						}
					}
					else
					{
						num2 *= Main.soundVolume;
					}
					if (num2 > 1f)
					{
						num2 = 1f;
					}
					if (num2 <= 0f && (type < 30 || type > 35) && type != 39)
					{
						return null;
					}
					SoundEffectInstance soundEffectInstance = null;
					switch (type)
					{
					case 0:
					{
						int num15 = Main.rand.Next(3);
						if (_soundInstanceDig[num15] != null)
						{
							_soundInstanceDig[num15].Stop();
						}
						_soundInstanceDig[num15] = _soundDig[num15].Value.CreateInstance();
						_soundInstanceDig[num15].Volume = num2;
						_soundInstanceDig[num15].Pan = num3;
						_soundInstanceDig[num15].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
						soundEffectInstance = _soundInstanceDig[num15];
						break;
					}
					case 43:
					{
						int num14 = Main.rand.Next(_soundThunder.Length);
						for (int j = 0; j < _soundThunder.Length; j++)
						{
							if (_soundInstanceThunder[num14] == null)
							{
								break;
							}
							if (_soundInstanceThunder[num14].State != 0)
							{
								break;
							}
							num14 = Main.rand.Next(_soundThunder.Length);
						}
						if (_soundInstanceThunder[num14] != null)
						{
							_soundInstanceThunder[num14].Stop();
						}
						_soundInstanceThunder[num14] = _soundThunder[num14].Value.CreateInstance();
						_soundInstanceThunder[num14].Volume = num2;
						_soundInstanceThunder[num14].Pan = num3;
						_soundInstanceThunder[num14].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
						soundEffectInstance = _soundInstanceThunder[num14];
						break;
					}
					case 63:
					{
						int num25 = Main.rand.Next(1, 4);
						if (_soundInstanceResearch[num25] != null)
						{
							_soundInstanceResearch[num25].Stop();
						}
						_soundInstanceResearch[num25] = _soundResearch[num25].Value.CreateInstance();
						_soundInstanceResearch[num25].Volume = num2;
						_soundInstanceResearch[num25].Pan = num3;
						soundEffectInstance = _soundInstanceResearch[num25];
						break;
					}
					case 64:
						if (_soundInstanceResearch[0] != null)
						{
							_soundInstanceResearch[0].Stop();
						}
						_soundInstanceResearch[0] = _soundResearch[0].Value.CreateInstance();
						_soundInstanceResearch[0].Volume = num2;
						_soundInstanceResearch[0].Pan = num3;
						soundEffectInstance = _soundInstanceResearch[0];
						break;
					case 1:
					{
						int num19 = Main.rand.Next(3);
						if (_soundInstancePlayerHit[num19] != null)
						{
							_soundInstancePlayerHit[num19].Stop();
						}
						_soundInstancePlayerHit[num19] = _soundPlayerHit[num19].Value.CreateInstance();
						_soundInstancePlayerHit[num19].Volume = num2;
						_soundInstancePlayerHit[num19].Pan = num3;
						soundEffectInstance = _soundInstancePlayerHit[num19];
						break;
					}
					case 2:
						if (num == 129)
						{
							num2 *= 0.6f;
						}
						if (num == 123)
						{
							num2 *= 0.5f;
						}
						if (num == 124 || num == 125)
						{
							num2 *= 0.65f;
						}
						if (num == 116)
						{
							num2 *= 0.5f;
						}
						switch (num)
						{
						case 1:
						{
							int num17 = Main.rand.Next(3);
							if (num17 == 1)
							{
								num = 18;
							}
							if (num17 == 2)
							{
								num = 19;
							}
							break;
						}
						case 53:
						case 55:
							num2 *= 0.75f;
							if (num == 55)
							{
								num2 *= 0.75f;
							}
							if (_soundInstanceItem[num] != null && _soundInstanceItem[num].State == SoundState.Playing)
							{
								return null;
							}
							break;
						case 37:
							num2 *= 0.5f;
							break;
						case 52:
							num2 *= 0.35f;
							break;
						case 157:
							num2 *= 0.7f;
							break;
						case 158:
							num2 *= 0.8f;
							break;
						}
						switch (num)
						{
						case 159:
							if (_soundInstanceItem[num] != null && _soundInstanceItem[num].State == SoundState.Playing)
							{
								return null;
							}
							num2 *= 0.75f;
							break;
						default:
							if (_soundInstanceItem[num] != null)
							{
								_soundInstanceItem[num].Stop();
							}
							break;
						case 9:
						case 10:
						case 24:
						case 26:
						case 34:
						case 43:
						case 103:
						case 156:
						case 162:
							break;
						}
						_soundInstanceItem[num] = _soundItem[num].Value.CreateInstance();
						_soundInstanceItem[num].Volume = num2;
						_soundInstanceItem[num].Pan = num3;
						switch (num)
						{
						case 53:
							_soundInstanceItem[num].Pitch = (float)Main.rand.Next(-20, -11) * 0.02f;
							break;
						case 55:
							_soundInstanceItem[num].Pitch = (float)(-Main.rand.Next(-20, -11)) * 0.02f;
							break;
						case 132:
							_soundInstanceItem[num].Pitch = (float)Main.rand.Next(-20, 21) * 0.001f;
							break;
						case 153:
							_soundInstanceItem[num].Pitch = (float)Main.rand.Next(-50, 51) * 0.003f;
							break;
						case 156:
							_soundInstanceItem[num].Pitch = (float)Main.rand.Next(-50, 51) * 0.002f;
							_soundInstanceItem[num].Volume *= 0.6f;
							break;
						default:
							_soundInstanceItem[num].Pitch = (float)Main.rand.Next(-6, 7) * 0.01f;
							break;
						}
						if (num == 26 || num == 35 || num == 47)
						{
							_soundInstanceItem[num].Volume = num2 * 0.75f;
							_soundInstanceItem[num].Pitch = Main.musicPitch;
						}
						if (num == 169)
						{
							_soundInstanceItem[num].Pitch -= 0.8f;
						}
						soundEffectInstance = _soundInstanceItem[num];
						break;
					case 3:
						if (num >= 20 && num <= 54)
						{
							num2 *= 0.5f;
						}
						if (num == 57 && _soundInstanceNpcHit[num] != null && _soundInstanceNpcHit[num].State == SoundState.Playing)
						{
							return null;
						}
						if (num == 57)
						{
							num2 *= 0.6f;
						}
						if (num == 55 || num == 56)
						{
							num2 *= 0.5f;
						}
						if (_soundInstanceNpcHit[num] != null)
						{
							_soundInstanceNpcHit[num].Stop();
						}
						_soundInstanceNpcHit[num] = _soundNpcHit[num].Value.CreateInstance();
						_soundInstanceNpcHit[num].Volume = num2;
						_soundInstanceNpcHit[num].Pan = num3;
						_soundInstanceNpcHit[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
						soundEffectInstance = _soundInstanceNpcHit[num];
						break;
					case 4:
						if (num >= 23 && num <= 57)
						{
							num2 *= 0.5f;
						}
						if (num == 61)
						{
							num2 *= 0.6f;
						}
						if (num == 62)
						{
							num2 *= 0.6f;
						}
						if (num == 10 && _soundInstanceNpcKilled[num] != null && _soundInstanceNpcKilled[num].State == SoundState.Playing)
						{
							return null;
						}
						_soundInstanceNpcKilled[num] = _soundNpcKilled[num].Value.CreateInstance();
						_soundInstanceNpcKilled[num].Volume = num2;
						_soundInstanceNpcKilled[num].Pan = num3;
						_soundInstanceNpcKilled[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
						soundEffectInstance = _soundInstanceNpcKilled[num];
						break;
					case 5:
						if (_soundInstancePlayerKilled != null)
						{
							_soundInstancePlayerKilled.Stop();
						}
						_soundInstancePlayerKilled = _soundPlayerKilled.Value.CreateInstance();
						_soundInstancePlayerKilled.Volume = num2;
						_soundInstancePlayerKilled.Pan = num3;
						soundEffectInstance = _soundInstancePlayerKilled;
						break;
					case 6:
						if (_soundInstanceGrass != null)
						{
							_soundInstanceGrass.Stop();
						}
						_soundInstanceGrass = _soundGrass.Value.CreateInstance();
						_soundInstanceGrass.Volume = num2;
						_soundInstanceGrass.Pan = num3;
						_soundInstanceGrass.Pitch = (float)Main.rand.Next(-30, 31) * 0.01f;
						soundEffectInstance = _soundInstanceGrass;
						break;
					case 7:
						if (_soundInstanceGrab != null)
						{
							_soundInstanceGrab.Stop();
						}
						_soundInstanceGrab = _soundGrab.Value.CreateInstance();
						_soundInstanceGrab.Volume = num2;
						_soundInstanceGrab.Pan = num3;
						_soundInstanceGrab.Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
						soundEffectInstance = _soundInstanceGrab;
						break;
					case 8:
						if (_soundInstanceDoorOpen != null)
						{
							_soundInstanceDoorOpen.Stop();
						}
						_soundInstanceDoorOpen = _soundDoorOpen.Value.CreateInstance();
						_soundInstanceDoorOpen.Volume = num2;
						_soundInstanceDoorOpen.Pan = num3;
						_soundInstanceDoorOpen.Pitch = (float)Main.rand.Next(-20, 21) * 0.01f;
						soundEffectInstance = _soundInstanceDoorOpen;
						break;
					case 9:
						if (_soundInstanceDoorClosed != null)
						{
							_soundInstanceDoorClosed.Stop();
						}
						_soundInstanceDoorClosed = _soundDoorClosed.Value.CreateInstance();
						_soundInstanceDoorClosed.Volume = num2;
						_soundInstanceDoorClosed.Pan = num3;
						_soundInstanceDoorClosed.Pitch = (float)Main.rand.Next(-20, 21) * 0.01f;
						soundEffectInstance = _soundInstanceDoorClosed;
						break;
					case 10:
						if (_soundInstanceMenuOpen != null)
						{
							_soundInstanceMenuOpen.Stop();
						}
						_soundInstanceMenuOpen = _soundMenuOpen.Value.CreateInstance();
						_soundInstanceMenuOpen.Volume = num2;
						_soundInstanceMenuOpen.Pan = num3;
						soundEffectInstance = _soundInstanceMenuOpen;
						break;
					case 11:
						if (_soundInstanceMenuClose != null)
						{
							_soundInstanceMenuClose.Stop();
						}
						_soundInstanceMenuClose = _soundMenuClose.Value.CreateInstance();
						_soundInstanceMenuClose.Volume = num2;
						_soundInstanceMenuClose.Pan = num3;
						soundEffectInstance = _soundInstanceMenuClose;
						break;
					case 12:
						if (Main.hasFocus)
						{
							if (_soundInstanceMenuTick != null)
							{
								_soundInstanceMenuTick.Stop();
							}
							_soundInstanceMenuTick = _soundMenuTick.Value.CreateInstance();
							_soundInstanceMenuTick.Volume = num2;
							_soundInstanceMenuTick.Pan = num3;
							soundEffectInstance = _soundInstanceMenuTick;
						}
						break;
					case 13:
						if (_soundInstanceShatter != null)
						{
							_soundInstanceShatter.Stop();
						}
						_soundInstanceShatter = _soundShatter.Value.CreateInstance();
						_soundInstanceShatter.Volume = num2;
						_soundInstanceShatter.Pan = num3;
						soundEffectInstance = _soundInstanceShatter;
						break;
					case 14:
						switch (Style)
						{
						case 542:
						{
							int num22 = 7;
							_soundInstanceZombie[num22] = _soundZombie[num22].Value.CreateInstance();
							_soundInstanceZombie[num22].Volume = num2 * 0.4f;
							_soundInstanceZombie[num22].Pan = num3;
							soundEffectInstance = _soundInstanceZombie[num22];
							break;
						}
						case 489:
						case 586:
						{
							int num21 = Main.rand.Next(21, 24);
							_soundInstanceZombie[num21] = _soundZombie[num21].Value.CreateInstance();
							_soundInstanceZombie[num21].Volume = num2 * 0.4f;
							_soundInstanceZombie[num21].Pan = num3;
							soundEffectInstance = _soundInstanceZombie[num21];
							break;
						}
						default:
						{
							int num20 = Main.rand.Next(3);
							_soundInstanceZombie[num20] = _soundZombie[num20].Value.CreateInstance();
							_soundInstanceZombie[num20].Volume = num2 * 0.4f;
							_soundInstanceZombie[num20].Pan = num3;
							soundEffectInstance = _soundInstanceZombie[num20];
							break;
						}
						}
						break;
					case 15:
					{
						float num16 = 1f;
						if (num == 4)
						{
							num = 1;
							num16 = 0.25f;
						}
						if (_soundInstanceRoar[num] == null || _soundInstanceRoar[num].State == SoundState.Stopped)
						{
							_soundInstanceRoar[num] = _soundRoar[num].Value.CreateInstance();
							_soundInstanceRoar[num].Volume = num2 * num16;
							_soundInstanceRoar[num].Pan = num3;
							soundEffectInstance = _soundInstanceRoar[num];
						}
						break;
					}
					case 16:
						if (_soundInstanceDoubleJump != null)
						{
							_soundInstanceDoubleJump.Stop();
						}
						_soundInstanceDoubleJump = _soundDoubleJump.Value.CreateInstance();
						_soundInstanceDoubleJump.Volume = num2;
						_soundInstanceDoubleJump.Pan = num3;
						_soundInstanceDoubleJump.Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
						soundEffectInstance = _soundInstanceDoubleJump;
						break;
					case 17:
						if (_soundInstanceRun != null)
						{
							_soundInstanceRun.Stop();
						}
						_soundInstanceRun = _soundRun.Value.CreateInstance();
						_soundInstanceRun.Volume = num2;
						_soundInstanceRun.Pan = num3;
						_soundInstanceRun.Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
						soundEffectInstance = _soundInstanceRun;
						break;
					case 18:
						_soundInstanceCoins = _soundCoins.Value.CreateInstance();
						_soundInstanceCoins.Volume = num2;
						_soundInstanceCoins.Pan = num3;
						soundEffectInstance = _soundInstanceCoins;
						break;
					case 19:
						if (_soundInstanceSplash[num] == null || _soundInstanceSplash[num].State == SoundState.Stopped)
						{
							_soundInstanceSplash[num] = _soundSplash[num].Value.CreateInstance();
							_soundInstanceSplash[num].Volume = num2;
							_soundInstanceSplash[num].Pan = num3;
							_soundInstanceSplash[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
							soundEffectInstance = _soundInstanceSplash[num];
						}
						break;
					case 20:
					{
						int num24 = Main.rand.Next(3);
						if (_soundInstanceFemaleHit[num24] != null)
						{
							_soundInstanceFemaleHit[num24].Stop();
						}
						_soundInstanceFemaleHit[num24] = _soundFemaleHit[num24].Value.CreateInstance();
						_soundInstanceFemaleHit[num24].Volume = num2;
						_soundInstanceFemaleHit[num24].Pan = num3;
						soundEffectInstance = _soundInstanceFemaleHit[num24];
						break;
					}
					case 21:
					{
						int num23 = Main.rand.Next(3);
						if (_soundInstanceTink[num23] != null)
						{
							_soundInstanceTink[num23].Stop();
						}
						_soundInstanceTink[num23] = _soundTink[num23].Value.CreateInstance();
						_soundInstanceTink[num23].Volume = num2;
						_soundInstanceTink[num23].Pan = num3;
						soundEffectInstance = _soundInstanceTink[num23];
						break;
					}
					case 22:
						if (_soundInstanceUnlock != null)
						{
							_soundInstanceUnlock.Stop();
						}
						_soundInstanceUnlock = _soundUnlock.Value.CreateInstance();
						_soundInstanceUnlock.Volume = num2;
						_soundInstanceUnlock.Pan = num3;
						soundEffectInstance = _soundInstanceUnlock;
						break;
					case 23:
						if (_soundInstanceDrown != null)
						{
							_soundInstanceDrown.Stop();
						}
						_soundInstanceDrown = _soundDrown.Value.CreateInstance();
						_soundInstanceDrown.Volume = num2;
						_soundInstanceDrown.Pan = num3;
						soundEffectInstance = _soundInstanceDrown;
						break;
					case 24:
						_soundInstanceChat = _soundChat.Value.CreateInstance();
						_soundInstanceChat.Volume = num2;
						_soundInstanceChat.Pan = num3;
						soundEffectInstance = _soundInstanceChat;
						break;
					case 25:
						_soundInstanceMaxMana = _soundMaxMana.Value.CreateInstance();
						_soundInstanceMaxMana.Volume = num2;
						_soundInstanceMaxMana.Pan = num3;
						soundEffectInstance = _soundInstanceMaxMana;
						break;
					case 26:
					{
						int num18 = Main.rand.Next(3, 5);
						_soundInstanceZombie[num18] = _soundZombie[num18].Value.CreateInstance();
						_soundInstanceZombie[num18].Volume = num2 * 0.9f;
						_soundInstanceZombie[num18].Pan = num3;
						_soundInstanceZombie[num18].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
						soundEffectInstance = _soundInstanceZombie[num18];
						break;
					}
					case 27:
						if (_soundInstancePixie != null && _soundInstancePixie.State == SoundState.Playing)
						{
							_soundInstancePixie.Volume = num2;
							_soundInstancePixie.Pan = num3;
							_soundInstancePixie.Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
							return null;
						}
						if (_soundInstancePixie != null)
						{
							_soundInstancePixie.Stop();
						}
						_soundInstancePixie = _soundPixie.Value.CreateInstance();
						_soundInstancePixie.Volume = num2;
						_soundInstancePixie.Pan = num3;
						_soundInstancePixie.Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
						soundEffectInstance = _soundInstancePixie;
						break;
					case 28:
						if (_soundInstanceMech[num] != null && _soundInstanceMech[num].State == SoundState.Playing)
						{
							return null;
						}
						_soundInstanceMech[num] = _soundMech[num].Value.CreateInstance();
						_soundInstanceMech[num].Volume = num2;
						_soundInstanceMech[num].Pan = num3;
						_soundInstanceMech[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
						soundEffectInstance = _soundInstanceMech[num];
						break;
					case 29:
						if (num >= 24 && num <= 87)
						{
							num2 *= 0.5f;
						}
						if (num >= 88 && num <= 91)
						{
							num2 *= 0.7f;
						}
						if (num >= 93 && num <= 99)
						{
							num2 *= 0.4f;
						}
						if (num == 92)
						{
							num2 *= 0.5f;
						}
						if (num == 103)
						{
							num2 *= 0.4f;
						}
						if (num == 104)
						{
							num2 *= 0.55f;
						}
						if (num == 100 || num == 101)
						{
							num2 *= 0.25f;
						}
						if (num == 102)
						{
							num2 *= 0.4f;
						}
						if (_soundInstanceZombie[num] != null && _soundInstanceZombie[num].State == SoundState.Playing)
						{
							return null;
						}
						_soundInstanceZombie[num] = _soundZombie[num].Value.CreateInstance();
						_soundInstanceZombie[num].Volume = num2;
						_soundInstanceZombie[num].Pan = num3;
						_soundInstanceZombie[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
						soundEffectInstance = _soundInstanceZombie[num];
						break;
					case 44:
						num = Main.rand.Next(106, 109);
						_soundInstanceZombie[num] = _soundZombie[num].Value.CreateInstance();
						_soundInstanceZombie[num].Volume = num2 * 0.2f;
						_soundInstanceZombie[num].Pan = num3;
						_soundInstanceZombie[num].Pitch = (float)Main.rand.Next(-70, 1) * 0.01f;
						soundEffectInstance = _soundInstanceZombie[num];
						break;
					case 45:
						num = 109;
						if (_soundInstanceZombie[num] != null && _soundInstanceZombie[num].State == SoundState.Playing)
						{
							return null;
						}
						_soundInstanceZombie[num] = _soundZombie[num].Value.CreateInstance();
						_soundInstanceZombie[num].Volume = num2 * 0.3f;
						_soundInstanceZombie[num].Pan = num3;
						_soundInstanceZombie[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
						soundEffectInstance = _soundInstanceZombie[num];
						break;
					case 46:
						if (_soundInstanceZombie[110] != null && _soundInstanceZombie[110].State == SoundState.Playing)
						{
							return null;
						}
						if (_soundInstanceZombie[111] != null && _soundInstanceZombie[111].State == SoundState.Playing)
						{
							return null;
						}
						num = Main.rand.Next(110, 112);
						if (Main.rand.Next(300) == 0)
						{
							num = ((Main.rand.Next(3) == 0) ? 114 : ((Main.rand.Next(2) != 0) ? 112 : 113));
						}
						_soundInstanceZombie[num] = _soundZombie[num].Value.CreateInstance();
						_soundInstanceZombie[num].Volume = num2 * 0.9f;
						_soundInstanceZombie[num].Pan = num3;
						_soundInstanceZombie[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
						soundEffectInstance = _soundInstanceZombie[num];
						break;
					default:
						switch (type)
						{
						case 45:
							num = 109;
							_soundInstanceZombie[num] = _soundZombie[num].Value.CreateInstance();
							_soundInstanceZombie[num].Volume = num2 * 0.2f;
							_soundInstanceZombie[num].Pan = num3;
							_soundInstanceZombie[num].Pitch = (float)Main.rand.Next(-70, 1) * 0.01f;
							soundEffectInstance = _soundInstanceZombie[num];
							break;
						case 30:
							num = Main.rand.Next(10, 12);
							if (Main.rand.Next(300) == 0)
							{
								num = 12;
								if (_soundInstanceZombie[num] != null && _soundInstanceZombie[num].State == SoundState.Playing)
								{
									return null;
								}
							}
							_soundInstanceZombie[num] = _soundZombie[num].Value.CreateInstance();
							_soundInstanceZombie[num].Volume = num2 * 0.75f;
							_soundInstanceZombie[num].Pan = num3;
							if (num != 12)
							{
								_soundInstanceZombie[num].Pitch = (float)Main.rand.Next(-70, 1) * 0.01f;
							}
							else
							{
								_soundInstanceZombie[num].Pitch = (float)Main.rand.Next(-40, 21) * 0.01f;
							}
							soundEffectInstance = _soundInstanceZombie[num];
							break;
						case 31:
							num = 13;
							_soundInstanceZombie[num] = _soundZombie[num].Value.CreateInstance();
							_soundInstanceZombie[num].Volume = num2 * 0.35f;
							_soundInstanceZombie[num].Pan = num3;
							_soundInstanceZombie[num].Pitch = (float)Main.rand.Next(-40, 21) * 0.01f;
							soundEffectInstance = _soundInstanceZombie[num];
							break;
						case 32:
							if (_soundInstanceZombie[num] != null && _soundInstanceZombie[num].State == SoundState.Playing)
							{
								return null;
							}
							_soundInstanceZombie[num] = _soundZombie[num].Value.CreateInstance();
							_soundInstanceZombie[num].Volume = num2 * 0.15f;
							_soundInstanceZombie[num].Pan = num3;
							_soundInstanceZombie[num].Pitch = (float)Main.rand.Next(-70, 26) * 0.01f;
							soundEffectInstance = _soundInstanceZombie[num];
							break;
						case 33:
							num = 15;
							if (_soundInstanceZombie[num] != null && _soundInstanceZombie[num].State == SoundState.Playing)
							{
								return null;
							}
							_soundInstanceZombie[num] = _soundZombie[num].Value.CreateInstance();
							_soundInstanceZombie[num].Volume = num2 * 0.2f;
							_soundInstanceZombie[num].Pan = num3;
							_soundInstanceZombie[num].Pitch = (float)Main.rand.Next(-10, 31) * 0.01f;
							soundEffectInstance = _soundInstanceZombie[num];
							break;
						case 47:
						case 48:
						case 49:
						case 50:
						case 51:
						case 52:
						{
							num = 133 + type - 47;
							for (int i = 133; i <= 138; i++)
							{
								if (_soundInstanceItem[i] != null && _soundInstanceItem[i].State == SoundState.Playing)
								{
									_soundInstanceItem[i].Stop();
								}
							}
							_soundInstanceItem[num] = _soundItem[num].Value.CreateInstance();
							_soundInstanceItem[num].Volume = num2 * 0.45f;
							_soundInstanceItem[num].Pan = num3;
							soundEffectInstance = _soundInstanceItem[num];
							break;
						}
						default:
							if (type >= 53 && type <= 62)
							{
								num = 139 + type - 53;
								if (_soundInstanceItem[num] != null && _soundInstanceItem[num].State == SoundState.Playing)
								{
									_soundInstanceItem[num].Stop();
								}
								_soundInstanceItem[num] = _soundItem[num].Value.CreateInstance();
								_soundInstanceItem[num].Volume = num2 * 0.7f;
								_soundInstanceItem[num].Pan = num3;
								soundEffectInstance = _soundInstanceItem[num];
								break;
							}
							switch (type)
							{
							case 34:
							{
								float num10 = (float)num / 50f;
								if (num10 > 1f)
								{
									num10 = 1f;
								}
								num2 *= num10;
								num2 *= 0.2f;
								if (num2 <= 0f || x == -1 || y == -1)
								{
									if (_soundInstanceLiquid[0] != null && _soundInstanceLiquid[0].State == SoundState.Playing)
									{
										_soundInstanceLiquid[0].Stop();
									}
								}
								else if (_soundInstanceLiquid[0] != null && _soundInstanceLiquid[0].State == SoundState.Playing)
								{
									_soundInstanceLiquid[0].Volume = num2;
									_soundInstanceLiquid[0].Pan = num3;
									_soundInstanceLiquid[0].Pitch = -0.2f;
								}
								else
								{
									_soundInstanceLiquid[0] = _soundLiquid[0].Value.CreateInstance();
									_soundInstanceLiquid[0].Volume = num2;
									_soundInstanceLiquid[0].Pan = num3;
									soundEffectInstance = _soundInstanceLiquid[0];
								}
								break;
							}
							case 35:
							{
								float num9 = (float)num / 50f;
								if (num9 > 1f)
								{
									num9 = 1f;
								}
								num2 *= num9;
								num2 *= 0.65f;
								if (num2 <= 0f || x == -1 || y == -1)
								{
									if (_soundInstanceLiquid[1] != null && _soundInstanceLiquid[1].State == SoundState.Playing)
									{
										_soundInstanceLiquid[1].Stop();
									}
								}
								else if (_soundInstanceLiquid[1] != null && _soundInstanceLiquid[1].State == SoundState.Playing)
								{
									_soundInstanceLiquid[1].Volume = num2;
									_soundInstanceLiquid[1].Pan = num3;
									_soundInstanceLiquid[1].Pitch = -0f;
								}
								else
								{
									_soundInstanceLiquid[1] = _soundLiquid[1].Value.CreateInstance();
									_soundInstanceLiquid[1].Volume = num2;
									_soundInstanceLiquid[1].Pan = num3;
									soundEffectInstance = _soundInstanceLiquid[1];
								}
								break;
							}
							case 36:
							{
								int num12 = Style;
								if (Style == -1)
								{
									num12 = 0;
								}
								_soundInstanceRoar[num12] = _soundRoar[num12].Value.CreateInstance();
								_soundInstanceRoar[num12].Volume = num2;
								_soundInstanceRoar[num12].Pan = num3;
								if (Style == -1)
								{
									_soundInstanceRoar[num12].Pitch += 0.6f;
								}
								soundEffectInstance = _soundInstanceRoar[num12];
								break;
							}
							case 37:
							{
								int num11 = Main.rand.Next(57, 59);
								num2 *= (float)Style * 0.05f;
								_soundInstanceItem[num11] = _soundItem[num11].Value.CreateInstance();
								_soundInstanceItem[num11].Volume = num2;
								_soundInstanceItem[num11].Pan = num3;
								_soundInstanceItem[num11].Pitch = (float)Main.rand.Next(-40, 41) * 0.01f;
								soundEffectInstance = _soundInstanceItem[num11];
								break;
							}
							case 38:
							{
								int num13 = Main.rand.Next(5);
								_soundInstanceCoin[num13] = _soundCoin[num13].Value.CreateInstance();
								_soundInstanceCoin[num13].Volume = num2;
								_soundInstanceCoin[num13].Pan = num3;
								_soundInstanceCoin[num13].Pitch = (float)Main.rand.Next(-40, 41) * 0.002f;
								soundEffectInstance = _soundInstanceCoin[num13];
								break;
							}
							case 39:
								num = Style;
								_soundInstanceDrip[num] = _soundDrip[num].Value.CreateInstance();
								_soundInstanceDrip[num].Volume = num2 * 0.5f;
								_soundInstanceDrip[num].Pan = num3;
								_soundInstanceDrip[num].Pitch = (float)Main.rand.Next(-30, 31) * 0.01f;
								soundEffectInstance = _soundInstanceDrip[num];
								break;
							case 40:
								if (_soundInstanceCamera != null)
								{
									_soundInstanceCamera.Stop();
								}
								_soundInstanceCamera = _soundCamera.Value.CreateInstance();
								_soundInstanceCamera.Volume = num2;
								_soundInstanceCamera.Pan = num3;
								soundEffectInstance = _soundInstanceCamera;
								break;
							case 41:
								_soundInstanceMoonlordCry = _soundNpcKilled[10].Value.CreateInstance();
								_soundInstanceMoonlordCry.Volume = 1f / (1f + (new Vector2(x, y) - Main.player[Main.myPlayer].position).Length());
								_soundInstanceMoonlordCry.Pan = num3;
								_soundInstanceMoonlordCry.Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
								soundEffectInstance = _soundInstanceMoonlordCry;
								break;
							case 42:
								soundEffectInstance = _trackableSounds[num].Value.CreateInstance();
								soundEffectInstance.Volume = num2;
								soundEffectInstance.Pan = num3;
								_trackableSoundInstances[num] = soundEffectInstance;
								break;
							case 65:
							{
								if (_soundInstanceZombie[115] != null && _soundInstanceZombie[115].State == SoundState.Playing)
								{
									return null;
								}
								if (_soundInstanceZombie[116] != null && _soundInstanceZombie[116].State == SoundState.Playing)
								{
									return null;
								}
								if (_soundInstanceZombie[117] != null && _soundInstanceZombie[117].State == SoundState.Playing)
								{
									return null;
								}
								int num8 = Main.rand.Next(115, 118);
								_soundInstanceZombie[num8] = _soundZombie[num8].Value.CreateInstance();
								_soundInstanceZombie[num8].Volume = num2 * 0.5f;
								_soundInstanceZombie[num8].Pan = num3;
								soundEffectInstance = _soundInstanceZombie[num8];
								break;
							}
							}
							break;
						}
						break;
					}
					if (soundEffectInstance != null)
					{
						soundEffectInstance.Pitch += pitchOffset;
						soundEffectInstance.Volume *= volumeScale;
						soundEffectInstance.Play();
						SoundInstanceGarbageCollector.Track(soundEffectInstance);
					}
					return soundEffectInstance;
				}
			}
			catch
			{
			}
			return null;
		}

		public SoundEffect GetTrackableSoundByStyleId(int id)
		{
			return _trackableSounds[id].Value;
		}

		public void StopAmbientSounds()
		{
			for (int i = 0; i < _soundInstanceLiquid.Length; i++)
			{
				if (_soundInstanceLiquid[i] != null)
				{
					_soundInstanceLiquid[i].Stop();
				}
			}
		}
	}
}
