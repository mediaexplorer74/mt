using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.DataStructures;
using GameManager.GameContent;
using GameManager.GameContent.ObjectInteractions;

namespace GameManager.Graphics.Renderers
{
	internal class ReturnGatePlayerRenderer : IPlayerRenderer
	{
		private List<DrawData> _voidLensData = new List<DrawData>();

		private PotionOfReturnGateInteractionChecker _interactionChecker = new PotionOfReturnGateInteractionChecker();

		public void DrawPlayers(Camera camera, IEnumerable<Player> players)
		{
			foreach (Player player in players)
			{
				DrawReturnGateInWorld(camera, player);
			}
		}

		public void DrawPlayerHead(Camera camera, Player drawPlayer, Vector2 position, float alpha = 1f, float scale = 1f, Color borderColor = default(Color))
		{
			DrawReturnGateInMap(camera, drawPlayer);
		}

		public void DrawPlayer(Camera camera, Player drawPlayer, Vector2 position, float rotation, Vector2 rotationOrigin, float shadow = 0f, float scale = 1f)
		{
			DrawReturnGateInWorld(camera, drawPlayer);
		}

		private void DrawReturnGateInMap(Camera camera, Player player)
		{
		}

		private void DrawReturnGateInWorld(Camera camera, Player player)
		{
			Rectangle homeHitbox = Rectangle.Empty;
			if (!PotionOfReturnHelper.TryGetGateHitbox(player, out homeHitbox))
			{
				return;
			}
			int num = 0;
			if (player == Main.LocalPlayer)
			{
				_interactionChecker.AttemptInteraction(player, homeHitbox);
			}
			num = 0;
			if (!player.PotionOfReturnOriginalUsePosition.HasValue)
			{
				return;
			}
			SpriteBatch spriteBatch = camera.SpriteBatch;
			SamplerState sampler = camera.Sampler;
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, sampler, DepthStencilState.None, camera.Rasterizer, null, camera.GameViewMatrix.TransformationMatrix);
			float opacity = ((player.whoAmI == Main.myPlayer) ? 1f : 0.1f);
			Vector2 value = player.PotionOfReturnOriginalUsePosition.Value;
			Vector2 value2 = new Vector2(0f, -player.height / 2);
			Vector2 worldPosition = value + value2;
			Vector2 worldPosition2 = homeHitbox.Center.ToVector2();
			PotionOfReturnGateHelper potionOfReturnGateHelper = new PotionOfReturnGateHelper(PotionOfReturnGateHelper.GateType.ExitPoint, worldPosition, opacity);
			PotionOfReturnGateHelper potionOfReturnGateHelper2 = new PotionOfReturnGateHelper(PotionOfReturnGateHelper.GateType.EntryPoint, worldPosition2, opacity);
			if (!Main.gamePaused)
			{
				potionOfReturnGateHelper.Update();
				potionOfReturnGateHelper2.Update();
			}
			_voidLensData.Clear();
			potionOfReturnGateHelper.DrawToDrawData(_voidLensData, 0);
			potionOfReturnGateHelper2.DrawToDrawData(_voidLensData, num);
			foreach (DrawData voidLensDatum in _voidLensData)
			{
				voidLensDatum.Draw(spriteBatch);
			}
			spriteBatch.End();
		}
	}
}
