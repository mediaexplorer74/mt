using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Ionic.Zlib;
using Microsoft.Xna.Framework;
using GameManager.IO;
using GameManager.Social;
using GameManager.Utilities;

namespace GameManager.Map
{
	public static class MapHelper
	{
		private struct OldMapHelper
		{
			public byte misc;

			public byte misc2;

			public bool active()
			{
				if ((misc & 1) == 1)
				{
					return true;
				}
				return false;
			}

			public bool water()
			{
				if ((misc & 2) == 2)
				{
					return true;
				}
				return false;
			}

			public bool lava()
			{
				if ((misc & 4) == 4)
				{
					return true;
				}
				return false;
			}

			public bool honey()
			{
				if ((misc2 & 0x40) == 64)
				{
					return true;
				}
				return false;
			}

			public bool changed()
			{
				if ((misc & 8) == 8)
				{
					return true;
				}
				return false;
			}

			public bool wall()
			{
				if ((misc & 0x10) == 16)
				{
					return true;
				}
				return false;
			}

			public byte option()
			{
				byte b = 0;
				if ((misc & 0x20) == 32)
				{
					b = (byte)(b + 1);
				}
				if ((misc & 0x40) == 64)
				{
					b = (byte)(b + 2);
				}
				if ((misc & 0x80) == 128)
				{
					b = (byte)(b + 4);
				}
				if ((misc2 & 1) == 1)
				{
					b = (byte)(b + 8);
				}
				return b;
			}

			public byte color()
			{
				return (byte)((misc2 & 0x1E) >> 1);
			}
		}

		public const int drawLoopMilliseconds = 5;

		private const int HeaderEmpty = 0;

		private const int HeaderTile = 1;

		private const int HeaderWall = 2;

		private const int HeaderWater = 3;

		private const int HeaderLava = 4;

		private const int HeaderHoney = 5;

		private const int HeaderHeavenAndHell = 6;

		private const int HeaderBackground = 7;

		private const int maxTileOptions = 12;

		private const int maxWallOptions = 2;

		private const int maxLiquidTypes = 3;

		private const int maxSkyGradients = 256;

		private const int maxDirtGradients = 256;

		private const int maxRockGradients = 256;

		public static int maxUpdateTile = 1000;

		public static int numUpdateTile = 0;

		public static short[] updateTileX = new short[maxUpdateTile];

		public static short[] updateTileY = new short[maxUpdateTile];

		private static object IOLock = new object();

		public static int[] tileOptionCounts;

		public static int[] wallOptionCounts;

		public static ushort[] tileLookup;

		public static ushort[] wallLookup;

		private static ushort tilePosition;

		private static ushort wallPosition;

		private static ushort liquidPosition;

		private static ushort skyPosition;

		private static ushort dirtPosition;

		private static ushort rockPosition;

		private static ushort hellPosition;

		private static Color[] colorLookup;

		private static ushort[] snowTypes;

		private static ushort wallRangeStart;

		private static ushort wallRangeEnd;

		public static bool noStatusText = false;

