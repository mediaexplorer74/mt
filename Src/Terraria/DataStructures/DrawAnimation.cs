// DrawAnimation

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager;

namespace GameManager.DataStructures
{
    public class DrawAnimation
    {
        public int Frame;
        public int FrameCount;
        public int TicksPerFrame;
        public int FrameCounter;

        public virtual void Update() { }

        public virtual Rectangle GetFrame(Texture2D texture)
        {
            return Utils.Frame(texture, 1, 1, 0, 0);
        }
    }
}
