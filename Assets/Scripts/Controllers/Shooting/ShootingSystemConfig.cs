using System.Collections.Generic;
using AirCoder.Core;
using NaughtyAttributes;
using UnityEngine;

namespace AirCoder.Controllers.Shooting
{
    [CreateAssetMenu(menuName = "Application/Systems/Shooting/ShootingConfig")]
    public class ShootingSystemConfig: SystemConfig
    {
        [BoxGroup("Establishing Parameters")] public List<BulletData> bulletsData;
    }
}