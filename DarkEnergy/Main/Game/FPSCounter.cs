using System.Diagnostics;

namespace DarkEnergy
{
    public class FPSCounter
    {
        private uint ticks;
        private Stopwatch time;

        public float FrameRate { get; private set; }

        public FPSCounter()
        {
            ticks = 0;
            FrameRate = 0;
            time = Stopwatch.StartNew();
        }

        public void Tick()
        {
            ++ticks;
            if (time.ElapsedTicks >= Stopwatch.Frequency)
            {
                FrameRate = (float)ticks / time.ElapsedTicks * Stopwatch.Frequency;
                ticks = 0;
                time.Restart();
            }
        }
    }
}
