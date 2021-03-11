using System.Collections.Generic;
using AirCoder.Core;
using NaughtyAttributes;
using UnityEngine;

namespace AirCoder.Controllers.Sounds
{
    [CreateAssetMenu(menuName = "Application/Systems/SoundConfig")]
    public class SoundSystemConfig: SystemConfig
    {
        [BoxGroup("Establishing Parameters")][Range(0,1)]
        public float masterVolume = 1f; 
        [BoxGroup("Establishing Parameters")][Range(0,1)]
        public float sfxVolume = 1f;
        [BoxGroup("Establishing Parameters")][Range(0,1)]
        public float musicVolume = 1f;

        [BoxGroup("Mapping")] public List<Sound> sfxMap;
        [BoxGroup("Mapping")] public List<MusicMap> musicMap;
    }
}