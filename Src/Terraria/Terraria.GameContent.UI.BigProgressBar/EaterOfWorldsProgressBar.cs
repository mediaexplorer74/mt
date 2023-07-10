using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.ID;

namespace GameManager.GameContent.UI.BigProgressBar
{
	public class EaterOfWorldsProgressBar : IBigProgressBar
	{
		private float _lifePercentToShow;

		public bool ValidateAndCollectNecessaryInfo(BigProgressBarInfo info)
		{
			if (info.npcIndexToAimAt < 0 || info.npcIndexToAimAt > 200)
			{
				return false;
			}
			if (!Main.npc[info.npcIndexToAimAt].active && !TryFindingAnotherEOWPiece(info))
			{
				return false;
			}
			int eaterOfWorldsSegmentsCount = NPC.GetEaterOfWorldsSegmentsCount();
			float num = 0f;
			for (int i = 0; i < 200; i++)
			{
				NPC nPC = Main.npc[i];
				if (nPC.active && nPC.type >= 13 && nPC.type <= 15)
				{
					num += (float)nPC.life / (float)nPC.lifeMax;
				}
			}
			_lifePercentToShow = Utils.Clamp(num / (float)eaterOfWorldsSegmentsCount, 0f, 1f);
			return true;
		}

		public void Draw(BigProgressBarInfo info, SpriteBatch spriteBatch)
		{
			int num = NPCID.Sets.BossHeadTextures[13];
			Texture2D value = TextureAssets.NpcHeadBoss[num].Value;
			Rectangle barIconFrame = value.Frame();
			BigProgressBarHelper.DrawFancyBar(spriteBatch, _lifePercentToShow, value, barIconFrame);
		}

		private bool TryFindingAnotherEOWPiece(BigProgressBarInfo info)
		{
			for (int i = 0; i < 200; i++)
			{
				NPC nPC = Main.npc[i];
				if (nPC.active && nPC.type >= 13 && nPC.type <= 15)
				{
					info.npcIndexToAimAt = i;
					return true;
				}
			}
			return false;
		}
	}
}
