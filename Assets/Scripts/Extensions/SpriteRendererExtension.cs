using UnityEngine;

namespace AirCoder.Extensions
{
    public static class SpriteRendererExtension
    {
        public static void ChangeAlpha(this SpriteRenderer inRenderer, float inNewAlpha)
        {
            var color = inRenderer.color;
            color.a = inNewAlpha;
            inRenderer.color = color;
        }
        
        public static void DecreaseAlpha(this SpriteRenderer inRenderer, float inAmount)
        {
            var color = inRenderer.color;
            color.a -= inAmount;
            if (color.a < 0) color.a = 0f;
            inRenderer.color = color;
        }
        
        public static void IncreaseAlpha(this SpriteRenderer inRenderer, float inAmount)
        {
            var color = inRenderer.color;
            color.a += inAmount;
            if (color.a > 255) color.a = 255f;
            inRenderer.color = color;
        }
    }
}