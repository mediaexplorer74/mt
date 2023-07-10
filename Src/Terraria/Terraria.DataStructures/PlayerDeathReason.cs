using System.IO;
using GameManager.Localization;

namespace GameManager.DataStructures
{
	public class PlayerDeathReason
	{
		private int _sourcePlayerIndex = -1;

		private int _sourceNPCIndex = -1;

		private int _sourceProjectileIndex = -1;

		private int _sourceOtherIndex = -1;

		private int _sourceProjectileType;

		private int _sourceItemType;

		private int _sourceItemPrefix;

		private string _sourceCustomReason;

		public int? SourceProjectileType
		{
			get
			{
				if (_sourceProjectileIndex == -1)
				{
					return null;
				}
				return _sourceProjectileType;
			}
		}

		public static PlayerDeathReason LegacyEmpty()
		{
			return new PlayerDeathReason
			{
				_sourceOtherIndex = 254
			};
		}

		public static PlayerDeathReason LegacyDefault()
		{
			return new PlayerDeathReason
			{
				_sourceOtherIndex = 255
			};
		}

		public static PlayerDeathReason ByNPC(int index)
		{
			return new PlayerDeathReason
			{
				_sourceNPCIndex = index
			};
		}

		public static PlayerDeathReason ByCustomReason(string reasonInEnglish)
		{
			return new PlayerDeathReason
			{
				_sourceCustomReason = reasonInEnglish
			};
		}

		public static PlayerDeathReason ByPlayer(int index)
		{
			return new PlayerDeathReason
			{
				_sourcePlayerIndex = index,
				_sourceItemType = Main.player[index].inventory[Main.player[index].selectedItem].type,
				_sourceItemPrefix = Main.player[index].inventory[Main.player[index].selectedItem].prefix
			};
		}

		public static PlayerDeathReason ByOther(int type)
		{
			return new PlayerDeathReason
			{
				_sourceOtherIndex = type
			};
		}

		public static PlayerDeathReason ByProjectile(int playerIndex, int projectileIndex)
		{
			PlayerDeathReason playerDeathReason = new PlayerDeathReason
			{
				_sourcePlayerIndex = playerIndex,
				_sourceProjectileIndex = projectileIndex,
				_sourceProjectileType = Main.projectile[projectileIndex].type
			};
			if (playerIndex >= 0 && playerIndex <= 255)
			{
				playerDeathReason._sourceItemType = Main.player[playerIndex].inventory[Main.player[playerIndex].selectedItem].type;
				playerDeathReason._sourceItemPrefix = Main.player[playerIndex].inventory[Main.player[playerIndex].selectedItem].prefix;
			}
			return playerDeathReason;
		}

		public NetworkText GetDeathText(string deadPlayerName)
		{
			if (_sourceCustomReason != null)
			{
				return NetworkText.FromLiteral(_sourceCustomReason);
			}
			return Lang.CreateDeathMessage(deadPlayerName, _sourcePlayerIndex, _sourceNPCIndex, _sourceProjectileIndex, _sourceOtherIndex, _sourceProjectileType, _sourceItemType);
		}

		public void WriteSelfTo(BinaryWriter writer)
		{
			BitsByte bb = (byte)0;
			bb[0] = _sourcePlayerIndex != -1;
			bb[1] = _sourceNPCIndex != -1;
			bb[2] = _sourceProjectileIndex != -1;
			bb[3] = _sourceOtherIndex != -1;
			bb[4] = _sourceProjectileType != 0;
			bb[5] = _sourceItemType != 0;
			bb[6] = _sourceItemPrefix != 0;
			bb[7] = _sourceCustomReason != null;
			writer.Write(bb);
			if (bb[0])
			{
				writer.Write((short)_sourcePlayerIndex);
			}
			if (bb[1])
			{
				writer.Write((short)_sourceNPCIndex);
			}
			if (bb[2])
			{
				writer.Write((short)_sourceProjectileIndex);
			}
			if (bb[3])
			{
				writer.Write((byte)_sourceOtherIndex);
			}
			if (bb[4])
			{
				writer.Write((short)_sourceProjectileType);
			}
			if (bb[5])
			{
				writer.Write((short)_sourceItemType);
			}
			if (bb[6])
			{
				writer.Write((byte)_sourceItemPrefix);
			}
			if (bb[7])
			{
				writer.Write(_sourceCustomReason);
			}
		}

		public static PlayerDeathReason FromReader(BinaryReader reader)
		{
			PlayerDeathReason playerDeathReason = new PlayerDeathReason();
			BitsByte bitsByte = reader.ReadByte();
			if (bitsByte[0])
			{
				playerDeathReason._sourcePlayerIndex = reader.ReadInt16();
			}
			if (bitsByte[1])
			{
				playerDeathReason._sourceNPCIndex = reader.ReadInt16();
			}
			if (bitsByte[2])
			{
				playerDeathReason._sourceProjectileIndex = reader.ReadInt16();
			}
			if (bitsByte[3])
			{
				playerDeathReason._sourceOtherIndex = reader.ReadByte();
			}
			if (bitsByte[4])
			{
				playerDeathReason._sourceProjectileType = reader.ReadInt16();
			}
			if (bitsByte[5])
			{
				playerDeathReason._sourceItemType = reader.ReadInt16();
			}
			if (bitsByte[6])
			{
				playerDeathReason._sourceItemPrefix = reader.ReadByte();
			}
			if (bitsByte[7])
			{
				playerDeathReason._sourceCustomReason = reader.ReadString();
			}
			return playerDeathReason;
		}
	}
}
