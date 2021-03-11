using System;
using AirCoder.Controllers.Animations;
using AirCoder.Controllers.Animations.Components;
using AirCoder.Controllers.GamePhysics;
using AirCoder.Controllers.GamePhysics.Components;
using AirCoder.Controllers.Player;
using AirCoder.Controllers.Player.Components;
using AirCoder.Controllers.Shooting;
using AirCoder.Controllers.VirtualInput;
using AirCoder.Core;
using NaughtyAttributes;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Views
{
    [System.Serializable]
    public class PlayerView : SpriteView, IDestructible
    {
        private float _health;
        public event Action<GameView> Destroyed;
        public event Action<GameView, Vector2> OnTakeDamage;

        public bool IsAlive { get; private set; }
        public Vector2 LastHitDirection { get; private set; }

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
        
        public PlayerProfile profile { get;}
       
        private ControlView _ctrl;
        private GameAnimator _a;
        private GameCollider _c;
        public GameCollider Collider => _c ?? (_c = GetComponent<HitBox>());
        public GameAnimator Animator => _a ?? (_a = GetComponent<GameAnimator>());
        public ControlView Controller => _ctrl ?? (_ctrl = GetComponent<ControlView>());

        public PlayerView(Application inApp, string inName, string inLayerName, PlayerProfile inProfile) : base(inApp, inName, inLayerName)
        {
            profile = inProfile;
            IsAlive = true;
            Health = profile.startHealth;
        }

        public void TakeDamage(float inDamage, Vector2 inDirection)
        {
            if(!IsAlive) return;
            Health -= inDamage;
            LastHitDirection = inDirection;
            Log("Take Damage " + inDamage + " Rest : " + _health);
            OnTakeDamage?.Invoke(this,inDirection);
        }

    }
}