using System.IO;
using Microsoft.Xna.Framework;

namespace GameManager.GameContent.Drawing
{
	public struct ParticleOrchestraSettings
	{
		public Vector2 PositionInWorld;

		public Vector2 MovementVector;

		public int PackedShaderIndex;

		public byte IndexOfPlayerWhoInvokedThis;

		public const int SerializationSize = 21;

		public void Serialize(BinaryWriter writer)
		{
			writer.WriteVector2(PositionInWorld);
			writer.WriteVector2(MovementVector);
			writer.Write(PackedShaderIndex);
			writer.Write(IndexOfPlayerWhoInvokedThis);
		}

		public void DeserializeFrom(BinaryReader reader)
		{
			PositionInWorld = reader.ReadVector2();
			MovementVector = reader.ReadVector2();
			PackedShaderIndex = reader.ReadInt32();
			IndexOfPlayerWhoInvokedThis = reader.ReadByte();
		}
	}
}
