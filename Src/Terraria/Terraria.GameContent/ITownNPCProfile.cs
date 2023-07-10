using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace GameManager.GameContent
{
	public interface ITownNPCProfile
	{
		int RollVariation();

		string GetNameForVariant(NPC npc);

		Asset<Texture2D> GetTextureNPCShouldUse(NPC npc);

		int GetHeadTextureIndex(NPC npc);
	}
}
