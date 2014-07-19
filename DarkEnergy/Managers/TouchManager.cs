using System;
using SharpDX;

namespace DarkEnergy
{
    static class TouchManager
    {
        private static int memoryTap, memoryManipulationEnd;

        public static bool Enabled { get; set; }
        public static bool IsDragging { get { return Delta != Vector2.Zero; } }
        public static bool IsHandlingRelease { get; private set; }
        public static bool IsTapped { get; private set; }
        public static Vector2 Delta { get; set; }
        public static Vector2 Position { get; private set; }

        public static void Tap(Vector2 position)
        {
            if (Enabled)
            {
                memoryTap = 2;
                IsTapped = true;
                Position = position / App.Game.Screen.Scaling;
            }
        }

        public static void Drag(Vector2 position)
        {
            if (Enabled)
            {
                Delta += position / App.Game.Screen.Scaling;
            }
        }

        public static void Release(Vector2 translation)
        {
            if (Enabled && translation == Vector2.Zero)
            {
                memoryManipulationEnd = 2;
                IsHandlingRelease = true;
            }
        }

        /// <summary>
        /// Activates the designated action if the screen is tapped.
        /// </summary>
        /// <param name="action"></param>
        public static void OnTap(Action action)
        {
            if (IsTapped)
            {
                action();
                StopHandlingTap();
            }
        }

        /// <summary>
        /// Activates the designated action if the target is tapped.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="action"></param>
        public static void OnTap(ITappable target, Action action)
        {
            if (target.Tapped)
            {
                action();
                StopHandlingTap();
            }
        }

        /// <summary>
        /// Activates the designated action if the target is tapped. The out parameter becomes true if tapped.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="action"></param>
        /// <param name="isTapped"></param>
        public static void OnTap(ITappable target, Action action, out bool isTapped)
        {
            if (target.Tapped)
            {
                action();
                StopHandlingTap();
                isTapped = true;
            }
            else
            {
                isTapped = false;
            }
        }

        /// <summary>
        /// Activates the designated action while dragging over the target.
        /// </summary>
        /// <param name="region"></param>
        /// <param name="action"></param>
        public static void OnDrag(Region region, Action action)
        {
            if (IsDragging)
            {
                if (region.Dragged)
                {
                    action();
                    StopHandlingDrag();
                }
            }
        }

        /// <summary>
        /// Activates the designated action if the user tapped the target and then released without dragging.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="action"></param>
        public static void OnRelease(ITappable target, Action action)
        {
            if (IsHandlingRelease)
            {
                if (target.ContainsPoint(Position))
                {
                    action();
                    StopHandlingRelease();
                }
            }
        }

        public static void Refresh()
        {
            if (memoryTap > 1) --memoryTap;
            else StopHandlingTap();

            if (memoryManipulationEnd > 1) --memoryManipulationEnd;
            else StopHandlingRelease();
        }

        public static void StopHandlingTap()
        {
            IsTapped = false;
            StopHandlingDrag();
        }

        public static void StopHandlingDrag()
        {
            Delta = Vector2.Zero;
        }

        public static void StopHandlingRelease()
        {
            IsHandlingRelease = false;
        }
    }
}