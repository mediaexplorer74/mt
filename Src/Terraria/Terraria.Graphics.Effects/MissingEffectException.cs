using System;

namespace GameManager.Graphics.Effects
{
	public class MissingEffectException : Exception
	{
		public MissingEffectException(string text)
			: base(text)
		{
		}
	}
}
