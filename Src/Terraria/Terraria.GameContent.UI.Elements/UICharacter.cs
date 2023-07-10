using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using GameManager.UI;

namespace GameManager.GameContent.UI.Elements
{
	public class UICharacter : UIElement
	{
		private Player _player;

		private Asset<Texture2D> _texture;

		private static Item _blankItem = new Item();

		private bool _animated;

		private bool _drawsBackPanel;

		private float _characterScale = 1f;

		private int _animationCounter;

		public bool IsAnimated => _animated;

		public UICharacter(Player player, bool animated = false, bool hasBackPanel = true, float characterScale = 1f)
		{
			_player = player;
			Width.Set(59f, 0f);
			Height.Set(58f, 0f);
			_texture = Main.Assets.Request<Texture2D>("Images/UI/PlayerBackground", Main.content, (AssetRequestMode)1);
			UseImmediateMode = true;
			_animated = animated;
			_drawsBackPanel = hasBackPanel;
			_characterScale = characterScale;
			OverrideSamplerState = SamplerState.PointClamp;
		}

		public override void Update(GameTime gameTime)
		{
			_player.ResetEffects();
			_player.ResetVisibleAccessories();
			_player.UpdateMiscCounter();
			_player.UpdateDyes();
			_player.PlayerFrame();
			if (_animated)
			{
				_animationCounter++;
			}
			base.Update(gameTime);
		}

		private void UpdateAnim()
		{
			if (!_animated)
			{
				_player.bodyFrame.Y = (_player.legFrame.Y = (_player.headFrame.Y = 0));
				return;
			}
			int num = (int)(Main.GlobalTimeWrappedHourly / 0.07f) % 14 + 6;
			_player.bodyFrame.Y = (_player.legFrame.Y = (_player.headFrame.Y = num * 56));
			_player.WingFrame(wingFlap: false);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			if (_drawsBackPanel)
			{
				spriteBatch.Draw(_texture.Value, dimensions.Position(), Color.White);
			}
			UpdateAnim();
			Vector2 value = dimensions.Position() + new Vector2(dimensions.Width * 0.5f - (float)(_player.width >> 1), dimensions.Height * 0.5f - (float)(_player.height >> 1));
			Item item = _player.inventory[_player.selectedItem];
			_player.inventory[_player.selectedItem] = _blankItem;
			Main.PlayerRenderer.DrawPlayer(Main.Camera, _player, value + Main.screenPosition, 0f, Vector2.Zero, 0f, _characterScale);
			_player.inventory[_player.selectedItem] = item;
		}

		public void SetAnimated(bool animated)
		{
			_animated = animated;
		}
	}
}
