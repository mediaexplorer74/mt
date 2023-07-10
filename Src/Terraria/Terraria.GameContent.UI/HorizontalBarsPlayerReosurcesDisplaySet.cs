using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;

namespace GameManager.GameContent.UI
{
	public class HorizontalBarsPlayerReosurcesDisplaySet : IPlayerResourcesDisplaySet
	{
		private int _maxSegmentCount;

		private int _hpSegmentsCount;

		private int _mpSegmentsCount;

		private int _hpFruitCount;

		private float _hpPercent;

		private float _mpPercent;

		private bool _hpHovered;

		private bool _mpHovered;

		private Asset<Texture2D> _hpFill;

		private Asset<Texture2D> _hpFillHoney;

		private Asset<Texture2D> _mpFill;

		private Asset<Texture2D> _panelLeft;

		private Asset<Texture2D> _panelMiddleHP;

		private Asset<Texture2D> _panelRightHP;

		private Asset<Texture2D> _panelMiddleMP;

		private Asset<Texture2D> _panelRightMP;

		public HorizontalBarsPlayerReosurcesDisplaySet(string name, AssetRequestMode mode)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			string str = "Images\\UI\\PlayerResourceSets\\" + name;
			_hpFill = Main.Assets.Request<Texture2D>(str + "\\HP_Fill", Main.content, mode);
			_hpFillHoney = Main.Assets.Request<Texture2D>(str + "\\HP_Fill_Honey", Main.content, mode);
			_mpFill = Main.Assets.Request<Texture2D>(str + "\\MP_Fill", Main.content, mode);
			_panelLeft = Main.Assets.Request<Texture2D>(str + "\\Panel_Left", Main.content, mode);
			_panelMiddleHP = Main.Assets.Request<Texture2D>(str + "\\HP_Panel_Middle", Main.content, mode);
			_panelRightHP = Main.Assets.Request<Texture2D>(str + "\\HP_Panel_Right", Main.content, mode);
			_panelMiddleMP = Main.Assets.Request<Texture2D>(str + "\\MP_Panel_Middle", Main.content, mode);
			_panelRightMP = Main.Assets.Request<Texture2D>(str + "\\MP_Panel_Right", Main.content, mode);
		}

		public void Draw()
		{
			PrepareFields(Main.LocalPlayer);
			SpriteBatch spriteBatch = Main.spriteBatch;
			int num = 16;
			int num2 = 18;
			int num3 = Main.screenWidth - 300 - 22 + num;
			Vector2 vector = new Vector2(num3, num2);
			vector.X += (_maxSegmentCount - _hpSegmentsCount) * _panelMiddleHP.Width();
			bool isHovered = false;
			ResourceDrawSettings resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = _hpSegmentsCount + 2;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector;
			resourceDrawSettings.GetTextureMethod = LifePanelDrawer;
			resourceDrawSettings.OffsetPerDraw = Vector2.Zero;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.Draw(spriteBatch, isHovered);
			resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = _hpSegmentsCount;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector + new Vector2(6f, 6f);
			resourceDrawSettings.GetTextureMethod = LifeFillingDrawer;
			resourceDrawSettings.OffsetPerDraw = new Vector2(_hpFill.Width(), 0f);
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.Draw(spriteBatch, isHovered);
			_hpHovered = isHovered;
			isHovered = false;
			Vector2 vector2 = new Vector2(num3 - 10, num2 + 24);
			vector2.X += (_maxSegmentCount - _mpSegmentsCount) * _panelMiddleMP.Width();
			resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = _mpSegmentsCount + 2;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector2;
			resourceDrawSettings.GetTextureMethod = ManaPanelDrawer;
			resourceDrawSettings.OffsetPerDraw = Vector2.Zero;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.Draw(spriteBatch, isHovered);
			resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = _mpSegmentsCount;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector2 + new Vector2(6f, 6f);
			resourceDrawSettings.GetTextureMethod = ManaFillingDrawer;
			resourceDrawSettings.OffsetPerDraw = new Vector2(_mpFill.Width(), 0f);
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.Draw(spriteBatch, isHovered);
			_mpHovered = isHovered;
		}

