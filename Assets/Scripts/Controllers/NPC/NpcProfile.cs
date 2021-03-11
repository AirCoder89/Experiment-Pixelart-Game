using System.Collections.Generic;
using System.Linq;
using AirCoder.Controllers.Animations;
using AirCoder.Controllers.Shooting;
using AirCoder.Core;
using NaughtyAttributes;
using UnityEngine;
using Vector2Int = AirCoder.Core.Vector2Int;

namespace AirCoder.Controllers.NPC
{
    [CreateAssetMenu(menuName = "Application/Systems/NPC/Npc Profile")]
    public class NpcProfile : ScriptableObject
    {
        public float mass;
        public float speed;
        public float startHealth;
        public BulletsList bullet;
        public Vector2 bulletSlot;
        
        [BoxGroup("AI Settings")]public float fireRate;
        [BoxGroup("AI Settings")] public float sightDistance;
        [BoxGroup("Animations")] public List<AnimationBehaviour> animationsMap;

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