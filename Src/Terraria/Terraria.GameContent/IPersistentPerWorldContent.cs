using System.IO;

namespace GameManager.GameContent
{
	public interface IPersistentPerWorldContent
	{
		void Save(BinaryWriter writer);

		void Load(BinaryReader reader, int gameVersionSaveWasMadeOn);

		void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn);

		void Reset();
	}
}