		public static void Initialize()
		{
			Color[][] array = new Color[623][];
			for (int i = 0; i < 623; i++)
			{
				array[i] = new Color[12];
			}
			array[621][0] = new Color(250, 250, 250);
			array[622][0] = new Color(235, 235, 249);
			array[518][0] = new Color(26, 196, 84);
			array[518][1] = new Color(48, 208, 234);
			array[518][2] = new Color(135, 196, 26);
			array[519][0] = new Color(28, 216, 109);
			array[519][1] = new Color(107, 182, 0);
			array[519][2] = new Color(75, 184, 230);
			array[519][3] = new Color(208, 80, 80);
			array[519][4] = new Color(141, 137, 223);
			array[519][5] = new Color(182, 175, 130);
			array[549][0] = new Color(54, 83, 20);
			array[528][0] = new Color(182, 175, 130);
			array[529][0] = new Color(99, 150, 8);
			array[529][1] = new Color(139, 154, 64);
			array[529][2] = new Color(34, 129, 168);
			array[529][3] = new Color(180, 82, 82);
			array[529][4] = new Color(113, 108, 205);
			Color color = new Color(151, 107, 75);
			array[0][0] = color;
			array[5][0] = color;
			array[5][1] = new Color(182, 175, 130);
			Color color2 = new Color(127, 127, 127);
			array[583][0] = color2;
			array[584][0] = color2;
			array[585][0] = color2;
			array[586][0] = color2;
			array[587][0] = color2;
			array[588][0] = color2;
			array[589][0] = color2;
			array[590][0] = color2;
			array[595][0] = color;
			array[596][0] = color;
			array[615][0] = color;
			array[616][0] = color;
			array[30][0] = color;
			array[191][0] = color;
			array[272][0] = new Color(121, 119, 101);
			color = new Color(128, 128, 128);
			array[1][0] = color;
			array[38][0] = color;
			array[48][0] = color;
			array[130][0] = color;
			array[138][0] = color;
			array[273][0] = color;
			array[283][0] = color;
			array[618][0] = color;
			array[2][0] = new Color(28, 216, 94);
			array[477][0] = new Color(28, 216, 94);
			array[492][0] = new Color(78, 193, 227);
			color = new Color(26, 196, 84);
			array[3][0] = color;
			array[192][0] = color;
			array[73][0] = new Color(27, 197, 109);
			array[52][0] = new Color(23, 177, 76);
			array[353][0] = new Color(28, 216, 94);
			array[20][0] = new Color(163, 116, 81);
			array[6][0] = new Color(140, 101, 80);
			color = new Color(150, 67, 22);
			array[7][0] = color;
			array[47][0] = color;
			array[284][0] = color;
			array[560][0] = color;
			color = new Color(185, 164, 23);
			array[8][0] = color;
			array[45][0] = color;
			array[560][2] = color;
			color = new Color(185, 194, 195);
			array[9][0] = color;
			array[46][0] = color;
			array[560][1] = color;
			color = new Color(98, 95, 167);
			array[22][0] = color;
			array[140][0] = color;
			array[23][0] = new Color(141, 137, 223);
			array[24][0] = new Color(122, 116, 218);
			array[25][0] = new Color(109, 90, 128);
			array[37][0] = new Color(104, 86, 84);
			array[39][0] = new Color(181, 62, 59);
			array[40][0] = new Color(146, 81, 68);
			array[41][0] = new Color(66, 84, 109);
			array[481][0] = new Color(66, 84, 109);
			array[43][0] = new Color(84, 100, 63);
			array[482][0] = new Color(84, 100, 63);
			array[44][0] = new Color(107, 68, 99);
			array[483][0] = new Color(107, 68, 99);
			array[53][0] = new Color(186, 168, 84);
			color = new Color(190, 171, 94);
			array[151][0] = color;
			array[154][0] = color;
			array[274][0] = color;
			array[328][0] = new Color(200, 246, 254);
			array[329][0] = new Color(15, 15, 15);
			array[54][0] = new Color(200, 246, 254);
			array[56][0] = new Color(43, 40, 84);
			array[75][0] = new Color(26, 26, 26);
			array[57][0] = new Color(68, 68, 76);
			color = new Color(142, 66, 66);
			array[58][0] = color;
			array[76][0] = color;
			color = new Color(92, 68, 73);
			array[59][0] = color;
			array[120][0] = color;
			array[60][0] = new Color(143, 215, 29);
			array[61][0] = new Color(135, 196, 26);
			array[74][0] = new Color(96, 197, 27);
			array[62][0] = new Color(121, 176, 24);
			array[233][0] = new Color(107, 182, 29);
			array[63][0] = new Color(110, 140, 182);
			array[64][0] = new Color(196, 96, 114);
			array[65][0] = new Color(56, 150, 97);
			array[66][0] = new Color(160, 118, 58);
			array[67][0] = new Color(140, 58, 166);
			array[68][0] = new Color(125, 191, 197);
			array[566][0] = new Color(233, 180, 90);
			array[70][0] = new Color(93, 127, 255);
			color = new Color(182, 175, 130);
			array[71][0] = color;
			array[72][0] = color;
			array[190][0] = color;
			array[578][0] = new Color(172, 155, 110);
			color = new Color(73, 120, 17);
			array[80][0] = color;
			array[484][0] = color;
			array[188][0] = color;
			array[80][1] = new Color(87, 84, 151);
			array[80][2] = new Color(34, 129, 168);
			array[80][3] = new Color(130, 56, 55);
			color = new Color(11, 80, 143);
			array[107][0] = color;
			array[121][0] = color;
			color = new Color(91, 169, 169);
			array[108][0] = color;
			array[122][0] = color;
			color = new Color(128, 26, 52);
			array[111][0] = color;
			array[150][0] = color;
			array[109][0] = new Color(78, 193, 227);
			array[110][0] = new Color(48, 186, 135);
			array[113][0] = new Color(48, 208, 234);
			array[115][0] = new Color(33, 171, 207);
			array[112][0] = new Color(103, 98, 122);
			color = new Color(238, 225, 218);
			array[116][0] = color;
			array[118][0] = color;
			array[117][0] = new Color(181, 172, 190);
			array[119][0] = new Color(107, 92, 108);
			array[123][0] = new Color(106, 107, 118);
			array[124][0] = new Color(73, 51, 36);
			array[131][0] = new Color(52, 52, 52);
			array[145][0] = new Color(192, 30, 30);
			array[146][0] = new Color(43, 192, 30);
			color = new Color(211, 236, 241);
			array[147][0] = color;
			array[148][0] = color;
			array[152][0] = new Color(128, 133, 184);
			array[153][0] = new Color(239, 141, 126);
			array[155][0] = new Color(131, 162, 161);
			array[156][0] = new Color(170, 171, 157);
			array[157][0] = new Color(104, 100, 126);
			color = new Color(145, 81, 85);
			array[158][0] = color;
			array[232][0] = color;
			array[575][0] = new Color(125, 61, 65);
			array[159][0] = new Color(148, 133, 98);
			array[160][0] = new Color(200, 0, 0);
			array[160][1] = new Color(0, 200, 0);
			array[160][2] = new Color(0, 0, 200);
			array[161][0] = new Color(144, 195, 232);
			array[162][0] = new Color(184, 219, 240);
			array[163][0] = new Color(174, 145, 214);
			array[164][0] = new Color(218, 182, 204);
			array[170][0] = new Color(27, 109, 69);
			array[171][0] = new Color(33, 135, 85);
			color = new Color(129, 125, 93);
			array[166][0] = color;
			array[175][0] = color;
			array[167][0] = new Color(62, 82, 114);
			color = new Color(132, 157, 127);
			array[168][0] = color;
			array[176][0] = color;
			color = new Color(152, 171, 198);
			array[169][0] = color;
			array[177][0] = color;
			array[179][0] = new Color(49, 134, 114);
			array[180][0] = new Color(126, 134, 49);
			array[181][0] = new Color(134, 59, 49);
			array[182][0] = new Color(43, 86, 140);
			array[183][0] = new Color(121, 49, 134);
			array[381][0] = new Color(254, 121, 2);
			array[534][0] = new Color(114, 254, 2);
			array[536][0] = new Color(0, 197, 208);
			array[539][0] = new Color(208, 0, 126);
			array[512][0] = new Color(49, 134, 114);
			array[513][0] = new Color(126, 134, 49);
			array[514][0] = new Color(134, 59, 49);
			array[515][0] = new Color(43, 86, 140);
			array[516][0] = new Color(121, 49, 134);
			array[517][0] = new Color(254, 121, 2);
			array[535][0] = new Color(114, 254, 2);
			array[537][0] = new Color(0, 197, 208);
			array[540][0] = new Color(208, 0, 126);
			array[184][0] = new Color(29, 106, 88);
			array[184][1] = new Color(94, 100, 36);
			array[184][2] = new Color(96, 44, 40);
			array[184][3] = new Color(34, 63, 102);
			array[184][4] = new Color(79, 35, 95);
			array[184][5] = new Color(253, 62, 3);
			array[184][6] = new Color(22, 123, 62);
			array[184][7] = new Color(0, 106, 148);
			array[184][8] = new Color(148, 0, 132);
			array[189][0] = new Color(223, 255, 255);
			array[193][0] = new Color(56, 121, 255);
			array[194][0] = new Color(157, 157, 107);
			array[195][0] = new Color(134, 22, 34);
			array[196][0] = new Color(147, 144, 178);
			array[197][0] = new Color(97, 200, 225);
			array[198][0] = new Color(62, 61, 52);
			array[199][0] = new Color(208, 80, 80);
			array[201][0] = new Color(203, 61, 64);
			array[205][0] = new Color(186, 50, 52);
			array[200][0] = new Color(216, 152, 144);
			array[202][0] = new Color(213, 178, 28);
			array[203][0] = new Color(128, 44, 45);
			array[204][0] = new Color(125, 55, 65);
			array[206][0] = new Color(124, 175, 201);
			array[208][0] = new Color(88, 105, 118);
			array[211][0] = new Color(191, 233, 115);
			array[213][0] = new Color(137, 120, 67);
			array[214][0] = new Color(103, 103, 103);
			array[221][0] = new Color(239, 90, 50);
			array[222][0] = new Color(231, 96, 228);
			array[223][0] = new Color(57, 85, 101);
			array[224][0] = new Color(107, 132, 139);
			array[225][0] = new Color(227, 125, 22);
			array[226][0] = new Color(141, 56, 0);
			array[229][0] = new Color(255, 156, 12);
			array[230][0] = new Color(131, 79, 13);
			array[234][0] = new Color(53, 44, 41);
			array[235][0] = new Color(214, 184, 46);
			array[236][0] = new Color(149, 232, 87);
			array[237][0] = new Color(255, 241, 51);
			array[238][0] = new Color(225, 128, 206);
			array[243][0] = new Color(198, 196, 170);
			array[248][0] = new Color(219, 71, 38);
			array[249][0] = new Color(235, 38, 231);
			array[250][0] = new Color(86, 85, 92);
			array[251][0] = new Color(235, 150, 23);
			array[252][0] = new Color(153, 131, 44);
			array[253][0] = new Color(57, 48, 97);
			array[254][0] = new Color(248, 158, 92);
			array[255][0] = new Color(107, 49, 154);
			array[256][0] = new Color(154, 148, 49);
			array[257][0] = new Color(49, 49, 154);
			array[258][0] = new Color(49, 154, 68);
			array[259][0] = new Color(154, 49, 77);
			array[260][0] = new Color(85, 89, 118);
			array[261][0] = new Color(154, 83, 49);
			array[262][0] = new Color(221, 79, 255);
			array[263][0] = new Color(250, 255, 79);
			array[264][0] = new Color(79, 102, 255);
			array[265][0] = new Color(79, 255, 89);
			array[266][0] = new Color(255, 79, 79);
			array[267][0] = new Color(240, 240, 247);
			array[268][0] = new Color(255, 145, 79);
			array[287][0] = new Color(79, 128, 17);
			color = new Color(122, 217, 232);
			array[275][0] = color;
			array[276][0] = color;
			array[277][0] = color;
			array[278][0] = color;
			array[279][0] = color;
			array[280][0] = color;
			array[281][0] = color;
			array[282][0] = color;
			array[285][0] = color;
			array[286][0] = color;
			array[288][0] = color;
			array[289][0] = color;
			array[290][0] = color;
			array[291][0] = color;
			array[292][0] = color;
			array[293][0] = color;
			array[294][0] = color;
			array[295][0] = color;
			array[296][0] = color;
			array[297][0] = color;
			array[298][0] = color;
			array[299][0] = color;
			array[309][0] = color;
			array[310][0] = color;
			array[413][0] = color;
			array[339][0] = color;
			array[542][0] = color;
			array[358][0] = color;
			array[359][0] = color;
			array[360][0] = color;
			array[361][0] = color;
			array[362][0] = color;
			array[363][0] = color;
			array[364][0] = color;
			array[391][0] = color;
			array[392][0] = color;
			array[393][0] = color;
			array[394][0] = color;
			array[414][0] = color;
			array[505][0] = color;
			array[543][0] = color;
			array[598][0] = color;
			array[521][0] = color;
			array[522][0] = color;
			array[523][0] = color;
			array[524][0] = color;
			array[525][0] = color;
			array[526][0] = color;
			array[527][0] = color;
			array[532][0] = color;
			array[533][0] = color;
			array[538][0] = color;
			array[544][0] = color;
			array[550][0] = color;
			array[551][0] = color;
			array[553][0] = color;
			array[554][0] = color;
			array[555][0] = color;
			array[556][0] = color;
			array[558][0] = color;
			array[559][0] = color;
			array[580][0] = color;
			array[582][0] = color;
			array[599][0] = color;
			array[600][0] = color;
			array[601][0] = color;
			array[602][0] = color;
			array[603][0] = color;
			array[604][0] = color;
			array[605][0] = color;
			array[606][0] = color;
			array[607][0] = color;
			array[608][0] = color;
			array[609][0] = color;
			array[610][0] = color;
			array[611][0] = color;
			array[612][0] = color;
			array[619][0] = color;
			array[620][0] = color;
			array[552][0] = array[53][0];
			array[564][0] = new Color(87, 127, 220);
			array[408][0] = new Color(85, 83, 82);
			array[409][0] = new Color(85, 83, 82);
			array[415][0] = new Color(249, 75, 7);
			array[416][0] = new Color(0, 160, 170);
			array[417][0] = new Color(160, 87, 234);
			array[418][0] = new Color(22, 173, 254);
			array[489][0] = new Color(255, 29, 136);
			array[490][0] = new Color(211, 211, 211);
			array[311][0] = new Color(117, 61, 25);
			array[312][0] = new Color(204, 93, 73);
			array[313][0] = new Color(87, 150, 154);
			array[4][0] = new Color(253, 221, 3);
			array[4][1] = new Color(253, 221, 3);
			color = new Color(253, 221, 3);
			array[93][0] = color;
			array[33][0] = color;
			array[174][0] = color;
			array[100][0] = color;
			array[98][0] = color;
			array[173][0] = color;
			color = new Color(119, 105, 79);
			array[11][0] = color;
			array[10][0] = color;
			array[593][0] = color;
			array[594][0] = color;
			color = new Color(191, 142, 111);
			array[14][0] = color;
			array[469][0] = color;
			array[486][0] = color;
			array[488][0] = new Color(127, 92, 69);
			array[487][0] = color;
			array[487][1] = color;
			array[15][0] = color;
			array[15][1] = color;
			array[497][0] = color;
			array[18][0] = color;
			array[19][0] = color;
			array[55][0] = color;
			array[79][0] = color;
			array[86][0] = color;
			array[87][0] = color;
			array[88][0] = color;
			array[89][0] = color;
			array[94][0] = color;
			array[101][0] = color;
			array[104][0] = color;
			array[106][0] = color;
			array[114][0] = color;
			array[128][0] = color;
			array[139][0] = color;
			array[172][0] = color;
			array[216][0] = color;
			array[269][0] = color;
			array[334][0] = color;
			array[471][0] = color;
			array[470][0] = color;
			array[475][0] = color;
			array[377][0] = color;
			array[380][0] = color;
			array[395][0] = color;
			array[573][0] = color;
			array[12][0] = new Color(174, 24, 69);
			array[13][0] = new Color(133, 213, 247);
			color = new Color(144, 148, 144);
			array[17][0] = color;
			array[90][0] = color;
			array[96][0] = color;
			array[97][0] = color;
			array[99][0] = color;
			array[132][0] = color;
			array[142][0] = color;
			array[143][0] = color;
			array[144][0] = color;
			array[207][0] = color;
			array[209][0] = color;
			array[212][0] = color;
			array[217][0] = color;
			array[218][0] = color;
			array[219][0] = color;
			array[220][0] = color;
			array[228][0] = color;
			array[300][0] = color;
			array[301][0] = color;
			array[302][0] = color;
			array[303][0] = color;
			array[304][0] = color;
			array[305][0] = color;
			array[306][0] = color;
			array[307][0] = color;
			array[308][0] = color;
			array[567][0] = color;
			array[349][0] = new Color(144, 148, 144);
			array[531][0] = new Color(144, 148, 144);
			array[105][0] = new Color(144, 148, 144);
			array[105][1] = new Color(177, 92, 31);
			array[105][2] = new Color(201, 188, 170);
			array[137][0] = new Color(144, 148, 144);
			array[137][1] = new Color(141, 56, 0);
			array[16][0] = new Color(140, 130, 116);
			array[26][0] = new Color(119, 101, 125);
			array[26][1] = new Color(214, 127, 133);
			array[36][0] = new Color(230, 89, 92);
			array[28][0] = new Color(151, 79, 80);
			array[28][1] = new Color(90, 139, 140);
			array[28][2] = new Color(192, 136, 70);
			array[28][3] = new Color(203, 185, 151);
			array[28][4] = new Color(73, 56, 41);
			array[28][5] = new Color(148, 159, 67);
			array[28][6] = new Color(138, 172, 67);
			array[28][7] = new Color(226, 122, 47);
			array[28][8] = new Color(198, 87, 93);
			array[29][0] = new Color(175, 105, 128);
			array[51][0] = new Color(192, 202, 203);
			array[31][0] = new Color(141, 120, 168);
			array[31][1] = new Color(212, 105, 105);
			array[32][0] = new Color(151, 135, 183);
			array[42][0] = new Color(251, 235, 127);
			array[50][0] = new Color(170, 48, 114);
			array[85][0] = new Color(192, 192, 192);
			array[69][0] = new Color(190, 150, 92);
			array[77][0] = new Color(238, 85, 70);
			array[81][0] = new Color(245, 133, 191);
			array[78][0] = new Color(121, 110, 97);
			array[141][0] = new Color(192, 59, 59);
			array[129][0] = new Color(255, 117, 224);
			array[126][0] = new Color(159, 209, 229);
			array[125][0] = new Color(141, 175, 255);
			array[103][0] = new Color(141, 98, 77);
			array[95][0] = new Color(255, 162, 31);
			array[92][0] = new Color(213, 229, 237);
			array[91][0] = new Color(13, 88, 130);
			array[215][0] = new Color(254, 121, 2);
			array[592][0] = new Color(254, 121, 2);
			array[316][0] = new Color(157, 176, 226);
			array[317][0] = new Color(118, 227, 129);
			array[318][0] = new Color(227, 118, 215);
			array[319][0] = new Color(96, 68, 48);
			array[320][0] = new Color(203, 185, 151);
			array[321][0] = new Color(96, 77, 64);
			array[574][0] = new Color(76, 57, 44);
			array[322][0] = new Color(198, 170, 104);
			array[149][0] = new Color(220, 50, 50);
			array[149][1] = new Color(0, 220, 50);
			array[149][2] = new Color(50, 50, 220);
			array[133][0] = new Color(231, 53, 56);
			array[133][1] = new Color(192, 189, 221);
			array[134][0] = new Color(166, 187, 153);
			array[134][1] = new Color(241, 129, 249);
			array[102][0] = new Color(229, 212, 73);
			array[49][0] = new Color(89, 201, 255);
			array[35][0] = new Color(226, 145, 30);
			array[34][0] = new Color(235, 166, 135);
			array[136][0] = new Color(213, 203, 204);
			array[231][0] = new Color(224, 194, 101);
			array[239][0] = new Color(224, 194, 101);
			array[240][0] = new Color(120, 85, 60);
			array[240][1] = new Color(99, 50, 30);
			array[240][2] = new Color(153, 153, 117);
			array[240][3] = new Color(112, 84, 56);
			array[240][4] = new Color(234, 231, 226);
			array[241][0] = new Color(77, 74, 72);
			array[244][0] = new Color(200, 245, 253);
			color = new Color(99, 50, 30);
			array[242][0] = color;
			array[245][0] = color;
			array[246][0] = color;
			array[242][1] = new Color(185, 142, 97);
			array[247][0] = new Color(140, 150, 150);
			array[271][0] = new Color(107, 250, 255);
			array[270][0] = new Color(187, 255, 107);
			array[581][0] = new Color(255, 150, 150);
			array[572][0] = new Color(255, 186, 212);
			array[572][1] = new Color(209, 201, 255);
			array[572][2] = new Color(200, 254, 255);
			array[572][3] = new Color(199, 255, 211);
			array[572][4] = new Color(180, 209, 255);
			array[572][5] = new Color(255, 220, 214);
			array[314][0] = new Color(181, 164, 125);
			array[324][0] = new Color(228, 213, 173);
			array[351][0] = new Color(31, 31, 31);
			array[424][0] = new Color(146, 155, 187);
			array[429][0] = new Color(220, 220, 220);
			array[445][0] = new Color(240, 240, 240);
			array[21][0] = new Color(174, 129, 92);
			array[21][1] = new Color(233, 207, 94);
			array[21][2] = new Color(137, 128, 200);
			array[21][3] = new Color(160, 160, 160);
			array[21][4] = new Color(106, 210, 255);
			array[441][0] = array[21][0];
			array[441][1] = array[21][1];
			array[441][2] = array[21][2];
			array[441][3] = array[21][3];
			array[441][4] = array[21][4];
			array[27][0] = new Color(54, 154, 54);
			array[27][1] = new Color(226, 196, 49);
			color = new Color(246, 197, 26);
			array[82][0] = color;
			array[83][0] = color;
			array[84][0] = color;
			color = new Color(76, 150, 216);
			array[82][1] = color;
			array[83][1] = color;
			array[84][1] = color;
			color = new Color(185, 214, 42);
			array[82][2] = color;
			array[83][2] = color;
			array[84][2] = color;
			color = new Color(167, 203, 37);
			array[82][3] = color;
			array[83][3] = color;
			array[84][3] = color;
			array[591][6] = color;
			color = new Color(32, 168, 117);
			array[82][4] = color;
			array[83][4] = color;
			array[84][4] = color;
			color = new Color(177, 69, 49);
			array[82][5] = color;
			array[83][5] = color;
			array[84][5] = color;
			color = new Color(40, 152, 240);
			array[82][6] = color;
			array[83][6] = color;
			array[84][6] = color;
			array[591][1] = new Color(246, 197, 26);
			array[591][2] = new Color(76, 150, 216);
			array[591][3] = new Color(32, 168, 117);
			array[591][4] = new Color(40, 152, 240);
			array[591][5] = new Color(114, 81, 56);
			array[591][6] = new Color(141, 137, 223);
			array[591][7] = new Color(208, 80, 80);
			array[591][8] = new Color(177, 69, 49);
			array[165][0] = new Color(115, 173, 229);
			array[165][1] = new Color(100, 100, 100);
			array[165][2] = new Color(152, 152, 152);
			array[165][3] = new Color(227, 125, 22);
			array[178][0] = new Color(208, 94, 201);
			array[178][1] = new Color(233, 146, 69);
			array[178][2] = new Color(71, 146, 251);
			array[178][3] = new Color(60, 226, 133);
			array[178][4] = new Color(250, 30, 71);
			array[178][5] = new Color(166, 176, 204);
			array[178][6] = new Color(255, 217, 120);
			color = new Color(99, 99, 99);
			array[185][0] = color;
			array[186][0] = color;
			array[187][0] = color;
			array[565][0] = color;
			array[579][0] = color;
			color = new Color(114, 81, 56);
			array[185][1] = color;
			array[186][1] = color;
			array[187][1] = color;
			array[591][0] = color;
			color = new Color(133, 133, 101);
			array[185][2] = color;
			array[186][2] = color;
			array[187][2] = color;
			color = new Color(151, 200, 211);
			array[185][3] = color;
			array[186][3] = color;
			array[187][3] = color;
			color = new Color(177, 183, 161);
			array[185][4] = color;
			array[186][4] = color;
			array[187][4] = color;
			color = new Color(134, 114, 38);
			array[185][5] = color;
			array[186][5] = color;
			array[187][5] = color;
			color = new Color(82, 62, 66);
			array[185][6] = color;
			array[186][6] = color;
			array[187][6] = color;
			color = new Color(143, 117, 121);
			array[185][7] = color;
			array[186][7] = color;
			array[187][7] = color;
			color = new Color(177, 92, 31);
			array[185][8] = color;
			array[186][8] = color;
			array[187][8] = color;
			color = new Color(85, 73, 87);
			array[185][9] = color;
			array[186][9] = color;
			array[187][9] = color;
			color = new Color(26, 196, 84);
			array[185][10] = color;
			array[186][10] = color;
			array[187][10] = color;
			array[227][0] = new Color(74, 197, 155);
			array[227][1] = new Color(54, 153, 88);
			array[227][2] = new Color(63, 126, 207);
			array[227][3] = new Color(240, 180, 4);
			array[227][4] = new Color(45, 68, 168);
			array[227][5] = new Color(61, 92, 0);
			array[227][6] = new Color(216, 112, 152);
			array[227][7] = new Color(200, 40, 24);
			array[227][8] = new Color(113, 45, 133);
			array[227][9] = new Color(235, 137, 2);
			array[227][10] = new Color(41, 152, 135);
			array[227][11] = new Color(198, 19, 78);
			array[373][0] = new Color(9, 61, 191);
			array[374][0] = new Color(253, 32, 3);
			array[375][0] = new Color(255, 156, 12);
			array[461][0] = new Color(212, 192, 100);
			array[461][1] = new Color(137, 132, 156);
			array[461][2] = new Color(148, 122, 112);
			array[461][3] = new Color(221, 201, 206);
			array[323][0] = new Color(182, 141, 86);
			array[325][0] = new Color(129, 125, 93);
			array[326][0] = new Color(9, 61, 191);
			array[327][0] = new Color(253, 32, 3);
			array[507][0] = new Color(5, 5, 5);
			array[508][0] = new Color(5, 5, 5);
			array[330][0] = new Color(226, 118, 76);
			array[331][0] = new Color(161, 172, 173);
			array[332][0] = new Color(204, 181, 72);
			array[333][0] = new Color(190, 190, 178);
			array[335][0] = new Color(217, 174, 137);
			array[336][0] = new Color(253, 62, 3);
			array[337][0] = new Color(144, 148, 144);
			array[338][0] = new Color(85, 255, 160);
			array[315][0] = new Color(235, 114, 80);
			array[340][0] = new Color(96, 248, 2);
			array[341][0] = new Color(105, 74, 202);
			array[342][0] = new Color(29, 240, 255);
			array[343][0] = new Color(254, 202, 80);
			array[344][0] = new Color(131, 252, 245);
			array[345][0] = new Color(255, 156, 12);
			array[346][0] = new Color(149, 212, 89);
			array[347][0] = new Color(236, 74, 79);
			array[348][0] = new Color(44, 26, 233);
			array[350][0] = new Color(55, 97, 155);
			array[352][0] = new Color(238, 97, 94);
			array[354][0] = new Color(141, 107, 89);
			array[355][0] = new Color(141, 107, 89);
			array[463][0] = new Color(155, 214, 240);
			array[491][0] = new Color(60, 20, 160);
			array[464][0] = new Color(233, 183, 128);
			array[465][0] = new Color(51, 84, 195);
			array[466][0] = new Color(205, 153, 73);
			array[356][0] = new Color(233, 203, 24);
			array[357][0] = new Color(168, 178, 204);
			array[367][0] = new Color(168, 178, 204);
			array[561][0] = new Color(148, 158, 184);
			array[365][0] = new Color(146, 136, 205);
			array[366][0] = new Color(223, 232, 233);
			array[368][0] = new Color(50, 46, 104);
			array[369][0] = new Color(50, 46, 104);
			array[576][0] = new Color(30, 26, 84);
			array[370][0] = new Color(127, 116, 194);
			array[372][0] = new Color(252, 128, 201);
			array[371][0] = new Color(249, 101, 189);
			array[376][0] = new Color(160, 120, 92);
			array[378][0] = new Color(160, 120, 100);
			array[379][0] = new Color(251, 209, 240);
			array[382][0] = new Color(28, 216, 94);
			array[383][0] = new Color(221, 136, 144);
			array[384][0] = new Color(131, 206, 12);
			array[385][0] = new Color(87, 21, 144);
			array[386][0] = new Color(127, 92, 69);
			array[387][0] = new Color(127, 92, 69);
			array[388][0] = new Color(127, 92, 69);
			array[389][0] = new Color(127, 92, 69);
			array[390][0] = new Color(253, 32, 3);
			array[397][0] = new Color(212, 192, 100);
			array[396][0] = new Color(198, 124, 78);
			array[577][0] = new Color(178, 104, 58);
			array[398][0] = new Color(100, 82, 126);
			array[399][0] = new Color(77, 76, 66);
			array[400][0] = new Color(96, 68, 117);
			array[401][0] = new Color(68, 60, 51);
			array[402][0] = new Color(174, 168, 186);
			array[403][0] = new Color(205, 152, 186);
			array[404][0] = new Color(212, 148, 88);
			array[405][0] = new Color(140, 140, 140);
			array[406][0] = new Color(120, 120, 120);
			array[407][0] = new Color(255, 227, 132);
			array[411][0] = new Color(227, 46, 46);
			array[494][0] = new Color(227, 227, 227);
			array[421][0] = new Color(65, 75, 90);
			array[422][0] = new Color(65, 75, 90);
			array[425][0] = new Color(146, 155, 187);
			array[426][0] = new Color(168, 38, 47);
			array[430][0] = new Color(39, 168, 96);
			array[431][0] = new Color(39, 94, 168);
			array[432][0] = new Color(242, 221, 100);
			array[433][0] = new Color(224, 100, 242);
			array[434][0] = new Color(197, 193, 216);
			array[427][0] = new Color(183, 53, 62);
			array[435][0] = new Color(54, 183, 111);
			array[436][0] = new Color(54, 109, 183);
			array[437][0] = new Color(255, 236, 115);
			array[438][0] = new Color(239, 115, 255);
			array[439][0] = new Color(212, 208, 231);
			array[440][0] = new Color(238, 51, 53);
			array[440][1] = new Color(13, 107, 216);
			array[440][2] = new Color(33, 184, 115);
			array[440][3] = new Color(255, 221, 62);
			array[440][4] = new Color(165, 0, 236);
			array[440][5] = new Color(223, 230, 238);
			array[440][6] = new Color(207, 101, 0);
			array[419][0] = new Color(88, 95, 114);
			array[419][1] = new Color(214, 225, 236);
			array[419][2] = new Color(25, 131, 205);
			array[423][0] = new Color(245, 197, 1);
			array[423][1] = new Color(185, 0, 224);
			array[423][2] = new Color(58, 240, 111);
			array[423][3] = new Color(50, 107, 197);
			array[423][4] = new Color(253, 91, 3);
			array[423][5] = new Color(254, 194, 20);
			array[423][6] = new Color(174, 195, 215);
			array[420][0] = new Color(99, 255, 107);
			array[420][1] = new Color(99, 255, 107);
			array[420][4] = new Color(99, 255, 107);
			array[420][2] = new Color(218, 2, 5);
			array[420][3] = new Color(218, 2, 5);
			array[420][5] = new Color(218, 2, 5);
			array[476][0] = new Color(160, 160, 160);
			array[410][0] = new Color(75, 139, 166);
			array[480][0] = new Color(120, 50, 50);
			array[509][0] = new Color(50, 50, 60);
			array[412][0] = new Color(75, 139, 166);
			array[443][0] = new Color(144, 148, 144);
			array[442][0] = new Color(3, 144, 201);
			array[444][0] = new Color(191, 176, 124);
			array[446][0] = new Color(255, 66, 152);
			array[447][0] = new Color(179, 132, 255);
			array[448][0] = new Color(0, 206, 180);
			array[449][0] = new Color(91, 186, 240);
			array[450][0] = new Color(92, 240, 91);
			array[451][0] = new Color(240, 91, 147);
			array[452][0] = new Color(255, 150, 181);
			array[453][0] = new Color(179, 132, 255);
			array[453][1] = new Color(0, 206, 180);
			array[453][2] = new Color(255, 66, 152);
			array[454][0] = new Color(174, 16, 176);
			array[455][0] = new Color(48, 225, 110);
			array[456][0] = new Color(179, 132, 255);
			array[457][0] = new Color(150, 164, 206);
			array[457][1] = new Color(255, 132, 184);
			array[457][2] = new Color(74, 255, 232);
			array[457][3] = new Color(215, 159, 255);
			array[457][4] = new Color(229, 219, 234);
			array[458][0] = new Color(211, 198, 111);
			array[459][0] = new Color(190, 223, 232);
			array[460][0] = new Color(141, 163, 181);
			array[462][0] = new Color(231, 178, 28);
			array[467][0] = new Color(129, 56, 121);
			array[467][1] = new Color(255, 249, 59);
			array[467][2] = new Color(161, 67, 24);
			array[467][3] = new Color(89, 70, 72);
			array[467][4] = new Color(233, 207, 94);
			array[467][5] = new Color(254, 158, 35);
			array[467][6] = new Color(34, 221, 151);
			array[467][7] = new Color(249, 170, 236);
			array[467][8] = new Color(35, 200, 254);
			array[467][9] = new Color(190, 200, 200);
			array[467][10] = new Color(230, 170, 100);
			array[467][11] = new Color(165, 168, 26);
			for (int j = 0; j < 12; j++)
			{
				array[468][j] = array[467][j];
			}
			array[472][0] = new Color(190, 160, 140);
			array[473][0] = new Color(85, 114, 123);
			array[474][0] = new Color(116, 94, 97);
			array[478][0] = new Color(108, 34, 35);
			array[479][0] = new Color(178, 114, 68);
			array[485][0] = new Color(198, 134, 88);
			array[492][0] = new Color(78, 193, 227);
			array[492][0] = new Color(78, 193, 227);
			array[493][0] = new Color(250, 249, 252);
			array[493][1] = new Color(240, 90, 90);
			array[493][2] = new Color(98, 230, 92);
			array[493][3] = new Color(95, 197, 238);
			array[493][4] = new Color(241, 221, 100);
			array[493][5] = new Color(213, 92, 237);
			array[494][0] = new Color(224, 219, 236);
			array[495][0] = new Color(253, 227, 215);
			array[496][0] = new Color(165, 159, 153);
			array[498][0] = new Color(202, 174, 165);
			array[499][0] = new Color(160, 187, 142);
			array[500][0] = new Color(254, 158, 35);
			array[501][0] = new Color(34, 221, 151);
			array[502][0] = new Color(249, 170, 236);
			array[503][0] = new Color(35, 200, 254);
			array[506][0] = new Color(61, 61, 61);
			array[510][0] = new Color(191, 142, 111);
			array[511][0] = new Color(187, 68, 74);
			array[520][0] = new Color(224, 219, 236);
			array[545][0] = new Color(255, 126, 145);
			array[530][0] = new Color(107, 182, 0);
			array[530][1] = new Color(23, 154, 209);
			array[530][2] = new Color(238, 97, 94);
			array[530][3] = new Color(113, 108, 205);
			array[546][0] = new Color(60, 60, 60);
			array[557][0] = new Color(60, 60, 60);
			array[547][0] = new Color(120, 110, 100);
			array[548][0] = new Color(120, 110, 100);
			array[562][0] = new Color(165, 168, 26);
			array[563][0] = new Color(165, 168, 26);
			array[571][0] = new Color(165, 168, 26);
			array[568][0] = new Color(248, 203, 233);
			array[569][0] = new Color(203, 248, 218);
			array[570][0] = new Color(160, 242, 255);
			array[597][0] = new Color(28, 216, 94);
			array[597][1] = new Color(183, 237, 20);
			array[597][2] = new Color(185, 83, 200);
			array[597][3] = new Color(131, 128, 168);
			array[597][4] = new Color(38, 142, 214);
			array[597][5] = new Color(229, 154, 9);
			array[597][6] = new Color(142, 227, 234);
			array[597][7] = new Color(98, 111, 223);
			array[597][8] = new Color(241, 233, 158);
			array[617][0] = new Color(233, 207, 94);
			Color color3 = new Color(250, 100, 50);
			array[548][1] = color3;
			array[613][0] = color3;
			array[614][0] = color3;
			Color[] array2 = new Color[3]
			{
				new Color(9, 61, 191),
				new Color(253, 32, 3),
				new Color(254, 194, 20)
			};
			Color[][] array3 = new Color[316][];
			for (int k = 0; k < 316; k++)
			{
				array3[k] = new Color[2];
			}
			array3[158][0] = new Color(107, 49, 154);
			array3[163][0] = new Color(154, 148, 49);
			array3[162][0] = new Color(49, 49, 154);
			array3[160][0] = new Color(49, 154, 68);
			array3[161][0] = new Color(154, 49, 77);
			array3[159][0] = new Color(85, 89, 118);
			array3[157][0] = new Color(154, 83, 49);
			array3[154][0] = new Color(221, 79, 255);
			array3[166][0] = new Color(250, 255, 79);
			array3[165][0] = new Color(79, 102, 255);
			array3[156][0] = new Color(79, 255, 89);
			array3[164][0] = new Color(255, 79, 79);
			array3[155][0] = new Color(240, 240, 247);
			array3[153][0] = new Color(255, 145, 79);
			array3[169][0] = new Color(5, 5, 5);
			array3[224][0] = new Color(57, 55, 52);
			array3[225][0] = new Color(68, 68, 68);
			array3[226][0] = new Color(148, 138, 74);
			array3[227][0] = new Color(95, 137, 191);
			array3[170][0] = new Color(59, 39, 22);
			array3[171][0] = new Color(59, 39, 22);
			color = new Color(52, 52, 52);
			array3[1][0] = color;
			array3[53][0] = color;
			array3[52][0] = color;
			array3[51][0] = color;
			array3[50][0] = color;
			array3[49][0] = color;
			array3[48][0] = color;
			array3[44][0] = color;
			array3[5][0] = color;
			color = new Color(88, 61, 46);
			array3[2][0] = color;
			array3[16][0] = color;
			array3[59][0] = color;
			array3[3][0] = new Color(61, 58, 78);
			array3[4][0] = new Color(73, 51, 36);
			array3[6][0] = new Color(91, 30, 30);
			color = new Color(27, 31, 42);
			array3[7][0] = color;
			array3[17][0] = color;
			color = new Color(32, 40, 45);
			array3[94][0] = color;
			array3[100][0] = color;
			color = new Color(44, 41, 50);
			array3[95][0] = color;
			array3[101][0] = color;
			color = new Color(31, 39, 26);
			array3[8][0] = color;
			array3[18][0] = color;
			color = new Color(36, 45, 44);
			array3[98][0] = color;
			array3[104][0] = color;
			color = new Color(38, 49, 50);
			array3[99][0] = color;
			array3[105][0] = color;
			color = new Color(41, 28, 36);
			array3[9][0] = color;
			array3[19][0] = color;
			color = new Color(72, 50, 77);
			array3[96][0] = color;
			array3[102][0] = color;
			color = new Color(78, 50, 69);
			array3[97][0] = color;
			array3[103][0] = color;
			array3[10][0] = new Color(74, 62, 12);
			array3[11][0] = new Color(46, 56, 59);
			array3[12][0] = new Color(75, 32, 11);
			array3[13][0] = new Color(67, 37, 37);
			color = new Color(15, 15, 15);
			array3[14][0] = color;
			array3[20][0] = color;
			array3[15][0] = new Color(52, 43, 45);
			array3[22][0] = new Color(113, 99, 99);
			array3[23][0] = new Color(38, 38, 43);
			array3[24][0] = new Color(53, 39, 41);
			array3[25][0] = new Color(11, 35, 62);
			array3[26][0] = new Color(21, 63, 70);
			array3[27][0] = new Color(88, 61, 46);
			array3[27][1] = new Color(52, 52, 52);
			array3[28][0] = new Color(81, 84, 101);
			array3[29][0] = new Color(88, 23, 23);
			array3[30][0] = new Color(28, 88, 23);
			array3[31][0] = new Color(78, 87, 99);
			color = new Color(69, 67, 41);
			array3[34][0] = color;
			array3[37][0] = color;
			array3[32][0] = new Color(86, 17, 40);
			array3[33][0] = new Color(49, 47, 83);
			array3[35][0] = new Color(51, 51, 70);
			array3[36][0] = new Color(87, 59, 55);
			array3[38][0] = new Color(49, 57, 49);
			array3[39][0] = new Color(78, 79, 73);
			array3[45][0] = new Color(60, 59, 51);
			array3[46][0] = new Color(48, 57, 47);
			array3[47][0] = new Color(71, 77, 85);
			array3[40][0] = new Color(85, 102, 103);
			array3[41][0] = new Color(52, 50, 62);
			array3[42][0] = new Color(71, 42, 44);
			array3[43][0] = new Color(73, 66, 50);
			array3[54][0] = new Color(40, 56, 50);
			array3[55][0] = new Color(49, 48, 36);
			array3[56][0] = new Color(43, 33, 32);
			array3[57][0] = new Color(31, 40, 49);
			array3[58][0] = new Color(48, 35, 52);
			array3[60][0] = new Color(1, 52, 20);
			array3[61][0] = new Color(55, 39, 26);
			array3[62][0] = new Color(39, 33, 26);
			array3[69][0] = new Color(43, 42, 68);
			array3[70][0] = new Color(30, 70, 80);
			color = new Color(30, 80, 48);
			array3[63][0] = color;
			array3[65][0] = color;
			array3[66][0] = color;
			array3[68][0] = color;
			color = new Color(53, 80, 30);
			array3[64][0] = color;
			array3[67][0] = color;
			array3[78][0] = new Color(63, 39, 26);
			array3[244][0] = new Color(63, 39, 26);
			array3[71][0] = new Color(78, 105, 135);
			array3[72][0] = new Color(52, 84, 12);
			array3[73][0] = new Color(190, 204, 223);
			color = new Color(64, 62, 80);
			array3[74][0] = color;
			array3[80][0] = color;
			array3[75][0] = new Color(65, 65, 35);
			array3[76][0] = new Color(20, 46, 104);
			array3[77][0] = new Color(61, 13, 16);
			array3[79][0] = new Color(51, 47, 96);
			array3[81][0] = new Color(101, 51, 51);
			array3[82][0] = new Color(77, 64, 34);
			array3[83][0] = new Color(62, 38, 41);
			array3[234][0] = new Color(60, 36, 39);
			array3[84][0] = new Color(48, 78, 93);
			array3[85][0] = new Color(54, 63, 69);
			color = new Color(138, 73, 38);
			array3[86][0] = color;
			array3[108][0] = color;
			color = new Color(50, 15, 8);
			array3[87][0] = color;
			array3[112][0] = color;
			array3[109][0] = new Color(94, 25, 17);
			array3[110][0] = new Color(125, 36, 122);
			array3[111][0] = new Color(51, 35, 27);
			array3[113][0] = new Color(135, 58, 0);
			array3[114][0] = new Color(65, 52, 15);
			array3[115][0] = new Color(39, 42, 51);
			array3[116][0] = new Color(89, 26, 27);
			array3[117][0] = new Color(126, 123, 115);
			array3[118][0] = new Color(8, 50, 19);
			array3[119][0] = new Color(95, 21, 24);
			array3[120][0] = new Color(17, 31, 65);
			array3[121][0] = new Color(192, 173, 143);
			array3[122][0] = new Color(114, 114, 131);
			array3[123][0] = new Color(136, 119, 7);
			array3[124][0] = new Color(8, 72, 3);
			array3[125][0] = new Color(117, 132, 82);
			array3[126][0] = new Color(100, 102, 114);
			array3[127][0] = new Color(30, 118, 226);
			array3[128][0] = new Color(93, 6, 102);
			array3[129][0] = new Color(64, 40, 169);
			array3[130][0] = new Color(39, 34, 180);
			array3[131][0] = new Color(87, 94, 125);
			array3[132][0] = new Color(6, 6, 6);
			array3[133][0] = new Color(69, 72, 186);
			array3[134][0] = new Color(130, 62, 16);
			array3[135][0] = new Color(22, 123, 163);
			array3[136][0] = new Color(40, 86, 151);
			array3[137][0] = new Color(183, 75, 15);
			array3[138][0] = new Color(83, 80, 100);
			array3[139][0] = new Color(115, 65, 68);
			array3[140][0] = new Color(119, 108, 81);
			array3[141][0] = new Color(59, 67, 71);
			array3[142][0] = new Color(17, 172, 143);
			array3[143][0] = new Color(90, 112, 105);
			array3[144][0] = new Color(62, 28, 87);
			array3[146][0] = new Color(120, 59, 19);
			array3[147][0] = new Color(59, 59, 59);
			array3[148][0] = new Color(229, 218, 161);
			array3[149][0] = new Color(73, 59, 50);
			array3[151][0] = new Color(102, 75, 34);
			array3[167][0] = new Color(70, 68, 51);
			array3[172][0] = new Color(163, 96, 0);
			array3[242][0] = new Color(5, 5, 5);
			array3[243][0] = new Color(5, 5, 5);
			array3[173][0] = new Color(94, 163, 46);
			array3[174][0] = new Color(117, 32, 59);
			array3[175][0] = new Color(20, 11, 203);
			array3[176][0] = new Color(74, 69, 88);
			array3[177][0] = new Color(60, 30, 30);
			array3[183][0] = new Color(111, 117, 135);
			array3[179][0] = new Color(111, 117, 135);
			array3[178][0] = new Color(111, 117, 135);
			array3[184][0] = new Color(25, 23, 54);
			array3[181][0] = new Color(25, 23, 54);
			array3[180][0] = new Color(25, 23, 54);
			array3[182][0] = new Color(74, 71, 129);
			array3[185][0] = new Color(52, 52, 52);
			array3[186][0] = new Color(38, 9, 66);
			array3[216][0] = new Color(158, 100, 64);
			array3[217][0] = new Color(62, 45, 75);
			array3[218][0] = new Color(57, 14, 12);
			array3[219][0] = new Color(96, 72, 133);
			array3[187][0] = new Color(149, 80, 51);
			array3[235][0] = new Color(140, 75, 48);
			array3[220][0] = new Color(67, 55, 80);
			array3[221][0] = new Color(64, 37, 29);
			array3[222][0] = new Color(70, 51, 91);
			array3[188][0] = new Color(82, 63, 80);
			array3[189][0] = new Color(65, 61, 77);
			array3[190][0] = new Color(64, 65, 92);
			array3[191][0] = new Color(76, 53, 84);
			array3[192][0] = new Color(144, 67, 52);
			array3[193][0] = new Color(149, 48, 48);
			array3[194][0] = new Color(111, 32, 36);
			array3[195][0] = new Color(147, 48, 55);
			array3[196][0] = new Color(97, 67, 51);
			array3[197][0] = new Color(112, 80, 62);
			array3[198][0] = new Color(88, 61, 46);
			array3[199][0] = new Color(127, 94, 76);
			array3[200][0] = new Color(143, 50, 123);
			array3[201][0] = new Color(136, 120, 131);
			array3[202][0] = new Color(219, 92, 143);
			array3[203][0] = new Color(113, 64, 150);
			array3[204][0] = new Color(74, 67, 60);
			array3[205][0] = new Color(60, 78, 59);
			array3[206][0] = new Color(0, 54, 21);
			array3[207][0] = new Color(74, 97, 72);
			array3[208][0] = new Color(40, 37, 35);
			array3[209][0] = new Color(77, 63, 66);
			array3[210][0] = new Color(111, 6, 6);
			array3[211][0] = new Color(88, 67, 59);
			array3[212][0] = new Color(88, 87, 80);
			array3[213][0] = new Color(71, 71, 67);
			array3[214][0] = new Color(76, 52, 60);
			array3[215][0] = new Color(89, 48, 59);
			array3[223][0] = new Color(51, 18, 4);
			array3[228][0] = new Color(160, 2, 75);
			array3[229][0] = new Color(100, 55, 164);
			array3[230][0] = new Color(0, 117, 101);
			array3[236][0] = new Color(127, 49, 44);
			array3[231][0] = new Color(110, 90, 78);
			array3[232][0] = new Color(47, 69, 75);
			array3[233][0] = new Color(91, 67, 70);
			array3[237][0] = new Color(200, 44, 18);
			array3[238][0] = new Color(24, 93, 66);
			array3[239][0] = new Color(160, 87, 234);
			array3[240][0] = new Color(6, 106, 255);
			array3[245][0] = new Color(102, 102, 102);
			array3[315][0] = new Color(181, 230, 29);
			array3[246][0] = new Color(61, 58, 78);
			array3[247][0] = new Color(52, 43, 45);
			array3[248][0] = new Color(81, 84, 101);
			array3[249][0] = new Color(85, 102, 103);
			array3[250][0] = new Color(52, 52, 52);
			array3[251][0] = new Color(52, 52, 52);
			array3[252][0] = new Color(52, 52, 52);
			array3[253][0] = new Color(52, 52, 52);
			array3[254][0] = new Color(52, 52, 52);
			array3[255][0] = new Color(52, 52, 52);
			array3[314][0] = new Color(52, 52, 52);
			array3[256][0] = new Color(40, 56, 50);
			array3[257][0] = new Color(49, 48, 36);
			array3[258][0] = new Color(43, 33, 32);
			array3[259][0] = new Color(31, 40, 49);
			array3[260][0] = new Color(48, 35, 52);
			array3[261][0] = new Color(88, 61, 46);
			array3[262][0] = new Color(55, 39, 26);
			array3[263][0] = new Color(39, 33, 26);
			array3[264][0] = new Color(43, 42, 68);
			array3[265][0] = new Color(30, 70, 80);
			array3[266][0] = new Color(78, 105, 135);
			array3[267][0] = new Color(51, 47, 96);
			array3[268][0] = new Color(101, 51, 51);
			array3[269][0] = new Color(62, 38, 41);
			array3[270][0] = new Color(59, 39, 22);
			array3[271][0] = new Color(59, 39, 22);
			array3[272][0] = new Color(111, 117, 135);
			array3[273][0] = new Color(25, 23, 54);
			array3[274][0] = new Color(52, 52, 52);
			array3[275][0] = new Color(149, 80, 51);
			array3[276][0] = new Color(82, 63, 80);
			array3[277][0] = new Color(65, 61, 77);
			array3[278][0] = new Color(64, 65, 92);
			array3[279][0] = new Color(76, 53, 84);
			array3[280][0] = new Color(144, 67, 52);
			array3[281][0] = new Color(149, 48, 48);
			array3[282][0] = new Color(111, 32, 36);
			array3[283][0] = new Color(147, 48, 55);
			array3[284][0] = new Color(97, 67, 51);
			array3[285][0] = new Color(112, 80, 62);
			array3[286][0] = new Color(88, 61, 46);
			array3[287][0] = new Color(127, 94, 76);
			array3[288][0] = new Color(143, 50, 123);
			array3[289][0] = new Color(136, 120, 131);
			array3[290][0] = new Color(219, 92, 143);
			array3[291][0] = new Color(113, 64, 150);
			array3[292][0] = new Color(74, 67, 60);
			array3[293][0] = new Color(60, 78, 59);
			array3[294][0] = new Color(0, 54, 21);
			array3[295][0] = new Color(74, 97, 72);
			array3[296][0] = new Color(40, 37, 35);
			array3[297][0] = new Color(77, 63, 66);
			array3[298][0] = new Color(111, 6, 6);
			array3[299][0] = new Color(88, 67, 59);
			array3[300][0] = new Color(88, 87, 80);
			array3[301][0] = new Color(71, 71, 67);
			array3[302][0] = new Color(76, 52, 60);
			array3[303][0] = new Color(89, 48, 59);
			array3[304][0] = new Color(158, 100, 64);
			array3[305][0] = new Color(62, 45, 75);
			array3[306][0] = new Color(57, 14, 12);
			array3[307][0] = new Color(96, 72, 133);
			array3[308][0] = new Color(67, 55, 80);
			array3[309][0] = new Color(64, 37, 29);
			array3[310][0] = new Color(70, 51, 91);
			array3[311][0] = new Color(51, 18, 4);
			array3[312][0] = new Color(78, 110, 51);
			array3[313][0] = new Color(78, 110, 51);
			Color[] array4 = new Color[256];
			Color color4 = new Color(50, 40, 255);
			Color color5 = new Color(145, 185, 255);
			for (int l = 0; l < array4.Length; l++)
			{
				float num = (float)l / (float)array4.Length;
				float num2 = 1f - num;
				array4[l] = new Color((byte)((float)(int)color4.R * num2 + (float)(int)color5.R * num), (byte)((float)(int)color4.G * num2 + (float)(int)color5.G * num), (byte)((float)(int)color4.B * num2 + (float)(int)color5.B * num));
			}
			Color[] array5 = new Color[256];
			Color color6 = new Color(88, 61, 46);
			Color color7 = new Color(37, 78, 123);
			for (int m = 0; m < array5.Length; m++)
			{
				float num3 = (float)m / 255f;
				float num4 = 1f - num3;
				array5[m] = new Color((byte)((float)(int)color6.R * num4 + (float)(int)color7.R * num3), (byte)((float)(int)color6.G * num4 + (float)(int)color7.G * num3), (byte)((float)(int)color6.B * num4 + (float)(int)color7.B * num3));
			}
			Color[] array6 = new Color[256];
			Color color8 = new Color(74, 67, 60);
			color7 = new Color(53, 70, 97);
			for (int n = 0; n < array6.Length; n++)
			{
				float num5 = (float)n / 255f;
				float num6 = 1f - num5;
				array6[n] = new Color((byte)((float)(int)color8.R * num6 + (float)(int)color7.R * num5), (byte)((float)(int)color8.G * num6 + (float)(int)color7.G * num5), (byte)((float)(int)color8.B * num6 + (float)(int)color7.B * num5));
			}
			Color color9 = new Color(50, 44, 38);
			int num7 = 0;
			tileOptionCounts = new int[623];
			for (int num8 = 0; num8 < 623; num8++)
			{
				Color[] array7 = array[num8];
				int num9;
				for (num9 = 0; num9 < 12 && !(array7[num9] == Color.Transparent); num9++)
				{
				}
				tileOptionCounts[num8] = num9;
				num7 += num9;
			}
			wallOptionCounts = new int[316];
			for (int num10 = 0; num10 < 316; num10++)
			{
				Color[] array8 = array3[num10];
				int num11;
				for (num11 = 0; num11 < 2 && !(array8[num11] == Color.Transparent); num11++)
				{
				}
				wallOptionCounts[num10] = num11;
				num7 += num11;
			}
			num7 += 773;
			colorLookup = new Color[num7];
			colorLookup[0] = Color.Transparent;
			ushort num12 = (tilePosition = 1);
			tileLookup = new ushort[623];
			for (int num13 = 0; num13 < 623; num13++)
			{
				if (tileOptionCounts[num13] > 0)
				{
					_ = array[num13];
					tileLookup[num13] = num12;
					for (int num14 = 0; num14 < tileOptionCounts[num13]; num14++)
					{
						colorLookup[num12] = array[num13][num14];
						num12 = (ushort)(num12 + 1);
					}
				}
				else
				{
					tileLookup[num13] = 0;
				}
			}
			wallPosition = num12;
			wallLookup = new ushort[316];
			wallRangeStart = num12;
			for (int num15 = 0; num15 < 316; num15++)
			{
				if (wallOptionCounts[num15] > 0)
				{
					_ = array3[num15];
					wallLookup[num15] = num12;
					for (int num16 = 0; num16 < wallOptionCounts[num15]; num16++)
					{
						colorLookup[num12] = array3[num15][num16];
						num12 = (ushort)(num12 + 1);
					}
				}
				else
				{
					wallLookup[num15] = 0;
				}
			}
			wallRangeEnd = num12;
			liquidPosition = num12;
			for (int num17 = 0; num17 < 3; num17++)
			{
				colorLookup[num12] = array2[num17];
				num12 = (ushort)(num12 + 1);
			}
			skyPosition = num12;
			for (int num18 = 0; num18 < 256; num18++)
			{
				colorLookup[num12] = array4[num18];
				num12 = (ushort)(num12 + 1);
			}
			dirtPosition = num12;
			for (int num19 = 0; num19 < 256; num19++)
			{
				colorLookup[num12] = array5[num19];
				num12 = (ushort)(num12 + 1);
			}
			rockPosition = num12;
			for (int num20 = 0; num20 < 256; num20++)
			{
				colorLookup[num12] = array6[num20];
				num12 = (ushort)(num12 + 1);
			}
			hellPosition = num12;
			colorLookup[num12] = color9;
			snowTypes = new ushort[6];
			snowTypes[0] = tileLookup[147];
			snowTypes[1] = tileLookup[161];
			snowTypes[2] = tileLookup[162];
			snowTypes[3] = tileLookup[163];
			snowTypes[4] = tileLookup[164];
			snowTypes[5] = tileLookup[200];
			Lang.BuildMapAtlas();
		}

