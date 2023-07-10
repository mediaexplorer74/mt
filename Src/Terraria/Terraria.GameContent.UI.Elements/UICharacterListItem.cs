using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using GameManager.Audio;
using GameManager.IO;
using GameManager.Localization;
using GameManager.Social;
using GameManager.UI;

namespace GameManager.GameContent.UI.Elements
{
	public class UICharacterListItem : UIPanel
	{
		private PlayerFileData _data;

		private Asset<Texture2D> _dividerTexture;

		private Asset<Texture2D> _innerPanelTexture;

		private UICharacter _playerPanel;

		private UIText _buttonLabel;

		private UIText _deleteButtonLabel;

		private Asset<Texture2D> _buttonCloudActiveTexture;

		private Asset<Texture2D> _buttonCloudInactiveTexture;

		private Asset<Texture2D> _buttonFavoriteActiveTexture;

		private Asset<Texture2D> _buttonFavoriteInactiveTexture;

		private Asset<Texture2D> _buttonPlayTexture;

		private Asset<Texture2D> _buttonDeleteTexture;

		private UIImageButton _deleteButton;

		public bool IsFavorite => _data.IsFavorite;

		public UICharacterListItem(PlayerFileData data, int snapPointIndex)
		{
			BorderColor = new Color(89, 116, 213) * 0.7f;
			_dividerTexture = Main.Assets.Request<Texture2D>("Images/UI/Divider", Main.content, (AssetRequestMode)1);
			_innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground", Main.content, (AssetRequestMode)1);
			_buttonCloudActiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonCloudActive", Main.content, (AssetRequestMode)1);
			_buttonCloudInactiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonCloudInactive", Main.content, (AssetRequestMode)1);
			_buttonFavoriteActiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteActive", Main.content, (AssetRequestMode)1);
			_buttonFavoriteInactiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteInactive", Main.content, (AssetRequestMode)1);
			_buttonPlayTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay", Main.content, (AssetRequestMode)1);
			_buttonDeleteTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete", Main.content, (AssetRequestMode)1);
			Height.Set(96f, 0f);
			Width.Set(0f, 1f);
			SetPadding(6f);
			_data = data;
			_playerPanel = new UICharacter(data.Player);
			_playerPanel.Left.Set(4f, 0f);
			_playerPanel.OnDoubleClick += PlayGame;
			base.OnDoubleClick += PlayGame;
			Append(_playerPanel);
			UIImageButton uIImageButton = new UIImageButton(_buttonPlayTexture);
			uIImageButton.VAlign = 1f;
			uIImageButton.Left.Set(4f, 0f);
			uIImageButton.OnClick += PlayGame;
			uIImageButton.OnMouseOver += PlayMouseOver;
			uIImageButton.OnMouseOut += ButtonMouseOut;
			Append(uIImageButton);
			UIImageButton uIImageButton2 = new UIImageButton(_data.IsFavorite ? _buttonFavoriteActiveTexture : _buttonFavoriteInactiveTexture);
			uIImageButton2.VAlign = 1f;
			uIImageButton2.Left.Set(28f, 0f);
			uIImageButton2.OnClick += FavoriteButtonClick;
			uIImageButton2.OnMouseOver += FavoriteMouseOver;
			uIImageButton2.OnMouseOut += ButtonMouseOut;
			uIImageButton2.SetVisibility(1f, _data.IsFavorite ? 0.8f : 0.4f);
			Append(uIImageButton2);
			if (SocialAPI.Cloud != null)
			{
				UIImageButton uIImageButton3 = new UIImageButton(_data.IsCloudSave ? _buttonCloudActiveTexture : _buttonCloudInactiveTexture);
				uIImageButton3.VAlign = 1f;
				uIImageButton3.Left.Set(52f, 0f);
				uIImageButton3.OnClick += CloudButtonClick;
				uIImageButton3.OnMouseOver += CloudMouseOver;
				uIImageButton3.OnMouseOut += ButtonMouseOut;
				Append(uIImageButton3);
				uIImageButton3.SetSnapPoint("Cloud", snapPointIndex);
			}
			UIImageButton uIImageButton4 = new UIImageButton(_buttonDeleteTexture)
			{
				VAlign = 1f,
				HAlign = 1f
			};
			if (!_data.IsFavorite)
			{
				uIImageButton4.OnClick += DeleteButtonClick;
			}
			uIImageButton4.OnMouseOver += DeleteMouseOver;
			uIImageButton4.OnMouseOut += DeleteMouseOut;
			_deleteButton = uIImageButton4;
			Append(uIImageButton4);
			_buttonLabel = new UIText("");
			_buttonLabel.VAlign = 1f;
			_buttonLabel.Left.Set(80f, 0f);
			_buttonLabel.Top.Set(-3f, 0f);
			Append(_buttonLabel);
			_deleteButtonLabel = new UIText("");
			_deleteButtonLabel.VAlign = 1f;
			_deleteButtonLabel.HAlign = 1f;
			_deleteButtonLabel.Left.Set(-30f, 0f);
			_deleteButtonLabel.Top.Set(-3f, 0f);
			Append(_deleteButtonLabel);
			uIImageButton.SetSnapPoint("Play", snapPointIndex);
			uIImageButton2.SetSnapPoint("Favorite", snapPointIndex);
			uIImageButton4.SetSnapPoint("Delete", snapPointIndex);
		}

