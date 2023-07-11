using System;

namespace GameManager.Social.WeGame
{
    internal class RailSystemStateChanged
    {
        internal RailSystemState state;

        public static explicit operator RailSystemStateChanged(EventBase v)
        {
            throw new NotImplementedException();
        }
    }
}