		public static void ResetMapData()
		{
			numUpdateTile = 0;
		}

		public static bool HasOption(int tileType, int option)
		{
			return option < tileOptionCounts[tileType];
		}

		public static int TileToLookup(int tileType, int option)
		{
			return tileLookup[tileType] + option;
		}

		public static int LookupCount()
		{
			return colorLookup.Length;
		}

		private static void MapColor(ushort type, Color oldColor, byte colorType)
		{
			Color color = WorldGen.paintColor(colorType);
			float num = (float)(int)oldColor.R / 255f;
			float num2 = (float)(int)oldColor.G / 255f;
			float num3 = (float)(int)oldColor.B / 255f;
			if (num2 > num)
			{
				float num4 = num;
				num = num2;
				num2 = num4;
			}
			if (num3 > num)
			{
				float num5 = num;
				num = num3;
				num3 = num5;
			}
			switch (colorType)
			{
			case 29:
			{
				float num7 = num3 * 0.3f;
				oldColor.R = (byte)((float)(int)color.R * num7);
				oldColor.G = (byte)((float)(int)color.G * num7);
				oldColor.B = (byte)((float)(int)color.B * num7);
				break;
			}
			case 30:
				if (type >= wallRangeStart && type <= wallRangeEnd)
				{
					oldColor.R = (byte)((float)(255 - oldColor.R) * 0.5f);
					oldColor.G = (byte)((float)(255 - oldColor.G) * 0.5f);
					oldColor.B = (byte)((float)(255 - oldColor.B) * 0.5f);
				}
				else
				{
					oldColor.R = (byte)(255 - oldColor.R);
					oldColor.G = (byte)(255 - oldColor.G);
					oldColor.B = (byte)(255 - oldColor.B);
				}
				break;
			default:
			{
				float num6 = num;
				oldColor.R = (byte)((float)(int)color.R * num6);
				oldColor.G = (byte)((float)(int)color.G * num6);
				oldColor.B = (byte)((float)(int)color.B * num6);
				break;
			}
			case 31:
				break;
			}
		}

