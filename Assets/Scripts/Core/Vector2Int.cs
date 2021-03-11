using UnityEngine;

namespace AirCoder.Core
{
    [System.Serializable]
    public struct Vector2Int
    {
        public int x;
        public int y;

        public Vector2Int(Vector2 inVector2)
        {
            x = (int)inVector2.x;
            y = (int)inVector2.y;
        }
        public Vector2Int(float inX, float inY)
        {
            x = (int)inX;
            y = (int)inY;
        }
        public Vector2Int(int inX, int inY)
        {
            x = inX;
            y = inY;
        }

        public Vector2 ToVector2 => new Vector2(x,y);
        public Vector2Int ToVector2Int(Vector2 inVector2) => new Vector2Int(inVector2.x,inVector2.y); 
    }
}