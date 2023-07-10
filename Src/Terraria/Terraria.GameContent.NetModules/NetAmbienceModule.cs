using System.IO;
using GameManager.GameContent.Ambience;
using GameManager.GameContent.Skies;
using GameManager.Graphics.Effects;
using GameManager.Net;

namespace GameManager.GameContent.NetModules
{
	public class NetAmbienceModule : NetModule
	{
		public static NetPacket SerializeSkyEntitySpawn(Player player, SkyEntityType type)
		{
			int value = Main.rand.Next();
			NetPacket result = NetModule.CreatePacket<NetAmbienceModule>(6);
			result.Writer.Write((byte)player.whoAmI);
			result.Writer.Write(value);
			result.Writer.Write((byte)type);
			return result;
		}

		public override bool Deserialize(BinaryReader reader, int userId)
		{
			byte playerId = reader.ReadByte();
			int seed = reader.ReadInt32();
			SkyEntityType type = (SkyEntityType)reader.ReadByte();
			Main.QueueMainThreadAction(delegate
			{
				((AmbientSky)SkyManager.Instance["Ambience"]).Spawn(Main.player[playerId], type, seed);
			});
			return true;
		}
	}
}