		public static Color GetMapTileXnaColor(MapTile tile)
		{
			Color oldColor = colorLookup[tile.Type];
			byte color = tile.Color;
			if (color > 0)
			{
				MapColor(tile.Type, oldColor, color);
			}
			if (tile.Light == byte.MaxValue || color == 31)
			{
				return oldColor;
			}
			float num = (float)(int)tile.Light / 255f;
			oldColor.R = (byte)((float)(int)oldColor.R * num);
			oldColor.G = (byte)((float)(int)oldColor.G * num);
			oldColor.B = (byte)((float)(int)oldColor.B * num);
			return oldColor;
		}

		public static MapTile CreateMapTile(int i, int j, byte Light)
		{
			Tile tile = Main.tile[i, j];
			if (tile == null)
			{
				return default(MapTile);
			}
			int num = 0;
			int num2 = Light;
			_ = Main.Map[i, j];
			int num3 = 0;
			int baseOption = 0;
			if (tile.active())
			{
				int type = tile.type;
				num3 = tileLookup[type];
				if (type == 5)
				{
					if (WorldGen.IsThisAMushroomTree(i, j))
					{
						baseOption = 1;
					}
					num = tile.color();
				}
				else
				{
					if (type == 51 && (i + j) % 2 == 0)
					{
						num3 = 0;
					}
					if (num3 != 0)
					{
						num = ((type != 160) ? tile.color() : 0);
						GetTileBaseOption(j, tile, baseOption);
					}
				}
			}
			if (num3 == 0)
			{
				if (tile.liquid > 32)
				{
					int num4 = tile.liquidType();
					num3 = liquidPosition + num4;
				}
				else if (tile.wall > 0 && tile.wall < 316)
				{
					int wall = tile.wall;
					num3 = wallLookup[wall];
					num = tile.wallColor();
					switch (wall)
					{
					case 21:
					case 88:
					case 89:
					case 90:
					case 91:
					case 92:
					case 93:
					case 168:
					case 241:
						num = 0;
						break;
					case 27:
						baseOption = i % 2;
						break;
					default:
						baseOption = 0;
						break;
					}
				}
			}
			if (num3 == 0)
			{
				if ((double)j < Main.worldSurface)
				{
					int num5 = (byte)(255.0 * ((double)j / Main.worldSurface));
					num3 = skyPosition + num5;
					num2 = 255;
					num = 0;
				}
				else if (j < Main.UnderworldLayer)
				{
					num = 0;
					byte b = 0;
					float num6 = Main.screenPosition.X / 16f - 5f;
					float num7 = (Main.screenPosition.X + (float)Main.screenWidth) / 16f + 5f;
					float num8 = Main.screenPosition.Y / 16f - 5f;
					float num9 = (Main.screenPosition.Y + (float)Main.screenHeight) / 16f + 5f;
					if (((float)i < num6 || (float)i > num7 || (float)j < num8 || (float)j > num9) && i > 40 && i < Main.maxTilesX - 40 && j > 40 && j < Main.maxTilesY - 40)
					{
						for (int k = i - 36; k <= i + 30; k += 10)
						{
							for (int l = j - 36; l <= j + 30; l += 10)
							{
								int type2 = Main.Map[k, l].Type;
								for (int m = 0; m < snowTypes.Length; m++)
								{
									if (snowTypes[m] == type2)
									{
										b = byte.MaxValue;
										k = i + 31;
										l = j + 31;
										break;
									}
								}
							}
						}
					}
					else
					{
						float num10 = (float)Main.SceneMetrics.SnowTileCount / (float)SceneMetrics.SnowTileMax;
						num10 *= 255f;
						if (num10 > 255f)
						{
							num10 = 255f;
						}
						b = (byte)num10;
					}
					num3 = ((!((double)j < Main.rockLayer)) ? (rockPosition + b) : (dirtPosition + b));
				}
				else
				{
					num3 = hellPosition;
				}
			}
			return MapTile.Create((ushort)(num3 + baseOption), (byte)num2, (byte)num);
		}

