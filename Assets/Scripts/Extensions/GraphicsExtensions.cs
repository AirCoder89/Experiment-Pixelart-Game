using UnityEngine.UI;

namespace AirCoder.Extensions
{
    public static class GraphicsExtensions
    {
        public static T ChangeAlpha<T>(this T g, float newAlpha)
            where T : Graphic
        {
            var color = g.color;
            color.a = newAlpha;
            g.color = color;
            return g;
        }
    }
}