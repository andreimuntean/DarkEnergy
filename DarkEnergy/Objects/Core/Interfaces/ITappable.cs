using SharpDX;

namespace DarkEnergy
{
    interface ITappable
    {
        /// <summary>
        /// Indicates whether the object is tapped or not.
        /// </summary>
        bool Tapped { get; }

        /// <summary>
        /// Indicates whether the object contains the specified point or not.
        /// </summary>
        bool ContainsPoint(Vector2 point);
    }
}