		public static void GetTileBaseOption(int y, Tile tileCache, int baseOption)
		{
			switch (tileCache.type)
			{
			case 461:
				if (Main.player[Main.myPlayer].ZoneCorrupt)
				{
					baseOption = 1;
				}
				else if (Main.player[Main.myPlayer].ZoneCrimson)
				{
					baseOption = 2;
				}
				else if (Main.player[Main.myPlayer].ZoneHallow)
				{
					baseOption = 3;
				}
				break;
			case 530:
				baseOption = tileCache.frameY / 36;
				break;
			case 15:
			{
				int num4 = tileCache.frameY / 40;
				baseOption = 0;
				if (num4 == 1 || num4 == 20)
				{
					baseOption = 1;
				}
				break;
			}
			case 529:
				baseOption = tileCache.frameY / 34;
				break;
			case 518:
			case 519:
				baseOption = tileCache.frameY / 18;
				break;
			case 4:
				if (tileCache.frameX < 66)
				{
					baseOption = 1;
				}
				baseOption = 0;
				break;
			case 572:
				baseOption = tileCache.frameY / 36;
				break;
			case 21:
			case 441:
				switch (tileCache.frameX / 36)
				{
				case 1:
				case 2:
				case 10:
				case 13:
				case 15:
					baseOption = 1;
					break;
				case 3:
				case 4:
					baseOption = 2;
					break;
				case 6:
					baseOption = 3;
					break;
				case 11:
				case 17:
					baseOption = 4;
					break;
				default:
					baseOption = 0;
					break;
				}
				break;
			case 467:
			case 468:
			{
				int num = tileCache.frameX / 36;
				switch (num)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
					baseOption = num;
					break;
				case 12:
				case 13:
					baseOption = 10;
					break;
				default:
					baseOption = 0;
					break;
				}
				break;
			}
			case 560:
			{
				int num = tileCache.frameX / 36;
				if ((uint)num <= 2u)
				{
					baseOption = num;
				}
				else
				{
					baseOption = 0;
				}
				break;
			}
			case 28:
				if (tileCache.frameY < 144)
				{
					baseOption = 0;
				}
				else if (tileCache.frameY < 252)
				{
					baseOption = 1;
				}
				else if (tileCache.frameY < 360 || (tileCache.frameY > 900 && tileCache.frameY < 1008))
				{
					baseOption = 2;
				}
				else if (tileCache.frameY < 468)
				{
					baseOption = 3;
				}
				else if (tileCache.frameY < 576)
				{
					baseOption = 4;
				}
				else if (tileCache.frameY < 684)
				{
					baseOption = 5;
				}
				else if (tileCache.frameY < 792)
				{
					baseOption = 6;
				}
				else if (tileCache.frameY < 898)
				{
					baseOption = 8;
				}
				else if (tileCache.frameY < 1006)
				{
					baseOption = 7;
				}
				else if (tileCache.frameY < 1114)
				{
					baseOption = 0;
				}
				else if (tileCache.frameY < 1222)
				{
					baseOption = 3;
				}
				else
				{
					baseOption = 7;
				}
				break;
			case 27:
				if (tileCache.frameY < 34)
				{
					baseOption = 1;
				}
				else
				{
					baseOption = 0;
				}
				break;
			case 31:
				if (tileCache.frameX >= 36)
				{
					baseOption = 1;
				}
				else
				{
					baseOption = 0;
				}
				break;
			case 26:
				if (tileCache.frameX >= 54)
				{
					baseOption = 1;
				}
				else
				{
					baseOption = 0;
				}
				break;
			case 137:
				if (tileCache.frameY == 0)
				{
					baseOption = 0;
				}
				else
				{
					baseOption = 1;
				}
				break;
			case 82:
			case 83:
			case 84:
				if (tileCache.frameX < 18)
				{
					baseOption = 0;
				}
				else if (tileCache.frameX < 36)
				{
					baseOption = 1;
				}
				else if (tileCache.frameX < 54)
				{
					baseOption = 2;
				}
				else if (tileCache.frameX < 72)
				{
					baseOption = 3;
				}
				else if (tileCache.frameX < 90)
				{
					baseOption = 4;
				}
				else if (tileCache.frameX < 108)
				{
					baseOption = 5;
				}
				else
				{
					baseOption = 6;
				}
				break;
			case 591:
				baseOption = tileCache.frameX / 36;
				break;
			case 105:
				if (tileCache.frameX >= 1548 && tileCache.frameX <= 1654)
				{
					baseOption = 1;
				}
				else if (tileCache.frameX >= 1656 && tileCache.frameX <= 1798)
				{
					baseOption = 2;
				}
				else
				{
					baseOption = 0;
				}
				break;
			case 133:
				if (tileCache.frameX < 52)
				{
					baseOption = 0;
				}
				else
				{
					baseOption = 1;
				}
				break;
			case 134:
				if (tileCache.frameX < 28)
				{
					baseOption = 0;
				}
				else
				{
					baseOption = 1;
				}
				break;
			case 149:
				baseOption = y % 3;
				break;
			case 160:
				baseOption = y % 3;
				break;
			case 165:
				if (tileCache.frameX < 54)
				{
					baseOption = 0;
				}
				else if (tileCache.frameX < 106)
				{
					baseOption = 1;
				}
				else if (tileCache.frameX >= 216)
				{
					baseOption = 1;
				}
				else if (tileCache.frameX < 162)
				{
					baseOption = 2;
				}
				else
				{
					baseOption = 3;
				}
				break;
			case 178:
				if (tileCache.frameX < 18)
				{
					baseOption = 0;
				}
				else if (tileCache.frameX < 36)
				{
					baseOption = 1;
				}
				else if (tileCache.frameX < 54)
				{
					baseOption = 2;
				}
				else if (tileCache.frameX < 72)
				{
					baseOption = 3;
				}
				else if (tileCache.frameX < 90)
				{
					baseOption = 4;
				}
				else if (tileCache.frameX < 108)
				{
					baseOption = 5;
				}
				else
				{
					baseOption = 6;
				}
				break;
			case 184:
				if (tileCache.frameX < 22)
				{
					baseOption = 0;
				}
				else if (tileCache.frameX < 44)
				{
					baseOption = 1;
				}
				else if (tileCache.frameX < 66)
				{
					baseOption = 2;
				}
				else if (tileCache.frameX < 88)
				{
					baseOption = 3;
				}
				else if (tileCache.frameX < 110)
				{
					baseOption = 4;
				}
				else if (tileCache.frameX < 132)
				{
					baseOption = 5;
				}
				else if (tileCache.frameX < 154)
				{
					baseOption = 6;
				}
				else if (tileCache.frameX < 176)
				{
					baseOption = 7;
				}
				else if (tileCache.frameX < 198)
				{
					baseOption = 8;
				}
				break;
			case 185:
			{
				int num;
				if (tileCache.frameY < 18)
				{
					num = tileCache.frameX / 18;
					if (num < 6 || num == 28 || num == 29 || num == 30 || num == 31 || num == 32)
					{
						baseOption = 0;
					}
					else if (num < 12 || num == 33 || num == 34 || num == 35)
					{
						baseOption = 1;
					}
					else if (num < 28)
					{
						baseOption = 2;
					}
					else if (num < 48)
					{
						baseOption = 3;
					}
					else if (num < 54)
					{
						baseOption = 4;
					}
					else if (num < 72)
					{
						baseOption = 0;
					}
					else if (num == 72)
					{
						baseOption = 1;
					}
					break;
				}
				num = tileCache.frameX / 36;
				int num5 = tileCache.frameY / 18 - 1;
				num += num5 * 18;
				if (num < 6 || num == 19 || num == 20 || num == 21 || num == 22 || num == 23 || num == 24 || num == 33 || num == 38 || num == 39 || num == 40)
				{
					baseOption = 0;
				}
				else if (num < 16)
				{
					baseOption = 2;
				}
				else if (num < 19 || num == 31 || num == 32)
				{
					baseOption = 1;
				}
				else if (num < 31)
				{
					baseOption = 3;
				}
				else if (num < 38)
				{
					baseOption = 4;
				}
				else if (num < 59)
				{
					baseOption = 0;
				}
				else if (num < 62)
				{
					baseOption = 1;
				}
				break;
			}
			case 186:
			{
				int num = tileCache.frameX / 54;
				if (num < 7)
				{
					baseOption = 2;
				}
				else if (num < 22 || num == 33 || num == 34 || num == 35)
				{
					baseOption = 0;
				}
				else if (num < 25)
				{
					baseOption = 1;
				}
				else if (num == 25)
				{
					baseOption = 5;
				}
				else if (num < 32)
				{
					baseOption = 3;
				}
				break;
			}
			case 187:
			{
				int num = tileCache.frameX / 54;
				int num3 = tileCache.frameY / 36;
				num += num3 * 36;
				if (num < 3 || num == 14 || num == 15 || num == 16)
				{
					baseOption = 0;
				}
				else if (num < 6)
				{
					baseOption = 6;
				}
				else if (num < 9)
				{
					baseOption = 7;
				}
				else if (num < 14)
				{
					baseOption = 4;
				}
				else if (num < 18)
				{
					baseOption = 4;
				}
				else if (num < 23)
				{
					baseOption = 8;
				}
				else if (num < 25)
				{
					baseOption = 0;
				}
				else if (num < 29)
				{
					baseOption = 1;
				}
				else if (num < 47)
				{
					baseOption = 0;
				}
				else if (num < 50)
				{
					baseOption = 1;
				}
				else if (num < 52)
				{
					baseOption = 10;
				}
				else if (num < 55)
				{
					baseOption = 2;
				}
				break;
			}
			case 227:
				baseOption = tileCache.frameX / 34;
				break;
			case 240:
			{
				int num = tileCache.frameX / 54;
				int num2 = tileCache.frameY / 54;
				num += num2 * 36;
				if ((num < 0 || num > 11) && (num < 47 || num > 53))
				{
					switch (num)
					{
					case 72:
						break;
					case 12:
					case 13:
					case 14:
					case 15:
						baseOption = 1;
						return;
					default:
						switch (num)
						{
						case 16:
						case 17:
							baseOption = 2;
							return;
						default:
							if (num < 63 || num > 71)
							{
								break;
							}
							goto case 18;
						case 18:
						case 19:
						case 20:
						case 21:
						case 22:
						case 23:
						case 24:
						case 25:
						case 26:
						case 27:
						case 28:
						case 29:
						case 30:
						case 31:
						case 32:
						case 33:
						case 34:
						case 35:
							baseOption = 1;
							return;
						}
						if (num >= 41 && num <= 45)
						{
							baseOption = 3;
						}
						else if (num == 46)
						{
							baseOption = 4;
						}
						return;
					}
				}
				baseOption = 0;
				break;
			}
			case 242:
			{
				int num = tileCache.frameY / 72;
				if (num >= 22 && num <= 24)
				{
					baseOption = 1;
				}
				else
				{
					baseOption = 0;
				}
				break;
			}
			case 440:
			{
				int num = tileCache.frameX / 54;
				if (num > 6)
				{
					num = 6;
				}
				baseOption = num;
				break;
			}
			case 457:
			{
				int num = tileCache.frameX / 36;
				if (num > 4)
				{
					num = 4;
				}
				baseOption = num;
				break;
			}
			case 453:
			{
				int num = tileCache.frameX / 36;
				if (num > 2)
				{
					num = 2;
				}
				baseOption = num;
				break;
			}
			case 419:
			{
				int num = tileCache.frameX / 18;
				if (num > 2)
				{
					num = 2;
				}
				baseOption = num;
				break;
			}
			case 428:
			{
				int num = tileCache.frameY / 18;
				if (num > 3)
				{
					num = 3;
				}
				baseOption = num;
				break;
			}
			case 420:
			{
				int num = tileCache.frameY / 18;
				if (num > 5)
				{
					num = 5;
				}
				baseOption = num;
				break;
			}
			case 423:
			{
				int num = tileCache.frameY / 18;
				if (num > 6)
				{
					num = 6;
				}
				baseOption = num;
				break;
			}
			case 493:
				if (tileCache.frameX < 18)
				{
					baseOption = 0;
				}
				else if (tileCache.frameX < 36)
				{
					baseOption = 1;
				}
				else if (tileCache.frameX < 54)
				{
					baseOption = 2;
				}
				else if (tileCache.frameX < 72)
				{
					baseOption = 3;
				}
				else if (tileCache.frameX < 90)
				{
					baseOption = 4;
				}
				else
				{
					baseOption = 5;
				}
				break;
			case 548:
				if (tileCache.frameX / 54 < 7)
				{
					baseOption = 0;
				}
				else
				{
					baseOption = 1;
				}
				break;
			case 597:
			{
				int num = tileCache.frameX / 54;
				if ((uint)num <= 8u)
				{
					baseOption = num;
				}
				else
				{
					baseOption = 0;
				}
				break;
			}
			default:
				baseOption = 0;
				break;
			}
		}

