using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using GameManager.Audio;
using GameManager.UI;

namespace GameManager.GameContent.UI.Elements
{
	public class UIHairStyleButton : UIImageButton
	{
		private readonly Player _player;

		public readonly int HairStyleId;

		private readonly Asset<Texture2D> _selectedBorderTexture;

		private readonly Asset<Texture2D> _hoveredBorderTexture;

		private bool _hovered;

		private bool _soundedHover;

		public UIHairStyleButton(Player player, int hairStyleId)
			: base(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanel", Main.content, (AssetRequestMode)1))
		{
			_player = player;
			HairStyleId = hairStyleId;
			Width = StyleDimension.FromPixels(44f);
			Height = StyleDimension.FromPixels(44f);
			_selectedBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", Main.content, (AssetRequestMode)1);
			_hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder", Main.content, (AssetRequestMode)1);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (_hovered)
			{
				if (!_soundedHover)
				{
					SoundEngine.PlaySound(12);
				}
				_soundedHover = true;
			}
			else
			{
				_soundedHover = false;
			}
			Vector2 value = new Vector2(-5f, -5f);
			base.DrawSelf(spriteBatch);
			if (_player.hair == HairStyleId)
			{
				spriteBatch.Draw(_selectedBorderTexture.Value, GetDimensions().Center() - _selectedBorderTexture.Size() / 2f, Color.White);
			}
			if (_hovered)
			{
				spriteBatch.Draw(_hoveredBorderTexture.Value, GetDimensions().Center() - _hoveredBorderTexture.Size() / 2f, Color.White);
			}
			int hair = _player.hair;
			_player.hair = HairStyleId;
			Main.PlayerRenderer.DrawPlayerHead(Main.Camera, _player, GetDimensions().Center() + value);
			_player.hair = hair;
		}

		public override void MouseDown(UIMouseEvent evt)
		{
			_player.hair = HairStyleId;
			SoundEngine.PlaySound(12);
			base.MouseDown(evt);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			_hovered = true;
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			_hovered = false;
		}
	}
}
