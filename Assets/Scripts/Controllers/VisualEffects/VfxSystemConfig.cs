using System.Collections.Generic;
using AirCoder.Core;
using NaughtyAttributes;
using UnityEngine;

namespace AirCoder.Controllers.VisualEffects
{
    [CreateAssetMenu(menuName = "Application/Systems/VfxConfig")]
    public class VfxSystemConfig: SystemConfig
    {
        [BoxGroup("Mapping")] public List<VfxMap> vfxMap;
    }
}