		public static void SaveMap()
		{
			if ((Main.ActivePlayerFileData.IsCloudSave && SocialAPI.Cloud == null) || !Main.mapEnabled || !Monitor.TryEnter(IOLock))
			{
				return;
			}
			try
			{
				FileUtilities.ProtectedInvoke(InternalSaveMap);
			}
			catch (Exception value)
			{
				using StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", append: true);
				streamWriter.WriteLine(DateTime.Now);
				streamWriter.WriteLine(value);
				streamWriter.WriteLine("");
			}
			finally
			{
				Monitor.Exit(IOLock);
			}
		}

		private static void InternalSaveMap()
		{
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Expected O, but got Unknown
			bool isCloudSave = Main.ActivePlayerFileData.IsCloudSave;
			string text = Main.playerPathName.Substring(0, Main.playerPathName.Length - 4);
			if (!isCloudSave)
			{
				Utils.TryCreatingDirectory(text);
			}
			text += Path.DirectorySeparatorChar;
			text = ((!Main.ActiveWorldFileData.UseGuidAsMapName) ? (text + Main.worldID + ".map") : string.Concat(text, Main.ActiveWorldFileData.UniqueId, ".map"));
			new Stopwatch().Start();
			if (!Main.gameMenu)
			{
				noStatusText = true;
			}
			using (MemoryStream memoryStream = new MemoryStream(4000))
			{
				using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
				DeflateStream val = new DeflateStream((Stream)memoryStream, (CompressionMode)0);
				try
				{
					int num = 0;
					byte[] array = new byte[16384];
					binaryWriter.Write(230);
					Main.MapFileMetadata.IncrementAndWrite(binaryWriter);
					binaryWriter.Write(Main.worldName);
					binaryWriter.Write(Main.worldID);
					binaryWriter.Write(Main.maxTilesY);
					binaryWriter.Write(Main.maxTilesX);
					binaryWriter.Write((short)623);
					binaryWriter.Write((short)316);
					binaryWriter.Write((short)3);
					binaryWriter.Write((short)256);
					binaryWriter.Write((short)256);
					binaryWriter.Write((short)256);
					byte b = 1;
					byte b2 = 0;
					int i;
					for (i = 0; i < 623; i++)
					{
						if (tileOptionCounts[i] != 1)
						{
							b2 = (byte)(b2 | b);
						}
						if (b == 128)
						{
							binaryWriter.Write(b2);
							b2 = 0;
							b = 1;
						}
						else
						{
							b = (byte)(b << 1);
						}
					}
					if (b != 1)
					{
						binaryWriter.Write(b2);
					}
					i = 0;
					b = 1;
					b2 = 0;
					for (; i < 316; i++)
					{
						if (wallOptionCounts[i] != 1)
						{
							b2 = (byte)(b2 | b);
						}
						if (b == 128)
						{
							binaryWriter.Write(b2);
							b2 = 0;
							b = 1;
						}
						else
						{
							b = (byte)(b << 1);
						}
					}
					if (b != 1)
					{
						binaryWriter.Write(b2);
					}
					for (i = 0; i < 623; i++)
					{
						if (tileOptionCounts[i] != 1)
						{
							binaryWriter.Write((byte)tileOptionCounts[i]);
						}
					}
					for (i = 0; i < 316; i++)
					{
						if (wallOptionCounts[i] != 1)
						{
							binaryWriter.Write((byte)wallOptionCounts[i]);
						}
					}
					binaryWriter.Flush();
					for (int j = 0; j < Main.maxTilesY; j++)
					{
						if (!noStatusText)
						{
							float num2 = (float)j / (float)Main.maxTilesY;
							Main.statusText = Lang.gen[66].Value + " " + (int)(num2 * 100f + 1f) + "%";
						}
						int num3;
						for (num3 = 0; num3 < Main.maxTilesX; num3++)
						{
							MapTile mapTile = Main.Map[num3, j];
							byte b3;
							byte b4 = (b3 = 0);
							int num4 = 0;
							bool flag = true;
							bool flag2 = true;
							int num5 = 0;
							int num6 = 0;
							byte b5 = 0;
							int num7;
							ushort num8;
							if (mapTile.Light <= 18)
							{
								flag2 = false;
								flag = false;
								num7 = 0;
								num8 = 0;
								num4 = 0;
								int num9 = num3 + 1;
								int num10 = Main.maxTilesX - num3 - 1;
								while (num10 > 0 && Main.Map[num9, j].Light <= 18)
								{
									num4++;
									num10--;
									num9++;
								}
							}
							else
							{
								b5 = mapTile.Color;
								num8 = mapTile.Type;
								if (num8 < wallPosition)
								{
									num7 = 1;
									num8 = (ushort)(num8 - tilePosition);
								}
								else if (num8 < liquidPosition)
								{
									num7 = 2;
									num8 = (ushort)(num8 - wallPosition);
								}
								else if (num8 < skyPosition)
								{
									num7 = 3 + (num8 - liquidPosition);
									flag = false;
								}
								else if (num8 < dirtPosition)
								{
									num7 = 6;
									flag2 = false;
									flag = false;
								}
								else if (num8 < hellPosition)
								{
									num7 = 7;
									num8 = ((num8 >= rockPosition) ? ((ushort)(num8 - rockPosition)) : ((ushort)(num8 - dirtPosition)));
								}
								else
								{
									num7 = 6;
									flag = false;
								}
								if (mapTile.Light == byte.MaxValue)
								{
									flag2 = false;
								}
								if (flag2)
								{
									num4 = 0;
									int num9 = num3 + 1;
									int num10 = Main.maxTilesX - num3 - 1;
									num5 = num9;
									while (num10 > 0)
									{
										MapTile other = Main.Map[num9, j];
										if (mapTile.EqualsWithoutLight(other))
										{
											num10--;
											num4++;
											num9++;
											continue;
										}
										num6 = num9;
										break;
									}
								}
								else
								{
									num4 = 0;
									int num9 = num3 + 1;
									int num10 = Main.maxTilesX - num3 - 1;
									while (num10 > 0)
									{
										MapTile other2 = Main.Map[num9, j];
										if (!mapTile.Equals(other2))
										{
											break;
										}
										num10--;
										num4++;
										num9++;
									}
								}
							}
							if (b5 > 0)
							{
								b3 = (byte)(b3 | (byte)(b5 << 1));
							}
							if (b3 != 0)
							{
								b4 = (byte)(b4 | 1u);
							}
							b4 = (byte)(b4 | (byte)(num7 << 1));
							if (flag && num8 > 255)
							{
								b4 = (byte)(b4 | 0x10u);
							}
							if (flag2)
							{
								b4 = (byte)(b4 | 0x20u);
							}
							if (num4 > 0)
							{
								b4 = ((num4 <= 255) ? ((byte)(b4 | 0x40u)) : ((byte)(b4 | 0x80u)));
							}
							array[num] = b4;
							num++;
							if (b3 != 0)
							{
								array[num] = b3;
								num++;
							}
							if (flag)
							{
								array[num] = (byte)num8;
								num++;
								if (num8 > 255)
								{
									array[num] = (byte)(num8 >> 8);
									num++;
								}
							}
							if (flag2)
							{
								array[num] = mapTile.Light;
								num++;
							}
							if (num4 > 0)
							{
								array[num] = (byte)num4;
								num++;
								if (num4 > 255)
								{
									array[num] = (byte)(num4 >> 8);
									num++;
								}
							}
							for (int k = num5; k < num6; k++)
							{
								array[num] = Main.Map[k, j].Light;
								num++;
							}
							num3 += num4;
							if (num >= 4096)
							{
								((Stream)(object)val).Write(array, 0, num);
								num = 0;
							}
						}
					}
					if (num > 0)
					{
						((Stream)(object)val).Write(array, 0, num);
					}
					((Stream)(object)val).Dispose();
					FileUtilities.WriteAllBytes(text, memoryStream.ToArray(), isCloudSave);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			noStatusText = false;
		}

		public static void LoadMapVersion1(BinaryReader fileIO, int release)
		{
			string a = fileIO.ReadString();
			int num = fileIO.ReadInt32();
			int num2 = fileIO.ReadInt32();
			int num3 = fileIO.ReadInt32();
			if (a != Main.worldName || num != Main.worldID || num3 != Main.maxTilesX || num2 != Main.maxTilesY)
			{
				throw new Exception("Map meta-data is invalid.");
			}
			OldMapHelper oldMapHelper = default(OldMapHelper);
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				float num4 = (float)i / (float)Main.maxTilesX;
				Main.statusText = Lang.gen[67].Value + " " + (int)(num4 * 100f + 1f) + "%";
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					if (fileIO.ReadBoolean())
					{
						int num5 = ((release <= 77) ? fileIO.ReadByte() : fileIO.ReadUInt16());
						byte b = fileIO.ReadByte();
						oldMapHelper.misc = fileIO.ReadByte();
						if (release >= 50)
						{
							oldMapHelper.misc2 = fileIO.ReadByte();
						}
						else
						{
							oldMapHelper.misc2 = 0;
						}
						bool flag = false;
						int num6 = oldMapHelper.option();
						int num7;
						if (oldMapHelper.active())
						{
							num7 = num6 + tileLookup[num5];
						}
						else if (oldMapHelper.water())
						{
							num7 = liquidPosition;
						}
						else if (oldMapHelper.lava())
						{
							num7 = liquidPosition + 1;
						}
						else if (oldMapHelper.honey())
						{
							num7 = liquidPosition + 2;
						}
						else if (oldMapHelper.wall())
						{
							num7 = num6 + wallLookup[num5];
						}
						else if ((double)j < Main.worldSurface)
						{
							flag = true;
							int num8 = (byte)(256.0 * ((double)j / Main.worldSurface));
							num7 = skyPosition + num8;
						}
						else if ((double)j < Main.rockLayer)
						{
							flag = true;
							if (num5 > 255)
							{
								num5 = 255;
							}
							num7 = num5 + dirtPosition;
						}
						else if (j < Main.UnderworldLayer)
						{
							flag = true;
							if (num5 > 255)
							{
								num5 = 255;
							}
							num7 = num5 + rockPosition;
						}
						else
						{
							num7 = hellPosition;
						}
						MapTile tile = MapTile.Create((ushort)num7, b, 0);
						Main.Map.SetTile(i, j, tile);
						int num9 = fileIO.ReadInt16();
						if (b == byte.MaxValue)
						{
							while (num9 > 0)
							{
								num9--;
								j++;
								if (flag)
								{
									if ((double)j < Main.worldSurface)
									{
										flag = true;
										int num10 = (byte)(256.0 * ((double)j / Main.worldSurface));
										num7 = skyPosition + num10;
									}
									else if ((double)j < Main.rockLayer)
									{
										flag = true;
										num7 = num5 + dirtPosition;
									}
									else if (j < Main.UnderworldLayer)
									{
										flag = true;
										num7 = num5 + rockPosition;
									}
									else
									{
										flag = true;
										num7 = hellPosition;
									}
									tile.Type = (ushort)num7;
								}
								Main.Map.SetTile(i, j, tile);
							}
							continue;
						}
						while (num9 > 0)
						{
							j++;
							num9--;
							b = fileIO.ReadByte();
							if (b <= 18)
							{
								continue;
							}
							tile.Light = b;
							if (flag)
							{
								if ((double)j < Main.worldSurface)
								{
									flag = true;
									int num11 = (byte)(256.0 * ((double)j / Main.worldSurface));
									num7 = skyPosition + num11;
								}
								else if ((double)j < Main.rockLayer)
								{
									flag = true;
									num7 = num5 + dirtPosition;
								}
								else if (j < Main.UnderworldLayer)
								{
									flag = true;
									num7 = num5 + rockPosition;
								}
								else
								{
									flag = true;
									num7 = hellPosition;
								}
								tile.Type = (ushort)num7;
							}
							Main.Map.SetTile(i, j, tile);
						}
					}
					else
					{
						int num12 = fileIO.ReadInt16();
						j += num12;
					}
				}
			}
		}

