using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager.GameContent.UI.BigProgressBar
{
	public abstract class LunarPillarBigProgessBar : IBigProgressBar
	{
		private float _lifePercentToShow;

		private float _shieldPercentToShow;

		private int _headIndex;

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
			int bossHeadTextureIndex = nPC.GetBossHeadTextureIndex();
			if (bossHeadTextureIndex == -1)
			{
				return false;
			}
			if (!IsPlayerInCombatArea())
			{
				return false;
			}
			if (nPC.ai[2] == 1f)
			{
				return false;
			}
			float lifePercentToShow = Utils.Clamp((float)nPC.life / (float)nPC.lifeMax, 0f, 1f);
			float shieldPercentToShow = GetCurrentShieldValue() / GetMaxShieldValue();
			_ = 600f * Main.GameModeInfo.EnemyMaxLifeMultiplier * GetMaxShieldValue() / (float)nPC.lifeMax;
			_lifePercentToShow = lifePercentToShow;
			_shieldPercentToShow = shieldPercentToShow;
			_headIndex = bossHeadTextureIndex;
			return true;
		}

		public void Draw(BigProgressBarInfo info, SpriteBatch spriteBatch)
		{
			Texture2D value = TextureAssets.NpcHeadBoss[_headIndex].Value;
			Rectangle barIconFrame = value.Frame();
			BigProgressBarHelper.DrawFancyBar(spriteBatch, _lifePercentToShow, value, barIconFrame, _shieldPercentToShow);
		}

		internal abstract float GetCurrentShieldValue();

		internal abstract float GetMaxShieldValue();

		internal abstract bool IsPlayerInCombatArea();
	}
}
