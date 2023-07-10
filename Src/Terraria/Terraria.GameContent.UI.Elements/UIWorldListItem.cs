using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.OS;
using GameManager.Audio;
using GameManager.IO;
using GameManager.Localization;
using GameManager.Social;
using GameManager.UI;

namespace GameManager.GameContent.UI.Elements
{
	public class UIWorldListItem : UIPanel
	{
		private WorldFileData _data;

		private Asset<Texture2D> _dividerTexture;

		private Asset<Texture2D> _innerPanelTexture;

		private UIImage _worldIcon;

		private UIText _buttonLabel;

		private UIText _deleteButtonLabel;

		private Asset<Texture2D> _buttonCloudActiveTexture;

		private Asset<Texture2D> _buttonCloudInactiveTexture;

		private Asset<Texture2D> _buttonFavoriteActiveTexture;

		private Asset<Texture2D> _buttonFavoriteInactiveTexture;

		private Asset<Texture2D> _buttonPlayTexture;

		private Asset<Texture2D> _buttonSeedTexture;

		private Asset<Texture2D> _buttonDeleteTexture;

		private UIImageButton _deleteButton;

		private int _orderInList;

		private bool _canBePlayed;

		public bool IsFavorite => _data.IsFavorite;

		public UIWorldListItem(WorldFileData data, int orderInList, bool canBePlayed)
		{
			_orderInList = orderInList;
			_data = data;
			_canBePlayed = canBePlayed;
			LoadTextures();
			InitializeAppearance();
			_worldIcon = new UIImage(GetIcon());
			_worldIcon.Left.Set(4f, 0f);
			_worldIcon.OnDoubleClick += PlayGame;
			Append(_worldIcon);
			float num = 4f;
			UIImageButton uIImageButton = new UIImageButton(_buttonPlayTexture);
			uIImageButton.VAlign = 1f;
			uIImageButton.Left.Set(num, 0f);
			uIImageButton.OnClick += PlayGame;
			base.OnDoubleClick += PlayGame;
			uIImageButton.OnMouseOver += PlayMouseOver;
			uIImageButton.OnMouseOut += ButtonMouseOut;
			Append(uIImageButton);
			num += 24f;
			UIImageButton uIImageButton2 = new UIImageButton(_data.IsFavorite ? _buttonFavoriteActiveTexture : _buttonFavoriteInactiveTexture);
			uIImageButton2.VAlign = 1f;
			uIImageButton2.Left.Set(num, 0f);
			uIImageButton2.OnClick += FavoriteButtonClick;
			uIImageButton2.OnMouseOver += FavoriteMouseOver;
			uIImageButton2.OnMouseOut += ButtonMouseOut;
			uIImageButton2.SetVisibility(1f, _data.IsFavorite ? 0.8f : 0.4f);
			Append(uIImageButton2);
			num += 24f;
			if (SocialAPI.Cloud != null)
			{
				UIImageButton uIImageButton3 = new UIImageButton(_data.IsCloudSave ? _buttonCloudActiveTexture : _buttonCloudInactiveTexture);
				uIImageButton3.VAlign = 1f;
				uIImageButton3.Left.Set(num, 0f);
				uIImageButton3.OnClick += CloudButtonClick;
				uIImageButton3.OnMouseOver += CloudMouseOver;
				uIImageButton3.OnMouseOut += ButtonMouseOut;
				uIImageButton3.SetSnapPoint("Cloud", orderInList);
				Append(uIImageButton3);
				num += 24f;
			}
			if (_data.WorldGeneratorVersion != 0L)
			{
				UIImageButton uIImageButton4 = new UIImageButton(_buttonSeedTexture);
				uIImageButton4.VAlign = 1f;
				uIImageButton4.Left.Set(num, 0f);
				uIImageButton4.OnClick += SeedButtonClick;
				uIImageButton4.OnMouseOver += SeedMouseOver;
				uIImageButton4.OnMouseOut += ButtonMouseOut;
				uIImageButton4.SetSnapPoint("Seed", orderInList);
				Append(uIImageButton4);
				num += 24f;
			}
			UIImageButton uIImageButton5 = new UIImageButton(_buttonDeleteTexture)
			{
				VAlign = 1f,
				HAlign = 1f
			};
			if (!_data.IsFavorite)
			{
				uIImageButton5.OnClick += DeleteButtonClick;
			}
			uIImageButton5.OnMouseOver += DeleteMouseOver;
			uIImageButton5.OnMouseOut += DeleteMouseOut;
			_deleteButton = uIImageButton5;
			Append(uIImageButton5);
			num += 4f;
			_buttonLabel = new UIText("");
			_buttonLabel.VAlign = 1f;
			_buttonLabel.Left.Set(num, 0f);
			_buttonLabel.Top.Set(-3f, 0f);
			Append(_buttonLabel);
			_deleteButtonLabel = new UIText("");
			_deleteButtonLabel.VAlign = 1f;
			_deleteButtonLabel.HAlign = 1f;
			_deleteButtonLabel.Left.Set(-30f, 0f);
			_deleteButtonLabel.Top.Set(-3f, 0f);
			Append(_deleteButtonLabel);
			uIImageButton.SetSnapPoint("Play", orderInList);
			uIImageButton2.SetSnapPoint("Favorite", orderInList);
			uIImageButton5.SetSnapPoint("Delete", orderInList);
		}

