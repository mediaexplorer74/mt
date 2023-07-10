namespace GameManager.Enums
{
	public enum TownNPCRoomCheckFailureReason
	{
		None,
		TooCloseToWorldEdge,
		RoomIsTooBig,
		RoomIsTooSmall,
		HoleInWallIsTooBig,
		RoomCheckStartedInASolidTile
	}
}