		private static void DrawManaText(SpriteBatch spriteBatch)
		{
			Color color = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);
			int num = 180;
			Player localPlayer = Main.LocalPlayer;
			string text = Lang.inter[2].Value + ":";
			string text2 = localPlayer.statMana + "/" + localPlayer.statManaMax2;
			Vector2 value = new Vector2(Main.screenWidth - num, 65f);
			string text3 = text + " " + text2;
			Vector2 vector = FontAssets.MouseText.Value.MeasureString(text3);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, text, value + new Vector2((0f - vector.X) * 0.5f, 0f), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, text2, value + new Vector2(vector.X * 0.5f, 0f), color, 0f, new Vector2(FontAssets.MouseText.Value.MeasureString(text2).X, 0f), 1f, SpriteEffects.None, 0f);
		}

		private static void DrawLifeBarText(SpriteBatch spriteBatch, Vector2 topLeftAnchor)
		{
			Vector2 value = topLeftAnchor + new Vector2(130f, -20f);
			Player localPlayer = Main.LocalPlayer;
			Color color = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);
			string text = Lang.inter[0].Value + " " + localPlayer.statLifeMax2 + "/" + localPlayer.statLifeMax2;
			Vector2 vector = FontAssets.MouseText.Value.MeasureString(text);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, Lang.inter[0].Value, value + new Vector2((0f - vector.X) * 0.5f, 0f), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, localPlayer.statLife + "/" + localPlayer.statLifeMax2, value + new Vector2(vector.X * 0.5f, 0f), color, 0f, new Vector2(FontAssets.MouseText.Value.MeasureString(localPlayer.statLife + "/" + localPlayer.statLifeMax2).X, 0f), 1f, SpriteEffects.None, 0f);
		}

		private void PrepareFields(Player player)
		{
			PlayerStatsSnapshot playerStatsSnapshot = new PlayerStatsSnapshot(player);
			_hpSegmentsCount = (int)((float)playerStatsSnapshot.LifeMax / playerStatsSnapshot.LifePerSegment);
			_mpSegmentsCount = (int)((float)playerStatsSnapshot.ManaMax / playerStatsSnapshot.ManaPerSegment);
			_maxSegmentCount = 20;
			_hpFruitCount = playerStatsSnapshot.LifeFruitCount;
			_hpPercent = (float)playerStatsSnapshot.Life / (float)playerStatsSnapshot.LifeMax;
			_mpPercent = (float)playerStatsSnapshot.Mana / (float)playerStatsSnapshot.ManaMax;
		}

		private void LifePanelDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sourceRect = null;
			offset = Vector2.Zero;
			sprite = _panelLeft;
			drawScale = 1f;
			if (elementIndex == lastElementIndex)
			{
				sprite = _panelRightHP;
				offset = new Vector2(-16f, -10f);
			}
			else if (elementIndex != firstElementIndex)
			{
				sprite = _panelMiddleHP;
			}
		}

		private void ManaPanelDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sourceRect = null;
			offset = Vector2.Zero;
			sprite = _panelLeft;
			drawScale = 1f;
			if (elementIndex == lastElementIndex)
			{
				sprite = _panelRightMP;
				offset = new Vector2(-16f, -6f);
			}
			else if (elementIndex != firstElementIndex)
			{
				sprite = _panelMiddleMP;
			}
		}

		private void LifeFillingDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sprite = _hpFill;
			if (elementIndex >= _hpSegmentsCount - _hpFruitCount)
			{
				sprite = _hpFillHoney;
			}
			FillBarByValues(elementIndex, sprite, _hpSegmentsCount, _hpPercent, out offset, out drawScale, out sourceRect);
		}

		private static void FillBarByValues(int elementIndex, Asset<Texture2D> sprite, int segmentsCount, float fillPercent, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sourceRect = null;
			offset = Vector2.Zero;
			float num = 1f;
			float num2 = 1f / (float)segmentsCount;
			float t = 1f - fillPercent;
			float lerpValue = Utils.GetLerpValue(num2 * (float)elementIndex, num2 * (float)(elementIndex + 1), t, clamped: true);
			num = 1f - lerpValue;
			drawScale = 1f;
			Rectangle value = sprite.Frame();
			int num3 = (int)((float)value.Width * (1f - num));
			offset.X += num3;
			value.X += num3;
			value.Width -= num3;
			sourceRect = value;
		}

		private void ManaFillingDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sprite = _mpFill;
			FillBarByValues(elementIndex, sprite, _mpSegmentsCount, _mpPercent, out offset, out drawScale, out sourceRect);
		}

		public void TryToHover()
		{
			if (_hpHovered)
			{
				CommonResourceBarMethods.DrawLifeMouseOver();
			}
			if (_mpHovered)
			{
				CommonResourceBarMethods.DrawManaMouseOver();
			}
		}
	}
}