		private void LoadTextures()
		{
			_dividerTexture = Main.Assets.Request<Texture2D>("Images/UI/Divider", Main.content, (AssetRequestMode)1);
			_innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground", Main.content, (AssetRequestMode)1);
			_buttonCloudActiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonCloudActive", Main.content, (AssetRequestMode)1);
			_buttonCloudInactiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonCloudInactive", Main.content, (AssetRequestMode)1);
			_buttonFavoriteActiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteActive", Main.content, (AssetRequestMode)1);
			_buttonFavoriteInactiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteInactive", Main.content, (AssetRequestMode)1);
			_buttonPlayTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay", Main.content, (AssetRequestMode)1);
			_buttonSeedTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonSeed", Main.content, (AssetRequestMode)1);
			_buttonDeleteTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete", Main.content, (AssetRequestMode)1);
		}

		private void InitializeAppearance()
		{
			Height.Set(96f, 0f);
			Width.Set(0f, 1f);
			SetPadding(6f);
			SetColorsToNotHovered();
		}

		private void SetColorsToHovered()
		{
			BackgroundColor = new Color(73, 94, 171);
			BorderColor = new Color(89, 116, 213);
			if (!_canBePlayed)
			{
				BorderColor = new Color(150, 150, 150) * 1f;
				BackgroundColor = Color.Lerp(BackgroundColor, new Color(120, 120, 120), 0.5f) * 1f;
			}
		}

		private void SetColorsToNotHovered()
		{
			BackgroundColor = new Color(63, 82, 151) * 0.7f;
			BorderColor = new Color(89, 116, 213) * 0.7f;
			if (!_canBePlayed)
			{
				BorderColor = new Color(127, 127, 127) * 0.7f;
				BackgroundColor = Color.Lerp(new Color(63, 82, 151), new Color(80, 80, 80), 0.5f) * 0.7f;
			}
		}

		private Asset<Texture2D> GetIcon()
		{
			if (_data.DrunkWorld)
			{
				return Main.Assets.Request<Texture2D>("Images/UI/Icon" + (_data.IsHardMode ? "Hallow" : "") + "CorruptionCrimson", Main.content, (AssetRequestMode)1);
			}
			return Main.Assets.Request<Texture2D>("Images/UI/Icon" + (_data.IsHardMode ? "Hallow" : "") + (_data.HasCorruption ? "Corruption" : "Crimson"), Main.content, (AssetRequestMode)1);
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

		private void SeedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			_buttonLabel.SetText(Language.GetTextValue("UI.CopySeed", _data.GetFullSeedText()));
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
			for (int i = 0; i < Main.WorldList.Count; i++)
			{
				if (Main.WorldList[i] == _data)
				{
					SoundEngine.PlaySound(10);
					Main.selectedWorld = i;
					Main.menuMode = 9;
					break;
				}
			}
		}

		private void PlayGame(UIMouseEvent evt, UIElement listeningElement)
		{
			if (listeningElement == evt.Target && !TryMovingToRejectionMenuIfNeeded(_data.GameMode))
			{
				_data.SetAsActive();
				SoundEngine.PlaySound(10);
				Main.GetInputText("");
				if (Main.menuMultiplayer && SocialAPI.Network != null)
				{
					Main.menuMode = 889;
				}
				else if (Main.menuMultiplayer)
				{
					Main.menuMode = 30;
				}
				else
				{
					Main.menuMode = 10;
				}
				if (!Main.menuMultiplayer)
				{
					WorldGen.playWorld();
				}
			}
		}

