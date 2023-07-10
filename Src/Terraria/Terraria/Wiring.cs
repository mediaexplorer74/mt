using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameManager.Audio;
using GameManager.DataStructures;
using GameManager.GameContent.Events;
using GameManager.GameContent.UI;
using GameManager.ID;
using GameManager.Localization;

namespace GameManager
{
	public static class Wiring
	{
		public static bool blockPlayerTeleportationForOneIteration;

		public static bool running;

		private static Dictionary<Point16, bool> _wireSkip;

		private static DoubleStack<Point16> _wireList;

		private static DoubleStack<byte> _wireDirectionList;

		private static Dictionary<Point16, byte> _toProcess;

		private static Queue<Point16> _GatesCurrent;

		private static Queue<Point16> _LampsToCheck;

		private static Queue<Point16> _GatesNext;

		private static Dictionary<Point16, bool> _GatesDone;

		private static Dictionary<Point16, byte> _PixelBoxTriggers;

		private static Vector2[] _teleport;

		private const int MaxPump = 20;

		private static int[] _inPumpX;

		private static int[] _inPumpY;

		private static int _numInPump;

		private static int[] _outPumpX;

		private static int[] _outPumpY;

		private static int _numOutPump;

		private const int MaxMech = 1000;

		private static int[] _mechX;

		private static int[] _mechY;

		private static int _numMechs;

		private static int[] _mechTime;

		private static int _currentWireColor;

		private static int CurrentUser = 255;

		public static void SetCurrentUser(int plr = -1)
		{
			if (plr < 0 || plr > 255)
			{
				plr = 255;
			}
			if (Main.netMode == 0)
			{
				plr = Main.myPlayer;
			}
			CurrentUser = plr;
		}

		public static void Initialize()
		{
			_wireSkip = new Dictionary<Point16, bool>();
			_wireList = new DoubleStack<Point16>();
			_wireDirectionList = new DoubleStack<byte>();
			_toProcess = new Dictionary<Point16, byte>();
			_GatesCurrent = new Queue<Point16>();
			_GatesNext = new Queue<Point16>();
			_GatesDone = new Dictionary<Point16, bool>();
			_LampsToCheck = new Queue<Point16>();
			_PixelBoxTriggers = new Dictionary<Point16, byte>();
			_inPumpX = new int[20];
			_inPumpY = new int[20];
			_outPumpX = new int[20];
			_outPumpY = new int[20];
			_teleport = new Vector2[2];
			_mechX = new int[1000];
			_mechY = new int[1000];
			_mechTime = new int[1000];
		}

		public static void SkipWire(int x, int y)
		{
			_wireSkip[new Point16(x, y)] = true;
		}

		public static void SkipWire(Point16 point)
		{
			_wireSkip[point] = true;
		}

		public static void UpdateMech()
		{
			SetCurrentUser();
			for (int num = _numMechs - 1; num >= 0; num--)
			{
				_mechTime[num]--;
				if (Main.tile[_mechX[num], _mechY[num]].active() && Main.tile[_mechX[num], _mechY[num]].type == 144)
				{
					if (Main.tile[_mechX[num], _mechY[num]].frameY == 0)
					{
						_mechTime[num] = 0;
					}
					else
					{
						int num2 = Main.tile[_mechX[num], _mechY[num]].frameX / 18;
						switch (num2)
						{
						case 0:
							num2 = 60;
							break;
						case 1:
							num2 = 180;
							break;
						case 2:
							num2 = 300;
							break;
						case 3:
							num2 = 30;
							break;
						case 4:
							num2 = 15;
							break;
						}
						if (Math.IEEERemainder(_mechTime[num], num2) == 0.0)
						{
							_mechTime[num] = 18000;
							TripWire(_mechX[num], _mechY[num], 1, 1);
						}
					}
				}
				if (_mechTime[num] <= 0)
				{
					if (Main.tile[_mechX[num], _mechY[num]].active() && Main.tile[_mechX[num], _mechY[num]].type == 144)
					{
						Main.tile[_mechX[num], _mechY[num]].frameY = 0;
						NetMessage.SendTileSquare(-1, _mechX[num], _mechY[num], 1);
					}
					if (Main.tile[_mechX[num], _mechY[num]].active() && Main.tile[_mechX[num], _mechY[num]].type == 411)
					{
						Tile tile = Main.tile[_mechX[num], _mechY[num]];
						int num3 = tile.frameX % 36 / 18;
						int num4 = tile.frameY % 36 / 18;
						int num5 = _mechX[num] - num3;
						int num6 = _mechY[num] - num4;
						int num7 = 36;
						if (Main.tile[num5, num6].frameX >= 36)
						{
							num7 = -36;
						}
						for (int i = num5; i < num5 + 2; i++)
						{
							for (int j = num6; j < num6 + 2; j++)
							{
								Main.tile[i, j].frameX = (short)(Main.tile[i, j].frameX + num7);
							}
						}
						NetMessage.SendTileSquare(-1, num5, num6, 2);
					}
					for (int k = num; k < _numMechs; k++)
					{
						_mechX[k] = _mechX[k + 1];
						_mechY[k] = _mechY[k + 1];
						_mechTime[k] = _mechTime[k + 1];
					}
					_numMechs--;
				}
			}
		}

		public static void HitSwitch(int i, int j)
		{
			if (!WorldGen.InWorld(i, j) || Main.tile[i, j] == null)
			{
				return;
			}
			if (Main.tile[i, j].type == 135 || Main.tile[i, j].type == 314 || Main.tile[i, j].type == 423 || Main.tile[i, j].type == 428 || Main.tile[i, j].type == 442 || Main.tile[i, j].type == 476)
			{
				SoundEngine.PlaySound(28, i * 16, j * 16, 0);
				TripWire(i, j, 1, 1);
			}
			else if (Main.tile[i, j].type == 440)
			{
				SoundEngine.PlaySound(28, i * 16 + 16, j * 16 + 16, 0);
				TripWire(i, j, 3, 3);
			}
			else if (Main.tile[i, j].type == 136)
			{
				if (Main.tile[i, j].frameY == 0)
				{
					Main.tile[i, j].frameY = 18;
				}
				else
				{
					Main.tile[i, j].frameY = 0;
				}
				SoundEngine.PlaySound(28, i * 16, j * 16, 0);
				TripWire(i, j, 1, 1);
			}
			else if (Main.tile[i, j].type == 443)
			{
				GeyserTrap(i, j);
			}
			else if (Main.tile[i, j].type == 144)
			{
				if (Main.tile[i, j].frameY == 0)
				{
					Main.tile[i, j].frameY = 18;
					if (Main.netMode != 1)
					{
						CheckMech(i, j, 18000);
					}
				}
				else
				{
					Main.tile[i, j].frameY = 0;
				}
				SoundEngine.PlaySound(28, i * 16, j * 16, 0);
			}
			else if (Main.tile[i, j].type == 441 || Main.tile[i, j].type == 468)
			{
				int num = Main.tile[i, j].frameX / 18 * -1;
				int num2 = Main.tile[i, j].frameY / 18 * -1;
				num %= 4;
				if (num < -1)
				{
					num += 2;
				}
				num += i;
				num2 += j;
				SoundEngine.PlaySound(28, i * 16, j * 16, 0);
				TripWire(num, num2, 2, 2);
			}
			else if (Main.tile[i, j].type == 467)
			{
				if (Main.tile[i, j].frameX / 36 == 4)
				{
					int num3 = Main.tile[i, j].frameX / 18 * -1;
					int num4 = Main.tile[i, j].frameY / 18 * -1;
					num3 %= 4;
					if (num3 < -1)
					{
						num3 += 2;
					}
					num3 += i;
					num4 += j;
					SoundEngine.PlaySound(28, i * 16, j * 16, 0);
					TripWire(num3, num4, 2, 2);
				}
			}
			else
			{
				if (Main.tile[i, j].type != 132 && Main.tile[i, j].type != 411)
				{
					return;
				}
				short num5 = 36;
				int num6 = Main.tile[i, j].frameX / 18 * -1;
				int num7 = Main.tile[i, j].frameY / 18 * -1;
				num6 %= 4;
				if (num6 < -1)
				{
					num6 += 2;
					num5 = -36;
				}
				num6 += i;
				num7 += j;
				if (Main.netMode != 1 && Main.tile[num6, num7].type == 411)
				{
					CheckMech(num6, num7, 60);
				}
				for (int k = num6; k < num6 + 2; k++)
				{
					for (int l = num7; l < num7 + 2; l++)
					{
						if (Main.tile[k, l].type == 132 || Main.tile[k, l].type == 411)
						{
							Main.tile[k, l].frameX += num5;
						}
					}
				}
				WorldGen.TileFrame(num6, num7);
				SoundEngine.PlaySound(28, i * 16, j * 16, 0);
				TripWire(num6, num7, 2, 2);
			}
		}

		public static void PokeLogicGate(int lampX, int lampY)
		{
			if (Main.netMode != 1)
			{
				_LampsToCheck.Enqueue(new Point16(lampX, lampY));
				LogicGatePass();
			}
		}

		public static bool Actuate(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			if (!tile.actuator())
			{
				return false;
			}
			if (tile.inActive())
			{
				ReActive(i, j);
			}
			else
			{
				DeActive(i, j);
			}
			return true;
		}

		public static void ActuateForced(int i, int j)
		{
			if (Main.tile[i, j].inActive())
			{
				ReActive(i, j);
			}
			else
			{
				DeActive(i, j);
			}
		}

		public static void MassWireOperation(Point ps, Point pe, Player master)
		{
			int wireCount = 0;
			int actuatorCount = 0;
			for (int i = 0; i < 58; i++)
			{
				if (master.inventory[i].type == 530)
				{
					wireCount += master.inventory[i].stack;
				}
				if (master.inventory[i].type == 849)
				{
					actuatorCount += master.inventory[i].stack;
				}
			}
			int num = wireCount;
			int num2 = actuatorCount;
			MassWireOperationInner(ps, pe, master.Center, master.direction == 1, wireCount, actuatorCount);
			int num3 = num - wireCount;
			int num4 = num2 - actuatorCount;
			if (Main.netMode == 2)
			{
				NetMessage.SendData(110, master.whoAmI, -1, null, 530, num3, master.whoAmI);
				NetMessage.SendData(110, master.whoAmI, -1, null, 849, num4, master.whoAmI);
				return;
			}
			for (int j = 0; j < num3; j++)
			{
				master.ConsumeItem(530);
			}
			for (int k = 0; k < num4; k++)
			{
				master.ConsumeItem(849);
			}
		}

