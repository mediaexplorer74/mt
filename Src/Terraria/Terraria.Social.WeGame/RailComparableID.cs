using System;

namespace GameManager.Social.WeGame
{
    internal class RailComparableID
    {
        internal object[] id_;

        public static explicit operator RailComparableID(RailGameID v)
        {
            throw new NotImplementedException();
        }

        public static explicit operator RailComparableID(RailID v)
        {
            throw new NotImplementedException();
        }
    }
}