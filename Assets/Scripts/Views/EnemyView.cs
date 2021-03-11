using System;
using AirCoder.Controllers.NPC;
using AirCoder.Controllers.Shooting;
using AirCoder.Core;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Views
{
    public class EnemyView : SpriteView , IDestructible
    {
        public event Action<GameView> Destroyed;
        public event Action<GameView, Vector2> OnTakeDamage;

        public Vector2 LastHitDirection { get; private set; }
        private float _health;
        public bool IsAlive { get; private set; }

        public float Health
        {
            get { return _health;}
            private set
            {
                if(!IsAlive) return;
                _health = value;
                if (_health > 100) _health = 100;
                if (_health > 0) return;
                _health = 0f;
                IsAlive = false;
                Destroyed?.Invoke(this);
            }
        }
        
        public NpcProfile Profile { get; }
        public EnemyView(Application inApp, string inName, string inLayerName, NpcProfile inProfile) : base(inApp, inName, inLayerName)
        {
            IsAlive = true;
            Profile = inProfile;
            Health = Profile.startHealth;
        }

        public void TakeDamage(float inDamage, Vector2 inDirection)
        {
            if(!IsAlive) return;
            Health -= inDamage;
            LastHitDirection = inDirection;
            OnTakeDamage?.Invoke(this, inDirection);
        }

        public override void Destroy()
        {
            Destroyed = null;
            OnTakeDamage = null;
            
           base.Destroy();
        }
    }
}