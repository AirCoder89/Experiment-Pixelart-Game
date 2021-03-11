using System.Collections.Generic;
using System.Linq;
using AirCoder.Controllers.Animations;
using AirCoder.Controllers.Shooting;
using AirCoder.Core;
using NaughtyAttributes;
using UnityEngine;

namespace AirCoder.Controllers.Player
{
    [CreateAssetMenu(menuName = "Application/Systems/Player/New profile")]
    public class PlayerProfile : ScriptableObject
    {
        public float mass;
        public float normalMoveSpeed;
        public float quickMoveSpeed;
        public float jumpForce;
        public float startHealth;
        public BulletsList bullet;
        public Vector2 bulletSlot;
        
        [BoxGroup("Ghost Effect")] public bool ghostEffect;
        [BoxGroup("Ghost Effect")][ShowIf("ghostEffect")] public float ghostDuration;
        [BoxGroup("Ghost Effect")][ShowIf("ghostEffect")] public float ghostDensity;
        [BoxGroup("Ghost Effect")][ShowIf("ghostEffect")] public Color ghostColor;
        
        [BoxGroup("Animations")] public List<AnimationBehaviour> animationsMap;
        public float colliderBlockDistance = 5f;

        public AnimationData GetAnimationData(ActorActions inAction)
            => animationsMap?.FirstOrDefault(a => a.action == inAction).animation;

        public void SetAnimationsSource(Sprite[] inSrc)
        {
            foreach (var anim in animationsMap)
                anim.animation.spriteSource = inSrc;
        }

        public void SetAnimationSource(Sprite[] inSrc, ActorActions inAction)
            => GetAnimationData(inAction).spriteSource = inSrc;

    }
}