		private bool TryMovingToRejectionMenuIfNeeded(int worldGameMode)
		{
			if (!Main.RegisterdGameModes.TryGetValue(worldGameMode, out var value))
			{
				SoundEngine.PlaySound(10);
				Main.statusText = Language.GetTextValue("UI.WorldCannotBeLoadedBecauseItHasAnInvalidGameMode");
				Main.menuMode = 1000000;
				return true;
			}
			bool flag = Main.ActivePlayerFileData.Player.difficulty == 3;
			bool isJourneyMode = value.IsJourneyMode;
			if (flag && !isJourneyMode)
			{
				SoundEngine.PlaySound(10);
				Main.statusText = Language.GetTextValue("UI.PlayerIsCreativeAndWorldIsNotCreative");
				Main.menuMode = 1000000;
				return true;
			}
			if (!flag && isJourneyMode)
			{
				SoundEngine.PlaySound(10);
				Main.statusText = Language.GetTextValue("UI.PlayerIsNotCreativeAndWorldIsCreative");
				Main.menuMode = 1000000;
				return true;
			}
			return false;
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

		private void SeedButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			//Platform.Get<IClipboard>().Value = (_data.GetFullSeedText());
			_buttonLabel.SetText(Language.GetTextValue("UI.SeedCopied"));
		}

		public override int CompareTo(object obj)
		{
			UIWorldListItem uIWorldListItem = obj as UIWorldListItem;
			if (uIWorldListItem != null)
			{
				return _orderInList.CompareTo(uIWorldListItem._orderInList);
			}
			return base.CompareTo(obj);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			SetColorsToHovered();
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			SetColorsToNotHovered();
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
			CalculatedStyle dimensions = _worldIcon.GetDimensions();
			float num = dimensions.X + dimensions.Width;
			Color color = (_data.IsValid ? Color.White : Color.Red);
			Utils.DrawBorderString(spriteBatch, _data.Name, new Vector2(num + 6f, dimensions.Y - 2f), color);
			spriteBatch.Draw(_dividerTexture.Value, new Vector2(num, innerDimensions.Y + 21f), null, Color.White, 0f, Vector2.Zero, new Vector2((GetDimensions().X + GetDimensions().Width - num) / 8f, 1f), SpriteEffects.None, 0f);
			Vector2 vector = new Vector2(num + 6f, innerDimensions.Y + 29f);
			float num2 = 100f;
			DrawPanel(spriteBatch, vector, num2);
			string text = "";
			Color color2 = Color.White;
			switch (_data.GameMode)
			{
			case 1:
				text = Language.GetTextValue("UI.Expert");
				color2 = Main.mcColor;
				break;
			case 2:
				text = Language.GetTextValue("UI.Master");
				color2 = Main.hcColor;
				break;
			case 3:
				text = Language.GetTextValue("UI.Creative");
				color2 = Main.creativeModeColor;
				break;
			default:
				text = Language.GetTextValue("UI.Normal");
				break;
			}
			float x = FontAssets.MouseText.Value.MeasureString(text).X;
			float x2 = num2 * 0.5f - x * 0.5f;
			Utils.DrawBorderString(spriteBatch, text, vector + new Vector2(x2, 3f), color2);
			vector.X += num2 + 5f;
			float num3 = 150f;
			if (!GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive)
			{
				num3 += 40f;
			}
			DrawPanel(spriteBatch, vector, num3);
			string textValue = Language.GetTextValue("UI.WorldSizeFormat", _data.WorldSizeName);
			float x3 = FontAssets.MouseText.Value.MeasureString(textValue).X;
			float x4 = num3 * 0.5f - x3 * 0.5f;
			Utils.DrawBorderString(spriteBatch, textValue, vector + new Vector2(x4, 3f), Color.White);
			vector.X += num3 + 5f;
			float num4 = innerDimensions.X + innerDimensions.Width - vector.X;
			DrawPanel(spriteBatch, vector, num4);
			string arg = ((!GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive) ? _data.CreationTime.ToShortDateString() : _data.CreationTime.ToString("d MMMM yyyy"));
			string textValue2 = Language.GetTextValue("UI.WorldCreatedFormat", arg);
			float x5 = FontAssets.MouseText.Value.MeasureString(textValue2).X;
			float x6 = num4 * 0.5f - x5 * 0.5f;
			Utils.DrawBorderString(spriteBatch, textValue2, vector + new Vector2(x6, 3f), Color.White);
			vector.X += num4 + 5f;
		}
	}
}
