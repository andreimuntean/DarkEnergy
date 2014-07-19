using System;
using SharpDX;
using SharpDX.Toolkit;

namespace DarkEnergy.World.Objects
{
    public enum MarkerType { Combat, Quest, Tavern, Travel, Vendor }

    public class MapMarker : TexturedElement
    {
        private const float range = 20;
        private float direction;
        private float y;

        public override float Y
        {
            get
            {
                return base.Y + y;
            }
        }

        public override RectangleF TouchArea
        {
            get
            {
                return new RectangleF(X - 100, Y - y + 120, 300, 230);
            }
        }

        public MapMarker(MarkerType type) : base(100, 150)
        {
            Path = @"Interface\World\Marker\" + type.ToString() + ".dds";
        }

        public override void Initialize()
        {
            base.Initialize();
            direction = 25;
            y = 0;
        }

        public override void Update(GameTime gameTime)
        {
            float modifier = (float)gameTime.ElapsedGameTime.TotalSeconds * direction;
            
            if (y + modifier >= range)
            {
                y = range;
                direction *= -1;
            }
            else if (y + modifier <= -range)
            {
                y = -range;
                direction *= -1;
            }
            else
            {
                y += modifier;
            }
        }
    }
}
