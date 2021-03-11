using NaughtyAttributes;
using UnityEngine;

namespace AirCoder.Controllers.Sounds
{
    public enum SoundList
    {
        None, Click, Boom, Bullet,      
    }
    
    [System.Serializable]
    public class Sound{
        [BoxGroup("Initialize")] public SoundList label;
        [BoxGroup("Initialize")] public AudioClip clip;

        [BoxGroup("Settings")] public bool hasOwnVolume;
        [BoxGroup("Settings")][ShowIf("hasOwnVolume")] [Range(0f,1f)] public float volume = 0.5f;
        [BoxGroup("Settings")] [Range(0f, 2f)] public float pitch = 1f;
        [BoxGroup("Settings")] public bool loop = false;
        [BoxGroup("Settings")] [ReadOnly] public bool play = true;

        [HideInInspector] public AudioSource source;
    }

}