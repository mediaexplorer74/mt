using System.IO;
using GameManager.ID;
using GameManager.Net;

namespace GameManager.GameContent.NetModules
{
	public class NetCreativeUnlocksModule : NetModule
	{
		public static NetPacket SerializeItemSacrifice(int itemId, int sacrificeCount)
		{
			NetPacket result = NetModule.CreatePacket<NetCreativeUnlocksModule>(3);
			result.Writer.Write((short)itemId);
			result.Writer.Write((ushort)sacrificeCount);
			return result;
		}

		public override bool Deserialize(BinaryReader reader, int userId)
		{
			short key = reader.ReadInt16();
			string persistentId = ContentSamples.ItemPersistentIdsByNetIds[key];
			ushort sacrificeCount = reader.ReadUInt16();
			Main.LocalPlayerCreativeTracker.ItemSacrifices.SetSacrificeCountDirectly(persistentId, sacrificeCount);
			return true;
		}
	}
}
