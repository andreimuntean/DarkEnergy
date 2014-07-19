using DarkEnergy.Notifications.SaveGameIcon;

namespace DarkEnergy
{
    public class SaveGameNotification
    {
        private static SavingIcon savingIcon;

        public static void Show()
        {
            if (!NotificationManager.ContainsItemOfType(typeof(SavingIcon)))
            {
                savingIcon = new SavingIcon();
                NotificationManager.Add(savingIcon);
            }
        }

        public static void Hide()
        {
            if (savingIcon != null)
            {
                savingIcon.FadeOut();
            }
        }
    }
}
