using AirCoder.Controllers.Animations;
using AirCoder.Controllers.Sounds;
using NaughtyAttributes;

namespace AirCoder.Controllers.VisualEffects
{
    public enum VfxList
    {
        BigExplosion, MidExplosion, SmallExplosion, BulletHit
    }

    public enum VfxType
    {
        ParticleSystem, Animation
    }
    
    [System.Serializable]
    public class VfxMap
    {
        public string vfxName;
        public VfxList label;
        public VfxType type;
        
        
        [BoxGroup("Extra")] public SoundList sfx;
        [BoxGroup("Extra")] public AnimationData animationData;
    }
}