		private static bool CheckMech(int i, int j, int time)
		{
			for (int k = 0; k < _numMechs; k++)
			{
				if (_mechX[k] == i && _mechY[k] == j)
				{
					return false;
				}
			}
			if (_numMechs < 999)
			{
				_mechX[_numMechs] = i;
				_mechY[_numMechs] = j;
				_mechTime[_numMechs] = time;
				_numMechs++;
				return true;
			}
			return false;
		}

		private static void XferWater()
		{
			for (int i = 0; i < _numInPump; i++)
			{
				int num = _inPumpX[i];
				int num2 = _inPumpY[i];
				int liquid = Main.tile[num, num2].liquid;
				if (liquid <= 0)
				{
					continue;
				}
				bool flag = Main.tile[num, num2].lava();
				bool flag2 = Main.tile[num, num2].honey();
				for (int j = 0; j < _numOutPump; j++)
				{
					int num3 = _outPumpX[j];
					int num4 = _outPumpY[j];
					int liquid2 = Main.tile[num3, num4].liquid;
					if (liquid2 >= 255)
					{
						continue;
					}
					bool flag3 = Main.tile[num3, num4].lava();
					bool flag4 = Main.tile[num3, num4].honey();
					if (liquid2 == 0)
					{
						flag3 = flag;
						flag4 = flag2;
					}
					if (flag == flag3 && flag2 == flag4)
					{
						int num5 = liquid;
						if (num5 + liquid2 > 255)
						{
							num5 = 255 - liquid2;
						}
						Main.tile[num3, num4].liquid += (byte)num5;
						Main.tile[num, num2].liquid -= (byte)num5;
						liquid = Main.tile[num, num2].liquid;
						Main.tile[num3, num4].lava(flag);
						Main.tile[num3, num4].honey(flag2);
						WorldGen.SquareTileFrame(num3, num4);
						if (Main.tile[num, num2].liquid == 0)
						{
							Main.tile[num, num2].lava(lava: false);
							WorldGen.SquareTileFrame(num, num2);
							break;
						}
					}
				}
				WorldGen.SquareTileFrame(num, num2);
			}
		}

		private static void TripWire(int left, int top, int width, int height)
		{
			if (Main.netMode == 1)
			{
				return;
			}
			running = true;
			if (_wireList.Count != 0)
			{
				_wireList.Clear(quickClear: true);
			}
			if (_wireDirectionList.Count != 0)
			{
				_wireDirectionList.Clear(quickClear: true);
			}
			Vector2[] array = new Vector2[8];
			int num = 0;
			for (int i = left; i < left + width; i++)
			{
				for (int j = top; j < top + height; j++)
				{
					Point16 back = new Point16(i, j);
					Tile tile = Main.tile[i, j];
					if (tile != null && tile.wire())
					{
						_wireList.PushBack(back);
					}
				}
			}
			_teleport[0].X = -1f;
			_teleport[0].Y = -1f;
			_teleport[1].X = -1f;
			_teleport[1].Y = -1f;
			if (_wireList.Count > 0)
			{
				_numInPump = 0;
				_numOutPump = 0;
				HitWire(_wireList, 1);
				if (_numInPump > 0 && _numOutPump > 0)
				{
					XferWater();
				}
			}
			array[num++] = _teleport[0];
			array[num++] = _teleport[1];
			for (int k = left; k < left + width; k++)
			{
				for (int l = top; l < top + height; l++)
				{
					Point16 back = new Point16(k, l);
					Tile tile2 = Main.tile[k, l];
					if (tile2 != null && tile2.wire2())
					{
						_wireList.PushBack(back);
					}
				}
			}
			_teleport[0].X = -1f;
			_teleport[0].Y = -1f;
			_teleport[1].X = -1f;
			_teleport[1].Y = -1f;
			if (_wireList.Count > 0)
			{
				_numInPump = 0;
				_numOutPump = 0;
				HitWire(_wireList, 2);
				if (_numInPump > 0 && _numOutPump > 0)
				{
					XferWater();
				}
			}
			array[num++] = _teleport[0];
			array[num++] = _teleport[1];
			_teleport[0].X = -1f;
			_teleport[0].Y = -1f;
			_teleport[1].X = -1f;
			_teleport[1].Y = -1f;
			for (int m = left; m < left + width; m++)
			{
				for (int n = top; n < top + height; n++)
				{
					Point16 back = new Point16(m, n);
					Tile tile3 = Main.tile[m, n];
					if (tile3 != null && tile3.wire3())
					{
						_wireList.PushBack(back);
					}
				}
			}
			if (_wireList.Count > 0)
			{
				_numInPump = 0;
				_numOutPump = 0;
				HitWire(_wireList, 3);
				if (_numInPump > 0 && _numOutPump > 0)
				{
					XferWater();
				}
			}
			array[num++] = _teleport[0];
			array[num++] = _teleport[1];
			_teleport[0].X = -1f;
			_teleport[0].Y = -1f;
			_teleport[1].X = -1f;
			_teleport[1].Y = -1f;
			for (int num2 = left; num2 < left + width; num2++)
			{
				for (int num3 = top; num3 < top + height; num3++)
				{
					Point16 back = new Point16(num2, num3);
					Tile tile4 = Main.tile[num2, num3];
					if (tile4 != null && tile4.wire4())
					{
						_wireList.PushBack(back);
					}
				}
			}
			if (_wireList.Count > 0)
			{
				_numInPump = 0;
				_numOutPump = 0;
				HitWire(_wireList, 4);
				if (_numInPump > 0 && _numOutPump > 0)
				{
					XferWater();
				}
			}
			array[num++] = _teleport[0];
			array[num++] = _teleport[1];
			running = false;
			for (int num4 = 0; num4 < 8; num4 += 2)
			{
				_teleport[0] = array[num4];
				_teleport[1] = array[num4 + 1];
				if (_teleport[0].X >= 0f && _teleport[1].X >= 0f)
				{
					Teleport();
				}
			}
			PixelBoxPass();
			LogicGatePass();
		}

		private static void PixelBoxPass()
		{
			foreach (KeyValuePair<Point16, byte> pixelBoxTrigger in _PixelBoxTriggers)
			{
				if (pixelBoxTrigger.Value == 2)
				{
					continue;
				}
				if (pixelBoxTrigger.Value == 1)
				{
					if (Main.tile[pixelBoxTrigger.Key.X, pixelBoxTrigger.Key.Y].frameX != 0)
					{
						Main.tile[pixelBoxTrigger.Key.X, pixelBoxTrigger.Key.Y].frameX = 0;
						NetMessage.SendTileSquare(-1, pixelBoxTrigger.Key.X, pixelBoxTrigger.Key.Y, 1);
					}
				}
				else if (pixelBoxTrigger.Value == 3 && Main.tile[pixelBoxTrigger.Key.X, pixelBoxTrigger.Key.Y].frameX != 18)
				{
					Main.tile[pixelBoxTrigger.Key.X, pixelBoxTrigger.Key.Y].frameX = 18;
					NetMessage.SendTileSquare(-1, pixelBoxTrigger.Key.X, pixelBoxTrigger.Key.Y, 1);
				}
			}
			_PixelBoxTriggers.Clear();
		}

		private static void LogicGatePass()
		{
			if (_GatesCurrent.Count != 0)
			{
				return;
			}
			_GatesDone.Clear();
			while (_LampsToCheck.Count > 0)
			{
				while (_LampsToCheck.Count > 0)
				{
					Point16 point = _LampsToCheck.Dequeue();
					CheckLogicGate(point.X, point.Y);
				}
				while (_GatesNext.Count > 0)
				{
					Utils.Swap(_GatesCurrent, _GatesNext);
					while (_GatesCurrent.Count > 0)
					{
						Point16 key = _GatesCurrent.Peek();
						if (_GatesDone.TryGetValue(key, out var value) && value)
						{
							_GatesCurrent.Dequeue();
							continue;
						}
						_GatesDone.Add(key, value: true);
						TripWire(key.X, key.Y, 1, 1);
						_GatesCurrent.Dequeue();
					}
				}
			}
			_GatesDone.Clear();
			if (blockPlayerTeleportationForOneIteration)
			{
				blockPlayerTeleportationForOneIteration = false;
			}
		}

		private static void CheckLogicGate(int lampX, int lampY)
		{
			if (!WorldGen.InWorld(lampX, lampY, 1))
			{
				return;
			}
			for (int i = lampY; i < Main.maxTilesY; i++)
			{
				Tile tile = Main.tile[lampX, i];
				if (!tile.active())
				{
					break;
				}
				if (tile.type == 420)
				{
					_GatesDone.TryGetValue(new Point16(lampX, i), out var value);
					int num = tile.frameY / 18;
					bool flag = tile.frameX == 18;
					bool flag2 = tile.frameX == 36;
					if (num < 0)
					{
						break;
					}
					int num2 = 0;
					int num3 = 0;
					bool flag3 = false;
					for (int num4 = i - 1; num4 > 0; num4--)
					{
						Tile tile2 = Main.tile[lampX, num4];
						if (!tile2.active() || tile2.type != 419)
						{
							break;
						}
						if (tile2.frameX == 36)
						{
							flag3 = true;
							break;
						}
						num2++;
						num3 += (tile2.frameX == 18).ToInt();
					}
					bool flag4 = false;
					switch (num)
					{
					default:
						return;
					case 0:
						flag4 = num2 == num3;
						break;
					case 2:
						flag4 = num2 != num3;
						break;
					case 1:
						flag4 = num3 > 0;
						break;
					case 3:
						flag4 = num3 == 0;
						break;
					case 4:
						flag4 = num3 == 1;
						break;
					case 5:
						flag4 = num3 != 1;
						break;
					}
					bool flag5 = !flag3 && flag2;
					bool flag6 = false;
					if (flag3 && Framing.GetTileSafely(lampX, lampY).frameX == 36)
					{
						flag6 = true;
					}
					if (!(flag4 != flag || flag5 || flag6))
					{
						break;
					}
					_ = tile.frameX % 18 / 18;
					tile.frameX = (short)(18 * flag4.ToInt());
					if (flag3)
					{
						tile.frameX = 36;
					}
					SkipWire(lampX, i);
					WorldGen.SquareTileFrame(lampX, i);
					NetMessage.SendTileSquare(-1, lampX, i, 1);
					bool flag7 = !flag3 || flag6;
					if (flag6)
					{
						if (num3 == 0 || num2 == 0)
						{
							flag7 = false;
						}
						flag7 = Main.rand.NextFloat() < (float)num3 / (float)num2;
					}
					if (flag5)
					{
						flag7 = false;
					}
					if (flag7)
					{
						if (!value)
						{
							_GatesNext.Enqueue(new Point16(lampX, i));
							break;
						}
						Vector2 position = new Vector2(lampX, i) * 16f - new Vector2(10f);
						Utils.PoofOfSmoke(position);
						NetMessage.SendData(106, -1, -1, null, (int)position.X, position.Y);
					}
					break;
				}
				if (tile.type != 419)
				{
					break;
				}
			}
		}

