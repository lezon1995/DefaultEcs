
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace DefaultSlap.Component
{
    public struct DrawInfo
    {
        public Point Size;
        public Color Color;
        public float? ZIndex;

        public DrawInfo(Point size, Color color)
        {
            Size = size;
            Color = color;
            ZIndex = default;
        }

        public DrawInfo(Point size, Color color, float zIndex)
        {
            Size = size;
            Color = color;
            ZIndex = zIndex;
        }
    }
}
