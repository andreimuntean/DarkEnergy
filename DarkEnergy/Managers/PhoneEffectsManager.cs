using System;
using Windows.Phone.Devices.Notification;

namespace DarkEnergy
{
    public enum PhoneEffect { Vibration, SlightVibration }

    static class PhoneEffectsManager
    {
        public static void Play(PhoneEffect effect)
        {
            switch (effect)
            {
                case PhoneEffect.Vibration:
                    VibrationDevice.GetDefault().Vibrate(TimeSpan.FromMilliseconds(40));
                    break;

                case PhoneEffect.SlightVibration:
                    VibrationDevice.GetDefault().Vibrate(TimeSpan.FromMilliseconds(20));
                    break;
            }
        }
    }
}