		private static void HitWire(DoubleStack<Point16> next, int wireType)
		{
			_wireDirectionList.Clear(quickClear: true);
			for (int i = 0; i < next.Count; i++)
			{
				Point16 point = next.PopFront();
				SkipWire(point);
				_toProcess.Add(point, 4);
				next.PushBack(point);
				_wireDirectionList.PushBack(0);
			}
			_currentWireColor = wireType;
			while (next.Count > 0)
			{
				Point16 key = next.PopFront();
				int num = _wireDirectionList.PopFront();
				int x = key.X;
				int y = key.Y;
				if (!_wireSkip.ContainsKey(key))
				{
					HitWireSingle(x, y);
				}
				for (int j = 0; j < 4; j++)
				{
					int num2;
					int num3;
					switch (j)
					{
					case 0:
						num2 = x;
						num3 = y + 1;
						break;
					case 1:
						num2 = x;
						num3 = y - 1;
						break;
					case 2:
						num2 = x + 1;
						num3 = y;
						break;
					case 3:
						num2 = x - 1;
						num3 = y;
						break;
					default:
						num2 = x;
						num3 = y + 1;
						break;
					}
					if (num2 < 2 || num2 >= Main.maxTilesX - 2 || num3 < 2 || num3 >= Main.maxTilesY - 2)
					{
						continue;
					}
					Tile tile = Main.tile[num2, num3];
					if (tile == null)
					{
						continue;
					}
					Tile tile2 = Main.tile[x, y];
					if (tile2 == null)
					{
						continue;
					}
					byte b = 3;
					if (tile.type == 424 || tile.type == 445)
					{
						b = 0;
					}
					if (tile2.type == 424)
					{
						switch (tile2.frameX / 18)
						{
						case 0:
							if (j != num)
							{
								continue;
							}
							break;
						case 1:
							if ((num != 0 || j != 3) && (num != 3 || j != 0) && (num != 1 || j != 2) && (num != 2 || j != 1))
							{
								continue;
							}
							break;
						case 2:
							if ((num != 0 || j != 2) && (num != 2 || j != 0) && (num != 1 || j != 3) && (num != 3 || j != 1))
							{
								continue;
							}
							break;
						}
					}
					if (tile2.type == 445)
					{
						if (j != num)
						{
							continue;
						}
						if (_PixelBoxTriggers.ContainsKey(key))
						{
							_PixelBoxTriggers[key] |= (byte)((!(j == 0 || j == 1)) ? 1 : 2);
						}
						else
						{
							_PixelBoxTriggers[key] = (byte)((!(j == 0 || j == 1)) ? 1u : 2u);
						}
					}
					if (wireType switch
					{
						1 => tile.wire() ? 1 : 0, 
						2 => tile.wire2() ? 1 : 0, 
						3 => tile.wire3() ? 1 : 0, 
						4 => tile.wire4() ? 1 : 0, 
						_ => 0, 
					} == 0)
					{
						continue;
					}
					Point16 point2 = new Point16(num2, num3);
					if (_toProcess.TryGetValue(point2, out var value))
					{
						value = (byte)(value - 1);
						if (value == 0)
						{
							_toProcess.Remove(point2);
						}
						else
						{
							_toProcess[point2] = value;
						}
						continue;
					}
					next.PushBack(point2);
					_wireDirectionList.PushBack((byte)j);
					if (b > 0)
					{
						_toProcess.Add(point2, b);
					}
				}
			}
			_wireSkip.Clear();
			_toProcess.Clear();
		}

