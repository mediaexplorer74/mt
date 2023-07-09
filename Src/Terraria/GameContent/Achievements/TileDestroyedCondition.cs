// TileDestroyedCondition

using System.Collections.Generic;
using GameManager;
using GameManager.Achievements;

namespace GameManager.GameContent.Achievements
{
    internal class TileDestroyedCondition : AchievementCondition
    {
        private static Dictionary<ushort, List<TileDestroyedCondition>> _listeners = new Dictionary<ushort, List<TileDestroyedCondition>>();
        private static bool _isListenerHooked = false;
        private const string Identifier = "TILE_DESTROYED";
        private ushort[] _tileIds;

        private TileDestroyedCondition(ushort[] tileIds)
            : base("TILE_DESTROYED_" + tileIds[0])
        {
            _tileIds = tileIds;
            ListenForDestruction(this);
        }

        private static void ListenForDestruction(TileDestroyedCondition condition)
        {
            if (!_isListenerHooked)
            {
                AchievementsHelper.OnTileDestroyed += new AchievementsHelper.TileDestroyedEvent(TileDestroyedListener);
                _isListenerHooked = true;
            }

            for (int index = 0; index < condition._tileIds.Length; ++index)
            {
                if (!_listeners.ContainsKey(condition._tileIds[index]))
                    _listeners[condition._tileIds[index]] = new List<TileDestroyedCondition>();
                _listeners[condition._tileIds[index]].Add(condition);
            }
        }

        private static void TileDestroyedListener(Player player, ushort tileId)
        {
            if (player.whoAmI != Game1.myPlayer || !_listeners.ContainsKey(tileId))
                return;

            foreach (AchievementCondition achievementCondition in _listeners[tileId])
                achievementCondition.Complete();
        }

        public static AchievementCondition Create(params ushort[] tileIds)
        {
            return (AchievementCondition)new TileDestroyedCondition(tileIds);
        }
    }
}
