using AirCoder.Controllers.Sounds;
using AirCoder.Controllers.VisualEffects;
using UnityEngine;

namespace AirCoder.Controllers.Shooting
{
    [CreateAssetMenu(menuName = "Application/Systems/Shooting/New Bullet")]
    public class BulletData : ScriptableObject
    {
        public BulletsList label;
        public Sprite sprite;
        public float radius;
        public float speed;
        public float damage;
        public float hitRadius;
        public bool isExplosion;
        public bool shakeCameraOnExplode;
        public VfxList vfx;
        public SoundList sfx;
    }
}