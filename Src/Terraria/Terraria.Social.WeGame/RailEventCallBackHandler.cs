namespace GameManager.Social.WeGame
{
    internal class RailEventCallBackHandler
    {
        private System.Action<RAILEventID, EventBase> railEventCallBack;

        public RailEventCallBackHandler(System.Action<RAILEventID, EventBase> railEventCallBack)
        {
            this.railEventCallBack = railEventCallBack;
        }
    }
}