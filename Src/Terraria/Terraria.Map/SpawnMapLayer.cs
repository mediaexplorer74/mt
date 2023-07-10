using Microsoft.Xna.Framework;
using GameManager.GameContent;
using GameManager.Localization;
using GameManager.UI;

namespace GameManager.Map
{
	public class SpawnMapLayer : IMapLayer
	{
		public void Draw(MapOverlayDrawContext context, string text)
		{
			Player localPlayer = Main.LocalPlayer;
			Vector2 position = new Vector2(localPlayer.SpawnX, localPlayer.SpawnY);
			if (context.Draw(position: new Vector2(Main.spawnTileX, Main.spawnTileY), texture: TextureAssets.SpawnPoint.Value, alignment: Alignment.Bottom).IsMouseOver)
			{
				text = Language.GetTextValue("UI.SpawnPoint");
			}
			if (localPlayer.SpawnX != -1 && context.Draw(TextureAssets.SpawnBed.Value, position, Alignment.Bottom).IsMouseOver)
			{
				text = Language.GetTextValue("UI.SpawnBed");
			}
		}
	}
}
