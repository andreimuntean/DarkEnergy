using SharpDX;

namespace DarkEnergy
{
    public class Region : ITappable
    {
        private RectangleF area;

        public bool Dragged { get { return TouchManager.IsDragging && area.Contains(TouchManager.Position); } }
        public bool Tapped { get { return TouchManager.IsTapped && area.Contains(TouchManager.Position); } }

        public float X { get { return area.X; } set { area.X = value; } }
        public float Y { get { return area.Y; } set { area.Y = value; } }

        public float Width { get { return area.Width; } set { area.Width = value; } }
        public float Height { get { return area.Height; } set { area.Height = value; } }

        public Region(float x, float y, float width, float height)
        {
            area = new RectangleF(x, y, width, height);
        }

        public Region(Vector2 position, Vector2 size)
        {
            area = new RectangleF(position.X, position.Y, size.X, size.Y);
        }

        public bool ContainsPoint(Vector2 point)
        {
            return area.Contains(point);
        }

        public override string ToString()
        {
            return "{" + X.ToString() + ", " + Y.ToString() + ", " + Width.ToString() + ", " + Height.ToString() + "}";
        }
    }
}
