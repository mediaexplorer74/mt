using System.Collections.Generic;
using System.IO;
using GameManager.UI;

namespace GameManager.GameContent.Creative
{
	public interface ICreativePower
	{
		ushort PowerId
		{
			get;
			set;
		}

		string ServerConfigName
		{
			get;
			set;
		}

		PowerPermissionLevel CurrentPermissionLevel
		{
			get;
			set;
		}

		PowerPermissionLevel DefaultPermissionLevel
		{
			get;
			set;
		}

		void DeserializeNetMessage(BinaryReader reader, int userId);

		void ProvidePowerButtons(CreativePowerUIElementRequestInfo info, List<UIElement> elements);

		bool GetIsUnlocked();
	}
}
