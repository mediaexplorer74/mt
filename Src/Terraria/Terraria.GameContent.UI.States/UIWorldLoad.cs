using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.GameContent.UI.Elements;
using GameManager.GameInput;
using GameManager.Localization;
using GameManager.UI;
using GameManager.UI.Gamepad;
using GameManager.WorldBuilding;

namespace GameManager.GameContent.UI.States
{
	public class UIWorldLoad : UIState
	{
		private UIGenProgressBar _progressBar = new UIGenProgressBar();

		private UIHeader _progressMessage = new UIHeader();

		private GenerationProgress _progress;

		public UIWorldLoad()
		{
			_progressBar.Top.Pixels = 270f;
			_progressBar.HAlign = 0.5f;
			_progressBar.VAlign = 0f;
			_progressBar.Recalculate();
			_progressMessage.CopyStyle(_progressBar);
			_progressMessage.Top.Pixels -= 70f;
			_progressMessage.Recalculate();
			Append(_progressBar);
			Append(_progressMessage);
		}

		public override void OnActivate()
		{
			if (PlayerInput.UsingGamepadUI)
			{
				UILinkPointNavigator.Points[3000].Unlink();
				UILinkPointNavigator.ChangePoint(3000);
			}
		}

		public override void Update(GameTime gameTime)
		{
			_progressBar.Top.Pixels = MathHelper.Lerp(270f, 370f, Utils.GetLerpValue(600f, 700f, Main.screenHeight, clamped: true));
			_progressMessage.Top.Pixels = _progressBar.Top.Pixels - 70f;
			_progressBar.Recalculate();
			_progressMessage.Recalculate();
			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			_progress = WorldGenerator.CurrentGenerationProgress;
			if (_progress != null)
			{
				base.Draw(spriteBatch);
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			float overallProgress = 0f;
			float currentProgress = 0f;
			string text = string.Empty;
			if (_progress != null)
			{
				overallProgress = _progress.TotalProgress;
				currentProgress = _progress.Value;
				text = _progress.Message;
			}
			_progressBar.SetProgress(overallProgress, currentProgress);
			_progressMessage.Text = text;
			if (WorldGen.drunkWorldGenText && !WorldGen.placingTraps)
			{
				_progressMessage.Text = string.Concat(Main.rand.Next(999999999));
				for (int i = 0; i < 3; i++)
				{
					if (Main.rand.Next(2) == 0)
					{
						_progressMessage.Text += Main.rand.Next(999999999);
					}
				}
			}
			if (WorldGen.notTheBees)
			{
				_progressMessage.Text = Language.GetTextValue("UI.WorldGenEasterEgg_GeneratingBees");
			}
			if (WorldGen.getGoodWorldGen)
			{
				string text2 = "";
				for (int num = _progressMessage.Text.Length - 1; num >= 0; num--)
				{
					text2 += _progressMessage.Text.Substring(num, 1);
				}
				_progressMessage.Text = text2;
			}
			Main.gameTips.Update();
			Main.gameTips.Draw();
			UpdateGamepadSquiggle();
		}

		private void UpdateGamepadSquiggle()
		{
			Vector2 value = new Vector2((float)Math.Cos(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f)), (float)Math.Sin(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f) * 2f)) * new Vector2(30f, 15f) + Vector2.UnitY * 20f;
			UILinkPointNavigator.Points[3000].Unlink();
			UILinkPointNavigator.SetPosition(3000, new Vector2(Main.screenWidth, Main.screenHeight) / 2f + value);
		}

		public string GetStatusText()
		{
			if (_progress == null)
			{
				return $"{0:0.0%} - ... - {0:0.0%}";
			}
			return string.Format("{0:0.0%} - " + _progress.Message + " - {1:0.0%}", _progress.TotalProgress, _progress.Value);
		}
	}
}
