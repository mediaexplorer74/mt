// DrawAnimationVertical

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager;

namespace GameManager.DataStructures
{
    public class DrawAnimationVertical : DrawAnimation
    {
        public DrawAnimationVertical(int ticksperframe, int frameCount)
        {
            Frame = 0;
            FrameCounter = 0;
            FrameCount = frameCount;
            TicksPerFrame = ticksperframe;
        }

        public override void Update()
        {
            if (++FrameCounter < TicksPerFrame)
                return;

            FrameCounter = 0;
            if (++Frame < FrameCount)
                return;
            Frame = 0;
        }

        public override Rectangle GetFrame(Texture2D texture)
        {
            return Utils.Frame(texture, 1, FrameCount, 0, Frame);
        }
    }
}
