using System;

namespace DarkEnergy
{
    static class SettingsManager
    {
        public static bool DebugMenuEnabled { get; private set; }
        public static double FrameRateLimit { get; private set; }
        public static int NameLengthLimit { get; private set; }

        public static void Load()
        {
            try
            {
                DebugMenuEnabled = bool.Parse(Resources.Settings.DebugMenuEnabled);
                FrameRateLimit = double.Parse(Resources.Settings.FrameRateLimit);
                NameLengthLimit = int.Parse(Resources.Settings.NameLengthLimit);
            }
            catch (Exception e)
            {
                ExceptionManager.Log(e);
            }
        }
    }
}
