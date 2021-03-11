using System.Collections.Generic;
using AirCoder.Core;
using NaughtyAttributes;
using UnityEngine;

namespace AirCoder.Controllers.NPC
{
    [CreateAssetMenu(menuName = "Application/Systems/NPC/NpcConfig")]
    public class NpcSystemConfig : SystemConfig
    {
        [BoxGroup("Establishing Parameters")] public List<NpcDataHolder> enemies;
    }
}