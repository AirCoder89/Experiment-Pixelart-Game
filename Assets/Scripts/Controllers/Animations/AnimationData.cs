using AirCoder.Controllers.Sounds;
using NaughtyAttributes;
using UnityEngine;
using Vector2Int = AirCoder.Core.Vector2Int;

namespace AirCoder.Controllers.Animations
{
    public enum OffsetType
    {
        None, ResetOffset, SetOffsetY, SetOffsetX, SetOffsetXY
    }
    
    [CreateAssetMenu(menuName = "Application/Animation/New animation")]
    public class AnimationData : ScriptableObject
    {
        [BoxGroup("Parameters")] public bool loop;
        [BoxGroup("Parameters")] public float duration;
        [BoxGroup("Parameters")] public bool updateColliderSize;
        [BoxGroup("Parameters")] public OffsetType updateOffset;
        [BoxGroup("Parameters")][Tooltip("-1 have no effect")]  public int updateOffsetAtFrame = -1;
        
        [BoxGroup("Range")][MinMaxSlider(0,50)][SerializeField] private Vector2 animationRange;
        
        [BoxGroup("Resources")] public Sprite[] spriteSource;
        
        [BoxGroup("Sound Effect")] public SoundList sfx;
        [BoxGroup("Sound Effect")] public bool perFrame;
        
        [BoxGroup("Sound Effect")][Tooltip("zero (0) have no effect")] 
        public int concreteFrame; //zero (0) have no effect

        public Vector2Int GetRange() => new Vector2Int(animationRange);
    }
}