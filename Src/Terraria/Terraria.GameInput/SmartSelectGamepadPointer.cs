using System;
using Microsoft.Xna.Framework;

namespace GameManager.GameInput
{
	public class SmartSelectGamepadPointer
	{
		private Vector2 _size;

		private Vector2 _center;

		private Vector2 _distUniform = new Vector2(80f, 64f);

		public bool ShouldBeUsed()
		{
			if (PlayerInput.UsingGamepad && Main.LocalPlayer.controlTorch)
			{
				return Main.SmartCursorEnabled;
			}
			return false;
		}

		public void SmartSelectLookup_GetTargetTile(Player player, out int tX, out int tY)
		{
			tX = (int)(((float)Main.mouseX + Main.screenPosition.X) / 16f);
			tY = (int)(((float)Main.mouseY + Main.screenPosition.Y) / 16f);
			if (player.gravDir == -1f)
			{
				tY = (int)((Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY) / 16f);
			}
			if (ShouldBeUsed())
			{
				Point point = GetPointerPosition().ToPoint();
				tX = (int)(((float)point.X + Main.screenPosition.X) / 16f);
				tY = (int)(((float)point.Y + Main.screenPosition.Y) / 16f);
				if (player.gravDir == -1f)
				{
					tY = (int)((Main.screenPosition.Y + (float)Main.screenHeight - (float)point.Y) / 16f);
				}
			}
		}

		public void UpdateSize(Vector2 size)
		{
			_size = size;
		}

		public void UpdateCenter(Vector2 center)
		{
			_center = center;
		}

		public Vector2 GetPointerPosition()
		{
			Vector2 value = (new Vector2(Main.mouseX, Main.mouseY) - _center) / _size;
			float num = Math.Abs(value.X);
			if (num < Math.Abs(value.Y))
			{
				num = Math.Abs(value.Y);
			}
			if (num > 1f)
			{
				value /= num;
			}
			value *= Main.GameViewMatrix.Zoom.X;
			return value * _distUniform + _center;
		}
	}
}
