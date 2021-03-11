using AirCoder.Core;
using NaughtyAttributes;
using UnityEngine;

namespace AirCoder.Controllers.Player
{
    [CreateAssetMenu(menuName = "Application/Systems/Player/PlayerConfig")]
    public class PlayerSystemConfig: SystemConfig
    {
        [BoxGroup("Establishing Parameters")] public Vector2 startPosition;
        [BoxGroup("Establishing Parameters")] [Expandable] public PlayerProfile profile;
    }
}