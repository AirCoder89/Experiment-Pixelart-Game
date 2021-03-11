using AirCoder.Core;
using AirCoder.Helper;
using AirCoder.Views;
using UnityEngine;
using Application = AirCoder.Core.Application;

namespace AirCoder.Controllers.Level.Components
{
    public class ParallaxEffect : GameComponent
    {
        private float _strength;
        private float _moveSpeed;
        private PlayerView _target;
        
        public ParallaxEffect(GameView inGameView, PlayerView inTarget, float inMoveSpeed, float inStrength) : base(inGameView)
        {
            _target = inTarget;
            _strength = inStrength;
            _moveSpeed = inMoveSpeed;
            SystemFacade.Level.AddComponent(this);
        }

        public override void Tick()
        {
            base.Tick();
            if(!_target.IsAlive || _target?.Controller == null || !_target.Controller.IsOnMove()) return;
            var current = gameView.gameObject.transform.position;
            var targetPos = new Vector3((current.x + (_target.Controller.horizontalMove * _strength)), current.y, current.z);
            gameView.gameObject.transform.position = Vector3.Lerp(current,targetPos, _moveSpeed * Application.DeltaTime);
        }

        public override void Destroy()
        {
            SystemFacade.Level.RemoveComponent(this);
            
            base.Destroy();
            _target = null;
        }
    }
}