		public static void LoadMapVersion2(BinaryReader fileIO, int release)
		{
			//IL_039b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a5: Expected O, but got Unknown
			if (release >= 135)
			{
				Main.MapFileMetadata = FileMetadata.Read(fileIO, FileType.Map);
			}
			else
			{
				Main.MapFileMetadata = FileMetadata.FromCurrentSettings(FileType.Map);
			}
			string a = fileIO.ReadString();
			int num = fileIO.ReadInt32();
			int num2 = fileIO.ReadInt32();
			int num3 = fileIO.ReadInt32();
			if (a != Main.worldName || num != Main.worldID || num3 != Main.maxTilesX || num2 != Main.maxTilesY)
			{
				throw new Exception("Map meta-data is invalid.");
			}
			short num4 = fileIO.ReadInt16();
			short num5 = fileIO.ReadInt16();
			short num6 = fileIO.ReadInt16();
			short num7 = fileIO.ReadInt16();
			short num8 = fileIO.ReadInt16();
			short num9 = fileIO.ReadInt16();
			bool[] array = new bool[num4];
			byte b = 0;
			byte b2 = 128;
			for (int i = 0; i < num4; i++)
			{
				if (b2 == 128)
				{
					b = fileIO.ReadByte();
					b2 = 1;
				}
				else
				{
					b2 = (byte)(b2 << 1);
				}
				if ((b & b2) == b2)
				{
					array[i] = true;
				}
			}
			bool[] array2 = new bool[num5];
			b = 0;
			b2 = 128;
			for (int i = 0; i < num5; i++)
			{
				if (b2 == 128)
				{
					b = fileIO.ReadByte();
					b2 = 1;
				}
				else
				{
					b2 = (byte)(b2 << 1);
				}
				if ((b & b2) == b2)
				{
					array2[i] = true;
				}
			}
			byte[] array3 = new byte[num4];
			ushort num10 = 0;
			for (int i = 0; i < num4; i++)
			{
				if (array[i])
				{
					array3[i] = fileIO.ReadByte();
				}
				else
				{
					array3[i] = 1;
				}
				num10 = (ushort)(num10 + array3[i]);
			}
			byte[] array4 = new byte[num5];
			ushort num11 = 0;
			for (int i = 0; i < num5; i++)
			{
				if (array2[i])
				{
					array4[i] = fileIO.ReadByte();
				}
				else
				{
					array4[i] = 1;
				}
				num11 = (ushort)(num11 + array4[i]);
			}
			ushort[] array5 = new ushort[num10 + num11 + num6 + num7 + num8 + num9 + 2];
			array5[0] = 0;
			ushort num12 = 1;
			ushort num13 = 1;
			ushort num14 = num13;
			for (int i = 0; i < 623; i++)
			{
				if (i < num4)
				{
					int num15 = array3[i];
					int num16 = tileOptionCounts[i];
					for (int j = 0; j < num16; j++)
					{
						if (j < num15)
						{
							array5[num13] = num12;
							num13 = (ushort)(num13 + 1);
						}
						num12 = (ushort)(num12 + 1);
					}
				}
				else
				{
					num12 = (ushort)(num12 + (ushort)tileOptionCounts[i]);
				}
			}
			ushort num17 = num13;
			for (int i = 0; i < 316; i++)
			{
				if (i < num5)
				{
					int num18 = array4[i];
					int num19 = wallOptionCounts[i];
					for (int k = 0; k < num19; k++)
					{
						if (k < num18)
						{
							array5[num13] = num12;
							num13 = (ushort)(num13 + 1);
						}
						num12 = (ushort)(num12 + 1);
					}
				}
				else
				{
					num12 = (ushort)(num12 + (ushort)wallOptionCounts[i]);
				}
			}
			ushort num20 = num13;
			for (int i = 0; i < 3; i++)
			{
				if (i < num6)
				{
					array5[num13] = num12;
					num13 = (ushort)(num13 + 1);
				}
				num12 = (ushort)(num12 + 1);
			}
			ushort num21 = num13;
			for (int i = 0; i < 256; i++)
			{
				if (i < num7)
				{
					array5[num13] = num12;
					num13 = (ushort)(num13 + 1);
				}
				num12 = (ushort)(num12 + 1);
			}
			ushort num22 = num13;
			for (int i = 0; i < 256; i++)
			{
				if (i < num8)
				{
					array5[num13] = num12;
					num13 = (ushort)(num13 + 1);
				}
				num12 = (ushort)(num12 + 1);
			}
			ushort num23 = num13;
			for (int i = 0; i < 256; i++)
			{
				if (i < num9)
				{
					array5[num13] = num12;
					num13 = (ushort)(num13 + 1);
				}
				num12 = (ushort)(num12 + 1);
			}
			ushort num24 = num13;
			array5[num13] = num12;
			BinaryReader binaryReader = ((release < 93) ? new BinaryReader(fileIO.BaseStream) : new BinaryReader((Stream)new DeflateStream(fileIO.BaseStream, (CompressionMode)1)));
			for (int l = 0; l < Main.maxTilesY; l++)
			{
				float num25 = (float)l / (float)Main.maxTilesY;
				Main.statusText = Lang.gen[67].Value + " " + (int)(num25 * 100f + 1f) + "%";
				for (int m = 0; m < Main.maxTilesX; m++)
				{
					byte b3 = binaryReader.ReadByte();
					byte b4 = (byte)(((b3 & 1) == 1) ? binaryReader.ReadByte() : 0);
					byte b5 = (byte)((b3 & 0xE) >> 1);
					bool flag;
					switch (b5)
					{
					case 0:
						flag = false;
						break;
					case 1:
					case 2:
					case 7:
						flag = true;
						break;
					case 3:
					case 4:
					case 5:
						flag = false;
						break;
					case 6:
						flag = false;
						break;
					default:
						flag = false;
						break;
					}
					ushort num26 = (ushort)(flag ? (((b3 & 0x10) != 16) ? binaryReader.ReadByte() : binaryReader.ReadUInt16()) : 0);
					byte b6 = (((b3 & 0x20) != 32) ? byte.MaxValue : binaryReader.ReadByte());
					int num27 = (byte)((b3 & 0xC0) >> 6) switch
					{
						0 => 0, 
						1 => binaryReader.ReadByte(), 
						2 => binaryReader.ReadInt16(), 
						_ => 0, 
					};
					switch (b5)
					{
					case 0:
						m += num27;
						continue;
					case 1:
						num26 = (ushort)(num26 + num14);
						break;
					case 2:
						num26 = (ushort)(num26 + num17);
						break;
					case 3:
					case 4:
					case 5:
						num26 = (ushort)(num26 + (ushort)(num20 + (b5 - 3)));
						break;
					case 6:
						if ((double)l < Main.worldSurface)
						{
							ushort num28 = (ushort)((double)num7 * ((double)l / Main.worldSurface));
							num26 = (ushort)(num26 + (ushort)(num21 + num28));
						}
						else
						{
							num26 = num24;
						}
						break;
					case 7:
						num26 = ((!((double)l < Main.rockLayer)) ? ((ushort)(num26 + num23)) : ((ushort)(num26 + num22)));
						break;
					}
					MapTile tile = MapTile.Create(array5[num26], b6, (byte)((uint)(b4 >> 1) & 0x1Fu));
					Main.Map.SetTile(m, l, tile);
					if (b6 == byte.MaxValue)
					{
						while (num27 > 0)
						{
							m++;
							Main.Map.SetTile(m, l, tile);
							num27--;
						}
						continue;
					}
					while (num27 > 0)
					{
						m++;
						tile = tile.WithLight(binaryReader.ReadByte());
						Main.Map.SetTile(m, l, tile);
						num27--;
					}
				}
			}
			binaryReader.Close();
		}
	}
}
