using AirCoder.Core;
using NaughtyAttributes;
using UnityEngine;

namespace AirCoder.Controllers.Level
{
    
    [CreateAssetMenu(menuName = "Application/Systems/Level/LevelConfig")]
    public class LevelSystemConfig: SystemConfig
    {
        [BoxGroup("Establishing Parameters")] public Vector2 backgroundPosition;
        [BoxGroup("Establishing Parameters")][Required] public GameObject levelCollider;
        
        [BoxGroup("Parallax")][Range(0,20)] public float speed;
        [BoxGroup("Parallax")][Range(-30,30)] public float strength;
       
    }
}