		private void FavoriteMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			if (_data.IsFavorite)
			{
				_buttonLabel.SetText(Language.GetTextValue("UI.Unfavorite"));
			}
			else
			{
				_buttonLabel.SetText(Language.GetTextValue("UI.Favorite"));
			}
		}

		private void CloudMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			if (_data.IsCloudSave)
			{
				_buttonLabel.SetText(Language.GetTextValue("UI.MoveOffCloud"));
			}
			else
			{
				_buttonLabel.SetText(Language.GetTextValue("UI.MoveToCloud"));
			}
		}

		private void PlayMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			_buttonLabel.SetText(Language.GetTextValue("UI.Play"));
		}

		private void DeleteMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			if (_data.IsFavorite)
			{
				_deleteButtonLabel.SetText(Language.GetTextValue("UI.CannotDeleteFavorited"));
			}
			else
			{
				_deleteButtonLabel.SetText(Language.GetTextValue("UI.Delete"));
			}
		}

		private void DeleteMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			_deleteButtonLabel.SetText("");
		}

		private void ButtonMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			_buttonLabel.SetText("");
		}

		private void CloudButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (_data.IsCloudSave)
			{
				_data.MoveToLocal();
			}
			else
			{
				_data.MoveToCloud();
			}
			((UIImageButton)evt.Target).SetImage(_data.IsCloudSave ? _buttonCloudActiveTexture : _buttonCloudInactiveTexture);
			if (_data.IsCloudSave)
			{
				_buttonLabel.SetText(Language.GetTextValue("UI.MoveOffCloud"));
			}
			else
			{
				_buttonLabel.SetText(Language.GetTextValue("UI.MoveToCloud"));
			}
		}

		private void DeleteButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			for (int i = 0; i < Main.PlayerList.Count; i++)
			{
				if (Main.PlayerList[i] == _data)
				{
					SoundEngine.PlaySound(10);
					Main.selectedPlayer = i;
					Main.menuMode = 5;
					break;
				}
			}
		}

		private void PlayGame(UIMouseEvent evt, UIElement listeningElement)
		{
			if (listeningElement == evt.Target && _data.Player.loadStatus == 0)
			{
				Main.SelectPlayer(_data);
			}
		}

		private void FavoriteButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			_data.ToggleFavorite();
			((UIImageButton)evt.Target).SetImage(_data.IsFavorite ? _buttonFavoriteActiveTexture : _buttonFavoriteInactiveTexture);
			((UIImageButton)evt.Target).SetVisibility(1f, _data.IsFavorite ? 0.8f : 0.4f);
			if (_data.IsFavorite)
			{
				_buttonLabel.SetText(Language.GetTextValue("UI.Unfavorite"));
				_deleteButton.OnClick -= DeleteButtonClick;
			}
			else
			{
				_buttonLabel.SetText(Language.GetTextValue("UI.Favorite"));
				_deleteButton.OnClick += DeleteButtonClick;
			}
			(base.Parent.Parent as UIList)?.UpdateOrder();
		}

		public override int CompareTo(object obj)
		{
			UICharacterListItem uICharacterListItem = obj as UICharacterListItem;
			if (uICharacterListItem != null)
			{
				if (IsFavorite && !uICharacterListItem.IsFavorite)
				{
					return -1;
				}
				if (!IsFavorite && uICharacterListItem.IsFavorite)
				{
					return 1;
				}
				if (_data.Name.CompareTo(uICharacterListItem._data.Name) != 0)
				{
					return _data.Name.CompareTo(uICharacterListItem._data.Name);
				}
				return _data.GetFileName().CompareTo(uICharacterListItem._data.GetFileName());
			}
			return base.CompareTo(obj);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			BackgroundColor = new Color(73, 94, 171);
			BorderColor = new Color(89, 116, 213);
			_playerPanel.SetAnimated(animated: true);
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			BackgroundColor = new Color(63, 82, 151) * 0.7f;
			BorderColor = new Color(89, 116, 213) * 0.7f;
			_playerPanel.SetAnimated(animated: false);
		}

		private void DrawPanel(SpriteBatch spriteBatch, Vector2 position, float width)
		{
			spriteBatch.Draw(_innerPanelTexture.Value, position, new Rectangle(0, 0, 8, _innerPanelTexture.Height()), Color.White);
			spriteBatch.Draw(_innerPanelTexture.Value, new Vector2(position.X + 8f, position.Y), new Rectangle(8, 0, 8, _innerPanelTexture.Height()), Color.White, 0f, Vector2.Zero, new Vector2((width - 16f) / 8f, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(_innerPanelTexture.Value, new Vector2(position.X + width - 8f, position.Y), new Rectangle(16, 0, 8, _innerPanelTexture.Height()), Color.White);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			CalculatedStyle innerDimensions = GetInnerDimensions();
			CalculatedStyle dimensions = _playerPanel.GetDimensions();
			float num = dimensions.X + dimensions.Width;
			Utils.DrawBorderString(spriteBatch, _data.Name, new Vector2(num + 6f, dimensions.Y - 2f), Color.White);
			spriteBatch.Draw(_dividerTexture.Value, new Vector2(num, innerDimensions.Y + 21f), null, Color.White, 0f, Vector2.Zero, new Vector2((GetDimensions().X + GetDimensions().Width - num) / 8f, 1f), SpriteEffects.None, 0f);
			Vector2 vector = new Vector2(num + 6f, innerDimensions.Y + 29f);
			float num2 = 200f;
			Vector2 vector2 = vector;
			DrawPanel(spriteBatch, vector2, num2);
			spriteBatch.Draw(TextureAssets.Heart.Value, vector2 + new Vector2(5f, 2f), Color.White);
			vector2.X += 10f + (float)TextureAssets.Heart.Width();
			Utils.DrawBorderString(spriteBatch, _data.Player.statLifeMax + Language.GetTextValue("GameUI.PlayerLifeMax"), vector2 + new Vector2(0f, 3f), Color.White);
			vector2.X += 65f;
			spriteBatch.Draw(TextureAssets.Mana.Value, vector2 + new Vector2(5f, 2f), Color.White);
			vector2.X += 10f + (float)TextureAssets.Mana.Width();
			Utils.DrawBorderString(spriteBatch, _data.Player.statManaMax + Language.GetTextValue("GameUI.PlayerManaMax"), vector2 + new Vector2(0f, 3f), Color.White);
			vector.X += num2 + 5f;
			Vector2 vector3 = vector;
			float num3 = 140f;
			if (GameCulture.FromCultureName(GameCulture.CultureName.Russian).IsActive)
			{
				num3 = 180f;
			}
			DrawPanel(spriteBatch, vector3, num3);
			string text = "";
			Color color = Color.White;
			switch (_data.Player.difficulty)
			{
			case 0:
				text = Language.GetTextValue("UI.Softcore");
				break;
			case 1:
				text = Language.GetTextValue("UI.Mediumcore");
				color = Main.mcColor;
				break;
			case 2:
				text = Language.GetTextValue("UI.Hardcore");
				color = Main.hcColor;
				break;
			case 3:
				text = Language.GetTextValue("UI.Creative");
				color = Main.creativeModeColor;
				break;
			}
			vector3 += new Vector2(num3 * 0.5f - FontAssets.MouseText.Value.MeasureString(text).X * 0.5f, 3f);
			Utils.DrawBorderString(spriteBatch, text, vector3, color);
			vector.X += num3 + 5f;
			Vector2 vector4 = vector;
			float num4 = innerDimensions.X + innerDimensions.Width - vector4.X;
			DrawPanel(spriteBatch, vector4, num4);
			TimeSpan playTime = _data.GetPlayTime();
			int num5 = playTime.Days * 24 + playTime.Hours;
			string text2 = ((num5 < 10) ? "0" : "") + num5 + playTime.ToString("\\:mm\\:ss");
			vector4 += new Vector2(num4 * 0.5f - FontAssets.MouseText.Value.MeasureString(text2).X * 0.5f, 3f);
			Utils.DrawBorderString(spriteBatch, text2, vector4, Color.White);
		}
	}
}
