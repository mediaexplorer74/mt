using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.ID;

namespace GameManager.GameContent.UI.BigProgressBar
{
	public class BrainOfCthuluBigProgressBar : IBigProgressBar
	{
		private float _lifePercentToShow;

		private NPC _creeperForReference;

		public BrainOfCthuluBigProgressBar()
		{
			_creeperForReference = new NPC();
		}

		public bool ValidateAndCollectNecessaryInfo(BigProgressBarInfo info)
		{
			if (info.npcIndexToAimAt < 0 || info.npcIndexToAimAt > 200)
			{
				return false;
			}
			NPC nPC = Main.npc[info.npcIndexToAimAt];
			if (!nPC.active)
			{
				return false;
			}
			int brainOfCthuluCreepersCount = NPC.GetBrainOfCthuluCreepersCount();
			_creeperForReference.SetDefaults(267, nPC.GetMatchingSpawnParams());
			int num = _creeperForReference.lifeMax * brainOfCthuluCreepersCount;
			float num2 = 0f;
			for (int i = 0; i < 200; i++)
			{
				NPC nPC2 = Main.npc[i];
				if (nPC2.active && nPC2.type == _creeperForReference.type)
				{
					num2 += (float)nPC2.life;
				}
			}
			float num3 = (float)nPC.life + num2;
			int num4 = nPC.lifeMax + num;
			_lifePercentToShow = Utils.Clamp(num3 / (float)num4, 0f, 1f);
			return true;
		}

		public void Draw(BigProgressBarInfo info, SpriteBatch spriteBatch)
		{
			int num = NPCID.Sets.BossHeadTextures[266];
			Texture2D value = TextureAssets.NpcHeadBoss[num].Value;
			Rectangle barIconFrame = value.Frame();
			BigProgressBarHelper.DrawFancyBar(spriteBatch, _lifePercentToShow, value, barIconFrame);
		}
	}
}