		private static void HitWireSingle(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int type = tile.type;
			if (tile.actuator())
			{
				ActuateForced(i, j);
			}
			if (!tile.active())
			{
				return;
			}
			switch (type)
			{
			case 144:
				HitSwitch(i, j);
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j, 1);
				break;
			case 421:
				if (!tile.actuator())
				{
					tile.type = 422;
					WorldGen.SquareTileFrame(i, j);
					NetMessage.SendTileSquare(-1, i, j, 1);
				}
				break;
			case 422:
				if (!tile.actuator())
				{
					tile.type = 421;
					WorldGen.SquareTileFrame(i, j);
					NetMessage.SendTileSquare(-1, i, j, 1);
				}
				break;
			}
			if (type >= 255 && type <= 268)
			{
				if (!tile.actuator())
				{
					if (type >= 262)
					{
						tile.type -= 7;
					}
					else
					{
						tile.type += 7;
					}
					WorldGen.SquareTileFrame(i, j);
					NetMessage.SendTileSquare(-1, i, j, 1);
				}
				return;
			}
			if (type == 419)
			{
				int num = 18;
				if (tile.frameX >= num)
				{
					num = -num;
				}
				if (tile.frameX == 36)
				{
					num = 0;
				}
				SkipWire(i, j);
				tile.frameX = (short)(tile.frameX + num);
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j, 1);
				_LampsToCheck.Enqueue(new Point16(i, j));
				return;
			}
			if (type == 406)
			{
				int num2 = tile.frameX % 54 / 18;
				int num3 = tile.frameY % 54 / 18;
				int num4 = i - num2;
				int num5 = j - num3;
				int num6 = 54;
				if (Main.tile[num4, num5].frameY >= 108)
				{
					num6 = -108;
				}
				for (int k = num4; k < num4 + 3; k++)
				{
					for (int l = num5; l < num5 + 3; l++)
					{
						SkipWire(k, l);
						Main.tile[k, l].frameY = (short)(Main.tile[k, l].frameY + num6);
					}
				}
				NetMessage.SendTileSquare(-1, num4 + 1, num5 + 1, 3);
				return;
			}
			if (type == 452)
			{
				int num7 = tile.frameX % 54 / 18;
				int num8 = tile.frameY % 54 / 18;
				int num9 = i - num7;
				int num10 = j - num8;
				int num11 = 54;
				if (Main.tile[num9, num10].frameX >= 54)
				{
					num11 = -54;
				}
				for (int m = num9; m < num9 + 3; m++)
				{
					for (int n = num10; n < num10 + 3; n++)
					{
						SkipWire(m, n);
						Main.tile[m, n].frameX = (short)(Main.tile[m, n].frameX + num11);
					}
				}
				NetMessage.SendTileSquare(-1, num9 + 1, num10 + 1, 3);
				return;
			}
			if (type == 411)
			{
				int num12 = tile.frameX % 36 / 18;
				int num13 = tile.frameY % 36 / 18;
				int num14 = i - num12;
				int num15 = j - num13;
				int num16 = 36;
				if (Main.tile[num14, num15].frameX >= 36)
				{
					num16 = -36;
				}
				for (int num17 = num14; num17 < num14 + 2; num17++)
				{
					for (int num18 = num15; num18 < num15 + 2; num18++)
					{
						SkipWire(num17, num18);
						Main.tile[num17, num18].frameX = (short)(Main.tile[num17, num18].frameX + num16);
					}
				}
				NetMessage.SendTileSquare(-1, num14, num15, 2);
				return;
			}
			if (type == 425)
			{
				int num19 = tile.frameX % 36 / 18;
				int num20 = tile.frameY % 36 / 18;
				int num21 = i - num19;
				int num22 = j - num20;
				for (int num23 = num21; num23 < num21 + 2; num23++)
				{
					for (int num24 = num22; num24 < num22 + 2; num24++)
					{
						SkipWire(num23, num24);
					}
				}
				if (Main.AnnouncementBoxDisabled)
				{
					return;
				}
				Color pink = Color.Pink;
				int num25 = Sign.ReadSign(num21, num22, CreateIfMissing: false);
				if (num25 == -1 || Main.sign[num25] == null || string.IsNullOrWhiteSpace(Main.sign[num25].text))
				{
					return;
				}
				if (Main.AnnouncementBoxRange == -1)
				{
					if (Main.netMode == 0)
					{
						Main.NewTextMultiline(Main.sign[num25].text, force: false, pink, 460);
					}
					else if (Main.netMode == 2)
					{
						NetMessage.SendData(107, -1, -1, NetworkText.FromLiteral(Main.sign[num25].text), 255, (int)pink.R, (int)pink.G, (int)pink.B, 460);
					}
				}
				else if (Main.netMode == 0)
				{
					if (Main.player[Main.myPlayer].Distance(new Vector2(num21 * 16 + 16, num22 * 16 + 16)) <= (float)Main.AnnouncementBoxRange)
					{
						Main.NewTextMultiline(Main.sign[num25].text, force: false, pink, 460);
					}
				}
				else
				{
					if (Main.netMode != 2)
					{
						return;
					}
					for (int num26 = 0; num26 < 255; num26++)
					{
						if (Main.player[num26].active && Main.player[num26].Distance(new Vector2(num21 * 16 + 16, num22 * 16 + 16)) <= (float)Main.AnnouncementBoxRange)
						{
							NetMessage.SendData(107, num26, -1, NetworkText.FromLiteral(Main.sign[num25].text), 255, (int)pink.R, (int)pink.G, (int)pink.B, 460);
						}
					}
				}
				return;
			}
			if (type == 405)
			{
				int num27 = tile.frameX % 54 / 18;
				int num28 = tile.frameY % 36 / 18;
				int num29 = i - num27;
				int num30 = j - num28;
				int num31 = 54;
				if (Main.tile[num29, num30].frameX >= 54)
				{
					num31 = -54;
				}
				for (int num32 = num29; num32 < num29 + 3; num32++)
				{
					for (int num33 = num30; num33 < num30 + 2; num33++)
					{
						SkipWire(num32, num33);
						Main.tile[num32, num33].frameX = (short)(Main.tile[num32, num33].frameX + num31);
					}
				}
				NetMessage.SendTileSquare(-1, num29 + 1, num30 + 1, 3);
				return;
			}
			if (type == 209)
			{
				int num34 = tile.frameX % 72 / 18;
				int num35 = tile.frameY % 54 / 18;
				int num36 = i - num34;
				int num37 = j - num35;
				int num38 = tile.frameY / 54;
				int num39 = tile.frameX / 72;
				int num40 = -1;
				if (num34 == 1 || num34 == 2)
				{
					num40 = num35;
				}
				int num41 = 0;
				if (num34 == 3)
				{
					num41 = -54;
				}
				if (num34 == 0)
				{
					num41 = 54;
				}
				if (num38 >= 8 && num41 > 0)
				{
					num41 = 0;
				}
				if (num38 == 0 && num41 < 0)
				{
					num41 = 0;
				}
				bool flag = false;
				if (num41 != 0)
				{
					for (int num42 = num36; num42 < num36 + 4; num42++)
					{
						for (int num43 = num37; num43 < num37 + 3; num43++)
						{
							SkipWire(num42, num43);
							Main.tile[num42, num43].frameY = (short)(Main.tile[num42, num43].frameY + num41);
						}
					}
					flag = true;
				}
				if ((num39 == 3 || num39 == 4) && (num40 == 0 || num40 == 1))
				{
					num41 = ((num39 == 3) ? 72 : (-72));
					for (int num44 = num36; num44 < num36 + 4; num44++)
					{
						for (int num45 = num37; num45 < num37 + 3; num45++)
						{
							SkipWire(num44, num45);
							Main.tile[num44, num45].frameX = (short)(Main.tile[num44, num45].frameX + num41);
						}
					}
					flag = true;
				}
				if (flag)
				{
					NetMessage.SendTileSquare(-1, num36 + 1, num37 + 1, 4);
				}
				if (num40 != -1)
				{
					bool flag2 = true;
					if ((num39 == 3 || num39 == 4) && num40 < 2)
					{
						flag2 = false;
					}
					if (CheckMech(num36, num37, 30) && flag2)
					{
						WorldGen.ShootFromCannon(num36, num37, num38, num39 + 1, 0, 0f, CurrentUser);
					}
				}
				return;
			}
			if (type == 212)
			{
				int num46 = tile.frameX % 54 / 18;
				int num47 = tile.frameY % 54 / 18;
				int num48 = i - num46;
				int num49 = j - num47;
				int num50 = tile.frameX / 54;
				int num51 = -1;
				if (num46 == 1)
				{
					num51 = num47;
				}
				int num52 = 0;
				if (num46 == 0)
				{
					num52 = -54;
				}
				if (num46 == 2)
				{
					num52 = 54;
				}
				if (num50 >= 1 && num52 > 0)
				{
					num52 = 0;
				}
				if (num50 == 0 && num52 < 0)
				{
					num52 = 0;
				}
				bool flag3 = false;
				if (num52 != 0)
				{
					for (int num53 = num48; num53 < num48 + 3; num53++)
					{
						for (int num54 = num49; num54 < num49 + 3; num54++)
						{
							SkipWire(num53, num54);
							Main.tile[num53, num54].frameX = (short)(Main.tile[num53, num54].frameX + num52);
						}
					}
					flag3 = true;
				}
				if (flag3)
				{
					NetMessage.SendTileSquare(-1, num48 + 1, num49 + 1, 4);
				}
				if (num51 != -1 && CheckMech(num48, num49, 10))
				{
					float num55 = 12f + (float)Main.rand.Next(450) * 0.01f;
					float num56 = Main.rand.Next(85, 105);
					float num57 = Main.rand.Next(-35, 11);
					int type2 = 166;
					int damage = 0;
					float knockBack = 0f;
					Vector2 vector = new Vector2((num48 + 2) * 16 - 8, (num49 + 2) * 16 - 8);
					if (tile.frameX / 54 == 0)
					{
						num56 *= -1f;
						vector.X -= 12f;
					}
					else
					{
						vector.X += 12f;
					}
					float num58 = num56;
					float num59 = num57;
					float num60 = (float)Math.Sqrt(num58 * num58 + num59 * num59);
					num60 = num55 / num60;
					num58 *= num60;
					num59 *= num60;
					Projectile.NewProjectile(vector.X, vector.Y, num58, num59, type2, damage, knockBack, CurrentUser);
				}
				return;
			}
			if (type == 215)
			{
				int num61 = tile.frameX % 54 / 18;
				int num62 = tile.frameY % 36 / 18;
				int num63 = i - num61;
				int num64 = j - num62;
				int num65 = 36;
				if (Main.tile[num63, num64].frameY >= 36)
				{
					num65 = -36;
				}
				for (int num66 = num63; num66 < num63 + 3; num66++)
				{
					for (int num67 = num64; num67 < num64 + 2; num67++)
					{
						SkipWire(num66, num67);
						Main.tile[num66, num67].frameY = (short)(Main.tile[num66, num67].frameY + num65);
					}
				}
				NetMessage.SendTileSquare(-1, num63 + 1, num64 + 1, 3);
				return;
			}
			if (type == 130)
			{
				if (Main.tile[i, j - 1] == null || !Main.tile[i, j - 1].active() || (!TileID.Sets.BasicChest[Main.tile[i, j - 1].type] && !TileID.Sets.BasicChestFake[Main.tile[i, j - 1].type] && Main.tile[i, j - 1].type != 88))
				{
					tile.type = 131;
					WorldGen.SquareTileFrame(i, j);
					NetMessage.SendTileSquare(-1, i, j, 1);
				}
				return;
			}
			if (type == 131)
			{
				tile.type = 130;
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j, 1);
				return;
			}
			if (type == 387 || type == 386)
			{
				bool value = type == 387;
				int num68 = WorldGen.ShiftTrapdoor(i, j, playerAbove: true).ToInt();
				if (num68 == 0)
				{
					num68 = -WorldGen.ShiftTrapdoor(i, j, playerAbove: false).ToInt();
				}
				if (num68 != 0)
				{
					NetMessage.SendData(19, -1, -1, null, 3 - value.ToInt(), i, j, num68);
				}
				return;
			}
			if (type == 389 || type == 388)
			{
				bool flag4 = type == 389;
				WorldGen.ShiftTallGate(i, j, flag4);
				NetMessage.SendData(19, -1, -1, null, 4 + flag4.ToInt(), i, j);
				return;
			}
			if (type == 11)
			{
				if (WorldGen.CloseDoor(i, j, forced: true))
				{
					NetMessage.SendData(19, -1, -1, null, 1, i, j);
				}
				return;
			}
			if (type == 10)
			{
				int num69 = 1;
				if (Main.rand.Next(2) == 0)
				{
					num69 = -1;
				}
				if (!WorldGen.OpenDoor(i, j, num69))
				{
					if (WorldGen.OpenDoor(i, j, -num69))
					{
						NetMessage.SendData(19, -1, -1, null, 0, i, j, -num69);
					}
				}
				else
				{
					NetMessage.SendData(19, -1, -1, null, 0, i, j, num69);
				}
				return;
			}
			if (type == 216)
			{
				WorldGen.LaunchRocket(i, j);
				SkipWire(i, j);
				return;
			}
			if (type == 497 || (type == 15 && tile.frameY / 40 == 1) || (type == 15 && tile.frameY / 40 == 20))
			{
				int num70 = j - tile.frameY % 40 / 18;
				SkipWire(i, num70);
				SkipWire(i, num70 + 1);
				if (CheckMech(i, num70, 60))
				{
					Projectile.NewProjectile(i * 16 + 8, num70 * 16 + 12, 0f, 0f, 733, 0, 0f, Main.myPlayer);
				}
				return;
			}
			switch (type)
			{
			case 335:
			{
				int num176 = j - tile.frameY / 18;
				int num177 = i - tile.frameX / 18;
				SkipWire(num177, num176);
				SkipWire(num177, num176 + 1);
				SkipWire(num177 + 1, num176);
				SkipWire(num177 + 1, num176 + 1);
				if (CheckMech(num177, num176, 30))
				{
					WorldGen.LaunchRocketSmall(num177, num176);
				}
				break;
			}
			case 338:
			{
				int num77 = j - tile.frameY / 18;
				int num78 = i - tile.frameX / 18;
				SkipWire(num78, num77);
				SkipWire(num78, num77 + 1);
				if (!CheckMech(num78, num77, 30))
				{
					break;
				}
				bool flag5 = false;
				for (int num79 = 0; num79 < 1000; num79++)
				{
					if (Main.projectile[num79].active && Main.projectile[num79].aiStyle == 73 && Main.projectile[num79].ai[0] == (float)num78 && Main.projectile[num79].ai[1] == (float)num77)
					{
						flag5 = true;
						break;
					}
				}
				if (!flag5)
				{
					Projectile.NewProjectile(num78 * 16 + 8, num77 * 16 + 2, 0f, 0f, 419 + Main.rand.Next(4), 0, 0f, Main.myPlayer, num78, num77);
				}
				break;
			}
			case 235:
			{
				int num132 = i - tile.frameX / 18;
				if (tile.wall == 87 && (double)j > Main.worldSurface && !NPC.downedPlantBoss)
				{
					break;
				}
				if (_teleport[0].X == -1f)
				{
					_teleport[0].X = num132;
					_teleport[0].Y = j;
					if (tile.halfBrick())
					{
						_teleport[0].Y += 0.5f;
					}
				}
				else if (_teleport[0].X != (float)num132 || _teleport[0].Y != (float)j)
				{
					_teleport[1].X = num132;
					_teleport[1].Y = j;
					if (tile.halfBrick())
					{
						_teleport[1].Y += 0.5f;
					}
				}
				break;
			}
			case 4:
				if (tile.frameX < 66)
				{
					tile.frameX += 66;
				}
				else
				{
					tile.frameX -= 66;
				}
				NetMessage.SendTileSquare(-1, i, j, 1);
				break;
			case 429:
			{
				int num97 = Main.tile[i, j].frameX / 18;
				bool flag6 = num97 % 2 >= 1;
				bool flag7 = num97 % 4 >= 2;
				bool flag8 = num97 % 8 >= 4;
				bool flag9 = num97 % 16 >= 8;
				bool flag10 = false;
				short num98 = 0;
				switch (_currentWireColor)
				{
				case 1:
					num98 = 18;
					flag10 = !flag6;
					break;
				case 2:
					num98 = 72;
					flag10 = !flag8;
					break;
				case 3:
					num98 = 36;
					flag10 = !flag7;
					break;
				case 4:
					num98 = 144;
					flag10 = !flag9;
					break;
				}
				if (flag10)
				{
					tile.frameX += num98;
				}
				else
				{
					tile.frameX -= num98;
				}
				NetMessage.SendTileSquare(-1, i, j, 1);
				break;
			}
			case 149:
				if (tile.frameX < 54)
				{
					tile.frameX += 54;
				}
				else
				{
					tile.frameX -= 54;
				}
				NetMessage.SendTileSquare(-1, i, j, 1);
				break;
			case 244:
			{
				int num80;
				for (num80 = tile.frameX / 18; num80 >= 3; num80 -= 3)
				{
				}
				int num81;
				for (num81 = tile.frameY / 18; num81 >= 3; num81 -= 3)
				{
				}
				int num82 = i - num80;
				int num83 = j - num81;
				int num84 = 54;
				if (Main.tile[num82, num83].frameX >= 54)
				{
					num84 = -54;
				}
				for (int num85 = num82; num85 < num82 + 3; num85++)
				{
					for (int num86 = num83; num86 < num83 + 2; num86++)
					{
						SkipWire(num85, num86);
						Main.tile[num85, num86].frameX = (short)(Main.tile[num85, num86].frameX + num84);
					}
				}
				NetMessage.SendTileSquare(-1, num82 + 1, num83 + 1, 3);
				break;
			}
			case 565:
			{
				int num90;
				for (num90 = tile.frameX / 18; num90 >= 2; num90 -= 2)
				{
				}
				int num91;
				for (num91 = tile.frameY / 18; num91 >= 2; num91 -= 2)
				{
				}
				int num92 = i - num90;
				int num93 = j - num91;
				int num94 = 36;
				if (Main.tile[num92, num93].frameX >= 36)
				{
					num94 = -36;
				}
				for (int num95 = num92; num95 < num92 + 2; num95++)
				{
					for (int num96 = num93; num96 < num93 + 2; num96++)
					{
						SkipWire(num95, num96);
						Main.tile[num95, num96].frameX = (short)(Main.tile[num95, num96].frameX + num94);
					}
				}
				NetMessage.SendTileSquare(-1, num92 + 1, num93 + 1, 3);
				break;
			}
			case 42:
			{
				int num99;
				for (num99 = tile.frameY / 18; num99 >= 2; num99 -= 2)
				{
				}
				int num100 = j - num99;
				short num101 = 18;
				if (tile.frameX > 0)
				{
					num101 = -18;
				}
				Main.tile[i, num100].frameX += num101;
				Main.tile[i, num100 + 1].frameX += num101;
				SkipWire(i, num100);
				SkipWire(i, num100 + 1);
				NetMessage.SendTileSquare(-1, i, j, 3);
				break;
			}
			case 93:
			{
				int num156;
				for (num156 = tile.frameY / 18; num156 >= 3; num156 -= 3)
				{
				}
				num156 = j - num156;
				short num157 = 18;
				if (tile.frameX > 0)
				{
					num157 = -18;
				}
				Main.tile[i, num156].frameX += num157;
				Main.tile[i, num156 + 1].frameX += num157;
				Main.tile[i, num156 + 2].frameX += num157;
				SkipWire(i, num156);
				SkipWire(i, num156 + 1);
				SkipWire(i, num156 + 2);
				NetMessage.SendTileSquare(-1, i, num156 + 1, 3);
				break;
			}
			case 95:
			case 100:
			case 126:
			case 173:
			case 564:
			{
				int num121;
				for (num121 = tile.frameY / 18; num121 >= 2; num121 -= 2)
				{
				}
				num121 = j - num121;
				int num122 = tile.frameX / 18;
				if (num122 > 1)
				{
					num122 -= 2;
				}
				num122 = i - num122;
				short num123 = 36;
				if (Main.tile[num122, num121].frameX > 0)
				{
					num123 = -36;
				}
				Main.tile[num122, num121].frameX += num123;
				Main.tile[num122, num121 + 1].frameX += num123;
				Main.tile[num122 + 1, num121].frameX += num123;
				Main.tile[num122 + 1, num121 + 1].frameX += num123;
				SkipWire(num122, num121);
				SkipWire(num122 + 1, num121);
				SkipWire(num122, num121 + 1);
				SkipWire(num122 + 1, num121 + 1);
				NetMessage.SendTileSquare(-1, num122, num121, 3);
				break;
			}
			case 593:
			{
				SkipWire(i, j);
				short num130 = (short)((Main.tile[i, j].frameX != 0) ? (-18) : 18);
				Main.tile[i, j].frameX += num130;
				if (Main.netMode == 2)
				{
					NetMessage.SendTileRange(-1, i, j, 1, 1);
				}
				int num131 = ((num130 > 0) ? 4 : 3);
				Animation.NewTemporaryAnimation(num131, 593, i, j);
				NetMessage.SendTemporaryAnimation(-1, num131, 593, i, j);
				break;
			}
			case 594:
			{
				int num102;
				for (num102 = tile.frameY / 18; num102 >= 2; num102 -= 2)
				{
				}
				num102 = j - num102;
				int num103 = tile.frameX / 18;
				if (num103 > 1)
				{
					num103 -= 2;
				}
				num103 = i - num103;
				SkipWire(num103, num102);
				SkipWire(num103, num102 + 1);
				SkipWire(num103 + 1, num102);
				SkipWire(num103 + 1, num102 + 1);
				short num104 = (short)((Main.tile[num103, num102].frameX != 0) ? (-36) : 36);
				for (int num105 = 0; num105 < 2; num105++)
				{
					for (int num106 = 0; num106 < 2; num106++)
					{
						Main.tile[num103 + num105, num102 + num106].frameX += num104;
					}
				}
				if (Main.netMode == 2)
				{
					NetMessage.SendTileRange(-1, num103, num102, 2, 2);
				}
				int num107 = ((num104 > 0) ? 4 : 3);
				Animation.NewTemporaryAnimation(num107, 594, num103, num102);
				NetMessage.SendTemporaryAnimation(-1, num107, 594, num103, num102);
				break;
			}
			case 34:
			{
				int num124;
				for (num124 = tile.frameY / 18; num124 >= 3; num124 -= 3)
				{
				}
				int num125 = j - num124;
				int num126 = tile.frameX % 108 / 18;
				if (num126 > 2)
				{
					num126 -= 3;
				}
				num126 = i - num126;
				short num127 = 54;
				if (Main.tile[num126, num125].frameX % 108 > 0)
				{
					num127 = -54;
				}
				for (int num128 = num126; num128 < num126 + 3; num128++)
				{
					for (int num129 = num125; num129 < num125 + 3; num129++)
					{
						Main.tile[num128, num129].frameX += num127;
						SkipWire(num128, num129);
					}
				}
				NetMessage.SendTileSquare(-1, num126 + 1, num125 + 1, 3);
				break;
			}
			case 314:
				if (CheckMech(i, j, 5))
				{
					Minecart.FlipSwitchTrack(i, j);
				}
				break;
			case 33:
			case 49:
			case 174:
			case 372:
			{
				short num108 = 18;
				if (tile.frameX > 0)
				{
					num108 = -18;
				}
				tile.frameX += num108;
				NetMessage.SendTileSquare(-1, i, j, 3);
				break;
			}
			case 92:
			{
				int num87 = j - tile.frameY / 18;
				short num88 = 18;
				if (tile.frameX > 0)
				{
					num88 = -18;
				}
				for (int num89 = num87; num89 < num87 + 6; num89++)
				{
					Main.tile[i, num89].frameX += num88;
					SkipWire(i, num89);
				}
				NetMessage.SendTileSquare(-1, i, num87 + 3, 7);
				break;
			}
			case 137:
			{
				int num158 = tile.frameY / 18;
				Vector2 vector3 = Vector2.Zero;
				float speedX = 0f;
				float speedY = 0f;
				int num159 = 0;
				int damage3 = 0;
				switch (num158)
				{
				case 0:
				case 1:
				case 2:
					if (CheckMech(i, j, 200))
					{
						int num167 = ((tile.frameX == 0) ? (-1) : ((tile.frameX == 18) ? 1 : 0));
						int num168 = ((tile.frameX >= 36) ? ((tile.frameX >= 72) ? 1 : (-1)) : 0);
						vector3 = new Vector2(i * 16 + 8 + 10 * num167, j * 16 + 8 + 10 * num168);
						float num169 = 3f;
						if (num158 == 0)
						{
							num159 = 98;
							damage3 = 20;
							num169 = 12f;
						}
						if (num158 == 1)
						{
							num159 = 184;
							damage3 = 40;
							num169 = 12f;
						}
						if (num158 == 2)
						{
							num159 = 187;
							damage3 = 40;
							num169 = 5f;
						}
						speedX = (float)num167 * num169;
						speedY = (float)num168 * num169;
					}
					break;
				case 3:
				{
					if (!CheckMech(i, j, 300))
					{
						break;
					}
					int num162 = 200;
					for (int num163 = 0; num163 < 1000; num163++)
					{
						if (Main.projectile[num163].active && Main.projectile[num163].type == num159)
						{
							float num164 = (new Vector2(i * 16 + 8, j * 18 + 8) - Main.projectile[num163].Center).Length();
							num162 = ((!(num164 < 50f)) ? ((!(num164 < 100f)) ? ((!(num164 < 200f)) ? ((!(num164 < 300f)) ? ((!(num164 < 400f)) ? ((!(num164 < 500f)) ? ((!(num164 < 700f)) ? ((!(num164 < 900f)) ? ((!(num164 < 1200f)) ? (num162 - 1) : (num162 - 2)) : (num162 - 3)) : (num162 - 4)) : (num162 - 5)) : (num162 - 6)) : (num162 - 8)) : (num162 - 10)) : (num162 - 15)) : (num162 - 50));
						}
					}
					if (num162 > 0)
					{
						num159 = 185;
						damage3 = 40;
						int num165 = 0;
						int num166 = 0;
						switch (tile.frameX / 18)
						{
						case 0:
						case 1:
							num165 = 0;
							num166 = 1;
							break;
						case 2:
							num165 = 0;
							num166 = -1;
							break;
						case 3:
							num165 = -1;
							num166 = 0;
							break;
						case 4:
							num165 = 1;
							num166 = 0;
							break;
						}
						speedX = (float)(4 * num165) + (float)Main.rand.Next(-20 + ((num165 == 1) ? 20 : 0), 21 - ((num165 == -1) ? 20 : 0)) * 0.05f;
						speedY = (float)(4 * num166) + (float)Main.rand.Next(-20 + ((num166 == 1) ? 20 : 0), 21 - ((num166 == -1) ? 20 : 0)) * 0.05f;
						vector3 = new Vector2(i * 16 + 8 + 14 * num165, j * 16 + 8 + 14 * num166);
					}
					break;
				}
				case 4:
					if (CheckMech(i, j, 90))
					{
						int num160 = 0;
						int num161 = 0;
						switch (tile.frameX / 18)
						{
						case 0:
						case 1:
							num160 = 0;
							num161 = 1;
							break;
						case 2:
							num160 = 0;
							num161 = -1;
							break;
						case 3:
							num160 = -1;
							num161 = 0;
							break;
						case 4:
							num160 = 1;
							num161 = 0;
							break;
						}
						speedX = 8 * num160;
						speedY = 8 * num161;
						damage3 = 60;
						num159 = 186;
						vector3 = new Vector2(i * 16 + 8 + 18 * num160, j * 16 + 8 + 18 * num161);
					}
					break;
				}
				switch (num158)
				{
				case -10:
					if (CheckMech(i, j, 200))
					{
						int num174 = -1;
						if (tile.frameX != 0)
						{
							num174 = 1;
						}
						speedX = 12 * num174;
						damage3 = 20;
						num159 = 98;
						vector3 = new Vector2(i * 16 + 8, j * 16 + 7);
						vector3.X += 10 * num174;
						vector3.Y += 2f;
					}
					break;
				case -9:
					if (CheckMech(i, j, 200))
					{
						int num170 = -1;
						if (tile.frameX != 0)
						{
							num170 = 1;
						}
						speedX = 12 * num170;
						damage3 = 40;
						num159 = 184;
						vector3 = new Vector2(i * 16 + 8, j * 16 + 7);
						vector3.X += 10 * num170;
						vector3.Y += 2f;
					}
					break;
				case -8:
					if (CheckMech(i, j, 200))
					{
						int num175 = -1;
						if (tile.frameX != 0)
						{
							num175 = 1;
						}
						speedX = 5 * num175;
						damage3 = 40;
						num159 = 187;
						vector3 = new Vector2(i * 16 + 8, j * 16 + 7);
						vector3.X += 10 * num175;
						vector3.Y += 2f;
					}
					break;
				case -7:
				{
					if (!CheckMech(i, j, 300))
					{
						break;
					}
					num159 = 185;
					int num171 = 200;
					for (int num172 = 0; num172 < 1000; num172++)
					{
						if (Main.projectile[num172].active && Main.projectile[num172].type == num159)
						{
							float num173 = (new Vector2(i * 16 + 8, j * 18 + 8) - Main.projectile[num172].Center).Length();
							num171 = ((!(num173 < 50f)) ? ((!(num173 < 100f)) ? ((!(num173 < 200f)) ? ((!(num173 < 300f)) ? ((!(num173 < 400f)) ? ((!(num173 < 500f)) ? ((!(num173 < 700f)) ? ((!(num173 < 900f)) ? ((!(num173 < 1200f)) ? (num171 - 1) : (num171 - 2)) : (num171 - 3)) : (num171 - 4)) : (num171 - 5)) : (num171 - 6)) : (num171 - 8)) : (num171 - 10)) : (num171 - 15)) : (num171 - 50));
						}
					}
					if (num171 > 0)
					{
						speedX = (float)Main.rand.Next(-20, 21) * 0.05f;
						speedY = 4f + (float)Main.rand.Next(0, 21) * 0.05f;
						damage3 = 40;
						vector3 = new Vector2(i * 16 + 8, j * 16 + 16);
						vector3.Y += 6f;
						Projectile.NewProjectile((int)vector3.X, (int)vector3.Y, speedX, speedY, num159, damage3, 2f, Main.myPlayer);
					}
					break;
				}
				case -6:
					if (CheckMech(i, j, 90))
					{
						speedX = 0f;
						speedY = 8f;
						damage3 = 60;
						num159 = 186;
						vector3 = new Vector2(i * 16 + 8, j * 16 + 16);
						vector3.Y += 10f;
					}
					break;
				}
				if (num159 != 0)
				{
					Projectile.NewProjectile((int)vector3.X, (int)vector3.Y, speedX, speedY, num159, damage3, 2f, Main.myPlayer);
				}
				break;
			}
			case 443:
				GeyserTrap(i, j);
				break;
			case 531:
			{
				int num151 = tile.frameX / 36;
				int num152 = tile.frameY / 54;
				int num153 = i - (tile.frameX - num151 * 36) / 18;
				int num154 = j - (tile.frameY - num152 * 54) / 18;
				if (CheckMech(num153, num154, 900))
				{
					Vector2 vector2 = new Vector2(num153 + 1, num154) * 16f;
					vector2.Y += 28f;
					int num155 = 99;
					int damage2 = 70;
					float knockBack2 = 10f;
					if (num155 != 0)
					{
						Projectile.NewProjectile((int)vector2.X, (int)vector2.Y, 0f, 0f, num155, damage2, knockBack2, Main.myPlayer);
					}
				}
				break;
			}
			case 35:
			case 139:
				WorldGen.SwitchMB(i, j);
				break;
			case 207:
				WorldGen.SwitchFountain(i, j);
				break;
			case 410:
			case 480:
			case 509:
				WorldGen.SwitchMonolith(i, j);
				break;
			case 455:
				BirthdayParty.ToggleManualParty();
				break;
			case 141:
				WorldGen.KillTile(i, j, fail: false, effectOnly: false, noItem: true);
				NetMessage.SendTileSquare(-1, i, j, 1);
				Projectile.NewProjectile(i * 16 + 8, j * 16 + 8, 0f, 0f, 108, 500, 10f, Main.myPlayer);
				break;
			case 210:
				WorldGen.ExplodeMine(i, j);
				break;
			case 142:
			case 143:
			{
				int num115 = j - tile.frameY / 18;
				int num116 = tile.frameX / 18;
				if (num116 > 1)
				{
					num116 -= 2;
				}
				num116 = i - num116;
				SkipWire(num116, num115);
				SkipWire(num116, num115 + 1);
				SkipWire(num116 + 1, num115);
				SkipWire(num116 + 1, num115 + 1);
				if (type == 142)
				{
					for (int num117 = 0; num117 < 4; num117++)
					{
						if (_numInPump >= 19)
						{
							break;
						}
						int num118;
						int num119;
						switch (num117)
						{
						case 0:
							num118 = num116;
							num119 = num115 + 1;
							break;
						case 1:
							num118 = num116 + 1;
							num119 = num115 + 1;
							break;
						case 2:
							num118 = num116;
							num119 = num115;
							break;
						default:
							num118 = num116 + 1;
							num119 = num115;
							break;
						}
						_inPumpX[_numInPump] = num118;
						_inPumpY[_numInPump] = num119;
						_numInPump++;
					}
					break;
				}
				for (int num120 = 0; num120 < 4; num120++)
				{
					if (_numOutPump >= 19)
					{
						break;
					}
					int num118;
					int num119;
					switch (num120)
					{
					case 0:
						num118 = num116;
						num119 = num115 + 1;
						break;
					case 1:
						num118 = num116 + 1;
						num119 = num115 + 1;
						break;
					case 2:
						num118 = num116;
						num119 = num115;
						break;
					default:
						num118 = num116 + 1;
						num119 = num115;
						break;
					}
					_outPumpX[_numOutPump] = num118;
					_outPumpY[_numOutPump] = num119;
					_numOutPump++;
				}
				break;
			}
			case 105:
			{
				int num133 = j - tile.frameY / 18;
				int num134 = tile.frameX / 18;
				int num135 = 0;
				while (num134 >= 2)
				{
					num134 -= 2;
					num135++;
				}
				num134 = i - num134;
				num134 = i - tile.frameX % 36 / 18;
				num133 = j - tile.frameY % 54 / 18;
				int num136 = tile.frameY / 54;
				num136 %= 3;
				num135 = tile.frameX / 36 + num136 * 55;
				SkipWire(num134, num133);
				SkipWire(num134, num133 + 1);
				SkipWire(num134, num133 + 2);
				SkipWire(num134 + 1, num133);
				SkipWire(num134 + 1, num133 + 1);
				SkipWire(num134 + 1, num133 + 2);
				int num137 = num134 * 16 + 16;
				int num138 = (num133 + 3) * 16;
				int num139 = -1;
				int num140 = -1;
				bool flag11 = true;
				bool flag12 = false;
				switch (num135)
				{
				case 5:
					num140 = 73;
					break;
				case 13:
					num140 = 24;
					break;
				case 30:
					num140 = 6;
					break;
				case 35:
					num140 = 2;
					break;
				case 51:
					num140 = Utils.SelectRandom(Main.rand, new short[2]
					{
						299,
						538
					});
					break;
				case 52:
					num140 = 356;
					break;
				case 53:
					num140 = 357;
					break;
				case 54:
					num140 = Utils.SelectRandom(Main.rand, new short[2]
					{
						355,
						358
					});
					break;
				case 55:
					num140 = Utils.SelectRandom(Main.rand, new short[2]
					{
						367,
						366
					});
					break;
				case 56:
					num140 = Utils.SelectRandom(Main.rand, new short[5]
					{
						359,
						359,
						359,
						359,
						360
					});
					break;
				case 57:
					num140 = 377;
					break;
				case 58:
					num140 = 300;
					break;
				case 59:
					num140 = Utils.SelectRandom(Main.rand, new short[2]
					{
						364,
						362
					});
					break;
				case 60:
					num140 = 148;
					break;
				case 61:
					num140 = 361;
					break;
				case 62:
					num140 = Utils.SelectRandom(Main.rand, new short[3]
					{
						487,
						486,
						485
					});
					break;
				case 63:
					num140 = 164;
					flag11 &= NPC.MechSpawn(num137, num138, 165);
					break;
				case 64:
					num140 = 86;
					flag12 = true;
					break;
				case 65:
					num140 = 490;
					break;
				case 66:
					num140 = 82;
					break;
				case 67:
					num140 = 449;
					break;
				case 68:
					num140 = 167;
					break;
				case 69:
					num140 = 480;
					break;
				case 70:
					num140 = 48;
					break;
				case 71:
					num140 = Utils.SelectRandom(Main.rand, new short[3]
					{
						170,
						180,
						171
					});
					flag12 = true;
					break;
				case 72:
					num140 = 481;
					break;
				case 73:
					num140 = 482;
					break;
				case 74:
					num140 = 430;
					break;
				case 75:
					num140 = 489;
					break;
				case 76:
					num140 = 611;
					break;
				case 77:
					num140 = 602;
					break;
				case 78:
					num140 = Utils.SelectRandom(Main.rand, new short[6]
					{
						595,
						596,
						599,
						597,
						600,
						598
					});
					break;
				case 79:
					num140 = Utils.SelectRandom(Main.rand, new short[2]
					{
						616,
						617
					});
					break;
				}
				if (num140 != -1 && CheckMech(num134, num133, 30) && NPC.MechSpawn(num137, num138, num140) && flag11)
				{
					if (!flag12 || !Collision.SolidTiles(num134 - 2, num134 + 3, num133, num133 + 2))
					{
						num139 = NPC.NewNPC(num137, num138, num140);
					}
					else
					{
						Vector2 position = new Vector2(num137 - 4, num138 - 22) - new Vector2(10f);
						Utils.PoofOfSmoke(position);
						NetMessage.SendData(106, -1, -1, null, (int)position.X, position.Y);
					}
				}
				if (num139 <= -1)
				{
					switch (num135)
					{
					case 4:
						if (CheckMech(num134, num133, 30) && NPC.MechSpawn(num137, num138, 1))
						{
							num139 = NPC.NewNPC(num137, num138 - 12, 1);
						}
						break;
					case 7:
						if (CheckMech(num134, num133, 30) && NPC.MechSpawn(num137, num138, 49))
						{
							num139 = NPC.NewNPC(num137 - 4, num138 - 6, 49);
						}
						break;
					case 8:
						if (CheckMech(num134, num133, 30) && NPC.MechSpawn(num137, num138, 55))
						{
							num139 = NPC.NewNPC(num137, num138 - 12, 55);
						}
						break;
					case 9:
					{
						int type3 = 46;
						if (BirthdayParty.PartyIsUp)
						{
							type3 = 540;
						}
						if (CheckMech(num134, num133, 30) && NPC.MechSpawn(num137, num138, type3))
						{
							num139 = NPC.NewNPC(num137, num138 - 12, type3);
						}
						break;
					}
					case 10:
						if (CheckMech(num134, num133, 30) && NPC.MechSpawn(num137, num138, 21))
						{
							num139 = NPC.NewNPC(num137, num138, 21);
						}
						break;
					case 16:
						if (CheckMech(num134, num133, 30) && NPC.MechSpawn(num137, num138, 42))
						{
							if (!Collision.SolidTiles(num134 - 1, num134 + 1, num133, num133 + 1))
							{
								num139 = NPC.NewNPC(num137, num138 - 12, 42);
								break;
							}
							Vector2 position3 = new Vector2(num137 - 4, num138 - 22) - new Vector2(10f);
							Utils.PoofOfSmoke(position3);
							NetMessage.SendData(106, -1, -1, null, (int)position3.X, position3.Y);
						}
						break;
					case 18:
						if (CheckMech(num134, num133, 30) && NPC.MechSpawn(num137, num138, 67))
						{
							num139 = NPC.NewNPC(num137, num138 - 12, 67);
						}
						break;
					case 23:
						if (CheckMech(num134, num133, 30) && NPC.MechSpawn(num137, num138, 63))
						{
							num139 = NPC.NewNPC(num137, num138 - 12, 63);
						}
						break;
					case 27:
						if (CheckMech(num134, num133, 30) && NPC.MechSpawn(num137, num138, 85))
						{
							num139 = NPC.NewNPC(num137 - 9, num138, 85);
						}
						break;
					case 28:
						if (CheckMech(num134, num133, 30) && NPC.MechSpawn(num137, num138, 74))
						{
							num139 = NPC.NewNPC(num137, num138 - 12, Utils.SelectRandom(Main.rand, new short[3]
							{
								74,
								297,
								298
							}));
						}
						break;
					case 34:
					{
						for (int num149 = 0; num149 < 2; num149++)
						{
							for (int num150 = 0; num150 < 3; num150++)
							{
								Tile tile2 = Main.tile[num134 + num149, num133 + num150];
								tile2.type = 349;
								tile2.frameX = (short)(num149 * 18 + 216);
								tile2.frameY = (short)(num150 * 18);
							}
						}
						Animation.NewTemporaryAnimation(0, 349, num134, num133);
						if (Main.netMode == 2)
						{
							NetMessage.SendTileRange(-1, num134, num133, 2, 3);
						}
						break;
					}
					case 42:
						if (CheckMech(num134, num133, 30) && NPC.MechSpawn(num137, num138, 58))
						{
							num139 = NPC.NewNPC(num137, num138 - 12, 58);
						}
						break;
					case 37:
						if (CheckMech(num134, num133, 600) && Item.MechSpawn(num137, num138, 58) && Item.MechSpawn(num137, num138, 1734) && Item.MechSpawn(num137, num138, 1867))
						{
							Item.NewItem(num137, num138 - 16, 0, 0, 58);
						}
						break;
					case 50:
						if (CheckMech(num134, num133, 30) && NPC.MechSpawn(num137, num138, 65))
						{
							if (!Collision.SolidTiles(num134 - 2, num134 + 3, num133, num133 + 2))
							{
								num139 = NPC.NewNPC(num137, num138 - 12, 65);
								break;
							}
							Vector2 position2 = new Vector2(num137 - 4, num138 - 22) - new Vector2(10f);
							Utils.PoofOfSmoke(position2);
							NetMessage.SendData(106, -1, -1, null, (int)position2.X, position2.Y);
						}
						break;
					case 2:
						if (CheckMech(num134, num133, 600) && Item.MechSpawn(num137, num138, 184) && Item.MechSpawn(num137, num138, 1735) && Item.MechSpawn(num137, num138, 1868))
						{
							Item.NewItem(num137, num138 - 16, 0, 0, 184);
						}
						break;
					case 17:
						if (CheckMech(num134, num133, 600) && Item.MechSpawn(num137, num138, 166))
						{
							Item.NewItem(num137, num138 - 20, 0, 0, 166);
						}
						break;
					case 40:
					{
						if (!CheckMech(num134, num133, 300))
						{
							break;
						}
						int num145 = 50;
						int[] array2 = new int[num145];
						int num146 = 0;
						for (int num147 = 0; num147 < 200; num147++)
						{
							if (Main.npc[num147].active && (Main.npc[num147].type == 17 || Main.npc[num147].type == 19 || Main.npc[num147].type == 22 || Main.npc[num147].type == 38 || Main.npc[num147].type == 54 || Main.npc[num147].type == 107 || Main.npc[num147].type == 108 || Main.npc[num147].type == 142 || Main.npc[num147].type == 160 || Main.npc[num147].type == 207 || Main.npc[num147].type == 209 || Main.npc[num147].type == 227 || Main.npc[num147].type == 228 || Main.npc[num147].type == 229 || Main.npc[num147].type == 368 || Main.npc[num147].type == 369 || Main.npc[num147].type == 550 || Main.npc[num147].type == 441 || Main.npc[num147].type == 588))
							{
								array2[num146] = num147;
								num146++;
								if (num146 >= num145)
								{
									break;
								}
							}
						}
						if (num146 > 0)
						{
							int num148 = array2[Main.rand.Next(num146)];
							Main.npc[num148].position.X = num137 - Main.npc[num148].width / 2;
							Main.npc[num148].position.Y = num138 - Main.npc[num148].height - 1;
							NetMessage.SendData(23, -1, -1, null, num148);
						}
						break;
					}
					case 41:
					{
						if (!CheckMech(num134, num133, 300))
						{
							break;
						}
						int num141 = 50;
						int[] array = new int[num141];
						int num142 = 0;
						for (int num143 = 0; num143 < 200; num143++)
						{
							if (Main.npc[num143].active && (Main.npc[num143].type == 18 || Main.npc[num143].type == 20 || Main.npc[num143].type == 124 || Main.npc[num143].type == 178 || Main.npc[num143].type == 208 || Main.npc[num143].type == 353 || Main.npc[num143].type == 633))
							{
								array[num142] = num143;
								num142++;
								if (num142 >= num141)
								{
									break;
								}
							}
						}
						if (num142 > 0)
						{
							int num144 = array[Main.rand.Next(num142)];
							Main.npc[num144].position.X = num137 - Main.npc[num144].width / 2;
							Main.npc[num144].position.Y = num138 - Main.npc[num144].height - 1;
							NetMessage.SendData(23, -1, -1, null, num144);
						}
						break;
					}
					}
				}
				if (num139 >= 0)
				{
					Main.npc[num139].value = 0f;
					Main.npc[num139].npcSlots = 0f;
					Main.npc[num139].SpawnedFromStatue = true;
				}
				break;
			}
			case 349:
			{
				int num109 = tile.frameY / 18;
				num109 %= 3;
				int num110 = j - num109;
				int num111;
				for (num111 = tile.frameX / 18; num111 >= 2; num111 -= 2)
				{
				}
				num111 = i - num111;
				SkipWire(num111, num110);
				SkipWire(num111, num110 + 1);
				SkipWire(num111, num110 + 2);
				SkipWire(num111 + 1, num110);
				SkipWire(num111 + 1, num110 + 1);
				SkipWire(num111 + 1, num110 + 2);
				short num112 = (short)((Main.tile[num111, num110].frameX != 0) ? (-216) : 216);
				for (int num113 = 0; num113 < 2; num113++)
				{
					for (int num114 = 0; num114 < 3; num114++)
					{
						Main.tile[num111 + num113, num110 + num114].frameX += num112;
					}
				}
				if (Main.netMode == 2)
				{
					NetMessage.SendTileRange(-1, num111, num110, 2, 3);
				}
				Animation.NewTemporaryAnimation((num112 <= 0) ? 1 : 0, 349, num111, num110);
				break;
			}
			case 506:
			{
				int num71 = tile.frameY / 18;
				num71 %= 3;
				int num72 = j - num71;
				int num73;
				for (num73 = tile.frameX / 18; num73 >= 2; num73 -= 2)
				{
				}
				num73 = i - num73;
				SkipWire(num73, num72);
				SkipWire(num73, num72 + 1);
				SkipWire(num73, num72 + 2);
				SkipWire(num73 + 1, num72);
				SkipWire(num73 + 1, num72 + 1);
				SkipWire(num73 + 1, num72 + 2);
				short num74 = (short)((Main.tile[num73, num72].frameX >= 72) ? (-72) : 72);
				for (int num75 = 0; num75 < 2; num75++)
				{
					for (int num76 = 0; num76 < 3; num76++)
					{
						Main.tile[num73 + num75, num72 + num76].frameX += num74;
					}
				}
				if (Main.netMode == 2)
				{
					NetMessage.SendTileRange(-1, num73, num72, 2, 3);
				}
				break;
			}
			case 546:
				tile.type = 557;
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j, 1);
				break;
			case 557:
				tile.type = 546;
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j, 1);
				break;
			}
		}

		private static void GeyserTrap(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			if (tile.type != 443)
			{
				return;
			}
			int num = tile.frameX / 36;
			int num2 = i - (tile.frameX - num * 36) / 18;
			if (CheckMech(num2, j, 200))
			{
				Vector2 zero = Vector2.Zero;
				Vector2 zero2 = Vector2.Zero;
				int num3 = 654;
				int damage = 20;
				if (num < 2)
				{
					zero = new Vector2(num2 + 1, j) * 16f;
					zero2 = new Vector2(0f, -8f);
				}
				else
				{
					zero = new Vector2(num2 + 1, j + 1) * 16f;
					zero2 = new Vector2(0f, 8f);
				}
				if (num3 != 0)
				{
					Projectile.NewProjectile((int)zero.X, (int)zero.Y, zero2.X, zero2.Y, num3, damage, 2f, Main.myPlayer);
				}
			}
		}

		private static void Teleport()
		{
			if (_teleport[0].X < _teleport[1].X + 3f && _teleport[0].X > _teleport[1].X - 3f && _teleport[0].Y > _teleport[1].Y - 3f && _teleport[0].Y < _teleport[1].Y)
			{
				return;
			}
			Rectangle[] array = new Rectangle[2];
			array[0].X = (int)(_teleport[0].X * 16f);
			array[0].Width = 48;
			array[0].Height = 48;
			array[0].Y = (int)(_teleport[0].Y * 16f - (float)array[0].Height);
			array[1].X = (int)(_teleport[1].X * 16f);
			array[1].Width = 48;
			array[1].Height = 48;
			array[1].Y = (int)(_teleport[1].Y * 16f - (float)array[1].Height);
			for (int i = 0; i < 2; i++)
			{
				Vector2 value = new Vector2(array[1].X - array[0].X, array[1].Y - array[0].Y);
				if (i == 1)
				{
					value = new Vector2(array[0].X - array[1].X, array[0].Y - array[1].Y);
				}
				if (!blockPlayerTeleportationForOneIteration)
				{
					for (int j = 0; j < 255; j++)
					{
						if (Main.player[j].active && !Main.player[j].dead && !Main.player[j].teleporting && TeleporterHitboxIntersects(array[i], Main.player[j].Hitbox))
						{
							Vector2 vector = Main.player[j].position + value;
							Main.player[j].teleporting = true;
							if (Main.netMode == 2)
							{
								RemoteClient.CheckSection(j, vector);
							}
							Main.player[j].Teleport(vector);
							if (Main.netMode == 2)
							{
								NetMessage.SendData(65, -1, -1, null, 0, j, vector.X, vector.Y);
							}
						}
					}
				}
				for (int k = 0; k < 200; k++)
				{
					if (Main.npc[k].active && !Main.npc[k].teleporting && Main.npc[k].lifeMax > 5 && !Main.npc[k].boss && !Main.npc[k].noTileCollide)
					{
						int type = Main.npc[k].type;
						if (!NPCID.Sets.TeleportationImmune[type] && TeleporterHitboxIntersects(array[i], Main.npc[k].Hitbox))
						{
							Main.npc[k].teleporting = true;
							Main.npc[k].Teleport(Main.npc[k].position + value);
						}
					}
				}
			}
			for (int l = 0; l < 255; l++)
			{
				Main.player[l].teleporting = false;
			}
			for (int m = 0; m < 200; m++)
			{
				Main.npc[m].teleporting = false;
			}
		}

		private static bool TeleporterHitboxIntersects(Rectangle teleporter, Rectangle entity)
		{
			Rectangle rectangle = Rectangle.Union(teleporter, entity);
			if (rectangle.Width <= teleporter.Width + entity.Width)
			{
				return rectangle.Height <= teleporter.Height + entity.Height;
			}
			return false;
		}

		private static void DeActive(int i, int j)
		{
			if (!Main.tile[i, j].active() || (Main.tile[i, j].type == 226 && (double)j > Main.worldSurface && !NPC.downedPlantBoss))
			{
				return;
			}
			bool flag = Main.tileSolid[Main.tile[i, j].type] && !TileID.Sets.NotReallySolid[Main.tile[i, j].type];
			ushort type = Main.tile[i, j].type;
			if (type == 314 || (uint)(type - 386) <= 3u || type == 476)
			{
				flag = false;
			}
			if (flag && (!Main.tile[i, j - 1].active() || (!TileID.Sets.BasicChest[Main.tile[i, j - 1].type] && Main.tile[i, j - 1].type != 26 && Main.tile[i, j - 1].type != 77 && Main.tile[i, j - 1].type != 88 && Main.tile[i, j - 1].type != 470 && Main.tile[i, j - 1].type != 475 && Main.tile[i, j - 1].type != 237 && Main.tile[i, j - 1].type != 597 && WorldGen.CanKillTile(i, j - 1))))
			{
				Main.tile[i, j].inActive(inActive: true);
				WorldGen.SquareTileFrame(i, j, resetFrame: false);
				if (Main.netMode != 1)
				{
					NetMessage.SendTileSquare(-1, i, j, 1);
				}
			}
		}

		private static void ReActive(int i, int j)
		{
			Main.tile[i, j].inActive(inActive: false);
			WorldGen.SquareTileFrame(i, j, resetFrame: false);
			if (Main.netMode != 1)
			{
				NetMessage.SendTileSquare(-1, i, j, 1);
			}
		}

		private static void MassWireOperationInner(Point ps, Point pe, Vector2 dropPoint, bool dir, int wireCount, int actuatorCount)
		{
			Math.Abs(ps.X - pe.X);
			Math.Abs(ps.Y - pe.Y);
			int num = Math.Sign(pe.X - ps.X);
			int num2 = Math.Sign(pe.Y - ps.Y);
			WiresUI.Settings.MultiToolMode toolMode = WiresUI.Settings.ToolMode;
			Point pt = default(Point);
			bool flag = false;
			Item.StartCachingType(530);
			Item.StartCachingType(849);
			bool flag2 = dir;
			int num3;
			int num4;
			int num5;
			if (flag2)
			{
				pt.X = ps.X;
				num3 = ps.Y;
				num4 = pe.Y;
				num5 = num2;
			}
			else
			{
				pt.Y = ps.Y;
				num3 = ps.X;
				num4 = pe.X;
				num5 = num;
			}
			for (int i = num3; i != num4; i += num5)
			{
				if (flag)
				{
					break;
				}
				if (flag2)
				{
					pt.Y = i;
				}
				else
				{
					pt.X = i;
				}
				bool? flag3 = MassWireOperationStep(pt, toolMode, wireCount, actuatorCount);
				if (flag3.HasValue && !flag3.Value)
				{
					flag = true;
					break;
				}
			}
			if (flag2)
			{
				pt.Y = pe.Y;
				num3 = ps.X;
				num4 = pe.X;
				num5 = num;
			}
			else
			{
				pt.X = pe.X;
				num3 = ps.Y;
				num4 = pe.Y;
				num5 = num2;
			}
			for (int j = num3; j != num4; j += num5)
			{
				if (flag)
				{
					break;
				}
				if (!flag2)
				{
					pt.Y = j;
				}
				else
				{
					pt.X = j;
				}
				bool? flag4 = MassWireOperationStep(pt, toolMode, wireCount, actuatorCount);
				if (flag4.HasValue && !flag4.Value)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				MassWireOperationStep(pe, toolMode, wireCount, actuatorCount);
			}
			Item.DropCache(dropPoint, Vector2.Zero, 530);
			Item.DropCache(dropPoint, Vector2.Zero, 849);
		}

		private static bool? MassWireOperationStep(Point pt, WiresUI.Settings.MultiToolMode mode, int wiresLeftToConsume, int actuatorsLeftToConstume)
		{
			if (!WorldGen.InWorld(pt.X, pt.Y, 1))
			{
				return null;
			}
			Tile tile = Main.tile[pt.X, pt.Y];
			if (tile == null)
			{
				return null;
			}
			if (!mode.HasFlag(WiresUI.Settings.MultiToolMode.Cutter))
			{
				if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Red) && !tile.wire())
				{
					if (wiresLeftToConsume <= 0)
					{
						return false;
					}
					wiresLeftToConsume--;
					WorldGen.PlaceWire(pt.X, pt.Y);
					NetMessage.SendData(17, -1, -1, null, 5, pt.X, pt.Y);
				}
				if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Green) && !tile.wire3())
				{
					if (wiresLeftToConsume <= 0)
					{
						return false;
					}
					wiresLeftToConsume--;
					WorldGen.PlaceWire3(pt.X, pt.Y);
					NetMessage.SendData(17, -1, -1, null, 12, pt.X, pt.Y);
				}
				if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Blue) && !tile.wire2())
				{
					if (wiresLeftToConsume <= 0)
					{
						return false;
					}
					wiresLeftToConsume--;
					WorldGen.PlaceWire2(pt.X, pt.Y);
					NetMessage.SendData(17, -1, -1, null, 10, pt.X, pt.Y);
				}
				if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Yellow) && !tile.wire4())
				{
					if (wiresLeftToConsume <= 0)
					{
						return false;
					}
					wiresLeftToConsume--;
					WorldGen.PlaceWire4(pt.X, pt.Y);
					NetMessage.SendData(17, -1, -1, null, 16, pt.X, pt.Y);
				}
				if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Actuator) && !tile.actuator())
				{
					if (actuatorsLeftToConstume <= 0)
					{
						return false;
					}
					actuatorsLeftToConstume--;
					WorldGen.PlaceActuator(pt.X, pt.Y);
					NetMessage.SendData(17, -1, -1, null, 8, pt.X, pt.Y);
				}
			}
			if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Cutter))
			{
				if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Red) && tile.wire() && WorldGen.KillWire(pt.X, pt.Y))
				{
					NetMessage.SendData(17, -1, -1, null, 6, pt.X, pt.Y);
				}
				if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Green) && tile.wire3() && WorldGen.KillWire3(pt.X, pt.Y))
				{
					NetMessage.SendData(17, -1, -1, null, 13, pt.X, pt.Y);
				}
				if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Blue) && tile.wire2() && WorldGen.KillWire2(pt.X, pt.Y))
				{
					NetMessage.SendData(17, -1, -1, null, 11, pt.X, pt.Y);
				}
				if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Yellow) && tile.wire4() && WorldGen.KillWire4(pt.X, pt.Y))
				{
					NetMessage.SendData(17, -1, -1, null, 17, pt.X, pt.Y);
				}
				if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Actuator) && tile.actuator() && WorldGen.KillActuator(pt.X, pt.Y))
				{
					NetMessage.SendData(17, -1, -1, null, 9, pt.X, pt.Y);
				}
			}
			return true;
		}
	}
}
