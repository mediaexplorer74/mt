using System.Collections.Generic;
using System.Runtime.Serialization;
using rail;

namespace GameManager.Social.WeGame
{
	[DataContract]
	public class WeGameFriendListInfo
	{
		[DataMember]
		public List<RailFriendInfo> _friendList;
	}
}
