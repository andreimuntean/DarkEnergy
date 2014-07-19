using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;

namespace DarkEnergy
{
    public class NotificationManager : GameSystem
    {
        private static List<INotification> notifications { get; set; }
        private static List<INotification> notificationsOutOfScope { get; set; }

        public NotificationManager()
        {
            notifications = new List<INotification>();
            notificationsOutOfScope = new List<INotification>();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            foreach (var notification in notifications)
            {
                notification.LoadContent(contentManager);
            }
        }

        public override void Update(GameTime gameTime)
        {
            List<INotification> temp = new List<INotification>(notifications);
            foreach (var notification in temp)
            {
                notification.Update(gameTime);
            }
        }

        public override void Draw(Renderer renderer)
        {
            List<INotification> drawOrder = notifications.OrderBy(notification => notification.Importance).ToList();
            foreach (var notification in drawOrder)
            {
                notification.Draw(renderer);
            }
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            foreach (var notification in notifications)
            {
                notification.UnloadContent(contentManager);
            }

            foreach (var notification in notificationsOutOfScope)
            {
                notification.UnloadContent(contentManager);
            }
        }

        public static void Add(INotification notification)
        {
            if (notification != null)
            {
                var contentManager = App.Game.Content;
                notification.Initialize();
                notification.LoadContent(contentManager);
                notifications.Add(notification);
            }
            else
            {
                ExceptionManager.Log("Cannot add a null notification. (Type " + notification.GetType().FullName + ")");
            }
        }

        public static void Remove(INotification notification)
        {
            if (notification != null)
            {
                int index = notifications.IndexOf(notification);
                if (index > -1)
                {
                    notifications.RemoveAt(index);
                    notificationsOutOfScope.Add(notification);
                }
            }
            else
            {
                ExceptionManager.Log("Cannot remove a null notification. (Type " + notification.GetType().FullName + ")");
            }
        }

        public static bool ContainsItemOfType(Type type)
        {
            foreach (var notification in notifications)
            {
                if (notification.GetType() == type)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
