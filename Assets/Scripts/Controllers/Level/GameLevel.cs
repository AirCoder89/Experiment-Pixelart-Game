using NaughtyAttributes;
using UnityEngine;

namespace AirCoder.Controllers.Level
{
    [CreateAssetMenu(menuName = "Application/Systems/Level/New level")]
    public class GameLevel : ScriptableObject
    {
        [BoxGroup("Dimension")] public int width;
        [BoxGroup("Dimension")] public int height;
    }
}