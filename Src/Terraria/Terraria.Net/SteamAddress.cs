//using Steamworks;

namespace GameManager.Net
{
	public class SteamAddress : RemoteAddress
	{
		//public readonly CSteamID SteamId;

		private string _friendlyName;

		/*public SteamAddress(CSteamID steamId)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			Type = AddressType.Steam;
			SteamId = steamId;
		}*/

		public override string ToString()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//string str = (SteamId.m_SteamID % 2uL).ToString();
			//string str2 = ((SteamId.m_SteamID - (76561197960265728L + SteamId.m_SteamID % 2uL)) / 2uL).ToString();
			return "STEAM_0:"; //+ str + ":" + str2;
		}

		public override string GetIdentifier()
		{
			return ToString();
		}

		public override bool IsLocalHost()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			if (Program.LaunchParameters.ContainsKey("-localsteamid"))
			{
				return true;//Program.LaunchParameters["-localsteamid"].Equals(SteamId.m_SteamID.ToString());
			}
			return false;
		}

		public override string GetFriendlyName()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (_friendlyName == null)
			{
				_friendlyName = "nmsl";//SteamFriends.GetFriendPersonaName(SteamId);
			}
			return _friendlyName;
		}
	}
}
